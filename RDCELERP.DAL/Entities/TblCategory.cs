using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblCategory
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public int? BrandId { get; set; }
        public int? CompanyId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? BusinessUnitId { get; set; }

        public virtual TblBrand? Brand { get; set; }
        public virtual TblCompany? Company { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
    }
}
