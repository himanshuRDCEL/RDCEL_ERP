using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Enums
{
    public enum EcomVoucherTypeEnum
    {
        [Description("Generic")]
        GenericVoucher = 1,
        [Description("RD-Brand-Specific")]
        BrandSpecificVoucher = 2,
        [Description("RD-Brand-Phone-Specific")]
        PhoneSpecificVoucher = 3

    }

    public enum EcomVoucherStatusEnum
    {
       
        Generated = 1,

        Captured = 2,

        Redeemed = 3,

        Setteled = 4
    }

    public enum EcomVoucherValueTypeEnum
    {
        [Description("Fixed")]
        Fixed = 1,
        [Description("Percentage")]
        Percentage = 2

    }

}
