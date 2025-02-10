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
    public class TimeLineStatusMappingRepository : AbstractRepository<TblTimelineStatusMapping>, ITimeLineMappingStatusRepository
    {
        private readonly Digi2l_DevContext _db;

        public TimeLineStatusMappingRepository(Digi2l_DevContext dbContext)
        : base(dbContext)
        {
        }
    }
}
