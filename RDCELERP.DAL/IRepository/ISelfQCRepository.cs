using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface ISelfQCRepository : IAbstractRepository<TblSelfQc>
    {
        public TblSelfQc GetSelfqcorder(string regdno);
        public List<TblSelfQc> GetSelfQCListByRegdNo(string regdno);
    }
}
