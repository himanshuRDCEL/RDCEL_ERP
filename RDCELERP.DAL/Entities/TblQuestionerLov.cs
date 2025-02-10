using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblQuestionerLov
    {
        public TblQuestionerLov()
        {
            TblOrderQcratings = new HashSet<TblOrderQcrating>();
            TblQcratingMasters = new HashSet<TblQcratingMaster>();
            TblQuestionerLovmappings = new HashSet<TblQuestionerLovmapping>();
        }

        public int QuestionerLovid { get; set; }
        public string? QuestionerLovname { get; set; }
        public int? QuestionerLovparentId { get; set; }
        public decimal? QuestionerLovratingWeightage { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual ICollection<TblOrderQcrating> TblOrderQcratings { get; set; }
        public virtual ICollection<TblQcratingMaster> TblQcratingMasters { get; set; }
        public virtual ICollection<TblQuestionerLovmapping> TblQuestionerLovmappings { get; set; }
    }
}
