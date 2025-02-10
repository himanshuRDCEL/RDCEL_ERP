using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.CashfreeModel
{
    public class RemoveBeneficiary
    {
        public string beneId {  get; set; } 
    }

    public class RemoveBeneficiaryResponse
    {
        public string? status { get; set; }
        public string? subCode { get; set; }
        public string? message { get; set; }
    }
}
