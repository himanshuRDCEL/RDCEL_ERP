using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTC.WebAPI.Base
{
    public class ApplicationSettings
    {
        public string BaseURL { get; set; }
        public string Digi2l_DevContext { get; set; }
        public string ExcelConString { get; set; }
        public string SecurityKey { get; set; }
        
        public string FromEmail { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public string PortNumber { get; set; }
        public string IsSSL { get; set; }
        public string UseDefaultCredentials { get; set; }
        public string BccEmailAddress { get; set; }
        public string FromDisplayName { get; set; }
        public string MailjetAPIKey { get; set; }
        public string MailjetAPISecret { get; set; }


    }
}
