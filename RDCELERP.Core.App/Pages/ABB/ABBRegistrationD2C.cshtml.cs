using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.AbbRegistration;
using RDCELERP.DAL.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using RDCELERP.Model.City;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Data;
using ExcelDataReader;
using RDCELERP.Model.PinCode;
using RDCELERP.BAL.Helper;
using RDCELERP.Common.Helper;
using RDCELERP.Common.Enums;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.Master;
using RDCELERP.Model.ABBPlanMaster;

namespace RDCELERP.Core.App.Pages.AbbReg
{
    public class ABBRegistrationD2CModel : PageModel
    {
        #region Variable Declaration
        private readonly IAbbRegistrationManager _AbbRegistrationManager;
        private readonly IUserManager _UserManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStateManager _stateManager;
        private readonly ICityManager _cityManager;
        private readonly IBrandManager _brandManager;
        private readonly IBusinessPartnerManager _businessPartnerManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        private readonly IStoreCodeManager _storeCodeManager;
        private readonly IConfiguration configuration;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IPinCodeManager _pinCodeManager;
        IExcelDataReader reader;
        IABBPlanMasterManager _aBBPlanMasterManager;
        #endregion

        #region Constructor
        public ABBRegistrationD2CModel(IPinCodeManager pinCodeManager, IStoreCodeManager storeCodeManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IConfiguration _configuration, IProductTypeManager productTypeManager, IAbbRegistrationManager AbbRegistrationManager, IProductCategoryManager productCategoryManager, IBusinessPartnerManager businessPartnerManager, IBrandManager brandManager, IStateManager StateManager, ICityManager CityManager, IWebHostEnvironment webHostEnvironment, IUserManager userManager, IABBPlanMasterManager aBBPlanMasterManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _AbbRegistrationManager = AbbRegistrationManager;
            _stateManager = StateManager;
            _cityManager = CityManager;
            _brandManager = brandManager;
            _businessPartnerManager = businessPartnerManager;
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
            configuration = _configuration;
            _context = context;
            _storeCodeManager = storeCodeManager;
            _UserManager = userManager;
            _pinCodeManager = pinCodeManager;
            _aBBPlanMasterManager = aBBPlanMasterManager;
        }
        #endregion

        [BindProperty(SupportsGet = true)]
        public AbbRegistrationModel AbbRegistrationModel { get; set; }

        public List<TblAbbregistration> TblAbbregistration { get; set; }

        public List<CityViewModel> citylist = new List<CityViewModel>();
        ResponseResult responseResult = new ResponseResult();

        List<BuProductCatDataModel> buProductCatDataModelList = new List<BuProductCatDataModel>();
        string UserD2C = "DtoC@digimart.co.in";
        Abbplandetail Abbplandetail = new Abbplandetail();

        public int UserId { get; set; }
        public IActionResult OnGet()
        {
            //var NewBrandId = _brandManager.GetAllBrand();
            //if (NewBrandId != null)
            //{
            //    ViewData["NewBrandId"] = new SelectList(NewBrandId, "Id", "Name");
            //}
            buProductCatDataModelList = _AbbRegistrationManager.GetAllProductCategoryByAbbPlanMaster();

            if (buProductCatDataModelList.Count > 0)
            {
                ViewData["ProductGroup"] = new SelectList(buProductCatDataModelList, "Id", "Description");
            }


            return Page();
        }

        public IActionResult OnPostAsync()
        {
            //int result = 0;
            ResponseResult responseResult = new ResponseResult();
            if (ModelState.IsValid)
            {
                responseResult = _AbbRegistrationManager.CreateAbbOrder(AbbRegistrationModel, UserD2C);
            }
            if (responseResult.Status == true)
                return RedirectToPage("NotApproved");

            else
                return RedirectToPage("Manage");
        }



        #region Product category type
        public JsonResult OnGetProductCategoryTypeAsync()
        {
            var productTypeList = _AbbRegistrationManager.GetAllProductTypeByAbbPlanMaster(Convert.ToInt32(AbbRegistrationModel.NewProductCategoryId));
            if (productTypeList != null)
            {
                ViewData["productTypeList"] = new SelectList(productTypeList, "Id", "Description");
            }
            return new JsonResult(productTypeList);
        }

        #endregion

        #region Get Brand by Product category from tblbrandsmartbuy
        public JsonResult OnGetBrandAsync(int cat)
        {
            //incomplete code need to change
            var brandList = _AbbRegistrationManager.GetAllBrandForAbbD2c(Convert.ToInt32(cat));
            if (brandList != null)
            {
                ViewData["NewBrandId"] = new SelectList(brandList, "Id", "Name");
            }
            
            return new JsonResult(brandList);
        }

        #endregion 

        #region AbbPlanPrice 
        public JsonResult OnGetAbbPlanPriceAsync(string amount, int cat, int type)
        {
            Abbplandetail = _aBBPlanMasterManager.GetabbPlanPrice(Convert.ToInt32(cat), Convert.ToInt32(type), amount.ToString(), UserD2C);

            return new JsonResult(Abbplandetail);
        }

        #endregion

        #region Get Brand  
        //not 
        /* public List<BuProductCatDataModel> OnGetProductCategory()
         {
             List<BuProductCatDataModel> buProductCatDataModels = new List<BuProductCatDataModel>();
             buProductCatDataModels = _AbbRegistrationManager.GetAllProductCategoryByAbbPlanMaster();

             return buProductCatDataModels;
         }*/

        #endregion
    }
}
