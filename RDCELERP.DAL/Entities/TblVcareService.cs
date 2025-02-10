using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblVcareService
    {
        public int Id { get; set; }
        public string? SponsorName { get; set; }
        public string? SpOrderNo { get; set; }
        public string? Salutation { get; set; }
        public string? Cust1stName { get; set; }
        public string? LastName { get; set; }
        public string? CustAdd1 { get; set; }
        public string? CustAdd2 { get; set; }
        public string? Landmark { get; set; }
        public string? CustPinCode { get; set; }
        public string? CustCity { get; set; }
        public string? CustState { get; set; }
        public string? CustMobile { get; set; }
        public string? Custemail { get; set; }
        public string? ExchProdGroup { get; set; }
        public string? ExchProdType { get; set; }
        public string? ExchBrand { get; set; }
        public string? ExchSize { get; set; }
        public string? ExchAge { get; set; }
        public string? ExchCondition { get; set; }
        public string? UtcregNo { get; set; }
        public string? QuoteP { get; set; }
        public string? QuoteQ { get; set; }
        public string? QuoteR { get; set; }
        public string? QuoteS { get; set; }
        public string? CallNo { get; set; }
    }
}
