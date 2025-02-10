using System;
using System.Collections.Generic;

#nullable disable

namespace RDCELERP.DAL.Entities
{
    public partial class MyJioStore
    {
        public double? Store { get; set; }
        public string OperationalStatus { get; set; }
        public string StoreName { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zone { get; set; }
        public string MasterState { get; set; }
        public string MasterCity { get; set; }
        public string StoreCode { get; set; }
        public double? PinCode { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string StoreTimings { get; set; }
        public string OpenDays { get; set; }
    }
}
