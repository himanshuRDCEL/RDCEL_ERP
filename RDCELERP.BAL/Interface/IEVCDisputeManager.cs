using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.EVC;
using RDCELERP.Model.EVCdispute;

namespace RDCELERP.BAL.Interface
{
    public interface IEVCDisputeManager
    {
         int SaveEVCDisputeDetails(EVCDisputeViewModel EVCDisputeViewModels, int UserId);
        int SaveEVCDisputeDetailsForAdmin(EVCDisputeViewModel EVCDisputeViewModels, int UserId);

        public EVCDisputeViewModel GetDisputeByEVCDisputeId(int id);

    }
}
