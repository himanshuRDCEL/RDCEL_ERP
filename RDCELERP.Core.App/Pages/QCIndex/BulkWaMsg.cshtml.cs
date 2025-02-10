using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;
using RDCELERP.Core.App.Pages.Base;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.BAL.Interface;
using RDCELERP.Model.Company;
using RDCELERP.Model.SearchFilters;
using Microsoft.AspNetCore.Mvc.Rendering;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using DocumentFormat.OpenXml.Wordprocessing;
using static RDCELERP.Model.Whatsapp.WhatsappSelfqcViewModel;
using WhatasappResponse = RDCELERP.Model.Whatsapp.WhatsappSelfqcViewModel.WhatasappResponse;
using UserDetails = RDCELERP.Model.Whatsapp.WhatsappSelfqcViewModel.UserDetails;
using static Org.BouncyCastle.Math.EC.ECCurve;
using RDCELERP.Common.Constant;
using RestSharp;
using Newtonsoft.Json;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.Core.App.Pages.QCIndex
{
    public class BulkWaMsgModel : BasePageModel
    {
        #region Variable declartion
        private readonly Digi2l_DevContext _context;
        IOptions<ApplicationSettings> _config;
        IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _WhatsAppMessageRepository;
        ILogging _logging;
        #endregion

        #region Constructor
        public BulkWaMsgModel(IOptions<ApplicationSettings> config, Digi2l_DevContext context, IWhatsappNotificationManager whatsappNotificationManager, IWhatsAppMessageRepository whatsAppMessageRepository, ILogging logging)
       : base(config)
        {
            _config = config;
            _context = context;
            _whatsappNotificationManager = whatsappNotificationManager;
            _WhatsAppMessageRepository = whatsAppMessageRepository;
            _logging = logging;
        }
        #endregion

        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }
        public void OnGet()
        {
           
        }
        public IActionResult OnPost()
        {
            List<TblExchangeOrder> tblExchangeOrderList = new List<TblExchangeOrder>();
            WhatasappResponse? whatasappResponse = null;
            TblWhatsAppMessage? tblwhatsappmessage = null;
            //string baseurl = "https://utcbridge.com/ERP/";
            //string baseurl = "https://techutcdigital.com/ERP_QA/";
            string? baseurl = _config?.Value?.BaseURL;
            string SelfQCLink = "";
            int linkSendCount = 0;
            int inActiveCount = 0;
            try
            {
                #region Get Exchange Orders List
                tblExchangeOrderList = _context.TblExchangeOrders
                                .Include(x => x.CustomerDetails)
                               .Where(x => x.IsActive == true
                              && (
                              x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCInProgress_3Q)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.CallAndGoScheduledAppointmentTaken_3P)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenOrder)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.InstalledbySponsor)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderWithDiagnostic)
                                )).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.Id).ToList();
                #endregion

                #region Get Exchange Orders List
                if (tblExchangeOrderList != null && tblExchangeOrderList.Count > 0)
                {
                    foreach (TblExchangeOrder item in tblExchangeOrderList)
                    {
                        if (item != null)
                        {
                            if (item.CustomerDetails != null)
                            {
                                if (string.IsNullOrEmpty(item.CustomerDetails.PhoneNumber))
                                {
                                    _logging.WriteErrorToDB("BulkWaMsg", item.RegdNo + " Customer_PhoneNumber_Not_Found");
                                }
                                else
                                {
                                    try
                                    {
                                        if (item.CustomerDetails.IsActive == true)
                                        {
                                            SelfQCLink = baseurl + "QCPortal/SelfQC?regdno=" + item.RegdNo;
                                            #region code to send selfqc link on whatsappNotification
                                            WhatsappTemplate whatsappObj = new WhatsappTemplate();
                                            whatsappObj.userDetails = new UserDetails();
                                            whatsappObj.notification = new SelfQC();
                                            whatsappObj.notification.@params = new URL();
                                            whatsappObj.userDetails.number = item.CustomerDetails.PhoneNumber;
                                            whatsappObj.notification.sender = _config.Value.YelloaiSenderNumber;
                                            whatsappObj.notification.type = _config.Value.YellowaiMesssaheType;
                                            whatsappObj.notification.templateId = NotificationConstants.SelfQC_Link;
                                            whatsappObj.notification.@params.Link = SelfQCLink;
                                            whatsappObj.notification.@params.CustomerName = item.CustomerDetails.FirstName + " " + item.CustomerDetails.LastName;
                                            string url = _config.Value.YellowAiUrl;
                                            RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                                            if (response.Content != null)
                                            {
                                                whatasappResponse = JsonConvert.DeserializeObject<WhatasappResponse>(response.Content);
                                                tblwhatsappmessage = new TblWhatsAppMessage();
                                                tblwhatsappmessage.TemplateName = NotificationConstants.SelfQC_Link;
                                                tblwhatsappmessage.IsActive = true;
                                                tblwhatsappmessage.PhoneNumber = item.CustomerDetails.PhoneNumber;
                                                tblwhatsappmessage.SendDate = DateTime.Now;
                                                tblwhatsappmessage.MsgId = whatasappResponse.msgId;
                                                _WhatsAppMessageRepository.Create(tblwhatsappmessage);
                                                _WhatsAppMessageRepository.SaveChanges();
                                            }
                                            #endregion
                                            linkSendCount++;
                                        }
                                        else
                                        {
                                            inActiveCount++;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logging.WriteErrorToDB("BulkWaMsg", "OnPostReSendSelfQCLinkToCustomer", ex);
                                    }
                                }
                            }
                            else
                            {
                                _logging.WriteErrorToDB("BulkWaMsg", item.RegdNo +" Customer_Details_Not_Found");
                            }
                        }
                        else
                        {
                            _logging.WriteErrorToDB("BulkWaMsg", item.RegdNo + " Exchange_Details_Not_Found");
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BulkWaMsg", "OnPostBulkSendSelfQCLinkToCustomer", ex);
            }
            return Page();
        }
    }
}
