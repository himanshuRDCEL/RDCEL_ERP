using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblOrderTran
    {
        public TblOrderTran()
        {
            TblCustomerFiles = new HashSet<TblCustomerFile>();
            TblEvcdisputes = new HashSet<TblEvcdispute>();
            TblEvcpoddetails = new HashSet<TblEvcpoddetail>();
            TblEvcwalletHistories = new HashSet<TblEvcwalletHistory>();
            TblExchangeAbbstatusHistories = new HashSet<TblExchangeAbbstatusHistory>();
            TblLogistics = new HashSet<TblLogistic>();
            TblNpssqresponses = new HashSet<TblNpssqresponse>();
            TblOrderLgcs = new HashSet<TblOrderLgc>();
            TblOrderQcratings = new HashSet<TblOrderQcrating>();
            TblOrderQcs = new HashSet<TblOrderQc>();
            TblTempData = new HashSet<TblTempDatum>();
            TblVehicleJourneyTrackingDetails = new HashSet<TblVehicleJourneyTrackingDetail>();
            TblWalletTransactions = new HashSet<TblWalletTransaction>();
        }

        public int OrderTransId { get; set; }
        public int? OrderType { get; set; }
        public int? ExchangeId { get; set; }
        public int? AbbredemptionId { get; set; }
        public string? SponsorOrderNumber { get; set; }
        public string? RegdNo { get; set; }
        public decimal? ExchangePrice { get; set; }
        public decimal? QuotedPrice { get; set; }
        public decimal? Sweetner { get; set; }
        public decimal? FinalPriceAfterQc { get; set; }
        public int? EvcpriceMasterId { get; set; }
        public decimal? Evcprice { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? StatusId { get; set; }
        public bool? AmountPaidToCustomer { get; set; }
        public int? SelfQclinkResendby { get; set; }
        public int? AssignBy { get; set; }
        public int? AssignTo { get; set; }

        public virtual TblAbbredemption? Abbredemption { get; set; }
        public virtual TblUser? AssignByNavigation { get; set; }
        public virtual TblUser? AssignToNavigation { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblExchangeOrder? Exchange { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblLoV? OrderTypeNavigation { get; set; }
        public virtual TblUser? SelfQclinkResendbyNavigation { get; set; }
        public virtual TblExchangeOrderStatus? Status { get; set; }
        public virtual ICollection<TblCustomerFile> TblCustomerFiles { get; set; }
        public virtual ICollection<TblEvcdispute> TblEvcdisputes { get; set; }
        public virtual ICollection<TblEvcpoddetail> TblEvcpoddetails { get; set; }
        public virtual ICollection<TblEvcwalletHistory> TblEvcwalletHistories { get; set; }
        public virtual ICollection<TblExchangeAbbstatusHistory> TblExchangeAbbstatusHistories { get; set; }
        public virtual ICollection<TblLogistic> TblLogistics { get; set; }
        public virtual ICollection<TblNpssqresponse> TblNpssqresponses { get; set; }
        public virtual ICollection<TblOrderLgc> TblOrderLgcs { get; set; }
        public virtual ICollection<TblOrderQcrating> TblOrderQcratings { get; set; }
        public virtual ICollection<TblOrderQc> TblOrderQcs { get; set; }
        public virtual ICollection<TblTempDatum> TblTempData { get; set; }
        public virtual ICollection<TblVehicleJourneyTrackingDetail> TblVehicleJourneyTrackingDetails { get; set; }
        public virtual ICollection<TblWalletTransaction> TblWalletTransactions { get; set; }
    }
}
