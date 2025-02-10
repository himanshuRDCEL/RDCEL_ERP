using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.TicketGenrateModel
{
    public class SponserListDataContract
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public List<SponserData>? data { get; set; }
    }

    public class SponserData
    {
        [DataMember]
        public string? ABB { get; set; }
        [DataMember]
        public string? Order_Status { get; set; }
        [DataMember]
        public string? Customer_State_Name { get; set; }
        [DataMember]
        public string? NewProductTechnologySubProductTechnology { get; set; }
        [DataMember]
        public CustomerName? Customer_Name { get; set; }
        [DataMember]
        public string? age_years { get; set; }
        [DataMember]
        public string? ABB_Fees { get; set; }
        [DataMember]
        public string? New_Product_Brand_Name1 { get; set; }
        [DataMember]
        public string? Prexo_Working_Condition { get; set; }
        [DataMember]
        public NewProdGroup? New_Prod_Group { get; set; }
        [DataMember]
        public string? Customer_Address_Landmark { get; set; }
        [DataMember]
        public string? ID { get; set; }
        [DataMember]
        public string? Customer_Mobile { get; set; }
        [DataMember]
        public string? Customer_Pincode { get; set; }
        [DataMember]
        public string? New_product_Invoice_Number { get; set; }
        [DataMember]
        public string? New_Product_Size { get; set; }
        [DataMember]
        public string? Sweetener_Bonus_Amount_By_Sponsor { get; set; }
        [DataMember]
        public string? NewProdGroupProductTechnology { get; set; }
        [DataMember]
        public string? Regd_No { get; set; }
        [DataMember]
        public string? Prexo_Brand { get; set; }
        [DataMember]
        public string? New_Product_Delivered_Date { get; set; }
        [DataMember]
        public string? New_Product_Delivery_Status { get; set; }
        [DataMember]
        public string? Exchange_Status { get; set; }
        [DataMember]
        public NewProductTechnology? New_Product_Technology { get; set; }
        [DataMember]
        public string? Sr_No { get; set; }
        [DataMember]
        public string? ABB_Status { get; set; }
        [DataMember]
        public string? New_product_invoice_Date { get; set; }
        [DataMember]
        public string? Exchange_Price_Displayed_Max { get; set; }
        [DataMember]
        public string? Customer_City { get; set; }
        [DataMember]
        public string? Exchange { get; set; }
        [DataMember]
        public string? New_Product_Model_Number { get; set; }
        [DataMember]
        public string? New_Product_Installation_Date { get; set; }
        [DataMember]
        public string? New_product_Net_price { get; set; }
        [DataMember]
        public NewProdGroup1? New_Prod_Group1 { get; set; }
        [DataMember]
        public string? Customer_Address_1 { get; set; }
        [DataMember]
        public string? Customer_Email_Address { get; set; }
        [DataMember]
        public string? Prexo_Size_Capacity { get; set; }
        [DataMember]
        public string? Customer_Address_2 { get; set; }
        [DataMember]
        public NewProductTechnology1? New_Product_Technology1 { get; set; }
        [DataMember]
        public string? Sp_Order_No { get; set; }
        [DataMember]
        public SponsorName? Sponsor_Name { get; set; }
        [DataMember]
        public string? QC { get; set; }
        [DataMember]
        public string? Is_Under_Warranty { get; set; }
        [DataMember]
        public string? Nature_Of_Complaint { get; set; }
        [DataMember]
        public string? ABB_Plan_Name { get; set; }
        [DataMember]
        public string? Order { get; set; }
        [DataMember]
        public string? Payment { get; set; }
        [DataMember]
        public string? EVC_Drop { get; set; }
        [DataMember]
        public string? Tech_Evl_Required { get; set; }
        [DataMember]
        public string? Physical_Evolution { get; set; }
        [DataMember]
        public string? Installation { get; set; }
        [DataMember]
        public string? Amount_Need_To_Be_Paid_To_Customer { get; set; }

        [DataMember]
        public string? Retailer_Phone_Number { get; set; }
        [DataMember]
        public string? EVC_Status { get; set; }
        [DataMember]
        public string? Pickup { get; set; }
        [DataMember]
        public string? Pickup_Date { get; set; }
        [DataMember]
        public string? Date_Of_Complaint { get; set; }
        [DataMember]
        public string? ABB_Plan_Period { get; set; }
        [DataMember]
        public string? E_Mail_SMS { get; set; }
        [DataMember]
        public string? Accounts { get; set; }
        [DataMember]
        public string? Mode { get; set; }
        [DataMember]
        public string? Total_Amt_Paid { get; set; }
        [DataMember]
        public string? Product_Category { get; set; }
        [DataMember]
        public string? Base_Exch_Value_Q { get; set; }
        [DataMember]
        public string? Order_Type { get; set; }
        [DataMember]
        public string? Brand_Type { get; set; }
        [DataMember]
        public OldBrand? Old_Brand { get; set; }
        [DataMember]
        public string? Cust_Declared_Qlty { get; set; }
        [DataMember]
        public string? Latest_Status { get; set; }
        [DataMember]
        public string? Posting_Flag { get; set; }
        [DataMember]
        public string? Retailer_Phone_Number_UTC { get; set; }
        [DataMember]
        public string? Physical_Evaluation { get; set; }
        [DataMember]
        public string? Status_Reason { get; set; }
        [DataMember]
        public string? Estimate_Delivery_Date { get; set; }
        [DataMember]
        public string? Actual_Amount_Paid { get; set; }
        [DataMember]
        public string? Amount_Payable_Through_LGC { get; set; }
        [DataMember]
        public string? LGC_Tkt_No { get; set; }
        [DataMember]
        public string? Order_Date { get; set; }
        [DataMember]
        public string? Secondary_Order_Flag { get; set; }
        [DataMember]
        public string? Expected_Pickup_Date { get; set; }
        [DataMember]
        public string? Actual_Pickup_Date { get; set; }
        [DataMember]
        public string? Actual_Amt_payable_incl_Bonus { get; set; }
        [DataMember]
        public size? Size { get; set; }
        [DataMember]
        public string? Ready_For_Logistic_Ticket { get; set; }
        [DataMember]
        public string? Pickup_Priority { get; set; }
        [DataMember]
        public string? Actual_Total_Amount_as_per_QC { get; set; }


    }

    public class CustomerName
    {
        //[DataMember]
        //public string? display_value { get; set; }
        //[DataMember]
        //public string? prefix { get; set; }
        [DataMember]
        public string? last_name { get; set; }
        //[DataMember]
        //public string? suffix { get; set; }
        [DataMember]
        public string? first_name { get; set; }
    }

    public class size
    {
        [DataMember]
        public string? display_value { get; set; }
        [DataMember]
        public string? ID { get; set; }
    }
    public class NewProdGroup
    {
        [DataMember]
        public string? display_value { get; set; }
        [DataMember]
        public string? ID { get; set; }
    }

    public class NewProductTechnology
    {
        [DataMember]
        public string? display_value { get; set; }
        [DataMember]
        public string? ID { get; set; }
    }

    public class NewProdGroup1
    {
        [DataMember]
        public string? display_value { get; set; }
        [DataMember]
        public string? ID { get; set; }
    }

    public class NewProductTechnology1
    {
        [DataMember]
        public string? display_value { get; set; }
        [DataMember]
        public string? ID { get; set; }
    }

    public class SponsorName
    {
        [DataMember]
        public string? display_value { get; set; }
        [DataMember]
        public string? ID { get; set; }
    }

    public class OldBrand
    {
        public string? display_value { get; set; }
        public string? ID { get; set; }
    }


    #region Get Created Order Details

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    //public class CustomerName
    //{
    //    public string? display_value { get; set; }
    //    public string? prefix { get; set; }
    //    public string? last_name { get; set; }
    //    public string? suffix { get; set; }
    //    public string? first_name { get; set; }
    //}

    public class SponsorOrderDetail
    {
        public string? Evc_P { get; set; }
        public string? LGC_Tkt_No { get; set; }
        public string? Pic_1 { get; set; }
        public string? Actual_Type { get; set; }
        public string? Evc_S { get; set; }
        public string? Pic_2 { get; set; }
        public string? Evc_R { get; set; }
        public Size? Size { get; set; }
        public string? Pic_3 { get; set; }
        public string? Is_DtoC { get; set; }
        public string? Date_paid { get; set; }
        public string? Evc_Q { get; set; }
        public string? Pic_4 { get; set; }
        public string? Base_Exch_Value_P { get; set; }
        public string? Expected_Pickup_Date { get; set; }
        public string? Base_Exch_Value_Q { get; set; }
        public string? Base_Exch_Value_R { get; set; }
        public string? logistics_Bonus { get; set; }
        public string? Base_Exch_Value_S { get; set; }
        public string? Purchased_Product_Category { get; set; }
        public string? Sp_Order_No { get; set; }
        public string? Voucher_Code { get; set; }
        public string? QC { get; set; }
        public string? Order_Type { get; set; }
        public string? Actual_Brand { get; set; }
        public string? Nature_Of_Complaint { get; set; }
        public string? pdf_file_for_QC { get; set; }
        public string? Logistic_Pic_3 { get; set; }
        public SponsorName? Sponsor_Name { get; set; }
        public string? Logistic_Pic_2 { get; set; }
        public string? Logistic_Pic_1 { get; set; }
        public string? ID { get; set; }
        public string? Customer_login { get; set; }
        public string? Payment_To_Customer { get; set; }
        public string? New_Product_Size { get; set; }
        public string? Actual_Base_Amount_As_per_QC { get; set; }
        public string? Logistic_Pic_4 { get; set; }
        public string? ABB_Status { get; set; }
        public string? Prod_Sr_No1 { get; set; }
        public string? Record_Status1 { get; set; }
        public string? Exch_Price_ID { get; set; }
        public OldBrand? Old_Brand { get; set; }
        public string? Voucher_Redeem_Date { get; set; }
        public string? New_Product_Model_Number { get; set; }
        public string? Exchange { get; set; }
        public string? Store_Phone_Number { get; set; }
        public string? New_Prod_Group1 { get; set; }
        public string? New_Product_Code { get; set; }
        public string? ABB_Price_ID { get; set; }
        public string? Prod_Sr_No { get; set; }
        public string? Reason_For_Cancellation { get; set; }
        public string? Status_Reason { get; set; }
        public string? ABB { get; set; }
        public string? Is_Deferred { get; set; }
        public string? HSN_Code { get; set; }
        public string? Associate_Email { get; set; }
        public string? New_Product_Brand_Name1 { get; set; }
        public string? SVC_s_Call_No { get; set; }
        public string? Actual_Prod_Qlty_at_time_of_QC { get; set; }
        public string? Salutation { get; set; }
        public string? Pickup_Date { get; set; }
        public string? QC_Date { get; set; }
        public string? Store_Status { get; set; }
        public NewProdGroup? New_Prod_Group { get; set; }
        public string? Associate_Code { get; set; }
        public string? Preferred_QC_DateTime { get; set; }
        public string? Price_Start_Date { get; set; }
        public string? Actual_Size { get; set; }
        public string? Logistic_By { get; set; }
        public string? Amount_Payable_Through_LGC { get; set; }
        public string? Upload_Date_Time { get; set; }
        public string? Installation { get; set; }
        public string? Proof_Of_Delivery { get; set; }
        public string? Store_Code { get; set; }
        public string? EVC_Status { get; set; }
        public string? Auto_Number1 { get; set; }
        public NewProductTechnology? New_Product_Technology { get; set; }
        public string? EVC_Drop_Date { get; set; }
        public string? Cust_Declaration { get; set; }
        public string? Is_Voucher_Redeemed { get; set; }
        public string? First_Attempt_Date { get; set; }
        public string? New_product_Net_price { get; set; }
        public string? Voucher_Redeemed_By { get; set; }
        public string? Sponsor_Prog_code { get; set; }
        public string? Customer_Email_Address { get; set; }
        public string? New_Product_Technology1 { get; set; }
        public string? Customer_State_Name { get; set; }
        public string? Compressor_No_ODU_Sr_No { get; set; }
        public CustomerName? Customer_Name { get; set; }
        public string? QC_Comment { get; set; }
        public string? Lgc_Tkt_Created_Date { get; set; }
        public string? Cust_Declared_Qlty { get; set; }
        public string? ABB_Fees { get; set; }
        public string? Logistics_Status_Remark { get; set; }
        public string? Payment_Date { get; set; }
        public string? Latest_Status { get; set; }
        public string? Is_Under_Warranty { get; set; }
        public string? Invoice_No { get; set; }
        public string? E_Mail_SMS_Date { get; set; }
        public string? Actual_Amt_payable_incl_Bonus { get; set; }
        public string? New_Product_Name { get; set; }
        public string? ABB_Plan_Name { get; set; }
        public string? Invoice_Posted { get; set; }
        public string? Customer_Address_Landmark { get; set; }
        public string? Customer_Pincode { get; set; }
        public string? Posting_Flag { get; set; }
        public string? bank_reference { get; set; }
        public string? Order { get; set; }
        public string? Short_Number { get; set; }
        public string? Ready_For_Logistic_Ticket { get; set; }
        public string? Exchange_Status { get; set; }
        public string? Total_Quote_P { get; set; }
        public string? Total_Quote_Q { get; set; }
        public string? QC_by_LGC_Revise_EVC_Code { get; set; }
        public string? Total_Quote_R { get; set; }
        public string? Total_Quote_S { get; set; }
        public string? Payment { get; set; }
        public string? Associate_Name { get; set; }
        public string? Actual_Pickup_Date { get; set; }
        public string? EVC_Drop { get; set; }
        public string? Estimate_Delivery_Date { get; set; }
        public string? Preferred_Time_For_QC { get; set; }
        public string? Installation_Date { get; set; }
        public string? Actual_Amount_Paid { get; set; }
        public string? Actual_EVC_Amount_As_Per_QC { get; set; }
        public string? Invoice_Image { get; set; }
        public string? Actual_Age { get; set; }
        public string? First_Name { get; set; }
        public string? Cust_OK_for_Price { get; set; }
        public string? Voucher_Amount { get; set; }
        public string? Price_End_Date { get; set; }
        public string? EVC_Acknowledge { get; set; }
        public string? Customer_Mobile { get; set; }
        public string? Actual_Total_Amount_as_per_QC { get; set; }
        public string? Posting_Date { get; set; }
        public string? Sweetener_Bonus_Amount_By_Sponsor { get; set; }
        public string? Regd_No { get; set; }
        public string? Pickup_Priority { get; set; }
        public string? Pickup { get; set; }
        public string? Mode { get; set; }
        public string? New_product_invoice_Date { get; set; }
        public string? ABB_Plan_Period { get; set; }
        public string? Order_Date { get; set; }
        public string? Customer_City { get; set; }
        public string? Customer_Declared_Age { get; set; }
        public string? Last_Name { get; set; }
        public string? Closed { get; set; }
        public string? Secondary_Order_Flag { get; set; }
        public string? E_Mail_SMS { get; set; }
        public string? Latest_Date_Time { get; set; }
        public string? Customer_Address_1 { get; set; }
        public string? Customer_Address_2 { get; set; }
    }

    //public class NewProdGroup
    //{
    //    public string? display_value { get; set; }
    //    public string? ID { get; set; }
    //}

    //public class NewProductTechnology
    //{
    //    public string? display_value { get; set; }
    //    public string? ID { get; set; }
    //}

    //public class OldBrand
    //{
    //    public string? display_value { get; set; }
    //    public string? ID { get; set; }
    //}

    public class SponsorOrderDetailForExchangeDataContract
    {
        public int code { get; set; }
        public SponsorOrderDetail? data { get; set; }
    }

    //public class Size
    //{
    //    public string? display_value { get; set; }
    //    public string? ID { get; set; }
    //}

    //public class SponsorName
    //{
    //    public string? display_value { get; set; }
    //    public string? ID { get; set; }
    //}



    #endregion
}
