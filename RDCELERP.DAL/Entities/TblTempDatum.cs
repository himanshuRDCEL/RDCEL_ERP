using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblTempDatum
    {
        public int Id { get; set; }
        public string? RegdNo { get; set; }
        public string? FileName { get; set; }
        public int? LovId { get; set; }
        public int? StatusId { get; set; }
        public int? ImageLabelid { get; set; }
        public int? OrderTransId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Modifiedby { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblImageLabelMaster? ImageLabel { get; set; }
        public virtual TblLoV? Lov { get; set; }
        public virtual TblUser? ModifiedbyNavigation { get; set; }
        public virtual TblOrderTran? OrderTrans { get; set; }
        public virtual TblExchangeOrderStatus? Status { get; set; }
    }
}
