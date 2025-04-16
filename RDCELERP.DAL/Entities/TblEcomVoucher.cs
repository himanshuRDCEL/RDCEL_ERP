using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblEcomVoucher
    {
        public int EcomVoucherId { get; set; }
        public string? VoucherCode { get; set; }
        public string? Phoneno { get; set; }
        public int? BrandId { get; set; }
        public string? CategoryIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? CompanyId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? EcomVoucherType { get; set; }
        public int? VoucherCount { get; set; }
        public string? Voucherstatus { get; set; }
        public int? ValueType { get; set; }
        public int? FixedValue { get; set; }
        public int? Percentage { get; set; }
        public int? PercLimit { get; set; }
        public bool? IsUsed { get; set; }

        public virtual TblBrand? Brand { get; set; }
        public virtual TblCompany? Company { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
    }
}
