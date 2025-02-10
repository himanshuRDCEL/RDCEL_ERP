using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC;
using RDCELERP.Model.SearchFilters;

namespace RDCELERP.Core.App.Pages.EVC
{
    public class AddEVCWalletBalanceModel : CrudBasePageModel
    {
        #region Variable declartion 
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IEVCManager _EVCManager;
        private readonly ICreditRequestRepository _creditRequestRepository;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        #endregion

        #region Constructor
        public AddEVCWalletBalanceModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IEVCManager EVCManager, CustomDataProtection protector,ICreditRequestRepository creditRequestRepository, IProductCategoryManager productCategoryManager, IProductTypeManager productTypeManager) : base(config, protector)
        
        {
            _EVCManager = EVCManager;
            _context = context;
            _creditRequestRepository = creditRequestRepository;
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
        }
        #endregion

        #region Model Binding

        [BindProperty(SupportsGet = true)]
        public TblEvcregistration TblEvcregistrations { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<EVC_ApprovedModel> EVC_ApprovedModels { get; set; }
        public TblEvcwalletAddition EvcwalletAdditions { get; set; }
        public EVCWalletAdditionViewModel EVCWalletAdditionViewModels { get; set; }
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }
        #endregion
        public void OnGet()
        {
            var ProductGroup = _productCategoryManager.GetAllProductCategory();
            if (ProductGroup != null)
            {
                ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
            }
        }
        public IActionResult OnGetApproveCreditRequestAsync(int Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            TblCreditRequest? tblCreditRequest = _context.TblCreditRequests.Where(x => x.CreditRequestId == Id).FirstOrDefault();

            if (tblCreditRequest != null)
            {
                #region update                                                              
                tblCreditRequest.IsCreditRequestApproved = true;
                tblCreditRequest.CreditRequestApproveUserId = _loginSession.UserViewModel.UserId;
                tblCreditRequest.ModifiedBy = _loginSession.UserViewModel.UserId;
                tblCreditRequest.ModifiedDate = _currentDatetime;
                _creditRequestRepository.Update(tblCreditRequest);
                _creditRequestRepository.SaveChanges();

                #endregion
            }
            return RedirectToPage("EVCWalletRechargeByAdmin");
        }
        #region Product category type
        public JsonResult OnGetProductCategoryTypeAsync()
        {
            var productTypeList = _productTypeManager.GetProductTypeByCategoryId(Convert.ToInt32(searchFilterVM.ProductCatId));
            if (productTypeList != null)
            {
                ViewData["productTypeList"] = new SelectList(productTypeList, "Id", "Description");
            }
            return new JsonResult(productTypeList);
        }

        #endregion

        #region Autopopulate Search Filter for search by RegdNo
        public IActionResult OnGetSearchRegdNo(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            //string searchTerm = term.ToString();

            var data = _context.TblWalletTransactions
            .Where(x => x.RegdNo.Contains(term)
            && (x.StatusId == Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted).ToString()))
            .Select(x => x.RegdNo)
            .ToArray();
            return new JsonResult(data);
        }
        #endregion

        #region Autopopulate Search Filter for search by EVCRegdNo
        public IActionResult OnGetSearchEVCRegdNo(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }

            var data = _context.TblWalletTransactions.Include(x => x.Evcregistration).Where(x => x.Evcregistration != null && x.Evcregistration.EvcregdNo.Contains(term)
              && (x.StatusId == Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted).ToString()))
            .Select(x => x.Evcregistration.EvcregdNo)
            .ToArray().Distinct();
            return new JsonResult(data);
        }
        #endregion
    }
}
