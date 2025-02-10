using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Whatsapp
{
    public class WhatsappCashVoucherViewModel
    {
        //Notification For VoucherCode
        public class Notification
        {
            public string? type { get; set; }
            public string? sender { get; set; }
            public string? templateId { get; set; }
            public SendVoucherOnWhatssapp? @params { get; set; }
        }
        //for voucher Code
        public class WhatsappTemplate
        {
            public UserDetails? userDetails { get; set; }
            public Notification? notification { get; set; }
        }
        public class WhatasappResponse
        {
            public string? msgId { get; set; }
            public string? message { get; set; }
        }
        public class UserDetails
        {
            public string? number { get; set; }
        }
        //Paremeters to send voucher code on whatssapp yellow.ai
        public class SendVoucherOnWhatssapp
        {
            [JsonProperty("1")]
            public decimal voucherAmount { get; set; }
            [JsonProperty("2")]
            public string? BrandName { get; set; }
            [JsonProperty("3")]
            public string? voucherCode { get; set; }
            [JsonProperty("4")]
            public string? VoucherExpiry { get; set; }
            [JsonProperty("5")]
            public string? VoucherLink { get; set; }
            [JsonProperty("6")]
            public string? BrandName2 { get; set; }

        }
    }
}
