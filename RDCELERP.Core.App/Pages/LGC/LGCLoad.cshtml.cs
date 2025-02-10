using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.DriverDetails;

namespace RDCELERP.Core.App.Pages.LGC
{
    public class LGCLoadModel : BasePageModel
    {
        #region Variable declartion
        private readonly ILogisticManager _logisticManager;
        private readonly IProductCategoryManager _productCategoryManager;
        #endregion

        #region Constructor
        public LGCLoadModel(IOptions<ApplicationSettings> config, ILogisticManager logisticManager, IProductCategoryManager productCategoryManager) : base(config)
        {
            _logisticManager = logisticManager;
            _productCategoryManager = productCategoryManager;
        }
        #endregion

        #region Bind Properties
        [BindProperty(SupportsGet = true)]
        public DriverDetailsViewModel driverDetailsViewModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<TblOrderLgc> orderLgcList { get; set; }
        public List<TblCity> tblCities { get; set; }
        public List<TblEvcregistration> evcregistrations { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblServicePartner tblServicePartner { get; set; }
        public List<TblEvcPartner>? tblEvcPartnerList { get; set; }
        #endregion
        public IActionResult OnGet()
        {
            tblCities = new List<TblCity>();
            evcregistrations = new List<TblEvcregistration>();
            int userId = 0;
            if (_loginSession != null)
            {
                userId = _loginSession.UserViewModel.UserId;
                tblServicePartner = _logisticManager.GetServicePartnerDetails(userId);
                if (tblServicePartner == null)
                {
                    tblServicePartner = new TblServicePartner();
                    tblServicePartner.UserId = 0;
                }
            }
            orderLgcList = _logisticManager.GetCityAndEvcList(userId);
            if (orderLgcList != null && orderLgcList.Count > 0)
            {
                foreach (TblOrderLgc item in orderLgcList)
                {
                    if (item.Evcpartner != null && item.Evcpartner.City != null)
                    {
                        tblCities.Add(item.Evcpartner.City);
                    }
                }
            }
            if (tblCities.Count > 0)
            {
                var citylist = tblCities.Distinct().ToList();
                driverDetailsViewModel.CityList = citylist.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = a.Name,
                        Value = a.CityId.ToString(),
                        Selected = false
                    };
                });
            }
            // Get Product Category List
            var ProductGroup = _productCategoryManager.GetAllProductCategory();
            if (ProductGroup != null)
            {
                ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            int LoggedInUserId = _loginSession.UserViewModel.UserId;
            if (driverDetailsViewModel != null)
            {
                bool result = _logisticManager.saveListOfLoads(driverDetailsViewModel, LoggedInUserId);
                if(result == true)
                {
                    return RedirectToPage("./LogiPickDrop");
                }
                else
                {
                    return Page();
                }
            }
            
            return Page();
        }

        public IActionResult OnGetEVCByCityId(int cityId)
        {
            evcregistrations = new List<TblEvcregistration>();
            tblEvcPartnerList = new List<TblEvcPartner>();
            List<SelectListItem>? evcListObj = null;
            var userId = _loginSession.UserViewModel.UserId;
            orderLgcList = _logisticManager.GetCityAndEvcList(userId).Where(x => x.Evcregistration != null && x.Evcpartner?.Evcregistration != null && x.Evcpartner?.CityId == cityId).ToList();
            if (orderLgcList != null && orderLgcList.Count > 0)
            {
                foreach (TblOrderLgc item in orderLgcList)
                {
                    if (item.Evcpartner != null && item.Evcpartner.Evcregistration != null)
                    {
                        tblEvcPartnerList.Add(item.Evcpartner);
                    }
                }
            }
            if (tblEvcPartnerList.Count > 0)
            {
                var evclist = tblEvcPartnerList.Distinct().ToList();
                evcListObj = evclist.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = a.Evcregistration?.BussinessName+"-"+ a.EvcStoreCode,
                        Value = a.EvcPartnerId.ToString(),
                        Selected = false
                    };
                });
            }
            if (evcListObj == null)
            {
                evcListObj = new List<SelectListItem>();
            }
            var result = new SelectList(evcListObj, "Value", "Text");
            //var result1 = _logisticManager.getEvcListByCityId(cityId);
            return new JsonResult(result);
        }
    }
}
