using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTC_Model.PluralGateway
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class MerchantDataPaymentresponse
    {
        public int merchant_id { get; set; }
        public string? order_id { get; set; }
    }

    public class OrderDataPaymentResponse
    {
        public string? order_status { get; set; }
        public int plural_order_id { get; set; }
        public int amount { get; set; }
        public string? order_desc { get; set; }
        public string? refund_amount { get; set; }
    }

    public class PaymentInfoDatapaymentresponse
    {
        public string? acquirer_name { get; set; }
        public string? auth_code { get; set; }
        public string? captured_amount_in_paisa { get; set; }
        public string? card_holder_name { get; set; }
        public string? masked_card_number { get; set; }
        public string? merchant_return_url { get; set; }
        public string? mobile_no { get; set; }
        public DateTime payment_completion_date_time { get; set; }
        public string? payment_id { get; set; }
        public string? payment_status { get; set; }
        public int payment_response_code { get; set; }
        public string? payment_response_message { get; set; }
        public string? product_code { get; set; }
        public string? rrn { get; set; }
        public string? refund_amount_in_paisa { get; set; }
        public string? salted_card_hash { get; set; }
        public string? udf_field_1 { get; set; }
        public string? udf_field_2 { get; set; }
        public string? udf_field_3 { get; set; }
        public string? udf_field_4 { get; set; }
        public string? payment_mode { get; set; }
        public string? issuer_name { get; set; }
        public string? gateway_payment_id { get; set; }
    }

    public class GetpaymentStatusResponse
    {
        public MerchantDataPaymentresponse? merchant_data { get; set; }
        public OrderDataPaymentResponse? order_data { get; set; }
        public PaymentInfoDatapaymentresponse? payment_info_data { get; set; }
    }


}
