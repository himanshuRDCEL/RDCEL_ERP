using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC;
using RDCELERP.Model.EVC_Allocated;
using RDCELERP.Model.EVCdispute;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Pages.EVC_Dispute
{
    public class EVC_DisputeFormModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly IEVCManager _EVCManager;
        public readonly CustomDataProtection _protector;
        public readonly IEVCDisputeManager _eVCDisputeManager;
        private readonly Digi2l_DevContext _context;

        #endregion
        public EVC_DisputeFormModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IEVCManager EVCManager, IStateManager StateManager, ICityManager CityManager, IEntityManager EntityManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, IUserManager userManager, CustomDataProtection protector, IEVCDisputeManager EVCDisputeManager) : base(config, protector)

        {
            _EVCManager = EVCManager;
            _eVCDisputeManager = EVCDisputeManager;
            _protector = protector;
            _context = context;

        }

        [BindProperty(SupportsGet = true)]
        public EVCDisputeViewModel EVCDisputeViewModels { get; set; }

        public enum Statusenum { get };


        public IActionResult OnGet(string id)
        {
            int loginUserid = _loginSession.UserViewModel.UserId;

            var EVCRegNoList = _EVCManager.GetEvcByUserId(loginUserid);
            if (EVCRegNoList != null)
            {           
                ViewData["EVCRegNoList"] = EVCRegNoList.EvcregdNo;
                EVCDisputeViewModels.EvcregistrationId = EVCRegNoList.EvcregistrationId;
                OnGetGetOrderByEVCIDAsync();               
            }
            EVCDisputeViewModels.StatusName = EnumHelper.DescriptionAttr(EVCDisputeStatusEnum.Open);
            EVCDisputeViewModels.Status = Convert.ToInt32(EVCDisputeStatusEnum.Open).ToString();
            return Page();
        }
        public IActionResult OnPostAsync()
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
        }
        public JsonResult OnGetGetOrderByEVCIDAsync()
        {
            var OrdersList = _EVCManager.GetOrderByEvcregistrationId(EVCDisputeViewModels.EvcregistrationId);
            if (OrdersList != null)
            {
                ViewData["OrdersList"] = new SelectList(OrdersList, "orderTransId", "RegdNo");
            }
            return new JsonResult(OrdersList);
        }
        public JsonResult OnGetProductDetailsByTransIdAsync()
        {
            return new JsonResult(_EVCManager.GetProductDetailsByTransId(EVCDisputeViewModels.orderTransId));
        }
    }
}
