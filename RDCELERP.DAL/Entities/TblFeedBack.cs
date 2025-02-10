using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblFeedBack
    {
        public int Id { get; set; }
        public string? RatingNo { get; set; }
        public int? QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public string? Comment { get; set; }
        public int? CustomerId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public int? ExchangeOrderId { get; set; }

        public virtual TblFeedBackAnswer? Answer { get; set; }
        public virtual TblCustomerDetail? Customer { get; set; }
        public virtual TblExchangeOrder? ExchangeOrder { get; set; }
        public virtual TblFeedBackQuestion? Question { get; set; }
    }
}
