using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface IVehicleJourneyTrackingRepository : IAbstractRepository<TblVehicleJourneyTracking>
    {
        public TblVehicleJourneyTracking? GetVehicleJourneyTrackingById(int trackingId);
        public void UpdateVehicleJourneyTracking(TblVehicleJourneyTracking VehicleJourneyTracking);
        public List<TblVehicleJourneyTracking> GetJourneyDetailsByServicePIdorDriverId(int? driverId, int servicePartnerId);
    }
}
