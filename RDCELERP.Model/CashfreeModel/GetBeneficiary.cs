 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.CashfreeModel
{
   
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class BaneficiaryDetails
    {
        public string? beneId { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? address1 { get; set; }
        public string? address2 { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? pincode { get; set; }
        public string? bankAccount { get; set; }
        public string? ifsc { get; set; }
        public string? status { get; set; }
        public string? vpa { get; set; }
        public string? addedOn { get; set; }
        public string? FinalExchangePrice { get; set; }
        public int ExchangeId { get; set; }
        public int ordertype { get; set; }
    }

    public class GetBeneficiary
    {
        public string? status { get; set; }
        public string? subCode { get; set; }
        public string? message { get; set; }
        public BaneficiaryDetails? data { get; set; }
    }

    public class TransferAmount
    {
        public string beneId { get; set; }
        public string amount { get; set; }
        public string transferId { get; set; }
        public string transferMode { get; set; }

    }
    
}
