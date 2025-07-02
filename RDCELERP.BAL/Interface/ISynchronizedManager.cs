using RDCELERP.Model.BusinessCustomer;
using RDCELERP.Model.SynchronizedModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.Interface
{
    public interface ISynchronizedManager
    {

        /// <summary>
        /// method to get master item by item code
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        
        public Task<LstItemDetail> GetItemDetailsByItemCodeFromApiAsync(string itemDesc, string projectCode, string whCode);

       
        /// <summary>
        /// method to get itemdetail stock list
        /// </summary>
        /// <returns></returns>
        public Task<List<LstStockReport>> GetStockDataAsync(string itemcode, string ean);

        public  Task<string> ImportOrdersToWmsAsync(List<ItemCartViewModel> itemlistVM, int userid, string orderNo);
    }
}
