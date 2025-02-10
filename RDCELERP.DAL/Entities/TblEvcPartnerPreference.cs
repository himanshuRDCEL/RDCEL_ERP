using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblEvcPartnerPreference
    {
        public int EvcPartnerpreferenceId { get; set; }
        public int EvcpartnerId { get; set; }
        public int ProductCatId { get; set; }
        public int ProductQualityId { get; set; }
        public bool? IsActive { get; set; }
        public int? Createdby { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblUser? CreatedbyNavigation { get; set; }
        public virtual TblEvcPartner Evcpartner { get; set; } = null!;
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblProductCategory ProductCat { get; set; } = null!;
        public virtual TblConfiguration ProductQuality { get; set; } = null!;
    }
}
