using RDCELERP.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.BusinessCustomer
{
    public class BusinessCustomerViewModel :BaseViewModel
    {
        public int BusinessCustomerId { get; set; }
        public int? ZohoId { get; set; }
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
        public string? UnEncPassword { get; set; }
        public string? OtherZip { get; set; }
        public string? ShippingZip { get; set; }
        public string? ShippingStreet { get; set; }
        public string? ShippingCity { get; set; }
        public string? ShippingCountry { get; set; }
        public string? ShippingState { get; set; }
        public string? ImageName {  get; set; }
    }
}
