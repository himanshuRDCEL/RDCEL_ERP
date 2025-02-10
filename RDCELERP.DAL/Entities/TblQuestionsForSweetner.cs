using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblQuestionsForSweetner
    {
        public TblQuestionsForSweetner()
        {
            TblBubasedSweetnerValidations = new HashSet<TblBubasedSweetnerValidation>();
        }

        public int QuestionId { get; set; }
        public string? Question { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? QuestionKey { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual ICollection<TblBubasedSweetnerValidation> TblBubasedSweetnerValidations { get; set; }
    }
}
