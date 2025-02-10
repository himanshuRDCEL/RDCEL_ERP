using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.TicketGenrateModel.Mahindra
{
    public class Address
    {
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? zipcode { get; set; }
        public string? street_address { get; set; }
        public string? telephone { get; set; }
        public string? state { get; set; }
        public string? city { get; set; }
        public string? email { get; set; }
    }

    public class LineItem
    {
        public string? sku { get; set; }
        public string? name { get; set; }
        public string? price_per_each_item { get; set; }
        public double total_price { get; set; }
        public int quantity { get; set; }
    }

    public class Location
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class PickUpAddress
    {
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? zipcode { get; set; }
        public string? street_address { get; set; }
        public string? telephone { get; set; }
        public string? state { get; set; }
        public string? city { get; set; }
        public string? email { get; set; }
        public Location? location { get; set; }
    }

    public class MahindraLogisticsDataContract
    {
        public string? client_order_id { get; set; }
        public Address? address { get; set; }
        public PickUpAddress? pickUpAddress { get; set; }
        public List<LineItem>? line_items { get; set; }
        public string? order_delivery_date { get; set; }
        public double total_price { get; set; }
        public int quantity { get; set; }
    }
}
