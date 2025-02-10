using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC;
using RDCELERP.Model.EVC_Allocated;
using RDCELERP.Model.EVC_Portal;

namespace RDCELERP.Core.App.Pages.EVC_Allocation
{
    public class Allocate_EVCFromModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IEVCManager _EVCManager;
        public Allocate_EVCFromModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IEVCManager EVCManager)
      : base(config)
        {
            _EVCManager = EVCManager;
            _context = context;
        }
        //[BindProperty(SupportsGet = true)]
        //public String MyIds { get; set; }
        public TblWalletTransaction tblWalletTransaction { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<Allocate_EVCFromViewModel> Allocate_EVCFromViewModels { get; set; }

        [BindProperty(SupportsGet = true)]
        public Allocate_EVCFromViewModel Allocate_EVCFromViewModel { get; set; }

        public IActionResult OnGet(string ids)
        {
            if (ids != null)
            {
                Allocate_EVCFromViewModel.NotAllocatelistids = ids;
                Allocate_EVCFromViewModels = _EVCManager.ListOfEVCBycity(ids);
            }
            else
            {
                return Page();
            }

            return Page();
        }


        public IActionResult OnPostAsync()
        {
            string NotAssignRNumber;
            //list string result;
            if (ModelState.IsValid)
            {
                NotAssignRNumber = _EVCManager.AllocateEVCByOrder(Allocate_EVCFromViewModels, _loginSession.UserViewModel.UserId);
                if (NotAssignRNumber != null && NotAssignRNumber != " ")
                {
                    return RedirectToPage("Not_Allocated", new { ReturnList = NotAssignRNumber });
                    //return RedirectToPage("Not_Allocated");
                }
                else
                {
                    return RedirectToPage("Not_Allocated", new { ReturnList = "Success" });
                }
            }
            else
                return RedirectToPage("Not_Allocated");

        }
        public JsonResult OnGetEVCDetailsByEVCRegNoAsync(string evcPartnerId,int? orderTransId)
        {
            EVC_PartnerViewModel eVcPartnerList = null;
            if (!string.IsNullOrEmpty(evcPartnerId))
            {
                eVcPartnerList = _EVCManager.GetEVCByEVCRegNo(Convert.ToInt32(evcPartnerId), orderTransId);
                return new JsonResult(eVcPartnerList);
            }
            else
            {
                return new JsonResult(eVcPartnerList);
            }
        }

        //public IActionResult OnGetSearchEVCList(string term, string term2)
        //{
        //    List<KeyValuePair<string, string>> EVCList = new List<KeyValuePair<string, string>>();
        //    string value = string.Empty;
        //    string key = string.Empty;

        //    if (term == null)
        //    {
        //        return BadRequest();
        //    }
        //    var data = _EVCManager.ListOfEVCBycity(term2);

        //    foreach (var list in data[0].EVCLists)
        //    {
        //        if(term == "#")
        //        {
        //            value = list.CombinedDisplay.ToString();
        //            key = list.EvcregistrationId.ToString();
        //            EVCList.Add(new KeyValuePair<string, string>(key, value));
        //        }
        //        else
        //        {
        //            var item = list.CombinedDisplay.Contains(term, StringComparison.OrdinalIgnoreCase);
        //            if (item == true)
        //            {
        //                value = list.CombinedDisplay.ToString();
        //                key = list.EvcregistrationId.ToString();
        //                EVCList.Add(new KeyValuePair<string, string>(key, value));
        //            }
        //        }

        //    }
        //    return new JsonResult(EVCList);
        //}
    }
}
