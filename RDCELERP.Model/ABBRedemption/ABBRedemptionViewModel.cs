using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.AbbRegistration;
using RDCELERP.Model.Base;
using RDCELERP.Model.DealerDashBoard;

namespace RDCELERP.Model.ABBRedemption
{
    public class ABBRedemptionViewModel : BaseViewModel
    {
        [Key]
        public int RedemptionId { get; set; }
        public int ABBRegistrationId { get; set; }
        public string? ZohoAbbregistrationId { get; set; }
        public string? RegdNo { get; set; }
        public string? StoreOrderNo { get; set; }
        public string? Sponsor { get; set; }
        public int? BusinessUnitId { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z]+[a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        [Display(Name = "Enter Customer FirstName")]
        public string? CustFirstName { get; set; }
        public string? CustLastName { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(10)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "Customer Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Phone] public string? CustMobile { get; set; }
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? CustEmail { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Customer Address Line 1")]
        [StringLength(maximumLength: 100)]
        public string? CustAddress1 { get; set; }
        public string? CustAddress2 { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^([0-9]{6})$", ErrorMessage = "Please Enter Valid 6 Digit Mobile Number.")]
        public string? CustPinCode { get; set; }
        [Required(ErrorMessage = "Required")]
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
        public string? LogisticsComments { get; set; }
        public string? QCComments { get; set; }
        public string? InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string? Registrationdate { get; set; }

        [Display(Name = "Invoice Image")]
        public string? InvoiceImage { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Select RedemptionPeriod")]
        public int RedemptionPeriod { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Select RedemptionPeriod")]
        public int RedemptionPercentage { get; set; }

        [Display(Name = "Select RedemptionPeriod")]
        public string? ABBRedemptionStatus { get; set; }

        [Display(Name = "Select RedemptionDate")]

        public DateTime? RedemptionDate { get; set; }
        public decimal RedemptionValue { get; set; }
        //public bool? IsActive { get; set; }
        //public int? CreatedBy { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public int? ModifiedBy { get; set; }
        //public DateTime? ModifiedDate { get; set; }
        public AbbRegistrationModel? AbbRegistrationModel { get; set; }
        public int OrderTransId { get; set; }
        public string? CustState { get; set; }
        public string? StatusCode { get; set; }
        public string? StatusDescription { get; set; }
        public string? CompanyName { get; set; }
        public string? StoreCode { get; set; }
        public string? StorePhoneNumber { get; set; }
        public string? StoreName { get; set; }
        public string? StoreAssociateCode { get; set; }
        public string? StoreEmail { get; set; }
        public string? AbbCreatedDate { get; set; }
        public string? AbbModifiedDate { get; set; }
        public string? InvoiceDateOnly { get; set; }
        public int StatusId { get; set; }
        public int CustomerDetailId { get; set; }
        public string? OrderDate { get; set; }
        public decimal? FinalRedemptionValue { get; set; }
        public bool IsCustomerDetailsEdittable { get; set; }
        public int? CustomerDetailsId { get; set; }
        public bool isFutureDateAllow { get; set; }
        public DateTime currentDate { get; set; }
        public bool isPastDateAllow { get; set; }
        public string? BrandName { get; set; }
        [StringLength(maximumLength: 100)]
        public string? ReferenceId { get; set; }
        public string? QcDate { get; set; }
        public string? ModifiedDateString { get; set; }
        public int? Reschedulecount { get; set; }
        public string? RescheduleDate { get; set; }
        public DateTime? PreferredQCDate { get; set; }
        public string? PreferredQCDateString { get; set; }
        public string? CustFullname { get; set; }
        public string? CustPhoneNumber { get; set; }
        public string? CustAddress { get; set; }
        public string? InvoiceImageName { get; set; }
        public int NoClaimPeriod { get; set; }
        public string? ModelName { get; set; }
        public DateTime? OrderCreatedDate { get; set; }
        public string? OrderCreatedDateString { get; set; }
        public string? LinksendDate { get; set; }
        public string? InvoiceImageUrlMVC { get; set; }
        public string? InvoiceImageUrlERP { get; set; }
        public string? ModelNumber { get; set; }
        public bool isABBInstant { get; set; } = false;
        public string? IsDefferedSettlement { get; set; }
    }

    public class ABBDashBoardViewModel
    {
        public int AbbregistrationId { get; set; }
        public int? BusinessUnitId { get; set; }
        public string? RegdNo { get; set; }
        public string? SponsorOrderNo { get; set; }
        public string? CustFirstName { get; set; }
        public string? CustLastName { get; set; }
        public string? CustMobile { get; set; }
        public string? CustEmail { get; set; }
        public string? CustAddress1 { get; set; }
        public string? CustAddress2 { get; set; }
        public string? Location { get; set; }
        public string? CustPinCode { get; set; }
        public string? CustCity { get; set; }
        public string? CustState { get; set; }
        public int? NewProductCategoryId { get; set; }
        public int? NewProductCategoryTypeId { get; set; }
        public int? NewBrandId { get; set; }
        public string? NewSize { get; set; }
        public string? ProductSrNo { get; set; }
        public int? ModelNumberId { get; set; }
        public string? AbbplanName { get; set; }
        public string? Hsncode { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string? InvoiceNo { get; set; }
        public decimal? NewPrice { get; set; }
        public decimal? Abbfees { get; set; }
        public string? OrderType { get; set; }
        public string? SponsorProdCode { get; set; }
        public int? AbbpriceId { get; set; }
        public DateTime? UploadDateTime { get; set; }
        public int? BusinessPartnerId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? YourRegistrationNo { get; set; }
        public string? InvoiceImage { get; set; }
        public string? AbbplanPeriod { get; set; }
        public string? NoOfClaimPeriod { get; set; }
        public decimal? ProductNetPrice { get; set; }
        public bool? OtherCommunications { get; set; }
        public bool? FollowupCommunication { get; set; }
        public int? StateId { get; set; }
        public string? StoreCode { get; set; }
        public string? StoreName { get; set; }
        public string? NewProductCategoryName { get; set; }
        public string? NewProductCategoryType { get; set; }
        public bool? AbbApprove { get; set; }
        public int? CustomerId { get; set; }
        public int? OrderId { get; set; }
        public string? StoreManagerEmail { get; set; }
        public string? UploadImage { get; set; }
        public bool? SponsorStatus { get; set; }
        public string? MarCom { get; set; }
        public bool? AbbReject { get; set; }
        public bool? PaymentStatus { get; set; }
        public string? OtherModelNo { get; set; }
        public int? StatusId { get; set; }
        public string? EmployeeId { get; set; }
        public string? BusinessUnitName { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductType { get; set; }
        public string? BrandName { get; set; }
        public string? ActionUrl { get; set; }
        public string? AbbAproved { get; set; }
        public string? AbbStoreCode { get; set; }
    }

    public class ABBRedemptionListViewModel
    {
        public int RedemptionId { get; set; }
        public int? AbbregistrationId { get; set; }
        public int? ZohoAbbredemptionId { get; set; }
        public int? CustomerDetailsId { get; set; }
        public string? RegdNo { get; set; }
        public string? AbbredemptionStatus { get; set; }
        public string? StoreOrderNo { get; set; }
        public string? Sponsor { get; set; }
        public string? LogisticsComments { get; set; }
        public string? Qccomments { get; set; }
        public string? InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string? InvoiceImage { get; set; }
        public int? RedemptionPeriod { get; set; }
        public int? RedemptionPercentage { get; set; }
        public string? RedemptionDate { get; set; }
        public decimal? RedemptionValue { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? StatusId { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductType { get; set; }
        public string? BrandName { get; set; }
        public string? BusinessUnitname { get; set; }
        public string? sponsorOrderNumber { get; set; }
        public string? customerPhoneNumber { get; set; }
        public string? customerName { get; set; }
        public string? customerEmail { get; set; }
        public string? ActionUrl { get; set; }
        public string? ReferenceId { get; set; }
        public string? StoreCode { get; set; }
    }

    public class RedemptionDataContract
    {
        public int RedemptionId { get; set; }
        public int? BusinessUnitId { get; set; }
        public Nullable<int> ABBRegistrationId { get; set; }
        public Nullable<int> ZohoABBRedemptionId { get; set; }
        public Nullable<int> CustomerDetailsId { get; set; }
        public string? RegdNo { get; set; }
        public string? ABBRedemptionStatus { get; set; }
        public string? BULogoName { get; set; }
        public string? StoreOrderNo { get; set; }
        public string? Sponsor { get; set; }
        public string? LogisticsComments { get; set; }
        public string? QCComments { get; set; }
        public string? InvoiceNo { get; set; }
        public string? VoucherCodeExpDateString { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string? InvoiceImage { get; set; }
        public Nullable<int> RedemptionPeriod { get; set; }
        public Nullable<int> RedemptionPercentage { get; set; }
        public Nullable<System.DateTime> RedemptionDate { get; set; }
        public Nullable<decimal> RedemptionValue { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<decimal> FinalRedemptionValue { get; set; }
        public string? ReferenceId { get; set; }
        public Nullable<bool> IsVoucherUsed { get; set; }
        public string? VoucherCode { get; set; }
        public Nullable<System.DateTime> VoucherCodeExpDate { get; set; }
        public Nullable<int> VoucherStatusId { get; set; }

        public string? ErrorMessage { get; set; }
        public List<SelectListItem>? TermsandCondition { get; set; }


    }

    public class VoucherDataContract
    {
        public VoucherDataContract()
        {
            ExchangeOrderDataContract = new ExchangeOrderDataContract();
        }

        public int VoucherVerficationId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> ExchangeOrderId { get; set; }
        public Nullable<int> RedemptionId { get; set; }
        public string InvoiceName { get; set; }
        public Nullable<bool> IsDealerConfirm { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string VoucherCode { get; set; }
        public Nullable<bool> IsVoucherused { get; set; }
        public string InvoiceImageName { get; set; }

        public ExchangeOrderDataContract ExchangeOrderDataContract { get; set; }
        public string RNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Number:")]
        [Required(ErrorMessage = "Mobile Number is required.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        public string PhoneNumber { get; set; }

        public string StateName { get; set; }
        public string CityName { get; set; }
        public List<SelectListItem> CityList { get; set; }
        public List<SelectListItem> StoreList { get; set; }
        public int? BusinessPartnerId { get; set; }
        public int BusinessUnitId { get; set; }


        [DataMember]
        [Display(Name = "Product Group")]
        [Required]
        public int? NewProductCategoryId { get; set; }
        [DataMember]
        [Display(Name = "Product Type")]
        [Required]
        public int? NewProductCategoryTypeId { get; set; }
        [DataMember]
        public string BrandName { get; set; }
        [DataMember]
        public int? NewBrandId { get; set; }
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        [Display(Name = "Model Number")]
        [Required]
        public int? ModelNumberId { get; set; }
        public List<SelectListItem> ProductTypeList { get; set; }
        public List<SelectListItem> BrandList { get; set; }
        public List<SelectListItem> ProductModelList { get; set; }
        public string Base64StringValue { get; set; }
        public decimal ExchangePrice { get; set; }
        public decimal Sweetner { get; set; }

        [Required(ErrorMessage = "Invoice Number is required.")]
        public string InvoiceNumber { get; set; }
        public string InvoiceNumberv { get; set; }

        [Required(ErrorMessage = "Serial Number is required.")]
        public string SerialNumber { get; set; }

        public int ExchangePriceOld { get; set; }
        public int ProductTypeIdf { get; set; }
        public string DiffrenceAmount { get; set; }
        public bool isDealer { get; set; }
        public bool IsBuMultiBrand { get; set; }
        public string ImageName { get; set; }
        public string BULogoName { get; set; }

    }
}
