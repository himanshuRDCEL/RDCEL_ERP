using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.VoucherRedemption
{
    public class WhatsappDataContract
    {
        public class UserDetails
        {
            public string number { get; set; }
            public string name { get; set; }
        }
        //Paremeters to send voucher code on whatssapp yellow.ai
        public class SendCashVoucherOnWhatssapp
        {
            [JsonProperty("1")]
            public string voucherAmount { get; set; }
            [JsonProperty("2")]
            public string BrandName { get; set; }
            [JsonProperty("3")]
            public string voucherCode { get; set; }
            [JsonProperty("4")]
            public string VoucherExpiry { get; set; }
            [JsonProperty("5")]
            public string VoucherLink { get; set; }

        }
        public class NotificationForCash
        {
            public string type { get; set; }
            public string sender { get; set; }
            public string templateId { get; set; }
            public SendCashVoucherOnWhatssapp @params { get; set; }
        }
        //For cash VoucherCode
        public class WhatsappTemplatecashvoucher
        {
            public UserDetails userDetails { get; set; }
            public NotificationForCash notification { get; set; }
        }
    }
}
