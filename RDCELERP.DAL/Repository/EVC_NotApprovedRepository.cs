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
    public class EVC_NotApprovedRepository:AbstractRepository<TblEvcregistration>, IEVC_NotApprovedRepository
    {
        public EVC_NotApprovedRepository(Digi2l_DevContext dbContext)
          : base(dbContext)
        {

        }
    
    }
}

