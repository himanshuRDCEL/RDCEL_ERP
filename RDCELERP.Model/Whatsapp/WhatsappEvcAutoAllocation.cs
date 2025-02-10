using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Whatsapp
{
    public class WhatsappEvcAutoAllocation
    {
        public class WhatsappEvcAutoAllocationTemplate
        {
            public UserDetailsAutoAllocation? userDetails { get; set; }
            public AutoAllocation? notification { get; set; }
        }

        public class AutoAllocation
        {
            public string? type { get; set; }
            public string? sender { get; set; }
            public string? templateId { get; set; }
            public AutoAllocationURL? @params { get; set; }
        }

        public class UserDetailsAutoAllocation
        {
            public string? number { get; set; }
        }

        public class AutoAllocationURL
        {
            [JsonProperty("1")]
            public string? Customername { get; set; }

            [JsonProperty("2")]
            public string? OrderNumber { get; set; }

            [JsonProperty("3")]
            public decimal? EvcPrice { get; set; }

            [JsonProperty("4")]
            public string? ProductCategory { get; set; }
            [JsonProperty("5")]
            public string? ProductType { get; set; }

        }

        public class WhatasappEvcAutoAllocationResponse
        {
            public string? msgId { get; set; }
            public string? message { get; set; }
        }
    }
}
