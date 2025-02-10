using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Enums
{
    public enum BussinessUnitEnum
    {
        //ADD company name in description for bussiness unit =4 (D2C)
        [Description("UTC Digital")]
        D2C = 4,
        [Description("Demo")]
        Demo = 14,

        [Description("TechGaurd")]
        Tech_Guard = 21,

        [Description("Others")]
        Others = 2008,

    }
}
