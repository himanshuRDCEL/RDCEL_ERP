using Newtonsoft.Json;
using RDCELERP.BAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.RazorPayX;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.Common.Helper;

namespace RDCELERP.BAL.MasterManager
{
    public class RazorpayXService : IRazorpayXService
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public readonly IOptions<ApplicationSettings> _config;
        ILogging _logging;

        public RazorpayXService(ILogging logging, HttpClient httpClient, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config)
        {
            _httpClient = httpClient;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
            var byteArray = Encoding.ASCII.GetBytes(_config.Value.RazorPaykey_secret);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            _logging = logging;
        }

        public async Task<RazorpayContactInfo> GetContactAsync(string regdNo, string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.razorpay.com/v1/contacts/{regdNo}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<RazorpayContactInfo>(json);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RazorPayManager", "GetContactAsync", ex);
                return null;
            }
        }

        public async Task<RazorpayFundAccountInfo> GetFundAccountAsync(string contactId, string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.razorpay.com/v1/fund_accounts?contact_id={contactId}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                var root = JsonConvert.DeserializeObject<RazorpayFundAccountListResponse>(json);
                return root?.items?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RazorPayManager", "GetFundAccountAsync", ex);
                return null;
            }
        }

        public async Task<RazorpayContactInfo> CreateContactAsync(object contactPayload)
        {
            try
            {
                string key = _config.Value.RazorPayKey_Id;
                string secret = _config.Value.RazorPaykey_secret;
                var token = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{key}:{secret}"));

                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.razorpay.com/v1/contacts");
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);
                request.Content = new StringContent(JsonConvert.SerializeObject(contactPayload), Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    string err = await response.Content.ReadAsStringAsync();
                    _logging.WriteErrorToDB("RazorPayManager", "CreateContactAsync Failed", new Exception(err));
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<RazorpayContactInfo>(json);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RazorPayManager", "CreateContactAsync", ex);
                return null;
            }
        }

        public async Task<RazorpayFundAccountInfo> CreateFundAccountAsync(object fundAccountPayload)
        {
            try
            {
                string key = _config.Value.RazorPayKey_Id;
                string secret = _config.Value.RazorPaykey_secret;
                var token = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{key}:{secret}"));
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.razorpay.com/v1/fund_accounts");
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);
                request.Content = new StringContent(JsonConvert.SerializeObject(fundAccountPayload), Encoding.UTF8, "application/json");
                var response = await _httpClient.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logging.WriteErrorToDB("RazorPayManager", "CreateFundAccountAsync - ResponseError", new Exception(json));
                    return null;
                }
                return JsonConvert.DeserializeObject<RazorpayFundAccountInfo>(json);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RazorPayManager", "CreateFundAccountAsync", ex);
                return null;
            }
        }
        public async Task<RazorpayPayoutResponse> MakePayoutAsync(object payoutPayload, string basicAuthToken)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.razorpay.com/v1/payouts");
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuthToken);
                request.Content = new StringContent(JsonConvert.SerializeObject(payoutPayload), Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logging.WriteErrorToDB("RazorPayManager", "MakePayoutAsync", new Exception(json));
                    return null;
                }

                return JsonConvert.DeserializeObject<RazorpayPayoutResponse>(json);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RazorPayManager", "MakePayoutAsync", ex);
                return null;
            }
        }

        public string GenerateToken(string keyId, string keySecret)
        {
            try
            {
                var byteArray = Encoding.ASCII.GetBytes($"{keyId}:{keySecret}");
                return Convert.ToBase64String(byteArray);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RazorPayManager", "GenerateToken", ex);
                return null;
            }
        }
    }
}
