using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblEvcregistration
    {
        public TblEvcregistration()
        {
            TblEvcPartners = new HashSet<TblEvcPartner>();
            TblEvcdisputes = new HashSet<TblEvcdispute>();
            TblEvcpoddetails = new HashSet<TblEvcpoddetail>();
            TblEvcwalletHistories = new HashSet<TblEvcwalletHistory>();
            TblOrderLgcs = new HashSet<TblOrderLgc>();
            TblVehicleJourneyTrackingDetails = new HashSet<TblVehicleJourneyTrackingDetail>();
            TblWalletTransactions = new HashSet<TblWalletTransaction>();
        }

        public int EvcregistrationId { get; set; }
        public string? ZohoEvcapprovedId { get; set; }
        public string? BussinessName { get; set; }
        public int? EntityTypeId { get; set; }
        public string? EvcregdNo { get; set; }
        public string? ContactPerson { get; set; }
        public string? EvcmobileNumber { get; set; }
        public string? AlternateMobileNumber { get; set; }
        public string? EmailId { get; set; }
        public string? RegdAddressLine1 { get; set; }
        public string? RegdAddressLine2 { get; set; }
        public string? PinCode { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
        public decimal? EvcwalletAmount { get; set; }
        public string? ContactPersonAddress { get; set; }
        public string? UploadGstregistration { get; set; }
        public string? BankName { get; set; }
        public string? Ifsccode { get; set; }
        public string? BankAccountNo { get; set; }
        public int? InsertOtp { get; set; }
        public int? EvcapprovalStatusId { get; set; }
        public bool? Isevcapprovrd { get; set; }
        public string? CopyofCancelledCheque { get; set; }
        public string? EwasteRegistrationNumber { get; set; }
        public string? EwasteCertificate { get; set; }
        public bool? IconfirmTermsCondition { get; set; }
        public string? Pocname { get; set; }
        public string? Pocplace { get; set; }
        public DateTime? Date { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? EmployeeId { get; set; }
        public string? Gstno { get; set; }
        public int? UserId { get; set; }
        public string? AadharfrontImage { get; set; }
        public string? AadharBackImage { get; set; }
        public string? ProfilePic { get; set; }
        public string? EvczohoBookName { get; set; }
        public bool? IsInHouse { get; set; }
        public string? NatureOfBusiness { get; set; }
        public string? Country { get; set; }
        public string? PanNo { get; set; }
        public string? AccountHolder { get; set; }
        public string? Branch { get; set; }
        public string? UtcEmployeeName { get; set; }
        public string? UtcEmployeeEmail { get; set; }
        public string? UtcEmployeeContact { get; set; }
        public string? ApproverName { get; set; }
        public string? UnitDepartment { get; set; }
        public string? ManagerEmail { get; set; }
        public string? ManagerContact { get; set; }
        public string? CompanyRegNo { get; set; }
        public bool? IsSweetenerAmtInclude { get; set; }
        public int? GsttypeId { get; set; }

        public virtual TblCity? City { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblEntityType? EntityType { get; set; }
        public virtual TblLoV? Gsttype { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblState? State { get; set; }
        public virtual ICollection<TblEvcPartner> TblEvcPartners { get; set; }
        public virtual ICollection<TblEvcdispute> TblEvcdisputes { get; set; }
        public virtual ICollection<TblEvcpoddetail> TblEvcpoddetails { get; set; }
        public virtual ICollection<TblEvcwalletHistory> TblEvcwalletHistories { get; set; }
        public virtual ICollection<TblOrderLgc> TblOrderLgcs { get; set; }
        public virtual ICollection<TblVehicleJourneyTrackingDetail> TblVehicleJourneyTrackingDetails { get; set; }
        public virtual ICollection<TblWalletTransaction> TblWalletTransactions { get; set; }
    }
}
