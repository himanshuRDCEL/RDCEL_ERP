using RDCELERP.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.BusinessCustomer
{
   public class ItemCartViewModel :BaseViewModel
    {
        public int ItemCartId { get; set; }
        public int? ItemMasterId { get; set; }
        public int? ItemId { get; set; }
        public string? Itemcode { get; set; }
        public string? Ean { get; set; }
        public string? ItemDesc { get; set; }
        public string? Brand { get; set; }
        public decimal? Mrp { get; set; }
        public string? PurchaseQty { get; set; }
        public decimal? B2bPrice { get; set; }
        public decimal? SubTotalPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? B2BTotalPrice { get; set; }

        public int CartCount { get; set; }
        public string ImageUrl { get; set; }


        public decimal GrandTotalAmount => Convert.ToDecimal(B2bPrice) * Convert.ToDecimal(PurchaseQty);

    }

    public class QuantityVM
 {
        public int ItemCartId { get; set; }
        public int ItemMasterId { get; set; }

        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
    public class RemoveItemVM
    {
        public int ItemCartId { get; set; }
        public int ItemId { get; set; }
    }

    public class ResponseCartVM
    {
        public int Result { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotalPrice { get; set; }
        public decimal TotalPrice { get; set; }

    }

    public class PaymentResponseViewModel
    {
        public string PaymentId { get; set; }      // Razorpay payment ID (returned after success)
        public string OrderId { get; set; }        // Razorpay order ID (you generated)
    }
}
