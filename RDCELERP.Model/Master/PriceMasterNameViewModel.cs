using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.Master
{
    public class PriceMasterNameViewModel : BaseViewModel
    {
        public int PriceMasterNameId { get; set; }  
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(maximumLength: 50)]
        [RegularExpression("^[ A-Za-z _ ]*$", ErrorMessage = "Only Alphabets allowed.")]
        public string? Name { get; set; }
        
        [StringLength(maximumLength: 100)]
        [RegularExpression("^[a-zA-Z _]*$", ErrorMessage = "Only Alphabets allowed.")]
        [Required(ErrorMessage = "Description is required.")]
        public string? Description { get; set; }
        public string? Action { get; set; }
        public string? Date { get; set; }
    }

    
}
