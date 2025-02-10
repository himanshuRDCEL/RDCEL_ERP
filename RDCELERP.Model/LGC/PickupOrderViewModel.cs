using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.LGC
{
    public class PickupOrderViewModel:BaseViewModel
    {
        public int? Id { get; set; }
        public int orderTransId { get; set; }
        public string? StatusCode { get; set; }
        public string? RegdNo { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductType { get; set; }
        public string? CustomerName { get; set; }
        public decimal AmountPayableThroughLGC { get; set; }
        /*public string? Action { get; set; }*/
        public string? TicketNumber { get; set; }
        public string? SponsorName { get; set; }
        public string? BrandName { get; set; }
        public TblEvcregistration? tblEvcregistration { get; set; }
        public TblCustomerDetail? tblCustomerDetail { get; set; }

        public TblCustomerDetail TblCustomerDetail = new TblCustomerDetail();

        public TblEvcregistration Tblevcregistration = new TblEvcregistration();

        public TblBrand TblBrand = new TblBrand();
        public PODViewModel? podVM { get; set; }
        //---
        public string? AfterQCProductQty { get; set; }
        public decimal EvcAmount { get; set; }
        public int DriverId { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhoneNumber { get; set; }
        public string? VehicleNumber { get; set; }
        public int EVCRegistrationId { get; set; }
        public string? City { get; set; }
        public string? EVCBusinessName { get; set; }
        public string? InvoicePdfName { get; set; }
        public string? ServicePartnerName { get; set; }
        public string? Date { get; set; }
        public string? Comment { get; set; }
        public string? RescheduleDate { get; set; }
        public string? State { get; set; } 
        public string? EvcName { get; set; }
        public string? OrderState { get; set; }
        public string? PickupScheduleDate { get; set; }
        public string? PickupScheduleTime { get; set; }
        public string? EvcStoreCode { get; set; }
    }
}
