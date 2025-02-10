using AutoMapper;
using ExcelDataReader.Log.Logger;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using RDCELERP.BAL.Enum;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model;
using RDCELERP.Model.ABBRedemption;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.CashfreeModel;
using RDCELERP.Model.Dashboards;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.ImageLabel;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.LGC;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.Product;
using RDCELERP.Model.QC;
using RDCELERP.Model.QCComment;
using RDCELERP.Model.Sweetener;
using RDCELERP.Model.UniversalPriceMaster;
using static RDCELERP.Model.ABBRedemption.LoVViewModel;
using static RDCELERP.Model.Whatsapp.WhatsappCashVoucherViewModel;
using static RDCELERP.Model.Whatsapp.WhatsappDiagnosticReport;

namespace RDCELERP.BAL.MasterManager
{
    public class QCCommentManager : IQCCommentManager
    {
        #region variables
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        IExchangeOrderRepository _ExchangeOrderRepository;
        IBusinessPartnerRepository _businessPartnerRepository;
        IBusinessUnitRepository _businessUnitRepository;
        ILoginRepository _loginRepository;
        IPriceMasterRepository _priceMasterRepository;
        IBrandRepository _brandRepository;
        IOrderQCRepository _orderQCRepository;
        IUserRoleRepository _userRoleRepository;
        ICustomerDetailsRepository _customerDetailsRepository;
        IExchangeOrderStatusRepository _ExchangeOrderStatusRepository;
        IRoleRepository _roleRepository;
        IOrderTransRepository _orderTransRepository;
        ILogisticsRepository _logisticsRepository;
        IOrderLGCRepository _orderLGCRepository;
        IServicePartnerRepository _servicePartnerRepository;
        IVoucherRepository _voucherRepository;
        IOrderImageUploadRepository _orderImageUploadRepository;
        IWebHostEnvironment _webHostEnvironment;
        IImageHelper _imageHelper;
        IExchangeABBStatusHistoryRepository _exchangeABBStatusHistoryRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        IProductTypeRepository _productTypeRepository;
        IProductCategoryRepository _productCategoryRepository;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IExchangeOrderManager _exchangeOrderManager;
        IWalletTransactionRepository _walletTransactionRepository;
        IVoucherStatusRepository _voucherStatusRepository;
        ISelfQCRepository _selfQCRepository;
        ILovRepository _lovRepository;
        IOptions<ApplicationSettings> _config;
        IEVCPODDetailsRepository _eVCPODDetailsRepository;
        IHistoryRepository _historyRepository;
        IBillCloudServiceCall _billCloudServiceCall;
        public readonly IOptions<ApplicationSettings> _baseConfig;
        IWhatsAppMessageRepository _WhatsAppMessageRepository;
        IWhatsappNotificationManager _whatsappNotificationManager;
        IHtmlToPDFConverterHelper _htmlToPDFConverterHelper;
        IOrderQCRatingRepository _orderQCRatingRepository;
        ICommonManager _commonManager;
        ICashfreePayoutCall _cashfreePayoutCall;
        ICompanyRepository _companyRepository;
        IAbbRegistrationRepository _abbRegistrationRepository;
        IABBRedemptionRepository _aBBRedemptionRepository;
        IImageLabelRepository _imageLabelRepository;
        IBUBasedSweetnerValidationRepository _bUBasedSweetnerValidationRepository;
        IQuestionsForSweetnerRepository _questionsForSweetnerRepository;
        IAreaLocalityRepository _areaLocalityRepository;
        IPriceMasterMappingRepository _priceMasterMappingRepository;
        IUniversalPriceMasterRepository _universalPriceMasterRepository;
        IProductConditionLabelRepository _productConditionLabelRepository;
        ISweetenerManager _sweetenerManager;
        IOrderBasedConfigRepository _orderBasedConfigRepository;
        IPriceMasterQuestionersRepository _priceMasterQuestionersRepository;
        IProdCatBrandMappingRepository _prodCatBrandMappingRepository;
        IQCRatingMasterRepository _qcRatingMasterRepository;
        IQuestionerLOVRepository _questionerLOVRepository;
        IQuestionerLovmappingRepository _questionerLovmappingRepository;
        IQcratingMasterMappingRepository _qcratingMasterMappingRepository;
        #endregion

        #region Constructor
        public QCCommentManager(IWebHostEnvironment webHostEnvironment, RDCELERP.DAL.Entities.Digi2l_DevContext context, IProductTypeRepository productTypeRepository, IProductCategoryRepository productCategoryRepository, IVoucherRepository voucherRepository, IOrderImageUploadRepository orderImageUploadRepository, IServicePartnerRepository servicePartnerRepository, IOrderLGCRepository orderLGCRepository, ILogisticsRepository logisticsRepository, IOrderTransRepository orderTransRepository, IBrandRepository brandRepository, IPriceMasterRepository priceMasterRepository, ILoginRepository loginRepository, IBusinessUnitRepository businessUnitRepository, IBusinessPartnerRepository businessPartnerRepository, IExchangeOrderStatusRepository exchangeOrderStatusRepository, IExchangeOrderRepository exchangeOrderRepository, IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository, IOrderQCRepository orderQCRepository, ICustomerDetailsRepository customerDetailsRepository, IUserRoleRepository userRoleRepository, IRoleRepository roleRepository, IMapper mapper, ILogging logging, IImageHelper imageHelper, ISelfQCRepository selfQCRepository, IWalletTransactionRepository walletTransactionRepository, IVoucherStatusRepository voucherStatusRepository, ILovRepository lovRepository, IOptions<ApplicationSettings> config, IEVCPODDetailsRepository eVCPODDetailsRepository, IHistoryRepository historyRepository, IBillCloudServiceCall billCloudServiceCall, IOptions<ApplicationSettings> baseConfig, IWhatsAppMessageRepository whatsAppMessageRepository, IWhatsappNotificationManager whatsappNotificationManager, IHtmlToPDFConverterHelper htmlToPDFConverterHelper, IOrderQCRatingRepository orderQCRatingRepository, ICommonManager commonManager, ICashfreePayoutCall cashfreePayoutCall, ICompanyRepository companyRepository, IAbbRegistrationRepository abbRegistrationRepository, IABBRedemptionRepository aBBRedemptionRepository, IImageLabelRepository imageLabelRepository, IAreaLocalityRepository areaLocalityRepository, IBUBasedSweetnerValidationRepository bUBasedSweetnerValidationRepository, IQuestionsForSweetnerRepository questionsForSweetnerRepository, IPriceMasterMappingRepository priceMasterMappingRepository, IUniversalPriceMasterRepository universalPriceMasterRepository, IProductConditionLabelRepository productConditionLabelRepository, ISweetenerManager sweetenerManager, IOrderBasedConfigRepository orderBasedConfigRepository, IPriceMasterQuestionersRepository priceMasterQuestionersRepository = null, IProdCatBrandMappingRepository prodCatBrandMappingRepository = null, IQCRatingMasterRepository qcRatingMasterRepository = null, IQuestionerLOVRepository questionerLOVRepository = null, IQuestionerLovmappingRepository questionerLovmappingRepository = null, IQcratingMasterMappingRepository qcratingMasterMappingRepository = null)

        {
            _productTypeRepository = productTypeRepository;
            _productCategoryRepository = productCategoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _voucherRepository = voucherRepository;
            _servicePartnerRepository = servicePartnerRepository;
            _orderLGCRepository = orderLGCRepository;
            _logisticsRepository = logisticsRepository;
            _brandRepository = brandRepository;
            _priceMasterRepository = priceMasterRepository;
            _loginRepository = loginRepository;
            _businessUnitRepository = businessUnitRepository;
            _businessPartnerRepository = businessPartnerRepository;
            _userRoleRepository = userRoleRepository;
            _orderQCRepository = orderQCRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logging = logging;
            _customerDetailsRepository = customerDetailsRepository;
            _ExchangeOrderRepository = exchangeOrderRepository;
            _ExchangeOrderStatusRepository = exchangeOrderStatusRepository;
            _orderTransRepository = orderTransRepository;
            _orderImageUploadRepository = orderImageUploadRepository;
            _imageHelper = imageHelper;
            _exchangeABBStatusHistoryRepository = exchangeABBStatusHistoryRepository;
            _walletTransactionRepository = walletTransactionRepository;
            _voucherStatusRepository = voucherStatusRepository;
            _selfQCRepository = selfQCRepository;
            _lovRepository = lovRepository;
            _config = config;
            _eVCPODDetailsRepository = eVCPODDetailsRepository;
            _historyRepository = historyRepository;
            _billCloudServiceCall = billCloudServiceCall;
            _baseConfig = baseConfig;
            _WhatsAppMessageRepository = whatsAppMessageRepository;
            _whatsappNotificationManager = whatsappNotificationManager;
            _htmlToPDFConverterHelper = htmlToPDFConverterHelper;
            _orderQCRatingRepository = orderQCRatingRepository;
            _commonManager = commonManager;
            _cashfreePayoutCall = cashfreePayoutCall;
            _companyRepository = companyRepository;
            _abbRegistrationRepository = abbRegistrationRepository;
            _aBBRedemptionRepository = aBBRedemptionRepository;
            _imageLabelRepository = imageLabelRepository;
            _bUBasedSweetnerValidationRepository = bUBasedSweetnerValidationRepository;
            _questionsForSweetnerRepository = questionsForSweetnerRepository;
            _areaLocalityRepository = areaLocalityRepository;
            _priceMasterMappingRepository = priceMasterMappingRepository;
            _universalPriceMasterRepository = universalPriceMasterRepository;
            _productConditionLabelRepository = productConditionLabelRepository;
            _sweetenerManager = sweetenerManager;
            _orderBasedConfigRepository = orderBasedConfigRepository;
            _priceMasterQuestionersRepository = priceMasterQuestionersRepository;
            _prodCatBrandMappingRepository = prodCatBrandMappingRepository;
            _qcRatingMasterRepository = qcRatingMasterRepository;
            _questionerLOVRepository = questionerLOVRepository;
            _questionerLovmappingRepository = questionerLovmappingRepository;
            _qcratingMasterMappingRepository = qcratingMasterMappingRepository;
        }
        #endregion

        #region Method to get All Regd no from TblExchnageOrder
        /// <summary>
        /// 
        /// </summary>
        /// <returns>exchangeOrderVMList</returns>
        public List<ExchangeOrderViewModel> GetAllRegdNo()
        {
            List<ExchangeOrderViewModel> exchangeOrderVMList = null;
            List<TblExchangeOrder> tblExchangeOrderlist = new List<TblExchangeOrder>();
            try
            {

                tblExchangeOrderlist = _ExchangeOrderRepository.GetList(x => x.IsActive == true).ToList();

                if (tblExchangeOrderlist != null && tblExchangeOrderlist.Count > 0)
                {
                    exchangeOrderVMList = _mapper.Map<List<TblExchangeOrder>, List<ExchangeOrderViewModel>>(tblExchangeOrderlist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetAllRegdNo", ex);
            }
            return exchangeOrderVMList;

        }
        #endregion

        #region Method to get QCUrl details By Exchangeid
        /// <summary>
        /// Method to get QC details By Exchangeid
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>QcCommentViewModel</returns>
        public QCCommentViewModel GetQCByExchangeId(int Id)
        {
            QCCommentViewModel QcCommentViewModel = new QCCommentViewModel();
            try
            {
                TblOrderTran orderTran = _orderTransRepository.GetSingle(x => x.IsActive == true && x.ExchangeId == Id);
                TblAreaLocality? tblAreaLocality = null;
                TblProductType? tblProductType = null;
                TblProductCategory? tblProductCategory = null;
                if (orderTran != null)
                {
                    TblExchangeOrder exchangeOrder = _ExchangeOrderRepository.GetSingleOrder(Id);

                    if (exchangeOrder != null)
                    {
                        QcCommentViewModel.ExchangeOrderViewModel = _mapper.Map<TblExchangeOrder, ExchangeOrderViewModel>(exchangeOrder);
                        QcCommentViewModel.CustomerDetailViewModel = _mapper.Map<TblCustomerDetail, CustomerDetailViewModel>(exchangeOrder.CustomerDetails);
                        if (QcCommentViewModel.CustomerDetailViewModel != null)
                        {
                            QcCommentViewModel.CustomerDetailViewModel.CustName = QcCommentViewModel.CustomerDetailViewModel.FirstName + " " + QcCommentViewModel.CustomerDetailViewModel.LastName;
                        }
                        if (QcCommentViewModel.CustomerDetailViewModel.AreaLocalityId != null)
                        {
                            tblAreaLocality = _areaLocalityRepository.GetArealocalityById(QcCommentViewModel.CustomerDetailViewModel.AreaLocalityId);
                            if (tblAreaLocality != null)
                            {
                                QcCommentViewModel.CustomerDetailViewModel.AreaLocality = tblAreaLocality.AreaLocality;
                            }
                        }

                        QcCommentViewModel.ProductCategory = exchangeOrder.ProductType.ProductCat != null ? exchangeOrder.ProductType.ProductCat.Description : "";
                        QcCommentViewModel.ProductCategoryId = exchangeOrder.ProductType.ProductCat.Id;
                        QcCommentViewModel.ProductType = !string.IsNullOrEmpty(exchangeOrder.ProductType.Size) ? exchangeOrder.ProductType.Description + " (" + exchangeOrder.ProductType.Size + ")" : exchangeOrder.ProductType.Description;
                        QcCommentViewModel.ProductBrand = exchangeOrder.Brand.Name;
                        QcCommentViewModel.CustDeclaredQlty = exchangeOrder.ProductCondition;
                        QcCommentViewModel.Bonus = exchangeOrder.Sweetener;
                        QcCommentViewModel.OrderTransId = orderTran.OrderTransId;

                        if (exchangeOrder.NewProductCategoryId != null && exchangeOrder.NewProductTypeId != null)
                        {
                            tblProductType = _productTypeRepository.GetCatTypebytypeid(exchangeOrder.NewProductTypeId);
                            QcCommentViewModel.ExchangeOrderViewModel.NewProductType = !string.IsNullOrEmpty(tblProductType.Size) ? tblProductType.Description + " (" + tblProductType.Size + ")" : tblProductType.Description;
                            QcCommentViewModel.ExchangeOrderViewModel.NewProductcategory = tblProductType.ProductCat != null ? tblProductType.ProductCat.Description : "";
                            if (exchangeOrder.ModelNumberId > 0)
                            {
                                TblModelNumber tblModelNumber = _context.TblModelNumbers.FirstOrDefault(x => x.IsActive == true && x.ModelNumberId == exchangeOrder.ModelNumberId);
                                QcCommentViewModel.ExchangeOrderViewModel.ModelNumber = tblModelNumber != null && tblModelNumber.ModelName != null ? tblModelNumber.ModelName : "";
                                //QcCommentViewModel.ExchangeOrderViewModel.ModelNumber = tblModelNumber.ModelName != null ? tblModelNumber.ModelName : "";
                            }
                        }
                        if (exchangeOrder.NewBrandId != null)
                        {
                            TblBrand tblBrand = _brandRepository.GetBrand(exchangeOrder.NewBrandId);
                            QcCommentViewModel.ExchangeOrderViewModel.NewBrandName = tblBrand != null ? tblBrand.Name : "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetQCByExchangeId", ex);
            }
            return QcCommentViewModel;
        }
        #endregion

        #region Method to manage (Add/Edit) QCComment 
        /// <summary>
        /// Method to manage (Add/Edit) QCComment 
        /// </summary>
        /// <param name="QCCommentVM">QCCommentVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>

        public int ManageQCComment(QCCommentViewModel QCCommentVM, IList<ImageLabelViewModel> imageLabelViewModels, int UserId)
        {
            QCCommentViewModel qCCommentView = new QCCommentViewModel();
            TblOrderQc TblOrderQc = new TblOrderQc();
            TblExchangeOrder tblExchangeOrder = null;
            TblOrderImageUpload tblOrderImageUpload = null;
            TblOrderTran tblOrderTran = null;
            TblLoV tblLoV = null;
            int CountOrderImgUpload = 1;
            string lovName = "QC Team";
            bool imgSave = false;
            bool videoSave = false;
            string filePath = null;
            TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = null;
            try
            {
                if (QCCommentVM != null)
                {
                    tblExchangeOrder = _ExchangeOrderRepository.GetRegdNo(QCCommentVM.ExchangeOrderViewModel.RegdNo);

                    if (tblExchangeOrder != null)
                    {
                        #region Update Address Fields in Customer Details Table
                        TblCustomerDetail tblCustomerDetail = _customerDetailsRepository.GetCustDetails(tblExchangeOrder.CustomerDetailsId);
                        if (tblCustomerDetail != null)
                        {
                            tblCustomerDetail.Address1 = QCCommentVM.CustomerDetailViewModel.Address1;
                            tblCustomerDetail.Address2 = QCCommentVM.CustomerDetailViewModel.Address2;
                            tblCustomerDetail.ModifiedBy = UserId;
                            tblCustomerDetail.ModifiedDate = _currentDatetime;
                            _customerDetailsRepository.Update(tblCustomerDetail);
                            _customerDetailsRepository.SaveChanges();
                        }
                        #endregion

                        tblOrderTran = _orderTransRepository.GetRegdno(tblExchangeOrder.RegdNo);
                    }
                }

                #region Upload Video Qc images In Folder and DB
                tblLoV = _lovRepository.GetSingle(x => x.IsActive == true && x.LoVname.ToLower().Trim() == lovName.ToLower().Trim());
                if (QCCommentVM.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval) || QCCommentVM.StatusId == Convert.ToInt32(OrderStatusEnum.QCOrderCancel))
                {
                    if (imageLabelViewModels.Count > 0 && tblOrderTran != null)
                    {                        
                        foreach (var items in imageLabelViewModels)
                        {
                            if(items.Base64StringValue != null)
                            {
                                if (items.Base64StringValue == "Videofile" && QCCommentVM.VideoBase64StringValue != null)
                                {
                                    items.FileName = tblExchangeOrder.RegdNo + "_" + "FinalQCVideo_" + CountOrderImgUpload + ".webm";
                                    filePath = EnumHelper.DescriptionAttr(FilePathEnum.VideoQC);
                                    string filetempPath = string.Concat(_webHostEnvironment.WebRootPath, "\\", filePath);
                                    string videoFilePath = Path.Combine(filetempPath, items.FileName);
                                    if (File.Exists(videoFilePath))                                        
                                    {
                                        byte[] videoBytes = File.ReadAllBytes(videoFilePath);
                                        string base64String = Convert.ToBase64String(videoBytes);
                                        videoSave = _imageHelper.SaveVideoFileFromBase64(base64String, filePath, items.FileName);
                                    }
                                    //videoSave = _imageHelper.SaveVideoFileFromBase64(QCCommentVM.VideoBase64StringValue, filePath, items.FileName);
                                    //if (videoSave == true)
                                    //{
                                    //    QCCommentVM.VideoBase64StringValue = null;
                                    //}
                                }
                                else
                                {
                                    items.FileName = tblExchangeOrder.RegdNo + "_" + "FinalQCImage" + CountOrderImgUpload + ".jpg";
                                    filePath = EnumHelper.DescriptionAttr(FilePathEnum.VideoQC);
                                    imgSave = _imageHelper.SaveFileFromBase64(items.Base64StringValue, filePath, items.FileName);
                                }

                                tblOrderImageUpload = _orderImageUploadRepository.GetSingle(x => x.IsActive == true && x.ImageName == items.FileName);
                                if (tblOrderImageUpload != null)
                                {
                                    tblOrderImageUpload.Modifiedby = UserId;
                                    tblOrderImageUpload.ModifiedDate = _currentDatetime;
                                    _orderImageUploadRepository.Update(tblOrderImageUpload);
                                }
                                else
                                {
                                    tblOrderImageUpload = new TblOrderImageUpload();
                                    tblOrderImageUpload.ImageName = items.FileName;
                                    tblOrderImageUpload.OrderTransId = tblOrderTran.OrderTransId;
                                    if (tblLoV != null)
                                    {
                                        tblOrderImageUpload.ImageUploadby = tblLoV.LoVid;
                                    }
                                    tblOrderImageUpload.IsActive = true;
                                    tblOrderImageUpload.CreatedBy = UserId;
                                    tblOrderImageUpload.CreatedDate = _currentDatetime;
                                    _orderImageUploadRepository.Create(tblOrderImageUpload);
                                }
                                _orderImageUploadRepository.SaveChanges();
                            }
                            
                            CountOrderImgUpload += 1;
                        }
                    }
                }
                #endregion


                #region Save TblOrderQc Insert and update
                TblOrderQc orderQC = new TblOrderQc();

                TblOrderQc = _mapper.Map<QCCommentViewModel, TblOrderQc>(QCCommentVM);

                TblOrderQc objtblOrderQc = new TblOrderQc();
                objtblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran.OrderTransId);
                if (objtblOrderQc != null && objtblOrderQc.OrderQcid > 0)
                {
                    if (QCCommentVM.StatusId == (int?)OrderStatusEnum.QCByPass)
                    {
                        objtblOrderQc.PriceAfterQc = tblExchangeOrder.ExchangePrice;
                        objtblOrderQc.QualityAfterQc = tblExchangeOrder.ProductCondition;
                        objtblOrderQc.Sweetener = (tblExchangeOrder.Sweetener != null && tblExchangeOrder.Sweetener > 0) ? tblExchangeOrder.Sweetener : 0;
                        objtblOrderQc.SweetenerBu = (tblExchangeOrder.SweetenerBu != null && tblExchangeOrder.SweetenerBu > 0) ? tblExchangeOrder.SweetenerBu : 0;
                        objtblOrderQc.SweetenerBp = (tblExchangeOrder.SweetenerBp != null && tblExchangeOrder.SweetenerBp > 0) ? tblExchangeOrder.SweetenerBp : 0;
                        objtblOrderQc.SweetenerDigi2l = (tblExchangeOrder.SweetenerDigi2l != null && tblExchangeOrder.SweetenerDigi2l > 0) ? tblExchangeOrder.SweetenerDigi2l : 0;
                        objtblOrderQc.Qcdate = _currentDatetime;
                        objtblOrderQc.Qccomments = TblOrderQc.Qccomments;
                        objtblOrderQc.StatusId = TblOrderQc.StatusId;
                        objtblOrderQc.OrderTransId = tblOrderTran.OrderTransId;
                        objtblOrderQc.ModifiedBy = UserId;
                        objtblOrderQc.ModifiedDate = _currentDatetime;
                        _orderQCRepository.Update(objtblOrderQc);
                    }
                    else
                    {
                        if (QCCommentVM.PriceAfterQC != null)
                        {
                            objtblOrderQc.SweetenerBu = TblOrderQc.SweetenerBu;
                            objtblOrderQc.SweetenerBp = TblOrderQc.SweetenerBp;
                            objtblOrderQc.SweetenerDigi2l = TblOrderQc.SweetenerDigi2l;
                            objtblOrderQc.Sweetener = TblOrderQc.Sweetener;
                            objtblOrderQc.CollectedAmount = QCCommentVM.CollectedAmount;
                        }
                        objtblOrderQc.PriceAfterQc = TblOrderQc.PriceAfterQc;
                        objtblOrderQc.QualityAfterQc = TblOrderQc.QualityAfterQc;
                        objtblOrderQc.Qcdate = _currentDatetime;
                        objtblOrderQc.Qccomments = TblOrderQc.Qccomments;
                        objtblOrderQc.StatusId = TblOrderQc.StatusId;
                        objtblOrderQc.OrderTransId = tblOrderTran.OrderTransId;
                        objtblOrderQc.ModifiedBy = UserId;
                        objtblOrderQc.ModifiedDate = _currentDatetime;
                        if (tblExchangeOrder.BusinessPartner.BusinessUnit.IsValidationBasedSweetner == true)
                        {
                            objtblOrderQc.IsInstallationValidated = QCCommentVM.IsInstallationValidated;
                            objtblOrderQc.IsInvoiceValidated = QCCommentVM.IsInvoiceValidated;

                        }
                        _orderQCRepository.Update(objtblOrderQc);
                    }
                    _orderQCRepository.SaveChanges();
                }
                else
                {
                    TblOrderQc.OrderTransId = tblOrderTran.OrderTransId;
                    if (TblOrderQc.StatusId == (int)OrderStatusEnum.Waitingforcustapproval)
                    {
                        TblOrderQc.PriceAfterQc = TblOrderQc.PriceAfterQc;
                        TblOrderQc.QualityAfterQc = TblOrderQc.QualityAfterQc;
                        TblOrderQc.Qcdate = _currentDatetime;
                        TblOrderQc.SweetenerBu = TblOrderQc.SweetenerBu;
                        TblOrderQc.SweetenerBp = TblOrderQc.SweetenerBp;
                        TblOrderQc.SweetenerDigi2l = TblOrderQc.SweetenerDigi2l;
                        TblOrderQc.Sweetener = TblOrderQc.Sweetener;
                    }
                    else if (QCCommentVM.StatusId == (int?)OrderStatusEnum.QCByPass)
                    {
                        TblOrderQc.PriceAfterQc = tblExchangeOrder.ExchangePrice;
                        TblOrderQc.QualityAfterQc = tblExchangeOrder.ProductCondition;
                        TblOrderQc.Sweetener = (tblExchangeOrder.Sweetener != null && tblExchangeOrder.Sweetener > 0) ? tblExchangeOrder.Sweetener : 0;
                        TblOrderQc.SweetenerBu = (tblExchangeOrder.SweetenerBu != null && tblExchangeOrder.SweetenerBu > 0) ? tblExchangeOrder.SweetenerBu : 0;
                        TblOrderQc.SweetenerBp = (tblExchangeOrder.SweetenerBp != null && tblExchangeOrder.SweetenerBp > 0) ? tblExchangeOrder.SweetenerBp : 0;
                        TblOrderQc.SweetenerDigi2l = (tblExchangeOrder.SweetenerDigi2l != null && tblExchangeOrder.SweetenerDigi2l > 0) ? tblExchangeOrder.SweetenerDigi2l : 0;
                        TblOrderQc.Qcdate = _currentDatetime;
                    }
                    else
                    {
                        TblOrderQc.PriceAfterQc = TblOrderQc.PriceAfterQc;
                    }
                    TblOrderQc.IsActive = true;
                    TblOrderQc.CreatedDate = _currentDatetime;
                    TblOrderQc.CreatedBy = UserId;
                    _orderQCRepository.Create(TblOrderQc);
                    _orderQCRepository.SaveChanges();
                }
                #endregion

                #region Code to update the tblOrderTran
                if (TblOrderQc != null)
                {
                    if (tblOrderTran != null && tblOrderTran.OrderTransId > 0)
                    {
                        if (TblOrderQc.StatusId == (int?)OrderStatusEnum.QCByPass)
                        {
                            tblOrderTran.FinalPriceAfterQc = tblExchangeOrder.ExchangePrice;
                        }
                        else
                        {
                            tblOrderTran.FinalPriceAfterQc = TblOrderQc.PriceAfterQc;
                            tblOrderTran.Sweetner = TblOrderQc.Sweetener != null ? (TblOrderQc.Sweetener > 0 ? TblOrderQc.Sweetener : 0) : 0;

                        }
                        tblOrderTran.StatusId = TblOrderQc.StatusId;
                        tblOrderTran.ModifiedBy = UserId;
                        tblOrderTran.ModifiedDate = _currentDatetime;
                        _orderTransRepository.Update(tblOrderTran);
                        _orderTransRepository.SaveChanges();
                    }
                }
                #endregion

                #region Insert FinalExchangePrice and update statusid in ExchangeOrder Table
                if (tblExchangeOrder.RegdNo == QCCommentVM.ExchangeOrderViewModel.RegdNo)
                {
                    if (QCCommentVM.PriceAfterQC != null && QCCommentVM.StatusId == (int)OrderStatusEnum.Waitingforcustapproval)
                    {
                        tblExchangeOrder.FinalExchangePrice = QCCommentVM.PriceAfterQC;
                        tblExchangeOrder.BaseExchangePrice = QCCommentVM.BasePrice != null ? QCCommentVM.BasePrice : 0;
                        if (QCCommentVM.TotalSweetner != null && QCCommentVM.TotalSweetner > 0)
                        {
                            tblExchangeOrder.IsFinalExchangePriceWithoutSweetner = false;
                        }
                        else
                        {
                            tblExchangeOrder.IsFinalExchangePriceWithoutSweetner = true;
                        }
                        tblExchangeOrder.OrderStatus = "Waiting For Customer Acceptance";
                        if (tblExchangeOrder?.BusinessPartner.BusinessUnit.IsValidationBasedSweetner == true)
                        {
                            //tblExchangeOrder.IsFinalExchangePriceWithoutSweetner = QCCommentVM.SweetnerAmount == 0 ? true : false;
                            //tblExchangeOrder.BaseExchangePrice = QCCommentVM.PriceAfterQC - QCCommentVM.SweetnerAmount;
                            tblExchangeOrder.InvoiceImageName = QCCommentVM.InvoiceImageName;
                        }
                        if (QCCommentVM.NewModelNoName != null)
                        {
                            tblExchangeOrder.ModelNumberId = Convert.ToInt32(QCCommentVM.NewModelNoName);
                        }
                    }
                    if (QCCommentVM.StatusId == (int?)OrderStatusEnum.QCByPass)
                    {
                        tblExchangeOrder.FinalExchangePrice = tblExchangeOrder.ExchangePrice;
                        tblExchangeOrder.OrderStatus = "QC By Pass";
                        //Modify by Pooja jatav
                        if (tblExchangeOrder?.IsExchangePriceWithoutSweetner == true)
                        {
                            tblExchangeOrder.IsFinalExchangePriceWithoutSweetner = true;
                        }
                        else
                        {
                            tblExchangeOrder.IsFinalExchangePriceWithoutSweetner = false;
                        }
                    }
                    tblExchangeOrder.StatusId = QCCommentVM.StatusId;
                    tblExchangeOrder.ModifiedBy = UserId;
                    tblExchangeOrder.ModifiedDate = _currentDatetime;

                    _ExchangeOrderRepository.Update(tblExchangeOrder);
                    _ExchangeOrderRepository.SaveChanges();
                }
                #endregion

                #region Create and update TblExchangeAbbstatusHistory
                tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                tblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(LoVEnum.Exchange);
                if (QCCommentVM.InstallationComment != null)
                {
                    tblExchangeAbbstatusHistory.Comment = "InstallationComment = " + QCCommentVM.InstallationComment + " Qccomments =" + QCCommentVM.Qccomments;
                }
                else
                {
                    tblExchangeAbbstatusHistory.Comment = QCCommentVM.Qccomments;
                }
                tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                tblExchangeAbbstatusHistory.RegdNo = tblExchangeOrder.RegdNo;
                tblExchangeAbbstatusHistory.CustId = tblExchangeOrder.CustomerDetailsId;
                tblExchangeAbbstatusHistory.StatusId = QCCommentVM.StatusId;
                //tblExchangeAbbstatusHistory.StatusId = Convert.ToInt32(OrderStatusEnum.AmountApprovedbyCustomerAfterQC);
                tblExchangeAbbstatusHistory.IsActive = true;
                tblExchangeAbbstatusHistory.CreatedBy = UserId;
                tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                tblExchangeAbbstatusHistory.JsonObjectString = JsonConvert.SerializeObject(tblExchangeAbbstatusHistory);
                _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                _exchangeABBStatusHistoryRepository.SaveChanges();
                #endregion
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "ManageQCComment", ex);
            }
            //return TblOrderQc.OrderQcid;
            return tblExchangeAbbstatusHistory.StatusHistoryId;
        }
        #endregion

        #region Method to get the QCComment by id 
        /// <summary>
        /// Method to get the QCComment by id 
        /// </summary>
        /// <param name="id">QCCommentId</param>
        /// <returns>QCCommentViewModel</returns>
        public QCCommentViewModel GetQCCommentById(int id)
        {
            QCCommentViewModel QCCommentVM = null;
            TblOrderQc TblOrderQc = null;

            try
            {
                TblOrderQc = _orderQCRepository.GetOrderQcById(id);
                if (TblOrderQc != null)
                {
                    QCCommentVM = _mapper.Map<TblOrderQc, QCCommentViewModel>(TblOrderQc);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetQCCommentById", ex);
            }
            return QCCommentVM;
        }
        #endregion

        #region Method to delete QCComment by id
        /// <summary>
        /// Method to delete QCComment by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletQCCommentById(int id)
        {
            bool flag = false;
            try
            {
                // TblOrderQc TblOrderQc = _orderQCRepository.GetSingle(x => x.IsActive == true && x.OrderQcid == id);
                TblOrderQc TblOrderQc = _orderQCRepository.GetOrderQcById(id);
                if (TblOrderQc != null)
                {
                    TblOrderQc.IsActive = false;
                    _orderQCRepository.Update(TblOrderQc);
                    _orderQCRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "DeletQCCommentById", ex);
            }
            return flag;
        }
        #endregion

        #region Method to get QC Flag
        /// <summary>
        /// Method to get QC Flag
        /// </summary>
        /// <param name></param>       
        /// <returns> list</returns>
        public List<ExchangeOrderStatusViewModel> GetQcFlag()
        {
            List<ExchangeOrderStatusViewModel> exchangeOrderStatusViews = null;

            List<TblExchangeOrderStatus> tblExchangeOrderStatuses = new List<TblExchangeOrderStatus>();
            try
            {
                tblExchangeOrderStatuses = _ExchangeOrderStatusRepository.GetList(x => x.IsActive == true
                && x.StatusName == "QC" && x.StatusCode != "3R" && x.StatusCode != "5R" && x.StatusCode != "5"
                && x.StatusCode != "3A" && x.StatusCode != "5P" && x.StatusCode != "3RA").ToList();


                if (tblExchangeOrderStatuses != null && tblExchangeOrderStatuses.Count > 0)
                {
                    exchangeOrderStatusViews = _mapper.Map<List<TblExchangeOrderStatus>, List<ExchangeOrderStatusViewModel>>(tblExchangeOrderStatuses);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetQcFlag", ex);
            }
            return exchangeOrderStatusViews;

        }
        #endregion

        #region Method to get PriceAfterQc by cust declare quality Added by Pooja Jatav
        /// <summary>
        /// Method to get PriceAfterQc according to brand,product category, type
        /// </summary>
        /// <param name="id">qCCommentViewModel</param>
        /// <returns>QCCommentViewModel</returns>
        public List<decimal> GetPriceAfterQc(string exchangerno, string QcQuailty, string custQuality, int UserId)
        {
            List<decimal> objfinal = new List<decimal>();
            List<decimal> Qcprice = new List<decimal>();
            decimal Actualpriceafterqc = 0;
            decimal QCDeclareprice = 0;
            decimal Cust_declaredPrice = 0;
            decimal AmountCollectFromCustomer = 0;
            TblExchangeOrder tblExchangeOrder = null;
            TblPriceMaster priceMasters = null;
            TblBrand tblBrand = null;
            try
            {
                tblExchangeOrder = _ExchangeOrderRepository.GetRegdNo(exchangerno);
                if (tblExchangeOrder != null)
                {
                    if (tblExchangeOrder.IsDefferedSettlement == true || Convert.ToInt32(tblExchangeOrder.IsDefferedSettlement) == 1)
                    {
                        priceMasters = _ExchangeOrderRepository.GetOrderPrices(tblExchangeOrder.ProductTypeId, tblExchangeOrder.BusinessPartner != null ? tblExchangeOrder.BusinessPartner.BusinessUnit.Login.PriceCode : null, tblExchangeOrder.CompanyName);

                        tblBrand = _brandRepository.GetSingle(where: x => x.Id == tblExchangeOrder.BrandId);
                        if (tblBrand.Name == "Others" && priceMasters != null)
                        {
                            if (QcQuailty == "Excellent")
                            {
                                QCDeclareprice = Convert.ToDecimal(priceMasters.QuoteP);
                                Actualpriceafterqc = QCDeclareprice;
                            }
                            else if (QcQuailty == "Good")
                            {
                                QCDeclareprice = Convert.ToDecimal(priceMasters.QuoteQ);
                                Actualpriceafterqc = QCDeclareprice;
                            }
                            else if (QcQuailty == "Average")
                            {
                                QCDeclareprice = Convert.ToDecimal(priceMasters.QuoteR);
                                Actualpriceafterqc = QCDeclareprice;
                            }
                            else if (QcQuailty == "NotWorking")
                            {
                                QCDeclareprice = Convert.ToDecimal(priceMasters.QuoteS);
                                Actualpriceafterqc = QCDeclareprice;
                            }
                            Qcprice.Add(AmountCollectFromCustomer);
                            if (tblExchangeOrder.Sweetener != null)
                            {
                                QCDeclareprice = (decimal)(QCDeclareprice + tblExchangeOrder.Sweetener);
                                Qcprice.Add(QCDeclareprice);
                            }
                            else
                            {
                                Qcprice.Add(QCDeclareprice);
                            }
                        }
                        else
                        {
                            if (priceMasters != null)
                            {
                                if (tblBrand.Name == priceMasters.BrandName1 || tblBrand.Name == priceMasters.BrandName2 || tblBrand.Name == priceMasters.BrandName3 || tblBrand.Name == priceMasters.BrandName4)
                                {
                                    if (QcQuailty == "Excellent")
                                    {
                                        QCDeclareprice = Convert.ToDecimal(priceMasters.QuotePHigh);
                                        Actualpriceafterqc = QCDeclareprice;
                                    }
                                    else if (QcQuailty == "Good")
                                    {
                                        QCDeclareprice = Convert.ToDecimal(priceMasters.QuoteQHigh);
                                        Actualpriceafterqc = QCDeclareprice;
                                    }
                                    else if (QcQuailty == "Average")
                                    {
                                        QCDeclareprice = Convert.ToDecimal(priceMasters.QuoteRHigh);
                                        Actualpriceafterqc = QCDeclareprice;
                                    }
                                    else if (QcQuailty == "NotWorking")
                                    {
                                        QCDeclareprice = Convert.ToDecimal(priceMasters.QuoteSHigh);
                                        Actualpriceafterqc = QCDeclareprice;
                                    }
                                }

                                Qcprice.Add(AmountCollectFromCustomer);
                                if (tblExchangeOrder.Sweetener != null)
                                {
                                    QCDeclareprice = (decimal)(QCDeclareprice + tblExchangeOrder.Sweetener);
                                    Qcprice.Add(QCDeclareprice);
                                }
                                else
                                {
                                    Qcprice.Add(QCDeclareprice);
                                }
                            }
                        }
                        Qcprice.Add(AmountCollectFromCustomer);
                        Qcprice.Add(QCDeclareprice);
                    }
                    else
                    {
                        if (tblExchangeOrder != null)
                        {
                            priceMasters = _ExchangeOrderRepository.GetOrderPrices(tblExchangeOrder.ProductTypeId, tblExchangeOrder.BusinessPartner != null ? tblExchangeOrder.BusinessPartner.BusinessUnit.Login.PriceCode : null, tblExchangeOrder.CompanyName);
                            tblBrand = _brandRepository.GetSingle(where: x => x.Id == tblExchangeOrder.BrandId);
                            if (tblBrand.Name == "Others" && priceMasters != null)
                            {
                                // QC declared price
                                if (QcQuailty == "Excellent")
                                {
                                    QCDeclareprice = Convert.ToDecimal(priceMasters.QuoteP);
                                    Actualpriceafterqc = QCDeclareprice;
                                }
                                else if (QcQuailty == "Good")
                                {
                                    QCDeclareprice = Convert.ToDecimal(priceMasters.QuoteQ);
                                    Actualpriceafterqc = QCDeclareprice;
                                }
                                else if (QcQuailty == "Average")
                                {
                                    QCDeclareprice = Convert.ToDecimal(priceMasters.QuoteR);
                                    Actualpriceafterqc = QCDeclareprice;
                                }
                                else if (QcQuailty == "NotWorking")
                                {
                                    QCDeclareprice = Convert.ToDecimal(priceMasters.QuoteS);
                                    Actualpriceafterqc = QCDeclareprice;
                                }

                                // cust declared price
                                if (custQuality == "Excellent")
                                {
                                    Cust_declaredPrice = Convert.ToDecimal(priceMasters.QuoteP);
                                }
                                else if (custQuality == "Good")
                                {
                                    Cust_declaredPrice = Convert.ToDecimal(priceMasters.QuoteQ);

                                }
                                else if (custQuality == "Average")
                                {
                                    Cust_declaredPrice = Convert.ToDecimal(priceMasters.QuoteR);
                                }
                                else if (custQuality == "Not Working")
                                {
                                    Cust_declaredPrice = Convert.ToDecimal(priceMasters.QuoteS);
                                }

                                if (tblExchangeOrder.Sweetener != null || tblExchangeOrder.Sweetener == 0)
                                {
                                    AmountCollectFromCustomer = Cust_declaredPrice - QCDeclareprice;
                                    Qcprice.Add(AmountCollectFromCustomer);
                                    Qcprice.Add(QCDeclareprice);
                                }
                                else
                                {
                                    if (QCDeclareprice > Cust_declaredPrice)
                                    {
                                        Qcprice.Add(AmountCollectFromCustomer);
                                        QCDeclareprice = (decimal)(QCDeclareprice + (tblExchangeOrder.Sweetener != null ? tblExchangeOrder.Sweetener : 0));
                                        Qcprice.Add(QCDeclareprice);
                                    }
                                    else
                                    {
                                        AmountCollectFromCustomer = (decimal)((tblExchangeOrder.ExchangePrice - (tblExchangeOrder.Sweetener != null ? tblExchangeOrder.Sweetener : 0)) - QCDeclareprice);
                                        Qcprice.Add(AmountCollectFromCustomer);
                                        QCDeclareprice = (decimal)(QCDeclareprice + (tblExchangeOrder.Sweetener != null ? tblExchangeOrder.Sweetener : 0));
                                        Qcprice.Add(QCDeclareprice);
                                    }
                                }

                            }
                            else if (priceMasters != null)
                            {
                                if (tblBrand.Name == priceMasters.BrandName1 || tblBrand.Name == priceMasters.BrandName2 || tblBrand.Name == priceMasters.BrandName3 || tblBrand.Name == priceMasters.BrandName4)
                                {
                                    if (QcQuailty == "Excellent")
                                    {
                                        QCDeclareprice = Convert.ToDecimal(priceMasters.QuotePHigh);
                                        Actualpriceafterqc = QCDeclareprice;
                                    }
                                    else if (QcQuailty == "Good")
                                    {
                                        QCDeclareprice = Convert.ToDecimal(priceMasters.QuoteQHigh);
                                        Actualpriceafterqc = QCDeclareprice;
                                    }
                                    else if (QcQuailty == "Average")
                                    {
                                        QCDeclareprice = Convert.ToDecimal(priceMasters.QuoteRHigh);
                                        Actualpriceafterqc = QCDeclareprice;
                                    }
                                    else if (QcQuailty == "NotWorking")
                                    {
                                        QCDeclareprice = Convert.ToDecimal(priceMasters.QuoteSHigh);
                                        Actualpriceafterqc = QCDeclareprice;
                                    }
                                }
                                // cust declared price
                                if (custQuality == "Excellent")
                                {
                                    Cust_declaredPrice = Convert.ToDecimal(priceMasters.QuotePHigh);
                                }
                                else if (custQuality == "Good")
                                {
                                    Cust_declaredPrice = Convert.ToDecimal(priceMasters.QuoteQHigh);

                                }
                                else if (custQuality == "Average")
                                {
                                    Cust_declaredPrice = Convert.ToDecimal(priceMasters.QuoteRHigh);
                                }
                                else if (custQuality == "Not Working")
                                {
                                    Cust_declaredPrice = Convert.ToDecimal(priceMasters.QuoteSHigh);
                                }

                                if (tblExchangeOrder.Sweetener != null || tblExchangeOrder.Sweetener == 0)
                                {
                                    AmountCollectFromCustomer = Cust_declaredPrice - QCDeclareprice;
                                    Qcprice.Add(AmountCollectFromCustomer);
                                    Qcprice.Add(QCDeclareprice);
                                }
                                else
                                {
                                    if (QCDeclareprice > Cust_declaredPrice)
                                    {
                                        Qcprice.Add(AmountCollectFromCustomer);
                                        QCDeclareprice = (decimal)(QCDeclareprice + (tblExchangeOrder.Sweetener != null ? tblExchangeOrder.Sweetener : 0));
                                        Qcprice.Add(QCDeclareprice);
                                    }
                                    else
                                    {
                                        AmountCollectFromCustomer = (decimal)((tblExchangeOrder.ExchangePrice - (tblExchangeOrder.Sweetener != null ? tblExchangeOrder.Sweetener : 0)) - QCDeclareprice);
                                        Qcprice.Add(AmountCollectFromCustomer);
                                        QCDeclareprice = (decimal)(QCDeclareprice + (tblExchangeOrder.Sweetener != null ? tblExchangeOrder.Sweetener : 0));
                                        Qcprice.Add(QCDeclareprice);
                                    }
                                }
                            }
                        }
                    }
                    Qcprice.Add(AmountCollectFromCustomer);
                    Qcprice.Add(QCDeclareprice);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetPriceAfterQc", ex);
            }
            return Qcprice;
        }
        #endregion

        #region Method to get the QCDetail by Exchangeid 
        /// <summary>
        /// Method to get the QCDetail by Exchangeid 
        /// </summary>
        /// <param name="id">ExchangeId</param>
        /// <returns>QCCommentViewModel</returns>
        public QCCommentViewModel GetQcDetails(int Id)
        {
            QCCommentViewModel QcCommentViewModel = new QCCommentViewModel();
            //ExchangeOrderViewModel exchangeOrderViewModel = new ExchangeOrderViewModel();
            TblOrderTran tblOrderTran = new TblOrderTran();
            TblOrderQc orderQc = new TblOrderQc();
            CashfreeAuth cashfreeAuthCall = new CashfreeAuth();
            TblWalletTransaction walletTransaction = new TblWalletTransaction();
            TblExchangeOrder exchangeOrder = null;
            AddBeneficiary addBeneficiarry = new AddBeneficiary();
            AddBeneficiaryResponse beneficiaryResponse = new AddBeneficiaryResponse();
            string subcode = Convert.ToInt32(CashfreeEnum.Succcess).ToString();
            GetBeneficiary getBeneficiarry = new GetBeneficiary();
            try
            {
                exchangeOrder = _ExchangeOrderRepository.GetSingleOrder(Id);
                if (exchangeOrder != null)
                {
                    tblOrderTran = _orderTransRepository.GetQcDetailsByExchangeId(exchangeOrder.Id);
                }
                if (tblOrderTran != null)
                {
                    orderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran.OrderTransId);

                    if (orderQc != null)
                    {
                        QcCommentViewModel = _mapper.Map<TblOrderQc, QCCommentViewModel>(orderQc);

                        if (QcCommentViewModel != null || QcCommentViewModel.StatusId != null)
                        {
                            QcCommentViewModel.ProductCategory = exchangeOrder.ProductType.ProductCat.Description;
                            TblExchangeOrderStatus tblExchangeOrderStatus = _ExchangeOrderStatusRepository.GetByStatusId(QcCommentViewModel.StatusId);
                            QcCommentViewModel.StatusCode = tblExchangeOrderStatus.StatusCode != null ? tblExchangeOrderStatus.StatusCode : string.Empty;
                            QcCommentViewModel.Sweetener = QcCommentViewModel.Sweetener != null ? QcCommentViewModel.Sweetener : 0;
                            QcCommentViewModel.SweetenerBu = QcCommentViewModel.SweetenerBu != null ? QcCommentViewModel.SweetenerBu : 0;
                            QcCommentViewModel.SweetenerBP = QcCommentViewModel.SweetenerBP != null ? QcCommentViewModel.SweetenerBP : 0;
                            QcCommentViewModel.SweetenerDigi2L = QcCommentViewModel.SweetenerDigi2L != null ? QcCommentViewModel.SweetenerDigi2L : 0;

                        }
                        if (QcCommentViewModel.PickupStartTime != null && QcCommentViewModel.PickupEndTime != null && QcCommentViewModel.PreferredPickupDate != null)
                        {
                            DateTime dt = Convert.ToDateTime(QcCommentViewModel.PreferredPickupDate);
                            string datedt = dt.ToString("dd/MM/yyyy");
                            QcCommentViewModel.PreferredPickupDate = datedt;
                            QcCommentViewModel.PickupDateTime = QcCommentViewModel.PreferredPickupDate + " " + QcCommentViewModel.PickupStartTime + "-" + QcCommentViewModel.PickupEndTime;
                        }

                        if (exchangeOrder.Qcdate != null && exchangeOrder.StartTime != null && exchangeOrder.EndTime != null)
                        {
                            QcCommentViewModel.QCDate = exchangeOrder.Qcdate + "" + exchangeOrder.StartTime + "" + exchangeOrder.EndTime;
                        }

                        //This code  is to set the EVC price.
                        //if (QcCommentViewModel.PriceAfterQC != null && exchangeOrder.StatusId > 15) //15 = Amount Approved by customer after QC
                        if (QcCommentViewModel.PriceAfterQC != null && exchangeOrder.StatusId > 15 && exchangeOrder.StatusId != (int?)OrderStatusEnum.Waitingforcustapproval && exchangeOrder.StatusId != (int?)OrderStatusEnum.QCByPass && exchangeOrder.StatusId != Convert.ToInt32(OrderStatusEnum.OrderWithDiagnostic) && exchangeOrder.StatusId != Convert.ToInt32(OrderStatusEnum.UpperBonusCapPending))
                        {
                            walletTransaction = _walletTransactionRepository.GetSingle(x => x.OrderTransId == tblOrderTran.OrderTransId);
                            if (walletTransaction != null)
                            {
                                QcCommentViewModel.PriceasperEVC = walletTransaction.OrderAmount;
                            }
                        }
                        //else if (QcCommentViewModel.PriceAfterQC != null && exchangeOrder.StatusId == (int?)OrderStatusEnum.AmountApprovedbyCustomerAfterQC || exchangeOrder.StatusId == (int?)OrderStatusEnum.QCByPass)
                        else if (QcCommentViewModel.PriceAfterQC != null && (exchangeOrder.StatusId == (int?)OrderStatusEnum.Waitingforcustapproval || exchangeOrder.StatusId == (int?)OrderStatusEnum.QCByPass || exchangeOrder.StatusId == (int?)OrderStatusEnum.AmountApprovedbyCustomerAfterQC || exchangeOrder.StatusId == Convert.ToInt32(OrderStatusEnum.OrderWithDiagnostic) || exchangeOrder.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled) || exchangeOrder.StatusId == Convert.ToInt32(OrderStatusEnum.UpperBonusCapPending)))
                        {
                            //Changes for DebitNote and Invoice new req Date 8-Jan-24
                            //var result = _commonManager.CalculateEVCPrice(tblOrderTran.OrderTransId);
                            //if (result > 0)
                            //{
                            //    QcCommentViewModel.PriceasperEVC = result;
                            //}
                            //else
                            //{
                            //    QcCommentViewModel.PriceasperEVC = 0;
                            //}
                            QcCommentViewModel.PriceasperEVC = null;
                        }
                        else
                        {
                            QcCommentViewModel.PriceasperEVC = 0;
                        }
                        if (orderQc != null && orderQc.Upiid == null)
                        {
                            QcCommentViewModel.IsUPINo = true;
                        }
                        else
                        {
                            cashfreeAuthCall = _cashfreePayoutCall.CashFreeAuthCall();
                            if (cashfreeAuthCall.subCode == subcode)
                            {
                                getBeneficiarry = _cashfreePayoutCall.GetBeneficiary(cashfreeAuthCall.data.token, tblOrderTran.RegdNo);
                                if (getBeneficiarry.subCode == subcode)
                                {
                                    QcCommentViewModel.IsUPINo = false;
                                    QcCommentViewModel.UPIId = QcCommentViewModel.UPIId;
                                }
                                else
                                {
                                    QcCommentViewModel.IsUPINo = true;
                                }
                            }
                        }

                        if (orderQc.DagnosticPdfName != null)
                        {
                            QcCommentViewModel.QuestionerPdfName = orderQc.DagnosticPdfName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetQcDetails", ex);
            }
            return QcCommentViewModel;
        }
        #endregion

        #region  Method to get the LGCDetail by transid
        /// <summary>
        /// Method to get the LGCDetail by transid
        /// </summary>
        /// <param name="id">transid</param>
        /// <returns>QCCommentViewModel</returns>
        public QCCommentViewModel GetLGCDetails(int TransId)
        {
            QCCommentViewModel QcCommentViewModel = new QCCommentViewModel();
            QcCommentViewModel.LogisticViewModel = new LogisticViewModel();
            QcCommentViewModel.OrderLGCViewModel = new OrderLGCViewModel();
            TblLogistic tblLogistic = null;
            TblServicePartner tblServicePartner = null;
            TblOrderLgc tblOrderLgc = null;
            TblEvcpoddetail tblEvcpoddetail = null;
            try
            {
                tblLogistic = _logisticsRepository.GetExchangeDetailsByOrdertransId(TransId);

                if (tblLogistic != null)
                {
                    //EVCPod Details
                    tblEvcpoddetail = _eVCPODDetailsRepository.GetSingle(x => x.IsActive == true && x.RegdNo == tblLogistic.RegdNo);
                    if (tblEvcpoddetail != null)
                    {
                        QcCommentViewModel.LogisticViewModel.PoDPdf = tblEvcpoddetail.Podurl;
                        QcCommentViewModel.LogisticViewModel.DebitNotePdf = tblEvcpoddetail.DebitNotePdfName;
                        QcCommentViewModel.LogisticViewModel.InvoieImagePdf = tblEvcpoddetail.InvoicePdfName;
                    }
                    //logistic information
                    QcCommentViewModel.LogisticViewModel.TicketNumber = tblLogistic.TicketNumber;
                    QcCommentViewModel.LogisticViewModel.ServicePartnerId = tblLogistic.ServicePartnerId;
                    QcCommentViewModel.LogisticViewModel.CreatedDate = tblLogistic.CreatedDate.ToString() != null ? tblLogistic.CreatedDate.ToString() : null;
                    QcCommentViewModel.LogisticViewModel.AmtPaybleThroughLGC = tblLogistic.AmtPaybleThroughLgc;
                    QcCommentViewModel.LogisticViewModel.LGCRescheduleDate = tblLogistic.PickupScheduleDate.ToString();
                    QcCommentViewModel.ServicePartnerName = tblLogistic.ServicePartner.ServicePartnerName;
                  
                    //tblorder lgc for order logistics data                    
                    tblOrderLgc = _orderLGCRepository.GetOrderDetailsByOrderTransId(TransId);
                    if (tblOrderLgc != null)
                    {
                        QcCommentViewModel.OrderLGCViewModel.Lgccomments = tblOrderLgc.Lgccomments;
                        QcCommentViewModel.OrderLGCViewModel.ProposedPickDate = tblOrderLgc.ProposedPickDate;
                        QcCommentViewModel.OrderLGCViewModel.ActualPickupDate = tblOrderLgc.ActualPickupDate.ToString() != null ? tblOrderLgc.ActualPickupDate.ToString() : null;
                        QcCommentViewModel.OrderLGCViewModel.ActualDropDate = tblOrderLgc.ActualDropDate.ToString() != null ? tblOrderLgc.ActualDropDate.ToString() : null;
                        TblExchangeOrderStatus tblExchangeOrderStatus = _ExchangeOrderStatusRepository.GetByStatusId(tblOrderLgc.StatusId);
                        if (tblExchangeOrderStatus != null)
                        {
                            QcCommentViewModel.OrderLGCViewModel.StatusCode = tblExchangeOrderStatus.StatusCode;
                        }
                        QcCommentViewModel.LogisticViewModel.CustomerDeclarationPdf = tblOrderLgc.CustDeclartionpdfname;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetQcDetails", ex);
            }
            return QcCommentViewModel;
        }
        #endregion

        #region Method to Get Voucher Details
        /// <summary>
        /// Method to Get Voucher Details
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns>VoucherDetailsViewModel</returns>
        public VoucherDetailsViewModel GetVoucherDetails(int exchangeid)
        {
            TblModelNumber modelNumber = null;
            TblBusinessPartner businessPartner = null;
            TblProductType producttype = null;
            TblProductCategory productCategory = null;
            VoucherDetailsViewModel voucherDetailsViewModel = new VoucherDetailsViewModel();
            try
            {

                if (exchangeid != 0)
                {
                    //need optimization
                    TblVoucherVerfication voucherVerfication = _voucherRepository.GetSingle(x => x.ExchangeOrderId == exchangeid);
                    if (voucherVerfication != null)
                    {
                        voucherDetailsViewModel = _mapper.Map<TblVoucherVerfication, VoucherDetailsViewModel>(voucherVerfication);
                        if (voucherDetailsViewModel != null)
                        {
                            modelNumber = _context.TblModelNumbers.FirstOrDefault(x => x.ModelNumberId == voucherDetailsViewModel.ModelNumberId);
                            if (modelNumber != null)
                            {
                                voucherDetailsViewModel.ModelNumber = modelNumber.ModelName != null ? modelNumber.ModelName : "";
                            }
                            businessPartner = _businessPartnerRepository.GetBPId(voucherDetailsViewModel.BusinessPartnerId);
                            if (businessPartner != null)
                            {
                                voucherDetailsViewModel.BusinessPartnerViewModel = _mapper.Map<TblBusinessPartner, BusinessPartnerViewModel>(businessPartner);
                                if (voucherDetailsViewModel.BusinessPartnerViewModel.IsVoucher == true && voucherDetailsViewModel.BusinessPartnerViewModel.VoucherType == 2)
                                {
                                    voucherDetailsViewModel.BusinessPartnerViewModel.BPName = null;
                                }
                                else
                                {
                                    voucherDetailsViewModel.BusinessPartnerViewModel.BPName = voucherDetailsViewModel.BusinessPartnerViewModel.Name + " " + voucherDetailsViewModel.BusinessPartnerViewModel.AddressLine1;
                                }
                            }
                            producttype = _productTypeRepository.GetSingle(x => x.Id == voucherDetailsViewModel.NewProductTypeId);
                            if (producttype != null)
                            {
                                voucherDetailsViewModel.ProductTypeViewModel = _mapper.Map<TblProductType, ProductTypeViewModel>(producttype);
                                if (voucherDetailsViewModel.ProductTypeViewModel != null)
                                {
                                    productCategory = _productCategoryRepository.GetSingle(x => x.Id == voucherDetailsViewModel.ProductTypeViewModel.ProductCatId);
                                    if (productCategory != null)
                                    {
                                        voucherDetailsViewModel.NewProductCategoryName = productCategory.Description;
                                    }
                                }
                                voucherDetailsViewModel.NewProductTypeName = voucherDetailsViewModel.ProductTypeViewModel.Description;
                                voucherDetailsViewModel.NewProductSize = voucherDetailsViewModel.ProductTypeViewModel.Size;
                            }
                            if (voucherDetailsViewModel.NewBrandId > 0)
                            {
                                TblBrand tblBrand = _brandRepository.GetBrand(voucherDetailsViewModel.NewBrandId);
                                voucherDetailsViewModel.NewBrandName = tblBrand != null ? tblBrand.Name : "";
                            }
                            TblVoucherStatus voucherStatus = _voucherStatusRepository.GetSingle(x => x.VoucherStatusId == voucherDetailsViewModel.VoucherStatusId);
                            if (voucherStatus != null)
                            {
                                voucherDetailsViewModel.VoucherStatusName = voucherStatus.VoucherStatusName;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetVoucherDetails", ex);
            }
            return voucherDetailsViewModel;
        }
        #endregion

        #region Method to Get Self QC Images by Regdno
        public IList<SelfQCViewModel> GetImagesUploadedBySelfQC(string regdNo)
        {
            List<SelfQCViewModel> selfQCVM = null;
            List<TblSelfQc> tblSelfQcs = new List<TblSelfQc>();
            try
            {
                if (regdNo != null)
                {
                    tblSelfQcs = _selfQCRepository.GetList(x => x.IsActive == true && x.RegdNo == regdNo).ToList();
                    if (tblSelfQcs != null)
                    {
                        selfQCVM = _mapper.Map<List<TblSelfQc>, List<SelfQCViewModel>>(tblSelfQcs);
                    }
                    else
                    {
                        selfQCVM = new List<SelfQCViewModel>();
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetImagesUploadedBySelfQC", ex);
            }
            return selfQCVM;
        }
        #endregion

        #region Method to Get Video QC Images by Regdno
        public IList<OrderImageUploadViewModel> GetImagesUploadedByVideoQC(string regdNo)
        {
            List<OrderImageUploadViewModel> OrderImageUploadVM = null;
            List<TblOrderImageUpload> tblOrderImageUpload = new List<TblOrderImageUpload>();
            TblOrderTran tblOrderTran = null;
            try
            {
                if (regdNo != null)
                {
                    tblOrderTran = _orderTransRepository.GetRegdno(regdNo);
                    if (tblOrderTran != null)
                    {
                        tblOrderImageUpload = _orderImageUploadRepository.GetList(x => x.IsActive == true && x.OrderTransId == tblOrderTran.OrderTransId && x.ImageUploadby == 23).ToList();
                        if (tblOrderImageUpload != null)
                        {
                            OrderImageUploadVM = _mapper.Map<List<TblOrderImageUpload>, List<OrderImageUploadViewModel>>(tblOrderImageUpload);
                        }
                        else
                        {
                            OrderImageUploadVM = new List<OrderImageUploadViewModel>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetImagesUploadedBySelfQC", ex);
            }
            return OrderImageUploadVM;
        }
        #endregion

        #region Method to Get LGC Images by Regdno
        public IList<OrderImageUploadViewModel> GetImagesUploadedByLGCQC(string regdNo)
        {
            List<OrderImageUploadViewModel> OrderImageUploadVM = null;
            List<TblOrderImageUpload> tblOrderImageUpload = new List<TblOrderImageUpload>();
            TblOrderTran tblOrderTran = null;
            try
            {
                if (regdNo != null)
                {
                    tblOrderTran = _orderTransRepository.GetRegdno(regdNo);
                    if (tblOrderTran != null)
                    {
                        tblOrderImageUpload = _orderImageUploadRepository.GetList(x => x.IsActive == true && x.OrderTransId == tblOrderTran.OrderTransId && x.LgcpickDrop == "Pickup").ToList();
                        if (tblOrderImageUpload != null)
                        {
                            OrderImageUploadVM = _mapper.Map<List<TblOrderImageUpload>, List<OrderImageUploadViewModel>>(tblOrderImageUpload);
                        }
                        else
                        {
                            OrderImageUploadVM = new List<OrderImageUploadViewModel>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetImagesUploadedBySelfQC", ex);
            }
            return OrderImageUploadVM;
        }
        #endregion

        #region Method to Update rescheduled date and qccomment
        /// <summary>
        /// Method to update rescheduled date and qc comment
        /// </summary>
        /// <param name="QCCommentVM"></param>
        /// <returns>QCCommentViewModel</returns>
        //public QCCommentViewModel Rescheduled(QCCommentViewModel QCComment, int UserId)
        public bool Rescheduled(QCCommentViewModel QCComment, int UserId)
        {
            TblOrderTran? orderTran = null;
            TblOrderQc tblOrderQC = null;
            //TblOrderQc tblOrderQC = new TblOrderQc();
            TblOrderQc orderQC = new TblOrderQc();
            TblExchangeOrder? exchangeOrder = null;
            try
            {
                if (QCComment != null)
                {
                    orderTran = _orderTransRepository.GetRegdno(QCComment.RegdNo);

                    tblOrderQC = _mapper.Map<QCCommentViewModel, TblOrderQc>(QCComment);

                    if (tblOrderQC != null)
                    {
                        if (QCComment.Reschedulecount > 3)
                        {
                            QCComment.StatusId = Convert.ToInt32(OrderStatusEnum.QCAppointmentRescheduled_3RA);
                        }
                        exchangeOrder = _ExchangeOrderRepository.GetRegdNo(QCComment.RegdNo);

                        if (exchangeOrder != null)
                        {
                            tblOrderQC = _orderQCRepository.GetQcorderBytransId(orderTran.OrderTransId);

                            if (tblOrderQC != null)
                            {
                                #region  Update in OrderQC table                              
                                if (tblOrderQC.OrderQcid > 0)
                                {
                                    tblOrderQC.Reschedulecount = QCComment.Reschedulecount;
                                    tblOrderQC.Qccomments = QCComment.Qccomments;
                                    tblOrderQC.StatusId = QCComment.StatusId;
                                    tblOrderQC.ProposedQcdate = QCComment.ProposedQcdate;
                                    tblOrderQC.ModifiedBy = UserId;
                                    tblOrderQC.ModifiedDate = _currentDatetime;
                                    _orderQCRepository.Update(tblOrderQC);
                                    _orderQCRepository.SaveChanges();
                                }
                                #endregion

                                #region Update entry in TblExchangeOrder
                                exchangeOrder.StatusId = QCComment.StatusId;
                                exchangeOrder.OrderStatus = "Order Rescheduled";
                                exchangeOrder.ModifiedBy = UserId;
                                exchangeOrder.ModifiedDate = _currentDatetime;
                                _ExchangeOrderRepository.Update(exchangeOrder);
                                _ExchangeOrderRepository.SaveChanges();
                                #endregion

                                #region Update in OrderTrans table                                
                                if (orderTran != null && orderTran.OrderTransId > 0)
                                {
                                    orderTran.StatusId = QCComment.StatusId;
                                    orderTran.ModifiedBy = UserId;
                                    orderTran.ModifiedDate = _currentDatetime;
                                    _orderTransRepository.Update(orderTran);
                                    _orderTransRepository.SaveChanges();
                                }
                                #endregion

                                #region Insert entry in TblExchangeAbbstatusHistory
                                TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                tblExchangeAbbstatusHistory.OrderType = (int)ExchangeABBEnum.Exchange;
                                tblExchangeAbbstatusHistory.SponsorOrderNumber = exchangeOrder.SponsorOrderNumber;
                                tblExchangeAbbstatusHistory.RegdNo = exchangeOrder.RegdNo;
                                tblExchangeAbbstatusHistory.Comment = QCComment.Qccomments;
                                tblExchangeAbbstatusHistory.CustId = exchangeOrder.CustomerDetailsId;
                                tblExchangeAbbstatusHistory.StatusId = QCComment.StatusId;
                                tblExchangeAbbstatusHistory.IsActive = true;
                                tblExchangeAbbstatusHistory.CreatedBy = UserId;
                                tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                tblExchangeAbbstatusHistory.OrderTransId = orderTran.OrderTransId;
                                tblExchangeAbbstatusHistory.JsonObjectString = JsonConvert.SerializeObject(tblExchangeAbbstatusHistory);
                                _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                                _exchangeABBStatusHistoryRepository.SaveChanges();
                                QCComment.Isrescheduled = true;
                                #endregion
                            }
                            else
                            {
                                #region Create entry in OrderQC table
                                tblOrderQC = new TblOrderQc();
                                tblOrderQC.Reschedulecount = QCComment.Reschedulecount;
                                tblOrderQC.Qccomments = QCComment.Qccomments;
                                tblOrderQC.OrderTransId = orderTran.OrderTransId;
                                tblOrderQC.StatusId = QCComment.StatusId;
                                tblOrderQC.ProposedQcdate = QCComment.ProposedQcdate;
                                tblOrderQC.IsActive = true;
                                tblOrderQC.CreatedBy = UserId;
                                tblOrderQC.CreatedDate = _currentDatetime;
                                _orderQCRepository.Create(tblOrderQC);
                                _orderQCRepository.SaveChanges();
                                #endregion

                                #region Update entry in TblExchangeOrder
                                exchangeOrder.StatusId = QCComment.StatusId;
                                exchangeOrder.OrderStatus = "Order Rescheduled";
                                exchangeOrder.ModifiedBy = UserId;
                                exchangeOrder.ModifiedDate = _currentDatetime;
                                _ExchangeOrderRepository.Update(exchangeOrder);
                                _ExchangeOrderRepository.SaveChanges();
                                #endregion

                                #region Update in OrderTrans table
                                if (orderTran != null && orderTran.OrderTransId > 0)
                                {
                                    orderTran.StatusId = QCComment.StatusId;
                                    orderTran.ModifiedBy = UserId;
                                    orderTran.ModifiedDate = _currentDatetime;
                                    _orderTransRepository.Update(orderTran);
                                    _orderTransRepository.SaveChanges();
                                }
                                #endregion

                                #region Insert entry in TblExchangeAbbstatusHistory
                                TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                tblExchangeAbbstatusHistory.OrderType = (int)ExchangeABBEnum.Exchange;
                                tblExchangeAbbstatusHistory.SponsorOrderNumber = exchangeOrder.SponsorOrderNumber;
                                tblExchangeAbbstatusHistory.RegdNo = exchangeOrder.RegdNo;
                                tblExchangeAbbstatusHistory.Comment = QCComment.Qccomments;
                                tblExchangeAbbstatusHistory.CustId = exchangeOrder.CustomerDetailsId;
                                tblExchangeAbbstatusHistory.StatusId = QCComment.StatusId;
                                tblExchangeAbbstatusHistory.IsActive = true;
                                tblExchangeAbbstatusHistory.CreatedBy = UserId;
                                tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                tblExchangeAbbstatusHistory.OrderTransId = orderTran.OrderTransId;
                                tblExchangeAbbstatusHistory.JsonObjectString = JsonConvert.SerializeObject(tblExchangeAbbstatusHistory);
                                _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                                _exchangeABBStatusHistoryRepository.SaveChanges();
                                QCComment.Isrescheduled = true;
                                #endregion
                            }
                        }
                        else
                        {
                            exchangeOrder = new TblExchangeOrder();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "Rescheduled", ex);
            }
            return QCComment.Isrescheduled;
        }
        #endregion

        #region Method to Cancel QC order by Id
        /// <summary>
        /// Method to get QC details By Exchangeid
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>QcCommentViewModel</returns>
        //public ExchangeOrderStatusViewModel GetQCCancelById(string RegdNo, string CancelComment, int UserId)
        public bool GetQCCancelById(string RegdNo, string CancelComment, int UserId)
        {
            QCCommentViewModel QcCommentViewModel = new QCCommentViewModel();
            // ExchangeOrderStatusViewModel exchangeOrderStatusViews = null;
            TblExchangeOrderStatus tblExchangeOrderStatuses = null;
            TblExchangeOrder exchangeOrder = null;
            TblOrderTran tblOrderTran = null;
            bool cancelflag = false;
            try
            {
                exchangeOrder = _ExchangeOrderRepository.GetRegdNo(RegdNo);

                if (exchangeOrder != null)
                {
                    tblExchangeOrderStatuses = _ExchangeOrderStatusRepository.GetSingle(x => x.IsActive == true && x.StatusCode == "0X");
                    // exchangeOrderStatusViews = _mapper.Map<TblExchangeOrderStatus, ExchangeOrderStatusViewModel>(tblExchangeOrderStatuses);
                    if (exchangeOrder != null && exchangeOrder.RegdNo == RegdNo)
                    {
                        exchangeOrder.StatusId = tblExchangeOrderStatuses != null ? tblExchangeOrderStatuses.Id : null;
                        exchangeOrder.OrderStatus = "Order Cancel";
                        exchangeOrder.ModifiedBy = UserId;
                        exchangeOrder.ModifiedDate = _currentDatetime;
                        _ExchangeOrderRepository.Update(exchangeOrder);
                        _ExchangeOrderRepository.SaveChanges();
                    }

                    tblOrderTran = _orderTransRepository.GetRegdno(RegdNo);
                    if (tblOrderTran != null)
                    {
                        tblOrderTran.StatusId = exchangeOrder.StatusId;
                        tblOrderTran.ModifiedBy = UserId;
                        tblOrderTran.ModifiedDate = _currentDatetime;
                        _orderTransRepository.Update(tblOrderTran);
                        _orderTransRepository.SaveChanges();

                        #region Create & Update in OrderQC table 
                        TblOrderQc tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran.OrderTransId);
                        if (tblOrderQc != null && tblOrderQc.OrderQcid > 0)
                        {
                            tblOrderQc.Qccomments = CancelComment;
                            tblOrderQc.StatusId = exchangeOrder.StatusId;
                            tblOrderQc.ModifiedBy = UserId;
                            tblOrderQc.ModifiedDate = _currentDatetime;
                            _orderQCRepository.Update(tblOrderQc);
                            _orderQCRepository.SaveChanges();
                        }                        
                        #endregion
                    }

                    #region Insert entry in TblExchangeAbbstatusHistory
                    TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                    tblExchangeAbbstatusHistory.OrderType = (int)ExchangeABBEnum.Exchange;
                    tblExchangeAbbstatusHistory.Comment = CancelComment;
                    tblExchangeAbbstatusHistory.SponsorOrderNumber = exchangeOrder.SponsorOrderNumber;
                    tblExchangeAbbstatusHistory.RegdNo = exchangeOrder.RegdNo;
                    tblExchangeAbbstatusHistory.CustId = exchangeOrder.CustomerDetailsId;
                    tblExchangeAbbstatusHistory.StatusId = tblExchangeOrderStatuses.Id;
                    tblExchangeAbbstatusHistory.IsActive = true;
                    tblExchangeAbbstatusHistory.CreatedBy = UserId;
                    tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                    tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                    _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                    _exchangeABBStatusHistoryRepository.SaveChanges();
                    cancelflag = true;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetQCByExchangeId", ex);
            }
            return cancelflag;
        }
        #endregion

        #region method to save selfQC images as FinalQCImages
        /// <summary>
        /// method to save selfQC images as FinalQCImages
        /// </summary>
        /// <param name="RegdNo"></param>
        /// <returns>flag</returns>
        public MessageResponseViewModel saveSelfQCImageAsFinalImage(string RegdNo, int UserId)
        {
            MessageResponseViewModel messageResponseViewModel = new MessageResponseViewModel();
            List<TblSelfQc> tblSelfQc = null;
            string sourcePath = _config.Value.SelfQCImagePath;
            string destinationPath = _config.Value.VideoQCImagePath;
            List<String> fileNameWithPath = new List<string>();
            string fileWithPath = string.Empty;
            TblOrderImageUpload tblOrderImageUpload = null;
            List<TblOrderImageUpload> tblOrderImageUploadsList = null;
            TblOrderTran tblOrderTran = null;
            TblLoV tblLoV = null;
            string lovName = "QC Team";
            string fileName = string.Empty;

            try
            {
                tblSelfQc = _context.TblSelfQcs.Where(x => x.IsActive == true && x.RegdNo == RegdNo).ToList();
                if (tblSelfQc.Count == 0)
                {
                    messageResponseViewModel.Flag = 1;
                    messageResponseViewModel.Message = "Customer Declared Images Not Uploaded Yet.";
                    return messageResponseViewModel;
                }
                tblOrderTran = _context.TblOrderTrans.Where(x => x.IsActive == true && x.RegdNo == RegdNo).FirstOrDefault();
                tblLoV = _context.TblLoVs.Where(x => x.IsActive == true && x.LoVname == lovName).FirstOrDefault();
                tblOrderImageUploadsList = _context.TblOrderImageUploads.Where(x => x.IsActive == true && x.OrderTransId == tblOrderTran.OrderTransId && x.ImageUploadby == tblLoV.LoVid).ToList();
                if (tblOrderImageUploadsList.Count > 0)
                {
                    messageResponseViewModel.Flag = 2;
                    messageResponseViewModel.Message = "You Already Saved Final QC Images.";
                    return messageResponseViewModel;
                }
                if (tblSelfQc != null && tblOrderImageUploadsList.Count == 0)
                {
                    foreach (var items in tblSelfQc)
                    {
                        fileWithPath = sourcePath + items.ImageName;
                        fileNameWithPath.Add(fileWithPath);
                    }
                    if (fileNameWithPath.Count > 0)
                    {
                        for (int i = 0; i < fileNameWithPath.Count; i++)
                        {
                            string extn = Path.GetExtension(fileNameWithPath[i]);
                            if (extn == ".jpg")
                            {
                                fileName = RegdNo + "_" + "FinalQCImage" + i + ".jpg";
                            }
                            else
                            {
                                fileName = RegdNo + "_" + "FinalQCImage" + i + ".webm";
                            }

                            File.Copy(fileNameWithPath[i], destinationPath + fileName, true);
                            tblOrderImageUpload = new TblOrderImageUpload();
                            tblOrderImageUpload.OrderTransId = tblOrderTran.OrderTransId;
                            tblOrderImageUpload.ImageName = fileName;
                            tblOrderImageUpload.ImageUploadby = tblLoV.LoVid;
                            tblOrderImageUpload.IsActive = true;
                            tblOrderImageUpload.CreatedBy = UserId;
                            tblOrderImageUpload.CreatedDate = DateTime.Now;
                            _orderImageUploadRepository.Create(tblOrderImageUpload);
                        }
                        _orderImageUploadRepository.SaveChanges();
                        messageResponseViewModel.Flag = 3;
                        messageResponseViewModel.Message = "Customer Declared Images Saved As Final QC Images.";
                        return messageResponseViewModel;
                    }
                    else
                    {
                        messageResponseViewModel.Flag = 1;
                        messageResponseViewModel.Message = "Customer Declared Images Not Uploaded Yet.";
                        return messageResponseViewModel;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "saveSelfQCImageAsFinalImage", ex);
            }
            return messageResponseViewModel;
        }
        #endregion

        #region Get cash voucher price 
        //public void GetCashVoucheronQc(string ExchangeOrderrno, string QcQuailty, decimal QCDeclareprice, int UserId)
        public void GetCashVoucheronQc(string ExchangeOrderrno, string QcQuailty, decimal QCDeclareprice)
        {
            TblExchangeOrder tblExchangeOrder = null;
            List<decimal> Qcprice = new List<decimal>();
            try
            {
                if (ExchangeOrderrno != null)
                {
                    tblExchangeOrder = _ExchangeOrderRepository.GetRegdNo(ExchangeOrderrno);
                    if (tblExchangeOrder != null)
                    {
                        TblBusinessPartner tblBusinessPartner = _businessPartnerRepository.GetBPId(tblExchangeOrder.BusinessPartnerId);
                        if (tblBusinessPartner != null && tblBusinessPartner.IsDefferedSettlement == true && tblBusinessPartner.IsVoucher == true && tblBusinessPartner.VoucherType == 2)
                        {
                            if (tblExchangeOrder.ProductCondition == QcQuailty && tblExchangeOrder.ExchangePrice == QCDeclareprice)
                            {
                                tblExchangeOrder.IsVoucherused = true;
                                tblExchangeOrder.VoucherStatusId = (int?)VoucherStatusEnum.Captured;
                                tblExchangeOrder.VoucherCodeExpDate = DateTime.Now.AddDays(7);
                                // tblExchangeOrder.ModifiedBy = UserId;
                                tblExchangeOrder.ModifiedDate = _currentDatetime;
                                _ExchangeOrderRepository.Update(tblExchangeOrder);
                                _ExchangeOrderRepository.SaveChanges();

                                TblVoucherVerfication tblVoucherVerfication = new TblVoucherVerfication();
                                tblVoucherVerfication.CustomerId = tblExchangeOrder.CustomerDetailsId;
                                tblVoucherVerfication.ExchangeOrderId = tblExchangeOrder.Id;
                                tblVoucherVerfication.ExchangePrice = QCDeclareprice;
                                tblVoucherVerfication.VoucherCode = tblExchangeOrder.VoucherCode;
                                tblVoucherVerfication.IsVoucherused = true;
                                tblVoucherVerfication.BusinessPartnerId = tblExchangeOrder.BusinessPartnerId;
                                tblVoucherVerfication.Sweetneer = tblExchangeOrder.Sweetener;
                                tblVoucherVerfication.VoucherStatusId = (int?)VoucherStatusEnum.Captured;
                                tblVoucherVerfication.IsActive = true;
                                tblVoucherVerfication.CreatedDate = _currentDatetime;
                                //tblVoucherVerfication.CreatedBy = UserId;
                                _voucherRepository.Create(tblVoucherVerfication);
                                _voucherRepository.SaveChanges();
                            }
                            else
                            {
                                TblHistory tblHistory = new TblHistory();
                                tblHistory.RegdNo = tblExchangeOrder.RegdNo;
                                tblHistory.VoucherCode = tblExchangeOrder.VoucherCode;
                                tblHistory.Sweetner = tblExchangeOrder.Sweetener;
                                tblHistory.ExchangeAmount = tblExchangeOrder.ExchangePrice;
                                tblHistory.ExchangeOrderId = tblExchangeOrder.Id;
                                tblHistory.CustId = tblExchangeOrder.CustomerDetailsId;
                                tblHistory.IsActive = true;
                                tblHistory.Createdate = _currentDatetime;
                                _historyRepository.Create(tblHistory);
                                _historyRepository.SaveChanges();

                                GenerateVoucher(tblExchangeOrder, QCDeclareprice);
                            }
                        }
                    }
                    else
                    {
                        tblExchangeOrder = new TblExchangeOrder();
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetPriceAfterQc", ex);
            }
        }
        #endregion        

        #region Genereate Voucher
        /// <summary>
        /// Method to generate the voucher code
        /// </summary>
        /// <param name="">exchangeOrderDetails, QCDeclareprice, UserId</param>
        /// <returns>string</returns>
        //public void GenerateVoucher(TblExchangeOrder exchangeOrderDetails, decimal QCDeclareprice, int UserId)
        public void GenerateVoucher(TblExchangeOrder exchangeOrderDetails, decimal QCDeclareprice)
        {
            try
            {
                DateTime date = DateTime.Now.TrimMilliseconds();
                WhatsappTemplate whatsapptemplate = new WhatsappTemplate();
                WhatasappResponse whatasappResponse = new WhatasappResponse();
                TblWhatsAppMessage tblwhatsappmessage = null;

                TblBusinessPartner tblBusinessPartner = _businessPartnerRepository.GetBPId(exchangeOrderDetails.BusinessPartnerId);
                if (tblBusinessPartner != null)
                {
                    TblBusinessUnit businessUnit = _businessUnitRepository.Getbyid(tblBusinessPartner.BusinessUnitId);

                    #region update in tblexchangeorder 
                    TblExchangeOrder tblExchangeOrder = _ExchangeOrderRepository.GetRegdNo(exchangeOrderDetails.RegdNo);
                    if (tblExchangeOrder != null)
                    {
                        tblExchangeOrder.VoucherCode = "R" + UniqueString.RandomNumberByLength(8);
                        tblExchangeOrder.IsVoucherused = true;
                        tblExchangeOrder.VoucherStatusId = (int?)VoucherStatusEnum.Captured;
                        tblExchangeOrder.ExchangePrice = QCDeclareprice;/* sucessObj.data.context.C1;*/
                        //tblExchangeOrder.ModifiedBy = UserId;
                        tblExchangeOrder.ModifiedDate = _currentDatetime;
                        _ExchangeOrderRepository.Update(tblExchangeOrder);
                        _ExchangeOrderRepository.SaveChanges();

                    }
                    #endregion

                    #region update in TblVoucherVerfication
                    TblVoucherVerfication tblVoucherVerfication = new TblVoucherVerfication();
                    tblVoucherVerfication.CustomerId = tblExchangeOrder.CustomerDetailsId;
                    tblVoucherVerfication.ExchangeOrderId = tblExchangeOrder.Id;
                    tblVoucherVerfication.VoucherCode = tblExchangeOrder.VoucherCode;
                    tblVoucherVerfication.IsVoucherused = true;
                    tblVoucherVerfication.BusinessPartnerId = tblExchangeOrder.BusinessPartnerId;
                    tblVoucherVerfication.Sweetneer = tblExchangeOrder.Sweetener;
                    tblVoucherVerfication.ExchangePrice = QCDeclareprice; /*sucessObj.data.context.C1;*/
                    tblVoucherVerfication.VoucherStatusId = (int?)VoucherStatusEnum.Captured;
                    tblVoucherVerfication.IsActive = true;
                    tblVoucherVerfication.CreatedDate = _currentDatetime;
                    // tblVoucherVerfication.CreatedBy = UserId;
                    _voucherRepository.Create(tblVoucherVerfication);
                    _voucherRepository.SaveChanges();
                    #endregion                   

                    TblBusinessUnit tblBusinessUnit = _businessUnitRepository.Getbyid(businessUnit.BusinessUnitId);
                    if (tblBusinessUnit != null)
                    {
                        #region Whatsapp Send and insert in Tblwhatsappmessage
                        TblCustomerDetail tblCustomerDetail = _customerDetailsRepository.GetCustDetails(tblExchangeOrder.CustomerDetailsId);
                        if (tblCustomerDetail != null)
                        {
                            WhatsappTemplate whatsappObj = new WhatsappTemplate();
                            whatsappObj.userDetails = new UserDetails();
                            whatsappObj.notification = new Notification();
                            whatsappObj.notification.@params = new SendVoucherOnWhatssapp();
                            whatsappObj.userDetails.number = tblCustomerDetail.PhoneNumber;
                            whatsappObj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
                            whatsappObj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
                            whatsappObj.notification.templateId = NotificationConstants.ReissueCashVoucher;
                            whatsappObj.notification.@params.voucherAmount = QCDeclareprice;
                            whatsappObj.notification.@params.BrandName = tblBusinessUnit.Name;
                            whatsappObj.notification.@params.voucherCode = tblExchangeOrder.VoucherCode;
                            whatsappObj.notification.@params.BrandName2 = tblBusinessUnit.Name;
                            whatsappObj.notification.@params.VoucherExpiry = DateTime.Now.AddDays(7).ToString();
                            string baseUrl = _baseConfig.Value.BaseURL + "CashVoucher/CashVoucherRevised?id=" + tblExchangeOrder.Id + "&&companyname=" + tblBusinessUnit.Name;
                            whatsappObj.notification.@params.VoucherLink = baseUrl;
                            string url = _baseConfig.Value.YellowAiUrl;
                            RestResponse restresponse = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                            if (restresponse.Content != null)
                            {
                                whatasappResponse = JsonConvert.DeserializeObject<WhatasappResponse>(restresponse.Content);
                                tblwhatsappmessage = new TblWhatsAppMessage();
                                tblwhatsappmessage.TemplateName = NotificationConstants.ReissueCashVoucher;
                                tblwhatsappmessage.IsActive = true;
                                tblwhatsappmessage.PhoneNumber = tblCustomerDetail.PhoneNumber;
                                tblwhatsappmessage.SendDate = DateTime.Now;
                                tblwhatsappmessage.MsgId = whatasappResponse.msgId;
                                _WhatsAppMessageRepository.Create(tblwhatsappmessage);
                                _WhatsAppMessageRepository.SaveChanges();
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GenerateVoucher", ex);
            }
        }
        #endregion

        #region Save Upi No & Pickup date
        public int SaveUpino(UPINoViewModel UpinoViewModel)
        {
            TblExchangeOrder tblExchangeOrder = null;
            TblOrderTran tblOrderTran = null;
            TblAbbregistration tblAbbregistration = null;
            TblAbbredemption tblAbbredemption = null;
            TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
            try
            {
                if (UpinoViewModel != null)
                {
                    #region Fill Time slot in TblOrderQc
                    if (UpinoViewModel.PreferredPickupTime != null)
                    {
                        if (UpinoViewModel.PreferredPickupTime.Equals("1"))
                        {
                            UpinoViewModel.PickupStartTime = "10:00AM";
                            UpinoViewModel.PickupEndTime = "12:00PM";
                        }
                        else if (UpinoViewModel.PreferredPickupTime.Equals("2"))
                        {
                            UpinoViewModel.PickupStartTime = "12:00PM";
                            UpinoViewModel.PickupEndTime = "2:00PM";
                        }
                        else if (UpinoViewModel.PreferredPickupTime.Equals("3"))
                        {
                            UpinoViewModel.PickupStartTime = "2:00PM";
                            UpinoViewModel.PickupEndTime = "4:00PM";
                        }
                        else if (UpinoViewModel.PreferredPickupTime.Equals("4"))
                        {
                            UpinoViewModel.PickupStartTime = "4:00PM";
                            UpinoViewModel.PickupEndTime = "6:00PM";
                        }
                        else if (UpinoViewModel.PreferredPickupTime.Equals("5"))
                        {
                            UpinoViewModel.PickupStartTime = "6:00PM";
                            UpinoViewModel.PickupEndTime = "8:00PM";
                        }

                    }
                    #endregion

                    tblOrderTran = _orderTransRepository.GetRegdno(UpinoViewModel.Regdno);
                    if (tblOrderTran != null && tblOrderTran.OrderType == (int?)LoVEnum.Exchange)
                    {
                        tblExchangeOrder = _ExchangeOrderRepository.GetRegdNo(UpinoViewModel.Regdno);
                        if (tblExchangeOrder != null)
                        {
                            #region code to update tblExchangeorder
                            if (UpinoViewModel.StatusId == (int)OrderStatusEnum.Waitingforcustapproval || UpinoViewModel.StatusId == (int)OrderStatusEnum.QCByPass)
                            {
                                tblExchangeOrder.StatusId = (int)OrderStatusEnum.AmountApprovedbyCustomerAfterQC;
                            }
                            tblExchangeOrder.OrderStatus = "Video QC done";
                            //tblExchangeOrder.Qcdate = _currentDatetime.ToString();
                            tblExchangeOrder.ModifiedDate = _currentDatetime;
                            tblExchangeOrder.ModifiedBy = UpinoViewModel.Userid;
                            _ExchangeOrderRepository.Update(tblExchangeOrder);
                            _ExchangeOrderRepository.SaveChanges();
                            #endregion

                            #region Create code for TblExchangeAbbstatusHistory
                            tblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(LoVEnum.Exchange);
                            tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                            tblExchangeAbbstatusHistory.RegdNo = tblExchangeOrder.RegdNo;
                            tblExchangeAbbstatusHistory.CustId = tblExchangeOrder.CustomerDetailsId;
                            if (UpinoViewModel.StatusId == (int)OrderStatusEnum.Waitingforcustapproval || UpinoViewModel.StatusId == (int)OrderStatusEnum.QCByPass)
                            {
                                tblExchangeAbbstatusHistory.StatusId = (int)OrderStatusEnum.AmountApprovedbyCustomerAfterQC;
                            }
                            tblExchangeAbbstatusHistory.IsActive = true;
                            tblExchangeAbbstatusHistory.CreatedBy = UpinoViewModel.Userid;
                            tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                            tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                            tblExchangeAbbstatusHistory.JsonObjectString = JsonConvert.SerializeObject(tblExchangeAbbstatusHistory);
                            _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                            _exchangeABBStatusHistoryRepository.SaveChanges();
                            #endregion
                        }
                    }
                    else
                    {
                        tblAbbregistration = _abbRegistrationRepository.GetRegdNo(UpinoViewModel.Regdno);
                        if (tblAbbregistration != null)
                        {
                            tblAbbredemption = _aBBRedemptionRepository.GetRegdNo(UpinoViewModel.Regdno);
                            #region code to update tblAbbredemption
                            if (tblAbbredemption != null)
                            {
                                if (UpinoViewModel.StatusId == (int)OrderStatusEnum.Waitingforcustapproval || UpinoViewModel.StatusId == (int)OrderStatusEnum.QCByPass)
                                {
                                    tblAbbredemption.StatusId = (int)OrderStatusEnum.AmountApprovedbyCustomerAfterQC;
                                }
                                tblAbbredemption.ModifiedDate = _currentDatetime;
                                tblAbbredemption.ModifiedBy = UpinoViewModel.Userid;
                                _aBBRedemptionRepository.Update(tblAbbredemption);
                                _aBBRedemptionRepository.SaveChanges();
                            }
                            #endregion

                            #region Create code for TblExchangeAbbstatusHistory
                            tblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(LoVEnum.ABB);
                            tblExchangeAbbstatusHistory.SponsorOrderNumber = tblAbbregistration.SponsorOrderNo != null ? tblAbbregistration.SponsorOrderNo : string.Empty;
                            tblExchangeAbbstatusHistory.RegdNo = tblAbbregistration.RegdNo;
                            tblExchangeAbbstatusHistory.CustId = tblAbbregistration.CustomerId;
                            if (UpinoViewModel.StatusId == (int)OrderStatusEnum.Waitingforcustapproval || UpinoViewModel.StatusId == (int)OrderStatusEnum.QCByPass)
                            {
                                tblExchangeAbbstatusHistory.StatusId = (int)OrderStatusEnum.AmountApprovedbyCustomerAfterQC;
                            }
                            tblExchangeAbbstatusHistory.IsActive = true;
                            tblExchangeAbbstatusHistory.CreatedBy = UpinoViewModel.Userid;
                            tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                            tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                            tblExchangeAbbstatusHistory.JsonObjectString = JsonConvert.SerializeObject(tblExchangeAbbstatusHistory);
                            _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                            _exchangeABBStatusHistoryRepository.SaveChanges();
                            #endregion
                        }


                    }

                    #region Code to update the tblOrderTran
                    if (tblOrderTran.OrderTransId > 0)
                    {
                        if (UpinoViewModel.StatusId == (int)OrderStatusEnum.Waitingforcustapproval || UpinoViewModel.StatusId == (int)OrderStatusEnum.QCByPass)
                        {
                            tblOrderTran.StatusId = (int)OrderStatusEnum.AmountApprovedbyCustomerAfterQC;
                        }
                        tblOrderTran.ModifiedDate = _currentDatetime;
                        tblOrderTran.ModifiedBy = UpinoViewModel.Userid;
                        _orderTransRepository.Update(tblOrderTran);
                        _orderTransRepository.SaveChanges();
                    }
                    #endregion

                    #region Code to update the TblorderQC 
                    TblOrderQc tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran.OrderTransId);
                    if (tblOrderQc != null)
                    {

                        if (UpinoViewModel.StatusId == (int)OrderStatusEnum.Waitingforcustapproval || UpinoViewModel.StatusId == (int)OrderStatusEnum.QCByPass)
                        {
                            tblOrderQc.StatusId = (int)OrderStatusEnum.AmountApprovedbyCustomerAfterQC;
                        }
                        tblOrderQc.Upiid = UpinoViewModel.UPIId;
                        tblOrderQc.PreferredPickupDate = Convert.ToDateTime(UpinoViewModel.PreferredPickupDate);
                        tblOrderQc.PickupStartTime = UpinoViewModel.PickupStartTime;
                        tblOrderQc.PickupEndTime = UpinoViewModel.PickupEndTime;
                        tblOrderQc.OrderTransId = tblOrderTran.OrderTransId;
                        //tblOrderQc.Qcdate = _currentDatetime;
                        tblOrderQc.ModifiedDate = _currentDatetime;
                        tblOrderQc.ModifiedBy = UpinoViewModel.Userid;
                        _orderQCRepository.Update(tblOrderQc);
                        _orderQCRepository.SaveChanges();
                    }
                    #endregion

                    if (tblOrderTran != null && tblOrderTran.OrderType == (int?)LoVEnum.Exchange)
                    {
                        #region Set cash voucher capture & generate New Revised voucher
                        if (tblExchangeOrder.StatusId == (int)OrderStatusEnum.AmountApprovedbyCustomerAfterQC)
                        {
                            if ((tblOrderQc.QualityAfterQc == "Excellent" || tblOrderQc.QualityAfterQc == "Working" || tblOrderQc.QualityAfterQc == "Well Maintained") && (tblExchangeOrder.ProductCondition == EnumHelper.DescriptionAttr(QCCommentViewModelEnum.Excellent)) && tblExchangeOrder.ExchangePrice == tblOrderQc.PriceAfterQc && tblExchangeOrder.VoucherCode != null)
                            {
                                #region Update in tblExchangeOrder and Insert in tblVoucherVerfication
                                tblExchangeOrder.IsVoucherused = true;
                                tblExchangeOrder.VoucherStatusId = (int?)VoucherStatusEnum.Captured;
                                tblExchangeOrder.ModifiedBy = UpinoViewModel.Userid;
                                tblExchangeOrder.ModifiedDate = _currentDatetime;
                                _ExchangeOrderRepository.Update(tblExchangeOrder);
                                _ExchangeOrderRepository.SaveChanges();

                                TblVoucherVerfication tblVoucherVerfication = new TblVoucherVerfication();
                                tblVoucherVerfication.CustomerId = tblExchangeOrder.CustomerDetailsId;
                                tblVoucherVerfication.ExchangeOrderId = tblExchangeOrder.Id;
                                tblVoucherVerfication.ExchangePrice = tblExchangeOrder.ExchangePrice;
                                tblVoucherVerfication.VoucherCode = tblExchangeOrder.VoucherCode;
                                tblVoucherVerfication.IsVoucherused = true;
                                tblVoucherVerfication.BusinessPartnerId = tblExchangeOrder.BusinessPartnerId;
                                tblVoucherVerfication.Sweetneer = tblExchangeOrder.Sweetener;
                                tblVoucherVerfication.VoucherStatusId = (int?)VoucherStatusEnum.Captured;
                                tblVoucherVerfication.IsActive = true;
                                tblVoucherVerfication.CreatedDate = _currentDatetime;
                                tblVoucherVerfication.CreatedBy = UpinoViewModel.Userid;
                                _voucherRepository.Create(tblVoucherVerfication);
                                _voucherRepository.SaveChanges();
                                #endregion
                            }
                            else if (tblOrderQc.QualityAfterQc == "Good" && tblExchangeOrder.ProductCondition == EnumHelper.DescriptionAttr(QCCommentViewModelEnum.Good) && tblExchangeOrder.ExchangePrice == tblOrderQc.PriceAfterQc && tblExchangeOrder.VoucherCode != null)
                            {
                                #region Update in tblExchangeOrder and Insert in tblVoucherVerfication
                                tblExchangeOrder.IsVoucherused = true;
                                tblExchangeOrder.VoucherStatusId = (int?)VoucherStatusEnum.Captured;
                                tblExchangeOrder.ModifiedBy = UpinoViewModel.Userid;
                                tblExchangeOrder.ModifiedDate = _currentDatetime;
                                _ExchangeOrderRepository.Update(tblExchangeOrder);
                                _ExchangeOrderRepository.SaveChanges();

                                TblVoucherVerfication tblVoucherVerfication = new TblVoucherVerfication();
                                tblVoucherVerfication.CustomerId = tblExchangeOrder.CustomerDetailsId;
                                tblVoucherVerfication.ExchangeOrderId = tblExchangeOrder.Id;
                                tblVoucherVerfication.ExchangePrice = tblExchangeOrder.ExchangePrice;
                                tblVoucherVerfication.VoucherCode = tblExchangeOrder.VoucherCode;
                                tblVoucherVerfication.IsVoucherused = true;
                                tblVoucherVerfication.BusinessPartnerId = tblExchangeOrder.BusinessPartnerId;
                                tblVoucherVerfication.Sweetneer = tblExchangeOrder.Sweetener;
                                tblVoucherVerfication.VoucherStatusId = (int?)VoucherStatusEnum.Captured;
                                tblVoucherVerfication.IsActive = true;
                                tblVoucherVerfication.CreatedDate = _currentDatetime;
                                tblVoucherVerfication.CreatedBy = UpinoViewModel.Userid;
                                _voucherRepository.Create(tblVoucherVerfication);
                                _voucherRepository.SaveChanges();
                                #endregion
                            }
                            else if ((tblOrderQc.QualityAfterQc == "Average" || tblOrderQc.QualityAfterQc == "Heavily Used") && (tblExchangeOrder.ProductCondition == EnumHelper.DescriptionAttr(QCCommentViewModelEnum.Average)) && tblExchangeOrder.ExchangePrice == tblOrderQc.PriceAfterQc && tblExchangeOrder.VoucherCode != null)
                            {
                                #region Update in tblExchangeOrder and Insert in tblVoucherVerfication
                                tblExchangeOrder.IsVoucherused = true;
                                tblExchangeOrder.VoucherStatusId = (int?)VoucherStatusEnum.Captured;
                                tblExchangeOrder.ModifiedBy = UpinoViewModel.Userid;
                                tblExchangeOrder.ModifiedDate = _currentDatetime;
                                _ExchangeOrderRepository.Update(tblExchangeOrder);
                                _ExchangeOrderRepository.SaveChanges();

                                TblVoucherVerfication tblVoucherVerfication = new TblVoucherVerfication();
                                tblVoucherVerfication.CustomerId = tblExchangeOrder.CustomerDetailsId;
                                tblVoucherVerfication.ExchangeOrderId = tblExchangeOrder.Id;
                                tblVoucherVerfication.ExchangePrice = tblExchangeOrder.ExchangePrice;
                                tblVoucherVerfication.VoucherCode = tblExchangeOrder.VoucherCode;
                                tblVoucherVerfication.IsVoucherused = true;
                                tblVoucherVerfication.BusinessPartnerId = tblExchangeOrder.BusinessPartnerId;
                                tblVoucherVerfication.Sweetneer = tblExchangeOrder.Sweetener;
                                tblVoucherVerfication.VoucherStatusId = (int?)VoucherStatusEnum.Captured;
                                tblVoucherVerfication.IsActive = true;
                                tblVoucherVerfication.CreatedDate = _currentDatetime;
                                tblVoucherVerfication.CreatedBy = UpinoViewModel.Userid;
                                _voucherRepository.Create(tblVoucherVerfication);
                                _voucherRepository.SaveChanges();
                                #endregion
                            }
                            else if (tblOrderQc.QualityAfterQc == "NotWorking" && tblExchangeOrder.ProductCondition == EnumHelper.DescriptionAttr(QCCommentViewModelEnum.NotWorking) && tblExchangeOrder.ExchangePrice == tblOrderQc.PriceAfterQc && tblExchangeOrder.VoucherCode != null)
                            {
                                #region Update in tblExchangeOrder and Insert in tblVoucherVerfication
                                tblExchangeOrder.IsVoucherused = true;
                                tblExchangeOrder.VoucherStatusId = (int?)VoucherStatusEnum.Captured;
                                tblExchangeOrder.ModifiedBy = UpinoViewModel.Userid;
                                tblExchangeOrder.ModifiedDate = _currentDatetime;
                                _ExchangeOrderRepository.Update(tblExchangeOrder);
                                _ExchangeOrderRepository.SaveChanges();

                                TblVoucherVerfication tblVoucherVerfication = new TblVoucherVerfication();
                                tblVoucherVerfication.CustomerId = tblExchangeOrder.CustomerDetailsId;
                                tblVoucherVerfication.ExchangeOrderId = tblExchangeOrder.Id;
                                tblVoucherVerfication.ExchangePrice = tblExchangeOrder.ExchangePrice;
                                tblVoucherVerfication.VoucherCode = tblExchangeOrder.VoucherCode;
                                tblVoucherVerfication.IsVoucherused = true;
                                tblVoucherVerfication.BusinessPartnerId = tblExchangeOrder.BusinessPartnerId;
                                tblVoucherVerfication.Sweetneer = tblExchangeOrder.Sweetener;
                                tblVoucherVerfication.VoucherStatusId = (int?)VoucherStatusEnum.Captured;
                                tblVoucherVerfication.IsActive = true;
                                tblVoucherVerfication.CreatedDate = _currentDatetime;
                                tblVoucherVerfication.CreatedBy = UpinoViewModel.Userid;
                                _voucherRepository.Create(tblVoucherVerfication);
                                _voucherRepository.SaveChanges();
                                #endregion
                            }
                            else
                            {
                                GetCashVoucheronQc(tblExchangeOrder.RegdNo, tblOrderQc.QualityAfterQc, (decimal)tblOrderQc.PriceAfterQc);
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "SaveUpino", ex);
            }
            return tblExchangeAbbstatusHistory.StatusHistoryId;
        }
        #endregion

        #region get product details by regdno for questioners
        /// <summary>
        /// get product details by regdno for questioners
        /// </summary>
        /// <returns>questionerViewModel</returns>
        public QuestionerViewModel GetProductDetailsByRegdNo(string regdNo)
        {
            QuestionerViewModel questionerViewModel = null;
            TblOrderTran? tblOrderTran = null;
            try
            {
                if (!string.IsNullOrEmpty(regdNo))
                {
                    tblOrderTran = _orderTransRepository.GetOrderDetailsByRegdNo(regdNo);
                    if (tblOrderTran != null && tblOrderTran.Exchange != null)
                    {
                        questionerViewModel = new QuestionerViewModel();
                        questionerViewModel.TblProductCategory = tblOrderTran?.Exchange?.ProductType?.ProductCat;
                        questionerViewModel.TblProductType = tblOrderTran?.Exchange?.ProductType;
                        questionerViewModel.TblBrand = tblOrderTran?.Exchange?.Brand;
                        questionerViewModel.Sweetner = Convert.ToDecimal(tblOrderTran?.Exchange?.Sweetener != null ? tblOrderTran?.Exchange?.Sweetener : 0);
                        questionerViewModel.OrderTrandId = tblOrderTran.OrderTransId;
                        questionerViewModel.TblCustomerDetail = tblOrderTran?.Exchange?.CustomerDetails;
                        questionerViewModel.TblExchangeOrder = tblOrderTran?.Exchange;
                        questionerViewModel.ProdCatId = tblOrderTran?.Exchange?.ProductType?.ProductCat?.Id;
                        questionerViewModel.ProdTypeId = tblOrderTran?.Exchange?.ProductType?.Id;
                        questionerViewModel.ProdTechId = tblOrderTran?.Exchange?.ProductTechnology?.ProductTechnologyId;
                        questionerViewModel.IsDiagnoseV2 = tblOrderTran.Exchange.IsDiagnoseV2;
                    }
                    else
                    {
                        questionerViewModel = new QuestionerViewModel();
                    }
                }
                else
                {
                    questionerViewModel = new QuestionerViewModel();
                }
            }
            catch (Exception ex)
            {

                _logging.WriteErrorToDB("QCCommentManager", "GetProductDetailsByRegdNo", ex);
            }
            return questionerViewModel;
        }
        #endregion

        #region Get ASP by producttypeid and Techid
        /// <summary>
        /// Get ASP by producttypeid and Techid
        /// </summary>
        /// <param name="regdNo"></param>
        /// <returns>result</returns>
        public decimal GetASP(int productTypeId, int techId, int brandId)
        {
            decimal result = 0;
            TblPriceMasterQuestioner tblPriceMasterQuestioner = null;
            try
            {
                if (productTypeId > 0 && techId > 0)
                {
                    tblPriceMasterQuestioner = _context.TblPriceMasterQuestioners.Where(x => x.IsActive == true && x.ProductTypeId == productTypeId && x.ProductTechnologyId == techId).FirstOrDefault();
                    if (tblPriceMasterQuestioner == null)
                    {
                        return result;
                    }
                    if (tblPriceMasterQuestioner != null && brandId == Convert.ToInt32(ConfigurationEnum.BrandId))
                    {
                        result = Convert.ToDecimal(tblPriceMasterQuestioner.AverageSellingPrice);
                    }
                    else
                    {
                        result = Convert.ToDecimal(tblPriceMasterQuestioner.AverageSellingPrice) + (Convert.ToDecimal(tblPriceMasterQuestioner.AverageSellingPrice) * Convert.ToInt32(ConfigurationEnum.BrandSpecificASP) / 100);
                        int numberRound = Convert.ToInt32(result);
                        result = Convert.ToDecimal(Math.Round(numberRound / 10.0) * 10);
                    }
                }
                else
                {
                    return result;
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetASP", ex);
            }

            return result;
        }
        #endregion

        #region Get NonWorkingPrice by producttypeid and Techid
        /// <summary>
        /// Get NonWorkingPrice by producttypeid and Techid
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <param name="techId"></param>
        /// <returns></returns>
        public decimal GetNonWorkingPrice(int productTypeId, int techId)
        {
            decimal result = 0;
            TblPriceMasterQuestioner tblPriceMasterQuestioner = null;
            try
            {
                if (productTypeId > 0 && techId > 0)
                {
                    tblPriceMasterQuestioner = _context.TblPriceMasterQuestioners.Where(x => x.IsActive == true && x.ProductTypeId == productTypeId && x.ProductTechnologyId == techId).FirstOrDefault();
                    if (tblPriceMasterQuestioner != null)
                    {
                        result = Convert.ToDecimal(tblPriceMasterQuestioner.NonWorkingPrice);
                    }
                    else
                    {
                        return result;
                    }
                }
                else
                {
                    return result;
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetASP", ex);
            }

            return result;
        }
        #endregion

        #region get all question for rating by product category id
        /// <summary>
        /// get all question for rating by product category id
        /// </summary>
        /// <param name="prodCatId"></param>
        /// <returns></returns>
        public List<QCRatingViewModel> GetDynamicQuestionbyProdCatId(int prodCatId)
        {
            List<QCRatingViewModel> qCRatingList = new List<QCRatingViewModel>();
            List<TblQcratingMaster> tblQcratingMasterList = null;
            try
            {
                if (prodCatId > 0)
                {
                    tblQcratingMasterList = _context.TblQcratingMasters.Where(x => x.IsActive == true && x.ProductCatId == prodCatId && x.IsDiagnoseV2 == null).ToList();
                    if (tblQcratingMasterList.Count > 0 || tblQcratingMasterList != null)
                    {
                        qCRatingList = _mapper.Map<List<TblQcratingMaster>, List<QCRatingViewModel>>(tblQcratingMasterList);
                        if (qCRatingList.Count > 0)
                        {
                            return qCRatingList;
                        }
                    }
                    else
                    {
                        return qCRatingList;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetDynamicQuestionbyProdCatId", ex);
            }
            return qCRatingList;
        }
        #endregion

        #region get all question for rating by product category id for version 2
        /// <summary>
        /// get all question for rating by product category id
        /// </summary>
        /// <param name="prodCatId"></param>
        /// <returns></returns>
        public List<QCRatingViewModel> GetDynamicQuestionbyProdCatIdV2(int prodCatId)
        {
            List<QCRatingViewModel> qCRatingList = new List<QCRatingViewModel>();
            List<TblQcratingMaster> tblQcratingMasterList = null;
            try
            {
                if (prodCatId > 0)
                {
                    tblQcratingMasterList = _context.TblQcratingMasters.Where(x => x.IsActive == true && x.ProductCatId == prodCatId && x.IsDiagnoseV2 == true).ToList();
                    if (tblQcratingMasterList.Count > 0 || tblQcratingMasterList != null)
                    {
                        qCRatingList = _mapper.Map<List<TblQcratingMaster>, List<QCRatingViewModel>>(tblQcratingMasterList);
                        if (qCRatingList.Count > 0)
                        {
                            return qCRatingList;
                        }
                    }
                    else
                    {
                        return qCRatingList;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetDynamicQuestionbyProdCatId", ex);
            }
            return qCRatingList;
        }
        #endregion

        #region get the quoted price on selection of answer key
        /// <summary>
        /// get the quoted price on selection of answer key
        /// </summary>
        /// <param name="qCRatingViewModels"></param>
        /// <returns></returns>
        public List<double> GetQuotedPrice(List<QCRatingViewModel> qCRatingViewModels)
        {
            var averageSellingPrice = qCRatingViewModels[0].AverageSellingPrice;
            var sweetner = 0.00;
            var calculatedWeightage = 0.00;
            var excellentPrice = 0.00;
            var quotedPrice = 0.00;
            var quotedPriceWithSweetner = 0.00;
            var finalPrice = 0.00;
            List<double> priceList = new List<double>();
            try
            {
                if (qCRatingViewModels.Count > 0)
                {
                    foreach (var item in qCRatingViewModels)
                    {
                        if (item.QuestionerLovid == Convert.ToInt32(QuestionerLOV.Upper_Boolen))
                        {
                            if (item.Condition == Convert.ToInt32(QuestionerLOV.Upper_Yes) || item.Condition == Convert.ToInt32(QuestionerLOV.Upper_No))
                            {
                                var Weightage = GetWeighatge((int)item.Condition, item.QcratingId);
                                calculatedWeightage += Weightage;
                            }
                        }
                        else if (item.QuestionerLovid == Convert.ToInt32(QuestionerLOV.Lower_Boolen))
                        {
                            if (item.Condition == Convert.ToInt32(QuestionerLOV.Lower_Yes) || item.Condition == Convert.ToInt32(QuestionerLOV.Lower_No))
                            {
                                var Weightage = GetWeighatge((int)item.Condition, item.QcratingId);
                                calculatedWeightage += Weightage;
                            }
                        }
                        else if (item.QuestionerLovid == Convert.ToInt32(QuestionerLOV.Numeric))
                        {
                            if (item.Condition == Convert.ToInt32(QuestionerLOV.Zero_To_One) || item.Condition == Convert.ToInt32(QuestionerLOV.Two) || item.Condition == Convert.ToInt32(QuestionerLOV.Three) || item.Condition == Convert.ToInt32(QuestionerLOV.Four) ||
                                item.Condition == Convert.ToInt32(QuestionerLOV.Five) || item.Condition == Convert.ToInt32(QuestionerLOV.Six) || item.Condition == Convert.ToInt32(QuestionerLOV.Seven) || item.Condition == Convert.ToInt32(QuestionerLOV.Eight) ||
                                item.Condition == Convert.ToInt32(QuestionerLOV.Nine) || item.Condition == Convert.ToInt32(QuestionerLOV.TenPlus))
                            {
                                var Weightage = GetWeighatge((int)item.Condition, item.QcratingId);
                                calculatedWeightage += Weightage;
                            }
                        }
                        else if (item.QuestionerLovid == Convert.ToInt32(QuestionerLOV.Upper_Range))
                        {
                            if (item.Condition == Convert.ToInt32(QuestionerLOV.Zero_Percentage) || item.Condition == Convert.ToInt32(QuestionerLOV.Upto_Fifty_Percentage) || item.Condition == Convert.ToInt32(QuestionerLOV.FiftyOne_Ninty_Percentage) || item.Condition == Convert.ToInt32(QuestionerLOV.More_Than_Ninty_Percentage))
                            {
                                var Weightage = GetWeighatge((int)item.Condition, item.QcratingId);
                                calculatedWeightage += Weightage;
                            }
                        }
                        else if (item.QuestionerLovid == Convert.ToInt32(QuestionerLOV.Lower_Range))
                        {
                            if (item.Condition == Convert.ToInt32(QuestionerLOV.Lower_Zero_Percentage) || item.Condition == Convert.ToInt32(QuestionerLOV.Upto_Ten_Percentage) || item.Condition == Convert.ToInt32(QuestionerLOV.Eleven_To_TwentyFive_Percentage) || item.Condition == Convert.ToInt32(QuestionerLOV.More_Than_TwentyFive))
                            {
                                var Weightage = GetWeighatge((int)item.Condition, item.QcratingId);
                                calculatedWeightage += Weightage;
                            }
                        }


                    }
                }
                if (averageSellingPrice > 0)
                {
                    var aspPercentile = _config.Value.ASPPercentage;
                    excellentPrice = (averageSellingPrice * Convert.ToDouble(aspPercentile)) / 100;
                    excellentPrice = Math.Round(excellentPrice / 10.0) * 10;
                    if (calculatedWeightage > 0 && excellentPrice > 0)
                    {
                        quotedPrice = (excellentPrice * calculatedWeightage) / 100;
                        quotedPrice = Math.Round(quotedPrice / 10.0) * 10;
                        priceList.Add(excellentPrice);
                        priceList.Add(quotedPrice);
                    }
                }
                if (qCRatingViewModels[0].Sweetner > 0)
                {
                    sweetner = Convert.ToDouble(qCRatingViewModels[0].Sweetner);
                    priceList.Add(Convert.ToDouble(sweetner));
                }
                else
                {
                    priceList.Add(Convert.ToDouble(sweetner));
                }
                if (sweetner > 0)
                {
                    quotedPriceWithSweetner = quotedPrice + sweetner;
                    finalPrice = quotedPrice + sweetner;
                    priceList.Add(quotedPriceWithSweetner);
                    priceList.Add(finalPrice);
                }
                else
                {
                    quotedPriceWithSweetner = quotedPrice;
                    finalPrice = quotedPrice;
                    priceList.Add(quotedPriceWithSweetner);
                    priceList.Add(finalPrice);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetQuotedPrice", ex);
            }
            return priceList;
        }
        #endregion

        #region get weightage by score
        /// <summary>
        /// get weightage by score
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="qcRatingId"></param>
        /// <returns>calculatedWeightage</returns>
        public double GetWeighatge(int condition, int qcRatingId)
        {
            var ratingWeightage = 0;
            var score = 0.00;
            var calculatedWeightage = 0.00;
            try
            {
                if (condition > 0 && qcRatingId > 0)
                {
                    TblQcratingMaster tblQcratingMaster = _context.TblQcratingMasters.Where(x => x.IsActive == true && x.QcratingId == qcRatingId).FirstOrDefault();
                    if (tblQcratingMaster != null)
                    {
                        ratingWeightage = Convert.ToInt32(tblQcratingMaster.RatingWeightage);
                    }
                    TblQuestionerLov tblQuestionerLov = _context.TblQuestionerLovs.Where(x => x.IsActive == true && x.QuestionerLovid == condition).FirstOrDefault();
                    if (tblQuestionerLov != null)
                    {
                        score = Convert.ToDouble(tblQuestionerLov.QuestionerLovratingWeightage);
                    }
                    return calculatedWeightage = (ratingWeightage * score) / 10;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetWeighatge", ex);
            }
            return calculatedWeightage;
        }
        #endregion

        #region get final price after qc bonus cap
        /// <summary>
        /// get final price after qc bonus cap
        /// </summary>
        /// <param name="bonusCap"></param>
        /// <param name="finalPrice"></param>
        /// <returns>finalPriceAfterQCBonus</returns>
        public double FinalPriceAfterQCBonus(int bonusCap, double quotedPrice)
        {
            var finalPriceAfterQCBonus = 0.00;
            try
            {
                if (quotedPrice > 0)
                {
                    finalPriceAfterQCBonus = quotedPrice + (quotedPrice * bonusCap) / 100;
                    finalPriceAfterQCBonus = Math.Round(finalPriceAfterQCBonus / 10.0) * 10;
                }
                else
                {
                    return finalPriceAfterQCBonus;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "FinalPriceAfterQCBonus", ex);
            }
            return finalPriceAfterQCBonus;
        }
        #endregion

        #region save details by qc for upper level cap
        /// <summary>
        /// save details by qc for upper level cap
        /// </summary>
        /// <param name="questionerViewModel"></param>
        /// <param name="qCRatingViewModelList"></param>
        /// <returns>bool</returns>
        public bool saveForFinalCap(QuestionerViewModel questionerViewModel, List<QCRatingViewModel> qCRatingViewModelList, int? userId)
        {
            decimal calculatedWeightage = 0;
            TblOrderQc tblOrderQc = null;
            TblOrderTran tblOrderTran = null;
            TblExchangeOrder tblExchangeOrder = null;
            string? productQuality = null;
            try
            {
                tblOrderTran = _context.TblOrderTrans.Where(x => x.IsActive == true && x.OrderTransId == questionerViewModel.OrderTrandId).FirstOrDefault();
                if (qCRatingViewModelList.Count > 0 && questionerViewModel != null)
                {
                    #region Insert Into tblOrderQCRating
                    if (questionerViewModel.IsDiagnoseV2 == null || questionerViewModel.IsDiagnoseV2 == false)
                    {
                        calculatedWeightage = saveAnswersbyQuestionId(qCRatingViewModelList, questionerViewModel.OrderTrandId, userId);
                    }
                    else if (questionerViewModel.IsDiagnoseV2 == true)
                    {
                        calculatedWeightage = saveAnswersbyQuestionIdV2(qCRatingViewModelList, questionerViewModel.OrderTrandId, userId);
                    }

                    #endregion

                    #region Set Product Quality
                    int NWP = Convert.ToInt32(questionerViewModel.NonWorkingPrice);
                    if (NWP > 0)
                    {
                        productQuality = EnumHelper.DescriptionAttr(EvcPartnerPreferenceEnum.Not_Working);
                    }
                    else if (!string.IsNullOrEmpty(questionerViewModel.Quality))
                    {
                        productQuality = questionerViewModel.Quality;
                    }
                    #endregion

                    #region Insert Into TblOrderQc
                    tblOrderQc = _context.TblOrderQcs.Where(x => x.IsActive == true && x.OrderTransId == tblOrderTran.OrderTransId).FirstOrDefault();
                    if (tblOrderQc == null)
                    {
                        tblOrderQc = new TblOrderQc();
                        tblOrderQc.OrderTransId = questionerViewModel.OrderTrandId;
                        tblOrderQc.BonusbyQc = (decimal?)questionerViewModel.FinalPrice - (decimal?)questionerViewModel.QuotedPriceWithSweetner;
                        tblOrderQc.Qcdate = DateTime.Now;
                        tblOrderQc.StatusId = questionerViewModel.StatusId;
                        tblOrderQc.IsActive = true;
                        if (userId > 0)
                        {
                            tblOrderQc.CreatedBy = userId;
                        }
                        else
                        {
                            tblOrderQc.CreatedBy = 3;
                        }
                        tblOrderQc.QualityAfterQc = productQuality;
                        tblOrderQc.CreatedDate = DateTime.Now;
                        tblOrderQc.AverageSellingPrice = (decimal?)questionerViewModel.AverageSellingPrice;
                        tblOrderQc.ExcellentPriceByAsp = (decimal?)questionerViewModel.ExcellentPriceByASP;
                        tblOrderQc.QuotedPrice = (decimal?)questionerViewModel.QuotedPrice;
                        tblOrderQc.Sweetner = questionerViewModel.Sweetner;
                        tblOrderQc.QuotedWithSweetner = (decimal?)questionerViewModel.QuotedPriceWithSweetner;
                        tblOrderQc.PriceAfterQc = (decimal?)questionerViewModel.FinalPrice;
                        tblOrderQc.BonusPercentQc = questionerViewModel.BonusCapQC;
                        tblOrderQc.FinalCalculatedWeightage = calculatedWeightage;
                        _orderQCRepository.Create(tblOrderQc);
                        _orderQCRepository.SaveChanges();
                    }
                    else
                    {
                        tblOrderQc.OrderTransId = questionerViewModel.OrderTrandId;
                        tblOrderQc.BonusbyQc = (decimal?)questionerViewModel.FinalPrice - (decimal?)questionerViewModel.QuotedPriceWithSweetner;
                        tblOrderQc.Qcdate = DateTime.Now;
                        tblOrderQc.StatusId = questionerViewModel.StatusId;
                        tblOrderQc.IsActive = true;
                        if (userId > 0)
                        {
                            tblOrderQc.ModifiedBy = userId;
                        }
                        else
                        {
                            tblOrderQc.ModifiedBy = 3;
                        }
                        tblOrderQc.QualityAfterQc = productQuality;
                        tblOrderQc.ModifiedDate = DateTime.Now;
                        tblOrderQc.AverageSellingPrice = (decimal?)questionerViewModel.AverageSellingPrice;
                        tblOrderQc.ExcellentPriceByAsp = (decimal?)questionerViewModel.ExcellentPriceByASP;
                        tblOrderQc.QuotedPrice = (decimal?)questionerViewModel.QuotedPrice;
                        tblOrderQc.Sweetner = questionerViewModel.Sweetner;
                        tblOrderQc.QuotedWithSweetner = (decimal?)questionerViewModel.QuotedPriceWithSweetner;
                        tblOrderQc.PriceAfterQc = (decimal?)questionerViewModel.FinalPrice;
                        tblOrderQc.BonusPercentQc = questionerViewModel.BonusCapQC;
                        tblOrderQc.FinalCalculatedWeightage = calculatedWeightage;
                        _orderQCRepository.Update(tblOrderQc);
                        _orderQCRepository.SaveChanges();
                    }
                    #endregion

                    #region update statusId in tblordertrans
                    if (tblOrderTran != null)
                    {
                        tblOrderTran.StatusId = questionerViewModel.StatusId;
                        tblOrderTran.FinalPriceAfterQc = tblOrderQc.PriceAfterQc;
                        if (userId > 0)
                        {
                            tblOrderTran.ModifiedBy = userId;
                        }
                        else
                        {
                            tblOrderTran.ModifiedBy = userId;
                        }
                        tblOrderTran.ModifiedDate = DateTime.Now;
                        _orderTransRepository.Update(tblOrderTran);
                        _orderTransRepository.SaveChanges();
                    }
                    #endregion

                    #region update price and statusId in tblexchangeOrder
                    tblExchangeOrder = _context.TblExchangeOrders.Where(x => x.IsActive == true && x.RegdNo == tblOrderTran.RegdNo).FirstOrDefault();
                    if (tblExchangeOrder != null)
                    {
                        tblExchangeOrder.OrderStatus = EnumHelper.DescriptionAttr(OrderStatusEnum.UpperBonusCapPending);
                        tblExchangeOrder.StatusId = Convert.ToInt32(OrderStatusEnum.UpperBonusCapPending);
                        tblExchangeOrder.FinalExchangePrice = (decimal?)questionerViewModel.FinalPrice;
                        if (userId > 0)
                        {
                            tblExchangeOrder.ModifiedBy = userId;
                        }
                        else
                        {
                            tblExchangeOrder.ModifiedBy = 3;
                        }
                        if (questionerViewModel.ProductTechnologyId > 0)
                        {
                            tblExchangeOrder.ProductTechnologyId = questionerViewModel.ProductTechnologyId;
                        }
                        tblExchangeOrder.ModifiedDate = DateTime.Now;
                        _ExchangeOrderRepository.Update(tblExchangeOrder);
                        _ExchangeOrderRepository.SaveChanges();
                    }
                    #endregion

                    #region Insert in tblexchangeabbhistory
                    TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                    tblExchangeAbbstatusHistory.OrderType = 17;
                    tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                    tblExchangeAbbstatusHistory.RegdNo = tblExchangeOrder.RegdNo;
                    tblExchangeAbbstatusHistory.ZohoSponsorId = tblExchangeOrder.ZohoSponsorOrderId != null ? tblExchangeOrder.ZohoSponsorOrderId : string.Empty; ;
                    tblExchangeAbbstatusHistory.CustId = tblExchangeOrder.CustomerDetailsId;
                    tblExchangeAbbstatusHistory.StatusId = tblExchangeOrder.StatusId;
                    tblExchangeAbbstatusHistory.IsActive = true;
                    tblExchangeAbbstatusHistory.CreatedBy = userId;
                    tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                    tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                    _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                    _exchangeABBStatusHistoryRepository.SaveChanges();
                    #endregion

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "saveForFinalCap", ex);
            }
            return false;
        }
        #endregion


        #region save details of dynamic questioners and generate report
        /// <summary>
        /// save details of dynamic questioners and generate report
        /// </summary>
        /// <param name="questionerViewModel"></param>
        /// <param name="qCRatingViewModelList"></param>
        /// <returns></returns>
        public int saveAndSubmit(QuestionerViewModel questionerViewModel, List<QCRatingViewModel> qCRatingViewModelList, int? userId, bool flag = false)
        {
            decimal calculatedWeightage = 0;
            TblOrderQc tblOrderQc = null;
            TblOrderTran tblOrderTran = null;
            TblExchangeOrder tblExchangeOrder = null;
            int statusId = 0;
            bool isupirequired = false;
            string? productQuality = null;
            try
            {
                tblOrderTran = GetOrderDetailsByOrderTransId(questionerViewModel.OrderTrandId);
                if (qCRatingViewModelList.Count > 0 && questionerViewModel != null)
                {
                    #region Insert Into tblOrderQCRating
                    if (questionerViewModel.IsDiagnoseV2 == null || questionerViewModel.IsDiagnoseV2 == false)
                    {
                        calculatedWeightage = saveAnswersbyQuestionId(qCRatingViewModelList, questionerViewModel.OrderTrandId, userId,flag);
                    }
                    else if (questionerViewModel.IsDiagnoseV2 == true)
                    {
                        calculatedWeightage = saveAnswersbyQuestionIdV2(qCRatingViewModelList, questionerViewModel.OrderTrandId, userId, flag);
                    }
                    #endregion

                    #region Set Product Quality
                    int NWP = Convert.ToInt32(questionerViewModel.NonWorkingPrice);
                    if (NWP > 0)
                    {
                        productQuality = EnumHelper.DescriptionAttr(EvcPartnerPreferenceEnum.Not_Working);
                    }
                    else if (!string.IsNullOrEmpty(questionerViewModel.Quality))
                    {
                        productQuality = questionerViewModel.Quality;
                    }
                    #endregion

                    #region insert into tblorderqc
                    tblOrderQc = _context.TblOrderQcs.Where(x => x.IsActive == true && x.OrderTransId == tblOrderTran.OrderTransId).FirstOrDefault();
                    if (tblOrderQc == null)
                    {
                        tblOrderQc = new TblOrderQc();
                        tblOrderQc.OrderTransId = questionerViewModel.OrderTrandId;
                        tblOrderQc.BonusbyQc = (decimal?)questionerViewModel.FinalPrice - (decimal?)questionerViewModel.QuotedPriceWithSweetner;
                        tblOrderQc.Qcdate = DateTime.Now;
                        if (flag == true)
                        {
                            tblOrderQc.StatusId = Convert.ToInt32(OrderStatusEnum.OrderWithDiagnostic);
                        }
                        else
                        {
                            tblOrderQc.StatusId = Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval);
                        }
                        tblOrderQc.IsActive = true;
                        if (userId > 0)
                        {
                            tblOrderQc.CreatedBy = userId;
                        }
                        else
                        {
                            tblOrderQc.CreatedBy = 3;
                        }
                        tblOrderQc.QualityAfterQc = productQuality;
                        tblOrderQc.CreatedDate = DateTime.Now;
                        tblOrderQc.AverageSellingPrice = (decimal?)questionerViewModel.AverageSellingPrice;
                        tblOrderQc.ExcellentPriceByAsp = (decimal?)questionerViewModel.ExcellentPriceByASP;
                        tblOrderQc.QuotedPrice = (decimal?)questionerViewModel.QuotedPrice;
                        tblOrderQc.Sweetner = questionerViewModel.Sweetner;
                        tblOrderQc.QuotedWithSweetner = (decimal?)questionerViewModel.QuotedPriceWithSweetner;
                        if (questionerViewModel.NonWorkingPrice > 0)
                        {
                            tblOrderQc.PriceAfterQc = questionerViewModel.NonWorkingPrice;
                        }
                        else
                        {
                            tblOrderQc.PriceAfterQc = (decimal?)questionerViewModel.FinalPrice;
                        }
                        tblOrderQc.BonusPercentQc = questionerViewModel.BonusCapQC;
                        tblOrderQc.FinalCalculatedWeightage = calculatedWeightage;
                        _orderQCRepository.Create(tblOrderQc);
                        _orderQCRepository.SaveChanges();
                    }
                    else
                    {
                        tblOrderQc.OrderTransId = questionerViewModel.OrderTrandId;
                        tblOrderQc.BonusbyQc = (decimal?)questionerViewModel.FinalPrice - (decimal?)questionerViewModel.QuotedPriceWithSweetner;
                        tblOrderQc.Qcdate = DateTime.Now;
                        if (flag == true)
                        {
                            tblOrderQc.StatusId = Convert.ToInt32(OrderStatusEnum.OrderWithDiagnostic);
                        }
                        else
                        {
                            tblOrderQc.StatusId = Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval);
                        }
                        tblOrderQc.IsActive = true;
                        if (userId > 0)
                        {
                            tblOrderQc.ModifiedBy = userId;
                        }
                        else
                        {
                            tblOrderQc.ModifiedBy = 3;
                        }
                        tblOrderQc.QualityAfterQc = productQuality;
                        tblOrderQc.ModifiedDate = DateTime.Now;
                        tblOrderQc.AverageSellingPrice = (decimal?)questionerViewModel.AverageSellingPrice;
                        tblOrderQc.ExcellentPriceByAsp = (decimal?)questionerViewModel.ExcellentPriceByASP;
                        tblOrderQc.QuotedPrice = (decimal?)questionerViewModel.QuotedPrice;
                        tblOrderQc.Sweetner = questionerViewModel.Sweetner;
                        tblOrderQc.QuotedWithSweetner = (decimal?)questionerViewModel.QuotedPriceWithSweetner;
                        if (questionerViewModel.NonWorkingPrice > 0)
                        {
                            tblOrderQc.PriceAfterQc = questionerViewModel.NonWorkingPrice;
                        }
                        else
                        {
                            tblOrderQc.PriceAfterQc = (decimal?)questionerViewModel.FinalPrice;
                        }
                        tblOrderQc.BonusPercentQc = questionerViewModel.BonusCapQC;
                        tblOrderQc.FinalCalculatedWeightage = calculatedWeightage;
                        _orderQCRepository.Update(tblOrderQc);
                        _orderQCRepository.SaveChanges();
                    }

                    #endregion

                    #region update statusId in tblordertrans
                    if (tblOrderTran != null)
                    {
                        if (flag == true)
                        {
                            tblOrderTran.StatusId = Convert.ToInt32(OrderStatusEnum.OrderWithDiagnostic);
                        }
                        else
                        {
                            tblOrderTran.StatusId = Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval);
                            tblOrderTran.FinalPriceAfterQc = tblOrderQc.PriceAfterQc;
                        }
                        if (userId > 0)
                        {
                            tblOrderTran.ModifiedBy = userId;
                        }
                        else
                        {
                            tblOrderTran.ModifiedBy = userId;
                        }
                        tblOrderTran.ModifiedDate = DateTime.Now;
                        _orderTransRepository.Update(tblOrderTran);
                        _orderTransRepository.SaveChanges();
                    }
                    #endregion

                    #region update price and statusId in tblexchangeOrder
                    tblExchangeOrder = _context.TblExchangeOrders.Where(x => x.IsActive == true && x.RegdNo == tblOrderTran.RegdNo).FirstOrDefault();
                    if (tblExchangeOrder != null)
                    {
                        if (flag == true)
                        {
                            tblExchangeOrder.OrderStatus = EnumHelper.DescriptionAttr(OrderStatusEnum.OrderWithDiagnostic);
                            tblExchangeOrder.StatusId = Convert.ToInt32(OrderStatusEnum.OrderWithDiagnostic);
                        }
                        else
                        {
                            tblExchangeOrder.OrderStatus = EnumHelper.DescriptionAttr(OrderStatusEnum.Waitingforcustapproval);
                            tblExchangeOrder.StatusId = Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval);
                        }
                        tblExchangeOrder.FinalExchangePrice = tblOrderQc.PriceAfterQc;
                        if (questionerViewModel.ProductTechnologyId > 0)
                        {
                            tblExchangeOrder.ProductTechnologyId = questionerViewModel.ProductTechnologyId;
                        }
                        if (userId > 0)
                        {
                            tblExchangeOrder.ModifiedBy = userId;
                        }
                        else
                        {
                            tblExchangeOrder.ModifiedBy = 3;
                        }
                        tblExchangeOrder.ModifiedDate = DateTime.Now;
                        _ExchangeOrderRepository.Update(tblExchangeOrder);
                        _ExchangeOrderRepository.SaveChanges();
                    }
                    #endregion

                    #region Insert in tblexchangeabbhistory
                    TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                    tblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(LoVEnum.Exchange);
                    tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                    tblExchangeAbbstatusHistory.RegdNo = tblExchangeOrder.RegdNo;
                    tblExchangeAbbstatusHistory.ZohoSponsorId = tblExchangeOrder.ZohoSponsorOrderId != null ? tblExchangeOrder.ZohoSponsorOrderId : string.Empty; ;
                    tblExchangeAbbstatusHistory.CustId = tblExchangeOrder.CustomerDetailsId;
                    tblExchangeAbbstatusHistory.StatusId = tblExchangeOrder.StatusId;
                    tblExchangeAbbstatusHistory.IsActive = true;
                    tblExchangeAbbstatusHistory.CreatedBy = userId;
                    tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                    tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                    _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                    _exchangeABBStatusHistoryRepository.SaveChanges();
                    #endregion

                    #region Generate Report as PDF
                    if (flag == false)
                    {
                        string fileNameWithPath = string.Empty;
                        string questionersPDF = tblOrderTran.RegdNo + "_" + "DiagnosticReport" + ".pdf";
                        string questionersFilePath = EnumHelper.DescriptionAttr(FilePathEnum.DiagnosticReport);
                        string questionersHtmlString = GetQuestionerhtmlstring(questionerViewModel, qCRatingViewModelList, "DiagnosticReport");
                        bool debitNotePDFSave = _htmlToPDFConverterHelper.GeneratePDF(questionersHtmlString, questionersFilePath, questionersPDF);
                        if (debitNotePDFSave)
                        {
                            #region to save pdf name in tblorderqc
                            tblOrderQc.DagnosticPdfName = questionersPDF != null ? questionersPDF : string.Empty;
                            _orderQCRepository.Update(tblOrderQc);
                            _orderQCRepository.SaveChanges();
                            #endregion

                            #region whatsappNotification for Diagnostic report

                            if (tblOrderTran != null)
                            {
                                isupirequired = _commonManager.CheckUpiisRequired(tblOrderTran.OrderTransId);
                            }

                            questionersFilePath = questionersFilePath.Replace("\\", "/");
                            fileNameWithPath = string.Concat(questionersFilePath, "/", questionersPDF);
                            WhatasappDiagnosticResponse whatasappResponse = new WhatasappDiagnosticResponse();
                            TblWhatsAppMessage tblwhatsappmessage = null;
                            string message = string.Empty;

                            WhatsappDiagnosticTemplate whatsappObj = new WhatsappDiagnosticTemplate();
                            whatsappObj.userDetails = new UserDetailsDiagnostic();
                            whatsappObj.notification = new DiagnosticReport();
                            whatsappObj.notification.@params = new DiagnosticURL();
                            whatsappObj.userDetails.number = tblOrderTran.Exchange.CustomerDetails.PhoneNumber;
                            whatsappObj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
                            whatsappObj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
                            whatsappObj.notification.templateId = NotificationConstants.DiagnoticReport;
                            whatsappObj.notification.@params.Customername = tblOrderTran.Exchange.CustomerDetails.FirstName + " " + tblOrderTran.Exchange.CustomerDetails.LastName;
                            whatsappObj.notification.@params.ProductName = tblOrderTran.Exchange.ProductType.ProductCat.Description;
                            whatsappObj.notification.@params.Price = tblOrderTran.FinalPriceAfterQc;
                            string link = _baseConfig.Value.BaseURL + fileNameWithPath;
                            whatsappObj.notification.@params.Link = link;
                            string url = _baseConfig.Value.YellowAiUrl;

                            RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                            int statusCode = Convert.ToInt32(response.StatusCode);
                            if (response.Content != null && statusCode == 202)
                            {
                                tblwhatsappmessage = new TblWhatsAppMessage();
                                whatasappResponse = JsonConvert.DeserializeObject<WhatasappDiagnosticResponse>(response.Content);
                                tblwhatsappmessage.TemplateName = NotificationConstants.DiagnoticReport;
                                tblwhatsappmessage.IsActive = true;
                                tblwhatsappmessage.PhoneNumber = tblOrderTran.Exchange.CustomerDetails.PhoneNumber;
                                tblwhatsappmessage.SendDate = DateTime.Now;
                                tblwhatsappmessage.MsgId = whatasappResponse.msgId;
                                _WhatsAppMessageRepository.Create(tblwhatsappmessage);
                                _WhatsAppMessageRepository.SaveChanges();
                            }
                            #endregion
                        }
                    }
                    #endregion

                    statusId = (int)tblExchangeOrder.StatusId;

                    return statusId;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "saveAndSubmit", ex);
            }
            return statusId;
        }

        #endregion

        #region method to save questioner's with answer
        /// <summary>
        /// method to save questioner's with answer
        /// </summary>
        /// <param name="qCRatingViewModelList"></param>
        /// <param name="orderTransId"></param>
        /// <returns>calculatedWeightage</returns>
        public decimal saveAnswersbyQuestionId(List<QCRatingViewModel> qCRatingViewModelList, int orderTransId, int? userId, bool flag = false)
        {
            TblOrderQcrating tblOrderQcrating = null;
            TblQuestionerLov tblQuestionerLov = null;
            decimal calculatedWeightage = 0.00M;
            try
            {
                if (qCRatingViewModelList.Count > 0 && orderTransId > 0)
                {
                    foreach (var item in qCRatingViewModelList)
                    {
                        tblOrderQcrating = new TblOrderQcrating();
                        tblOrderQcrating.OrderTransId = orderTransId;
                        tblOrderQcrating.ProductCatId = item.ProductCatId;
                        tblOrderQcrating.QcquestionId = item.QcratingId;
                        tblOrderQcrating.Rating = item.RatingWeightage;
                        tblQuestionerLov = _context.TblQuestionerLovs.Where(x => x.IsActive == true && x.QuestionerLovid == item.Condition).FirstOrDefault();
                        if (tblQuestionerLov != null)
                        {
                            tblOrderQcrating.CalculatedWeightage = tblQuestionerLov.QuestionerLovratingWeightage != null ? tblQuestionerLov.QuestionerLovratingWeightage : 0;
                        }
                        else
                        {
                            tblOrderQcrating.CalculatedWeightage = 0;
                        }
                        tblOrderQcrating.IsActive = true;
                        if (userId > 0 || userId != null)
                        {
                            tblOrderQcrating.CreatedBy = userId;
                        }
                        else
                        {
                            tblOrderQcrating.CreatedBy = 3;
                        }
                        tblOrderQcrating.CreatedDate = DateTime.Now;
                        tblOrderQcrating.QcComments = item.CommentByQC != null ? item.CommentByQC : string.Empty;
                        tblOrderQcrating.QuestionerLovid = item.Condition;
                        if (flag == true)
                        {
                            tblOrderQcrating.DoneBy = Convert.ToInt32(LoVEnum.Customer);
                        }
                        else
                        {
                            tblOrderQcrating.DoneBy = Convert.ToInt32(LoVEnum.QCTeam);
                        }
                        _orderQCRatingRepository.Create(tblOrderQcrating);
                        calculatedWeightage += Convert.ToDecimal(tblOrderQcrating.CalculatedWeightage);
                    }
                    _orderQCRatingRepository.SaveChanges();
                    if (calculatedWeightage > 0)
                    {
                        return calculatedWeightage;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "saveAnswersbyQuestionId", ex);
            }
            return calculatedWeightage;
        }
        #endregion

        #region create pdf string for questioner report
        /// <summary>
        /// create pdf string for questioner report
        /// </summary>
        /// <param name="orderLGCViewModel"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns></returns>
        public string GetQuestionerhtmlstring(QuestionerViewModel questionerViewModel, List<QCRatingViewModel> qCRatingViewModelList, string HtmlTemplateNameOnly)
        {
            var DateTime = System.DateTime.Now;
            string FinalDate = DateTime.Date.ToShortDateString();
            string bunch = string.Empty;
            string htmlString = "";
            string fileName = HtmlTemplateNameOnly + ".html";
            string fileNameWithPath = "";
            try
            {
                TblOrderTran tblOrderTran = _context.TblOrderTrans.Include(x => x.Exchange).ThenInclude(x => x.CustomerDetails).Where(x => x.IsActive == true && x.OrderTransId == questionerViewModel.OrderTrandId).FirstOrDefault();
                TblProductTechnology tblProductTechnology = _context.TblProductTechnologies.Where(x => x.IsActive == true && x.ProductTechnologyId == questionerViewModel.ProductTechnologyId).FirstOrDefault();
                var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"\PdfTemplates");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                fileNameWithPath = string.Concat(filePath, "\\", fileName);
                htmlString = File.ReadAllText(fileNameWithPath);

                if (HtmlTemplateNameOnly == "DiagnosticReport")
                    htmlString = htmlString.Replace("[Product_Category]", questionerViewModel.TblProductType.ProductCat.Description)
                        .Replace("[Product_Type]", questionerViewModel.TblProductType.Description)
                        .Replace("[Brand_Name]", questionerViewModel.TblBrand.Name)
                        .Replace("[Product_Technology]", tblProductTechnology.ProductTechnologyName)
                        .Replace("[Product_Size]", questionerViewModel.TblProductType.Size)
                        .Replace("[Date]", FinalDate)
                        .Replace("[Customer_Name]", tblOrderTran.Exchange.CustomerDetails.FirstName + " " + tblOrderTran.Exchange.CustomerDetails.LastName != null ? tblOrderTran.Exchange.CustomerDetails.FirstName + " " + tblOrderTran.Exchange.CustomerDetails.LastName : string.Empty)
                        .Replace("[RegdNo]", tblOrderTran.Exchange.RegdNo != null ? tblOrderTran.Exchange.RegdNo : string.Empty);

                if (questionerViewModel.NonWorkingPrice > 0)
                {
                    htmlString = htmlString.Replace("[Final_Price]", questionerViewModel.NonWorkingPrice.ToString());
                }
                else
                {
                    htmlString = htmlString.Replace("[Final_Price]", questionerViewModel.FinalPrice.ToString());
                }
                int srNum = 1;
                string questionerLovName = string.Empty;
                foreach (var item in qCRatingViewModelList)
                {
                    TblQuestionerLov tblQuestionerLov = _context.TblQuestionerLovs.Where(x => x.IsActive == true && x.QuestionerLovid == item.Condition).FirstOrDefault();
                    if (tblQuestionerLov != null)
                    {
                        questionerLovName = tblQuestionerLov.QuestionerLovname;
                    }
                    else
                    {
                        questionerLovName = "NA";
                    }
                    //Create dynamic rows
                    bunch += "<tr><td> " + srNum + " </td>" +
                  "<td>" + item.Qcquestion + " </td>" +
                  "<td>" + questionerLovName + "</td>" +
                  "<td>" + item.CommentByQC + "</td></tr>";
                    srNum++;
                }
                htmlString = htmlString.Replace("BunchRow", bunch);

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetQuestionerhtmlstring", ex);
            }
            return htmlString;
        }
        #endregion

        #region get order details for Upper level bonus cap by QC Admin
        /// <summary>
        /// get order details for Upper level bonus cap by QC Admin
        /// </summary>
        /// <param name="orderTransId"></param>
        /// <returns>adminBonusCapViewModel</returns>
        public AdminBonusCapViewModel GetOrderDetailsPendingForUpperCap(int orderTransId)
        {
            AdminBonusCapViewModel adminBonusCapViewModel = null;
            TblOrderQc tblOrderQc = null;
            try
            {
                if (orderTransId > 0)
                {
                    tblOrderQc = _context.TblOrderQcs
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.Brand)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductTechnology)
                        .Where(x => x.IsActive == true && x.OrderTransId == orderTransId).FirstOrDefault();
                    if (tblOrderQc != null)
                    {
                        adminBonusCapViewModel = new AdminBonusCapViewModel();
                        adminBonusCapViewModel.CustomerName = tblOrderQc.OrderTrans.Exchange.CustomerDetails.FirstName + " " + tblOrderQc.OrderTrans.Exchange.CustomerDetails.LastName;
                        adminBonusCapViewModel.RegdNo = tblOrderQc.OrderTrans.RegdNo != null ? tblOrderQc.OrderTrans.RegdNo : string.Empty;
                        adminBonusCapViewModel.ProductCategoryName = tblOrderQc.OrderTrans.Exchange.ProductType.ProductCat.Description != null ? tblOrderQc.OrderTrans.Exchange.ProductType.ProductCat.Description : string.Empty;
                        adminBonusCapViewModel.ProductTypeName = tblOrderQc.OrderTrans.Exchange.ProductType.Description != null ? tblOrderQc.OrderTrans.Exchange.ProductType.Description : string.Empty;
                        adminBonusCapViewModel.BrandName = tblOrderQc.OrderTrans.Exchange.Brand.Name != null ? tblOrderQc.OrderTrans.Exchange.Brand.Name : string.Empty;
                        adminBonusCapViewModel.Size = tblOrderQc.OrderTrans.Exchange.ProductType.Size != null ? tblOrderQc.OrderTrans.Exchange.ProductType.Size : string.Empty;
                        adminBonusCapViewModel.Technology = string.Empty;
                        adminBonusCapViewModel.AverageSellingPrice = (double)(tblOrderQc.AverageSellingPrice != null ? tblOrderQc.AverageSellingPrice : 0);
                        adminBonusCapViewModel.ExcellentPriceByASP = (double)tblOrderQc.ExcellentPriceByAsp;
                        adminBonusCapViewModel.QuotedPrice = (double)tblOrderQc.QuotedPrice;
                        adminBonusCapViewModel.Sweetner = (decimal)tblOrderQc.Sweetner;
                        adminBonusCapViewModel.QuotedPriceWithSweetner = (double)tblOrderQc.QuotedWithSweetner;
                        adminBonusCapViewModel.BonusCapQC = (int)tblOrderQc.BonusbyQc;
                        adminBonusCapViewModel.FinalPriceAfterQC = (double)tblOrderQc.PriceAfterQc;
                        adminBonusCapViewModel.BonusPercentageByQC = (double)tblOrderQc.BonusPercentQc;
                        adminBonusCapViewModel.FinalCalculatedWeightage = (double)tblOrderQc.FinalCalculatedWeightage;
                        adminBonusCapViewModel.OrderTransId = orderTransId;
                        adminBonusCapViewModel.ProductTechnolgyName = tblOrderQc.OrderTrans.Exchange.ProductTechnology.ProductTechnologyName != null ? tblOrderQc.OrderTrans.Exchange.ProductTechnology.ProductTechnologyName : string.Empty;
                        return adminBonusCapViewModel;
                    }
                    else
                    {
                        adminBonusCapViewModel = new AdminBonusCapViewModel();
                    }
                }
                else
                {
                    adminBonusCapViewModel = new AdminBonusCapViewModel();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetOrderDetailsPendingForUpperCap", ex);
            }

            return adminBonusCapViewModel;
        }
        #endregion

        #region get questioner report for Upper level bonus cap by QC Admin
        /// <summary>
        /// get order details for Upper level bonus cap by QC Admin
        /// </summary>
        /// <param name="orderTransId"></param>
        /// <returns>adminBonusCapViewModel</returns>
        public List<QCRatingViewModel> GetQuestionerReport(int orderTransId)
        {
            List<QCRatingViewModel> qCRatingList = new List<QCRatingViewModel>();
            QCRatingViewModel qCRatingViewModel = null;
            List<TblOrderQcrating> tblOrderQcratingList = null;
            try
            {
                if (orderTransId > 0)
                {
                    tblOrderQcratingList = _context.TblOrderQcratings
                        .Include(x => x.OrderTrans).Include(x => x.Qcquestion).Include(x => x.QuestionerLov)
                        .Where(x => x.IsActive == true && x.OrderTransId == orderTransId && x.DoneBy == Convert.ToInt32(LoVEnum.Customer)).ToList();
                    if (tblOrderQcratingList != null)
                    {
                        foreach (var item in tblOrderQcratingList)
                        {
                            qCRatingViewModel = new QCRatingViewModel();
                            qCRatingViewModel.Qcquestion = item.Qcquestion.Qcquestion;
                            if (item.QuestionerLovid != null)
                            {
                                qCRatingViewModel.Condition = (int)item.QuestionerLovid;
                                qCRatingViewModel.QuestionerLOVName = item.QuestionerLov.QuestionerLovname;
                            }
                            qCRatingViewModel.CommentByQC = item.QcComments;
                            qCRatingList.Add(qCRatingViewModel);
                        }

                        if (qCRatingList != null)
                        {
                            return qCRatingList;
                        }
                        else
                        {
                            return qCRatingList;
                        }
                    }
                    else
                    {
                        return qCRatingList;
                    }
                }
                else
                {
                    return qCRatingList;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetOrderDetailsPendingForUpperCap", ex);
            }

            return qCRatingList;
        }
        #endregion

        #region save bous details by admin for status 5P orders
        /// <summary>
        /// save bous details by admin for status 5P orders
        /// </summary>
        /// <param name="FinalPriceAfterAdminCap"></param>
        /// <param name="BonusCapAdmin"></param>
        /// <returns>bool</returns>
        public bool SaveBonusDetailByAdimn(AdminBonusCapViewModel adminBonusCapViewModel, List<QCRatingViewModel> qCRatingViewModelList, int UserId)
        {
            TblOrderQc tblOrderQc = null;
            TblOrderTran tblOrderTran = null;
            TblExchangeOrder tblExchangeOrder = null;
            try
            {
                if (adminBonusCapViewModel != null && qCRatingViewModelList.Count > 0)
                {
                    tblOrderQc = _context.TblOrderQcs.Where(x => x.IsActive == true && x.OrderTransId == adminBonusCapViewModel.OrderTransId).FirstOrDefault();
                    tblOrderTran = GetOrderDetailsByOrderTransId(adminBonusCapViewModel.OrderTransId);
                    if (tblOrderQc != null)
                    {
                        #region Update TblOrderQc
                        tblOrderQc.PriceAfterQc = (decimal?)adminBonusCapViewModel.FinalPriceAfterAdminCap;
                        tblOrderQc.AdditionalBonus = (decimal?)(adminBonusCapViewModel.FinalPriceAfterAdminCap - adminBonusCapViewModel.FinalPriceAfterQC);
                        tblOrderQc.StatusId = Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval);
                        tblOrderQc.ModifiedBy = UserId;
                        tblOrderQc.ModifiedDate = DateTime.Now;
                        tblOrderQc.BonusPercentAdmin = adminBonusCapViewModel.BonusCapAdmin;
                        _orderQCRepository.Update(tblOrderQc);
                        _orderQCRepository.SaveChanges();
                        #endregion

                        #region update statusId in tblordertrans
                        if (tblOrderTran != null)
                        {
                            tblOrderTran.StatusId = tblOrderQc.StatusId;
                            tblOrderTran.FinalPriceAfterQc = tblOrderQc.PriceAfterQc;
                            tblOrderTran.ModifiedBy = UserId;
                            tblOrderTran.ModifiedDate = DateTime.Now;
                            _orderTransRepository.Update(tblOrderTran);
                            _orderTransRepository.SaveChanges();
                        }
                        #endregion

                        #region update price and statusId in tblexchangeOrder
                        tblExchangeOrder = _context.TblExchangeOrders.Where(x => x.IsActive == true && x.RegdNo == tblOrderTran.RegdNo).FirstOrDefault();
                        if (tblExchangeOrder != null)
                        {
                            tblExchangeOrder.OrderStatus = EnumHelper.DescriptionAttr(OrderStatusEnum.Waitingforcustapproval);
                            tblExchangeOrder.StatusId = Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval);
                            tblExchangeOrder.FinalExchangePrice = (decimal?)adminBonusCapViewModel.FinalPriceAfterAdminCap;
                            tblExchangeOrder.ModifiedBy = UserId;
                            tblExchangeOrder.ModifiedDate = DateTime.Now;
                            _ExchangeOrderRepository.Update(tblExchangeOrder);
                            _ExchangeOrderRepository.SaveChanges();
                        }
                        #endregion

                        #region Insert in tblexchangeabbhistory
                        TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                        tblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(LoVEnum.Exchange);
                        tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                        tblExchangeAbbstatusHistory.RegdNo = tblExchangeOrder.RegdNo;
                        tblExchangeAbbstatusHistory.ZohoSponsorId = tblExchangeOrder.ZohoSponsorOrderId != null ? tblExchangeOrder.ZohoSponsorOrderId : string.Empty; ;
                        tblExchangeAbbstatusHistory.CustId = tblExchangeOrder.CustomerDetailsId;
                        tblExchangeAbbstatusHistory.StatusId = tblExchangeOrder.StatusId;
                        tblExchangeAbbstatusHistory.IsActive = true;
                        tblExchangeAbbstatusHistory.CreatedBy = UserId;
                        tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                        tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                        _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                        _exchangeABBStatusHistoryRepository.SaveChanges();
                        #endregion

                        #region Generate Report as PDF
                        string questionersPDF = tblOrderTran.RegdNo + "_" + "DiagnosticReport" + ".pdf";
                        string questionersFilePath = EnumHelper.DescriptionAttr(FilePathEnum.DiagnosticReport);
                        string questionersHtmlString = GetAdminQuestionerhtmlstring(adminBonusCapViewModel, qCRatingViewModelList, "DiagnosticReport");
                        bool debitNotePDFSave = _htmlToPDFConverterHelper.GeneratePDF(questionersHtmlString, questionersFilePath, questionersPDF);

                        if (debitNotePDFSave)
                        {
                            #region to save pdf name in tblorderqc
                            tblOrderQc.DagnosticPdfName = questionersPDF != null ? questionersPDF : string.Empty;
                            _orderQCRepository.Update(tblOrderQc);
                            _orderQCRepository.SaveChanges();
                            #endregion

                            #region whatsappNotification for Diagnostic report
                            questionersFilePath = questionersFilePath.Replace("\\", "/");
                            string fileNameWithPath = string.Concat(questionersFilePath, "/", questionersPDF);
                            WhatasappDiagnosticResponse whatasappResponse = new WhatasappDiagnosticResponse();
                            TblWhatsAppMessage tblwhatsappmessage = null;
                            string message = string.Empty;

                            WhatsappDiagnosticTemplate whatsappObj = new WhatsappDiagnosticTemplate();
                            whatsappObj.userDetails = new UserDetailsDiagnostic();
                            whatsappObj.notification = new DiagnosticReport();
                            whatsappObj.notification.@params = new DiagnosticURL();
                            whatsappObj.userDetails.number = tblOrderTran.Exchange.CustomerDetails.PhoneNumber;
                            whatsappObj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
                            whatsappObj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
                            whatsappObj.notification.templateId = NotificationConstants.DiagnoticReport;
                            whatsappObj.notification.@params.Customername = tblOrderTran.Exchange.CustomerDetails.FirstName + " " + tblOrderTran.Exchange.CustomerDetails.LastName;
                            whatsappObj.notification.@params.ProductName = tblOrderTran.Exchange.ProductType.ProductCat.Description;
                            whatsappObj.notification.@params.Price = tblOrderTran.FinalPriceAfterQc;
                            string link = _baseConfig.Value.BaseURL + fileNameWithPath;
                            whatsappObj.notification.@params.Link = link;
                            string url = _baseConfig.Value.YellowAiUrl;

                            RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                            int statusCode = Convert.ToInt32(response.StatusCode);
                            if (response.Content != null && statusCode == 202)
                            {
                                tblwhatsappmessage = new TblWhatsAppMessage();
                                whatasappResponse = JsonConvert.DeserializeObject<WhatasappDiagnosticResponse>(response.Content);
                                tblwhatsappmessage.TemplateName = NotificationConstants.DiagnoticReport;
                                tblwhatsappmessage.IsActive = true;
                                tblwhatsappmessage.PhoneNumber = tblOrderTran.Exchange.CustomerDetails.PhoneNumber;
                                tblwhatsappmessage.SendDate = DateTime.Now;
                                tblwhatsappmessage.MsgId = whatasappResponse.msgId;
                                _WhatsAppMessageRepository.Create(tblwhatsappmessage);
                                _WhatsAppMessageRepository.SaveChanges();
                            }
                            #endregion

                            return true;
                        }
                        #endregion
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "SaveBonusDetailByAdimn", ex);
            }
            return false;
        }
        #endregion

        #region create pdf string for questioner report after qc admin cap
        /// <summary>
        /// create pdf string for questioner report after qc admin cap
        /// </summary>
        /// <param name="adminBonusCapViewModel"></param>
        /// <param name="qCRatingViewModelList"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns>htmlString</returns>
        public string GetAdminQuestionerhtmlstring(AdminBonusCapViewModel adminBonusCapViewModel, List<QCRatingViewModel> qCRatingViewModelList, string HtmlTemplateNameOnly)
        {
            var DateTime = System.DateTime.Now;
            string FinalDate = DateTime.Date.ToShortDateString();
            string bunch = string.Empty;
            string htmlString = "";
            string fileName = HtmlTemplateNameOnly + ".html";
            string fileNameWithPath = "";
            try
            {
                TblOrderTran tblOrderTran = _context.TblOrderTrans.Include(x => x.Exchange).ThenInclude(x => x.CustomerDetails).Where(x => x.IsActive == true && x.OrderTransId == adminBonusCapViewModel.OrderTransId).FirstOrDefault();
                var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"\PdfTemplates");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                fileNameWithPath = string.Concat(filePath, "\\", fileName);
                htmlString = File.ReadAllText(fileNameWithPath);

                if (HtmlTemplateNameOnly == "DiagnosticReport")
                {
                    htmlString = htmlString.Replace("[Product_Category]", adminBonusCapViewModel.ProductCategoryName)
                       .Replace("[Product_Type]", adminBonusCapViewModel.ProductTypeName)
                       .Replace("[Brand_Name]", adminBonusCapViewModel.BrandName)
                       .Replace("[Product_Technology]", adminBonusCapViewModel.ProductTechnolgyName)
                       .Replace("[Product_Size]", adminBonusCapViewModel.Size)
                       .Replace("[Date]", FinalDate)
                       .Replace("[Customer_Name]", tblOrderTran.Exchange.CustomerDetails.FirstName + " " + tblOrderTran.Exchange.CustomerDetails.LastName != null ? tblOrderTran.Exchange.CustomerDetails.FirstName + " " + tblOrderTran.Exchange.CustomerDetails.LastName : string.Empty)
                       .Replace("[RegdNo]", tblOrderTran.Exchange.RegdNo != null ? tblOrderTran.Exchange.RegdNo : string.Empty)
                       .Replace("[Final_Price]", adminBonusCapViewModel.FinalPriceAfterAdminCap.ToString());
                }

                int srNum = 1;
                string questionerLovName = string.Empty;
                foreach (var item in qCRatingViewModelList)
                {
                    TblQuestionerLov tblQuestionerLov = _context.TblQuestionerLovs.Where(x => x.IsActive == true && x.QuestionerLovid == item.Condition).FirstOrDefault();
                    if (tblQuestionerLov != null)
                    {
                        questionerLovName = tblQuestionerLov.QuestionerLovname;
                    }
                    else
                    {
                        questionerLovName = "NA";
                    }
                    //Create dynamic rows
                    bunch += "<tr><td> " + srNum + " </td>" +
                  "<td>" + item.Qcquestion + " </td>" +
                  "<td>" + questionerLovName + "</td>" +
                  "<td>" + item.CommentByQC + "</td></tr>";
                    srNum++;
                }
                htmlString = htmlString.Replace("BunchRow", bunch);

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetAdminQuestionerhtmlstring", ex);
            }
            return htmlString;
        }
        #endregion

        #region get order details by exchange Id
        /// <summary>
        /// get order details by exchange Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ExchangeOrderViewModel GetOrderDetailsById(int id)
        {
            ExchangeOrderViewModel exchangeOrderViewModel = new ExchangeOrderViewModel();
            TblExchangeOrder tblExchangeOrder = null;
            TblOrderTran tblOrderTran = null;
            string customerName = string.Empty;
            string customerAddress = string.Empty;
            try
            {
                if (id > 0)
                {
                    tblExchangeOrder = _context.TblExchangeOrders.Include(x => x.CustomerDetails).Include(x => x.Brand)
                        .Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                        .Where(x => x.IsActive == true && x.Id == id)
                        .FirstOrDefault();
                    tblOrderTran = _context.TblOrderTrans.Where(x => x.IsActive == true && x.RegdNo == tblExchangeOrder.RegdNo).FirstOrDefault();
                    if (tblExchangeOrder != null && tblOrderTran != null)
                    {
                        exchangeOrderViewModel.CompanyName = tblExchangeOrder.CompanyName != null ? tblExchangeOrder.CompanyName : string.Empty;
                        exchangeOrderViewModel.RegdNo = tblExchangeOrder.RegdNo != null ? tblExchangeOrder.RegdNo : string.Empty;
                        customerName = tblExchangeOrder.CustomerDetails.FirstName + " " + tblExchangeOrder.CustomerDetails.LastName;
                        exchangeOrderViewModel.CustFullname = customerName;
                        exchangeOrderViewModel.CustEmail = tblExchangeOrder.CustomerDetails.Email != null ? tblExchangeOrder.CustomerDetails.Email : string.Empty;
                        exchangeOrderViewModel.CustPhoneNumber = tblExchangeOrder.CustomerDetails.PhoneNumber != null ? tblExchangeOrder.CustomerDetails.PhoneNumber : string.Empty;
                        customerAddress = tblExchangeOrder.CustomerDetails.Address1 + ", " + tblExchangeOrder.CustomerDetails.Address2;
                        exchangeOrderViewModel.CustAddress = customerAddress;
                        exchangeOrderViewModel.CustCity = tblExchangeOrder.CustomerDetails.City != null ? tblExchangeOrder.CustomerDetails.City : string.Empty;
                        exchangeOrderViewModel.CustState = tblExchangeOrder.CustomerDetails.State != null ? tblExchangeOrder.CustomerDetails.State : string.Empty;
                        exchangeOrderViewModel.CustPinCode = tblExchangeOrder.CustomerDetails.ZipCode != null ? tblExchangeOrder.CustomerDetails.ZipCode : string.Empty;
                        exchangeOrderViewModel.ProductCategoryName = tblExchangeOrder.ProductType.ProductCat.Description != null ? tblExchangeOrder.ProductType.ProductCat.Description : string.Empty;
                        exchangeOrderViewModel.ProductTypeName = tblExchangeOrder.ProductType.Description + tblExchangeOrder.ProductType.Size;
                        exchangeOrderViewModel.BrandName = tblExchangeOrder.Brand.Name != null ? tblExchangeOrder.Brand.Name : string.Empty;
                        exchangeOrderViewModel.OrderTransId = tblOrderTran.OrderTransId;
                        exchangeOrderViewModel.IsDiagnosev2 = tblExchangeOrder.IsDiagnoseV2;
                    }
                    else
                    {
                        return exchangeOrderViewModel;
                    }
                }
                else
                {
                    return exchangeOrderViewModel;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetOrderDetailsById", ex);
            }
            return exchangeOrderViewModel;
        }
        #endregion

        #region save final image by qc team for questioner's
        /// <summary>
        /// save final image by qc team for questioner's
        /// </summary>
        /// <param name="imageLabelViewModels"></param>
        /// <returns>flag</returns>
        public string FinalQCImagesUploadedByQC(IList<ImageLabelViewModel> imageLabelViewModels, ExchangeOrderViewModel exchangeOrderViewModel, int userId)
        {
            string regdNo = string.Empty;
            TblLoV tblLoV = null;
            TblOrderTran tblOrderTran = null;
            TblOrderImageUpload tblOrderImageUpload = null;
            string lovName = "QC Team";
            int CountOrderImgUpload = 0;
            try
            {
                #region Upload Video Qc images In Folder and DB
                tblLoV = _lovRepository.GetSingle(x => x.IsActive == true && x.LoVname.ToLower().Trim() == lovName.ToLower().Trim());
                tblOrderTran = _context.TblOrderTrans.Where(x => x.IsActive == true && x.RegdNo == exchangeOrderViewModel.RegdNo).FirstOrDefault();
                if (imageLabelViewModels.Count > 0 && tblOrderTran != null)
                {
                    foreach (var items in imageLabelViewModels)
                    {
                        if (items.Base64StringValue != null)
                        {
                            items.FileName = tblOrderTran.RegdNo + "_" + "FinalQCImage" + CountOrderImgUpload + ".jpg";
                            string filePath = EnumHelper.DescriptionAttr(FilePathEnum.VideoQC);
                            bool imgSave = _imageHelper.SaveFileFromBase64(items.Base64StringValue, filePath, items.FileName);

                            tblOrderImageUpload = _orderImageUploadRepository.GetSingle(x => x.IsActive == true && x.ImageName == items.FileName);
                            if (tblOrderImageUpload != null)
                            {
                                tblOrderImageUpload.Modifiedby = userId;
                                tblOrderImageUpload.ModifiedDate = _currentDatetime;
                                _orderImageUploadRepository.Update(tblOrderImageUpload);
                            }
                            else
                            {
                                tblOrderImageUpload = new TblOrderImageUpload();
                                tblOrderImageUpload.ImageName = items.FileName;
                                tblOrderImageUpload.OrderTransId = tblOrderTran.OrderTransId;
                                if (tblLoV != null)
                                {
                                    tblOrderImageUpload.ImageUploadby = tblLoV.LoVid;
                                }
                                tblOrderImageUpload.IsActive = true;
                                tblOrderImageUpload.CreatedBy = userId;
                                tblOrderImageUpload.CreatedDate = _currentDatetime;
                                _orderImageUploadRepository.Create(tblOrderImageUpload);
                            }
                            _orderImageUploadRepository.SaveChanges();
                        }
                        CountOrderImgUpload += 1;
                    }
                }

                if (CountOrderImgUpload == imageLabelViewModels.Count)
                {
                    return regdNo = exchangeOrderViewModel.RegdNo;
                }
                else
                {
                    return regdNo;
                }
                #endregion

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "FinalQCImagesUploadedByQC", ex);
            }
            return regdNo;
        }
        #endregion

        #region GetQuestionswithLov for rating by product category - addedby ashwin
        /// <summary>
        /// get all question for rating by product category id
        /// GetQuestionswithLov (child/parent)
        /// </summary>
        /// <param name="prodCatId"></param>
        /// <returns></returns>
        public ResponseResult GetQuestionswithLov(int prodCatId)
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.message = string.Empty;
            List<QCRatingLOVDataViewModel> qCRatingLOVDataViewModel = new List<QCRatingLOVDataViewModel>();

            List<TblQcratingMaster> tblQcratingMasterList = null;

            List<TblQuestionerLov> tblQuestionerLOV = new List<TblQuestionerLov>();
            try
            {
                if (prodCatId > 0)
                {
                    tblQcratingMasterList = _context.TblQcratingMasters.Where(x => x.IsActive == true && x.ProductCatId == prodCatId && x.IsDiagnoseV2==null).ToList();
                    if (tblQcratingMasterList.Count > 0 || tblQcratingMasterList != null)
                    {
                        qCRatingLOVDataViewModel = _mapper.Map<List<TblQcratingMaster>, List<QCRatingLOVDataViewModel>>(tblQcratingMasterList);

                        if (qCRatingLOVDataViewModel.Count > 0)
                        {
                            foreach (var ques in qCRatingLOVDataViewModel)
                            {
                                tblQuestionerLOV = _context.TblQuestionerLovs.Where(x => x.IsActive == true && x.QuestionerLovparentId == ques.QuestionerLovid).ToList();
                                List<QuestionerLovidViewModel> questionerLovidViewModels = new List<QuestionerLovidViewModel>();
                                if (tblQuestionerLOV != null && tblQuestionerLOV.Count > 0)
                                {
                                    questionerLovidViewModels = _mapper.Map<List<TblQuestionerLov>, List<QuestionerLovidViewModel>>(tblQuestionerLOV).ToList();
                                    ques.questionerLovidViewModels = questionerLovidViewModels;
                                }
                                #region Add images path for Questions
                                string imagepath = string.Empty;

                                if (!string.IsNullOrEmpty(ques.QuestionsImage))
                                {
                                    imagepath = _baseConfig.Value.BaseURL + "DBFiles\\Masters\\QuestionsImages\\" + ques.QuestionsImage;

                                    if (!string.IsNullOrEmpty(imagepath))
                                    {
                                        ques.QuestionsImage = "";
                                        ques.QuestionsImage = imagepath;
                                    }
                                    imagepath = string.Empty;
                                }
                                #endregion
                            }
                            if (qCRatingLOVDataViewModel.Count > 0)
                            {
                                responseResult.Data = qCRatingLOVDataViewModel;
                                responseResult.message = "Success";
                                responseResult.Status = true;
                                responseResult.Status_Code = HttpStatusCode.OK;
                                return responseResult;
                            }
                            else
                            {
                                responseResult.message = "No Success";
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.BadRequest;
                            }
                        }
                        else
                        {
                            responseResult.message = "Error occurs while mapping the data";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseResult.message = "Questiones not found";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.message = "No catid found";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("QCCommentManager", "GetQuestionswithLov", ex);
            }
            return responseResult;
        }
        #endregion

        #region Save Upi No & Pickup date
        public int SaveUPIIdByExchangemanage(UPINoViewModel UpinoViewModel)
        {
            TblExchangeOrder tblExchangeOrder = null;
            TblOrderTran tblOrderTran = null;
            TblOrderQc tblOrderQc = null;
            try
            {
                if (UpinoViewModel != null)
                {
                    if (UpinoViewModel.PreferredPickupTime != null)
                    {
                        if (UpinoViewModel.PreferredPickupTime.Equals("1"))
                        {
                            UpinoViewModel.PickupStartTime = "10:00AM";
                            UpinoViewModel.PickupEndTime = "12:00PM";
                        }
                        else if (UpinoViewModel.PreferredPickupTime.Equals("2"))
                        {
                            UpinoViewModel.PickupStartTime = "12:00PM";
                            UpinoViewModel.PickupEndTime = "2:00PM";
                        }
                        else if (UpinoViewModel.PreferredPickupTime.Equals("3"))
                        {
                            UpinoViewModel.PickupStartTime = "2:00PM";
                            UpinoViewModel.PickupEndTime = "4:00PM";
                        }
                        else if (UpinoViewModel.PreferredPickupTime.Equals("4"))
                        {
                            UpinoViewModel.PickupStartTime = "4:00PM";
                            UpinoViewModel.PickupEndTime = "6:00PM";
                        }
                        else if (UpinoViewModel.PreferredPickupTime.Equals("5"))
                        {
                            UpinoViewModel.PickupStartTime = "6:00PM";
                            UpinoViewModel.PickupEndTime = "8:00PM";
                        }

                    }
                    tblExchangeOrder = _ExchangeOrderRepository.GetRegdNo(UpinoViewModel.Regdno);
                    if (tblExchangeOrder != null)
                    {
                        tblOrderTran = _orderTransRepository.GetRegdno(tblExchangeOrder.RegdNo);
                        if (tblOrderTran != null)
                        {
                            tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran.OrderTransId);
                            if (tblOrderQc != null)
                            {
                                #region Code to update the TblorderQC 
                                tblOrderQc.Upiid = UpinoViewModel.UPIId;
                                tblOrderQc.PreferredPickupDate = Convert.ToDateTime(UpinoViewModel.PreferredPickupDate);
                                tblOrderQc.PickupStartTime = UpinoViewModel.PickupStartTime;
                                tblOrderQc.PickupEndTime = UpinoViewModel.PickupEndTime;
                                tblOrderQc.OrderTransId = tblOrderTran.OrderTransId;
                                tblOrderQc.ModifiedBy = UpinoViewModel.Userid;
                                tblOrderQc.ModifiedDate = _currentDatetime;
                                tblOrderQc.ModifiedBy = UpinoViewModel.Userid;
                                _orderQCRepository.Update(tblOrderQc);
                                _orderQCRepository.SaveChanges();
                                #endregion
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "SaveUPIIdByExchangemanage", ex);
            }
            return tblOrderQc.OrderQcid;
        }
        #endregion

        #region CheckUPIandBeneficiary
        /// <summary>
        /// CheckUPIandBeneficiary
        /// </summary>
        /// <param name="regdno"></param>
        /// <returns></returns>
        public string CheckUPIandBeneficiary(string regdno)
        {
            CashfreeAuth cashfreeAuthCall = new CashfreeAuth();
            AddBeneficiary addBeneficiarry = new AddBeneficiary();
            AddBeneficiaryResponse beneficiaryResponse = new AddBeneficiaryResponse();
            string subcode = Convert.ToInt32(CashfreeEnum.Succcess).ToString();
            TblExchangeOrder tblExchangeOrder = null;
            try
            {
                tblExchangeOrder = _ExchangeOrderRepository.GetRegdNo(regdno);
                if (tblExchangeOrder != null && tblExchangeOrder.StatusId >= Convert.ToInt32(OrderStatusEnum.AmountApprovedbyCustomerAfterQC))
                {
                    TblOrderTran tblOrderTran = _orderTransRepository.GetRegdno(regdno);
                    if (tblOrderTran != null)
                    {
                        TblOrderQc tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran.OrderTransId);
                        if (tblOrderQc != null && tblOrderQc.Upiid != null)
                        {
                            if (tblExchangeOrder != null && tblExchangeOrder.CustomerDetailsId != null)
                            {
                                TblCustomerDetail tblCustomerDetail = _customerDetailsRepository.GetCustDetails(tblExchangeOrder.CustomerDetailsId);
                                if (tblCustomerDetail != null)
                                {
                                    cashfreeAuthCall = _cashfreePayoutCall.CashFreeAuthCall();
                                    if (cashfreeAuthCall.subCode == subcode)
                                    {
                                        addBeneficiarry.beneId = regdno;
                                        addBeneficiarry.name = tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName;
                                        addBeneficiarry.email = tblCustomerDetail.Email != null ? tblCustomerDetail.Email : string.Empty;
                                        addBeneficiarry.phone = tblCustomerDetail.PhoneNumber != null ? tblCustomerDetail.PhoneNumber : string.Empty;
                                        addBeneficiarry.address1 = tblCustomerDetail.Address1 != null ? tblCustomerDetail.Address1 : string.Empty;
                                        addBeneficiarry.city = tblCustomerDetail.City != null ? tblCustomerDetail.City : string.Empty;
                                        addBeneficiarry.state = tblCustomerDetail.State != null ? tblCustomerDetail.State : string.Empty;
                                        addBeneficiarry.pincode = tblCustomerDetail.ZipCode != null ? tblCustomerDetail.ZipCode : string.Empty;
                                        addBeneficiarry.vpa = tblOrderQc.Upiid;
                                        beneficiaryResponse = _cashfreePayoutCall.AddBenefiaciary(addBeneficiarry, cashfreeAuthCall.data.token);
                                        if (beneficiaryResponse.subCode == subcode)
                                        {
                                            return beneficiaryResponse.message;
                                        }
                                        else
                                        {
                                            return beneficiaryResponse.message;
                                        }
                                    }
                                    else
                                    {
                                        return cashfreeAuthCall.message;
                                    }
                                }
                                else
                                {
                                    tblCustomerDetail = new TblCustomerDetail();
                                }
                            }
                            else
                            {
                                tblExchangeOrder = new TblExchangeOrder();
                            }
                        }
                        else
                        {
                            return "0";
                        }
                    }
                    else
                    {
                        tblOrderTran = new TblOrderTran();
                    }
                }
                else
                {
                    string msg = "Please complete video QC ";
                    return msg;
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "CheckUPIandBeneficiary", ex);
            }
            return regdno;
        }
        #endregion

        #region Get Status based Count for QC Dashboard 
        /// <summary>
        /// Get Status based Count for QC Dashboard 
        /// </summary>
        /// <param name="BUId"></param>
        /// <returns></returns>
        public QCDashboardViewModel GetQCFlagBasedCount(int companyId)
        {
            QCDashboardViewModel CountListData = new QCDashboardViewModel();

            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            try
            {
                #region Company Check and Filter
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                if (tblBusinessUnit == null) { tblBusinessUnit = new TblBusinessUnit(); }
                #endregion
                CountListData.CountOrderForQC = _ExchangeOrderRepository.GetList(x => x.IsActive == true && (tblBusinessUnit.Name == null || x.CompanyName == tblBusinessUnit.Name)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCInProgress_3Q)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.CallAndGoScheduledAppointmentTaken_3P)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenOrder)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.InstalledbySponsor)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderWithDiagnostic))).Count();

                CountListData.CountSelfQCOrders = _ExchangeOrderRepository.GetList(x => x.IsActive == true && (tblBusinessUnit.Name == null || x.CompanyName == tblBusinessUnit.Name)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.SelfQCbyCustomer))).Count();

                CountListData.CountAllResheduledQC = _orderQCRepository.GetCountByStatusId(tblBusinessUnit.Name,
                    Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled),
                    Convert.ToInt32(OrderStatusEnum.QCAppointmentRescheduled_3RA));

                CountListData.CountResheduledQCAging = _orderQCRepository.GetCountByStatusId
                                (tblBusinessUnit.Name, Convert.ToInt32(OrderStatusEnum.QCAppointmentRescheduled_3RA));

                CountListData.CountResheduledQCCount1 = _orderQCRepository.GetCountByStatusId
                               (tblBusinessUnit.Name, Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled), null, 1);

                CountListData.CountResheduledQCCount2 = _orderQCRepository.GetCountByStatusId
                               (tblBusinessUnit.Name, Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled), null, 2);

                CountListData.CountResheduledQCCount3 = _orderQCRepository.GetCountByStatusId
                               (tblBusinessUnit.Name, Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled), null, 3);

                CountListData.CountPriceQuotedQC = _ExchangeOrderRepository.GetList(x => x.IsActive == true && (tblBusinessUnit.Name == null || x.CompanyName == tblBusinessUnit.Name)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCByPass))).Count();

                CountListData.CountCancelledOrders = _ExchangeOrderRepository.GetList(x => x.IsActive == true && (tblBusinessUnit.Name == null || x.CompanyName == tblBusinessUnit.Name)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.CancelOrder) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentDeclined)
                               || x.StatusId == Convert.ToInt32(OrderStatusEnum.CustomerNotResponding_3C) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCFail_3Y)
                               || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCOrderCancel))).Count();

                CountListData.CountReopenedQC = _ExchangeOrderRepository.GetList(x => x.IsActive == true && (tblBusinessUnit.Name == null || x.CompanyName == tblBusinessUnit.Name)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.QCOrderCancel))).Count();
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetQCFlagBasedCount", ex);
            }
            return CountListData;
        }
        #endregion

        #region Get Order Details By OrderTrans Id
        /// <summary>
        /// Get Order Details By OrderTrans Id 
        /// </summary>
        /// <param name="orderTransId"></param>
        /// <returns>tblOrderTran</returns>
        public TblOrderTran GetOrderDetailsByOrderTransId(int orderTransId)
        {
            TblOrderTran tblOrderTran = null;
            try
            {
                if (orderTransId > 0)
                {
                    tblOrderTran = _orderTransRepository.GetOrderDetailsByTransId(orderTransId);
                }
                else
                {
                    tblOrderTran = new TblOrderTran();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetOrderDetailsByOrderTransId", ex);
            }
            return tblOrderTran;
        }
        #endregion

        #region get questioner report for Upper level bonus cap by QC Admin
        /// <summary>
        /// get order details for Upper level bonus cap by QC Admin
        /// </summary>
        /// <param name="orderTransId"></param>
        /// <returns>adminBonusCapViewModel</returns>
        public List<QCRatingViewModel> GetQuestionerReportByQCTeam(int orderTransId)
        {
            List<QCRatingViewModel> qCRatingList = new List<QCRatingViewModel>();
            QCRatingViewModel qCRatingViewModel = null;
            List<TblOrderQcrating> tblOrderQcratingList = null;
            try
            {
                if (orderTransId > 0)
                {
                    tblOrderQcratingList = _context.TblOrderQcratings
                        .Include(x => x.OrderTrans).Include(x => x.Qcquestion).Include(x => x.QuestionerLov)
                        .Where(x => x.IsActive == true && x.OrderTransId == orderTransId && x.DoneBy == Convert.ToInt32(LoVEnum.QCTeam)).ToList();
                    if (tblOrderQcratingList != null)
                    {
                        foreach (var item in tblOrderQcratingList)
                        {
                            qCRatingViewModel = new QCRatingViewModel();
                            qCRatingViewModel.Qcquestion = item.Qcquestion.Qcquestion;
                            if (item.QuestionerLovid != null)
                            {
                                qCRatingViewModel.Condition = (int)item.QuestionerLovid;
                                qCRatingViewModel.QuestionerLOVName = item.QuestionerLov.QuestionerLovname;
                            }
                            qCRatingViewModel.CommentByQC = item.QcComments;
                            qCRatingList.Add(qCRatingViewModel);
                        }

                        if (qCRatingList != null)
                        {
                            return qCRatingList;
                        }
                        else
                        {
                            return qCRatingList;
                        }
                    }
                    else
                    {
                        return qCRatingList;
                    }
                }
                else
                {
                    return qCRatingList;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetQuestionerReportByQCTeam", ex);
            }

            return qCRatingList;
        }
        #endregion

        #region get QCDetail by OrderTransId -- Added by shashi
        /// <summary>
        /// get QCDetail by OrderTransId 
        /// </summary>
        /// <param name="id">OrderTransId</param>
        /// <returns>QCCommentViewModel</returns>
        public QCCommentViewModel GetQCDetailsByOrderTransId(int orderTransId)
        {
            QCCommentViewModel QcCommentViewModel = new QCCommentViewModel();
            TblWalletTransaction walletTransaction = new TblWalletTransaction();
            TblOrderQc tblOrderQc = null;
            TblExchangeOrderStatus tblExchangeOrderStatus = null;
            TblOrderTran tblOrderTran = null;
            TblAbbredemption tblAbbredemption = null;
            string subcode = Convert.ToInt32(CashfreeEnum.Succcess).ToString();
            GetBeneficiary getBeneficiarry = new GetBeneficiary();
            CashfreeAuth cashfreeAuthCall = new CashfreeAuth();

            try
            {
                if (orderTransId > 0)
                {
                    tblOrderTran = _orderTransRepository.GetOrderDetailsByOrderTransId(orderTransId);
                    if (tblOrderTran != null)
                    {
                        tblAbbredemption = tblOrderTran.Abbredemption;
                    }
                    tblOrderQc = _orderQCRepository.GetQcorderBytransId(orderTransId);
                    if (tblOrderQc != null)
                    {
                        QcCommentViewModel.OrderQCId = tblOrderQc.OrderQcid;
                        QcCommentViewModel.Qccomments = tblOrderQc.Qccomments != null ? tblOrderQc.Qccomments : string.Empty;
                        QcCommentViewModel.ProposedQcdate = tblOrderQc.ProposedQcdate;
                        QcCommentViewModel.PriceAfterQC = tblOrderQc.PriceAfterQc;
                        QcCommentViewModel.CollectedAmount = tblOrderQc.CollectedAmount > 0 ? tblOrderQc.CollectedAmount : 0;

                        if (tblOrderQc.StatusId != null && tblOrderQc.StatusId > 0)
                        {
                            tblExchangeOrderStatus = _ExchangeOrderStatusRepository.GetByStatusId(tblOrderQc.StatusId);
                            if (tblExchangeOrderStatus != null)
                            {
                                QcCommentViewModel.StatusCode = tblExchangeOrderStatus.StatusCode != null ? tblExchangeOrderStatus.StatusCode : string.Empty;
                            }
                            else
                            {
                                QcCommentViewModel.StatusCode = "Not Available";
                            }
                        }
                        QcCommentViewModel.QCDate = Convert.ToDateTime(tblOrderQc.CreatedDate).ToString("dd/MM/yyyy");
                        if (tblOrderQc.ProposedQcdate != null)
                        {
                            QcCommentViewModel.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                        }
                        if (tblOrderQc.Reschedulecount > 0)
                        {
                            QcCommentViewModel.Reschedulecount = (int)tblOrderQc.Reschedulecount;
                        }
                        else
                        {
                            QcCommentViewModel.Reschedulecount = 0;
                        }
                    }
                    //This code  is to set the EVC price.
                    walletTransaction = _walletTransactionRepository.GetdatainOrdertransh(orderTransId);
                    if (walletTransaction != null)
                    {
                        QcCommentViewModel.PriceasperEVC = walletTransaction.OrderAmount;
                    }
                    //else if (QcCommentViewModel.PriceAfterQC != null && (tblOrderQc.StatusId == (int?)OrderStatusEnum.Waitingforcustapproval || tblOrderQc.StatusId == (int?)OrderStatusEnum.AmountApprovedbyCustomerAfterQC || tblOrderQc.StatusId == (int?)OrderStatusEnum.QCByPass))
                    //{
                    //    var result = _commonManager.CalculateEVCPrice(orderTransId);
                    //    if (result > 0)
                    //    {
                    //        QcCommentViewModel.PriceasperEVC = result;
                    //    }
                    //    else
                    //    {
                    //        QcCommentViewModel.PriceasperEVC = 0;
                    //    }
                    //}
                    else
                    {
                        QcCommentViewModel.PriceasperEVC = 0;
                    }

                    if (tblOrderQc != null && tblOrderQc.Upiid == null)
                    {
                        QcCommentViewModel.IsUPINo = true;
                    }
                    else
                    {
                        cashfreeAuthCall = _cashfreePayoutCall.CashFreeAuthCall();
                        if (cashfreeAuthCall.subCode == subcode)
                        {
                            getBeneficiarry = _cashfreePayoutCall.GetBeneficiary(cashfreeAuthCall.data.token, tblOrderTran.RegdNo);
                            if (getBeneficiarry.subCode == subcode)
                            {
                                QcCommentViewModel.IsUPINo = false;
                            }
                            else
                            {
                                QcCommentViewModel.IsUPINo = true;
                            }
                        }
                    }
                }
                else
                {
                    return QcCommentViewModel;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetQcDetails", ex);
            }
            return QcCommentViewModel;
        }
        #endregion

        #region Method to Update rescheduled date and qccomment for orders for ABB -- Added by shashi
        /// <summary>
        /// Method to Update rescheduled date and qccomment for orders for ABB
        /// </summary>
        /// <param name="QCComment"></param>
        /// <param name="UserId"></param>
        /// <returns>QCComment</returns>
        public bool RescheduledQCOrder(QCCommentViewModel QCComment, int UserId)
        {
            TblOrderTran tblOrderTran = null;
            TblOrderQc tblOrderQC = null;
            TblExchangeOrder tblExchangeOrder = null;
            TblAbbredemption tblAbbredemption = null;

            if (QCComment != null)
            {
                tblOrderTran = _orderTransRepository.GetRegdno(QCComment.RegdNo);
                if (tblOrderTran != null)
                {
                    tblOrderQC = _orderQCRepository.GetQcorderBytransId(tblOrderTran.OrderTransId);
                    #region to check whether qc order exit if not create otherwise create 
                    if (tblOrderQC != null && tblOrderQC.OrderQcid > 0)
                    {
                        QCComment.Isrescheduled = true;
                        tblOrderQC.Reschedulecount = QCComment.Reschedulecount;
                        tblOrderQC.Qccomments = QCComment.Qccomments;
                        tblOrderQC.StatusId = QCComment.StatusId;
                        tblOrderQC.ProposedQcdate = QCComment.ProposedQcdate;
                        tblOrderQC.ModifiedBy = UserId;
                        tblOrderQC.ModifiedDate = _currentDatetime;
                        _orderQCRepository.Update(tblOrderQC);
                        _orderQCRepository.SaveChanges();
                    }
                    else
                    {
                        tblOrderQC = new TblOrderQc();
                        QCComment.Isrescheduled = true;
                        tblOrderQC.Reschedulecount = QCComment.Reschedulecount;
                        tblOrderQC.Qccomments = QCComment.Qccomments;
                        tblOrderQC.OrderTransId = tblOrderTran.OrderTransId;
                        tblOrderQC.StatusId = QCComment.StatusId;
                        tblOrderQC.ProposedQcdate = QCComment.ProposedQcdate;
                        tblOrderQC.IsActive = true;
                        tblOrderQC.CreatedBy = UserId;
                        tblOrderQC.CreatedDate = _currentDatetime;
                        _orderQCRepository.Create(tblOrderQC);
                        _orderQCRepository.SaveChanges();
                    }
                    #endregion

                    #region for ABB Orders
                    if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.ABB))
                    {
                        tblAbbredemption = _aBBRedemptionRepository.GetSingle(x => x.IsActive == true && x.RegdNo == QCComment.RegdNo);

                        #region update statusid in tblabbredemption
                        if (tblAbbredemption != null)
                        {
                            tblAbbredemption.StatusId = QCComment.StatusId;
                            tblAbbredemption.ModifiedBy = UserId;
                            tblAbbredemption.ModifiedDate = DateTime.Now;
                            _aBBRedemptionRepository.Update(tblAbbredemption);
                            _aBBRedemptionRepository.SaveChanges();
                        }
                        #endregion

                        #region insert history in tblexchangeabbhistorytable
                        TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                        tblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(LoVEnum.ABB);
                        tblExchangeAbbstatusHistory.SponsorOrderNumber = tblAbbredemption.Sponsor != null ? tblAbbredemption.Sponsor : string.Empty;
                        tblExchangeAbbstatusHistory.RegdNo = tblAbbredemption.RegdNo;
                        tblExchangeAbbstatusHistory.ZohoSponsorId = tblAbbredemption.ZohoAbbredemptionId != null ? Convert.ToString(tblAbbredemption.ZohoAbbredemptionId) : string.Empty;
                        tblExchangeAbbstatusHistory.CustId = tblAbbredemption.CustomerDetailsId != null ? tblAbbredemption.CustomerDetailsId : 0;
                        tblExchangeAbbstatusHistory.StatusId = Convert.ToInt32(tblAbbredemption.StatusId);
                        tblExchangeAbbstatusHistory.IsActive = true;
                        tblExchangeAbbstatusHistory.CreatedBy = UserId;
                        tblExchangeAbbstatusHistory.CreatedDate = DateTime.Now;
                        tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                        tblExchangeAbbstatusHistory.Comment = QCComment.Qccomments;
                        _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                        _exchangeABBStatusHistoryRepository.SaveChanges();
                        QCComment.Isrescheduled = true;
                        #endregion

                    }
                    #endregion

                    #region for Exchange Orders
                    else if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                    {
                        tblExchangeOrder = _ExchangeOrderRepository.GetSingle(x => x.IsActive == true && x.RegdNo == QCComment.RegdNo);
                        #region update statusid in tblexchangeorder
                        tblExchangeOrder.StatusId = QCComment.StatusId;
                        tblExchangeOrder.ModifiedBy = UserId;
                        tblExchangeOrder.ModifiedDate = _currentDatetime;
                        _ExchangeOrderRepository.Update(tblExchangeOrder);
                        _ExchangeOrderRepository.SaveChanges();
                        #endregion

                        #region insert history in tblexchangeabbhistorytable
                        TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
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
                        QCComment.Isrescheduled = true;
                        #endregion
                    }
                    #endregion

                    #region to update for Ordertrans
                    tblOrderTran.StatusId = QCComment.StatusId;
                    tblOrderTran.ModifiedBy = UserId;
                    tblOrderTran.ModifiedDate = _currentDatetime;
                    _orderTransRepository.Update(tblOrderTran);
                    _orderTransRepository.SaveChanges();
                    #endregion
                }
            }
            return QCComment.Isrescheduled;
        }
        #endregion

        #region Method to Cancel order by sponsor -- Added by Shashi
        /// <summary>
        /// Method to Cancel order by sponsor
        /// </summary>
        /// <param name="RegdNo"></param>
        /// <param name="CancelComment"></param>
        /// <param name="UserId"></param>
        /// <returns>flag</returns>
        public bool CancelQCOrder(string RegdNo, string CancelComment, int UserId)
        {
            QCCommentViewModel QcCommentViewModel = new QCCommentViewModel();
            TblExchangeOrderStatus tblExchangeOrderStatuses = null;
            TblExchangeOrder tblExchangeOrder = null;
            TblOrderTran tblOrderTran = null;
            bool flag = false;
            TblAbbredemption tblAbbredemption = null;
            try
            {
                tblOrderTran = _orderTransRepository.GetSingle(x => x.IsActive == true && x.RegdNo == RegdNo);
                tblExchangeOrderStatuses = _ExchangeOrderStatusRepository.GetSingle(x => x.IsActive == true && x.Id == Convert.ToInt32(OrderStatusEnum.CancelOrder));
                if (tblOrderTran != null)
                {
                    #region update ordertrans
                    tblOrderTran.StatusId = tblExchangeOrderStatuses.Id;
                    tblOrderTran.ModifiedBy = UserId;
                    tblOrderTran.ModifiedDate = _currentDatetime;
                    _orderTransRepository.Update(tblOrderTran);
                    _orderTransRepository.SaveChanges();
                    #endregion

                    #region for ABB Orders
                    if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.ABB))
                    {
                        tblAbbredemption = _aBBRedemptionRepository.GetSingle(x => x.IsActive == true && x.RegdNo == RegdNo);

                        #region update statusid in tblabbredemption
                        if (tblAbbredemption != null)
                        {
                            tblAbbredemption.StatusId = tblOrderTran.StatusId;
                            tblAbbredemption.ModifiedBy = UserId;
                            tblAbbredemption.ModifiedDate = DateTime.Now;
                            _aBBRedemptionRepository.Update(tblAbbredemption);
                            _aBBRedemptionRepository.SaveChanges();
                        }
                        #endregion

                        #region insert history in tblexchangeabbhistorytable
                        TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                        tblExchangeAbbstatusHistory.OrderType = 16;
                        tblExchangeAbbstatusHistory.SponsorOrderNumber = tblAbbredemption.Sponsor != null ? tblAbbredemption.Sponsor : string.Empty;
                        tblExchangeAbbstatusHistory.RegdNo = tblAbbredemption.RegdNo;
                        tblExchangeAbbstatusHistory.ZohoSponsorId = tblAbbredemption.ZohoAbbredemptionId != null ? Convert.ToString(tblAbbredemption.ZohoAbbredemptionId) : string.Empty;
                        tblExchangeAbbstatusHistory.CustId = tblAbbredemption.CustomerDetailsId != null ? tblAbbredemption.CustomerDetailsId : 0;
                        tblExchangeAbbstatusHistory.StatusId = Convert.ToInt32(tblAbbredemption.StatusId);
                        tblExchangeAbbstatusHistory.IsActive = true;
                        tblExchangeAbbstatusHistory.CreatedBy = UserId;
                        tblExchangeAbbstatusHistory.CreatedDate = DateTime.Now;
                        tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                        tblExchangeAbbstatusHistory.Comment = CancelComment;
                        _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                        _exchangeABBStatusHistoryRepository.SaveChanges();
                        #endregion

                    }
                    #endregion

                    #region for Exchange Orders
                    else if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                    {
                        tblExchangeOrder = _ExchangeOrderRepository.GetSingle(x => x.IsActive == true && x.RegdNo == RegdNo);
                        #region update statusid in tblexchangeorder
                        tblExchangeOrder.StatusId = tblOrderTran.StatusId;
                        tblExchangeOrder.ModifiedBy = UserId;
                        tblExchangeOrder.ModifiedDate = _currentDatetime;
                        _ExchangeOrderRepository.Update(tblExchangeOrder);
                        _ExchangeOrderRepository.SaveChanges();
                        #endregion

                        #region insert history in tblexchangeabbhistorytable
                        TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                        tblExchangeAbbstatusHistory.OrderType = 17;
                        tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                        tblExchangeAbbstatusHistory.RegdNo = tblExchangeOrder.RegdNo;
                        tblExchangeAbbstatusHistory.ZohoSponsorId = tblExchangeOrder.ZohoSponsorOrderId != null ? tblExchangeOrder.ZohoSponsorOrderId : string.Empty; ;
                        tblExchangeAbbstatusHistory.CustId = tblExchangeOrder.CustomerDetailsId;
                        tblExchangeAbbstatusHistory.StatusId = tblExchangeOrder.StatusId;
                        tblExchangeAbbstatusHistory.IsActive = true;
                        tblExchangeAbbstatusHistory.CreatedBy = UserId;
                        tblExchangeAbbstatusHistory.CreatedDate = DateTime.Now;
                        tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                        tblExchangeAbbstatusHistory.Comment = CancelComment;
                        _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                        _exchangeABBStatusHistoryRepository.SaveChanges();
                        #endregion
                    }
                    #endregion

                    return true;
                }
                else
                {
                    return flag;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "CancelQCOrder", ex);
            }
            return flag;
        }
        #endregion

        #region get abb orderdetails for qc by order trans id -- Added by Shashi
        /// <summary>
        /// get abb orderdetails for qc by order trans id
        /// </summary>
        /// <param name="orderTransId"></param>
        /// <returns>qCCommentViewModel</returns>
        public QCCommentViewModel GetAbbOrderDetailsByTransId(int orderTransId)
        {
            QCCommentViewModel qCCommentViewModel = new QCCommentViewModel();
            TblAbbredemption tblAbbredemption = null;
            TblBrandSmartBuy tblBrandSmartBuy = null;
            TblBrand tblBrand = null;
            try
            {
                if (orderTransId > 0)
                {
                    tblAbbredemption = _aBBRedemptionRepository.GetAbbOrderDetailsByOrderTransId(orderTransId);
                    if (tblAbbredemption != null)
                    {
                        qCCommentViewModel.ABBRedemptionViewModel = new ABBRedemptionViewModel();
                        qCCommentViewModel.ABBRedemptionViewModel.OrderTransId = orderTransId;
                        qCCommentViewModel.ABBRedemptionViewModel.RegdNo = tblAbbredemption.RegdNo != null ? tblAbbredemption.RegdNo : string.Empty;
                        qCCommentViewModel.ABBRedemptionViewModel.RedemptionValue = Math.Ceiling((decimal)tblAbbredemption.RedemptionValue);
                        if (tblAbbredemption.IsDefferedSettelment == false)
                        {
                            qCCommentViewModel.ABBRedemptionViewModel.isABBInstant = true;
                        }
                        else
                        {
                            qCCommentViewModel.ABBRedemptionViewModel.isABBInstant = false;

                        }
                        if (tblAbbredemption.Abbregistration.BusinessUnit != null)
                        {
                            if (tblAbbredemption.Abbregistration.BusinessUnit.IsBumultiBrand == true)
                            {
                                tblBrandSmartBuy = _context.TblBrandSmartBuys.Where(x => x.IsActive == true && x.Id == tblAbbredemption.Abbregistration.NewBrandId).FirstOrDefault();
                                if (tblBrandSmartBuy != null)
                                {
                                    qCCommentViewModel.ABBRedemptionViewModel.BrandName = tblBrandSmartBuy.Name;
                                }
                                else
                                {
                                    qCCommentViewModel.ABBRedemptionViewModel.BrandName = "";
                                }
                            }
                            else
                            {
                                tblBrand = _context.TblBrands.Where(x => x.IsActive == true && x.Id == tblAbbredemption.Abbregistration.NewBrandId).FirstOrDefault();
                                if (tblBrand != null)
                                {
                                    qCCommentViewModel.ABBRedemptionViewModel.BrandName = tblBrand.Name;
                                }
                                else
                                {
                                    qCCommentViewModel.ABBRedemptionViewModel.BrandName = "";
                                }
                            }
                            qCCommentViewModel.ABBRedemptionViewModel.CompanyName = tblAbbredemption.Abbregistration.BusinessUnit.Name != null ? tblAbbredemption.Abbregistration.BusinessUnit.Name : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.ABBRedemptionViewModel.CompanyName = "";
                        }
                        if (tblAbbredemption.CustomerDetails != null)
                        {
                            qCCommentViewModel.ABBRedemptionViewModel.CustomerDetailId = tblAbbredemption.CustomerDetails.Id;
                            qCCommentViewModel.ABBRedemptionViewModel.CustFirstName = tblAbbredemption.CustomerDetails.FirstName != null ? tblAbbredemption.CustomerDetails.FirstName : string.Empty;
                            qCCommentViewModel.ABBRedemptionViewModel.CustLastName = tblAbbredemption.CustomerDetails.LastName != null ? tblAbbredemption.CustomerDetails.LastName : string.Empty;
                            qCCommentViewModel.ABBRedemptionViewModel.CustEmail = tblAbbredemption.CustomerDetails.Email != null ? tblAbbredemption.CustomerDetails.Email : string.Empty;
                            qCCommentViewModel.ABBRedemptionViewModel.CustCity = tblAbbredemption.CustomerDetails.City != null ? tblAbbredemption.CustomerDetails.City : string.Empty;
                            qCCommentViewModel.ABBRedemptionViewModel.CustPinCode = tblAbbredemption.CustomerDetails.ZipCode != null ? tblAbbredemption.CustomerDetails.ZipCode : string.Empty;
                            qCCommentViewModel.ABBRedemptionViewModel.CustAddress1 = tblAbbredemption.CustomerDetails.Address1 != null ? tblAbbredemption.CustomerDetails.Address1 : string.Empty;
                            qCCommentViewModel.ABBRedemptionViewModel.CustAddress2 = tblAbbredemption.CustomerDetails.Address2 != null ? tblAbbredemption.CustomerDetails.Address2 : string.Empty;
                            qCCommentViewModel.ABBRedemptionViewModel.CustMobile = tblAbbredemption.CustomerDetails.PhoneNumber != null ? tblAbbredemption.CustomerDetails.PhoneNumber : string.Empty;
                            qCCommentViewModel.ABBRedemptionViewModel.CustState = tblAbbredemption.CustomerDetails.State != null ? tblAbbredemption.CustomerDetails.State : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.ABBRedemptionViewModel.CustomerDetailId = 0;
                            qCCommentViewModel.ABBRedemptionViewModel.CustFirstName = "";
                            qCCommentViewModel.ABBRedemptionViewModel.CustLastName = "";
                            qCCommentViewModel.ABBRedemptionViewModel.CustEmail = "";
                            qCCommentViewModel.ABBRedemptionViewModel.CustCity = "";
                            qCCommentViewModel.ABBRedemptionViewModel.CustPinCode = "";
                            qCCommentViewModel.ABBRedemptionViewModel.CustAddress1 = "";
                            qCCommentViewModel.ABBRedemptionViewModel.CustAddress2 = "";
                            qCCommentViewModel.ABBRedemptionViewModel.CustMobile = "";
                            qCCommentViewModel.ABBRedemptionViewModel.CustState = "";
                        }
                        if (tblAbbredemption.Abbregistration.NewProductCategory != null)
                        {
                            qCCommentViewModel.ABBRedemptionViewModel.NewProductCategoryName = tblAbbredemption.Abbregistration.NewProductCategory.Description != null ? tblAbbredemption.Abbregistration.NewProductCategory.Description : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.ABBRedemptionViewModel.NewProductCategoryName = "";
                        }
                        if (tblAbbredemption.Abbregistration.NewProductCategoryTypeNavigation != null)
                        {
                            qCCommentViewModel.ABBRedemptionViewModel.NewProductCategoryType = tblAbbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description != null ? tblAbbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.ABBRedemptionViewModel.NewProductCategoryType = "";
                        }
                    }
                }
                else
                {
                    return qCCommentViewModel;
                }
            }
            catch (Exception ex)
            {

                _logging.WriteErrorToDB("QCCommentManager", "GetAbbOrderDetailsByTransId", ex);
            }

            return qCCommentViewModel;
        }
        #endregion

        #region Get Image labels to upload Image for Video QC -- Added by shashi
        /// <summary>
        /// Get Image labels to upload Image for Video QC
        /// </summary>
        /// <param name="regdNo"></param>
        /// <returns></returns>
        public List<ImageLabelViewModel> GetImageLabelUploadByProductCat(string regdNo)
        {
            TblAbbredemption tblAbbredemption = null;
            TblProductType tblProductType = null;
            List<ImageLabelViewModel> imageLabelViewModels = new List<ImageLabelViewModel>();
            List<TblImageLabelMaster> tblImageLabel = new List<TblImageLabelMaster>();
            try
            {
                if (!string.IsNullOrEmpty(regdNo))
                {
                    tblAbbredemption = _aBBRedemptionRepository.GetAbbOrderDetails(regdNo);
                    if (tblAbbredemption != null && tblAbbredemption.Abbregistration != null && tblAbbredemption.Abbregistration.NewProductCategoryTypeId > 0)
                    {
                        tblProductType = _productTypeRepository.GetSingle(x => x.IsActive == true && x.Id == tblAbbredemption.Abbregistration.NewProductCategoryTypeId);
                        if (tblProductType != null)
                        {
                            tblImageLabel = _imageLabelRepository.GetList(x => x.ProductCatId == tblProductType.ProductCatId && x.IsActive == true).ToList();
                        }
                        else
                        {
                            return imageLabelViewModels;
                        }
                    }
                    else
                    {
                        return imageLabelViewModels;
                    }
                }
                imageLabelViewModels = _mapper.Map<List<TblImageLabelMaster>, List<ImageLabelViewModel>>(tblImageLabel);
                return imageLabelViewModels;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetImageLabelUploadByProductCat", ex);
            }
            return imageLabelViewModels;
        }
        #endregion

        #region save abb qc order details -- Added by Shashi
        /// <summary>
        /// save abb qc order details
        /// </summary>
        /// <param name="qCCommentViewModel"></param>
        /// <param name="imageLabelViewModels"></param>
        /// <param name="UserId"></param>
        /// <returns>flag</returns>
        public bool SaveAbbQcOrder(QCCommentViewModel qCCommentViewModel, IList<ImageLabelViewModel> imageLabelViewModels, int UserId)
        {
            bool flag = false;
            TblOrderQc tblOrderQc = null;
            TblLoV tblLoV = null;
            int CountOrderImgUpload = 0;
            bool imgSave = false;
            TblOrderImageUpload tblOrderImageUpload = null;
            TblOrderTran tblOrderTran = null;
            TblAbbredemption tblAbbredemption = null;
            string filePath = null;
            bool videoSave = false;

            if (qCCommentViewModel != null)
            {
                tblOrderQc = _orderQCRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == qCCommentViewModel.ABBRedemptionViewModel.OrderTransId);
                if (qCCommentViewModel.ABBRedemptionViewModel.StatusId == Convert.ToInt32(OrderStatusEnum.QCOrderCancel)
                    || qCCommentViewModel.ABBRedemptionViewModel.StatusId == Convert.ToInt32(OrderStatusEnum.QCByPass)
                    || qCCommentViewModel.ABBRedemptionViewModel.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval))
                {
                    if (tblOrderQc != null && tblOrderQc.OrderQcid > 0)
                    {
                        tblOrderQc.Qccomments = qCCommentViewModel.Qccomments != null ? qCCommentViewModel.Qccomments : string.Empty;
                        tblOrderQc.PriceAfterQc = qCCommentViewModel.ABBRedemptionViewModel.RedemptionValue;
                        tblOrderQc.Qcdate = DateTime.Now;
                        tblOrderQc.StatusId = qCCommentViewModel.ABBRedemptionViewModel.StatusId;
                        tblOrderQc.ModifiedBy = UserId;
                        tblOrderQc.ModifiedDate = DateTime.Now;
                        tblOrderQc.QualityAfterQc = qCCommentViewModel.ABBProductQulity != null ? qCCommentViewModel.ABBProductQulity : tblOrderQc.QualityAfterQc;
                        tblOrderQc.CollectedAmount = qCCommentViewModel.CollectedAmount > 0 ? qCCommentViewModel.CollectedAmount : 0;
                        tblOrderQc.IsPaymentConnected = qCCommentViewModel.IsPaymentConnected;

                        _orderQCRepository.Update(tblOrderQc);
                        _orderQCRepository.SaveChanges();
                    }
                    else
                    {
                        tblOrderQc = new TblOrderQc();
                        tblOrderQc.OrderTransId = qCCommentViewModel.ABBRedemptionViewModel.OrderTransId;
                        tblOrderQc.Qccomments = qCCommentViewModel.Qccomments != null ? qCCommentViewModel.Qccomments : string.Empty;
                        tblOrderQc.PriceAfterQc = qCCommentViewModel.ABBRedemptionViewModel.RedemptionValue;
                        tblOrderQc.Qcdate = DateTime.Now;
                        tblOrderQc.StatusId = qCCommentViewModel.ABBRedemptionViewModel.StatusId;
                        tblOrderQc.IsActive = true;
                        tblOrderQc.CreatedBy = UserId;
                        tblOrderQc.CreatedDate = DateTime.Now;
                        tblOrderQc.QualityAfterQc = qCCommentViewModel.ABBProductQulity != null ? qCCommentViewModel.ABBProductQulity : tblOrderQc.QualityAfterQc;
                        tblOrderQc.IsPaymentConnected = qCCommentViewModel.IsPaymentConnected;
                        tblOrderQc.CollectedAmount = qCCommentViewModel.CollectedAmount > 0 ? qCCommentViewModel.CollectedAmount : 0;
                        _orderQCRepository.Create(tblOrderQc);
                        _orderQCRepository.SaveChanges();
                    }

                    #region Upload Video Qc images In Folder and DB
                    tblLoV = _lovRepository.GetSingle(x => x.IsActive == true && x.LoVname.ToLower().Trim() == EnumHelper.DescriptionAttr(LoVEnum.QCTeam).ToLower().Trim());
                    if (qCCommentViewModel.ABBRedemptionViewModel.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval) || qCCommentViewModel.ABBRedemptionViewModel.StatusId == Convert.ToInt32(OrderStatusEnum.QCOrderCancel))
                    {
                        if (imageLabelViewModels.Count > 0)
                        {
                            foreach (var items in imageLabelViewModels)
                            {
                                if (items.Base64StringValue != null)
                                {
                                    if (items.Base64StringValue == "Videofile" && qCCommentViewModel.VideoBase64StringValue != null)
                                    {
                                        items.FileName = qCCommentViewModel.ABBRedemptionViewModel.RegdNo + "_" + "FinalQCVideo_" + CountOrderImgUpload + ".webm";
                                        filePath = EnumHelper.DescriptionAttr(FilePathEnum.VideoQC);                                        
                                        string filetempPath = string.Concat(_webHostEnvironment.WebRootPath, "\\", filePath);
                                        string videoFilePath = Path.Combine(filetempPath, items.FileName);
                                        if (File.Exists(videoFilePath))
                                        {
                                            byte[] videoBytes = File.ReadAllBytes(videoFilePath);
                                            string base64String = Convert.ToBase64String(videoBytes);
                                            videoSave = _imageHelper.SaveVideoFileFromBase64(base64String, filePath, items.FileName);
                                        }
                                        //videoSave = _imageHelper.SaveVideoFileFromBase64(qCCommentViewModel.VideoBase64StringValue, filePath, items.FileName);
                                        if (videoSave == true)
                                        {
                                            qCCommentViewModel.VideoBase64StringValue = null;
                                        }
                                    }
                                    else
                                    {
                                        items.FileName = qCCommentViewModel.ABBRedemptionViewModel.RegdNo + "_" + "FinalQCImage" + CountOrderImgUpload + ".jpg";
                                        filePath = EnumHelper.DescriptionAttr(FilePathEnum.VideoQC);
                                        imgSave = _imageHelper.SaveFileFromBase64(items.Base64StringValue, filePath, items.FileName);
                                    }
                                    tblOrderImageUpload = _orderImageUploadRepository.GetSingle(x => x.IsActive == true && x.ImageName == items.FileName);
                                    if (tblOrderImageUpload != null)
                                    {
                                        tblOrderImageUpload.Modifiedby = UserId;
                                        tblOrderImageUpload.ModifiedDate = _currentDatetime;
                                        _orderImageUploadRepository.Update(tblOrderImageUpload);
                                    }
                                    else
                                    {
                                        tblOrderImageUpload = new TblOrderImageUpload();
                                        tblOrderImageUpload.ImageName = items.FileName;
                                        tblOrderImageUpload.OrderTransId = qCCommentViewModel.ABBRedemptionViewModel.OrderTransId;
                                        if (tblLoV != null)
                                        {
                                            tblOrderImageUpload.ImageUploadby = tblLoV.LoVid;
                                        }
                                        tblOrderImageUpload.IsActive = true;
                                        tblOrderImageUpload.CreatedBy = UserId;
                                        tblOrderImageUpload.CreatedDate = _currentDatetime;
                                        _orderImageUploadRepository.Create(tblOrderImageUpload);
                                    }
                                    _orderImageUploadRepository.SaveChanges();
                                }
                                CountOrderImgUpload += 1;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    if (tblOrderQc != null && tblOrderQc.OrderQcid > 0)
                    {
                        tblOrderQc.Qccomments = qCCommentViewModel.Qccomments != null ? qCCommentViewModel.Qccomments : string.Empty;
                        tblOrderQc.PriceAfterQc = qCCommentViewModel.ABBRedemptionViewModel.RedemptionValue;
                        tblOrderQc.StatusId = qCCommentViewModel.ABBRedemptionViewModel.StatusId;
                        tblOrderQc.ModifiedBy = UserId;
                        tblOrderQc.ModifiedDate = DateTime.Now;
                        tblOrderQc.QualityAfterQc = qCCommentViewModel.ABBProductQulity != null ? qCCommentViewModel.ABBProductQulity : tblOrderQc.QualityAfterQc;

                        _orderQCRepository.Update(tblOrderQc);
                        _orderQCRepository.SaveChanges();
                    }
                    else
                    {
                        tblOrderQc = new TblOrderQc();
                        tblOrderQc.OrderTransId = qCCommentViewModel.ABBRedemptionViewModel.OrderTransId;
                        tblOrderQc.Qccomments = qCCommentViewModel.Qccomments != null ? qCCommentViewModel.Qccomments : string.Empty;
                        tblOrderQc.PriceAfterQc = qCCommentViewModel.ABBRedemptionViewModel.RedemptionValue;
                        tblOrderQc.StatusId = qCCommentViewModel.ABBRedemptionViewModel.StatusId;
                        tblOrderQc.IsActive = true;
                        tblOrderQc.CreatedBy = UserId;
                        tblOrderQc.CreatedDate = DateTime.Now;
                        tblOrderQc.QualityAfterQc = qCCommentViewModel.ABBProductQulity != null ? qCCommentViewModel.ABBProductQulity : tblOrderQc.QualityAfterQc;

                        _orderQCRepository.Create(tblOrderQc);
                        _orderQCRepository.SaveChanges();
                    }
                }

                #region update orderTrans
                tblOrderTran = _orderTransRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == qCCommentViewModel.ABBRedemptionViewModel.OrderTransId);
                tblOrderTran.StatusId = qCCommentViewModel.ABBRedemptionViewModel.StatusId;
                tblOrderTran.FinalPriceAfterQc = qCCommentViewModel.ABBRedemptionViewModel.RedemptionValue;
                tblOrderTran.ModifiedBy = UserId;
                tblOrderTran.ModifiedDate = DateTime.Now;
                _orderTransRepository.Update(tblOrderTran);
                _orderTransRepository.SaveChanges();
                #endregion

                #region update TblAbbRedemption
                tblAbbredemption = _aBBRedemptionRepository.GetSingle(x => x.IsActive == true && x.RegdNo == qCCommentViewModel.ABBRedemptionViewModel.RegdNo);
                tblAbbredemption.StatusId = qCCommentViewModel.ABBRedemptionViewModel.StatusId;
                tblAbbredemption.FinalRedemptionValue = qCCommentViewModel.ABBRedemptionViewModel.RedemptionValue;
                tblAbbredemption.ModifiedBy = UserId;
                tblAbbredemption.ModifiedDate = DateTime.Now;
                _aBBRedemptionRepository.Update(tblAbbredemption);
                _aBBRedemptionRepository.SaveChanges();
                #endregion

                #region Create and update TblExchangeAbbstatusHistory
                TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                tblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(LoVEnum.ABB);
                tblExchangeAbbstatusHistory.Comment = qCCommentViewModel.Qccomments;
                //tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                tblExchangeAbbstatusHistory.RegdNo = qCCommentViewModel.ABBRedemptionViewModel.RegdNo;
                tblExchangeAbbstatusHistory.CustId = tblAbbredemption.CustomerDetailsId;
                tblExchangeAbbstatusHistory.StatusId = qCCommentViewModel.ABBRedemptionViewModel.StatusId;
                tblExchangeAbbstatusHistory.IsActive = true;
                tblExchangeAbbstatusHistory.CreatedBy = UserId;
                tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                tblExchangeAbbstatusHistory.OrderTransId = qCCommentViewModel.ABBRedemptionViewModel.OrderTransId;
                tblExchangeAbbstatusHistory.Comment = qCCommentViewModel.Qccomments != null ? qCCommentViewModel.Qccomments : string.Empty;
                tblExchangeAbbstatusHistory.JsonObjectString = JsonConvert.SerializeObject(tblExchangeAbbstatusHistory);
                _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                _exchangeABBStatusHistoryRepository.SaveChanges();
                #endregion

                return true;
            }
            else
            {
                return flag;
            }
        }
        #endregion

        #region CheckUPIandBeneficiary for ABB CASES ADDED BY SHASHI
        /// <summary>
        /// CheckUPIandBeneficiaryForAbb
        /// </summary>
        /// <param name="regdno"></param>
        /// <returns></returns>
        public string CheckUPIandBeneficiaryForAbb(string regdno)
        {
            CashfreeAuth cashfreeAuthCall = new CashfreeAuth();
            AddBeneficiary addBeneficiarry = new AddBeneficiary();
            AddBeneficiaryResponse beneficiaryResponse = new AddBeneficiaryResponse();
            string subcode = Convert.ToInt32(CashfreeEnum.Succcess).ToString();
            TblOrderTran tblOrderTran = null;
            TblOrderQc tblOrderQc = null;
            try
            {
                tblOrderTran = _orderTransRepository.GetOrderTransByRegdno(regdno);
                if (tblOrderTran != null && tblOrderTran.Abbredemption != null && tblOrderTran.StatusId >= Convert.ToInt32(OrderStatusEnum.AmountApprovedbyCustomerAfterQC))
                {
                    tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran.OrderTransId);
                    if (tblOrderQc != null && tblOrderQc.Upiid != null)
                    {
                        if (tblOrderTran.Abbredemption.CustomerDetailsId != null)
                        {
                            TblCustomerDetail tblCustomerDetail = _customerDetailsRepository.GetCustDetails(tblOrderTran.Abbredemption.CustomerDetailsId);
                            if (tblCustomerDetail != null)
                            {
                                cashfreeAuthCall = _cashfreePayoutCall.CashFreeAuthCall();
                                if (cashfreeAuthCall.subCode == subcode)
                                {
                                    addBeneficiarry.beneId = regdno;
                                    addBeneficiarry.name = tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName;
                                    addBeneficiarry.email = tblCustomerDetail.Email != null ? tblCustomerDetail.Email : string.Empty;
                                    addBeneficiarry.phone = tblCustomerDetail.PhoneNumber != null ? tblCustomerDetail.PhoneNumber : string.Empty;
                                    addBeneficiarry.address1 = tblCustomerDetail.Address1 != null ? tblCustomerDetail.Address1 : string.Empty;
                                    addBeneficiarry.city = tblCustomerDetail.City != null ? tblCustomerDetail.City : string.Empty;
                                    addBeneficiarry.state = tblCustomerDetail.State != null ? tblCustomerDetail.State : string.Empty;
                                    addBeneficiarry.pincode = tblCustomerDetail.ZipCode != null ? tblCustomerDetail.ZipCode : string.Empty;
                                    addBeneficiarry.vpa = tblOrderQc.Upiid;
                                    beneficiaryResponse = _cashfreePayoutCall.AddBenefiaciary(addBeneficiarry, cashfreeAuthCall.data.token);
                                    if (beneficiaryResponse.subCode == subcode)
                                    {
                                        return beneficiaryResponse.message;
                                    }
                                    else
                                    {
                                        return beneficiaryResponse.message;
                                    }
                                }
                                else
                                {
                                    return cashfreeAuthCall.message;
                                }
                            }
                            else
                            {
                                tblCustomerDetail = new TblCustomerDetail();
                            }
                        }
                        else
                        {
                            tblOrderTran.Abbredemption = new TblAbbredemption();
                        }
                    }
                    else
                    {
                        return "0";
                    }

                }
                else
                {
                    string msg = "Please complete video QC ";
                    return msg;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "CheckUPIandBeneficiaryForAbb", ex);
            }
            return regdno;
        }
        #endregion

        #region Method to get PriceAfterQc Validation based GetPriceAfterQc
        /// <summary>
        /// Method to get PriceAfterQc according to brand,product category, type
        /// </summary>
        /// <param name="id">qCCommentViewModel</param>
        /// <returns>QCCommentViewModel</returns>
        //public UniversalPMViewModel ValidationbasedGetPriceAfterQc(string exchangerno, int ProdConditionId, string custQuality, int UserId, bool InvoiceV, bool Instollestion, int newbrandid, int Modelnoid)
        //{

        //    List<decimal> Qcprice = new List<decimal>();
        //    TblExchangeOrder? tblExchangeOrder = null;
        //    bool? isInvoiceR = true;
        //    bool? isInstollestionR = true;
        //    bool SweetnerEligible = false;
        //    bool InvoiceSweetnerEligible = true;
        //    bool InstollestionSweetnerEligible = true;
        //    List<TblBubasedSweetnerValidation> tblBubasedSweetnerValidations = new List<TblBubasedSweetnerValidation>();
        //    UniversalPMViewModel universalPMView = new UniversalPMViewModel();

        //    try
        //    {
        //        tblExchangeOrder = _ExchangeOrderRepository.GetRegdNo(exchangerno);

        //        tblBubasedSweetnerValidations = _bUBasedSweetnerValidationRepository.GetQustionList(tblExchangeOrder.BusinessPartner.BusinessUnit.BusinessUnitId);
        //        if (tblBubasedSweetnerValidations != null)
        //        {

        //            foreach (TblBubasedSweetnerValidation item in tblBubasedSweetnerValidations)
        //            {
        //                if (item.Question.QuestionKey == (SweetnerValidationQustionEnum.InvoiceV).ToString())
        //                {
        //                    isInvoiceR = item.IsRequired== true? item.IsRequired : false;
        //                    if (InvoiceV == isInvoiceR || InvoiceV == true)
        //                    {
        //                        InvoiceSweetnerEligible = true;
        //                    }
        //                    else
        //                    {
        //                        InvoiceSweetnerEligible = false;
        //                    }
        //                }
        //                if (item.Question.QuestionKey == (SweetnerValidationQustionEnum.InstallationV).ToString())
        //                {
        //                    isInstollestionR = item.IsRequired == true ? item.IsRequired : false;
        //                    if (Instollestion == isInstollestionR || Instollestion == true)
        //                    {
        //                        InstollestionSweetnerEligible = true;
        //                    }
        //                    else
        //                    {
        //                        InstollestionSweetnerEligible = false;
        //                    }
        //                }

        //            }
        //            if (InvoiceSweetnerEligible && InstollestionSweetnerEligible)
        //            {
        //                SweetnerEligible = true;
        //            }
        //            else
        //            {
        //                SweetnerEligible = false;
        //            }
        //        }
        //        QCProductPriceDetails qCProductPriceDetails = new QCProductPriceDetails();
        //        qCProductPriceDetails.RegdNo = exchangerno;
        //        qCProductPriceDetails.conditionId = ProdConditionId;
        //        qCProductPriceDetails.CustQuality = custQuality;
        //        qCProductPriceDetails.NewBrandId = newbrandid;
        //        qCProductPriceDetails.NewModelId = Modelnoid;
        //        universalPMView = GetProductPrice(qCProductPriceDetails);

        //        if (universalPMView != null)
        //        {                    
        //            if (universalPMView.CollectedAmount == null)
        //            {
        //                universalPMView.CollectedAmount = 0;
        //            }
        //            else
        //            {
        //                universalPMView.CollectedAmount = universalPMView.CollectedAmount;
        //            }
        //            if (SweetnerEligible == true && universalPMView.TotalSweetener != null && universalPMView.TotalSweetener > 0)
        //            {
        //                universalPMView.SweetenerBP = universalPMView.SweetenerBP > 0 ? universalPMView.SweetenerBP : 0;
        //                universalPMView.SweetenerBU = universalPMView.SweetenerBU > 0 ? universalPMView.SweetenerBU : 0;
        //                universalPMView.SweetenerDigi2l = universalPMView.SweetenerDigi2l > 0 ? universalPMView.SweetenerDigi2l : 0;
        //                universalPMView.TotalSweetener = universalPMView.TotalSweetener > 0 ? universalPMView.TotalSweetener : 0;

        //            }
        //            else
        //            {
        //                universalPMView.SweetenerBP = 0;
        //                universalPMView.SweetenerBU = 0;
        //                universalPMView.SweetenerDigi2l = 0;
        //                universalPMView.TotalSweetener = 0;
        //            }
        //            //var sweetner = (universalPMView.TotalSweetener != null && universalPMView.TotalSweetener > 0) ? universalPMView.TotalSweetener : 0;

        //            universalPMView.BaseValue = universalPMView.BaseValue > 0 ? universalPMView.BaseValue : 0;
        //           // universalPMView.FinalQCPrice = universalPMView.BaseValue + sweetner;
        //            universalPMView.FinalQCPrice = (universalPMView.BaseValue ?? 0) + (universalPMView.TotalSweetener ?? 0);
        //        }
        //        else
        //        {
        //            universalPMView = new UniversalPMViewModel
        //            {
        //                BaseValue = 0,                      
        //                CollectedAmount = 0,
        //                TotalSweetener = 0,
        //                SweetenerBP = 0,
        //                SweetenerBU = 0,
        //                SweetenerDigi2l = 0,
        //                FinalQCPrice = 0
        //            };

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.WriteErrorToDB("QCCommentManager", "GetPriceAfterQc", ex);
        //    }
        //    return universalPMView;
        //}

        public UniversalPMViewModel ValidationbasedGetPriceAfterQc(string exchangerno, int ProdConditionId, string custQuality, int UserId, bool InvoiceV, bool Instollestion, int newbrandid, int Modelnoid)
        {

            List<decimal> Qcprice = new List<decimal>();
            TblExchangeOrder? tblExchangeOrder = null;
            bool? isInvoiceR = true;
            bool? isInstollestionR = true;
            bool SweetnerEligible = true;
            bool InvoiceSweetnerEligible = true;
            bool InstollestionSweetnerEligible = true;
            List<TblBubasedSweetnerValidation> tblBubasedSweetnerValidations = new List<TblBubasedSweetnerValidation>();
            UniversalPMViewModel universalPMView = new UniversalPMViewModel();

            try
            {
                tblExchangeOrder = _ExchangeOrderRepository.GetRegdNo(exchangerno);

                tblBubasedSweetnerValidations = _bUBasedSweetnerValidationRepository.GetQustionList(tblExchangeOrder.BusinessPartner.BusinessUnit.BusinessUnitId);
                if (tblBubasedSweetnerValidations != null && tblBubasedSweetnerValidations.Count > 0)
                {
                    int count = 0;
                    tblBubasedSweetnerValidations = tblBubasedSweetnerValidations.Where(x => x.IsRequired == true).ToList();
                    foreach (TblBubasedSweetnerValidation item in tblBubasedSweetnerValidations)
                    {
                        if ((item.Question.QuestionKey == (SweetnerValidationQustionEnum.InvoiceV.ToString()) && !(InvoiceV)))
                        {
                            count++;
                        }
                        if ((item.Question.QuestionKey == (SweetnerValidationQustionEnum.InstallationV.ToString()) && !(Instollestion)))
                        {
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        SweetnerEligible = false;
                    }
                    else
                    {
                        SweetnerEligible = true;
                    }
                }
                QCProductPriceDetails qCProductPriceDetails = new QCProductPriceDetails();
                qCProductPriceDetails.RegdNo = exchangerno;
                qCProductPriceDetails.conditionId = ProdConditionId;
                qCProductPriceDetails.CustQuality = custQuality;
                qCProductPriceDetails.NewBrandId = newbrandid;
                qCProductPriceDetails.NewModelId = Modelnoid;
                universalPMView = GetProductPrice(qCProductPriceDetails);

                if (universalPMView != null)
                {
                    if (universalPMView.CollectedAmount == null)
                    {
                        universalPMView.CollectedAmount = 0;
                    }
                    else
                    {
                        universalPMView.CollectedAmount = universalPMView.CollectedAmount;
                    }
                    if (SweetnerEligible == true && universalPMView.TotalSweetener != null && universalPMView.TotalSweetener > 0)
                    {
                        universalPMView.SweetenerBP = universalPMView.SweetenerBP > 0 ? universalPMView.SweetenerBP : 0;
                        universalPMView.SweetenerBU = universalPMView.SweetenerBU > 0 ? universalPMView.SweetenerBU : 0;
                        universalPMView.SweetenerDigi2l = universalPMView.SweetenerDigi2l > 0 ? universalPMView.SweetenerDigi2l : 0;
                        universalPMView.TotalSweetener = universalPMView.TotalSweetener > 0 ? universalPMView.TotalSweetener : 0;
                    }
                    else
                    {
                        universalPMView.SweetenerBP = 0;
                        universalPMView.SweetenerBU = 0;
                        universalPMView.SweetenerDigi2l = 0;
                        universalPMView.TotalSweetener = 0;
                    }
                    //var sweetner = (universalPMView.TotalSweetener != null && universalPMView.TotalSweetener > 0) ? universalPMView.TotalSweetener : 0;

                    universalPMView.BaseValue = universalPMView.BaseValue > 0 ? universalPMView.BaseValue : 0;
                    // universalPMView.FinalQCPrice = universalPMView.BaseValue + sweetner;
                    universalPMView.FinalQCPrice = (universalPMView.BaseValue ?? 0) + (universalPMView.TotalSweetener ?? 0);
                }
                else
                {
                    universalPMView = new UniversalPMViewModel
                    {
                        BaseValue = 0,
                        CollectedAmount = 0,
                        TotalSweetener = 0,
                        SweetenerBP = 0,
                        SweetenerBU = 0,
                        SweetenerDigi2l = 0,
                        FinalQCPrice = 0
                    };

                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetPriceAfterQc", ex);
            }
            return universalPMView;
        }

        private decimal GetPriceByQuality(string quality, TblPriceMaster priceMasters, bool isHighBrand)
        {
            decimal price = 0;
            if (isHighBrand)
            {
                switch (quality)
                {
                    case "Excellent":
                        price = Convert.ToDecimal(priceMasters.QuotePHigh);
                        break;
                    case "Good":
                        price = Convert.ToDecimal(priceMasters.QuoteQHigh);
                        break;
                    case "Average":
                        price = Convert.ToDecimal(priceMasters.QuoteRHigh);
                        break;
                    case "NotWorking":
                        price = Convert.ToDecimal(priceMasters.QuoteSHigh);
                        break;
                }
            }
            else
            {
                switch (quality)
                {
                    case "Excellent":
                        price = Convert.ToDecimal(priceMasters.QuoteP);
                        break;
                    case "Good":
                        price = Convert.ToDecimal(priceMasters.QuoteQ);
                        break;
                    case "Average":
                        price = Convert.ToDecimal(priceMasters.QuoteR);
                        break;
                    case "NotWorking":
                        price = Convert.ToDecimal(priceMasters.QuoteS);
                        break;
                }
            }
            return price;
        }

        #endregion

        #region GetSweetnerPrice Add by Priyanshi sahu Date 21-July, For BU Based Validation on Sweetner
        public List<BUBasedSweetnerValidation> GetBUSweetnerValidationQuestions(int businessUnitId)
        {
            //_buisnessUnitRepository = new BusinessUnitRepository();
            //_questionsForSweetnerRepository = new QuestionsForSweetnerRepository();
            //_bUBasedSweetnerValidationRepository = new BUBasedSweetnerValidationRepository();
            List<BUBasedSweetnerValidation> buBasedSweetnerValidationsList = new List<BUBasedSweetnerValidation>();
            BUBasedSweetnerValidation bUBasedSweetnerValidation = null;
            TblQuestionsForSweetner tblQuestionsForSweetner = null;
            List<TblBubasedSweetnerValidation> tblBUBasedSweetnerValidationList = null;
            try
            {
                if (businessUnitId > 0)
                {
                    TblBusinessUnit buisnessobj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == businessUnitId);
                    if (buisnessobj != null)
                    {
                        tblBUBasedSweetnerValidationList = _bUBasedSweetnerValidationRepository.GetList(x => x.IsActive == true && x.IsDisplay == true && x.BusinessUnitId == buisnessobj.BusinessUnitId).ToList();
                        if (tblBUBasedSweetnerValidationList != null && tblBUBasedSweetnerValidationList.Count > 0)
                        {
                            foreach (TblBubasedSweetnerValidation item in tblBUBasedSweetnerValidationList)
                            {
                                if (item != null)
                                {
                                    bUBasedSweetnerValidation = new BUBasedSweetnerValidation();
                                    tblQuestionsForSweetner = _questionsForSweetnerRepository.GetSingle(x => x.IsActive == true && x.QuestionId == item.QuestionId);
                                    if (tblQuestionsForSweetner != null)
                                    {
                                        bUBasedSweetnerValidation = _mapper.Map<TblBubasedSweetnerValidation, BUBasedSweetnerValidation>(item);

                                        bUBasedSweetnerValidation.Question = tblQuestionsForSweetner.Question;
                                        bUBasedSweetnerValidation.QuestionKey = tblQuestionsForSweetner.QuestionKey;
                                        buBasedSweetnerValidationsList.Add(bUBasedSweetnerValidation);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetPriceAfterQc", ex);
            }
            return buBasedSweetnerValidationsList;
        }
        #endregion

        #region save qc comment and statusid for qc details page
        /// <summary>
        /// save qc comment and statusid for qc details page
        /// </summary>
        /// <param name="exchangeOrderViewModel"></param>
        /// <param name="userId"></param>
        /// <returns>bool</returns>
        public bool SaveStatuIdForQcDetails(ExchangeOrderViewModel exchangeOrderViewModel, int userId)
        {
            bool flag = false;
            TblOrderTran tblOrderTran = null;
            TblOrderQc tblOrderQc = null;
            TblExchangeOrder tblExchangeOrder = null;
            try
            {
                if (exchangeOrderViewModel != null && exchangeOrderViewModel.QCCommentViewModel != null && exchangeOrderViewModel.QCCommentViewModel.Qccomments != null && exchangeOrderViewModel.QCCommentViewModel.StatusId > 0)
                {
                    #region update statusId in tblordertrans
                    tblOrderTran = _context.TblOrderTrans.Where(x => x.IsActive == true && x.RegdNo == exchangeOrderViewModel.RegdNo).FirstOrDefault();
                    if (tblOrderTran != null)
                    {
                        tblOrderTran.StatusId = exchangeOrderViewModel.QCCommentViewModel.StatusId;
                        tblOrderTran.ModifiedBy = userId;
                        tblOrderTran.ModifiedDate = DateTime.Now;
                        _orderTransRepository.Update(tblOrderTran);
                        _orderTransRepository.SaveChanges();
                    }
                    #endregion

                    #region insert into tblorderqc
                    tblOrderQc = _context.TblOrderQcs.Where(x => x.IsActive == true && x.OrderTransId == tblOrderTran.OrderTransId).FirstOrDefault();
                    if (tblOrderQc == null)
                    {
                        tblOrderQc = new TblOrderQc();
                        tblOrderQc.OrderTransId = tblOrderTran.OrderTransId;
                        tblOrderQc.Qcdate = DateTime.Now;
                        tblOrderQc.StatusId = exchangeOrderViewModel.QCCommentViewModel.StatusId;
                        tblOrderQc.Qccomments = exchangeOrderViewModel.QCCommentViewModel.Qccomments;
                        tblOrderQc.IsActive = true;
                        tblOrderQc.CreatedBy = userId;
                        tblOrderQc.CreatedDate = DateTime.Now;
                        _orderQCRepository.Create(tblOrderQc);
                        _orderQCRepository.SaveChanges();
                    }
                    else
                    {
                        tblOrderQc.OrderTransId = tblOrderTran.OrderTransId;
                        tblOrderQc.Qcdate = DateTime.Now;
                        tblOrderQc.StatusId = exchangeOrderViewModel.QCCommentViewModel.StatusId;
                        tblOrderQc.Qccomments = exchangeOrderViewModel.QCCommentViewModel.Qccomments;
                        tblOrderQc.IsActive = true;
                        tblOrderQc.ModifiedDate = DateTime.Now;
                        tblOrderQc.ModifiedBy = userId;
                        _orderQCRepository.Update(tblOrderQc);
                        _orderQCRepository.SaveChanges();
                    }

                    #endregion

                    #region update price and statusId in tblexchangeOrder
                    tblExchangeOrder = _context.TblExchangeOrders.Where(x => x.IsActive == true && x.RegdNo == tblOrderTran.RegdNo).FirstOrDefault();
                    if (tblExchangeOrder != null)
                    {
                        tblExchangeOrder.StatusId = exchangeOrderViewModel.QCCommentViewModel.StatusId;
                        tblExchangeOrder.ModifiedBy = userId;
                        tblExchangeOrder.ModifiedDate = DateTime.Now;
                        _ExchangeOrderRepository.Update(tblExchangeOrder);
                        _ExchangeOrderRepository.SaveChanges();
                    }
                    #endregion

                    #region Insert in tblexchangeabbhistory
                    TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                    tblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(LoVEnum.Exchange);
                    tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                    tblExchangeAbbstatusHistory.RegdNo = tblExchangeOrder.RegdNo;
                    tblExchangeAbbstatusHistory.ZohoSponsorId = tblExchangeOrder.ZohoSponsorOrderId != null ? tblExchangeOrder.ZohoSponsorOrderId : string.Empty; ;
                    tblExchangeAbbstatusHistory.CustId = tblExchangeOrder.CustomerDetailsId;
                    tblExchangeAbbstatusHistory.StatusId = tblExchangeOrder.StatusId;
                    tblExchangeAbbstatusHistory.IsActive = true;
                    tblExchangeAbbstatusHistory.CreatedBy = userId;
                    tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                    tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                    tblExchangeAbbstatusHistory.Comment = exchangeOrderViewModel.QCCommentViewModel.Qccomments;
                    _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                    _exchangeABBStatusHistoryRepository.SaveChanges();
                    #endregion

                    return true;

                }
                else
                {
                    return flag;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "SaveStatuIdForQcDetails", ex);
            }
            return flag;
        }
        #endregion

        #region MVC Method to get Price After Qc by cust declare quality Added by Pooja Jatav
        public UniversalPMViewModel GetProductPrice(QCProductPriceDetails QCdetails)
        {
            UniversalPMViewModel? universalPMViewModel = null;
            TblUniversalPriceMaster? tblUniversalPriceMaster = null;
            TblPriceMasterMapping? priceMasterMappingObj = null;
            ProductPriceViewModel? productPriceViewModel = null;
            TblExchangeOrder tblExchangeOrder = null;
            TblProductConditionLabel? tblProductConditionLabel = null;
            GetSweetenerDetailsDataContract? getsweetenerDataViewModel = null;
            SweetenerDataViewModel? sweetenerDataVM = null;
            TblOrderBasedConfig tblOrderBasedConfig = null;
            TblBrand? tblBrand = null;
            TblProductType? tblProductType = null;
            TblBusinessUnit? tblBusinessUnit = null;
            try
            {
                if (QCdetails != null && QCdetails.RegdNo != null)
                {
                    tblExchangeOrder = _ExchangeOrderRepository.GetRegdNo(QCdetails.RegdNo);
                    if (tblExchangeOrder != null)
                    {
                        if (tblExchangeOrder.NewProductTypeId != null && tblExchangeOrder.NewProductTypeId > 0)
                        {
                            tblProductType = _productTypeRepository.GetCatTypebytypeid(tblExchangeOrder.NewProductTypeId);
                        }
                        if (tblExchangeOrder.IsDefferedSettlement == true || Convert.ToInt32(tblExchangeOrder.IsDefferedSettlement ?? false) == 1)
                        {
                            getsweetenerDataViewModel = new GetSweetenerDetailsDataContract();
                            if (QCdetails.NewBrandId > 0)
                            {
                                //priceMasterMappingObj = _priceMasterMappingRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == tblExchangeOrder.BusinessUnitId && x.BusinessPartnerId == tblExchangeOrder.BusinessPartnerId && x.BrandId == QCdetails.NewBrandId);
                                priceMasterMappingObj = _priceMasterMappingRepository.GetProductPriceByBUIdBPIdBrandId(tblExchangeOrder.BusinessUnitId, tblExchangeOrder.BusinessPartnerId, QCdetails.NewBrandId);
                                if (priceMasterMappingObj != null)
                                {
                                    QCdetails.PriceNameId = priceMasterMappingObj.PriceMasterNameId;
                                    tblProductConditionLabel = _productConditionLabelRepository.GetOrderSequenceNo(QCdetails.conditionId);

                                    if (tblProductConditionLabel != null)
                                    {
                                        universalPMViewModel = new UniversalPMViewModel();
                                        QCdetails.FinalProdQualityId = tblProductConditionLabel.OrderSequence;
                                        QCdetails.BrandId = tblExchangeOrder.BrandId != null ? tblExchangeOrder.BrandId : null;
                                        QCdetails.ProductTypeId = tblExchangeOrder.ProductTypeId != null ? tblExchangeOrder.ProductTypeId : null;
                                        universalPMViewModel = GetBasePrice(QCdetails);
                                        if (tblExchangeOrder.BusinessUnitId != null && tblExchangeOrder.BusinessUnitId > 0)
                                        {
                                            tblBusinessUnit = _businessUnitRepository.GetBusinessunitDetails(Convert.ToInt32(tblExchangeOrder.BusinessUnitId));
                                            if (tblBusinessUnit != null && tblBusinessUnit.IsSweetenerIndependent != null && tblBusinessUnit.IsSweetenerIndependent == true)
                                            {
                                                universalPMViewModel.FinalQCPrice = universalPMViewModel.BaseValue + tblExchangeOrder.Sweetener;
                                                universalPMViewModel.TotalSweetener = tblExchangeOrder.Sweetener;
                                            }
                                            else
                                            {
                                                if (tblProductConditionLabel.IsSweetenerApplicable == true)
                                                {
                                                    tblOrderBasedConfig = _orderBasedConfigRepository.GetIsSweetenerModelbase(tblExchangeOrder.BusinessUnitId, tblExchangeOrder.BusinessPartnerId);
                                                    getsweetenerDataViewModel.IsSweetenerModalBased = tblOrderBasedConfig != null ? tblOrderBasedConfig.IsSweetenerModalBased ?? false : false;
                                                    getsweetenerDataViewModel.BrandId = QCdetails.NewBrandId;
                                                    getsweetenerDataViewModel.NewProdCatId = tblProductType != null ? tblProductType.ProductCatId : null;
                                                    getsweetenerDataViewModel.NewProdTypeId = tblExchangeOrder.NewProductTypeId != null ? tblExchangeOrder.NewProductTypeId : null;
                                                    getsweetenerDataViewModel.BusinessUnitId = tblExchangeOrder.BusinessUnitId;
                                                    getsweetenerDataViewModel.BusinessPartnerId = tblExchangeOrder.BusinessPartnerId;
                                                    if (QCdetails.NewModelId > 0)
                                                    {
                                                        getsweetenerDataViewModel.ModalId = QCdetails.NewModelId;
                                                    }
                                                    else
                                                    {
                                                        getsweetenerDataViewModel.ModalId = tblExchangeOrder.ModelNumberId;
                                                    }

                                                    sweetenerDataVM = _sweetenerManager.GetSweetenerAmtExchange(getsweetenerDataViewModel);

                                                    if (sweetenerDataVM != null)
                                                    {
                                                        universalPMViewModel.FinalQCPrice = universalPMViewModel.BaseValue + sweetenerDataVM.SweetenerTotal;
                                                        universalPMViewModel.TotalSweetener = sweetenerDataVM.SweetenerTotal;
                                                        universalPMViewModel.SweetenerBU = sweetenerDataVM.SweetenerBu;
                                                        universalPMViewModel.SweetenerBP = sweetenerDataVM.SweetenerBP;
                                                        universalPMViewModel.SweetenerDigi2l = sweetenerDataVM.SweetenerDigi2L;
                                                        universalPMViewModel.ErrorMessage = sweetenerDataVM.ErrorMessage != null ? sweetenerDataVM.ErrorMessage : null;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            universalPMViewModel = new UniversalPMViewModel();
                                            universalPMViewModel.ErrorMessage = "BusinessUnitId is null in Exchange order table";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                priceMasterMappingObj = _priceMasterMappingRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == tblExchangeOrder.BusinessUnitId && x.BusinessPartnerId == tblExchangeOrder.BusinessPartnerId && x.BrandId == null);
                                if (priceMasterMappingObj != null)
                                {
                                    QCdetails.PriceNameId = priceMasterMappingObj.PriceMasterNameId;
                                    tblProductConditionLabel = _productConditionLabelRepository.GetOrderSequenceNo(QCdetails.conditionId);
                                    if (tblProductConditionLabel != null)
                                    {
                                        universalPMViewModel = new UniversalPMViewModel();
                                        QCdetails.FinalProdQualityId = tblProductConditionLabel.OrderSequence;
                                        QCdetails.BrandId = tblExchangeOrder.BrandId != null ? tblExchangeOrder.BrandId : null;
                                        QCdetails.ProductTypeId = tblExchangeOrder.ProductTypeId != null ? tblExchangeOrder.ProductTypeId : null;
                                        universalPMViewModel = GetBasePrice(QCdetails);
                                        if (tblExchangeOrder.BusinessUnitId != null && tblExchangeOrder.BusinessUnitId > 0)
                                        {
                                            tblBusinessUnit = _businessUnitRepository.GetBusinessunitDetails(Convert.ToInt32(tblExchangeOrder.BusinessUnitId));
                                            if (tblBusinessUnit != null && tblBusinessUnit.IsSweetenerIndependent != null && tblBusinessUnit.IsSweetenerIndependent == true)
                                            {
                                                universalPMViewModel.FinalQCPrice = universalPMViewModel.BaseValue + tblExchangeOrder.Sweetener;
                                                universalPMViewModel.TotalSweetener = tblExchangeOrder.Sweetener;
                                            }
                                            else
                                            {
                                                if (tblProductConditionLabel.IsSweetenerApplicable == true)
                                                {
                                                    tblOrderBasedConfig = _orderBasedConfigRepository.GetIsSweetenerModelbase(tblExchangeOrder.BusinessUnitId, tblExchangeOrder.BusinessPartnerId);
                                                    getsweetenerDataViewModel.IsSweetenerModalBased = tblOrderBasedConfig != null ? tblOrderBasedConfig.IsSweetenerModalBased ?? false : false;
                                                    getsweetenerDataViewModel.BrandId = QCdetails.NewBrandId;
                                                    getsweetenerDataViewModel.NewProdCatId = tblProductType != null ? tblProductType.ProductCatId : null;
                                                    getsweetenerDataViewModel.NewProdTypeId = tblExchangeOrder.NewProductTypeId != null ? tblExchangeOrder.NewProductTypeId : null;
                                                    getsweetenerDataViewModel.BusinessUnitId = tblExchangeOrder.BusinessUnitId;
                                                    getsweetenerDataViewModel.BusinessPartnerId = tblExchangeOrder.BusinessPartnerId;
                                                    if (QCdetails.NewModelId > 0)
                                                    {
                                                        getsweetenerDataViewModel.ModalId = QCdetails.NewModelId;
                                                    }
                                                    else
                                                    {
                                                        getsweetenerDataViewModel.ModalId = tblExchangeOrder.ModelNumberId;
                                                    }

                                                    sweetenerDataVM = _sweetenerManager.GetSweetenerAmtExchange(getsweetenerDataViewModel);
                                                    if (sweetenerDataVM != null)
                                                    {

                                                        universalPMViewModel.FinalQCPrice = universalPMViewModel.BaseValue + sweetenerDataVM.SweetenerTotal;
                                                        universalPMViewModel.TotalSweetener = sweetenerDataVM.SweetenerTotal;
                                                        universalPMViewModel.SweetenerBU = sweetenerDataVM.SweetenerBu;
                                                        universalPMViewModel.SweetenerBP = sweetenerDataVM.SweetenerBP;
                                                        universalPMViewModel.SweetenerDigi2l = sweetenerDataVM.SweetenerDigi2L;
                                                        universalPMViewModel.ErrorMessage = sweetenerDataVM.ErrorMessage != null ? sweetenerDataVM.ErrorMessage : null;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            universalPMViewModel = new UniversalPMViewModel();
                                            universalPMViewModel.ErrorMessage = "BusinessUnitId is null in Exchange order table";
                                        }
                                    }
                                }
                                //Code to fetch Default price master for  that  businessunit which is mapped in price master mapping table
                                else
                                {
                                    priceMasterMappingObj = _priceMasterMappingRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == null && x.BusinessUnitId == tblExchangeOrder.BusinessUnitId && x.BrandId == null);
                                    if (priceMasterMappingObj != null)
                                    {
                                        QCdetails.PriceNameId = priceMasterMappingObj.PriceMasterNameId;

                                        tblProductConditionLabel = _productConditionLabelRepository.GetOrderSequenceNo(QCdetails.conditionId);
                                        if (tblProductConditionLabel != null)
                                        {
                                            universalPMViewModel = new UniversalPMViewModel();
                                            QCdetails.FinalProdQualityId = tblProductConditionLabel.OrderSequence;
                                            QCdetails.BrandId = tblExchangeOrder.BrandId != null ? tblExchangeOrder.BrandId : null;
                                            QCdetails.ProductTypeId = tblExchangeOrder.ProductTypeId != null ? tblExchangeOrder.ProductTypeId : null;
                                            universalPMViewModel = GetBasePrice(QCdetails);
                                            if (tblExchangeOrder.BusinessUnitId != null && tblExchangeOrder.BusinessUnitId > 0)
                                            {
                                                tblBusinessUnit = _businessUnitRepository.GetBusinessunitDetails(Convert.ToInt32(tblExchangeOrder.BusinessUnitId));
                                                if (tblBusinessUnit != null && tblBusinessUnit.IsSweetenerIndependent != null && tblBusinessUnit.IsSweetenerIndependent == true)
                                                {
                                                    universalPMViewModel.FinalQCPrice = universalPMViewModel.BaseValue + tblExchangeOrder.Sweetener;
                                                    universalPMViewModel.TotalSweetener = tblExchangeOrder.Sweetener;
                                                }
                                                else
                                                {
                                                    if (tblProductConditionLabel.IsSweetenerApplicable == true)
                                                    {
                                                        tblOrderBasedConfig = _orderBasedConfigRepository.GetIsSweetenerModelbase(tblExchangeOrder.BusinessUnitId, tblExchangeOrder.BusinessPartnerId);
                                                        getsweetenerDataViewModel.IsSweetenerModalBased = tblOrderBasedConfig != null ? tblOrderBasedConfig.IsSweetenerModalBased ?? false : false;
                                                        getsweetenerDataViewModel.BrandId = QCdetails.NewBrandId;
                                                        getsweetenerDataViewModel.NewProdCatId = tblProductType != null ? tblProductType.ProductCatId : null;
                                                        getsweetenerDataViewModel.NewProdTypeId = tblExchangeOrder.NewProductTypeId != null ? tblExchangeOrder.NewProductTypeId : null;
                                                        getsweetenerDataViewModel.BusinessUnitId = tblExchangeOrder.BusinessUnitId;
                                                        getsweetenerDataViewModel.BusinessPartnerId = tblExchangeOrder.BusinessPartnerId;
                                                        if (QCdetails.NewModelId > 0)
                                                        {
                                                            getsweetenerDataViewModel.ModalId = QCdetails.NewModelId;
                                                        }
                                                        else
                                                        {
                                                            getsweetenerDataViewModel.ModalId = tblExchangeOrder.ModelNumberId;
                                                        }

                                                        sweetenerDataVM = _sweetenerManager.GetSweetenerAmtExchange(getsweetenerDataViewModel);
                                                        if (sweetenerDataVM != null)
                                                        {
                                                            universalPMViewModel.FinalQCPrice = universalPMViewModel.BaseValue + sweetenerDataVM.SweetenerTotal;
                                                            universalPMViewModel.TotalSweetener = sweetenerDataVM.SweetenerTotal;
                                                            universalPMViewModel.SweetenerBU = sweetenerDataVM.SweetenerBu;
                                                            universalPMViewModel.SweetenerBP = sweetenerDataVM.SweetenerBP;
                                                            universalPMViewModel.SweetenerDigi2l = sweetenerDataVM.SweetenerDigi2L;
                                                            universalPMViewModel.ErrorMessage = sweetenerDataVM.ErrorMessage != null ? sweetenerDataVM.ErrorMessage : null;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                universalPMViewModel = new UniversalPMViewModel();
                                                universalPMViewModel.ErrorMessage = "BusinessUnitId is null in Exchange order table";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        universalPMViewModel = new UniversalPMViewModel();
                                        universalPMViewModel.ErrorMessage = "No price master data found in mapping table for this order";
                                    }
                                }
                            }
                        }
                        else
                        {
                            universalPMViewModel = new UniversalPMViewModel();
                            getsweetenerDataViewModel = new GetSweetenerDetailsDataContract();
                            if (QCdetails.NewBrandId > 0)
                            {
                                //priceMasterMappingObj = _priceMasterMappingRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == tblExchangeOrder.BusinessUnitId && x.BusinessPartnerId == tblExchangeOrder.BusinessPartnerId && x.BrandId == QCdetails.NewBrandId);
                                priceMasterMappingObj = _priceMasterMappingRepository.GetProductPriceByBUIdBPIdBrandId(tblExchangeOrder.BusinessUnitId, tblExchangeOrder.BusinessPartnerId, QCdetails.NewBrandId);
                                if (priceMasterMappingObj != null)
                                {
                                    QCdetails.PriceNameId = priceMasterMappingObj.PriceMasterNameId;

                                    tblProductConditionLabel = _productConditionLabelRepository.GetOrderSequenceNo(QCdetails.conditionId);
                                    if (tblProductConditionLabel != null)
                                    {
                                        QCdetails.FinalProdQualityId = tblProductConditionLabel.OrderSequence;
                                        QCdetails.BrandId = tblExchangeOrder.BrandId != null ? tblExchangeOrder.BrandId : null;
                                        QCdetails.ProductTypeId = tblExchangeOrder.ProductTypeId != null ? tblExchangeOrder.ProductTypeId : null;
                                        universalPMViewModel = GetBasePrice(QCdetails);
                                        if (tblExchangeOrder.BusinessUnitId != null && tblExchangeOrder.BusinessUnitId > 0)
                                        {
                                            tblBusinessUnit = _businessUnitRepository.GetBusinessunitDetails(Convert.ToInt32(tblExchangeOrder.BusinessUnitId));
                                            if (tblBusinessUnit != null && tblBusinessUnit.IsSweetenerIndependent != null && tblBusinessUnit.IsSweetenerIndependent == true)
                                            {
                                                universalPMViewModel.FinalQCPrice = universalPMViewModel.BaseValue + tblExchangeOrder.Sweetener;
                                                universalPMViewModel.TotalSweetener = tblExchangeOrder.Sweetener;
                                            }
                                            else
                                            {
                                                if (tblProductConditionLabel.IsSweetenerApplicable == true)
                                                {
                                                    tblOrderBasedConfig = _orderBasedConfigRepository.GetIsSweetenerModelbase(tblExchangeOrder.BusinessUnitId, tblExchangeOrder.BusinessPartnerId);
                                                    getsweetenerDataViewModel.IsSweetenerModalBased = tblOrderBasedConfig != null ? tblOrderBasedConfig.IsSweetenerModalBased ?? false : false;
                                                    getsweetenerDataViewModel.BrandId = QCdetails.NewBrandId;
                                                    getsweetenerDataViewModel.NewProdCatId = tblProductType != null ? tblProductType.ProductCatId : null;
                                                    getsweetenerDataViewModel.NewProdTypeId = tblExchangeOrder.NewProductTypeId != null ? tblExchangeOrder.NewProductTypeId : null;
                                                    getsweetenerDataViewModel.BusinessUnitId = tblExchangeOrder.BusinessUnitId;
                                                    getsweetenerDataViewModel.BusinessPartnerId = tblExchangeOrder.BusinessPartnerId;
                                                    if (QCdetails.NewModelId > 0)
                                                    {
                                                        getsweetenerDataViewModel.ModalId = QCdetails.NewModelId;
                                                    }
                                                    else
                                                    {
                                                        getsweetenerDataViewModel.ModalId = tblExchangeOrder.ModelNumberId;
                                                    }

                                                    sweetenerDataVM = _sweetenerManager.GetSweetenerAmtExchange(getsweetenerDataViewModel);
                                                    if (sweetenerDataVM != null)
                                                    {

                                                        universalPMViewModel.FinalQCPrice = universalPMViewModel.BaseValue + sweetenerDataVM.SweetenerTotal;
                                                        universalPMViewModel.TotalSweetener = sweetenerDataVM.SweetenerTotal;
                                                        universalPMViewModel.SweetenerBU = sweetenerDataVM.SweetenerBu;
                                                        universalPMViewModel.SweetenerBP = sweetenerDataVM.SweetenerBP;
                                                        universalPMViewModel.SweetenerDigi2l = sweetenerDataVM.SweetenerDigi2L;
                                                        if (tblExchangeOrder.ExchangePrice > universalPMViewModel.FinalQCPrice)
                                                        {
                                                            universalPMViewModel.CollectedAmount = tblExchangeOrder.ExchangePrice - universalPMViewModel.FinalQCPrice;
                                                        }
                                                        universalPMViewModel.ErrorMessage = sweetenerDataVM.ErrorMessage != null ? sweetenerDataVM.ErrorMessage : null;
                                                    }

                                                }
                                                else
                                                {
                                                    if (tblExchangeOrder.ExchangePrice > universalPMViewModel.BaseValue)
                                                    {
                                                        universalPMViewModel.CollectedAmount = tblExchangeOrder.ExchangePrice - universalPMViewModel.BaseValue;
                                                    }

                                                }
                                            }
                                        }
                                        else
                                        {
                                            universalPMViewModel.ErrorMessage = "BusinessUnitId is null in Exchange order table";
                                        }
                                    }
                                }
                                else
                                {
                                    universalPMViewModel.ErrorMessage = "No Price Master data found in mapping table for this order";
                                }
                            }
                            else
                            {
                                priceMasterMappingObj = _priceMasterMappingRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == tblExchangeOrder.BusinessUnitId && x.BusinessPartnerId == tblExchangeOrder.BusinessPartnerId && x.BrandId == null);
                                if (priceMasterMappingObj != null)
                                {
                                    QCdetails.PriceNameId = priceMasterMappingObj.PriceMasterNameId;

                                    tblProductConditionLabel = _productConditionLabelRepository.GetOrderSequenceNo(QCdetails.conditionId);
                                    if (tblProductConditionLabel != null)
                                    {
                                        QCdetails.FinalProdQualityId = tblProductConditionLabel.OrderSequence;
                                        QCdetails.BrandId = tblExchangeOrder.BrandId != null ? tblExchangeOrder.BrandId : null;
                                        QCdetails.ProductTypeId = tblExchangeOrder.ProductTypeId != null ? tblExchangeOrder.ProductTypeId : null;
                                        universalPMViewModel = GetBasePrice(QCdetails);
                                        if (tblExchangeOrder.BusinessUnitId != null && tblExchangeOrder.BusinessUnitId > 0)
                                        {
                                            tblBusinessUnit = _businessUnitRepository.GetBusinessunitDetails(Convert.ToInt32(tblExchangeOrder.BusinessUnitId));
                                            if (tblBusinessUnit != null && tblBusinessUnit.IsSweetenerIndependent != null && tblBusinessUnit.IsSweetenerIndependent == true)
                                            {
                                                universalPMViewModel.FinalQCPrice = universalPMViewModel.BaseValue + tblExchangeOrder.Sweetener;
                                                universalPMViewModel.TotalSweetener = tblExchangeOrder.Sweetener;
                                            }
                                            else
                                            {
                                                if (tblProductConditionLabel.IsSweetenerApplicable == true)
                                                {
                                                    tblOrderBasedConfig = _orderBasedConfigRepository.GetIsSweetenerModelbase(tblExchangeOrder.BusinessUnitId, tblExchangeOrder.BusinessPartnerId);
                                                    getsweetenerDataViewModel.IsSweetenerModalBased = tblOrderBasedConfig != null ? tblOrderBasedConfig.IsSweetenerModalBased ?? false : false;
                                                    getsweetenerDataViewModel.BrandId = QCdetails.NewBrandId;
                                                    getsweetenerDataViewModel.NewProdCatId = tblProductType != null ? tblProductType.ProductCatId : null;
                                                    getsweetenerDataViewModel.NewProdTypeId = tblExchangeOrder.NewProductTypeId != null ? tblExchangeOrder.NewProductTypeId : null;
                                                    getsweetenerDataViewModel.BusinessUnitId = tblExchangeOrder.BusinessUnitId;
                                                    getsweetenerDataViewModel.BusinessPartnerId = tblExchangeOrder.BusinessPartnerId;
                                                    if (QCdetails.NewModelId > 0)
                                                    {
                                                        getsweetenerDataViewModel.ModalId = QCdetails.NewModelId;
                                                    }
                                                    else
                                                    {
                                                        getsweetenerDataViewModel.ModalId = tblExchangeOrder.ModelNumberId;
                                                    }

                                                    sweetenerDataVM = _sweetenerManager.GetSweetenerAmtExchange(getsweetenerDataViewModel);
                                                    if (sweetenerDataVM != null)
                                                    {

                                                        universalPMViewModel.FinalQCPrice = universalPMViewModel.BaseValue + sweetenerDataVM.SweetenerTotal;
                                                        universalPMViewModel.TotalSweetener = sweetenerDataVM.SweetenerTotal;
                                                        universalPMViewModel.SweetenerBU = sweetenerDataVM.SweetenerBu;
                                                        universalPMViewModel.SweetenerBP = sweetenerDataVM.SweetenerBP;
                                                        universalPMViewModel.SweetenerDigi2l = sweetenerDataVM.SweetenerDigi2L;
                                                        if (tblExchangeOrder.ExchangePrice > universalPMViewModel.FinalQCPrice)
                                                        {
                                                            universalPMViewModel.CollectedAmount = tblExchangeOrder.ExchangePrice - universalPMViewModel.FinalQCPrice;
                                                        }
                                                        universalPMViewModel.ErrorMessage = sweetenerDataVM.ErrorMessage != null ? sweetenerDataVM.ErrorMessage : null;
                                                    }
                                                }
                                                else
                                                {
                                                    if (tblExchangeOrder.ExchangePrice > universalPMViewModel.BaseValue)
                                                    {

                                                        universalPMViewModel.CollectedAmount = tblExchangeOrder.ExchangePrice - universalPMViewModel.BaseValue;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            universalPMViewModel.ErrorMessage = "BusinessUnitId is null in Exchange order table";
                                        }
                                    }
                                }
                                //Code to fetch Default price master for  that  businessunit which is mapped in price master mapping table
                                else
                                {
                                    priceMasterMappingObj = _priceMasterMappingRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == null && x.BusinessUnitId == tblExchangeOrder.BusinessUnitId && x.BrandId == null);
                                    if (priceMasterMappingObj != null)
                                    {
                                        QCdetails.PriceNameId = priceMasterMappingObj.PriceMasterNameId;

                                        tblProductConditionLabel = _productConditionLabelRepository.GetOrderSequenceNo(QCdetails.conditionId);
                                        if (tblProductConditionLabel != null)
                                        {
                                            QCdetails.FinalProdQualityId = tblProductConditionLabel.OrderSequence;
                                            QCdetails.BrandId = tblExchangeOrder.BrandId != null ? tblExchangeOrder.BrandId : null;
                                            QCdetails.ProductTypeId = tblExchangeOrder.ProductTypeId != null ? tblExchangeOrder.ProductTypeId : null;
                                            universalPMViewModel = GetBasePrice(QCdetails);
                                            if (tblExchangeOrder.BusinessUnitId != null && tblExchangeOrder.BusinessUnitId > 0)
                                            {
                                                tblBusinessUnit = _businessUnitRepository.GetBusinessunitDetails(Convert.ToInt32(tblExchangeOrder.BusinessUnitId));
                                                if (tblBusinessUnit != null && tblBusinessUnit.IsSweetenerIndependent != null && tblBusinessUnit.IsSweetenerIndependent == true)
                                                {
                                                    universalPMViewModel.FinalQCPrice = universalPMViewModel.BaseValue + tblExchangeOrder.Sweetener;
                                                    universalPMViewModel.TotalSweetener = tblExchangeOrder.Sweetener;
                                                }
                                                else
                                                {
                                                    if (tblProductConditionLabel.IsSweetenerApplicable == true)
                                                    {
                                                        tblOrderBasedConfig = _orderBasedConfigRepository.GetIsSweetenerModelbase(tblExchangeOrder.BusinessUnitId, tblExchangeOrder.BusinessPartnerId);
                                                        getsweetenerDataViewModel.IsSweetenerModalBased = tblOrderBasedConfig != null ? tblOrderBasedConfig.IsSweetenerModalBased ?? false : false;
                                                        getsweetenerDataViewModel.BrandId = QCdetails.NewBrandId;
                                                        getsweetenerDataViewModel.NewProdCatId = tblProductType != null ? tblProductType.ProductCatId : null;
                                                        getsweetenerDataViewModel.NewProdTypeId = tblExchangeOrder.NewProductTypeId != null ? tblExchangeOrder.NewProductTypeId : null;
                                                        getsweetenerDataViewModel.BusinessUnitId = tblExchangeOrder.BusinessUnitId;
                                                        getsweetenerDataViewModel.BusinessPartnerId = tblExchangeOrder.BusinessPartnerId;
                                                        if (QCdetails.NewModelId > 0)
                                                        {
                                                            getsweetenerDataViewModel.ModalId = QCdetails.NewModelId;
                                                        }
                                                        else
                                                        {
                                                            getsweetenerDataViewModel.ModalId = tblExchangeOrder.ModelNumberId;
                                                        }

                                                        sweetenerDataVM = _sweetenerManager.GetSweetenerAmtExchange(getsweetenerDataViewModel);
                                                        if (sweetenerDataVM != null)
                                                        {

                                                            universalPMViewModel.FinalQCPrice = universalPMViewModel.BaseValue + sweetenerDataVM.SweetenerTotal;
                                                            universalPMViewModel.TotalSweetener = sweetenerDataVM.SweetenerTotal;
                                                            universalPMViewModel.SweetenerBU = sweetenerDataVM.SweetenerBu;
                                                            universalPMViewModel.SweetenerBP = sweetenerDataVM.SweetenerBP;
                                                            universalPMViewModel.SweetenerDigi2l = sweetenerDataVM.SweetenerDigi2L;
                                                            if (tblExchangeOrder.ExchangePrice > universalPMViewModel.FinalQCPrice)
                                                            {
                                                                universalPMViewModel.CollectedAmount = tblExchangeOrder.ExchangePrice - universalPMViewModel.FinalQCPrice;
                                                            }
                                                            universalPMViewModel.ErrorMessage = sweetenerDataVM.ErrorMessage != null ? sweetenerDataVM.ErrorMessage : null;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (tblExchangeOrder.ExchangePrice > universalPMViewModel.BaseValue)
                                                        {

                                                            universalPMViewModel.CollectedAmount = tblExchangeOrder.ExchangePrice - universalPMViewModel.BaseValue;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                universalPMViewModel.ErrorMessage = "BusinessUnitId is null in Exchange order table";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        universalPMViewModel.ErrorMessage = "No Price Master data found in Mapping Table for this Order";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        tblExchangeOrder = new TblExchangeOrder();
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetProductPrice", ex);
            }
            return universalPMViewModel;
        }

        public UniversalPMViewModel GetBasePrice(QCProductPriceDetails details)
        {
            UniversalPMViewModel? universalPMViewModel = null;
            TblBrand? tblBrand = null;
            TblUniversalPriceMaster? tblUniversalPrice = null;
            ProductConditionList? productconditionlist = null;
            TblProductConditionLabel? tblProductConditionLabel = null;
            ProductPriceViewModel? productPriceViewModel = null;
            int? qualityId = 0;
            string? productPrice = null;
            try
            {
                tblBrand = _brandRepository.GetSingle(x => x.IsActive == true && x.Id == details.BrandId);
                if (tblBrand != null)
                {
                    universalPMViewModel = new UniversalPMViewModel();
                    if (details.BrandId != Convert.ToInt32(BussinessUnitEnum.Others))
                    {
                        //tblUniversalPrice = _universalPriceMasterRepository.GetSingle(x => x.IsActive == true && x.ProductCategoryId == details.ProductCatId && x.ProductTypeId == details.ProductTypeId && x.PriceMasterNameId == details.PriceNameId);
                        tblUniversalPrice = _universalPriceMasterRepository.GetSingle(x => x.IsActive == true && x.ProductTypeId == details.ProductTypeId && x.PriceMasterNameId == details.PriceNameId);
                        if (tblUniversalPrice != null)
                        {
                            productconditionlist = new ProductConditionList();
                            productconditionlist.P_Price = tblUniversalPrice.QuotePHigh;
                            productconditionlist.Q_Price = tblUniversalPrice.QuoteQHigh;
                            productconditionlist.R_Price = tblUniversalPrice.QuoteRHigh;
                            productconditionlist.S_Price = tblUniversalPrice.QuoteSHigh;

                            qualityId = details.FinalProdQualityId;
                            switch (qualityId)
                            {
                                case 1:
                                    productPrice = productconditionlist.P_Price;
                                    break;
                                case 2:
                                    productPrice = productconditionlist.Q_Price;
                                    break;
                                case 3:
                                    productPrice = productconditionlist.R_Price;
                                    break;
                                case 4:
                                    productPrice = productconditionlist.S_Price;
                                    break;
                                default:
                                    break;
                            }
                            if (!string.IsNullOrEmpty(productPrice))
                            {
                                universalPMViewModel.BaseValue = Convert.ToDecimal(productPrice);
                            }
                            else
                            {
                                universalPMViewModel.ErrorMessage = "Price not available for this product in Price Master";
                            }

                        }
                        else
                        {
                            universalPMViewModel.ErrorMessage = "Price Master data not found";
                        }
                    }
                    else
                    {
                        //tblUniversalPrice = _universalPriceMasterRepository.GetSingle(x => x.IsActive == true && x.ProductCategoryId == details.ProductCatId && x.ProductTypeId == details.ProductTypeId && x.PriceMasterNameId == details.PriceNameId);
                        tblUniversalPrice = _universalPriceMasterRepository.GetSingle(x => x.IsActive == true && x.ProductTypeId == details.ProductTypeId && x.PriceMasterNameId == details.PriceNameId);
                        if (tblUniversalPrice != null)
                        {
                            productconditionlist = new ProductConditionList();
                            productconditionlist.P_Price = tblUniversalPrice.QuoteP;
                            productconditionlist.Q_Price = tblUniversalPrice.QuoteQ;
                            productconditionlist.R_Price = tblUniversalPrice.QuoteR;
                            productconditionlist.S_Price = tblUniversalPrice.QuoteS;
                            qualityId = details.FinalProdQualityId;
                            switch (qualityId)
                            {
                                case 1:
                                    productPrice = productconditionlist.P_Price;
                                    break;
                                case 2:
                                    productPrice = productconditionlist.Q_Price;
                                    break;
                                case 3:
                                    productPrice = productconditionlist.R_Price;
                                    break;
                                case 4:
                                    productPrice = productconditionlist.S_Price;
                                    break;
                                default:
                                    break;
                            }
                            if (!string.IsNullOrEmpty(productPrice))
                            {
                                universalPMViewModel.BaseValue = Convert.ToDecimal(productPrice);

                            }
                            else
                            {
                                universalPMViewModel.ErrorMessage = "Price not available for this Product in Price Master";
                            }

                        }
                        else
                        {
                            universalPMViewModel.ErrorMessage = "Price Master data not found";
                        }
                    }
                }
                else
                {
                    universalPMViewModel = new UniversalPMViewModel();
                    universalPMViewModel.ErrorMessage = "Brand id is not valid no brand found for this order";
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetBasePrice", ex);
            }
            return universalPMViewModel;
        }
        #endregion

        #region Method to Get ABB Voucher Details
        /// <summary>
        /// Method to Get Voucher Details
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns>VoucherDetailsViewModel</returns>
        public ABBVoucherDetailsViewModel GetABBVoucherDetails(int redemptionId)
        {

            ABBVoucherDetailsViewModel voucherDetailsViewModel = new ABBVoucherDetailsViewModel();
            try
            {
                if (redemptionId != 0)
                {
                    //need optimization
                    TblVoucherVerfication voucherVerfication = _voucherRepository.GetVoucherDataByredemptionId(redemptionId);
                    if (voucherVerfication != null)
                    {
                        if (voucherDetailsViewModel != null)
                        {
                            voucherDetailsViewModel.VoucherStatusName = voucherVerfication.VoucherStatus.VoucherStatusName;
                            voucherDetailsViewModel.VoucherCode = voucherVerfication.VoucherCode;
                            voucherDetailsViewModel.IsVoucherused = (bool)voucherVerfication.Redemption.IsVoucherUsed;
                            if (voucherVerfication.Redemption.VoucherCodeExpDate != null)
                            {
                                DateTime dt = Convert.ToDateTime(voucherDetailsViewModel.VoucherCodeExpDate);
                                voucherDetailsViewModel.VoucherCodeExpDate = dt.ToString("dd/MM/yyyy");
                            }
                            voucherDetailsViewModel.RedemptionPrice = (decimal)voucherVerfication.Redemption.RedemptionValue;
                            voucherDetailsViewModel.VoucherRedemptionby = voucherVerfication.BusinessPartner.Name + voucherVerfication.BusinessPartner.AddressLine1;
                        }
                    }
                    else
                    {
                        TblAbbredemption tblAbbredemption = _aBBRedemptionRepository.GetRedemptionData(redemptionId);
                        if (tblAbbredemption != null)
                        {
                            voucherDetailsViewModel.VoucherStatusName = tblAbbredemption.VoucherStatus?.VoucherStatusName;
                            voucherDetailsViewModel.VoucherCode = tblAbbredemption.VoucherCode;
                            voucherDetailsViewModel.IsVoucherused = (bool)tblAbbredemption.IsVoucherUsed;
                            if (tblAbbredemption.VoucherCodeExpDate != null)
                            {
                                DateTime dt = Convert.ToDateTime(tblAbbredemption.VoucherCodeExpDate);
                                voucherDetailsViewModel.VoucherCodeExpDate = dt.ToString("dd/MM/yyyy");
                            }
                            voucherDetailsViewModel.RedemptionPrice = (decimal)tblAbbredemption.RedemptionValue;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetABBVoucherDetails", ex);
            }
            return voucherDetailsViewModel;
        }
        #endregion


        #region Diagnose 2.0 Added by VK And YR

        #region Get ASP by productTypeId, TechId and brandId  Added by VK
        /// <summary>
        /// Get ASP by productTypeId, TechId and brandId
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <param name="techId"></param>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public ProdCatBrandMapViewModel GetASPV2(int productTypeId, int techId, int brandId)
        {
            decimal result = 0;
            TblPriceMasterQuestioner? tblPriceMasterQuestioner = null;
            TblProdCatBrandMapping? tblProdCatBrandMapping = null;
            ProdCatBrandMapViewModel? prodCatBrandMapVM = new ProdCatBrandMapViewModel();
            try
            {
                if (productTypeId > 0 && techId > 0 && brandId > 0)
                {
                    tblPriceMasterQuestioner = _priceMasterQuestionersRepository.GetBaseASPByTypeAndTech(productTypeId, techId);
                    if (tblPriceMasterQuestioner == null)
                    {
                        return prodCatBrandMapVM;
                    }
                    else
                    {
                        #region Map Producat Category Brand Mapping Data
                        tblProdCatBrandMapping = _prodCatBrandMappingRepository.GetProdCatBrandByBrandAndCat(brandId, tblPriceMasterQuestioner?.ProductCatId);
                        if (tblProdCatBrandMapping != null)
                        {
                            prodCatBrandMapVM = _mapper.Map<TblProdCatBrandMapping, ProdCatBrandMapViewModel>(tblProdCatBrandMapping);
                            prodCatBrandMapVM.Weightage = Convert.ToDecimal(tblProdCatBrandMapping?.BrandGroup?.Weightage ?? 0);
                        }
                        #endregion

                        #region Get and Set ASP With respect to Brand

                        result = (Convert.ToDecimal(tblPriceMasterQuestioner?.AverageSellingPrice ?? 0) * Convert.ToDecimal(tblProdCatBrandMapping?.BrandGroup?.Weightage ?? 0));
                        int numberRound = Convert.ToInt32(result);
                        result = Convert.ToDecimal(Math.Round(numberRound / 10.0) * 10);
                        prodCatBrandMapVM.FinalASP = result;
                        #endregion
                        #region Get And Set ASP With Respect to Product Percentile
                        prodCatBrandMapVM.ASPPercentile = Convert.ToDecimal(tblProdCatBrandMapping?.ProductCat?.Asppercentage ?? 0);
                        result = result * prodCatBrandMapVM.ASPPercentile / 100;
                        int NumRound = Convert.ToInt32(result);
                        result = Convert.ToDecimal(Math.Round(NumRound / 10.0) * 10);
                        #endregion

                        #region Set final ASP and Weightage
                        prodCatBrandMapVM.BaseASP = Convert.ToDecimal(tblPriceMasterQuestioner?.AverageSellingPrice);
                        prodCatBrandMapVM.ExcellentPrice = result;
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetASPV2", ex);
            }
            return prodCatBrandMapVM;
        }
        #endregion

        #region GetAgeQuestionwithLov -- YR 
        public ResponseResult GetAgeQuestionwithLov(int prodCatId)
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.message = string.Empty;
            List<QCRatingLOVDataViewModel> qCRatingLOVDataViewModel = new List<QCRatingLOVDataViewModel>();
            List<TblQcratingMaster> tblQcratingMasterList = null;
            List<TblQuestionerLov> tblQuestionerLOV = new List<TblQuestionerLov>();

            List<TblQuestionerLovmapping> tblQuestionerLovmappings = null;
            List<QuestionerLovMappingViewModel> questionerLovMappingViewModel = new List<QuestionerLovMappingViewModel>();
            List<TblQuestionerLovmapping> tblQuestionerLovmappings1 = null;
            QuestionerLovMappingViewModel? questLovMappingVM = null;

            try
            {
                if (prodCatId > 0)
                {
                    tblQcratingMasterList = _context.TblQcratingMasters
                        .Where(x => x.IsActive == true && x.ProductCatId == prodCatId && x.IsAgeingQues == true).ToList();

                    if (tblQcratingMasterList != null && tblQcratingMasterList.Count > 0)
                    {
                        qCRatingLOVDataViewModel = _mapper.Map<List<TblQcratingMaster>, List<QCRatingLOVDataViewModel>>(tblQcratingMasterList);

                        if (qCRatingLOVDataViewModel.Count > 0)
                        {
                            foreach (var ques in qCRatingLOVDataViewModel)
                            {
                                ques.questionerLovMappingViewModel = new List<QuestionerLovMappingViewModel>();

                                tblQuestionerLovmappings1 = _context.TblQuestionerLovmappings
                                    .Include(x => x.QuestionerLov)
                                    .Where(x => x.IsActive == true && x.ProductCatId == ques.ProductCatId && x.ParentId == ques.QuestionerLovid).ToList();


                                if (tblQuestionerLovmappings1 != null && tblQuestionerLovmappings1.Count > 0)
                                {
                                    foreach (var item1 in tblQuestionerLovmappings1)
                                    {
                                        questLovMappingVM = new QuestionerLovMappingViewModel();
                                        questLovMappingVM.QuestionerLovname = item1?.QuestionerLov?.QuestionerLovname;
                                        questLovMappingVM.RatingWeightageLov = item1?.RatingWeightageLov;
                                        questLovMappingVM.QuestionerLovmappingId = item1?.QuestionerLovmappingId ?? 0;
                                        questLovMappingVM.QuestionerLovid = item1?.QuestionerLovid ?? 0;
                                        ques?.questionerLovMappingViewModel?.Add(questLovMappingVM);
                                    }
                                }

                            }
                            if (qCRatingLOVDataViewModel != null && qCRatingLOVDataViewModel?.Count > 0)
                            {
                                responseResult.Data = qCRatingLOVDataViewModel;
                                responseResult.message = "Success";
                                responseResult.Status = true;
                                responseResult.Status_Code = HttpStatusCode.OK;
                                return responseResult;
                            }
                            else
                            {
                                responseResult.message = "No Success";
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.BadRequest;
                            }
                        }
                        else
                        {
                            responseResult.message = "Error occurs while mapping the data";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseResult.message = "Questiones not found";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.message = "No catid found";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("QCCommentManager", "GetAgeQuestionwithLov", ex);
            }
            return responseResult;
        }
        #endregion

        #region Get NonWorkingPrice by producttypeid and Techid exessit
        /// <summary>
        /// Get NonWorkingPrice by producttypeid and Techid
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <param name="techId"></param>
        /// <returns></returns>
        //public decimal GetNonWorkingPriceV2(int productTypeId, int techId)
        //{
        //    decimal result = 0;
        //    TblPriceMasterQuestioner? tblPriceMasterQuestioner = null;
        //    try
        //    {
        //        if (productTypeId > 0 && techId > 0)
        //        {
        //            tblPriceMasterQuestioner = _context.TblPriceMasterQuestioners.Where(x => x.IsActive == true && x.ProductTypeId == productTypeId && x.ProductTechnologyId == techId).FirstOrDefault();
        //            if (tblPriceMasterQuestioner != null)
        //            {
        //                result = Convert.ToDecimal(tblPriceMasterQuestioner.NonWorkingPrice);
        //            }
        //            else
        //            {
        //                return result;
        //            }
        //        }
        //        else
        //        {
        //            return result;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.WriteErrorToDB("QCCommentManager", "GetASP", ex);
        //    }

        //    return result;
        //}
        #endregion

        #region GetNewQuestionswithLov V2 -- YR

        public ResponseResult GetNewQuestionswithLov(int prodCatId, int prodtypeid, int prodtechid)
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.message = string.Empty;
            List<QCRatingLOVDataViewModel> qCRatingLOVDataViewModel = new List<QCRatingLOVDataViewModel>();
            List<TblQuestionerLovmapping>? tblQuestionerLovmappings = null;
            List<TblQcratingMaster>? tblQcratingMasterList = null;
            List<TblQuestionerLov> tblQuestionerLOV = new List<TblQuestionerLov>();
            List<TblQcratingMasterMapping>? tblQcratingMasterMappings = null;
            List<QcratingMasterMappingVM>? qcratingMasterMappingVMs = null;
            // QuestionerLovMappingViewModel? questLovMappingVM = null;
            try
            {
                if (prodCatId > 0 && prodtypeid > 0 && prodtechid > 0)
                {
                    // tblQcratingMasterList = _qcRatingMasterRepository.GetNewQue(prodCatId);
                    tblQcratingMasterMappings = _qcratingMasterMappingRepository.GetNewQueV2(prodCatId, prodtypeid, prodtechid);

                    if (tblQcratingMasterMappings != null && tblQcratingMasterMappings.Count > 0)
                    {
                        //qCRatingLOVDataViewModel = _mapper.Map<List<TblQcratingMaster>, List<QCRatingLOVDataViewModel>>(tblQcratingMasterList);
                        qcratingMasterMappingVMs = _mapper.Map<List<TblQcratingMasterMapping>, List<QcratingMasterMappingVM>>(tblQcratingMasterMappings);

                        if (qcratingMasterMappingVMs.Count > 0)
                        {
                            foreach (var item in qcratingMasterMappingVMs)
                            {
                                item.qCRatingLOVDataViewModels = new List<QCRatingLOVDataViewModel>();
                                tblQcratingMasterList = _context.TblQcratingMasters.
                                    Where(x => x.IsActive == true && x.QcratingId == item.QcratingId && x.ProductCatId == item.ProductCatId && x.IsAgeingQues == false && x.IsDiagnoseV2 == true).ToList();
                                item.qCRatingLOVDataViewModels = _mapper.Map<List<TblQcratingMaster>, List<QCRatingLOVDataViewModel>>(tblQcratingMasterList);

                                if (item.qCRatingLOVDataViewModels.Count > 0)
                                {
                                    foreach (var ques in item.qCRatingLOVDataViewModels)
                                    {
                                        tblQuestionerLovmappings = _context.TblQuestionerLovmappings
                                            .Include(x => x.QuestionerLov)
                                            .Where(x => x.IsActive == true && x.ProductCatId == ques.ProductCatId && x.ParentId == ques.QuestionerLovid).ToList();

                                        if (tblQuestionerLovmappings != null && tblQuestionerLovmappings.Count > 0)
                                        {
                                            ques.questionerLovMappingViewModel = new List<QuestionerLovMappingViewModel>();
                                            ques.questionerLovMappingViewModel = _mapper.Map<List<TblQuestionerLovmapping>, List<QuestionerLovMappingViewModel>>(tblQuestionerLovmappings);

                                            for (int i = 0; i < tblQuestionerLovmappings.Count; i++)
                                            {
                                                ques.questionerLovMappingViewModel[i].QuestionerLovname = tblQuestionerLovmappings[i].QuestionerLov.QuestionerLovname;
                                            }

                                        }
                                        #region Add images path for Questions
                                        string imagepath = string.Empty;

                                        if (!string.IsNullOrEmpty(ques.QuestionsImage))
                                        {
                                            imagepath = _baseConfig.Value.BaseURL + "DBFiles/Masters/QuestionsImages/" + ques.QuestionsImage;

                                            if (!string.IsNullOrEmpty(imagepath))
                                            {
                                                ques.QuestionsImage = "";
                                                ques.QuestionsImage = imagepath;
                                            }
                                            imagepath = string.Empty;
                                        }
                                        #endregion
                                    }
                                }
                            }
                            /* foreach (var ques in qCRatingLOVDataViewModel)
                             {
                                 //tblQuestionerLOV = _context.TblQuestionerLovs.Where(x => x.IsActive == true && x.QuestionerLovparentId == ques.QuestionerLovid).ToList();
                                 tblQuestionerLOV = _context.TblQuestionerLovs.Where(x => x.IsActive == true && x.QuestionerLovparentId == ques.QuestionerLovid).ToList();
                                 List<QuestionerLovidViewModel> questionerLovidViewModels = new List<QuestionerLovidViewModel>();
                                 if (tblQuestionerLOV != null && tblQuestionerLOV.Count > 0)
                                 {
                                     questionerLovidViewModels = _mapper.Map<List<TblQuestionerLov>, List<QuestionerLovidViewModel>>(tblQuestionerLOV).ToList();
                                     ques.questionerLovidViewModels = questionerLovidViewModels;
                                 }
                                 #region Add images path for Questions
                                 string imagepath = string.Empty;

                                 if (!string.IsNullOrEmpty(ques.QuestionsImage))
                                 {
                                     imagepath = _baseConfig.Value.BaseURL + "DBFiles/Masters/QuestionsImages/" + ques.QuestionsImage;

                                     if (!string.IsNullOrEmpty(imagepath))
                                     {
                                         ques.QuestionsImage = "";
                                         ques.QuestionsImage = imagepath;
                                     }
                                     imagepath = string.Empty;
                                 }
                                 #endregion
                             }*/
                            if (qcratingMasterMappingVMs.Count > 0)
                            {
                                responseResult.Data = qcratingMasterMappingVMs;
                                responseResult.message = "Success";
                                responseResult.Status = true;
                                responseResult.Status_Code = HttpStatusCode.OK;
                                return responseResult;
                            }
                            else
                            {
                                responseResult.message = "No Success";
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.BadRequest;
                            }
                        }
                        else
                        {
                            responseResult.message = "Error occurs while mapping the data";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseResult.message = "Questiones not found";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.message = "No catid found";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("QCCommentManager", "GetNewQuestionswithLov", ex);
            }
            return responseResult;
        }
        #endregion

        #region get new quoted price on selection of answer key-- YR
        /// <summary>
        /// get the quoted price on selection of answer key
        /// </summary>
        /// <param name="qCRatingViewModels"></param>
        /// <returns></returns>
        public List<double> GetNewQuotedPrice(List<QCRatingViewModel> qCRatingViewModels)
        {
            var averageSellingPrice = qCRatingViewModels[0].AverageSellingPrice;
            var sweetner = 0.00;
            var calculatedWeightage = 0.00;
            var quotedPrice = 0.00;
            var quotedPriceWithSweetner = 0.00;
            var finalPrice = 0.00;
            List<double> priceList = new List<double>();
            try
            {
                if (qCRatingViewModels != null && qCRatingViewModels.Count > 0)
                {
                    foreach (var item in qCRatingViewModels)
                    {
                        if (item.QuestionerLovid == Convert.ToInt32(QuestionerLOV.Upper_Boolen))
                        {
                            if (item.Condition == Convert.ToInt32(QuestionerLOV.Upper_Yes) || item.Condition == Convert.ToInt32(QuestionerLOV.Upper_No))
                            {
                                var Weightage = GetnewQueWeighatgeV2((int)item.Condition, item.QcratingId, item.ProductCatId);
                                calculatedWeightage += Weightage;
                            }
                        }
                        else if (item.QuestionerLovid == Convert.ToInt32(QuestionerLOV.Lower_Boolen))
                        {
                            if (item.Condition == Convert.ToInt32(QuestionerLOV.Lower_Yes) || item.Condition == Convert.ToInt32(QuestionerLOV.Lower_No))
                            {
                                var Weightage = GetnewQueWeighatgeV2((int)item.Condition, item.QcratingId, item.ProductCatId);
                                calculatedWeightage += Weightage;
                            }
                        }
                        else if (item.QuestionerLovid == Convert.ToInt32(QuestionerLOV.Numeric))
                        {
                            if (item.Condition == Convert.ToInt32(QuestionerLOV.Zero_To_One) || item.Condition == Convert.ToInt32(QuestionerLOV.Two) || item.Condition == Convert.ToInt32(QuestionerLOV.Three) || item.Condition == Convert.ToInt32(QuestionerLOV.Four) ||
                                item.Condition == Convert.ToInt32(QuestionerLOV.Five) || item.Condition == Convert.ToInt32(QuestionerLOV.Six) || item.Condition == Convert.ToInt32(QuestionerLOV.Seven) || item.Condition == Convert.ToInt32(QuestionerLOV.Eight) ||
                                item.Condition == Convert.ToInt32(QuestionerLOV.Nine) || item.Condition == Convert.ToInt32(QuestionerLOV.TenPlus))
                            {
                                var Weightage = GetnewQueWeighatgeV2((int)item.Condition, item.QcratingId, item.ProductCatId);
                                calculatedWeightage += Weightage;
                            }
                        }
                        else if (item.QuestionerLovid == Convert.ToInt32(QuestionerLOV.Upper_Range))
                        {
                            if (item.Condition == Convert.ToInt32(QuestionerLOV.Zero_Percentage) || item.Condition == Convert.ToInt32(QuestionerLOV.Upto_Fifty_Percentage) || item.Condition == Convert.ToInt32(QuestionerLOV.FiftyOne_Ninty_Percentage) || item.Condition == Convert.ToInt32(QuestionerLOV.More_Than_Ninty_Percentage))
                            {
                                var Weightage = GetnewQueWeighatgeV2((int)item.Condition, item.QcratingId, item.ProductCatId);
                                calculatedWeightage += Weightage;
                            }
                        }
                        else if (item.QuestionerLovid == Convert.ToInt32(QuestionerLOV.Lower_Range))
                        {
                            if (item.Condition == Convert.ToInt32(QuestionerLOV.Lower_Zero_Percentage) || item.Condition == Convert.ToInt32(QuestionerLOV.Upto_Ten_Percentage) || item.Condition == Convert.ToInt32(QuestionerLOV.Eleven_To_TwentyFive_Percentage) || item.Condition == Convert.ToInt32(QuestionerLOV.More_Than_TwentyFive))
                            {
                                var Weightage = GetnewQueWeighatgeV2((int)item.Condition, item.QcratingId, item.ProductCatId);
                                calculatedWeightage += Weightage;
                            }
                        }
                        else if (item.QuestionerLovid == Convert.ToInt32(QuestionerLOV.Rust_Level))
                        {
                            if (item.Condition == Convert.ToInt32(QuestionerLOV.No_Rust) || item.Condition == Convert.ToInt32(QuestionerLOV.Low_Rust) || item.Condition == Convert.ToInt32(QuestionerLOV.Heavy_Rust))
                            {
                                var Weightage = GetnewQueWeighatgeV2((int)item.Condition, item.QcratingId, item.ProductCatId);
                                calculatedWeightage += Weightage;
                            }
                        }
                    }
                }
                if (calculatedWeightage > 0 && averageSellingPrice > 0)
                {
                    if (calculatedWeightage > 0 && averageSellingPrice > 0)
                    {
                        quotedPrice = (averageSellingPrice * calculatedWeightage) / 100;
                        quotedPrice = Math.Round(quotedPrice / 10.0) * 10;
                        priceList.Add(averageSellingPrice);
                        priceList.Add(quotedPrice);
                    }
                }
                if (qCRatingViewModels[0].Sweetner > 0)
                {
                    sweetner = Convert.ToDouble(qCRatingViewModels[0].Sweetner);
                    priceList.Add(Convert.ToDouble(sweetner));
                }
                else
                {
                    priceList.Add(Convert.ToDouble(sweetner));
                }
                if (sweetner > 0)
                {
                    quotedPriceWithSweetner = quotedPrice + sweetner;
                    finalPrice = quotedPrice + sweetner;
                    priceList.Add(quotedPriceWithSweetner);
                    priceList.Add(finalPrice);
                }
                else
                {
                    quotedPriceWithSweetner = quotedPrice;
                    finalPrice = quotedPrice;
                    priceList.Add(quotedPriceWithSweetner);
                    priceList.Add(finalPrice);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetNewQuotedPrice", ex);
            }
            return priceList;
        }
        #endregion

        #region get NewQueweightage by score
        /// <summary>
        /// get weightage by score
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="qcRatingId"></param>
        /// <param name="ProductCatId"></param>
        /// <returns>calculatedWeightage</returns>
        public double GetnewQueWeighatgeV2(int condition, int qcRatingId, int ProductCatId)
        {
            var ratingWeightage = 0;
            var score = 0.00;
            var calculatedWeightage = 0.00;
            try
            {
                if (condition > 0 && qcRatingId > 0)
                {
                    TblQcratingMaster tblQcratingMaster = _context.TblQcratingMasters.Where(x => x.IsActive == true && x.QcratingId == qcRatingId).FirstOrDefault();
                    if (tblQcratingMaster != null)
                    {
                        ratingWeightage = Convert.ToInt32(tblQcratingMaster.RatingWeightage);
                    }
                    TblQuestionerLovmapping tblQuestionerLovmapping = _context.TblQuestionerLovmappings.
                        Where(x => x.IsActive == true && x.ProductCatId == ProductCatId && x.QuestionerLovid == condition).FirstOrDefault();
                    if (tblQuestionerLovmapping != null)
                    {
                        score = Convert.ToDouble(tblQuestionerLovmapping.RatingWeightageLov);
                    }
                    return calculatedWeightage = (ratingWeightage * score) / 10;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetWeighatgeV2", ex);
            }
            return calculatedWeightage;
        }
        #endregion


        #endregion

        #region Get Media Files for QC
        public string[] OnPostGetMediaFiles(string? regdNo)
        {
            string[]? files = null;
            try
            {
                string documentsPath = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"DBFiles\QC\SelfQC");
                DirectoryInfo? object1 = new DirectoryInfo(documentsPath);
                files = object1.GetFiles().Where(x => (x.Name ?? "").Contains(regdNo)).Select(x => x.Name).ToArray();
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetMediaFiles", ex);
            }
            return files;
        }


        #region get questioner report V2 for Upper level bonus cap by QC Admin
        /// <summary>
        /// get order details for Upper level bonus cap by QC Admin
        /// </summary>
        /// <param name="orderTransId"></param>
        /// <returns>adminBonusCapViewModel</returns>
        public List<QCRatingViewModel> GetQuestionerReportByQCTeamV2(int orderTransId)
        {
            List<QCRatingViewModel> qCRatingList = new List<QCRatingViewModel>();
            QCRatingViewModel qCRatingViewModel = null;
            List<TblOrderQcrating> tblOrderQcratingList = null;
            try
            {
                if (orderTransId > 0)
                {
                    tblOrderQcratingList = _context.TblOrderQcratings
                        .Include(x => x.OrderTrans).Include(x => x.Qcquestion).Include(x => x.QuestionerLov)
                        .Where(x => x.IsActive == true && x.OrderTransId == orderTransId && x.Qcquestion.IsDiagnoseV2 == true && x.DoneBy == Convert.ToInt32(LoVEnum.QCTeam)).ToList();
                    if (tblOrderQcratingList != null)
                    {
                        foreach (var item in tblOrderQcratingList)
                        {
                            qCRatingViewModel = new QCRatingViewModel();
                            qCRatingViewModel.Qcquestion = item.Qcquestion.Qcquestion;
                            if (item.QuestionerLovid != null)
                            {
                                qCRatingViewModel.Condition = (int)item.QuestionerLovid;
                                qCRatingViewModel.QuestionerLOVName = item.QuestionerLov.QuestionerLovname;
                            }
                            qCRatingViewModel.CommentByQC = item.QcComments;
                            qCRatingList.Add(qCRatingViewModel);
                        }

                        if (qCRatingList != null)
                        {
                            return qCRatingList;
                        }
                        else
                        {
                            return qCRatingList;
                        }
                    }
                    else
                    {
                        return qCRatingList;
                    }
                }
                else
                {
                    return qCRatingList;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetQuestionerReportByQCTeam", ex);
            }

            return qCRatingList;
        }
        #endregion

        // #region get all question for rating by product category id for version 2 -- yash
        /// <summary>
        /// get all question for rating by product category id
        /// </summary>
        /// <param name="prodCatId"></param>
        /// <returns></returns>
        //public List<QCRatingViewModel> GetDynamicQuestsionbyProdCatIdV2(int prodCatId)
        //{
        //    List<QCRatingViewModel> qCRatingList = new List<QCRatingViewModel>();
        //    List<TblQcratingMaster> tblQcratingMasterList = null;
        //    try
        //    {
        //        if (prodCatId > 0)
        //        {
        //            tblQcratingMasterList = _context.TblQcratingMasters.Where(x => x.IsActive == true && x.ProductCatId == prodCatId && x.IsDiagnoseV2 == true).ToList();
        //            if (tblQcratingMasterList.Count > 0 || tblQcratingMasterList != null)
        //            {
        //                qCRatingList = _mapper.Map<List<TblQcratingMaster>, List<QCRatingViewModel>>(tblQcratingMasterList);
        //                if (qCRatingList.Count > 0)
        //                {
        //                    return qCRatingList;
        //                }
        //            }
        //            else
        //            {
        //                return qCRatingList;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.WriteErrorToDB("QCCommentManager", "GetDynamicQuestionbyProdCatId", ex);
        //    }
        //    return qCRatingList;
        //}

        //public List<QCRatingViewModel> GetDynamicQuestsionbyProdCatIdV2(int? prodTypeId,int? prodTechId)
        //{
        //    List<QCRatingViewModel> qCRatingList = new List<QCRatingViewModel>();
        //    List<TblQcratingMaster>? tblQcratingMasterList = null;
        //    try
        //    {
        //        if (prodTypeId > 0 && prodTechId > 0)
        //        {
        //            tblQcratingMasterList = _qcratingMasterMappingRepository.GetNewQueListV2(prodTypeId, prodTechId);
        //            if (tblQcratingMasterList != null && tblQcratingMasterList.Count > 0)
        //            {
        //                qCRatingList = _mapper.Map<List<TblQcratingMaster>, List<QCRatingViewModel>>(tblQcratingMasterList);
        //                if (qCRatingList.Count > 0)
        //                {
        //                    return qCRatingList;
        //                }
        //            }
        //            else
        //            {
        //                return qCRatingList;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.WriteErrorToDB("QCCommentManager", "GetDynamicQuestionbyProdCatId", ex);
        //    }
        //    return qCRatingList;
        //}

        //public List<QuestionsWithLovViewModel> GetDynamicQuestsionbyProdCatIdV2(int? prodTypeId, int? prodTechId)
        //{
        //    List<TblQcratingMasterMapping>? tblQcratingMasterMappingList = null;
        //    List<QuestionsWithLovViewModel>? questionsWithLovVMList = new List<QuestionsWithLovViewModel>();
        //    QuestionsWithLovViewModel? questionsWithLovVM = null;
        //    try
        //    {
        //        if (prodTypeId > 0 && prodTechId > 0)
        //        {
        //            tblQcratingMasterMappingList = _qcratingMasterMappingRepository.GetNewQueListV2(prodTypeId, prodTechId);
        //            if (tblQcratingMasterMappingList != null && tblQcratingMasterMappingList.Count > 0)
        //            {
        //                //tblQcratingMasterList = tblQcratingMasterMappingList?.Select(x => x.Qcrating)?.ToList();
        //                //questionsWithLovVMList = _mapper.Map<List<TblQcratingMaster>, List<QuestionsWithLovViewModel>>(tblQcratingMasterList);
        //                //questionsWithLovVMList = _mapper.Map<List<TblQcratingMasterMapping>, List<QuestionsWithLovViewModel>>(tblQcratingMasterMappingList);
        //                foreach (TblQcratingMasterMapping item in tblQcratingMasterMappingList)
        //                {
        //                    if (item != null && item.Qcrating != null)
        //                    {
        //                        questionsWithLovVM = new QuestionsWithLovViewModel();
        //                        questionsWithLovVM.QcratingMasterMappingId = item.QcratingMasterMappingId;
        //                        questionsWithLovVM.QcratingId = item.QcratingId;
        //                        questionsWithLovVM.Qcquestion = item.Qcrating?.Qcquestion;
        //                        questionsWithLovVM.RatingWeightage = item.Qcrating?.RatingWeightage;
        //                        questionsWithLovVM.QuestionerLovid = item.Qcrating?.QuestionerLovid;
        //                        questionsWithLovVM.IsAgeingQues = item.Qcrating?.IsAgeingQues;
        //                        questionsWithLovVM.IsDecidingQues = item.Qcrating?.IsDecidingQues;
        //                        questionsWithLovVM.IsDiagnoseV2 = item.Qcrating?.IsDiagnoseV2;
        //                        questionsWithLovVM.ProductCatId = item.ProductCatId;
        //                        questionsWithLovVM.ProductTypeId = item.ProductTypeId;
        //                        questionsWithLovVM.ProductTechnologyId = item.ProductTechnologyId;
        //                        questionsWithLovVM.QuestsSequence = item.QuestsSequence;
        //                        questionsWithLovVM.questionerLovMappingViewModel = GetQuestionerLovListV2(item.ProductCatId, questionsWithLovVM.QuestionerLovid);
        //                        questionsWithLovVM.OptionsList = new SelectList(questionsWithLovVM.questionerLovMappingViewModel, "QuestionerLovid", "QuestionerLovname");
        //                        questionsWithLovVMList.Add(questionsWithLovVM);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.WriteErrorToDB("QCCommentManager", "GetDynamicQuestionbyProdCatId", ex);
        //    }
        //    return questionsWithLovVMList;
        //}

        public List<QCRatingViewModel> GetDynamicQuestsionbyProdCatIdV2(int? prodTypeId, int? prodTechId)
        {
            List<TblQcratingMasterMapping>? tblQcratingMasterMappingList = null;
            List<QCRatingViewModel>? qcRatingVMList = new List<QCRatingViewModel>();
            QCRatingViewModel? qcRatingVM = null;
            try
            {
                if (prodTypeId > 0 && prodTechId > 0)
                {
                    tblQcratingMasterMappingList = _qcratingMasterMappingRepository.GetNewQueListV2(prodTypeId, prodTechId);
                    if (tblQcratingMasterMappingList != null && tblQcratingMasterMappingList.Count > 0)
                    {
                        //tblQcratingMasterList = tblQcratingMasterMappingList?.Select(x => x.Qcrating)?.ToList();
                        //qcRatingVMList = _mapper.Map<List<TblQcratingMaster>, List<QuestionsWithLovViewModel>>(tblQcratingMasterList);
                        //qcRatingVMList = _mapper.Map<List<TblQcratingMasterMapping>, List<QuestionsWithLovViewModel>>(tblQcratingMasterMappingList);
                        foreach (TblQcratingMasterMapping item in tblQcratingMasterMappingList)
                        {
                            if (item != null && item.Qcrating != null)
                            {
                                qcRatingVM = new QCRatingViewModel();
                                qcRatingVM.QcratingMasterMappingId = item.QcratingMasterMappingId;
                                qcRatingVM.QcratingId = item.QcratingId ?? 0;
                                qcRatingVM.Qcquestion = item.Qcrating?.Qcquestion;
                                qcRatingVM.RatingWeightage = item.Qcrating?.RatingWeightage ?? 0;
                                qcRatingVM.QuestionerLovid = item.Qcrating?.QuestionerLovid ?? 0;
                                qcRatingVM.IsAgeingQues = item.Qcrating?.IsAgeingQues;
                                qcRatingVM.IsDecidingQues = item.Qcrating?.IsDecidingQues;
                                qcRatingVM.IsDiagnoseV2 = item.Qcrating?.IsDiagnoseV2;
                                qcRatingVM.ProductCatId = item.ProductCatId ?? 0;
                                qcRatingVM.ProductTypeId = item.ProductTypeId;
                                qcRatingVM.ProductTechnologyId = item.ProductTechnologyId;
                                qcRatingVM.QuestsSequence = item.QuestsSequence;
                                qcRatingVM.questionerLovMappingViewModel = GetQuestionerLovListV2(item.ProductCatId, qcRatingVM.QuestionerLovid);
                                qcRatingVM.OptionsList = new SelectList(qcRatingVM.questionerLovMappingViewModel, "QuestionerLovid", "QuestionerLovname");
                                qcRatingVMList.Add(qcRatingVM);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetDynamicQuestionbyProdCatId", ex);
            }
            return qcRatingVMList;
        }
        #endregion

        #region Get list of Lov by parent id and Category Id
        public List<QuestionerLovMappingViewModel> GetQuestionerLovListV2(int? prodCatId, int? lovParentId)
        {
            List<QuestionerLovMappingViewModel> questionerLovMappList = new List<QuestionerLovMappingViewModel>();
            List<TblQuestionerLovmapping>? tblQuestionerLovmappings = null;
            QuestionerLovMappingViewModel? QuestionerLovMappingVM = null;
            try
            {
                if (prodCatId > 0 && lovParentId > 0)
                {
                    tblQuestionerLovmappings = _questionerLovmappingRepository.GetQuestionerLovMapping(prodCatId, lovParentId);
                    if (tblQuestionerLovmappings != null && tblQuestionerLovmappings.Count > 0)
                    {
                        foreach (TblQuestionerLovmapping item in tblQuestionerLovmappings)
                        {
                            if (item != null && item.QuestionerLov != null)
                            {
                                QuestionerLovMappingVM = new QuestionerLovMappingViewModel();
                                QuestionerLovMappingVM.QuestionerLovmappingId = item.QuestionerLovmappingId;
                                QuestionerLovMappingVM.QuestionerLovid = item.QuestionerLovid;
                                QuestionerLovMappingVM.QuestionerLovname = item.QuestionerLov?.QuestionerLovname;
                                QuestionerLovMappingVM.RatingWeightageLov = item?.RatingWeightageLov;
                                QuestionerLovMappingVM.ParentId = item?.ParentId;
                                QuestionerLovMappingVM.ProductCatId = item.ProductCatId;
                                questionerLovMappList.Add(QuestionerLovMappingVM);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "GetQuestionerLovListV2", ex);
            }
            return questionerLovMappList;
        }
        #endregion


        #region method to save questioner's v2 with answer -- yash rathore
        /// <summary>
        /// method to save questioner's with answer
        /// </summary>
        /// <param name="qCRatingViewModelList"></param>
        /// <param name="orderTransId"></param>
        /// <returns>calculatedWeightage</returns>
        public decimal saveAnswersbyQuestionIdV2(List<QCRatingViewModel> qCRatingViewModelList, int orderTransId, int? userId, bool flag = false)
        {
            TblOrderQcrating tblOrderQcrating = null;
            TblQuestionerLovmapping? tblQuestionerLovmapping = null;
            decimal calculatedWeightage = 0.00M;
            try
            {
                if (qCRatingViewModelList.Count > 0 && orderTransId > 0)
                {
                    foreach (var item in qCRatingViewModelList)
                    {
                        tblOrderQcrating = new TblOrderQcrating();
                        tblOrderQcrating.OrderTransId = orderTransId;
                        tblOrderQcrating.ProductCatId = item.ProductCatId;
                        tblOrderQcrating.QcquestionId = item.QcratingId;
                        tblOrderQcrating.Rating = item.RatingWeightage;
                        tblQuestionerLovmapping = _context.TblQuestionerLovmappings.
                            Where(x => x.IsActive == true && x.QuestionerLovid == item.Condition && x.ProductCatId == item.ProductCatId).FirstOrDefault();
                        if (tblQuestionerLovmapping != null)
                        {
                            tblOrderQcrating.CalculatedWeightage = tblQuestionerLovmapping.RatingWeightageLov != null ? tblQuestionerLovmapping.RatingWeightageLov : 0;
                        }
                        else
                        {
                            tblOrderQcrating.CalculatedWeightage = 0;
                        }
                        tblOrderQcrating.IsActive = true;
                        if (userId > 0 || userId != null)
                        {
                            tblOrderQcrating.CreatedBy = userId;
                        }
                        else
                        {
                            tblOrderQcrating.CreatedBy = 3;
                        }
                        tblOrderQcrating.CreatedDate = DateTime.Now;
                        tblOrderQcrating.QcComments = item.CommentByQC != null ? item.CommentByQC : string.Empty;
                        tblOrderQcrating.QuestionerLovid = item.Condition;
                        if (flag == true)
                        {
                            tblOrderQcrating.DoneBy = Convert.ToInt32(LoVEnum.Customer);
                        }
                        else
                        {
                            tblOrderQcrating.DoneBy = Convert.ToInt32(LoVEnum.QCTeam);
                        }
                        _orderQCRatingRepository.Create(tblOrderQcrating);
                        calculatedWeightage += Convert.ToDecimal(tblOrderQcrating.CalculatedWeightage);
                    }
                    _orderQCRatingRepository.SaveChanges();
                    if (calculatedWeightage > 0)
                    {
                        return calculatedWeightage;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCCommentManager", "saveAnswersbyQuestionIdv2", ex);
            }
            return calculatedWeightage;
        }
        #endregion


    }


}
