using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RDCELERP.BAL.MasterManager.WhatsappNotificationManager;

namespace RDCELERP.BAL.Interface
{
    public interface IWhatsappNotificationManager
    {
        public RestResponse Rest_InvokeWhatsappserviceCall(string url, Method methodType, object content = null);

        public  Task<Model.Whatsapp.HttpResponseDetails> SendWhatsAppMessageAsync(string templateId, string recipientNumber, List<string> templateParams);
    }
}
