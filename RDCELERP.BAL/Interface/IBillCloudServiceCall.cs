using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.Interface
{
    public interface IBillCloudServiceCall
    {
        public RestResponse Rest_InvokeZohoInvoiceServiceForPlainText(string url, Method methodType, object content);
    }
}
