using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.MapSerVicePartner
{
    public class MapServicePartnerViewModel
    {
        public int MapServicePartnerCityStateId { get; set; }
        public int? ServicePartnerId { get; set; }
        public int? PincodeId { get; set; }
        public bool? IsPrimaryPincode { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
    }
}
