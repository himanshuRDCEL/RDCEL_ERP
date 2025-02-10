using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.Master
{
    public class PriceMasterMappingViewModel : BaseViewModel
    {
        public int PriceMasterMappingId { get; set; }
        [Required(ErrorMessage = "Company is required.")]
        public string? BusinessUnitName { get; set; }
        public int? BusinessUnitId { get; set; }

        
        public string? BusinessPartnerName { get; set; }
        public int? BusinessPartnerId { get; set; }

        [Required(ErrorMessage = "Brand is required.")]
        public string? BrandName { get; set; }
        public int? BrandId { get; set; }
       
        [Required(ErrorMessage = "Price master name is required.")]
        public string? PriceMasterName { get; set; }
        public int? PriceMasterNameId { get; set; }

        public string? Action { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        //public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "Start date is required.")]
        public string? StartDate { get; set; }
        //public DateTime? EndDate { get; set; }
        [Required(ErrorMessage = "End date is required.")]
        public string? EndDate { get; set; }
        public string? Date { get; set; }
    }
}
