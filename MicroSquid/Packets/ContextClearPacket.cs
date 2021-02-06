using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class ContextClearPacket : Packet {
        public bool ClearMessages { get; }
        public bool ClearUsers { get; }
        public bool ClearChannels { get; }
        public string Channel { get; }

        public ContextClearPacket(IEnumerable<string> data) : base(data) {
            string mode = data.ElementAt(1);
            ClearMessages = mode == @"0" || mode == @"3" || mode == @"4";
            ClearUsers = mode == @"1" || mode == @"3" || mode == @"4";
            ClearMessages = mode == @"2" || mode == @"4";
            Channel = data.ElementAtOrDefault(2) ?? string.Empty;
        }
    }
}
