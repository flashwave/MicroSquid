using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class ChannelUpdatePacket : ChannelPacket {
        public string PreviousName { get; }
        public string NewName { get; }
        public bool HasPassword { get; }
        public bool IsTemporary { get; }

        public ChannelUpdatePacket(IEnumerable<string> data) : base(data) {
            PreviousName = data.ElementAt(2);
            NewName = data.ElementAt(3);
            HasPassword = data.ElementAt(4) != @"0";
            IsTemporary = data.ElementAt(5) != @"0";
        }

        public void UpdateChannel(ChatChannel channel) {
            channel.Name = NewName;
            channel.HasPassword = HasPassword;
            channel.IsTemporary = IsTemporary;
        }

        public ChatChannel CreateChannel() {
            return new ChatChannel(NewName, HasPassword, IsTemporary);
        }

        public override string ToString() {
            return $@"[{DateTimeOffset.Now:HH:mm:ss}] Channel {PreviousName} was renamed to {NewName}.";
        }
    }
}
