using RDCELERP.Model.BusinessCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.Interface
{
    public interface IItemCartManager
    {
        /// <summary>
        /// method to get cart items
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<ItemCartViewModel> GetItemCartList(int userid);

        /// <summary>
        /// method to add Cart item
        /// </summary>
        /// <param name="itemcartVM"></param>
        /// <returns></returns>
        public int ManageItemCart(ItemCartViewModel itemcartVM, int userid);

        /// <summary>
        /// remove item from cart
        /// </summary>
        /// <param name="itemCartId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ResponseCartVM RemoveItem(RemoveItemVM removeItemVM, int userid);


        /// <summary>
        /// check item qnty
        /// </summary>
        /// <param name="itemid"></param>
        /// <param name="qnty"></param>
        /// <returns></returns>
        public Task<bool> CheckItemQnty(int itemid, int qnty);

        /// <summary>
        /// method to update cart item qnty
        /// </summary>
        /// <param name="quantityVM"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Task<ResponseCartVM> ManageItemQuantity(QuantityVM quantityVM, int userid);

        public bool UpdateCartPaymentStatus(int businessCustomerId);

    }
}
