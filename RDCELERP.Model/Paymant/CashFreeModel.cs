using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RDCELERP.Model.Paymant
{
    class CashFreeModel
    {
    }

    public class CashFreeStatusViewModel 
    { 
        public int? ReferenceId {  get; set; }
        public string? Status { get; set; }
        public string? Amount {  get; set; }
        public string? BeneficiaryId { get; set; }
        public string Email {  get; set; }
        public string? UTRNumber { get; set; }
        public string FundSource {  get; set; }
        public string? transferId { get; set; }  
        public string? TransferMethod { get; set; }
        public string? InitiatedAt { get; set; }    
        public int? TransferAcknowledge { get; set; }
        public string? ProcessedOn { get; set; }
        public string? StatusDescription { get; set;}
        public string? phone { get; set; }  
        public string? Vpa { get; set; }
    }
}
