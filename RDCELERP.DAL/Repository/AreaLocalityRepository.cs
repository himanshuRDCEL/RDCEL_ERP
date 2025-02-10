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
    public class AreaLocalityRepository : AbstractRepository<TblAreaLocality>, IAreaLocalityRepository
    {
        Digi2l_DevContext _context;
        public AreaLocalityRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }

        public TblAreaLocality GetArealocalityById(int? ArealocalityId)
        {
            TblAreaLocality TblAreaLocality = null;
            
            TblAreaLocality = _context.TblAreaLocalities.FirstOrDefault(x => x.IsActive == true && x.AreaId == ArealocalityId);

            return TblAreaLocality;

        }

        public TblAreaLocality GetArealocalityByname(string Arealocality)
        {
            {
                TblAreaLocality TblAreaLocality = null;

                TblAreaLocality = _context.TblAreaLocalities.FirstOrDefault(x => x.IsActive == true && x.AreaLocality == Arealocality);

                return TblAreaLocality;

            }
        }
    }
}
