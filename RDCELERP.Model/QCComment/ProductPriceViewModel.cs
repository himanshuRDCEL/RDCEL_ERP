using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.QCComment
{
    public class ProductPriceViewModel : BaseViewModel
    {
        public int? QualityId { get; set; }
        public decimal? BasePrice { get; set; }
        public decimal? Sweetener { get; set; }
        public decimal? FinalQcPrice { get; set; }
        public decimal? CollectedAmount { get; set; }
        public decimal? Actualpriceafterqc { get; set; }
        public decimal? QCDeclareprice { get; set; }
        public decimal? Cust_declaredPrice { get; set; }
        public decimal? AmountCollectFromCustomer { get; set; }
    }
}
