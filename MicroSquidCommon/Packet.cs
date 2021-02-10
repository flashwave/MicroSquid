using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid {
    public abstract class Packet {
        public string PacketId { get; }
        public IEnumerable<string> Raw { get; }

        public Packet(IEnumerable<string> data) {
            Raw = data ?? throw new ArgumentNullException(nameof(data));
            PacketId = data.ElementAt(0);
        }
    }
}
