using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblBlowHornTicket
    {
        public int Id { get; set; }
        public string AwbNumber { get; set; } = null!;
        public string? DeliveryHub { get; set; }
        public string? PickupHub { get; set; }
        public string? IsHyperlocal { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerMobile { get; set; }
        public string? PinNumber { get; set; }
        public string? AlternateCustomerMobile { get; set; }
        public string? CustomerEmail { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? DeliveryPostalCode { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? What3words { get; set; }
        public string? CustomerReferenceNumber { get; set; }
        public string? DeliveryLat { get; set; }
        public string? DeliveryLon { get; set; }
        public string? PickupAddress { get; set; }
        public string? PickupPostalCode { get; set; }
        public string? PickupLat { get; set; }
        public string? PickupLon { get; set; }
        public string? PickupCustomerName { get; set; }
        public string? PickupCustomerMobile { get; set; }
        public string? IsReturnOrder { get; set; }
        public string? CommercialClass { get; set; }
        public string? IsCommercialAddress { get; set; }
        public DateTime? PickupDatetime { get; set; }
        public string? Division { get; set; }
        public DateTime? ExpectedDeliveryTime { get; set; }
        public bool? IsCod { get; set; }
        public string? CashOnDelivery { get; set; }
        public string? ProductStatus { get; set; }
        public string? SponsrorOrderNo { get; set; }
    }
}
