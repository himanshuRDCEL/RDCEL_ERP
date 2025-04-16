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
    public class BussinessTypeRepository :AbstractRepository<TblBusinessType>,IBussinessTypeRepository
    {
        public BussinessTypeRepository(Digi2l_DevContext dbContext)
            : base(dbContext)
        {

        }
    }
}
