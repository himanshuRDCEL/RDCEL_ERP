using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.ABBRedemption;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.LGC;
using RDCELERP.Model.QCComment;

namespace RDCELERP.Model
{
    public class QCCommentViewModel : BaseViewModel
    {
        [Key]
        public int OrderQCId { get; set; }
        public bool Isrescheduled { get; set; }
        public string? RegdNo { get; set; }
        public int? OrderTransId { get; set; }
        public List<SelectListItem>? StatusList { get; set; }
        public string? StatusCode { get; set; }

        [Required(ErrorMessage = "Select Status")]
        [Display(Name = "Select Status")]
        public int? StatusId { get; set; }

        [Required(ErrorMessage = "Please Comment")]
        public string? Qccomments { get; set; }
        public DateTime? ExpectedPickupDate { get; set; }
        public string? QCDate { get; set; }
        public string? ActualSize { get; set; }

        [Display(Name = "Select Quality")]
        public string? QualityAfterQC { get; set; }
        public int? QualityAfterQCId { get; set; }
        public List<SelectListItem>? ProductQualityList { get; set; }
        public decimal? PriceAfterQC { get; set; }
        public IFormFile? Image { get; set; }
        public string? FileName { get; set; }
        public bool? IsPaymentConnected { get; set; }
        public decimal? CollectedAmount { get; set; }
        public DateTime? ProposedQcdate { get; set; }
        public ExchangeOrderViewModel? ExchangeOrderViewModel { get; set; }
        public CustomerDetailViewModel? CustomerDetailViewModel { get; set; }
        public BusinessPartnerViewModel? BusinessPartnerViewModel { get; set; }
        public LogisticViewModel? LogisticViewModel { get; set; }
        public OrderLGCViewModel? OrderLGCViewModel { get; set; }
        public VoucherDetailsViewModel? VoucherDetailsViewModel { get; set; }

        List<OrderImageUploadViewModel> OrderImageUploadViewModel = new List<OrderImageUploadViewModel>();
        public string? ProductCategory { get; set; }
        public string? ProductType { get; set; }
        public string? ProductBrand { get; set; }
        public string? CustDeclaredQlty { get; set; }
        public string? ServicePartnerName { get; set; }
        public decimal? PriceasperEVC { get; set; }
        public decimal? Bonus { get; set; }
        public int Reschedulecount { get; set; }
        public string? RescheduleNote { get; set; }
        public bool? IsUPINo { get; set; }
        public string? CompanyName { get; set; }
        public string? CustFullname { get; set; }
        public string? CustEmail { get; set; }
        public string? CustAddress { get; set; }
        public string? CustCity { get; set; }
        public string? CustState { get; set; }
        public string? ProductDetail { get; set; }
        public string? ZipCode { get; set; }
        public string? CustPhoneNumber { get; set; }
        public string? RescheduleDate { get; set; }
        public string? RescheduleTime { get; set; }
        public int ExchangeId { get; set; }
        public string? UTClogo { get; set; }
        public string? PreferredPickupDate { get; set; }
        public string? CompanyUTC { get; set; }
        public string? UserCompany { get; set; }
        public bool IsVisible { get; set; }
        public string? Exchangeidlist { get; set; }
        public string? PickupStartTime{ get; set; }
        public string? PickupEndTime{ get; set; }
        public string? PickupDateTime{ get; set; }
        public Nullable<System.DateTime> OrderCreatedDate { get; set; }
        public string? ProductCondition { get; set; }
        public ABBRedemptionViewModel? ABBRedemptionViewModel { get; set; }
        public int AbbRedemptionId { get; set; }
        public string? RedemptionIdList { get; set; }
        public bool? IsValidationBasedSweetner { get; set; }
        public bool? IsInvoiceValidated { get; set; }
        public bool? IsInstallationValidated { get; set; }
        public string? InvoiceImageName { get; set; }
        public string? InstallationComment { get; set; }
        public decimal SweetnerAmount { get; set; }
        public string? InvoiceValidated { get; set; }
        public bool? IsInvoiceEnabale { get; set; }
        public bool? IsInstallationEnabale { get; set; }
        public string? QuestionerPdfName { get; set; }
        public string? UPIId { get; set; }
        public string? NewProductInvoiceImage { get; set; }
        public string? NewModelno { get; set; }
        public bool IsModelRequired { get; set; }
        public bool IsBumultiBrand { get; set; }
        public bool IsInvoiceDetailsRequired { get; set; }
        public decimal TotalSweetner{ get; set; }
        public decimal BasePrice { get; set; }
        public string? NewBrandName { get; set; }
        public string? NewModelNoName { get; set; }
        public decimal? SweetenerBu { get; set; }
        public decimal? SweetenerBP { get; set; }
        public decimal? SweetenerDigi2L { get; set; }
        public decimal? Sweetener { get; set; }
        public string? MVCInvoiceImageName { get; set; }
        public string? Base64StringValue { get; set; }
        //public List<string>? VideoBase64StringValue { get; set; }
        public string? VideoBase64StringValue { get; set; }

        public string? ABBProductQulity { get; set; }
        public int? ProductCategoryId { get; set; }

    }

    public class BUBasedSweetnerValidation
    {
        public int Id { get; set; }
        public int BusinessUnitId { get; set; }
        public int QuestionId { get; set; }
        public string? Question { get; set; }
        public bool IsDisplay { get; set; }
        public bool IsRequired { get; set; }
        public string? QuestionKey { get; set; }
    }
}
