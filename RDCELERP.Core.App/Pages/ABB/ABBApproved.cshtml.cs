using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.Base;

namespace RDCELERP.Core.App.Pages.ABBRegistration
{
    public class ABBApprovedModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IAbbRegistrationManager _AbbRegistrationManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ABBApprovedModel(IWebHostEnvironment webHostEnvironment, IAbbRegistrationManager AbbRegistrationManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config)
       : base(config)
        {
            _webHostEnvironment = webHostEnvironment;
            _AbbRegistrationManager = AbbRegistrationManager;
            _context = context;
        }
        public void OnGet()
        {

        }


    }
}
