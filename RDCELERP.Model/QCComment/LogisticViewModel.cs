using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.QCComment
{
    public class LogisticViewModel
    {
        public int LogisticId { get; set; }
        public string? RegdNo { get; set; }
        public string? TicketNumber { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedDate { get; set; }
        public string? Modifiedby { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ServicePartnerId { get; set; }
        public int? OrderTransId { get; set; }
        public int? StatusId { get; set; }
        public string? LGCpic1 { get; set; }
        public string? LGCpic2 { get; set; }
        public string? LGCpic3 { get; set; }
        public string? LGCpic4 { get; set; }
        public string? PoDPdf { get; set; }
        public string? DebitNotePdf { get; set; }
        public string? InvoieImagePdf { get; set; }
        public string? CustomerDeclarationPdf { get; set; }
        public decimal? AmtPaybleThroughLGC { get; set; }
        public string? LGCRescheduleDate { get; set; }

    }
}
