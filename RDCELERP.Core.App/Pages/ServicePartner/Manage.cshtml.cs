using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.ServicePartner;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Pages.ServicePartner
{
    public class ManageModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly IServicePartnerManager _servicePartnerManager;
        private readonly IStateManager _stateManager;
        private IWebHostEnvironment _webHostEnvironment;
        public readonly IOptions<ApplicationSettings> _config;
        private readonly ICityManager _cityManager;
        private readonly IPinCodeManager _pinCodeManager;
        private readonly IUserManager _userManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        #endregion

        public ManageModel(IServicePartnerManager servicePartnerManager,  IWebHostEnvironment webHostEnvironment, IPinCodeManager pinCodeManager, ICityManager cityManager, IStateManager stateManager, Digi2l_DevContext context, IUserManager userManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _servicePartnerManager = servicePartnerManager;
            _userManager = userManager;
            _stateManager = stateManager;
            _webHostEnvironment = webHostEnvironment;
            _pinCodeManager = pinCodeManager;
            _cityManager = cityManager;
            _context = context;
            _config = config;
        }

        [BindProperty(SupportsGet = true)]
        public ServicePartnerViewModel ServicePartnerViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<ServicePartnerViewModel> ServicePartnerVMList { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool Editmode { get; set; } = false;


        public IActionResult OnGet(string id)
        {
            var statelist = _stateManager.GetAllState();
            if (statelist != null)
            {
                statelist = statelist.OrderBy(s => s.Name).ToList();
                ViewData["statelist"] = new SelectList(statelist, "Name", "Name");
            }

            if (id != null)
            {
                id = _protector.Decode(id);
                ServicePartnerViewModel = _servicePartnerManager.GetServicePartnerById(Convert.ToInt32(id));
                if (ServicePartnerViewModel != null)
                {
                    if (ServicePartnerViewModel.IsActive == true && ServicePartnerViewModel.ServicePartnerIsApprovrd == true)
                    {
                        Editmode = true;
                    }
                }
                IList<MapServicePartnerCityState> MapServicePartnerCityStatelist = null;
                MapServicePartnerCityStatelist = _context.MapServicePartnerCityStates
    .Where(p => p.ServicePartnerId == Convert.ToInt32(id)).ToList();


                List<string> Cities = new List<string>();
                List<string> States = new List<string>();
                var cityListS = new List<SelectListItem>();
                foreach (var item in MapServicePartnerCityStatelist)
                {
                    var city = _context.TblCities.FirstOrDefault(p => p.CityId == item.CityId);
                    if (city != null)
                    {
                        cityListS.Add(new SelectListItem { Value = city.Name, Text = city.Name });
                    }
                    string[] CityForItem = _context.TblCities
                        .Where(p => p.CityId == item.CityId)
                        .Select(p => p.Name.ToString())
                        .ToArray();
                    Cities.AddRange(CityForItem); // concatenate the results for this item


                    string[] StateForItem = _context.TblStates
                        .Where(p => p.StateId == item.StateId)
                        .Select(p => p.Name.ToString())
                        .ToArray();
                    States.AddRange(StateForItem); // concatenate the results for this item
                }
                ViewData["citylistS"] = new SelectList(cityListS, "Value", "Text");

                // convert the list to an array and store it in ServicePartnerViewModel
                ServicePartnerViewModel.ServicePartnerCities = Cities.ToArray();
                ServicePartnerViewModel.ServicePartnerStates = States.ToArray();

                if (!string.IsNullOrEmpty(ServicePartnerViewModel.ServicePartnerCancelledCheque))
                {

                    ServicePartnerViewModel.CancelledChequeURL = _baseConfig.Value.BaseURL + "/DBFiles/ServicePartner/CancelledCheque/" + ServicePartnerViewModel.ServicePartnerCancelledCheque;
                }
                if (!string.IsNullOrEmpty(ServicePartnerViewModel.ServicePartnerGstregisteration))
                {
                    ServicePartnerViewModel.GSTURL = _baseConfig.Value.BaseURL + "/DBFiles/ServicePartner/GST/" + ServicePartnerViewModel.ServicePartnerGstregisteration;
                }
                if (!string.IsNullOrEmpty(ServicePartnerViewModel.ServicePartnerAadharfrontImage))
                {

                    ServicePartnerViewModel.AadharFrontURL = _baseConfig.Value.BaseURL + "/DBFiles/ServicePartner/Aadhar/" + ServicePartnerViewModel.ServicePartnerAadharfrontImage;
                }
                if (!string.IsNullOrEmpty(ServicePartnerViewModel.ServicePartnerAadharBackImage))
                {
                    ServicePartnerViewModel.AadharBackURL = _baseConfig.Value.BaseURL + "/DBFiles/ServicePartner/Aadhar/" + ServicePartnerViewModel.ServicePartnerAadharBackImage;
                }

                if (!string.IsNullOrEmpty(ServicePartnerViewModel.ServicePartnerProfilePic))
                {

                    ServicePartnerViewModel.ProfilePicURL = _baseConfig.Value.BaseURL + "/DBFiles/ServicePartner/ProfilePic/" + ServicePartnerViewModel.ServicePartnerProfilePic;
                }
            }

            //ServicePartnerViewModel = _servicePartnerManager.GetServicePartnerById(Convert.ToInt32(id));

            if (ServicePartnerViewModel == null)
                ServicePartnerViewModel = new ServicePartnerViewModel();
            if (!string.IsNullOrEmpty(ServicePartnerViewModel.ServicePartnerCancelledCheque))
            {

                ServicePartnerViewModel.CancelledChequeURL = _baseConfig.Value.BaseURL + "/DBFiles/ServicePartner/CancelledCheque/" + ServicePartnerViewModel.ServicePartnerCancelledCheque;
            }
            if (!string.IsNullOrEmpty(ServicePartnerViewModel.ServicePartnerGstregisteration))
            {
                ServicePartnerViewModel.GSTURL = _baseConfig.Value.BaseURL + "/DBFiles/ServicePartner/GST/" + ServicePartnerViewModel.ServicePartnerGstregisteration;
            }
            if (!string.IsNullOrEmpty(ServicePartnerViewModel.ServicePartnerAadharfrontImage))
            {

                ServicePartnerViewModel.AadharFrontURL = _baseConfig.Value.BaseURL + "/DBFiles/ServicePartner/Aadhar/" + ServicePartnerViewModel.ServicePartnerAadharfrontImage;
            }
            if (!string.IsNullOrEmpty(ServicePartnerViewModel.ServicePartnerAadharBackImage))
            {
                ServicePartnerViewModel.AadharBackURL = _baseConfig.Value.BaseURL + "/DBFiles/ServicePartner/Aadhar/" + ServicePartnerViewModel.ServicePartnerAadharBackImage;
            }

            if (!string.IsNullOrEmpty(ServicePartnerViewModel.ServicePartnerProfilePic))
            {

                ServicePartnerViewModel.ProfilePicURL = _baseConfig.Value.BaseURL + "/DBFiles/ServicePartner/ProfilePic/" + ServicePartnerViewModel.ServicePartnerProfilePic;
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

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(IFormFile CencelledCheque, IFormFile GST, IFormFile AadharFront, IFormFile AadharBack, IFormFile ProfilePic)
        {
            if (ServicePartnerViewModel.ServicePartnerId == 0)
            {
                ServicePartnerViewModel.ServicePartnerRegdNo = "SP" + UniqueString.RandomNumber();
            }

            int result = 0;
            if (CencelledCheque != null)
            {
                string fileName = ServicePartnerViewModel.ServicePartnerRegdNo + "CancelledCheck" + ".jpg";
                var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"\DBFiles\ServicePartner\CancelledCheque");
                var fileNameWithPath = string.Concat(filePath, "\\", fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    CencelledCheque.CopyTo(stream);

                    ServicePartnerViewModel.ServicePartnerCancelledCheque = fileName;
                }

            }
            if (GST != null)
            {
                string fileName = ServicePartnerViewModel.ServicePartnerRegdNo + "ServicePartner_GST" + ".jpg";
                var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"\DBFiles\ServicePartner\GST");
                var fileNameWithPath = string.Concat(filePath, "\\", fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    GST.CopyTo(stream);

                    ServicePartnerViewModel.ServicePartnerGstregisteration = fileName;
                }

            }

            if (AadharFront != null)
            {
                string fileName = ServicePartnerViewModel.ServicePartnerRegdNo + "AadharFront" + ".jpg";
                var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"\DBFiles\ServicePartner\Aadhar");
                var fileNameWithPath = string.Concat(filePath, "\\", fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    AadharFront.CopyTo(stream);

                    ServicePartnerViewModel.ServicePartnerAadharfrontImage = fileName;
                }

            }
            if (AadharBack != null)
            {
                string fileName = ServicePartnerViewModel.ServicePartnerRegdNo + "AadharBack" + ".jpg";
                var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"\DBFiles\ServicePartner\Aadhar");
                var fileNameWithPath = string.Concat(filePath, "\\", fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    AadharBack.CopyTo(stream);

                    ServicePartnerViewModel.ServicePartnerAadharBackImage = fileName;
                }

            }

            if (ProfilePic != null)
            {
                string fileName = ServicePartnerViewModel.ServicePartnerRegdNo + "ProfilePic" + ".jpg";
                var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"\DBFiles\ServicePartner\ProfilePic");
                var fileNameWithPath = string.Concat(filePath, "\\", fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    ProfilePic.CopyTo(stream);

                    ServicePartnerViewModel.ServicePartnerProfilePic = fileName;
                }

            }

            result = _servicePartnerManager.ManageSevicePartner(ServicePartnerViewModel, _loginSession.UserViewModel.UserId);
            //if(result == 0)
            //{
                

            //    var statelist = _stateManager.GetAllState();
            //    if (statelist != null)
            //    {
            //        statelist = statelist.OrderBy(s => s.Name).ToList();
            //        ViewData["statelist"] = new SelectList(statelist, "Name", "Name");
            //    }
            //    ViewData["Msg"] = "Please dont use existing Email and Mobile Number";
            //    return Page();
            //}


            if (result > 0)
                return RedirectToPage("./Index", new { id = _protector.Encode(result) });
            else

                return RedirectToPage("./Manage");
        }


        public JsonResult OnGetCityByStateIdAsync()
        {
            var citylistS = _cityManager.GetCityByStatesBulk(ServicePartnerViewModel.ServicePartnerState);
            if (citylistS != null)
            {
                citylistS = citylistS.OrderBy(s => s.Name).ToList();
                ViewData["citylistS"] = new SelectList(citylistS, "Name", "Name");
            }
            return new JsonResult(citylistS);

        }
        //public IActionResult OnPostCheckName(string Name)
        //{
        //      TblServicePartner TblServicePartner = new TblServicePartner();
        //      bool isValid = !_context.TblServicePartners.ToList().Exists(p => p.ServicePartnerFirstName == Name);
            
        //    return new JsonResult(isValid);
        //}
       
        public IActionResult OnPostCheckPhone(string Phone)
        {
            TblServicePartner TblServicePartner = new TblServicePartner();
            bool isValid = !_context.TblServicePartners.ToList().Exists
                        (p => p.ServicePartnerMobileNumber == Phone);

            return new JsonResult(isValid);

        }

        public IActionResult OnPostCheckUserEmailId(string EmailId)
        {
            TblServicePartner TblServicePartner = new TblServicePartner();
            bool isValid = !_context.TblServicePartners.ToList().Exists(p => p.ServicePartnerEmailId == EmailId);
            dynamic response = null;

            if (isValid == true)
            {
                var Email = SecurityHelper.EncryptString(EmailId, _config.Value.SecurityKey);
                var UserList = _context.TblUsers.Where(p => p.Email == Email).ToList();

                if (UserList.Count > 0)
                {
                    var roles = _context.TblUserRoles.Where(x => x.UserId == UserList[0].UserId).FirstOrDefault();
                    var role = _context.TblRoles.Where(x => x.RoleId == roles.RoleId).FirstOrDefault();
                    if (role.RoleName == "EVC Portal")
                    {
                        response = new
                        {
                            Value = role.RoleName,

                        };
                        response = new
                        {
                            Value = "EVCExists",
                            Message = "This email is already in use with a user. The role of this user is: " + role.RoleName + " do you want to change the Role with EVC&LGC? If you dont want to change the role then please enter another email."
                        };

                    }

                   else if (role.RoleName != "EVC Portal" && UserList != null)
                    {
                        response = new
                        {
                            Value = role.RoleName,

                        };

                        // User with matching email exists
                        response = new
                        {
                            Value = "UserExists",
                            Message = "This email is already in use with a user. The role of this user is: " + role.RoleName + "please enter another email."
                        };

                    }
                   
                }
                else
                {
                 
                    // User with matching email exists
                    response = new
                    {
                        Value = "NoUserExists",
                        Message = " "
                    };
                }
                
            }
            else
            {
               
                    // Email exists in service partner table but no user found
                    response = new
                    {
                        Value = "ServicePartnerExists",
                        Message = "This email is already in registered with a service partner. Please use another email."
                    };
                
            }
           
            return new JsonResult(response);
        }
        public IActionResult OnPostCheckUserPhone(string phone)
        {
            TblServicePartner TblServicePartner = new TblServicePartner();
            bool isValid = !_context.TblServicePartners.ToList().Exists(p => p.ServicePartnerMobileNumber == phone);
            dynamic response = null;

            if (isValid == true)
            {
                var Phone = SecurityHelper.EncryptString(phone, _config.Value.SecurityKey);
                var UserList = _context.TblUsers.Where(p => p.Phone == Phone).ToList();

                if (UserList.Count > 0)
                {
                    var roles = _context.TblUserRoles.Where(x => x.UserId == UserList[0].UserId).FirstOrDefault();
                    var role = _context.TblRoles.Where(x => x.RoleId == roles.RoleId).FirstOrDefault();
                    if (role.RoleName == "EVC Portal")
                    {
                        response = new
                        {
                            Value = role.RoleName,

                        };
                        response = new
                        {
                            Value = "EVCExists",
                            Message = "This Mobile No. is already in use with a user. The role of this user is: " + role.RoleName + " do you want to change the Role with EVC&LGC? If you dont want to change the role then please enter another Mobile No."
                        };

                    }
                    else if(role.RoleName != "EVC Portal" && UserList != null)
                    {
                        response = new
                        {
                            Value = role.RoleName,

                        };

                        // User with matching email exists
                        response = new
                        {
                            Value = "UserExists",
                            Message = "This Mobile No. is already in use with a user. The role of this user is: " + role.RoleName + " please enter another Mobile No."
                        };

                    }

                }
                else
                {

                    // User with matching email exists
                    response = new
                    {
                        Value = "NoUserExists",
                        Message = " "
                    };
                }

            }
            else
            {
                // Email exists in service partner table but no user found
                response = new
                {
                    Value = "ServicePartnerExists",
                    Message = "This Mobile No. is already in use with a service partner."
                };
            }

            return new JsonResult(response);
        }
        public IActionResult OnGetCheckYes()
        {

            ServicePartnerViewModel servicePartnerViewModel = new ServicePartnerViewModel();
            servicePartnerViewModel.Selected = true;

            return new JsonResult(servicePartnerViewModel.Selected);

        }

        public IActionResult OnGetSearch(int? term)
        {
            //var options = new[] { "Option 1", "Option 2", "Option 3" };
            //var filteredOptions = options.Where(o => o.Contains(term));
            //return new JsonResult(filteredOptions);
            //if (term == null)
            //{
            //    return BadRequest();
            //}

            //string searchTerm = term.ToString();

            //var data = _context.TblPinCodes
            //    .Where(p => p.ZipCode.ToString().Contains(searchTerm))
            //    .Select(p => p.ZipCode.ToString())
            //    .ToArray();
            if (term == null)
            {
                return BadRequest();
            }

            string searchTerm = term.ToString();

            var data = _context.TblPinCodes
                .Where(p => p.ZipCode.ToString().Contains(searchTerm))
                .Select(p => p.ZipCode.ToString())
                .ToArray();

            return new JsonResult(data);
        }

        public IActionResult OnGetSearchState(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }

            //string searchTerm = term.ToString();

            var data = _context.TblStates
                .Where(p => p.Name.Contains(term))
                .Select(p => p.Name)
                .ToArray();

            return new JsonResult(data);
        }

        public IActionResult OnGetSearchCity(string term)
        {

            if (term == null)
            {
                return BadRequest();
            }
            else
            {
                var data = _context.TblCities
               .Where(p => p.Name.Contains(term))
               .Select(p => p.Name)
               .ToArray();
                var abc = data;
                return new JsonResult(abc);
            }

            //string searchTerm = term;

           
        }
    }
}
