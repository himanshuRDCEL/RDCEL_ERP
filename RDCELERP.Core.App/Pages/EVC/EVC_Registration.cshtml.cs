using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.City;
using RDCELERP.Model.Company;
using RDCELERP.Model.EVC;
using RDCELERP.Model.Users;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace RDCELERP.Core.App.Pages.EVC
{
    public class EVC_Registration : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly IEVCManager _EVCManager;
        private readonly IUserManager _UserManager;
        private readonly IStateManager _stateManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICityManager _cityManager;
        private readonly IEntityManager _entityManager;
        private readonly Digi2l_DevContext _context;

        #endregion

        #region Constructor
        public EVC_Registration(IEVCManager EVCManager, Digi2l_DevContext context, IStateManager StateManager, ICityManager CityManager, IEntityManager EntityManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, IUserManager userManager, CustomDataProtection protector) : base(config, protector)

        {
            _EVCManager = EVCManager;
            _webHostEnvironment = webHostEnvironment;
            _UserManager = userManager;
            _stateManager = StateManager;
            _cityManager = CityManager;
            _entityManager = EntityManager;
            _context = context;
        }
        #endregion

        #region model binding 
        [BindProperty(SupportsGet = true)]
        public EVC_RegistrationModel evc_RegistrationModel { get; set; }
        public int stateId;
        public List<CityViewModel> citylist = new List<CityViewModel>();
        public UserViewModel UserViewModel { get; set; }

        #endregion
        public IActionResult OnGet(string id)
        {
            if (id != null)
            {
                id = _protector.Decode(id);

                evc_RegistrationModel = _EVCManager.GetEvcByEvcregistrationId(Convert.ToInt32(id));

                //var citylist = _cityManager.GetCityBYStateID(evc_RegistrationModel.StateId);
                //if (citylist != null)
                //{
                //    ViewData["citylistS"] = new SelectList(citylist, "CityId", "Name");
                //}
                var EmployeeList1 = _UserManager.GetUserById(evc_RegistrationModel.EmployeeId);
                if (EmployeeList1 != null)
                {
                    evc_RegistrationModel.EmployeeName = EmployeeList1.FirstName;
                    evc_RegistrationModel.EmployeeEMail = EmployeeList1.Email;
                    evc_RegistrationModel.EmployeeEMail = SecurityHelper.DecryptString(evc_RegistrationModel.EmployeeEMail, _baseConfig.Value.SecurityKey);
                }
                #region get image for edit 
                if (evc_RegistrationModel != null && !string.IsNullOrEmpty(evc_RegistrationModel.CopyofCancelledCheque))
                    evc_RegistrationModel.CopyofCancelledChequeLinkURL = _baseConfig.Value.BaseURL + "DBFiles/EVC/CancelledCheque/" + evc_RegistrationModel.CopyofCancelledCheque;

                if (evc_RegistrationModel != null && !string.IsNullOrEmpty(evc_RegistrationModel.UploadGSTRegistration))
                    evc_RegistrationModel.UploadGSTRegistrationLinkURL = _baseConfig.Value.BaseURL + "DBFiles/EVC/GSTRegistration/" + evc_RegistrationModel.UploadGSTRegistration;


                if (evc_RegistrationModel != null && !string.IsNullOrEmpty(evc_RegistrationModel.EWasteCertificate))
                    evc_RegistrationModel.EWasteCertificateLinkURL = _baseConfig.Value.BaseURL + "DBFiles/EVC/EWasteCertificate/" + evc_RegistrationModel.EWasteCertificate;

                if (evc_RegistrationModel != null && !string.IsNullOrEmpty(evc_RegistrationModel.AadharfrontImage))
                    evc_RegistrationModel.AadharfrontImageLinkURL = _baseConfig.Value.BaseURL + "DBFiles/EVC/AadharfrontImage/" + evc_RegistrationModel.AadharfrontImage;

                if (evc_RegistrationModel != null && !string.IsNullOrEmpty(evc_RegistrationModel.AadharBackImage))
                    evc_RegistrationModel.AadharBackImageLinkURL = _baseConfig.Value.BaseURL + "DBFiles/EVC/AadharBackImage/" + evc_RegistrationModel.AadharBackImage;

                if (evc_RegistrationModel != null && !string.IsNullOrEmpty(evc_RegistrationModel.ProfilePic))
                    evc_RegistrationModel.ProfilePicLinkURL = _baseConfig.Value.BaseURL + "DBFiles/EVC/EVCProfilePic/" + evc_RegistrationModel.ProfilePic;
                #endregion
            }

            //var EmployeeList = _UserManager.GetAllUser();
            //if (EmployeeList != null)
            //{
            //    ViewData["EmployeeList"] = new SelectList((from s in EmployeeList.ToList()
            //        select new
            //        {
            //            UserId = s.UserId,
            //            FullName = s.UserId + "-" + s.FirstName
            //        }), "UserId", "FullName", null);

            //    //var items = UserId + FirstName;
            //  //  ViewData["EmployeeList"] = new SelectList(EmployeeList, "UserId", "FirstName");
            //}
            //var Statelist = _stateManager.GetAllState();

            //if (Statelist != null)
            //{
            //    ViewData["Statelist"] = new SelectList(Statelist, "StateId", "Name");
            //}
            var EntityList = _entityManager.GetAllEntity();
            if (EntityList != null)
            {
                ViewData["EntityList"] = new SelectList(EntityList, "EntityTypeId", "Name");
            }

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                ViewData["BaseUrl"] = _baseConfig.Value.BaseURL;
                return Page();
            }
        }



        public IActionResult OnPostAsync(int AFlag)
        {
            int result = 0;
            if (ModelState.IsValid)
            {
                if (evc_RegistrationModel != null)
                {
                    result = _EVCManager.SaveEVCDetails(evc_RegistrationModel, _loginSession.UserViewModel.UserId);
                    if (result == 2)
                    {
                        if (AFlag == 1)
                        {
                            ViewData["Message"] = "Update Successfully";
                            return RedirectToPage("EVC_NotApproved");

                        }
                        if (AFlag == 2)
                        {
                            ViewData["Message"] = "Update Successfully";
                            return RedirectToPage("EVC_Approved");
                        }
                    }
                    if (result == 1)
                    {
                        ViewData["Message"] = "Created Successfully";
                        return RedirectToPage("EVC_NotApproved");
                    }
                    if (result == 0)
                    {
                        ViewData["Message"] = "Registration Failed";
                        return RedirectToPage("EVC_Registration");
                    }

                    else
                        return RedirectToPage("EVC_Registration");
                }
                return RedirectToPage("EVC_Registration");
            }
            return RedirectToPage("EVC_Registration");
        }


        public JsonResult OnGetCityByStateIdAsync()
        {
            var citylistS = _cityManager.GetCityBYStateID(evc_RegistrationModel.StateId);
            if (citylistS != null)
            {
                ViewData["citylistS"] = new SelectList(citylistS, "CityId", "Name");
            }
            return new JsonResult(citylistS);
        }
        public JsonResult OnGetEmpDetailsByEmpIdAsync()
        {

            return new JsonResult(_UserManager.UserById(evc_RegistrationModel.EmployeeId));


        }
        //Email Validation[FromBody]
        public IActionResult OnPostCheckEmail(string email, int? evcId, int? evcUserId)
        {

            //TblEvcregistration tblEvcregistration = new TblEvcregistration();
            //bool isValid = !_context.TblEvcregistrations.ToList().Exists(p => p.EmailId.Equals(email, StringComparison.CurrentCultureIgnoreCase));
            //return new JsonResult(isValid);
            if (evcId == 0 && evcUserId == 0)
            {
                TblUser TblUser = new TblUser();
                string emailTrimmed = email?.Trim().ToLower();
                var email1 = SecurityHelper.EncryptString(emailTrimmed, _baseConfig.Value.SecurityKey);
                bool isValidUser = !_context.TblUsers.ToList().Exists(p => (p.Email ?? "") == email1);
                if (isValidUser)
                {

                    bool isValidEVC = !_context.TblEvcregistrations.ToList().Exists(p => (p.EmailId ?? "").Equals(email, StringComparison.CurrentCultureIgnoreCase));
                    return new JsonResult(isValidEVC);
                }
                else
                {
                    return new JsonResult(isValidUser);
                }
            }
            else
            {
                TblUser TblUser = new TblUser();
                string emailTrimmed = email?.Trim().ToLower();
                var email1 = SecurityHelper.EncryptString(emailTrimmed, _baseConfig.Value.SecurityKey);
                bool isValidUser = !_context.TblUsers.Where(x => x.UserId != evcUserId).ToList().Exists(p => (p.Email ?? "") == email1);
                if (isValidUser)
                {
                    // bool isValidEVC = !_context.TblEvcregistrations.ToList().Exists(p => (p.EmailId ?? "").Equals(email, StringComparison.CurrentCultureIgnoreCase) && p.EvcregistrationId != evcId);
                    bool isValidEVC = !_context.TblEvcregistrations.Where(x => x.EvcregistrationId != evcId).ToList().Exists(p => (p.EmailId ?? "").Equals(email, StringComparison.CurrentCultureIgnoreCase));

                    return new JsonResult(isValidEVC);
                }
                else
                {
                    return new JsonResult(isValidUser);
                }
            }
        }

        #region AutoComplete and Validation for EVC EmployeeName, City, State Added By PJ
        #region AutoFill To Get EVC Employee List Added By PJ
        public IActionResult OnGetSearchevcemployee(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblUsers
                       .Where(e => e.FirstName.Contains(term) || term == "#" && e.IsActive == true)
                        .Select(e => new
                        {
                            Value = e.FirstName,
                            Fullname = e.UserId.ToString() + "-" + e.FirstName
                        })
                       .Select(e => e.Fullname)
                       .ToArray();
            return new JsonResult(data);
        }
        #endregion

        #region AutoFill To Get EVC State List Added By PJ
        public IActionResult OnGetSearchEVCState(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblStates
                       .Where(s => s.Name.Contains(term) || term == "#" && s.IsActive == true)
                       .Select(s => new SelectListItem
                       {
                           Value = s.Name,
                           Text = s.StateId.ToString()
                       })
                       .ToArray();
            return new JsonResult(data);
        }
        #endregion

        #region AutoFill To Get EVC City List Added By PJ
        public IActionResult OnGetSearchEVCCity(string term, string term2)
        {
            //IList <string> list = null;           

            if (term == null)
            {
                return BadRequest();
            }
            term = term.TrimStart(); ///use to trim blank space before character

            if (term == "#")
            {
                //list = _context.TblCities
                //     .Where(e => e.StateId == Convert.ToInt32(term2))
                //     .Select(e => e.Name)
                //     .ToArray();
                var list = _context.TblCities
                     .Where(s => s.StateId == Convert.ToInt32(term2) && s.IsActive == true)
                     .Select(s => new SelectListItem
                     {
                         Value = s.Name,
                         Text = s.CityId.ToString()
                     })
                     .ToArray();
                return new JsonResult(list);
            }
            else
            {
                var list = _context.TblCities
                        .Where(e => e.Name.Contains(term) && e.StateId == Convert.ToInt32(term2) && e.IsActive == true)
                        .Select(s => new SelectListItem
                        {
                            Value = s.Name,
                            Text = s.CityId.ToString()
                        })
                        .ToArray();
                return new JsonResult(list);
            }

            //return new JsonResult(list);            
        }
        #endregion
        #region AutoFill To Get EVC Pincode List Added By priyanshi
        public IActionResult OnGetAutoPinCode(int term, int term2)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var list = _context.TblPinCodes
                       .Where(e => e.ZipCode.ToString().Contains(term.ToString()) && e.CityId == term2 && e.IsActive == true)
                        .Select(s => new SelectListItem
                        {
                            Value = s.ZipCode.ToString(),
                            Text = s.Id.ToString()
                        })
                       .ToArray();
            return new JsonResult(list);
        }
        #endregion

        #region Check City is Valid Added By PJ
        public IActionResult OnGetCheckCityisValid(string cityname, int StateId)
        {
            IList<string> list = null;
            if (cityname == null)
            {
                return BadRequest();
            }
            else
            {
                cityname = cityname.TrimStart();
                list = _context.TblCities
                           .Where(e => e.StateId == StateId && e.Name.Contains(cityname))
                           .Select(e => e.Name)
                           .ToArray();
                if (list.Count == 0)
                {
                    list = null;
                }
            }
            return new JsonResult(list);
        }
        #endregion

        #region Check State is Valid Added By PJ
        public IActionResult OnGetCheckStateisValid(string Statename)
        {
            IList<string> list = null;
            if (Statename == null)
            {
                return BadRequest();
            }
            else
            {
                Statename = Statename.TrimStart();
                list = _context.TblStates
                           .Where(e => e.Name.Contains(Statename))
                           .Select(e => e.Name)
                           .ToArray();
                if (list.Count == 0)
                {
                    list = null;
                }
            }
            return new JsonResult(list);
        }
        #endregion

        #region Check Employee Name is Valid Added By PJ
        public IActionResult OnGetCheckEmpNameValid(string Empname)
        {
            IList<string> list = null;
            if (Empname == null)
            {
                return BadRequest();
            }
            else
            {
                Empname = Empname.TrimStart();

                var splitArray = Empname.Split('-');
                var Employeeid = splitArray[1];
                list = _context.TblUsers
                           .Where(e => e.FirstName.Contains(Employeeid))
                           .Select(e => new
                           {
                               Value = e.FirstName,
                               Fullname = e.UserId.ToString() + "-" + e.FirstName
                           })
                       .Select(e => e.Fullname)
                           //.Select(e => e.Name)
                           .ToArray();
                if (list.Count == 0)
                {
                    list = null;
                }
            }
            return new JsonResult(list);
        }
        #endregion
        #endregion
    }
}

