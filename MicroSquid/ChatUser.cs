using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid {
    public class ChatUser {
        public long UserId { get; }
        public string UserName { get; set; }
        public Colour UserColour { get; set; }
        public IEnumerable<int> Perms { get; set; }
        public bool IsFake { get; }
        public bool IsVisible { get; }
        public bool IsPrimary { get; }

        public bool IsBot => UserId == -1;
        public int Rank => Perms.FirstOrDefault();

        public ChatUser(long userId, string userName, Colour colour, IEnumerable<int> perms = null, bool isFake = false, bool isVisible = true, bool isPrimary = false) {
            UserId = userId;
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            UserColour = colour;
            Perms = perms ?? Enumerable.Empty<int>();
            IsFake = isFake;
            IsVisible = isVisible;
            IsPrimary = isPrimary;
        }
    }
}
