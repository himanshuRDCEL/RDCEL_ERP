using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface IMapLoginUserDeviceRepository : IAbstractRepository<MapLoginUserDevice>
    {
        public int UpdateDeviceId(int userId);
    }
}
