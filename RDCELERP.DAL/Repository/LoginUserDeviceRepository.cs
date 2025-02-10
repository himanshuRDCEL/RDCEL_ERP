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
    public class LoginUserDeviceRepository : AbstractRepository<MapLoginUserDevice>, IMapLoginUserDeviceRepository
    {
        Digi2l_DevContext _context;
        public LoginUserDeviceRepository(Digi2l_DevContext dbContext)
      : base(dbContext)
        {
            _context = dbContext;
        }

        public int UpdateDeviceId(int userId)
        {
            MapLoginUserDevice device = new MapLoginUserDevice();
            int result = 0;

            device = _context.MapLoginUserDevices.Where(x => x.IsActive == true && x.UserId == userId).FirstOrDefault();
            if(device != null)
            {
                device.UserDeviceId = null;
                device.ModifiedDate = DateTime.Now;
                _context.Update(device);
                _context.SaveChanges();

                result = 1;
            }

            return result;
        }
    }
}

