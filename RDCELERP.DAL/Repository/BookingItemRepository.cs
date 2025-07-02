using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.DAL.Repository
{
    public class BookingItemRepository :AbstractRepository<TblBookingItem>, IBookingItemRepository
    {
        private readonly Digi2l_DevContext _db;
        public BookingItemRepository(Digi2l_DevContext dbContext)
       : base(dbContext)
        {
            _db = dbContext;
        }

        public async Task<List<TblBookingItem>> GetAllBookingItemsByCustomerAsync(int? customerId)
        {
            return await _db.TblBookingItems.Include(x => x.Item) 
                                 .Where(x => x.CustomerId == customerId && x.IsActive==true)
                                 .OrderByDescending(x => x.CreatedDate)
                                 .ToListAsync();
        }

        public async Task<TblBtoBpayment?> GetPaymentByOrderNoAsync(string orderNo)
        {
            return await _db.TblBtoBpayments
                                  .FirstOrDefaultAsync(x => x.OrderNo == orderNo);
        }
        public async Task<List<TblBookingItem>> GetOrderItemsAsync(string orderNo)
        {
            return await _db.TblBookingItems
                .Include(x => x.Item)
                .Include(x => x.Customer)
                .Where(x => x.OrderNo == orderNo && x.IsActive == true)
                .ToListAsync();
        }


    }
}
