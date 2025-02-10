using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblPriceMasterQuestioner
    {
        public int PriceMasterQuestionerId { get; set; }
        public int? ProductTypeId { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? ProductTechnologyId { get; set; }
        public decimal? AverageSellingPrice { get; set; }
        public decimal? NonWorkingPrice { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ProductCatId { get; set; }
        public bool? IsDiagnoseV2 { get; set; }

        public virtual TblBusinessUnit? BusinessUnit { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblProductCategory? ProductCat { get; set; }
        public virtual TblProductTechnology? ProductTechnology { get; set; }
        public virtual TblProductType? ProductType { get; set; }
    }
}
