using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using RDCELERP.DAL.Entities;

namespace RDCELERP.Model.DealerDashBoard
{
    public class OrderCountViewModel
    {
        public int OderCount { get; set; }
        public int VoucherIssued { get; set; }
        public int VoucherRedeemed { get; set; }
        public int CompletedOrders { get; set; }
        public int InProcessOrders { get; set; }
        public int TodaysOrders { get; set; }
        public int CancelledOrders { get; set; }

        public List<SelectListItem>? BusinessPartnerList { get; set; }
        public List<SelectListItem>? CompanyList { get; set; }
        public List<SelectListItem>? CityList { get; set; }
        public List<SelectListItem>? StateList { get; set; }
        public int BusinessPartnerId { get; set; }
        public int BusinessUnitId { get; set; }
        public string? UserEmail { get; set; }
        public string? AssociateCode { get; set; }
        public string? CompanyName { get; set; }
        public string? userRole { get; set; }
        public string? UserCompanyName { get; set; }
        public string? InternalCompanyName { get; set; }
        public string? RoleForSuperAdmin { get; set; }
        public int SelectedCompanyName { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Date { get; set; }
        public bool IsDealer { get; set; }
        public string? VoucherType { get; set; }
    }

    public class CityListModel
    {
        public List<SelectListItem>? CityList { get; set; }
    }

    public class StateListModel
    {
        public List<SelectListItem>? StateList { get; set; }
    }

    public class StoreListModel
    {
        public List<SelectListItem>? StoreList { get; set; }
    }

    public class DealerDashboardViewModel
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string? actionUrl { get; set; }
        public string? RegdNo { get; set; }
        public string? VoucherCode { get; set; }
        public string? ExchangePrice { get; set; }
        public string? Sweetner { get; set; }
        public string? OrderDate { get; set; }
        public string? VoucherRedeemDate { get; set; }
        public string? Paymentstatus { get; set; }
        public string? CustomerName { get; set; }
        public string? OldProductType { get; set; }
        public string? OldProductCategory { get; set; }
        public string? TypeOfSettelment { get; set; }
        public string? VoucherStatus { get; set; }
        public string? OrderReferenceId { get; set; }
        public string? SponsorOrderNumber { get; set; }
        public string? Status { get; set; }
        public string? StoreCode { get; set; }
        public string? LatestStatusDescription { get; set;}
        public string? ActualAmountAsPerQC { get; set; }
        public string? QCComment { get; set; }
        public string? ActualProdQlty { get; set; }
        public string? LatestDateAndTime { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PinCode { get; set; }
        public string? CustomerDeclareQtly { get; set; }
        public string? ActualPickupDate { get; set; }
        public string? AmountPayableThroughLGC { get; set; }
    }


    public class ExchangeOrderDataContract
    {
        [Required(ErrorMessage = "First Name Required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last Name Required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string? LastName { get; set; }
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string? Email { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        [RegularExpression(@"^\d{6}(-\d{5})?$", ErrorMessage = "Invalid Pincode")]
        public string? ZipCode { get; set; }

        [Required]
        [RegularExpression(@"^\d{6}(-\d{5})?$", ErrorMessage = "Invalid Pincode")]
        public string? PinCode { get; set; }
        [Required(ErrorMessage = "Address Required")]
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Number:")]
        [Required(ErrorMessage = "Mobile Number required.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        public string? PhoneNumber { get; set; }
        public string? Bonus { get; set; }
        public string? EstimatedDeliveryDate { get; set; }
        public string? ProductCondition { get; set; }
        public string? CompanyName { get; set; }
        public string? BULogoName { get; set; }
        [Display(Name = "Invoice Number")]
        public string? SponsorOrderNumber { get; set; }
        public string? ZohoSponsorNumber { get; set; }
        public string? BrandName { get; set; }
        [Required(ErrorMessage = "Please select appliance brand")]
        public int BrandId { get; set; }
        public int Id { get; set; }
        public int CustomerDetailsId { get; set; }
        [Required(ErrorMessage = "Please select appliance category")]
        public int ProductCategoryId { get; set; }
        [Required(ErrorMessage = "Please select appliance type")]
        public int ProductTypeId { get; set; }
        public int? LoginID { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public bool OtherCommunications { get; set; }
        public bool OtherCommunications1 { get; set; }
        public bool FollowupCommunication { get; set; }
        public bool FollowupCommunication1 { get; set; }
        [RegularExpression(@"^([0-9]{4})$", ErrorMessage = "Use only 4 Digit")]
        [Range(2002, 2025, ErrorMessage = "Year value should not be greater then 2002")]
        public int ProductAge { get; set; }
        [Required]
        public int QualityCheck { get; set; }
        [Required]
        public int QualityCheckValue { get; set; }
        public Nullable<bool> IsDtoC { get; set; }
        public int? SocietyId { get; set; }
       
        public List<SelectListItem>? ProductTypeList { get; set; }
        public List<SelectListItem>? SocietyDataContractList { get; set; }
        public List<SelectListItem>? ProductModelList { get; set; }
        public List<SelectListItem>? ProductAgeList { get; set; }
        public List<SelectListItem>? QualityCheckList { get; set; }
        public List<SelectListItem>? StoreList { get; set; }
        public List<SelectListItem>? CityList { get; set; }

        public List<SelectListItem>? BrandList { get; set; }
        public string? RegdNo { get; set; }
        [Required(ErrorMessage = "Please select store")]
        public Nullable<int> BusinessPartnerId { get; set; }

        [Display(Name = "Sales Associate Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string? SaleAssociateName { get; set; }
        [Display(Name = "Sales Associate Code")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string? SaleAssociateCode { get; set; }
        public string? BUName { get; set; }
        [Required]
        public int? BusinessUnitId { get; set; }
        public int? ExpectedDeliveryHours { get; set; }
        public string? StoreCode { get; set; }
        public List<SelectListItem>? PurchasedProductCategoryList { get; set; }
        public string? PurchasedProductCategory { get; set; }
        public string? Name { get; set; }
        public Nullable<bool> IsDelivered { get; set; }
        public List<SelectListItem>? PincodeList { get; set; }
        public string? ZohoSponsorOrderId { get; set; }

        public string? AssociateName { get; set; }
        public string? AssociateEmail { get; set; }
        [Required]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        public string? StorePhoneNumber { get; set; }
        public string? FormatName { get; set; }
        public decimal ExchangePrice { get; set; }
        public decimal Sweetner { get; set; }
        public string? ExchangePriceString { get; set; }
        public string? VoucherCode { get; set; }
        public Nullable<System.DateTime> VoucherCodeExpDate { get; set; }
        public string? VoucherCodeExpDateString { get; set; }

        public string? StateName { get; set; }

        public string? CityName { get; set; }
        public int Month { get; set; }
        public string? Month1 { get; set; }
        public int Year { get; set; }
        public string? Year1 { get; set; }
        public string? AssociateCode { get; set; }
        public List<SelectList>? YearList { get; set; }
        public List<SelectList>? MonthList { get; set; }
        public string? AmountStatus { get; set; }
        public int NewProductCategoryId { get; set; }
        public int NewProductCategoryTypeId { get; set; }
        public int NewBrandId { get; set; }
        public string? NewBrandName { get; set; }
        public int ModelNumberId { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? InvoiceImageName { get; set; }
        public string? Base64StringValue { get; set; }
       
        public string? Comment1 { get; set; }
        public string? Comment2 { get; set; }
        public string? Comment3 { get; set; }
        public string? ProductNumber { get; set; }
        public string? ProductName { get; set; }
        public int productBrandID { get; set; }
        public string? IsUnInstallationRequired { get; set; }
        public Nullable<decimal> UnInstallationPrice { get; set; }
        public string? stringUnInstallationPrice { get; set; }

        //Time Slot
        public string? StartTime { get; set; }
        public List<SelectListItem>? StartTimeList { get; set; }
        public string? OrderStatus { get; set; }
        public string? ProductSize { get; set; }
        public string? EndTime { get; set; }
        public string? QCDate { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductType { get; set; }
        public string? OldBrand { get; set; }
        public string? NewProductType { get; set; }
        public string? NewProductCategory { get; set; }
        public string? NewBrand { get; set; }
        public string? OrderCreatedOn { get; set; }
        public int StatusId { get; set; }
        public string? PickupStatus { get; set; }
        public decimal? Sweetener { get; set; }
        public string? Condition1 { get; set; }
        public string? Condition2 { get; set; }
        public string? Condition3 { get; set; }
        public string? Condition4 { get; set; }
        public string? City1 { get; set; }
        public string? State1 { get; set; }

        public string? State { get; set; }
        public string? PriceCode { get; set; }
        public bool IsOtpRequired { get; set; }
        public bool IsOrc { get; set; }
        public bool IsD2C { get; set; }
        public string? PhoneNo { get; set; }
        public bool IsDifferedSettlement { get; set; }
        public bool IsSweetnerModelBased { get; set; }
        public bool IsCustomerAcceptenceRequired { get; set; }
        public bool IsCustomerEmailRequired { get; set; }
        public string? ImageName { get; set; }
        public string? Response { get; set; }
        public string? userRole { get; set; }
        public string? userCompany { get; set; }
        public string? startdate { get; set; }    
        public DateTime StartRangedate { get; set; }    
        public string? enddate { get; set; }   
        public DateTime EndRangeDate { get; set; }
        public int PriceMasterNameId { get; set; }
        public string? ProductBrand { get; set; }
        public bool IsBumultiBrand { get; set; }
        public bool IsModelDetailRequired { get; set; }
        public string? SerialNumber { get; set; }
        public bool? IsProductSerialNumberRequired { get; set; }
    }

    public class VoucherViewModel
    {
        public int VoucherVerficationId { get; set; }
        public int? CustomerId { get; set; }
        public int? ExchangeOrderId { get; set; }
        public string? InvoiceName { get; set; }
        public bool? IsDealerConfirm { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? VoucherCode { get; set; }
        public bool? IsVoucherused { get; set; }
        public string? InvoiceImageName { get; set; }
        public int? BusinessPartnerId { get; set; }
        public int? NewProductCategoryId { get; set; }
        public int? NewProductTypeId { get; set; }
        public int? NewBrandId { get; set; }
        public int? ModelNumberId { get; set; }
        public decimal? ExchangePrice { get; set; }
        public string? InvoiceNumber { get; set; }
        public decimal? Sweetneer { get; set; }
        public int? VoucherStatusId { get; set; }

        public virtual TblBusinessPartner? BusinessPartner { get; set; }
        public virtual TblCustomerDetail? Customer { get; set; }
        public virtual TblExchangeOrder? ExchangeOrder { get; set; }
        public virtual TblModelNumber? ModelNumber { get; set; }
        public virtual TblBrand? NewBrand { get; set; }
        public virtual TblProductCategory? NewProductCategory { get; set; }
        public virtual TblProductType? NewProductType { get; set; }
        public virtual TblVoucherStatus? VoucherStatus { get; set; }
    }

    public class CompanyList
    {
        public List<SelectListItem>? BusinessUnitList { get; set; }
    }

    public class BusinessUnitObject
    {
        public int BusinessUnitId { get; set; }
        public string? Name { get; set; }
    }
}
