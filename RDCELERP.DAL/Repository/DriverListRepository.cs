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
    public class DriverListRepository : AbstractRepository<TblDriverList>, IDriverListRepository
    {
        Digi2l_DevContext _context;
        public DriverListRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }

        public TblDriverList GetDriverlistById(int? driverId)
        {
            TblDriverList driverlist = new TblDriverList();
            if (driverId != null && driverId > 0)
            {
                driverlist = _context.TblDriverLists
                    .Include(x=>x.City)
                    .Where(x => x.IsActive == true && x.DriverId == driverId).FirstOrDefault();
            }
            return driverlist;
        }

    }
}
