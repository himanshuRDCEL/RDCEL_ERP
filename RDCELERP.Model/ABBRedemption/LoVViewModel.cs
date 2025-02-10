using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.ABBRedemption
{
    public class LoVViewModel : BaseViewModel
    {
        public int LoVId { get; set; }
        public string? LoVName { get; set; }
        public int ParentId { get; set; }
        public enum LoVViewModeEnum
        {
            [Description("Redemppercent")]
            Redemppercent = 7,

            [Description("Redempperiod")]
            Redempperiod = 1,
        }

        public enum ExchangeABBEnum
        {
            [Description("ABB")]
            ABB = 16,

            [Description("Exchange")]
            Exchange = 17,
        }
    }
   
}
