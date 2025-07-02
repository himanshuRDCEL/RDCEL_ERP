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
        /// method to add order in DB,Razorpay,Sync
        /// </summary>
        /// <param name="itemlistVM"></param>
        /// <param name="businessCustomerViewModel"></param>
        /// <param name="userid"></param>
        /// <returns></returns>

        public Task<string> AddBookingItem( List<ItemCartViewModel> itemlistVM, BusinessCustomerViewModel businessCustomerViewModel, int userid);
        /// <summary>
        /// method to get booking item detail with respect to customerId
        /// </summary>
        /// <param name="customerid"></param>
        /// <returns></returns>
        public List<BookingItemViewModel> GetBookingItemDetailByCustomerId(int customerid);

        /// <summary>
        /// method to update order status
        /// </summary>
        /// <param name="bookingitemVMs"></param>
        /// <returns></returns>
        public int ManageOrderStatus(UpdateBookingItemViewModel bookingitemVM);

        /// <summary>
        /// method to get all order by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public  Task<List<OrderHistoryViewModel>> GetOrderHistoryAsync(int customerId);

        /// <summary>
        /// method to get order detail by order no
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public Task<AdminOrderDetailViewModel> GetOrderDetailsAsync(string orderNo);


    }
}
