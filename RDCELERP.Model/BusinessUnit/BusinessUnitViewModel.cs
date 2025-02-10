using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.ABBPriceMaster;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.Company;
using RDCELERP.Model.Master;
using RDCELERP.Model.PriceMaster;
using RDCELERP.Model.Product;

namespace RDCELERP.Model.BusinessUnit
{
    public class BusinessUnitViewModel : BaseViewModel
    {
        public int BusinessUnitId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [DisplayName("Name")]
       
        public string? Name { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        [DisplayName("Description")]
       
        public string? Description { get; set; }
      
        public string? RegistrationNumber { get; set; }
       
        public string? QrcodeUrl { get; set; }
      
        public string? LogoName { get; set; }
        public string? LogoUrlLink { get; set; }
        public string? ContactPersonFirstName { get; set; }
       
        public string? ContactPersonLastName { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                 ErrorMessage = "Entered phone format is not valid.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string? Email { get; set; }
       
        public string? AddressLine1 { get; set; }
       
        public string? AddressLine2 { get; set; }
        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Please enter valid 6 digit pincode.")]
        public string? Pincode { get; set; }
       
        public string? City { get; set; }
     
        public string? State { get; set; }

        public int? LoginId { get; set; }
     
        public string? ZohoSponsorId { get; set; }
        [Required(ErrorMessage = "Expected delivery hours is required.")]

        public int? ExpectedDeliveryHours { get; set; }
        [Required(ErrorMessage = "Voucher expiry time is required.")]
      
        public int? VoucherExpiryTime { get; set; }
        [Required(ErrorMessage = "If nothing then enter 0")]
       
        public decimal? SweetnerForDtd { get; set; }
        [Required(ErrorMessage = "If nothing then enter 0")]
        public decimal? SweetnerForDtc { get; set; }
        [DisplayName("Is Sweetner Model Based")]
        public bool IsSweetnerModelBased { get; set; }
        [DisplayName("Is BU D2C")]
        public bool IsBud2c { get; set; }
        [DisplayName("Is Abb")]
        public bool IsAbb { get; set; }
        [DisplayName("Is Exchange")]
        public bool IsExchange { get; set; }
        [DisplayName("Is BU Multi Brand")]
        public bool IsBumultiBrand { get; set; }
        [DisplayName("Show Abb Plan")]
        public bool ShowAbbPlan { get; set; }
        [DisplayName("Is Invoice Details Required")]
        public bool IsInvoiceDetailsRequired { get; set; }
        [DisplayName("Is New Product Details Required")]
        public bool IsNewProductDetailsRequired { get; set; }
        [Required(ErrorMessage = "GST type is required")]
        [DisplayName("GST Type")]
        public int? Gsttype { get; set; }
        [Required(ErrorMessage = "Margin type is required")]
        [DisplayName("Margin Type")]
        public int? MarginType { get; set; }
        [DisplayName("Is Sponsor Number Required On UI")]
        public bool IsSponsorNumberRequiredOnUi { get; set; }
        [DisplayName("Is UPI Id Required")]
        public bool IsUpiIdRequired { get; set; }
        [DisplayName("Is Payment Third Party ")]
        public bool IsPaymentThirdParty { get; set; }
        [DisplayName("Is Model Detail Required ")]
        public bool IsModelDetailRequired { get; set; }
        [DisplayName("Is New Brand Required ")]
        public bool IsNewBrandRequired { get; set; }
        [DisplayName("Is Area Locality ")]
        public bool IsAreaLocality { get; set; }
        [DisplayName("Is Validation Based Sweetner")]
        public bool IsValidationBasedSweetner { get; set; }
        [DisplayName("Is Quality Required On UI")]
        public bool IsQualityRequiredOnUi { get; set; }
        [DisplayName("Is Quality Working NonWorking")]
        public bool IsQualityWorkingNonWorking { get; set; }
        [DisplayName("Is Standard Price Master")]
        public bool IsStandardPriceMaster { get; set; }
        [DisplayName("Show Emplyee Code")]
        public bool ShowEmplyeeCode { get; set; }
        [DisplayName("Is Qc DateTime Required On D2c")]
        public bool IsQcdateTimeRequiredOnD2c { get; set; }
        [DisplayName("Is Bulk Order")]
        public bool IsBulkOrder { get; set; }
        [DisplayName("Is Certificate Available")]
        public bool IsCertificateAvailable { get; set; }
        [DisplayName("Is Sweetner Independent")]
        public bool IsSweetenerIndependent { get; set; }
        [DisplayName("Is Invoice Available")]
        public bool IsInvoiceAvailable { get; set; }

        [DisplayName("Is Abb")]
        public bool Abb { get; set; }
        [DisplayName("Is Exchange")]
        public bool Exchange { get; set; }


        public string? Date { get; set; }
        
        public int? ActiveTabId { get; set; }
        public IFormFile? UploadBusinessPartner { get; set; }
        public IFormFile? UploadExchangePriceMaster { get; set; }
        public CompanyViewModel? CompanyVM { get; set; }
        public BusinessPartnerVMExcelModel? BusinessPartnerVM { get; set; }
        public List<BusinessPartnerVMExcelModel>? BusinessPartnerVMList { get; set; }
        public List<BusinessPartnerVMExcelModel>? BusinessPartnerVMErrorList { get; set; }
        public ABBPriceMasterViewModel? ABBPriceMasterVM { get; set; }
        public PriceMasterViewModel? ExchangePriceMasterVM { get; set; }
        public List<PriceMasterVMExcel>? ExchangePriceMasterVMList { get; set; }
        public List<PriceMasterVMExcel>? ExchangePriceMasterVMErrorList { get; set; }
        public IList<ProductCategoryViewModel>? ProductCategoryVMList { get; set; }
        public IList<ProductTypeViewModel>? ProductTypeVMList { get; set; }
        public BusinessUnitLoginVM? BuLoginVM { get; set; }
    }
}
