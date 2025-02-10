using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.DAL.Repository
{
    public class VehicleJourneyTrackingRepository : AbstractRepository<TblVehicleJourneyTracking>, IVehicleJourneyTrackingRepository
    {
        Digi2l_DevContext _context;
        public VehicleJourneyTrackingRepository(Digi2l_DevContext dbContext)
          : base(dbContext)
        {
            _context = dbContext;
        }

        public TblVehicleJourneyTracking? GetVehicleJourneyTrackingById(int trackingId)
        {
            TblVehicleJourneyTracking? tblVehicleJourneyTracking = null;
            if (trackingId > 0)
            {
                tblVehicleJourneyTracking = _context.TblVehicleJourneyTrackings.Where(x => x.IsActive == true && x.TrackingId == trackingId).FirstOrDefault();
            }
            return tblVehicleJourneyTracking;
        }

        public void UpdateVehicleJourneyTracking(TblVehicleJourneyTracking VehicleJourneyTracking)
        {
            _context.Update(VehicleJourneyTracking);
            _context.SaveChanges();
        }

        #region Get Journey details by servicePartnerId or driverId
        public List<TblVehicleJourneyTracking> GetJourneyDetailsByServicePIdorDriverId(int? driverId, int servicePartnerId)
        {
            List<TblVehicleJourneyTracking> tblVehicleJourneyTrackings = new List<TblVehicleJourneyTracking>();
            if (driverId > 0)
            {
                tblVehicleJourneyTrackings = _context.TblVehicleJourneyTrackings.Where(x => x.IsActive == true && x.DriverId == driverId && x.ServicePartnerId == servicePartnerId).ToList();
            }
            else
            {
                tblVehicleJourneyTrackings = _context.TblVehicleJourneyTrackings.Where(x => x.IsActive == true && x.ServicePartnerId == servicePartnerId).ToList();

            }
            return tblVehicleJourneyTrackings;
        }
        #endregion

    }
}
