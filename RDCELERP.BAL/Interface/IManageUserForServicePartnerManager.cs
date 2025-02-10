using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Users;

namespace RDCELERP.BAL.Interface
{
    public interface IManageUserForServicePartnerManager
    {
        public bool UserNotificationServicePartner(UserViewModel userObj, string subject, string tempateName);
        public int ManageUserRole(UserRoleViewModel UserRoleVM, int userId, int? companyId);
        public int ManageUser(UserViewModel UserVM, int userId, int? companyId);
        public int ManageUserAndUserRole(UserViewModel userVM, int loginUserId);

    }
}
