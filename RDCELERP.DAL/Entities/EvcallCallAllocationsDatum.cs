using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class EvcallCallAllocationsDatum
    {
        public int AllocationsDataId { get; set; }
        public bool? IsDataTransfered { get; set; }
        public string? EvcCode { get; set; }
        public string? EvcBusinessName { get; set; }
        public string? RNo { get; set; }
        public double? CustPinCode { get; set; }
        public string? CustCity { get; set; }
        public string? ProdGroup { get; set; }
        public string? Type { get; set; }
        public string? ActualProdQltyAtTimeOfQc { get; set; }
        public double? ActualEvcAmountAsPerQc { get; set; }
        public string? AllocationStatus { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? EvcAddress { get; set; }
        public string? InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string? ProofOfDelivery { get; set; }
        public string? Errorlog { get; set; }
    }
}
