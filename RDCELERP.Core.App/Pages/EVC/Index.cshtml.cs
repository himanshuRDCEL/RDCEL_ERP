using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.Base;

namespace RDCELERP.Core.App.Pages.EVC
{
    public class IndexModel : BasePageModel
    {
        #region Variable declartion

        #endregion

        #region Constructor
        public IndexModel(IOptions<ApplicationSettings> config, ILogisticManager logisticManager) : base(config)
        {

        }
        #endregion
        public void OnGet()
        {
        }
    }
    
}
