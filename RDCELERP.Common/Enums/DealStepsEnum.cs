using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Enums
{
    public enum DealStepsEnum
    {
        [Description("Welcome call")]
        Welcomecall = 1,
        [Description("Draft solar vic quote")]
        Draftsolarvicquote = 2,
        [Description("Finalize sale")]
        Finalizesale = 3,
        [Description("Send mail to Customer")]
        SendmailtoCustomer = 4,
        [Description("Process Call")]
        ProcessCall = 5,
        [Description("Follow Up")]
        FollowUp = 6,
        [Description("Sales Completed")]
        SalesCompleted = 7,
        [Description("Check Installer")]
        CheckInstaller = 8,
        [Description("Plan Installation")]
        PlanInstallation = 9,
        [Description("Call Customer")]
        CallCustomer = 10,
        [Description("Upload Info")]
        UploadInfo = 11,
        [Description("Email Installer")]
        EmailInstaller = 12,
        [Description("Make Invoice")]
        MakeInvoice = 13,
        [Description("Place the Order")]
        PlacetheOrder = 14,
        [Description("Email Transporter")]
        EmailTransporter = 15,
        [Description("Send Invoice")]
        SendInvoice = 16,
        [Description("Collect the Money")]
        CollecttheMoney = 17,
        [Description("On installation day")]
        Oninstallationday = 18,
        [Description("Call cust for process")]
        Callcustforprocess = 19,
        [Description("Collect Paperwork")]
        CollectPaperwork = 20,
        [Description("Check EWR, EG and CES")]
        CheckEWREGandCES = 21,
        [Description("Check Details")]
        CheckDetails = 22,
        [Description("Upload STC")]
        UploadSTC = 23,
        [Description("Payment claim")]
        Paymentclaim = 24,
        [Description("Upload cust folder")]
        Uploadcustfolder = 25,
        [Description("Call Customer")]
        CallCustomer26 = 26,
        [Description("Raise the connection")]
        Raisetheconnection = 27,
        [Description("Send Paperwork")]
        SendPaperwork = 28,
        [Description("Thank you call")]
        SendThankyoucall = 29,
        [Description("Curtesy call")]
        Curtesycall = 30,
        [Description("3 Month Call")]
        ThreeMonthCall = 31,
        [Description("6 Month Call")]
        SixMonthCall = 32,
        [Description("1 Year Call")]
        OneYearCall = 33,
        [Description("Final Step")]
        FinalStep = 34

    }
}
