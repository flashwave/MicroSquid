using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class UserConnectPacket : Packet {
        public DateTimeOffset DateTime { get; }
        public long UserId { get; }
        public string UserName { get; }
        public Colour UserColour { get; }
        public IEnumerable<int> Perms { get; }
        public long SequenceId { get; }

        public UserConnectPacket(IEnumerable<string> data) : base(data) {
            DateTime = DateTimeOffset.FromUnixTimeSeconds(int.Parse(data.ElementAt(1)));
            UserId = long.Parse(data.ElementAt(2));
            UserName = data.ElementAt(3);
            UserColour = data.ElementAt(4);
            string perms = data.ElementAt(5);
            Perms = perms.Split(perms.Contains('\f') ? '\f' : ' ').Select(x => int.Parse(x));
            SequenceId = long.Parse(data.ElementAt(6));
        }

        public ChatUser CreateUser() {
            return new ChatUser(UserId, UserName, UserColour, Perms);
        }

        public override string ToString() {
            return $@"[{DateTime:HH:mm:ss}] {UserId}/{UserName} with colour {UserColour} has connected.";
        }
    }
}
