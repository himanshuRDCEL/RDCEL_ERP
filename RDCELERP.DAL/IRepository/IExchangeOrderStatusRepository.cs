using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface IExchangeOrderStatusRepository : IAbstractRepository<TblExchangeOrderStatus>
    {
        TblExchangeOrderStatus GetByStatusId(int? id);
    }
}
