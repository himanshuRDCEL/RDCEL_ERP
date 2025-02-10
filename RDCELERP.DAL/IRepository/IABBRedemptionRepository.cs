using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface IABBRedemptionRepository : IAbstractRepository<TblAbbredemption>
    {
        #region
        public TblAbbredemption GetRegdNo(string regdno);
        #endregion
        #region
        List<TblAbbredemption> GetRedemptionDataList(int? BusinessUnitId, string regdNo, string sponsorOrderNumber, string phoneNumber, string referenceId, string storeCode);

        #endregion
        #region
        TblAbbredemption GetRedemptionData(int? RedemptionId);
        #endregion

        #region Get Order details of only Redemption
        TblAbbredemption GetOrderDetails(string RegdNo, int? ABBRedemptionId);
        #endregion'
        #region
        public TblAbbredemption GetAbbOrderDetails(string regdNo);
        #endregion
        #region
        public TblAbbredemption GetAbbOrderDetailsByOrderTransId(int orderTransId);
        #endregion

        /// <summary>
        /// Update StatusId On TblAbbRedemption and TblAbbRegistration
        /// </summary>
        /// <param name="regdNo"></param>
        /// <param name="statusId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public TblAbbredemption UpdateABBOrderStatus(string regdNo, int statusId, int? loggedInUserId, string statusDesc = null);
    }
}
