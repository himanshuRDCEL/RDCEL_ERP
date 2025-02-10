using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblPromotionalVoucherMaster
    {
        public int PromotionalVoucherId { get; set; }
        public string? VoucherName { get; set; }
        public string? VoucherCode { get; set; }
        public string? CompanyName { get; set; }
        public int? VoucherAmt { get; set; }
        public int? ExpiringInDays { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
