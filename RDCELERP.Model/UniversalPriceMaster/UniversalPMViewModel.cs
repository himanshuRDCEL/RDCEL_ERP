using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.UniversalPriceMaster
{
    public class UniversalPMViewModel : BaseViewModel
    {
        public decimal? BaseValue { get; set; }
        public decimal? FinalQCPrice { get; set; }
        public decimal? TotalSweetener { get; set; }
        public decimal? CollectedAmount { get; set; }
        public decimal? SweetenerBU { get; set; }
        public decimal? SweetenerBP { get; set; }
        public decimal? SweetenerDigi2l { get; set; }
        public decimal? Sweetener { get; set; }
        public decimal? ExchangePrice { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class QCProductPriceDetails
    {
        public int? ExchangeId { get; set; }
        public int? BrandId { get; set; }
        public int? NewBrandId { get; set; }
        public int? ProductCatId { get; set; }
        public int? ProductTypeId { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? BusinessPartnerId { get; set; }
        public int? conditionId { get; set; }
        public int? PriceNameId { get; set; }
        public string? RegdNo { get; set; }
        public int? NewModelId { get; set; }
        public string? CustQuality { get; set; }
        public int? FinalProdQualityId { get; set; }

    }

    public class ProductConditionList
    {
        public string? P_Price { get; set; }
        public string? Q_Price { get; set; }
        public string? R_Price { get; set; }
        public string? S_Price { get; set; }
    }
    public class QCProductPrice
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
    public class PriceCalculate
    {
        public decimal? ExchangePrice { get; set; }
        public decimal? QCPrice { get; set; }
        public decimal? EVCPrice { get; set; }
        public decimal? LGCPrice { get; set; }        
    }
}
