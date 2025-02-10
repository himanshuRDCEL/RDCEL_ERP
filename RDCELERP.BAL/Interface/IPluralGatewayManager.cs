
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTC_Model.PluralGateway;

namespace RDCELERP.BAL.Interface
{
    public interface IPluralGatewayManager
    {
        public RestResponse Rest_InvokePluralServiceCall(string url, Method methodType, string Hasstring, object content = null);
        public RestResponse Rest_InvokePluralServiceCallGetPaymentStatus(string url, Method methodType, string Encryption, object content = null);
       // public async Task<string> MakeRequest(string url, string username, string password);
        public RestResponse CreateOrderManager(PluralCreateOrder pluralCreateOrder, string SecretKey);

        public GetpaymentStatusResponse GetpaymentStatus(string PaymentId, string OrderId);
    }
}
