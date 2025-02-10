using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblSociety
    {
        public TblSociety()
        {
            TblExchangeOrders = new HashSet<TblExchangeOrder>();
        }

        public int SocietyId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? QrcodeUrl { get; set; }
        public string? LogoName { get; set; }
        public string? ContactPersonFirstName { get; set; }
        public string? ContactPersonLastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Pincode { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int? LoginId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? BusinessUnitId { get; set; }

        public virtual TblBusinessUnit? BusinessUnit { get; set; }
        public virtual Login? Login { get; set; }
        public virtual ICollection<TblExchangeOrder> TblExchangeOrders { get; set; }
    }
}
