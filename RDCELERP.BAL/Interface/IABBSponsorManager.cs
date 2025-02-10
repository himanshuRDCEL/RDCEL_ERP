using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.ABBDashBoardModel;

namespace RDCELERP.BAL.Interface
{
   public interface IABBSponsorManager
    {
        public ABBDashBoardCountModel GetOrderCounts(UserDetailsForABBDashBoard userDetailsDC);
    }
}
