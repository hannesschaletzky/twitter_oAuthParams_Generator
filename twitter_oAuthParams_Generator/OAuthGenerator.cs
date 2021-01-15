using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace twitter_oAuthParams_Generator {
    public partial class OAuthGenerator : Form {

        const string TwitterApiBaseUrl = "https://api.twitter.com/1.1/";
        readonly string consumerKey, consumerKeySecret, accessToken, accessTokenSecret;
        readonly HMACSHA1 sigHasher;
        
        public OAuthGenerator(string consumerKey, string consumerKeySecret, string accessToken, string accessTokenSecret) {
            InitializeComponent();

            this.consumerKey = consumerKey;
            this.consumerKeySecret = consumerKey;
            this.accessToken = accessToken;
            this.accessTokenSecret = accessTokenSecret;
            sigHasher = new HMACSHA1(new ASCIIEncoding().GetBytes(string.Format("{0}&{1}", consumerKeySecret, accessTokenSecret)));

            startAsync();
        }

        private void startAsync() {

            Console.WriteLine("Program started");

            string url = "https://api.twitter.com/1.1/users/search.json";

            //oauth_timestamp
            var oauth_timestamp = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            Console.WriteLine($"oauth_timestamp: {oauth_timestamp}");

            //oauth_nonce
            string oauth_nonce = randomString(32);
            Console.WriteLine($"oauth_nonce: {oauth_nonce}");
            
            var data = new Dictionary<string, string> {};
            //Add all the OAuth headers we'll need to use when constructing the hash.
            data.Add("oauth_consumer_key", consumerKey);
            data.Add("oauth_signature_method", "HMAC-SHA1");
            data.Add("oauth_timestamp", oauth_timestamp.ToString());
            data.Add("oauth_nonce", oauth_nonce); // Required, but Twitter doesn't appear to use it, so "a" will do.
            data.Add("oauth_token", accessToken);
            data.Add("oauth_version", "1.0");
            //data.Add("q", "soccer");

            //Generate the OAuth signature and add it to our payload.
            //https://developer.twitter.com/en/docs/authentication/oauth-1-0a/creating-a-signature
            //https://medium.com/@jeremylenz/making-an-oauth-1-0-signature-for-twitter-4040dc30776
            string oauth_signature = GenerateSignature(url, data);
            data.Add("oauth_signature", oauth_signature);
            Console.WriteLine($"oauth_signature: {oauth_signature}");

            
            //Build the OAuth HTTP Header from the data.
            url = url + "?q=soccer";
            string oAuthHeader = GenerateOAuthHeader(data);
            Console.WriteLine($"oAuthHeader: {oAuthHeader}");

        }


        //https://blog.dantup.com/2016/07/simplest-csharp-code-to-post-a-tweet-using-oauth/
        /// <summary>
        /// Generate an OAuth signature from OAuth header values.
        /// </summary>
        string GenerateSignature(string url, Dictionary<string, string> data) {
            var sigString = string.Join(
                "&",
                data
                    .Union(data)
                    .Select(kvp => string.Format("{0}={1}", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value)))
                    .OrderBy(s => s)
            );

            var fullSigData = string.Format(
                "{0}&{1}&{2}",
                "GET",
                Uri.EscapeDataString(url),
                Uri.EscapeDataString(sigString.ToString())
            );

            return Convert.ToBase64String(sigHasher.ComputeHash(new ASCIIEncoding().GetBytes(fullSigData.ToString())));
        }


        /// <summary>
        /// Generate the raw OAuth HTML header from the values (including signature).
        /// </summary>
        string GenerateOAuthHeader(Dictionary<string, string> data) {
            return "OAuth " + string.Join(
                ", ",
                data
                    .Where(kvp => kvp.Key.StartsWith("oauth_"))
                    .Select(kvp => string.Format("{0}=\"{1}\"", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value)))
                    .OrderBy(s => s)
            );
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
