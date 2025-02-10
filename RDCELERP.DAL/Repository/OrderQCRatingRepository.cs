using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.DAL.Repository
{
    public class OrderQCRatingRepository : AbstractRepository<TblOrderQcrating>, IOrderQCRatingRepository
    {
        public OrderQCRatingRepository(Digi2l_DevContext dbContext)
        : base(dbContext)
        {
        }
    }
}
