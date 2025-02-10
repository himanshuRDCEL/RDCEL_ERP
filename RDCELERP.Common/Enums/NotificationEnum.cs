using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Enums
{
    public enum NotificationEnum
    {
        [Description("Order Assigned by Service Partner")]
        OrderAssignedbyServicePartner = 1,
        [Description("Order Accepted by Driver")]
        OrderAcceptedbyDriver = 2,
        [Description("Order Rejected by Driver")]
        OrderRejectedbyDriver = 3,
        [Description("Journey Started by Driver")]
        JourneyStartedbyDriver = 4,
        [Description("Order Pickuped by Driver")]
        OrderPickupedbyDriver = 5,
        [Description("Order Droped by Driver")]
        OrderDropedbyDriver = 6,
        [Description("Order Assigned by Digi2L")]
        OrderAssignedbyDigi2L = 7,
        [Description("EVC Changed by Admin")]
        EVCChangedbyAdmin = 8,
        [Description("Pickup Declined by Customer")]
        PickUpDeclinedByCustomer = 9,
        [Description("Pickup Rescheduled by Customer")]
        PickUpRescheduledByCustomer = 10,
        [Description("Order Cancellation Alert")]
        OrderCancellationAlert = 11,
        [Description("Journey Start Reminder")]
        JourneyStartReminder = 12,
        [Description("Pending Deliveries Alert")]
        PendingDeliveriesAlert = 13,
        [Description("New Journey Assigned")]
        NewJourneyAssigned = 14,
        [Description("Assignment is cancelled")]
        Assignmentiscancelled = 15
    }
}
