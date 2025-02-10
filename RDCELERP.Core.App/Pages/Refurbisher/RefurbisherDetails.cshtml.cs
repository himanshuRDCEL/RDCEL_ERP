using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.Base;

namespace RDCELERP.Core.App.Pages.Refurbisher
{
    public class RefurbisherDetailsModel : BasePageModel
    {

        private readonly IOptions<ApplicationSettings> _config;

        public RefurbisherDetailsModel(IOptions<ApplicationSettings> config) : base(config)
        {
            _config = config;
        }
        [BindProperty(SupportsGet = true)]
        public string URLPrefixforProd { get; set; }
        public IActionResult OnGet()
        {
            string trimmedUrl = _config.Value.BaseURL;
            URLPrefixforProd = trimmedUrl.TrimEnd('/');
            return Page();
        }
    }
}
