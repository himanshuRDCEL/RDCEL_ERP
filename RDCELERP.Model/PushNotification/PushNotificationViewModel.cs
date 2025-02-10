using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.PushNotification
{
    public class PushNotificationViewModel
    {
        [JsonProperty("deviceId")]
        public string? DeviceId { get; set; }
        [JsonProperty("isAndroiodDevice")]
        public bool IsAndroiodDevice { get; set; }
        [JsonProperty("title")]
        public string? Title { get; set; }
        [JsonProperty("body")]
        public string? Body { get; set; }
        [JsonProperty("click_action")]
        public string? Click_Action { get; set; }
        [JsonProperty("small_icon")]
        public string? Small_Icon { get; set; }
        [JsonProperty("tag")]
        public string? Tag { get; set; }
    }

    public class GoogleNotification
    {
        public class DataPayload
        {
            [JsonProperty("title")]
            public string? Title { get; set; }
            [JsonProperty("body")]
            public string? Body { get; set; }
            [JsonProperty("click_action")]
            public string? Click_Action { get; set; }
            [JsonProperty("small_icon")]
            public string? Small_Icon { get; set; }
            [JsonProperty("tag")]
            public string? Tag { get; set; }
            public android? AndroidAttributes; // Add this property
            public class android
            {

                [JsonProperty("click_action")]
                public string? Click_Action { get; set; }
                [JsonProperty("small_icon")]
                public string? Small_Icon { get; set; }
                [JsonProperty("tag")]
                public string? Tag { get; set; }


            }
        }
       
        [JsonProperty("priority")]
        public string? Priority { get; set; } = "high";
        [JsonProperty("data")]
        public DataPayload? Data { get; set; }
        [JsonProperty("notification")]
        public DataPayload? Notification { get; set; }
        

        
    }
    
}

