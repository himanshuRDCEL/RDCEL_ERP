using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.ExchangeOrder
{
    public class ExchangeBulkLiquidatioModel : BaseViewModel
    {
        [Key]
        public int Id { get; set; }
        public string? exchangeId { get; set; }
        
        public string? CompanyName { get; set; }
        public string? ZohoSponsorOrderId { get; set; }
        public string? OrderStatus { get; set; }
        public int? CustomerDetailsId { get; set; }
        public int? ProductTypeId { get; set; }
        public int? BrandId { get; set; }
        public string? Bonus { get; set; }
        public string? SponsorOrderNumber { get; set; }
        public string? EstimatedDeliveryDate { get; set; }
        public string? ProductCondition { get; set; }
        public int? LoginId { get; set; }
        public string? ExchPriceCode { get; set; }
        public string? IsDtoC { get; set; }
        public string? IsDefferedSettlement { get; set; }
        public int? SocietyId { get; set; }
        public string? RegdNo { get; set; }
        public int? BusinessPartnerId { get; set; }
        public string? SaleAssociateName { get; set; }
        public string? SaleAssociateCode { get; set; }
        public string? ProductCategory { get; set; }
        public string? StoreCode { get; set; }
        public bool? IsDelivered { get; set; }
        public string? VoucherCode { get; set; }
        public bool? IsVoucherused { get; set; }
        public string? SalesAssociateEmail { get; set; }
        public string? SalesAssociatePhone { get; set; }
        public string? InvoiceImageName { get; set; }
        public string? VoucherCodeExpDate { get; set; }
        //public DateTime? VoucherCodeExpDate { get; set; }
        public decimal? ExchangePrice { get; set; }
        public string? ProductNumber { get; set; }
        public int? NewProductCategoryId { get; set; }
        public int? NewProductTypeId { get; set; }
        public int? NewBrandId { get; set; }
        public int? ModelNumberId { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? ModelNumber { get; set; }
        public string? Qcdate { get; set; }
        public string? RescheduleDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? FinalDate { get; set; }
        public int? StatusId { get; set; }
        public string? Comment1 { get; set; }
        public string? Comment2 { get; set; }
        public string? Comment3 { get; set; }
        public string? IsUnInstallationRequired { get; set; }
        public decimal? UnInstallationPrice { get; set; }
        public int? VoucherStatusId { get; set; }
        public decimal? Sweetener { get; set; }
        public bool? OtherCommunications { get; set; }
        public bool? OtherCommunications1 { get; set; }
        public bool? FollowupCommunication { get; set; }
        public bool? FollowupCommunication1 { get; set; }
        public string? SerialNumber { get; set; }
        public decimal FinalExchangePrice { get; set; }
        public string? ZipCode { get; set; }
        public string? ProductDetail { get; set; }
        public string? NewProductcategory { get; set; }
        public string? NewProductType { get; set; }
        public string? BrandName { get; set; }
        public int? ProductCategoryId { get; set; }
        public string? ProductType { get; set; }
        public decimal Excellent { get; set; }
        public decimal Good { get; set; }
        public decimal Average { get; set; }
        public decimal Notworking { get; set; }
        public string? POD { get; set; }
        public string? CreateDateString { get; set; }
        public string? StatusName { get; set; }
        public string? PodURL { get; set; }
        public string? PodURLERP { get; set; }
        public int OrderTransId { get; set; }
        public string? ProductTypeSize { get; set; }
        public string? NewProductTypeSize { get; set; }

        //Exchange for bulk upload Lov

        public ExchangeVMExcel? ExchangeVM { get; set; }
        //public ExchangeVMExcel?  ExchangeVM { get; set; }
        public IFormFile? UploadExchange { get; set; }
        public List<ExchangeVMExcel>? ExchangeVMList { get; set; }
        public List<ExchangeVMExcel>? ExchangeVMErrorList { get; set; }


        //Exchange for bulk upload 
        public ExchangeBulkExcel? ExchangeVM1 { get; set; }
        [Required(ErrorMessage = "Please upload the file.")]
        public IFormFile? UploadExchange1 { get; set; }
        public List<ExchangeBulkExcel>? ExchangeVMList1 { get; set; }
        public List<ExchangeBulkExcel>? ExchangeVMErrorList1 { get; set; }
    }
}
