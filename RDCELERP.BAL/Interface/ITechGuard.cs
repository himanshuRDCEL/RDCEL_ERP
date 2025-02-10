using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.TechGuard;

namespace RDCELERP.BAL.Interface
{
    public interface ITechGuard
    {
        public RestResponse TechGuardServiceCall(string url, Method methodType, object content = null, int? loggedInUserId=null);

        public TechGuardResponse PaymentResponse(TechGuardRequestModel techGuardDC, int? loggedInUserId);

        
    }
}
