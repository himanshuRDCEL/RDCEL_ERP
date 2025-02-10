using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.EVCPriceMaster
{
    public class EVCPriceMasterViewModel : BaseViewModel
    {
        public int EvcpriceMasterId { get; set; }
        public string? SponsorName { get; set; }
        public string? ProductGroup { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? ProductCategoryId { get; set; }
        public string? ProductType { get; set; }
        public int? ProductTypeId { get; set; }
        public string? Size { get; set; }
        public int? EvcP { get; set; }
        public int? EvcQ { get; set; }
        public int? EvcR { get; set; }
        public int? EvcS { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Lgccost { get; set; }
    }
}
