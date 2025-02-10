using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblNpssquestion
    {
        public TblNpssquestion()
        {
            TblNpssqoptions = new HashSet<TblNpssqoption>();
            TblNpssqresponses = new HashSet<TblNpssqresponse>();
        }

        public int NpssquestionId { get; set; }
        public string? Question { get; set; }
        public int? ParentQuestionId { get; set; }
        public int? StepId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? RatingType { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual ICollection<TblNpssqoption> TblNpssqoptions { get; set; }
        public virtual ICollection<TblNpssqresponse> TblNpssqresponses { get; set; }
    }
}
