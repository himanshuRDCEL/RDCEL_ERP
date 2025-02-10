using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface IOrderTransRepository : IAbstractRepository<TblOrderTran>
    {
        #region
        TblOrderTran GetQcDetailsByExchangeId(int? Id);
        TblOrderTran GetRegdno(string regdno);
        int UpdateTransRecordStatus(int transid, int? status, int? userid);
        public TblOrderTran GetRegdnoinTicketG(string regdno);
        TblOrderTran GetSingleOrder(int Id);
        TblOrderTran GetSingleOrderWithExchangereference(int Id);
        public TblOrderTran GetOrderDetailsByTransId(int OrderTransId);
        TblOrderTran GetOrderDataForSettelment(string RegdNo);
        TblOrderTran GetTransactionDetailsForABBRedemption(string RergdNo);
        public TblOrderTran GetDetailsByRedemptionId(int? Id);
        public TblOrderTran GetOrderTransByRegdno(string regdno);
        TblOrderTran GetOrdertransDetails(string RegdNo);
        public TblOrderTran GetABBOrderDetailsByTransId(int Id);
        public TblOrderTran GetQcDetailsByABBRedemptionId(int? Id);       
        public TblOrderTran GetExchangeABBbyordertrans(int Id);
        public TblOrderTran GetOrderDetailsByOrderTransId(int OrderTransId);
        public TblOrderTran UpdateStatusOnBaseTblsByTransId(int transid, int? status, int? userid, string statusDesc);
        public List<TblOrderTran> GetOrderDetailsByUserId(int userId);
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
            int? statusId5 = null, int? statusId6 = null, int? statusId7 = null);
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
            int? statusId5 = null, int? statusId6 = null, int? statusId7 = null);
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
             int? statusId1 = null);
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
             int? statusId1 = null);
        #endregion

        public TblOrderTran GetQcDetailsByAssignUsers(int? Id, int? userId);

        #region Get Product Details and Customer Details for Diagnose V2 
        /// <summary>
        /// Get Product Details and Customer Details for Diagnose V2 
        /// </summary>
        /// <param name="regdNo"></param>
        /// <returns></returns>
        public TblOrderTran? GetOrderDetailsByRegdNo(string? regdNo);
        #endregion

        #region Get Order Trans Exchange and ABB Redemption Details
        /// <summary>
        /// Get Order Trans Exchange and ABB Redemption Details
        /// </summary>
        /// <param name="regdno"></param>
        /// <returns>TblOrderTran</returns>
        public TblOrderTran GetTransExchAbbByRegdno(string regdno);
        #endregion

    }
}
