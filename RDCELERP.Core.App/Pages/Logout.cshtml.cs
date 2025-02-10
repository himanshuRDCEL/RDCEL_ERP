using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RDCELERP.Core.App.Pages
{
    public class LogoutModel : PageModel
    {

        public ActionResult OnGet()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/index");
        }
    }
}
