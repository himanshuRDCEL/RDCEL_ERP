using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblOrderPromoVoucher
    {
        public int OrderPromoVoucherId { get; set; }
        public int? OrderTransId { get; set; }
        public int? CustId { get; set; }
        public string? VoucherCode { get; set; }
        public int? VoucherAmt { get; set; }
        public int? VoucherStatus { get; set; }
        public bool? IsVoucherUsed { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
