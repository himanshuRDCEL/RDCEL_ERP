using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Whatsapp
{
    public class WhatsappDiagnosticReport
    {

        public class WhatsappDiagnosticTemplate
        {
            public UserDetailsDiagnostic? userDetails { get; set; }
            public DiagnosticReport? notification { get; set; }
        }

        public class DiagnosticReport
        {
            public string? type { get; set; }
            public string? sender { get; set; }
            public string? templateId { get; set; }
            public DiagnosticURL? @params { get; set; }
        }

        public class UserDetailsDiagnostic
        {
            public string? number { get; set; }
        }

        public class DiagnosticURL
        {
            [JsonProperty("1")]
            public string? Customername { get; set; }

            [JsonProperty("2")]
            public string? ProductName { get; set; }

            [JsonProperty("3")]
            public decimal? Price { get; set; }

            [JsonProperty("4")]
            public string? Link { get; set; }

        }

        public class WhatasappDiagnosticResponse
        {
            public string? msgId { get; set; }
            public string? message { get; set; }
        }

    }
}
