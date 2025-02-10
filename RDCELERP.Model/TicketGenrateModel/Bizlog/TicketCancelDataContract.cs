using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.TicketGenrateModel.Bizlog
{
    public class TicketCancelResponseDataContract
    {
        public List<string>? ticketNo { get; set; }
        public string? message { get; set; }
        public bool success { get; set; }
        public string? apiToken { get; set; }
    }

    public class RegnioList
    {
        public List<string>? regNo { get; set; }
    }
}
