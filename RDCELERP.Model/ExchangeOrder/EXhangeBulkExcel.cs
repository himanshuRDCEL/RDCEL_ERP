using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.ExchangeOrder
{
    public class ExchangeBulkExcel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product type is required.")]
        public string? ProductType { get; set; }

        [Required(ErrorMessage = "Brand is required.")]
        public string? Brand { get; set; }
        [Required(ErrorMessage = "Purchased product category is required.")]
        public string? ProductCategory { get; set; }
        [Required(ErrorMessage = "Product condition is required.")]
        public string? ProductCondition { get; set; }
        public string? SponsorOrderNumber { get; set; }
        public string? Remarks { get; set; }
    }
}
