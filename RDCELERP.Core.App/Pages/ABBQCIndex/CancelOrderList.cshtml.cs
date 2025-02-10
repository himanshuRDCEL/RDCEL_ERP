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
using RDCELERP.Common.Helper;
using RDCELERP.Common.Enums;
using RDCELERP.Model.SearchFilters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RDCELERP.Core.App.Pages.ABBQCIndex
{
    public class CancelOrderListModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        IABBRedemptionRepository _aBBRedemptionRepository;
        IExchangeABBStatusHistoryRepository _exchangeABBStatusHistoryRepository;
        IOrderTransRepository _orderTransRepository;
        IOrderQCRepository _orderQCRepository;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;


        public string MyIds = "";
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();


        public CancelOrderListModel(IOptions<ApplicationSettings> config, Digi2l_DevContext context, IABBRedemptionRepository aBBRedemptionRepository, IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository, IOrderTransRepository orderTransRepository, IOrderQCRepository orderQCRepository, IProductCategoryManager productCategoryManager, IProductTypeManager productTypeManager)
         : base(config)
        {
            _context = context;
            _aBBRedemptionRepository = aBBRedemptionRepository;
            _exchangeABBStatusHistoryRepository = exchangeABBStatusHistoryRepository;
            _orderTransRepository = orderTransRepository;
            _orderQCRepository = orderQCRepository;
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
        }
        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<TblAbbredemption> tblAbbredemptionslist { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblAbbredemption tblAbbredemption { get; set; }
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
            string mystring = tblAbbredemption.Qccomments;
            string comment = tblAbbredemption.LogisticsComments;
            int? setStatusId = Convert.ToInt32(OrderStatusEnum.ReopenOrder);
            string setOrderStatus = "Reopen Cancelled Order";
            if (_loginSession == null)
            {
                return RedirectToPage("cancelorderlist");
            }
            else
            {
                if (mystring != null)
                {
                    var query = from val in mystring.Split(',')
                                select int.Parse(val);
                    foreach (int num in query)
                    {
                        TblAbbredemption TblAbbredemptions = _aBBRedemptionRepository.GetSingle(x => x.RedemptionId == num);
                        if (TblAbbredemptions != null)
                        {
                            TblOrderTran tblOrderTrans = _orderTransRepository.GetSingle(x => x.RegdNo == TblAbbredemptions.RegdNo);
                            if (tblOrderTrans != null)
                            {
                                #region AbbRedemption
                                if (TblAbbredemptions.StatusId == Convert.ToInt32(OrderStatusEnum.QCOrderCancel))
                                {
                                    setStatusId = Convert.ToInt32(OrderStatusEnum.ReopenforQC);
                                    //setOrderStatus = "Reopen for QC After Decline";
                                    setOrderStatus = "Order reopened by customer on Digi2L's offered price";
                                }
                                TblAbbredemptions.IsActive = true;
                                TblAbbredemptions.AbbredemptionStatus = setOrderStatus;
                                TblAbbredemptions.StatusId = setStatusId;
                                TblAbbredemptions.LogisticsComments = comment;
                                TblAbbredemptions.ModifiedBy = _loginSession.UserViewModel.UserId;
                                TblAbbredemptions.ModifiedDate = _currentDatetime;
                                _aBBRedemptionRepository.Update(TblAbbredemptions);
                                _aBBRedemptionRepository.SaveChanges();
                                TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                #endregion

                                tblOrderTrans.StatusId = setStatusId;
                                _orderTransRepository.Update(tblOrderTrans);
                                _orderTransRepository.SaveChanges();
                                tblExchangeAbbstatusHistory.OrderTransId = tblOrderTrans.OrderTransId;
                                //Code to update status of cancel order in Tblorderqc Added by PJ  
                                TblOrderQc tblOrderQc = _orderQCRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == tblOrderTrans.OrderTransId);
                                if (tblOrderQc != null)
                                {
                                    tblOrderQc.StatusId = setStatusId;
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
                                tblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(LoVEnum.ABB);
                                tblExchangeAbbstatusHistory.CustId = TblAbbredemptions.CustomerDetailsId;
                                tblExchangeAbbstatusHistory.RegdNo = TblAbbredemptions.RegdNo;
                                tblExchangeAbbstatusHistory.StatusId = setStatusId;
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
                return RedirectToPage("CancelOrderList");
            }
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
            .Where(x => x.RegdNo.Contains(term)
            && (x.StatusId == Convert.ToInt32(OrderStatusEnum.CancelOrder) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentDeclined)
                               || x.StatusId == Convert.ToInt32(OrderStatusEnum.CustomerNotResponding_3C) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCFail_3Y)
                               || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCOrderCancel)))
            .Select(x => x.RegdNo)
            .ToArray();
            return new JsonResult(data);
        }
        #endregion
    }
}
