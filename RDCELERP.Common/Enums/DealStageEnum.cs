using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Enums
{
    public enum DealStageEnum
    {
        [Description("After The Sales")]
        After_The_Sales = 1,
        [Description("After The Approval")]
        After_The_Approval = 2,
        [Description("After The Installation")]
        After_The_Installation = 3,
        [Description("Closed Won")]
        Closed_Won = 4,
        [Description("Closed Lost To Complition")]
        Closed_Lost_To_Complition = 5

    }
}
