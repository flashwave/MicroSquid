using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class UserForceJoinPacket : UserSwitchPacket {
        public string Channel { get; }

        public UserForceJoinPacket(IEnumerable<string> data) : base(data) {
            Channel = data.ElementAt(2);
        }

        public override string ToString() {
            return $@"[{DateTimeOffset.Now:HH:mm:ss}] You were forced to join {Channel}.";
        }
    }
}
