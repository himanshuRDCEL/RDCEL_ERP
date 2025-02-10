using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.ImagLabel;

namespace RDCELERP.Model.ImageLabel
{
    public class SelfQcVideoImageViewModel
    {
        public string? SelfQCVideo { get; set; }
        public int? LoginId { get; set; }
        public List<ImageLabelViewModel>? imageLabelViewModels { get; set; }
    }
}
