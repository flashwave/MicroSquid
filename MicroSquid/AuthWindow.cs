using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MicroSquid {
    public partial class AuthWindow : Form {
        public const string CONFIG = @"config.txt";

        public static readonly Regex UmiCookieRegex = new Regex(@"^Umi\.Cookies\.Set\('ybabstat', '([A-Za-z0-9_\-]+)'\);$");

        public string ServerUrl { get; private set; }
        public string AuthToken { get; private set; }

        public AuthWindow() {
            InitializeComponent();

            if(File.Exists(CONFIG)) {
                string[] config = File.ReadAllLines(CONFIG);
                ServerURL.Text = config.ElementAtOrDefault(0) ?? string.Empty;
                LoginToken.Text = config.ElementAtOrDefault(1)?.Replace('\t', '_') ?? string.Empty;
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e) {
            Process.Start(new ProcessStartInfo {
                FileName = @"https://misuzu.misaka.nl/_sockchat/login",
                UseShellExecute = true,
            });
        }

        private void button1_Click(object sender, EventArgs e) {
            Match regexMatch = UmiCookieRegex.Match(LoginToken.Text);
            if(regexMatch.Success)
                LoginToken.Text = regexMatch.Groups[1].Value;

            ServerUrl = ServerURL.Text;
            AuthToken = LoginToken.Text.Replace('_', '\t');

            if(File.Exists(CONFIG))
                File.Delete(CONFIG);
            File.WriteAllLines(CONFIG, new[] { ServerUrl, AuthToken });

            Close();
        }
    }
}
