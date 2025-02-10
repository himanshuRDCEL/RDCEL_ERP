using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;

namespace RDCELERP.Core.App.Pages.QCIndex
{
    public class AdminBonusModel : BasePageModel
    {
        #region constructor
        public AdminBonusModel(IOptions<ApplicationSettings> config) : base(config)
        {
        }
        #endregion
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
