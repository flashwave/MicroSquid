using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public abstract class ChannelPacket : Packet {
        public string SubPacketId { get; }

        protected ChannelPacket(IEnumerable<string> data) : base(data) {
            SubPacketId = data.ElementAt(1);
        }
    }
}
