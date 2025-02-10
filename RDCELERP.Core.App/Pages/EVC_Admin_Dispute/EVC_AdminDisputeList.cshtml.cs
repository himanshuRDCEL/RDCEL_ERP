using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC;
using RDCELERP.Model.EVC_Allocated;
using RDCELERP.Model.EVCdispute;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Pages.EVC_Admin_Dispute
{
    public class EVC_AdminDisputeListModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly IEVCManager _EVCManager;
        private readonly IUserManager _UserManager;
        private readonly IStateManager _stateManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICityManager _cityManager;
        private readonly IEntityManager _entityManager;
        public readonly CustomDataProtection _protector;
        public readonly IEVCDisputeManager _eVCDisputeManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        #endregion
        public EVC_AdminDisputeListModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IEVCManager EVCManager, IStateManager StateManager, ICityManager CityManager, IEntityManager EntityManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, IUserManager userManager, CustomDataProtection protector, IEVCDisputeManager EVCDisputeManager) : base(config, protector)

        {
            _EVCManager = EVCManager;
            _webHostEnvironment = webHostEnvironment;
            _UserManager = userManager;
            _stateManager = StateManager;
            _cityManager = CityManager;
            _entityManager = EntityManager;
            _eVCDisputeManager = EVCDisputeManager;
            _protector = protector;
            _context = context;

        }

        [BindProperty(SupportsGet = true)]
        public EVCDisputeViewModel EVCDisputeViewModels { get; set; }
        public enum Statusenum { get };


        public IActionResult OnGet(string id)
        {
            /*if (id != null)
            {
                id = _protector.Decode(id);
                EVCDisputeViewModels = _eVCDisputeManager.GetDisputeByEVCDisputeId(Convert.ToInt32(id));

                var EVCREg = _context.TblEvcregistrations.Find(EVCDisputeViewModels.EvcregistrationId);
                EVCDisputeViewModels.EVCRegdNo = EVCREg.EvcregdNo;
                var OrderReg = _context.TblExchangeOrders.Find(EVCDisputeViewModels.orderTransId);
                EVCDisputeViewModels.OrderRegdNo = OrderReg.RegdNo;
                var enumData = from EVCDisputeStatusEnum e in Enum.GetValues(typeof(EVCDisputeStatusEnum))
                               select new
                               {
                                   ID = (int)e,
                                   Name = e.ToString()
                               };
                //  EVCDisputeStatusEnum Statusenum = new EVCDisputeStatusEnum();
                ViewData["StatusList"] = new SelectList(enumData, "ID", "Name");


                var ProductDetails = _EVCManager.GetProductDetailsByTransId(EVCDisputeViewModels.orderTransId);
                if (ProductDetails != null)
                {
                    EVCDisputeViewModels.ProductTypeName = ProductDetails.ProductTypeName;
                    EVCDisputeViewModels.ProductCatName = ProductDetails.ProductCatName;
                    //EVCDisputeViewModels.EmployeeEMail = SecurityHelper.DecryptString(EVCDisputeViewModels.EmployeeEMail, _baseConfig.Value.SecurityKey);
                }
            }
            EVCDisputeViewModels.Status = Convert.ToInt32(EVCDisputeStatusEnum.Open).ToString();
            var EVCRegNoList = _EVCManager.GetAllEVCRegistration();
            if (EVCRegNoList != null)
            {
                ViewData["EVCRegNoList"] = new SelectList(EVCRegNoList, "EvcregistrationId", "EvcregdNo");
            }*/
            return Page();
        }
       /* public IActionResult OnPostAsync()
        {
            int result = 0;
            if (ModelState.IsValid)
            {
                result = _eVCDisputeManager.SaveEVCDisputeDetails(EVCDisputeViewModels, _loginSession.UserViewModel.UserId);
                if (result > 0)
                {
                    return RedirectToPage("ShowDisputeList");
                }
                else
                {
                    return RedirectToPage("EVC_DisputeForm");
                }
            }
            else
            {
                return RedirectToPage("EVC_DisputeForm");
            }
        }*/
       /* public JsonResult OnGetGetOrderByEVCIDAsync()
        {
            var OrdersList = _EVCManager.GetOrderByEvcregistrationId(EVCDisputeViewModels.EvcregistrationId);
            if (OrdersList != null)
            {
                ViewData["OrdersList"] = new SelectList(OrdersList, "ExchangeOrderId", "RegdNo");
            }
            return new JsonResult(OrdersList);
        }*/
    }
}
