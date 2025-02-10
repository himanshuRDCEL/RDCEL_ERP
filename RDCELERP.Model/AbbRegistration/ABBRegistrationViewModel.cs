using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.AbbRegistration;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.ABBRedemption
{
    public class ABBRegistrationViewModel : BaseViewModel
    {
        public int ABBRegistrationId { get; set; }
        public int ABBRedemptionId { get; set; }
        public string? RegdNo { get; set; }
        public string? CustFirstName { get; set; }
        public string? CustLastName { get; set; }        
        [Phone] public string? CustMobile { get; set; }
        public string? CustEmail { get; set; }
        public string? CustAddress1 { get; set; }
        public string? CustAddress2 { get; set; }
        public string? CustPinCode { get; set; }
        public string? CustCity { get; set; }
        public int StateId { get; set; }
        public string? Location { get; set; }
        public int? NewProductCategoryId { get; set; }
        public int? NewProductCategoryTypeId { get; set; }
        public string? NewProductCategoryName { get; set; }
        public string? NewProductCategoryType { get; set; }
        public int NewBrandId { get; set; }
        public string? SponsorName { get; set; }
        public string? NewSize { get; set; }
        public string? ProductSrNo { get; set; }
        public int? ModelNumberId { get; set; }
        public string? AbbplanName { get; set; }
        public decimal? ProductNetPrice { get; set; }
        public string? AbbplanPeriod { get; set; }
        public string? InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string? InvoiceImage { get; set; }
        //Abb for bulk upload
        public AbbRegistrationVMExcel? AbbVM { get; set; }
        public IFormFile? UploadAbb { get; set; }
        public List<AbbRegistrationVMExcel>? AbbVMList { get; set; }
        public List<AbbRegistrationVMExcel>? AbbVMErrorList { get; set; }
        //ABB Redemption Fields
        public string? LogisticComments { get; set; }
        public string? QCComments { get; set; }
        public int RedemptionPeriod { get; set; }
        public int RedemptionPercentage { get; set; }
        public string? ABBRedemptionStatus { get; set; }
        public DateTime? RedemptionDate { get; set; }
        public decimal RedemptionValue { get; set; }
        public AbbRegistrationModel? AbbRegistrationModel { get; set; }

        //Added By VK for Generate Certificate
        public string? BrandName { get; set; }
        public string? ProductDesc { get; set; }
        public string? ProductSrNum { get; set; }
        public string? DateOfPurchase { get; set; }
        public string? DateOfExpiry { get; set; }
        public bool? IsCertificateAvailable { get; set; }

        // Added for ABB Invoices
        public string? CustState { get; set; }
        public bool? IsInvoiceAvailable { get; set; }
        public decimal? Abbfees { get; set; }
        public decimal? BaseValue { get; set; }
        public string? GSTType { get; set; }
        public decimal? Cgst { get; set; }
        public decimal? Sgst { get; set; }
        public string? BillNumber { get; set; }
        public string? BillCounterNum { get; set; }
        public string? FinancialYear { get; set; }
        public string? BUEmail { get; set; }
        public string? BPEmail { get; set; }
    }
}
