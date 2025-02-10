using RDCELERP.Model.Base;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RDCELERP.Model.Company
{
   public class CompanyViewModel :BaseViewModel
    {
        public int CompanyId { get; set; }
        [Required(ErrorMessage = "Business unit name is required")]
        public string?  BusinessUnitName { get; set; }
        public int? BusinessUnitId { get; set; }
        //[RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Required]
        [StringLength(100)]
        [Display(Name = "Company Name")]       
        public string? CompanyName { get; set; }

        [Required(ErrorMessage = "Phone Number Required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
        public string? Phone { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name ="Email")]       
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Url)]
        [Display(Name ="Website")]
        public string? Website { get; set; }
        
        [Display(Name ="Registration Number")]
        public string? RegistrtionNumber { get; set; }
        [Display(Name = "Upload Logo")]
        public string? CompanyLogoUrl { get; set; }
        public string? CompanyLogoUrlLink { get; set; }
        public IFormFile? CompanyLogo { get; set; }
        public string? CompanyLogoBase64String { get; set; }
    }
    public class CompanyListViewModel
    {
        public List<CompanyViewModel>? CompanyViewModelList { get; set; }
        public int Count { get; set; }
    }
}
