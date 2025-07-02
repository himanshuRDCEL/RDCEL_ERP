using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.RazorPayX
{
    public class RazorpayPayoutResponse
    {
        public string id { get; set; }
        public string status { get; set; }
        public string fund_account_id { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string reference_id { get; set; }
        public string utr { get; set; }
        public string narration { get; set; }
        public string purpose { get; set; }
        public DateTime created_at { get; set; }
    }
}
