using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model;
using RDCELERP.Model.State;

namespace RDCELERP.BAL.Interface
{
    public interface IEntityManager
    {
        IList<EntityViewModel> GetAllEntity();
        
    }
}
