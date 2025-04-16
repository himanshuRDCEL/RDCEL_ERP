using RDCELERP.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.BusinessCustomer
{
   public class ItemViewModel :BaseViewModel
    {
        public int ItemId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Sku { get; set; }
        public decimal? Price { get; set; }
        public string? Brand { get; set; }
        public string? ImageName { get; set; }
        public bool IsSelected { get; set; }
        public int Quantity { get; set; }
    }
}
