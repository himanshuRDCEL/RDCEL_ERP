using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Enums
{
    public enum RoleEnum
    {
        [Description("Super Admin")]
        SuperAdmin = 1,
        [Description("Admin")]
        Admin = 2,
        [Description("DPIA Author")]
        DPIAAuthor = 3,
        [Description("Approver")]
        Approver = 4,
        [Description("Risk Ownery")]
        RiskOwner = 5,
        [Description("Sponsor Admin")]
        SponsorAdmin = 6,
        [Description("Dealer Admin")]
        DealerAdmin = 7,
        [Description("LGC")]
        LGC = 8,
        [Description("EVC Admin")]
        EVCAdmin = 9,
        [Description("EVC")]
        EVC = 10,
        [Description("EVC Portal and LGC Admin")]
        EVCLGC = 11,
        [Description("Service Partner")]
        ServicePartner = 12,
        [Description("QC Admin")]
        QcAdmin = 13,
        [Description("QC")]
        QC = 14,
    }
}
