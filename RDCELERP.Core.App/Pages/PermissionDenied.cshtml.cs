using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.Base;

namespace RDCELERP.Core.App.Pages
{
    public class PermissionDeniedModel : PageModel
    {
        /*public PermissionDeniedModel(IOptions<ApplicationSettings> config)
         : base(config)
        { 
        }*/
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
