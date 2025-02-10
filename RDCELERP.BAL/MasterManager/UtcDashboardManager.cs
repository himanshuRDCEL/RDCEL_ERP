using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Dashboards;

namespace RDCELERP.BAL.MasterManager
{
    public class UtcDashboardManager : IUtcDashboardManager
    {

        #region  Variable Declaration
        private readonly IBusinessPartnerRepository _bussinesPartnerRepository;
        private readonly IExchangeOrderRepository _exchangeOrderRepository;
        private readonly IBusinessUnitRepository _businessUnitRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IMapper _mapper;
        private readonly ILogging _logging;
        private readonly DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        private readonly Digi2l_DevContext _context;
        private readonly IListofValueRepository _listofValueRepository;
        private readonly IOrderTransactionManager _orderTransactionManager;
        private readonly IExchangeABBStatusHistoryManager _exchangeABBStatusHistoryManager;
        #endregion
        public UtcDashboardManager(IBusinessPartnerRepository bussinesPartnerRepository, IExchangeOrderRepository exchangeOrderRepository, ILogging logging, IBusinessUnitRepository businessUnitRepository, IVoucherRepository voucherRepository, Digi2l_DevContext context)
        {
            _bussinesPartnerRepository = bussinesPartnerRepository;
            _exchangeOrderRepository = exchangeOrderRepository;
            _logging = logging;
            _businessUnitRepository = businessUnitRepository;
            _voucherRepository = voucherRepository;
            _context = context;
        }

        public UtcDashboardViewModel GetUtcDashBoardDataFromDatabase()
        {
            UtcDashboardViewModel utcDashboardViewModel = new UtcDashboardViewModel();
            try
            {
                var AbbOrders = _context.TblAbbregistrations.Count(x => x.IsActive == true);
                utcDashboardViewModel.ABBOrders = Convert.ToString(AbbOrders);


                var ExchangeOrders = _context.TblExchangeOrders.Count(x => x.IsActive == true);
                utcDashboardViewModel.ExchangeOrders = Convert.ToString(ExchangeOrders);

                var AbbRedemption = _context.TblAbbredemptions.Count(x => x.IsActive == true);
                utcDashboardViewModel.ABBRedemptions = Convert.ToString(AbbRedemption);


                var EvcDisputes = _context.TblEvcdisputes.Count(x => x.IsActive == true);
                utcDashboardViewModel.EVCDisputes = Convert.ToString(EvcDisputes);

                var PendingAbbForApprovals = _context.TblAbbregistrations.Count(x => x.AbbApprove == false);
                utcDashboardViewModel.PendingABBforApprovals = Convert.ToString(PendingAbbForApprovals);

                var EvcAllocations = _context.TblWalletTransactions.Count(x => x.StatusId == "34");
                utcDashboardViewModel.EVCAllocations = Convert.ToString(EvcAllocations);


                var QcDone = _context.TblOrderQcs.Count(x => x.StatusId == 15);
                utcDashboardViewModel.QCDone = Convert.ToString(QcDone);


                var PickupDone = _context.TblOrderLgcs.Count(x => x.StatusId == 23);
                utcDashboardViewModel.PickUpDone = Convert.ToString(PickupDone);


                var DropDone = _context.TblOrderLgcs.Count(x => x.StatusId == 30);
                utcDashboardViewModel.DropDone = Convert.ToString(DropDone);

                var AbbInProgress = _context.TblAbbregistrations.Count(x => x.IsActive == true && x.StatusId != null && x.StatusId != 6 && x.StatusId != 16 && x.StatusId != 32 && x.StatusId != 58);
                utcDashboardViewModel.AbbInProgress = Convert.ToString(AbbInProgress);

                var ExchangeInProgress = _context.TblExchangeOrders.Count(x => x.IsActive == true && x.StatusId != null && x.StatusId != 6 && x.StatusId != 16 && x.StatusId != 32 && x.StatusId != 58);
                utcDashboardViewModel.ExchangeInProgress = Convert.ToString(ExchangeInProgress);

            }
            catch (Exception ex)
            {
                utcDashboardViewModel = new UtcDashboardViewModel();
            }
            return utcDashboardViewModel;
        }
    }
}
