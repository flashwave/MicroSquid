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
            SendButton.Enabled = MessageInput.Enabled = false;
            ChannelList.Items.Clear();
            UserList.Items.Clear();
            Update();

            string serverUrl = string.Empty, authToken = string.Empty;
            using(AuthWindow aw = new AuthWindow { Text = title }) {
                aw.ShowDialog();
                if(aw.DialogResult != DialogResult.OK) {
                    Application.Exit();
                    return;
                }

                serverUrl = aw.ServerUrl;
                authToken = aw.AuthToken;
            }

            ChatClient?.Dispose();
            ChatClient = new SockChatClient(serverUrl);
            ChatClient.OnOpen += ChatClient_OnOpen;
            ChatClient.OnClose += ChatClient_OnClose;
            ChatClient.OnReceive += ChatClient_OnReceive;
            ChatClient.OnUserAdd += ChatClient_OnUserAdd; // This should be replaced by user join probably
            ChatClient.OnUserRemove += ChatClient_OnUserRemove;
            ChatClient.OnChannelAdd += ChatClient_OnChannelAdd;
            ChatClient.OnChannelRemove += ChatClient_OnChannelRemove;
            ChatClient.OnUserTyping += ChatClient_OnUserTyping;
            ChatClient.OnUsersClear += ChatClient_OnUsersClear;
            ChatClient.OnChannelsClear += ChatClient_OnChannelsClear;
            ChatClient.OnAuthSuccess += ChatClient_OnAuthSuccess;
            ChatClient.Connect(authToken);
        }

        private void ChatClient_OnUsersClear(ChatChannel obj) {
            DoInvoke(() => UserList.Items.Clear());
        }

        private void ChatClient_OnChannelsClear() {
            DoInvoke(() => ChannelList.Items.Clear());
        }

        private void ChatClient_OnUserTyping(ChatChannel arg1, ChatUser arg2, DateTimeOffset arg3) {
            DoInvoke(() => StatusLabel.Text = $@"{arg2.UserName} is typing...");
        }

        private void ChatClient_OnAuthSuccess(ChatUser obj) {
            DoInvoke(() => { SendButton.Enabled = MessageInput.Enabled = true; });
        }

        private void ChatClient_OnChannelAdd(ChatChannel obj) {
            DoInvoke(() => ChannelList.Items.Add(obj));
        }

        private void ChatClient_OnChannelRemove(ChatChannel obj) {
            DoInvoke(() => ChannelList.Items.Remove(obj));
        }

        private void ChatClient_OnUserAdd(DateTimeOffset arg1, ChatUser arg2) {
            if(arg2.IsVisible)
                DoInvoke(() => UserList.Items.Add(arg2));
        }

        private void ChatClient_OnUserRemove(DateTimeOffset arg1, ChatUser arg2) {
            if(arg2.IsVisible)
                DoInvoke(() => UserList.Items.Remove(arg2));
        }

        private void ChatClient_OnOpen() {
            DoInvoke(() => WriteLog(@"****** CONNECTED ******"));
        }

        private void ChatClient_OnClose() {
            DoInvoke(() => {
                WriteLog(@"****** DISCONNECTED ******");
                ShowAuthDiag(@"Connection lost.");
            });
        }

        private void ChatClient_OnReceive(Packet obj) {
            DoInvoke(() => WriteLog(obj));
        }

        public void WriteLog(object obj)
            => WriteLog(obj.ToString());

        public void DoInvoke(Action action) {
            if(InvokeRequired)
                Invoke(action);
            else
                action.Invoke();
        }

        public void WriteLog(string str) {
            lock(LogSync) {
                MessageHistory.Text += str + Environment.NewLine;
                MessageHistory.SelectionStart = MessageHistory.TextLength;
                MessageHistory.ScrollToCaret();
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            ChatClient.Dispose();
        }

        private void SendButton_Click(object sender, EventArgs e) {
            if(MessageInput.Text.Length < 1)
                return;
            ChatClient.SendMessage(ChatClient.DefaultChannel, MessageInput.Text);
            MessageInput.Text = string.Empty;
        }

        private void MessageInput_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.Enter && !e.Shift) {
                e.Handled = e.SuppressKeyPress = true;
                SendButton.PerformClick();
            }
        }
    }
}
