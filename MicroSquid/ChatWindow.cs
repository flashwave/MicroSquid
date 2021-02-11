using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace MicroSquid {
    public partial class ChatWindow : Form {
        public SockChatClient ChatClient { get; private set; }
        private object LogSync { get; } = new object();

        public ChatWindow() {
            InitializeComponent();
        }

        private void ChatWindow_Shown(object sender, EventArgs e) {
            ShowAuthDiag(@"Log in to Chat");
        }

        private void ShowAuthDiag(string title) {
            SendButton.Enabled = MessageInput.Enabled = false;
            ChannelList.Items.Clear();
            UserList.Items.Clear();
            UpdateChannelDisplay(false);
            Update();

            string serverUrl = string.Empty, authToken = string.Empty;
            using(AuthWindow aw = new AuthWindow { Text = title }) {
                aw.ShowDialog(this);
                if(aw.DialogResult != DialogResult.OK) {
                    Application.Exit();
                    return;
                }

                serverUrl = aw.ServerUrl;
                authToken = aw.AuthToken;
            }

            ChatClient?.Dispose();
            ChatClient = new SockChatClient(serverUrl);
            ChatClient.OnOpen += ChatClient_OnOpen;
            ChatClient.OnClose += ChatClient_OnClose;
            ChatClient.OnReceive += ChatClient_OnReceive;
            ChatClient.OnUserAdd += ChatClient_OnUserAdd; // This should be replaced by user join probably
            ChatClient.OnUserRemove += ChatClient_OnUserRemove;
            ChatClient.OnChannelAdd += ChatClient_OnChannelAdd;
            ChatClient.OnChannelRemove += ChatClient_OnChannelRemove;
            ChatClient.OnUserTyping += ChatClient_OnUserTyping;
            ChatClient.OnUsersClear += ChatClient_OnUsersClear;
            ChatClient.OnChannelsClear += ChatClient_OnChannelsClear;
            ChatClient.OnAuthSuccess += ChatClient_OnAuthSuccess;
            ChatClient.OnAuthFail += ChatClient_OnAuthFail;
            ChatClient.OnForceDisconnect += ChatClient_OnForceDisconnect;
            ChatClient.OnCapabilitiesUpdate += ChatClient_OnCapabilitiesUpdate;
            ChatClient.Connect(authToken);
        }

        private void ChatClient_OnCapabilitiesUpdate(IEnumerable<string> caps) {
            DoInvoke(() => UpdateChannelDisplay(caps.Contains(SockChatClient.CAP_MULTI_CHANNEL)));
        }

        private void UpdateChannelDisplay(bool supportsMultiChannel) {
            AllChannelsButton.Enabled = LeaveChannelButton.Enabled = supportsMultiChannel;

            // Update on-screen list to only show channels we're currently in
            // Leave button should issue the /leave command
            // Channels button should open a new window with a list of all channels
            // Meaning 7.2 should probably not be fired immediately.
            // I should probably make it possible to specify capabilities in the auth packet, or maybe before it
            // issue is that the auth packet has no format so there should be some kind of "unique enough" prefix to allow identification
            // We already know the main channel from 2.y and v2/MCHAN supports being in 0 channels so we can still autojoin the lounge.
        }

        private void DisplayBan(bool hasExpiry, bool isPermanent, DateTimeOffset expiry) {
            using BanNotice bn = new BanNotice(hasExpiry, isPermanent, expiry);
            bn.ShowDialog(this);
            ShowAuthDiag(@"You were banned.");
        }

        private void ChatClient_OnAuthFail(string arg1, bool arg2, bool arg3, DateTimeOffset arg4) {
            if(arg2)
                DoInvoke(() => DisplayBan(arg2, arg3, arg4));
        }

        private void ChatClient_OnForceDisconnect(bool arg1, bool arg2, DateTimeOffset arg3) {
            DoInvoke(() => DisplayBan(arg1, arg2, arg3));
        }

        private void ChatClient_OnUsersClear(ChatChannel obj) {
            DoInvoke(() => UserList.Items.Clear());
        }

        private void ChatClient_OnChannelsClear() {
            DoInvoke(() => ChannelList.Items.Clear());
        }

        private void ChatClient_OnUserTyping(ChatChannel arg1, ChatUser arg2, DateTimeOffset arg3) {
            DoInvoke(() => StatusLabel.Text = $@"{arg2.UserName} is typing...");
        }

        private void ChatClient_OnAuthSuccess(ChatUser obj) {
            DoInvoke(() => { SendButton.Enabled = MessageInput.Enabled = true; });
        }

        private void ChatClient_OnChannelAdd(ChatChannel obj) {
            DoInvoke(() => ChannelList.Items.Add(obj));
        }

        private void ChatClient_OnChannelRemove(ChatChannel obj) {
            DoInvoke(() => ChannelList.Items.Remove(obj));
        }

        private void ChatClient_OnUserAdd(DateTimeOffset arg1, ChatUser arg2) {
            if(arg2.IsVisible)
                DoInvoke(() => UserList.Items.Add(arg2));
        }

        private void ChatClient_OnUserRemove(DateTimeOffset arg1, ChatUser arg2) {
            if(arg2.IsVisible)
                DoInvoke(() => UserList.Items.Remove(arg2));
        }

        private void ChatClient_OnOpen() {
            DoInvoke(() => WriteLog(@"****** CONNECTED ******"));
        }

        private void ChatClient_OnClose(bool wasClean) {
            DoInvoke(() => {
                WriteLog(@"****** DISCONNECTED ******");
                if(!ChatClient.WasBanned)
                    ShowAuthDiag(@"Connection lost.");
            });
        }

        private void ChatClient_OnReceive(Packet obj) {
            DoInvoke(() => WriteLog(obj));
        }

        public void WriteLog(object obj)
            => WriteLog(obj.ToString());

        public void DoInvoke(Action action) {
            if(InvokeRequired)
                Invoke(action);
            else
                action.Invoke();
        }

        public void WriteLog(string str) {
            if(IsDisposed)
                return;
            lock(LogSync) {
                MessageHistory.Text += str + Environment.NewLine;
                MessageHistory.SelectionStart = MessageHistory.TextLength;
                MessageHistory.ScrollToCaret();
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            ChatClient.Dispose();
        }

        private void SendButton_Click(object sender, EventArgs e) {
            if(MessageInput.Text.Length < 1)
                return;
            ChatClient.SendMessage(ChatClient.DefaultChannel, MessageInput.Text);
            MessageInput.Text = string.Empty;
        }

        private void MessageInput_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.Enter && !e.Shift) {
                e.Handled = e.SuppressKeyPress = true;
                SendButton.PerformClick();
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            MessageHistory.Clear();
        }

        private void ShowUserActions() {
            if(UserList.SelectedItem is ChatUser cu)
                Debug.WriteLine($@"Show user actions for {cu}");
        }

        private void UserList_DoubleClick(object sender, EventArgs e) {
            ShowUserActions();
        }

        private void UserList_KeyDown(object sender, KeyEventArgs e) {
            switch(e.KeyCode) {
                case Keys.Enter:
                    ShowUserActions();
                    break;
                case Keys.Delete:
                    if(UserList.SelectedItem is ChatUser cu)
                        ChatClient.SendKick(cu);
                    break;
            }
        }

        private void JoinChannel() {
            if(ChannelList.SelectedItem is ChatChannel cc)
                Debug.WriteLine($@"Join {cc}");
        }

        private void ChannelList_DoubleClick(object sender, EventArgs e) {
            JoinChannel();
        }

        private void ChannelList_KeyDown(object sender, KeyEventArgs e) {
            switch(e.KeyCode) {
                case Keys.Enter:
                    JoinChannel();
                    break;
            }
        }
    }
}
