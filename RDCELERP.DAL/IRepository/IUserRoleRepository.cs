using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.DAL.IRepository
{
    public interface IUserRoleRepository : IAbstractRepository<TblUserRole>
    {
        public TblUserRole GetUserRoleByUserId(int? id);
        public List<TblUserRole> GetListofQCUser();


    }
}
