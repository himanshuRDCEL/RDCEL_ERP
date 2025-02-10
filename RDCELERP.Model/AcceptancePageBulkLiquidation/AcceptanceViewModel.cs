using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.AcceptancePageBulkLiquidation
{
    public class AcceptanceViewModel : BaseViewModel
    {

        public string? RegdNo { get; set; }
        public string? UPIId { get; set; }
        
        public DateTime? PreferredPickupDate { get; set; }
       
        public string? PreferredPickupTime { get; set; }
        public string? PriceGrade { get; set; }
    }
}
