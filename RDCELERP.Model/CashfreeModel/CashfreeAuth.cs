using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.CashfreeModel
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data
    {
        public string? token { get; set; }
        public int expiry { get; set; }
    }

    public class CashfreeAuth
    {
        public string? status { get; set; }
        public string? subCode { get; set; }
        public string? message { get; set; }
        public Data? data { get; set; }
    }

    public class PayOutDetails
    {
        public string? Message { get; set; }
        public string? PageRedirectionURL { get; set; }
    }
}
