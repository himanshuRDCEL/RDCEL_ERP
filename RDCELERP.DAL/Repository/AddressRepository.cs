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
   public class AddressRepository : AbstractRepository<TblAddress>, IAddressRepository
    {
        public AddressRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
        }
    }
}
