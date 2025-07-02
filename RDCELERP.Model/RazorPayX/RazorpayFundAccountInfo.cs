using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.RazorPayX
{
    public class RazorpayFundAccountInfo
    {
        public string id { get; set; }
        public string entity { get; set; }
        public string contact_id { get; set; }
        public string account_type { get; set; }
        public BankAccount bank_account { get; set; }

        public Vpa vpa { get; set; } 
        public string FinalExchangePrice { get; set; }
        public int ExchangeId { get; set; }
        public int ordertype { get; set; }
    }

    public class BankAccount
    {
        public string name { get; set; }
        public string ifsc { get; set; }
        public string account_number { get; set; }
    }

    public class Vpa
    {
        public string username { get; set; }
        public string handle { get; set; }
        public string address { get; set; }
    }

}
