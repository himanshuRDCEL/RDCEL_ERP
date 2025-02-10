using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.TicketGenrateModel.Mahindra
{
    public class MahindraLogisticsResponseDataContract
    {
        public object? orderId { get; set; }
        public int awbNumber { get; set; }
        public object? wmsOrderId { get; set; }
        public object? clientOrderId { get; set; }
        public object? clientCode { get; set; }
        public object? id { get; set; }
        public object? status { get; set; }
        public object? delivery_date { get; set; }
        public object? createdDate { get; set; }
        public object? createdOn { get; set; }
        public object? slot { get; set; }
        public object? updatedAt { get; set; }
        public object? siteCode { get; set; }
        public int itemQuantity { get; set; }
        public double orderTotal { get; set; }
        public object? address { get; set; }
        public object? orderImageUrl { get; set; }
        public object? orderImageDisplayName { get; set; }
        public object? pickupLocation { get; set; }
        public object? dropOffLocation { get; set; }
        public object? shippingInfo { get; set; }
        public object? orderItems { get; set; }
        public object? customer_firstname { get; set; }
        public object? customer_lastname { get; set; }
        public object? payment { get; set; }
        public object? reason { get; set; }
        public object? customer_location { get; set; }
        public bool createdBatch { get; set; }
        public object? expectedDeliverySlot { get; set; }
        public double weight { get; set; }
        public double volume { get; set; }
        public int crates { get; set; }
        public object? phoneNumber { get; set; }
        public object? trackLink { get; set; }
        public object? eta { get; set; }
        public object? driverName { get; set; }
        public object? location { get; set; }
        public object? estimatedTimeOfArrival { get; set; }
        public object? completedAt { get; set; }
        public object? paymentType { get; set; }
        public object? orderHistory { get; set; }
        public object? confirmedLocation { get; set; }
        public bool requestForSignature { get; set; }
        public bool requestForOTP { get; set; }
        public bool requestForImageUpload { get; set; }
    }
}
