using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblFeedBackAnswer
    {
        public TblFeedBackAnswer()
        {
            TblFeedBacks = new HashSet<TblFeedBack>();
        }

        public int Id { get; set; }
        public string? Answers { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ParentId { get; set; }
        public string? ModifiedBy { get; set; }

        public virtual TblFeedBackQuestion? Parent { get; set; }
        public virtual ICollection<TblFeedBack> TblFeedBacks { get; set; }
    }
}
