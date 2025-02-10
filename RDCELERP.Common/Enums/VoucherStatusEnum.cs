using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.Enum
{
    public enum VoucherStatusEnum
    {
        [Description("Captured")]
        Captured = 2,

        [Description("Redeemed")]
        Redeemed = 3,

        [Description("Discount")]
        Discount = 1,
    }

    public enum VoucherTypeEnum
    {
        [Description("Cash")]
        Cash = 2,

        [Description("Discount")]
        Discount = 1,

        [Description("NoVoucher")]
        NoVoucher = 0,
    }
}
