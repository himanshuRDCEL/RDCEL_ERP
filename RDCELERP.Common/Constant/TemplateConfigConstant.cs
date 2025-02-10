using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Constant
{
    public class TemplateConfigConstant
    {
        #region ABB Welcome Email
        public const string ABBWelcomeMailTempName = "ABBWelcomeEmail";
        public const string ABBWelcomeMailSubject = "Customer Details";
        #endregion

        #region ABB Certificate
        public const string ABBCertificateName = "ABBApprovalCertificate";
        #endregion

        #region ABB Customer Invoice
        public const string ABBInvoiceName = "ABB_Invoice";
        #endregion

        #region Pending Order Reporting Mail Template
        public const string PendingOrderMailTempName = "PendingOrdersReportingEmail";
        public const string PendingOrderMailSubject = "Reporting mail for pending orders";
        #endregion
    }
}
