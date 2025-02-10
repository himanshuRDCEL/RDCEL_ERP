using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTC_Model.PluralGateway
{
 
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CreateOrderResponse
    {
        public string? token { get; set; }
        public string? plural_order_id { get; set; }
    }
    
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CreateOrderErrorResponse
    {
        public string? error_code { get; set; }
        public string? error_message { get; set; }
    }
}
