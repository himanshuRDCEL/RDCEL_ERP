using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblOrderQcrating
    {
        public int OrderQcratingId { get; set; }
        public int OrderTransId { get; set; }
        public int? ProductCatId { get; set; }
        public int? QcquestionId { get; set; }
        public int? Rating { get; set; }
        public decimal? CalculatedWeightage { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? QcComments { get; set; }
        public int? QuestionerLovid { get; set; }
        public int? DoneBy { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblOrderTran OrderTrans { get; set; } = null!;
        public virtual TblProductCategory? ProductCat { get; set; }
        public virtual TblQcratingMaster? Qcquestion { get; set; }
        public virtual TblQuestionerLov? QuestionerLov { get; set; }
    }
}
