using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblLoV
    {
        public TblLoV()
        {
            TblBusinessUnitGsttypeNavigations = new HashSet<TblBusinessUnit>();
            TblBusinessUnitMarginTypeNavigations = new HashSet<TblBusinessUnit>();
            TblEvcregistrations = new HashSet<TblEvcregistration>();
            TblOrderTrans = new HashSet<TblOrderTran>();
            TblTempData = new HashSet<TblTempDatum>();
            TblWalletTransactions = new HashSet<TblWalletTransaction>();
        }

        public int LoVid { get; set; }
        public string? LoVname { get; set; }
        public int? ParentId { get; set; }
        public bool? IsEditable { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<TblBusinessUnit> TblBusinessUnitGsttypeNavigations { get; set; }
        public virtual ICollection<TblBusinessUnit> TblBusinessUnitMarginTypeNavigations { get; set; }
        public virtual ICollection<TblEvcregistration> TblEvcregistrations { get; set; }
        public virtual ICollection<TblOrderTran> TblOrderTrans { get; set; }
        public virtual ICollection<TblTempDatum> TblTempData { get; set; }
        public virtual ICollection<TblWalletTransaction> TblWalletTransactions { get; set; }
    }
}
