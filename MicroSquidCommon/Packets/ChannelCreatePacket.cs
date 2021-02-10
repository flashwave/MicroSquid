using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class ChannelCreatePacket : ChannelPacket {
        public string Name { get; }
        public bool HasPassword { get; }
        public bool IsTemporary { get; }

        public ChannelCreatePacket(IEnumerable<string> data) : base(data) {
            Name = data.ElementAt(2);
            HasPassword = data.ElementAt(3) != @"0";
            IsTemporary = data.ElementAt(4) != @"0";
        }

        public ChatChannel CreateChannel() {
            return new ChatChannel(Name, HasPassword, IsTemporary);
        }

        public override string ToString() {
            return $@"[{DateTimeOffset.Now:HH:mm:ss}] Received channel with name {Name}.";
        }
    }
}
