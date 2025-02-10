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

namespace RDCELERP.Core.App.Pages.QCIndex
{
    public class CancelOrderListModel : BasePageModel
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

        public CancelOrderListModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IExchangeOrderRepository exchangeOrderRepository, IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository, IOrderTransRepository orderTransRepository, IOrderQCRepository orderQCRepository, IProductCategoryManager productCategoryManager, IProductTypeManager productTypeManager)
         : base(config)
        {
            _context = context;
            _exchangeOrderRepository = exchangeOrderRepository;
            _exchangeABBStatusHistoryRepository = exchangeABBStatusHistoryRepository;
            _orderTransRepository = orderTransRepository;
            _orderQCRepository = orderQCRepository;
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
        }
        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<TblExchangeOrder> TblExchangeOrder { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblExchangeOrder TblExchangeOrderObj { get; set; }
        public void OnGet()
        {
            searchFilterVM.UserId = _loginSession.UserViewModel.UserId;
            // Get Product Category List
            var ProductGroup = _productCategoryManager.GetAllProductCategory();
            if (ProductGroup != null)
            {
                ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
            }
        }

        public IActionResult OnPostUpdateAsync()
        {
            string mystring = TblExchangeOrderObj.Comment3;
            string comment = TblExchangeOrderObj.Comment1;
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
                    if (query != null)
                    {
                        foreach (int num in query)
                        {
                            TblExchangeOrder tblExchangeOrders = _exchangeOrderRepository.GetSingle(x => x.Id == num);
                            if (tblExchangeOrders != null)
                            {
                                TblOrderTran tblOrderTrans = _orderTransRepository.GetSingle(x => x.RegdNo == tblExchangeOrders.RegdNo);
                                if (tblOrderTrans != null)
                                {
                                    #region  Exchange Order
                                    if (tblExchangeOrders.StatusId == Convert.ToInt32(OrderStatusEnum.QCOrderCancel))
                                    {
                                        setStatusId = Convert.ToInt32(OrderStatusEnum.ReopenforQC);
                                        //setOrderStatus = "Reopen for QC After Decline";
                                        setOrderStatus = "Order reopened by customer on Digi2L's offered price";
                                    }
                                    tblExchangeOrders.IsActive = true;
                                    tblExchangeOrders.OrderStatus = setOrderStatus;
                                    tblExchangeOrders.StatusId = setStatusId;
                                    tblExchangeOrders.Comment1 = comment;
                                    tblExchangeOrders.ModifiedBy = _loginSession.UserViewModel.UserId;
                                    _exchangeOrderRepository.Update(tblExchangeOrders);
                                    _exchangeOrderRepository.SaveChanges();
                                    TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                    #endregion

                                    #region Order Trans
                                    tblOrderTrans.StatusId = setStatusId;
                                    _orderTransRepository.Update(tblOrderTrans);
                                    _orderTransRepository.SaveChanges();
                                    #endregion

                                    #region Order QC
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
                                    #endregion

                                    #region History
                                    tblExchangeAbbstatusHistory.OrderTransId = tblOrderTrans.OrderTransId;
                                    tblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(LoVEnum.Exchange);
                                    tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeOrders.SponsorOrderNumber;
                                    tblExchangeAbbstatusHistory.CustId = tblExchangeOrders.CustomerDetailsId;
                                    tblExchangeAbbstatusHistory.RegdNo = tblExchangeOrders.RegdNo;
                                    tblExchangeAbbstatusHistory.StatusId = setStatusId;
                                    tblExchangeAbbstatusHistory.Comment = comment;
                                    tblExchangeAbbstatusHistory.IsActive = true;
                                    tblExchangeAbbstatusHistory.CreatedBy = _loginSession.UserViewModel.UserId;
                                    tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                    _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                                    _exchangeABBStatusHistoryRepository.SaveChanges();
                                    #endregion

                                }
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
