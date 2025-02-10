using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblVehicleIncentive
    {
        public int IncentiveId { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? ProductTypeId { get; set; }
        public int? BusinessUnitId { get; set; }
        public decimal? BasePrice { get; set; }
        public TimeSpan? PickupTatinHr { get; set; }
        public decimal? PickupIncAmount { get; set; }
        public TimeSpan? DropTatinHr { get; set; }
        public decimal? DropIncAmount { get; set; }
        public decimal? PackagingIncentive { get; set; }
        public decimal? DropImageIncentive { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblBusinessUnit? BusinessUnit { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblProductCategory? ProductCategory { get; set; }
        public virtual TblProductType? ProductType { get; set; }
    }
}
