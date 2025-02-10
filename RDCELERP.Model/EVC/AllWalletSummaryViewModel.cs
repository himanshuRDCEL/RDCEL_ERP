
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.EVC
{
    public class AllWalletSummaryViewModel:BaseViewModel
    {
        public int EvcregistrationId { get; set; }        
        public string? EvcName { get; set; }//EVCReg+BussinessName
        public string? EvcregdNo { get; set; }       
        public string? ContactPerson { get; set; }
        public decimal? EvcwalletAmount { get; set; } = 0;
        public decimal? RuningBalance { get; set; } = 0;
        public decimal? TotalofInprogress { get; set; } = 0;
        public decimal? TotalofDeliverd { get; set; } = 0;
        public decimal? TotalofCompleted { get; set; } = 0;
        public DateTime ModifiedTime { get; set; }
        public int ZohoId { get; set; }
        public string? BussinessName { get; set; }
        public int? EvcapprovalStatusId { get; set; }
        public string? EmployeeName { get; set; }
        public string? Date { get; set; }

        public int? UserId { get; set; }


    }
}
