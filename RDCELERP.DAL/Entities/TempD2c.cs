using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TempD2c
    {
        public double? PriceMasterQuestionerId { get; set; }
        public double? ProductTypeId { get; set; }
        public string? BusinessUnitId { get; set; }
        public double? ProductTechnologyId { get; set; }
        public double? AverageSellingPrice { get; set; }
        public double? NonWorkingPrice { get; set; }
        public double? IsActive { get; set; }
        public double? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedDate { get; set; }
        public double? ProductCatId { get; set; }
    }
}
