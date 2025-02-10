using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;
using RDCELERP.Model.State;

namespace RDCELERP.Model.City
{
    public class CityViewModel : BaseViewModel
    {
        public int CityId { get; set; }
        [Required(ErrorMessage = "City name is required.")]
        [RegularExpression(@"^[A-Za-z][a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        [StringLength(50)]
        [DisplayName("City Name")]
        
        public string? Name { get; set; }
        [Required(ErrorMessage = "City code is required.")]
        [RegularExpression(@"^(?!-)\w*$", ErrorMessage = "City code should be a positive integer.")]
        [StringLength(5)]
        [DisplayName("City Code")]
        public string? CityCode { get; set; }
        [Required(ErrorMessage = "State name is required.")]
        [DisplayName ("State Name")]
        public string? StateName { get; set; }
        public int? StateId { get; set; }
        public bool? IsMetro { get; set; }
        
        public string? CityLogoUrl { get; set; }
        public string? CityLogo { get; set; }
        public string? CityLogoLink { get; set; }

        public string? Date { get; set; }

        public CityVMExcel? CityVM { get; set; }
        public IFormFile? UploadCity { get; set; }
        public List<CityVMExcel>? CityVMList { get; set; }
        public List<CityVMExcel>? CityVMErrorList { get; set; }
    }

    public class CityList
    {
        public int CityId { get; set; }
        public string? Name { get; set; }
        public int StateId { get; set; }
    }
}
