using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class ForcedDisconnectPacket : Packet {
        public bool HasExpiry { get; }
        public DateTimeOffset Expiry { get; }

        public ForcedDisconnectPacket(IEnumerable<string> data) : base(data) {
            HasExpiry = data.ElementAt(1) != @"0";
            if(HasExpiry)
                Expiry = DateTimeOffset.FromUnixTimeSeconds(int.Parse(data.ElementAt(2)));
        }
    }
}
