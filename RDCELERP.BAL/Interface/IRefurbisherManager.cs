using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.BusinessUnit;
using RDCELERP.Model.Company;
using RDCELERP.Model.Refurbisher;

namespace RDCELERP.BAL.Interface
{
  public  interface IRefurbisherManager
    {
        /// <summary>
        /// Method to manage (Add/Edit) Refurbisher 
        /// </summary>
        /// <param name="RefurbisherViewModel">RefurbisherViewModel</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        int ManageRefurbisher(RefurbisherRegViewModel RefurbisherViewModel, int userId);

        /// <summary>
        /// Method to get the Refurbisher by id 
        /// </summary>
        /// <param name="id">Refurbisher id</param>
        /// <returns>CompanyViewModel</returns>
        RefurbisherRegViewModel GetRefurbisherById(int RefurbisherId);

        /// <summary>
        /// Method to delete Refurbisher by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        bool DeletRefurbisherById(int RefurbisherId);
    }
}
