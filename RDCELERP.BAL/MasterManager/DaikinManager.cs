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
using RDCELERP.Model.CashfreeModel;
using RDCELERP.Model.DaikinModel;

namespace RDCELERP.BAL.MasterManager
{
    public class DaikinManager : IDaikinManager
    {
        #region variable declaration
        IOptions<ApplicationSettings> _baseConfig;
        ILogging _logging;
        #endregion

        public DaikinManager(IOptions<ApplicationSettings> baseConfig, ILogging logging)
        {
            _baseConfig = baseConfig;
            _logging = logging;
        }

        public DaikinAuth DaikinAuthCall()
        {
            DaikinAuth daikinAuth = new DaikinAuth();
            string url = _baseConfig.Value.DaikinAuthUrl;
            string response = null;
            try
            {
                response = Rest_ResponseDaikinAuthCall(url, Method.Post);
                if(response != null)
                {
                    daikinAuth = JsonConvert.DeserializeObject<DaikinAuth>(response);
                    if(daikinAuth.access_token != null)
                    {
                        daikinAuth.access_token = "Bearer " + daikinAuth.access_token;
                    }
                }
            }
            catch(Exception ex)
            {
                _logging.WriteErrorToDB("DaikinManager", "DaikinAuthCall", ex);
            }
            return daikinAuth;
        }

        public string Rest_ResponseDaikinAuthCall(string url, Method method)
        {
            string grantType = _baseConfig.Value.DaikinGrantType;
            string scope = _baseConfig.Value.DaikinScope;
            string clientId = _baseConfig.Value.DaikinClientId;
            string clientSecret = _baseConfig.Value.DaikinClientSecret;
            bool refreshToken = (bool)_baseConfig.Value.DaikinRefreshToken;
            string authorities = _baseConfig.Value.DaikinAuthorities;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            RestResponse restResponse = null;
            string response = null;
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest();
                request.Method = method;

                request.AddQueryParameter("grant_type", grantType);
                request.AddQueryParameter("scope", scope);
                request.AddQueryParameter("client_id", clientId);
                request.AddQueryParameter("client_secret", clientSecret);
                request.AddQueryParameter("supportRefreshToken", refreshToken);
                request.AddQueryParameter("authorities", authorities);

                restResponse = client.Execute(request);
                response = restResponse.Content;
            }
            catch(Exception ex)
            {
                _logging.WriteErrorToDB("DaikinManager", "Rest_ResponseDaikinAuthCall", ex);
            }
            return response;
        }

        public string PushOrderStatus(DaikinOrderStatus daikinOrderStatus, string AuthToken)
        {
            string response = null;
            string url = _baseConfig.Value.DaikinPushOrderStatusUrl;
            try
            {
                if(daikinOrderStatus != null && AuthToken != null)
                {
                    url = url + daikinOrderStatus.GreenExchangeOrderId + "/sendOrderStatus/" + daikinOrderStatus.OrderStatus;
                    response = Rest_ResponseDaikinServiceCall(url, daikinOrderStatus, Method.Post, AuthToken);
                    if(response != null)
                    {
                        _logging.WriteAPIRequestToDB("DaikinManager", "PushOrderStatus", daikinOrderStatus.GreenExchangeOrderId, response);
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DaikinManager", "PushOrderStatus", ex);
            }
            return response;
        }

        public string Rest_ResponseDaikinServiceCall(string url, DaikinOrderStatus daikinOrderStatus, Method method, string AuthToken)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            RestResponse restResponse = null;
            string response = null;
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest();
                request.Method = method;

                request.AddHeader("Authorization", AuthToken);
                request.AddQueryParameter("greenExchangeOrderId", daikinOrderStatus.GreenExchangeOrderId);
                request.AddQueryParameter("orderStatus", daikinOrderStatus.OrderStatus);
                restResponse = client.Execute(request);
                response = restResponse.Content;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DaikinManager", "Rest_ResponseDaikinServiceCall", ex);
            }
            return response;
        }
    }
}
