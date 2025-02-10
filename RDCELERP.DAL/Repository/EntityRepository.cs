using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.DAL.Repository
{
    public class EntityRepository : AbstractRepository<TblEntityType>, IEntityRepository
    {
        public EntityRepository(Digi2l_DevContext dbContext)
       : base(dbContext)
        {

        }
    }
}
