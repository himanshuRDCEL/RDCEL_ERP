using RDCELERP.Model.Base;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.BusinessUnit;

namespace RDCELERP.Model.Role
{
   public  class RoleViewModel : BaseViewModel
    {
        public int RoleId { get; set; }
        
        [Required(ErrorMessage = "Name is required.")]
       
        [Display(Name = "Role Name")]
        public string? RoleName { get; set; }
        //[Required]
       
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? PortalName { get; set; }
        public string? PortalLink { get; set; }
        public string? Date { get; set; }
        public string? AcesslistIds { get; set; }
        public List<RoleAccessViewModel>? RoleAccessViewModelList { get; set; }
        public BusinessUnitViewModel? BusinessUnitViewModel { get; set; }
    }

    public class RoleListViewModel
    {
        public List<RoleViewModel>? RoleViewModelList { get; set; }
        public int Count { get; set; }
    }
}
