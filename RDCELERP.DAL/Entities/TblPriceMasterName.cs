using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblPriceMasterName
    {
        public TblPriceMasterName()
        {
            Logins = new HashSet<Login>();
            TblExchangeOrders = new HashSet<TblExchangeOrder>();
            TblPriceMasterMappings = new HashSet<TblPriceMasterMapping>();
            TblUniversalPriceMasters = new HashSet<TblUniversalPriceMaster>();
        }

        public int PriceMasterNameId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual ICollection<Login> Logins { get; set; }
        public virtual ICollection<TblExchangeOrder> TblExchangeOrders { get; set; }
        public virtual ICollection<TblPriceMasterMapping> TblPriceMasterMappings { get; set; }
        public virtual ICollection<TblUniversalPriceMaster> TblUniversalPriceMasters { get; set; }
    }
}
