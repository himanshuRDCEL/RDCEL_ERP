using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface ILogisticsRepository : IAbstractRepository<TblLogistic>
    {
        public TblLogistic GetExchangeDetailsByRegdno(string Regdno);
        public TblWalletTransaction GetEvcDetailsByRegdno(string Regdno);
        public int GetDriverAssignOrderCount(int DriverDetailsId, int status,int LgcId);
        public TblLogistic GetExchangeDetailsByOrdertransId(int OrdertransId);
        public TblWalletTransaction GetEvcDetailsByOrdertranshId(int OrdertransId);
        public int UpdateLogiticStatus(TblLogistic tblLogistic);

        public TblLogistic GetAbbRedumptionDetailsByRegdno(string Regdno);
        public TblLogistic GetExchangeDetailsByOID(int OrdertransId);
        public List<TblLogistic> GetOrderListByStatus(int? status);
        public List<TblLogistic> GetOrderListBySPIdAndStatus(int? servicePartnerId, int? status1 = null, int? status2 = null);
    }
}
