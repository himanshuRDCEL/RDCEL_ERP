using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model
{
    public class GenerateVoucherViewModel
    {
        public GenerateVoucherData? data { get; set; }
    }

    public class GenerateVoucherData
    {
        public string? event_id { get; set; }
        public string? rrn { get; set; }
        public string? dao_name { get; set; }
        public GenerateVoucherPayload? payload { get; set; }
    }

    public class GenerateVoucherPayload
    {
        public string? service_id { get; set; }
        public string? amount { get; set; }
        public string? expiry { get; set; }
        public string? beneficiary_ref_id { get; set; }
        public string? consumer_ref_id { get; set; }
        public string? issuer_ref_id { get; set; }
        public string? brand_ref_id { get; set; }
        public string? merchant_ref_id { get; set; }

    }
}
