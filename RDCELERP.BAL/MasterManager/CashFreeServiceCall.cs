using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Model.Base;
using RDCELERP.Model.CashfreeModel;

namespace RDCELERP.BAL.MasterManager
{
    public class CashFreeServiceCall : ICashfreePayoutCall
    {
        #region variable declaration
        IOptions<ApplicationSettings> _baseConfig;
        ILogging _logging;
        #endregion

        #region constructor
        public CashFreeServiceCall(IOptions<ApplicationSettings> baseConfig, ILogging logging)
        {
            _baseConfig = baseConfig;
            _logging = logging;
        }
        #endregion

        public AddBeneficiaryResponse AddBenefiaciary(AddBeneficiary addBaneficiary,string AuthToken)
        {
            AddBeneficiaryResponse beneficiaryResponse = new AddBeneficiaryResponse();
            string response = null;
            string? url = _baseConfig.Value.cashfreeAddBeneficiary;
            try
            {
                if(addBaneficiary!=null && AuthToken != null)
                {
                    response = Rest_ResponseCashfreeServiceCall(url,Method.Post,AuthToken,addBaneficiary);

                    if (response != null)
                    {
                        beneficiaryResponse = JsonConvert.DeserializeObject<AddBeneficiaryResponse>(response);
                    }

                }
            }
            catch (Exception ex)
            {
                dynamic responseforBeneficiary = JsonConvert.DeserializeObject(response);
                beneficiaryResponse.subCode = responseforBeneficiary.subCode;
                beneficiaryResponse.message = responseforBeneficiary.message;
                _logging.WriteErrorToDB("CashFreeServiceCall", "AddBenefiaciary", ex);
            }
            return beneficiaryResponse;
        }

        public CashfreeAuth CashFreeAuthCall()
        {

            CashfreeAuth cashfreeauth = new CashfreeAuth();
            string url = _baseConfig.Value.CashfreeAuthUrl;
            string response = null;
            string subcode = Convert.ToInt32(CashfreeEnum.Succcess).ToString();
            try
            {
                response = Rest_ResponseCashfreeAuthCall(url, Method.Post);
                if (response != null)
                {
                    cashfreeauth = JsonConvert.DeserializeObject<CashfreeAuth>(response);
                    if(cashfreeauth.subCode== subcode)
                    {
                        cashfreeauth.data.token = "Bearer " + cashfreeauth.data.token;
                    }
                }
            }
            catch(Exception ex)
            {
                dynamic responseforauth= JsonConvert.DeserializeObject(response);
                cashfreeauth.subCode = responseforauth.subCode;
                cashfreeauth.message = responseforauth.message;

                _logging.WriteErrorToDB("CashFreeServiceCall", "CashFreeAuthCall", ex);
            }
           
            return cashfreeauth;
        }

        public string Rest_ResponseCashfreeAuthCall(string url, Method methodType)
        {
            string clientId = _baseConfig.Value.cashfreeClientId;
            string SecretKey = _baseConfig.Value.CashFreeSecretKey;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            RestResponse getResponse = null;
            string Response = null;
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest();
                request.Method = methodType;
                
                request.AddHeader("X-Client-Id", clientId); //Add Header tocken
                request.AddHeader("X-Client-Secret", SecretKey); //Add Header tocken
              
                getResponse = client.Execute(request);
                Response = getResponse.Content;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CashFreeServiceCall", "Rest_InvokeWhatsappserviceCall", ex);
            }
            return Response;
        }

        public string Rest_ResponseCashfreeServiceCall(string url, Method methodType, string AuthToken, object content = null)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            RestResponse getResponse = null;
            string aouthToken = string.Empty;
            string jsonString = string.Empty;
            string response = null;
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest();
                request.Method = methodType;
                request.AddHeader("Authorization", AuthToken);
                request.AddHeader("content-type", "application/json");
                if (content != null)
                {
                    jsonString = JsonConvert.SerializeObject(content);
                    request.AddJsonBody(jsonString);
                }
                getResponse = client.Execute(request);
                response = getResponse.Content;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CashFreeServiceCall", "Rest_InvokeWhatsappserviceCall", ex);
            }
            return response;
        }

        public TransactionResponseCashfree Transaction(ProcessTransactionCashfree transaction,string AuthToken)
        {
            TransactionResponseCashfree transactionResponse = new TransactionResponseCashfree();
            string Response = null;
            string Url = _baseConfig.Value.cashfreeTransaction;
            try
            {
                if(transaction!=null && AuthToken != null)
                {
                    Response = Rest_ResponseCashfreeServiceCall(Url,Method.Post,AuthToken,transaction);
                    if (Response!=null)
                    {
                        transactionResponse = JsonConvert.DeserializeObject<TransactionResponseCashfree>(Response);
                    }
                }
            }
            catch(Exception ex)
            {
                dynamic responseforauth = JsonConvert.DeserializeObject(Response);
                transactionResponse.subCode = responseforauth.subCode;
                transactionResponse.message = responseforauth.message;

                _logging.WriteErrorToDB("CashFreeServiceCall", "Transaction", ex);
            }

            return transactionResponse;
        }

        public GetBeneficiary GetBeneficiary(string AuthToken, string BeneficiaryId)
        {
            GetBeneficiary getBeneficiary = new GetBeneficiary();
            string Response = null;
            string url = _baseConfig.Value.cashfreeGetBeneficiary+""+ BeneficiaryId;
            try
            {
                if(BeneficiaryId!=null && AuthToken != null)
                {
                    Response = Rest_ResponseCashfreeGetBeneficiaryCall(url,Method.Get,AuthToken);
                    if (Response != null)
                    {
                        getBeneficiary = JsonConvert.DeserializeObject<GetBeneficiary>(Response);
                    }
                }
            }
            catch (Exception ex)
            {
                dynamic desirialize = JsonConvert.DeserializeObject(Response);
                getBeneficiary.subCode = desirialize.subCode;
                getBeneficiary.message = desirialize.message;
                _logging.WriteErrorToDB("CashFreeServiceCall", "GetBeneficiary", ex);
            }
            return getBeneficiary;
        }

        public string Rest_ResponseCashfreeGetBeneficiaryCall(string url, Method methodType,string AuthToken)
        {
            string clientId = _baseConfig.Value.cashfreeClientId;
            string SecretKey = _baseConfig.Value.CashFreeSecretKey;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            RestResponse getResponse = null;
            string Response = null;
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest();
                request.Method = methodType;
                request.AddHeader("Authorization", AuthToken); //Add Header tocken
                request.AddHeader("Content-Type", "application/json"); //Add Header tocken
                //if (content != null)
                //{
                //    jsonString = JsonConvert.SerializeObject(content);
                //    request.AddJsonBody(jsonString);
                //}
                getResponse = client.Execute(request);
                Response = getResponse.Content;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CashFreeServiceCall", "Rest_InvokeWhatsappserviceCall", ex);
            }
            return Response;
        }

        public TransactionCashFree CashFreeTransaction(string regdno, string AuthToken)
        {
            TransactionCashFree CashFreetransactionResponse = new TransactionCashFree();
            string Response = null;
            string Url = _baseConfig.Value.cashfreeApiTransaction;
            try
            {
                if (regdno != null && AuthToken != null)
                {
                    Url = _baseConfig.Value.cashfreeVerifyUPI + "transferId=" + regdno;
                    Response = Rest_ResponseCashFreeTransaction(Url, Method.Get, AuthToken);
                    if (Response != null)
                    {
                        CashFreetransactionResponse = JsonConvert.DeserializeObject<TransactionCashFree>(Response);
                    }
                }
            }
            catch (Exception ex)
            {
                dynamic responseforauth = JsonConvert.DeserializeObject(Response);
                CashFreetransactionResponse.subCode = responseforauth.subCode;
                CashFreetransactionResponse.message = responseforauth.message;

                _logging.WriteErrorToDB("CashFreeServiceCall", "Transaction", ex);
            }

            return CashFreetransactionResponse;
        }

        public string Rest_ResponseCashFreeTransaction(string url, Method methodType, string Authtoken)
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
                _logging.WriteErrorToDB("CashFreeTransaction", "Rest_InvokeWhatsappserviceCall", ex);
            }
            return Response;
        }

        public TransactionCashFree GetPaymentTransferStatus(string utcReferenceID, string AuthToken)
        {
            TransactionCashFree cashFreeStatusResponseModel = new TransactionCashFree();
            string? Response = null;
            string Url = string.Empty;
            try
            {
                if (utcReferenceID != null && AuthToken != null)
                {
                    Url = _baseConfig.Value.cashfreePaymentStatusAPI + "transferId=" + utcReferenceID;
                    Response = Rest_ResponseCashFreeGetPaymentStatus(Url, Method.Get, AuthToken);
                    if (Response != null)
                    {
                        cashFreeStatusResponseModel = JsonConvert.DeserializeObject<TransactionCashFree>(Response);
                    }
                }
                else
                {
                    string message = "UtcReferenceID getting as null in the database for the regdNumber";
                    cashFreeStatusResponseModel.message = message;
                }
            }
            catch (Exception ex)
            {
                dynamic responseforauth = JsonConvert.DeserializeObject(Response);
                _logging.WriteErrorToDB("CashFreeServiceCall", "PaymentStatus", ex);
            }

            return cashFreeStatusResponseModel;
        }

        public string Rest_ResponseCashFreeGetPaymentStatus(string url, Method methodType, string AuthToken)
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
                request.AddHeader("Authorization", AuthToken);
                request.AddHeader("Content-Type", "application/json");

                getResponse = client.Execute(request);
                Response = getResponse.Content;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CashFreeService", "Rest_InvokePaymentStatusCall", ex);
            }
            return Response;
        }

        public RemoveBeneficiaryResponse RemoveBeneficiary(RemoveBeneficiary removeBeneficiary, string AuthToken)
        {
            RemoveBeneficiaryResponse? removeBeneficiaryResponse = new RemoveBeneficiaryResponse();
            string response = null;
            string url = _baseConfig.Value.cashfreeRemoveBeneficiaryAPI;
            try
            {
                if (removeBeneficiary != null && AuthToken != null)
                {
                    response = Rest_ResponseCashfreeRemoveBeneficiaryCall(url, Method.Post, AuthToken, removeBeneficiary);

                    if (response != null)
                    {
                        removeBeneficiaryResponse = JsonConvert.DeserializeObject<RemoveBeneficiaryResponse>(response);
                    }

                }
            }
            catch (Exception ex)
            {
                dynamic responseforBeneficiary = JsonConvert.DeserializeObject(response);
                removeBeneficiaryResponse.subCode = responseforBeneficiary.subCode;
                removeBeneficiaryResponse.message = responseforBeneficiary.message;
                _logging.WriteErrorToDB("CashFreeServiceCall", "RemoveBenefiaciary", ex);
            }
            return removeBeneficiaryResponse;
        }

        public string Rest_ResponseCashfreeRemoveBeneficiaryCall(string url, Method methodType, string AuthToken, RemoveBeneficiary removeBeneficiary)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            RestResponse getResponse = null;
            string aouthToken = string.Empty;
            string jsonString = string.Empty;
            string response = null;
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest();
                request.Method = methodType;
                request.AddHeader("Authorization", AuthToken);
                request.AddHeader("content-type", "application/json");
                if (removeBeneficiary != null)
                {
                    jsonString = JsonConvert.SerializeObject(removeBeneficiary);
                    request.AddJsonBody(jsonString);
                }
                getResponse = client.Execute(request);
                response = getResponse.Content;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CashFreeServiceCall", "Rest_InvokeRemoveBeneficiaryCall", ex);
            }
            return response;
        }

        public GetWalletBalanceResponse GetWalletBalance(string AuthToken)
        {
            GetWalletBalanceResponse getWalletBalanceResponse = new GetWalletBalanceResponse();
            string Response = null;
            string url = _baseConfig.Value.CashfreeGetWalletBalanceAPI;

            try
            {
                if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(AuthToken))
                {
                    Response = Rest_ResponseCashFreeGetWalletBalance(url, Method.Get, AuthToken);
                }
                else
                {
                    getWalletBalanceResponse.message = "Either token or Requested URL is null";
                }
                if (Response != null)
                {
                    getWalletBalanceResponse = JsonConvert.DeserializeObject<GetWalletBalanceResponse>(Response);
                }
            }
            catch(Exception ex)
            {
                dynamic responseforBeneficiary = JsonConvert.DeserializeObject(Response);
                getWalletBalanceResponse.subCode = responseforBeneficiary?.subCode;
                getWalletBalanceResponse.message = responseforBeneficiary?.message;
                _logging.WriteErrorToDB("CashFreeServiceCall", "GetWalletBalance", ex);
            }
            return getWalletBalanceResponse;
        }

        public string Rest_ResponseCashFreeGetWalletBalance(string url, Method methodType, string AuthToken)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            RestResponse getResponse = null;
            string Response = null;
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest();
                request.Method = methodType;
                request.AddHeader("Authorization", AuthToken);
                request.AddHeader("Content-Type", "application/json");

                getResponse = client.Execute(request);
                Response = getResponse.Content;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CashFreeService", "Rest_InvokeGetWalletBalanceCall", ex);
            }
            return Response;
        }
    }
}
