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
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC;
using RDCELERP.Model.EVC_Allocated;
using RDCELERP.Model.EVCdispute;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Pages.EVC_Admin_Dispute
{
    public class EVC_Admin_DisputeFormModel : CrudBasePageModel
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
        private readonly Digi2l_DevContext _context;

        #endregion
        public EVC_Admin_DisputeFormModel(Digi2l_DevContext context, IEVCManager EVCManager, IStateManager StateManager, ICityManager CityManager, IEntityManager EntityManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, IUserManager userManager, CustomDataProtection protector, IEVCDisputeManager EVCDisputeManager) : base(config, protector)

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
            if (id != null)
            {
                id = _protector.Decode(id);
                EVCDisputeViewModels = _eVCDisputeManager.GetDisputeByEVCDisputeId(Convert.ToInt32(id));
                if (EVCDisputeViewModels != null)
                {
                    var EVCREG = _context.TblEvcregistrations.Find(EVCDisputeViewModels.EvcregistrationId);
                    EVCDisputeViewModels.EVCRegdNo = EVCREG.EvcregdNo;
                    TblOrderTran ordertrans = _context.TblOrderTrans.Where(x => x.OrderTransId == EVCDisputeViewModels.orderTransId).FirstOrDefault();

                    // TblExchangeOrder orderReg = _context.TblExchangeOrders.Where(x => x.Id == ordertrans.ExchangeId).FirstOrDefault();
                    EVCDisputeViewModels.OrderRegdNo = ordertrans.RegdNo;
                    var maxStatusId = int.Parse(EVCDisputeViewModels.Status); // change this to the maximum status ID you want to display
                    var enumData = from EVCDisputeStatusEnum e in Enum.GetValues(typeof(EVCDisputeStatusEnum))
                                   where (int)e > maxStatusId
                                   select new
                                   {
                                       ID = (int)e,
                                       Name = e.ToString()
                                   };
                    ViewData["StatusList"] = new SelectList(enumData, "ID", "Name");

                    //var enumData = from EVCDisputeStatusEnum e in Enum.GetValues(typeof(EVCDisputeStatusEnum))
                    //               select new
                    //               {
                    //                   ID = (int)e,
                    //                   Name = e.ToString()
                    //               };
                    //ViewData["StatusList"] = new SelectList(enumData, "ID", "Name");
                    var ProductDetails = _EVCManager.GetProductDetailsByTransId(EVCDisputeViewModels.orderTransId);
                    if (ProductDetails != null)
                    {
                        EVCDisputeViewModels.ProductTypeName = ProductDetails.ProductTypeName;
                        EVCDisputeViewModels.ProductCatName = ProductDetails.ProductCatName;
                    }
                }
            }
            EVCDisputeViewModels.StatusName = EnumHelper.DescriptionAttr(EVCDisputeStatusEnum.Open);
            EVCDisputeViewModels.Status = Convert.ToInt32(EVCDisputeStatusEnum.Open).ToString();
            var EVCRegNoList = _EVCManager.GetAllEVCRegistration();

            if (EVCRegNoList != null)
            {
                ViewData["EVCRegNoList"] = new SelectList((from s in EVCRegNoList.ToList()
                                                           select new
                                                           {
                                                               EvcregistrationId = s.EvcregistrationId,
                                                               EvcregdNo = s.EvcregdNo + "-" + s.BussinessName
                                                           }), "EvcregistrationId", "EvcregdNo", null);
            }
            //if (EVCRegNoList != null)
            //{
            //    ViewData["EVCRegNoList"] = new SelectList(EVCRegNoList, "EvcregistrationId", "EvcregdNo");
            //}
            return Page();
        }
        public IActionResult OnPostAsync()
        {
            int result = 0;
            if (ModelState.IsValid)
            {
                result = _eVCDisputeManager.SaveEVCDisputeDetailsForAdmin(EVCDisputeViewModels, _loginSession.UserViewModel.UserId);
                if (result > 0)
                {
                    return RedirectToPage("EVC_AdminDisputeList");
                }
                else
                {
                    return RedirectToPage("EVC_Admin_DisputeForm");
                }
            }
            else
            {
                return RedirectToPage("EVC_Admin_DisputeForm");
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
        public IActionResult OnGetSearchEVCRedgid(string term)
        {
            var data = _context.TblEvcregistrations.Where(e => (term == "#" || (e.EvcregdNo + "-" + e.BussinessName).Contains(term)))
                         .Select(e => new { Evcregistration = e.EvcregdNo + "-" + e.BussinessName, EVCId = e.EvcregistrationId })
                         .Select(e => new SelectListItem
                         {
                             Value = e.Evcregistration.ToString(),
                             Text = e.EVCId.ToString()
                         })
                         .ToArray();
            return new JsonResult(data);
        }
    }
}
