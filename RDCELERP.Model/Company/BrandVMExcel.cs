using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Company
{
    public class BrandVMExcel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Brand name is required.")]
        [RegularExpression("^[ A-Za-z0-9 +-]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [DisplayName("Brand Name")]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Company name is required.")]
        public string? Company { get; set; }    
        public string? Remarks { get; set; }
    }
}
