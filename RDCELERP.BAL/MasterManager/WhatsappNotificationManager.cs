using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Model.Base;

namespace RDCELERP.BAL.MasterManager
{
    public class WhatsappNotificationManager : IWhatsappNotificationManager
    {
        #region variable declaration
        IOptions<ApplicationSettings> _baseConfig;
        ILogging _logging;
        #endregion

        #region constructor
        public WhatsappNotificationManager(IOptions<ApplicationSettings> baseConfig, ILogging logging)
        {
            _baseConfig = baseConfig;
            _logging = logging;
        }
        #endregion

        public RestResponse Rest_InvokeWhatsappserviceCall(string url, Method methodType, object content = null)
        {
            string headerToken = _baseConfig.Value.YellowAi_ApiKey;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            RestResponse getResponse = null;
            string aouthToken = string.Empty;
            string jsonString = string.Empty;
            try
            {

                var client = new RestClient(url);
                var request = new RestRequest();
                request.Method = methodType;
                request.AddHeader("content-type", "application/json");
                request.AddHeader("x-api-key", _baseConfig.Value.YellowAi_ApiKey); //Add Header tocken
                if (content != null)
                {
                    jsonString = JsonConvert.SerializeObject(content);
                    request.AddJsonBody(jsonString);
                }
                getResponse = client.Execute(request);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", ex);
            }
            return getResponse;

        }
    }
}
