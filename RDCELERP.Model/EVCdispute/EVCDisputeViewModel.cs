using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.EVCdispute
{
    public class EVCDisputeViewModel : BaseViewModel
    {
        public int EvcdisputeId { get; set; }
        public int EvcregistrationId { get; set; }
        public string? EVCRegdNo { get; set; }
       // public int ExchangeOrderId { get; set; }
        public int orderTransId { get; set; }
        public string? OrderRegdNo { get; set; }
        public int ProductTypeId { get; set; }
        public string? ProductTypeName { get; set; }//TblProductType
        public string? ProductCatName { get; set; }
        public int ProductCatId { get; set; }//TblProductCat
        public string? Status { get; set; }
        public string? StatusName { get; set; }
        public string? LevelStatus { get; set; }
        public decimal Amount { get; set; }
        public string? Evcdisputedescription { get; set; }
        public string? Digi2Lresponse { get; set; }
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public string? Image4 { get; set; }
        public string? DisputeRegno { get; set; }
        public string? Evcregistration { get; set; }
    }
    public class EVCWalletTransaction : BaseViewModel
    {
        public int ExchangeOrderId { get; set; }
        public string? RegdNo { get; set; }
        public int ProductTypeId { get; set; }
        public int ProductCatId { get; set; }
       public int? orderTransId { get; set; }

    }
}
