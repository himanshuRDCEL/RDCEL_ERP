using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Zoho
{
  
    public class ZohoContactViewModel
    {
        public List<ZohoContactModel> data { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Approval
    {
        public bool @delegate { get; set; }
        public bool takeover { get; set; }
        public bool approve { get; set; }
        public bool reject { get; set; }
        public bool resubmit { get; set; }
    }

    public class CreatedBy
    {
        public string name { get; set; }
        public string id { get; set; }
        public string email { get; set; }
    }

    public class ZohoContactModel
    {
        public Owner Owner { get; set; }
        public object Email { get; set; }

        [JsonProperty("$currency_symbol")]
        public string currency_symbol { get; set; }

        [JsonProperty("$field_states")]
        public object field_states { get; set; }
        public object Other_Phone { get; set; }
        public object Mailing_State { get; set; }
        public object Other_State { get; set; }
        public string Gender { get; set; }
        public object Other_Country { get; set; }
        public object Last_Activity_Time { get; set; }
        public object Department { get; set; }

        [JsonProperty("$state")]
        public string state { get; set; }
        public object Unsubscribed_Mode { get; set; }

        [JsonProperty("$process_flow")]
        public bool process_flow { get; set; }
        public object Assistant { get; set; }
        public object Mailing_Country { get; set; }

        [JsonProperty("$locked_for_me")]
        public bool locked_for_me { get; set; }
        public bool Create_Quote { get; set; }
        public string id { get; set; }

        [JsonProperty("$approved")]
        public bool approved { get; set; }
        public object Reporting_To { get; set; }

        [JsonProperty("$approval")]
        public Approval approval { get; set; }
        public object Other_City { get; set; }
        public DateTime Created_Time { get; set; }

        [JsonProperty("$editable")]
        public bool editable { get; set; }
        public object Home_Phone { get; set; }
        public object Item_Category { get; set; }
        public CreatedBy Created_By { get; set; }

        [JsonProperty("$zia_owner_assignment")]
        public string zia_owner_assignment { get; set; }
        public object Secondary_Email { get; set; }

        [JsonProperty("$is_duplicate")]
        public bool is_duplicate { get; set; }
        public bool KYC_Done { get; set; }
        public string Site { get; set; }
        public object Description { get; set; }
        public string Category { get; set; }
        public object Mailing_Zip { get; set; }

        [JsonProperty("$review_process")]
        public ReviewProcess review_process { get; set; }
        public object Twitter { get; set; }
        public object Other_Zip { get; set; }
        public object Mailing_Street { get; set; }

        [JsonProperty("$layout_id")]
        public LayoutId layout_id { get; set; }
        public object Salutation { get; set; }
        public object First_Name { get; set; }
        public string Full_Name { get; set; }
        public object Asst_Phone { get; set; }
        public object Record_Image { get; set; }
        public ModifiedBy Modified_By { get; set; }

        [JsonProperty("$review")]
        public object review { get; set; }
        public object Skype_ID { get; set; }
        public string Phone { get; set; }
        public object Account_Name { get; set; }
        public bool Email_Opt_Out { get; set; }
        public DateTime Modified_Time { get; set; }
        public object Date_of_Birth { get; set; }
        public object Mailing_City { get; set; }
        public object Unsubscribed_Time { get; set; }
        public object Title { get; set; }
        public object Other_Street { get; set; }
        public object Mobile { get; set; }

        [JsonProperty("$orchestration")]
        public object orchestration { get; set; }
        public string Type { get; set; }
        public string Last_Name { get; set; }

        [JsonProperty("$in_merge")]
        public bool in_merge { get; set; }
        public bool Locked__s { get; set; }
        public object Lead_Source { get; set; }
        public string Last_Modified_Site { get; set; }
        public List<object> Tag { get; set; }
        public object Fax { get; set; }

        [JsonProperty("$approval_state")]
        public string approval_state { get; set; }
    }

    public class LayoutId
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class ModifiedBy
    {
        public string name { get; set; }
        public string id { get; set; }
        public string email { get; set; }
    }

    public class Owner
    {
        public string name { get; set; }
        public string id { get; set; }
        public string email { get; set; }
    }

    public class ReviewProcess
    {
        public bool approve { get; set; }
        public bool reject { get; set; }
        public bool resubmit { get; set; }
    }

   
}
