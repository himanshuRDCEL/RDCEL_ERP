using RDCELERP.Model.BusinessCustomer;
using RDCELERP.Model.Users;
using RDCELERP.Model.Zoho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.Interface
{
    public interface IBusinessCustomerManager
    {/// <summary>
     /// Method to get the customer object by login detail
     /// </summary>
     /// <param name="username">username</param>
     /// <param name="password">password</param>
     /// <returns>LoginViewModel</returns>
        public LoginViewModel GetCustomerByLogin(string username, string password);
             /// <summary>
        /// method to manager zoho customer
        /// </summary>
        /// <param name="bussinessCustomerVM"></param>
        /// <param name="bussinessCustomerId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public int ManageBusinessCustomer(BusinessCustomerViewModel bussinessCustomerViewModelVM);
    }
}
