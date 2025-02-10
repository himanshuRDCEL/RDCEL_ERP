using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.TicketGenrateModel
{
    public class EvcAllocationReportListDataContract
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public List<EvcAllocationReportData>? data { get; set; }
    }

    public class EvcAllocationReportData
    {
        [DataMember]
        public string? Product_Group { get; set; }
        [DataMember]
        public string? Status { get; set; }
        [DataMember]
        public string? Regd_No { get; set; }
        [DataMember]
        public string? Cust_Pin_Code { get; set; }
        [DataMember]
        public string? EVC_Name { get; set; }
        [DataMember]
        public string? Prexo_Technology { get; set; }

        [DataMember]
        public string? RegdNoActualTotalAmountAsPerQC { get; set; }
        [DataMember]
        public string? Exchange_Price_Displayed_Max { get; set; }
        [DataMember]
        public string? RegdNoLatestDateTime { get; set; }
        [DataMember]
        public string? ID { get; set; }
        [DataMember]
        public string? RegdNoUploadDateTime { get; set; }
        [DataMember]
        public string? RegdNoLatestStatus { get; set; }
        [DataMember]
        public string? Address_Line_1 { get; set; }
        [DataMember]
        public string? Address_Line_2 { get; set; }
        [DataMember]
        public string? City1 { get; set; }
        [DataMember]
        public string? State { get; set; }
        [DataMember]
        public string? EVC_PIN_Code { get; set; }
        [DataMember]
        public string? EVC_Address { get; set; }
    }
}
