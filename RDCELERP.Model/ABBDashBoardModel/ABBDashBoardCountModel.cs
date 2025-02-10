using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.ABBDashBoardModel
{
  public  class ABBDashBoardCountModel
    {
        public int BusinessUnitId { get; set; }
        public string? BusinessUnitName { get; set; }
        public int UserCompanyId { get; set; }
        public string? UserCompanyName { get; set; }
        public int TotalABBOrdersRecieved { get; set; }
        public int OrdersApproved { get; set; }
        public int OrdersNotApproved { get; set; }
       
    }

    public class UserDetailsForABBDashBoard
    {
        public string? UserComapny { get; set; }
        public int UserComapnyId { get; set; }
        public string? BusinessUnitName { get; set; }
        public int BusinessUnitId { get; set; }
    }
}
