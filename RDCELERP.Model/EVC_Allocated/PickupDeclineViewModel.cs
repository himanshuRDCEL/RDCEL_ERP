using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.EVC_Allocated
{
   public class PickupDeclineViewModel : BaseViewModel
    {
        public int? OrderTransId { get; set; }
        public string? RegdNo { get; set; }
        public string? EvcRegNo { get; set; }
        public string? FirstName { get; set; }//custname
        public decimal? FinalExchangePrice { get; set; }
        public decimal? EvcRate { get; set; }
        public string? CustCity { get; set; }
        public string? ExchProdGroup { get; set; }
        public string? OldProdType { get; set; }
        public string? CustPin { get; set; }
        public string? Date { get; set; }
       public string? ordertype { get; set; }
       public string? EvcStoreCode { get; set; }
    }
}
