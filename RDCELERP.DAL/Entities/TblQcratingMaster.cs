using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblQcratingMaster
    {
        public TblQcratingMaster()
        {
            TblOrderQcratings = new HashSet<TblOrderQcrating>();
        }

        public int QcratingId { get; set; }
        public int? ProductCatId { get; set; }
        public string? Qcquestion { get; set; }
        public int? RatingWeightage { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? QuestionerLovid { get; set; }
        public string? QuestionsImage { get; set; }
        public bool? IsAgeingQues { get; set; }
        public bool? IsDecidingQues { get; set; }
        public bool? IsDiagnoseV2 { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblQuestionerLov? QuestionerLov { get; set; }
        public virtual ICollection<TblOrderQcrating> TblOrderQcratings { get; set; }
    }
}
