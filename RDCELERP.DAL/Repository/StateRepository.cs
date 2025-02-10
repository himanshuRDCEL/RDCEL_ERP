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
    public class StateRepository : AbstractRepository<TblState>, IStateRepository
    {
        Digi2l_DevContext _context;
        public StateRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }

        public TblState GetStateById(int? stateid)
        {
            TblState TblState = null;
            if (stateid > 0)
            {
                TblState = _context.TblStates.FirstOrDefault(x => x.IsActive == true && x.StateId == stateid);
            }
            return TblState;
        }
    }
}
