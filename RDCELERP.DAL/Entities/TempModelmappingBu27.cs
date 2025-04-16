using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TempModelmappingBu27
    {
        public int? ModelId { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? BusinessPartnerId { get; set; }
        public int? BrandId { get; set; }
        public decimal? SweetenerBu { get; set; }
        public decimal? SweetenerBp { get; set; }
        public decimal? SweetenerDigi2l { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsDefault { get; set; }
    }
}
