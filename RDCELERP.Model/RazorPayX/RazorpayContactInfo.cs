using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.RazorPayX
{
    public class RazorpayContactInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string contact { get; set; }
        public string type { get; set; }
        public string reference_id { get; set; }
        public bool active { get; set; }
        public string created_at { get; set; }
    }

}
