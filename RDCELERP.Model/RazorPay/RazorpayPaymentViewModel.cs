using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.RazorPay
{
    public class RazorpayPaymentViewModel
    {
        public string PaymentId { get; set; }
        public string OrderId { get; set; }
        public string Signature { get; set; }
        public decimal Amount { get; set; }
        public int CustomerId { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AcquirerData
    {
        public string auth_code { get; set; }
    }

    public class Card
    {
        public string id { get; set; }
        public string entity { get; set; }
        public string name { get; set; }
        public string last4 { get; set; }
        public string network { get; set; }
        public string type { get; set; }
        public object issuer { get; set; }
        public bool international { get; set; }
        public bool emi { get; set; }
        public string sub_type { get; set; }
        public object token_iin { get; set; }
    }

    public class ResponseRazorpayPaymentNotes
    {
        public string notes_key_1 { get; set; }
    }

    public class ResponseRazorpayPaymentViewModel
    {
        public string id { get; set; }
        public string entity { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string status { get; set; }
        public string order_id { get; set; }
        public object invoice_id { get; set; }
        public bool international { get; set; }
        public string method { get; set; }
        public int amount_refunded { get; set; }
        public object refund_status { get; set; }
        public bool captured { get; set; }
        public string description { get; set; }
        public string card_id { get; set; }
      //  public Card card { get; set; }
        public object bank { get; set; }
        public object wallet { get; set; }
        public object vpa { get; set; }
        public string email { get; set; }
        public string contact { get; set; }
      //  public ResponseRazorpayPaymentNotes notes { get; set; }
        public int fee { get; set; }
        public int tax { get; set; }
        public object error_code { get; set; }
        public object error_description { get; set; }
        public object error_source { get; set; }
        public object error_step { get; set; }
        public object error_reason { get; set; }
      //  public AcquirerData acquirer_data { get; set; }
        public int created_at { get; set; }
    }


}
