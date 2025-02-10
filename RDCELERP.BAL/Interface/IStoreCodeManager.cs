using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.StoreCode;

namespace RDCELERP.BAL.Interface
{
    public interface IStoreCodeManager
    {
        /// <summary>
        /// Method to manage (Add/Edit) StoreCode 
        /// </summary>
        /// <param name="StoreCodeVM">StoreCodeVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        int ManageStoreCode(StoreCodeViewModel StoreCodeVM, int userId);

        /// <summary>
        /// Method to get the StoreCode by id 
        /// </summary>
        /// <param name="id">StoreCodeId</param>
        /// <returns>StoreCodeViewModel</returns>
        StoreCodeViewModel GetStoreCodeById(int id);

        /// <summary>
        /// Method to delete StoreCode by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        bool DeletStoreCodeById(int id);

        /// <summary>
        /// Method to get the All StoreCode
        /// </summary>     
        /// <returns>List  StoreCodeViewModel</returns>

        IList<StoreCodeViewModel> GetAllStoreCode(object i, int roleId, int userId);
    }
}
