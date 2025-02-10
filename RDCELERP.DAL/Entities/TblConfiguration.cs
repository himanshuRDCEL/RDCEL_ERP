using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblConfiguration
    {
        public TblConfiguration()
        {
            TblEvcPartnerPreferences = new HashSet<TblEvcPartnerPreference>();
        }

        public int ConfigId { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<TblEvcPartnerPreference> TblEvcPartnerPreferences { get; set; }
    }
}
