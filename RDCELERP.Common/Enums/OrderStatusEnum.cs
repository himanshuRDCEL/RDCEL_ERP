using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Enums
{
    public enum OrderStatusEnum
    {
        //Status of Orders for QC
        [Description("Order created by Sponsor")]
        OrderCreatedbySponsor = 5,

        //[Description("QC In Progress")]
        [Description("Qc verification in progress")]
        QCInProgress_3Q = 13,

        [Description("Call & Go scheduled: Appointment taken")]
        CallAndGoScheduledAppointmentTaken_3P = 10,

        //[Description("Appointment rescheduled")]
        [Description("QC Appointment rescheduled by Customer")]
        QCAppointmentrescheduled = 11,

        [Description("Reopen Order")]
        ReopenOrder = 43,

        [Description(" New product delivered/ Installed by Sponsor")]
        InstalledbySponsor = 17,
        
        //[Description("Exchange order cancelled after creation (Sponsor)"]
        [Description("Order Cancelled by Sponsor")]
        CancelOrder = 6,

        [Description("QC appointment declined by customer: Not Interested")]
        QCAppointmentDeclined = 12,

        //[Description("Customer not Responding")]
        [Description("After 4 Attempt still customer not responding")]
        CustomerNotResponding_3C = 57,

        //[Description("QC fail (product substantially different)")]
        [Description("Product mismatch compared to Declaration")]
        QCFail_3Y = 14,

        //[Description("QC Amount Confirmation Declined by customer after QC")]
        [Description("Amount Rejected by customer after QC Verification")]
        QCOrderCancel = 16,
        //Status of Cancel Order QC

        //[Description("Amount Approved by customer after QC")]
        [Description("Amount Accepted by customer after QC Verification")]
        AmountApprovedbyCustomerAfterQC = 15,

        //[Description("Self QC by Customer")]
        [Description("Self QC Completed by Customer")]
        SelfQCbyCustomer = 33,

        [Description("Pickup completed")]
        LGCPickup = 23,
        [Description("Product Delivered at EVC")]
        LGCDrop = 30,
        [Description("Posted")]
        Posted = 32,
        
        //[Description("QC Appointment Rescheduled > 3 times")]
        [Description("Customer perpetually rescheduling qc appointment ( 4 Attempts )")]
        QCAppointmentRescheduled_3RA = 58,

        [Description(" QC ByPass")]
        QCByPass = 42,

        //[Description("EVC Allocation completed")]
        [Description("Process for pickup")]
        EVCAllocationcompleted = 34,

        //[Description("Reopen for Logistics")]
        [Description("Customer requested to reopen ticket for pickup")]
        ReopenforLogistics = 44,

        //[Description("Reopen for QC After Decline")]
        [Description("Order reopened by customer on Digi2L's offered price")]
        ReopenforQC= 41,
        
        //[Description("Waiting for customer Approval")]
        [Description("Waiting for customer Acceptance on Dig2L offered Price")]
        Waitingforcustapproval = 48,

        [Description("Customer has asked for Pickup Reschedule")]
        PickupReschedule = 22,
        [Description("Pending For Upper Cap By QC Admin")]
        UpperBonusCapPending = 53,


        [Description("Pickup Ticket cancellation by UTC")]
        PickupTicketcancellationbyUTC = 21,

        //[Description("Pickup Appointment declined by customer to call center")]
        [Description("Pickup declined by customer post confirmation")]
        PickupDecline = 26,

        [Description("Call Assigned for Pickup")]
        CallAssignedforPickup = 7,

        [Description("Order Accepted by Vehicle")]
        OrderAcceptedbyVehicle = 54,
        [Description("Order Rejected by Vehicle")]
        OrderRejectedbyVehicle = 55,
        [Description("Vehicle Assign by Service Partner")]
        VehicleAssignbyServicePartner = 56,

        [Description("LogisticsTicketRaised")]
        LogisticsTicketUpdated = 18,

        [Description("ABB plan Redemped")]
        ABBplanRedemped = 40,

        [Description("Payment Successful")]
        PaymentSuccessful = 47,

        [Description("Payment not initiated")]
        PaymentNotInitiated = 45,

        [Description("Payment Failed")]
        PaymentFailed = 46,

        [Description("Order With Diagnostic")]
        OrderWithDiagnostic = 59,
    }
    public enum OrderTypeEnum
    {

        [Description("ABB")]
        ABB = 16,

        [Description("Exchange")]
        Exchange = 17,
    }
    public enum ServicePartnerEnum
    {
        [Description("Bizlog")]
        Bizlog = 1,

        [Description("Mahindra")]
        Mahindra = 2,
    }
}
