using MicroSquid.Packets;
using PureWebSockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MicroSquid {
    public class SockChatClient : IDisposable {
        public PureWebSocket WebSocket { get; }
        public string Server { get; }

        public const string CAP_MULTI_CHANNEL = @"MCHAN";
        public const string CAP_TYPING_INFO = @"TYPING";
        private static readonly string[] SupportedCapabilities = new[] { CAP_TYPING_INFO, CAP_MULTI_CHANNEL, };

        public event Action OnOpen;
        public event Action<bool> OnClose;
        public event Action<Packet> OnReceive;

        public event Action<DateTimeOffset> OnPing;
        public event Action<DateTimeOffset> OnPong;

        public event Action<ChatUser> OnAuthSuccess;
        public event Action<string, bool, bool, DateTimeOffset> OnAuthFail;

        public event Action<IEnumerable<string>> OnCapabilitiesUpdate;
        public event Action<bool, bool, DateTimeOffset> OnForceDisconnect;
        public event Action<ChatChannel, ChatUser, DateTimeOffset> OnUserTyping;

        public event Action<ChatMessage> OnMessageAdd;
        public event Action<ChatMessage> OnMessageDelete;
        public event Action<ChatChannel> OnMessagesClear;

        public event Action<DateTimeOffset, ChatUser> OnUserAdd;
        public event Action<DateTimeOffset, ChatUser> OnUserRemove;
        public event Action<ChatUser> OnUserUpdate;
        public event Action<ChatChannel> OnUsersClear;

        public event Action<ChatChannel> OnChannelAdd;
        public event Action<ChatChannel> OnChannelRemove;
        public event Action<ChatChannel> OnChannelUpdate;
        public event Action OnChannelsClear;

        public event Action<DateTimeOffset, ChatChannel, ChatUser> OnUserJoin;
        public event Action<DateTimeOffset, ChatChannel, ChatUser> OnUserLeave;

        public int Version { get; private set; }
        public string DefaultChannel { get; private set; }
        public int MaxLength { get; private set; }

        public ChatUser User { get; private set; }
        public bool HasUser => User != null;

        public string SessionId { get; private set; }
        public bool HasSession => SessionId != null;

        public string LastChannel { get; private set; }

        public IEnumerable<string> Capabilities { get; private set; } = Enumerable.Empty<string>();
        public bool SupportsTypingEvent { get; private set; }
        public bool SupportsMultiChannel { get; private set; }

        public DateTimeOffset LastPing { get; private set; }
        public DateTimeOffset LastPong { get; private set; }

        public bool WasBanned { get; private set; }
        public bool BanIsPermanent { get; private set; }
        public bool BanHasExpiry { get; private set; }
        public DateTimeOffset BanExpires { get; private set; }

        public ChatUser Bot { get; } = new ChatUser(-1, @"ChatBot", 0x9E8DA7, isVisible: false);

        public List<ChatUser> Users { get; } = new List<ChatUser>();
        private object UserSync { get; } = new object();

        public List<ChatChannel> Channels { get; } = new List<ChatChannel>();
        private object ChannelSync { get; } = new object();

        public List<ChatMessage> Messages { get; } = new List<ChatMessage>();
        private object MessageSync { get; } = new object();

        public bool PingThreadEnabled { get; }
        private PingThread PingThread { get; set; }

        public SockChatClient(string server, bool enablePing = true) {
            PingThreadEnabled = enablePing;
            Server = server ?? throw new ArgumentNullException(nameof(server));
            WebSocket = new PureWebSocket(server, new PureWebSocketOptions());
            WebSocket.OnOpened += WebSocket_OnOpened;
            WebSocket.OnClosed += WebSocket_OnClosed;
            WebSocket.OnMessage += WebSocket_OnMessage;
        }

        public bool Connect(string authToken) {
            if(!WebSocket.Connect())
                return false;

            SendAuth(authToken);

            return true;
        }

        public void Send(string data) {
            Debug.WriteLine(data);
            WebSocket.Send(data);
        }

        public void Send(object data) {
            Send(data.ToString());
        }

        public void Send(params object[] data) {
            Send(string.Join('\t', data));
        }

        public void SendPing() {
            if(!HasUser)
                return;
            LastPing = DateTimeOffset.Now;
            Send(0, User.UserId, LastPing.ToUnixTimeSeconds());
            OnPing?.Invoke(LastPing);
        }

        public void SendAuth(string authToken) {
            if(HasUser)
                return;
            Send(1, authToken);
        }

        public void SendMessage(string text) {
            SendMessage(LastChannel, text);
        }

        public void SendMessage(string channel, string text) {
            if(!HasUser)
                return;
            LastChannel = channel ?? throw new ArgumentNullException(nameof(channel));
            Send(2, User.UserId, text.Replace("\t", @"    "), channel);
        }

        public void SendCapabilities(params string[] caps) {
            if(!HasUser)
                return;
            Send(3, string.Join(' ', caps));
        }

        public void SendTyping() {
            SendTyping(LastChannel);
        }

        public void SendTyping(string channel) {
            if(!HasUser)
                return;
            LastChannel = channel ?? throw new ArgumentNullException(nameof(channel));
            Send(4, User.UserId, channel);
        }

        public void SendKick(ChatUser user, DateTimeOffset? expiry = null, bool banAddress = false) {
            long expires = expiry == DateTimeOffset.MaxValue ? -1 : (expiry.HasValue ? expiry.Value.ToUnixTimeSeconds() : 0);
            SendMessage($@"/{(banAddress ? @"ban" : @"kick")} {user.UserName} {expires}");
        }

        private void WebSocket_OnOpened(object sender) {
            OnOpen?.Invoke();
        }

        private void WebSocket_OnClosed(object sender, System.Net.WebSockets.WebSocketCloseStatus reason) {
            PingThread?.Dispose();
            OnClose?.Invoke(reason == System.Net.WebSockets.WebSocketCloseStatus.NormalClosure);
        }

        private void WebSocket_OnMessage(object sender, string message) {
            Debug.WriteLine(message);

            IEnumerable<string> parts = message.Split('\t');
            string packetId = parts.ElementAtOrDefault(0),
                subPacketId = parts.ElementAtOrDefault(1);

            Type type = packetId switch {
                @"0" => typeof(PongPacket),
                @"1" => subPacketId switch {
                    @"y" => typeof(AuthSuccessPacket),
                    @"n" => typeof(AuthFailPacket),
                    _ => typeof(UserConnectPacket),
                },
                @"2" => typeof(MessageAddPacket),
                @"3" => typeof(UserDisconnectPacket),
                @"4" => subPacketId switch {
                    @"0" => typeof(ChannelCreatePacket),
                    @"1" => typeof(ChannelUpdatePacket),
                    @"2" => typeof(ChannelDeletePacket),
                    _ => null,
                },
                @"5" => subPacketId switch {
                    @"0" => typeof(UserJoinPacket),
                    @"1" => typeof(UserLeavePacket),
                    @"2" => typeof(UserForceJoinPacket),
                    _ => null,
                },
                @"6" => typeof(MessageDeletePacket),
                @"7" => subPacketId switch {
                    @"0" => typeof(ContextUsersPacket),
                    @"1" => typeof(ContextMessageAddPacket),
                    @"2" => typeof(ContextChannelsPacket),
                    _ => null,
                },
                @"8" => typeof(ContextClearPacket),
                @"9" => typeof(ForcedDisconnectPacket),
                @"10" => typeof(UserUpdatePacket),
                @"11" => typeof(CapabilityConfirmPacket),
                @"12" => typeof(TypingInfoPacket),
                _ => null,
            };

            if(type == null)
                throw new Exception($@"Unknown packet type {packetId} ({subPacketId})");

            Packet packet = (Packet)Activator.CreateInstance(type, parts);

            switch(packet) {
                case PongPacket pp:
                    LastPong = pp.DateTime;
                    OnPong?.Invoke(LastPing);
                    break;

                case AuthSuccessPacket asp:
                    Version = asp.Extensions;
                    LastChannel = DefaultChannel = asp.Channel;

                    lock(UserSync) {
                        Users.Add(Bot);
                        User = asp.CreateUser();
                        Users.Add(User);
                        OnUserAdd?.Invoke(DateTimeOffset.Now, User);
                    }

                    if(Version >= 2) {
                        SessionId = asp.Session;
                        MaxLength = asp.MaxLength;
                        SendCapabilities(SupportedCapabilities);
                    }

                    PingThread?.Dispose();
                    if(PingThreadEnabled)
                        PingThread = new PingThread(this);

                    OnAuthSuccess?.Invoke(User);
                    break;

                case AuthFailPacket afp:
                    if(afp.Reason == @"joinfail") {
                        WasBanned = BanHasExpiry = true;
                        BanIsPermanent = afp.IsPermanent;
                        BanExpires = afp.Expiry;
                    }
                    OnAuthFail?.Invoke(afp.Reason, BanHasExpiry, BanIsPermanent, BanExpires);
                    break;

                case UserConnectPacket ucp:
                    lock(UserSync) {
                        ChatUser ucpu = ucp.CreateUser();
                        Users.Add(ucpu);
                        OnUserAdd?.Invoke(ucp.DateTime, ucpu);
                    }
                    break;

                case MessageAddPacket map:
                    lock(ChannelSync) {
                        lock(UserSync) {
                            lock(MessageSync) {
                                ChatMessage mapm = map.CreateMessage(Channels, Users);
                                Messages.Add(mapm);
                                OnMessageAdd?.Invoke(mapm);
                            }
                        }
                    }
                    break;

                case UserDisconnectPacket udp:
                    lock(UserSync) {
                        ChatUser udpu = Users.FirstOrDefault(u => u.UserId == udp.UserId);
                        if(udpu != null) {
                            Users.Remove(udpu);
                            OnUserRemove?.Invoke(udp.DateTime, udpu);
                        }
                    }
                    break;

                case ChannelCreatePacket ccp:
                    lock(ChannelSync) {
                        ChatChannel ccpc = ccp.CreateChannel();
                        Channels.Add(ccpc);
                        OnChannelAdd?.Invoke(ccpc);
                    }
                    break;

                case ChannelUpdatePacket cup:
                    lock(ChannelSync) {
                        ChatChannel cupc = Channels.FirstOrDefault(c => c.Name == cup.PreviousName);
                        if(cupc == null) {
                            cupc = cup.CreateChannel();
                            Channels.Add(cupc);
                            OnChannelAdd?.Invoke(cupc);
                        } else {
                            cup.UpdateChannel(cupc);
                            OnChannelUpdate?.Invoke(cupc);
                        }
                    }
                    break;

                case ChannelDeletePacket cdp:
                    lock(ChannelSync) {
                        ChatChannel cdpc = Channels.FirstOrDefault(c => c.Name == cdp.Name);
                        if(cdpc != null) {
                            Channels.Remove(cdpc);
                            OnChannelRemove?.Invoke(cdpc);
                        }
                    }
                    break;

                case UserJoinPacket ujp:
                    lock(ChannelSync) {
                        lock(UserSync) {
                            ChatChannel ujpc = null; // packet does not include channel name yet (should be shimmed for v1 with LastChannel)
                            if(ujpc != null) {
                                ChatUser ujpu = Users.FirstOrDefault(u => u.UserId == ujp.UserId) ?? ujp.CreateUser();
                                OnUserJoin?.Invoke(DateTimeOffset.Now, ujpc, ujpu);
                            }
                        }
                    }
                    break;

                case UserLeavePacket ulp:
                    lock(ChannelSync) {
                        lock(UserSync) {
                            ChatUser ulpu = Users.FirstOrDefault(u => u.UserId == ulp.UserId);
                            if(ulpu != null) {
                                ChatChannel ulpc = null; // packet does not include channel name yet (should be shimmed for v1 with LastChannel)
                                if(ulpc == null)
                                    OnUserLeave?.Invoke(DateTimeOffset.Now, ulpc, ulpu);
                            }
                        }
                    }
                    break;

                case UserForceJoinPacket ufjp:
                    lock(ChannelSync) {
                        // figure out what to do here
                    }
                    break;

                case MessageDeletePacket mdp:
                    lock(MessageSync) {
                        ChatMessage mdpm = Messages.FirstOrDefault(m => m.MessageId == mdp.MessageId);
                        if(mdpm != null) {
                            Messages.Remove(mdpm);
                            OnMessageDelete?.Invoke(mdpm);
                        }
                    }
                    break;

                case ContextUsersPacket cup:
                    lock(UserSync) {
                        lock(ChannelSync) {
                            ChatChannel cupc = Channels.FirstOrDefault(c => c.Name == DefaultChannel);
                            if(cupc != null || !SupportsMultiChannel) {
                                IEnumerable<ChatUser> cupus = cup.CreateUsers();
                                foreach(ChatUser cupu in cupus) {
                                    if(!Users.Any(u => u.UserId == cupu.UserId)) {
                                        Users.Add(cupu);
                                        OnUserAdd?.Invoke(DateTimeOffset.Now, cupu);
                                    }

                                    OnUserJoin?.Invoke(DateTimeOffset.Now, cupc, cupu);
                                }
                            }
                        }
                    }
                    break;

                case ContextMessageAddPacket cmap:
                    lock(MessageSync) {
                        lock(UserSync) { // needs channel support
                            lock(ChannelSync) {
                                ChatMessage cmapm = cmap.CreateMessage(Channels, Users);
                                Messages.Add(cmapm);
                                OnMessageAdd?.Invoke(cmapm);
                            }
                        }
                    }
                    break;

                case ContextChannelsPacket ccp:
                    lock(ChannelSync) {
                        IEnumerable<ChatChannel> ccpcs = ccp.CreateChannels();
                        foreach(ChatChannel ccpc in ccpcs) {
                            if(!Channels.Any(c => c.Name == ccpc.Name)) {
                                Channels.Add(ccpc);
                                OnChannelAdd?.Invoke(ccpc);
                            }
                        }
                    }
                    break;

                case ContextClearPacket ccp:
                    lock(MessageSync) {
                        lock(ChannelSync) {
                            lock(UserSync) {
                                ChatChannel channel = null; // packet needs a channel param

                                if(ccp.ClearMessages) {
                                    Messages.Clear();
                                    OnMessagesClear?.Invoke(channel);
                                }

                                if(ccp.ClearUsers) {
                                    Users.Clear();
                                    Users.Add(Bot);
                                    OnUsersClear?.Invoke(channel);
                                }

                                if(ccp.ClearChannels) {
                                    Channels.Clear();
                                    OnChannelsClear?.Invoke();
                                }
                            }
                        }
                    }
                    break;

                case ForcedDisconnectPacket fdp:
                    WasBanned = true;
                    BanHasExpiry = fdp.HasExpiry;
                    BanIsPermanent = fdp.IsPermanent;
                    BanExpires = fdp.Expiry;
                    OnForceDisconnect?.Invoke(fdp.HasExpiry, fdp.IsPermanent, fdp.Expiry);
                    break;

                case UserUpdatePacket uup:
                    lock(UserSync) {
                        ChatUser uupu = Users.FirstOrDefault(u => u.UserId == uup.UserId);
                        if(uupu == null) {
                            uupu = uup.CreateUser();
                            Users.Add(uupu);
                            OnUserAdd?.Invoke(DateTimeOffset.Now, uupu);
                        } else {
                            uup.UpdateUser(uupu);
                            OnUserUpdate?.Invoke(uupu);
                        }
                    }
                    break;

                case CapabilityConfirmPacket ccp:
                    Capabilities = ccp.Capabilities;
                    SupportsMultiChannel = ccp.SupportsMultiChannel;
                    SupportsTypingEvent = ccp.SupportsTypingEvent;
                    OnCapabilitiesUpdate?.Invoke(Capabilities);
                    break;

                case TypingInfoPacket tip:
                    lock(ChannelSync) {
                        lock(UserSync) {
                            ChatChannel tipc = Channels.FirstOrDefault(c => c.Name == tip.Channel);
                            if(tipc != null) {
                                ChatUser tipu = Users.FirstOrDefault(u => u.UserId == tip.UserId);
                                if(tipu != null)
                                    OnUserTyping?.Invoke(tipc, tipu, tip.DateTime);
                            }
                        }
                    }
                    break;
            }

            OnReceive?.Invoke(packet);
        }

        private bool IsDisposed;
        ~SockChatClient()
            => DoDispose();
        public void Dispose() {
            DoDispose();
            GC.SuppressFinalize(this);
        }
        private void DoDispose() {
            if(IsDisposed)
                return;
            IsDisposed = true;
            PingThread?.Dispose();
            WebSocket.Dispose();
        }
    }
}
