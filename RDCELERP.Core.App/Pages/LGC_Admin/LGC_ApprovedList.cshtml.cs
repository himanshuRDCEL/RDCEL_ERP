using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Core.App.Pages.EVC;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC;

namespace RDCELERP.Core.App.Pages.LGC_Admin
{
    public class LGC_ApprovedListModel : BasePageModel
    {
        #region Variable declartion
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
       
        #endregion

        # region Constructor
        public LGC_ApprovedListModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config)
        {             
            _context = context;
        }
        #endregion

        #region Model Binding
        [BindProperty(SupportsGet = true)]
        public TblServicePartner TblServicePartners { get; set; }
        
         DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion

        public async Task OnGetAsync()
        {

        }

        //This Method use for Delet EVC
        public IActionResult OnPostDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                if (TblServicePartners != null && TblServicePartners.ServicePartnerId > 0)
                {
                    TblServicePartners = _context.TblServicePartners.Find(TblServicePartners.ServicePartnerId);
                    if (TblServicePartners != null)
                    {
                        TblServicePartners.IsActive = false;
                        TblServicePartners.ServicePartnerIsApprovrd = false;                      
                       // TblServicePartners.ModifiedBy = _loginSession.UserViewModel.UserId;
                        TblServicePartners.ModifiedDate = _currentDatetime;
                        _context.TblServicePartners.Update(TblServicePartners);
                        _context.SaveChanges();
                    }
                }
                return RedirectToPage("./LGC_ApprovedList/");
            }
        }
        
    }
}
