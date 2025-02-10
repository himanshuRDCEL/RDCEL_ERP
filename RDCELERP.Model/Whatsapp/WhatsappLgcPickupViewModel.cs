using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Whatsapp
{
    public class WhatsappLgcPickupViewModel
    {
        public class LogiPickup
        {
            public string? type { get; set; }
            public string? sender { get; set; }
            public string? templateId { get; set; }
            public SendOTP? @params { get; set; }
        }

        public class WhatsappTemplate
        {
            public UserDetails? userDetails { get; set; }
            public LogiPickup? notification { get; set; }
        }

        public class UserDetails
        {
            public string? number { get; set; }
        }

        public class WhatasappResponse
        {
            public string? msgId { get; set; }
            public string? message { get; set; }
        }

        public class SendOTP
        {
            [JsonProperty("1")]
            public string? OTP { get; set; }

        }
    }
}
