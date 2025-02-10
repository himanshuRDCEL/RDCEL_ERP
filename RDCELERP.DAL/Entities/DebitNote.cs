using System;
using System.Collections.Generic;

#nullable disable

namespace RDCELERP.DAL.Entities
{
    public partial class DebitNote
    {
        public string InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public string RNo { get; set; }
        public double? Quantity { get; set; }
        public double? ItemTotal { get; set; }
        public double? ItemPrice { get; set; }
        public string InvoiceDate1 { get; set; }
        public string InvoiceNumber1 { get; set; }
        public string CustomerName1 { get; set; }
    }
}
