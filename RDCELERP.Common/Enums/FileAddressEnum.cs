using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Enums
{
    public enum FileAddressEnum
    {
        [Description("DBFiles/LGC/LGCPickup/")]
        LGCPickup = 1,
        [Description("DBFiles/LGC/LGCDrop/")]
        LGCDrop = 2,
        [Description("DBFiles/EVC/POD/")]
        EVCPoD = 3,
        [Description("DBFiles/EVC/DebitNote/")]
        EVCDebitNote = 4,
        [Description("DBFiles/EVC/Invoice/")]
        EVCInvoice = 5,
        [Description("DBFiles/EVC/CustomerDeclaration/")]
        CustomerDeclaration = 6,
        [Description("DBFiles/QC/SelfQC/")]
        SelfQC = 7,
        [Description("DBFiles/QC/VideoQC/")]
        VideoQC = 8,
        [Description("DBFiles/ABB/InvoiceImage/")]
        ABBInvoice = 9,
        [Description("DBFiles/QC/DiagnosticReport/")]
        DiagnosticPdf = 10,
        [Description("DBFiles/Masters/ImageLabel/")]
        ImageLabelMaster = 11,
        [Description("DBFiles/Company/")]
        Company,
        [Description("DBFiles/GeneratedQRImages/")]
        QRImage,
        [Description("PdfTemplates/img/UtcSeel_INV.png")]
        UTCACSeel,
        [Description("PdfTemplates/certificate/img/bg.png")]
        BgImg,
        [Description("PdfTemplates/certificate/img/logo.svg")]
        logo,
        [Description("PdfTemplates/certificate/img/digi2l-logo.svg")]
        digi2l_logo,
        [Description("DBFiles/EVC/EVCBulkZip/")]
        EVCBulkZip,
    }
}
