using System.Net;
using System.Runtime.Serialization;

namespace RDCELERP.Model.InfoMessage
{
    /// <summary>
    /// Represents information or success message in response object
    /// </summary>
    public class InfoMessage
    {

        public InfoMessage(bool status, string message)
        {
            Status = status;
            Message = message;
            Data = new object();
        }

        public InfoMessage(bool status, string message, object data)
        {
            Status = status;
            Message = message;
            Data = data;
        }

        [DataMember]
        public bool Status { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public object Data { get; set; }
        
    }
}
