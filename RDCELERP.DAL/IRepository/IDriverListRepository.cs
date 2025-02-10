using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.Repository;

namespace RDCELERP.DAL.IRepository
{
    public interface IDriverListRepository: IAbstractRepository <TblDriverList>
    {
        public TblDriverList GetDriverlistById(int? driverId);
        
    }
}
