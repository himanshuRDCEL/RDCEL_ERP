using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.TimeLine
{
    public class TimeLineStatusMappingViewModel : BaseViewModel
    {
        public int TimelineStatusMappingId { get; set; }
        public int OrderTimeLineId { get; set; }
        public int StatusId { get; set; }
        public string? StatusCode { get; set; }
    }
}
