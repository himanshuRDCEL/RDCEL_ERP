using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.EVC_Allocation
{
    public class NotAllocatedOrderViewModel : BaseViewModel
    {     
        public int? ExchangeId { get; set; }
        public int abbRedemtionId { get; set; }
        public int OrderTransId { get; set; }      
        public string? RegdNo { get; set; }
        public string? FirstName { get; set; }//custname
        public string? EVCRate { get; set; }
        public string? ActualProdQltyAtQc{get; set;}     
        public string? CustCity { get; set; }
        public string? Prodcat { get; set; }
        public string? OldProdType { get; set; }
        public string? Bonus { get; set; }
        public string? CustPin { get; set; }     
        public string? SponsorOrderNumber { get; set; }
        public decimal? FinalExchangePrice { get; set; }
        public string? Date { get; set; }
        public string? status { get; set; }
        public string? StateName { get; set; }
        public string? SponsorName { get; set; }
        public string? ordertype { get; set; }
    }
}
