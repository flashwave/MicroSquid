using System;
using System.Collections.Generic;

namespace MicroSquid {
    public class ChatMessage {
        public long MessageId { get; }
        public DateTimeOffset DateTime { get; }
        public ChatChannel Channel { get; }
        public string Text { get; }
        public IEnumerable<bool> Flags { get; }
        public bool Notify { get; }

        public ChatUser User { get; }
        public string UserName { get; }
        public Colour UserColour { get; }
        public IEnumerable<int> UserPerms { get; }

        public bool IsNotice => User.IsBot;
        public bool CanBeDeleted => MessageId > 0;

        public ChatMessage(
            long msgId,
            DateTimeOffset dateTime,
            ChatChannel channel,
            string text,
            IEnumerable<bool> flags,
            ChatUser user,
            string userName = null,
            Colour? userColour = null,
            IEnumerable<int> userPerms = null,
            bool notify = true
        ) {
            MessageId = msgId;
            DateTime = dateTime;
            Channel = channel;
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Flags = flags ?? throw new ArgumentNullException(nameof(flags));
            Notify = notify;

            User = user ?? throw new ArgumentNullException(nameof(user));
            UserName = userName ?? User.UserName;
            UserColour = userColour ?? User.UserColour;
            UserPerms = userPerms ?? User.Perms;
        }
    }
}
