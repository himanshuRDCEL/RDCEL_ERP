using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblProductTechnology
    {
        public TblProductTechnology()
        {
            TblExchangeOrders = new HashSet<TblExchangeOrder>();
            TblPriceMasterQuestioners = new HashSet<TblPriceMasterQuestioner>();
        }

        public int ProductTechnologyId { get; set; }
        public string? ProductTechnologyName { get; set; }
        public int? ProductCatId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsusedNew { get; set; }
        public bool? Isusedold { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblProductCategory? ProductCat { get; set; }
        public virtual ICollection<TblExchangeOrder> TblExchangeOrders { get; set; }
        public virtual ICollection<TblPriceMasterQuestioner> TblPriceMasterQuestioners { get; set; }
    }
}
