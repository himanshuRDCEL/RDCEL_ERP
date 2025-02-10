using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblNpssqoption
    {
        public TblNpssqoption()
        {
            TblNpssqresponses = new HashSet<TblNpssqresponse>();
        }

        public int NpssqoptionId { get; set; }
        public string? Option { get; set; }
        public int? NpssquestionId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblNpssquestion? Npssquestion { get; set; }
        public virtual ICollection<TblNpssqresponse> TblNpssqresponses { get; set; }
    }
}
