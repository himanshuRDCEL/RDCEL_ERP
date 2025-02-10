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
    public class SelfQCRepository : AbstractRepository<TblSelfQc>, ISelfQCRepository
    {
        Digi2l_DevContext _context;
        public SelfQCRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }

        public TblSelfQc GetSelfqcorder(string regdno)
        {
            TblSelfQc tblSelfQc = new TblSelfQc();
            string ext = "webm";
            if (regdno != null) 
            {
                tblSelfQc = _context.TblSelfQcs.FirstOrDefault(x => x.IsActive == true && x.RegdNo == regdno && x.ImageName.Contains(ext));
            }
            return tblSelfQc;
        }
        public List<TblSelfQc> GetSelfQCListByRegdNo(string regdno)
        {
            List<TblSelfQc> tblSelfQcList = new List<TblSelfQc>();
            if (string.IsNullOrEmpty(regdno))
            {
                tblSelfQcList = _context.TblSelfQcs.Where(x => x.IsActive == true && x.RegdNo == regdno).ToList();
            }
            return tblSelfQcList;
        }
    }
}
