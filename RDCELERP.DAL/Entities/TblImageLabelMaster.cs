using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblImageLabelMaster
    {
        public TblImageLabelMaster()
        {
            TblTempData = new HashSet<TblTempDatum>();
        }

        public int ImageLabelid { get; set; }
        public string? ProductName { get; set; }
        public string? ProductImageLabel { get; set; }
        public string? ProductImageDescription { get; set; }
        public int? Pattern { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Modifiedby { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ProductCatId { get; set; }
        public int? ProductTypeId { get; set; }
        public string? ImagePlaceHolder { get; set; }
        public bool? IsMediaTypeVideo { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedbyNavigation { get; set; }
        public virtual TblProductCategory? ProductCat { get; set; }
        public virtual TblProductType? ProductType { get; set; }
        public virtual ICollection<TblTempDatum> TblTempData { get; set; }
    }
}
