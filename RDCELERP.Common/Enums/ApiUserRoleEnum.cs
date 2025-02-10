using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Enums
{
    public enum ApiUserRoleEnum
    {
        [Description("Service Partner")]
        Service_Partner = 1,
        [Description("Service Partner Driver")]
        Service_Partner_Driver = 2,
        [Description("D2C")]
        D2C = 3,
        [Description("LGC Admin")]
        LGC_Admin = 4,
    }
}
