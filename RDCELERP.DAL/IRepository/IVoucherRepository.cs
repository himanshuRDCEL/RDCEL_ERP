using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface IVoucherRepository : IAbstractRepository<TblVoucherVerfication>
    {
        TblVoucherVerfication GetVoucherDataByExchangeId(int exchnageId);
        List<TblVoucherVerfication> GetOrderDataforsingeldealer(string AssociateCode);
        TblVoucherVerfication GetVoucherDataByredemptionId(int redemptionId);

    }
}
