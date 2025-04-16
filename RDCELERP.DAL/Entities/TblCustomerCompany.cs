using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblCustomerCompany
    {
        public int CustomerCompanyId { get; set; }
        public string? Name { get; set; }
        public int? BusinessCustomerId { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
        public string? Zipcode { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblBusinessCustomer? BusinessCustomer { get; set; }
        public virtual TblCity? City { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblState? State { get; set; }
    }
}
