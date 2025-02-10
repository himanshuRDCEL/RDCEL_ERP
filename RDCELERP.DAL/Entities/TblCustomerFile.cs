using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblCustomerFile
    {
        public int Id { get; set; }
        public int? AbbregistrationId { get; set; }
        public string? RegdNo { get; set; }
        public string? CertificatePdfName { get; set; }
        public string? InvoicePdfName { get; set; }
        public int? InvSrNum { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public string? FinancialYear { get; set; }
        public int? OrderTransId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblAbbregistration? Abbregistration { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblOrderTran? OrderTrans { get; set; }
    }
}
