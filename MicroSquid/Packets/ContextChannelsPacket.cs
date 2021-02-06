using System.Collections.Generic;
using System.Linq;

namespace MicroSquid.Packets {
    public class ContextChannelsPacket : ContextPopulatePacket {
        public int Count { get; }
        public IEnumerable<ChannelInfo> Channels { get; }

        public ContextChannelsPacket(IEnumerable<string> data) : base(data) {
            Count = int.Parse(data.ElementAt(2));
            Channels = ReadChannels(Count, data.Skip(3));
        }

        private static IEnumerable<ChannelInfo> ReadChannels(int count, IEnumerable<string> data) {
            for(int i = 0; i < count; ++i)
                yield return new ChannelInfo(data.Skip(3 * i));
        }

        public class ChannelInfo {
            public string Name { get; }
            public bool HasPassword { get; }
            public bool IsTemporary { get; }

            public ChannelInfo(IEnumerable<string> data) {
                Name = data.ElementAt(0);
                HasPassword = data.ElementAt(1) != @"0";
                IsTemporary = data.ElementAt(2) != @"0";
            }

            public ChatChannel CreateChannel() {
                return new ChatChannel(Name, HasPassword, IsTemporary);
            }
        }

        public IEnumerable<ChatChannel> CreateChannels() {
            return Channels.Select(c => c.CreateChannel());
        }
    }
}
