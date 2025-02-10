using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.ExchangeOrder;

namespace RDCELERP.Model.TimeLine
{
    public class TimeListViewModel : BaseViewModel
    {
       

        public int StatusHistoryId { get; set; }
        public string? RegdNo { get; set; }
        public string? FirstName { get; set; }
        
        public string? OrderStatus { get; set; }
        public string? OrderTimeline { get; set; }
        public string? StatusCode { get; set; }
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? StatusDescription { get; set; }
        public string? CurrentStatus { get; set; }

        public string? CreatedByName { get; set; }
        public string? CreatedByLastName { get; set; }
        public string? CreatedByName1 { get; set; }
        public DateTime? CreatedDate1 { get; set; }
        public int? EVCId { get; set; }
        public string? EVCName { get; set; }
        public string? Comment { get; set; }
        public int? ServicePartnerId { get; set; }
        public string? ServicePartnerName { get; set; }
        public int? DriverDetailId { get; set; }
        public string? DriverName { get; set; }
    }

    public class TimeLineGroupBy
    {
        public string? OrderTimeLine { get; set; }
        public List<TimeListViewModel>? StatusDiscription { get; set; }
        public string? CreatedByName { get; set; }
        public string? CreatedByName1 { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? EVCId { get; set; }
        public string? Comment { get; set; }

    }
}
