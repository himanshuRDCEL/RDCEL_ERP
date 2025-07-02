using RDCELERP.Model.BusinessCustomer;
using RDCELERP.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.Interface
{
    public interface IItemMasterManager
    {

        /// <summary>
        /// method to manager item
        /// </summary>
        /// <param name="itemcartVM"></param>
        /// <returns></returns>
        public int ManageMasterItem(ItemMasterViewModel itemcartVM, int userid);

        /// <summary>
        /// Method to get the item by id 
        /// </summary>
        /// <param name="id">Item</param>
        /// <returns>ItemMasterViewModel</returns>
        ItemMasterViewModel GetMasterItemById(int id);
    }
}
