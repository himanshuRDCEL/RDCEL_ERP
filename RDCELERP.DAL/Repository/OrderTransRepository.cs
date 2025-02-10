using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;


namespace RDCELERP.DAL.Repository
{
    public class OrderTransRepository : AbstractRepository<TblOrderTran>, IOrderTransRepository
    {
        Digi2l_DevContext _context;
        public OrderTransRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }

        #region GetQcDetailsByExchangeId
        public TblOrderTran GetQcDetailsByExchangeId(int? Id)
        {
            TblOrderTran TblOrderTran = null;

            if (Id > 0)
            {
                TblOrderTran = _context.TblOrderTrans.FirstOrDefault(x => x.IsActive == true && x.ExchangeId == Id);
            }

            return TblOrderTran;
        }
        #endregion

        #region GetRegdno
        public TblOrderTran GetRegdno(string regdno)
        {
            TblOrderTran TblOrderTran = null;

            if (regdno != null)
            {
                TblOrderTran = _context.TblOrderTrans.FirstOrDefault(x => x.IsActive == true && x.RegdNo == regdno);
            }

            return TblOrderTran;
        }
        #endregion

        #region UpdateTransRecordStatus
        /// <summary>
        /// Method to udpate the trans record status
        /// </summary>
        /// <param name="transid">transid</param>
        /// <param name="status">status</param>
        /// <param name="userid">userid</param>
        /// <returns></returns>
        public int UpdateTransRecordStatus(int transid, int? status, int? userid)
        {
            TblOrderTran tblOrderTrans = _context.TblOrderTrans.FirstOrDefault(x => x.IsActive == true && x.OrderTransId == transid);

            tblOrderTrans.StatusId = status;  //(int)OrderStatusEnum.Posted;
            tblOrderTrans.ModifiedBy = userid;
            tblOrderTrans.ModifiedDate = DateTime.Now;
            _context.Update(tblOrderTrans);
            _context.SaveChanges();
            return (int)tblOrderTrans.OrderType;

        }
        #endregion

        #region GetRegdnoinTicketG
        public TblOrderTran GetRegdnoinTicketG(string regdno)
        {
            TblOrderTran tblOrderTran = null;

            if (regdno != null)
            {
                tblOrderTran = _context.TblOrderTrans

                   .Include(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                   .Include(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                   //Add for ABB Redumption By VK
                   .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                   .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                   .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    ///Add for ABB Redumption By VK
                    .Where(x => !string.IsNullOrEmpty(x.RegdNo) && x.RegdNo.ToLower() == regdno.ToLower() && x.IsActive == true).FirstOrDefault();
            }

            return tblOrderTran;
        }
        #endregion

        #region Method to get the Order transby id 
        /// <summary>
        /// Method to get the Order transby id 
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>TblOrderTran</returns>
        public TblOrderTran GetSingleOrder(int Id)
        {
            TblOrderTran TblOrderTran = _context.TblOrderTrans.FirstOrDefault(x => x.IsActive == true && x.OrderTransId == Id);

            return TblOrderTran;
        }
        #endregion

        #region Method to get the Order transby id with all exchange reference
        /// <summary>
        /// Method to get the Order transby id with all exchange reference
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>TblOrderTran</returns>
        public TblOrderTran GetSingleOrderWithExchangereference(int Id)
        {
            TblOrderTran? TblOrderTran = null;
            try
            {
                TblOrderTran = _context.TblOrderTrans.FirstOrDefault(x => x.IsActive == true && x.OrderTransId == Id);
                if (TblOrderTran != null)
                {
                    if (TblOrderTran != null && TblOrderTran.OrderType == 17) //17=Exchange
                    {
                        TblOrderTran.Exchange = _context.TblExchangeOrders
                                   .Include(x => x.Brand)
                                   .Include(x => x.BusinessPartner).ThenInclude(x => x.BusinessUnit)
                                   .Include(x => x.CustomerDetails)
                                   .Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                   .Include(x => x.CustomerDetails)
                                   .FirstOrDefault(x => x.IsActive == true && x.Id == TblOrderTran.ExchangeId);
                    }
                    else
                    {
                        TblOrderTran.Abbredemption = _context.TblAbbredemptions
                                 .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                           .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                            .Include(x => x.CustomerDetails)
                              .Include(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit).FirstOrDefault(x => x.IsActive == true && x.RedemptionId == TblOrderTran.AbbredemptionId);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return TblOrderTran;
        }
        #endregion

        #region Get Order Details By OrderTrans Id 
        /// <summary>
        /// Get Order Details By OrderTrans Id
        /// </summary>
        /// <param name="OrderTransId"></param>
        /// <returns></returns>
        public TblOrderTran GetOrderDetailsByTransId(int OrderTransId)
        {
            TblOrderTran tblOrderTran = null;
            try
            {
                if (OrderTransId > 0)
                {
                    tblOrderTran = _context.TblOrderTrans
                        .Include(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                        .Include(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                        .Include(x => x.Exchange).ThenInclude(x => x.Brand)
                        .Where(x => x.IsActive == true && x.OrderTransId == OrderTransId)

                        .FirstOrDefault();
                }
                else
                {
                    tblOrderTran = new TblOrderTran();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblOrderTran;
        }
        #endregion

        #region get order data for settelment
        /// <summary>
        /// get order data for settelment
        /// </summary>
        /// <param name="RegdNo"></param>
        /// <returns></returns>

        public TblOrderTran GetOrderDataForSettelment(string RegdNo)
        {
            TblOrderTran orderTransaction = new TblOrderTran();
            if (RegdNo != null)
            {
                orderTransaction = _context.TblOrderTrans
                    .Include(x => x)
                    .FirstOrDefault(x => x.RegdNo != null && x.RegdNo == RegdNo && x.IsActive == true && x.AbbredemptionId != null);
            }
            return orderTransaction;
        }
        #endregion

        #region Get transcation details for ABB Redemption order
        /// <summary>
        /// Get transcation details for ABB Redemption order
        /// </summary>
        /// <param name="RergdNo"></param>
        /// <returns></returns>

        public TblOrderTran GetTransactionDetailsForABBRedemption(string RergdNo)
        {
            TblOrderTran ordertransObj = new TblOrderTran();
            if (!string.IsNullOrEmpty(RergdNo))
            {
                ordertransObj = _context.TblOrderTrans
                    .Include(x => x.Abbredemption)
                    .ThenInclude(x => x.Abbregistration)
                    .FirstOrDefault(x => x.IsActive == true && x.RegdNo != null && x.RegdNo == RergdNo);

            }
            return ordertransObj;
        }
        #endregion

        #region Get order details for updating  amount paid to customer or not
        public TblOrderTran GetOrdertransDetails(string RegdNo)
        {
            TblOrderTran orderTrans = new TblOrderTran();
            if (!string.IsNullOrEmpty(RegdNo))
            {
                orderTrans = _context.TblOrderTrans.FirstOrDefault(x => x.IsActive == true && x.RegdNo != null && x.RegdNo == RegdNo);
            }
            return orderTrans;
        }
        #endregion

        #region get details by redemption id
        /// <summary>
        ///  get details by redemption id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>TblOrderTran</returns>
        public TblOrderTran GetDetailsByRedemptionId(int? Id)
        {
            TblOrderTran TblOrderTran = null;

            if (Id > 0)
            {
                TblOrderTran = _context.TblOrderTrans.FirstOrDefault(x => x.IsActive == true && x.AbbredemptionId == Id);
            }

            return TblOrderTran;
        }
        #endregion

        #region Get Order Trans by RegdNo with includes Exchange or ABB
        /// <summary>
        /// Get Order Trans by RegdNo with includes Exchange or ABB
        /// </summary>
        /// <param name="regdno"></param>
        /// <returns></returns>
        public TblOrderTran GetOrderTransByRegdno(string regdno)
        {
            TblOrderTran? TblOrderTran = null;

            if (regdno != null)
            {
                TblOrderTran = _context.TblOrderTrans
                    .Include(x => x.Exchange)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration)
                     .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessPartner)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Status)
                         .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.ModelNumber)
                        .Where(x => x.IsActive == true && x.RegdNo.ToLower() == regdno.ToLower())
                    .FirstOrDefault();
            }

            return TblOrderTran;
        }
        #endregion

        #region GetABBOrderDetailsByTransId
        /// <summary>
        /// Method to get the Order transby id with all ABB reference
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>TblOrderTran</returns>
        public TblOrderTran GetABBOrderDetailsByTransId(int Id)
        {
            TblOrderTran TblOrderTran = null;
            if (Id > 0)
            {
                TblOrderTran = _context.TblOrderTrans
                           .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                           .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                           .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                           .FirstOrDefault(x => x.IsActive == true && x.OrderTransId == Id);

            }
            return TblOrderTran;
        }
        #endregion

        #region Get Order Details by abb redemption Id
        public TblOrderTran GetQcDetailsByABBRedemptionId(int? Id)
        {
            TblOrderTran TblOrderTran = null;

            if (Id > 0)
            {
                TblOrderTran = _context.TblOrderTrans.FirstOrDefault(x => x.IsActive == true && x.AbbredemptionId == Id);
            }

            return TblOrderTran;
        }
        #endregion

        #region Method to get exchange and Abb data by ordertrans Added by PJ
        /// <summary>
        /// Method to get exchange and Abb data by ordertrans id
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>TblOrderTran</returns>
        public TblOrderTran GetExchangeABBbyordertrans(int Id)
        {
            TblOrderTran TblOrderTran = null;
            if (Id > 0)
            {
                TblOrderTran = _context.TblOrderTrans.FirstOrDefault(x => x.IsActive == true && x.OrderTransId == Id);
                if (TblOrderTran != null && TblOrderTran.OrderType == 17)
                {
                    TblOrderTran = _context.TblOrderTrans
                          .Include(x => x.Exchange)
                          .ThenInclude(x => x.BusinessPartner)
                          .ThenInclude(x => x.BusinessUnit)
                          .FirstOrDefault(x => x.IsActive == true
                          && x.Exchange != null && x.Exchange.BusinessPartner != null && x.Exchange.BusinessPartner.BusinessUnit != null && x.OrderTransId == Id);
                }
                else
                {
                    TblOrderTran = _context.TblOrderTrans
                          .Include(x => x.Abbredemption)
                          .ThenInclude(x => x.Abbregistration)
                          .ThenInclude(x => x.BusinessUnit)
                          .FirstOrDefault(x => x.IsActive == true && x.Abbredemption != null && x.Abbredemption.Abbregistration != null
                          && x.Abbredemption.Abbregistration.BusinessUnit != null &&
                          x.OrderTransId == Id);
                }
            }
            return TblOrderTran;
        }
        #endregion

        #region UpdateStatusOnBaseTblsByTransId
        /// <summary>
        /// Method to udpate the trans record status by TransId
        /// </summary>
        /// <param name="transid">transid</param>
        /// <param name="status">status</param>
        /// <param name="userid">userid</param>
        /// <returns></returns>
        public TblOrderTran UpdateStatusOnBaseTblsByTransId(int transid, int? status, int? userid, string statusDesc)
        {
            TblOrderTran tblOrderTrans = _context.TblOrderTrans
                .Include(x => x.Exchange).Include(x => x.Abbredemption)
                .Where(x => x.IsActive == true && x.OrderTransId == transid).FirstOrDefault();
            if (tblOrderTrans != null)
            {
                #region update OrderTrans
                tblOrderTrans.StatusId = status;
                tblOrderTrans.ModifiedBy = userid;
                tblOrderTrans.ModifiedDate = DateTime.Now;
                _context.Update(tblOrderTrans);
                #endregion

                if (tblOrderTrans.Exchange != null)
                {
                    if (!string.IsNullOrWhiteSpace(statusDesc))
                    {
                        tblOrderTrans.Exchange.OrderStatus = statusDesc;
                    }
                    tblOrderTrans.Exchange.StatusId = status;
                    tblOrderTrans.Exchange.ModifiedBy = userid;
                    tblOrderTrans.Exchange.ModifiedDate = DateTime.Now;
                    _context.Update(tblOrderTrans.Exchange);
                }
                else if (tblOrderTrans.Abbredemption != null)
                {
                    if (!string.IsNullOrWhiteSpace(statusDesc))
                    {
                        tblOrderTrans.Abbredemption.AbbredemptionStatus = statusDesc;
                    }
                    tblOrderTrans.Abbredemption.StatusId = status;
                    tblOrderTrans.Abbredemption.ModifiedBy = userid;
                    tblOrderTrans.Abbredemption.ModifiedDate = DateTime.Now;
                    _context.Update(tblOrderTrans.Abbredemption);
                }
                _context.SaveChanges();
            }
            return tblOrderTrans;
        }
        #endregion

        #region Get Order Details By OrderTrans Id 
        /// <summary>
        /// Get Order Details By OrderTrans Id
        /// </summary>
        /// <param name="OrderTransId"></param>
        /// <returns></returns>
        public TblOrderTran GetOrderDetailsByOrderTransId(int OrderTransId)
        {
            TblOrderTran? tblOrderTran = null;
            try
            {
                if (OrderTransId > 0)
                {
                    tblOrderTran = _context.TblOrderTrans
                        .Include(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                        .Include(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                        .Include(x => x.Exchange).ThenInclude(x => x.Brand)
                        // For ABB
                        .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                        .Where(x => x.IsActive == true && x.OrderTransId == OrderTransId).FirstOrDefault();
                }
                else
                {
                    tblOrderTran = new TblOrderTran();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblOrderTran;
        }
        #endregion

        #region Get Exch Orders Pending List
        /// <summary>
        /// Get Exch Orders Pending List
        /// </summary>
        /// <param name="buid"></param>
        /// <param name="DateTimeByElapsedHours"></param>
        /// <param name="statusId1"></param>
        /// <param name="statusId2"></param>
        /// <param name="statusId3"></param>
        /// <param name="statusId4"></param>
        /// <param name="statusId5"></param>
        /// <param name="statusId6"></param>
        /// <param name="statusId7"></param>
        /// <returns></returns>
        public List<TblOrderTran> GetExchOrdersPendingList(int? buid, bool? isFilterByModifiedDate, DateTime? DateTimeByElapsedHours = null,
            int? statusId1 = null, int? statusId2 = null, int? statusId3 = null, int? statusId4 = null,
            int? statusId5 = null, int? statusId6 = null, int? statusId7 = null)
        {
            List<TblOrderTran>? tblOrderTranList = null;
            try
            {
                #region table object Initialization
                tblOrderTranList = _context.TblOrderTrans
                    .Include(x => x.Exchange).ThenInclude(x => x.Status)
                    .Include(x => x.Exchange).ThenInclude(x => x.Brand)
                    .Include(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Where(x => (x.StatusId == statusId1
                                || (statusId2 != null ? x.StatusId == statusId2 : false)
                                || (statusId3 != null ? x.StatusId == statusId3 : false)
                                || (statusId4 != null ? x.StatusId == statusId4 : false)
                                || (statusId5 != null ? x.StatusId == statusId5 : false)
                                || (statusId6 != null ? x.StatusId == statusId6 : false)
                                || (statusId7 != null ? x.StatusId == statusId7 : false))
                               && ((DateTimeByElapsedHours == null)
                               || (isFilterByModifiedDate == true ? (x.ModifiedDate <= DateTimeByElapsedHours) : (x.CreatedDate <= DateTimeByElapsedHours)))
                               && ((x.Exchange != null && x.Exchange.IsActive == true
                               && (buid == null || (x.Exchange.BusinessUnitId == buid)))
                               )).OrderByDescending(x => x.ModifiedDate ?? x.CreatedDate).ToList();
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblOrderTranList;
        }
        #endregion

        #region Get ABB Pending List
        /// <summary>
        /// Get ABB Pending List
        /// </summary>
        /// <param name="buid"></param>
        /// <param name="DateTimeByElapsedHours"></param>
        /// <param name="statusId1"></param>
        /// <param name="statusId2"></param>
        /// <param name="statusId3"></param>
        /// <param name="statusId4"></param>
        /// <param name="statusId5"></param>
        /// <param name="statusId6"></param>
        /// <param name="statusId7"></param>
        /// <returns></returns>
        public List<TblOrderTran> GetABBOrdersPendingList(int? buid, bool? isFilterByModifiedDate, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null, int? statusId2 = null, int? statusId3 = null, int? statusId4 = null,
            int? statusId5 = null, int? statusId6 = null, int? statusId7 = null)
        {
            List<TblOrderTran>? tblOrderTranList = null;
            try
            {
                #region table object Initialization
                tblOrderTranList = _context.TblOrderTrans
                    .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    .Where(x => (x.StatusId == statusId1
                                || (statusId2 != null ? x.StatusId == statusId2 : false)
                                || (statusId3 != null ? x.StatusId == statusId3 : false)
                                || (statusId4 != null ? x.StatusId == statusId4 : false)
                                || (statusId5 != null ? x.StatusId == statusId5 : false)
                                || (statusId6 != null ? x.StatusId == statusId6 : false)
                                || (statusId7 != null ? x.StatusId == statusId7 : false))
                               && ((DateTimeByElapsedHours == null)
                               || (isFilterByModifiedDate == true ? (x.ModifiedDate <= DateTimeByElapsedHours) : (x.CreatedDate <= DateTimeByElapsedHours)))
                               && ((x.Abbredemption != null && x.Abbredemption.Abbregistration != null && x.Abbredemption.IsActive == true && x.Abbredemption.Abbregistration.IsActive == true
                               && (buid == null || (x.Abbredemption.Abbregistration.BusinessUnitId == buid))
                               ))).OrderByDescending(x => x.ModifiedDate ?? x.CreatedDate).ToList();
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblOrderTranList;
        }
        #endregion

        #region Get Exch Orders Pickup Pending List
        /// <summary>
        /// Get Exch Orders Pickup Pending List
        /// </summary>
        /// <param name="buid"></param>
        /// <param name="DateTimeByElapsedHours"></param>
        /// <param name="statusId1"></param>
        /// <param name="statusId2"></param>
        /// <param name="statusId3"></param>
        /// <param name="statusId4"></param>
        /// <param name="statusId5"></param>
        /// <param name="statusId6"></param>
        /// <param name="statusId7"></param>
        /// <returns></returns>
        public List<TblLogistic> GetExchOrdersPickupPendingList(int? buid, int? spid, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null)
        {
            List<TblLogistic>? tblLogisticsList = null;
            try
            {
                #region table object Initialization
                tblLogisticsList = _context.TblLogistics
                            .Include(x => x.TblOrderLgcs)
                             .Include(x => x.ServicePartner)
                             .Include(x => x.Status)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                                        .Where(x => x.IsActive == true && x.OrderTrans != null && x.OrderTrans.IsActive == true
                                        && x.StatusId == statusId1 && x.OrderTrans.StatusId == statusId1
                                        && (spid == null || (x.ServicePartner != null && x.ServicePartnerId == spid))
                                        && ((DateTimeByElapsedHours == null) || (x.CreatedDate <= DateTimeByElapsedHours))
                                        && (x.OrderTrans.Exchange != null ?
                                        (buid == null || (x.OrderTrans.Exchange.BusinessUnitId == buid))
                                        : false)
                                        ).OrderByDescending(x => x.ModifiedDate ?? x.CreatedDate).ToList();
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblLogisticsList;
        }
        #endregion

        #region Get ABB Orders Pickup Pending List
        /// <summary>
        /// Get ABB Orders Pickup Pending List
        /// </summary>
        /// <param name="buid"></param>
        /// <param name="DateTimeByElapsedHours"></param>
        /// <param name="statusId1"></param>
        /// <param name="statusId2"></param>
        /// <param name="statusId3"></param>
        /// <param name="statusId4"></param>
        /// <param name="statusId5"></param>
        /// <param name="statusId6"></param>
        /// <param name="statusId7"></param>
        /// <returns></returns>
        public List<TblLogistic> GetABBOrdersPickupPendingList(int? buid, int? spid, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null)
        {
            List<TblLogistic>? tblLogisticsList = null;
            try
            {
                #region table object Initialization
                tblLogisticsList = _context.TblLogistics
                            .Include(x => x.TblOrderLgcs)
                             .Include(x => x.ServicePartner)
                             .Include(x => x.Status)
                                        //Changes for ABB Redemption by Vk 
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                                        //Changes for ABB Redemption by Vk
                                        .Where(x => x.IsActive == true && x.OrderTrans != null && x.OrderTrans.IsActive == true
                                        && x.StatusId == statusId1 && x.OrderTrans.StatusId == statusId1
                                        && (spid == null || (x.ServicePartner != null && x.ServicePartnerId == spid))
                                        && ((DateTimeByElapsedHours == null) || (x.CreatedDate <= DateTimeByElapsedHours))
                                        && ((x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null) ?
                                        (buid == null || (x.OrderTrans.Abbredemption.Abbregistration.BusinessUnitId == buid))
                                        : false)
                                        ).OrderByDescending(x => x.ModifiedDate ?? x.CreatedDate).ToList();
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblLogisticsList;
        }
        #endregion

        #region Get Order Details By userId
        public List<TblOrderTran> GetOrderDetailsByUserId(int userId)
        {
            List<TblOrderTran> tblOrderTran = new List<TblOrderTran>();
            if (userId > 0)
            {
                tblOrderTran = _context.TblOrderTrans.Where(x => x.IsActive == true && x.ModifiedBy == userId).ToList();
            }
            return tblOrderTran;
        }
        #endregion


        #region Get assign orders for qc users
        public TblOrderTran GetQcDetailsByAssignUsers(int? Id, int? userId)
        {
            TblOrderTran TblOrderTran = null;

            if (Id > 0)
            {
                TblOrderTran = _context.TblOrderTrans.FirstOrDefault(x => x.IsActive == true && x.ExchangeId == Id && x.AssignTo == userId);
            }

            return TblOrderTran;

        }
        #endregion

        #region Get Product Details and Customer Details for Diagnose V2 
        /// <summary>
        /// Get Product Details and Customer Details for Diagnose V2 
        /// </summary>
        /// <param name="regdNo"></param>
        /// <returns></returns>
        public TblOrderTran? GetOrderDetailsByRegdNo(string? regdNo)
        {
            TblOrderTran? tblOrderTrans = null;
            if (!string.IsNullOrEmpty(regdNo))
            {
                tblOrderTrans = _context.TblOrderTrans
                    .Include(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Include(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.Exchange).ThenInclude(x => x.Brand)
                    .Include(x => x.Exchange).ThenInclude(x => x.ProductTechnology)
                    .Where(x => x.IsActive == true && x.RegdNo == regdNo).FirstOrDefault();
            }
            return tblOrderTrans;

        }
        #endregion

        #region Get Order Trans Exchange and ABB Redemption Details
        public TblOrderTran GetTransExchAbbByRegdno(string regdno)
        {
            TblOrderTran? TblOrderTran = null;

            if (!string.IsNullOrEmpty(regdno))
            {
                TblOrderTran = _context.TblOrderTrans
                    .Include(x => x.Exchange)
                    .Include(x => x.Abbredemption)
                    .Where(x => x.IsActive == true && (x.RegdNo??"").ToLower() == regdno.ToLower())
                    .FirstOrDefault();
            }

            return TblOrderTran;
        }
        #endregion
    }
}