using System;

namespace MicroSquid {
    public class ChatChannel {
        public string Name { get; set; }
        public bool HasPassword { get; set; }
        public bool IsTemporary { get; set; }

        public ChatChannel(string name, bool hasPassword, bool isTemporary) {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            HasPassword = hasPassword;
            IsTemporary = isTemporary;
        }

        public override string ToString() {
            return Name;
        }
    }
}
