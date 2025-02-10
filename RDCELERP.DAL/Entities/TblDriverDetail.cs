using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblDriverDetail
    {
        public TblDriverDetail()
        {
            TblExchangeAbbstatusHistories = new HashSet<TblExchangeAbbstatusHistory>();
            TblLogistics = new HashSet<TblLogistic>();
            TblOrderLgcs = new HashSet<TblOrderLgc>();
            TblVehicleJourneyTrackingDetails = new HashSet<TblVehicleJourneyTrackingDetail>();
            TblVehicleJourneyTrackings = new HashSet<TblVehicleJourneyTracking>();
        }

        public int DriverDetailsId { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhoneNumber { get; set; }
        public string? VehicleNumber { get; set; }
        public string? City { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Modifiedby { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? VehicleRcNumber { get; set; }
        public string? VehicleRcCertificate { get; set; }
        public string? VehiclefitnessCertificate { get; set; }
        public string? DriverlicenseNumber { get; set; }
        public string? DriverlicenseImage { get; set; }
        public bool? IsApproved { get; set; }
        public int? ApprovedBy { get; set; }
        public string? ProfilePicture { get; set; }
        public string? VehicleInsuranceCertificate { get; set; }
        public int? UserId { get; set; }
        public int? CityId { get; set; }
        public int? ServicePartnerId { get; set; }
        public int? DriverId { get; set; }
        public DateTime? JourneyPlanDate { get; set; }
        public int? VehicleId { get; set; }

        public virtual TblCity? CityNavigation { get; set; }
        public virtual TblDriverList? Driver { get; set; }
        public virtual TblUser? User { get; set; }
        public virtual TblVehicleList? Vehicle { get; set; }
        public virtual ICollection<TblExchangeAbbstatusHistory> TblExchangeAbbstatusHistories { get; set; }
        public virtual ICollection<TblLogistic> TblLogistics { get; set; }
        public virtual ICollection<TblOrderLgc> TblOrderLgcs { get; set; }
        public virtual ICollection<TblVehicleJourneyTrackingDetail> TblVehicleJourneyTrackingDetails { get; set; }
        public virtual ICollection<TblVehicleJourneyTracking> TblVehicleJourneyTrackings { get; set; }
    }
}
