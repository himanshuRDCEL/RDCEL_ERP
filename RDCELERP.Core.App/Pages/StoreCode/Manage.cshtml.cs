using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.StoreCode;

namespace RDCELERP.Core.App.Pages.StoreCode
{
    public class ManageModel : BasePageModel
    {
        #region Variable Declaration
        private readonly IStoreCodeManager _StoreCodeManager;
        #endregion

        public ManageModel(IStoreCodeManager StoreCodeManager, IOptions<ApplicationSettings> config) : base(config)
        {
            _StoreCodeManager = StoreCodeManager;
        }

        [BindProperty(SupportsGet = true)]
        public StoreCodeViewModel StoreCodeViewModel { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id != null)
                StoreCodeViewModel = _StoreCodeManager.GetStoreCodeById(Convert.ToInt32(id));

            if (StoreCodeViewModel == null)
                StoreCodeViewModel = new StoreCodeViewModel();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                return Page();
            }
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            int result = 0;
            if (ModelState.IsValid)
            {
                result = _StoreCodeManager.ManageStoreCode(StoreCodeViewModel, _loginSession.UserViewModel.UserId);
            }

            if (result > 0)
                return RedirectToPage("./Index");
            else

                return RedirectToPage("./Manage");
        }
    }
}
