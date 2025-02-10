using RDCELERP.Model.Base;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Role
{
   public  class RoleAccessViewModel : BaseViewModel
    {
        public int RoleAccessId { get; set; }
        public int? AccessListId { get; set; }
        public int? RoleId { get; set; }
        [Display(Name = "Add/Edit")]
        public bool CanAdd { get; set; }
        [Display(Name = "View Detail")]
        public bool CanView { get; set; }
        [Display(Name = "Delete")]
        public bool CanDelete { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ActionName { get; set; }
        public string? ActionUrl { get; set; }
        public int? ParentAccessListId { get; set; }
        public bool Selected { get; set; }
        public string? SetIcon { get; set; }
        [Display(Name = "Role Name")]
        public string? RoleName { get; set; }
       
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public bool? IsMenu { get; set; }
        public IList<RoleAccessViewModel>? ChildRoleAccessViewModelList { get; set; }


    }
}
