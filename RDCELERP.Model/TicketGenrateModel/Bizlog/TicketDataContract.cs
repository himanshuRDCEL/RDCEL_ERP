using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.TicketGenrateModel.Bizlog
{
    public class TicketDataContract
    {
        public string? consumerName { get; set; }
        public string? consumerComplaintNumber { get; set; }
        public string? addressLine1 { get; set; }
        public string? addressLine2 { get; set; }
        public string? city { get; set; }
        public string? pincode { get; set; }
        public string? telephoneNumber { get; set; }
        public string? retailerPhoneNo { get; set; }
        public string? alternateTelephoneNumber { get; set; }
        public string? emailId { get; set; }
        public string? orderNumber { get; set; }
        public string? dateOfPurchase { get; set; }
        public string? dateOfComplaint { get; set; }
        public string? natureOfComplaint { get; set; }
        public string? isUnderWarranty { get; set; }
        public string? brand { get; set; }
        public string? productCategory { get; set; }
        public string? productName { get; set; }
        public string? productCode { get; set; }
        public string? model { get; set; }
        public string? identificationNo { get; set; }
        public string? dropLocation { get; set; }
        public string? dropLocAddress1 { get; set; }
        public string? dropLocAddress2 { get; set; }
        public string? dropLocCity { get; set; }
        public string? dropLocState { get; set; }
        public string? dropLocPincode { get; set; }
        public string? dropLocContactPerson { get; set; }
        public string? dropLocContactNo { get; set; }
        public string? dropLocAlternateNo { get; set; }
        public string? physicalEvaluation { get; set; }
        public string? TechEvalRequired { get; set; }
        public string? value { get; set; }
        public string? levelOfIrritation { get; set; }

    }


    public class Response
    {
        public string? success { get; set; }
    }
    public class TicketResponceDataContract
    {
        public Response? response { get; set; }
        public List<string>? ticketNo { get; set; }
        public string? message { get; set; }
        public bool success { get; set; }
        public string? apiToken { get; set; }
    }

    public class RunningBalance
    {
        public decimal TotalofInprogress { get; set; }
        public decimal TotalofDeliverd { get; set; }
        public decimal runningBalance { get; set; }
    }
}
