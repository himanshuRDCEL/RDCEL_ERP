using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.DAL.IRepository
{
    public interface IErrorLogRepository : IAbstractRepository<TblErrorLog>
    {
        // public void WriteError(string Source, string Code, Exception ex = null);
        public void WriteErrorToDB(string Source, string Code, Exception ex = null);
    }
}
