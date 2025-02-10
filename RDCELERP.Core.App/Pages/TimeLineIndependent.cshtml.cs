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
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.TimeLine;

namespace RDCELERP.Core.App.Pages
{
    public class TimeLineIndepenent : PageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        IOptions<ApplicationSettings> _config;
        IExchangeABBStatusHistoryRepository _exchangeABBStatusHistoryRepository;
        ICustomerDetailsRepository _customerRepository;
        IExchangeOrderRepository _exchangeOrderRepository;
        IExchangeOrderStatusRepository _exchangeOrderstatusRepository;
        ITimeLineMappingStatusRepository _timeLineMappingStatusRepository;
        ITimeLineRepository _timeLineRepository;
        IOrderTransRepository _orderTransRepository;


        public TimeLineIndepenent(RDCELERP.DAL.Entities.Digi2l_DevContext context, IExchangeOrderStatusRepository exchangeOrderstatusRepository, IOrderTransRepository orderTransRepository, ITimeLineRepository timeLineRepository, ITimeLineMappingStatusRepository timeLineMappingStatusRepository, IExchangeOrderRepository exchangeOrderRepository, IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository, ICustomerDetailsRepository customerRepository, IOptions<ApplicationSettings> config)
      
        {
            _context = context;
            _config = config;
            _exchangeABBStatusHistoryRepository = exchangeABBStatusHistoryRepository;
            _customerRepository = customerRepository;
            _exchangeOrderRepository = exchangeOrderRepository;
            _exchangeOrderstatusRepository = exchangeOrderstatusRepository;
            _timeLineMappingStatusRepository = timeLineMappingStatusRepository;
            _timeLineRepository = timeLineRepository;
            _orderTransRepository = orderTransRepository;
        }

        [BindProperty(SupportsGet = true)]
        public List<TblExchangeAbbstatusHistory> tblExchangeABBStatusHistory { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblExchangeAbbstatusHistory TblExchangeAbbstatusHistory { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblCustomerDetail TblCustomerDetail { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblEvcregistration TblEvcregistration { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<TblEvcregistration> TblEvcregistrations { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblExchangeOrderStatus TblExchangeOrderStatuses { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblOrderTran TblOrderTrans { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblTimelineStatusMapping TblTimelineStatusMapping { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblTimeLine TblTimeLine { get; set; }
        public TblExchangeOrder TblExchangeOrder { get; set; }
        public TblAbbredemption TblAbbredemption { get; set; }
        List<TblExchangeOrder> TblExchangeOrders = null;
        List<TblUser> TblUser = null;

        [BindProperty(SupportsGet = true)]
        public List<TblExchangeOrderStatus> TblExchangeOrderStatus { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<TblTimelineStatusMapping> TblTimelineStatusMappings { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<TblTimeLine> TblTimeLines { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<TimeListViewModel> TimeList { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<TimeLineGroupBy> timeLineObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public TimeLineGroupBy timeLineObj1 { get; set; }
        [BindProperty(SupportsGet = true)]

        public TimeListViewModel timeListViewModel { get; set; }
        [ViewData]
        public string timeLineObj2 { get; set; }
        public IActionResult OnGet(string regdNo)
        {
            TblOrderTran trans = new TblOrderTran();
            int orderTransId = 0;
            trans = _orderTransRepository.GetSingle(x => x.RegdNo == regdNo);
            if(trans != null)
            {
                orderTransId = trans.OrderTransId;
            }
            TblOrderTrans = _orderTransRepository.GetSingle(x => x.OrderTransId == orderTransId);
            if(TblOrderTrans != null)
            {
                if (TblOrderTrans.OrderType == 17)
                {
                    //for list data

//@<<<<<<< Dev@
                    tblExchangeABBStatusHistory = _context.TblExchangeAbbstatusHistories.Where(x => x.IsActive == true && x.OrderTransId == orderTransId)
                                                .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                                                .Include(x => x.CreatedByNavigation)
                                                .ToList();

                    //for get the single value



                    TblExchangeOrder = _context.TblExchangeOrders.Where(x => x.Id == TblOrderTrans.ExchangeId).FirstOrDefault();


                    var ExchangeAbbstatusHistory = _context.TblExchangeAbbstatusHistories.OrderByDescending(e => e.StatusHistoryId).FirstOrDefault(e => e.IsActive == true && e.OrderTransId == orderTransId);
                    if(ExchangeAbbstatusHistory != null)
                    {
                        TblTimelineStatusMapping = _context.TblTimelineStatusMappings.Where(x => x.StatusId == ExchangeAbbstatusHistory.StatusId).FirstOrDefault();
                    }
                    

                    if (TblTimelineStatusMapping.StatusId != null)
                    {
                        TblTimeLine = _context.TblTimeLines.Where(x => x.TimeLineId == TblTimelineStatusMapping.OrderTimeLineId).FirstOrDefault();
                    }

                    if (TblExchangeOrder.CustomerDetailsId > 0)
                    {
                        TblCustomerDetail = _context.TblCustomerDetails.Where(x => x.Id == TblExchangeOrder.CustomerDetailsId).FirstOrDefault();
                    }


                    // for get the in the list 

                    if (tblExchangeABBStatusHistory != null && tblExchangeABBStatusHistory.Count > 0)
//@=======@
//            //for list data

//            @tblExchangeABBStatusHistory = _context.TblExchangeAbbstatusHistories.Where(x => x.IsActive == true && x.OrderTransId == orderTransId)
//                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x=>x.CustomerDetails)                     
//                                        .Include(x => x.CreatedByNavigation)
//                                        .ToList();

//            //for get the single value

           
//            TblOrderTrans = _orderTransactionRepository.GetSingle(x => x.OrderTransId == orderTransId);
//            TblExchangeOrder = _context.TblExchangeOrders.Where(x => x.Id == TblOrderTrans.ExchangeId).FirstOrDefault();

           
//            var ExchangeAbbstatusHistory = _context.TblExchangeAbbstatusHistories.OrderBy(e => e.CreatedDate).LastOrDefault(e => e.IsActive == true && e.OrderTransId == orderTransId);
//            TblTimelineStatusMapping = _context.TblTimelineStatusMappings.Where(x => x.StatusId == ExchangeAbbstatusHistory.StatusId).FirstOrDefault();

//            if (TblTimelineStatusMapping.StatusId != null)
//            {
//                TblTimeLine = _context.TblTimeLines.Where(x => x.TimeLineId == TblTimelineStatusMapping.OrderTimeLineId).FirstOrDefault();
//            }
            
//            if (TblExchangeOrder.CustomerDetailsId > 0)
//            {
//                TblCustomerDetail = _context.TblCustomerDetails.Where(x => x.Id == TblExchangeOrder.CustomerDetailsId).FirstOrDefault();
//            }

//            // for get the in the list 

//            if (tblExchangeABBStatusHistory != null && tblExchangeABBStatusHistory.Count > 0)
//            {
//                TimeList = new List<TimeListViewModel>();
//                foreach (var item in tblExchangeABBStatusHistory)
//                {
//                    TblExchangeOrderStatus = _context.TblExchangeOrderStatuses.Where(x => x.Id == item.StatusId).ToList();
//                    foreach (var item1 in TblExchangeOrderStatus)@
//@>>>>>>> Rathod_Ashwin@
                    {

                        TimeList = new List<TimeListViewModel>();
                        foreach (var item in tblExchangeABBStatusHistory)
                        {
                            if (item.Evcid != null && item.Evcid > 0)
                            {
                                TblEvcregistrations = _context.TblEvcregistrations.Where(x => x.EvcregistrationId == item.Evcid).ToList();
                            }
                            TblExchangeOrderStatus = _context.TblExchangeOrderStatuses.Where(x => x.Id == item.StatusId).ToList();

                            //var ordertimeline = TblExchangeOrderStatus.GroupBy(p => p.StatusName).ToList();
                            foreach (var item1 in TblExchangeOrderStatus)
                            {
                                TblTimelineStatusMappings = _context.TblTimelineStatusMappings.Where(x => x.StatusId == item1.Id).Include(x => x.CreatedByNavigation).ToList();

                                foreach (var item2 in TblTimelineStatusMappings)
                                {

                                    TblTimeLines = _context.TblTimeLines.Where(x => x.TimeLineId == item2.OrderTimeLineId).ToList();
                                    timeListViewModel = new TimeListViewModel();
                                    timeListViewModel.RegdNo = item.RegdNo;
                                    timeListViewModel.Comment = item.Comment;
                                    if (item.Evcid != null && item.Evcid > 0)
                                    {
                                        timeListViewModel.EVCName = TblEvcregistrations[0].BussinessName;
                                    }

                                    timeListViewModel.OrderStatus = item.OrderTrans.Exchange.OrderStatus;
                                    timeListViewModel.OrderTimeline = TblTimeLines[0].OrderTimeline;
                                    timeListViewModel.StatusName = TblExchangeOrderStatus[0].StatusName;
                                    timeListViewModel.StatusDescription = TblExchangeOrderStatus[0].StatusDescription;
                                    timeListViewModel.CreatedBy = item.CreatedBy;
                                    if (item2.StatusId == 5)
                                    {
                                     timeListViewModel.CreatedByName1 = item.OrderTrans.Exchange.CustomerDetails.FirstName + " " + item.OrderTrans.Exchange.CustomerDetails.LastName;
                                    }
                                
                                    if (item2.StatusId == 33)
                                    {
                                     timeListViewModel.CreatedByName1 = item.OrderTrans.Exchange.CustomerDetails.FirstName + " " + item.OrderTrans.Exchange.CustomerDetails.LastName;
                                    }
                                    if (item2.StatusId == 15)
                                    {
                                     timeListViewModel.CreatedByName1 = item.OrderTrans.Exchange.CustomerDetails.FirstName + " " + item.OrderTrans.Exchange.CustomerDetails.LastName;
                                    }
                                    if (item.CreatedByNavigation != null)
                                    {
                                        timeListViewModel.CreatedByName = item.CreatedByNavigation.FirstName + " " + item.CreatedByNavigation.LastName;
                                    }

                                    timeListViewModel.CreatedDate = item.CreatedDate;

                                    TimeList.Add(timeListViewModel);
                                }
                            }
                        }

                    }

                    timeLineObj = TimeList
                  .GroupBy(p => p.OrderTimeline,
                           (k, c) => new TimeLineGroupBy()
                           {
                               OrderTimeLine = k,

                               StatusDiscription = c.ToList()
                           }
                          ).ToList();



                }

                else if (TblOrderTrans.OrderType == 16)
                {

                    //for list data

                    tblExchangeABBStatusHistory = _context.TblExchangeAbbstatusHistories.Where(x => x.IsActive == true && x.OrderTransId == orderTransId)
                                                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration)
                                                .ThenInclude(x => x.Customer)
                                                .Include(x => x.CreatedByNavigation)
                                                .ToList();

                    //for get the single value



                    TblAbbredemption = _context.TblAbbredemptions.Where(x => x.RedemptionId == TblOrderTrans.AbbredemptionId).FirstOrDefault();


                    var ExchangeAbbstatusHistory = _context.TblExchangeAbbstatusHistories.OrderByDescending(e => e.StatusHistoryId).FirstOrDefault(e => e.IsActive == true && e.OrderTransId == orderTransId);
                    TblTimelineStatusMapping = _context.TblTimelineStatusMappings.Where(x => x.StatusId == ExchangeAbbstatusHistory.StatusId).FirstOrDefault();

//@<<<<<<< Dev@
                    if (TblTimelineStatusMapping.StatusId != null)
                    {
                        TblTimeLine = _context.TblTimeLines.Where(x => x.TimeLineId == TblTimelineStatusMapping.OrderTimeLineId).FirstOrDefault();
                    }

                    if (TblAbbredemption.CustomerDetailsId > 0)
                    {
                        TblCustomerDetail = _context.TblCustomerDetails.Where(x => x.Id == TblAbbredemption.CustomerDetailsId).FirstOrDefault();
                    }


                    // for get the in the list 

                    if (tblExchangeABBStatusHistory != null && tblExchangeABBStatusHistory.Count > 0)
                    {

                        TimeList = new List<TimeListViewModel>();
                        foreach (var item in tblExchangeABBStatusHistory)
                        {
                            if (item.Evcid != null && item.Evcid > 0)
//@=======@
//                           @ TblTimeLines = _context.TblTimeLines.Where(x => x.TimeLineId == item2.OrderTimeLineId).ToList();
//                            timeListViewModel = new TimeListViewModel();
//                            timeListViewModel.RegdNo = item.RegdNo;
//                            timeListViewModel.OrderStatus = item.OrderTrans.Exchange.OrderStatus;
//                            timeListViewModel.OrderTimeline = TblTimeLines[0].OrderTimeline;
//                            timeListViewModel.StatusName = TblExchangeOrderStatus[0].StatusName;
//                            timeListViewModel.StatusDescription = TblExchangeOrderStatus[0].StatusDescription;
//                            timeListViewModel.CreatedBy = item.CreatedBy;
//                            if(item2.StatusId == 5)
//                            {
//                                timeListViewModel.CreatedByName1 = item.OrderTrans.Exchange.CustomerDetails.FirstName + " " + item.OrderTrans.Exchange.CustomerDetails.LastName;
//                            }
//                            if (item2.StatusId == 33)
//                            {
//                                timeListViewModel.CreatedByName1 = item.OrderTrans.Exchange.CustomerDetails.FirstName + " " + item.OrderTrans.Exchange.CustomerDetails.LastName;
//                            }
//                            if (item.CreatedByNavigation != null)@
//@>>>>>>> Rathod_Ashwin@
                            {
                                TblEvcregistrations = _context.TblEvcregistrations.Where(x => x.EvcregistrationId == item.Evcid).ToList();
                            }
                            TblExchangeOrderStatus = _context.TblExchangeOrderStatuses.Where(x => x.Id == item.StatusId).ToList();

//@<<<<<<< Dev@

                            foreach (var item1 in TblExchangeOrderStatus)
                            {
                                TblTimelineStatusMappings = _context.TblTimelineStatusMappings.Where(x => x.StatusId == item1.Id).Include(x => x.CreatedByNavigation).ToList();

                                foreach (var item2 in TblTimelineStatusMappings)
                                {

                                    TblTimeLines = _context.TblTimeLines.Where(x => x.TimeLineId == item2.OrderTimeLineId).ToList();
                                    timeListViewModel = new TimeListViewModel();
                                    timeListViewModel.RegdNo = item.RegdNo;
                                    timeListViewModel.Comment = item.Comment;
                                    if (item.Evcid != null && item.Evcid > 0)
                                    {
                                        timeListViewModel.EVCName = TblEvcregistrations[0].BussinessName;
                                    }

                                    timeListViewModel.OrderStatus = item.OrderTrans.Abbredemption.AbbredemptionStatus;
                                    timeListViewModel.OrderTimeline = TblTimeLines[0].OrderTimeline;
                                    timeListViewModel.StatusName = TblExchangeOrderStatus[0].StatusName;
                                    timeListViewModel.StatusDescription = TblExchangeOrderStatus[0].StatusDescription;
                                    timeListViewModel.CreatedBy = item.CreatedBy;
                                    if (item2.StatusId == 5)
                                    {
                                     timeListViewModel.CreatedByName1 = item.OrderTrans.Abbredemption.CustomerDetails.FirstName + " " + item.OrderTrans.Abbredemption.CustomerDetails.LastName;
                                    }
                                
                                    if (item2.StatusId == 33)
                                    {
                                     timeListViewModel.CreatedByName1 = item.OrderTrans.Abbredemption.CustomerDetails.FirstName + " " + item.OrderTrans.Abbredemption.CustomerDetails.LastName;
                                    }
                                    if (item2.StatusId == 15)
                                    {
                                      timeListViewModel.CreatedByName1 = item.OrderTrans.Abbredemption.CustomerDetails.FirstName + " " + item.OrderTrans.Abbredemption.CustomerDetails.LastName;
                                    }
                                    if (item.CreatedByNavigation != null)
                                    {
                                        timeListViewModel.CreatedByName = item.CreatedByNavigation.FirstName + " " + item.CreatedByNavigation.LastName;
                                    }

                                    timeListViewModel.CreatedDate = item.CreatedDate;

                                    TimeList.Add(timeListViewModel);
                                }
                            }
//@=======@
//                            @timeListViewModel.CreatedDate = item.CreatedDate;
//                            TimeList.Add(timeListViewModel);@
//@>>>>>>> Rathod_Ashwin@
                        }

                    }

                    timeLineObj = TimeList
                  .GroupBy(p => p.OrderTimeline,
                           (k, c) => new TimeLineGroupBy()
                           {
                               OrderTimeLine = k,

                               StatusDiscription = c.ToList()
                           }
                          ).ToList();


                }

            }
            
            
            return Page();
        }

    }
}











