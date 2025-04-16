using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.EcomVoucher;
using RDCELERP.Model.Master;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Pages.EcomVoucher
{
    public class IndexModel : BasePageModel
    {

        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public IList<TblEcomVoucher> TblEcomVoucher { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblEcomVoucher TblEcomVoucherObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public EcomVoucherViewModel EcomVoucherVM { get; set; }


        public IActionResult OnGet()
        {
            TblEcomVoucherObj = new TblEcomVoucher();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
               

                return Page();
            }
        }
    }
}
