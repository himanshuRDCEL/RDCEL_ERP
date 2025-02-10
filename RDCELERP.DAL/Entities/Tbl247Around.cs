using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class Tbl247Around
    {
        public int Id { get; set; }
        public string? PartnerName { get; set; }
        public string? OrderId { get; set; }
        public string? ItemId { get; set; }
        public string? Product { get; set; }
        public string? Brand { get; set; }
        public string? ProductType { get; set; }
        public string? Category { get; set; }
        public string? SubCategory { get; set; }
        public string? Name { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Pincode { get; set; }
        public string? City { get; set; }
        public string? RequestType { get; set; }
        public string? PaidByCustomer { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? TwoFourSevenAroundBooKingId { get; set; }
    }
}
