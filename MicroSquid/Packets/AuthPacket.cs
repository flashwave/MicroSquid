using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public abstract class AuthPacket : Packet {
        public bool Success { get; }

        public AuthPacket(IEnumerable<string> data) : base(data) {
            Success = data.ElementAt(1) == @"y";
        }
    }
}
