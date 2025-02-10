using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblVoucherStatus
    {
        public TblVoucherStatus()
        {
            TblAbbredemptions = new HashSet<TblAbbredemption>();
            TblExchangeOrders = new HashSet<TblExchangeOrder>();
            TblVoucherVerfications = new HashSet<TblVoucherVerfication>();
        }

        public int VoucherStatusId { get; set; }
        public string VoucherStatusName { get; set; } = null!;

        public virtual ICollection<TblAbbredemption> TblAbbredemptions { get; set; }
        public virtual ICollection<TblExchangeOrder> TblExchangeOrders { get; set; }
        public virtual ICollection<TblVoucherVerfication> TblVoucherVerfications { get; set; }
    }
}
