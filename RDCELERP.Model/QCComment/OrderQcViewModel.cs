using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.QCComment
{
    public class OrderQcViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string? RegdNo { get; set; }
        public string? CustomerName { get; set; }
        public string? ProductType { get; set; }
        public int StatusCode { get; set; }
        public string? QCDate { get; set; }
        public string? CustomerPhoneNumber { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerCity { get; set; }
        public string? CustomerState { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
