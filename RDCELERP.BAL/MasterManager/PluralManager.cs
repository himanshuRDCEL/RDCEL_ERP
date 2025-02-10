using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Common.Helper;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UTC_Model.PluralGateway;
using RDCELERP.BAL.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;

namespace RDCELERP.BAL.MasterManager
{
    public class PluralManager : IPluralGatewayManager
        {
        #region variable declaration
        private readonly IExchangeOrderManager _ExchangeOrderManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStateManager _stateManager;
        private readonly ICityManager _cityManager;
        private readonly IBrandManager _brandManager;
        private readonly IBusinessPartnerManager _businessPartnerManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        private readonly IPinCodeManager _pinCodeManager;
        private readonly IQCCommentManager _QcCommentManager;
        IOrderQCRepository _orderQCRepository;
        IOrderTransRepository _orderTransRepository;
        INotificationManager _notificationManager;
        IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _WhatsAppMessageRepository;
        IExchangeOrderRepository _exchangeOrderRepository;
        ICustomerDetailsRepository _customerDetailsRepository;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly ILogisticsRepository _logisticsRepository;
        private readonly IAbbRegistrationRepository _abbRegistrationRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        ILogging _logging;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        public readonly IOptions<ApplicationSettings> _config;
       
        public readonly IWalletTransactionRepository _walletTransactionRepository;
        public readonly IExchangeABBStatusHistoryRepository _exchangeABBStatusHistoryRepository;
        public readonly ICommonManager _commonManager;
        public readonly IABBRedemptionRepository _aBBRedemptionRepository;


        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();



        #endregion

        #region constructor
        public PluralManager(IQCCommentManager QcCommentManager, IPinCodeManager pinCodeManager, IStoreCodeManager storeCodeManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IProductTypeManager productTypeManager, IExchangeOrderManager exchangeOrderManager, IProductCategoryManager productCategoryManager, IBusinessPartnerManager businessPartnerManager, IBrandManager brandManager, IStateManager StateManager, ICityManager CityManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, IUserManager userManager, CustomDataProtection protector, IOrderQCRepository orderQCRepository, IOrderTransRepository orderTransRepository, ICustomerDetailsRepository customerDetailsRepository, IExchangeOrderRepository exchangeOrderRepository, IWhatsAppMessageRepository whatsAppMessageRepository, IWhatsappNotificationManager whatsappNotificationManager, ILogisticsRepository logisticsRepository, IAbbRegistrationRepository abbRegistrationRepository, IProductTypeRepository productTypeRepository, IProductCategoryRepository productCategoryRepository, ILogging logging, IStateRepository stateRepository, ICityRepository cityRepository, IWalletTransactionRepository walletTransactionRepository, IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository, ICommonManager commonManager, IABBRedemptionRepository aBBRedemptionRepository)

        {
            _webHostEnvironment = webHostEnvironment;
            _ExchangeOrderManager = exchangeOrderManager;
            _stateManager = StateManager;
            _cityManager = CityManager;
            _brandManager = brandManager;
            _businessPartnerManager = businessPartnerManager;
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
            _pinCodeManager = pinCodeManager;
            _QcCommentManager = QcCommentManager;
            _orderQCRepository = orderQCRepository;
            _orderTransRepository = orderTransRepository;
            _customerDetailsRepository = customerDetailsRepository;
            _exchangeOrderRepository = exchangeOrderRepository;
            _WhatsAppMessageRepository = whatsAppMessageRepository;
            _whatsappNotificationManager = whatsappNotificationManager;
            _context = context;
            _logisticsRepository = logisticsRepository;
            _abbRegistrationRepository = abbRegistrationRepository;
            _productTypeRepository = productTypeRepository;
            _productCategoryRepository = productCategoryRepository;
            _logging = logging;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _config = config;           
            _walletTransactionRepository = walletTransactionRepository;
            _commonManager = commonManager;
            _aBBRedemptionRepository = aBBRedemptionRepository;
        }
        #endregion
        #region Plural  service Call
        /// <summary>
        /// Method to POST form-data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public RestResponse Rest_InvokePluralServiceCall(string url, Method methodType, string Hasstring, object content = null)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            RestResponse getResponse = null;
            string jsonString = string.Empty;
            string responseString = string.Empty;
            string secretKey = _config.Value.SecretKeyPlural.ToString();
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest();
                request.Method = methodType;
                request.AddHeader("x-verify", Hasstring);
                request.AddHeader("content-type", "application/json");
                if (content != null)
                {
                    jsonString = JsonConvert.SerializeObject(content);
                    request.AddJsonBody(jsonString);
                }
                getResponse = client.Execute(request);
            }
            catch (Exception ex)
            {
                //.WriteErrorToDB("PluralServicecall", "Rest_InvokePluralServiceCall", ex);
            }
            return getResponse;
        }

        #endregion
        #region GetOrderStatus for payment
        public RestResponse Rest_InvokePluralServiceCallGetPaymentStatus(string url, Method methodType, string Encryption, object content = null)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            RestResponse getResponse = null;
            string jsonString = string.Empty;
            string responseString = string.Empty;
            string secretKey = _config.Value.SecretKeyPlural.ToString();
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest();
                request.Method = methodType;
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Authorization", Encryption);

                if (content != null)
                {
                    //jsonString = JsonConvert.SerializeObject(content);
                    //request.AddJsonBody(jsonString);
                }
                getResponse = client.Execute(request);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PluralServicecall", "Rest_InvokePluralServiceCallGetPaymentStatus", ex);
            }
            return getResponse;

        }
        #endregion

        #region 
        public async Task<string> MakeRequest(string url, string username, string password)
        {
            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(username, password)
            };

            var client = new HttpClient(handler);

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            return content;
        }
        #endregion
        #region create order service manager
        public RestResponse CreateOrderManager(PluralCreateOrder pluralCreateOrder, string SecretKey)
        {
            CreateOrderResponse orderResponse = new CreateOrderResponse();
            CreateOrderEncryption orderEncryption = new CreateOrderEncryption();
            RestResponse Response = null;
            string URl = null;
            try
            {
                URl = _config.Value.CreateOrderUrl.ToString();
                string Jsonrequest = JsonConvert.SerializeObject(pluralCreateOrder);
                byte[] jsonbytes = Encoding.UTF8.GetBytes(Jsonrequest);
                string newbase64 = Convert.ToBase64String(jsonbytes);
                string hashset = ChecksumCalculator.GetSHAGenerated(newbase64, SecretKey);
                orderEncryption.request = newbase64;
                Response = Rest_InvokePluralServiceCall(URl, Method.Post, hashset, orderEncryption);

                //string responsecode = Response.StatusCode.ToString();
                //if (responsecode == "OK")
                //{
                //    string response = Response.Content;
                //    orderResponse = JsonConvert.DeserializeObject<CreateOrderResponse>(response);
                //}
               
            }
            catch (Exception ex)
            {
                //LibLogging.WriteErrorToDB("ABBRegManager", "createOrderManager", ex);
            }
            return Response;
        }
        #endregion

        #region Get Order status
        public GetpaymentStatusResponse GetpaymentStatus(string PaymentId, string OrderId)
        {
            GetpaymentStatusResponse getsorderResponse = new GetpaymentStatusResponse();
            string Url = null;
            RestResponse Response = null;
            string MerchantId = null;
            string AccessCode = null;
            try
            {
                Url = _config.Value.GetOrderStatus.ToString() + "/order/" + OrderId + "/payment/" + PaymentId;
                if (PaymentId != null && OrderId != null)
                {
                    MerchantId = _config.Value.MerchantIdPlural.ToString();
                    AccessCode = _config.Value.AccessCodePlural.ToString();
                    string Encoded = MerchantId + ":" + AccessCode;
                    byte[] encrypted = Encoding.UTF8.GetBytes(Encoded);
                    string result = MakeRequest(Url, MerchantId, AccessCode).ToString();
                    Encoded = Convert.ToBase64String(encrypted);
                    Encoded = "Basic " + Encoded;
                    Response = Rest_InvokePluralServiceCallGetPaymentStatus(Url, Method.Get, Encoded);
                    string respoonsecode = Response.StatusCode.ToString();
                    if (respoonsecode == "OK")
                    {
                        getsorderResponse = JsonConvert.DeserializeObject<GetpaymentStatusResponse>(Response.Content);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PluralManager", "GetpaymentStatus", ex);
            }
            return getsorderResponse;
        }
        #endregion
    }

}
