using System;
using System.Collections.Generic;

#nullable disable

namespace RDCELERP.DAL.Entities
{
    public partial class Rdstore
    {
        public string Store { get; set; }
        public string City { get; set; }
        public string MasterState { get; set; }
        public string MasterCity { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string Format { get; set; }
        public string StoreAddress { get; set; }
        public double? PinCode { get; set; }
        public string StoreTimings { get; set; }
        public string PropertyType { get; set; }
        public string NoOfFloors { get; set; }
    }
}
