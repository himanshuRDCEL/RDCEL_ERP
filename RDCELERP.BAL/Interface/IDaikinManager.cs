using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.DaikinModel;

namespace RDCELERP.BAL.Interface
{
    public interface IDaikinManager
    {
        public DaikinAuth DaikinAuthCall();
        public string Rest_ResponseDaikinAuthCall(string url, Method method);
        public string PushOrderStatus(DaikinOrderStatus daikinOrderStatus, string AuthToken);
    }
}
