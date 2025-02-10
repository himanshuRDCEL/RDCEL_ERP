using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.ExchangeBulkLiquidation
{
    public class OrderConfirmationTemplateExchangeUpdated
    {
        public UserDetails? userDetails { get; set; }
        public OrderConfiirmationNotificationUpdated? notification { get; set; }
    }

    public class UserDetails
    {
        public string? number { get; set; }
    }

    public class OrderConfiirmationNotificationUpdated
    {
        public string? type { get; set; }
        public string? sender { get; set; }
        public string? templateId { get; set; }
        public SendWhatssappForExcahangeConfirmationUpdated? @params { get; set; }
    }

    public class SendWhatssappForExcahangeConfirmationUpdated
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

    }
    public class WhatasappResponseBulk
    {
        public string? msgId { get; set; }
        public string? message { get; set; }
    }

    public partial class tblWhatsAppMessage
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string TemplateName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> SendDate { get; set; }
        public string msgId { get; set; }
        public string code { get; set; }
    }
}
