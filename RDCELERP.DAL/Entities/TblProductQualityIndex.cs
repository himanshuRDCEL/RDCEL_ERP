using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblProductQualityIndex
    {
        public int ProductQualityIndexId { get; set; }
        public string? Name { get; set; }
        public int? ProductCategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? ExcellentDesc { get; set; }
        public string? GoodDesc { get; set; }
        public string? AverageDesc { get; set; }
        public string? NonWorkingDesc { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblProductCategory? ProductCategory { get; set; }
    }
}
