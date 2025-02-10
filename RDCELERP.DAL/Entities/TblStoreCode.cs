using System;
using System.Collections.Generic;

#nullable disable

namespace RDCELERP.DAL.Entities
{
    public partial class TblStoreCode
    {
        public int StoreCodeId { get; set; }
        public string SponsorName { get; set; }
        public string StoreCode { get; set; }
        public int? PinCode { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public string StoreManagerEmail { get; set; }
        public string AssociateCode { get; set; }
        public int? PaymentToCustomer { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
