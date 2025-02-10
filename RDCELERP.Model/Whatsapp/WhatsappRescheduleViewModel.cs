using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Whatsapp
{
    public class WhatsappRescheduleViewModel
    {
        public class RescheduleQCDate
        {
            public string? type { get; set; }
            public string? sender { get; set; }
            public string? templateId { get; set; }
            public SendDate? @params { get; set; }
        }

        public class WhatsappTemplate
        {
            public UserDetails? userDetails { get; set; }
            public RescheduleQCDate? notification { get; set; }
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

        public class SendDate
        {
            [JsonProperty("1")]
            public string? RescheduleDate { get; set; }

        }
    }
}
