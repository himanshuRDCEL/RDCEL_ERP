using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.SynchronizedModel
{
 
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class StockReportDataViewModel
    {
        public string Reporttype { get; set; }
        public string Itemcode { get; set; }
        public string EAN { get; set; }
        public string Locationcode { get; set; }
    }

    public class ResponseStockReportDataViewModel
    {
        [JsonProperty("lstItemDetails")]
        public List<LstStockReport> LstStockReportDetails { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class LstStockReport
    {
        public int SNo { get; set; }
        public string LocationCode { get; set; }
        public string Itemcode { get; set; }
        public string EAN { get; set; }
        public string ItemDesc { get; set; }
        public int MRP { get; set; }
        public int Qty { get; set; }
        public int PendingDispatch { get; set; }
        public int AvailableStock { get; set; }
        public string Brand { get; set; }
        public string SubCat { get; set; }
        public string Size { get; set; }
        public string Colour { get; set; }
        public string Condition { get; set; }
        public string Gender { get; set; }
        public string Division { get; set; }
        public string Section { get; set; }
        public string Department { get; set; }
        public double CostPrice { get; set; }
        public double RSP { get; set; }
        public string ManageBatchItem { get; set; }
        public string ExpiryDate { get; set; }
        public string UOM { get; set; }
        public string HSN { get; set; }
        public string HSNID { get; set; }
    }

   




}
