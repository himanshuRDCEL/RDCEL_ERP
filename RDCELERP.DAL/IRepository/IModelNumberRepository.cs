using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface IModelNumberRepository : IAbstractRepository<TblModelNumber>
    {
        public int UpdateModelNumber(int? modelNumberId, decimal? DtoC, decimal? DtoD, int? BusinessUnitId, int? userid);

        (List<TblModelNumber>, List<int>) GetListOfModelNumbersForBU(int? Buid);
    }
}
