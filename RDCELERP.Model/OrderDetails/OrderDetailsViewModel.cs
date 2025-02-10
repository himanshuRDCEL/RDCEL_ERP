using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.AbbRegistration;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.BusinessUnit;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.Product;

namespace RDCELERP.Model.OrderDetails
{
    public class OrderDetailsViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string? RegdNo { get; set; }
        public string? CustCity { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductType { get; set; }
        public string? ProductCondition { get; set; }
        public string? StatusCode { get; set; }
        public DateTime? PreferredQCDate { get; set; }
        public string? PreferredQCDateString { get; set; }
        public string? RescheduleDate { get; set; }
        public int? Reschedulecount { get; set; }
        public string? LinksendDate { get; set; }
        public string? OrderCreatedDate { get; set; }
        public string? OrderCreatedDateString { get; set; }
        public string? TicketNumber { get; set; }
        public string? PickupScheduleDate { get; set; }
        public string? PickupScheduleTime { get; set; }
        public string? ServicePartnerName { get; set; }
        public string? OrderDueDateTime { get; set; }
    }
    public class OrderDetailsVMExcelList
    {
        public List<PendingForQCVMExcel>? pendingForQCList { get; set; }
        public List<PendingForPriceAcceptVMExcel>? pendingForPriceAcceptanceList { get; set; }
        public List<PendingForPickupVMExcel>? pendingForPickupList { get; set; }
    }
}
