using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class PongPacket : Packet {
        public DateTimeOffset DateTime { get; }

        public PongPacket(IEnumerable<string> data) : base(data) {
            string arg = data.ElementAt(1);
            DateTime = arg == @"pong" ? DateTimeOffset.Now : DateTimeOffset.FromUnixTimeSeconds(int.Parse(arg));
        }
    }
}
