using RDCELERP.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.BusinessCustomer
{
    public class BookingItemViewModel :BaseViewModel
    {
        public int BookingItemId { get; set; }
        public int? CustomerId { get; set; }
        public int? ItemId { get; set; }
        public int? Quantity { get; set; }
        public bool? Qccheck { get; set; }
        public bool? QcverifiedQunatity { get; set; }
        public decimal? BookingPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? B2B_Price { get; set; }
        public string? OrderNo { get; set; }

        public string ItemDesc { get; set; }
        public string SyncOrderNo { get; set; }
        public string CreatedDateString { get; set; }
        public bool? IsBookingPaymentConfirm { get; set; }
        public BusinessCustomerViewModel BusinessCustomerViewModel { get; set; }
        public ItemMasterViewModel ItemMasterViewModel { get; set; }
        public ItemCartViewModel ItemCartViewModel { get; set; }
        public int? UnderPicking { get; set; }
        public int? PickedQty { get; set; }
        public int? DispatchedQty { get; set; }
        public int? AdjustedQty { get; set; }
        public int? PendingQty { get; set; }
        public string? RazorOrderId { get; set; }
        public string? ItemStatusName { get; set; }
        public DateTime? DeliveryDate { get; set; }

    }
    public class UpdateBookingItemViewModel
    {
      
        public string SyncOrderNo { get; set; }
        public int? UnderPicking { get; set; }
        public int? PickedQty { get; set; }
        public int? DispatchedQty { get; set; }
        public int? AdjustedQty { get; set; }
        public int? PendingQty { get; set; }
        public string? RazorOrderId { get; set; }
        public DateTime? DeliveryDate { get; set; }

    }


    public class AdminOrderDetailViewModel :BaseViewModel
    {
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }

        public BusinessCustomerViewModel CustomerVM { get; set; }
        public BtoBPaymentViewModel PaymentVM { get; set; }

        public List<BookingItemViewModel> BookingItemsVm { get; set; }
    }
}
