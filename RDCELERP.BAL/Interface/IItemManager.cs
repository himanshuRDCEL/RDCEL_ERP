using RDCELERP.Model.BusinessCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.Interface
{
    
     public interface IItemManager
    {
        /// <summary>
     /// Method to get Item list
     /// </summary>
     /// <returns>List<ItemViewModel></returns>
        public List<ItemViewModel> GetItemList();

    }
}
