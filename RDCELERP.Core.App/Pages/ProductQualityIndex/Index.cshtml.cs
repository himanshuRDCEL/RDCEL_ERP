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
using RDCELERP.Model.ProductQuality;

namespace RDCELERP.Core.App.Pages.ProductQualityIndex
{
    public class IndexModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IProductQualityIndexManager _ProductQualityIndexManager;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IProductQualityIndexManager ProductQualityIndexManager, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
            _ProductQualityIndexManager = ProductQualityIndexManager;
        }

        [BindProperty(SupportsGet = true)]
        public ProductQualityIndexViewModel ProductQualityIndexVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<TblProductQualityIndex> TblProductQualityIndex { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblProductQualityIndex TblProductQualityIndexObj { get; set; }
        public IActionResult OnGet()
        {
            TblProductQualityIndexObj = new TblProductQualityIndex();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                ProductQualityIndexVM = _ProductQualityIndexManager.GetProductQualityIndexById(_loginSession.UserViewModel.UserId);

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
                if (TblProductQualityIndexObj != null && TblProductQualityIndexObj.ProductQualityIndexId > 0)
                {
                    TblProductQualityIndexObj = _context.TblProductQualityIndices.Find(TblProductQualityIndexObj.ProductQualityIndexId);
                }

                if (TblProductQualityIndexObj != null)
                {
                    if (TblProductQualityIndexObj.IsActive == true)
                    {
                        TblProductQualityIndexObj.IsActive = false;
                    }
                    else
                    {
                        TblProductQualityIndexObj.IsActive = true;
                    }
                    
                    TblProductQualityIndexObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblProductQualityIndices.Update(TblProductQualityIndexObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
            }
        }
    }
}
