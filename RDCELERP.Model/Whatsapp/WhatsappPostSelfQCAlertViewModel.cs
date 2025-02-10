using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Whatsapp
{
    public class WhatsappPostSelfQCAlertViewModel
    {
        public class PostSelfQC
        {
            public string? type { get; set; }
            public string? sender { get; set; }
            public string? templateId { get; set; }
            //public URL? @params { get; set; }
        }

        public class WhatsAppTemplate
        {
            public SelfQCUserDetails? userDetails { get; set; }
            public PostSelfQC? notification { get; set; }
        }

        public class SelfQCUserDetails
        {
            public string? number { get; set; }
        }

        public class WhatsAppResponse
        {
            public string? msgId { get; set; }
            public string? message { get; set; }
        }

        //public class URL
        //{
        //    [JsonProperty("1")]
        //    public string? CustomerName { get; set; }

        //    [JsonProperty("2")]
        //    public string? Link { get; set; }

        //}
    }
}
