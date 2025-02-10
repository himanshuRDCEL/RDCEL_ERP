using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Model.Company;
using RDCELERP.Model.AbbRegistration;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL;
using RDCELERP.DAL.IRepository;
using AutoMapper;
using RDCELERP.Common.Helper;
using RDCELERP.Model.Product;
using RDCELERP.Model.PinCode;
using RDCELERP.Model.City;
using static RDCELERP.Model.ABBRedemption.LoVViewModel;
using RDCELERP.Common.Enums;
using RDCELERP.BAL.Helper;
using Newtonsoft.Json;
using RDCELERP.Model.MobileApplicationModel;
using System.Net;
using static RDCELERP.Model.Whatsapp.WhatsappSelfqcViewModel;
using RDCELERP.Model.Base;
using Microsoft.Extensions.Options;
using RDCELERP.Common.Constant;
using RestSharp;
using RDCELERP.Model.Master;
using RDCELERP.Model.ABBRedemption;
using RDCELERP.Model.BusinessPartner;
using Microsoft.AspNetCore.Hosting;
using RDCELERP.Model.QCComment;
using RDCELERP.Model.TemplateConfiguration;
using NPOI.HPSF;
using DocumentFormat.OpenXml.Spreadsheet;
using NPOI.SS.Formula.Functions;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.LGC;
using DocumentFormat.OpenXml.Wordprocessing;

namespace RDCELERP.BAL.MasterManager
{
    public class AbbRegistrationManager : IAbbRegistrationManager
    {
        #region  Variable Declaration
        IAbbRegistrationRepository _abbRegistrationRepository;
        IUserRepository _userRepository;
        IUserRoleRepository _userRoleRepository;
        IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        ILogging _logging;        
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IProductTypeRepository _productTypeRepository;
        IPinCodeRepository _pinCodeRepository;
        IExchangeABBStatusHistoryRepository _exchangeABBStatusHistoryRepository;
        IOrderTransRepository _orderTransRepository;
        ILovRepository _lovRepository;
        IImageHelper _imageHelper;        
        IModelNumberRepository _modelNumberRepository;
        ILoginRepository _loginRepository;
        IBusinessUnitRepository _businessUnitRepository;
        IBusinessPartnerRepository _BusinessPartnerRepository;
        ICustomerDetailsRepository _customerDetailsRepository;
        IBrandRepository _brandRepository;
        IProductCategoryRepository _productCategoryRepository;
        IExchangeOrderManager _exchangeOrderManager;
        IOptions<ApplicationSettings> _config;
        IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _WhatsAppMessageRepository;
        IABBPlanMasterRepository _aBBPlanMasterRepository;
        IBrandSmartBuyRepository _brandSmartBuyRepository;
        IWebHostEnvironment _webHostEnvironment;
        IHtmlToPDFConverterHelper _htmlToPDFConverterHelper;
        IMailManager _mailManager;
        ICommonManager _commonManager;
        ICustomerFilesRepository _customerFilesRepository;
        ITemplateConfigurationRepository _templateConfigurationRepository;
        #endregion

        #region Constructor
        public AbbRegistrationManager(IProductTypeRepository productTypeRepository, IAbbRegistrationRepository abbRegistrationRepository, IUserRoleRepository userRoleRepository, IRoleRepository roleRepository, IMapper mapper, ILogging logging, IPinCodeRepository pinCodeRepository, IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository, IOrderTransRepository orderTransRepository, ILovRepository lovRepository, IImageHelper imageHelper, IModelNumberRepository modelNumberRepository, ILoginRepository loginRepository, IBusinessUnitRepository businessUnitRepository, IBusinessPartnerRepository businessPartnerRepository, ICustomerDetailsRepository customerDetailsRepository, IBrandRepository brandRepository, IProductCategoryRepository productCategoryRepository, IExchangeOrderManager exchangeOrderManager, IWhatsappNotificationManager whatsappNotificationManager, IWhatsAppMessageRepository whatsAppMessageRepository, IABBPlanMasterRepository aBBPlanMasterRepository, IBrandSmartBuyRepository brandSmartBuyRepository, IWebHostEnvironment webHostEnvironment, IHtmlToPDFConverterHelper htmlToPDFConverterHelper, IMailManager mailManager, ICommonManager commonManager, IOptions<ApplicationSettings> config, ICustomerFilesRepository customerFilesRepository, ITemplateConfigurationRepository templateConfigurationRepository)
        {
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logging = logging;
            _abbRegistrationRepository = abbRegistrationRepository;
            _productTypeRepository = productTypeRepository;
            _pinCodeRepository = pinCodeRepository;
            _exchangeABBStatusHistoryRepository = exchangeABBStatusHistoryRepository;
            _orderTransRepository = orderTransRepository;
            _lovRepository = lovRepository;
            _imageHelper = imageHelper;
            _modelNumberRepository = modelNumberRepository;
            _loginRepository = loginRepository;
            _businessUnitRepository = businessUnitRepository;
            _BusinessPartnerRepository = businessPartnerRepository;
            _customerDetailsRepository = customerDetailsRepository;
            _brandRepository = brandRepository;
            _productCategoryRepository = productCategoryRepository;
            _exchangeOrderManager = exchangeOrderManager;
            _whatsappNotificationManager = whatsappNotificationManager;
            _WhatsAppMessageRepository = whatsAppMessageRepository;
            _aBBPlanMasterRepository = aBBPlanMasterRepository;
            _brandSmartBuyRepository = brandSmartBuyRepository;
            _webHostEnvironment = webHostEnvironment;
            _htmlToPDFConverterHelper = htmlToPDFConverterHelper;
            _mailManager = mailManager;
            _commonManager = commonManager;
            _config = config;
            _customerFilesRepository = customerFilesRepository;
            _templateConfigurationRepository = templateConfigurationRepository;
        }
        #endregion       

        #region Get All ABB Registration Details
        /// <summary>
        ///  Get All ABB Regsitration Details
        /// </summary>
        /// <returns>AbbRegistrationModelList</returns>
        public IList<AbbRegistrationModel> GetAllABBRegistrationDetails()
        {
            IList<AbbRegistrationModel> AbbRegistrationModelList = null;
            List<TblAbbregistration> tblABBRegistrationlist = new List<TblAbbregistration>();
            // TblUseRole TblUseRole = null;
            try
            {
                tblABBRegistrationlist = _abbRegistrationRepository.GetList(x => x.IsActive == true).ToList();                

                if (tblABBRegistrationlist != null && tblABBRegistrationlist.Count > 0)
                {
                    AbbRegistrationModelList = _mapper.Map<IList<TblAbbregistration>, IList<AbbRegistrationModel>>(tblABBRegistrationlist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "GetAllABBRegistrationDetails", ex);
            }
            return AbbRegistrationModelList;
        }
        #endregion


        #region Method to Bulk Upload (Add/Edit) ABB Registration
        /// <summary>
        /// Method to Bulk Upload (Add/Edit) ABB Registration
        /// </summary>
        /// <param name="AbbVM">AbbVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public ABBRegistrationViewModel ManageABBBulk(ABBRegistrationViewModel AbbVM, int userId)
        {

            if (AbbVM != null && AbbVM.AbbVMList != null && AbbVM.AbbVMList.Count > 0)
            {
                var ValidatedAbbList = AbbVM.AbbVMList.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
                AbbVM.AbbVMErrorList = AbbVM.AbbVMList.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();
                if (ValidatedAbbList != null && ValidatedAbbList.Count > 0)
                {
                    foreach (var item in ValidatedAbbList)
                    {
                        try
                        {
                            if (item.AbbregistrationId > 0)
                            {

                                TblCustomerDetail tblCustomerDetail = new TblCustomerDetail();
                                //Code to update the object 
                                tblCustomerDetail.FirstName = item.CustFirstName;
                                tblCustomerDetail.LastName = item.CustLastName;
                                tblCustomerDetail.PhoneNumber = item.CustMobile;
                                tblCustomerDetail.City = item.CustCity;
                                tblCustomerDetail.Email = item.CustEmail;
                                tblCustomerDetail.State = item.CustState;
                                tblCustomerDetail.ZipCode = item.CustPinCode;
                                tblCustomerDetail.Address1 = item.CustAddress1;
                                tblCustomerDetail.Address2 = item.CustAddress2;
                                tblCustomerDetail.ModifiedDate = _currentDatetime;
                                tblCustomerDetail.ModifiedBy = userId;
                                _customerDetailsRepository.Update(tblCustomerDetail);
                                _customerDetailsRepository.SaveChanges();

                                TblAbbregistration TblAbbregistration = new TblAbbregistration();
                                //Code to update the object 
                                TblAbbregistration.CustFirstName = item.CustFirstName;
                                TblAbbregistration.CustLastName = item.CustLastName;
                                TblAbbregistration.CustMobile = item.CustMobile;
                                TblAbbregistration.CustCity = item.CustCity;
                                TblAbbregistration.CustEmail = item.CustEmail;
                                TblAbbregistration.CustState = item.CustState;
                                TblAbbregistration.CustPinCode = item.CustPinCode;
                                TblAbbregistration.CustAddress1 = item.CustAddress1;
                                TblAbbregistration.CustAddress2 = item.CustAddress2;
                                TblAbbregistration.CustomerId = tblCustomerDetail.Id;
                                var ProductType = _productTypeRepository.GetSingle(x => x.Description + x.Size == item.NewProductType);
                                TblAbbregistration.NewProductCategoryTypeId = ProductType.Id;
                                TblAbbregistration.NewProductCategoryType = item.NewProductType;

                                var Brand = _brandRepository.GetSingle(x => x.Name == item.Brand);
                                TblAbbregistration.NewBrandId = Brand.Id;
                                TblAbbregistration.Location = item.Location;
                                var BusinessPartner = _BusinessPartnerRepository.GetSingle(x => x.StoreCode == item.StoreCode);
                                if (BusinessPartner != null)
                                {
                                    TblAbbregistration.BusinessPartnerId = BusinessPartner.BusinessPartnerId;
                                }
                                TblAbbregistration.Location = item.Location;
                                TblAbbregistration.AbbplanName = item.AbbplanName;
                                TblAbbregistration.StoreCode = item.StoreCode;
                                TblAbbregistration.StoreName = item.StoreName;
                                TblAbbregistration.StoreManagerEmail = item.StoreManagerEmail;
                                TblAbbregistration.SponsorOrderNo = item.SponsorOrderNo;
                                TblAbbregistration.AbbplanPeriod = item.AbbplanPeriod;
                                TblAbbregistration.Abbfees = item.Abbfees;
                                TblAbbregistration.NewSize = item.NewSize;
                                TblAbbregistration.InvoiceNo = item.InvoiceNo;
                                TblAbbregistration.InvoiceDate = item.InvoiceDate;
                                TblAbbregistration.ProductSrNo = item.ProductSrNo;
                                TblAbbregistration.ProductNetPrice = item.ProductNetPrice;
                                TblAbbregistration.NoOfClaimPeriod = item.NoOfClaimPeriod;
                                var ModelNumber = _modelNumberRepository.GetSingle(x => x.ModelName == item.NewModelName);
                                if (ModelNumber != null)
                                {
                                    TblAbbregistration.ModelNumberId = ModelNumber.ModelNumberId;
                                }

                                var ProductCategory = _productCategoryRepository.GetSingle(x => x.Description == item.NewProductCategoryName);
                                TblAbbregistration.NewProductCategoryId = ProductCategory.Id;
                                TblAbbregistration.NewProductCategoryName = item.NewProductCategoryName;


                                var BusinessUnit = _businessUnitRepository.GetSingle(x => x.Name == item.CompanyName);
                                TblAbbregistration.BusinessUnitId = BusinessUnit.BusinessUnitId;


                                TblAbbregistration.ModifiedDate = _currentDatetime;
                                TblAbbregistration.ModifiedBy = userId;
                                _abbRegistrationRepository.Update(TblAbbregistration);
                                _abbRegistrationRepository.SaveChanges();


                            }
                            else
                            {
                                TblCustomerDetail tblCustomerDetail = new TblCustomerDetail();
                                //Code to update the object 
                                tblCustomerDetail.FirstName = item.CustFirstName;
                                tblCustomerDetail.LastName = item.CustLastName;
                                tblCustomerDetail.PhoneNumber = item.CustMobile;
                                tblCustomerDetail.City = item.CustCity;
                                tblCustomerDetail.Email = item.CustEmail;
                                tblCustomerDetail.State = item.CustState;
                                tblCustomerDetail.ZipCode = item.CustPinCode;
                                tblCustomerDetail.Address1 = item.CustAddress1;
                                tblCustomerDetail.Address2 = item.CustAddress2;
                                tblCustomerDetail.ModifiedDate = _currentDatetime;
                                tblCustomerDetail.ModifiedBy = userId;
                                _customerDetailsRepository.Create(tblCustomerDetail);
                                _customerDetailsRepository.SaveChanges();

                                TblAbbregistration TblAbbregistration = new TblAbbregistration();
                                //Code to update the object 
                                TblAbbregistration.RegdNo = "A" + UniqueString.RandomNumberByLength(7);
                                TblAbbregistration.CustFirstName = item.CustFirstName;
                                TblAbbregistration.CustLastName = item.CustLastName;
                                TblAbbregistration.CustMobile = item.CustMobile;
                                TblAbbregistration.CustCity = item.CustCity;
                                TblAbbregistration.CustState = item.CustState;
                                TblAbbregistration.CustEmail = item.CustEmail;
                                TblAbbregistration.CustPinCode = item.CustPinCode;
                                TblAbbregistration.CustAddress1 = item.CustAddress1;
                                TblAbbregistration.CustAddress2 = item.CustAddress2;
                                TblAbbregistration.CustomerId = tblCustomerDetail.Id;
                                var ProductType = _productTypeRepository.GetSingle(x => x.Description + x.Size == item.NewProductType);
                                TblAbbregistration.NewProductCategoryTypeId = ProductType.Id;
                                TblAbbregistration.NewProductCategoryType = item.NewProductType;

                                var Brand = _brandRepository.GetSingle(x => x.Name == item.Brand);
                                TblAbbregistration.NewBrandId = Brand.Id;
                                TblAbbregistration.Location = item.Location;
                                var BusinessPartner = _BusinessPartnerRepository.GetSingle(x => x.StoreCode == item.StoreCode);
                                if (BusinessPartner != null)
                                {
                                    TblAbbregistration.BusinessPartnerId = BusinessPartner.BusinessPartnerId;
                                }
                                TblAbbregistration.Location = item.Location;
                                TblAbbregistration.AbbplanName = item.AbbplanName;
                                TblAbbregistration.StoreCode = item.StoreCode;
                                TblAbbregistration.StoreName = item.StoreName;
                                TblAbbregistration.StoreManagerEmail = item.StoreManagerEmail;
                                TblAbbregistration.SponsorOrderNo = item.SponsorOrderNo;
                                TblAbbregistration.AbbplanPeriod = item.AbbplanPeriod;
                                TblAbbregistration.Abbfees = item.Abbfees;
                                TblAbbregistration.NewSize = item.NewSize;
                                TblAbbregistration.InvoiceNo = item.InvoiceNo;
                                TblAbbregistration.InvoiceDate = item.InvoiceDate;
                                TblAbbregistration.ProductSrNo = item.ProductSrNo;
                                TblAbbregistration.ProductNetPrice = item.ProductNetPrice;
                                TblAbbregistration.NoOfClaimPeriod = item.NoOfClaimPeriod;
                                var ModelNumber = _modelNumberRepository.GetSingle(x => x.ModelName == item.NewModelName);
                                if (ModelNumber != null)
                                {
                                    TblAbbregistration.ModelNumberId = ModelNumber.ModelNumberId;
                                }

                                var ProductCategory = _productCategoryRepository.GetSingle(x => x.Description == item.NewProductCategoryName);
                                TblAbbregistration.NewProductCategoryId = ProductCategory.Id;
                                TblAbbregistration.NewProductCategoryName = item.NewProductCategoryName;


                                var BusinessUnit = _businessUnitRepository.GetSingle(x => x.Name == item.CompanyName);
                                TblAbbregistration.BusinessUnitId = BusinessUnit.BusinessUnitId;


                                TblAbbregistration.CreatedDate = _currentDatetime;
                                TblAbbregistration.CreatedBy = userId;
                                TblAbbregistration.ModifiedDate = _currentDatetime;
                                TblAbbregistration.ModifiedBy = userId;
                                TblAbbregistration.IsActive = true;
                                _abbRegistrationRepository.Create(TblAbbregistration);
                                _abbRegistrationRepository.SaveChanges();


                            }
                        }

                        catch (Exception ex)
                        {
                            item.Remarks += ex.Message + ", ";
                            AbbVM.AbbVMList.Add(item);
                        }
                    }
                }
            }

            return AbbVM;
        }
        #endregion


        #region Get All ABB Regd No 
        /// <summary>
        ///  Get All ABB Regsitration Details
        /// </summary>
        /// <returns>AbbRegistrationModelList</returns>
        public AbbRegistrationModel GetAllRegdNoList()
        {
            AbbRegistrationModel AbbRegistrationModel = null;
            TblAbbregistration tblABBRegistration= null;
            
            try
            {
                tblABBRegistration= _abbRegistrationRepository.GetAllRegdno();

                if (tblABBRegistration != null)
                {
                    AbbRegistrationModel = _mapper.Map<TblAbbregistration, AbbRegistrationModel>(tblABBRegistration);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "GetAllABBRegistrationDetails", ex);
            }
            return AbbRegistrationModel;
        }
        #endregion

        #region Method to get single ABB Registraion Detail by id

        /// <summary>
        /// Method to get single ABB Registraion Detail
        /// </summary>
        /// <param name="id">ABBRegistrationId</param>
        /// <returns>AbbRegistrationVM</returns>
        public AbbRegistrationModel GetABBRegistrationId(int id)
        {
            AbbRegistrationModel AbbRegistrationVM = null;
            TblAbbregistration TblABBRegistration = null;
            try
            {
                TblABBRegistration = _abbRegistrationRepository.GetSingleOrder(id);
                if (TblABBRegistration != null)
                {
                    AbbRegistrationVM = _mapper.Map<TblAbbregistration, AbbRegistrationModel>(TblABBRegistration);
                    if (AbbRegistrationVM.NewBrandId > 0)
                    {
                        TblBusinessUnit tblBuID = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == AbbRegistrationVM.BusinessUnitId);
                        if (tblBuID != null && tblBuID.IsBumultiBrand==true)
                        {
                            TblBrandSmartBuy tblBrandSmart = _brandSmartBuyRepository.GetSingle(x => x.IsActive == true && x.Id == AbbRegistrationVM.NewBrandId);
                            if(tblBrandSmart!=null)
                                AbbRegistrationVM.BrandName = string.IsNullOrEmpty(tblBrandSmart.Name) ? string.Empty : tblBrandSmart.Name.ToString();

                        }
                        else
                        {
                            TblBrand tblBrand = _brandRepository.GetSingle(x => x.IsActive == true && x.Id == AbbRegistrationVM.NewBrandId);
                            if (tblBrand != null)
                            {
                                AbbRegistrationVM.BrandName = string.IsNullOrEmpty(tblBrand.Name) ? string.Empty : tblBrand.Name.ToString();
                            }
                        }
                        
                    }
                    if (AbbRegistrationVM.NewProductCategoryId > 0)
                    {
                        TblProductCategory productCategory = _productCategoryRepository.GetSingle(x => x.IsActive == true && x.Id == AbbRegistrationVM.NewProductCategoryId);
                        if (productCategory != null)
                        {
                            AbbRegistrationVM.NewProductCategoryName = string.IsNullOrEmpty(productCategory.Description) ? string.Empty : productCategory.Description.ToString();
                        }
                    }
                    if (AbbRegistrationVM.NewProductCategoryTypeId > 0)
                    {
                        TblProductType productType = _productTypeRepository.GetSingle(x => x.IsActive == true && x.Id == AbbRegistrationVM.NewProductCategoryTypeId);
                        if (productType != null)
                        {
                            AbbRegistrationVM.NewProductCategoryType = string.IsNullOrEmpty(productType.Description) ? string.Empty : productType.Description.ToString();
                        }
                    }
                    if (AbbRegistrationVM.BusinessPartnerId > 0)
                    {
                        TblBusinessPartner businessPartner = _BusinessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == AbbRegistrationVM.BusinessPartnerId);
                        if (businessPartner != null)
                        {
                            AbbRegistrationVM.StoreCode = string.IsNullOrEmpty(businessPartner.StoreCode) ? string.Empty : businessPartner.StoreCode.ToString();
                            AbbRegistrationVM.StoreName = string.IsNullOrEmpty(businessPartner.Name) ? string.Empty : businessPartner.Name.ToString();
                            AbbRegistrationVM.StoreManagerEmail = string.IsNullOrEmpty(businessPartner.Email) ? string.Empty : businessPartner.Email.ToString();
                        }
                    }
                    if (AbbRegistrationVM.ModelNumberId > 0)
                    {
                        TblModelNumber tblModelNumber = _modelNumberRepository.GetSingle(x => x.IsActive == true && x.ModelNumberId == AbbRegistrationVM.ModelNumberId);
                        if (tblModelNumber != null)
                        {
                            AbbRegistrationVM.ModelName = string.IsNullOrEmpty(tblModelNumber.ModelName) ? string.Empty : tblModelNumber.ModelName.ToString();
                        }
                    }
                }
                else
                {
                    AbbRegistrationVM = new AbbRegistrationModel();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "GetABBRegistrationId", ex);
            }
            return AbbRegistrationVM;

        }
        #endregion

        #region Method to save ABB Registration Details
        /// <summary>
        ///  save ABB Registration Details
        /// </summary>
        /// <param name="abbRegistrationModel"></param>
        /// <returns>AbbregistrationId</returns>
        public int SaveABBRegDetails(AbbRegistrationModel abbRegistrationModel)
        {
            TblAbbregistration TblAbbregistration = new TblAbbregistration();
            TblLoV tblLoV = new TblLoV();
            string ordertype = "ABB";
            bool imgSave = false;
            try
            {
                if (abbRegistrationModel != null)
                {

                    TblAbbregistration = _mapper.Map<AbbRegistrationModel, TblAbbregistration>(abbRegistrationModel);


                    if (TblAbbregistration.AbbregistrationId > 0)
                    {
                        //Code to update the object
                        #region Upload Video Qc images In Folder and DB
                        tblLoV = _lovRepository.GetSingle(x => x.IsActive == true && x.LoVname.ToLower().Trim() == ordertype.ToLower().Trim());
                        if (abbRegistrationModel.Base64StringValue != null)
                        {
                            abbRegistrationModel.FileName = abbRegistrationModel.RegdNo + "_" + "FinalABBImage" + ".jpg";
                            string filePath = EnumHelper.DescriptionAttr(FilePathEnum.ABB);
                            imgSave = _imageHelper.SaveFileFromBase64(abbRegistrationModel.Base64StringValue, filePath, abbRegistrationModel.FileName);
                            TblAbbregistration.UploadImage = abbRegistrationModel.FileName;
                        }
                        #endregion
                        #region bp update
                        if (TblAbbregistration.StoreCode != null)
                        {
                            TblBusinessPartner businessPartner = _BusinessPartnerRepository.GetSingle(x => x.IsActive == true && x.StoreCode == TblAbbregistration.StoreCode);
                            if (businessPartner != null && businessPartner.BusinessPartnerId != TblAbbregistration.BusinessPartnerId)
                            {

                                TblAbbregistration.BusinessPartnerId = businessPartner.BusinessPartnerId;
                                TblAbbregistration.StoreCode = string.IsNullOrEmpty(businessPartner.StoreCode) ? string.Empty : businessPartner.StoreCode.ToString();
                                TblAbbregistration.StoreName = string.IsNullOrEmpty(businessPartner.Name) ? string.Empty : businessPartner.Name.ToString();
                                TblAbbregistration.StoreManagerEmail = string.IsNullOrEmpty(businessPartner.Email) ? string.Empty : businessPartner.Email.ToString();
                            }
                        }
                        #endregion

                        #region update name for product cat and type
                        if(TblAbbregistration!=null && TblAbbregistration.NewProductCategoryId > 0)
                        {
                            TblProductCategory tblProductCategory = _productCategoryRepository.GetSingle(x => x.IsActive == true && x.Id ==Convert.ToInt32( TblAbbregistration.NewProductCategoryId));
                            TblAbbregistration.NewProductCategoryName = tblProductCategory!=null ? tblProductCategory.Description : string.Empty;
                        }
                        if (TblAbbregistration != null && TblAbbregistration.NewProductCategoryTypeId > 0)
                        {
                            TblProductType tblProductCategorytype = _productTypeRepository.GetSingle(x => x.IsActive == true && x.Id ==Convert.ToInt32( TblAbbregistration.NewProductCategoryTypeId));
                            TblAbbregistration.NewProductCategoryType = tblProductCategorytype != null ? tblProductCategorytype.Description : string.Empty;
                        }
                        #endregion
                        //TblAbbregistration.StoreCode = abbRegistrationModel.StoreCode!=null? abbRegistrationModel.StoreCode:string.Empty;
                        //TblAbbregistration.StoreManagerEmail = abbRegistrationModel.StoreManagerEmail!=null? abbRegistrationModel.StoreManagerEmail:string.Empty;
                        //TblAbbregistration.StoreName = abbRegistrationModel.StoreName!=null? abbRegistrationModel.StoreName:string.Empty;
                        ////TblAbbregistration.ModifiedDate = _currentDatetime;
                        TblAbbregistration.ModifiedDate = _currentDatetime;
                        _abbRegistrationRepository.Update(TblAbbregistration);
                        #region Customer details Update
                        if (TblAbbregistration.CustomerId > 0)
                        {
                            TblCustomerDetail tblCustomerDetail = _customerDetailsRepository.GetSingle(x => x.IsActive == true && x.Id == TblAbbregistration.CustomerId);
                            if (tblCustomerDetail != null)
                            {
                                tblCustomerDetail.FirstName = TblAbbregistration.CustFirstName != string.Empty ? TblAbbregistration.CustFirstName : string.Empty;
                                tblCustomerDetail.LastName = TblAbbregistration.CustLastName != string.Empty ? TblAbbregistration.CustLastName : string.Empty;
                                tblCustomerDetail.PhoneNumber = TblAbbregistration.CustMobile != string.Empty ? TblAbbregistration.CustMobile : string.Empty;
                                tblCustomerDetail.Email = TblAbbregistration.CustEmail != string.Empty ? TblAbbregistration.CustEmail : string.Empty;
                                tblCustomerDetail.Address1 = TblAbbregistration.CustAddress1 != string.Empty ? TblAbbregistration.CustAddress1 : string.Empty;
                                tblCustomerDetail.Address2 = TblAbbregistration.CustAddress2 != string.Empty ? TblAbbregistration.CustAddress2 : string.Empty;
                                tblCustomerDetail.ZipCode = TblAbbregistration.CustPinCode != string.Empty ? TblAbbregistration.CustPinCode : string.Empty;
                                tblCustomerDetail.City = TblAbbregistration.CustCity != string.Empty ? TblAbbregistration.CustCity : string.Empty;
                                _customerDetailsRepository.Update(tblCustomerDetail);
                                _customerDetailsRepository.SaveChanges();

                            }
                        }
                        #endregion
                    }
                    else
                    {
                        //Code to Insert the object 
                        TblAbbregistration.IsActive = true;
                        TblAbbregistration.CreatedDate = _currentDatetime;
                        _abbRegistrationRepository.Create(TblAbbregistration);
                    }
                    _abbRegistrationRepository.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "ManageUser", ex);
            }
            return TblAbbregistration.AbbregistrationId;
        }
        #endregion

        #region  Method to get customer Details by mobile no.
        /// <summary>
        ///  get customer Details by mobile no.
        /// </summary>
        /// <param name="custmob"></param>
        /// <returns>AbbRegistrationVM</returns>
        public AbbRegistrationModel GetCustDetailsByMob(string custmob)
        {
            AbbRegistrationModel AbbRegistrationVM = null;
            TblAbbregistration TblABBRegistration = null;

            try
            {
                TblABBRegistration = _abbRegistrationRepository.GetSingle(where: x => x.IsActive == true && x.CustMobile == custmob);
                if (TblABBRegistration != null)
                {
                    AbbRegistrationVM = _mapper.Map<TblAbbregistration, AbbRegistrationModel>(TblABBRegistration);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "GetCustDetailsByMob", ex);
            }
            return AbbRegistrationVM;
        }
        #endregion

        #region Get Store manage E-mail on the basis of storecode
        /// <summary>
        /// Get E-mail on the basis of storecode
        /// </summary>
        /// <param name="storecode"></param>
        /// <returns>AbbRegistrationModel</returns>
        public BusinessPartnerViewModel GetStoremanageemail(string storecode)
        {
            //AbbRegistrationModel AbbRegistrationVM = new AbbRegistrationModel(); ;
            BusinessPartnerViewModel businessPartnerViewModel = null;
            //TblAbbregistration TblABBRegistration = null;
            TblBusinessPartner tblBusinessPartner = null;

            try
            {
                //TblABBRegistration = _abbRegistrationRepository.GetSingle(where: x => x.IsActive == true && x.BusinessPartnerId== storecode);
                tblBusinessPartner = _BusinessPartnerRepository.GetSingle(where: x => x.IsActive == true && x.StoreCode == storecode);
                if (tblBusinessPartner != null)
                {
                    businessPartnerViewModel = _mapper.Map<TblBusinessPartner, BusinessPartnerViewModel>(tblBusinessPartner);

                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "GetStoremanageemail", ex);
            }
            return businessPartnerViewModel;
        }
        #endregion

        #region Get Product type on the basis of product category
        /// <summary>
        /// Get ProductType By category
        /// </summary>
        /// <param name="NewProductCategoryId"></param>
        /// <returns></returns>
        public IList<ProductTypeViewModel> GetProductTypeBycategory(int NewProductCategoryId)
        {
            IList<ProductTypeViewModel> productTypeList = null;
            IList<ProductTypeNameDescription> productTypeNameDescription = new List<ProductTypeNameDescription>();
            List<TblProductType> tblProductTypesList = new List<TblProductType>();
            List<ProductTypeViewModel> uniqueModels = null;
            try
            {
                tblProductTypesList = _productTypeRepository.GetList(x => x.IsActive == true && x.ProductCatId == NewProductCategoryId).ToList();
                
                if(tblProductTypesList!=null && tblProductTypesList.Count > 0)
                {
                    productTypeList = _mapper.Map<IList<TblProductType>, IList<ProductTypeViewModel>>(tblProductTypesList);

                    uniqueModels = productTypeList.GroupBy(s => s.Description).Select(g => g.First()).ToList();
                }               
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "GetProductTypeBycategory", ex);
            }
            return uniqueModels;
        }
        #endregion

        #region Get State by Pincode
        /// <summary>
        /// GetStatebyPincode
        /// </summary>
        /// <param name="CustPinCode"></param>
        /// <returns>pincodestateList</returns>
        public IList<PinCodeViewModel> GetStatebyPincode(int CustPinCode)
        {
            IList<PinCodeViewModel> pincodestateList = null;
            List<TblPinCode> tblpincodeList = new List<TblPinCode>();

            try
            {
                tblpincodeList = _pinCodeRepository.GetList(x => x.IsActive == true && x.ZipCode == CustPinCode).ToList();
                if (tblpincodeList != null && tblpincodeList.Count > 0)
                {
                    pincodestateList = _mapper.Map<IList<TblPinCode>, IList<PinCodeViewModel>>(tblpincodeList);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "GetStatebyPincode", ex);
            }
            return pincodestateList;
        }
        #endregion

        #region pincode on city basis
        //public IList<PinCodeViewModel> GetPincodeonselectedCity(string CustCity)
        //{
        //    IList<PinCodeViewModel> pinCodesList = null;
        //    List<TblPinCode> tblPinCodesList = new List<TblPinCode>();

        //    try
        //    {
        //        tblPinCodesList = _pinCodeRepository.GetList(x => x.IsActive == true && x.Location == CustCity).ToList();
        //        if (tblPinCodesList != null && tblPinCodesList.Count > 0)
        //        {
        //            pinCodesList = _mapper.Map<IList<TblPinCode>, IList<PinCodeViewModel>>(tblPinCodesList);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.WriteErrorToDB("AbbRegistrationManager", "GetPincodeonselectedCity", ex);
        //    }
        //    return pinCodesList;
        //}
        #endregion

        #region Autocomplete for RegdNo
        /// <summary>
        /// Autocomplete for RegdNo
        /// </summary>
        /// <param name="regdNum"></param>
        /// <returns>tblAbbregistrations Regd no list</returns>
        public List<TblAbbregistration> GetRnoAutoComplete(string regdNum)
        {
            List<TblAbbregistration> tblAbbregistrations = null;
            try
            {
                if (regdNum != null)
                {
                    tblAbbregistrations = _abbRegistrationRepository.
                   GetList(x => x.IsActive == true && (x.RegdNo != null && x.RegdNo.StartsWith(regdNum.ToUpper()))).ToList();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "GetRnoAutoComplete", ex);
            }
            return tblAbbregistrations;
        }
        #endregion

        #region Autocomplete for ModelNo
        /// <summary>
        /// Autocomplete for RegdNo
        /// </summary>
        /// <param name="regdNum"></param>
        /// <returns>tblAbbregistrations Regd no list</returns>
        public List<TblModelNumber> GetModelNoAutoComplete(string modelno)
        {
            List<TblModelNumber> tblmodelnumber = null;

            try
            {
                if (modelno != null)
                {
                    tblmodelnumber = _modelNumberRepository.GetList(x => x.IsActive == true && x.ModelName!=null && x.ModelName.Contains(modelno.ToUpper().Trim())).ToList();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "GetRnoAutoComplete", ex);
            }
            return tblmodelnumber;
        }
        #endregion

        #region Method to save ABB Details in ExchangeABBHistory
        /// <summary>
        ///  save ABB Registration Details
        /// </summary>
        /// <param name="abbRegistrationModel"></param>
        /// <returns>AbbregistrationId</returns>
        public int SaveExchangeABBHistory(string regdno, int UserId,int ABBstatus)
        {
            TblAbbregistration tblAbbregistration = new TblAbbregistration();
            TblOrderTran tblOrderTran = new TblOrderTran();
            try
            {
                if (regdno != null )
                {                    
                    tblOrderTran = _orderTransRepository.GetRegdno(regdno);
                    tblAbbregistration = _abbRegistrationRepository.GetRegdNo(regdno);
                    //tblAbbregistration = _abbRegistrationRepository.GetSingle(x => x.IsActive == true && x.RegdNo == regdno);

                    #region Create TblExchangeAbbstatusHistory
                    //TblExchangeAbbstatusHistory tblExchangeAbbstatus = _exchangeABBStatusHistoryRepository.GetSingle(x=>x.OrderTransId== tblOrderTran.OrderTransId);
                    TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                    tblExchangeAbbstatusHistory.OrderType = (int)ExchangeABBEnum.ABB;
                    tblExchangeAbbstatusHistory.SponsorOrderNumber = tblAbbregistration.SponsorOrderNo;
                    tblExchangeAbbstatusHistory.RegdNo = tblAbbregistration.RegdNo;                   
                    tblExchangeAbbstatusHistory.StatusId = ABBstatus;
                    tblExchangeAbbstatusHistory.IsActive = true;
                    tblExchangeAbbstatusHistory.CreatedBy = UserId;
                    tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                    tblExchangeAbbstatusHistory.JsonObjectString = JsonConvert.SerializeObject(tblExchangeAbbstatusHistory);
                    //tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                    _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                    _exchangeABBStatusHistoryRepository.SaveChanges();
                    #endregion                    
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "ManageUser", ex);
            }
            return 0;
        }
        #endregion

        #region VK Method to Get List of Models by Product Cat and Product Type
        public List<TblModelNumber> GetModelNumListByProdCatAndProdType(int? prodCatId, int? prodTypeId,int BuId)
        {
            List<TblModelNumber> tblModelNumberList = null;
            try
            {
                if (prodCatId != null && prodCatId > 0 && prodTypeId != null && prodTypeId > 0)
                {
                    tblModelNumberList = _modelNumberRepository.GetList(x=>x.IsActive == true && x.IsDefaultProduct==false && x.ProductCategoryId == prodCatId && x.ProductTypeId == prodTypeId && x.BusinessUnitId == BuId ).ToList();
                }
                if (tblModelNumberList == null)
                {
                    tblModelNumberList = new List<TblModelNumber>();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "GetModelNumListByProdCatAndProdType", ex);
            }
            return tblModelNumberList;
        }
        #endregion

        #region Create Abb Order for d2c
        /// <summary>
        /// Create Abb Order
        /// added by ashwin
        /// </summary>
        /// <param name="aBBProductOrderDataContract"></param>
        /// <returns>aBBOrderResponse</returns>
        public ResponseResult CreateAbbOrder(AbbRegistrationModel RequestAbbRegistrationModel, string username)
        {
            #region Variable's and Object Define
            ResponseResult responseResult = new ResponseResult();
            responseResult.message = string.Empty;
            AbbRegistrationModel AbbRegistrationModel = new AbbRegistrationModel();
            TblAbbregistration tblABBRegistration = new TblAbbregistration();
            int result = 0;
            DateTime _dateTime = DateTime.Now.TrimMilliseconds();
            Login login = null;
            #endregion

            try
            {
                login = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.Username) && x.Username.ToLower().Equals(username.ToLower()));
                TblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => !string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Equals(username.ToLower()));
                if (login != null && businessUnit.BusinessUnitId > 0)
                {
                    #region add customer entries in tblcustomerdetails

                    TblCustomerDetail tblCustomerDetail = new TblCustomerDetail();
                    tblCustomerDetail.FirstName = RequestAbbRegistrationModel.CustFirstName;
                    tblCustomerDetail.LastName = RequestAbbRegistrationModel.CustLastName;
                    tblCustomerDetail.PhoneNumber = RequestAbbRegistrationModel.CustMobile;
                    tblCustomerDetail.Email = RequestAbbRegistrationModel.CustEmail;
                    tblCustomerDetail.CreatedDate = _currentDatetime;
                    tblCustomerDetail.IsActive = true;
                    tblCustomerDetail.City = RequestAbbRegistrationModel.CustCity;
                    _customerDetailsRepository.Create(tblCustomerDetail);
                    int custResult = _customerDetailsRepository.SaveChanges();
                    if (custResult > 0)
                    {
                        RequestAbbRegistrationModel.CustomerId = tblCustomerDetail.Id;
                    }
                    #endregion

                    #region add login entries
                    RequestAbbRegistrationModel.BusinessUnitId = login.SponsorId;
                    RequestAbbRegistrationModel.RegdNo = "A" + UniqueString.RandomNumberByLength(8);
                    RequestAbbRegistrationModel.UploadDateTime = _currentDatetime;
                    RequestAbbRegistrationModel.BusinessPartnerId = login.BusinessPartnerId;
                    RequestAbbRegistrationModel.CreatedDate = _currentDatetime;
                    RequestAbbRegistrationModel.IsActive = true;
                    #endregion

                    #region save invoice image 
                    //if (aBBProductOrderDataContract.Base64StringValue != null)
                    //{

                    //    byte[] bytes = System.Convert.FromBase64String(aBBProductOrderDataContract.Base64StringValue);

                    //    if (bytes != null)
                    //    {
                    //        aBBProductOrderDataContract.imageName = aBBProductOrderDataContract.RegdNo + _currentDatetime.ToString() + Path.GetExtension("image.jpeg");
                    //        string rootPath = @HostingEnvironment.ApplicationPhysicalPath;
                    //        string filePath = ConfigurationManager.AppSettings["InvoiceImage"].ToString() + aBBProductOrderDataContract.imageName;
                    //        System.IO.File.WriteAllBytes(rootPath + filePath, bytes);
                    //        aBBProductOrderDataContract.InvoiceImage = aBBProductOrderDataContract.imageName;
                    //    }

                    //}
                    #endregion

                    #region check abb is deffered or not
                    //if (login != null &&  login.Id > 0)
                    //{
                    //    TblBusinessPartner tblBusinessPartner = _BusinessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == login.BusinessPartnerId);
                    //    if (tblBusinessPartner != null)
                    //    {
                    //        aBBProductOrderDataContract.IsdefferedAbb = tblBusinessPartner.IsDefferedAbb;
                    //    }

                    //}
                    #endregion

                    

                    tblABBRegistration = _mapper.Map<AbbRegistrationModel, TblAbbregistration>(RequestAbbRegistrationModel);

                    _abbRegistrationRepository.Create(tblABBRegistration);
                    result = _abbRegistrationRepository.SaveChanges();

                    if (result != 0)
                    {
                        #region code to send selfqc link on whatsappNotification
                        //WhatasappResponse whatasappResponse = new WhatasappResponse();
                        //TblWhatsAppMessage tblwhatsappmessage = null;
                        
                        //_exchangeOrderManager.sendSelfQCUrl(tblABBRegistration.RegdNo, tblABBRegistration.CustMobile);
                        //string baseurl = _config.Value.BaseURL + "ABB/CreateOrder?" + custData;

                        //WhatsappTemplate whatsappObj = new WhatsappTemplate();
                        //whatsappObj.userDetails = new UserDetails();
                        //whatsappObj.notification = new SelfQC();
                        //whatsappObj.notification.@params = new URL();
                        //whatsappObj.userDetails.number = tblABBRegistration.CustMobile;
                        //whatsappObj.notification.sender = _config.Value.YelloaiSenderNumber;
                        //whatsappObj.notification.type = _config.Value.YellowaiMesssaheType;
                        //whatsappObj.notification.templateId = NotificationConstants.SelfQC_Link;
                        //whatsappObj.notification.@params.Link = baseurl;
                        //string url = _config.Value.YellowAiUrl;
                        //RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                        //if (response.Content != null)
                        //{
                        //    whatasappResponse = JsonConvert.DeserializeObject<WhatasappResponse>(response.Content);
                        //    tblwhatsappmessage = new TblWhatsAppMessage();
                        //    tblwhatsappmessage.TemplateName = NotificationConstants.Logi_Drop;
                        //    tblwhatsappmessage.IsActive = true;
                        //    tblwhatsappmessage.PhoneNumber = tblABBRegistration.CustMobile;
                        //    tblwhatsappmessage.SendDate = DateTime.Now;
                        //    tblwhatsappmessage.MsgId = whatasappResponse.msgId;
                        //    _WhatsAppMessageRepository.Create(tblwhatsappmessage);
                        //    _WhatsAppMessageRepository.SaveChanges();
                        //}
                        #endregion


                        responseResult.Status = true;
                        responseResult.Status_Code = HttpStatusCode.OK;
                        responseResult.message = "Success";
                    }

                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    responseResult.message = "businessUnit Not Found for login details";
                }


            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("AbbRegistrationManager", "CreateAbbOrder", ex);
            }

            return responseResult;
        }
        #endregion

        #region set custdata for payout 
        public string setUrlforPayment(string name,string contact,string planPrice,string productCategory)
        {
            string custData = string.Empty;
            try 
            {
                string custName = "";
            }
            catch(Exception ex)
            {
                _logging.WriteErrorToDB("setUrlforPayment", "AbbRegistrationManager", ex);
            }
            return custData;
        }
        //string custData = "name=" + SecurityHelper.EncryptString(tblABBRegistration.CustFirstName + tblABBRegistration.CustLastName, _config.Value.SecurityKey); ;
        #endregion

        #region Get Product Category on the basis of AbbPlanMaster for D2C Registration
        /// <summary>
        /// Get Product category by  AbbPlanMaster on basis of D2C Id
        /// </summary>
        /// <returns></returns>
        public List<BuProductCatDataModel> GetAllProductCategoryByAbbPlanMaster()
        {
            List<TblAbbplanMaster> tblAbbplanMasters = new List<TblAbbplanMaster>();
            List<BuProductCatDataModel> buProductCatDataModelList = new List<BuProductCatDataModel>();
            string UserName = "DtoC@digimart.co.in";
            try
            {
                Login tbllogin = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.Username) && x.Username.ToLower().Equals(UserName.ToLower()));
                if (tbllogin != null)
                {
                    tblAbbplanMasters = _aBBPlanMasterRepository.GetList(x => x.IsActive == true && x.BusinessUnitId == tbllogin.SponsorId)
                                     .GroupBy(x => x.ProductCatId).Select(g => g.FirstOrDefault()).ToList();

                    if (tblAbbplanMasters.Count > 0)
                    {
                        foreach (var productCat in tblAbbplanMasters)
                        {
                            TblProductCategory prodCatData = _productCategoryRepository.GetSingle(x => x.IsActive == true && x.Id == productCat.ProductCatId && x.IsAllowedForNew==true);
                            if (prodCatData != null)
                            {
                                BuProductCatDataModel buProductCatDataModel = new BuProductCatDataModel();
                                buProductCatDataModel = _mapper.Map<TblProductCategory, BuProductCatDataModel>(prodCatData);
                                buProductCatDataModelList.Add(buProductCatDataModel);
                            }

                        }
                    }
                    else
                    {
                        buProductCatDataModelList = new List<BuProductCatDataModel>();
                    }
                }
                else
                {
                    buProductCatDataModelList = new List<BuProductCatDataModel>();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "GetAllProductCategoryByAbbPlanMaster", ex);
            }
            return buProductCatDataModelList;
        }
        #endregion

        #region Get Product Type on the basis of AbbPlanMaster for D2C Registration
        /// <summary>
        /// Get Product Type by  AbbPlanMaster on basis of D2C Id
        /// </summary>
        /// <param name="NewProductCategoryId"></param>
        /// <returns></returns>
        public IList<ProductTypeViewModel> GetAllProductTypeByAbbPlanMaster(int NewProductCategoryId)
        {
            IList<ProductTypeViewModel> productTypeViewModels = new List<ProductTypeViewModel>();

            List<TblAbbplanMaster> tblAbbplanMasters = new List<TblAbbplanMaster>();
            
            string UserName = "DtoC@digimart.co.in";
            try
            {
                Login tbllogin = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.Username) && x.Username.ToLower().Equals(UserName.ToLower()));
                if (tbllogin != null && NewProductCategoryId>0)
                {
                    tblAbbplanMasters = _aBBPlanMasterRepository.GetList(x => x.IsActive == true && x.BusinessUnitId == tbllogin.SponsorId && x.ProductCatId == NewProductCategoryId)
                                     .GroupBy(x => x.ProductTypeId).Select(g => g.FirstOrDefault()).ToList();

                    if (tblAbbplanMasters.Count > 0)
                    {
                        foreach (var producttype in tblAbbplanMasters)
                        {
                            TblProductType prodTypeData = _productTypeRepository.GetSingle(x => x.IsActive == true && x.Id == producttype.ProductTypeId && x.IsAllowedForNew == true);
                            if (prodTypeData != null)
                            {
                                ProductTypeViewModel productTypeView = new ProductTypeViewModel();
                                productTypeView = _mapper.Map<TblProductType,ProductTypeViewModel>(prodTypeData);
                                if(string.IsNullOrEmpty(productTypeView.Size))
                                {
                                    //productTypeView.Description = productTypeView.Description + "(" + productTypeView.Size + ")";
                                    productTypeViewModels.Add(productTypeView);
                                }
                            }
                        }
                        
                    }
                    else
                    {
                        productTypeViewModels = new List<ProductTypeViewModel>();
                    }
                }
                else
                {
                    productTypeViewModels = new List<ProductTypeViewModel>();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "GetAllProductCategoryByAbbPlanMaster", ex);
            }
            return productTypeViewModels;
        }
        #endregion

        #region Get Brand  for ABB  D2C Registration 
        /// <summary>
        /// Get Product Type by  AbbPlanMaster on basis of D2C Id
        /// </summary>
        /// <param name="NewProductCategoryId"></param>
        /// <returns></returns>
        public List<BrandViewModel> GetAllBrandForAbbD2c(int NewProductCategoryId)
        {
            List<BrandViewModel> BrandVMList = null;
            List<TblBrand> TblBrandlist = new List<TblBrand>();
           
            try
            {
                TblBusinessUnit tblBuID = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == Convert.ToInt32(BussinessUnitEnum.D2C));
                if (tblBuID != null && tblBuID.IsBumultiBrand == true && NewProductCategoryId>0)
                {
                    List<TblBrandSmartBuy> tblBrandSmart = _brandSmartBuyRepository.GetList(x => x.IsActive == true && x.BusinessUnitId== tblBuID.BusinessUnitId && x.ProductCategoryId == NewProductCategoryId).ToList();
                    if (tblBrandSmart != null)
                    {
                        BrandVMList = _mapper.Map<List<TblBrandSmartBuy>, List<BrandViewModel>>(tblBrandSmart);
                    }

                }
                else
                {
                    List<TblBrand> tblBrand = _brandRepository.GetList(x => x.IsActive == true ).ToList();
                    if (tblBrand != null)
                    {
                        BrandVMList = _mapper.Map<List<TblBrand>, List<BrandViewModel>>(tblBrand);
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "GetAllBrand", ex);
            }
            return BrandVMList;
        }
        #endregion

        #region Create pdf string for ABB Approval Certificate for Customer Added by VK
        /// <summary>
        /// Create pdf string for ABB Approval Certificate for Customer Added by VK
        /// </summary>
        /// <param name="ABBRegistrationViewModel"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns></returns>
        public string GetCertificateHtmlString(ABBRegistrationViewModel abbRegVM, string HtmlTemplateNameOnly)
        {
            #region Variable Declaration
            var DateTime = System.DateTime.Now;
            string FinalDate = DateTime.Date.ToShortDateString();
            string htmlString = "";
            string fileName = HtmlTemplateNameOnly + ".html";
            string fileNameWithPath = "";
            #endregion

            try
            {
                if (abbRegVM != null && HtmlTemplateNameOnly != null)
                {
                    string baseUrl = _config.Value.BaseURL;
                    string BgImg = baseUrl + EnumHelper.DescriptionAttr(FileAddressEnum.BgImg);
                    string digi2l_logo = baseUrl + EnumHelper.DescriptionAttr(FileAddressEnum.digi2l_logo);
                    string companylogo = baseUrl + EnumHelper.DescriptionAttr(FileAddressEnum.logo);

                    #region Get Html String Dynamically
                    var filePath = string.Concat(_webHostEnvironment.WebRootPath, @"\PdfTemplates");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath); //Create directory if it doesn't exist
                    }
                    fileNameWithPath = string.Concat(filePath, "\\", fileName);

                    htmlString = File.ReadAllText(fileNameWithPath);
                    #endregion

                    if (HtmlTemplateNameOnly == TemplateConfigConstant.ABBCertificateName)
                        htmlString = htmlString.Replace("[Brand_Name]", abbRegVM.BrandName)
                            .Replace("[Regd_No]", abbRegVM.RegdNo)
                            .Replace("[Cust_Mobile_No]", abbRegVM.CustMobile)
                            .Replace("[Prod_Cat_And_Type_Desc]", abbRegVM.ProductDesc)
                            .Replace("[Serial_No]", abbRegVM.ProductSrNo)
                            .Replace("[Invoice_No]", abbRegVM.InvoiceNo)
                            .Replace("[Date_Of_Purchase]", abbRegVM.DateOfPurchase)
                            .Replace("[Date_of_Commencement]", abbRegVM.DateOfPurchase)
                            .Replace("[Date_of_Expiry]", abbRegVM.DateOfExpiry)
                            .Replace("[BgImg]", BgImg)
                            .Replace("[companylogo]", companylogo)
                            .Replace("[digi2l_logo]", digi2l_logo);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "GetCertificateHtmlString", ex);
            }
            return htmlString;
        }
        #endregion

        #region Send ABB Approval Certificate Added by VK
        /// <summary>
        /// Create pdf string for ABB Approval Certificate for Customer Added by VK
        /// </summary>
        /// <param name="ABBRegistrationViewModel"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns></returns>
        public bool? SendABBWelcomeMailToCust(ABBRegistrationViewModel aBBRegistrationVM, TemplateViewModel templateVM)
        {
            #region Variable Declaration
            var DateTime = System.DateTime.Now;
            string FinalDate = DateTime.Date.ToShortDateString();
            string htmlString = "";
            string certifTempName = TemplateConfigConstant.ABBCertificateName;
            string fileNameWithPath = "";
            bool IsCertificateGenerated = false;
            string filePath = "";
            string certificateFileName = "";
            string invoiceTempName = TemplateConfigConstant.ABBInvoiceName;
            bool IsInvoiceGenerated = false;
            string invHtmlString = "";
            Task<bool>? IsMailSent = null;
            bool? flag = false;
            IList<TblConfiguration>? tblConfigurationList = null;
            string? FinancialYear = null;
            int customerFilesId = 0;
            string? ABB_Bcc = null;
            #endregion

            try
            {
                if (aBBRegistrationVM != null && templateVM != null)
                {
                    #region Certificate Generate and Save
                    if (aBBRegistrationVM.IsCertificateAvailable != null && aBBRegistrationVM.IsCertificateAvailable == true)
                    {
                        filePath = EnumHelper.DescriptionAttr(FilePathEnum.ABBApprovalCertificate);
                        certificateFileName = aBBRegistrationVM.RegdNo + "_Certificate.pdf";
                        htmlString = GetCertificateHtmlString(aBBRegistrationVM, certifTempName);
                        IsCertificateGenerated = _htmlToPDFConverterHelper.GenerateCustomLayoutPDF(htmlString, filePath, certificateFileName);
                        if (IsCertificateGenerated)
                        {
                            templateVM.IsCertificateGenerated = IsCertificateGenerated;
                            fileNameWithPath = filePath + "\\" + certificateFileName;
                            templateVM.AttachmentFilePath = filePath;
                            templateVM.AttachmentFileName = certificateFileName;
                            templateVM.FileNameWithPath = fileNameWithPath;
                        }
                    }
                    #endregion

                    #region Invoice Generate
                    if (aBBRegistrationVM.IsInvoiceAvailable != null && aBBRegistrationVM.IsInvoiceAvailable == true)
                    {
                        #region Code for Get Data from TblConfiguration
                        tblConfigurationList = _templateConfigurationRepository.GetConfigurationList();
                        if (tblConfigurationList != null && tblConfigurationList.Count > 0)
                        {
                            var financialYear = tblConfigurationList.FirstOrDefault(x => x.Name == ConfigurationEnum.FinancialYear.ToString());
                            if (financialYear != null && financialYear.Value != null)
                            {
                                FinancialYear = financialYear.Value.Trim();
                            }
                            var GetBcc = tblConfigurationList.FirstOrDefault(x => x.Name == ConfigurationEnum.ABB_Bcc.ToString());
                            if (GetBcc != null && GetBcc.Value != null)
                            {
                                ABB_Bcc = GetBcc.Value.Trim();
                            }
                        }
                        #endregion

                        #region Code for get Max InvSrNum from tblCustomerFiles
                        int? MaxSrNum = _customerFilesRepository.GetAbbMaxInvSrNum(FinancialYear);
                        if (MaxSrNum == null || MaxSrNum == 0)
                        {
                            MaxSrNum = 1;
                        }
                        else
                        {
                            MaxSrNum++;
                        }
                        #endregion
                        
                        #region Set Counter Sr. Number 
                        FinancialYear = FinancialYear ?? "";
                        var BillCounterNum = String.Format("{0:D6}", MaxSrNum);
                        aBBRegistrationVM.BillNumber = "ABB-Inv-" + FinancialYear + "-" + BillCounterNum;
                        templateVM.InvAttachFileName = "ABB-Inv-"/*+ aBBRegistrationVM.RegdNo +"_"+ "Inv-"*/ + FinancialYear.Replace("/", "-") + "-" + BillCounterNum + ".pdf";
                        aBBRegistrationVM.BillCounterNum = BillCounterNum;
                        aBBRegistrationVM.FinancialYear = FinancialYear;
                        #endregion

                        templateVM.InvAttachFilePath = EnumHelper.DescriptionAttr(FilePathEnum.ABBCustomerInvoice);
                        invHtmlString = GetInvoiceHtmlString(aBBRegistrationVM, invoiceTempName);
                        IsInvoiceGenerated = _htmlToPDFConverterHelper.GeneratePDF(invHtmlString, templateVM.InvAttachFilePath, templateVM.InvAttachFileName);
                        templateVM.IsInvoiceGenerated = IsInvoiceGenerated;
                        if (IsInvoiceGenerated)
                        {
                            fileNameWithPath = templateVM.InvAttachFilePath + "\\" + templateVM.InvAttachFileName;
                            templateVM.InvFileNameWithPath = fileNameWithPath;
                        }
                    }
                    #endregion

                    if (!(aBBRegistrationVM.IsCertificateAvailable ?? false) && (aBBRegistrationVM.IsInvoiceAvailable ?? false))
                    {
                        templateVM.Cc = aBBRegistrationVM.BUEmail;
                        templateVM.Bcc = ABB_Bcc;
                    }
                    else if (aBBRegistrationVM.IsCertificateAvailable ?? false)
                    {
                        templateVM.Cc = aBBRegistrationVM.BUEmail + "," + aBBRegistrationVM.BPEmail;
                        templateVM.Bcc = ABB_Bcc;
                    }

                    if (IsCertificateGenerated || IsInvoiceGenerated)
                    {
                        customerFilesId = ManageABBCustomerFiles(aBBRegistrationVM, templateVM);
                    }
                    IsMailSent = _mailManager.JetMailSendWithAttachment(templateVM);
                    if (IsMailSent.Result)
                    {
                        flag = IsMailSent.Result;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "SendABBWelcomeMailToCust", ex);
            }
            return flag;
        }
        #endregion

        #region Create Html string for Customer ABB Invoice Added by VK
        /// <summary>
        /// Create Html string for Customer ABB Invoice Added by VK
        /// </summary>
        /// <param name="abbRegVM"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns></returns>
        public string GetInvoiceHtmlString(ABBRegistrationViewModel abbRegVM, string HtmlTemplateNameOnly)
        {
            #region Variable Declaration
            var DateTime = System.DateTime.Now;
            string FinalDate = DateTime.Date.ToShortDateString();
            string htmlString = "";
            string fileName = HtmlTemplateNameOnly + ".html";
            string fileNameWithPath = "";
            string? baseUrl = _config.Value.BaseURL;
            decimal? basePriceInv = 0;
            decimal? gstInv = 0;
            #endregion

            try
            {
                if (abbRegVM != null && HtmlTemplateNameOnly != null)
                {
                    #region Get Html String Dynamically
                    var filePath = string.Concat(_webHostEnvironment.WebRootPath, @"\PdfTemplates");
                    fileNameWithPath = string.Concat(filePath, "\\", fileName);
                    htmlString = File.ReadAllText(fileNameWithPath);
                    #endregion

                    #region Configuration setup for Invoice
                    var UtcSeel_INV = baseUrl + EnumHelper.DescriptionAttr(FileAddressEnum.UTCACSeel);
                    var CurrentDate = Convert.ToDateTime(_currentDatetime).ToString("dd/MM/yyyy");
                    decimal? finalPriceInv = abbRegVM.Abbfees??0;
                    basePriceInv = abbRegVM.BaseValue??0;
                    gstInv = abbRegVM.Cgst??0;

                    #region Invoice GST Calculation
                    //string? GSTInclusive = EnumHelper.DescriptionAttr(LoVEnum.GSTInclusive);// Get this value from BU table. For now its is kept as true;
                    //string? GSTExclusive = EnumHelper.DescriptionAttr(LoVEnum.GSTExclusive);
                    //string? GSTNotApplicable = EnumHelper.DescriptionAttr(LoVEnum.GSTNotApplicable);
                    //if (abbRegVM.GSTType == GSTInclusive)
                    //{
                    //    basePriceInv = finalPriceInv / Convert.ToDecimal(GeneralConstant.GSTPercentage);
                    //    basePriceInv = Math.Round((basePriceInv ?? 0), 2);
                    //    gstInv = basePriceInv * Convert.ToDecimal(GeneralConstant.CGST);
                    //    gstInv = Math.Round((gstInv ?? 0), 2);
                    //}
                    //else if (abbRegVM.GSTType == GSTExclusive)
                    //{
                    //    basePriceInv = finalPriceInv;
                    //    basePriceInv = Math.Round((basePriceInv ?? 0), 2);
                    //    gstInv = basePriceInv * Convert.ToDecimal(GeneralConstant.CGST);
                    //    gstInv = Math.Round((gstInv ?? 0), 2);
                    //}
                    //else if (abbRegVM.GSTType == GSTNotApplicable)
                    //{
                    //    basePriceInv = finalPriceInv;
                    //    basePriceInv = Math.Round((basePriceInv ?? 0), 2);
                    //    gstInv = 0;
                    //}
                    #endregion

                    decimal? finalPriceWithGSTInv = finalPriceInv;
                    string finalPiceInWordsInv = NumberToWordsConverterHelper.ConvertAmount(Convert.ToDecimal(finalPriceWithGSTInv));
                    #endregion

                    #region Replace Dynamic part from the template
                    if (HtmlTemplateNameOnly == TemplateConfigConstant.ABBInvoiceName)
                    {
                        htmlString = htmlString.Replace("[BillNumber]", abbRegVM.BillNumber)
                            .Replace("[PlaceOfSupply]", abbRegVM.CustState)
                            .Replace("[CurrentDate]", CurrentDate)
                            .Replace("[CustomerName]", abbRegVM.CustFirstName + " " + abbRegVM.CustLastName)
                            .Replace("[Address]", abbRegVM.CustAddress1)
                            .Replace("[City]", abbRegVM.CustCity)
                            .Replace("[Pincode]", abbRegVM.CustPinCode)
                            .Replace("[State]", abbRegVM.CustState)
                            .Replace("[RegdNo]", abbRegVM.RegdNo)
                            .Replace("[BasePrice]", basePriceInv.ToString())
                            .Replace("[GST]", gstInv.ToString())
                            .Replace("[FinalPrice]", finalPriceWithGSTInv.ToString())
                            .Replace("[FinalAmtInWords]", finalPiceInWordsInv)
                            .Replace("[UtcSeel_INV]", UtcSeel_INV);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "GetCertificateHtmlString", ex);
            }
            return htmlString;
        }
        #endregion


        #region ABB Approve Added by VK
        /// <summary>
        /// ABB Approve
        /// </summary>
        /// <param name="ABBRegistrationViewModel"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns></returns>
        public bool? ABBApproveById(int? ABBregistrationId, int? loggedInUserId)
        {
            #region Variable Declaration
            TblAbbregistration? tblAbbregistration = null;
            int result = 0;
            bool? flag = false;
            int statusUpdated = 0;
            bool? IsSendMail = false;
            #endregion

            try
            {
                tblAbbregistration = _abbRegistrationRepository.GetSingleOrder(Convert.ToInt32(ABBregistrationId));

                if (tblAbbregistration != null)
                {
                    tblAbbregistration.AbbApprove = true;
                    int ABBstatus = (int)ExchangeOrderStatusEnum.ABBApproved;
                    result = SaveExchangeABBHistory(tblAbbregistration.RegdNo, Convert.ToInt32(loggedInUserId), ABBstatus);
                    tblAbbregistration.ModifiedDate = DateTime.Now;
                    _abbRegistrationRepository.Update(tblAbbregistration);
                    statusUpdated = _abbRegistrationRepository.SaveChanges();
                    if (statusUpdated > 0)
                    {
                        flag = true;
                    }
                    IsSendMail = SendABBEmail(tblAbbregistration,loggedInUserId);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "ABBApproveById", ex);
            }
            return flag;
        }
        #endregion

        #region Send ABB Email Added by VK
        public bool? SendABBEmail(TblAbbregistration tblAbbregistrations, int? loggedInUserId)
        {
            bool? flag = false;
            string? body = null;
            string? mailTemplateName = TemplateConfigConstant.ABBWelcomeMailTempName;
            string? fileName = mailTemplateName + ".html";
            ABBRegistrationViewModel? aBBRegistrationVM = null;
            TemplateViewModel templateVM = new TemplateViewModel();
            BrandViewModel? brandVM = null;
            bool? IsCertificateAvailable = false;
            bool? IsInvoiceAvailable = false;
            try
            {
                
                if (tblAbbregistrations != null)
                {
                    #region Set ABB Welcome Mail Content 
                    TblProductCategory? tblProductCategory = null;
                    tblProductCategory = tblAbbregistrations.NewProductCategory;
                    tblAbbregistrations.NewProductCategoryName = tblProductCategory != null ? tblProductCategory.Description : string.Empty;

                    TblProductType? tblProductType = tblAbbregistrations.NewProductCategoryTypeNavigation;
                    tblAbbregistrations.NewProductCategoryType = tblProductType != null ? tblProductType.Description : string.Empty;

                    TblModelNumber tblmodelnumber = _modelNumberRepository.GetSingle(x => x.ModelNumberId == tblAbbregistrations.ModelNumberId);
                    tblAbbregistrations.MarCom = tblmodelnumber != null ? tblmodelnumber.ModelName : string.Empty;

                    TblPinCode tblPinCode = _pinCodeRepository.GetByPincode(Convert.ToInt32(tblAbbregistrations.CustPinCode));
                    tblAbbregistrations.CustCity = tblPinCode != null ? tblPinCode.Location : string.Empty;
                    tblAbbregistrations.CustState = tblPinCode != null ? tblPinCode.State : string.Empty;

                    TblCustomerDetail tblCustomerDetails = _customerDetailsRepository.GetCustDetails(Convert.ToInt32(tblAbbregistrations.CustomerId ?? 0));
                    tblAbbregistrations.CustEmail = tblCustomerDetails != null ? tblCustomerDetails.Email : string.Empty;

                    TblBusinessUnit? tblBusinessUnit = tblAbbregistrations.BusinessUnit;
                    string? buEmail = null; string? bpEmail = null;
                    if (tblBusinessUnit != null)
                    {
                        buEmail = tblBusinessUnit?.Email;
                        if (tblBusinessUnit.IsCertificateAvailable == true)
                        {
                            IsCertificateAvailable = tblBusinessUnit.IsCertificateAvailable;
                        }
                        if (tblBusinessUnit.IsInvoiceAvailable == true)
                        {
                            IsInvoiceAvailable = tblBusinessUnit.IsInvoiceAvailable;
                        }
                        int? newBrandId = tblAbbregistrations.NewBrandId;
                        brandVM = _commonManager.GetAbbBrandDetailsById(tblBusinessUnit.IsBumultiBrand, newBrandId);
                        tblAbbregistrations.NewSize = brandVM?.Name;
                    }
                    TblBusinessPartner? tblBusinessPartner = tblAbbregistrations.BusinessPartner;
                    if (tblBusinessPartner != null)
                    {
                        tblAbbregistrations.StoreName = tblBusinessPartner?.Name;
                        bpEmail = tblBusinessPartner?.Email;
                    }
                    #endregion

                    #region Get Html String Dynamically for Welcome Email
                    body = GetWelcomeMailHtmlString(tblAbbregistrations, mailTemplateName);
                    #endregion

                    aBBRegistrationVM = _mapper.Map<ABBRegistrationViewModel>(tblAbbregistrations);
                    if (tblAbbregistrations.InvoiceDate != null)
                    {
                        int planPeriod = Convert.ToInt32(tblAbbregistrations.AbbplanPeriod != null ? tblAbbregistrations.AbbplanPeriod : 0);
                        DateTime? DOE = Convert.ToDateTime(tblAbbregistrations.InvoiceDate).AddMonths(planPeriod);
                        aBBRegistrationVM.DateOfExpiry = Convert.ToDateTime(DOE).ToString("dd/MM/yyyy");
                        aBBRegistrationVM.DateOfPurchase = Convert.ToDateTime(tblAbbregistrations.InvoiceDate).ToString("dd/MM/yyyy");
                        aBBRegistrationVM.BrandName = tblAbbregistrations.NewSize;
                        aBBRegistrationVM.ProductDesc = tblAbbregistrations.NewProductCategoryName + " " + tblAbbregistrations.NewProductCategoryType;
                        aBBRegistrationVM.LoggedInUserId = loggedInUserId;
                        aBBRegistrationVM.IsCertificateAvailable = IsCertificateAvailable;
                        aBBRegistrationVM.IsInvoiceAvailable = IsInvoiceAvailable;
                        aBBRegistrationVM.BUEmail = buEmail;
                        aBBRegistrationVM.BPEmail = bpEmail;
                    }
                    templateVM.To = tblAbbregistrations.CustEmail;
                    templateVM.HtmlBody = body;
                    templateVM.Subject = TemplateConfigConstant.ABBWelcomeMailSubject;
                    flag = SendABBWelcomeMailToCust(aBBRegistrationVM, templateVM);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "SendABBEmail", ex);
            }
            return flag;
        }
        #endregion

        #region Store Customer Files in database
        /// <summary>
        /// Store Customer Files in database
        /// </summary>       
        /// <returns></returns>   
        public int ManageABBCustomerFiles(ABBRegistrationViewModel abbRegVM, TemplateViewModel templateVM)
        {
            TblCustomerFile? tblCustomerFile = null;
            string fileName = null;
            int result = 0;
            int customerFilesId = 0;
            try
            {
                if (abbRegVM != null && templateVM != null && (templateVM.IsCertificateGenerated == true || templateVM.IsInvoiceGenerated == true))
                {
                    tblCustomerFile = _customerFilesRepository.GetCustomerFilesByRegdNo(abbRegVM.RegdNo);
                    if (tblCustomerFile == null)
                    {
                        tblCustomerFile = new TblCustomerFile();
                        //tblEVCPODDetailObj = _mapper.Map<PODViewModel, TblEvcpoddetail>(podVM);
                    }

                    #region Data Mapping According to the Conditions 
                    if (templateVM.IsCertificateGenerated == true)
                    {
                        tblCustomerFile.CertificatePdfName = templateVM.AttachmentFileName;
                    }
                    if (templateVM.IsInvoiceGenerated == true)
                    {
                        tblCustomerFile.InvoicePdfName = templateVM.InvAttachFileName;
                        tblCustomerFile.InvSrNum = Convert.ToInt32(abbRegVM.BillCounterNum);
                        tblCustomerFile.InvoiceDate = _currentDatetime;
                        tblCustomerFile.InvoiceAmount = abbRegVM.Abbfees;
                        tblCustomerFile.FinancialYear = abbRegVM.FinancialYear;
                    }
                    #endregion

                    #region Data mapping for create or update
                    if (tblCustomerFile.Id > 0)
                    {
                        tblCustomerFile.AbbregistrationId = abbRegVM.ABBRegistrationId;
                        tblCustomerFile.ModifiedBy = abbRegVM.LoggedInUserId;
                        tblCustomerFile.ModifiedDate = _currentDatetime;
                        _customerFilesRepository.Update(tblCustomerFile);
                    }
                    else
                    {
                        tblCustomerFile.AbbregistrationId = abbRegVM.ABBRegistrationId;
                        tblCustomerFile.RegdNo = abbRegVM.RegdNo;
                        tblCustomerFile.IsActive = true;
                        tblCustomerFile.CreatedBy = abbRegVM.LoggedInUserId;
                        tblCustomerFile.CreatedDate = _currentDatetime;
                        _customerFilesRepository.Create(tblCustomerFile);
                    }
                    #endregion

                    result = _customerFilesRepository.SaveChanges();
                    if (result > 0)
                    {
                        customerFilesId = tblCustomerFile.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "ManageABBCustomerFiles", ex);
            }
            return customerFilesId;
        }
        #endregion

        #region Create pdf string for ABB Welcome Mail for Customer Added by VK
        /// <summary>
        /// Create pdf string for ABB Welcome Mail for Customer Added by VK
        /// </summary>
        /// <param name="ABBRegistrationViewModel"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns></returns>
        public string GetWelcomeMailHtmlString(TblAbbregistration tblAbbregistrations, string HtmlTemplateNameOnly)
        {
            #region Variable Declaration
            var DateTime = System.DateTime.Now;
            string FinalDate = DateTime.Date.ToShortDateString();
            string htmlString = "";
            string fileName = HtmlTemplateNameOnly + ".html";
            string fileNameWithPath = "";
            #endregion

            try
            {
                if (tblAbbregistrations != null && HtmlTemplateNameOnly != null)
                {
                    //string baseUrl = _config.Value.BaseURL;
                    //string BgImg = baseUrl + EnumHelper.DescriptionAttr(FileAddressEnum.BgImg);
                    //string digi2l_logo = baseUrl + EnumHelper.DescriptionAttr(FileAddressEnum.digi2l_logo);
                    //string companylogo = baseUrl + EnumHelper.DescriptionAttr(FileAddressEnum.logo);

                    #region Get Html String Dynamically
                    var filePath = string.Concat(_webHostEnvironment.WebRootPath, @"\MailTemplates");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath); //Create directory if it doesn't exist
                    }
                    fileNameWithPath = string.Concat(filePath, "\\", fileName);

                    htmlString = File.ReadAllText(fileNameWithPath);
                    #endregion

                    if (HtmlTemplateNameOnly == TemplateConfigConstant.ABBWelcomeMailTempName)
                    {
                        htmlString = htmlString.Replace("[BrandName]", tblAbbregistrations.NewSize)
                    .Replace("[Customer]", tblAbbregistrations.CustFirstName)
                    .Replace("[RegdNo]", tblAbbregistrations.RegdNo)
                    .Replace("[Email]", tblAbbregistrations.CustEmail)
                    .Replace("[Address1]", tblAbbregistrations.CustAddress1)
                    .Replace("[Address2]", tblAbbregistrations.CustAddress2)
                    .Replace("[pincode]", tblAbbregistrations.CustPinCode)
                    .Replace("[city]", tblAbbregistrations.CustCity)
                    .Replace("[productcategory]", tblAbbregistrations.NewProductCategoryName)
                    .Replace("[ptoductType]", tblAbbregistrations.NewProductCategoryType)
                    .Replace("[productSerialNo]", tblAbbregistrations.ProductSrNo)
                    .Replace("[invoicedate]", tblAbbregistrations.InvoiceDate != null ? Convert.ToDateTime(tblAbbregistrations.InvoiceDate).ToString("dd/MM/yyyy") : "")
                    .Replace("[invoiceNumber]", tblAbbregistrations.InvoiceNo)
                    .Replace("[Netvalue]", tblAbbregistrations.ProductNetPrice.ToString())
                    .Replace("[PlanPeriod]", tblAbbregistrations.AbbplanPeriod)
                    .Replace("[NoClaimPEriod]", tblAbbregistrations.NoOfClaimPeriod)
                    .Replace("[ABBPlanName]", tblAbbregistrations.AbbplanName)
                    .Replace("[uploadDate]", tblAbbregistrations.UploadDateTime != null ? Convert.ToDateTime(tblAbbregistrations.UploadDateTime).ToString("dd/MM/yyyy") : "");
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "GetWelcomeMailHtmlString", ex);
            }
            return htmlString;
        }
        #endregion
    }
}
