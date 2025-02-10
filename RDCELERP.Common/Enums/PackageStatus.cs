using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Enums
{
    public enum PackageStatusEnum
    {
        [Description("Assigned to Transporter")]
        Assigned_To_Transporter = 1,
        [Description("Package Picked Up")]
        Package_Picked_Up = 2,
        [Description("Pacakge Delayed")]
        Pacakge_Delayed = 3,
        [Description("Package Canceled")]
        Package_Canceled = 4,
        [Description("Package Delivered")]
        Package_Delivered = 5
    }
}
