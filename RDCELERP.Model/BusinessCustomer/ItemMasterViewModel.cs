using RDCELERP.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.BusinessCustomer
{
    public class ItemMasterViewModel :BaseViewModel
    {
        public int ItemMasterId { get; set; }
        public int? Sno { get; set; }
        public string? LocationCode { get; set; }
        public string? Itemcode { get; set; }
        public string? Ean { get; set; }
        public string? ItemDesc { get; set; }
        public int? Mrp { get; set; }
        public int? Qty { get; set; }
        public int? PendingDispatch { get; set; }
        public int? AvailableStock { get; set; }
        public string? Brand { get; set; }
        public string? SubCat { get; set; }
        public string? Size { get; set; }
        public string? Colour { get; set; }
        public string? Condition { get; set; }
        public string? Gender { get; set; }
        public string? Division { get; set; }
        public string? Section { get; set; }
        public string? Department { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? Rsp { get; set; }
        public string? ManageBatchItem { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Uom { get; set; }
        public string? Hsn { get; set; }
        public string? Hsnid { get; set; }
        public string? ItemImage1 { get; set; }
        public string? ItemImage2 { get; set; }
        public string? ItemImage3 { get; set; }
        public string? ItemImage4 { get; set; }
        public string? ItemImage5 { get; set; }
        public decimal? B2bPrice { get; set; }
        public decimal? Qnty { get; set; }

        public List<ItemViewModel> ItemViewModel { get; set; }
    }
}
