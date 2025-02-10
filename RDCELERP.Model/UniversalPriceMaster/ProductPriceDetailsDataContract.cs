using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.UniversalPriceMaster
{
    public class ProductPriceDetailsDataContract
    {
        public int? BrandId { get; set; }
        public int? NewBrandId { get; set; }
        public int? ProductCatId { get; set; }
        public int? ProductTypeId { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? BusinessPartnerId { get; set; }
        public int? conditionId { get; set; }
        public int? PriceNameId { get; set; }

    }
    public class UniversalPriceMasterDataContract
    {
        public decimal? BaseValue { get; set; }
        public string ErrorMessage { get; set; }
        public int? PricemasternameId { get; set; }
    }


}
