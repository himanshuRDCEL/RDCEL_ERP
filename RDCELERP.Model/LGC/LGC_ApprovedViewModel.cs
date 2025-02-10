using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.LGC
{
    public class LGC_ApprovedViewModel: BaseViewModel
    {
            
        public int ServicePartnerId { get; set; }
        public string? ServicePartnerName { get; set; }
        public string? ServicePartnerDescription { get; set; }
        public string? ServicePartnerRegdNo { get; set; }
        public int? ServicePartnerCityId { get; set; }
        public string? CityName { get; set; }
        public string? Date { get; set; }

    }
}
