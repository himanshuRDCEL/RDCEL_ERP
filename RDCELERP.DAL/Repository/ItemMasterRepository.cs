using Microsoft.EntityFrameworkCore;
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
    public class ItemMasterRepository : AbstractRepository<TblItemMaster>, IItemMasterRepository
    {
        private readonly Digi2l_DevContext _dbContext;

        public ItemMasterRepository(Digi2l_DevContext dbContext)
      : base(dbContext)
    {
            _dbContext=dbContext;
    }
        public IQueryable<TblItemMaster> Query()
        {
            return _dbContext.Set<TblItemMaster>().AsQueryable();
        }

    }
}
