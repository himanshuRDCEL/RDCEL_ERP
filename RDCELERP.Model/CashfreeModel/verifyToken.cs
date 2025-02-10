using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.CashfreeModel
{
  
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class verifyToken
    {
        public string? status { get; set; }
        public string? subCode { get; set; }
        public string? message { get; set; }
    }


}
