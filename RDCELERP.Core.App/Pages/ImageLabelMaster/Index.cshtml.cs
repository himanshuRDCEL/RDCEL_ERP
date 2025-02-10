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
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.Master;

namespace RDCELERP.Core.App.Pages.ImageLabelMaster
{
    public class IndexModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IImageLabelMasterManager _ImageLabelMasterManager;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IImageLabelMasterManager ImageLabelMasterManager, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
            _ImageLabelMasterManager = ImageLabelMasterManager;
        }
        [BindProperty(SupportsGet = true)]
        public IList<TblImageLabelMaster> TblImageLabelMaster { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblImageLabelMaster TblImageLabelMasterObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public ImageLabelNewViewModel ImageLabelVM { get; set; }


        public IActionResult OnGet()
        {
            TblImageLabelMasterObj = new TblImageLabelMaster();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                ImageLabelVM = _ImageLabelMasterManager.GetImageLabelById(_loginSession.UserViewModel.UserId);

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
                if (TblImageLabelMasterObj != null && TblImageLabelMasterObj.ImageLabelid > 0)
                {
                    TblImageLabelMasterObj = _context.TblImageLabelMasters.Find(TblImageLabelMasterObj.ImageLabelid);
                }

                if (TblImageLabelMasterObj != null)
                {
                    TblImageLabelMasterObj.IsActive = false;
                    TblImageLabelMasterObj.Modifiedby = _loginSession.UserViewModel.UserId;
                    _context.TblImageLabelMasters.Update(TblImageLabelMasterObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
            }
        }
    }
}
