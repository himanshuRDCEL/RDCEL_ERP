using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.EVC
{
    public class EVCPriceViewModel
    {
        public int OrderTransId { get; set; }
        public int OrderPrice { get; set; }
        public int SweetenerAmt { get; set; }
        public int LGCCost { get; set; }
        public decimal AmountWithLGCCost { get; set; }
        public decimal UTCPer { get; set; }
        public decimal UTCAmount { get; set; }
        public decimal GstAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public int EVCAmount { get; set; }
        public bool? IsSweetenerAmtInclude { get; set; }
        public int? GstTypeId { get; set; }
        public int? ProdCatId { get; set; }
        public int? ProdTypeId { get; set; }
        public int? Buid { get; set; }
        public decimal BaseValue { get; set; }
    }
}
