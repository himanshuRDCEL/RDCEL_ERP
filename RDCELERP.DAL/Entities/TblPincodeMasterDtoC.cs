using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblPincodeMasterDtoC
    {
        public int Id { get; set; }
        public string? PinCode { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public bool? IsService { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Modifiedby { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? AreaName { get; set; }
    }
}
