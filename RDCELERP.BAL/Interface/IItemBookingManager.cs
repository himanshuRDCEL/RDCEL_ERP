using RDCELERP.Model.BusinessCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.Interface
{
    public interface IItemBookingManager
    {
        /// <summary>
        /// method to add booking item
        /// </summary>
        /// <param name="ItemlistVM"></param>
        /// <returns></returns>
        public Task<bool> AddBookingItem(List<ItemViewModel> itemlistVM, int userid);
    }
}
