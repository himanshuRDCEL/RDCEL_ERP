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
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.ABBRedemption;
using RDCELERP.Model.Base;

namespace RDCELERP.Core.App.Pages.ABBRedemp
{
    public class RecordEditModel : BasePageModel
    {
        private readonly IABBRedemptionManager _AbbRedemptionManager;
        private readonly IUserManager _UserManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStateManager _stateManager;
        private readonly ICityManager _cityManager;
        private readonly IBrandManager _brandManager;
        private readonly IBusinessPartnerManager _businessPartnerManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        private readonly IStoreCodeManager _storeCodeManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IAbbRegistrationManager _AbbRegistrationManager;
        private readonly IPinCodeManager _pinCodeManager;

        public RecordEditModel(IPinCodeManager pinCodeManager,IStoreCodeManager storeCodeManager, IAbbRegistrationManager AbbRegistrationManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IProductTypeManager productTypeManager, IABBRedemptionManager abbRedemptionRegManager, IProductCategoryManager productCategoryManager, IBusinessPartnerManager businessPartnerManager, IBrandManager brandManager, IStateManager StateManager, ICityManager CityManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, IUserManager userManager)
           : base(config)
        {
            _webHostEnvironment = webHostEnvironment;
            _AbbRedemptionManager = abbRedemptionRegManager;
            _stateManager = StateManager;
            _cityManager = CityManager;
            _brandManager = brandManager;
            _businessPartnerManager = businessPartnerManager;
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
            _context = context;
            _storeCodeManager = storeCodeManager;
            _UserManager = userManager;
            _AbbRegistrationManager = AbbRegistrationManager;
            _pinCodeManager = pinCodeManager;
        }

        [BindProperty(SupportsGet = true)]
        public ABBRedemptionViewModel AbbRedemptionModel { get; set; }
        public ABBRedemptionViewModel AbbRedemptionVM { get; set; }
        public IActionResult OnGet(string id)
        {
            if (id != null)
            {
                AbbRedemptionModel = _AbbRedemptionManager.GetABBDetailsByRegdNo(id);

                AbbRedemptionVM = _AbbRedemptionManager.GetABBRedemptionByRegdNo(id);
                
                //var CustomerRecord = _AbbRedemptionManager.GetABBRedemptionByRegdNo(AbbRedemptionModel.ABBRedemptionId);
                //if (CustomerRecord != null)
                //{
                //    AbbRedemptionModel.CustFirstName = CustomerRecord.FirstName;
                //    AbbRedemptionModel.CustEmail = CustomerRecord.Email;
                //}
            }

            var ABBRegdno = _AbbRegistrationManager.GetAllABBRegistrationDetails();
            if (ABBRegdno != null)
            {
                ViewData["ABBRegdno"] = new SelectList(ABBRegdno, "RegdNo", "RegdNo");
            }

            //var StoreCode = _businessPartnerManager.GetAllBusinessPartner();
            //if (StoreCode != null)
            //{
            //    ViewData["StoreCode"] = new SelectList(StoreCode, "BusinessPartnerId", "StoreCode");
            //}

            var ProductGroup = _productCategoryManager.GetAllProductCategory();
            if (ProductGroup != null)
            {
                ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
            }

            var ProductType = _productTypeManager.GetAllProductType();
            if (ProductType != null)
            {
                ViewData["ProductType"] = new SelectList(ProductType, "Id", "Description");
            }

            var Newbrandid = _brandManager.GetAllBrand();
            if (Newbrandid != null)
            {
                ViewData["Newbrandid"] = new SelectList(Newbrandid, "Id", "Name");
            }

            var Redempperiod = _AbbRedemptionManager.GetRedemptionPeriod();
            if (Redempperiod != null)
            {
                ViewData["Redempperiod"] = new SelectList(Redempperiod, "LoVid", "LoVname");
            }

            var Redemppercent = _AbbRedemptionManager.GetRedemptionPercentage();
            if (Redemppercent != null)
            {
                ViewData["Redemppercent"] = new SelectList(Redemppercent, "LoVname", "LoVname");
            }

            var ExchangeStatus = _AbbRedemptionManager.GetExchangeOrderStatus();
            if (ExchangeStatus != null)
            {
                ViewData["ExchangeStatus"] = new SelectList(ExchangeStatus, "Id", "StatusCode");
            }

            var citylist = _pinCodeManager.GetAllPinCode();
            if (citylist != null)
            {
                var list = citylist.GroupBy(x => x.Location).Select(x => x.First()).ToList();
                ViewData["list"] = new SelectList(list, "Location", "Location");
            }

            var NewSize = _productTypeManager.GetAllProductType();
            if (NewSize != null)
            {
                ViewData["NewSize"] = new SelectList(NewSize, "Id", "Size");
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
        public IActionResult OnPostAsync()
        {
            int result = 0;
            if (ModelState.IsValid)
            {
                result = _AbbRedemptionManager.SaveABBRedemptionDetails(AbbRedemptionModel);
            }
            if (result > 0)
                return RedirectToPage("RedemptionRecord");

            else
                return RedirectToPage("RedemptionRecord");
        }
    }
}
