using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Enums
{
    public enum BUConfigKeyEnum
    {
        //Business unit configuration key enum
        [Description("IsBillingPartial")]
        IsBillingPartial = 1,
        [Description("IsDebitNoteSkiped")]
        IsDebitNoteSkiped = 2,
        [Description("IsPickupSkiped")]
        IsPickupSkiped = 3,
        
    }
}
