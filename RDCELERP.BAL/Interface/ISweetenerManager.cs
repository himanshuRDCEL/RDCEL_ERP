using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Sweetener;

namespace RDCELERP.BAL.Interface
{
    public interface ISweetenerManager
    {
        public SweetenerDataViewModel GetSweetenerAmtExchange(GetSweetenerDetailsDataContract details);
        public SweetenerDataViewModel GetModalBasedSweetener(GetSweetenerDetailsDataContract details);
        public SweetenerDataViewModel GetBasicSweetener(GetSweetenerDetailsDataContract details);
    }
}
