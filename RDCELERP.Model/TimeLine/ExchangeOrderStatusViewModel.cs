using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.TimeLine
{
     public class ExchangeOrdersStatusViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string? StatusCode { get; set; }
        public string? StatusDescription { get; set; }
        public string? StatusName { get; set; }
    }
}
