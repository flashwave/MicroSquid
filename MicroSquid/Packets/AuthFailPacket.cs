using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class AuthFailPacket : AuthPacket {
        public string Reason { get; }
        public DateTimeOffset Until { get; }

        public AuthFailPacket(IEnumerable<string> data) : base(data) {
            Reason = data.ElementAt(2);
            if(Reason == @"joinfail")
                Until = DateTimeOffset.FromUnixTimeSeconds(int.Parse(data.ElementAt(3)));
        }

        public override string ToString() {
            return $@"[{DateTimeOffset.Now:HH:mm:ss}] Authentication failed: {Reason} (Until: {Until}).";
        }
    }
}
