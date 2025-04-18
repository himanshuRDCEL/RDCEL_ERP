using RDCELERP.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.EcomVoucher
{
   public class EcomPhoneSpecificsViewModel :BaseViewModel
    {
        public int EcomPhoneSpecificId { get; set; }
        public string? VoucherCode { get; set; }
        public string? Phoneno { get; set; }
        public int? EcomVoucherId { get; set; }
        public string? Voucherstatus { get; set; }
        public bool? IsUsed { get; set; }

    }
}
