using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.UPIVerificationModel
{
  
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Cashfreeupidata
    {
        public string? nameAtBank { get; set; }
        public string? accountExists { get; set; }
    }

    public class UPIVerification
    {
        public string? status { get; set; }
        public string? subCode { get; set; }
        public string? message { get; set; }
        public Cashfreeupidata? data { get; set; }
    }

}
