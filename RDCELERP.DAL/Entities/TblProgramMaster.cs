using System;
using System.Collections.Generic;

#nullable disable

namespace RDCELERP.DAL.Entities
{
    public partial class TblProgramMaster
    {
        public int ProgramMasterId { get; set; }
        public string LoginCredentials { get; set; }
        public string SponsorNew { get; set; }
        public string SponsorName { get; set; }
        public string ProgramCode { get; set; }
        public string Exchange { get; set; }
        public string ExchangePriceCode { get; set; }
        public string Abb { get; set; }
        public string AbbpriceCode { get; set; }
        public string PreSvcQc { get; set; }
        public string ProgramStartDate { get; set; }
        public string ProgramEndDate { get; set; }
        public string PaymentTo { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
