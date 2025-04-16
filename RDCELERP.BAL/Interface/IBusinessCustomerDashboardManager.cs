using RDCELERP.Model.BusinessCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.Interface
{
   public interface IBusinessCustomerDashboardManager
    {  /// <summary>
       /// method to get dashboard detail by id
       /// </summary>
       /// <param name="BusinessCustomerId"></param>
       /// <returns>DashboardViewModel</returns>
        public DashboardViewModel GetCustomerDashboardById(int BusinessCustomerId);
    }
}
