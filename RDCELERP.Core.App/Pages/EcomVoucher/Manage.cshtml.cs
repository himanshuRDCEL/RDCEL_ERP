using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RDCELERP.DAL.Entities;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.BAL.Interface;
using RDCELERP.Model.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.Common.Helper;
using RDCELERP.BAL.MasterManager;
using Microsoft.AspNetCore.Mvc;
using RDCELERP.Model.EcomVoucher;
using RDCELERP.Common.Enums;
using RDCELERP.Model.BusinessUnit;
using System.ComponentModel;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RDCELERP.BAL.Enum;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;

namespace RDCELERP.Core.App.Pages.EcomVoucher
{
    public class ManageModel : BasePageModel
    {
        #region Variable Declaration
        private readonly IEcomVoucherManager _ecomVoucherManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private CustomDataProtection _protector;
        public readonly IOptions<ApplicationSettings> _config;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        IDropdownManager _dropdownManager;
        #endregion

        public ManageModel(IDropdownManager dropdownManager, IEcomVoucherManager ecomVoucherManager, IWebHostEnvironment webHostEnvironment, Digi2l_DevContext context, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config)

        {
            _ecomVoucherManager = ecomVoucherManager;
            _webHostEnvironment = webHostEnvironment;
            _protector = protector;
            _config = config;
            _context = context;
            _dropdownManager = dropdownManager;

        }

        [BindProperty(SupportsGet = true)]
        public EcomVoucherViewModel EcomVoucherViewModel { get; set; }
       
        public IActionResult OnGet(string id)
        {
            string URL = _config.Value.URLPrefixforProd;

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                if (id != null)
                {
                    id = _protector.Decode(id);

                    EcomVoucherViewModel = _ecomVoucherManager.GetEcomVoucherById(Convert.ToInt32(id));

                    if (EcomVoucherViewModel.Phoneno != null)
                    {
                        EcomVoucherViewModel.Phoneno = SecurityHelper.DecryptString(EcomVoucherViewModel.Phoneno, _config.Value.SecurityKey);
                    }                 
                }

                if (EcomVoucherViewModel == null)
                    EcomVoucherViewModel = new EcomVoucherViewModel();

                EcomVoucherViewModel.BrandList = _dropdownManager.GetBrandListByBUId(_loginSession.CompanyViewModel.BusinessUnitId);

                if (EcomVoucherViewModel.BrandId != null && EcomVoucherViewModel.BrandId > 0)
                {
                    EcomVoucherViewModel.CategoryList = _dropdownManager.GetcategoryListByBrandId(EcomVoucherViewModel.BrandId);

                    List<SelectListItem> CategoryLists = EcomVoucherViewModel.CategoryList.Select(x => new SelectListItem
                    {
                        Text = x.Text,
                        Value = x.Value,
                        Selected = IsChecked(EcomVoucherViewModel.CategoryIds, x.Value)
                    }).ToList();
                    ViewData["CategoryLists"] = CategoryLists;
                }
                EcomVoucherViewModel.EcomVoucherTypeList = GetVoucherTypeList(Convert.ToInt32(EcomVoucherViewModel.EcomVoucherType));

                EcomVoucherViewModel.EcomVoucherValueTypeList = GetVoucherValueTypeList(Convert.ToInt32(EcomVoucherViewModel.ValueType));



                return Page();
                
            }
        }

        public IActionResult OnPostAsync(EcomVoucherViewModel EcomVoucherViewModel)
        {
            bool result = false;

            if (EcomVoucherViewModel.SelectedCategoryIds != null && EcomVoucherViewModel.SelectedCategoryIds.Count > 0)
            {
                EcomVoucherViewModel.CategoryIds = string.Empty;
                foreach (int item in EcomVoucherViewModel.SelectedCategoryIds)
                {

                    EcomVoucherViewModel.CategoryIds = !string.IsNullOrEmpty(EcomVoucherViewModel.CategoryIds) ? EcomVoucherViewModel.CategoryIds + "," + item : item.ToString();
                }
            }
             result = _ecomVoucherManager.ManageEcomVoucher(EcomVoucherViewModel, _loginSession.UserViewModel.UserId, _loginSession.RoleViewModel.CompanyId);

            if (result==true)
            {
                return RedirectToPage("Index");
            }
            else
                return RedirectToPage("Manage");
        }

        public JsonResult OnGetGetCategoriesByBrand([FromQuery] int brandId)
        {
            var categories = _dropdownManager.GetcategoryListByBrandId
                (brandId);

            return new JsonResult(categories);
        }
        public bool IsChecked(string IdsList, string Id)
        {
            try
            {
                if (IdsList != null && IdsList.Count() > 0)

                {
                    string[] IdsListArray = IdsList.Split(',');
                    if (IdsListArray.Contains(Id.ToString()))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// method to get voucher type
        /// </summary>
        /// <param name="selectedVoucherType"></param>
        /// <returns></returns>
        public List<SelectListItem> GetVoucherTypeList(int selectedVoucherType)
        {
            var enumValues = Enum.GetValues(typeof(EcomVoucherTypeEnum)).Cast<EcomVoucherTypeEnum>();

            return enumValues.Select(e => new SelectListItem
            {
                Value = ((int)e).ToString(),
                Text = GetEnumDescription(e), 
                Selected = (int)e == selectedVoucherType
            }).ToList();
        }
        /// <summary>
        /// method to get voucher value type
        /// </summary>
        /// <param name="selectedVoucherType"></param>
        /// <returns></returns>
        public List<SelectListItem> GetVoucherValueTypeList(int selectedvalueType)
        {
            var enumValues = Enum.GetValues(typeof(EcomVoucherValueTypeEnum)).Cast<EcomVoucherValueTypeEnum>();

            return enumValues.Select(e => new SelectListItem
            {
                Value = ((int)e).ToString(),
                Text = GetEnumDescription(e),  
                Selected = (int)e == selectedvalueType
            }).ToList();
        }

        /// <summary>
        /// method to get voucher type enum description value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
            return attribute != null ? attribute.Description : value.ToString();
        }

        [HttpGet]
        public JsonResult OnGetVoucherDuration(int voucherTypeId, DateTime? startDate)
        {
            string vouchername = "";
           // var duration = "";
            DateTime? endate = null;
            if (voucherTypeId == Convert.ToInt32(EcomVoucherTypeEnum.GenericVoucher))
            {
                vouchername = "EcomGeneric";

            }
            else if (voucherTypeId == Convert.ToInt32(EcomVoucherTypeEnum.BrandSpecificVoucher))
                {
                vouchername = "EcomBrandSpecific";
                }
                else if (voucherTypeId == Convert.ToInt32(EcomVoucherTypeEnum.PhoneSpecificVoucher))
            {
                vouchername = "EcomBrandPhoneSpecific";
            }
            if (!string.IsNullOrEmpty(vouchername))
            {
                var durationStr = _context.TblConfigurations
                    .Where(x => x.Name == vouchername && x.IsActive==true)
                    .Select(x => x.Value)
                    .FirstOrDefault();

                if (int.TryParse(durationStr, out int duration))
                {
                    endate = Convert.ToDateTime(startDate).AddDays(Convert.ToInt32(durationStr));
                }
            }

            return new JsonResult(endate);
        }
    }
}
