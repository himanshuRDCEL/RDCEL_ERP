using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.DAL.Repository
{
    public class ExchangeOrderStatusRepository : AbstractRepository<TblExchangeOrderStatus>, IExchangeOrderStatusRepository
    {
        Digi2l_DevContext _context;
        public ExchangeOrderStatusRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }

        public TblExchangeOrderStatus GetByStatusId(int? id)
        {
            TblExchangeOrderStatus TblExchangeOrderStatus = _context.TblExchangeOrderStatuses.FirstOrDefault(x => x.Id == id);

            return TblExchangeOrderStatus;
        }
    }
}
