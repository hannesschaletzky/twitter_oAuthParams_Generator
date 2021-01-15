using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace twitter_oAuthParams_Generator {
    public partial class Form1 : Form {
        
        public Form1() {
            InitializeComponent();
            start();
        }

        private void start() {

            Console.WriteLine("Program started");

            //oauth_timestamp
            var oauth_timestamp = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            Console.WriteLine($"oauth_timestamp: {oauth_timestamp}");

            //oauth_nonce
            string oauth_nonce = randomString(32);
            Console.WriteLine($"oauth_nonce: {oauth_nonce}");

            //signature


        }

















        /// <summary>
        /// retrieve random alphanumeric string for oauth_nonce
        /// </summary>
        private static Random random = new Random();
        public static string randomString(int length) {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
