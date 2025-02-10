using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
    public class QuestionersV2Model : BasePageModel
    {
        #region variable declaration
        private readonly Digi2l_DevContext _dbcontext;
        IQCCommentManager _iQCCommentManager;
        IExchangeOrderRepository _ExchangeOrderRepository;
        IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _whatsAppMessageRepository;
        private readonly ITemplateConfigurationRepository _configurationRepository;
        #endregion

        #region constructor
        public QuestionersV2Model(IOptions<ApplicationSettings> config, Digi2l_DevContext dbcontext, IQCCommentManager iQCCommentManager, IExchangeOrderRepository exchangeOrderRepository, IWhatsappNotificationManager whatsappNotificationManager, IWhatsAppMessageRepository whatsAppMessageRepository, ITemplateConfigurationRepository configurationRepository) : base(config)
        {
            _dbcontext = dbcontext;
            _iQCCommentManager = iQCCommentManager;
            _ExchangeOrderRepository = exchangeOrderRepository;
            _whatsappNotificationManager = whatsappNotificationManager;
            _whatsAppMessageRepository = whatsAppMessageRepository;
            _configurationRepository = configurationRepository;
        }
        #endregion

        #region Model Binding
        [BindProperty(SupportsGet = true)]
        public QuestionerViewModel questionerViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<QCRatingViewModel> qCRatingViewModelList { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<QCRatingViewModel> qCRatingViewModel { get; set; }
        #endregion

        public IActionResult OnGet(string regdNo)
        {
            if (!string.IsNullOrEmpty(regdNo))
            {
                questionerViewModel = _iQCCommentManager.GetProductDetailsByRegdNo(regdNo);
                if (questionerViewModel != null)
                {
                    qCRatingViewModel = _iQCCommentManager.GetQuestionerReport(questionerViewModel.OrderTrandId);
                    //qCRatingViewModelList = _iQCCommentManager.GetDynamicQuestionbyProdCatId(questionerViewModel.TblProductCategory.Id);
                    qCRatingViewModelList = _iQCCommentManager.GetDynamicQuestsionbyProdCatIdV2(questionerViewModel.ProdTypeId, questionerViewModel.ProdTechId);

                    #region Get technology list and Set existing Technology
                    var techId = _dbcontext.TblProductTechnologies.Where(x => x.IsActive == true && x.ProductCatId == questionerViewModel.TblProductCategory.Id && x.IsusedNew==true).ToList();
                    if (techId != null)
                    {
                        ViewData["techId"] = new SelectList(techId, "ProductTechnologyId", "ProductTechnologyName");
                    }
                    questionerViewModel.ProductTechnologyId = questionerViewModel.ProdTechId ?? 0;
                    #endregion

                    #region Set ASP
                    var jsonResult = OnGetAverageSellingPricev2(questionerViewModel.ProdTypeId ?? 0, questionerViewModel.ProdTechId ?? 0, questionerViewModel?.TblBrand?.Id ?? 0);
                    questionerViewModel.AverageSellingPrice = Convert.ToDouble(jsonResult.Value ?? 0);
                    questionerViewModel.ExcellentPriceByASP = Convert.ToDouble(jsonResult?.SerializerSettings ?? 0);
                    #endregion

                    #region Set QC BonusCap
                    var qcBonuscap = _dbcontext.TblConfigurations.Where(x => x.IsActive == true && x.ConfigId == Convert.ToInt32(ConfigurationEnum.QCBonusCap)).FirstOrDefault();
                    if (qcBonuscap != null)
                    {
                        List<SelectListItem> dropdownList = new List<SelectListItem>();
                        for (int i = 1; i <= Convert.ToInt32(qcBonuscap.Value); i++)
                        {
                            dropdownList.Add(new SelectListItem
                            {
                                Value = i.ToString(),
                                Text = i.ToString() + "%"
                            });
                        }
                        ViewData["qcBonusCapLimit"] = dropdownList;
                    }
                    #endregion

                    #region get Quality
                    List<TblConfiguration> tblConfigurationList = _configurationRepository.GetList(x => x.IsActive == true && (x.Name == "Not Working" || x.Name == "Average" || x.Name == "Good" || x.Name == "Excellent")).ToList();
                    if (tblConfigurationList != null)
                    {
                        ViewData["Quality"] = new SelectList(tblConfigurationList, "Value", "Name");
                    }
                    #endregion
                }
                else
                {
                    questionerViewModel = new QuestionerViewModel();

                }
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            bool flag = false;
            int statuId = 0;
            TblOrderTran tblOrderTran = null;
            if (questionerViewModel.StatusId == Convert.ToInt32(OrderStatusEnum.UpperBonusCapPending))
            {
                flag = _iQCCommentManager.saveForFinalCap(questionerViewModel, qCRatingViewModelList, Convert.ToInt32(_loginSession.UserViewModel.UserId));
                if (flag == true)
                {
                    return RedirectToPage("/QCIndex/OrdersForQC");
                }
            }
            else
            {
                statuId = _iQCCommentManager.saveAndSubmit(questionerViewModel, qCRatingViewModelList, Convert.ToInt32(_loginSession.UserViewModel.UserId), flag);
                if (statuId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval))
                {
                    #region
                    tblOrderTran = _iQCCommentManager.GetOrderDetailsByOrderTransId(questionerViewModel.OrderTrandId);
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
                    string baseUrl = _baseConfig.Value.BaseURL + "PaymentDetails/ConfirmPaymentDetails?regdNo=" + tblOrderTran.Exchange.RegdNo + "&status=" + statuId;
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
                    return RedirectToPage("/QCIndex/OrdersForQC");
                }
                else
                {
                    return RedirectToPage("/QCIndex/OrdersForQC");
                }
            }
            return Page();
        }

        public IActionResult OnPostDynamicData()
        {
            bool flag = false;
            int statuId = 0;
            TblOrderTran tblOrderTran = null;
            if (questionerViewModel.StatusId == Convert.ToInt32(OrderStatusEnum.UpperBonusCapPending))
            {
                flag = _iQCCommentManager.saveForFinalCap(questionerViewModel, qCRatingViewModelList, Convert.ToInt32(_loginSession.UserViewModel.UserId));
                if (flag == true)
                {
                    return RedirectToPage("/QCIndex/OrdersForQC");
                }
            }
            else
            {
                statuId = _iQCCommentManager.saveAndSubmit(questionerViewModel, qCRatingViewModelList, Convert.ToInt32(_loginSession.UserViewModel.UserId), flag);
                if (statuId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval))
                {
                    #region
                    tblOrderTran = _iQCCommentManager.GetOrderDetailsByOrderTransId(questionerViewModel.OrderTrandId);
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
                    string baseUrl = _baseConfig.Value.BaseURL + "PaymentDetails/ConfirmPaymentDetails?regdNo=" + tblOrderTran.Exchange.RegdNo + "&status=" + statuId;
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
                    return RedirectToPage("/QCIndex/OrdersForQC");
                }
                else
                {
                    return RedirectToPage("/QCIndex/OrdersForQC");
                }
            }
            return Page();
        }

        //#region Get Average Selling Price by ProductTypeId and TechId 
        //public JsonResult OnGetAverageSellingPrice(int productTypeId, int techId, int brandId)
        //{
        //    decimal result = 0;
        //    result = _iQCCommentManager.GetASP(productTypeId, techId, brandId);
        //    return new JsonResult(result);
        //}
        //#endregion


        #region Get Average Selling Price by ProductTypeId and TechId V2
        public JsonResult OnGetAverageSellingPricev2(int productTypeId, int techId, int brandId)
        {

            //result = _iQCCommentManager.GetASP(productTypeId, techId, brandId);
            ProdCatBrandMapViewModel result = _iQCCommentManager.GetASPV2(productTypeId, techId, brandId);


            return new JsonResult(result.FinalASP,result.ExcellentPrice);
        }
        #endregion
      
        //#region Get Average Selling Price by ProductTypeId and TechId 
        //public JsonResult OnGetNonWorkingPrice(int productTypeId, int techId)
        //{
        //    decimal result = 0;
        //    result = _iQCCommentManager.GetNonWorkingPrice(productTypeId, techId);
        //    return new JsonResult(result);
        //}
        //#endregion
        #region Get NonWorking price by ProductTypeId and TechId 
        public JsonResult OnGetNonWorkingPrice(int productTypeId, int techId)
        {
            decimal result = 0;
            result = _iQCCommentManager.GetNonWorkingPrice(productTypeId, techId);
            return new JsonResult(result);
        }
        #endregion


        #region Get Quoted Price 
        public JsonResult OnPostQuotedPriceV2([FromBody] List<QCRatingViewModel> data)
        {
            dynamic? result = null;
            if (data != null && data.Count > 0)
            {
                result = _iQCCommentManager.GetNewQuotedPrice(data);
            }
            return new JsonResult(result);
        }
        #endregion


        #region get final price after QC Bonus
        public JsonResult OnGetFinalPriceAfterQCBonus(int bonusCap, double quotedPrice)
        {
            var result = _iQCCommentManager.FinalPriceAfterQCBonus(bonusCap, quotedPrice);
            return new JsonResult(result);
        }
        #endregion


        #region Get List of Questions by TechnologyId
        /// <summary>
        /// Get List of Questions by TechnologyId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public IActionResult OnGetQuestionListForQC(int? typeId, int? techId)
        //{
        //    if (typeId > 0 && techId > 0)
        //    {
        //        qCRatingViewModelList = _iQCCommentManager.GetDynamicQuestsionbyProdCatIdV2(typeId, techId);
        //    }
        //    return new JsonResult(qCRatingViewModelList);
        //}
        #endregion
        public IActionResult OnGetQuestionListForQC(int? typeId, int? techId)
        {
            if (typeId > 0 && techId > 0)
            {
                qCRatingViewModelList = _iQCCommentManager.GetDynamicQuestsionbyProdCatIdV2(typeId, techId);
            }
            var abc = Partial("_QuestionList", qCRatingViewModelList);
            //return new JsonResult(qCRatingViewModelList);
            return Partial("_QuestionList", qCRatingViewModelList);
        }
    }
}
