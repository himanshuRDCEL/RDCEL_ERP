using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Enums
{
    public enum SettlementEnumForDashboard
    {
        [Description("Direct to customer")]
        deffred = 1,

        [Description("To dealer")]
        instant = 2,

        [Description("Paid")]
        Paid = 3,

        [Description("Not Paid")]
        NotPaid = 4,
    }
}
