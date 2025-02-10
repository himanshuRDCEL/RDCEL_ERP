using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.QCComment
{
     public  class ExchangeOrderStatusViewModel : BaseViewModel
    {
        [Key]
        public int? Id { get; set; }
        public string? StatusCode { get; set; }
        public string? StatusDescription { get; set; }
        public string? StatusName { get; set; }

        public string? CombinedDisplay
        {
            get { return StatusCode + " - " + StatusDescription; }
        }

    }
    public enum ExchangeOrderStatusEnum
    {
        [Description("ABB Approve")]
        ABBApproved = 36,

        [Description("ABB Not Approve")]
        ABBNotApproved = 37,

        [Description("ABB Reject")]
        ABBReject = 38,

        [Description("ABB Reject Undo")]
        ABBRejectUndo = 39,

        [Description("ABB plan Redemped")]
        ABBplanRedemped = 40,
    }
}
