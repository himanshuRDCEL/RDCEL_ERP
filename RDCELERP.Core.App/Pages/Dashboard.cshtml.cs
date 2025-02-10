using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDCELERP.Core.App.Pages
{
    public class DashboardModel : BasePageModel
    {
        public DashboardModel(IOptions<ApplicationSettings> config)
        : base(config)
        {
        }

        [BindProperty(SupportsGet = true)]
        public UserViewModel UserViewModel { get; set; }
        public IActionResult OnGet()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("Index");
            }
            else
            {
                return Page();
            }
        }
    }
}
