using System;
using System.Drawing;
using System.Windows.Forms;

namespace MicroSquid {
    public partial class BanNotice : Form {
        public BanNotice(bool hasExpiry, bool isPermanent, DateTimeOffset expires) {
            InitializeComponent();
            ExpiryIndication.Text = !hasExpiry
                ? @"This ban expires immediately."
                : (
                    isPermanent
                    ? @"This ban will never expire."
                    : $@"This ban will expire on {expires}."
                );
            if(isPermanent)
                ExpiryIndication.Font = new Font(ExpiryIndication.Font, FontStyle.Bold);
        }

        private void button1_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
