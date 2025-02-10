using System;
using System.Collections.Generic;

#nullable disable

namespace RDCELERP.DAL.Entities
{
    public partial class TblPushNotificationSavedDeatil
    {
        public int SavedDetailsId { get; set; }
        public int? SentUserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
