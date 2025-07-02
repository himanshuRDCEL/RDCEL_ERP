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
    public class BtoBpaymentRepository : AbstractRepository<TblBtoBpayment>, IBtoBpaymentRepository
    {
        Digi2l_DevContext _dbContext;
        public BtoBpaymentRepository(Digi2l_DevContext dbContext)
      : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TblBtoBpayment>> GetPaymentsByCustomerAsync(int customerId)
        {
            return await _dbContext.TblBtoBpayments
                .Where(p => p.CreatedBy == customerId && p.IsActive==true)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }
    }
}
