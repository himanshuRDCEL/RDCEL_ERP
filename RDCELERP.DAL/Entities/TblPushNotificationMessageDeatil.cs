using System;
using System.Collections.Generic;

#nullable disable

namespace RDCELERP.DAL.Entities
{
    public partial class TblPushNotificationMessageDeatil
    {
        public int MessageDetailsId { get; set; }
        public string MessageType { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string UserType { get; set; }
        public bool? IsAndroid { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
