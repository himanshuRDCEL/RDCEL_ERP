using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.CashfreeModel
{
 


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    //Object to add banefiaciary for making payments
    public class AddBeneficiary
    {
        public string? beneId { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? vpa { get; set; }//upi id
        public string? address1 { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? pincode { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AddBeneficiaryResponse
    {
        public string? status { get; set; }
        public string? subCode { get; set; }
        public string? message { get; set; }
    }
}
