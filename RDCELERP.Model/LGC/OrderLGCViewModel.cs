using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.LGC
{
    public class OrderLGCViewModel : BaseViewModel
    {
        public int OrderLgcid { get; set; }
        public int OrderTransId { get; set; }
        public DateTime? ProposedPickDate { get; set; }
        public string? Lgccomments { get; set; }//Lgccomments//LGCComments
        public string? LGCComments { get; set; }//Lgccomments//LGCComments
        public decimal? LgcpayableAmt { get; set; }
        public string? ActualPickupDate { get; set; }
        public string? ActualDropDate { get; set; }
        public int? SecondaryOrderFlag { get; set; }
        public int? StatusId { get; set; }
        public string? StatusCode { get; set; }
        public int? Evcpodid { get; set; }
        public int? LogisticId { get; set; }

       

        public LGCOrderViewModel? lGCOrderViewModel { get; set; }
        public OrderImageUploadViewModel? OrderImageUploadViewModel { get; set; }


    }
}
