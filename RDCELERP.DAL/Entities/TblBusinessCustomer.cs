using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblBusinessCustomer
    {
        public TblBusinessCustomer()
        {
            TblBookingItems = new HashSet<TblBookingItem>();
            TblCustomerCompanies = new HashSet<TblCustomerCompany>();
        }

        public int BusinessCustomerId { get; set; }
        public string? ZohoId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNo { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Category { get; set; }
        public string? Type { get; set; }
        public string? Gender { get; set; }
        public string? MailingZip { get; set; }
        public string? MailingStreet { get; set; }
        public string? MailingCity { get; set; }
        public string? MailingCountry { get; set; }
        public string? MailingState { get; set; }
        public string? OtherCountry { get; set; }
        public string? OtherState { get; set; }
        public string? OtherCity { get; set; }
        public string? OtherStreet { get; set; }
        public string? OtherZip { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? LastLogin { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual ICollection<TblBookingItem> TblBookingItems { get; set; }
        public virtual ICollection<TblCustomerCompany> TblCustomerCompanies { get; set; }
    }
}
