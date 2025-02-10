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
  public   class ExchangeABBStatusHistoryRepository : AbstractRepository<TblExchangeAbbstatusHistory>, IExchangeABBStatusHistoryRepository
    {
        Digi2l_DevContext _context;
        public ExchangeABBStatusHistoryRepository(Digi2l_DevContext dbContext)
           : base(dbContext)
        {
            _context = dbContext;
        }
        public TblExchangeAbbstatusHistory GetByRegdstatusno(string Regdno, int? Statusid)
        {
            List<TblExchangeAbbstatusHistory> TblExchangeAbbstatusHistory1 = null;
            TblExchangeAbbstatusHistory TblExchangeAbbstatusHistory = null;
            if (Regdno != null && Statusid != null)
            {
                TblExchangeAbbstatusHistory1 = _context.TblExchangeAbbstatusHistories.Where(x => x.IsActive == true && x.RegdNo == Regdno && x.StatusId == Statusid).ToList();
                TblExchangeAbbstatusHistory = TblExchangeAbbstatusHistory1.LastOrDefault();
            }
            return TblExchangeAbbstatusHistory;
        }

        public List<TblExchangeAbbstatusHistory> GetHistoryByRegdNo(string Regdno)
        {
            List<TblExchangeAbbstatusHistory> TblExchangeAbbstatusHistory = null;
            if (Regdno != null)
            {
                TblExchangeAbbstatusHistory = _context.TblExchangeAbbstatusHistories.Where(x => x.IsActive == true && x.RegdNo == Regdno).ToList();
            }
            return TblExchangeAbbstatusHistory;
        }
    }
}
