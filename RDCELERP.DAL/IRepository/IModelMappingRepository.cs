using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface IModelMappingRepository : IAbstractRepository<TblModelMapping>
    {
        TblModelMapping GetbyModelnoid(int? Modelnoid,int? BUid, int? BPid);
        public TblModelMapping GetdefaultModelnoid(int? Modelnoid, int? BUid, int? BPid);

        bool InsertModelsForBusinessPartner(List<TblModelMapping> modelMappings);

    }

}
