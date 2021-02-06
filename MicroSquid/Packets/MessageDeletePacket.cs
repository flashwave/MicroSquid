using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class MessageDeletePacket : Packet {
        public long MessageId { get; }

        public MessageDeletePacket(IEnumerable<string> data) : base(data) {
            MessageId = long.Parse(data.ElementAt(1));
        }

        public override string ToString() {
            return $@"[{DateTimeOffset.Now:HH:mm:ss}] Message #{MessageId} has been deleted.";
        }
    }
}
