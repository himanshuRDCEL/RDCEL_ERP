using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTC_Model.PluralGateway
{
   


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AdditionalInfoData
    {
        public string? rfu1 { get; set; }
    }

    public class BillingAddressData
    {
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? address1 { get; set; }
        public string? address2 { get; set; }
        public string? address3 { get; set; }
        public string? pin_code { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? country { get; set; }
    }

    public class CustomerData
    {
        public string? country_code { get; set; }
        public string? mobile_number { get; set; }
        public string? email_id { get; set; }
    }

    public class MerchantData
    {
        public string? merchant_id { get; set; }
        public string? merchant_access_code { get; set; }
        public string? merchant_return_url { get; set; }
        public string? merchant_order_id { get; set; }
    }

    public class PaymentInfoData
    {
        public int amount { get; set; }
        public string? currency_code { get; set; }
        public string? order_desc { get; set; }
    }

    public class ProductDetail
    {
        public string? product_code { get; set; }
        public int product_amount { get; set; }
    }

    public class ProductInfoData
    {
        public List<ProductDetail>? product_details { get; set; }
    }

    public class PluralCreateOrder
    {
        public MerchantData? merchant_data { get; set; }
        public PaymentInfoData? payment_info_data { get; set; }
        public CustomerData? customer_data { get; set; }
        public BillingAddressData? billing_address_data { get; set; }
        public ShippingAddressData? shipping_address_data { get; set; }
        public ProductInfoData? product_info_data { get; set; }
        public AdditionalInfoData? additional_info_data { get; set; }
    }

    public class ShippingAddressData
    {
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? address1 { get; set; }
        public string? address2 { get; set; }
        public string? address3 { get; set; }
        public string? pin_code { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? country { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CreateOrderEncryption
    {
        public string? request { get; set; }
    }


}
