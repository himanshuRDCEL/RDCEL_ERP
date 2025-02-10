using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblFeedBackQuestion
    {
        public TblFeedBackQuestion()
        {
            TblFeedBackAnswers = new HashSet<TblFeedBackAnswer>();
            TblFeedBacks = new HashSet<TblFeedBack>();
        }

        public int Id { get; set; }
        public string? Question { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public virtual ICollection<TblFeedBackAnswer> TblFeedBackAnswers { get; set; }
        public virtual ICollection<TblFeedBack> TblFeedBacks { get; set; }
    }
}
