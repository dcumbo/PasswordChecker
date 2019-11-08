using System.Collections.Generic;
using CommandLine;

namespace PasswordChecker
{
    public class Options
    {
        public Options()
        {
            
        }
        
        [Option('p', "password", Required = true, 
            HelpText = "The password you would like to check for breaches.")]
        public IEnumerable<string> Password { get; set; }
    }
}