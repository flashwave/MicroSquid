using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class ForcedDisconnectPacket : Packet {
        public bool HasExpiry { get; }
        public bool IsPermanent { get; }
        public DateTimeOffset Expiry { get; }

        public ForcedDisconnectPacket(IEnumerable<string> data) : base(data) {
            HasExpiry = data.ElementAt(1) != @"0";

            if(HasExpiry) {
                int expiry = int.Parse(data.ElementAt(2));

                if(expiry == -1) {
                    IsPermanent = true;
                    Expiry = DateTimeOffset.MaxValue;
                } else
                    Expiry = DateTimeOffset.FromUnixTimeSeconds(expiry);
            }
        }

        public override string ToString() {
            return $@"[{DateTimeOffset.Now:HH:mm:ss}] You have been banned until {Expiry}.";
        }
    }
}
