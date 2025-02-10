using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblEntityType
    {
        public TblEntityType()
        {
            TblEvcregistrations = new HashSet<TblEvcregistration>();
            TblRefurbisherRegistrations = new HashSet<TblRefurbisherRegistration>();
        }

        public int EntityTypeId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual ICollection<TblEvcregistration> TblEvcregistrations { get; set; }
        public virtual ICollection<TblRefurbisherRegistration> TblRefurbisherRegistrations { get; set; }
    }
}
