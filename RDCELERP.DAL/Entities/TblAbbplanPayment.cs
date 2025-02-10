using System;
using System.Collections.Generic;

#nullable disable

namespace RDCELERP.DAL.Entities
{
    public partial class TblAbbplanPayment
    {
        public int Id { get; set; }
        public string RegdNo { get; set; }
        public string TransactionId { get; set; }
        public string OrderId { get; set; }
        public bool? IsActive { get; set; }
        public bool? PaymentStatus { get; set; }
        public string PaymentResponse { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? Amount { get; set; }
        public string Bank { get; set; }
        public string BankId { get; set; }
        public string CardId { get; set; }
        public string CardScheme { get; set; }
        public string CardToken { get; set; }
        public string CardHashId { get; set; }
        public string CheckSum { get; set; }
        public string PaymentMode { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
    }
}
