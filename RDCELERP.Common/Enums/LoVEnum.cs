using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Enums
{
    public enum LoVEnum
    {
        [Description("RedempPeriod")]
        RedempPeriod = 1,
        [Description("6-12 Months")]
        SixToTwelveMonths = 2,
        [Description("13-24 Months")]
        ThirteenToTwentyFourMonths = 3,
        [Description("25-36 Months")]
        TwentyFiveToThirtySixMonths = 4,
        [Description("37-48 Months")]
        ThirtySevenToFortyEightMonths = 5,
        [Description("49-60 Months")]
        FortyNineToSixtyMonths = 6,
        [Description("RedempPercentage")]
        RedempPercentage = 7,
        [Description("10")]
        Ten = 8,
        [Description("20")]
        Twenty = 9,
        [Description("30")]
        Thirty = 10,
        [Description("40")]
        Forty = 11,
        [Description("50")]
        Fifty = 12,
        [Description("60")]
        Sixty = 13,
        [Description("60 Months")]
        SixtyMonths = 14,
        [Description("OrderType")]
        OrderType = 15,
        [Description("ABB")]
        ABB = 16,
        [Description("Exchange")]
        Exchange = 17,
        [Description("Marcom")]
        Marcom = 18,
        [Description("YES")]
        YES = 19,
        [Description("NO")]
        NO = 20,
        [Description("ImageUploadby")]
        ImageUploadby = 21,
        [Description("Customer")]
        Customer = 22,
        [Description("QC Team")]
        QCTeam = 23,
        [Description("Logistics")]
        Logistics = 24,
        [Description("Pickup")]
        Pickup = 25,
        [Description("Drop")]
        Drop = 26,
        [Description("EVC_Dispute")]
        EVC_Dispute = 27,
        [Description("EVCWalletRechagedBy")]
        EVCWalletRechagedBy = 28,
        [Description("EVCAdmin")]
        EVCAdmin = 29,
        [Description("EVCPortal")]
        EVCPortal = 30,


        // Created by kranti for Business Unit master
        [Description("GSTType")]
        GSTType = 35,
        [Description("GSTInclusive")]
        GSTInclusive = 36,
        [Description("GSTExclusive")]
        GSTExclusive = 37,
        [Description("GSTNotApplicable")]
        GSTNotApplicable = 38,
    }
}
