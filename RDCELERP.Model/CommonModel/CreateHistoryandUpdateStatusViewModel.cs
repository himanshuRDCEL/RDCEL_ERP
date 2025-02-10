using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.CommonModel
{
    public class CreateHistoryandUpdateStatusViewModel: BaseViewModel
    {
      
        public int OrderType { get; set; }//Exchange&Abb
        public string? SponsorOrderNumber { get; set; }
        public string? RegdNo { get; set; }
        public string? ZohoSponsorId { get; set; }
        public int? CustId { get; set; }
        public int? StatusId { get; set; }    
        public int? OrderTransId { get; set; }       
        public string? Comment { get; set; }
        public int? Evcid { get; set; }
       public decimal? Evcprice { get; set; }
    }
}
