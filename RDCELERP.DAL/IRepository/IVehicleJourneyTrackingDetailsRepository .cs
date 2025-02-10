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
    public interface IVehicleJourneyTrackingDetailsRepository : IAbstractRepository<TblVehicleJourneyTrackingDetail>
    {
        public int UpdateVehicleJourney(int OrdertranshId, int? setStatusId, int? loggedUserId);
        public int UpdateVehicleInsentiveDeatils(TblVehicleJourneyTrackingDetail journeyTrackingDetail);
        public TblVehicleJourneyTrackingDetail GetVehicleJourneyTrackingDetail(int OrderTransId);
        public void UpdateLattLong(TblVehicleJourneyTrackingDetail journeyTrackingDetail);

        #region Get Order specific Earning Details by Tracking Details Id SPID
        /// <summary>
        /// Get Order specific Earning Details by Tracking Details Id
        /// </summary>
        /// <param name="trackingDetailsId"></param>
        /// <returns></returns>
        public TblVehicleJourneyTrackingDetail GetDriverEarningDetail(int trackingDetailsId, int spid = 0);
        #endregion
        public List<TblVehicleJourneyTrackingDetail> GetVehicleJourneyTrackingDetailByTrackingId(int TrackingId);

    }
}
