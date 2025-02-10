using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class Model
    {
        public double? ModelNumberId { get; set; }
        public string? ModelName { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        public double? BrandId { get; set; }
        public double? ProductCategoryId { get; set; }
        public double? ProductTypeId { get; set; }
        public double? IsActive { get; set; }
        public double? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedDate { get; set; }
        public double? SweetnerForDtd { get; set; }
        public double? SweetnerForDtc { get; set; }
        public double? IsDefaultProduct { get; set; }
        public double? BusinessUnitId { get; set; }
        public string? SweetenerBu { get; set; }
        public string? SweetenerBp { get; set; }
        public string? SweetenerDigi2l { get; set; }
        public string? BusinessPartnerId { get; set; }
    }
}
