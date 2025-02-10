using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.City;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.State;

namespace RDCELERP.BAL.Interface
{
    public interface IProductTechnologyManager
    {
        public ResponseResult ProductTechnologybycatid(int catid);
        public ResponseResult ProductTechnologybycatidv2(int catid);

    }
}
