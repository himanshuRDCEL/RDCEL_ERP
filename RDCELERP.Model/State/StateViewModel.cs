using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.State
{
    public class StateViewModel: BaseViewModel
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
        public int error { get; set; }
        public string? Date { get; set; }

        public StateVMExcel? StateVM { get; set; }
        public IFormFile? UploadState { get; set; }
        public List<StateVMExcel>? StateVMList { get; set; }
        public List<StateVMExcel>? StateVMErrorList { get; set; }

    }

    public class StateName
    {
        public int StateId { get; set; }
        public string? Name { get; set; }
    }

    public class StateIdList
    {
        public int id { get; set; }
    }

    public class StateList
    {
        public List<StateIdList>? stateIdLists { get; set; }
    }

   
}
