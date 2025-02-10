using System.ComponentModel.DataAnnotations;
using RDCELERP.DAL.Entities;

namespace RDCELERP.Model.QCComment
{
    public class QuestionerViewModel
    {
        public int ProductTechnologyId { get; set; }
        public double AverageSellingPrice { get; set; }
        public double ExcellentPriceByASP { get; set; }
        public double QuotedPrice { get; set; }
        public double QuotedPriceWithSweetner{ get; set; }
        public double FinalPrice { get; set; }
        public decimal Sweetner { get; set; }
        public int OrderTrandId { get; set; }
        public int StatusId { get; set; }
        public int BonusCapQC { get; set; }
        public decimal NonWorkingPrice { get; set; }
        public TblProductCategory? TblProductCategory { get; set; }
        public TblProductType? TblProductType { get; set; }
        public TblBrand? TblBrand { get; set; }
        public TblCustomerDetail? TblCustomerDetail { get; set; }
        public TblExchangeOrder? TblExchangeOrder { get; set; }
        public string? Quality { get; set; }
        public int? QualityId { get; set; }
        /// Added by Vk for Diagnose V2
        public int? ProdCatId { get; set; }
        public int? ProdTypeId { get; set; }
        public int? ProdTechId { get; set; }
        public bool? IsDiagnoseV2 { get; set; }
    }
}
