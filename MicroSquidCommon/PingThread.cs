using System;
using System.Threading;

namespace MicroSquid {
    public class PingThread : IDisposable {
        private Thread Thread { get; }
        private SockChatClient Client { get; }
        private TimeSpan CheckInterval { get; }
        private TimeSpan SendInterval { get; }

        private const int CHECK_INTERVAL = 5;

        public bool IsActive { get; private set; }

        public PingThread(SockChatClient client, int interval = 30) {
            CheckInterval = TimeSpan.FromSeconds(interval < CHECK_INTERVAL ? interval : CHECK_INTERVAL);
            SendInterval = TimeSpan.FromSeconds(interval);
            IsActive = true;
            Client = client ?? throw new ArgumentNullException(nameof(client));
            Thread = new Thread(Body) { IsBackground = true };
            Thread.Start();
        }

        private void Body() {
            do {
                if(IsActive && (DateTimeOffset.Now - Client.LastPing) >= SendInterval)
                    Client.SendPing();
                Thread.Sleep(CheckInterval);
            } while(IsActive);
        }

        ~PingThread()
            => DoDispose();
        public void Dispose() {
            DoDispose();
            GC.SuppressFinalize(this);
        }
        private void DoDispose() {
            if(!IsActive)
                return;
            IsActive = false;
        }
    }
}
