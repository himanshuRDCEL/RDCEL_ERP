using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblMahindraLogistic
    {
        public int Id { get; set; }
        public string? ClientOrderId { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Zipcode { get; set; }
        public string? StreetAddress { get; set; }
        public string? Telephone { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Email { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? PickUpAddress { get; set; }
        public string? FirstNamePickup { get; set; }
        public string? LastNamePickup { get; set; }
        public string? ZipcodePickup { get; set; }
        public string? StreetAddressPickup { get; set; }
        public string? TelephonePickup { get; set; }
        public string? StatePickup { get; set; }
        public string? CityPickup { get; set; }
        public string? EmailPickup { get; set; }
        public string? LatitudePickup { get; set; }
        public string? LongitudePickup { get; set; }
        public string? Sku { get; set; }
        public string? Name { get; set; }
        public string? PricePerEachItem { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? Quantity { get; set; }
        public string? FirstNameNone { get; set; }
        public string? LastNameNone { get; set; }
        public string? TelephoneNone { get; set; }
        public string? EmailNone { get; set; }
        public string? OrderId { get; set; }
        public string? AwbNumber { get; set; }
        public string? WmsOrderId { get; set; }
        public string? OrderDeliveryDate { get; set; }
    }
}
