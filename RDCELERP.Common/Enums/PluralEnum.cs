using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Enums
{
    public enum PluralEnum
    {
        [Description("CAPTURED")]
        PaymentStatus = 1,
       [Description("CHARGED")]
        OrderStatus = 2,
    }
}
