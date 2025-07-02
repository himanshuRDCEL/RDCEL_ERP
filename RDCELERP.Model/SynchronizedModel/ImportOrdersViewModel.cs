using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.SynchronizedModel
{
    public class ImportOrdersViewModel
    {
        public List<LstOrderItemDetail> lstOrderItemDetail { get; set; }
    }
    public class LstOrderItemDetail
    {
        public string orderNo { get; set; }
        public string deliveryNo { get; set; }
        public string deliveryDate { get; set; }
        public string itemCode { get; set; }
        public string eanNo { get; set; }
        public int itemMRP { get; set; }
        public int billingAmount { get; set; }
        public int qty { get; set; }
        public string consignee { get; set; }
        public string consigneeName { get; set; }
        public string createdBy { get; set; }
        public string projectCode { get; set; }
        public string whCode { get; set; }
    }

   

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ResponseImportOrders
    {
        public string status { get; set; }
        public string msg { get; set; }
        public object flag { get; set; }
        public string refno { get; set; }
    }


}
