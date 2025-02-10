using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblHistory
    {
        public int Id { get; set; }
        public string? RegdNo { get; set; }
        public string? VoucherCode { get; set; }
        public int? ProductTypeId { get; set; }
        public decimal? Sweetner { get; set; }
        public decimal? ExchangeAmount { get; set; }
        public int? ExchangeOrderId { get; set; }
        public int? CustId { get; set; }
        public DateTime? Createdate { get; set; }
        public DateTime? Modifieddate { get; set; }
        public bool? IsActive { get; set; }

        public virtual TblCustomerDetail? Cust { get; set; }
        public virtual TblExchangeOrder? ExchangeOrder { get; set; }
    }
}
