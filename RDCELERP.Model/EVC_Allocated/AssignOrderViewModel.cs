using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.EVC_Allocated
{
    public class AssignOrderViewModel:BaseViewModel
    {      
       public int ?OrderTransId { get; set; }
        public string? RegdNo { get; set; }
        public string? BussinessName { get; set; }
        public string? EvcRegNo { get; set; }
        public string? FirstName { get; set; }//custname
        public string? LastName { get; set; }
        public decimal? EvcRate { get; set; }     
        public string? CustCity { get; set; }
        public string? ExchProdGroup { get; set; }
        public string? OldProdType { get; set; }       
        public string? CustPin { get; set; }
        public string? Date { get; set; }
        public string? ReOpenDate { get; set; }     
        public decimal? FinalExchangePrice { get; set; }
        public string? IsPrimeProductId { get; set; }
        public decimal? EvcWalletAmount { get; set; }
        public decimal clearBalance { get; set; }
        public string? StatusCode { get; set; }
       public string? ModifyDate { get; set; }
        public string? RescheduleDate { get; set; }
        public string? RescheduleTime { get; set; }
        public string? ordertype { get; set; }
        public string? pickupScheduleDate { get; set; }
        public string? pickupScheduletime { get; set; }
        public string? EvcStoreCode { get; set; }
        public int WalletTransactionId { get; set; }
     
    }
}
