using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static NPOI.HSSF.Util.HSSFColor;
using static System.Net.Mime.MediaTypeNames;

namespace RDCELERP.Common.Constant
{
    public class NotificationConstants
    {
        //Demo
        public const string SMS_VoucherGeneration_OTP = "Dear Customer - OTP for voucher generation for the [STORENAME] is [OTP] by UTC Digital Technologies.";
        public const string SMS_VoucherRedemption_Confirmation = "Dear Customer - Congratulations!!! Your order has been validated and the Voucher code worth Rs. [ExchPrice]/- for [STORENAME] is [VCODE], Please share this with your dealer at the time of purchase of a [COMPANY] product. This code is valid for [VALIDTILLDATE].you can also download the same from [VLink]. From UTC Digital Technologies.";

        //Live
        public const string SMS_LGCPickup_OTP = "Dear Customer - Use [OTP] as your OTP for your product pickup by Digi2L. Thank You, Team DIGI2L.";
        public const string SMS_Drop_OTP = "Dear Retail Trade Partner - Use [OTP] as your product drop code on Digi2l. Thank You, Team DIGI2L.";
        public const string SMS_EVCRegistration_OTP = "Dear Reseller - [OTP] is your OTP for EVC registration. For any help please contact [Contact] by Team DIGI2L.";
        public const string SMS_Login_OTP = "Dear customer - Your OTP for registering as a Digi2L service partner driver is [OTP]. \r\nBest regards, Team DIGI2L(The United Trading Company).";


        //Whatsapp
        public const string Logi_Pickup = "lgc_pickup";
        public const string Logi_Drop = "lgc_drop";
        public const string SelfQC_Link = "self_qc";
        public const string orderConfirmationForExchangeUpdated = "self_qc_generic_template_new";

        // public const string SelfQC_Link = "self_qc_28_jan";
        public const string EVC_AssignOrder = "evc_allocation_28_jun";

        public const string RescheduleDate_Time = "qc_date_reschadule";
        public const string ReissueCashVoucher = "for_revised_voucher";

        //public const string WaitingForPrice_Approval = "cust_upi_no";        
        //public const string WaitingForPrice_Approval_deferred_settlement = "deferred_settlement_at_the_time_qc_with_variable";
        //public const string WaitingForPrice_Approval = "upi_nd_pickup_details_28_jun";
        public const string WaitingForPrice_Approval_deferred_settlement = "pickup_details_confirmation_after_qc";
        public const string WaitingForPrice_Approval_instant_settlement = "instant_settlement_at_the_qc_time_with_variable";
        
        
        public const string send_link_for_personal_details = "sms_pinelabs_customerdetails";  //added for smartsell api

        //For Questioner
        public const string SMS_ExchangeOtp = "Dear Customer - OTP for [BrandName] Exchange Program registration is [OTP] by Team DIGI2L.";
        public const string DiagnoticReport = "diagnostic_tool_report";

        //UPI Verification Link
        public const string UpiId_Verfication_SMS = "Hi (customer name)! Your product's quality check is complete. Final price: [Price]. Click here to schedule pickup: [link]. Provide your UPI No. for payment. Reach out if you have any questions. Team DIGI2L";
        public const string PickUp_Details_SMS = "Hi (customer name)! Your product's quality check is complete. Click here to schedule pickup: [link]. Reach out if you have any questions. Team DIGI2L";
        public const string UpiId_Verfication_Email = @"
                    <p>Hi <b>[Customername]</b>,</p>
                    <p>Hope you're doing well! We've completed the quality check of your product and have determined the final price: ₹[Priceafterqc]. To proceed, please click on the following link to schedule a convenient pickup date and time:</b>[link]</p>
                    <p>Kindly provide your UPI Id. for payment transfer.</p>
                    <p>If you have any questions or concerns about the price or pickup process, feel free to reach out. We're here to assist you.</p>
                    <p>Thank you for choosing to work with us.</p>
                    <p>
                        Best regards,<br>
                        Team DIGI2L
                    </p>";
        public const string PickUp_Details_Email = @"
                    <p>Hi <b>[Customername]</b>,</p>
                    <p>Hope you're doing well! We've completed the quality check of your product.Please click on the following link to schedule a convenient pickup date and time: </b>[link]</p>
                    <p>If you have any questions or concerns about the price or pickup process, feel free to reach out. We're here to assist you.</p>
                    <p>Thank you for choosing to work with us.</p>
                    <p>
                        Best regards,<br>
                        Team DIGI2L
                    </p>";

        //QC Reschedule 
        public const string QC_Reschedule_SMS = "Hi [Customer's Name]! Your pick-up is rescheduled to [date/time]. Need help? Contact us at +91 9619697745. Thank you for choosing Digi2L! - Team Digi2L";
        public const string QC_Reschedule_Email = @"
                     <p>Dear <b>[Customer's Name]</b>,</p>
                     <p>We wanted to inform you that your pick - up has been successfully rescheduled for [date and time] as per your request.If you have any questions or need further assistance, please don't hesitate to contact us on +91 9619697745</p>
                     <p>Thank you for choosing our service!</p>
                     <p>
                         Best regards,<br>
                         Team Digi2L
                     </p>";

        //Self QC
        public const string Self_QC_SMS = "Hi (customer name)Action Required: Complete Self-QC for your product. Click [Link], upload images as instructed, and submit form. You'll receive a price quote in 6 hours. Contact us if needed. Team DIGI2L";
        public const string Self_QC_Email = @"
                       <p>Dear <b>[Customer Name]</b>,
                       <p>Please complete the Self-QC(Quality Check) process for your product by clicking the link below: [Link]. Upload the requested images as instructed and submit the form.You'll receive a price quote within 6 hours.</b></p>
                       <p>If you have any questions or concerns, feel free to reach out. We're here to help!</p>
                       <p>
                        Thank you,<br>
                        Team DIGI2L
                       </p>";
        public const string PostSelfQCAlert = "post_selfqc_customer_alert";

        //public const string PostSelfQCAlert = "post_selfqc_customer_alert";

        //EVC Auto-Allocation
        public const string EvcAutoAllocation = "evc_autoallocation_181023";

        //Exchange order confirmation whatsapp 
        public const string orderConfirmationForExchange = "order_confirmation_updated_for_exchange";
        public const string SMS_VOUCHER_GENERATIONCash = "Dear Customer - Congratulations!!! Your order has been validated and the Voucher code worth Rs. [ExchPrice]/- for [STORENAME] is [VCODE], Please share this with Digi2L Partner at the time of pick up of the old appliance. The value of this voucher is subject to QC. You can also download the same from [VLink].*T&C applied From UTC Digital Technologies.";
        public const string Send_Voucher_Code_Template = "instant_voucher_";


        //voucher capture template
        public const string voucher_capture_working = "voucher_capture";
        public const string Test_Template = "customer_registration";

        //SMS OTP Smart-Buy 

        public const string SmartBuy_otp_30012024 = "Dear Customer, The OTP for your SmartBuy registration is [OTP], Team Digi2L.";

    }
}
