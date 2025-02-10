using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.TicketGenrateModel.Mahindra
{
    public class SponserDataContract
    {
        [DataMember]
        public string? Sponsor_Name { get; set; }
        //[DataMember]
        //public string? Sponsor_New { get; set; }

        [DataMember]
        public string? Sp_Order_No { get; set; }
        [DataMember]
        public string? First_Name { get; set; }
        [DataMember]
        public string? Last_Name { get; set; }
        [DataMember]
        public string? Order_Type { get; set; }
        [DataMember]
        public string? Customer_Email_Address { get; set; }
        [DataMember]
        public string? Customer_Address_1 { get; set; }
        [DataMember]
        public string? Customer_City { get; set; }
        [DataMember]
        public string? Customer_Mobile { get; set; }
        [DataMember]
        public string? Customer_Pincode { get; set; }
        [DataMember]
        public string? Customer_Address_2 { get; set; }
        [DataMember]
        public string? Customer_Address_Landmark { get; set; }
        [DataMember]
        public string? Customer_State_Name { get; set; }
        [DataMember]
        public string? New_Prod_Group { get; set; }
        [DataMember]
        public string? New_Product_Technology { get; set; }
        [DataMember]
        public string? Size { get; set; }
        [DataMember]
        public string? Prexo_Size_Capacity { get; set; }
        [DataMember]
        public string? Prexo_Working_Condition { get; set; }
        [DataMember]
        public string? Old_Brand { get; set; }
        [DataMember]
        public string? Brand_Type { get; set; }
        [DataMember]
        public string? age_years { get; set; }
        [DataMember]
        public string? Base_Prod_Value { get; set; }
        [DataMember]
        public string? Sweetener_Bonus_Amount_By_Sponsor { get; set; }
        [DataMember]
        public string? Exchange_Price_Displayed_Max { get; set; }

        [DataMember]
        public string? Total_Exch_Value_R { get; set; }
        [DataMember]
        public string? Actual_Value_PQRS { get; set; }
        [DataMember]
        public string? New_Prod_Group1 { get; set; }
        [DataMember]
        public string? New_Product_Technology1 { get; set; }
        [DataMember]
        public string? New_Product_Brand_Name1 { get; set; }
        [DataMember]
        public string? Sr_No { get; set; }
        [DataMember]
        public string? New_Product_Model_Number { get; set; }
        //[DataMember]
        //public string? ABB_Plan_Name { get; set; }
        [DataMember]
        public string? HSN_Code { get; set; }
        [DataMember]
        public string? New_product_Invoice_Number { get; set; }
        [DataMember]
        public string? New_product_invoice_Date { get; set; }
        [DataMember]
        public string? New_product_Net_price { get; set; }
        [DataMember]
        public string? New_Product_Size { get; set; }
        //[DataMember]
        //public string? ABB_Fees { get; set; }
        //[DataMember]
        //public string? ABB_Plan_Period { get; set; }
        [DataMember]
        public string? Price_Start_Date { get; set; }
        [DataMember]
        public string? Price_End_Date { get; set; }
        [DataMember]
        public string? Regd_No { get; set; }
        [DataMember]
        public string? Upload_Date_Time { get; set; }
        [DataMember]
        public string? Store_Code { get; set; }
        [DataMember]
        public string? Store_Phone_Number { get; set; }
        [DataMember]
        public string? Order_Status { get; set; }
        [DataMember]
        public string? New_Product_Delivered_Date { get; set; }
        [DataMember]
        public string? New_Product_Delivery_Status { get; set; }
        [DataMember]
        public string? New_Product_Installation_Date { get; set; }
        //[DataMember]
        //public string? ABB { get; set; }
        //[DataMember]
        //public string? ABB_Status { get; set; }
        //[DataMember]
        //public string? Exchange { get; set; }
        //[DataMember]
        //public string? Exchange_Status { get; set; }
        [DataMember]
        public string? Sponsor_Prog_code { get; set; }
        [DataMember]
        public string? Exch_Price_ID { get; set; }
        //[DataMember]
        //public string? ABB_Price_ID { get; set; }
        [DataMember]
        public string? Order_Date { get; set; }
        [DataMember]
        public string? EVC_Status { get; set; }
        [DataMember]
        public string? Order { get; set; }

        //new fields

        [DataMember]
        public string? Is_Under_Warranty { get; set; }
        [DataMember]
        public string? Nature_Of_Complaint { get; set; }
        [DataMember]
        public string? Bulk_Mode { get; set; }
        [DataMember]
        public string? Product_Category { get; set; }
        [DataMember]
        public string? Level_Of_Irritation { get; set; }
        [DataMember]
        public string? Tech_Evl_Required { get; set; }
        [DataMember]
        public string? Physical_Evolution { get; set; }
        [DataMember]
        public string? Retailer_Phone_Number { get; set; }
        [DataMember]
        public string? Alternate_Email { get; set; }
        [DataMember]
        public string? Problem_Description { get; set; }
        [DataMember]
        public string? Date_Of_Complaint { get; set; }
        [DataMember]
        public string? Actual_Prod_Qlty_at_time_of_QC { get; set; }
        [DataMember]
        public string? Cust_Declared_Qlty { get; set; }
        [DataMember]
        public string? Base_Exch_Value_P { get; set; }
        [DataMember]
        public string? Base_Exch_Value_Q { get; set; }
        [DataMember]
        public string? Base_Exch_Value_R { get; set; }
        [DataMember]
        public string? Base_Exch_Value_S { get; set; }
        // new 
        [DataMember]
        public string? Estimate_Delivery_Date { get; set; }
        [DataMember]
        public string? Latest_Status { get; set; }
        [DataMember]
        public string? Secondary_Order_Flag { get; set; }
        [DataMember]
        public string? Status_Reason { get; set; }
        [DataMember]
        public string? Expected_Pickup_Date { get; set; }

        public string? Associate_Name { get; set; }
        public string? Associate_Email { get; set; }
        public string? Associate_Code { get; set; }
        public string? Purchased_Product_Category { get; set; }

        [DataMember]
        public string? Preferred_QC_DateTime { get; set; }
        [DataMember]
        public string? Is_Deferred { get; set; }
        [DataMember]
        public string? Is_DtoC { get; set; }
    }

    public class Data
    {
        [DataMember]
        public string? ID { get; set; }
    }

    public class SponserFormResponseDataContract
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public Data? data { get; set; }
        [DataMember]
        public string? message { get; set; }

    }

    public class ResultInRequestDataContract
    {

        [DataMember]
        public bool message { get; set; }
        [DataMember]
        public bool tasks { get; set; }
    }

    public class SponserFormRequestDataContract
    {
        public SponserFormRequestDataContract()
        {
            result = new ResultInRequestDataContract();
        }
        [DataMember]
        public SponserDataContract data { get; set; }
        [DataMember]
        public ResultInRequestDataContract result { get; set; }

    }
}
