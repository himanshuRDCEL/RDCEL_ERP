using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC_Allocated;

namespace RDCELERP.Core.App.Pages.EVC_Allocation
{
    public class ReassignFromModel : BasePageModel
    {
        #region Variable Declaration
        private readonly IEVCManager _EVCManager;
        private readonly IUserManager _UserManager;
        private readonly IStateManager _stateManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICityManager _cityManager;
        private readonly IEntityManager _entityManager;
        public readonly CustomDataProtection _protector;
        public readonly IEVCDisputeManager _eVCDisputeManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        #endregion
        public ReassignFromModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IEVCManager EVCManager, IStateManager StateManager, ICityManager CityManager, IEntityManager EntityManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, IUserManager userManager, CustomDataProtection protector, IEVCDisputeManager EVCDisputeManager)
           : base(config)

        {
            _EVCManager = EVCManager;
            _webHostEnvironment = webHostEnvironment;
            _UserManager = userManager;
            _stateManager = StateManager;
            _cityManager = CityManager;
            _entityManager = EntityManager;
            _eVCDisputeManager = EVCDisputeManager;
            _protector = protector;
            _context = context;

        }
        public TblWalletTransaction tblWalletTransaction { get; set; }
        [BindProperty(SupportsGet = true)]
        public ReassignFromViewModel ReassignFromViewModels { get; set; }

        [BindProperty(SupportsGet = true)]
        public Allocate_EVCFromViewModel Allocate_EVCFromViewModels { get; set; }

        // [BindProperty(SupportsGet = true)]
        public List<EVcListRessign> EVcListReassign = new List<EVcListRessign>();
        public IActionResult OnGet(int OId)
        {
            if (OId > 0)
            {
                ReassignFromViewModels = _EVCManager.GetReassignEVC(OId);

                var EVcLists = _EVCManager.GetEVCListforEVCReassign(ReassignFromViewModels.ActualProdQltyAtQc, (int)ReassignFromViewModels.ProductCatId, ReassignFromViewModels.CustPin, ReassignFromViewModels.OldEvcregistrationId, ReassignFromViewModels.StatusId,ReassignFromViewModels.orderTransId, (int)ReassignFromViewModels.OldEvcExpectedPrice, ReassignFromViewModels.ordertype);
                if (EVcLists != null)
                {
                    ViewData["EVcLists"] = new SelectList((from s in EVcLists.ToList()
                                                           select new
                                                           {
                                                               EvcPartnerId = s.EvcPartnerId,
                                                               EvcregdNo = s.EvcregdNo + "-" + s.BussinessName
                                                           }), "EvcPartnerId", "EvcregdNo", null);
                }           
                #region this code for EVC Price Master and EVC Range Master
                //var tblConfiguration = _context.TblConfigurations.Where(x => x.Name == "UseEVCPriceMater" && x.IsActive == true).FirstOrDefault();
                //if (tblConfiguration != null)
                //{
                //    if (tblConfiguration.Value == "1")
                //    {
                //        TblOrderQc tblOrderQc = _context.TblOrderQcs.Where(x => x.OrderTransId == OId).FirstOrDefault();
                //        if (tblOrderQc != null)
                //        {
                //            Allocate_EVCFromViewModels.ActualProdQltyAtQc = tblOrderQc.QualityAfterQc;
                //            if (Allocate_EVCFromViewModels.ActualProdQltyAtQc == "Excellent")
                //            {
                //                TblEvcPriceMaster tblEvcPriceMaster = _context.TblEvcPriceMasters.Where(x => x.ProductTypeId == tblProductType.Id && x.ProductCategoryId == tblProductCategory.Id).FirstOrDefault();
                //                Allocate_EVCFromViewModels.ExpectedPrice = tblEvcPriceMaster.EvcP;
                //            }
                //            else if (Allocate_EVCFromViewModels.ActualProdQltyAtQc == "Good")
                //            {
                //                TblEvcPriceMaster tblEvcPriceMaster = _context.TblEvcPriceMasters.Where(x => x.ProductTypeId == tblProductType.Id && x.ProductCategoryId == tblProductCategory.Id).FirstOrDefault();
                //                Allocate_EVCFromViewModels.ExpectedPrice = tblEvcPriceMaster.EvcQ;
                //            }
                //            else if (Allocate_EVCFromViewModels.ActualProdQltyAtQc == "Average")
                //            {
                //                TblEvcPriceMaster tblEvcPriceMaster = _context.TblEvcPriceMasters.Where(x => x.ProductTypeId == tblProductType.Id && x.ProductCategoryId == tblProductCategory.Id).FirstOrDefault();
                //                Allocate_EVCFromViewModels.ExpectedPrice = tblEvcPriceMaster.EvcR;
                //            }
                //            else if (Allocate_EVCFromViewModels.ActualProdQltyAtQc == "NotWorking")
                //            {
                //                TblEvcPriceMaster tblEvcPriceMaster = _context.TblEvcPriceMasters.Where(x => x.ProductTypeId == tblProductType.Id && x.ProductCategoryId == tblProductCategory.Id).FirstOrDefault();
                //                Allocate_EVCFromViewModels.ExpectedPrice = tblEvcPriceMaster.EvcS;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        var PriceAfterQC = Convert.ToInt32(Allocate_EVCFromViewModels.ActualBasePrice);
                //        var per = 0;
                //        TblEvcpriceRangeMaster tblEvcpriceRangeMaster = _context.TblEvcpriceRangeMasters.Where(x => x.IsActive == true && x.PriceStartRange <= PriceAfterQC && x.PriceEndRange >= PriceAfterQC).FirstOrDefault();
                //        if (tblEvcpriceRangeMaster != null)
                //        {
                //            per = (int)tblEvcpriceRangeMaster.EvcApplicablePercentage;
                //            Allocate_EVCFromViewModels.ExpectedPrice = ((PriceAfterQC + ((PriceAfterQC / 100) * per)));
                //        }
                //        else
                //        {
                //            Allocate_EVCFromViewModels.ExpectedPrice = (int?)Allocate_EVCFromViewModels.ActualBasePrice;
                //        }
                //    }
                //}
                //else
                //{
                //    Allocate_EVCFromViewModels.ExpectedPrice = (int?)Allocate_EVCFromViewModels.ActualBasePrice;
                //}
                #endregion
            }

            return Page();
        }
        public IActionResult OnPostAsync()
        {
            int result = 0;
            if (ModelState.IsValid)
            {
                result = _EVCManager.SaveReassignEVC(ReassignFromViewModels, _loginSession.UserViewModel.UserId);
                if (result > 0 && result == 34)
                {
                    return RedirectToPage("Assign_Order");
                }
                else if (result > 0 && result == 18)
                {
                    return RedirectToPage("/LGC_Admin/ReadyforPickupList");
                   
                }
                else if (result > 0 && result == 23)
                {
                    return RedirectToPage("/LGC_Admin/SelectLGCPatner");
                   
                }
                else if (result > 0 && (result == 22||result==26||result==44||result==21))
                {
                    return RedirectToPage("/LGC_Admin/TicketGenrate");

                }
                else
                {
                    return RedirectToPage("Home");
                }
            }
            else
                return RedirectToPage("Home");

        }


    }
}




