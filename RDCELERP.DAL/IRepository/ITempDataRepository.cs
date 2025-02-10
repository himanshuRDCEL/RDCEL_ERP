using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface ITempDataRepository : IAbstractRepository<TblTempDatum>
    {
        public TblTempDatum GetTempDataByFileName(string? filename);
        public List<TblTempDatum>? GetMediaFilesTempDataList(string? regdNo);
    }
}
