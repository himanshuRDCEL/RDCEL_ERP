using RDCELERP.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Users
{
    public class UserRoleViewModel : BaseViewModel
    {
        public int UserRoleId { get; set; }
        [Required]
       
        [Display(Name = "Role")]
        public int? RoleId { get; set; }
        public int? UserId { get; set; }
        [Required]
        [Display(Name = "Company")]
        public int? CompanyId { get; set; }
       
        public string? Role { get; set; }

    }

    public class UserRoleListViewModel
    {
        public List<UserRoleViewModel>? UserRoleViewModelList { get; set; }
        public int Count { get; set; }
    }
}
