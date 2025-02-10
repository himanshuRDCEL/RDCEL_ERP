using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;

namespace RDCELERP.Core.App.Pages.Refurbisher
{
    public class RefurbisherIndexModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IAbbRegistrationManager _AbbRegistrationManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        IRefurbisherManager _refurbisherManager;

        public RefurbisherIndexModel(IWebHostEnvironment webHostEnvironment, IAbbRegistrationManager AbbRegistrationManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IRefurbisherManager refurbisherManager)
       : base(config)
        {
            _webHostEnvironment = webHostEnvironment;
            _AbbRegistrationManager = AbbRegistrationManager;
            _context = context;
            _refurbisherManager = refurbisherManager;
        }

        TblRefurbisherRegistration TblRefurbisherRegistration = null;
        public void OnGet()
        {

        }

        #region Refurbisher Delete
        public async Task<IActionResult> OnGetRefurbisherDeleteAsync(int RefurbisherId)
        {
            bool Isordercancel = false;
            if (RefurbisherId == null)
            {
                return NotFound();
            }
            else
            {
                Isordercancel = _refurbisherManager.DeletRefurbisherById(RefurbisherId);
            }
            return RedirectToPage("RefurbisherIndex", Isordercancel);
        }
        #endregion
    }
}
