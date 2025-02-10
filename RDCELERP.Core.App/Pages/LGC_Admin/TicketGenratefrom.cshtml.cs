using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Entities;
using TblLogistic = RDCELERP.DAL.Entities.TblLogistic;
using TblOrderTran = RDCELERP.DAL.Entities.TblOrderTran;
using RDCELERP.Common.Enums;
using TblExchangeOrder = RDCELERP.DAL.Entities.TblExchangeOrder;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.TicketGenrateModel;
using TblAbbredemption = RDCELERP.DAL.Entities.TblAbbredemption;
using TblWalletTransaction = RDCELERP.DAL.Entities.TblWalletTransaction;
using TblEvcregistration = RDCELERP.DAL.Entities.TblEvcregistration;
using RDCELERP.Model.TicketGenrateModel.Bizlog;
using SendGrid;
using RDCELERP.Model.TicketGenrateModel.Mahindra;
using Newtonsoft.Json;
using RDCELERP.Model.EVC_Allocated;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;
using SelectListItem = Microsoft.AspNetCore.Mvc.Rendering.SelectListItem;
using SelectList = Microsoft.AspNetCore.Mvc.Rendering.SelectList;
using TblServicePartner = RDCELERP.DAL.Entities.TblServicePartner;
using Org.BouncyCastle.Asn1.Ocsp;
using ResponseData = RDCELERP.Model.TicketGenrateModel.ResponseData;
using DocumentFormat.OpenXml.Drawing;
using SendGrid.Helpers.Mail;
using RDCELERP.Model.LGC;


namespace RDCELERP.Core.App.Pages.LGC_Admin
{
    public class TicketGenratefromModel : CrudBasePageModel
    {
        #region variable declaration
        private readonly IExchangeOrderManager _ExchangeOrderManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStateManager _stateManager;
        private readonly ICityManager _cityManager;
        private readonly IBrandManager _brandManager;
        private readonly IBusinessPartnerManager _businessPartnerManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        private readonly IPinCodeManager _pinCodeManager;
        private readonly IQCCommentManager _QcCommentManager;
        private readonly ILogisticManager _logisticManager;
        IOrderQCRepository _orderQCRepository;
        IOrderTransRepository _orderTransRepository;
        INotificationManager _notificationManager;
        IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _WhatsAppMessageRepository;
        IExchangeOrderRepository _exchangeOrderRepository;
        ICustomerDetailsRepository _customerDetailsRepository;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly ILogisticsRepository _logisticsRepository;
        private readonly ITicketGenrateManager _ticketGenrateManager;
        private readonly IABBRedemptionRepository _aBBRedemptionRepository;
        private readonly IWalletTransactionRepository _walletTransactionRepository;
        private readonly IEVCRepository _eVCRepository;
        private readonly IServicePartnerRepository _servicePartnerRepository;
        public readonly IPushNotificationManager _pushNotificationManager;
        public readonly IOptions<ApplicationSettings> _baseConfig;






        #endregion

        #region constructor
        public TicketGenratefromModel(IQCCommentManager QcCommentManager, IPinCodeManager pinCodeManager, IStoreCodeManager storeCodeManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IProductTypeManager productTypeManager, IExchangeOrderManager exchangeOrderManager, IProductCategoryManager productCategoryManager, IBusinessPartnerManager businessPartnerManager, IBrandManager brandManager, IStateManager StateManager, ICityManager CityManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, IUserManager userManager, CustomDataProtection protector, IOrderQCRepository orderQCRepository, IOrderTransRepository orderTransRepository, ICustomerDetailsRepository customerDetailsRepository, IExchangeOrderRepository exchangeOrderRepository, IWhatsAppMessageRepository whatsAppMessageRepository, IWhatsappNotificationManager whatsappNotificationManager, ILogisticsRepository logisticsRepository, ITicketGenrateManager ticketGenrateManager, IABBRedemptionRepository aBBRedemptionRepository, IWalletTransactionRepository walletTransactionRepository, IEVCRepository eVCRepository,ILogisticManager logisticManager,IServicePartnerRepository servicePartnerRepository, IPushNotificationManager pushNotificationManager, IOptions<ApplicationSettings> baseConfig) : base(config, protector)

        {
            _webHostEnvironment = webHostEnvironment;
            _ExchangeOrderManager = exchangeOrderManager;
            _stateManager = StateManager;
            _cityManager = CityManager;
            _brandManager = brandManager;
            _businessPartnerManager = businessPartnerManager;
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
            _pinCodeManager = pinCodeManager;
            _QcCommentManager = QcCommentManager;
            _orderQCRepository = orderQCRepository;
            _orderTransRepository = orderTransRepository;
            _customerDetailsRepository = customerDetailsRepository;
            _exchangeOrderRepository = exchangeOrderRepository;
            _WhatsAppMessageRepository = whatsAppMessageRepository;
            _whatsappNotificationManager = whatsappNotificationManager;
            _context = context;
            _logisticsRepository = logisticsRepository;
            _ticketGenrateManager = ticketGenrateManager;
            _aBBRedemptionRepository = aBBRedemptionRepository;
            _walletTransactionRepository = walletTransactionRepository;
            _eVCRepository = eVCRepository;
            _logisticManager = logisticManager;
            _servicePartnerRepository = servicePartnerRepository;
            _pushNotificationManager = pushNotificationManager;
            _baseConfig = baseConfig;
        }
        #endregion

        [BindProperty(SupportsGet = true)]
        public string RegnoList { get; set; }
        [BindProperty(SupportsGet = true)]
        public ServicePartnerLogin loginobj { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<RegdNoList> RegdNoLists { get; set; }

        HttpResponseMessage ResponseData { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<ResponseData> ResponseDataList { get; set; }
        public IActionResult OnGet(string ids)
        {
            if (ids != null && ids != "")
            {
                loginobj.RegdNo = ids;
                string[] OrderTranIDList = ids.Split(",");
                foreach (var itemId in OrderTranIDList)
                {
                    RegdNoLists.Add(new RegdNoList { RegdNo = itemId });
                }

                //var SelectServiceP = _logisticManager.SelectServicePartner();
                //if (SelectServiceP != null)
                //{
                //    ViewData["SelectService"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(SelectServiceP, "ServicePartnerId", "ServicePartnerName");
                //}

            }
            else
            {
                var responseData = TempData["ResponseDataList"] as string;
                var responseDataList = JsonConvert.DeserializeObject<List<ResponseData>>(responseData);
                ViewData["ResponseDataList1"] = responseDataList;
            }
            //if (ResponseDataList != null)
            //{
            //    List<ResponseData> responseDataList = JsonConvert.DeserializeObject<List<ResponseData>>(ResponseDataList);
            //    ViewData["ResponseDataList1"] = responseDataList;
            //}
            return Page();
        }
      
        public IActionResult OnPost()
        {
            var UserId = _loginSession.UserViewModel.UserId;
            int Count = 0;
            if (loginobj != null)
            {
                string[] OrderTranIDList = loginobj.RegdNo.Split(",");
               
                foreach (var itemId in OrderTranIDList)
                {
                    #region Bizlog
                    if (loginobj.ServicePartnerId == Convert.ToInt32(ServicePartnerEnum.Bizlog))
                    {
                        ResponseData = _ticketGenrateManager.CreateTicketWithBizlog(itemId, loginobj.priority, loginobj.ServicePartnerId, UserId);
                        // Inside the foreach loop
                        ResponseData responseData = new ResponseData
                        {
                            Regno = itemId,
                            ServicePartner = "Bizlog",
                            StatusCode = (int)ResponseData.StatusCode,
                            Content = ResponseData.Content.ReadAsStringAsync().Result
                        };

                        ContentData contentData = JsonConvert.DeserializeObject<ContentData>(responseData.Content);

                        // Fill the properties in the ResponseData model
                        if (responseData.StatusCode == 200) // Assuming 200 indicates a successful response
                        {
                            responseData.Success = contentData?.Detail?.Success ?? true;
                            responseData.Message = contentData?.Detail?.Msg != null ? contentData?.Detail?.Msg : responseData.Message = contentData.Message;
                            responseData.TicketNo = contentData?.Detail?.Data?.TicketNo;
                            responseData.ProductErrors = contentData?.Detail?.Data?.ProductErrors;
                        }
                        else
                        {
                            // Handle the failed response
                            responseData.Success = false;
                            responseData.Message = contentData.Message;
                            responseData.TicketNo = string.Empty;
                            responseData.ProductErrors = string.Empty;
                        }
                        ResponseDataList.Add(responseData);
                    }
                    #endregion

                    #region Mahindra
                    else if (loginobj.ServicePartnerId == Convert.ToInt32(ServicePartnerEnum.Mahindra))
                    {
                        ResponseData = _ticketGenrateManager.RequestMahindraLGC(itemId, loginobj.ServicePartnerId, UserId);
                        // Inside the foreach loop
                        ResponseData responseData = new ResponseData
                        {
                            ServicePartner = "Mahindra",
                            Regno = itemId,
                            StatusCode = (int)ResponseData.StatusCode,
                            Content = ResponseData.Content.ReadAsStringAsync().Result
                        };

                        ContentData contentData = JsonConvert.DeserializeObject<ContentData>(responseData.Content);

                        // Fill the properties in the ResponseData model
                        if (responseData.StatusCode == 200) // Assuming 200 indicates a successful response
                        {
                            responseData.Success = contentData?.Status ?? false;
                            responseData.Message = contentData?.Message;
                            //responseData.TicketNo = contentData?.Detail?.Data?.TicketNo;
                            responseData.awbNumber = contentData.Detail.awbNumber;
                            responseData.ProductErrors = contentData?.Detail?.Data?.ProductErrors;
                        }
                        else
                        {
                            // Handle the failed response
                            responseData.Success = false;
                            responseData.Message = contentData.Message;
                            responseData.TicketNo = string.Empty;
                            responseData.ProductErrors = string.Empty;
                        }
                        ResponseDataList.Add(responseData);
                    }
                    #endregion

                    #region Others
                    else if (loginobj.IsServicePartnerLocal == true)
                    {
                        ResponseData = _ticketGenrateManager.GenerateTicketForLocalLgcPartner(itemId, loginobj.ServicePartnerId, UserId);
                        TblServicePartner tblServicePartner = _context.TblServicePartners.Where(x => x.ServicePartnerId == loginobj.ServicePartnerId).FirstOrDefault();
                        // Inside the foreach loop
                        ResponseData responseData = new ResponseData
                        {
                            ServicePartner = tblServicePartner.ServicePartnerDescription,
                            Regno = itemId,
                            StatusCode = (int)ResponseData.StatusCode,
                            Content = ResponseData.Content.ReadAsStringAsync().Result
                        };
                        dynamic contentData = JsonConvert.DeserializeObject(responseData.Content);

                        // Fill the properties in the ResponseData model
                        if (responseData.StatusCode == 200) // Assuming 200 indicates a successful response
                        {
                            responseData.Success = contentData?.Status ?? false;
                            responseData.Message = contentData?.Message;
                            responseData.TicketNo = contentData?.Detail;
                            Count++;
                        }
                        else
                        {
                            // Handle the failed response
                            responseData.Success = false;
                            responseData.Message = contentData.Message;
                            responseData.TicketNo = string.Empty;
                            responseData.ProductErrors = string.Empty;
                        }
                        ResponseDataList.Add(responseData);
                    }
                    #endregion
                }
            }
            var responseDataJson = JsonConvert.SerializeObject(ResponseDataList);
            TempData["ResponseDataList"] = responseDataJson;
            if (_baseConfig.Value.SendPushNotification == true)
            {
                if (Count > 0)
                {
                    var Notification = _pushNotificationManager.SendNotification(loginobj.ServicePartnerId, null, EnumHelper.DescriptionAttr(NotificationEnum.OrderAssignedbyDigi2L), Count.ToString(), null);
                }
            }

            return RedirectToPage("/LGC_Admin/TicketGenratefrom", new { ids = "" });
            //  return RedirectToAction("TicketGenratefrom", "LGC_Admin", new { ids = "" });
            // return RedirectToPage("/LGC_Admin/TicketGenratefrom", new {ids="", ResponseDataList = JsonConvert.SerializeObject(ResponseDataList) });
            //return RedirectToPage("/LGC_Admin/TicketGenratefrom", new { ids = "", ResponseDataList = ResponseDataList });
        }

        public JsonResult OnGetGetServicepartnerDetails(int servicepartnerId)
        {
            bool flag = false;

            DAL.Entities.TblServicePartner srvicePartner = _servicePartnerRepository.GetSingle(x => x.ServicePartnerId == servicepartnerId);
            if (srvicePartner.IsServicePartnerLocal == true)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            return new JsonResult(flag);

        }

        
        public JsonResult OnGetGetOrderPriorityList(int servicepartnerId)
        {
            List<SelectListItem> priorityList = new List<SelectListItem>();
            if (servicepartnerId == Convert.ToInt32(ServicePartnerEnum.Bizlog))
            {
                priorityList.Insert(0, new SelectListItem() { Value = "high", Text = "High" });
                priorityList.Insert(0, new SelectListItem() { Value = "medium", Text = "Medium" });
                priorityList.Insert(0, new SelectListItem() { Value = "low", Text = "Low" });
            }
            var result = new SelectList(priorityList, "Value", "Text");
            return new JsonResult(result );
        }

        public JsonResult OnGetGetPriorityNeddedforpartner(int servicepartnerId)
        {
            bool flag = false;

            TblServicePartner srvicePartner = _servicePartnerRepository.GetSingle(x => x.ServicePartnerId == servicepartnerId);
            if (servicepartnerId == Convert.ToInt32(ServicePartnerEnum.Bizlog))
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            return new JsonResult(flag);
        }

        public IActionResult OnGetSearchServicePartner(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblServicePartners.Where(x => x.IsActive == true 
            && x.ServicePartnerIsApprovrd == true && x.IsServicePartnerLocal != null 
            && (term == "#" || x.ServicePartnerName.Contains(term)))
                          .Select(s => new SelectListItem
                          {
                              Value = s.ServicePartnerName,
                              Text = s.ServicePartnerId.ToString()
                          })
                      .ToArray();

            return new JsonResult(data);
        }
    }
}
