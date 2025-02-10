using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblAccessList
    {
        public TblAccessList()
        {
            InverseParentAccessList = new HashSet<TblAccessList>();
            TblRoleAccesses = new HashSet<TblRoleAccess>();
        }

        public int AccessListId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ActionName { get; set; }
        public string? ActionUrl { get; set; }
        public int? ParentAccessListId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? SetIcon { get; set; }
        public int? CompanyId { get; set; }
        public bool? IsMenu { get; set; }

        public virtual TblCompany? Company { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblAccessList? ParentAccessList { get; set; }
        public virtual ICollection<TblAccessList> InverseParentAccessList { get; set; }
        public virtual ICollection<TblRoleAccess> TblRoleAccesses { get; set; }
    }
}
