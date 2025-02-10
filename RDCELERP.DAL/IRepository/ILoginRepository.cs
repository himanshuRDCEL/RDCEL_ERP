using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface ILoginRepository : IAbstractRepository<Login>
    {
        #region Get BU Api Login Details by Username
        /// <summary>
        /// Get BU Api Login Details by Username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Login? GetBULoginByUsername(string? username);
        #endregion
    }
}
