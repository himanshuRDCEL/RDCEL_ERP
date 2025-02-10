using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.TicketGenrateModel;
using RDCELERP.Model.TicketGenrateModel.Bizlog;
using RestSharp;
using RDCELERP.Common.Enums;
using RDCELERP.Model.TicketGenrateModel.Mahindra;
using System.ComponentModel.DataAnnotations;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Address = RDCELERP.Model.TicketGenrateModel.Mahindra.Address;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using RDCELERP.BAL.Helper;
using Org.BouncyCastle.Asn1.Ocsp;
using RDCELERP.BAL.Enum;

namespace RDCELERP.BAL.MasterManager
{

    public class TicketGenrateManager : ITicketGenrateManager
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
        private readonly IEVCRepository _eVCRepository;
        private readonly IBusinessUnitRepository _businessUnitRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        ILogging _logging;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        public readonly IOptions<ApplicationSettings> _config;
        public readonly IBizlogTicketRepository _bizlogTicketRepository;
        public readonly IWalletTransactionRepository _walletTransactionRepository;
        public readonly IExchangeABBStatusHistoryRepository _exchangeABBStatusHistoryRepository;
        public readonly ICommonManager _commonManager;
        public readonly IABBRedemptionRepository _aBBRedemptionRepository;
        public readonly IOrderLGCRepository _orderLGCRepository;
        public readonly IVoucherRepository _voucherRepository;
        public readonly IBusinessPartnerRepository _businessPartnerRepository;

        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();



        #endregion

        #region constructor
        public TicketGenrateManager(IQCCommentManager QcCommentManager, IPinCodeManager pinCodeManager, IStoreCodeManager storeCodeManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IProductTypeManager productTypeManager, IExchangeOrderManager exchangeOrderManager, IProductCategoryManager productCategoryManager, IBusinessPartnerManager businessPartnerManager, IBrandManager brandManager, IStateManager StateManager, ICityManager CityManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, IUserManager userManager, CustomDataProtection protector, IOrderQCRepository orderQCRepository, IOrderTransRepository orderTransRepository, ICustomerDetailsRepository customerDetailsRepository, IExchangeOrderRepository exchangeOrderRepository, IWhatsAppMessageRepository whatsAppMessageRepository, IWhatsappNotificationManager whatsappNotificationManager, ILogisticsRepository logisticsRepository, IAbbRegistrationRepository abbRegistrationRepository, IProductTypeRepository productTypeRepository, IProductCategoryRepository productCategoryRepository, ILogging logging, IStateRepository stateRepository, ICityRepository cityRepository, IBizlogTicketRepository bizlogTicketRepository, IWalletTransactionRepository walletTransactionRepository, IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository, ICommonManager commonManager, IABBRedemptionRepository aBBRedemptionRepository, IEVCRepository eVCRepository, IBusinessUnitRepository businessUnitRepository, IOrderLGCRepository orderLGCRepository, IVoucherRepository voucherRepository, IBusinessPartnerRepository businessPartnerRepository)

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
            _bizlogTicketRepository = bizlogTicketRepository;
            _walletTransactionRepository = walletTransactionRepository;
            _commonManager = commonManager;
            _aBBRedemptionRepository = aBBRedemptionRepository;
            _eVCRepository = eVCRepository;
            _businessUnitRepository = businessUnitRepository;
            _orderLGCRepository = orderLGCRepository;
            _voucherRepository = voucherRepository;
            _businessPartnerRepository = businessPartnerRepository;
        }
        #endregion

        public class ValidationResponse
        {
            public List<ValidationResult> Results { get; set; }
            public bool IsValid { get; set; }

            public ValidationResponse()
            {
                Results = new List<ValidationResult>();
                IsValid = false;
            }
        }

        #region Set SponserObject for customer detils
        public CustomerandOrderDetailsDataContract SetOrderDetailsObjectForABB(TblAbbredemption aBBRedemption)
        {
            //_customerDetailsRepository = new CustomerDetailsRepository();
            //productCategoryRepository = new ProductCategoryRepository();
            //productTypeRepository = new ProductTypeRepository();
            //_abbregistrationRepository = new ABBRegistrationRepository();
            CustomerandOrderDetailsDataContract customerDC = new CustomerandOrderDetailsDataContract();
            try
            {
                TblAbbregistration abbRegistration = _abbRegistrationRepository.GetSingle(x => x.AbbregistrationId == aBBRedemption.AbbregistrationId);
                if (abbRegistration != null)
                {
                    customerDC.FirstName = abbRegistration.CustFirstName;
                    customerDC.LastName = abbRegistration.CustLastName;
                    customerDC.Email = abbRegistration.CustEmail;
                    customerDC.Address1 = abbRegistration.CustAddress1;
                    customerDC.Address2 = abbRegistration.CustAddress2;
                    customerDC.PhoneNumber = abbRegistration.CustMobile;
                    customerDC.state = abbRegistration.CustState;
                    customerDC.city = abbRegistration.CustCity;
                    customerDC.Pincode = abbRegistration.CustPinCode;
                    customerDC.RegdNo = abbRegistration.RegdNo;
                    customerDC.OrderDate = abbRegistration.UploadDateTime.ToString();
                    customerDC.SponserOrdrNumber = abbRegistration.SponsorOrderNo;


                    customerDC.IsDeffered = Convert.ToBoolean(aBBRedemption.IsDefferedSettelment);
                    if (customerDC.IsDeffered == true)
                    {
                        customerDC.productCost = aBBRedemption.FinalRedemptionValue.ToString();
                        if (aBBRedemption.RedemptionValue == null)
                        {
                            customerDC.Message = "Final Price is 0 for this order and case is deffered";
                        }
                    }
                    else
                    {
                        customerDC.productCost = 0.ToString();
                    }
                    //Remove this because Product Cost will be set from the OrderTrans.FinalPriceAfterQc
                    //customerDC.productCost = aBBRedemption.RedemptionValue.ToString();

                    if (string.IsNullOrEmpty(customerDC.Address1))
                    {
                        customerDC.Message = "customer address is not available for this order please check address details";
                    }
                    else if (string.IsNullOrEmpty(abbRegistration.CustMobile))
                    {
                        customerDC.PhoneNumber = "customer phone number is not provided";
                    }
                    else if (string.IsNullOrEmpty(abbRegistration.CustCity))
                    {
                        customerDC.Message = "customer city is not provided";
                    }
                    else if (string.IsNullOrEmpty(abbRegistration.CustPinCode))
                    {
                        customerDC.Message = "customer pincode is not provided";
                    }




                    //customerDC.IsDeffered = true;
                    TblProductType producttype = _productTypeRepository.GetSingle(x => x.Id == abbRegistration.NewProductCategoryTypeId);
                    if (producttype != null)
                    {
                        TblProductCategory productCategory = _productCategoryRepository.GetSingle(x => x.Id == producttype.ProductCatId);
                        if (productCategory != null)
                        {
                            customerDC.ProductCategory = productCategory.Description;
                            customerDC.ProductType = producttype.Description;
                        }
                        else
                        {
                            customerDC.Message = "Product category not found";
                        }
                    }
                    else
                    {
                        customerDC.Message = "product type not found";
                    }
                }
                else
                {
                    customerDC.Message = "Order details not found";

                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("TicketGenrateManager", "SetOrderDetailsObjectForABB", ex);
            }
            return customerDC;
        }

        #endregion
        #region Set SponserObject for customer detils
        public CustomerandOrderDetailsDataContract SetOrderDetailsObject(TblOrderTran transactionObj)
        {
            //_customerDetailsRepository = new CustomerDetailsRepository();
            //productCategoryRepository = new ProductCategoryRepository();
            //productTypeRepository = new ProductTypeRepository();
            CustomerandOrderDetailsDataContract customerDC = new CustomerandOrderDetailsDataContract();
            try
            {
                if (transactionObj != null)
                {
                    //TblCustomerDetail customerObj = _customerDetailsRepository.GetSingle(x => x.Id == exchangeOrder.CustomerDetailsId);
                    if (transactionObj.Exchange.CustomerDetailsId != null)
                    {
                        customerDC.FirstName = transactionObj.Exchange.CustomerDetails.FirstName;
                        customerDC.LastName = transactionObj.Exchange.CustomerDetails.LastName;
                        customerDC.Email = transactionObj.Exchange.CustomerDetails.Email;
                        customerDC.Address1 = transactionObj.Exchange.CustomerDetails.Address1;
                        if (customerDC.Address1 == null || customerDC.Address1 == "")
                        {
                            customerDC.Message = "customer address is not available for this order please check address details";
                        }
                        else if (string.IsNullOrEmpty(transactionObj.Exchange.CustomerDetails.PhoneNumber))
                        {
                            customerDC.Message = "customer phone number is not provided";
                        }
                        else if (string.IsNullOrEmpty(transactionObj.Exchange.CustomerDetails.City))
                        {
                            customerDC.Message = "customer city is not provided";
                        }
                        else if (string.IsNullOrEmpty(transactionObj.Exchange.CustomerDetails.ZipCode))
                        {
                            customerDC.Message = "customer pincode is not provided";
                        }

                        customerDC.Address2 = transactionObj.Exchange.CustomerDetails.Address2;
                        customerDC.PhoneNumber = transactionObj.Exchange.CustomerDetails.PhoneNumber;
                        customerDC.state = transactionObj.Exchange.CustomerDetails.State;
                        customerDC.city = transactionObj.Exchange.CustomerDetails.City;
                        customerDC.Pincode = transactionObj.Exchange.CustomerDetails.ZipCode;
                        customerDC.RegdNo = transactionObj.Exchange.RegdNo;
                        customerDC.OrderDate = transactionObj.Exchange.CreatedDate.ToString();
                        customerDC.SponserOrdrNumber = transactionObj.Exchange.SponsorOrderNumber;
                        customerDC.IsDeffered = Convert.ToBoolean(transactionObj.Exchange.IsDefferedSettlement);
                        if (transactionObj.Exchange.IsDefferedSettlement == true)
                        {
                            customerDC.productCost = transactionObj.Exchange.FinalExchangePrice.ToString();
                            if (transactionObj.Exchange.FinalExchangePrice == null)
                            {
                                customerDC.Message = "Final Price is 0 for this order and case is deffered";
                            }
                        }
                        else
                        {
                            customerDC.productCost = 0.ToString();
                        }
                        customerDC.IsDtoC = Convert.ToBoolean(transactionObj.Exchange.IsDtoC);
                        TblProductType producttype = _productTypeRepository.GetSingle(x => x.Id == transactionObj.Exchange.ProductTypeId);
                        if (producttype != null)
                        {
                            TblProductCategory productCategory = _productCategoryRepository.GetSingle(x => x.Id == producttype.ProductCatId);
                            if (productCategory != null)
                            {
                                customerDC.ProductCategory = productCategory.Description;
                                customerDC.ProductType = producttype.Description;
                            }
                            else
                            {
                                customerDC.Message = "Product category not found";
                            }
                        }
                        else
                        {
                            customerDC.Message = "product type not found";
                        }

                    }
                    else
                    {
                        customerDC.Message = "Customer details not found";

                    }


                }
                else
                {
                    customerDC.Message = "Order details not found";

                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("TicketGenrateManager", "SetOrderDetailsObject", ex);
            }

            return customerDC;
        }
        #endregion
        #region Set SponserObject for customer detils
        public EVCdetailsDataContract SetEvcDetailsDataContract(TblEvcregistration evcRegistration)
        {

            EVCdetailsDataContract EvcDataContract = new EVCdetailsDataContract();
            try
            {
                if (evcRegistration != null)
                {
                    EvcDataContract.EVCName = evcRegistration.BussinessName;
                    EvcDataContract.ContactPersonName = evcRegistration.ContactPerson;
                    EvcDataContract.Evc_Email = evcRegistration.EmailId;
                    EvcDataContract.Evc_PhoneNumber = evcRegistration.EvcmobileNumber;
                    EvcDataContract.Evc_AlternatePhoneNumber = evcRegistration.AlternateMobileNumber;
                    EvcDataContract.Evc_RegdNo = evcRegistration.EvcregdNo;
                    EvcDataContract.Evc_pincode = evcRegistration.PinCode;
                    if (evcRegistration.StateId > 0)
                    {
                        TblState stateObj = _stateRepository.GetSingle(x => x.StateId == evcRegistration.StateId);
                        if (stateObj != null)
                        {
                            EvcDataContract.Evc_state = stateObj.Name;
                        }
                    }
                    if (evcRegistration.CityId > 0)
                    {
                        TblCity cityObj = _cityRepository.GetSingle(x => x.CityId == evcRegistration.CityId);
                        if (cityObj != null)
                        {
                            EvcDataContract.Evc_city = cityObj.Name;
                        }
                    }
                    EvcDataContract.Evc_Address1 = evcRegistration.RegdAddressLine1;
                    EvcDataContract.Evc_Address2 = evcRegistration.RegdAddressLine2;

                    if (string.IsNullOrEmpty(EvcDataContract.Evc_Address1))
                    {
                        EvcDataContract.Message = "evc address 1 is null please provide evc address details";
                    }
                    else if (string.IsNullOrEmpty(EvcDataContract.ContactPersonName))
                    {
                        EvcDataContract.Message = "contact person name is not provided from evc please provide contact person name";
                    }
                    else if (string.IsNullOrEmpty(EvcDataContract.Evc_PhoneNumber))
                    {
                        EvcDataContract.Message = "EVC phone number is null please provide evc phone number";
                    }
                    else if (string.IsNullOrEmpty(EvcDataContract.Evc_city))
                    {
                        EvcDataContract.Message = "EVC city is null please provide city";
                    }
                    else if (string.IsNullOrEmpty(EvcDataContract.Evc_Email))
                    {
                        EvcDataContract.Message = "Evc email is null please provide email";
                    }
                }
                else
                {
                    EvcDataContract.Message = "Evc data not found";

                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("TicketGenrateManager", "SetEvcDetailsDataContract", ex);
            }
            return EvcDataContract;
        }
        #endregion
        #region Updated Create ticket for bizlog
        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>   
        public UpdatedTicketDataContract UpdatedSetTicketObjInfo(CustomerandOrderDetailsDataContract customerObj, EVCdetailsDataContract evcDataObj)
        {

            UpdatedTicketDataContract bizlogTicketObj = null;
            Product product = new Product();
            Primary primary = new Primary();
            List<Product> product1 = new List<Product>();


            try
            {

                //bizlogTicketObj.products = new List<Product>();
                if (customerObj != null && evcDataObj != null)
                {
                    bizlogTicketObj = new UpdatedTicketDataContract();
                    bizlogTicketObj.primary = new Primary();
                    bizlogTicketObj.primary.ticketPriority = !string.IsNullOrEmpty(customerObj.pickupPriority) ? customerObj.pickupPriority : "high";
                    bizlogTicketObj.primary.flowId = "PickAndDropOneWay";
                    bizlogTicketObj.primary.retailerId = "d0eb6dd5-2f8f-4e39-ae3e-50631201e122";
                    bizlogTicketObj.primary.retailerNo = evcDataObj.Evc_PhoneNumber;
                    bizlogTicketObj.primary.conComplaintNo = customerObj.RegdNo;
                    bizlogTicketObj.primary.ticketDetails = "Fragile Products, handle with care";
                    bizlogTicketObj.primary.isPhysicalEval = "no";
                    bizlogTicketObj.primary.orderNo = customerObj.RegdNo;
                    bizlogTicketObj.primary.isTechEval = "no";


                    //Data in list format to pass to bizlog 
                    //product details
                    product.primary = new Primary();
                    DateTime complaintDate = Convert.ToDateTime(customerObj.OrderDate);
                    string cDate = complaintDate.ToString("yyyy-MM-dd");
                    product.primary.productCode = customerObj.ProductCategory;
                    product.primary.productName = customerObj.ProductCategory + " " + customerObj.ProductType;
                    product.primary.dateOfPurchase = cDate;
                    product.primary.identificationNo = customerObj.RegdNo;
                    if (customerObj.IsDeffered == true)
                    {
                        product.primary.productValue = customerObj.productCost;
                        product.primary.cost = customerObj.productCost;
                        bizlogTicketObj.primary.cost = customerObj.productCost;
                        bizlogTicketObj.primary.productValue = customerObj.productCost;
                    }
                    else
                    {
                        product.primary.productValue = "0";
                        product.primary.cost = "0";
                        bizlogTicketObj.primary.cost = "0";
                        bizlogTicketObj.primary.productValue = "0";
                    }
                    //product.src_add to Add in bizlog
                    product.src_add = new SrcAdd();
                    product.src_add.srcAdd1 = customerObj.Address1;
                    product.src_add.srcAdd2 = customerObj.Address2;
                    product.src_add.srcCity = customerObj.city;
                    product.src_add.srcContact1 = customerObj.PhoneNumber;
                    product.src_add.srcContactPerson = customerObj.FirstName + " " + customerObj.LastName;
                    product.src_add.srcEmailId = customerObj.Email;
                    product.src_add.srcLandmark = customerObj.Address1;
                    product.src_add.srcLocation = customerObj.city;
                    product.src_add.srcPincode = customerObj.Pincode;
                    product.src_add.srcState = customerObj.state;

                    // Destination Address for Bizlog
                    product.dst_add = new DstAdd();
                    product.dst_add.dstAdd1 = evcDataObj.Evc_Address1;
                    product.dst_add.dstAdd2 = evcDataObj.Evc_Address2;
                    product.dst_add.dstContactPerson = evcDataObj.ContactPersonName;
                    product.dst_add.dstContact1 = evcDataObj.Evc_PhoneNumber;
                    product.dst_add.dstContact2 = evcDataObj.Evc_AlternatePhoneNumber;
                    product.dst_add.dstEmailId = evcDataObj.Evc_Email;
                    product.dst_add.dstLandmark = evcDataObj.BusinessName;
                    product.dst_add.dstPincode = evcDataObj.Evc_pincode;
                    product.dst_add.dstState = evcDataObj.Evc_state;
                    product.dst_add.dstCity = evcDataObj.Evc_city;

                    bizlogTicketObj.products = new List<Product>();
                    bizlogTicketObj.products.Add(product);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("TicketGenrateManager", "UpdatedSetTicketObjInfo", ex);
            }
            return bizlogTicketObj;
        }
        #endregion

        #region sync add ticket details New updated Integration
        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>   
        public UpdatedTicketResponceDataContract UpdatedProcessTicketInfo(UpdatedTicketDataContract updatedticketDataContract)
        {
            //    _ticketInformationCall = new TicketInformationCall();
            //    _bizlogCreateTicketSyncCall = new BizlogCreateTicketSyncCall();
            UpdatedTicketResponceDataContract TicketResponceDC = null;

            try
            {
                var code = updatedticketDataContract.primary.productCode;
                if (updatedticketDataContract != null)
                {
                    //Create bizlog ticket with API call
                    TicketResponceDC = AddUpdatedTicketToBizlog(updatedticketDataContract);
                    //get bizlogticketno as response and save it in local DB
                    //add status column in local DB
                    #region Code to add Ticket in database 
                    if (TicketResponceDC != null && TicketResponceDC.data != null)
                    {
                        if (TicketResponceDC.data.ticketNo != null && TicketResponceDC.success == true)
                        {
                            string BizlogTicketNo = TicketResponceDC.data.ticketNo;
                            AddUpdatedTicketToDB(updatedticketDataContract, BizlogTicketNo);
                        }
                    }
                    #endregion

                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("TicketGenrateManager", "SetEvcDetailsDataContract", ex);
            }

            return TicketResponceDC;
        }
        #endregion

        #region Add Updated Ticket in Bizlog
        /// <summary>
        /// Method to add the Ticket in Bizlog DB
        /// </summary>       
        /// <returns></returns>   
        public UpdatedTicketResponceDataContract AddUpdatedTicketToBizlog(UpdatedTicketDataContract UpdatedticketDataContract)
        {
            UpdatedTicketResponceDataContract TicketResponceDC = null;
            string responseString = string.Empty;
            string url = _config.Value.UpdatedCreateTicketUrl.ToString();
            try
            {
                if (UpdatedticketDataContract != null)
                {
                    //IRestResponse response = BizlogServiceCall.Rest_InvokeBizlogSeviceFormData(url, Method.POST, ticketDataContract);
                    responseString = Rest_InvokeUPdatedBlowHornServiceFormData(url, Method.Put, UpdatedticketDataContract);

                    if (responseString != null)
                    {
                        TicketResponceDC = JsonConvert.DeserializeObject<UpdatedTicketResponceDataContract>(responseString);

                    }
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("TicketGenrateManager", "SetEvcDetailsDataContract", ex);
            }
            return TicketResponceDC;
        }
        #endregion

        #region Add Updated Ticket in database
        /// <summary>
        /// Method to add the Ticket
        /// </summary>       
        /// <returns></returns>   
        public int AddUpdatedTicketToDB(UpdatedTicketDataContract updatedticketDataContract, string BizlogTicketNo)
        {
            //logisticsRepository = new LogisticsRepository();
            //ticketRepository = new BizlogTicketRepository();
            int result = 0;
            try
            {
                TblBizlogTicket ticketInfo = SetUpdatedTicketObjectDBJson(updatedticketDataContract, BizlogTicketNo);
                {
                    _bizlogTicketRepository.Create(ticketInfo);
                    _bizlogTicketRepository.SaveChanges();
                    result = ticketInfo.Id;

                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("TicketGenrateManager", "AddUpdatedTicketToDB", ex);
            }

            return result;
        }
        #endregion
        #region Bizlog service Call

        /// <summary>
        /// Method to POST form-data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string Rest_InvokeBizlogSeviceFormData(string url, Method methodType, object content = null, string ticketNo = null)
        {
            HttpResponseMessage getResponse = null;
            string jsonString = string.Empty;
            string responseString = string.Empty;
            string ApiToken = _config.Value.ApiToken.ToString();
            string RetailerId = _config.Value.RetailerId.ToString();
            try
            {

                HttpClient _httpclient = new HttpClient();
                MultipartFormDataContent multiPartStream = new MultipartFormDataContent();
                //multiPartStream.Add(new StringContent("31b26b73e64f56d6354eb632dafd7fe2"), "apiToken");
                //multiPartStream.Add(new StringContent("858c0120-7f10-4c46-b16f-9a3cb560561c"), "retailerId");
                multiPartStream.Add(new StringContent(ApiToken), "apiToken");
                multiPartStream.Add(new StringContent(RetailerId), "retailerId");
                if (ticketNo != null)
                {
                    multiPartStream.Add(new StringContent(ticketNo), "ticketNo");
                }
                if (content != null)
                {
                    jsonString = JsonConvert.SerializeObject(content);
                }

                multiPartStream.Add(new StringContent(jsonString), "fields");
                HttpRequestMessage requesttest = new HttpRequestMessage(HttpMethod.Post, url);
                requesttest.Content = multiPartStream;

                HttpCompletionOption option = HttpCompletionOption.ResponseContentRead;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                getResponse = _httpclient.SendAsync(requesttest, option).Result;
                if (getResponse.Content != null && getResponse.Content.ReadAsStringAsync().Result != null)
                {
                    responseString = getResponse.Content.ReadAsStringAsync().Result;
                }
                //getResponse = client.Execute(request);

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("TicketGenrateManager", "SetEvcDetailsDataContract", ex);
            }
            return responseString;

        }
        #endregion

        #region Updated Bizlog service Call

        /// <summary>
        /// Method to PUT form-data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string Rest_InvokeUPdatedBlowHornServiceFormData(string url, Method methodType, object content = null, string ticketNo = null)
        {
            RestResponse getResponse = null;
            string jsonString = string.Empty;
            string responseString = string.Empty;
            string ApiKey = _config.Value.TokenForBizLog.ToString();
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest();
                request.Method = methodType;
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", ApiKey);
                if (content != null)
                {
                    jsonString = JsonConvert.SerializeObject(content);
                    request.AddJsonBody(jsonString);
                }
                getResponse = client.Execute(request);
                responseString = getResponse.Content;

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("TicketGenrateManager", "Rest_InvokeUPdatedBlowHornServiceFormData", ex);
            }
            return responseString;

        }
        #endregion

        #region set Add Ticket obj
        /// <summary>
        /// Method to set Ticket info to table
        /// </summary>
        /// <param name="ticketDataContract">ticketDataContract</param>     
        public TblBizlogTicket SetUpdatedTicketObjectDBJson(UpdatedTicketDataContract ticketDataContract, string BizlogTicketNo)
        {
            TblBizlogTicket ticketObj = null;
            try
            {
                if (ticketDataContract != null)
                {
                    ticketObj = new TblBizlogTicket();
                    if (BizlogTicketNo != null)
                    {
                        ticketObj.BizlogTicketNo = BizlogTicketNo;
                    }
                    //add sponsor order id
                    ticketObj.SponsrorOrderNo = ticketDataContract.primary.orderNo;
                    if (ticketDataContract.products.Count > 0)
                    {
                        foreach (var item in ticketDataContract.products)
                        {
                            ticketObj.ConsumerName = item.src_add.srcContactPerson;
                            ticketObj.AddressLine1 = item.src_add.srcAdd1;
                            ticketObj.AddressLine2 = item.src_add.srcAdd2;
                            ticketObj.City = item.src_add.srcCity;
                            ticketObj.Pincode = item.src_add.srcPincode;
                            ticketObj.TelephoneNumber = item.src_add.srcContact1;
                            ticketObj.RetailerPhoneNo = item.dst_add.dstContact1;
                            ticketObj.AlternateTelephoneNumber = item.src_add.srcContact2;
                            ticketObj.EmailId = item.src_add.srcEmailId;
                            ticketObj.DateOfComplaint = _currentDatetime.ToString();
                            ticketObj.NatureOfComplaint = string.Empty;
                            ticketObj.IsUnderWarranty = item.primary.isUnderWarranty;
                            ticketObj.Brand = item.primary.brandName;
                            ticketObj.ProductCategory = item.primary.productCode;
                            ticketObj.ProductName = item.primary.productName;
                            ticketObj.ProductCode = item.primary.productCode;
                            ticketObj.Model = item.primary.modelName;
                            ticketObj.IdentificationNo = item.primary.identificationNo;
                            ticketObj.DropLocation = item.dst_add.dstLocation;
                            ticketObj.DropLocAddress1 = item.dst_add.dstAdd1;
                            ticketObj.DropLocAddress2 = item.dst_add.dstAdd2;
                            ticketObj.DropLocCity = item.dst_add.dstCity;
                            ticketObj.DropLocState = item.dst_add.dstState;
                            ticketObj.DropLocPincode = item.dst_add.dstPincode;
                            ticketObj.DropLocContactPerson = item.dst_add.dstContactPerson;
                            ticketObj.DropLocContactNo = item.dst_add.dstContact1;
                            ticketObj.DropLocAlternateNo = item.dst_add.dstContact2;
                            ticketObj.PhysicalEvaluation = item.primary.isPhysicalEval;
                            ticketObj.TechEvalRequired = item.primary.isTechEval;
                            ticketObj.Value = item.primary.cost;
                            ticketObj.CreatedDate = _currentDatetime;
                            ticketObj.IsActive = true;
                            ticketObj.DateOfPurchase = item.primary.dateOfPurchase;
                            ticketObj.TicketPriority = item.primary.ticketPriority;
                        }
                    }
                    ticketObj.ConsumerComplaintNumber = ticketDataContract.primary.conComplaintNo;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("TicketGenrateManager", "SetUpdatedTicketObjectDBJson", ex);
            }
            return ticketObj;
        }
        #endregion

        #region Add Data To Logistics Table Modified by VK for ABB Redumption Date 19-June-2023
        public int AddTicketToTblLogistics(string ticketNumber, int transId, int servicepartnerId, string regdno, CustomerandOrderDetailsDataContract customerDetails, int userId, int evcRegistrationId)
        {
            #region Variable Declaration
            int LogisticsId = 0;
            int typeOfOrder = 0;
            TblOrderTran transobj = null;
            TblExchangeOrder exchangeObj = new TblExchangeOrder();
            TblProductCategory tblProductCategory = new TblProductCategory();
            TblWalletTransaction wallettramsactionObj = new TblWalletTransaction();
            TblProductType tblProductType = new TblProductType();
            TblAbbredemption abbRedemption = new TblAbbredemption();
            TblAbbregistration abbregistration = new TblAbbregistration();
            TblExchangeAbbstatusHistory ordersatatusObj = null;
            TblBuconfiguration tblBuconfiguration = null;
            int setStatusId = Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated);
            string setStatusDesc = EnumHelper.DescriptionAttr(OrderStatusEnum.LogisticsTicketUpdated);
            #endregion

            try
            {
                if (ticketNumber != null && regdno != null && transId > 0 && servicepartnerId > 0)
                {
                    var IsPickupSkiped = _commonManager.CheckBUCongigByKey(transId, BUConfigKeyEnum.IsPickupSkiped.ToString());
                    if (IsPickupSkiped)
                    {
                        setStatusId = Convert.ToInt32(OrderStatusEnum.LGCPickup);
                        setStatusDesc = EnumHelper.DescriptionAttr(OrderStatusEnum.LGCPickup);
                    }
                    TblOrderQc tblOrderQc = _context.TblOrderQcs.Where(x => x.OrderTransId == transId).FirstOrDefault();

                    TblLogistic logistic = new TblLogistic();
                    logistic.TicketNumber = ticketNumber;
                    logistic.RegdNo = regdno;
                    logistic.CreatedBy = userId;
                    logistic.CreatedDate = DateTime.Now;
                    logistic.ServicePartnerId = servicepartnerId;
                    logistic.IsActive = true;
                    logistic.OrderTransId = transId;
                    var logisticRS = _context.TblLogistics.Where(x => x.IsActive == false && x.Rescheduledate != null && x.OrderTransId == transId).ToList().LastOrDefault();
                    if (logisticRS != null)
                    {
                        logistic.PickupScheduleDate = (DateTime?)logisticRS.Rescheduledate;
                    }
                    else
                    {
                        TblOrderQc tblOrderQc1 = _context.TblOrderQcs.Where(x => x.OrderTransId == transId).FirstOrDefault();
                        if (tblOrderQc1 != null && tblOrderQc1.PreferredPickupDate != null)
                        {
                            logistic.PickupScheduleDate = tblOrderQc1.PreferredPickupDate;
                        }
                        else
                        {
                            logistic.PickupScheduleDate = DateTime.Now;
                        }
                    }
                    logistic.AmtPaybleThroughLgc = Convert.ToDecimal(customerDetails.productCost);
                    logistic.StatusId = setStatusId;
                    _logisticsRepository.Create(logistic);
                    _logisticsRepository.SaveChanges();

                    LogisticsId = logistic.LogisticId;
                    if (LogisticsId > 0)
                    {
                        TblOrderTran tblOrderTrans = _orderTransRepository.UpdateStatusOnBaseTblsByTransId(transId, setStatusId, userId, setStatusDesc);

                        if (tblOrderTrans != null)
                        {
                            typeOfOrder = (int)(tblOrderTrans?.OrderType);
                            var result = _walletTransactionRepository.UpdateWalletTransRecordStatus(transId, setStatusId, userId, typeOfOrder);

                            string sponsorOrderNum = null; int? customerId = null; string regdNo = null;
                            if (typeOfOrder == Convert.ToInt32(OrderTypeEnum.Exchange))
                            {
                                sponsorOrderNum = tblOrderTrans.Exchange?.SponsorOrderNumber;
                                customerId = Convert.ToInt32(tblOrderTrans.Exchange?.CustomerDetailsId);
                                regdNo = tblOrderTrans.Exchange?.RegdNo;
                            }
                            else if (tblOrderTrans.Abbredemption != null)
                            {
                                customerId = tblOrderTrans.Abbredemption?.CustomerDetailsId;
                                regdNo = tblOrderTrans?.Abbredemption?.RegdNo;
                            }
                            if (IsPickupSkiped)
                            {
                                bool IsPickUpDone = PickUpDoneOnTicketGenerate(regdNo, userId);
                            }
                            #region Manage History Modified by VK
                            ordersatatusObj = new TblExchangeAbbstatusHistory();
                            ordersatatusObj.CreatedDate = DateTime.Now;
                            ordersatatusObj.OrderType = typeOfOrder;
                            ordersatatusObj.RegdNo = regdNo;
                            ordersatatusObj.SponsorOrderNumber = sponsorOrderNum;
                            ordersatatusObj.StatusId = setStatusId;
                            ordersatatusObj.OrderTransId = transId;
                            ordersatatusObj.CustId = customerId;
                            ordersatatusObj.CreatedBy = userId;
                            ordersatatusObj.IsActive = true;
                            ordersatatusObj.CreatedDate = DateTime.Now;
                            ordersatatusObj.Evcid = evcRegistrationId;
                            ordersatatusObj.ServicepartnerId = servicepartnerId;
                            _commonManager.InsertExchangeAbbstatusHistory(ordersatatusObj);
                            #endregion
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("TicketGenrateManager", "AddTicketToTblLogistics", ex);
            }
            return LogisticsId;
        }

        #endregion


        #region Submit request for Service
        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>   
        public MahindraLogisticsResponseDataContract ProcessLogisticsRequest(MahindraLogisticsDataContract mahindraDC)
        {


            MahindraLogisticsResponseDataContract _mahindraResponseDC = null;
            try
            {
                if (mahindraDC != null)
                {
                    _mahindraResponseDC = PlaceSingleOrder(mahindraDC);
                    if (_mahindraResponseDC != null)
                    {
                        #region Code to Add Submit request to Database
                        if (_mahindraResponseDC.awbNumber != null /*&& _mahindraResponseDC.status != null*/)
                        {
                            string BookingId = _mahindraResponseDC.awbNumber.ToString();
                            AddMahindraRequestToDB(mahindraDC, BookingId);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("MahindraLogisticsSyncManager", "ProcessServiceRequest", ex);
            }
            return _mahindraResponseDC;
        }
        #endregion

        #region Set SubmitRequest Object
        public MahindraLogisticsDataContract SetMahindraObj(CustomerandOrderDetailsDataContract customerObj, EVCdetailsDataContract evcObj)
        {

            MahindraLogisticsDataContract mahindraObj = null;
            LineItem lineItems = new LineItem();

            //tblExchangeOrder Exchangeobj = null;
            //tblBusinessPartner businessPartnerObj = null;
            //tblProductType productTypeObj = null;
            //tblProductCategory categoryObj = null;

            try
            {

                mahindraObj = new MahindraLogisticsDataContract();
                mahindraObj.line_items = new List<LineItem>();
                mahindraObj.pickUpAddress = new PickUpAddress();
                mahindraObj.address = new Address();
                if (customerObj.FirstName != null && customerObj.LastName != null)
                {
                    mahindraObj.pickUpAddress.first_name = customerObj.FirstName;  //Mandatory
                    mahindraObj.pickUpAddress.last_name = customerObj.LastName;
                }
                mahindraObj.pickUpAddress.city = customerObj.city;                //Mandatory
                mahindraObj.pickUpAddress.state = customerObj.state;   //Manadatory
                mahindraObj.pickUpAddress.street_address = customerObj.Address1 + " " + customerObj.Address2;                  //Mandatory
                mahindraObj.pickUpAddress.telephone = customerObj.PhoneNumber;  //Mandatory
                mahindraObj.pickUpAddress.zipcode = customerObj.Pincode;
                mahindraObj.pickUpAddress.email = customerObj.Email;
                mahindraObj.client_order_id = customerObj.RegdNo;
                mahindraObj.address.first_name = evcObj.ContactPersonName;
                mahindraObj.address.last_name = evcObj.ContactPersonName;
                mahindraObj.address.city = string.IsNullOrEmpty(evcObj.Evc_city) ? evcObj.Evc_city : evcObj.Evc_city;
                mahindraObj.address.state = string.IsNullOrEmpty(evcObj.Evc_state) ? evcObj.Evc_state : evcObj.Evc_state;
                mahindraObj.address.street_address = string.IsNullOrEmpty(evcObj.Evc_Address1) ? evcObj.Evc_Address1 + " , " + evcObj.Evc_Address2 : evcObj.Evc_Address1 + " , " + evcObj.Evc_Address2;
                mahindraObj.address.telephone = evcObj.Evc_PhoneNumber;
                mahindraObj.address.zipcode = string.IsNullOrEmpty(evcObj.Evc_pincode) ? evcObj.Evc_pincode : evcObj.Evc_pincode;
                mahindraObj.address.email = evcObj.Evc_Email;


                DateTime myDate = DateTime.Now.AddHours(24);
                mahindraObj.order_delivery_date = myDate.ToString("yyyy-MM-dd");
                mahindraObj.quantity = 1;
                lineItems.sku = customerObj.RegdNo;
                lineItems.name = customerObj.ProductCategory + " , " + customerObj.ProductType;
                if (customerObj.IsDeffered == true)
                {
                    lineItems.price_per_each_item = customerObj.productCost;
                    lineItems.total_price = customerObj.productCost != null ? Convert.ToDouble(customerObj.productCost) : Convert.ToDouble(customerObj.productCost);
                    mahindraObj.total_price = customerObj.productCost != null ? Convert.ToDouble(customerObj.productCost) : Convert.ToDouble(customerObj.productCost);
                }
                else
                {
                    lineItems.price_per_each_item = "0";
                    lineItems.total_price = 0;
                    mahindraObj.total_price = 0;
                }
                lineItems.quantity = 1;
                mahindraObj.line_items.Add(lineItems);


            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("MahindraLogisticsSyncManager", "SetMahindraObj", ex);
            }

            return mahindraObj;
        }
        #endregion

        #region SubmitRequest in 247Around 
        /// <summary>
        /// Method to  PlaceSingleOrder in Mahindra(Whizzard)  DB
        /// </summary>       
        /// <returns></returns>   
        public MahindraLogisticsResponseDataContract PlaceSingleOrder(MahindraLogisticsDataContract mahindraDataContract)
        {
            MahindraLogisticsResponseDataContract mahindraResponseDC = null;
            string responseString = string.Empty;
            string url = _config.Value.apiURl.ToString();
            try
            {
                if (mahindraDataContract != null)
                {

                    //IRestResponse response = BizlogServiceCall.Rest_InvokeBizlogSeviceFormData(url, Method.POST, ticketDataContract);
                    responseString = Rest_InvokeMahindraServiceFormData(url, Method.Post, mahindraDataContract);
                    if (responseString != null)
                    {

                        mahindraResponseDC = JsonConvert.DeserializeObject<MahindraLogisticsResponseDataContract>(responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("MahindraInformationCall", "PlaceSingleOrder", ex);
            }

            return mahindraResponseDC;
        }
        #endregion

        #region Add SubmitRequest in database
        /// <summary>
        /// Method to add the Ticket
        /// </summary>       
        /// <returns> </returns>   
        public int AddMahindraRequestToDB(MahindraLogisticsDataContract mahindraLogisticsDataContract, string mahindraLogisticsawbNo)
        {
            //mahindraLogisticsRepository = new MahindraLogisticsRepository();
            int result = 0;
            try
            {
                //tblMahindraLogistic mahindraObject = SetMahindraObjectDBJson(mahindraLogisticsDataContract, mahindraLogisticsawbNo);
                //{
                //    mahindraLogisticsRepository.Add(mahindraObject);
                //    mahindraLogisticsRepository.SaveChanges();
                //    result = mahindraObject.Id;
                //}
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("SubmitRequestSyncCall", "AddSubmitRequestToDB", ex);
            }

            return result;
        }
        #endregion

        #region set Add SubmitRequest obj
        /// <summary>
        /// Method to set Ticket info to table
        /// </summary>
        /// <param name="aroundDataContract">aroundDataContract</param>     
        public TblMahindraLogistic SetMahindraObjectDBJson(MahindraLogisticsDataContract mahindraLogisticsDataContract, string mahindraLogisticsawbNo)
        {
            TblMahindraLogistic mahindraObject = null;
            try
            {
                if (mahindraLogisticsDataContract != null)
                {
                    mahindraObject = new TblMahindraLogistic();
                    if (mahindraLogisticsawbNo != null)
                    {
                        mahindraObject.AwbNumber = mahindraLogisticsawbNo;
                    }
                    mahindraObject.FirstName = mahindraLogisticsDataContract.address.first_name;
                    mahindraObject.LastName = mahindraLogisticsDataContract.address.last_name; ;
                    mahindraObject.City = mahindraLogisticsDataContract.address.city;
                    mahindraObject.State = mahindraLogisticsDataContract.address.state;
                    mahindraObject.Zipcode = mahindraLogisticsDataContract.address.zipcode;
                    mahindraObject.StreetAddress = mahindraLogisticsDataContract.address.street_address;
                    mahindraObject.Telephone = mahindraLogisticsDataContract.address.telephone;
                    mahindraObject.Email = mahindraLogisticsDataContract.address.email;
                    mahindraObject.FirstNamePickup = mahindraLogisticsDataContract.pickUpAddress.first_name;
                    mahindraObject.LastNamePickup = mahindraLogisticsDataContract.pickUpAddress.last_name;
                    mahindraObject.ZipcodePickup = mahindraLogisticsDataContract.pickUpAddress.zipcode;
                    mahindraObject.StreetAddressPickup = mahindraLogisticsDataContract.pickUpAddress.street_address;
                    mahindraObject.CityPickup = mahindraLogisticsDataContract.pickUpAddress.city;
                    mahindraObject.StatePickup = mahindraLogisticsDataContract.pickUpAddress.state;
                    mahindraObject.EmailPickup = mahindraLogisticsDataContract.pickUpAddress.email;
                    //mahindraObject.longitude_pickup = mahindraLogisticsDataContract.pickUpAddress.location.longitude.ToString();
                    //mahindraObject.latitude_pickup = mahindraLogisticsDataContract.pickUpAddress.location.latitude.ToString();
                    mahindraObject.TelephonePickup = mahindraLogisticsDataContract.pickUpAddress.telephone;
                    mahindraObject.ClientOrderId = mahindraLogisticsDataContract.client_order_id;
                    mahindraObject.TotalPrice = Convert.ToDecimal(mahindraLogisticsDataContract.total_price);
                    mahindraObject.Quantity = mahindraLogisticsDataContract.quantity;


                    //add sponsor order id
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("SubmitRequestSyncCall", "Set247AroundObjectDBJson", ex);
            }
            return mahindraObject;
        }
        #endregion

        #region Mahindra Service Call

        /// <summary>
        /// Method to POST form-data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string Rest_InvokeMahindraServiceFormData(string url, Method methodType, object content = null, string ticketNo = null)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            RestResponse getResponse = null;
            string jsonString = string.Empty;
            string responseString = string.Empty;
            string apiKey = _config.Value.apiKey.ToString();
            try
            {

                var client = new RestClient(url);
                var request = new RestRequest();
                request.Method = methodType;
                request.AddHeader("apiKey", apiKey);
                request.AddHeader("content-type", "application/json");
                if (content != null)
                {
                    jsonString = JsonConvert.SerializeObject(content);
                    request.AddJsonBody(jsonString);
                }
                getResponse = client.Execute(request);
                responseString = getResponse.Content;

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ServiceCall URL: " + url, "Rest_InvokeMahindraServiceFormData", ex);
            }
            return responseString;

        }
        #endregion

        #region Create Ticket With Bizlog API
        public HttpResponseMessage CreateTicketWithBizlog(string RegdNo, string priority, int servicePartnerId, int userid)
        {
            #region Variable Declaration
            HttpResponseMessage response = null;
            EvcApprovedData evcApprovedObj = new EvcApprovedData();
            evcApprovedObj = new EvcApprovedData();
            UpdatedTicketDataContract updatedTicketDataContract = null;
            UpdatedTicketResponceDataContract updatedTicketResponseDC = null;
            RunningBalance runingbalancecalculation = new RunningBalance();
            EVCdetailsDataContract evcdetailsDC = new EVCdetailsDataContract();
            CustomerandOrderDetailsDataContract customerDetails = new CustomerandOrderDetailsDataContract();
            TicketDataContract ticketDataContract = new TicketDataContract();
            decimal TotalofInprogress = 0;
            decimal TotalofDeliverd = 0;
            decimal EvcWallletAmount = 0;
            TblAbbredemption abbredemptionObj = null;
            #endregion

            try
            {

                //point 1 >> get zoho sponsor order form by id (zohoSalesOrderid)
                TblOrderTran transactionObj = _orderTransRepository.GetRegdnoinTicketG(RegdNo);
                //point 2 >> Fill the TicketDataContract object using the response from point 1 
                if (transactionObj != null)
                {
                    TblLogistic lgcRecordObj = _logisticsRepository.GetSingle(x => x.RegdNo.ToLower() == RegdNo.ToLower() && x.IsActive == true);
                    //Ticket should not be generated already
                    if (lgcRecordObj == null)
                    {
                        if (transactionObj.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                        {
                            if (transactionObj.Exchange != null)
                            {
                                customerDetails = SetOrderDetailsObject(transactionObj);
                            }
                        }
                        else
                        {
                            abbredemptionObj = _aBBRedemptionRepository.GetRedemptionData(transactionObj.AbbredemptionId);
                            if (transactionObj.AbbredemptionId != null)
                            {
                                customerDetails = SetOrderDetailsObjectForABB(abbredemptionObj);
                            }
                        }
                        if (customerDetails.Message == null || customerDetails.Message == "")
                        {
                            // get EVC Call allocation report by sponserID for Drop customer details
                            TblWalletTransaction EVCassigned = _walletTransactionRepository.GetSingle(x => x.OrderTransId == transactionObj.OrderTransId);
                            if (EVCassigned != null)
                            {
                                //Get evc data from tblevcRegistration
                                TblEvcregistration evcRegistrartion = _eVCRepository.GetSingle(x => x.EvcregistrationId == EVCassigned.EvcregistrationId && x.Isevcapprovrd == true);

                                if (evcRegistrartion != null)
                                {
                                    evcdetailsDC = SetEvcDetailsDataContract(evcRegistrartion);
                                    //List<TblWalletTransaction> walletsummary = _walletTransactionRepository.GetList(x => x.EvcregistrationId == EVCassigned.EvcregistrationId && x.StatusId != Convert.ToInt32(OrderStatusEnum.PickupDecline).ToString()).ToList();
                                    TblCreditRequest? tblCreditRequest = _context.TblCreditRequests.Where(x => x.WalletTransactionId == EVCassigned.WalletTransactionId &&  x.IsCreditRequest == true && x.IsActive == true).FirstOrDefault();
                                    bool isBlanaceTrue = true;
                                    if (tblCreditRequest != null && tblCreditRequest.IsCreditRequestApproved == true)
                                    {
                                        isBlanaceTrue = true;
                                    }
                                    else if(tblCreditRequest != null && tblCreditRequest.IsCreditRequestApproved == false)
                                    {
                                        isBlanaceTrue = false;
                                    }
                                    else
                                    {
                                        #region Calculate clear balance
                                        List<TblWalletTransaction> walletsummary = _walletTransactionRepository.GetList(x => x.EvcregistrationId == EVCassigned.EvcregistrationId && x.IsActive == true && x.StatusId != Convert.ToInt32(OrderStatusEnum.PickupDecline).ToString() && x.StatusId != Convert.ToInt32(OrderStatusEnum.PickupTicketcancellationbyUTC).ToString() && x.StatusId != Convert.ToInt32(OrderStatusEnum.ReopenforLogistics).ToString()).ToList();
                                        ///
                                        foreach (var items in walletsummary)
                                        {
                                            if (items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate == null && items.OrderOfCompleteDate == null)
                                            {
                                                if (items.OrderAmount != null)
                                                {
                                                    TotalofInprogress = TotalofInprogress + Convert.ToDecimal(items.OrderAmount);

                                                }
                                                // tblWalletTransaction.OrderAmount += items.OrderAmount;
                                            }
                                            if (items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate != null && items.OrderOfCompleteDate == null)
                                            {
                                                if (items.OrderAmount != null)
                                                {
                                                    TotalofDeliverd = TotalofDeliverd + Convert.ToDecimal(items.OrderAmount);
                                                }
                                            }
                                        }
                                        runingbalancecalculation.TotalofInprogress = TotalofInprogress;
                                        runingbalancecalculation.TotalofDeliverd = TotalofDeliverd;
                                        EvcWallletAmount = Convert.ToDecimal(evcRegistrartion.EvcwalletAmount);
                                        runingbalancecalculation.runningBalance = EvcWallletAmount - (runingbalancecalculation.TotalofInprogress + runingbalancecalculation.TotalofDeliverd);
                                        ///


                                        if (transactionObj.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                                        {
                                            if (transactionObj.Exchange != null)
                                            {
                                                // .Exchange.FinalExchangePrice i have changed this to tblWalletTransaction. Order Amount becouse Primi product case not handal 

                                                if (EVCassigned.StatusId == "22" || EVCassigned.StatusId == "21" || EVCassigned.StatusId == "44")
                                                {
                                                    if (EVCassigned.StatusId == "44" || EVCassigned.StatusId == "21")
                                                    {
                                                        if (Convert.ToInt32(runingbalancecalculation.runningBalance) < 0)
                                                        {
                                                            isBlanaceTrue = false;
                                                        }
                                                        else
                                                        {
                                                            isBlanaceTrue = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        isBlanaceTrue = true;
                                                    }

                                                }
                                                else
                                                {
                                                    if (Convert.ToInt32(runingbalancecalculation.runningBalance) >= EVCassigned.OrderAmount)
                                                        isBlanaceTrue = true;
                                                    else
                                                        isBlanaceTrue = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (transactionObj.AbbredemptionId != null)
                                            {
                                                if (EVCassigned.StatusId == "22" || EVCassigned.StatusId == "21" || EVCassigned.StatusId == "44")
                                                {
                                                    if (EVCassigned.StatusId == "44" || EVCassigned.StatusId == "21")
                                                    {
                                                        if (Convert.ToInt32(runingbalancecalculation.runningBalance) < 0)
                                                        {
                                                            isBlanaceTrue = false;
                                                        }
                                                        else
                                                        {
                                                            isBlanaceTrue = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        isBlanaceTrue = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (Convert.ToInt32(runingbalancecalculation.runningBalance) >= EVCassigned.OrderAmount)
                                                        isBlanaceTrue = true;
                                                    else
                                                        isBlanaceTrue = false;
                                                };
                                            }
                                        }
                                        #endregion
                                    }
                                    if (evcRegistrartion != null && isBlanaceTrue)
                                    {
                                        // set value in bizlog ticket object
                                        customerDetails.pickupPriority = priority;
                                        updatedTicketDataContract = UpdatedSetTicketObjInfo(customerDetails, evcdetailsDC);

                                        if (updatedTicketDataContract != null)
                                        {
                                            //point 3 >> Create the tciket in bizlog and local DB (already happening)              
                                            updatedTicketResponseDC = UpdatedProcessTicketInfo(updatedTicketDataContract);
                                            //point To Add Ticket data into logistics table and update status in wallet as well as status history table and exchange order table
                                            int ticketid = 0;
                                            if (updatedTicketResponseDC != null && updatedTicketResponseDC.data != null)
                                            {
                                                ticketid = AddTicketToTblLogistics(updatedTicketResponseDC.data.ticketNo, transactionObj.OrderTransId, servicePartnerId, transactionObj.RegdNo, customerDetails, userid, evcRegistrartion.EvcregistrationId);
                                            }
                                            //point 4 >> update the zoho sponsor order form by ticket id received from bizlog response
                                            if (updatedTicketResponseDC != null)
                                            {
                                                if (updatedTicketResponseDC.success == true)
                                                {
                                                    if (updatedTicketResponseDC.data != null && updatedTicketResponseDC.data.ticketNo != null)
                                                    {
                                                        StatusDataContract structObj = new StatusDataContract(true, "Success", updatedTicketResponseDC);
                                                        response = new HttpResponseMessage(HttpStatusCode.OK)
                                                        {
                                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                        };
                                                    }
                                                }
                                                else
                                                {
                                                    StatusDataContract structObj = new StatusDataContract(false, "Error Requesting Bizlog Unable to Create ticket", updatedTicketResponseDC);
                                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                                    {
                                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                    };
                                                }
                                            }
                                            else
                                            {
                                                StatusDataContract structObj = new StatusDataContract(false, "Error Requesting Bizlog Unable to Create ticket", updatedTicketResponseDC);
                                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                                {
                                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                };
                                            }
                                        }
                                        //}
                                        else
                                        {
                                            StatusDataContract structObj = new StatusDataContract(false, "Invalid model request", null);
                                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                            {
                                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                            };
                                        }

                                    }
                                    else
                                    {
                                        StatusDataContract structObj = new StatusDataContract(false, evcApprovedObj.Evc_Name + ": does not have sufficient running balance i.e.", null);
                                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                    }
                                }
                                else
                                {
                                    StatusDataContract structObj = new StatusDataContract(false, "EVC Data Not Foud");
                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                            }

                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(false, "Evc not allocated to this order");
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }
                        }
                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(false, customerDetails.Message);
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                    }
                    else
                    {
                        StatusDataContract structObj = new StatusDataContract(false, "Logistics ticket already generated for this order " + lgcRecordObj.TicketNumber);
                        response = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };
                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Order not found.");
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
            }
            catch (Exception ex)
            {

                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }
            return response;
        }
        #endregion

        #region Generate Ticket for Local LGC Partner
        /// <summary>
        /// Generate Ticket for Local LGC Partner
        /// </summary>
        /// <param name="RegdNo"></param>
        /// <param name="servicePartnerId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public HttpResponseMessage GenerateTicketForLocalLgcPartner(string RegdNo, int servicePartnerId, int userid)
        {
            //TicketSyncManager ticketSyncManager = new TicketSyncManager();
            HttpResponseMessage response = null;
            RunningBalance runingbalancecalculation = new RunningBalance();
            EVCdetailsDataContract evcdetailsDC = new EVCdetailsDataContract();
            CustomerandOrderDetailsDataContract customerDetails = new CustomerandOrderDetailsDataContract();
            decimal TotalofInprogress = 0;
            decimal TotalofDeliverd = 0;
            decimal EvcWallletAmount = 0;
            TblAbbredemption abbredemptionObj = null;
            try
            {
                //point 1 >> get zoho sponsor order form by id (zohoSalesOrderid)
                TblOrderTran transactionObj = _orderTransRepository.GetRegdnoinTicketG(RegdNo);
                //point 2 >> Fill the TicketDataContract object using the response from point 1 
                if (transactionObj != null)
                {

                    TblLogistic lgcRecordObj = _logisticsRepository.GetSingle(x => x.RegdNo.ToLower() == RegdNo.ToLower() && x.IsActive == true);
                    //Ticket should not be generated already
                    if (lgcRecordObj == null)
                    {
                        if (transactionObj.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                        {
                            if (transactionObj.Exchange != null)
                            {
                                customerDetails = SetOrderDetailsObject(transactionObj);
                            }
                        }
                        else
                        {
                            abbredemptionObj = _aBBRedemptionRepository.GetRedemptionData(transactionObj.AbbredemptionId);
                            if (abbredemptionObj != null)
                            {
                                customerDetails = SetOrderDetailsObjectForABB(abbredemptionObj);
                                //if (customerDetails != null)
                                //{
                                //    customerDetails.productCost = transactionObj.FinalPriceAfterQc.ToString();
                                //}
                            }
                        }
                        if (customerDetails.Message == null || customerDetails.Message == "")
                        {
                            // get EVC Call allocation report by sponserID for Drop customer details

                            TblWalletTransaction EVCassigned = _walletTransactionRepository.GetSingle(x => x.OrderTransId == transactionObj.OrderTransId);
                            if (EVCassigned != null)
                            {
                                //Get evc data from tblevcRegistration
                                TblEvcregistration evcRegistrartion = _eVCRepository.GetSingle(x => x.EvcregistrationId == EVCassigned.EvcregistrationId && x.Isevcapprovrd == true && x.IsActive == true);

                                if (evcRegistrartion != null)
                                {
                                    evcdetailsDC = SetEvcDetailsDataContract(evcRegistrartion);
                                    //  List<TblWalletTransaction> walletsummary = _walletTransactionRepository.GetList(x => x.EvcregistrationId == EVCassigned.EvcregistrationId && x.StatusId != Convert.ToInt32(OrderStatusEnum.PickupDecline).ToString() ).ToList();
                                    TblCreditRequest? tblCreditRequest = _context.TblCreditRequests.Where(x => x.WalletTransactionId == EVCassigned.WalletTransactionId && x.IsCreditRequest == true && x.IsActive == true).FirstOrDefault();
                                    bool isBlanaceTrue = true;
                                    if (tblCreditRequest != null && tblCreditRequest.IsCreditRequestApproved == true)
                                    {
                                        isBlanaceTrue = true;
                                    }
                                    else if (tblCreditRequest != null && tblCreditRequest.IsCreditRequestApproved == false)
                                    {
                                        isBlanaceTrue = false;
                                    }
                                    else
                                    {
                                        #region calculate clear balance
                                        List<TblWalletTransaction> walletsummary = _walletTransactionRepository.GetList(x => x.EvcregistrationId == EVCassigned.EvcregistrationId && x.IsActive == true && x.StatusId != Convert.ToInt32(OrderStatusEnum.PickupDecline).ToString() && x.StatusId != Convert.ToInt32(OrderStatusEnum.PickupTicketcancellationbyUTC).ToString() && x.StatusId != Convert.ToInt32(OrderStatusEnum.ReopenforLogistics).ToString()).ToList();

                                        ///
                                        foreach (var items in walletsummary)
                                        {
                                            if (items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate == null && items.OrderOfCompleteDate == null)
                                            {
                                                if (items.OrderAmount != null)
                                                {
                                                    TotalofInprogress = TotalofInprogress + Convert.ToDecimal(items.OrderAmount);

                                                }
                                                // tblWalletTransaction.OrderAmount += items.OrderAmount;
                                            }
                                            if (items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate != null && items.OrderOfCompleteDate == null)
                                            {
                                                if (items.OrderAmount != null)
                                                {
                                                    TotalofDeliverd = TotalofDeliverd + Convert.ToDecimal(items.OrderAmount);
                                                }
                                            }
                                        }
                                        runingbalancecalculation.TotalofInprogress = TotalofInprogress;
                                        runingbalancecalculation.TotalofDeliverd = TotalofDeliverd;
                                        EvcWallletAmount = Convert.ToDecimal(evcRegistrartion.EvcwalletAmount);
                                        runingbalancecalculation.runningBalance = EvcWallletAmount - (runingbalancecalculation.TotalofInprogress + runingbalancecalculation.TotalofDeliverd);
                                        ///
                                        

                                        if (transactionObj.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                                        {
                                            if (transactionObj.Exchange != null)
                                            {
                                                if (EVCassigned.StatusId == "22" || EVCassigned.StatusId == "21" || EVCassigned.StatusId == "44")
                                                {
                                                    if (EVCassigned.StatusId == "44" || EVCassigned.StatusId == "21")
                                                    {
                                                        if (Convert.ToInt32(runingbalancecalculation.runningBalance) < 0)
                                                        {
                                                            isBlanaceTrue = false;
                                                        }
                                                        else
                                                        {
                                                            isBlanaceTrue = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        isBlanaceTrue = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (Convert.ToInt32(runingbalancecalculation.runningBalance) >= EVCassigned.OrderAmount)
                                                        isBlanaceTrue = true;
                                                    else
                                                        isBlanaceTrue = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (transactionObj.AbbredemptionId != null)
                                            {
                                                if (EVCassigned.StatusId == "22" || EVCassigned.StatusId == "21" || EVCassigned.StatusId == "44")
                                                {
                                                    if (EVCassigned.StatusId == "44" || EVCassigned.StatusId == "21")
                                                    {
                                                        if (Convert.ToInt32(runingbalancecalculation.runningBalance) < 0)
                                                        {
                                                            isBlanaceTrue = false;
                                                        }
                                                        else
                                                        {
                                                            isBlanaceTrue = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        isBlanaceTrue = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (Convert.ToInt32(runingbalancecalculation.runningBalance) >= EVCassigned.OrderAmount)
                                                        isBlanaceTrue = true;
                                                    else
                                                        isBlanaceTrue = false;
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    if (evcRegistrartion != null && isBlanaceTrue)
                                    {
                                        //Raise ticket for  local logistics partner
                                        string LgcTicket = UniqueString.RandomNumberByLength(10);
                                        //point To Add Ticket data into logistics table and update status in wallet as well as status history table and exchange order table

                                        int ticketid = AddTicketToTblLogistics(LgcTicket, transactionObj.OrderTransId, servicePartnerId, transactionObj.RegdNo, customerDetails, userid, evcRegistrartion.EvcregistrationId);

                                        if (LgcTicket != null && ticketid > 0)
                                        {
                                            StatusDataContract structObj = new StatusDataContract(true, "Success", LgcTicket);
                                            response = new HttpResponseMessage(HttpStatusCode.OK)
                                            {
                                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                            };

                                        }

                                    }
                                    else
                                    {
                                        StatusDataContract structObj = new StatusDataContract(false, evcRegistrartion.BussinessName + ": does not have sufficient running balance i.e.", null);
                                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                    }
                                }
                                else
                                {
                                    StatusDataContract structObj = new StatusDataContract(false, "EVC Data Not Foud");
                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(false, "EVC is not alocated");
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }
                        }
                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(false, customerDetails.Message);
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }

                    }
                    else
                    {
                        StatusDataContract structObj = new StatusDataContract(false, "Ticket is alredy generated for this order");
                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };
                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "order not found");
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }

            }

            catch (Exception ex)
            {
                //.WriteErrorToDB("ZohoExposedController", "RequestMahindraLGC", ex);
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }

            return response;
        }
        #endregion

        #region  Mahindra Logistics Modified by VK Date 19-June-2023

        public HttpResponseMessage RequestMahindraLGC(string RegdNo, int servicePartnerId, int userid)
        {
            #region Variable Declaration
            HttpResponseMessage response = null;
            TicketResponceDataContract TicketResponceDC = null;
            SponserListDataContract sponserListDC = null;
            SponserData sponserObj = null;
            sponserObj = new SponserData();
            EvcApprovedData evcApprovedObj = null;
            evcApprovedObj = new EvcApprovedData();
            EvcApprovedListDataContract EvcApprovedDC = null;
            EvcAllocationReportData EvcAllocationObj = null;
            evcApprovedObj = new EvcApprovedData();
            EvcAllocationReportListDataContract EvcAllocationReportDC = null;
            SponserFormResponseDataContract sponserResponseDC = null;
            MahindraLogisticsDataContract mahindraLogisticsDataContract = null;
            MahindraLogisticsResponseDataContract mahindraResponseDC = null;

            RunningBalance runingbalancecalculation = new RunningBalance();
            EVCdetailsDataContract evcdetailsDC = new EVCdetailsDataContract();
            CustomerandOrderDetailsDataContract customerDetails = new CustomerandOrderDetailsDataContract();
            decimal TotalofInprogress = 0;
            decimal TotalofDeliverd = 0;
            decimal EvcWallletAmount = 0;
            #endregion
            try
            {
                TblOrderTran transactionObj = _orderTransRepository.GetRegdnoinTicketG(RegdNo);
                //point 2 >> Fill the TicketDataContract object using the response from point 1 
                if (transactionObj != null)
                {

                    TblLogistic lgcRecordObj = _logisticsRepository.GetSingle(x => x.RegdNo.ToLower() == RegdNo.ToLower() && x.IsActive == true);
                    //Ticket should not be generated already
                    if (lgcRecordObj == null)
                    {

                        if (transactionObj.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                        {

                            if (transactionObj.Exchange != null)
                            {
                                customerDetails = SetOrderDetailsObject(transactionObj);
                            }
                        }
                        else
                        {
                            TblAbbredemption abbredemptionObj = _aBBRedemptionRepository.GetSingle(x => x.RedemptionId == transactionObj.AbbredemptionId);
                            if (abbredemptionObj != null)
                            {
                                customerDetails = SetOrderDetailsObjectForABB(abbredemptionObj);
                            }
                        }
                        if (customerDetails.Message == null || customerDetails.Message == "")
                        {
                            // get EVC Call allocation report by sponserID for Drop customer details

                            TblWalletTransaction EVCassigned = _walletTransactionRepository.GetSingle(x => x.OrderTransId == transactionObj.OrderTransId);
                            if (EVCassigned != null)
                            {
                                //Get evc data from tblevcRegistration
                                TblEvcregistration evcRegistrartion = _eVCRepository.GetSingle(x => x.EvcregistrationId == EVCassigned.EvcregistrationId && x.Isevcapprovrd == true && x.IsActive == true);

                                if (evcRegistrartion != null)
                                {
                                    evcdetailsDC = SetEvcDetailsDataContract(evcRegistrartion);
                                    TblCreditRequest? tblCreditRequest = _context.TblCreditRequests.Where(x => x.WalletTransactionId == EVCassigned.WalletTransactionId && x.IsCreditRequest == true && x.IsActive == true).FirstOrDefault();
                                    bool isBlanaceTrue = true;
                                    if (tblCreditRequest != null && tblCreditRequest.IsCreditRequestApproved == true)
                                    {
                                        isBlanaceTrue = true;
                                    }
                                    else if (tblCreditRequest != null && tblCreditRequest.IsCreditRequestApproved == false)
                                    {
                                        isBlanaceTrue = false;
                                    }
                                    else
                                    {
                                        #region calculate clear balance
                                        List<TblWalletTransaction> walletsummary = _walletTransactionRepository.GetList(x => x.EvcregistrationId == EVCassigned.EvcregistrationId && x.IsActive == true && x.StatusId != Convert.ToInt32(OrderStatusEnum.PickupDecline).ToString() && x.StatusId != Convert.ToInt32(OrderStatusEnum.PickupTicketcancellationbyUTC).ToString() && x.StatusId != Convert.ToInt32(OrderStatusEnum.ReopenforLogistics).ToString()).ToList();

                                        ///
                                        foreach (var items in walletsummary)
                                        {
                                            if (items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate == null && items.OrderOfCompleteDate == null)
                                            {
                                                if (items.OrderAmount != null)
                                                {
                                                    TotalofInprogress = TotalofInprogress + Convert.ToDecimal(items.OrderAmount);

                                                }
                                                // tblWalletTransaction.OrderAmount += items.OrderAmount;
                                            }
                                            if (items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate != null && items.OrderOfCompleteDate == null)
                                            {
                                                if (items.OrderAmount != null)
                                                {
                                                    TotalofDeliverd = TotalofDeliverd + Convert.ToDecimal(items.OrderAmount);
                                                }
                                            }
                                        }
                                        runingbalancecalculation.TotalofInprogress = TotalofInprogress;
                                        runingbalancecalculation.TotalofDeliverd = TotalofDeliverd;
                                        EvcWallletAmount = Convert.ToDecimal(evcRegistrartion.EvcwalletAmount);
                                        runingbalancecalculation.runningBalance = EvcWallletAmount - (runingbalancecalculation.TotalofInprogress + runingbalancecalculation.TotalofDeliverd);
                                        ///
                                       // bool isBlanaceTrue = true;

                                        if (EVCassigned.StatusId == "22" || EVCassigned.StatusId == "21" || EVCassigned.StatusId == "44")
                                        {
                                            if (EVCassigned.StatusId == "44" || EVCassigned.StatusId == "21")
                                            {
                                                if (Convert.ToInt32(runingbalancecalculation.runningBalance) < 0)
                                                {
                                                    isBlanaceTrue = false;
                                                }
                                                else
                                                {
                                                    isBlanaceTrue = true;
                                                }
                                            }
                                            else
                                            {
                                                isBlanaceTrue = true;
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToInt32(runingbalancecalculation.runningBalance) >= EVCassigned.OrderAmount)
                                                isBlanaceTrue = true;
                                            else
                                                isBlanaceTrue = false;
                                        }
                                        #endregion
                                    }
                                    if (evcRegistrartion != null && isBlanaceTrue)
                                    {
                                        // set value in bizlog ticket object
                                        mahindraLogisticsDataContract = SetMahindraObj(customerDetails, evcdetailsDC);

                                        if (mahindraLogisticsDataContract != null)
                                        {
                                            //point 3 >> Create the tciket in bizlog and local DB (already happening)              
                                            mahindraResponseDC = ProcessLogisticsRequest(mahindraLogisticsDataContract);
                                            //point To Add Ticket data into logistics table and update status in wallet as well as status history table and exchange order table

                                            int ticketid = AddTicketToTblLogistics(mahindraResponseDC.awbNumber.ToString(), transactionObj.OrderTransId, servicePartnerId, transactionObj.RegdNo, customerDetails, userid, evcRegistrartion.EvcregistrationId);


                                            //point 4 >> update the zoho sponsor order form by ticket id received from bizlog response
                                            if (mahindraResponseDC != null)
                                            {
                                                if (mahindraResponseDC.status/*.Equals(true)*/== null)
                                                {
                                                    if (mahindraResponseDC.awbNumber != null)
                                                    {

                                                        StatusDataContract structObj = new StatusDataContract(true, "Success", mahindraResponseDC);
                                                        response = new HttpResponseMessage(HttpStatusCode.OK)
                                                        {
                                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                        };
                                                    }
                                                }

                                                else
                                                {
                                                    StatusDataContract structObj = new StatusDataContract(false, "Invalid model request", mahindraResponseDC);
                                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                                    {
                                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                    };
                                                }
                                            }
                                        }
                                        else
                                        {
                                            StatusDataContract structObj = new StatusDataContract(false, "please check the data once something is invalid", mahindraResponseDC);
                                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                            {
                                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                            };
                                        }
                                    }
                                    else
                                    {
                                        StatusDataContract structObj = new StatusDataContract(false, evcApprovedObj.Evc_Name + ": does not have sufficient running balance i.e.", null);
                                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                    }

                                }
                                else
                                {
                                    StatusDataContract structObj = new StatusDataContract(false, "EVC Data Not Foud please check if evc is not approved");
                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(false, "EVC is not alocated");
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }
                        }
                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(false, customerDetails.Message);
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }

                    }
                    else
                    {
                        StatusDataContract structObj = new StatusDataContract(false, "Ticket is alredy generated for this order");
                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };
                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "order not found");
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }

            }

            catch (Exception ex)
            {
                // LibLogging.WriteErrorToDB("ZohoExposedController", "RequestMahindraLGC", ex);
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }

            return response;

        }
        #endregion

        #region save details by lgc pickup
        /// <summary>
        /// save details by lgc pickup
        /// </summary>
        /// <param name="regdNo"></param>
        /// <returns></returns>
        public bool PickUpDoneOnTicketGenerate(string? RegdNo, int loggedInUserId)
        {
            TblOrderLgc? tblOrderLgc = null;
            int totalCount = 0;
            bool flag = false;
            string LGCType = "Pickup";
            int saveOrderLgc = 0;
            TblBusinessPartner? tblBusinessPartner = null;
            TblVoucherVerfication? tblVoucherVerfication = null;
            TblWalletTransaction? tblWalletTransaction = null;
            #region Common Implementations for (ABB or Exchange)
            TblExchangeOrder? tblExchangeOrder = null;
            int? setStatusId = Convert.ToInt32(OrderStatusEnum.LGCPickup);
            //int? 
            #endregion

            try
            {
                #region Common Implementations for (ABB or Exchange)
                TblOrderTran tblOrderTrans = _orderTransRepository.GetOrderTransByRegdno(RegdNo);
                TblLogistic tblLogistic = _logisticsRepository.GetSingle(x => x.IsActive == true && x.RegdNo == RegdNo);
                tblWalletTransaction = _walletTransactionRepository.GetSingle(x => x.IsActive == true && x.RegdNo == RegdNo);
                if (tblOrderTrans != null && tblLogistic != null && tblWalletTransaction != null)
                {
                    #region insert details in tblorderlgc
                    if (tblOrderTrans != null && tblOrderTrans.OrderTransId > 0)
                    {
                        tblOrderLgc = _orderLGCRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == tblOrderTrans.OrderTransId);
                    }
                    if (tblOrderLgc != null && tblOrderLgc.OrderLgcid > 0)
                    {
                        // tblOrderLgc.Lgccomments = orderLGCViewModel.Lgccomments;
                        tblOrderLgc.ActualPickupDate = DateTime.Now;
                        tblOrderLgc.StatusId = Convert.ToInt32(OrderStatusEnum.LGCPickup);
                        tblOrderLgc.IsActive = true;
                        tblOrderLgc.ModifiedBy = loggedInUserId;
                        tblOrderLgc.ModifiedDate = DateTime.Now;
                        tblOrderLgc.LogisticId = tblLogistic.LogisticId;
                        tblOrderLgc.EvcregistrationId = tblWalletTransaction.EvcregistrationId;
                        tblOrderLgc.EvcpartnerId = tblWalletTransaction.EvcpartnerId;
                        _orderLGCRepository.Update(tblOrderLgc);
                        saveOrderLgc = _orderLGCRepository.SaveChanges();
                    }
                    else
                    {
                        tblOrderLgc = new TblOrderLgc();
                        tblOrderLgc.OrderTransId = tblOrderTrans.OrderTransId;
                        // tblOrderLgc.Lgccomments = orderLGCViewModel.Lgccomments;
                        tblOrderLgc.ActualPickupDate = DateTime.Now;
                        tblOrderLgc.StatusId = setStatusId;
                        tblOrderLgc.IsActive = true;
                        tblOrderLgc.CreatedBy = loggedInUserId;
                        tblOrderLgc.CreatedDate = DateTime.Now;
                        tblOrderLgc.LogisticId = tblLogistic.LogisticId;
                        tblOrderLgc.EvcregistrationId = tblWalletTransaction.EvcregistrationId;
                        tblOrderLgc.EvcpartnerId = tblWalletTransaction.EvcpartnerId;
                        _orderLGCRepository.Create(tblOrderLgc);
                        saveOrderLgc = _orderLGCRepository.SaveChanges();
                    }
                    #endregion
                    tblExchangeOrder = _exchangeOrderRepository.GetExchOrderByRegdNo(RegdNo);
                    #region Redeemed Mark on tblVoucherVerification if Order case is Deffered 
                    if (saveOrderLgc > 0 && tblExchangeOrder != null && tblExchangeOrder.IsDefferedSettlement == true)
                    {
                        tblBusinessPartner = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == tblExchangeOrder.BusinessPartnerId);
                        if (tblBusinessPartner != null && tblBusinessPartner.IsDefferedSettlement == true && tblBusinessPartner.IsVoucher == true && tblBusinessPartner.VoucherType == Convert.ToInt32(BusinessPartnerVoucherTypeEnum.Cash))
                        {
                            tblVoucherVerfication = _voucherRepository.GetSingle(x => x.IsActive == true && x.ExchangeOrderId == tblExchangeOrder.Id && x.BusinessPartnerId == tblBusinessPartner.BusinessPartnerId);
                            if (tblVoucherVerfication != null && tblVoucherVerfication.VoucherStatusId == Convert.ToInt32(VoucherStatusEnum.Captured))
                            {
                                tblVoucherVerfication.VoucherStatusId = Convert.ToInt32(VoucherStatusEnum.Redeemed);
                                tblVoucherVerfication.ModifiedBy = loggedInUserId;
                                tblVoucherVerfication.ModifiedDate = _currentDatetime;
                                _voucherRepository.Update(tblVoucherVerfication);
                                _voucherRepository.SaveChanges();
                            }
                        }
                    }
                    #endregion

                    return flag = true;
                }
                #endregion

            }
            catch (Exception ex)
            {

                _logging.WriteErrorToDB("QCManager", "AddImageToDB", ex);
            }
            return flag;
        }
        #endregion
    }
}
