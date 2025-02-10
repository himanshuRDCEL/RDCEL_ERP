using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblBizlogTicket
    {
        public TblBizlogTicket()
        {
            TblImages = new HashSet<TblImage>();
        }

        public int Id { get; set; }
        public string BizlogTicketNo { get; set; } = null!;
        public string? SponsrorOrderNo { get; set; }
        public string? ConsumerName { get; set; }
        public string? ConsumerComplaintNumber { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? Pincode { get; set; }
        public string? TelephoneNumber { get; set; }
        public string? RetailerPhoneNo { get; set; }
        public string? AlternateTelephoneNumber { get; set; }
        public string? EmailId { get; set; }
        public string? DateOfPurchase { get; set; }
        public string? DateOfComplaint { get; set; }
        public string? NatureOfComplaint { get; set; }
        public string? IsUnderWarranty { get; set; }
        public string? Brand { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public string? Model { get; set; }
        public string? IdentificationNo { get; set; }
        public string? DropLocation { get; set; }
        public string? DropLocAddress1 { get; set; }
        public string? DropLocAddress2 { get; set; }
        public string? DropLocCity { get; set; }
        public string? DropLocState { get; set; }
        public string? DropLocPincode { get; set; }
        public string? DropLocContactPerson { get; set; }
        public string? DropLocContactNo { get; set; }
        public string? DropLocAlternateNo { get; set; }
        public string? PhysicalEvaluation { get; set; }
        public string? TechEvalRequired { get; set; }
        public string? Value { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? TicketPriority { get; set; }

        public virtual ICollection<TblImage> TblImages { get; set; }
    }
}
