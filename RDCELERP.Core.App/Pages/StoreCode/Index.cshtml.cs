using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;

namespace RDCELERP.Core.App.Pages.StoreCode
{
    public class IndexModel : BasePageModel
    {


        public IndexModel(IOptions<ApplicationSettings> config) : base(config)
        {
        }


        public async Task OnGetAsync()
        {

        }
    }
}



