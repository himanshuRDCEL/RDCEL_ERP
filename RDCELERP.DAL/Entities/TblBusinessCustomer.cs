using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblBusinessCustomer
    {
        public TblBusinessCustomer()
        {
            TblBookingItemCreatedByNavigations = new HashSet<TblBookingItem>();
            TblBookingItemCustomers = new HashSet<TblBookingItem>();
            TblBookingItemModifiedByNavigations = new HashSet<TblBookingItem>();
            TblBtoBpayments = new HashSet<TblBtoBpayment>();
            TblCustomerCompanies = new HashSet<TblCustomerCompany>();
            TblItemCarts = new HashSet<TblItemCart>();
            TblItemMasterCreatedByNavigations = new HashSet<TblItemMaster>();
            TblItemMasterModifiedByNavigations = new HashSet<TblItemMaster>();
            TblItems = new HashSet<TblItem>();
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
        public string? ShippingZip { get; set; }
        public string? ShippingStreet { get; set; }
        public string? ShippingCity { get; set; }
        public string? ShippingCountry { get; set; }
        public string? ShippingState { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual ICollection<TblBookingItem> TblBookingItemCreatedByNavigations { get; set; }
        public virtual ICollection<TblBookingItem> TblBookingItemCustomers { get; set; }
        public virtual ICollection<TblBookingItem> TblBookingItemModifiedByNavigations { get; set; }
        public virtual ICollection<TblBtoBpayment> TblBtoBpayments { get; set; }
        public virtual ICollection<TblCustomerCompany> TblCustomerCompanies { get; set; }
        public virtual ICollection<TblItemCart> TblItemCarts { get; set; }
        public virtual ICollection<TblItemMaster> TblItemMasterCreatedByNavigations { get; set; }
        public virtual ICollection<TblItemMaster> TblItemMasterModifiedByNavigations { get; set; }
        public virtual ICollection<TblItem> TblItems { get; set; }
    }
}
