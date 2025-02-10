using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblEvcPartner
    {
        public TblEvcPartner()
        {
            TblEvcPartnerPreferences = new HashSet<TblEvcPartnerPreference>();
            TblEvcpoddetails = new HashSet<TblEvcpoddetail>();
            TblEvcwalletHistories = new HashSet<TblEvcwalletHistory>();
            TblOrderLgcs = new HashSet<TblOrderLgc>();
            TblVehicleJourneyTrackingDetails = new HashSet<TblVehicleJourneyTrackingDetail>();
            TblWalletTransactions = new HashSet<TblWalletTransaction>();
        }

        public int EvcPartnerId { get; set; }
        public int EvcregistrationId { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? PinCode { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string? EmailId { get; set; }
        public string? ContactNumber { get; set; }
        public string? EvcStoreCode { get; set; }
        public bool? IsActive { get; set; }
        public int? Createdby { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ListOfPincode { get; set; }
        public bool? IsApprove { get; set; }

        public virtual TblCity City { get; set; } = null!;
        public virtual TblUser? CreatedbyNavigation { get; set; }
        public virtual TblEvcregistration Evcregistration { get; set; } = null!;
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblState State { get; set; } = null!;
        public virtual ICollection<TblEvcPartnerPreference> TblEvcPartnerPreferences { get; set; }
        public virtual ICollection<TblEvcpoddetail> TblEvcpoddetails { get; set; }
        public virtual ICollection<TblEvcwalletHistory> TblEvcwalletHistories { get; set; }
        public virtual ICollection<TblOrderLgc> TblOrderLgcs { get; set; }
        public virtual ICollection<TblVehicleJourneyTrackingDetail> TblVehicleJourneyTrackingDetails { get; set; }
        public virtual ICollection<TblWalletTransaction> TblWalletTransactions { get; set; }
    }
}
