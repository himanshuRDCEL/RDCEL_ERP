using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.TicketGenrateModel.Bizlog
{
    public class DstAdd
    {
        public string? dstContactPerson { get; set; }
        public string? dstOrg { get; set; }
        public string? dstAdd1 { get; set; }
        public string? dstAdd2 { get; set; }
        public string? dstLocation { get; set; }
        public string? dstContact1 { get; set; }
        public string? dstContact2 { get; set; }
        public string? dstCity { get; set; }
        public string? dstState { get; set; }
        public string? dstPincode { get; set; }
        public string? dstLandmark { get; set; }
        public string? dstEmailId { get; set; }
        public string? dstEmailIdAlt { get; set; }
        public string? dstType { get; set; }
    }

    public class Primary
    {
        public string? ticketPriority { get; set; }
        public string? flowId { get; set; }
        public string? retailerId { get; set; }
        public string? retailerNo { get; set; }
        public string? conComplaintNo { get; set; }
        public string? orderNo { get; set; }
        public string? invoiceNo { get; set; }
        public string? parentTicket { get; set; }
        public string? ticketDetails { get; set; }
        public string? specialInstructions { get; set; }
        public string? buyerContactNumber { get; set; }
        public string? isAppointmentReq { get; set; }
        public string? isInstallationReq { get; set; }
        public string? isPhysicalEval { get; set; }
        public string? isTechEval { get; set; }
        public string? isPackingReq { get; set; }
        public string? paymentMode { get; set; }
        public string? productId { get; set; }
        public string? productCode { get; set; }
        public string? brandId { get; set; }
        public string? modelId { get; set; }
        public string? brandName { get; set; }
        public string? modelName { get; set; }
        public string? productName { get; set; }
        public string? dateOfPurchase { get; set; }
        public string? identificationNo { get; set; }
        public string? productDesc { get; set; }
        public string? problemDesc { get; set; }
        public string? productValue { get; set; }
        public string? cost { get; set; }
        public string? isUnderWarranty { get; set; }
        public string? accessories { get; set; }
        public string? pickupType { get; set; }
        public string? grade { get; set; }
        public string? estWeight { get; set; }
    }

    public class PrimaryDetails
    {
    }

    public class Product
    {
        public Primary? primary { get; set; }
        public ProductDetails? product_details { get; set; }
        public SrcAdd? src_add { get; set; }
        public DstAdd? dst_add { get; set; }
    }

    public class ProductDetails
    {
    }

    public class UpdatedTicketDataContract
    {
        public Primary? primary { get; set; }
        public PrimaryDetails? primary_details { get; set; }
        public List<Product>? products { get; set; }
    }

    public class SrcAdd
    {
        public string? srcContactPerson { get; set; }
        public string? srcOrg { get; set; }
        public string? srcAdd1 { get; set; }
        public string? srcAdd2 { get; set; }
        public string? srcLocation { get; set; }
        public string? srcContact1 { get; set; }
        public string? srcContact2 { get; set; }
        public string? srcCity { get; set; }
        public string? srcState { get; set; }
        public string? srcPincode { get; set; }
        public string? srcLandmark { get; set; }
        public string? srcEmailId { get; set; }
        public string? srcEmailIdAlt { get; set; }
        public string? srcType { get; set; }
    }
    public class Data
    {
        public string? ticketNo { get; set; }
        public List<ProductError>? productErrors { get; set; }
    }
    public class ProductError
    {
        public string? dstCity { get; set; }
    }
    public class UpdatedTicketResponceDataContract
    {
        public bool success { get; set; }
        public int statusCode { get; set; }
        public string? msg { get; set; }
        public Data? data { get; set; }
    }
}
