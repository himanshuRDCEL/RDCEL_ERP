using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Model.Base;
using RDCELERP.Model.Whatsapp;
using System.Text;
using static RDCELERP.Model.Whatsapp.CommonWhatsAppViewModel;

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
        public async Task<HttpResponseDetails> SendWhatsAppMessageAsync(string templateId, string recipientNumber, List<string> templateParams)
        {
            string apiURL = _baseConfig.Value.AiSensyApiURL?.ToString();
            string apiKey = _baseConfig.Value.AiSensyApiKey?.ToString();

            if (string.IsNullOrEmpty(apiURL) || string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("API URL/API Key missing");
            }

            var content = new
            {
                apiKey = apiKey,
                campaignName = templateId,
                destination = recipientNumber,
                templateParams = templateParams
            };

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var jsonContent = JsonConvert.SerializeObject(content);
                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(apiURL, httpContent).ConfigureAwait(false);
                    Console.WriteLine($"Response Status: {response.StatusCode}");
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response Content: {responseContent}");

                    return new HttpResponseDetails { Response = response, Content = responseContent };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WhatsApp: {ex.Message}");
                throw;
            }

        }

      

    }
}
