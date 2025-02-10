using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.SearchFilters;

namespace RDCELERP.Core.App.Pages.LGC_Admin
{
    public class TicketGenrateModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IEVCManager _EVCManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly ICreditRequestRepository _creditRequestRepository;

        public TicketGenrateModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IEVCManager EVCManager, IProductCategoryManager productCategoryManager,ICreditRequestRepository creditRequestRepository)
      : base(config)
        {
            _EVCManager = EVCManager;
            _context = context;
            _productCategoryManager = productCategoryManager;
            _creditRequestRepository= creditRequestRepository;
        }
        [BindProperty(SupportsGet = true)]
        public string ListNA { get; set; }
        [BindProperty(SupportsGet = true)]
        public string MyIds { get; set; }

        [BindProperty(SupportsGet = true)]
        public string text { get; set; }

        [BindProperty(SupportsGet = true)]
        public string title { get; set; }
        public int UserId { get; set; }
        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }

        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        public void OnGet(string? ReturnList)
        {
            if (ReturnList != null)
            {
                ListNA = ReturnList;
                if (ReturnList == "Success")
                {
                    title = "Successfully Assign";
                    text = "Order Assign Successfully";
                    ReturnList = null;
                }
                else
                {
                    title = "this Orders Not Assign";
                    text = ReturnList;
                    ReturnList = null;
                }
            }
            // Get Product Category List
            var ProductGroup = _productCategoryManager.GetAllProductCategory();
            if (ProductGroup != null)
            {
                ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
            }
        }
        public IActionResult OnPostExportAsync()
        {
            try
            {
                string ids = MyIds;

            }
            catch (Exception ex)
            {

                throw;
            }

            return RedirectToPage("TicketGenratefrom", new { ids = MyIds });
        }

        #region Autopopulate Search Filter for search by RegdNo
        public IActionResult OnGetSearchRegdNo(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            //string searchTerm = term.ToString();

            var data = _context.TblExchangeOrders
            .Where(x => (x.RegdNo??"").Contains(term)
            && (x.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCByPass)))
            .Select(x => x.RegdNo)
            .ToArray();
            return new JsonResult(data);
        }
        #endregion

        public IActionResult OnGetGenrateCreditRequestAsync(int Id)
        {
            if (Id == null)
            {
                return NotFound();
            }          

            TblCreditRequest? tblCreditRequest  =  _context.TblCreditRequests.Where(x=>x.WalletTransactionId==Id).FirstOrDefault();

            if (tblCreditRequest == null)
            {
                #region Create new User in TblUser Table 
                tblCreditRequest=new TblCreditRequest();    
                tblCreditRequest.WalletTransactionId = Id;
                tblCreditRequest.IsCreditRequest = true;
                tblCreditRequest.CreditRequestUserId = _loginSession.UserViewModel.UserId;
                tblCreditRequest.CreatedBy = _loginSession.UserViewModel.UserId;
                tblCreditRequest.CreatedDate = _currentDatetime;
                tblCreditRequest.IsActive = true;
                tblCreditRequest.IsCreditRequestApproved = false;
                _creditRequestRepository.Create(tblCreditRequest);
                _creditRequestRepository.SaveChanges();
               
                #endregion
            }
            return RedirectToPage("TicketGenrate");
        }
    }
}
