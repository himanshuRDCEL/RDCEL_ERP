using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.LGC
{
    public class PODViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string? RegdNo { get; set; }
        public int? ExchangeId { get; set; }
        public int? EVCId { get; set; }
        public string? Podurl { get; set; }
        public string? FullPoDUrl { get; set; }
        public string? FullDebitNoteUrl { get; set; }
        public string? FullInvoiceUrl { get; set; }
        public string? FullCustDeclUrl { get; set; }
        public string? PoDPdfName { get; set; }
        public string? DebitNotePdfName { get; set; }
        public string? InvoicePdfName { get; set; }
        public int? AbbredemptionId { get; set; }
        public string? PODBase64StringValue { get; set; }
        public string? InvoiceBase64StringValue { get; set; }
        public IFormFile? InvoicePDF { get; set; }
        public string? TicketNo { get; set; }
        public string? ProductCatName { get; set; }
        public string? CustName { get; set; }
        public string? CustPincode { get; set; }
        public string? UtcSeel_INV { get; set; }
        public List<PODViewModel>? podVMList { get; set; }
        public EVCDetailsViewModel? evcDetailsVM { get; set; }
        public Array? ListOfRNum { get; set; }
        public int DriverId { get; set; }
        public int EVCRegistrationId { get; set; }
        public string? CurrentDate { get; set; }
        public string? BaseUrl { get; set; }
        public decimal? OrderAmtForEVCDN { get; set; }
        public decimal? OrderAmtForEVCInv { get; set; }
        public decimal? GstAmtForEVCDN { get; set; }
        public decimal? GstAmtForEVCInv { get; set; }
        public int? InvSrNum { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public DateTime? DebitNoteDate { get; set; }
        public decimal? DebitNoteAmount { get; set; }
        public int? DnsrNum { get; set; }
        public int? OrderTransId { get; set; }
        public string? EVCBussinessName { get; set; }
        public string? EVCCity { get; set; }
        public string? EVCRegdNo { get; set; }
        public string? FinancialYear { get; set; }
        public string? ServicePartnerName { get; set; }
        public List<OrdertranList>? OrderTransListO { get; set; }
        public bool? IsDebitNoteSkiped { get; set; }
        public int? DNOrderCount { get; set; }
        public int? InvOrderCount { get; set; }
        public string? BillCounterNum { get; set; }
        public string? InvoiceDateString { get; set; }
        public int? EvcPartnerId { get; set; }

        //EVC Revised Algo Inv DN V2
        public bool? IsOrderAmtWithSweetener { get; set; }
        public int? GsttypeId { get; set; }
        public decimal? BaseValue { get; set; }
        public decimal? Cgstamt { get; set; }
        public decimal? Sgstamt { get; set; }
        public decimal? Igstamt { get; set; }
        public decimal? SweetenerAmt { get; set; }
        public int? Lgccost { get; set; }
        public int? MaxInvSrNum { get; set; }
        public string? GstType { get; set; }
        public decimal? PrimeProductDiffAmt { get; set; }
    }
    public class EVCDetailsViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string? DNnum { get; set; }
        public string? BillNumberDN { get; set; }
        public string? BillNumberInv { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Pincode { get; set; }
        public string? State { get; set; }
        public string? PlaceOfSupply { get; set; }
        public string? CurrentDate { get; set; }
        public decimal? FinalPriceDN { get; set; }
        public string? FinalPriceInWordsDN { get; set; }
        public decimal? FinalPriceInv { get; set; }
        public string? FinalPriceInWordsInv { get; set; }
        public int? EvcPartnerId { get; set; }
        public string? EvcStoreCode { get; set; }
        public decimal? TotalCgstamt { get; set; }
        public decimal? TotalSgstamt { get; set; }
        public decimal? TotalIgstamt { get; set; }
        public decimal? TotalSweetenerAmt { get; set; }
        public int? TotalLgccost { get; set; }
        public decimal? TotalPrimeProductDiffAmt { get; set; }
    }

    public class OrdertranList
    {
        public int OrdertransId { get; set; }
    }

}
