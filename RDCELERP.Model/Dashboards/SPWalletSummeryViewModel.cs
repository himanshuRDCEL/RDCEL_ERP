using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Dashboards
{
    public class SPWalletSummeryViewModel
    {
        public decimal TotalEarning { get; set; }//Pickup done 
        public decimal PrevoiusEarning { get; set; }
        public decimal TillDateEarning { get; set; }//Drop Done
    }
}
