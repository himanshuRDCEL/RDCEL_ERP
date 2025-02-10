using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Paymant
{
    class ZaakpayViewModel
    {
    }
    //public class OrderModel
    //{
    //    public string? orderId { get; set; }
    //    public string? zaakpayKey { get; set; }
    //    public string? amount { get; set; }
    //    public string? currency { get; set; }
    //    public string? name { get; set; }
    //    public string? EVCemail { get; set; }
    //    public string? buyerEmail { get; set; }
    //    public string? EVCcontactNumber { get; set; }
    //    public string? EVCaddress { get; set; }
    //    public string? description { get; set; }
    //    public string? checksum { get; set; }
    //    public string? merchantIdentifier { get; set; }
    //    public string? zaakPayKey { get; set; }
    //    public string? secretKey { get; set; }
    //}

    public class PaymentInitiateModel
    {
        public int EvcregistrationId { get; set; }
        public string? BussinessName { get; set; }
        public string? EVCemail { get; set; }
        public string? EVCcontactNumber { get; set; }
        public string? EVCaddress { get; set; }
        public decimal amount { get; set; }
        public string? EVCRegdNo { get; set; }
        public string? description { get; set; }
        public string? EVCCity { get; set; }
        public string? EVCstate { get; set; }

        public string? EVCPin { get; set; }

    }
    public class paymentInitialViewModel
    {
        public int EvcregistrationId { get; set; }
        public string? BussinessName { get; set; }
        public string? EVCemail { get; set; }
        public string? EVCcontactNumber { get; set; }
        public string? EVCaddress { get; set; }
        public decimal amount { get; set; }
        public string? EVCRegdNo { get; set; }
        public string? description { get; set; }
        public string? EVCCity { get; set; }
        public string? EVCstate { get; set; }
        public string? EVCPin { get; set; }
    }
    public class PaymentResponseModel
    {
        public string? orderStatus;

        public string? transactionId { get; set; }
        public string? OrderId { get; set; }
        public string? status { get; set; }
        public string? RegdNo { get; set; }
        public decimal amount { get; set; }
        public int responseCode { get; set; }
        public string? responseDescription { get; set; }
        public string? checksum { get; set; }
        public string? paymentMode { get; set; }
        public string? cardId { get; set; }
        public string? cardScheme { get; set; }
        public string? cardToken { get; set; }
        public string? bank { get; set; }
        public string? bankid { get; set; }
        public string? paymentmethod { get; set; }
        public string? cardhashId { get; set; }
        public string? productDescription { get; set; }
        public string? pgTransId { get; set; }
        public string? pgTransTime { get; set; }

        public string? gatewayTransactionId { get; set; }
    }
    public class ZaakpayKey
    {
        public string? merchantId { get; set; }
        public string? secretKey { get; set; }
    }
    public class ZaakPayResponseModel
    {
        public string? orderId { get; set; }
        public int responseCode { get; set; }
        public string? responseDescription { get; set; }
        public string? checksum { get; set; }
        public string? amount { get; set; }
        public string? paymentMode { get; set; }
        public string? cardId { get; set; }
        public string? cardScheme { get; set; }
        public string? cardToken { get; set; }
        public string? bank { get; set; }
        public string? bankid { get; set; }
        public string? paymentmethod { get; set; }
        public string? cardhashId { get; set; }
        public string? productDescription { get; set; }
        public string? pgTransId { get; set; }
        public string? pgTransTime { get; set; }
        public string? RegdNo { get; set; }
    }

    public class pluralgatewayKey
    {
        public string? merchantId { get; set; }
        public string? secretKey { get; set; }
        public string? accessCode { get; set; }
    }
    public class OrderModel
    {
        public string? theme { get; set; }
        public string? orderToken { get; set; }
        public string? channelId { get; set; }
        public string? paymentMode { get; set; }
        public string? countryCode { get; set; }
        public string? mobileNumber { get; set; }
        public string? emailId { get; set; }
        public string? OrderId { get; set; }
        public string? PaymentId { get; set; }
        public string? errorcode { get; set; }
        public string? errrorResponse { get; set; }
        public decimal amount { get; set; }
        public bool showSavedCardsFeature { get; set; }
    }
}

