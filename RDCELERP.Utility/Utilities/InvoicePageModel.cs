using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface; 
using RDCELERP.BAL.MasterManager;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;

namespace RDCELERP.Utility.Utilities
{
    public class InvoiceController
    {
        #region Variable declartion
        private readonly LogisticManager _logisticManager;

        public InvoiceController(LogisticManager logisticManager)
        {
            _logisticManager = logisticManager;
        }
        #endregion

        public bool GenerateInvoiceForEVC()
        {
            List<TblOrderLgc> orderLgcList = null;
            
            bool flag = false;
            orderLgcList = _logisticManager.GenerateInvoiceForEVC(null,null);
            if (orderLgcList != null && orderLgcList.Count > 0)
            {
                flag = true;
            }
            return flag;
        }
    }
}
