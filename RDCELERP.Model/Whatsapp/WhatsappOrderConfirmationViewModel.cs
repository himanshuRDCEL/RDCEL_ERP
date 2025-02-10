using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Whatsapp
{
    public class WhatsappOrderConfirmationViewModel
    {

        public class SelfQCOrderConfirmation
        {
            public string? type { get; set; }
            public string? sender { get; set; }
            public string? templateId { get; set; }
            public URLOrderConfirmation? @params { get; set; }
        }
        public class OrderPlaceWithSelfQCConfirmation
        {
            public string? type { get; set; }
            public string? sender { get; set; }
            public string? templateId { get; set; }
            public OrderPlaceConfirmation? @params { get; set; }
        }

        public class WhatsappTemplateOrderConfirmation
        {
            public UserDetailsOrderConfirmation? userDetails { get; set; }
            public SelfQCOrderConfirmation? notification { get; set; }
        }
        public class WhatsappTemplateExchangeOrderPlaceConfirmation
        {
            public UserDetailsOrderConfirmation? userDetails { get; set; }
            public OrderPlaceWithSelfQCConfirmation? notification { get; set; }
        }

        public class UserDetailsOrderConfirmation
        {
            public string? number { get; set; }
        }

        public class WhatasappResponseOrderConfirmation
        {
            public string? msgId { get; set; }
            public string? message { get; set; }
        }

        public class URLOrderConfirmation
        {
            [JsonProperty("1")]
            public string? CustName { get; set; }
            [JsonProperty("2")]
            public string? RegdNO { get; set; }
            [JsonProperty("3")]
            public string? ProdCategory { get; set; }
            [JsonProperty("4")]
            public string? ProdType { get; set; }
            [JsonProperty("5")]
            public string? CustomerName { get; set; }
            [JsonProperty("6")]
            public string? PhoneNumber { get; set; }
            [JsonProperty("7")]
            public string? Email { get; set; }
            [JsonProperty("8")]
            public string? Address { get; set; }
            [JsonProperty("9")]
            public string? Link { get; set; }

            public string? ProductBrand { get; set; }


        }

        public class OrderPlaceConfirmation
        {
            [JsonProperty("1")]
            public string? CustName { get; set; }
            [JsonProperty("4")]
            public string? RegdNO { get; set; }
            [JsonProperty("5")]
            public string? ProdCategory { get; set; }
            [JsonProperty("6")]
            public string? ProdType { get; set; }
            public string? CustomerName { get; set; }
            public string? PhoneNumber { get; set; }
            [JsonProperty("7")]
            public string? Email { get; set; }
            [JsonProperty("8")]
            public string? Address { get; set; }
            [JsonProperty("2")]
            public string? Link { get; set; }
            [JsonProperty("3")]
            public string? ProductBrand { get; set; }


        }
    }
}
