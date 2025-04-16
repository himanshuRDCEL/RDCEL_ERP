using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblCouponMaster
    {
        public TblCouponMaster()
        {
            TblCoupons = new HashSet<TblCoupon>();
        }

        public int CouponMasterId { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? BusinessPartnerId { get; set; }
        public string Title { get; set; } = null!;
        public decimal? TotalValueOfCoupons { get; set; }
        public int? CouponCount { get; set; }
        public decimal? CouponL1value { get; set; }
        public decimal? CouponL2value { get; set; }
        public decimal? CouponL3value { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblBusinessPartner? BusinessPartner { get; set; }
        public virtual TblBusinessUnit? BusinessUnit { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual ICollection<TblCoupon> TblCoupons { get; set; }
    }
}
