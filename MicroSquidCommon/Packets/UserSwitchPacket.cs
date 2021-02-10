using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public abstract class UserSwitchPacket : Packet {
        public string SubPacketId { get; }

        protected UserSwitchPacket(IEnumerable<string> data) : base(data) {
            SubPacketId = data.ElementAt(1);
        }
    }
}
