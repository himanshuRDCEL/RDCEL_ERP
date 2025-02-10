using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblAbbredemption
    {
        public TblAbbredemption()
        {
            TblOrderTrans = new HashSet<TblOrderTran>();
            TblSelfQcs = new HashSet<TblSelfQc>();
            TblVoucherVerfications = new HashSet<TblVoucherVerfication>();
        }

        public int RedemptionId { get; set; }
        public int? AbbregistrationId { get; set; }
        public int? ZohoAbbredemptionId { get; set; }
        public int? CustomerDetailsId { get; set; }
        public string? RegdNo { get; set; }
        public string? AbbredemptionStatus { get; set; }
        public string? StoreOrderNo { get; set; }
        public string? Sponsor { get; set; }
        public string? LogisticsComments { get; set; }
        public string? Qccomments { get; set; }
        public string? InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string? InvoiceImage { get; set; }
        public int? RedemptionPeriod { get; set; }
        public int? RedemptionPercentage { get; set; }
        public DateTime? RedemptionDate { get; set; }
        public decimal? RedemptionValue { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? StatusId { get; set; }
        public decimal? FinalRedemptionValue { get; set; }
        public string? ReferenceId { get; set; }
        public bool? IsVoucherUsed { get; set; }
        public string? VoucherCode { get; set; }
        public DateTime? VoucherCodeExpDate { get; set; }
        public int? VoucherStatusId { get; set; }
        public bool? IsDefferedSettelment { get; set; }
        public int? BusinessPartnerId { get; set; }

        public virtual TblAbbregistration? Abbregistration { get; set; }
        public virtual TblBusinessPartner? BusinessPartner { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblCustomerDetail? CustomerDetails { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblExchangeOrderStatus? Status { get; set; }
        public virtual TblVoucherStatus? VoucherStatus { get; set; }
        public virtual ICollection<TblOrderTran> TblOrderTrans { get; set; }
        public virtual ICollection<TblSelfQc> TblSelfQcs { get; set; }
        public virtual ICollection<TblVoucherVerfication> TblVoucherVerfications { get; set; }
    }
}
