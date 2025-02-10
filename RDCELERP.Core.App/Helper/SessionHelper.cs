using RDCELERP.Model.Users;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDCELERP.Core.App.Helper
{
    public static class SessionHelper
    {
        public static LoginViewModel LoginViewModel { get; set; }


        /// <summary>
        /// intializes session helper
        /// </summary>
        //public static SessionHelper()
        //{
        //    LoginViewModel = new LoginViewModel();
        //}

        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
