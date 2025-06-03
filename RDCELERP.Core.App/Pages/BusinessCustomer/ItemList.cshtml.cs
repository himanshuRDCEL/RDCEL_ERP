using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.BusinessCustomer;
using RDCELERP.Model.Base;
using RDCELERP.BAL.MasterManager;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.City;

namespace RDCELERP.Core.App.Pages.BusinessCustomer
{
    public class ItemListModel : BasePageModel
    {
        IItemBookingManager _itemBookingManager;
        IItemManager _itemManager;
        ISynchronizedManager _syncManager;
        public ItemListModel( IItemManager itemManager,IItemBookingManager itemBookingManager, IOptions<ApplicationSettings> config,ISynchronizedManager synchronizedManager)
      : base(config)
        {
            _itemBookingManager = itemBookingManager;  
            _itemManager= itemManager;
            _syncManager= synchronizedManager;
        }



        [BindProperty(SupportsGet = true)]
        public List<ItemViewModel> ListItemViewModels { get; set; }
        public IActionResult OnGet()
        {
            ListItemViewModels = _itemManager.GetItemList();

           // _syncManager.FetchAndSaveStockDataAsync();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                return Page();
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var selectedItems = ListItemViewModels.FindAll(item => item.IsSelected);

            bool flag = false;

            flag =  await _itemBookingManager.AddBookingItem(selectedItems, _loginSession.BusinessCustomerViewModel.BusinessCustomerId);
            if (flag)
            {
                return new RedirectToPageResult("Checkout");

            }
            else
            {
                return Page();

            }
        }
    }
}

