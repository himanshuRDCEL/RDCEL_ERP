using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblCompany
    {
        public TblCompany()
        {
            TblAccessLists = new HashSet<TblAccessList>();
            TblAddresses = new HashSet<TblAddress>();
            TblRoleAccesses = new HashSet<TblRoleAccess>();
            TblRoles = new HashSet<TblRole>();
            TblUserRoles = new HashSet<TblUserRole>();
            TblUsers = new HashSet<TblUser>();
        }

        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? RegistrtionNumber { get; set; }
        public string? CompanyLogoUrl { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? WebSite { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? BusinessUnitId { get; set; }

        public virtual TblBusinessUnit? BusinessUnit { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual ICollection<TblAccessList> TblAccessLists { get; set; }
        public virtual ICollection<TblAddress> TblAddresses { get; set; }
        public virtual ICollection<TblRoleAccess> TblRoleAccesses { get; set; }
        public virtual ICollection<TblRole> TblRoles { get; set; }
        public virtual ICollection<TblUserRole> TblUserRoles { get; set; }
        public virtual ICollection<TblUser> TblUsers { get; set; }
    }
}
