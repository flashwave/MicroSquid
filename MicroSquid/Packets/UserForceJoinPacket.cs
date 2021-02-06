using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class UserForceJoinPacket : UserSwitchPacket {
        public string Channel { get; }

        public UserForceJoinPacket(IEnumerable<string> data) : base(data) {
            Channel = data.ElementAt(2);
        }
    }
}
