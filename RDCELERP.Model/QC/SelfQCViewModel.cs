using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.QC
{
    public class SelfQCViewModel
    {
        [Key]
        public int SelfQcid { get; set; }
        public string? ImageName { get; set; }
        public string? RegdNo { get; set; }
        public bool? IsExchange { get; set; }
        public bool? IsAbb { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Modifiedby { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ExchangeOrderId { get; set; }
        public int? RedemptionId { get; set; }
        public string? FilePath { get; set; }
        public string? ImageWithPath { get; set; }
        public bool? IsMediaTypeVideo { get; set; }
        public string? ProductImageLabel { get; set; }
    }
}
