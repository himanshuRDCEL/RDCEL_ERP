using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Constant
{
    public class RoleConstant
    {
        public const string RoleSuperAdmin = "Super Admin";
        public const string RoleEVPPortal = "EVC Portal";
        public const string RoleABBExch = "ABB & Exchange";
        public const string RoleQCLGC = "QC & LGC";
        public const string RoleQC = "QC";
        public const string RoleEVCAdmin = "EVC Admin";
        public const string RoleSponsorAdmin = "Sponsor Admin";
        public const string RoleDealerAdmin = "Dealer Admin";
        public const string RoleServicePartner = "Service Partner";
    }
    public class PortalNameConstant
    {
        public const string LGCPortal = "LGC Portal";
        public const string EVCPortal = "EVC Portal";
        public const string QCPortal = "QC Portal";
        public const string EVCAdminPortal = "EVC Admin";
        public const string UTCAdminPortal = "UTC Portal";
        public const string SponsorAdminPortal = "Sponsor Portal";
        public const string DealerAdminPortal = "Dealer Portal";
    }
    public class PortalLinkConstant
    {
        public const string LGCPortalLink = "/LGC/LogiPickDrop";
        public const string EVCPortalLink = "/EVC_Portal/EVC_Dashboard";
        public const string QCPortalLink = "/QCPortal/QCDashboard";
        public const string EVCAdminPortalLink = "/EVC/Index";
        public const string UTCAdminPortalLink = "/UTCDashboard";
        public const string SponsorAdminPortalLink = "/DealerDashBoard/CompanyDashBoard";
        public const string DealerAdminPortalLink = "/DealerDashBoard/DealerDashBoard";
    }
}
