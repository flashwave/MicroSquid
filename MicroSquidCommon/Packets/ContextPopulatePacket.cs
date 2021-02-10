using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public abstract class ContextPopulatePacket : Packet {
        public string SubPacketId { get; }

        protected ContextPopulatePacket(IEnumerable<string> data) : base(data) {
            SubPacketId = data.ElementAt(1);
        }
    }
}
