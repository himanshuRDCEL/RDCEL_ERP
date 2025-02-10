using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.ImageLabel;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.QC;
using static RDCELERP.Model.Whatsapp.WhatsappPostSelfQCAlertViewModel;
using static RDCELERP.Model.Whatsapp.WhatsappSelfqcViewModel;

namespace RDCELERP.BAL.MasterManager
{
    public class QCManager : IQCManager
    {
        #region variable declaration
        IExchangeOrderRepository _exchangeOrderRepository;
        IABBRedemptionRepository _abbRedemptionRepository;
        IAbbRegistrationRepository _abbRegistrationRepository;
        ICustomerDetailsRepository _customerDetailsRepository;
        IImageLabelRepository _imageLabelRepository;
        IProductTypeRepository _productTypeRepository;
        IProductCategoryRepository _productCategoryRepository;
        IOrderImageUploadRepository _orderImageUploadRepository;
        ILovRepository _lovRepository;
        IWebHostEnvironment _webHostEnvironment;
        ISelfQCRepository _selfQCRepository;
        IMapper _mapper;
        ILogging _logging;
        IOrderTransRepository _orderTransRepository;
        IExchangeABBStatusHistoryRepository _exchangeABBStatusHistoryRepository;
        IOptions<ApplicationSettings> _baseConfig;
        ICommonManager _commonManager;
        IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _WhatsAppMessageRepository;
        IImageHelper _imageHelper;
        IBusinessUnitRepository _banginessUnitRepository;
        ITempDataRepository _tempDataRepository;
        #endregion

        #region Constructor

        public QCManager(IExchangeOrderRepository exchangeOrderRepository, IImageLabelRepository imageLabelRepository, IProductTypeRepository productTypeRepository, IWebHostEnvironment webHostEnvironment, IMapper mapper, ILogging logging, IABBRedemptionRepository abbRedemptionRepository, ISelfQCRepository selfQCRepository, ILovRepository lovRepository, IOrderImageUploadRepository orderImageUploadRepository, ICustomerDetailsRepository customerDetailsRepository, IProductCategoryRepository productCategoryRepository, IAbbRegistrationRepository abbRegistrationRepository, IOrderTransRepository orderTransRepository, IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository, IOptions<ApplicationSettings> baseConfig, ICommonManager commonManager, IWhatsAppMessageRepository whatsAppMessageRepository, IWhatsappNotificationManager whatsappNotificationManager, IImageHelper imageHelper, IBusinessUnitRepository banginessUnitRepository, ITempDataRepository tempDataRepository)

        {
            _exchangeOrderRepository = exchangeOrderRepository;
            _imageLabelRepository = imageLabelRepository;
            _productTypeRepository = productTypeRepository;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _logging = logging;
            _abbRedemptionRepository = abbRedemptionRepository;
            _selfQCRepository = selfQCRepository;
            _lovRepository = lovRepository;
            _orderImageUploadRepository = orderImageUploadRepository;
            _customerDetailsRepository = customerDetailsRepository;
            _productCategoryRepository = productCategoryRepository;
            _abbRegistrationRepository = abbRegistrationRepository;
            _orderTransRepository = orderTransRepository;
            _exchangeABBStatusHistoryRepository = exchangeABBStatusHistoryRepository;
            _baseConfig = baseConfig;
            _commonManager = commonManager;

            _WhatsAppMessageRepository = whatsAppMessageRepository;
            _whatsappNotificationManager = whatsappNotificationManager;
            _imageHelper = imageHelper;
            _banginessUnitRepository = banginessUnitRepository;
            _tempDataRepository = tempDataRepository;
        }

        #endregion

        #region method to get the pic of customer at selfQC
        /// <summary>
        /// method to get the pic of customer at selfQC
        /// </summary>
        /// <param name="regno"></param>
        /// <param name="business"></param>
        /// <returns></returns>
        public List<ImageLabelViewModel> SelfQC(string regdno)
        {
            TblExchangeOrder tblExchangeOrder = null;
            TblProductType tblProductType = null;
            List<ImageLabelViewModel> imageLabelViewModels = null;
            List<TblImageLabelMaster> tblImageLabel = new List<TblImageLabelMaster>();
            string baseUrl = _baseConfig.Value.BaseURL;
            TblOrderTran tblOrderTran = null;
            TblAbbregistration tblAbbregistration = null;
            try
            {
                if (!string.IsNullOrEmpty(regdno))
                {
                    tblOrderTran = _orderTransRepository.GetSingle(x => x.IsActive == true && (x.RegdNo??"").ToLower() == regdno.ToLower());
                    if (tblOrderTran != null)
                    {
                        if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.ABB))
                        {
                            tblAbbregistration = _abbRegistrationRepository.GetSingle(x => x.IsActive == true && x.RegdNo == tblOrderTran.RegdNo);
                            if (tblAbbregistration != null)
                            {
                                tblProductType = _productTypeRepository.GetSingle(x => x.IsActive == true && x.Id == tblAbbregistration.NewProductCategoryTypeId);
                            }
                            else
                            {
                                tblAbbregistration = new TblAbbregistration();
                            }
                        }
                        else if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                        {
                            tblExchangeOrder = _exchangeOrderRepository.GetSingle(x => x.IsActive == true && x.RegdNo == tblOrderTran.RegdNo);
                            if (tblExchangeOrder != null)
                            {
                                tblProductType = _productTypeRepository.GetSingle(x => x.IsActive == true && x.Id == tblExchangeOrder.ProductTypeId);
                            }
                            else
                            {
                                tblExchangeOrder = new TblExchangeOrder();
                            }
                        }
                        if (tblProductType != null)
                        {
                            tblImageLabel = _imageLabelRepository.GetList(x => x.IsActive == true && x.ProductCatId == tblProductType.ProductCatId).ToList();
                        }
                        imageLabelViewModels = new List<ImageLabelViewModel>();
                        if (tblImageLabel.Count > 0 && tblImageLabel != null)
                        {
                            imageLabelViewModels = _mapper.Map<List<TblImageLabelMaster>, List<ImageLabelViewModel>>(tblImageLabel);
                            foreach (var item in imageLabelViewModels)
                            {
                                item.RegdNo = tblOrderTran.RegdNo;
                                item.BusinessType = "Exchange";
                                if (item.ImagePlaceHolder != null)
                                {
                                    item.FullPlaceHolderImageUrl = baseUrl + EnumHelper.DescriptionAttr(FileAddressEnum.ImageLabelMaster) + item.ImagePlaceHolder;
                                }
                            }
                        }
                        else
                        {
                            imageLabelViewModels = new List<ImageLabelViewModel>();
                        }
                        return imageLabelViewModels;
                    }
                    else
                    {
                        imageLabelViewModels = new List<ImageLabelViewModel>();
                    }
                }
            }
            catch (Exception ex)
            {

                _logging.WriteErrorToDB("QCManager", "SelfQC", ex);
            }
            return imageLabelViewModels;
        }
        #endregion

        #region add customer provided images to DB

        /// <summary>
        /// add customer provided images to DB
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="imageLabelViewModel"></param>
        /// <returns>bool</returns>
        //public bool AddSelfQCImageToDB(List<ImageLabelViewModel> imageLabelViewModels,string SelfQCVideo)
        public bool AddSelfQCImageToDB(SelfQcVideoImageViewModel selfQcVideoImageViewModel)
        {
            TblExchangeOrder tblExchangeOrder = null;
            TblAbbredemption tblAbbredemption = null;
            TblOrderTran tblOrderTran = null;
            TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = null;
            TblSelfQc tblSelfQc = null;
            string businessExchange = "Exchange";
            string businessAbb = "ABB";
            int totalCount = 1;
            int resultCount = 0;
            int base64StringCount = 0;
            //status chck for 3C an 3RA
            int setStatusId = Convert.ToInt32(OrderStatusEnum.SelfQCbyCustomer);
            bool flag = false;
            TblCustomerDetail? tblCustomerDetail = null;
            try
            {
                if (selfQcVideoImageViewModel.imageLabelViewModels.Count > 0)
                {
                    string regdNo = selfQcVideoImageViewModel.imageLabelViewModels.FirstOrDefault().RegdNo;
                    tblOrderTran = _orderTransRepository.GetSingle(x => x.IsActive == true && x.RegdNo == regdNo);
                    bool verifySelfQC = verifyDuplicateSelfQC(regdNo);
                    if (!verifySelfQC)
                    {
                        for (int i = 0; i < selfQcVideoImageViewModel.imageLabelViewModels.Count; i++)
                        {
                            if (selfQcVideoImageViewModel.imageLabelViewModels[i].Base64StringValue != null)
                            {
                                base64StringCount += 1;
                            }
                        }
                        if (base64StringCount == selfQcVideoImageViewModel.imageLabelViewModels.Count)
                        {
                            tblExchangeOrder = _exchangeOrderRepository.GetSingle(x => x.IsActive == true && x.RegdNo == regdNo);
                            tblAbbredemption = _abbRedemptionRepository.GetSingle(x => x.IsActive == true && x.RegdNo == regdNo);
                            //chk for 3C and 3RA status 
                            if (tblExchangeOrder != null && (tblExchangeOrder.StatusId == Convert.ToInt32(OrderStatusEnum.CustomerNotResponding_3C)
                                 || tblExchangeOrder.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentRescheduled_3RA)))
                            {
                                setStatusId = Convert.ToInt32(OrderStatusEnum.ReopenOrder);
                            }
                            if (tblAbbredemption != null && (tblAbbredemption.StatusId == Convert.ToInt32(OrderStatusEnum.CustomerNotResponding_3C)
                                 || tblAbbredemption.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentRescheduled_3RA)))
                            {
                                setStatusId = Convert.ToInt32(OrderStatusEnum.ReopenOrder);
                            }
                            foreach (var items in selfQcVideoImageViewModel.imageLabelViewModels)
                            {
                                tblSelfQc = new TblSelfQc();
                                if (items.IsMediaTypeVideo == true)
                                {
                                    string VideoName = items.RegdNo + "_" + "Video_" + totalCount + ".webm";
                                    tblSelfQc.ImageName = VideoName;
                                    var VideofilePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"\DBFiles\QC\SelfQC");
                                    //Check if directory exist
                                    if (!Directory.Exists(VideofilePath))
                                    {
                                        Directory.CreateDirectory(VideofilePath); //Create directory if it doesn't exist
                                    }
                                    var VideofileNameWithPath = string.Concat(VideofilePath, "\\", VideoName);
                                    byte[] videoBytes = Convert.FromBase64String(items.Base64StringValue);
                                    File.WriteAllBytes(VideofileNameWithPath, videoBytes);
                                    selfQcVideoImageViewModel.SelfQCVideo = null;
                                }
                                else if (items.Base64StringValue != null)
                                {
                                    items.FileName = items.RegdNo + "_" + "Image_" + totalCount + ".jpg";
                                    tblSelfQc.ImageName = items.FileName;
                                    var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"\DBFiles\QC\SelfQC");
                                    //Check if directory exist
                                    if (!Directory.Exists(filePath))
                                    {
                                        Directory.CreateDirectory(filePath); //Create directory if it doesn't exist
                                    }
                                    var fileNameWithPath = string.Concat(filePath, "\\", items.FileName);
                                    byte[] imageBytes = Convert.FromBase64String(items.Base64StringValue);
                                    File.WriteAllBytes(fileNameWithPath, imageBytes);
                                }

                                tblSelfQc.RegdNo = items.RegdNo;
                                if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                                {
                                    tblSelfQc.ExchangeOrderId = tblExchangeOrder.Id;
                                    tblSelfQc.UserId = selfQcVideoImageViewModel.LoginId != null ? (selfQcVideoImageViewModel.LoginId > 0 ? selfQcVideoImageViewModel.LoginId : 0) : 0;
                                    tblSelfQc.IsActive = true;
                                    tblSelfQc.IsExchange = true;
                                    tblSelfQc.IsAbb = false;
                                }
                                else if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.ABB))
                                {
                                    tblSelfQc.RedemptionId = tblAbbredemption.RedemptionId;
                                    tblSelfQc.UserId = selfQcVideoImageViewModel.LoginId != null ? (selfQcVideoImageViewModel.LoginId > 0 ? selfQcVideoImageViewModel.LoginId : 0) : 0;
                                    tblSelfQc.IsActive = true;
                                    tblSelfQc.IsExchange = false;
                                    tblSelfQc.IsAbb = true;
                                }
                                tblSelfQc.CreatedDate = System.DateTime.Now;
                                _selfQCRepository.Create(tblSelfQc);
                                totalCount += 1;
                                resultCount += 1;
                            }
                            _selfQCRepository.SaveChanges();

                            //Shashi

                            #region update statusid in tblOrderTran
                            if (tblOrderTran != null)
                            {
                                tblOrderTran.StatusId = setStatusId;
                                tblOrderTran.ModifiedDate = DateTime.Now;
                                _orderTransRepository.Update(tblOrderTran);
                                _orderTransRepository.SaveChanges();

                                tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.ABB))
                                {
                                    #region update statusid in tblabbredemption
                                    tblAbbredemption.StatusId = setStatusId;
                                    tblAbbredemption.ModifiedDate = DateTime.Now;
                                    _abbRedemptionRepository.Update(tblAbbredemption);
                                    _abbRedemptionRepository.SaveChanges();
                                    #endregion

                                    #region insert history in tblexchangeabbhistorytable
                                    tblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(LoVEnum.ABB);
                                    tblExchangeAbbstatusHistory.SponsorOrderNumber = tblAbbredemption.Sponsor != null ? tblAbbredemption.Sponsor : string.Empty;
                                    tblExchangeAbbstatusHistory.RegdNo = tblAbbredemption.RegdNo;
                                    tblExchangeAbbstatusHistory.ZohoSponsorId = tblAbbredemption.ZohoAbbredemptionId != null ? Convert.ToString(tblAbbredemption.ZohoAbbredemptionId) : string.Empty;
                                    tblExchangeAbbstatusHistory.CustId = tblAbbredemption.CustomerDetailsId != null ? tblAbbredemption.CustomerDetailsId : 0;
                                    tblExchangeAbbstatusHistory.StatusId = setStatusId;
                                    tblExchangeAbbstatusHistory.IsActive = true;
                                    tblExchangeAbbstatusHistory.CreatedDate = DateTime.Now;
                                    tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                                    _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                                    _exchangeABBStatusHistoryRepository.SaveChanges();
                                    #endregion

                                }
                                else if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                                {
                                    #region update statusid in tblexchangeorder
                                    tblExchangeOrder.StatusId = setStatusId;
                                    tblExchangeOrder.ModifiedDate = DateTime.Now;
                                    _exchangeOrderRepository.Update(tblExchangeOrder);
                                    _exchangeOrderRepository.SaveChanges();
                                    #endregion

                                    #region insert history in tblexchangeabbhistorytable
                                    tblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(LoVEnum.Exchange);
                                    tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                                    tblExchangeAbbstatusHistory.RegdNo = tblExchangeOrder.RegdNo;
                                    tblExchangeAbbstatusHistory.ZohoSponsorId = tblExchangeOrder.ZohoSponsorOrderId != null ? tblExchangeOrder.ZohoSponsorOrderId : string.Empty; ;
                                    tblExchangeAbbstatusHistory.CustId = tblExchangeOrder.CustomerDetailsId;
                                    tblExchangeAbbstatusHistory.StatusId = tblExchangeOrder.StatusId;
                                    tblExchangeAbbstatusHistory.IsActive = true;
                                    tblExchangeAbbstatusHistory.CreatedDate = DateTime.Now;
                                    tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                                    _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                                    _exchangeABBStatusHistoryRepository.SaveChanges();
                                    #endregion
                                }
                                if (tblExchangeAbbstatusHistory != null && tblExchangeAbbstatusHistory.CustId != null && tblExchangeAbbstatusHistory.CustId > 0)
                                {
                                    tblCustomerDetail = _customerDetailsRepository.GetCustDetails(tblExchangeAbbstatusHistory.CustId);
                                    if (tblCustomerDetail != null && tblCustomerDetail.PhoneNumber != null)
                                    {
                                        #region code to send selfqc link on whatsappNotification
                                        WhatsAppResponse whatasappResponse = new WhatsAppResponse();
                                        WhatsAppTemplate whatsappObj = new WhatsAppTemplate();
                                        TblWhatsAppMessage tblwhatsappmessage = null;

                                        whatsappObj.userDetails = new SelfQCUserDetails();
                                        whatsappObj.notification = new PostSelfQC();
                                        whatsappObj.userDetails.number = tblCustomerDetail.PhoneNumber;
                                        whatsappObj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
                                        whatsappObj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
                                        whatsappObj.notification.templateId = NotificationConstants.PostSelfQCAlert;
                                        string url = _baseConfig.Value.YellowAiUrl;
                                        RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                                        if (response.Content != null)
                                        {
                                            whatasappResponse = JsonConvert.DeserializeObject<WhatsAppResponse>(response.Content);
                                            tblwhatsappmessage = new TblWhatsAppMessage();
                                            tblwhatsappmessage.TemplateName = NotificationConstants.PostSelfQCAlert;
                                            tblwhatsappmessage.IsActive = true;
                                            tblwhatsappmessage.PhoneNumber = tblCustomerDetail.PhoneNumber;
                                            tblwhatsappmessage.SendDate = DateTime.Now;
                                            tblwhatsappmessage.MsgId = whatasappResponse.msgId;
                                            _WhatsAppMessageRepository.Create(tblwhatsappmessage);
                                            _WhatsAppMessageRepository.SaveChanges();
                                        }
                                        #endregion
                                    }

                                }
                            }
                            #endregion
                            if (resultCount == selfQcVideoImageViewModel.imageLabelViewModels.Count)
                                return flag = true;
                            else
                                return flag = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCManager", "AddImageToDB", ex);
            }
            return flag;
        }
        #endregion

        #region method to get the images uploaded by customer during selfQC
        /// <summary>
        /// method to get the images uploaded by customer during selfQC
        /// </summary>
        /// <param name="regno"></param>
        /// <returns> List </returns>

        public List<SelfQCViewModel> GetImageUploadedByCustomer(string regno)
        {
            List<TblSelfQc> tblSelfQC = null;
            List<SelfQCViewModel> selfQCViewModels = new List<SelfQCViewModel>();

            try
            {
                tblSelfQC = _selfQCRepository.GetList(x => x.RegdNo == regno).ToList();
                selfQCViewModels = _mapper.Map<List<TblSelfQc>, List<SelfQCViewModel>>(tblSelfQC);
                return selfQCViewModels;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCManager", "GetImageUploadedByCustomer", ex);
            }
            return selfQCViewModels;
        }
        #endregion

        #region method to save image to tblOrderImageUpload on radio selection of customer uploaded
        /// <summary>
        /// method to save image to tblOrderImageUpload on radio selection of customer uploaded
        /// </summary>
        /// <param name="regno"></param>
        /// <returns> bool </returns>
        public bool saveCustomerUploadedImage(string regno)
        {
            string LovName = "Customer";
            List<TblSelfQc> tblSelfQC = null;
            bool flag = false;
            int Count = 0;
            TblOrderImageUpload tblOrderImageUploads = null;

            try
            {
                tblSelfQC = _selfQCRepository.GetList(x => x.RegdNo == regno).ToList();
                TblLoV tblLoV = _lovRepository.GetSingle(x => x.LoVname.ToLower() == LovName.ToLower());
                foreach (var items in tblSelfQC)
                {
                    tblOrderImageUploads = new TblOrderImageUpload();
                    tblOrderImageUploads.ImageName = items.ImageName;
                    tblOrderImageUploads.ImageUploadby = tblLoV.LoVid;
                    tblOrderImageUploads.IsActive = true;
                    tblOrderImageUploads.CreatedDate = DateTime.Now;
                    _orderImageUploadRepository.Create(tblOrderImageUploads);
                    _orderImageUploadRepository.SaveChanges();
                    Count += 1;
                }
                if (Count == tblSelfQC.Count)
                {
                    return flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCManager", "GetImageUploadedByCustomer", ex);
            }
            return flag;
        }
        #endregion

        #region get customer name,product cat,product type
        /// <summary>
        /// get customer name,product cat,product type
        /// </summary>
        /// <param name="regdno"></param>
        /// <returns></returns>
        public SelfQCExchangeDetailsViewModel getOrderDetailsbyRegdno(string regdno)
        {
            SelfQCExchangeDetailsViewModel selfQCExchangeDetailsViewModel = new SelfQCExchangeDetailsViewModel();
            TblExchangeOrder tblExchangeOrder = null;
            TblAbbredemption tblAbbredemption = null;
            TblOrderTran tblOrderTran = null;
            try
            {
                if (!string.IsNullOrEmpty(regdno))
                {
                    tblOrderTran = _orderTransRepository.GetSingle(x => x.IsActive == true && x.RegdNo.ToLower() == regdno.ToLower());
                    if (tblOrderTran != null)
                    {
                        selfQCExchangeDetailsViewModel.OrderTransId = tblOrderTran.OrderTransId;
                        selfQCExchangeDetailsViewModel.StatusId = tblOrderTran.StatusId;
                        if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.ABB))
                        {
                            tblAbbredemption = _abbRedemptionRepository.GetSingle(x => x.IsActive == true && x.RegdNo == tblOrderTran.RegdNo);
                            if (tblAbbredemption != null)
                            {
                                TblAbbregistration tblAbbregistration = _abbRegistrationRepository.GetSingle(x => x.IsActive == true && x.AbbregistrationId == tblAbbredemption.AbbregistrationId);
                                TblProductCategory tblProductCategory = _productCategoryRepository.GetSingle(x => x.IsActive == true && x.Id == tblAbbregistration.NewProductCategoryId);
                                selfQCExchangeDetailsViewModel.CustomerName = tblAbbregistration.CustFirstName + " " + tblAbbregistration.CustLastName;
                                selfQCExchangeDetailsViewModel.ProductCategory = tblProductCategory.Description;
                                selfQCExchangeDetailsViewModel.RegdNo = tblAbbregistration.RegdNo;
                                return selfQCExchangeDetailsViewModel;
                            }
                            else
                            {
                                return selfQCExchangeDetailsViewModel;
                            }
                        }
                        else if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                        {
                            tblExchangeOrder = _exchangeOrderRepository.GetSingle(x => x.IsActive == true && x.RegdNo == tblOrderTran.RegdNo);
                            if (tblExchangeOrder != null)
                            {
                                TblCustomerDetail tblCustomerDetail = _customerDetailsRepository.GetSingle(x => x.IsActive == true && x.Id == tblExchangeOrder.CustomerDetailsId);
                                TblProductType tblProductType = _productTypeRepository.GetSingle(x => x.IsActive == true && x.Id == tblExchangeOrder.ProductTypeId);
                                TblProductCategory tblProductCategory = _productCategoryRepository.GetSingle(x => x.IsActive == true && x.Id == tblProductType.ProductCatId);
                                selfQCExchangeDetailsViewModel.CustomerName = tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName;
                                selfQCExchangeDetailsViewModel.ProductCategory = tblProductCategory.Description;
                                selfQCExchangeDetailsViewModel.RegdNo = tblExchangeOrder.RegdNo;
                                return selfQCExchangeDetailsViewModel;
                            }
                            else
                            {
                                return selfQCExchangeDetailsViewModel;
                            }
                        }
                    }
                }
                return selfQCExchangeDetailsViewModel;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCManager", "getOrderDetailsbyRegdno", ex);
            }
            return selfQCExchangeDetailsViewModel;
        }
        #endregion

        #region verify duplicate selfqc
        /// <summary>
        /// verify duplicate selfqc
        /// </summary>
        /// <param name="regdno"></param>
        /// <returns>falg</returns>
        public bool verifyDuplicateSelfQC(string regdno)
        {
            bool flag = false;
            List<TblExchangeAbbstatusHistory> exchangeAbbHistory = null;
            try
            {
                exchangeAbbHistory = _exchangeABBStatusHistoryRepository.GetHistoryByRegdNo(regdno);
                exchangeAbbHistory = exchangeAbbHistory.Where(x=> x.StatusId == Convert.ToInt32(OrderStatusEnum.AmountApprovedbyCustomerAfterQC) 
                || x.StatusId == Convert.ToInt32(OrderStatusEnum.SelfQCbyCustomer) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCByPass) || x.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval)).ToList();
                if (exchangeAbbHistory != null && exchangeAbbHistory.Count > 0)
                {
                    flag = true;
                }
            }
            catch(Exception ex)
            {
                _logging.WriteErrorToDB("QCManager", "verifyDuplicateSelfQC", ex);
            }
            return flag;
        }
        #endregion


        #region Method to get the Vedio of customer at selfQC by Kranti
        /// <summary>
        /// method to get the pic of customer at selfQC
        /// </summary>
        /// <param name="regno"></param>
        /// <param name="business"></param>
        /// <returns></returns>
        public List<ImageLabelViewModel> SelfQCFlipkart(string regdno)
        {
            TblExchangeOrder tblExchangeOrder = null;
            TblProductType tblProductType = null;
            List<ImageLabelViewModel> imageLabelViewModels = null;
            List<TblImageLabelMaster> tblImageLabel = new List<TblImageLabelMaster>();
            string baseUrl = _baseConfig.Value.BaseURL;
            TblOrderTran tblOrderTran = null;
            TblAbbregistration tblAbbregistration = null;
            try
            {
                if (!string.IsNullOrEmpty(regdno))
                {
                    tblOrderTran = _orderTransRepository.GetSingle(x => x.IsActive == true && (x.RegdNo ?? "").ToLower() == regdno.ToLower());
                    if (tblOrderTran != null)
                    {
                        if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.ABB))
                        {
                            tblAbbregistration = _abbRegistrationRepository.GetSingle(x => x.IsActive == true && x.RegdNo == tblOrderTran.RegdNo);
                            if (tblAbbregistration != null)
                            {
                                tblProductType = _productTypeRepository.GetSingle(x => x.IsActive == true && x.Id == tblAbbregistration.NewProductCategoryTypeId);
                            }
                            else
                            {
                                tblAbbregistration = new TblAbbregistration();
                            }
                        }
                        else if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                        {
                            tblExchangeOrder = _exchangeOrderRepository.GetSingle(x => x.IsActive == true && x.RegdNo == tblOrderTran.RegdNo);
                            if (tblExchangeOrder != null)
                            {
                                tblProductType = _productTypeRepository.GetSingle(x => x.IsActive == true && x.Id == tblExchangeOrder.ProductTypeId);
                            }
                            else
                            {
                                tblExchangeOrder = new TblExchangeOrder();
                            }
                        }
                        if (tblProductType != null)
                        {
                            tblImageLabel = _imageLabelRepository.GetList(x => x.IsActive == true && x.ProductCatId == tblProductType.ProductCatId && (x.IsMediaTypeVideo == true)).ToList();
                        }
                        imageLabelViewModels = new List<ImageLabelViewModel>();
                        if (tblImageLabel.Count > 0 && tblImageLabel != null)
                        {
                            imageLabelViewModels = _mapper.Map<List<TblImageLabelMaster>, List<ImageLabelViewModel>>(tblImageLabel);
                            foreach (var item in imageLabelViewModels)
                            {
                                item.RegdNo = tblOrderTran.RegdNo;
                                item.BusinessType = "Exchange";
                                if (item.ImagePlaceHolder != null)
                                {
                                    item.FullPlaceHolderImageUrl = baseUrl + EnumHelper.DescriptionAttr(FileAddressEnum.ImageLabelMaster) + item.ImagePlaceHolder;
                                }
                            }
                        }
                        else
                        {
                            imageLabelViewModels = new List<ImageLabelViewModel>();
                        }
                        return imageLabelViewModels;
                    }
                    else
                    {
                        imageLabelViewModels = new List<ImageLabelViewModel>();
                    }
                }
            }
            catch (Exception ex)
            {

                _logging.WriteErrorToDB("QCManager", "SelfQC", ex);
            }
            return imageLabelViewModels;
        }
        #endregion
        #region Self QC Enhancements Added by VK
        #region Get User Id Which user is Send the Self QC Link to the Customer.
        /// <summary>
        /// Get User Id Which user is Send the Self QC Link to the Customer.
        /// </summary>
        /// <param name="regdno"></param>
        /// <returns>UserId</returns>
        public int? GetSelfLinkSenderUserId(string regdno)
        {
            int? userId = null;
            TblOrderTran? tblOrderTrans = null;
            try
            {
                tblOrderTrans = _orderTransRepository.GetRegdno(regdno);
                if (tblOrderTrans != null)
                {
                    userId = tblOrderTrans?.SelfQclinkResendby;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCManager", "GetSelfLinkSenderUserId", ex);
            }
            return userId;
        }
        #endregion

        #region Save the customer provided images to DB
        /// <summary>
        /// Save the customer provided images to DB
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="imageLabelViewModel"></param>
        /// <returns>bool</returns>
        //public bool UpdateSelfQCImageToDB(SelfQcVideoImageViewModel selfQcVideoImageViewModel)
        //{
        //    TblExchangeOrder? tblExchangeOrder = null;
        //    TblAbbredemption? tblAbbredemption = null;
        //    TblOrderTran? tblOrderTran = null;
        //    TblExchangeAbbstatusHistory? tblExchangeAbbstatusHistory = null;
        //    TblSelfQc? tblSelfQc = null;
        //    string businessExchange = "Exchange";
        //    string businessAbb = "ABB";
        //    int totalCount = 1;
        //    int resultCount = 0;
        //    int base64StringCount = 0;
        //    //status chck for 3C an 3RA
        //    int setStatusId = Convert.ToInt32(OrderStatusEnum.SelfQCbyCustomer);
        //    bool flag = false;
        //    TblCustomerDetail? tblCustomerDetail = null;
        //    try
        //    {
        //        if (selfQcVideoImageViewModel != null && selfQcVideoImageViewModel.imageLabelViewModels != null)
        //        {
        //            if (selfQcVideoImageViewModel.imageLabelViewModels.Count > 0)
        //            {
        //                string regdNo = selfQcVideoImageViewModel.imageLabelViewModels.FirstOrDefault().RegdNo;
        //                tblOrderTran = _orderTransRepository.GetRegdno(regdNo);
        //                bool verifySelfQC = verifyDuplicateSelfQC(regdNo);
        //                if (tblOrderTran != null && ((!verifySelfQC) || tblOrderTran.SelfQclinkResendby > 0))
        //                {
        //                    for (int i = 0; i < selfQcVideoImageViewModel.imageLabelViewModels.Count; i++)
        //                    {
        //                        if (selfQcVideoImageViewModel.imageLabelViewModels[i].Base64StringValue != null)
        //                        {
        //                            base64StringCount += 1;
        //                        }
        //                    }
        //                    if (base64StringCount == selfQcVideoImageViewModel.imageLabelViewModels.Count)
        //                    {
        //                        tblExchangeOrder = _exchangeOrderRepository.GetExchOrderByRegdNo(regdNo);
        //                        tblAbbredemption = _abbRedemptionRepository.GetRegdNo(regdNo);
        //                        //chk for 3C and 3RA status 
        //                        if (tblExchangeOrder != null && (tblExchangeOrder.StatusId == Convert.ToInt32(OrderStatusEnum.CustomerNotResponding_3C)
        //                             || tblExchangeOrder.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentRescheduled_3RA)))
        //                        {
        //                            setStatusId = Convert.ToInt32(OrderStatusEnum.ReopenOrder);
        //                        }
        //                        if (tblAbbredemption != null && (tblAbbredemption.StatusId == Convert.ToInt32(OrderStatusEnum.CustomerNotResponding_3C)
        //                             || tblAbbredemption.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentRescheduled_3RA)))
        //                        {
        //                            setStatusId = Convert.ToInt32(OrderStatusEnum.ReopenOrder);
        //                        }
        //                        foreach (var items in selfQcVideoImageViewModel.imageLabelViewModels)
        //                        {
        //                            tblSelfQc = new TblSelfQc();
        //                            if (items.IsMediaTypeVideo == true)
        //                            {
        //                                tblSelfQc.ImageName = items.RegdNo + "_" + "Video_" + totalCount + ".webm";
        //                            }
        //                            else
        //                            {
        //                                tblSelfQc.ImageName = items.RegdNo + "_" + "Image_" + totalCount + ".jpg";
        //                            }

        //                            tblSelfQc.RegdNo = items.RegdNo;
        //                            if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.Exchange))
        //                            {
        //                                tblSelfQc.ExchangeOrderId = tblExchangeOrder.Id;
        //                                tblSelfQc.UserId = selfQcVideoImageViewModel.LoginId != null ? (selfQcVideoImageViewModel.LoginId > 0 ? selfQcVideoImageViewModel.LoginId : 0) : 0;
        //                                tblSelfQc.IsActive = true;
        //                                tblSelfQc.IsExchange = true;
        //                                tblSelfQc.IsAbb = false;
        //                            }
        //                            else if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.ABB))
        //                            {
        //                                tblSelfQc.RedemptionId = tblAbbredemption.RedemptionId;
        //                                tblSelfQc.UserId = selfQcVideoImageViewModel.LoginId != null ? (selfQcVideoImageViewModel.LoginId > 0 ? selfQcVideoImageViewModel.LoginId : 0) : 0;
        //                                tblSelfQc.IsActive = true;
        //                                tblSelfQc.IsExchange = false;
        //                                tblSelfQc.IsAbb = true;
        //                            }
        //                            tblSelfQc.CreatedDate = System.DateTime.Now;
        //                            _selfQCRepository.Create(tblSelfQc);
        //                            totalCount += 1;
        //                            resultCount += 1;
        //                        }
        //                        _selfQCRepository.SaveChanges();

        //                        //Shashi

        //                        #region update statusid in tblOrderTran
        //                        if (tblOrderTran != null)
        //                        {
        //                            tblOrderTran.StatusId = setStatusId;
        //                            tblOrderTran.ModifiedDate = DateTime.Now;
        //                            _orderTransRepository.Update(tblOrderTran);
        //                            _orderTransRepository.SaveChanges();

        //                            tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
        //                            if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.ABB))
        //                            {
        //                                #region update statusid in tblabbredemption
        //                                tblAbbredemption.StatusId = setStatusId;
        //                                tblAbbredemption.ModifiedDate = DateTime.Now;
        //                                _abbRedemptionRepository.Update(tblAbbredemption);
        //                                _abbRedemptionRepository.SaveChanges();
        //                                #endregion

        //                                #region insert history in tblexchangeabbhistorytable
        //                                tblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(LoVEnum.ABB);
        //                                tblExchangeAbbstatusHistory.SponsorOrderNumber = tblAbbredemption.Sponsor != null ? tblAbbredemption.Sponsor : string.Empty;
        //                                tblExchangeAbbstatusHistory.RegdNo = tblAbbredemption.RegdNo;
        //                                tblExchangeAbbstatusHistory.ZohoSponsorId = tblAbbredemption.ZohoAbbredemptionId != null ? Convert.ToString(tblAbbredemption.ZohoAbbredemptionId) : string.Empty;
        //                                tblExchangeAbbstatusHistory.CustId = tblAbbredemption.CustomerDetailsId != null ? tblAbbredemption.CustomerDetailsId : 0;
        //                                tblExchangeAbbstatusHistory.StatusId = setStatusId;
        //                                tblExchangeAbbstatusHistory.IsActive = true;
        //                                tblExchangeAbbstatusHistory.CreatedDate = DateTime.Now;
        //                                tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
        //                                _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
        //                                _exchangeABBStatusHistoryRepository.SaveChanges();
        //                                #endregion

        //                            }
        //                            else if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.Exchange))
        //                            {
        //                                #region update statusid in tblexchangeorder
        //                                tblExchangeOrder.StatusId = setStatusId;
        //                                tblExchangeOrder.ModifiedDate = DateTime.Now;
        //                                _exchangeOrderRepository.Update(tblExchangeOrder);
        //                                _exchangeOrderRepository.SaveChanges();
        //                                #endregion

        //                                #region insert history in tblexchangeabbhistorytable
        //                                tblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(LoVEnum.Exchange);
        //                                tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
        //                                tblExchangeAbbstatusHistory.RegdNo = tblExchangeOrder.RegdNo;
        //                                tblExchangeAbbstatusHistory.ZohoSponsorId = tblExchangeOrder.ZohoSponsorOrderId != null ? tblExchangeOrder.ZohoSponsorOrderId : string.Empty; ;
        //                                tblExchangeAbbstatusHistory.CustId = tblExchangeOrder.CustomerDetailsId;
        //                                tblExchangeAbbstatusHistory.StatusId = tblExchangeOrder.StatusId;
        //                                tblExchangeAbbstatusHistory.IsActive = true;
        //                                tblExchangeAbbstatusHistory.CreatedDate = DateTime.Now;
        //                                tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
        //                                _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
        //                                _exchangeABBStatusHistoryRepository.SaveChanges();
        //                                #endregion
        //                            }
        //                            if (tblExchangeAbbstatusHistory != null && tblExchangeAbbstatusHistory.CustId != null && tblExchangeAbbstatusHistory.CustId > 0)
        //                            {
        //                                tblCustomerDetail = _customerDetailsRepository.GetCustDetails(tblExchangeAbbstatusHistory.CustId);
        //                                if (tblCustomerDetail != null && tblCustomerDetail.PhoneNumber != null)
        //                                {
        //                                    #region code to send selfqc link on whatsappNotification
        //                                    WhatsAppResponse whatasappResponse = new WhatsAppResponse();
        //                                    WhatsAppTemplate whatsappObj = new WhatsAppTemplate();
        //                                    TblWhatsAppMessage tblwhatsappmessage = null;

        //                                    whatsappObj.userDetails = new SelfQCUserDetails();
        //                                    whatsappObj.notification = new PostSelfQC();
        //                                    whatsappObj.userDetails.number = tblCustomerDetail.PhoneNumber;
        //                                    whatsappObj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
        //                                    whatsappObj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
        //                                    whatsappObj.notification.templateId = NotificationConstants.PostSelfQCAlert;
        //                                    string url = _baseConfig.Value.YellowAiUrl;
        //                                    RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
        //                                    if (response.Content != null)
        //                                    {
        //                                        whatasappResponse = JsonConvert.DeserializeObject<WhatsAppResponse>(response.Content);
        //                                        tblwhatsappmessage = new TblWhatsAppMessage();
        //                                        tblwhatsappmessage.TemplateName = NotificationConstants.PostSelfQCAlert;
        //                                        tblwhatsappmessage.IsActive = true;
        //                                        tblwhatsappmessage.PhoneNumber = tblCustomerDetail.PhoneNumber;
        //                                        tblwhatsappmessage.SendDate = DateTime.Now;
        //                                        tblwhatsappmessage.MsgId = whatasappResponse.msgId;
        //                                        _WhatsAppMessageRepository.Create(tblwhatsappmessage);
        //                                        _WhatsAppMessageRepository.SaveChanges();
        //                                    }
        //                                    #endregion
        //                                }

        //                            }
        //                        }
        //                        #endregion

        //                        if (resultCount == selfQcVideoImageViewModel.imageLabelViewModels.Count)
        //                            return flag = true;
        //                        else
        //                            return flag = false;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.WriteErrorToDB("QCManager", "AddImageToDB", ex);
        //    }
        //    return flag;
        //}

        public bool UpdateSelfQCImageToDB(SelfQcVideoImageViewModel selfQcVideoImageViewModel)
        {
            TblExchangeOrder? tblExchangeOrder = null;
            TblAbbredemption? tblAbbredemption = null;
            TblOrderTran? tblOrderTran = null;
            TblExchangeAbbstatusHistory? tblExchangeAbbstatusHistory = null;
            TblSelfQc? tblSelfQc = null;
            string businessExchange = "Exchange";
            string businessAbb = "ABB";
            int totalCount = 1;
            int resultCount = 0;
            int base64StringCount = 0;
            //status chck for 3C an 3RA
            int setStatusId = Convert.ToInt32(OrderStatusEnum.SelfQCbyCustomer);
            bool flag = false;
            TblCustomerDetail? tblCustomerDetail = null;
            try
            {
                if (selfQcVideoImageViewModel != null && selfQcVideoImageViewModel.imageLabelViewModels != null)
                {
                    if (selfQcVideoImageViewModel.imageLabelViewModels.Count > 0)
                    {
                        string regdNo = selfQcVideoImageViewModel.imageLabelViewModels.FirstOrDefault().RegdNo;
                        tblOrderTran = _orderTransRepository.GetTransExchAbbByRegdno(regdNo);
                        bool verifySelfQC = verifyDuplicateSelfQC(regdNo);
                        if (tblOrderTran != null && ((!verifySelfQC) || tblOrderTran.SelfQclinkResendby > 0))
                        {
                            tblExchangeOrder = tblOrderTran.Exchange;
                            tblAbbredemption = tblOrderTran.Abbredemption;
                            for (int i = 0; i < selfQcVideoImageViewModel.imageLabelViewModels.Count; i++)
                            {
                                var items = selfQcVideoImageViewModel.imageLabelViewModels[i];
                                if (items != null && items.Base64StringValue != null)
                                {
                                    base64StringCount += 1;
                                    bool IsImageSaved = true;
                                    tblSelfQc = new TblSelfQc();
                                    if (items.IsMediaTypeVideo == true)
                                    {
                                        tblSelfQc.ImageName = items.RegdNo + "_" + "Video_" + totalCount + ".webm";
                                    }
                                    else
                                    {
                                        tblSelfQc.ImageName = items.RegdNo + "_" + "Image_" + totalCount + ".jpg";
                                    }

                                    if (items.Base64StringValue != "File Saved Already" && items.IsMediaTypeVideo != true)
                                    {
                                        var filePath = @"\DBFiles\QC\SelfQC";
                                        IsImageSaved = _imageHelper.SaveFileFromBase64(items.Base64StringValue, filePath, tblSelfQc.ImageName);
                                    }
                                    if (IsImageSaved == true)
                                    {
                                        tblSelfQc.RegdNo = items.RegdNo;
                                        if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                                        {
                                            tblSelfQc.ExchangeOrderId = tblExchangeOrder.Id;
                                            tblSelfQc.UserId = selfQcVideoImageViewModel.LoginId != null ? (selfQcVideoImageViewModel.LoginId > 0 ? selfQcVideoImageViewModel.LoginId : 0) : 0;
                                            tblSelfQc.IsActive = true;
                                            tblSelfQc.IsExchange = true;
                                            tblSelfQc.IsAbb = false;
                                        }
                                        else if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.ABB))
                                        {
                                            tblSelfQc.RedemptionId = tblAbbredemption.RedemptionId;
                                            tblSelfQc.UserId = selfQcVideoImageViewModel.LoginId != null ? (selfQcVideoImageViewModel.LoginId > 0 ? selfQcVideoImageViewModel.LoginId : 0) : 0;
                                            tblSelfQc.IsActive = true;
                                            tblSelfQc.IsExchange = false;
                                            tblSelfQc.IsAbb = true;
                                        }
                                        tblSelfQc.CreatedDate = System.DateTime.Now;
                                        _selfQCRepository.Create(tblSelfQc);

                                        resultCount += 1;
                                    }
                                    totalCount += 1;
                                }
                            }
                            if (resultCount == selfQcVideoImageViewModel.imageLabelViewModels.Count)
                            {
                                #region Save images in tblSelfQC
                                int IsSelfQCDone = _selfQCRepository.SaveChanges();
                                #endregion

                                #region update statusid in tblOrderTran
                                if (IsSelfQCDone > 0)
                                {
                                    #region Check StatusId for 3C and 3RA status 
                                    if (tblExchangeOrder != null && (tblExchangeOrder.StatusId == Convert.ToInt32(OrderStatusEnum.CustomerNotResponding_3C)
                                         || tblExchangeOrder.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentRescheduled_3RA)))
                                    {
                                        setStatusId = Convert.ToInt32(OrderStatusEnum.ReopenOrder);
                                    }
                                    if (tblAbbredemption != null && (tblAbbredemption.StatusId == Convert.ToInt32(OrderStatusEnum.CustomerNotResponding_3C)
                                         || tblAbbredemption.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentRescheduled_3RA)))
                                    {
                                        setStatusId = Convert.ToInt32(OrderStatusEnum.ReopenOrder);
                                    }
                                    #endregion

                                    tblOrderTran.StatusId = setStatusId;
                                    tblOrderTran.ModifiedDate = DateTime.Now;
                                    _orderTransRepository.Update(tblOrderTran);
                                    _orderTransRepository.SaveChanges();

                                    tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                    if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.ABB) && tblAbbredemption != null)
                                    {
                                        #region update statusid in tblabbredemption
                                        tblAbbredemption.StatusId = setStatusId;
                                        tblAbbredemption.ModifiedDate = DateTime.Now;
                                        _abbRedemptionRepository.Update(tblAbbredemption);
                                        _abbRedemptionRepository.SaveChanges();
                                        #endregion

                                        #region insert history in tblexchangeabbhistorytable
                                        tblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(LoVEnum.ABB);
                                        tblExchangeAbbstatusHistory.SponsorOrderNumber = tblAbbredemption.Sponsor != null ? tblAbbredemption.Sponsor : string.Empty;
                                        tblExchangeAbbstatusHistory.RegdNo = tblAbbredemption.RegdNo;
                                        tblExchangeAbbstatusHistory.ZohoSponsorId = tblAbbredemption.ZohoAbbredemptionId != null ? Convert.ToString(tblAbbredemption.ZohoAbbredemptionId) : string.Empty;
                                        tblExchangeAbbstatusHistory.CustId = tblAbbredemption.CustomerDetailsId != null ? tblAbbredemption.CustomerDetailsId : 0;
                                        tblExchangeAbbstatusHistory.StatusId = setStatusId;
                                        tblExchangeAbbstatusHistory.IsActive = true;
                                        tblExchangeAbbstatusHistory.CreatedDate = DateTime.Now;
                                        tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                                        _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                                        _exchangeABBStatusHistoryRepository.SaveChanges();
                                        #endregion

                                    }
                                    else if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.Exchange) && tblExchangeOrder != null)
                                    {
                                        #region update statusid in tblexchangeorder
                                        tblExchangeOrder.StatusId = setStatusId;
                                        tblExchangeOrder.ModifiedDate = DateTime.Now;
                                        _exchangeOrderRepository.Update(tblExchangeOrder);
                                        _exchangeOrderRepository.SaveChanges();
                                        #endregion

                                        #region insert history in tblexchangeabbhistorytable
                                        tblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(LoVEnum.Exchange);
                                        tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                                        tblExchangeAbbstatusHistory.RegdNo = tblExchangeOrder.RegdNo;
                                        tblExchangeAbbstatusHistory.ZohoSponsorId = tblExchangeOrder.ZohoSponsorOrderId != null ? tblExchangeOrder.ZohoSponsorOrderId : string.Empty; ;
                                        tblExchangeAbbstatusHistory.CustId = tblExchangeOrder.CustomerDetailsId;
                                        tblExchangeAbbstatusHistory.StatusId = tblExchangeOrder.StatusId;
                                        tblExchangeAbbstatusHistory.IsActive = true;
                                        tblExchangeAbbstatusHistory.CreatedDate = DateTime.Now;
                                        tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                                        _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                                        _exchangeABBStatusHistoryRepository.SaveChanges();
                                        #endregion
                                    }
                                    if (tblExchangeAbbstatusHistory != null && tblExchangeAbbstatusHistory.CustId != null && tblExchangeAbbstatusHistory.CustId > 0)
                                    {
                                        tblCustomerDetail = _customerDetailsRepository.GetCustDetails(tblExchangeAbbstatusHistory.CustId);
                                        if (tblCustomerDetail != null && tblCustomerDetail.PhoneNumber != null)
                                        {
                                            #region code to send selfqc link on whatsappNotification
                                            WhatsAppResponse whatasappResponse = new WhatsAppResponse();
                                            WhatsAppTemplate whatsappObj = new WhatsAppTemplate();
                                            TblWhatsAppMessage tblwhatsappmessage = null;

                                            whatsappObj.userDetails = new SelfQCUserDetails();
                                            whatsappObj.notification = new PostSelfQC();
                                            whatsappObj.userDetails.number = tblCustomerDetail.PhoneNumber;
                                            whatsappObj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
                                            whatsappObj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
                                            whatsappObj.notification.templateId = NotificationConstants.PostSelfQCAlert;
                                            string url = _baseConfig.Value.YellowAiUrl;
                                            RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                                            if (response.Content != null)
                                            {
                                                whatasappResponse = JsonConvert.DeserializeObject<WhatsAppResponse>(response.Content);
                                                tblwhatsappmessage = new TblWhatsAppMessage();
                                                tblwhatsappmessage.TemplateName = NotificationConstants.PostSelfQCAlert;
                                                tblwhatsappmessage.IsActive = true;
                                                tblwhatsappmessage.PhoneNumber = tblCustomerDetail.PhoneNumber;
                                                tblwhatsappmessage.SendDate = DateTime.Now;
                                                tblwhatsappmessage.MsgId = whatasappResponse.msgId;
                                                _WhatsAppMessageRepository.Create(tblwhatsappmessage);
                                                _WhatsAppMessageRepository.SaveChanges();
                                            }
                                            #endregion
                                        }

                                    }
                                    return flag = true;
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCManager", "UpdateSelfQCImageToDB", ex);
            }
            return flag;
        }
        #endregion

        #region Save Media Files into the Folder
        /// <summary>
        /// Save Media Files into the Folder
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="imageLabelViewModel"></param>
        /// <returns>bool</returns>
        public bool SaveMediaFile(string? regdNo, string? base64String, bool isMediaTypeVideo, int srNum, int? orderTransId, int? statusId, int? imageLabelId)
        {
            bool flag = false;
            TblTempDatum tempDatum = null;
            int result = 0;
            bool isFileSaved = false;
            try
            {
                var filePath = @"\DBFiles\QC\SelfQC";
                string fileName = "";
                if (regdNo != null && base64String != null)
                {
                    if (isMediaTypeVideo == true)
                    {
                        fileName = regdNo + "_Video_" + srNum + ".webm";
                    }
                    else
                    {
                        fileName = regdNo + "_Image_" + srNum + ".jpg";
                    }

                    #region Save File into Temp DB
                    tempDatum = _tempDataRepository.GetTempDataByFileName(fileName);
                    if (tempDatum == null)
                    {
                        tempDatum = new TblTempDatum();
                        tempDatum.RegdNo = regdNo;
                        tempDatum.FileName = fileName;
                        tempDatum.ImageLabelid = imageLabelId;
                        tempDatum.StatusId = statusId;
                        tempDatum.OrderTransId = orderTransId;
                        tempDatum.IsActive = true;
                        tempDatum.CreatedDate = DateTime.Now;
                        _tempDataRepository.Create(tempDatum);
                    }
                    else
                    {
                        tempDatum.ImageLabelid = imageLabelId;
                        tempDatum.StatusId = statusId;
                        tempDatum.OrderTransId = orderTransId;
                        tempDatum.ModifiedDate = DateTime.Now;
                        _tempDataRepository.Update(tempDatum);
                    }
                    result = _tempDataRepository.SaveChanges();
                    if (result > 0)
                    {
                        flag = _imageHelper.SaveFileFromBase64(base64String, filePath, fileName);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCManager", "SaveMediaFile", ex);
            }
            return flag;
        }
        #endregion

        #region Delete Media Files
        /// <summary>
        /// Delete Media Files
        /// </summary>
        /// <param name="regdNo"></param>
        /// <param name="base64String"></param>
        /// <param name="isMediaTypeVideo"></param>
        /// <param name="srNum"></param>
        /// <returns></returns>
        public bool DeleteMediaFile(string? regdNo, bool isMediaTypeVideo, int srNum)
        {
            bool flag = false;
            TblTempDatum? tempDatum = null;
            int result = 0;
            bool isFileSaved = false;
            try
            {
                var filePath = @"\DBFiles\QC\SelfQC";
                string fileName = "";
                if (!string.IsNullOrEmpty(regdNo) && srNum > 0)
                {
                    if (isMediaTypeVideo == true)
                    {
                        fileName = regdNo + "_" + "Video_" + srNum + ".webm";
                    }
                    else
                    {
                        fileName = regdNo + "_" + "Image_" + srNum + ".jpg";
                    }

                    #region Delete File into Temp DB
                    tempDatum = _tempDataRepository.GetTempDataByFileName(fileName);
                    if (tempDatum != null)
                    {
                        tempDatum.IsActive = false;
                        tempDatum.ModifiedDate = DateTime.Now;
                        //_tempDataRepository.Update(tempDatum);
                        _tempDataRepository.Delete(tempDatum);
                    }
                    result = _tempDataRepository.SaveChanges();

                    string documentsPath = string.Concat(_webHostEnvironment.WebRootPath, "\\", filePath);
                    string tempInputFilePath = System.IO.Path.Combine(documentsPath, fileName);
                    System.IO.File.Delete(tempInputFilePath);
                    flag = true;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCManager", "DeleteMediaFile", ex);
            }
            return flag;
        }
        #endregion

        #region Common method to Get Image Labels from Image Label Master by RegdNo
        /// <summary>
        /// Common method to Get Image Labels from Image Label Master by RegdNo
        /// </summary>
        /// <param name="regdno"></param>
        /// <returns></returns>
        public List<ImageLabelViewModel> GetQCImageLabels(string regdno)
        {
            #region Variable Declaration
            TblExchangeOrder? tblExchangeOrder = null;
            TblProductType? tblProductType = null;
            List<ImageLabelViewModel>? imageLabelVMList = null;
            List<TblImageLabelMaster>? tblImageLabel = null;
            string? baseUrl = _baseConfig.Value.BaseURL;
            TblOrderTran? tblOrderTran = null;
            TblAbbregistration? tblAbbregistration = null;
            TblBusinessUnit? tblBusinessUnit = null;
            #endregion

            try
            {
                if (!string.IsNullOrEmpty(regdno))
                {
                    tblOrderTran = _orderTransRepository.GetRegdno(regdno);
                    if (tblOrderTran != null)
                    {
                        #region Common Implementation for Exchange or ABB
                        if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.ABB))
                        {
                            tblAbbregistration = _abbRegistrationRepository.GetRegdNo(regdno);
                            if (tblAbbregistration != null)
                            {
                                tblProductType = _productTypeRepository.GetBytypeid(tblAbbregistration.NewProductCategoryTypeId);
                                tblBusinessUnit = _banginessUnitRepository.Getbyid(tblAbbregistration.BusinessUnitId);
                            }
                            else
                            {
                                tblAbbregistration = new TblAbbregistration();
                            }
                        }
                        else if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                        {
                            tblExchangeOrder = _exchangeOrderRepository.GetExchOrderByRegdNo(regdno);
                            if (tblExchangeOrder != null)
                            {
                                tblProductType = _productTypeRepository.GetBytypeid(tblExchangeOrder.ProductTypeId);
                                tblBusinessUnit = _banginessUnitRepository.Getbyid(tblExchangeOrder.BusinessUnitId);
                            }
                            else
                            {
                                tblExchangeOrder = new TblExchangeOrder();
                            }
                        }
                        #endregion

                        if (tblProductType != null && tblBusinessUnit?.IsBulkOrder == true)
                        {
                            tblImageLabel = _imageLabelRepository.GetList(x => x.IsActive == true && x.ProductCatId == tblProductType.ProductCatId && x.IsMediaTypeVideo == true).ToList();
                        }
                        else if (tblProductType != null)
                        {
                            tblImageLabel = _imageLabelRepository.GetList(x => x.IsActive == true && x.ProductCatId == tblProductType.ProductCatId).ToList();
                        }

                        if (tblImageLabel != null && tblImageLabel.Count > 0)
                        {
                            imageLabelVMList = _mapper.Map<List<TblImageLabelMaster>, List<ImageLabelViewModel>>(tblImageLabel);
                            foreach (var item in imageLabelVMList)
                            {
                                item.RegdNo = tblOrderTran.RegdNo;
                                //item.BusinessType = "Exchange";
                                if (item.ImagePlaceHolder != null)
                                {
                                    item.FullPlaceHolderImageUrl = baseUrl + EnumHelper.DescriptionAttr(FileAddressEnum.ImageLabelMaster) + item.ImagePlaceHolder;
                                }
                            }
                        }
                        else
                        {
                            imageLabelVMList = new List<ImageLabelViewModel>();
                        }
                        return imageLabelVMList;
                    }
                    else
                    {
                        imageLabelVMList = new List<ImageLabelViewModel>();
                    }
                }
            }
            catch (Exception ex)
            {

                _logging.WriteErrorToDB("QCManager", "SelfQC", ex);
            }
            return imageLabelVMList;
        }
        #endregion

        #endregion
    }
}
