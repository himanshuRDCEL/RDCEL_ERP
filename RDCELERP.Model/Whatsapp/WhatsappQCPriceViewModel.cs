using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Whatsapp
{
   public class WhatsappQCPriceViewModel
    {

        public class QCFinalPrice
        {
            public string? type { get; set; }
            public string? sender { get; set; }
            public string? templateId { get; set; }
            public SendDate? @params { get; set; }
        }

        public class WhatsappTemplate
        {
            public UserDetails? userDetails { get; set; }
            public QCFinalPrice? notification { get; set; }
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
            public string? Customername { get; set; }

            [JsonProperty("2")]
            public decimal? FinalQcPrice { get; set; }

            [JsonProperty("3")]
            public string? PageLink { get; set; }

        }
    }
}
