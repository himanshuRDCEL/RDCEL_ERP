using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.DAL.IRepository
{
    public interface IBookingItemRepository :IAbstractRepository<TblBookingItem>
    {
        public  Task<List<TblBookingItem>> GetAllBookingItemsByCustomerAsync(int? customerId);

        public  Task<TblBtoBpayment?> GetPaymentByOrderNoAsync(string orderNo);


        Task<List<TblBookingItem>> GetOrderItemsAsync(string orderNo);
        

    }
}
