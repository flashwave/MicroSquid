using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class ContextUsersPacket : ContextPopulatePacket {
        public int Count { get; }
        public IEnumerable<UserInfo> Users { get; }

        public ContextUsersPacket(IEnumerable<string> data) : base(data) {
            Count = int.Parse(data.ElementAt(2));
            Users = ReadUsers(Count, data.Skip(3));
        }

        private static IEnumerable<UserInfo> ReadUsers(int count, IEnumerable<string> data) {
            for(int i = 0; i < count; ++i)
                yield return new UserInfo(data.Skip(i * 5));
        }

        public class UserInfo {
            public long UserId { get; }
            public string UserName { get; }
            public Colour UserColour { get; }
            public IEnumerable<int> Perms { get; }
            public bool IsVisible { get; }

            public UserInfo(IEnumerable<string> data) {
                UserId = long.Parse(data.ElementAt(0));
                UserName = data.ElementAt(1);
                UserColour = data.ElementAt(2);
                string perms = data.ElementAt(3);
                Perms = perms.Split(perms.Contains('\f') ? '\f' : ' ').Select(x => int.Parse(x));
                IsVisible = data.ElementAt(4) != @"0";
            }

            public ChatUser CreateUser() {
                return new ChatUser(UserId, UserName, UserColour, Perms, isVisible: IsVisible);
            }
        }

        public IEnumerable<ChatUser> CreateUsers() {
            return Users.Select(u => u.CreateUser());
        }
    }
}
