using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblTransMasterAbbplanMaster
    {
        public int Id { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? AbbregistrationId { get; set; }
        public string? Sponsor { get; set; }
        public int? FromMonth { get; set; }
        public int? ToMonth { get; set; }
        public int? AssuredBuyBackPercentage { get; set; }
        public int? ProductTypeId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblAbbregistration? Abbregistration { get; set; }
        public virtual TblBusinessUnit? BusinessUnit { get; set; }
    }
}
