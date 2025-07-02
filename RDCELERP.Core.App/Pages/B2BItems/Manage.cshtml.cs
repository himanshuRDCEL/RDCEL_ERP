using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RDCELERP.DAL.Entities;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.BAL.Interface;
using RDCELERP.Model.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.Common.Helper;
using RDCELERP.BAL.MasterManager;
using Microsoft.AspNetCore.Mvc;
using RDCELERP.Model.EcomVoucher;
using RDCELERP.Common.Enums;
using RDCELERP.Model.BusinessUnit;
using System.ComponentModel;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RDCELERP.Model.BusinessCustomer;


namespace RDCELERP.Core.App.Pages.B2BItem
{
    public class ManageModel : BasePageModel
    {
        #region Variable Declaration
        private readonly IItemMasterManager _itemMasterManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private CustomDataProtection _protector;
        public readonly IOptions<ApplicationSettings> _config;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        IDropdownManager _dropdownManager;
        #endregion

        public ManageModel(IDropdownManager dropdownManager, IItemMasterManager itemMasterManager, IWebHostEnvironment webHostEnvironment, Digi2l_DevContext context, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config)

        {
            _itemMasterManager = itemMasterManager;
            _webHostEnvironment = webHostEnvironment;
            _protector = protector;
            _config = config;
            _context = context;
            _dropdownManager = dropdownManager;

        }

        [BindProperty(SupportsGet = true)]
        public ItemMasterViewModel ItemMasterViewModel { get; set; }

        public IActionResult OnGet(string id)
        {
            string URL = _config.Value.URLPrefixforProd;

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                if (id != null)
                {
                    id = _protector.Decode(id);

                    ItemMasterViewModel = _itemMasterManager.GetMasterItemById(Convert.ToInt32(id));

                   
                }

                if (ItemMasterViewModel == null)
                {
                    ItemMasterViewModel = new ItemMasterViewModel();

                }

                return Page();

            }
        }

        public IActionResult OnPostAsync(ItemMasterViewModel ItemMasterViewModel)
        {
            int result = 0;

          
            result = _itemMasterManager.ManageMasterItem(ItemMasterViewModel, _loginSession.UserViewModel.UserId);

            if (result > 0)
            {
                return RedirectToPage("Index");
            }
            else
                return RedirectToPage("Manage");
        }

    }
}
