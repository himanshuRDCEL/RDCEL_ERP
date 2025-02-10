using Newtonsoft.Json;

namespace RDCELERP.Model.Whatsapp
{
    public class AssignEVCViewModel
    {

        public class AssignEVC_order
        {
            public string? type { get; set; }
            public string? sender { get; set; }
            public string? templateId { get; set; }
            public EVCDetalis? @params { get; set; }
        }

        public class WhatsappTemplate
        {
            public EVCNumber? EvcNumber { get; set; }
            public AssignEVC_order? notification { get; set; }
        }

        public class EVCNumber
        {
            public string? number { get; set; }
        }

        public class WhatasappResponse
        {
            public string? msgId { get; set; }
            public string? message { get; set; }
        }

        public class EVCDetalis
        {
            [JsonProperty("1")]
            public long  WalletAmount { get; set; }

        }
    }
}