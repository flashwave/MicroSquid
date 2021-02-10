using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class AuthFailPacket : AuthPacket {
        public string Reason { get; }
        public bool IsPermanent { get; }
        public DateTimeOffset Expiry { get; }

        public AuthFailPacket(IEnumerable<string> data) : base(data) {
            Reason = data.ElementAt(2);
            if(Reason == @"joinfail") {
                int expiry = int.Parse(data.ElementAt(3));

                if(expiry == -1) {
                    IsPermanent = true;
                    Expiry = DateTimeOffset.MaxValue;
                } else
                    Expiry = DateTimeOffset.FromUnixTimeSeconds(expiry);
            }
        }

        public override string ToString() {
            return $@"[{DateTimeOffset.Now:HH:mm:ss}] Authentication failed: {Reason} (Until: {Expiry}).";
        }
    }
}
