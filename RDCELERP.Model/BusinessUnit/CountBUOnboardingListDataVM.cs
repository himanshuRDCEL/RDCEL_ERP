using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.ABBPriceMaster;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.Company;
using RDCELERP.Model.PriceMaster;

namespace RDCELERP.Model.BusinessUnit
{
    public class CountBUOnboardingListDataVM 
    {
        public int? BPListCount { get; set; }
        public int? ExchPriceMasterListCount { get; set; }
        public int? ABBPlanMasterListCount { get; set; }
        public int? ABBPriceMasterListCount { get; set; }
    }
}
