using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblNpssqresponse
    {
        public int NpssqresponseId { get; set; }
        public int? NpssquestionId { get; set; }
        public int? NpssqoptionId { get; set; }
        public int? OrderTransId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblNpssqoption? Npssqoption { get; set; }
        public virtual TblNpssquestion? Npssquestion { get; set; }
        public virtual TblOrderTran? OrderTrans { get; set; }
    }
}
