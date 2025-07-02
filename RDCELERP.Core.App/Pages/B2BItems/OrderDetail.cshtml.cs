using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.BusinessCustomer;
using RDCELERP.Model.Base;
using Microsoft.AspNetCore.Mvc;
using RDCELERP.Model.MobileApplicationModel.LGC;


namespace RDCELERP.Core.App.Pages.B2BItems
{
    public class OrderDetailModel : BasePageModel
    {
        IItemBookingManager _itemBookingManager;
        IItemManager _itemManager;
        ISynchronizedManager _syncManager;
        private CustomDataProtection _protector;

        public OrderDetailModel(CustomDataProtection protector,IItemManager itemManager, IItemBookingManager itemBookingManager, IOptions<ApplicationSettings> config, ISynchronizedManager synchronizedManager)
      : base(config)
        {
            _itemBookingManager = itemBookingManager;
            _itemManager = itemManager;
            _syncManager = synchronizedManager;
            _protector= protector;
        }
        public AdminOrderDetailViewModel OrderDetail { get; set; }

        public async Task<IActionResult> OnGet(string orderNo)
        {
            if (orderNo != null)
            {
                orderNo = _protector.Decode(orderNo);

                OrderDetail = await _itemBookingManager.GetOrderDetailsAsync(orderNo);

            }


            return Page();
        }
    }
}
