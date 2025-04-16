using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.DAL.IRepository
{
public interface IBussinessCustomerRepository 
        :IAbstractRepository<TblBusinessCustomer>
    {
        /// <summary>
        /// method to get dashboard data by customerid
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public DataTable GetCustomerDashboardById(int customerId);
    }
}
