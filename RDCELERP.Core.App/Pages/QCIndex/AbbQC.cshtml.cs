using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaToolkit.Model;
using MediaToolkit.Options;
using MediaToolkit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model;
using RDCELERP.Model.Base;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.QC;
using static RDCELERP.Model.Whatsapp.WhatsappPickupDateViewModel;
using static RDCELERP.Model.Whatsapp.WhatsappQCPriceViewModel;
using System.Collections;
using RDCELERP.BAL.Helper;

namespace RDCELERP.Core.App.Pages.QCComment
{
    public class AbbQCModel : BasePageModel
    {
        #region Variable Declaration
        IQCCommentManager _iQCCommentManager;
        IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _whatsAppMessageRepository;
        IOrderTransRepository _orderTransRepository;
        ICommonManager _commonManager;
        IQCManager _iQCManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        IImageHelper _imageHelper;
        ITemplateConfigurationRepository _templateConfigurationRepository;
        ITempDataRepository _tempDataRepository;
        #endregion

        #region Model
        [BindProperty(SupportsGet = true)]
        public QCCommentViewModel QCCommentViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<SelfQCViewModel> SelfQCVMList { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<ImageLabelViewModel> imageLabelViewModels { get; set; }

        [BindProperty(SupportsGet = true)]
        public string VideotimerSec { get; set; }

        [BindProperty(SupportsGet = true)]
        public string MaxVideoFileSizeinMB { get; set; }
        public bool? IsSelfQCDone { get; set; }
        #endregion

        #region Constructor
        public AbbQCModel(IOptions<ApplicationSettings> config, IQCCommentManager iQCCommentManager, IWhatsappNotificationManager whatsappNotificationManager, IWhatsAppMessageRepository whatsAppMessageRepository, IOrderTransRepository orderTransRepository, ICommonManager commonManager, IQCManager iQCManager, Digi2l_DevContext context, IImageHelper imageHelper, ITemplateConfigurationRepository templateConfigurationRepository, ITempDataRepository tempDataRepository) : base(config)
        {
            _iQCCommentManager = iQCCommentManager;
            _whatsappNotificationManager = whatsappNotificationManager;
            _whatsAppMessageRepository = whatsAppMessageRepository;
            _orderTransRepository = orderTransRepository;
            _commonManager = commonManager;
            _iQCManager = iQCManager;
            _context = context;
            _imageHelper = imageHelper;
            _templateConfigurationRepository = templateConfigurationRepository;
            _tempDataRepository = tempDataRepository;
        }
        #endregion
        public IActionResult OnGet(int Id)
        {
            string url = _baseConfig.Value.BaseURL;
            List<TblConfiguration?> tblConfiguration = null;
            if (Id > 0)
            {
                QCCommentViewModel = _iQCCommentManager.GetAbbOrderDetailsByTransId(Id);
                if(QCCommentViewModel != null && QCCommentViewModel.ABBRedemptionViewModel != null && QCCommentViewModel.ABBRedemptionViewModel.RegdNo != null)
                {
                    #region SelfQC Images
                    SelfQCVMList = _iQCCommentManager.GetImagesUploadedBySelfQC(QCCommentViewModel.ABBRedemptionViewModel.RegdNo);

                    #region get imageupload option by product cat
                    imageLabelViewModels = _iQCManager.GetQCImageLabels(QCCommentViewModel.ABBRedemptionViewModel.RegdNo);
                    #endregion
                    if (imageLabelViewModels == null)
                    {
                        imageLabelViewModels = new List<ImageLabelViewModel>();
                    }
                    else
                    {
                        foreach (var item in imageLabelViewModels)
                        {
                            if (item.IsMediaTypeVideo == true)
                            {
                                //isVideoTrue = item.ProductImageLabel != null ? item.ProductImageLabel : "";
                                //tblConfiguration = _context.TblConfigurations.Where(x => x.IsActive == true && x.Name == "MaxVideoFileSizeMB").FirstOrDefault();
                                //MaxVideoFileSizeinMB = tblConfiguration.Value != null ? tblConfiguration.Value : 20.ToString();                                        
                                tblConfiguration = _templateConfigurationRepository.GetConfigurationList();
                                if (tblConfiguration != null)
                                {
                                    foreach (var item2 in tblConfiguration)
                                    {
                                        if (item2.Name == ConfigurationEnum.VideoRecordingTimerSec.ToString())
                                        {
                                            VideotimerSec = item2.Value;
                                        }
                                        if (item2.Name == ConfigurationEnum.MaxVideoFileSizeMB.ToString())
                                        {
                                            MaxVideoFileSizeinMB = item2.Value;
                                        }
                                    }

                                }
                            }

                            //string productLabel = item.ProductImageLabel.ToLower();
                            //if (productLabel.Contains("video"))
                            //{
                            //    isVideoTrue = item.ProductImageLabel;
                            //}
                        }
                    }

                    #region Display Self QC Images
                    IsSelfQCDone = _iQCManager.verifyDuplicateSelfQC(QCCommentViewModel?.ABBRedemptionViewModel?.RegdNo);
                    if (IsSelfQCDone == true)
                    {
                        if (SelfQCVMList != null && SelfQCVMList.Count > 0)
                        {
                            foreach (var item in SelfQCVMList)
                            {
                                if (item != null)
                                {
                                    item.FilePath = EnumHelper.DescriptionAttr(FileAddressEnum.SelfQC);
                                    item.ImageWithPath = url + item.FilePath + item.ImageName;
                                    var extn = item?.ImageName?.Split(".").Last();
                                    if (extn == "webm")
                                    {
                                        item.IsMediaTypeVideo = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        SelfQCVMList = new List<SelfQCViewModel>();
                        SelfQCViewModel? selfQCViewModel = null;
                        ArrayList list = new ArrayList();
                        list = GetMediaFiles(QCCommentViewModel?.ABBRedemptionViewModel?.RegdNo);
                        if (list != null && list.Count > 0)
                        {
                            foreach (var item in list)
                            {
                                selfQCViewModel = new SelfQCViewModel();
                                selfQCViewModel.ImageWithPath = item?.ToString();
                                var extn = selfQCViewModel.ImageWithPath?.Split(".").Last();
                                if (extn == "webm")
                                {
                                    selfQCViewModel.IsMediaTypeVideo = true;
                                }
                                SelfQCVMList.Add(selfQCViewModel);
                            }
                        }
                    }
                    #endregion
                    #endregion

                    var enumData = from EvcPartnerPreferenceEnum e in Enum.GetValues(typeof(EvcPartnerPreferenceEnum))
                                   select new
                                   {
                                       ID = (int)e,
                                       Name = e.ToString()
                                   };
                    ViewData["StatusList"] = new SelectList(enumData, "ID", "Name");
                }
            }
            #region get flag for QC
            var statusflag = _iQCCommentManager.GetQcFlag();
            if (statusflag != null)
            {
                ViewData["statusflag"] = new SelectList(statusflag, "Id", "CombinedDisplay");
            }
            #endregion

            return Page();
        }

        public IActionResult OnPost()
        {
            TblOrderTran? tblOrderTran = null;
            TblWhatsAppMessage? tblwhatsappmessage = null;
            bool isupirequired = false;
            bool flag = false;
            string baseUrl = string.Empty;
            string url = string.Empty;

            if (QCCommentViewModel.ABBRedemptionViewModel.StatusId > 0 && QCCommentViewModel.Qccomments != null && QCCommentViewModel.ABBRedemptionViewModel.RedemptionValue > 0)
            {
                if (QCCommentViewModel.ABBProductQulity != null)
                {
                   
                        switch (QCCommentViewModel.ABBProductQulity)
                        {
                            case "1":
                                QCCommentViewModel.ABBProductQulity = "Excellent";
                                break;
                            case "2":
                                QCCommentViewModel.ABBProductQulity = "Good";
                                break;
                            case "3":
                                QCCommentViewModel.ABBProductQulity = "Average";
                                break;
                            case "4":
                                QCCommentViewModel.ABBProductQulity = "Not Working";
                                break;
                            default:
                                break;
                        }
                    
                }
                flag = _iQCCommentManager.SaveAbbQcOrder(QCCommentViewModel, imageLabelViewModels, Convert.ToInt32(_loginSession.UserViewModel.UserId));
                if(flag)
                {
                    if(QCCommentViewModel.ABBRedemptionViewModel.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval) || QCCommentViewModel.ABBRedemptionViewModel.StatusId == Convert.ToInt32(OrderStatusEnum.QCByPass))
                    {
                        tblOrderTran = _orderTransRepository.GetOrderTransByRegdno(QCCommentViewModel.ABBRedemptionViewModel.RegdNo);
                        if (tblOrderTran != null)
                        {
                            isupirequired = _commonManager.CheckUpiisRequired(tblOrderTran.OrderTransId);
                        }

                        #region whatsappNotification for UPI No and Pickup Date time

                        if (isupirequired == true)
                        {
                            WhatasappResponse whatasappresponse = new WhatasappResponse();

                            WhatsappTemplate whatsappobj = new WhatsappTemplate();
                            whatsappobj.userDetails = new UserDetails();
                            whatsappobj.notification = new QCFinalPrice();
                            whatsappobj.notification.@params = new SendDate();
                            whatsappobj.userDetails.number = QCCommentViewModel.ABBRedemptionViewModel.CustMobile;
                            whatsappobj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
                            whatsappobj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
                            whatsappobj.notification.@params.Customername = QCCommentViewModel.ABBRedemptionViewModel.CustFirstName + " " + QCCommentViewModel.ABBRedemptionViewModel.CustLastName;
                            whatsappobj.notification.templateId = NotificationConstants.WaitingForPrice_Approval_instant_settlement;// deferred_settlement Template
                            whatsappobj.notification.@params.FinalQcPrice = tblOrderTran.FinalPriceAfterQc;

                            baseUrl = _baseConfig.Value.BaseURL + "PaymentDetails/ConfirmPaymentDetails?regdNo=" + QCCommentViewModel.ABBRedemptionViewModel.RegdNo + "&status=" + QCCommentViewModel.ABBRedemptionViewModel.StatusId;
                            whatsappobj.notification.@params.PageLink = baseUrl;
                            url = _baseConfig.Value.YellowAiUrl;

                            RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappobj);
                            if (response.Content != null)
                            {
                                tblwhatsappmessage = new TblWhatsAppMessage();
                                whatasappresponse = JsonConvert.DeserializeObject<WhatasappResponse>(response.Content);
                                //tblwhatsappmessage.TemplateName = NotificationConstants.WaitingForPrice_Approval_deferred_settlement;
                                tblwhatsappmessage.TemplateName = whatsappobj.notification.templateId;
                                tblwhatsappmessage.IsActive = true;
                                tblwhatsappmessage.PhoneNumber = QCCommentViewModel.ABBRedemptionViewModel.CustMobile;
                                tblwhatsappmessage.SendDate = DateTime.Now;
                                tblwhatsappmessage.MsgId = whatasappresponse.msgId;
                                _whatsAppMessageRepository.Create(tblwhatsappmessage);
                                _whatsAppMessageRepository.SaveChanges();
                            }
                        }
                        else
                        {
                            WhatsAppResponse whatsAppResponse = new WhatsAppResponse();

                            WhatsappPickUpDateTemplate whatsappPickUpDateObj = new WhatsappPickUpDateTemplate();
                            whatsappPickUpDateObj.userDetails = new PickUpDateUserDetails();
                            whatsappPickUpDateObj.notification = new WhatsappPickupDateVM();
                            whatsappPickUpDateObj.notification.@params = new SendPickUpDate();
                            whatsappPickUpDateObj.userDetails.number = QCCommentViewModel.ABBRedemptionViewModel.CustMobile;
                            whatsappPickUpDateObj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
                            whatsappPickUpDateObj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
                            whatsappPickUpDateObj.notification.@params.Customername = QCCommentViewModel.ABBRedemptionViewModel.CustFirstName + " " + QCCommentViewModel.ABBRedemptionViewModel.CustLastName;
                            whatsappPickUpDateObj.notification.templateId = NotificationConstants.WaitingForPrice_Approval_deferred_settlement; // instant_settlement Template

                            baseUrl = _baseConfig.Value.BaseURL + "PaymentDetails/ConfirmPaymentDetails?regdNo=" + QCCommentViewModel.ABBRedemptionViewModel.RegdNo + "&status=" + QCCommentViewModel.ABBRedemptionViewModel.StatusId;
                            whatsappPickUpDateObj.notification.@params.PageLink = baseUrl;
                            url = _baseConfig.Value.YellowAiUrl;

                            RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappPickUpDateObj);

                            if (response.Content != null)
                            {
                                tblwhatsappmessage = new TblWhatsAppMessage();
                                whatsAppResponse = JsonConvert.DeserializeObject<WhatsAppResponse>(response.Content);
                                //tblwhatsappmessage.TemplateName = NotificationConstants.WaitingForPrice_Approval_deferred_settlement;
                                tblwhatsappmessage.TemplateName = whatsappPickUpDateObj.notification.templateId;
                                tblwhatsappmessage.IsActive = true;
                                tblwhatsappmessage.PhoneNumber = QCCommentViewModel.ABBRedemptionViewModel.CustMobile;
                                tblwhatsappmessage.SendDate = DateTime.Now;
                                tblwhatsappmessage.MsgId = whatsAppResponse.msgId;
                                _whatsAppMessageRepository.Create(tblwhatsappmessage);
                                _whatsAppMessageRepository.SaveChanges();
                            }
                        }

                        #endregion

                        #region whatsappNotification for UPI No
                        //WhatasappResponse whatasappResponse = new WhatasappResponse();
                        //TblWhatsAppMessage tblwhatsappmessage = null;
                        //WhatsappTemplate whatsappObj = new WhatsappTemplate();
                        //whatsappObj.userDetails = new UserDetails();
                        //whatsappObj.notification = new QCFinalPrice();
                        //whatsappObj.notification.@params = new SendDate();
                        //whatsappObj.userDetails.number = QCCommentViewModel.ABBRedemptionViewModel.CustMobile;
                        //whatsappObj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
                        //whatsappObj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
                        //if (isupirequired == true)
                        //{
                        //    whatsappObj.notification.templateId = NotificationConstants.WaitingForPrice_Approval_instant_settlement;// deferred_settlement Template
                        //}
                        //else
                        //{
                        //    whatsappObj.notification.templateId = NotificationConstants.WaitingForPrice_Approval_deferred_settlement; // instant_settlement Template                                
                        //}
                        ////whatsappObj.notification.templateId = NotificationConstants.WaitingForPrice_Approval;
                        //whatsappObj.notification.@params.Customername = QCCommentViewModel.ABBRedemptionViewModel.CustFirstName + " " + QCCommentViewModel.ABBRedemptionViewModel.CustLastName;
                        //if (QCCommentViewModel.ABBRedemptionViewModel.StatusId == (int)OrderStatusEnum.QCByPass)
                        //{
                        //    whatsappObj.notification.@params.FinalQcPrice = QCCommentViewModel.ABBRedemptionViewModel.RedemptionValue;
                        //}
                        //else
                        //{
                        //    whatsappObj.notification.@params.FinalQcPrice = QCCommentViewModel.ABBRedemptionViewModel.RedemptionValue;
                        //}

                        //string baseUrl = _baseConfig.Value.BaseURL + "PaymentDetails/ConfirmPaymentDetails?regdNo=" + QCCommentViewModel.ABBRedemptionViewModel.RegdNo + "&status=" + QCCommentViewModel.ABBRedemptionViewModel.StatusId;
                        //whatsappObj.notification.@params.PageLink = baseUrl;
                        //string url = _baseConfig.Value.YellowAiUrl;

                        //RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);

                        //if (response.Content != null)
                        //{
                        //    tblwhatsappmessage = new TblWhatsAppMessage();
                        //    whatasappResponse = JsonConvert.DeserializeObject<WhatasappResponse>(response.Content);
                        //    tblwhatsappmessage.TemplateName = whatsappObj.notification.templateId;
                        //    //tblwhatsappmessage.TemplateName = NotificationConstants.WaitingForPrice_Approval;
                        //    tblwhatsappmessage.IsActive = true;
                        //    tblwhatsappmessage.PhoneNumber = QCCommentViewModel.ABBRedemptionViewModel.CustMobile;
                        //    tblwhatsappmessage.SendDate = DateTime.Now;
                        //    tblwhatsappmessage.MsgId = whatasappResponse.msgId;
                        //    _whatsAppMessageRepository.Create(tblwhatsappmessage);
                        //    _whatsAppMessageRepository.SaveChanges();
                        //}
                        #endregion

                    }
                    else
                    {
                        return RedirectToPage("/ABBRedemption/RedemptionRecord");
                    }
                    return RedirectToPage("/ABBRedemption/RedemptionRecord");
                }
                else
                {
                    return RedirectToPage("/ABBRedemption/RedemptionRecord");
                }
            }
            return RedirectToPage("/ABBRedemption/RedemptionRecord");
        }

        #region SaveSelfQCImageAsFinalQCImage 
        public JsonResult OnPostSaveSelfQCImageAsFinalQCImage(string RegdNo)
        {
            return new JsonResult(_iQCCommentManager.saveSelfQCImageAsFinalImage(RegdNo, Convert.ToInt32(_loginSession.UserViewModel.UserId)));
        }
        #endregion

        #region Video Compressor Added by Pooja Jatav
        [ValidateAntiForgeryToken]
        public JsonResult OnPostCompressVideo(string fileName, string regdNo, bool isMediaTypeVideo, int srNum)
        {
            try
            {
                string bytedata = null;
                bool videoSave = false;
                if (fileName != null)
                {
                    byte[] videobyte = Convert.FromBase64String(fileName);

                    string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string tempInputFilePath = System.IO.Path.Combine(documentsPath, "input_video.mp4");
                    System.IO.File.WriteAllBytes(tempInputFilePath, videobyte);
                    string outputFilePath = System.IO.Path.Combine(documentsPath, "compressed_video.mp4");
                    var inputFile = new MediaFile { Filename = tempInputFilePath };
                    var outputFile = new MediaFile { Filename = outputFilePath };
                    //using (var engine = new Engine())
                    using (var engine = new Engine())
                    {
                        engine.GetMetadata(inputFile);
                        var options = new ConversionOptions
                        {
                            VideoAspectRatio = VideoAspectRatio.R3_2,
                            VideoSize = VideoSize.Hd480,
                            //AudioSampleRate = AudioSampleRate.Hz22050,
                            VideoBitRate = 620,
                            VideoFps = 54,
                        };
                        engine.Convert(inputFile, outputFile, options);
                    }

                    Task<byte[]> compressedfile = System.IO.File.ReadAllBytesAsync(outputFilePath);

                    System.IO.File.Delete(tempInputFilePath);
                    System.IO.File.Delete(outputFilePath);

                    if (compressedfile != null)
                    {
                        if (compressedfile.Result.Length > 0)
                        {
                            string videoBase64 = Convert.ToBase64String(compressedfile.Result);
                            string FileName = regdNo + "_" + "FinalQCVideo_" + srNum + ".webm";
                            string filePath = EnumHelper.DescriptionAttr(FilePathEnum.VideoQC);
                            videoSave = _imageHelper.SaveVideoFileFromBase64(videoBase64, filePath, FileName);
                        }
                    }

                    //bytedata = Convert.ToBase64String(compressedfile.Result);
                    bytedata = "videofilesaved";
                    return new JsonResult(bytedata);
                }
                else
                {
                    return new JsonResult(fileName);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Get Media Files

        public JsonResult OnPostGetMediaFiles(string? regdNo)
        {
            ArrayList list = new ArrayList();
            JsonResult jsonResult = null;
            try
            {
                if (!string.IsNullOrEmpty(regdNo))
                {
                    list = GetMediaFiles(regdNo);
                }
                jsonResult = new JsonResult(list);
            }
            catch (Exception ex)
            {
            }
            return jsonResult;
        }

        public ArrayList GetMediaFiles(string? regdNo)
        {
            var fileName = "";
            var count = 1;
            List<ImageLabelViewModel>? imageLabelViewModels = null;
            List<TblTempDatum>? tempData = null;
            var placeholderPath = "";
            string fullFilePath = "";
            ArrayList list = new ArrayList();
            try
            {

                string? baseUrl = _baseConfig.Value.BaseURL;
                string documentsPath = string.Concat(baseUrl, "DBFiles/QC/SelfQC/");
                tempData = _tempDataRepository.GetMediaFilesTempDataList(regdNo);
                imageLabelViewModels = _iQCManager.GetQCImageLabels(regdNo);
                if (imageLabelViewModels != null && imageLabelViewModels.Count > 0)
                {
                    foreach (var imageLabel in imageLabelViewModels)
                    {
                        if (imageLabel != null)
                        {
                            if (imageLabel.IsMediaTypeVideo == true)
                            {
                                fileName = regdNo + "_Video_" + count + ".webm";
                            }
                            else
                            {
                                fileName = regdNo + "_Image_" + count + ".jpg";
                            }
                            var tempDataObj = tempData?.FirstOrDefault(x => x.FileName == fileName);
                            if (tempDataObj != null && !string.IsNullOrEmpty(tempDataObj.FileName))
                            {
                                fullFilePath = string.Concat(documentsPath, tempDataObj.FileName);
                            }
                            else
                            {
                                if (imageLabel.IsMediaTypeVideo == true)
                                {
                                    placeholderPath = "Images/VIDEO-placeholder.png";
                                }
                                else
                                {
                                    placeholderPath = "Images/placeholder-01.png";
                                }
                                fullFilePath = string.Concat(baseUrl, placeholderPath);
                            }
                            list.Add(fullFilePath);
                        }
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }
        #endregion
    }
}
