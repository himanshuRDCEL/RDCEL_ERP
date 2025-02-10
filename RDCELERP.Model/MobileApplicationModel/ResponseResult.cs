using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.MobileApplicationModel
{
    public class ResponseResult
    {
        public bool Status { get; set; }
        public HttpStatusCode Status_Code { get; set; }
        public string message { get; set; }
        public Object Data { get; set; }
    }

    public class ResultResponse
    {
        public Object token { get; set; }
        public bool Status { get; set; }
        public HttpStatusCode Status_Code { get; set; }
        public string message { get; set; }
        public Object Data { get; set; }
    }
}
