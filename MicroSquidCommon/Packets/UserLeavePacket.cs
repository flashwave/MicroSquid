using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class UserLeavePacket : UserSwitchPacket {
        public long UserId { get; }
        public long SequenceId { get; }

        public UserLeavePacket(IEnumerable<string> data) : base(data) {
            UserId = long.Parse(data.ElementAt(2));
            SequenceId = long.Parse(data.ElementAt(3));
        }

        public override string ToString() {
            return $@"[{DateTimeOffset.Now:HH:mm:ss}] {UserId} left this channel (which channel??????).";
        }
    }
}
