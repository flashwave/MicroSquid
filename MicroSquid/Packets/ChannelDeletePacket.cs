using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class ChannelDeletePacket : ChannelPacket {
        public string Name { get; }

        public ChannelDeletePacket(IEnumerable<string> data) : base(data) {
            Name = data.ElementAt(2);
        }
    }
}
