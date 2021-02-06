using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class TypingInfoPacket : Packet {
        public string Channel { get; }
        public long UserId { get; }
        public DateTimeOffset DateTime { get; }

        public TypingInfoPacket(IEnumerable<string> data) : base(data) {
            Channel = data.ElementAt(1);
            UserId = long.Parse(data.ElementAt(2));
            DateTime = DateTimeOffset.FromUnixTimeSeconds(int.Parse(data.ElementAt(3)));
        }

        public override string ToString() {
            return $@"[{DateTime:HH:mm:ss}] {UserId} is typing in {Channel}...";
        }
    }
}
