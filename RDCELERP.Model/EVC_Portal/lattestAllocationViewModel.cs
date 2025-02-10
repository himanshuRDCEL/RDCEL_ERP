using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.EVC_Portal
{
    public class lattestAllocationViewModel : BaseViewModel
    {
        public int? evcRegistrationId { get; set; }
        public string? regdNo { get; set; }
        public int? exchangeOrderId { get; set; }
        public int? OrdertransId { get; set; }

        public decimal? orderAmount { get; set; }
        public string? productTypeName { get; set; }

        public string? productCategoryName { get; set; }
        public DateTime? AssignDate { get; set; }

        public DateTime? deliveryDate { get; set; }
        public DateTime? completeDate { get; set; }
        public DateTime? inProgressDate { get; set; }
        public string? dateInProg { get; set; }
        public string? dateComplete { get; set; }
        public string? FinalDate { get; set; }
        public string? storecode { get; set; }
        public string? WalletStatus { get; set; }

    }
}
