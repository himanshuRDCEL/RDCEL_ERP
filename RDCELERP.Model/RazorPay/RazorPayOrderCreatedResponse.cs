using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.RazorPay
{

    public class RazorpayOrderModel
    {
        public string OrderId { get; set; }
        public string Key { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Description { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Notes
    {
        public string notes_key_1 { get; set; }
    }

    public class RazorPayOrderCreatedResponse
    {
        public int amount { get; set; }
        public int amount_due { get; set; }
        public int amount_paid { get; set; }
        public int attempts { get; set; }
        public int created_at { get; set; }
        public string currency { get; set; }
        public string entity { get; set; }
        public string id { get; set; }
        //public Notes notes { get; set; }
        public object offer_id { get; set; }
        public string receipt { get; set; }
        public string status { get; set; }
    }


}
