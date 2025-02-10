using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RDCELERP.Core.App.Pages.Base;

namespace RDCELERP.Core.App.Pages.ExchangeRegistration
{
    public class ThankYouModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string RedirectionUrlforback { get; set; }
        public string Message { get; set; }
        public void OnGet()
        {
            RedirectionUrlforback = TempData["URLredirection"] as string;
            Message = TempData["Message"] as string;
            ViewData["Message"] = Message;
            //RedirectionUrlforback = urlReturn;
        }
    }
}
