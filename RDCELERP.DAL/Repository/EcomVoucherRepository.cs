using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.DAL.Repository
{
    public class EcomVoucherRepository : AbstractRepository<TblEcomVoucher>, IEcomVoucherRepository
    {
        public EcomVoucherRepository(Digi2l_DevContext dbContext)
      : base(dbContext)
        {

        }

    }
}