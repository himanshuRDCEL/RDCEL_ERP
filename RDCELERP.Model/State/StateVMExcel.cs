using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.State
{
    public class StateVMExcel
    {
        public int StateId { get; set; }
        [Required(ErrorMessage = "State name is required.")]
        [RegularExpression(@"^[A-Za-z][a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        [StringLength(50)]
        public string? Name { get; set; }
        [RegularExpression(@"^(?!-)\w*$", ErrorMessage = "State code should be a positive integer.")]
        [Required(ErrorMessage = "State code is required.")]
        [StringLength(5)]
        public string? Code { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; }



    }
}
