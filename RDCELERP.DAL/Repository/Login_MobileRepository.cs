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
    public class Login_MobileRepository : AbstractRepository<TblLoginMobile>, ILogin_MobileRepository
    {
        public Login_MobileRepository(Digi2l_DevContext dbContext)
          : base(dbContext)
        {
        }
    }
}
