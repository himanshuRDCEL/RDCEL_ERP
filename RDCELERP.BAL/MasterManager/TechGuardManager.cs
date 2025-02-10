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
using RDCELERP.Model.APICalls;
using RDCELERP.Model.Base;
using RDCELERP.Model.TechGuard;

namespace RDCELERP.BAL.MasterManager
{
    public class TechGuardManager : ITechGuard
    {
        #region variable declaration
        IOptions<ApplicationSettings> _baseConfig;
        ILogging _logging;
        ICommonManager _commonManager;
        #endregion

        #region constructor
        public TechGuardManager(IOptions<ApplicationSettings> baseConfig, ILogging logging, ICommonManager commonManager)
        {
            _baseConfig = baseConfig;
            _logging = logging;
            _commonManager = commonManager;
        }
        #endregion
        public RestResponse TechGuardServiceCall(string url, Method methodType, object content = null, int? loggedInUserId=null)
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
                if (content != null)
                {
                    jsonString = JsonConvert.SerializeObject(content);
                    request.AddJsonBody(jsonString);
                }
                getResponse = client.Execute(request);
                ApicallViewModel apicallTable =new ApicallViewModel();
                apicallTable.Url = url;
                apicallTable.MethodType = methodType.ToString();
                apicallTable.RequestBody = jsonString.ToString();
                apicallTable.ResponseBody= JsonConvert.SerializeObject(getResponse);
                apicallTable.IsActive = true;   
                apicallTable.CreatedDate = DateTime.Now;
                apicallTable.CreatedBy= loggedInUserId;
                _commonManager.SaveApiCallInfo(apicallTable);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("TechGuardManager", "TechGuardServiceCall", ex);
            }
            return getResponse;
        }
        public TechGuardResponse PaymentResponse(TechGuardRequestModel techGuardDC, int? loggedInUserId)
        {
            TechGuardResponse responseDC = new TechGuardResponse();
            RestResponse response = null;
            try
            {
                if (techGuardDC != null)
                {
                    string url = _baseConfig.Value.TechGuardUrl;
                    response = TechGuardServiceCall(url,Method.Post,techGuardDC, loggedInUserId);
                    string ResposneTC = response.Content;
                    if (ResposneTC != null)
                    {
                        responseDC = JsonConvert.DeserializeObject<TechGuardResponse>(ResposneTC);
                        _logging.WriteAPIRequestToDB("TechGuardManager", "PaymentResponse", techGuardDC.repair_id, ResposneTC);
                    }

                  
                }
                else
                {
                    responseDC.remark = "Please provide valid data for processing request";
                }
            }
            catch(Exception ex)
            {
                _logging.WriteErrorToDB("TechGuardManager", "PaymentResponse", ex);
            }
            return responseDC;
        }
    }
}
