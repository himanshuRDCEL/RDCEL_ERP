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

        public  Task<bool> UpsertItemsAsync(IEnumerable<ItemViewModel> items);
        /// <summary>
        /// method to get item by item code
        /// </summary>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        public ItemViewModel GetItemByItemCode(string itemcode);
        /// <summary>
        /// method to sync item stock and master item
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Task SyncStockReportDataAsync(int userid);



    }
}
