using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.Interface
{
    public interface IWhatsappNotificationManager
    {
        public RestResponse Rest_InvokeWhatsappserviceCall(string url, Method methodType, object content = null);
    }
}
