using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface IWalletTransactionRepository : IAbstractRepository<TblWalletTransaction>
    {
        TblWalletTransaction GetSingleEvcDetails(int Id);
        public int UpdateWalletTransRecordStatus(int transid, int? status, int? userid,int Ordertype);
        public int UpdateWalletTransStatus(int transid, int? status, int? userid);
        public TblWalletTransaction GetdatainOrdertransh(int Ordertransid);


    }
}
