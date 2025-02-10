using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.EVC
{
    public class PendingCreditApprovalViewModel:BaseViewModel
    {
        public int WalletTransactionId { get; set; }
        public int CreditRequestId { get; set; }
        public int? OrderTransId { get; set; }
        public string? RegdNo { get; set; }
        public string? BussinessName { get; set; }
        public string? EvcRegNo { get; set; }
        public string? FirstName { get; set; }//custname
        public decimal? FinalExchangePrice { get; set; }
        public decimal? EvcRate { get; set; }
        public string? CustCity { get; set; }
        public string? ExchProdGroup { get; set; }
        public string? OldProdType { get; set; }       
        public string? RequestDate { get; set; }
        public string? GenrateRequestBy { get; set; }
        public decimal? EvcWalletAmount { get; set; }
        public decimal clearBalance { get; set; }
        public string? StatusCode { get; set; }       
    }
}
