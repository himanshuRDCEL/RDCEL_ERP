using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Enums
{
    public enum FilePathEnum
    {
        [Description("DBFiles\\LGC\\LGCPickup")]
        LGCPickup = 1,
        [Description("DBFiles\\LGC\\LGCDrop")]
        LGCDrop = 2,
        [Description("DBFiles\\EVC\\POD")]
        EVCPoD = 3,
        [Description("DBFiles\\EVC\\DebitNote")]
        EVCDebitNote = 4,
        [Description("DBFiles\\EVC\\Invoice")]
        EVCInvoice = 5,
        [Description("DBFiles\\EVC\\CustomerDeclaration")]
        CustomerDeclaration = 6,
        [Description("DBFiles\\QC\\SelfQC")]
        SelfQC = 7,
        [Description("DBFiles\\QC\\VideoQC")]
        VideoQC = 8,
        [Description("DBFiles\\EVCAttached\\")]
        EVCAttachedFolder = 9,
        [Description("DBFiles\\QC\\DiagnosticReport")]
        DiagnosticReport = 10,
        [Description("DBFiles\\ABB\\InvoiceImage")]
        ABB = 16,
        [Description("DBFiles\\Masters\\ImageLabel")]
        ImageLabelMaster,
        [Description("DBFiles\\Company")]
        Company,
        [Description("DBFiles\\GeneratedQRImages")]
        QRImage,
        [Description("DBFiles\\ABB\\ABBApprovalCertificate")]
        ABBApprovalCertificate,
        [Description("DBFiles\\ExchangeInvoiceImage")]
        ExchangeInvoiceImage,
        [Description("DBFiles\\ABBCustomerInvoice")]
        ABBCustomerInvoice,
        [Description("DBFiles\\EVC\\EVCBulkZip")]
        EVCBulkZip,
    }
}
