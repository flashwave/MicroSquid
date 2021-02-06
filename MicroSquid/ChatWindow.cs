using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSquid {
    public partial class ChatWindow : Form {
        public SockChatClient ChatClient { get; private set; }
        private object LogSync { get; } = new object();

        public ChatWindow() {
            InitializeComponent();
        }

        private void ChatWindow_Shown(object sender, EventArgs e) {
            ShowAuthDiag(@"Log in to Chat");
        }

        private void ShowAuthDiag(string title) {
            string serverUrl = string.Empty, authToken = string.Empty;
            using(AuthWindow aw = new AuthWindow { Text = title }) {
                aw.ShowDialog();
                serverUrl = aw.ServerUrl;
                authToken = aw.AuthToken;
            }

            if(ChatClient?.Server != serverUrl) {
                ChatClient?.Dispose();
                ChatClient = new SockChatClient(serverUrl);
                ChatClient.OnOpen += ChatClient_OnOpen;
                ChatClient.OnClose += ChatClient_OnClose;
                ChatClient.OnReceive += ChatClient_OnReceive;
            } else
                ChatClient.Disconnect();

            ChatClient.Connect(authToken);
        }

        private void ChatClient_OnOpen() {
            Debug.WriteLine(@"Connection opened.");
        }

        private void ChatClient_OnClose() {
            ShowAuthDiag(@"Connection lost.");
        }

        private void ChatClient_OnReceive(Packet obj) {
            WriteLog(obj);
        }

        public void WriteLog(object obj)
            => WriteLog(obj.ToString());

        private Action<ChatWindow, string> WriteLogBody { get; } = new Action<ChatWindow, string>((self, str) => {
            lock(self.LogSync) {
                self.MessageHistory.Text += str + Environment.NewLine;
            }
        });
        public void WriteLog(string str) {
            if(InvokeRequired)
                Invoke(WriteLogBody, this, str);
            else
                WriteLogBody(this, str);
        }

        private void button1_Click(object sender, EventArgs e) {
            ChatClient.Disconnect();
        }

        private void SendButton_Click(object sender, EventArgs e) {
            if(MessageInput.Text.Length < 1)
                return;
            ChatClient.SendMessage(string.Empty, MessageInput.Text);
            MessageInput.Text = string.Empty;
        }

        private void MessageInput_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.Enter && !e.Shift)
                SendButton.PerformClick();
        }
    }
}
