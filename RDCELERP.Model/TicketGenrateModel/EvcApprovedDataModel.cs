using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.TicketGenrateModel
{
    public class EvcApprovedListDataContract
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public List<EvcApprovedData>? data { get; set; }
    }

    public class EvcApprovedData
    {
        [DataMember]
        public string? E_mail_ID { get; set; }
        [DataMember]
        public string? Upload_GST_Registration { get; set; }
        [DataMember]
        public string? EVC_Name { get; set; }
        [DataMember]
        public string? Regd_Address_Line_1 { get; set; }
        [DataMember]
        public string? Copy_of_Cancelled_Cheque { get; set; }
        [DataMember]
        public string? EVC_Regd_No { get; set; }
        [DataMember]
        public string? EVC_Wallet_Amount { get; set; }
        [DataMember]
        public string? ID { get; set; }
        [DataMember]
        public string? EVC_Mobile_Number { get; set; }
        [DataMember]
        public string? Contact_Person_Address { get; set; }
        [DataMember]
        public string? Bussiness_Name { get; set; }
        [DataMember]
        public string? E_Waste_Certificate { get; set; }
        [DataMember]
        public string? City { get; set; }
        [DataMember]
        public string? PIN_Code { get; set; }
        [DataMember]
        public string? Regd_Address_Line_2 { get; set; }
        [DataMember]
        public string? State { get; set; }
        [DataMember]
        public string? Evc_Name { get; set; }
        [DataMember]
        public string? Running_Balance { get; set; }

    }




    #region EVC Master Data

    public class City1
    {
        public string? display_value { get; set; }
        public string? ID { get; set; }
    }

    public class EVCMasterDetail
    {
        public string? EVC_Wallet_Amount_Copy { get; set; }
        public string? Employee_Email_id { get; set; }
        public string? Copy_of_Cancelled_Cheque { get; set; }
        public string? Evc_Name { get; set; }
        public SelectEmployeeId? Select_Employee_Id { get; set; }
        public string? Insert_OTP { get; set; }
        public string? OTP { get; set; }
        public string? Name { get; set; }
        public State1? State1 { get; set; }
        public string? Regd_Address_Line_2 { get; set; }
        public string? Regd_Address_Line_1 { get; set; }
        public string? Employee_Name { get; set; }
        public string? ID { get; set; }
        public string? Bussiness_Name { get; set; }
        public string? E_mail_ID { get; set; }
        public string? Upload_GST_Registration { get; set; }
        public string? Running_Balance { get; set; }
        public string? EVC_Name { get; set; }
        public string? EVC_Status { get; set; }
        public string? I_Confirm_Terms_Condition { get; set; }
        public City1? City1 { get; set; }
        public string? Total_Of_Actual_Base_Amount { get; set; }
        public string? City { get; set; }
        public string? Amount_Add_In_Wallet { get; set; }
        public string? Alternate_Mobile_Number { get; set; }
        public string? Contact_Person_Address { get; set; }
        public string? City_Code { get; set; }
        public string? PIN_Code { get; set; }
        public string? Total_Of_In_Progress { get; set; }
        public string? Date_field { get; set; }
        public string? Total_Of_Delivered { get; set; }
        public string? Bank_Name { get; set; }
        public string? IFSC_Code { get; set; }
        public string? State { get; set; }
        public string? EVC_Wallet_Amount { get; set; }
        public string? EVC_Regd_No { get; set; }
        public string? Type_of_Entity { get; set; }
        public string? E_Waste_Registration_Number { get; set; }
        public string? EVC_Mobile_Number { get; set; }
        public string? Account_No { get; set; }
        public string? E_Waste_Certificate { get; set; }
        public string? Place { get; set; }
        public string? GST_Number { get; set; }
    }

    public class EVCMasterDetailDataContract
    {
        public int code { get; set; }
        public EVCMasterDetail? data { get; set; }
    }

    public class SelectEmployeeId
    {
        public string? display_value { get; set; }
        public string? ID { get; set; }
    }

    public class State1
    {
        public string? display_value { get; set; }
        public string? ID { get; set; }
    }


    #endregion
}
