using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Model.Base;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.BusinessCustomer;
using static Org.BouncyCastle.Math.EC.ECCurve;



namespace RDCELERP.Core.App.Pages.BusinessCustomer
{
    public class CheckoutModel : BasePageModel
    {
        #region Variable Declaration
        private readonly IItemBookingManager _ItemBookingManager;
        private readonly Digi2l_DevContext _context;
        IItemCartManager _itemCartManager;
        IBusinessCustomerManager _businessCustomerManager;
        IRazorPayManager _paymentManager;
        #endregion

        public CheckoutModel(IItemBookingManager ItemBookingManager, Digi2l_DevContext context, IOptions<ApplicationSettings> config, IItemCartManager itemCartManager, IBusinessCustomerManager businessCustomerManager, IRazorPayManager paymentManager)
        : base(config)
        {
            _ItemBookingManager = ItemBookingManager;
            _context = context;
            _itemCartManager = itemCartManager;
            _businessCustomerManager = businessCustomerManager;
            _paymentManager = paymentManager;
        }

        [BindProperty(SupportsGet = true)]
        public List<BookingItemViewModel> ItemBookingListViewModel { get; set; }
       // public BookingItemViewModel ItemBookingViewModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<ItemCartViewModel> ItemCartViewModelList { get; set; }
        [BindProperty(SupportsGet = true)]
        public BusinessCustomerViewModel BusinessCustomerViewModel { get; set; }
        [BindProperty(SupportsGet = true)]

        public decimal GrandTotalAmount { get; set; }
        public string RazorpayKey { get; set; }
        public string RazorpayOrderId { get; set; }
        [BindProperty(SupportsGet = true)]

        public ItemCartViewModel ItemCartViewModel { get; set; }

        public IActionResult OnGet(string razorpayOrderId)
        {
            
            if (_loginSession != null)
            {
                int customerid = _loginSession.BusinessCustomerViewModel.BusinessCustomerId;

                // Get Booking items and cart items
                
               //ItemBookingListViewModel = _ItemBookingManager.GetBookingItemDetailByCustomerId(customerid);
                ItemCartViewModelList = _itemCartManager.GetItemCartList(customerid);
                if (ItemCartViewModelList != null && ItemCartViewModelList.Any())
                {
                    decimal subtotal = ItemCartViewModelList.Sum(item => Convert.ToDecimal(item.Mrp) * Convert.ToDecimal(item.PurchaseQty));
                    decimal b2bsubtotal = ItemCartViewModelList.Sum(item => Convert.ToDecimal(item.B2bPrice) * Convert.ToDecimal(item.PurchaseQty));

                     ItemCartViewModel = new ItemCartViewModel
                    {
                        SubTotalPrice = subtotal,
                        TotalPrice = subtotal, // or subtotal + shipping, if applicable
                        B2BTotalPrice = b2bsubtotal,
                    };
                }
                // Get customer
                BusinessCustomerViewModel = _businessCustomerManager.GetCustomerById(customerid);

                // Calculate grand total
              //  GrandTotalAmount = ItemCartViewModelList?.Sum(x => x.TotalPrice) ?? 0;

                RazorpayOrderId = razorpayOrderId;

            }

            if (ItemBookingListViewModel == null)
                ItemBookingListViewModel = new List<BookingItemViewModel>();

            RazorpayKey = _baseConfig.Value.RazorPayKey_Id;

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
            int result = 0;
            bool flag = false;

            //decimal totalAmount = await _ItemBookingManager.AddBookingItem(ItemCartViewModelList, _loginSession.UserViewModel.UserId);

            //totalAmount = 10000;

            //if (totalAmount > 0)
            //{
            //    var orderModel = _paymentManager.CreateOrder((int)totalAmount, "Test User", "test@gmail.com", "9999999999");

            //    return Page();
            //}
            //else
            //{
            //    return RedirectToPage("Checkout");
            //}

            var razorpayOrderId = await _ItemBookingManager.AddBookingItem(ItemCartViewModelList,  BusinessCustomerViewModel,_loginSession.BusinessCustomerViewModel.BusinessCustomerId);

            if (!string.IsNullOrEmpty(razorpayOrderId))
            {
                // Redirect to Payment Page with Razorpay Order ID
                return RedirectToPage("checkout", new { razorpayOrderId = razorpayOrderId });
            }
            else
            {
                TempData["Error"] = "Failed to create Razorpay order.";
                return RedirectToPage("Checkout");
            }
        }



    }
}
