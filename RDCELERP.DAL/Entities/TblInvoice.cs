using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblInvoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = null!;
        public string OrderNo { get; set; } = null!;
        public int CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsActive { get; set; }

        public virtual TblBusinessCustomer? CreatedByNavigation { get; set; }
        public virtual TblBusinessCustomer Customer { get; set; } = null!;
        public virtual TblBusinessCustomer? ModifiedByNavigation { get; set; }
    }
}
