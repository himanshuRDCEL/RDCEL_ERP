using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblEvcwalletStatus
    {
        public int WalletStatusId { get; set; }
        public int? EvcregistrationId { get; set; }
        public string? RegdNo { get; set; }
        public int? OrderTransId { get; set; }
        public int? WalletTransStatusId { get; set; }
    }
}
