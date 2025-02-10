using Microsoft.EntityFrameworkCore;
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
    public class OrderQCRepository : AbstractRepository<TblOrderQc>, IOrderQCRepository
    {
        Digi2l_DevContext _context;
        public OrderQCRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }

        public TblOrderQc GetOrderQcById(int id)
        {
            TblOrderQc TblOrderQc = null;

            if (id > 0)
            {
                TblOrderQc = _context.TblOrderQcs.FirstOrDefault(x => x.IsActive == true && x.OrderQcid == id);
            }

            return TblOrderQc;
        }
        public TblOrderQc GetQcorderBytransId(int ordertransid)
        {
            TblOrderQc TblOrderQc = null;

            if (ordertransid > 0)
            {
                TblOrderQc = _context.TblOrderQcs.FirstOrDefault(x => x.IsActive == true && x.OrderTransId == ordertransid);
            }

            return TblOrderQc;
        }
        public TblOrderTran GetOrderBytransId(int transid)
        {
            TblOrderTran TblOrderTran = null;

            if (transid > 0)
            {
                TblOrderTran = _context.TblOrderTrans.FirstOrDefault(x => x.IsActive == true && x.OrderTransId == transid);
            }

            return TblOrderTran;
        }
        public TblOrderTran GetBytransId(int? transid)
        {
            TblOrderTran TblOrderTran = null;

            if (transid != null)
            {
                TblOrderTran = _context.TblOrderTrans.FirstOrDefault(x => x.IsActive == true && x.OrderTransId == transid);
            }

            return TblOrderTran;
        }
        public int GetCountByStatusId(string companyName, int? statusId1=null,int? statusId2=null, int? resheduleCount=null)
        {
            int count = 0;
            count = _context.TblOrderQcs.Include(x=>x.OrderTrans).ThenInclude(x => x.Exchange)
                .Count(x => x.IsActive == true && x.OrderTrans != null && x.OrderTrans.Exchange != null
            && (companyName == null ? true : x.OrderTrans.Exchange.CompanyName == companyName)
            && ((statusId1 == null ? false: x.StatusId == statusId1)
            || (statusId2 == null ? false : x.StatusId == statusId2))
            && (resheduleCount == null ? true : x.Reschedulecount == resheduleCount)
            );
            return count;
        }
    }
}
