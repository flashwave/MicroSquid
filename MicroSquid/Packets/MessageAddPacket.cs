using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class MessageAddPacket : Packet {
        public DateTimeOffset DateTime { get; }
        public long UserId { get; }
        public string Text { get; }
        public long MessageId { get; }
        public IEnumerable<bool> Flags { get; }
        public string Channel { get; }

        public MessageAddPacket(IEnumerable<string> data) : base(data) {
            DateTime = DateTimeOffset.FromUnixTimeSeconds(int.Parse(data.ElementAt(1)));
            UserId = long.Parse(data.ElementAt(2));
            Text = data.ElementAt(3);
            MessageId = long.Parse(data.ElementAt(4));
            Flags = data.ElementAt(5).ToCharArray().Select(c => c != '0');
            Channel = data.ElementAtOrDefault(6) ?? string.Empty;
        }

        public ChatMessage CreateMessage(IEnumerable<ChatChannel> channels, IEnumerable<ChatUser> users) {
            return new ChatMessage(
                MessageId,
                DateTime,
                channels.First(c => c.Name == Channel),
                Text,
                Flags,
                users.First(u => u.UserId == UserId)
            );
        }

        public override string ToString() {
            return $@"[{DateTime:HH:mm:ss}] #{MessageId} <{UserId}> {Text}";
        }
    }
}
