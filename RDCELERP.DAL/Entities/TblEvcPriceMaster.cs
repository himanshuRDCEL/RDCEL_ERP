using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblEvcPriceMaster
    {
        public int EvcpriceMasterId { get; set; }
        public string? SponsorName { get; set; }
        public string? ProductGroup { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? ProductCategoryId { get; set; }
        public string? ProductType { get; set; }
        public int? ProductTypeId { get; set; }
        public string? Size { get; set; }
        public int? EvcP { get; set; }
        public int? EvcQ { get; set; }
        public int? EvcR { get; set; }
        public int? EvcS { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Lgccost { get; set; }
    }
}
