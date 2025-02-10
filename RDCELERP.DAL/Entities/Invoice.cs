using System;
using System.Collections.Generic;

#nullable disable

namespace RDCELERP.DAL.Entities
{
    public partial class Invoice
    {
        public string InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public string RNo { get; set; }
        public double? Quantity { get; set; }
        public double? BasicInvoiceAmount { get; set; }
        public double? WithGstInvoiceAmount { get; set; }
        public string InvoiceDate1 { get; set; }
        public string InvoiceNumber1 { get; set; }
        public string CustomerName1 { get; set; }
        public string CgstRate { get; set; }
        public string SgstRate { get; set; }
        public string IgstRate { get; set; }
        public string CessRate { get; set; }
        public string CgstFcy { get; set; }
        public string SgstFcy { get; set; }
        public string IgstFcy { get; set; }
        public string CessFcy { get; set; }
        public string Cgst { get; set; }
        public string Sgst { get; set; }
        public string Igst { get; set; }
        public string Cess { get; set; }
        public string ItemTax { get; set; }
        public string ItemTax1 { get; set; }
        public double? ItemTaxAmount { get; set; }
        public string ItemTaxType { get; set; }
    }
}
