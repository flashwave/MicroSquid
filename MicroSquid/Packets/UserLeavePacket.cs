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
    }
}
