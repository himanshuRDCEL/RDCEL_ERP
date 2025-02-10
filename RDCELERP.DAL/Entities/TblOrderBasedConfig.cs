using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblOrderBasedConfig
    {
        public int Id { get; set; }
        public int BusinessUnitId { get; set; }
        public int? BusinessPartnerId { get; set; }
        public int? BrandId { get; set; }
        public bool? IsBpmultiBrand { get; set; }
        public bool? IsValidationBasedSweetener { get; set; }
        public bool? IsSweetenerModalBased { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual TblBrand? Brand { get; set; }
        public virtual TblBusinessPartner? BusinessPartner { get; set; }
        public virtual TblBusinessUnit BusinessUnit { get; set; } = null!;
    }
}
