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
    public interface ILovRepository : IAbstractRepository<TblLoV>
    {
        #region Get LoV by Id
        /// <summary>
        /// Get LoV by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TblLoV? GetById(int? id);
        #endregion
    }
}
