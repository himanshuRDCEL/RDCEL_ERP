using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.TicketGenrateModel
{
    public class CustomerandOrderDetailsDataContract
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? PhoneNumber { get; set; }
        public string? state { get; set; }
        public string? city { get; set; }
        public string? Pincode { get; set; }
        public string? RegdNo { get; set; }
        public string? SponserOrdrNumber { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductType { get; set; }
        public string? Brandname { get; set; }
        public string? productCost { get; set; }
        public bool IsDtoC { get; set; }
        public bool IsDeffered { get; set; }
        public string? Message { get; set; }
        public string? pickupPriority { get; set; }

        public string? OrderDate { get; set; }

    }

    public class ServicePartnerLogin
    {
        public string? RegdNo { get; set; }
        public string? priority { get; set; }
        public bool GenerateticketWithutCheckingBalance { get; set; }
        public int ServicePartnerId { get; set; }
        public List<SelectListItem>? priorityList { get; set; }
        public bool IsServicePartnerLocal { get; set; }
        public string? ServicePartnerName { get; set; }
    }
    public class RegdNoList
    {
        public string? RegdNo { get; set; }
    }
    public class ResponseData
    {
        public string? Regno { get; set; }
        public string? ServicePartner { get; set; }
        public int StatusCode { get; set; }
        public string? Content { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? TicketNo { get; set; }
        public int awbNumber { get; set; }
        public string? ProductErrors { get; set; }
    }
    public class ContentData
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public string? TicketNo { get; set; }       
        public DetailData? Detail { get; set; }
    }

    public class DetailData
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string? Msg { get; set; }
        public DataItem? Data { get; set; }
        public int awbNumber { get; set; }
    }

    public class DataItem
    {
        public string? TicketNo { get; set; }
        public string? ProductErrors { get; set; }
    }
}
