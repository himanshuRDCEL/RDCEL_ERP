using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblLogistic
    {
        public TblLogistic()
        {
            TblOrderLgcs = new HashSet<TblOrderLgc>();
        }

        public int LogisticId { get; set; }
        public string RegdNo { get; set; } = null!;
        public string TicketNumber { get; set; } = null!;
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Modifiedby { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ServicePartnerId { get; set; }
        public int? StatusId { get; set; }
        public int? OrderTransId { get; set; }
        public decimal? AmtPaybleThroughLgc { get; set; }
        public int? DriverDetailsId { get; set; }
        public bool? IsOrderAcceptedByDriver { get; set; }
        public bool? IsOrderRejectedByDriver { get; set; }
        public string? RescheduleComment { get; set; }
        public DateTime? Rescheduledate { get; set; }
        public int? RescheduleCount { get; set; }
        public DateTime? PickupScheduleDate { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblDriverDetail? DriverDetails { get; set; }
        public virtual TblUser? ModifiedbyNavigation { get; set; }
        public virtual TblOrderTran? OrderTrans { get; set; }
        public virtual TblServicePartner? ServicePartner { get; set; }
        public virtual TblExchangeOrderStatus? Status { get; set; }
        public virtual ICollection<TblOrderLgc> TblOrderLgcs { get; set; }
    }
}
