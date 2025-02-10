using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Dashboards;

namespace RDCELERP.BAL.Interface
{
    public interface IUtcDashboardManager
    {
        public UtcDashboardViewModel GetUtcDashBoardDataFromDatabase();
    }
}
