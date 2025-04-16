using RDCELERP.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Category
{
    public class CategoryViewModel :BaseViewModel
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public int? BrandId { get; set; }
        public int? CompanyId { get; set; }
    }
}
