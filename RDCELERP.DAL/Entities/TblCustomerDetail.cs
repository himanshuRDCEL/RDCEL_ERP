using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblCustomerDetail
    {
        public TblCustomerDetail()
        {
            TblAbbredemptions = new HashSet<TblAbbredemption>();
            TblAbbregistrations = new HashSet<TblAbbregistration>();
            TblExchangeOrders = new HashSet<TblExchangeOrder>();
            TblFeedBacks = new HashSet<TblFeedBack>();
            TblHistories = new HashSet<TblHistory>();
            TblVoucherVerfications = new HashSet<TblVoucherVerfication>();
        }

        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? PhoneNumber { get; set; }
        public string? State { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? SponsorRefId { get; set; }
        public string? AreaLocality { get; set; }
        public int? AreaLocalityId { get; set; }

        public virtual ICollection<TblAbbredemption> TblAbbredemptions { get; set; }
        public virtual ICollection<TblAbbregistration> TblAbbregistrations { get; set; }
        public virtual ICollection<TblExchangeOrder> TblExchangeOrders { get; set; }
        public virtual ICollection<TblFeedBack> TblFeedBacks { get; set; }
        public virtual ICollection<TblHistory> TblHistories { get; set; }
        public virtual ICollection<TblVoucherVerfication> TblVoucherVerfications { get; set; }
    }
}
