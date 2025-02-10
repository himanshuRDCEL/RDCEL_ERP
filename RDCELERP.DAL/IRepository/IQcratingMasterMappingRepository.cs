using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface IQcratingMasterMappingRepository : IAbstractRepository<TblQcratingMasterMapping>
    {
        public List<TblQcratingMasterMapping>? GetNewQueV2(int prodcatid,int prodtypeid, int prodtechid);
        public List<TblQcratingMasterMapping>? GetNewQueListV2(int? prodtypeid, int? prodtechid);
    }
}
