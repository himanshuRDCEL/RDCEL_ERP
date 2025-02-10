using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Product
{
    public class ProductTypeVMExcel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product Type Name is required.")]
        [RegularExpression("^[ A-Za-z0-9 +-]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [DisplayName("Product Type Name")]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [RegularExpression("^[ A-Za-z0-9 _@.*/#&+-]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [StringLength(150)]
        public string? Description { get; set; }
        [StringLength(150)]
        [Required(ErrorMessage = "Description For ABB is required")]
        [RegularExpression("^[ A-Za-z0-9 _@.*/#&+-]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string? DescriptionForAbb { get; set; }
        [Required(ErrorMessage = "Code is required")]
        [RegularExpression("^[ A-Za-z0-9 _@.*/#&+-]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [StringLength(30)]
        public string? Code { get; set; }
       
        [RegularExpression("^[ A-Za-z0-9 _@.*/#&+-]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [StringLength(150)]
        public string? Size { get; set; }

        public bool IsAllowedForOld { get; set; }
        public bool IsAllowedForNew { get; set; }
        public bool IsActive { get; set; }
        public string? ProductCategoryName { get; set; }
        public string? Remarks { get; set; }
    }
}
