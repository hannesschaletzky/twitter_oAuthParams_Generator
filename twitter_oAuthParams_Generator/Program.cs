using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace twitter_oAuthParams_Generator {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new OAuthGenerator("y5tnv0KUnZnaR81Jj2MKaBRH8", 
                                               "DEBCiqAzh5DuvJ1Q7CVRKIDAK3Q9Nf3F5OklTuodZaPw5kHuvH", 
                                               "1349709202332246017-hrYqUMTHlMetFvKTZQCGgXcwZpEg78", 
                                               "GAdWPmtCwhsNqdw5brtMpjXnqbdkjR0JVrqN3GfF7dGkg"));
        }
    }
}
