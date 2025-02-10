using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface IApiCallsRepository : IAbstractRepository<TblApicall>
    {
        //public TblAreaLocality GetArealocalityById(int? ArealocalityId);
        public void SaveApicall(TblApicall apicall);
    }
}
