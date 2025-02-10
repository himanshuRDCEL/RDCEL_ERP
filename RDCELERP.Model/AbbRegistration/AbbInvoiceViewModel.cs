using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.AbbRegistration
{
    public class AbbInvoiceViewModel : BaseViewModel
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

        // Local Data
        public string? BillNumber { get; set; }
        public string? PlaceOfSupply { get; set; }
        public string? CurrentDate { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Pincode { get; set; }
        public string? State { get; set; }
        public decimal? BasePrice { get; set; }
        public decimal? GST { get; set; }
        public decimal? FinalPrice { get; set; }
        public string? FinalAmtInWords { get; set; }
        public string? UtcSeel_INV { get; set; }
    }
}
