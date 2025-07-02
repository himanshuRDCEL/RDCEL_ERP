using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Razorpay.Api;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Model.Base;
using RDCELERP.Model.Paymant;
using RDCELERP.Model.RazorPay;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.MasterManager
{
    public class RazorPayManager : IRazorPayManager
    {
        ILogging _logging;
        //private readonly MailSettings _mailSettings;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        private readonly IWebHostEnvironment _webHostEnvironment;
        public readonly IOptions<ApplicationSettings> _config;

        public RazorPayManager(IWebHostEnvironment webHostEnvironment, ILogging logging, IOptions<ApplicationSettings> config)
        {
            _logging = logging;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
        }


        public RazorpayOrderModel CreateOrder(int amountInRupees, string name, string email, string contact,string OrderNo)
        {
            RazorpayOrderModel razorpayOrderModel = new RazorpayOrderModel();
            string key = _config.Value.RazorPayKey_Id;
            string secret = _config.Value.RazorPaykey_secret;
            RazorpayClient client = new RazorpayClient(key, secret);
            Dictionary<string, object> options = new Dictionary<string, object>
        {
            { "amount", amountInRupees * 100 }, // Amount in paise
            { "currency", "INR" },
            { "receipt", OrderNo },
            { "payment_capture", 1 }
        };
            try { 
            Order order = client.Order.Create(options);

            string jsonResponse = order.Attributes.ToString();

            RazorPayOrderCreatedResponse deserializedOrder = JsonConvert.DeserializeObject<RazorPayOrderCreatedResponse>(jsonResponse);

                razorpayOrderModel = new RazorpayOrderModel
                {
                    OrderId = order["id"].ToString(),
                Key = key,
                Amount = (amountInRupees * 100).ToString(),
                Currency = "INR",
                Name = name,
                Email = email,
                Contact = contact,
                Description = "Order Payment"
            };
        }
             catch (Exception ex)
            {
                _logging.WriteErrorToDB("RazorPayManager", "VerifySignature", ex);
            }
            return razorpayOrderModel;

        }
        public bool VerifySignature(string orderId, string paymentId, string signature)
        {
            bool flag=false;
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            attributes.Add("razorpay_order_id", orderId);
            attributes.Add("razorpay_payment_id", paymentId);
            attributes.Add("razorpay_signature", signature);

            try
            {
                bool isValid = VerifySignatureManual(orderId, paymentId, signature, "YourSecretKey");
                flag= true;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RazorPayManager", "VerifySignature", ex);
            }

            return flag;
        }

        public bool VerifySignatureManual(string orderId, string paymentId, string signature, string secret)
        {
            string payload = orderId + "|" + paymentId;

            try
            {
                using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
                {
                    var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                    var generatedSignature = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

                    return generatedSignature == signature.ToLowerInvariant();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RazorPayManager", "VerifySignatureManual", ex);
                return false;
            }
        }


        public  async Task<ResponseRazorpayPaymentViewModel> GetPaymentDetails(string paymentId)
        {
            ResponseRazorpayPaymentViewModel deserializedPayment = null;
            try
            {
                string key = _config.Value.RazorPayKey_Id;
                string secret = _config.Value.RazorPaykey_secret;
                var client = new RazorpayClient(key, secret);
                var payment = client.Payment.Fetch(paymentId);

                string jsonResponse = payment.Attributes.ToString();

               deserializedPayment = JsonConvert.DeserializeObject<ResponseRazorpayPaymentViewModel>(jsonResponse);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RazorPayManager", "GetPaymentDetail", ex);
            }

            return deserializedPayment;
        }
    }
}