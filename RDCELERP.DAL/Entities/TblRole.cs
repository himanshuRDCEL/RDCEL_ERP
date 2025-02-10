using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblRole
    {
        public TblRole()
        {
            TblRoleAccesses = new HashSet<TblRoleAccess>();
            TblUserRoles = new HashSet<TblUserRole>();
        }

        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public int? CompanyId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsRoleMultiBrand { get; set; }

        public virtual TblCompany? Company { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual ICollection<TblRoleAccess> TblRoleAccesses { get; set; }
        public virtual ICollection<TblUserRole> TblUserRoles { get; set; }
    }
}
