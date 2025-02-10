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
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.CashfreeModel;
using RDCELERP.Model.UPIVerificationModel;


namespace RDCELERP.BAL.MasterManager
{
    public class UPIIdVerification : IUPIIdVerification
    {
        #region variable declaration
        IOptions<ApplicationSettings> _baseConfig;
        ILogging _logging;
        IExchangeOrderRepository _exchangeOrderRepository;
        ICustomerDetailsRepository _customerDetailsRepository;
        IOrderTransRepository _orderTransRepository;
        IAbbRegistrationRepository _abbRegistrationRepository;
        #endregion

        #region constructor
        public UPIIdVerification(IOptions<ApplicationSettings> baseConfig, ILogging logging, IExchangeOrderRepository exchangeOrderRepository, ICustomerDetailsRepository customerDetailsRepository, IOrderTransRepository orderTransRepository, IAbbRegistrationRepository abbRegistrationRepository)
        {
            _baseConfig = baseConfig;
            _logging = logging;
            _exchangeOrderRepository = exchangeOrderRepository;
            _customerDetailsRepository = customerDetailsRepository;
            _orderTransRepository = orderTransRepository;
            _abbRegistrationRepository = abbRegistrationRepository;
        }
        #endregion
        public string Rest_ResponseUPIIdverificationCall(string url, Method methodType,string Authtoken)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            RestResponse getResponse = null;
            string Response = null;
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest();
                request.Method = methodType;
                request.AddHeader("Authorization", Authtoken);
                request.AddHeader("content-type", "application/json");

                getResponse = client.Execute(request);
                Response = getResponse.Content;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UPIIdverificationCall", "Rest_InvokeWhatsappserviceCall", ex);
            }
            return Response;
        }

        public UPIVerification CheckUpiId(string regdno, string UpiId,string AuthToken)
        {
            UPIVerification root = new UPIVerification();
            TblOrderTran tblOrderTran = null;
            TblAbbregistration tblAbbregistration = null;
            string url = null;
            string response = null;

            if (regdno != null)
            {
                tblOrderTran = _orderTransRepository.GetRegdno(regdno);
                if (tblOrderTran != null && tblOrderTran.OrderType == (int?)LoVEnum.Exchange)
                {
                    TblExchangeOrder tblExchangeOrder = _exchangeOrderRepository.GetRegdNo(regdno);
                    if (tblExchangeOrder != null)
                    {
                        TblCustomerDetail tblCustomerDetail = _customerDetailsRepository.GetCustDetails(tblExchangeOrder.CustomerDetailsId);
                        if (tblCustomerDetail != null)
                        {
                            url = _baseConfig.Value.cashfreeVerifyUPI + "name=" + tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName + "&vpa=" + UpiId;                      
                        }
                    }
                }
                else
                {
                    tblAbbregistration = _abbRegistrationRepository.GetRegdNo(regdno);
                    if (tblAbbregistration != null)
                    {
                        TblCustomerDetail tblCustomerDetail = _customerDetailsRepository.GetCustDetails(tblAbbregistration.CustomerId);
                        if (tblCustomerDetail != null)
                        {
                            url = _baseConfig.Value.cashfreeVerifyUPI + "name=" + tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName + "&vpa=" + UpiId;                            
                        }
                    }
                }
                string vpaValid = EnumHelper.DescriptionAttr(UPIResponseEnum.vpaValid);
                try
                {
                    response = Rest_ResponseUPIIdverificationCall(url, Method.Get, AuthToken);
                    if (response != null)
                    {
                        root = JsonConvert.DeserializeObject<UPIVerification>(response);
                    }
                }
                catch (Exception ex)
                {
                    dynamic desiralize = JsonConvert.DeserializeObject(response);
                    root.message = desiralize.message;
                    root.subCode = desiralize.subCode;
                    _logging.WriteErrorToDB("CashFreeServiceCall", "CashFreeAuthCall", ex);
                }
            }
            return root;
        }
    }
}
