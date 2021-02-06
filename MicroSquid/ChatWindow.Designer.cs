﻿
namespace MicroSquid {
    partial class ChatWindow {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.MessageHistory = new System.Windows.Forms.RichTextBox();
            this.MessageInput = new System.Windows.Forms.RichTextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.UserList = new System.Windows.Forms.ListBox();
            this.ChannelList = new System.Windows.Forms.ListBox();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MessageHistory
            // 
            this.MessageHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MessageHistory.Location = new System.Drawing.Point(12, 41);
            this.MessageHistory.Name = "MessageHistory";
            this.MessageHistory.ReadOnly = true;
            this.MessageHistory.Size = new System.Drawing.Size(605, 338);
            this.MessageHistory.TabIndex = 0;
            this.MessageHistory.Text = "";
            // 
            // MessageInput
            // 
            this.MessageInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MessageInput.Enabled = false;
            this.MessageInput.Location = new System.Drawing.Point(12, 398);
            this.MessageInput.Name = "MessageInput";
            this.MessageInput.Size = new System.Drawing.Size(559, 40);
            this.MessageInput.TabIndex = 1;
            this.MessageInput.Text = "";
            this.MessageInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MessageInput_KeyDown);
            // 
            // SendButton
            // 
            this.SendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SendButton.Enabled = false;
            this.SendButton.Location = new System.Drawing.Point(577, 398);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(40, 40);
            this.SendButton.TabIndex = 2;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // UserList
            // 
            this.UserList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UserList.FormattingEnabled = true;
            this.UserList.IntegralHeight = false;
            this.UserList.Location = new System.Drawing.Point(623, 12);
            this.UserList.Name = "UserList";
            this.UserList.Size = new System.Drawing.Size(165, 210);
            this.UserList.TabIndex = 3;
            // 
            // ChannelList
            // 
            this.ChannelList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelList.FormattingEnabled = true;
            this.ChannelList.IntegralHeight = false;
            this.ChannelList.Location = new System.Drawing.Point(623, 228);
            this.ChannelList.Name = "ChannelList";
            this.ChannelList.Size = new System.Drawing.Size(165, 210);
            this.ChannelList.TabIndex = 4;
            // 
            // StatusLabel
            // 
            this.StatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(12, 382);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(38, 13);
            this.StatusLabel.TabIndex = 5;
            this.StatusLabel.Text = "Status";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Disconnect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ChatWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.ChannelList);
            this.Controls.Add(this.UserList);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.MessageInput);
            this.Controls.Add(this.MessageHistory);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "ChatWindow";
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.ChatWindow_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox MessageHistory;
        private System.Windows.Forms.RichTextBox MessageInput;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.ListBox UserList;
        private System.Windows.Forms.ListBox ChannelList;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Button button1;
    }
}

