
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.BusinessCustomer;
using RDCELERP.Model.Base;


namespace RDCELERP.Core.App.Pages.B2BItem
{
    public class IndexModel  : BasePageModel
    {
        IItemBookingManager _itemBookingManager;
    IItemManager _itemManager;
    ISynchronizedManager _syncManager;
    public IndexModel(IItemManager itemManager, IItemBookingManager itemBookingManager, IOptions<ApplicationSettings> config, ISynchronizedManager synchronizedManager)
  : base(config)
    {
        _itemBookingManager = itemBookingManager;
        _itemManager = itemManager;
        _syncManager = synchronizedManager;
    }
    public void OnGet()
        {
        }
    }
}
