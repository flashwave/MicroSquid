using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class MessageDeletePacket : Packet {
        public long MessageId { get; }

        public MessageDeletePacket(IEnumerable<string> data) : base(data) {
            MessageId = long.Parse(data.ElementAt(1));
        }
    }
}
