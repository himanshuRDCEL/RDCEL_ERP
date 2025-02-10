using RDCELERP.Model.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Product;
using RDCELERP.Model.Master;
using RDCELERP.Model.BusinessUnit;

namespace RDCELERP.Model.Users
{
    public class LoginViewModel
    {
        public UserViewModel? UserViewModel { get; set; }
        public UserPasswordViewModel? UserPasswordViewModel { get; set; }
        public RoleViewModel? RoleViewModel { get; set; }
        public ProductTypeViewModel? ProductTypeViewModel { get; set; }
        public ProductCategoryViewModel? productCategoryViewModel { get; set; }
        public UserRoleLoginViewModel? UserRoleLoginViewModel { get; set; }
        public BusinessUnitViewModel? BusinessUnitViewModel { get; set; }
        
        public bool? IsAdmin { get; set; }
    }
}
