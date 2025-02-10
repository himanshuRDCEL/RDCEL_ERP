using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.City
{
    public class CityVMExcel
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
        [DisplayName("State Name")]
        public string? StateName { get; set; }
        public string? Remarks { get; set; }
        public bool IsMetro { get; set; }
        public bool IsActive { get; set; }


    }
}
