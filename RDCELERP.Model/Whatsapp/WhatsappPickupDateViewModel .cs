using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Whatsapp
{
   public class WhatsappPickupDateViewModel
    {

        public class WhatsappPickupDateVM
        {
            public string? type { get; set; }
            public string? sender { get; set; }
            public string? templateId { get; set; }
            public SendPickUpDate? @params { get; set; }
        }

        public class WhatsappPickUpDateTemplate
        {
            public PickUpDateUserDetails? userDetails { get; set; }
            public WhatsappPickupDateVM? notification { get; set; }
        }

        public class PickUpDateUserDetails
        {
            public string? number { get; set; }
        }

        public class WhatsAppResponse
        {
            public string? msgId { get; set; }
            public string? message { get; set; }
        }

        public class SendPickUpDate
        {
            [JsonProperty("1")]
            public string? Customername { get; set; }           

            [JsonProperty("2")]
            public string? PageLink { get; set; }

        }
    }
}
