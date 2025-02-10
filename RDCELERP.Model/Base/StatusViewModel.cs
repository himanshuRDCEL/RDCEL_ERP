using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Base
{
    [DataContract]
    [Serializable]
    public class StatusDataContract
    {

        public StatusDataContract()
        {

        }
        public StatusDataContract(bool status, string message)
        {
            Status = status;
            Message = message;
            Detail = new object();
        }

        public StatusDataContract(bool status, string message, object detail)
        {
            Status = status;
            Message = message;
            Detail = detail;
        }

        [DataMember]
        public bool Status { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public object Detail { get; set; }

    }
}
