using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Model.Base;

namespace RDCELERP.BAL.MasterManager
{
    public class BillCloudServiceCall : IBillCloudServiceCall
    {
        ILogging _logging;
        IOptions<ApplicationSettings> _config;


        public BillCloudServiceCall(ILogging logging, IOptions<ApplicationSettings> config)
        {
            _logging = logging;
            _config = config;
        }

        public RestResponse Rest_InvokeZohoInvoiceServiceForPlainText(string url, Method methodType, object content)
        {
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
                request.AddHeader("apikey", _config.Value.BillCloudKey); //Add Header tocken

                if (content != null)
                {
                    jsonString = JsonConvert.SerializeObject(content);
                    request.AddJsonBody(jsonString);
                }
                getResponse = client.Execute(request);

            }
            catch (Exception ex)
            {
                throw;
            }
            return getResponse;

        }
    }
}
