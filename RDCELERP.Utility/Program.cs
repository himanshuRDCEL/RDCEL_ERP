using System;
using System.Net;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Utility.Utilities;

namespace RDCELERP.Utility
{
    class Program
    {
        static void Main(string[] args)
        {
            Program obj = new Program();
            System.Net.ServicePointManager.SecurityProtocol =
            SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; 
            bool flag = false;
            InvoiceController InvPageModel = new InvoiceController(new LogisticManager());
            flag = InvPageModel.GenerateInvoiceForEVC();
        }
    }
}
