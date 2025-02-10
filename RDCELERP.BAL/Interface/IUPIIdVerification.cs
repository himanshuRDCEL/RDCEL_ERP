using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.CashfreeModel;
using RDCELERP.Model.UPIVerificationModel;

namespace RDCELERP.BAL.Interface
{
    public interface IUPIIdVerification
    {
        public string Rest_ResponseUPIIdverificationCall(string url, Method methodType, string Authtoken);
        public UPIVerification CheckUpiId(string regdno, string UPIId,string AuthToken);
    }
}
