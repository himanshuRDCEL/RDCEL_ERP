using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Enums
{
    public enum ConfigurationEnum
    {
        [Description("Email")]
        Email = 1,
        [Description("Password")]
        Password = 2,
        [Description("BaseUrl")]
        BaseUrl = 3,
        [Description("Total Agents")]
        TotalAgents = 4,
        [Description("Call Per Hour")]
        CallPerHour = 5,
        [Description("Use EVC Price Mater")]
        UseEVCPriceMater = 1,
        [Description("QC Bonus Cap")]
        QCBonusCap = 9,
        [Description("Excellent Calculation")]
        ExcellentCalculation = 45,
        [Description("BrandId")]
        BrandId = 2008,
        [Description("Brand Specific ASP")]
        BrandSpecificASP = 10,
        [Description("Upper Bonus Cap")]
        UpperBonusCap = 11,
        [Description("FinancialYear")]
        FinancialYear,
        [Description("StartInvoiceSrNum")]
        StartInvoiceSrNum,
        [Description("WaitingForPrice_Approval")]
        WaitingForPrice_Approval,
        [Description("TblReloadTimeMs")]
        TblReloadTimeMs,
        //start use for Evc Assign check
        [Description("EVCAssignbyState")]
        EVCAssignbyState,
        [Description("EVCAssignbypartner")]
        EVCAssignbypartner,
        [Description("EVCAssignbypartnerandWallet")]
        EVCAssignbypartnerandWallet,
        [Description("EVCAssignbyPartnerandWalletandlastTran")]
        EVCAssignbyPartnerandWalletandlastTran,
        [Description("ABB_Bcc")]
        ABB_Bcc,
        [Description("OrderPendingTimeH")]
        OrderPendingTimeH,
        [Description("VideoRecordingTimerSec")]
        VideoRecordingTimerSec,
        [Description("MaxVideoFileSizeMB")]
        MaxVideoFileSizeMB,
        //end  
    }
}
