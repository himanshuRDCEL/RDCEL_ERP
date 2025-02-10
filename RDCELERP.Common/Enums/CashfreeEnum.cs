using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Enums
{
    public enum CashfreeEnum
    {
        [Description("Succcess")]
        Succcess = 200,

        [Description("upi")]
        upi = 1,

        [Description("Exchange")]
        Exchange = 2,

        [Description("Dr")]
        TransactionType = 3,

        [Description("ABB")]
        ABB = 4,
    }
}
