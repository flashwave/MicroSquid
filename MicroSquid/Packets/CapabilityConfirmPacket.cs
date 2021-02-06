using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class CapabilityConfirmPacket : Packet {
        public IEnumerable<string> Capabilities { get; }
        public bool SupportsTypingEvent { get; }
        public bool SupportsMultiChannel { get; }

        public CapabilityConfirmPacket(IEnumerable<string> data) : base(data) {
            Capabilities = data.ElementAt(1).Split(' ');
            SupportsTypingEvent = Capabilities.Contains(@"TYPING");
            SupportsMultiChannel = Capabilities.Contains(@"MCHAN");
        }
    }
}
