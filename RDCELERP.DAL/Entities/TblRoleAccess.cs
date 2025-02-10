using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblRoleAccess
    {
        public int RoleAccessId { get; set; }
        public int? AccessListId { get; set; }
        public int? RoleId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? CanAdd { get; set; }
        public bool? CanView { get; set; }
        public bool? CanDelete { get; set; }
        public int? CompanyId { get; set; }

        public virtual TblAccessList? AccessList { get; set; }
        public virtual TblCompany? Company { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblRole? Role { get; set; }
    }
}
