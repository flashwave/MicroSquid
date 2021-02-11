using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class AuthSuccessPacket : AuthPacket {
        public long UserId { get; }
        public string UserName { get; }
        public Colour UserColour { get; }
        public IEnumerable<int> Perms { get; }
        public string Channel { get; }
        public int Extensions { get; }
        public string Session { get; }
        public int MaxLength { get; }

        public AuthSuccessPacket(IEnumerable<string> data) : base(data) {
            UserId = long.Parse(data.ElementAt(2));
            UserName = data.ElementAt(3);
            UserColour = data.ElementAt(4);
            string perms = data.ElementAt(5);
            Perms = perms.Split(perms.Contains('\f') ? '\f' : ' ').Select(x => int.Parse(x));
            Channel = data.ElementAt(6);
            Extensions = int.TryParse(data.ElementAtOrDefault(7), out int exts) ? exts : 1;

            if(Extensions >= 2) {
                Session = data.ElementAt(8);

                // Flashii runs an incomplete/experimental version of v2, just revert to v1 if MaxLength is missing.
                if(int.TryParse(data.ElementAtOrDefault(9), out int maxLength))
                    MaxLength = maxLength;
                else {
                    Extensions = 1;
                    MaxLength = -1;
                }
            }
        }

        public ChatUser CreateUser() {
            return new ChatUser(UserId, UserName, UserColour, Perms, isPrimary: true);
        }

        public override string ToString() {
            return $@"[{DateTimeOffset.Now:HH:mm:ss}] Successfully authenticated as {UserId}/{UserName} with colour {UserColour} with Extensions v{Extensions} and Session ID {Session} the Default Channel is {Channel} and the maximum message length is {MaxLength} characters.";
        }
    }
}
