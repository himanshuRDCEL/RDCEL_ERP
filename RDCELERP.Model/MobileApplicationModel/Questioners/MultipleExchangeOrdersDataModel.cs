using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.QCComment;

namespace RDCELERP.Model.MobileApplicationModel.Questioners
{
    public class MultipleExchangeOrdersDataModel
    {
        public CustomerDetailViewModel CustomerDetailViewModel { get; set; }
        public List<ProductDetailsDataViewModel> productDetailsDataViewModels { get; set; }

        public int orderCount { get; set; }
    }
    public class ProductDetailsDataViewModel
    {
        public int BrandId { get; set; }
        public string? Bonus { get; set; }

        //[Required(ErrorMessage = "Estimate Delivery Date Required (Preferred Format: dd-MMM-yyyy)")]
        public string? EstimatedDeliveryDate { get; set; }
        [Required]
        public string? ProductCondition { get; set; }
        public int Id { get; set; }
        [Required]
        public string? CompanyName { get; set; }
        public int CustomerDetailsId { get; set; }
        [Range(1, 10000, ErrorMessage = "ProductTypeId  is required.")]
        public int ProductTypeId { get; set; }
        public string? SponsorOrderNumber { get; set; }
        public int LoginID { get; set; }
        public string? RegdNo { get; set; }
        public string? UploadDateTime { get; set; }
        public string? StoreCode { get; set; }
        public int BusinessPartnerId { get; set; }
        public int BUId { get; set; }
        public bool IsDefferedSettlement { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        [AllowNull]
        public string? QCDate { get; set; }
        public string? productDescription { get; set; }

        public QuestionerViewModel? questionerViewModel { get; set; }
        public List<QCRatingViewModel>? qCRatingViewModels { get; set; }


    }

    public class OrderBasicDetailsDataViewModel
    {
        public int BusinessPartnerId { get; set; }
        public int BUId { get; set; }
        public string StoreCode { get; set; }
        public bool IsDefferedSettlement { get; set; }
        public string EstimatedDeliveryDate { get; set; }
        public string CompanyName { get; set; }
        public int LoginID { get; set; }
        public string CustomerMobileNumber { get; set; }
        // { get; set; }
        public int? CustomerDetailsId { get; set; }

    }

    public class OrderResult
    {
        public int OrderId { get; set; }
        public int ordertransId { get; set; }
        public string orderRegdNo { get; set; }

        public int? productId { get; set; }
        public int customerDetailsId { get; set; }

        public bool isQuestionersdone { get; set; }
        
        public bool isHistoryCreated { get; set; }      
        public bool isWhatsAppNotificationSend { get; set; }
    }

}
