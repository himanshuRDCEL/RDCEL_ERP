using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.SynchronizedModel
{
    internal class OutwardReportData
    {
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class LstOutwardItemDetail
    {
        public int SNo { get; set; }
        public string Consignee { get; set; }
        public string OrderNo { get; set; }
        public string OrderDate { get; set; }
        public string DeliveryNo { get; set; }
        public string Itemcode { get; set; }
        public string ItemDesc { get; set; }
        public string EAN { get; set; }
        public string BatchNo { get; set; }
        public int MRP { get; set; }
        public int OrderQty { get; set; }
        public int UnderPicking { get; set; }
        public int PickedQty { get; set; }
        public int DispatchedQty { get; set; }
        public int AdjustedQty { get; set; }
        public int PendingQty { get; set; }
        public string OrderCreateDateTime { get; set; }
    }

    public class ResponseOutwardReportData
    {
        public List<LstOutwardItemDetail> lstOutwardItemDetails { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class RequestOutwardReportData
    {
        public string ReportType { get; set; }
        public string itemcode { get; set; }
        public string ean { get; set; }
        public string Outwardno { get; set; }
        public string Fromdate { get; set; }
        public string ToDate { get; set; }
        public string WHCode { get; set; }
    }


}
