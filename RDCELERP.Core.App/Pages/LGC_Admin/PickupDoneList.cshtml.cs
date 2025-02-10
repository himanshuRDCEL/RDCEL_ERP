using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.Base;

namespace RDCELERP.Core.App.Pages.LGC_Admin
{
    public class PickupDoneListModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;


        public PickupDoneListModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config)
      : base(config)
        {

            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public int? ServiceId { get; set; }
        public void OnGet(int ServicePartnerId)
        {
            if (ServicePartnerId != null)
            {
                ServiceId = ServicePartnerId;
            }
            else
            {
                ServiceId = null;
            }
        }
     
    }
}
