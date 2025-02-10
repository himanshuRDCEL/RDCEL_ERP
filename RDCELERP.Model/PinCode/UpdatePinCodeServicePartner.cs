using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.PinCode
{
    public class UpdatePinCodeServicePartner
    {
        public int ServicePartnerId { get; set; }
        public int CityId { get; set; }
        public string? Pincodes { get; set; }
    }
}
