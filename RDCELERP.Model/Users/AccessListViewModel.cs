using RDCELERP.Model.Base;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Users
{
   public class AccessListViewModel : BaseViewModel
    {
        public int AccessListId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ActionName { get; set; }
        public string? ActionUrl { get; set; }
        public int? ParentAccessListId { get; set; }
        public int? CompanyId { get; set; }
        public List<AccessListViewModel>? AccessListViewModelList { get; set; }
        public int Count { get; set; }
        public string? SetIcon { get; set; }
        public bool? IsMenu { get; set; }
        public string? Action { get; set; }
      
        
    }
}
