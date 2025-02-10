using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.DealerDashBoard;
using RDCELERP.Model.ExchangeOrder;

namespace RDCELERP.Model.VoucherRedemption
{
    public class VoucherDataContract
    {
        public int VoucherVerficationId { get; set; }
        public int? CustomerId { get; set; }
        public int? ExchangeOrderId { get; set; }
        public int? RedemptionId { get; set; }
        public string? InvoiceName { get; set; }
        public bool? IsDealerConfirm { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [Required]
        public string? VoucherCode { get; set; }
        public bool? IsVoucherused { get; set; }
        public string? InvoiceImageName { get; set; }

        public ExchangeOrderDataContract? ExchangeOrderDataContract { get; set; }
        public string? RNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Number:")]
        [Required(ErrorMessage = "Mobile Number is required.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        public string? PhoneNumber { get; set; }

        public string? StateName { get; set; }
        public string? CityName { get; set; }
        public SelectListItem? CityList { get; set; }
        public SelectListItem? StoreList { get; set; }
        public int? BusinessPartnerId { get; set; }
        public int? BusinessUnitId { get; set; }


        [DataMember]
        [Display(Name = "Product Group")]
        [Required]
        public int? NewProductCategoryId { get; set; }
        [DataMember]
        [Display(Name = "Product Type")]
        [Required]
        public int? NewProductCategoryTypeId { get; set; }
        [DataMember]
        public string? BrandName { get; set; }
        [DataMember]
        public int? NewBrandId { get; set; }
        [DataMember]
        public string? ModelName { get; set; }
        [DataMember]
        [Display(Name = "Model Number")]
        [Required]
        public int? ModelNumberId { get; set; }
        public SelectListItem? ProductTypeList { get; set; }
        public SelectListItem? BrandList { get; set; }
        public SelectListItem? ProductModelList { get; set; }
        public string? Base64StringValue { get; set; }
        public decimal ExchangePrice { get; set; }
        public decimal Sweetner { get; set; }

        [Required(ErrorMessage = "Invoice Number is required.")]
        public string? InvoiceNumber { get; set; }
        public string? InvoiceNumberv { get; set; }

        [Required(ErrorMessage = "Serial Number is required.")]
        public string? SerialNumber { get; set; }

        public int? ExchangePriceOld { get; set; }
        public int ProductTypeIdf { get; set; }
        public string? DiffrenceAmount { get; set; }
        public bool isDealer { get; set; }
        public bool IsBuMultiBrand { get; set; }
        public string? ImageName { get; set; }
        public string? BULogoName { get; set; }
        public string? CompanyName { get; set; }
        public loginUserDetailsforVoucher? loginUserDetailsforVoucher { get; set; }
        public VerifyVoucherResult? verifyVoucherResult { get; set; }
        public CustomerDetailViewModel? customerDetails { get; set; }

    }

    public class loginUserDetailsforVoucher
    {
        public int? userId { get; set; }
        public string? userName { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? companyName { get; set; }
        public string? roleName { get; set; }
        public int businessPartnerId { get; set; }
        public int businessUnitId { get; set; }

    }
    public class VerifyVoucherResult
    {
        public bool isVerified { get; set; }
        public string? responseMesage { get; set; }
    }



    public class voucherCapture
    {
        public voucherUserDetails userDetails { get; set; }
        public vaucherCaptureNotification notification { get; set; }
    }


    //Capture voucher Notification
    public class vaucherCaptureNotification
    {
        public string type { get; set; }
        public string sender { get; set; }
        public string templateId { get; set; }
        public vouchercaptureProperties @params { get; set; }
    }
    //Paremeters to send sms for vouchercaptureProperties  on whatssapp yellow.ai
    public class vouchercaptureProperties
    {
        [JsonProperty("1")]
        public string customerName { get; set; }
        [JsonProperty("2")]
        public string vouchercode { get; set; }
        [JsonProperty("3")]
        public string companyName { get; set; }
        [JsonProperty("4")]
        public string oldProductcategory { get; set; }
        [JsonProperty("5")]
        public string olsProductCategory { get; set; }
        [JsonProperty("6")]
        public string workingQualities { get; set; }

    }

    public class voucherUserDetails
    {
        public string number { get; set; }
    }
}
