using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.RazorPayX
{
    public class RazorpayFundAccountListResponse
    {
        public string entity { get; set; }
        public int count { get; set; }
        public List<RazorpayFundAccountInfo> items { get; set; }
    }

}
