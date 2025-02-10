using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TimeSlotMaster
    {
        public int TimeSlotMasterid { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int SlotHour { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
