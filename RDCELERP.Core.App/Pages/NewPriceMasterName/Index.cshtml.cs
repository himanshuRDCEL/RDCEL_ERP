using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.Master;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Pages.NewPriceMasterName
{
    public class IndexModel : CrudBasePageModel
    {

        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IPriceMasterNameRepository _priceMasterNameRepository;
        private readonly IPriceMasterNameManager _priceMasterNameManager;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IPriceMasterNameManager priceMasterNameManager, IPriceMasterNameRepository priceMasterNameRepository, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {

            _context = context;
            _priceMasterNameRepository = priceMasterNameRepository;
            _priceMasterNameManager = priceMasterNameManager;

        }
        [BindProperty(SupportsGet = true)]
        public IList<TblPriceMasterName> TblPriceMasterName { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblPriceMasterName TblPriceMasterNameObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public PriceMasterNameViewModel PriceMasterNameVM { get; set; }


        public IActionResult OnGet()
        {
            TblPriceMasterNameObj = new TblPriceMasterName();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {

                PriceMasterNameVM = _priceMasterNameManager.GetPriceMasterNameById(_loginSession.UserViewModel.UserId);

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
                if (TblPriceMasterNameObj != null && TblPriceMasterNameObj.PriceMasterNameId > 0)
                {
                    TblPriceMasterNameObj = _context.TblPriceMasterNames.Find(TblPriceMasterNameObj.PriceMasterNameId);
                }

                if (TblPriceMasterNameObj != null)
                {
                    TblPriceMasterNameObj.IsActive = false;
                    TblPriceMasterNameObj.ModifiedBy = _loginSession.UserViewModel.UserId;

                    _context.TblPriceMasterNames.Update(TblPriceMasterNameObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
            }
        }

    }
}



