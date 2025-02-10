using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblEvcapproved
    {
        public int Id { get; set; }
        public string? ZohoEvcapprovedId { get; set; }
        public string? BussinessName { get; set; }
        public string? EvcregdNo { get; set; }
        public string? ContactPerson { get; set; }
        public string? EvcmobileNumber { get; set; }
        public string? EmailId { get; set; }
        public string? RegdAddressLine1 { get; set; }
        public string? RegdAddressLine2 { get; set; }
        public string? PinCode { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? EvcwalletAmount { get; set; }
        public string? ContactPersonAddress { get; set; }
        public string? UploadGstregistration { get; set; }
        public string? CopyofCancelledCheque { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
