using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.CashfreeModel
{
    public class CashFreeData
    {
        public Transfer? transfer { get; set; }
    }

    public class TransactionCashFree
    {
        public string? status { get; set; }
        public string? subCode { get; set; }
        public string? message { get; set; }
        public CashFreeData? data { get; set; }
    }

    public class Transfer
    {
        public int referenceId { get; set; }
        public string? bankAccount { get; set; }
        public string? ifsc { get; set; }
        public string? beneId { get; set; }
        public string? amount { get; set; }
        public string? status { get; set; }
        public string? utr { get; set; }
        public string? addedOn { get; set; }
        public string? processedOn { get; set; }
        public string? transferMode { get; set; }
        public int acknowledged { get; set; }
        public string? phone { get; set; }
        public string? vpa { get; set;}
    }
}
