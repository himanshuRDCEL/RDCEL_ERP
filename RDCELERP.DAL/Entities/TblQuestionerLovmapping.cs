using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblQuestionerLovmapping
    {
        public int QuestionerLovmappingId { get; set; }
        public int ProductCatId { get; set; }
        public int QuestionerLovid { get; set; }
        public int? ParentId { get; set; }
        public decimal? RatingWeightageLov { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblProductCategory ProductCat { get; set; } = null!;
        public virtual TblQuestionerLov QuestionerLov { get; set; } = null!;
    }
}
