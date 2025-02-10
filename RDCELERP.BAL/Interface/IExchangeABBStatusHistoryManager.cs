using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Master;

namespace RDCELERP.BAL.Interface
{
  public   interface IExchangeABBStatusHistoryManager
    {
        /// <summary>
        /// Method to manage (Add/Edit) ExchangeABBStatusHistory 
        /// </summary>
        /// <param name="ExchangeABBStatusHistoryVM">ExchangeABBStatusHistoryVM</param>
        /// <param name="userId">userId</param>
        int ManageExchangeABBStatusHistory(ExchangeABBStatusHistoryViewModel ExchangeABBStatusHistoryVM, int userId = 3);
    }
}
