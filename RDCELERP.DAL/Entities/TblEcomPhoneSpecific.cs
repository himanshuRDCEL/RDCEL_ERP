using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblEcomPhoneSpecific
    {
        public int EcomPhoneSpecificId { get; set; }
        public string? VoucherCode { get; set; }
        public string? Phoneno { get; set; }
        public int? EcomVoucherId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? Voucherstatus { get; set; }
        public bool? IsUsed { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblEcomVoucher? EcomVoucher { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
    }
}
