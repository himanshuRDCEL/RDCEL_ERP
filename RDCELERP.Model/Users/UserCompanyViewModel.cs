using RDCELERP.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Users
{
    public class UserCompanyViewModel : BaseViewModel
    {
        public int UserCompanyId { get; set; }
        public int? UserId { get; set; }
        public int? CompanyId { get; set; }
    }
}
