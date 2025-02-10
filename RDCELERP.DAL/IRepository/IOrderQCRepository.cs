using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RDCELERP.DAL.IRepository
{
   public  interface IOrderQCRepository : IAbstractRepository<TblOrderQc>
    {
        TblOrderQc GetOrderQcById(int id);
        TblOrderTran GetOrderBytransId(int transid);
        TblOrderTran GetBytransId(int? transid);
        TblOrderQc GetQcorderBytransId(int ordertransid);
        int GetCountByStatusId(string companyName, int? statusId1 = null, int? statusId2 = null, int? resheduleCount = null);
    }
}
