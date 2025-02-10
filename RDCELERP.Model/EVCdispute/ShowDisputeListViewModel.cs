using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.EVCdispute
{
   public  class ShowDisputeListViewModel: BaseViewModel
   {
        public int EvcdisputeId { get; set; }
        public int orderTransId { get; set; }
        public int? EvcregistrationId { get; set; }
        public int? ExchangeOrderId { get; set; }
        public string? DisputeRegno { get; set; }  
        public string? EvcRegNo { get; set; }
        public string? RegdNo { get; set; }
        public int ProductTypeId { get; set; }
        public string? ProductTypeName { get; set; }//TblProductType
        public string? ProductCatName { get; set; }
        public int ProductCatId { get; set; }//TblProductCat
        public string? Status { get; set; }
        public decimal Amount { get; set; }
        public string? Evcdisputedescription { get; set; }
        public string? Date { get; set; }
        // public string? Digi2Lresponse { get; set; }
        public int? NewProductCategoryId { get; set; }

        public int NewProductCategoryTypeId { get; set; }
        // public string? Digi2Lresponse { get; set; }      
    }
}
