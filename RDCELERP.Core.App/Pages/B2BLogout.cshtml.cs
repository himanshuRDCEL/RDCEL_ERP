using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RDCELERP.Core.App.Pages
{
    public class B2BLogoutModel : PageModel
    {

        public ActionResult OnGet()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/b2blogin");
        }
    }
}