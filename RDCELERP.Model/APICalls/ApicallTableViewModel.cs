using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;

namespace RDCELERP.Model.APICalls
{
    public class ApicallViewModel
    {
        public int ApicallId { get; set; }
        public string? Url { get; set; }
        public string? MethodType { get; set; }
        public string? RequestBody { get; set; }
        public string? ResponseBody { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
    }
}
