using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.Paymant
{
    public class RePaymentViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string? RegdNo { get; set; }
        public string? TransactionId { get; set; }
        public string? OrderId { get; set; }
        public bool? IsActive { get; set; }
        public bool? PaymentStatus { get; set; }
        public string? PaymentResponse { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? Amount { get; set; }
        public string? Bank { get; set; }
        public string? BankId { get; set; }
        public string? CardId { get; set; }
        public string? CardScheme { get; set; }
        public string? CardToken { get; set; }
        public string? CardHashId { get; set; }
        public string? CheckSum { get; set; }
        public string? PaymentMode { get; set; }
        public string? ResponseCode { get; set; }
        public string? ResponseDescription { get; set; }
        public string? TransactionType { get; set; }
        public string? ModuleType { get; set; }
        public int? ModuleReferenceId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? GatewayTransactioId { get; set; }
        public string? UtcReferenceId { get; set; }
        public string? ActionURL { get; set; }
        public string? OrderStatus { get; set; }
    }
}
