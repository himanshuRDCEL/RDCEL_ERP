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
    public class LoginRepository : AbstractRepository<Login>, ILoginRepository
    {
        private readonly Digi2l_DevContext _context;
        public LoginRepository(Digi2l_DevContext dbContext)
          : base(dbContext)
        {
            _context = dbContext;
        }

        #region Get BU Api Login Details by Username
        /// <summary>
        /// Get BU Api Login Details by Username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Login? GetBULoginByUsername(string? username)
        {
            Login? login = null;
            if (!string.IsNullOrEmpty(username))
            {
                login = _context.Logins.Where(x => (x.Username ?? "").ToLower().Equals(username.ToLower())).FirstOrDefault();
            }
            return login;
        }
        #endregion
    }
}