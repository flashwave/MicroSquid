using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class UserDisconnectPacket : Packet {
        public long UserId { get; }
        public string UserName { get; }
        public string Reason { get; }
        public DateTimeOffset DateTime { get; }
        public long SequenceId { get; }

        public UserDisconnectPacket(IEnumerable<string> data) : base(data) {
            UserId = long.Parse(data.ElementAt(1));
            UserName = data.ElementAt(2);
            Reason = data.ElementAt(3);
            DateTime = DateTimeOffset.FromUnixTimeSeconds(int.Parse(data.ElementAt(4)));
            SequenceId = long.Parse(data.ElementAt(5));
        }

        public override string ToString() {
            return $@"[{DateTime:HH:mm:ss}] {UserId}/{UserName} has disconnected ({Reason}).";
        }
    }
}
