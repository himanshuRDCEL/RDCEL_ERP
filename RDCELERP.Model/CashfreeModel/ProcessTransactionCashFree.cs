using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.CashfreeModel
{
 

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ProcessTransactionCashfree
    {
        public string? beneId { get; set; }
        public string? amount { get; set; }
        public string? transferId { get; set; }
        public string? transferMode { get; set; }
       
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class TransactionDataCashfree
    {
        public string? referenceId { get; set; }
        public string? utr { get; set; }
        public int acknowledged { get; set; }
    }

    public class TransactionResponseCashfree
    {
        public string? status { get; set; }
        public string? subCode { get; set; }
        public string? message { get; set; }
        public TransactionDataCashfree? data { get; set; }
    }

    //Use for Api
    public class SavePaymentResponce
    {
        public string? Regdno { get; set; }
        public string? JsonResponce { get; set; }
        //  public GetBeneficiary? getBeneficiary { get; set; }
    }
}
