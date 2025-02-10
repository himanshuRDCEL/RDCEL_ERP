using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.DAL.Repository
{
    public class EVCWalletHistoryRepository : AbstractRepository<TblEvcwalletHistory>, IEVCWalletHistoryRepository
    {
        public EVCWalletHistoryRepository(Digi2l_DevContext dbContext)
          : base(dbContext)
        {

        }
    }
}
