using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Enums;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.ServicePartner;

namespace RDCELERP.LGCMobile.Scheduler.LGCMobileApp
{
    public class LGCMobileScheduler : ILGCMobileScheduler, IDisposable
    {
        Digi2l_DevContext _devContext;
        DateTime _currentDatetime = DateTime.Now;
        IWalletTransactionRepository _walletTransactionRepository;
        IOrderTransRepository _orderTransRepository;
        ILogisticsRepository _logisticsRepository;
        IExchangeOrderRepository _exchangeOrderRepository;
        IABBRedemptionRepository _abbRedemptionRepository;
        ICommonManager _commonManager;

        public LGCMobileScheduler(Digi2l_DevContext digi2L_DevContext, IWalletTransactionRepository walletTransactionRepository, IOrderTransRepository orderTransRepository, ILogisticsRepository logisticsRepository, IExchangeOrderRepository exchangeOrderRepository, IABBRedemptionRepository aBBRedemptionRepository, ICommonManager commonManager)
        {
            _devContext = digi2L_DevContext;
            _walletTransactionRepository = walletTransactionRepository;
            _orderTransRepository = orderTransRepository;
            _logisticsRepository = logisticsRepository;
            _exchangeOrderRepository = exchangeOrderRepository;
            _abbRedemptionRepository = aBBRedemptionRepository;
            _commonManager = commonManager;
        }
        public void RollbackOrderFromDriver()
        {
            int OrderStatusId = Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated);
            TblOrderTran tblOrderTran = null;
            TblExchangeOrder tblExchangeOrder = null;
            TblAbbredemption tblAbbredemption = null;
            TblAbbregistration tblAbbregistration = null;
            TblWalletTransaction tblWalletTransaction = null;
            List<TblLogistic> tblLogistic = new List<TblLogistic>();
            int orderType = 0;
            string regdNo = null; int? customerDetailsId = null; string sponsorOrderNumber = null;
            tblLogistic = _devContext.TblLogistics.Where(x => x.ModifiedDate.Value.Date == _currentDatetime.Date).ToList();
            if (tblLogistic != null)
            {
                foreach (var item in tblLogistic)
                {
                    if (item.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner))
                    {
                        Console.WriteLine(item.OrderTransId);
                        #region update statusId in tblLogistic
                        item.StatusId = OrderStatusId;
                        item.ModifiedDate = _currentDatetime;
                        var Logisticresult = _logisticsRepository.UpdateLogiticStatus(item);
                        #endregion

                        tblOrderTran = _orderTransRepository.GetOrderDetailsByOrderTransId((int)item.OrderTransId);
                        
                        if (tblOrderTran != null && tblOrderTran.OrderTransId > 0)
                        {
                            orderType = (int)tblOrderTran.OrderType;
                            if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                            {
                                tblExchangeOrder = tblOrderTran.Exchange;
                                if (tblExchangeOrder != null)
                                {
                                    regdNo = tblExchangeOrder.RegdNo;
                                    customerDetailsId = tblExchangeOrder.CustomerDetailsId;
                                    sponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                                }
                            }
                            else if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.ABB))
                            {
                                tblAbbredemption = tblOrderTran.Abbredemption;
                                if (tblAbbredemption != null)
                                {
                                    customerDetailsId = tblAbbredemption.CustomerDetailsId;
                                    tblAbbregistration = tblAbbredemption.Abbregistration;
                                    if (tblAbbregistration != null)
                                    {
                                        regdNo = tblAbbregistration.RegdNo;
                                        sponsorOrderNumber = tblAbbregistration.SponsorOrderNo;
                                    }
                                }
                            }

                            #region update statusId in Base tbl Exchange or ABB
                            if (tblExchangeOrder != null)
                            {
                                tblExchangeOrder.StatusId = OrderStatusId;
                                tblExchangeOrder.ModifiedDate = _currentDatetime;
                                _exchangeOrderRepository.Update(tblExchangeOrder);
                                _exchangeOrderRepository.SaveChanges();
                            }
                            else if (tblAbbredemption != null && tblAbbregistration != null)
                            {
                                tblAbbredemption.StatusId = OrderStatusId;
                                tblAbbredemption.ModifiedDate = _currentDatetime;
                                _abbRedemptionRepository.Update(tblAbbredemption);
                                _abbRedemptionRepository.SaveChanges();
                            }
                            #endregion

                            #region update statusId in tblOrderTrans
                            tblOrderTran.StatusId = OrderStatusId;
                            tblOrderTran.ModifiedDate = _currentDatetime;
                            _orderTransRepository.Update(tblOrderTran);
                            _orderTransRepository.SaveChanges();
                            #endregion

                            #region update statusId in tblWalletTransaction
                            var walletTransactionresult = _walletTransactionRepository.UpdateWalletTransStatus((int)item.OrderTransId, OrderStatusId, null);
                            #endregion

                            tblWalletTransaction = _devContext.TblWalletTransactions.Where(x => x.OrderTransId == item.OrderTransId).FirstOrDefault();

                            #region Insert into tblexchangeabbhistory
                            TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                            tblExchangeAbbstatusHistory.OrderType = orderType;
                            tblExchangeAbbstatusHistory.SponsorOrderNumber = sponsorOrderNumber;
                            tblExchangeAbbstatusHistory.RegdNo = tblOrderTran.RegdNo;
                            tblExchangeAbbstatusHistory.CustId = customerDetailsId;
                            tblExchangeAbbstatusHistory.StatusId = OrderStatusId;
                            tblExchangeAbbstatusHistory.IsActive = true;
                            tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                            tblExchangeAbbstatusHistory.CreatedBy = Convert.ToInt32(SchedulerEnum.SuperAdmin);
                            tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                            tblExchangeAbbstatusHistory.ServicepartnerId = item.ServicePartnerId;
                            tblExchangeAbbstatusHistory.DriverDetailId = null;
                            tblExchangeAbbstatusHistory.Evcid = tblWalletTransaction.EvcregistrationId;
                            _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                            #endregion

                        }
                    }
                }
            }
        }

        public void Dispose()
        {
           
        }
    }
}
