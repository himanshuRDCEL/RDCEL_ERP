using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.Company
{
    public class BrandViewModel : BaseViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Brand name is required.")]
        [RegularExpression("^[ A-Za-z0-9 +-]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [DisplayName("Brand Name")]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Business unit name is required.")]
        public string? BusinessUnitName { get; set; }
        public int? BusinessUnitId { get; set; }
        

        public string? BrandLogoUrl { get; set; }
        public string? BrandLogoUrlLink { get; set; }
        public string? ImageName { get; set; }
        public string? ImageURL { get; set; }
        public string? Date { get; set; }
        public BrandVMExcel? BrandVM { get; set; }
        public IFormFile? UploadBrand { get; set; }
        public List<BrandVMExcel>? BrandVMList { get; set; }
        public List<BrandVMExcel>? BrandVMErrorList { get; set; }


    }

    public class BrandViewModels 
    {
        public class Brand
        {
            public List<Brand>? Brands{ get; set; }
        }
        public int Id { get; set; }
        [Required]
        [DisplayName("Brand Name")]
        public string? Name { get; set; }
        

    }

    


}
