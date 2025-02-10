using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.LGC;

namespace RDCELERP.Model.EVC_Portal
{
    public class BulkDownlodeCDPdfviewmodel
    {
        public int? Id { get; set; }
        public string? RegdNo { get; set; }
        public string? CDPdfName { get; set; }

        public string? ProductCatName { get; set; }
        public string? CustName { get; set; }
        public string? EVCPartnerCode { get; set; }
        public string? EVCBussinessName { get; set; }
        public string? EVCCity { get; set; }
        public string? CDDate { get; set; }
        public string? CustPincode { get; set; }
        public string? FullCustDeclUrl { get; set; }
        public int? EVCId { get; set; }
        public string? EVCRegdNo { get; set; }
        public int? EvcPartnerId { get; set; }
    }
    public class BulkDownlodeInvoidePdfviewmodel
    {

        public int? Id { get; set; }
        public string? InvoicePdfName { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public string? EVCPartnerCode { get; set; }
        public string? EVCBussinessName { get; set; }
        public string? EVCCity { get; set; }
        public string? InvoiceDate { get; set; }

        public int? EVCId { get; set; }
        public string? EVCRegdNo { get; set; }
        public int? EvcPartnerId { get; set; }
        public string? FullInvoiceUrl { get; set; }
        public decimal? OrderAmtForEVCInv { get; set; }
        public decimal? GstAmtForEVCInv { get; set; }
        public int? InvSrNum { get; set; }

    }
    public class BulkDownlodeDebitnotePdfviewmodel
    {
        public int? Id { get; set; }
        public int? EVCId { get; set; }
        public string? EVCBussinessName { get; set; }
        public string? EVCCity { get; set; }
        public string? EVCRegdNo { get; set; }
        public int? EvcPartnerId { get; set; }
        public string? EVCPartnerCode { get; set; }
        public string? FullDebitNoteUrl { get; set; }
        public string? DebitNotePdfName { get; set; }
        public decimal? OrderAmtForEVCDN { get; set; }
        public decimal? GstAmtForEVCDN { get; set; }
        public string? DebitNoteDate { get; set; }
        public decimal? DebitNoteAmount { get; set; }
        public int? DnsrNum { get; set; }
        public int? DNOrderCount { get; set; }

    }
    public class BulkDownlodePODPdfviewmodel
    {
        public int? Id { get; set; }
        public int? EVCId { get; set; }
        public string? EVCBussinessName { get; set; }
        public string? EVCCity { get; set; }
        public string? EVCRegdNo { get; set; }
        public int? EvcPartnerId { get; set; }
        public string? EVCPartnerCode { get; set; }
        public string? Podurl { get; set; }
        public string? FullPoDUrl { get; set; }
        public string? PODPdfName { get; set; }
        public string? TicketNo { get; set; }
        public string? PODDate { get; set; }


    }
}
