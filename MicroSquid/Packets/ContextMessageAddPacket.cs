using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class ContextMessageAddPacket : ContextPopulatePacket {
        public DateTimeOffset DateTime { get; }
        public long UserId { get; }
        public string UserName { get; }
        public Colour UserColour { get; }
        public IEnumerable<int> Perms { get; }
        public string Text { get; }
        public long MessageId { get; }
        public bool Notify { get; }
        public bool IsWelcome { get; }

        public IEnumerable<bool> Flags { get; }

        public ContextMessageAddPacket(IEnumerable<string> data) : base(data) {
            DateTime = DateTimeOffset.FromUnixTimeSeconds(int.Parse(data.ElementAt(2)));
            UserId = long.Parse(data.ElementAt(3));
            UserName = data.ElementAt(4);
            UserColour = data.ElementAt(5);
            string perms = data.ElementAt(6);
            Perms = perms.Split(perms.Contains('\f') ? '\f' : ' ').Select(x => int.Parse(x));
            Text = data.ElementAt(7);
            string msgId = data.ElementAt(8);
            MessageId = (IsWelcome = msgId == @"welcome") ? -1 : long.Parse(msgId);
            Notify = data.ElementAt(9) != @"0";
            Flags = data.ElementAt(10).ToCharArray().Select(c => c != '0');
        }

        public ChatMessage CreateMessage(IEnumerable<ChatUser> users) {
            return new ChatMessage(
                MessageId,
                DateTime,
                Text,
                users?.FirstOrDefault(u => u.UserId == UserId) ?? new ChatUser(UserId, UserName, UserColour, Perms, true),
                UserName,
                UserColour,
                Perms,
                Notify
            );
        }
    }
}
