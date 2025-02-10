using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.Company;
using RDCELERP.Model.EVC;
using RDCELERP.Model.Users;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;

namespace RDCELERP.Core.App.Pages.EVC
{
    public class EVC_InvoicesModel : BasePageModel
    {
        #region Variable declartion
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IEVCManager _EVCManager;
        private readonly ILogisticManager _LogisticManager;
        private readonly IDropdownManager _dropdownManager;
        #endregion

        # region Constructor
        public EVC_InvoicesModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IEVCManager EVCManager, CustomDataProtection protector, ILogisticManager logisticManager, IDropdownManager dropdownManager) : base(config)
        {
            _EVCManager = EVCManager;
            _context = context;
            _LogisticManager = logisticManager;
            _dropdownManager = dropdownManager;
        }
        #endregion 

        #region Model Binding
        [BindProperty(SupportsGet = true)]
        public IList<TblOrderLgc> tblOrderLgcs { get; set; }
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();

        public IList<EVC_ApprovedModel> EVC_ApprovedModels { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<TblOrderLgc> orderLgcList { get; set; }
        public List<TblCity> tblCities { get; set; }
        public List<TblEvcregistration> evcregistrations { get; set; }
        #endregion

        public async Task OnGetAsync()
        {
            IEnumerable<SelectListItem> EVCCityDDL = null;
            EVCCityDDL = _dropdownManager.GetEVCCityDDList();
            ViewData["EVCCityDDL"] = EVCCityDDL;
        }
        public IActionResult OnPostGenerateInvoice(int? EvcRegistrationId)
        {
            bool flag = false;
            tblOrderLgcs = _LogisticManager.GenerateInvoiceForEVC(EvcRegistrationId,_loginSession.UserViewModel.UserId);
            if (tblOrderLgcs != null && tblOrderLgcs.Count > 0)
            {
                flag = true;
            }
            /*return RedirectToPage("./EVC_Invoices");*/
            return new JsonResult(flag);
        }
        
        public IActionResult OnGetEVCByCityId(int cityId)
        {
            IEnumerable<SelectListItem> EVCDDList = new List<SelectListItem>();
            EVCDDList = _dropdownManager.GetEVCDDListByCityId(cityId);
            return new JsonResult(EVCDDList);
        }
    }
}

