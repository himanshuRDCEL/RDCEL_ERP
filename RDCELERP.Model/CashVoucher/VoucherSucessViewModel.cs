using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.CashVoucher
{
    public class VoucherSucessViewModel
    {
        public class VoucherSucessResponseModel
        {
            public Data? data { get; set; }
        }

        public class BRAND
        {
            public int BRAND_DECLINE { get; set; }
        }

        public class CONSUMER
        {
            public int CONSUMER_WALLET { get; set; }
        }

        public class Context
        {
            public EVENT? EVENT { get; set; }
            public BRAND? BRAND { get; set; }
            public CONSUMER? CONSUMER { get; set; }
            public int D1 { get; set; }
            public int C1 { get; set; }
            public int D2 { get; set; }
            public int C2 { get; set; }
        }

        public class Data
        {
            public string? status { get; set; }
            public string? reason { get; set; }
            public string? event_qrn { get; set; }
            public string? event_rrn { get; set; }
            public Context? context { get; set; }
            public string? voucher_id { get; set; }
        }

        public class EVENT
        {
            public string? service_id { get; set; }
            public int amount { get; set; }
            public string? expiry { get; set; }
            public int beneficiary_ref_id { get; set; }
            public int consumer_ref_id { get; set; }
            public int issuer_ref_id { get; set; }
            public int brand_ref_id { get; set; }
            public string? voucher_id { get; set; }
            public int dealer_ref_id { get; set; }
            public int acquirer_ref_id { get; set; }
            public string? merchant_ref_id { get; set; }
        }

        public class Root
        {
            public Data? data { get; set; }
        }
    }
}
