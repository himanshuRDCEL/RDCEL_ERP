using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblPushNotificationSavedDetail
    {
        public int SavedDetailsId { get; set; }
        public int? SentUserId { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
    }
}
