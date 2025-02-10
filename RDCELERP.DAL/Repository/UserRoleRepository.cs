using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace RDCELERP.DAL.Repository
{
    public class UserRoleRepository : AbstractRepository<TblUserRole>, IUserRoleRepository
    {
        private readonly Digi2l_DevContext _db;
        public UserRoleRepository(Digi2l_DevContext dbContext)
       : base(dbContext)
        {
            _db = dbContext;
        }


        public TblUserRole GetUserRoleByUserId(int? id)
        {
            TblUserRole TblUser = _db.TblUserRoles.Where(x=>x.IsActive==true && x.UserId==id).Include(x=>x.Role)
                .Include(x=>x.User).FirstOrDefault();

            return TblUser;
        }

        public List<TblUserRole> GetListofQCUser()
        {
            string rolename = "QC";
            List<TblUserRole> TblUser = _db.TblUserRoles
     .Include(x => x.Role)
     .Include(x => x.User)
     .Where(x => x.IsActive==true && x.UserId > 0 && x.RoleId > 0 && x.Role.RoleName == rolename)
     .ToList();

            return TblUser;
        }
    }
}
