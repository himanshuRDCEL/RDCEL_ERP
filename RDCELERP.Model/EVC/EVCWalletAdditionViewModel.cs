using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.EVC
{
    public class EVCWalletAdditionViewModel: BaseViewModel
    {

        public int EvcregistrationId { get; set; }
        public string? EVCemail { get; set; }
        public string? EVCcontactNumber { get; set; }
        public string? EVCaddress { get; set; }
        public string? EVCRegdNo { get; set; }
        public decimal? EvcWallet { get; set; }
        public string? BussinessName { get; set; }
        public int? Rechargeby { get; set; }

        //wallet recharge
        public string? TransactionId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? InvoiceImage { get; set; }
        public decimal? Amount { get; set; }

        //credit recharge
        public string? CreditTransactionId { get; set; }
        public DateTime? CreditTransactionDate { get; set; }
        public string? CreditComments { get; set; }
        public decimal? CreditAmount { get; set; }
        public bool? IsCreaditNote { get; set; }
        

    }
}
