using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblServicePartner
    {
        public TblServicePartner()
        {
            MapServicePartnerCityStates = new HashSet<MapServicePartnerCityState>();
            TblDriverLists = new HashSet<TblDriverList>();
            TblExchangeAbbstatusHistories = new HashSet<TblExchangeAbbstatusHistory>();
            TblLogistics = new HashSet<TblLogistic>();
            TblVehicleJourneyTrackingDetails = new HashSet<TblVehicleJourneyTrackingDetail>();
            TblVehicleJourneyTrackings = new HashSet<TblVehicleJourneyTracking>();
            TblVehicleLists = new HashSet<TblVehicleList>();
        }

        public int ServicePartnerId { get; set; }
        public string? ServicePartnerName { get; set; }
        public string? ServicePartnerDescription { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Modifiedby { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsServicePartnerLocal { get; set; }
        public int? UserId { get; set; }
        public string? ServicePartnerRegdNo { get; set; }
        public string? ServicePartnerMobileNumber { get; set; }
        public string? ServicePartnerAlternateMobileNumber { get; set; }
        public string? ServicePartnerEmailId { get; set; }
        public string? ServicePartnerAddressLine1 { get; set; }
        public string? ServicePartnerAddressLine2 { get; set; }
        public string? ServicePartnerPinCode { get; set; }
        public int? ServicePartnerCityId { get; set; }
        public int? ServicePartnerStateId { get; set; }
        public string? ServicePartnerGstno { get; set; }
        public string? ServicePartnerGstregisteration { get; set; }
        public string? ServicePartnerBankName { get; set; }
        public string? ServicePartnerIfsccode { get; set; }
        public string? ServicePartnerBankAccountNo { get; set; }
        public int? ServicePartnerInsertOtp { get; set; }
        public int? ServicePartnerLoginId { get; set; }
        public bool? ServicePartnerIsApprovrd { get; set; }
        public string? ServicePartnerCancelledCheque { get; set; }
        public bool? IconfirmTermsCondition { get; set; }
        public string? ServicePartnerAadharfrontImage { get; set; }
        public string? ServicePartnerAadharBackImage { get; set; }
        public string? ServicePartnerProfilePic { get; set; }
        public string? ServicePartnerFirstName { get; set; }
        public string? ServicePartnerLastName { get; set; }
        public string? ServicePartnerBusinessName { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedbyNavigation { get; set; }
        public virtual TblCity? ServicePartnerCity { get; set; }
        public virtual TblState? ServicePartnerState { get; set; }
        public virtual TblUser? User { get; set; }
        public virtual ICollection<MapServicePartnerCityState> MapServicePartnerCityStates { get; set; }
        public virtual ICollection<TblDriverList> TblDriverLists { get; set; }
        public virtual ICollection<TblExchangeAbbstatusHistory> TblExchangeAbbstatusHistories { get; set; }
        public virtual ICollection<TblLogistic> TblLogistics { get; set; }
        public virtual ICollection<TblVehicleJourneyTrackingDetail> TblVehicleJourneyTrackingDetails { get; set; }
        public virtual ICollection<TblVehicleJourneyTracking> TblVehicleJourneyTrackings { get; set; }
        public virtual ICollection<TblVehicleList> TblVehicleLists { get; set; }
    }
}
