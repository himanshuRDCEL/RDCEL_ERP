using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
using RDCELERP.Model.Company;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.Core.App.Helper;
using RDCELERP.Model.BusinessCustomer;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Model.EcomVoucher;
using ZXing;
using System.Text.Json;


namespace RDCELERP.Core.App.Pages.BusinessCustomer
{
    public class CartModel : BasePageModel
    {
       
        IItemManager _itemManager;
        IItemCartManager _itemCartManager;
        ISynchronizedManager _syncManager;
        public CartModel(  IItemManager itemManager, IOptions<ApplicationSettings> config, ISynchronizedManager synchronizedManager, IItemCartManager itemCartManager)
      : base(config)
        {
            _itemManager = itemManager;
            _syncManager = synchronizedManager;
            _itemCartManager = itemCartManager;
        }



        [BindProperty(SupportsGet = true)]
        public List<ItemCartViewModel> ItemCartViewModelList { get; set; }
        public ItemCartViewModel ItemCartViewModel { get; set; }
        public IActionResult OnGet()
        {
            if (_loginSession != null)
            {
                int customerid = _loginSession.BusinessCustomerViewModel.BusinessCustomerId;
                ItemCartViewModelList = _itemCartManager.GetItemCartList(Convert.ToInt32(customerid));
                if (ItemCartViewModelList != null && ItemCartViewModelList.Any())
                {
                    decimal subtotal = ItemCartViewModelList.Sum(item => Convert.ToDecimal(item.B2bPrice) * Convert.ToDecimal(item.PurchaseQty));

                    ItemCartViewModel = new ItemCartViewModel
                    {
                        SubTotalPrice = subtotal,
                        TotalPrice = subtotal // or subtotal + shipping, if applicable
                    };
                }
            }

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                return Page();
            }
        }

        public IActionResult OnPostAddToCart([FromBody] CartItemViewModel item)
        {
            int result = 0;
            ItemCartViewModel itemCartViewModel = new ItemCartViewModel();
            itemCartViewModel.ItemCartId = 0;
            itemCartViewModel.ItemId = item.ItemId;
            itemCartViewModel.ItemMasterId = item.ItemMasterId;
            itemCartViewModel.Brand=item.Brand;
            itemCartViewModel.B2bPrice = item.B2BPrice;
            itemCartViewModel.PurchaseQty=item.Quantity.ToString();
            itemCartViewModel.ItemDesc = item.Name;
           

            result = _itemCartManager.ManageItemCart(itemCartViewModel, _loginSession.BusinessCustomerViewModel.BusinessCustomerId);

            if (result > 0)
            {
                return new JsonResult(new { success = true, message = "Item added to cart" });
            }

            else
            {
                return new JsonResult(new { success = true, message = "Something Wrong!" });
            }
        }

        public async Task<IActionResult> OnPostUpdateQuantity([FromBody] QuantityVM QuantityVM)
        {
           ResponseCartVM responseCartVM = new ResponseCartVM();
            responseCartVM = await _itemCartManager.ManageItemQuantity(QuantityVM, _loginSession.BusinessCustomerViewModel.BusinessCustomerId);

            if (responseCartVM.Result > 0)
            {
                return new JsonResult(new { success = true, message = "cart update" , data = responseCartVM });
            }
            if (responseCartVM.Result == -1)
            {
                return new JsonResult(new { success = false, message = "Item out of stock" });
            }

            else
            {
                return new JsonResult(new { success = false, message = "Something Wrong!" });
            }

        }


        public IActionResult OnPostRemoveItem([FromBody] RemoveItemVM removeItemVM)
        {
            bool flag = false;
           // flag = _itemCartManager.RemoveItem(removeItemVM, _loginSession.BusinessCustomerViewModel.BusinessCustomerId);

            var response = _itemCartManager.RemoveItem(removeItemVM, _loginSession.BusinessCustomerViewModel.BusinessCustomerId);

            if (response != null && response.Result == 1)
            {
                return new JsonResult(new{success = true,message = "Item removed successfully",
                    data = new
                    {
                        totalPrice = response.TotalPrice,
                        //itemCartId = response.ItemCartId
                    }
                });
            }
            else
            {
                return new JsonResult(new { success = false, message = "Something went wrong!" });
            }
        }
        }
}
