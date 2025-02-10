using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.UserMapping
{
    public class UserMappingViewModel : BaseViewModel
    {
        public int UserMappingId { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        public string UserName { get; set; }
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Business partner is required.")]
        public string BusinessPartnerName { get; set; }
        public int? BusinessPartnerId { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        public string BusinessUnitName { get; set; }
        public int? BusinessUnitId { get; set; }
    }
}
