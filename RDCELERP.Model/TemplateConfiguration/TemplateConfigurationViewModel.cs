using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.MessageConfiguration
{
    public class TemplateConfigurationViewModel: BaseViewModel
    {
        public int Id { get; set; }        
        public string? Name { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(150)]
        public string? TemplateType { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public IList<TemplateConfigurationViewModel>? TemplateConfigurationViewModelList { get; set; }
    }
}
