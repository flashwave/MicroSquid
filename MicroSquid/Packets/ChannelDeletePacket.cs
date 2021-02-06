using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class ChannelDeletePacket : ChannelPacket {
        public string Name { get; }

        public ChannelDeletePacket(IEnumerable<string> data) : base(data) {
            Name = data.ElementAt(2);
        }

        public override string ToString() {
            return $@"[{DateTimeOffset.Now:HH:mm:ss}] Channel {Name} was deleted.";
        }
    }
}
