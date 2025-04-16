using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblCoupon
    {
        public int CouponId { get; set; }
        public int? CouponMasterId { get; set; }
        public string? CouponL1 { get; set; }
        public string? CouponL2 { get; set; }
        public string? CouponL3 { get; set; }
        public string? UsedCoupon { get; set; }
        public decimal? UsedCouponValue { get; set; }
        public bool? IsUsed { get; set; }
        public int? OrderTransId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblCouponMaster? CouponMaster { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblOrderTran? OrderTrans { get; set; }
    }
}
