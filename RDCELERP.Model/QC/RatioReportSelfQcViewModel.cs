using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.QC
{
    public class RatioReportSelfQcViewModel
    {
        public int? Id {  get; set; }
        public string? CompanyName { get; set; }
        public string? RegdNo { get; set; }
        public string? CustCity { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductCondition { get; set; }
        public string? StatusCode { get; set; }
        public string? OrderCreatedDate { get; set; }
        public string? CustFullname { get; set; }
        public string? CustAddress { get; set; }
        public string? CustState { get; set; }
        public string? CustPincode { get; set; }
        public string? UserEmailId { get; set; }
        public string? Action { get; set; }
    }
}
