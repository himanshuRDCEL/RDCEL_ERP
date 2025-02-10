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
    public class LovRepository : AbstractRepository<TblLoV>, ILovRepository
    {
        private Digi2l_DevContext _context;
        public LovRepository(Digi2l_DevContext dbContext)
        : base(dbContext)
        {
            _context = dbContext;
        }
        #region Get LoV by Id
        /// <summary>
        /// Get LoV by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TblLoV? GetById(int? id)
        {
            TblLoV? tblLoV = null;
            if (id > 0)
            {
                tblLoV = _context.TblLoVs.Where(x=> x.IsActive == true && x.LoVid == id).FirstOrDefault();
            }
            return tblLoV;
        }
        #endregion
    }
}
