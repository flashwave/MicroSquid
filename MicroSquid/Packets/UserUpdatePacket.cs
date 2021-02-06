using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class UserUpdatePacket : Packet {
        public long UserId { get; }
        public string UserName { get; }
        public Colour UserColour { get; }
        public IEnumerable<int> Perms { get; }

        public UserUpdatePacket(IEnumerable<string> data) : base(data) {
            UserId = long.Parse(data.ElementAt(1));
            UserName = data.ElementAt(2);
            UserColour = data.ElementAt(3);
            string perms = data.ElementAt(4);
            Perms = perms.Split(perms.Contains('\f') ? '\f' : ' ').Select(x => int.Parse(x));
        }

        public void UpdateUser(ChatUser user) {
            user.UserName = UserName;
            user.UserColour = UserColour;
            user.Perms = Perms;
        }

        public ChatUser CreateUser() {
            return new ChatUser(UserId, UserName, UserColour, Perms);
        }
    }
}
