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
using RDCELERP.Model.Master;
using RDCELERP.Model.ProductConditionLabel;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Pages.ProductConditionLabel
{
    public class IndexModel : BasePageModel
    {

        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IProductConditionLabelManager _ProductConditionLabelManager;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IProductConditionLabelManager ProductConditionLabelManager, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
            _ProductConditionLabelManager = ProductConditionLabelManager;
        }
        [BindProperty(SupportsGet = true)]
        public IList<TblProductConditionLabel> TblProductConditionLabel { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblProductConditionLabel TblProductConditionLabelObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public ProductConditionLabelViewModel ProductConditionLabelVM { get; set; }


        public IActionResult OnGet()
        {
            TblProductConditionLabelObj = new TblProductConditionLabel();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                ProductConditionLabelVM = _ProductConditionLabelManager.GetProductConditionLabelById(_loginSession.UserViewModel.UserId);

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
                if (TblProductConditionLabelObj != null && TblProductConditionLabelObj.Id > 0)
                {
                    TblProductConditionLabelObj = _context.TblProductConditionLabels.Find(TblProductConditionLabelObj.Id);
                }

                if (TblProductConditionLabelObj != null)
                {

                    if (TblProductConditionLabelObj.IsActive == true)
                    {
                        TblProductConditionLabelObj.IsActive = false;
                    }
                    else
                    {
                        TblProductConditionLabelObj.IsActive = true;
                    }
                    TblProductConditionLabelObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblProductConditionLabels.Update(TblProductConditionLabelObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
            }
        }

    }
}



