using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblQcratingMasterMapping
    {
        public int QcratingMasterMappingId { get; set; }
        public int? QcratingId { get; set; }
        public int? ProductCatId { get; set; }
        public int? ProductTypeId { get; set; }
        public int? ProductTechnologyId { get; set; }
        public int? QuestsSequence { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblProductCategory? ProductCat { get; set; }
        public virtual TblProductTechnology? ProductTechnology { get; set; }
        public virtual TblProductType? ProductType { get; set; }
        public virtual TblQcratingMaster? Qcrating { get; set; }
    }
}
