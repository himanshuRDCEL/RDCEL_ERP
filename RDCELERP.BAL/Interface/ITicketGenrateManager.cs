using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.TicketGenrateModel;
using RDCELERP.Model.TicketGenrateModel.Bizlog;
using RDCELERP.Model.TicketGenrateModel.Mahindra;

namespace RDCELERP.BAL.Interface
{
    public interface ITicketGenrateManager
    {
        public CustomerandOrderDetailsDataContract SetOrderDetailsObjectForABB(TblAbbredemption aBBRedemption);
        public CustomerandOrderDetailsDataContract SetOrderDetailsObject(TblOrderTran tblOrderTran);
        public EVCdetailsDataContract SetEvcDetailsDataContract(TblEvcregistration evcRegistration);
        public UpdatedTicketDataContract UpdatedSetTicketObjInfo(CustomerandOrderDetailsDataContract customerObj, EVCdetailsDataContract evcDataObj);
        public UpdatedTicketResponceDataContract UpdatedProcessTicketInfo(UpdatedTicketDataContract updatedticketDataContract);
        public UpdatedTicketResponceDataContract AddUpdatedTicketToBizlog(UpdatedTicketDataContract UpdatedticketDataContract);

        public int AddUpdatedTicketToDB(UpdatedTicketDataContract updatedticketDataContract, string BizlogTicketNo);
        public string Rest_InvokeBizlogSeviceFormData(string url, Method methodType, object content = null, string ticketNo = null);
        public string Rest_InvokeUPdatedBlowHornServiceFormData(string url, Method methodType, object content = null, string ticketNo = null);


        public TblBizlogTicket SetUpdatedTicketObjectDBJson(UpdatedTicketDataContract ticketDataContract, string BizlogTicketNo);

        public int AddTicketToTblLogistics(string ticketNumber, int transId, int servicepartnerId, string regdno, CustomerandOrderDetailsDataContract customerDetails, int userId, int evcRegistrationId);

        public MahindraLogisticsResponseDataContract ProcessLogisticsRequest(MahindraLogisticsDataContract mahindraDC);
        public MahindraLogisticsDataContract SetMahindraObj(CustomerandOrderDetailsDataContract customerObj, EVCdetailsDataContract evcObj);

        public int AddMahindraRequestToDB(MahindraLogisticsDataContract mahindraLogisticsDataContract, string mahindraLogisticsawbNo);

        public TblMahindraLogistic SetMahindraObjectDBJson(MahindraLogisticsDataContract mahindraLogisticsDataContract, string mahindraLogisticsawbNo);

        public string Rest_InvokeMahindraServiceFormData(string url, Method methodType, object content = null, string ticketNo = null);
        public HttpResponseMessage CreateTicketWithBizlog(string RegdNo, string priority, int servicePartnerId,int userid);

        public HttpResponseMessage GenerateTicketForLocalLgcPartner(string RegdNo, int servicePartnerId, int userid);

        public HttpResponseMessage RequestMahindraLGC(string RegdNo, int servicePartnerId, int userid);
    }
}
