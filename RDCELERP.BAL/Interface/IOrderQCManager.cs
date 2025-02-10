using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;

namespace RDCELERP.BAL.Interface
{
    public interface IOrderQCManager
    {
        public TblOrderQc GetOrderDetails(int orderTransId);
    }
}
