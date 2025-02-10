using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.QCComment;
using static RDCELERP.Model.Whatsapp.WhatsappQCPriceViewModel;

namespace RDCELERP.Core.App.Pages.QCIndex
{
    public class UpperBonusCapModel : BasePageModel
    {
        #region Variables
        IQCCommentManager _iQCCommentManager;
        private readonly Digi2l_DevContext _dbcontext;
        IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _whatsAppMessageRepository;
        #endregion

        #region constructor
        public UpperBonusCapModel(IOptions<ApplicationSettings> config, IQCCommentManager iQCCommentManager, Digi2l_DevContext dbcontext, IWhatsappNotificationManager whatsappNotificationManager, IWhatsAppMessageRepository whatsAppMessageRepository) : base(config)
        {
            _iQCCommentManager = iQCCommentManager;
            _dbcontext = dbcontext;
            _whatsappNotificationManager = whatsappNotificationManager;
            _whatsAppMessageRepository = whatsAppMessageRepository;
        }
        #endregion

        #region Models
        [BindProperty]
        public AdminBonusCapViewModel adminBonusCapViewModel { get; set; }

        [BindProperty]
        public List<QCRatingViewModel> qCRatingViewModelList { get; set; }
        #endregion

        public IActionResult OnGet(int OrderTransId)
        {
            if(OrderTransId > 0)
            {
                adminBonusCapViewModel = _iQCCommentManager.GetOrderDetailsPendingForUpperCap(OrderTransId);
                qCRatingViewModelList = _iQCCommentManager.GetQuestionerReportByQCTeam(OrderTransId);
                var upperBonuscap = _dbcontext.TblConfigurations.Where(x => x.IsActive == true && x.ConfigId == Convert.ToInt32(ConfigurationEnum.UpperBonusCap)).FirstOrDefault();
                if (upperBonuscap != null)
                {
                    List<SelectListItem> dropdownList = new List<SelectListItem>();
                    for (int i = 1; i <= Convert.ToInt32(upperBonuscap.Value); i++)
                    {
                        dropdownList.Add(new SelectListItem
                        {
                            Value = i.ToString(),
                            Text = i.ToString() + "%"
                        });
                    }

                    ViewData["upperBonusCapLimit"] = dropdownList;
                }
            }
            else
            {
                return Page();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            bool flag;
            TblOrderTran tblOrderTran = null;

            if(adminBonusCapViewModel !=null && qCRatingViewModelList !=null)
            {
                flag = _iQCCommentManager.SaveBonusDetailByAdimn(adminBonusCapViewModel, qCRatingViewModelList, Convert.ToInt32(_loginSession.UserViewModel.UserId));
                if(flag == true)
                {
                    #region
                    tblOrderTran = _iQCCommentManager.GetOrderDetailsByOrderTransId(adminBonusCapViewModel.OrderTransId);
                    #endregion
                    #region whatsappNotification for UPI No
                    WhatasappResponse whatasappResponse = new WhatasappResponse();
                    TblWhatsAppMessage tblwhatsappmessage = null;
                    string message = string.Empty;

                    WhatsappTemplate whatsappObj = new WhatsappTemplate();
                    whatsappObj.userDetails = new UserDetails();
                    whatsappObj.notification = new QCFinalPrice();
                    whatsappObj.notification.@params = new SendDate();
                    whatsappObj.userDetails.number = tblOrderTran.Exchange.CustomerDetails.PhoneNumber;
                    whatsappObj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
                    whatsappObj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
                    whatsappObj.notification.templateId = NotificationConstants.WaitingForPrice_Approval_instant_settlement;// deferred_settlement Template
                    whatsappObj.notification.@params.Customername = tblOrderTran.Exchange.CustomerDetails.FirstName + " " + tblOrderTran.Exchange.CustomerDetails.LastName;
                    whatsappObj.notification.@params.FinalQcPrice = tblOrderTran.FinalPriceAfterQc;
                    string baseUrl = _baseConfig.Value.BaseURL + "PaymentDetails/ConfirmPaymentDetails?regdNo=" + tblOrderTran.Exchange.RegdNo + "&status=" + tblOrderTran.StatusId;
                    whatsappObj.notification.@params.PageLink = baseUrl;
                    string url = _baseConfig.Value.YellowAiUrl;

                    RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                    int statusCode = Convert.ToInt32(response.StatusCode);
                    if (response.Content != null && statusCode == 202)
                    {
                        tblwhatsappmessage = new TblWhatsAppMessage();
                        whatasappResponse = JsonConvert.DeserializeObject<WhatasappResponse>(response.Content);
                        tblwhatsappmessage.TemplateName = NotificationConstants.WaitingForPrice_Approval_instant_settlement;// deferred_settlement Template
                        tblwhatsappmessage.IsActive = true;
                        tblwhatsappmessage.PhoneNumber = tblOrderTran.Exchange.CustomerDetails.PhoneNumber;
                        tblwhatsappmessage.SendDate = DateTime.Now;
                        tblwhatsappmessage.MsgId = whatasappResponse.msgId;
                        _whatsAppMessageRepository.Create(tblwhatsappmessage);
                        _whatsAppMessageRepository.SaveChanges();
                    }
                    #endregion
                    return RedirectToPage("/QCIndex/AdminBonus");
                }
            }
            else
            {
                return RedirectToPage("/QCIndex/AdminBonus");
            }
            return Page();
        }

        #region get final price after QC Admin Bonus
        public JsonResult OnGetFinalPriceAfterQCAdminBonus(int bonusCap, double quotedPrice)
        {
            var result = _iQCCommentManager.FinalPriceAfterQCBonus(bonusCap, quotedPrice);
            return new JsonResult(result);
        }
        #endregion
    }
}
