using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.StoreCode
{
    public class StoreCodeViewModel: BaseViewModel
    {
        public int StoreCodeId { get; set; }
        public string? SponsorName { get; set; }
        public string? StoreCode { get; set; }
        public int? PinCode { get; set; }
        public string? City { get; set; }
        public string? Location { get; set; }
        public string? StoreManagerEmail { get; set; }
        public string? AssociateCode { get; set; }
        public int? PaymentToCustomer { get; set; }
    }
}
