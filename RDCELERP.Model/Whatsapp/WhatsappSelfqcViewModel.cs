using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Whatsapp
{
    public class WhatsappSelfqcViewModel
    {
        public class SelfQC
        {
            public string? type { get; set; }
            public string? sender { get; set; }
            public string? templateId { get; set; }
            public URL? @params { get; set; }
        }

        public class WhatsappTemplate
        {
            public UserDetails? userDetails { get; set; }
            public SelfQC? notification { get; set; }
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

        public class URL
        {
            [JsonProperty("1")]
            public string? CustomerName { get; set; }

            [JsonProperty("2")]
            public string? Link { get; set; }

        }

        //Notification For VoucherCode
        public class NotificationInstant
        {
            public string type { get; set; }
            public string sender { get; set; }
            public string templateId { get; set; }
            public SendVoucherOnWhatssapp @params { get; set; }
        }
        //for voucher Code
        public class WhatsappTemplateInstant
        {
            public UserDetails userDetails { get; set; }
            public NotificationInstant notification { get; set; }
        }

        //Paremeters to send voucher code on whatssapp yellow.ai
        public class SendVoucherOnWhatssapp
        {
            [JsonProperty("1")]
            public string voucherAmount { get; set; }
            [JsonProperty("2")]
            public string BrandName { get; set; }
            [JsonProperty("3")]
            public string voucherCode { get; set; }
            [JsonProperty("4")]
            public string BrandName2 { get; set; }
            [JsonProperty("5")]
            public string VoucherExpiry { get; set; }
            [JsonProperty("6")]
            public string VoucherLink { get; set; }

        }
    }
}
