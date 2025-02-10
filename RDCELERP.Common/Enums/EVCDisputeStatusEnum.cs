using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Wordprocessing;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Xml.Linq;
using RDCELERP.DAL.Entities;

namespace RDCELERP.Common.Enums
{
    public enum EVCDisputeStatusEnum
    {
        [Description("Open Status")]
        Open = 1,
        [Description("Work In Progress")]
        WorkInProgress = 2,
        [Description("Hold")]
        Hold = 3,
        [Description("Close")]
        Close = 4
    }

    public enum EVCApprovalStatusId
    {
        [Description("EVCApprovalStatusId")]
        EVCApprovalStatusId = 1,
    }
    public enum EVCWalletstatus
    {
        [Description("Hold")]
        Hold = 1,
        [Description("Debit")]
        Debit = 2,
    }

}
