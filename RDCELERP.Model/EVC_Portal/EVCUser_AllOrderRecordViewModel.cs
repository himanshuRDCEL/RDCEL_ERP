using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.EVC_Portal
{
    public class EVCUser_AllOrderRecordViewModel:BaseViewModel
    {
        public int? EvcregistrationId { get; set; }
        public int orderTransId { get; set; }

        public string? RegdNo { get; set; }
        public string? TicketNumber { get; set; }
        public decimal? OrderAmount { get; set; }
        public int ProductTypeId { get; set; }
        public string? ProductTypeName { get; set; }//TblProductType
        public string? ProductCatName { get; set; }
        public int ProductCatId { get; set; }
        public string? ActualProdQlty { get; set; }
        public DateTime? OrderofAssignDate { get; set; }
        public DateTime? OrderOfDeliverdDate { get; set; }
        public DateTime? OrderOfCompleteDate { get; set; }
        public DateTime? OrderOfInprogressDate { get; set; }
        public string? Date { get; set; }
        public string? Status { get; set; }

        //Added by Pooja Jatav
        public string? EvcStoreCode { get; set; }




    }
}
