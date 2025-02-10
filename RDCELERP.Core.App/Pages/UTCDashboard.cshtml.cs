using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RDCELERP.DAL.IRepository;
using static Org.BouncyCastle.Math.EC.ECCurve;
using RDCELERP.Common.Helper;

namespace RDCELERP.Core.App.Pages
{
    public class UTCDashboard : BasePageModel
    {
        IBusinessPartnerRepository _businessPartnerRepository;
        IBusinessUnitRepository _businessUnitRepository;
        IUserRepository _userRepository;
        IOptions<ApplicationSettings> _config;
        public UTCDashboard(IOptions<ApplicationSettings> config, IBusinessPartnerRepository businessPartnerRepository, IBusinessUnitRepository businessUnitRepository, IUserRepository userRepository)
        : base(config)
        {
            _businessPartnerRepository = businessPartnerRepository;
            _businessUnitRepository = businessUnitRepository;
            _userRepository = userRepository;
            _config = config;
        }

        [BindProperty(SupportsGet = true)]
        public LoginViewModel LoginViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblBusinessUnit tblBusinessUnit { get; set; }
        public IActionResult OnGet()
        {

            if (_loginSession == null)
            {
                return RedirectToPage("Index");
            }
            else
            {
                TblBusinessPartner tblBusinessPartner = new TblBusinessPartner();
                TblUser tblUser = _userRepository.GetSingle(x=>x.Email == _loginSession.UserViewModel.Email && x.Password == _loginSession.UserViewModel.Password);
                if (tblUser != null)
                {
                    tblUser.Email = SecurityHelper.DecryptString(tblUser.Email, _config.Value.SecurityKey);
                    tblBusinessPartner = _businessPartnerRepository.GetSingle(x => x.Email == tblUser.Email && x.IsActive == true);
                    if(tblBusinessPartner != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.GetSingle(x=>x.BusinessUnitId == tblBusinessPartner.BusinessUnitId && x.IsActive == true);
                    }
                }
                
                return Page();
            }
        }
    }
}
