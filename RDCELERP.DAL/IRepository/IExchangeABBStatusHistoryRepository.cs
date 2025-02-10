using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
//using RDCELERP.DAL.Models;
//using TblEvcregistration = RDCELERP.DAL.Models.TblEvcregistration;

namespace RDCELERP.DAL.IRepository
{
   public interface IExchangeABBStatusHistoryRepository : IAbstractRepository<TblExchangeAbbstatusHistory>
    {
        TblExchangeAbbstatusHistory GetByRegdstatusno(string Regdno, int? Statusid);
        List<TblExchangeAbbstatusHistory> GetHistoryByRegdNo(string Regdno);
    }
}
