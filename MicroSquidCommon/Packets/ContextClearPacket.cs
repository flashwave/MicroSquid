using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class ContextClearPacket : Packet {
        public string Mode { get; }
        public bool ClearMessages { get; }
        public bool ClearUsers { get; }
        public bool ClearChannels { get; }
        public string Channel { get; }

        public ContextClearPacket(IEnumerable<string> data) : base(data) {
            Mode = data.ElementAt(1);
            ClearMessages = Mode == @"0" || Mode == @"3" || Mode == @"4";
            ClearUsers = Mode == @"1" || Mode == @"3" || Mode == @"4";
            ClearMessages = Mode == @"2" || Mode == @"4";
            Channel = data.ElementAtOrDefault(2) ?? string.Empty;
        }

        public override string ToString() {
            return $@"[{DateTimeOffset.Now:HH:mm:ss}] A context clear with mode {Mode} was issued.";
        }
    }
}
