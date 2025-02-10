using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.ServicePartner;

namespace RDCELERP.Core.App.Pages.ServicePartner
{
    public class IndexModel : CrudBasePageModel
    {
       
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IServicePartnerManager _servicePartnerManager;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IServicePartnerManager servicePartnerManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)

        {
            _context = context;
            _servicePartnerManager = servicePartnerManager;
        }



        [BindProperty(SupportsGet = true)]
        public ServicePartnerViewModel ServicePartnerVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<TblServicePartner> TblServicePartner { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblServicePartner TblServicePartnerObj { get; set; }

        public IActionResult OnGet()
        {
            TblServicePartnerObj = new TblServicePartner();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                ServicePartnerVM = _servicePartnerManager.GetServicePartnerById(_loginSession.UserViewModel.UserId);

                return Page();
            }
        }

        public IActionResult OnPostDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                if (TblServicePartnerObj != null && TblServicePartnerObj.ServicePartnerId > 0)
                {
                    TblServicePartnerObj = _context.TblServicePartners.Find(TblServicePartnerObj.ServicePartnerId);
                }

                if (TblServicePartnerObj != null)
                {
                    TblServicePartnerObj.IsActive = false;
                    TblServicePartnerObj.Modifiedby = _loginSession.UserViewModel.UserId;
                    _context.TblServicePartners.Update(TblServicePartnerObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
            }
        }

    }
}
