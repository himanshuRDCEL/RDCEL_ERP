using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RDCELERP.DAL.Entities;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.BAL.Interface;
using RDCELERP.Model.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using RDCELERP.Model.Company;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;


namespace RDCELERP.Core.App.Pages.BusinessCustomer
{
    public class CartModel : BasePageModel
    { 
        public CartModel( IOptions<ApplicationSettings> config)
 : base(config)
    {
        
    }

    public void OnGet()
        {
        }
    }
}
