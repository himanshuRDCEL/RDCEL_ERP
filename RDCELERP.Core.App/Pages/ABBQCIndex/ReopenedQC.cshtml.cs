using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;
using RDCELERP.Core.App.Pages.Base;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.BAL.Interface;
using RDCELERP.Model.Company;
using RDCELERP.BAL.MasterManager;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using static RDCELERP.Model.ABBRedemption.LoVViewModel;
using RDCELERP.Common.Helper;
using RDCELERP.Common.Enums;
using RDCELERP.Model.SearchFilters;
using Microsoft.AspNetCore.Mvc.Rendering;
using RDCELERP.Model;

namespace RDCELERP.Core.App.Pages.ABBQCIndex
{
    public class ReopenedQCModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        public string MyIds = "";
        IExchangeOrderRepository _exchangeOrderRepository;
        IExchangeABBStatusHistoryRepository _exchangeABBStatusHistoryRepository;
        IOrderTransRepository _orderTransRepository;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IOrderQCRepository _orderQCRepository;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        IAbbRegistrationRepository _abbRegistrationRepository;
        IABBRedemptionRepository _aBBRedemptionRepository;
        ILogging _logging;

        public ReopenedQCModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IExchangeOrderRepository exchangeOrderRepository, IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository, IOrderTransRepository orderTransRepository, IOrderQCRepository orderQCRepository, IProductCategoryManager productCategoryManager, IProductTypeManager productTypeManager, IAbbRegistrationRepository abbRegistrationRepository, IABBRedemptionRepository aBBRedemptionRepository, ILogging logging)
         : base(config)
        {
            _context = context;
            _exchangeOrderRepository = exchangeOrderRepository;
            _exchangeABBStatusHistoryRepository = exchangeABBStatusHistoryRepository;
            _orderTransRepository = orderTransRepository;
            _orderQCRepository = orderQCRepository;
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
            _abbRegistrationRepository = abbRegistrationRepository;
            _aBBRedemptionRepository = aBBRedemptionRepository;
            _logging = logging;
        }
        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<TblExchangeOrder> TblExchangeOrder { get; set; }
        [BindProperty(SupportsGet = true)]
        public QCCommentViewModel QCCommentViewModel { get; set; }
        public void OnGet()
        {
            // Get Product Category List
            var ProductGroup = _productCategoryManager.GetAllProductCategory();
            if (ProductGroup != null)
            {
                ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
            }
        }

        public IActionResult OnPostUpdateAsync()
        {
            string mystring = QCCommentViewModel.RedemptionIdList;
            string comment = QCCommentViewModel.Qccomments;

            if (_loginSession == null)
            {
                return RedirectToPage("ReopenedQC");
            }
            else
            {
                if (mystring != null)
                {
                    var query = from val in mystring.Split(',')
                                select int.Parse(val);
                    foreach (int num in query)
                    {
                        TblAbbredemption tblAbbredemption = _aBBRedemptionRepository.GetSingle(x => x.RedemptionId == num);
                        if (tblAbbredemption != null)
                        {
                            TblOrderTran tblOrderTrans = _orderTransRepository.GetSingle(x => x.RegdNo == tblAbbredemption.RegdNo);
                            if (tblOrderTrans != null)
                            {
                                #region ABB Redemption
                                tblAbbredemption.IsActive = true;
                                tblAbbredemption.AbbredemptionStatus = "Reopen Order for QC";
                                tblAbbredemption.StatusId = Convert.ToInt32(OrderStatusEnum.ReopenforQC);
                                tblAbbredemption.Qccomments = comment;
                                tblAbbredemption.ModifiedBy = _loginSession.UserViewModel.UserId;
                                _aBBRedemptionRepository.Update(tblAbbredemption);
                                _aBBRedemptionRepository.SaveChanges();
                                TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                #endregion

                                tblOrderTrans.StatusId = Convert.ToInt32(OrderStatusEnum.ReopenforQC); 
                                _orderTransRepository.Update(tblOrderTrans);
                                _orderTransRepository.SaveChanges();
                                tblExchangeAbbstatusHistory.OrderTransId = tblOrderTrans.OrderTransId;
                                TblOrderQc tblOrderQc = _orderQCRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == tblOrderTrans.OrderTransId);
                                if (tblOrderQc != null)
                                {
                                    tblOrderQc.StatusId = Convert.ToInt32(OrderStatusEnum.ReopenforQC);
                                    tblOrderQc.ModifiedBy = _loginSession.UserViewModel.UserId;
                                    tblOrderQc.ModifiedDate = _currentDatetime;
                                    tblOrderQc.IsActive = true;
                                    _orderQCRepository.Update(tblOrderQc);
                                    _orderQCRepository.SaveChanges();
                                }
                                else
                                {
                                    tblOrderQc = new TblOrderQc();
                                }
                                tblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(OrderTypeEnum.ABB);
                                //tblExchangeAbbstatusHistory.SponsorOrderNumber = tblAbbredemption.SponsorOrderNumber;
                                tblExchangeAbbstatusHistory.CustId = tblAbbredemption.CustomerDetailsId;
                                tblExchangeAbbstatusHistory.RegdNo = tblAbbredemption.RegdNo;
                                tblExchangeAbbstatusHistory.StatusId = Convert.ToInt32(OrderStatusEnum.ReopenforQC);
                                tblExchangeAbbstatusHistory.Comment = comment;
                                tblExchangeAbbstatusHistory.IsActive = true;
                                tblExchangeAbbstatusHistory.CreatedBy = _loginSession.UserViewModel.UserId;
                                tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                                _exchangeABBStatusHistoryRepository.SaveChanges();
                            }
                        }
                    }
                }
            }
            return RedirectToPage("ReopenedQC");
        }

        #region Autopopulate Search Filter for search by RegdNo
        public IActionResult OnGetSearchRegdNo(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            //string searchTerm = term.ToString();

            var data = _context.TblAbbredemptions
            .Where(x => x.RegdNo.Contains(term)
            && (x.StatusId == Convert.ToInt32(OrderStatusEnum.QCOrderCancel)))
            .Select(x => x.RegdNo)
            .ToArray();
            return new JsonResult(data);
        }
        #endregion
    }
}
