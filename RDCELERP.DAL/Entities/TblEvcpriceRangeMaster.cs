using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblEvcpriceRangeMaster
    {
        public int EvcpriceRangeMasterId { get; set; }
        public int? PriceStartRange { get; set; }
        public int? PriceEndRange { get; set; }
        public decimal? EvcApplicablePercentage { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? BusinessUnitId { get; set; }
    }
}
