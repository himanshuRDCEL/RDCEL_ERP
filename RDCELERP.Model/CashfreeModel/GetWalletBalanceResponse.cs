using Microsoft.AspNetCore.Server.HttpSys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.CashfreeModel
{
    public class GetWalletBalanceResponse
    {
        public string? status { get; set; }
        public string? subCode { get; set; }
        public string? message { get; set; } 
        public ResponseData? data { get; set; }
    }
    public class ResponseData 
    { 
        public string? balance { get; set; }
        public string? availableBalance { get; set; }    
    }

}
