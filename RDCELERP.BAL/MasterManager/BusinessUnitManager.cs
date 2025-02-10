using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NPOI.HPSF;
using NPOI.OpenXmlFormats.Dml;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.ABBRedemption;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.BusinessUnit;
using RDCELERP.Model.Master;
using RDCELERP.Model.OrderDetails;
using RDCELERP.Model.OrderTrans;
using RDCELERP.Model.PriceMaster;
using RDCELERP.Model.Product;
using RDCELERP.Model.TemplateConfiguration;
using RDCELERP.Model.Users;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace RDCELERP.BAL.MasterManager
{
    public class BusinessUnitManager : IBusinessUnitManager
    {
        #region  Variable Declaration
        IBusinessUnitRepository _businessUnitRepository;
        IBusinessPartnerRepository _businessPartnerRepository;
        IPriceMasterRepository _priceMasterRepository;
        IABBPlanMasterRepository _aBBPlanMasterRepository;
        IABBPriceMasterRepository _aBBPriceMasterRepository;
        IProductCategoryRepository _productCategoryRepository;
        IProductTypeRepository _productTypeRepository;
        IBUProductCategoryMapping _bUProductCategoryMapping;
        IBrandRepository _brandRepository;
        ILoginRepository _loginRepository;
        IUserRepository _userRepository;
        IUserRoleRepository _userRoleRepository;
        IRoleRepository _roleRepository;
        ICompanyRepository _companyRepository;
        IUserManager _userManager;
        IImageHelper _imageHelper;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IOrderTransRepository _orderTransRepository;
        IWebHostEnvironment _webHostEnvironment;
        IOptions<ApplicationSettings> _config;
        IMailManager _mailManager;
        ITemplateConfigurationRepository _templateConfigurationRepository;
        IOrderQCRepository _orderQCRepository;
        #endregion

        public BusinessUnitManager(IBusinessUnitRepository businessUnitRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging, IBusinessPartnerRepository businessPartnerRepository, IPriceMasterRepository priceMasterRepository, ILoginRepository loginRepository, IABBPlanMasterRepository aBBPlanMasterRepository, IABBPriceMasterRepository aBBPriceMasterRepository, IProductCategoryRepository productCategoryRepository, IProductTypeRepository productTypeRepository, IBrandRepository brandRepository, IBUProductCategoryMapping bUProductCategoryMapping, IUserRepository userRepository, IRoleRepository roleRepository, ICompanyRepository companyRepository, IImageHelper imageHelper, IUserManager userManager, IOrderTransRepository orderTransRepository, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, IMailManager mailManager, ITemplateConfigurationRepository templateConfigurationRepository, IOrderQCRepository orderQCRepository)
        {
            _businessUnitRepository = businessUnitRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
            _logging = logging;
            _businessPartnerRepository = businessPartnerRepository;
            _priceMasterRepository = priceMasterRepository;
            _loginRepository = loginRepository;
            _aBBPlanMasterRepository = aBBPlanMasterRepository;
            _aBBPriceMasterRepository = aBBPriceMasterRepository;
            _productCategoryRepository = productCategoryRepository;
            _productTypeRepository = productTypeRepository;
            _brandRepository = brandRepository;
            _bUProductCategoryMapping = bUProductCategoryMapping;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _companyRepository = companyRepository;
            _imageHelper = imageHelper;
            _userManager = userManager;
            _orderTransRepository = orderTransRepository;
            _webHostEnvironment = webHostEnvironment;
            _config = config;
            _mailManager = mailManager;
            _templateConfigurationRepository = templateConfigurationRepository;
            _orderQCRepository = orderQCRepository;
        }

        /// <summary>
        /// Method to manage (Add/Edit) Business Unit 
        /// </summary>
        /// <param name="BusinessUnitVM">BusinessUnitVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageBusinessUnit(BusinessUnitViewModel BusinessUnitVM, int userId)
        {
            TblBusinessUnit TblBusinessUnit = new TblBusinessUnit();

            try
            {
                if (BusinessUnitVM != null)
                {
                    TblBusinessUnit = _mapper.Map<BusinessUnitViewModel, TblBusinessUnit>(BusinessUnitVM);

                    if (TblBusinessUnit.BusinessUnitId > 0)
                    {
                        //Code to update the object                      
                        TblBusinessUnit.ModifiedBy = userId;
                        TblBusinessUnit.ModifiedDate = _currentDatetime;
                        TblBusinessUnit = TrimHelper.TrimAllValuesInModel(TblBusinessUnit);
                        _businessUnitRepository.Update(TblBusinessUnit);
                    }
                    else
                    {
                        var Check = _businessUnitRepository.GetSingle(x => x.Name == BusinessUnitVM.Name);
                        if (Check == null)
                        {
                            //Code to Insert the object 
                            TblBusinessUnit.IsActive = true;
                            TblBusinessUnit.CreatedDate = _currentDatetime;
                            TblBusinessUnit.CreatedBy = userId;
                            TblBusinessUnit = TrimHelper.TrimAllValuesInModel(TblBusinessUnit);
                            _businessUnitRepository.Create(TblBusinessUnit);
                        }
                    }
                    _businessUnitRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessPartnerManager", "BusinessPartnerManager", ex);
            }

            return TblBusinessUnit.BusinessUnitId;
        }

        /// <summary>
        /// Method to get the Business Unit by id 
        /// </summary>
        /// <param name="id">BusinessUnitId</param>
        /// <returns>BusinessUnitViewModel</returns>
        public BusinessUnitViewModel GetBusinessUnitById(int id)
        {
            BusinessUnitViewModel BusinessUnitVM = null;
            TblBusinessUnit TblBusinessUnit = null;

            try
            {
                TblBusinessUnit = _businessUnitRepository.GetSingle(where: x => x.IsActive == true && x.BusinessUnitId == id);
                if (TblBusinessUnit != null)
                {
                    BusinessUnitVM = _mapper.Map<TblBusinessUnit, BusinessUnitViewModel>(TblBusinessUnit);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "GetBusinessUnitById", ex);
            }
            return BusinessUnitVM;
        }

        /// <summary>
        /// Method to get the Business Unit by id 
        /// </summary>
        /// <param name="id">BusinessUnitId</param>
        /// <returns>BusinessUnitViewModel</returns>
        public BusinessUnitViewModel GetDefaultProductByModelBaised(int id)
        {
            BusinessUnitViewModel BusinessUnitVM = null;
            TblBusinessUnit TblBusinessUnit = null;

            try
            {
                TblBusinessUnit = _businessUnitRepository.GetSingle(where: x => x.IsActive == true && x.BusinessUnitId == id && x.IsSweetnerModelBased == true);
                if (TblBusinessUnit != null)
                {
                    BusinessUnitVM = _mapper.Map<TblBusinessUnit, BusinessUnitViewModel>(TblBusinessUnit);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "GetBusinessUnitById", ex);
            }
            return BusinessUnitVM;
        }

        /// <summary>
        /// Method to delete Business Unit by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletBusinessUnitById(int id)
        {
            bool flag = false;
            try
            {
                TblBusinessUnit TblBusinessUnit = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == id);
                if (TblBusinessUnit != null)
                {
                    TblBusinessUnit.IsActive = false;
                    _businessUnitRepository.Update(TblBusinessUnit);
                    _businessUnitRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "DeleteBusinessUnitById", ex);
            }
            return flag;
        }

        /// <summary>
        /// Method to get the All Business Unit
        /// </summary>     
        /// <returns>BusinessUnitViewModel</returns>
        public IList<BusinessUnitViewModel> GetAllBusinessUnit()
        {
            IList<BusinessUnitViewModel> BusinessUnitVMList = null;
            List<TblBusinessUnit> TblBusinessUnitlist = new List<TblBusinessUnit>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblBusinessUnitlist = _businessUnitRepository.GetList(x => x.IsActive == true).ToList();

                if (TblBusinessUnitlist != null && TblBusinessUnitlist.Count > 0)
                {
                    BusinessUnitVMList = _mapper.Map<IList<TblBusinessUnit>, IList<BusinessUnitViewModel>>(TblBusinessUnitlist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "GetAllBusinessUnit", ex);
            }
            return BusinessUnitVMList;
        }

        #region Manage Business Unit and Business Partner Onboarding
        #region Manage Business Partner Onboarding With Excel Upload
        /// <summary>
        /// Method to manage (Add/Edit) BusinessPartner
        /// </summary>
        /// <param name="BusinessPartnerVM">BusinessPartnerVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public BusinessUnitViewModel ManageBusinessPartnerBulk(BusinessUnitViewModel businessUnitVM, int loggedInUserId)
        {
            List<TblBusinessPartner> tblBusinessPartnerList = new List<TblBusinessPartner>();
            TblBusinessPartner tblBusinessPartner = new TblBusinessPartner();
            UserViewModel userVM = null;
            int result = 0; int BpResult = 0;
            try
            {
                if (businessUnitVM != null && businessUnitVM.BusinessUnitId > 0 && businessUnitVM.BusinessPartnerVMList != null && businessUnitVM.BusinessPartnerVMList.Count > 0)
                {
                    var ValidatedBusinessPList = businessUnitVM.BusinessPartnerVMList.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
                    businessUnitVM.BusinessPartnerVMErrorList = businessUnitVM.BusinessPartnerVMList.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();
                    
                    tblBusinessPartnerList = _mapper.Map<List<BusinessPartnerVMExcelModel>, List<TblBusinessPartner>>(ValidatedBusinessPList);
                    if (tblBusinessPartnerList != null && tblBusinessPartnerList.Count > 0)
                    {
                        foreach (var item in ValidatedBusinessPList)
                        {
                            try
                            {
                                if (item.BusinessPartnerId > 0)
                                {
                                    tblBusinessPartner = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == item.BusinessPartnerId);
                                    if (tblBusinessPartner != null && tblBusinessPartner.BusinessPartnerId > 0)
                                    {
                                        //Code to update the tblBusinessPartner object
                                        #region Initialize tblBusinessPartner for Update
                                        tblBusinessPartner.Name = item.Name;
                                        tblBusinessPartner.Description = item.Description;
                                        tblBusinessPartner.StoreCode = item.StoreCode;
                                        tblBusinessPartner.ContactPersonFirstName = item.ContactPersonFirstName;
                                        tblBusinessPartner.ContactPersonLastName = item.ContactPersonLastName;
                                        tblBusinessPartner.PhoneNumber = item.PhoneNumber;
                                        tblBusinessPartner.Email = item.Email;
                                        tblBusinessPartner.AddressLine1 = item.AddressLine1;
                                        tblBusinessPartner.AddressLine2 = item.AddressLine2;
                                        tblBusinessPartner.Pincode = item.Pincode;
                                        tblBusinessPartner.City = item.City;
                                        tblBusinessPartner.State = item.State;
                                        tblBusinessPartner.IsActive = true;
                                        tblBusinessPartner.IsAbbbp = item.IsAbbbp;
                                        tblBusinessPartner.IsExchangeBp = item.IsExchangeBp;
                                        tblBusinessPartner.FormatName = item.FormatName;
                                        tblBusinessPartner.IsDealer = item.IsDealer;
                                        tblBusinessPartner.AssociateCode = item.AssociateCode;
                                        tblBusinessPartner.Bppassword = item.Bppassword;
                                        tblBusinessPartner.IsOrc = item.IsORC;
                                        tblBusinessPartner.IsDefferedSettlement = item.IsDefferedSettlement;
                                        tblBusinessPartner.SponsorName = item.CompanyName;
                                        tblBusinessPartner.IsDefferedAbb = item.IsDefferedAbb;
                                        tblBusinessPartner.IsD2c = item.IsD2c;
                                        tblBusinessPartner.IsVoucher = item.IsVoucher;
                                        tblBusinessPartner.VoucherType = Convert.ToInt32(item.VoucherType);
                                        tblBusinessPartner.ModifiedBy = loggedInUserId;
                                        tblBusinessPartner.ModifiedDate = _currentDatetime;
                                        #endregion
                                        _businessPartnerRepository.Update(tblBusinessPartner);
                                    }
                                }
                                else
                                {
                                    if (item.StoreCode != null)
                                    {
                                        tblBusinessPartner = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == businessUnitVM.BusinessUnitId && x.StoreCode == item.StoreCode);
                                        
                                        if (tblBusinessPartner != null)
                                        {
                                            //Code to update the tblBusinessPartner object
                                            #region Initialize tblBusinessPartner for Update
                                            tblBusinessPartner.Name = item.Name;
                                            tblBusinessPartner.Description = item.Description;
                                            tblBusinessPartner.StoreCode = item.StoreCode;
                                            tblBusinessPartner.ContactPersonFirstName = item.ContactPersonFirstName;
                                            tblBusinessPartner.ContactPersonLastName = item.ContactPersonLastName;
                                            tblBusinessPartner.PhoneNumber = item.PhoneNumber;
                                            tblBusinessPartner.Email = item.Email;
                                            tblBusinessPartner.AddressLine1 = item.AddressLine1;
                                            tblBusinessPartner.AddressLine2 = item.AddressLine2;
                                            tblBusinessPartner.Pincode = item.Pincode;
                                            tblBusinessPartner.City = item.City;
                                            tblBusinessPartner.State = item.State;
                                            tblBusinessPartner.IsActive = true;
                                            tblBusinessPartner.IsAbbbp = item.IsAbbbp;
                                            tblBusinessPartner.IsExchangeBp = item.IsExchangeBp;
                                            tblBusinessPartner.FormatName = item.FormatName;
                                            tblBusinessPartner.IsDealer = item.IsDealer;
                                            tblBusinessPartner.AssociateCode = item.AssociateCode;
                                            tblBusinessPartner.Bppassword = item.Bppassword;
                                            tblBusinessPartner.IsOrc = item.IsORC;
                                            tblBusinessPartner.IsDefferedSettlement = item.IsDefferedSettlement;
                                            tblBusinessPartner.SponsorName = item.CompanyName;
                                            tblBusinessPartner.IsDefferedAbb = item.IsDefferedAbb;
                                            tblBusinessPartner.IsD2c = item.IsD2c;
                                            tblBusinessPartner.IsVoucher = item.IsVoucher;
                                            tblBusinessPartner.VoucherType = Convert.ToInt32(item.VoucherType);
                                            tblBusinessPartner.ModifiedBy = loggedInUserId;
                                            tblBusinessPartner.ModifiedDate = _currentDatetime;
                                            #endregion
                                            _businessPartnerRepository.Update(tblBusinessPartner);
                                        }
                                        else
                                        {
                                            // Code for insert the list of tblBusinessPartner
                                            tblBusinessPartner = _mapper.Map<BusinessPartnerVMExcelModel, TblBusinessPartner>(item);
                                            tblBusinessPartner.BusinessUnitId = businessUnitVM.BusinessUnitId;
                                            tblBusinessPartner.CreatedBy = loggedInUserId;
                                            tblBusinessPartner.CreatedDate = _currentDatetime;
                                            _businessPartnerRepository.Create(tblBusinessPartner);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                item.Remarks += ex.Message + ", ";
                                businessUnitVM.BusinessPartnerVMErrorList.Add(item);
                            }
                        }
                        result = _businessPartnerRepository.SaveChanges();
                        if (result > 0)
                        {
                            var groupBPByEmail = ValidatedBusinessPList.Where(x => x.Remarks == "").GroupBy(x => x.Email).Select(x=>x.FirstOrDefault()).ToList();
                            if (groupBPByEmail != null )
                            {
                                foreach (var item in groupBPByEmail)
                                {
                                    userVM = new UserViewModel();
                                    #region Create User and Role for Business Unit (Company Dashboard)
                                    userVM = new UserViewModel();
                                    userVM.Name = item.Name;
                                    userVM.Email = item.Email;
                                    userVM.Phone = item.PhoneNumber;
                                    userVM.Password = item.Bppassword;
                                    userVM.RoleName = EnumHelper.DescriptionAttr(RoleEnum.DealerAdmin);
                                    userVM.MailTemplate = EmailTemplateConstant.NewUserAdded_User;
                                    userVM.MailSubject = "Wellcome Mail";
                                    BpResult = _userManager.ManageUserAndUserRole(userVM, loggedInUserId);
                                    #endregion
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "ManageBusinessPartnerBulk", ex);
            }
            return businessUnitVM;
        }
        #endregion
        #region Manage Price Master Upload By Excel
        /// <summary>
        /// Method to manage (Add/Edit) BusinessPartner
        /// </summary>
        /// <param name="BusinessPartnerVM">BusinessPartnerVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public BusinessUnitViewModel ManageExchangePriceMasterBulk(BusinessUnitViewModel businessUnitVM, int loggedInUserId)
        {
            List<TblPriceMaster> tblPriceMasterList = new List<TblPriceMaster>();
            TblPriceMaster tblPriceMaster = new TblPriceMaster();
            TblProductCategory tblProductCat = new TblProductCategory();
            TblProductType tblProductType = new TblProductType();
            TblProductType tblProductTypeCode = new TblProductType();
            TblBrand tblBrand1 = new TblBrand(); TblBrand tblBrand2 = new TblBrand(); TblBrand tblBrand3 = new TblBrand(); TblBrand tblBrand4 = new TblBrand();
            int result = 0;
            try
            {
                if (businessUnitVM != null && businessUnitVM.BusinessUnitId > 0 && businessUnitVM.ExchangePriceMasterVMList != null && businessUnitVM.ExchangePriceMasterVMList.Count > 0)
                {
                    var ValidatedPriceMasterList = businessUnitVM.ExchangePriceMasterVMList.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
                    businessUnitVM.ExchangePriceMasterVMErrorList = businessUnitVM.ExchangePriceMasterVMList.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();

                    tblPriceMasterList = _mapper.Map<List<PriceMasterVMExcel>, List<TblPriceMaster>>(ValidatedPriceMasterList);
                    if (tblPriceMasterList != null && tblPriceMasterList.Count > 0)
                    {
                        var login = _loginRepository.GetSingle(x => x.SponsorId == businessUnitVM.BusinessUnitId);
                        if (login != null)
                        {
                            void demo()
                            {

                            }
                            foreach (var item in ValidatedPriceMasterList)
                            {
                                try
                                {
                                    #region Product Validations
                                    if (!String.IsNullOrWhiteSpace(item.ProductCat))
                                    {
                                        tblProductCat = _productCategoryRepository.GetSingle(x => x.IsActive == true && x.Name.ToLower() == item.ProductCat.ToLower());
                                        if (tblProductCat == null)
                                        {
                                            item.Remarks += "Product Category Not found" + ", ";
                                        }
                                    }
                                    else { item.Remarks += "Product Category is Required" + ", "; }
                                    if (!String.IsNullOrWhiteSpace(item.ProductType))
                                    {
                                        tblProductType = _productTypeRepository.GetSingle(x => x.IsActive == true && x.Description.ToLower() == item.ProductType.ToLower());
                                        if (tblProductType == null)
                                        {
                                            item.Remarks += "Product Type Not found" + ", ";
                                        }
                                    }
                                    else { item.Remarks += "Product Type is Required" + ", "; }
                                    if (!String.IsNullOrWhiteSpace(item.ProductTypeCode))
                                    {
                                        tblProductTypeCode = _productTypeRepository.GetSingle(x => x.IsActive == true && x.Code.ToLower() == item.ProductTypeCode.ToLower());
                                        if (tblProductTypeCode == null)
                                        {
                                            item.Remarks += "Product Type Code Not found" + ", ";
                                        }
                                    }
                                    else { item.Remarks += "Product Type Code is Required" + ", "; }
                                    if (String.IsNullOrWhiteSpace(item.Remarks) && tblProductType != null && tblProductCat != null)
                                    {
                                        if (tblProductType.ProductCatId != tblProductCat.Id)
                                        {
                                            item.Remarks += "Product Category and Product Type Mismatch" + ", ";
                                        }
                                        if (tblProductType.Id != tblProductTypeCode.Id)
                                        {
                                            item.Remarks += "Product Type and Product Type Code Mismatch" + ", ";
                                        }
                                    }
                                    #endregion
                                    #region BrandName Validations
                                    if (!String.IsNullOrWhiteSpace(item.BrandName1))
                                    {
                                        tblBrand1 = _brandRepository.GetSingle(x => x.IsActive == true && x.Name == item.BrandName1);
                                        if (tblBrand1 == null)
                                        {
                                            item.Remarks += "BrandName-1 Not found" + ", ";
                                        }
                                    }
                                    else { item.Remarks += "BrandName-1 is Required" + ", "; }
                                    if (!String.IsNullOrWhiteSpace(item.BrandName2))
                                    {
                                        tblBrand2 = _brandRepository.GetSingle(x => x.IsActive == true && x.Name == item.BrandName2);
                                        if (tblBrand2 == null)
                                        {
                                            item.Remarks += "BrandName-2 Not found" + ", ";
                                        }
                                    }
                                    else { item.Remarks += "BrandName-2 is Required" + ", "; }
                                    if (!String.IsNullOrWhiteSpace(item.BrandName3))
                                    {
                                        tblBrand3 = _brandRepository.GetSingle(x => x.IsActive == true && x.Name == item.BrandName3);
                                        if (tblBrand3 == null)
                                        {
                                            item.Remarks += "BrandName-3 Not found" + ", ";
                                        }
                                    }
                                    else { item.Remarks += "BrandName-3 is Required" + ", "; }
                                    if (!String.IsNullOrWhiteSpace(item.BrandName4))
                                    {
                                        tblBrand4 = _brandRepository.GetSingle(x => x.IsActive == true && x.Name == item.BrandName4);
                                        if (tblBrand4 == null)
                                        {
                                            item.Remarks += "BrandName-4 Not found" + ", ";
                                        }
                                    }
                                    else { item.Remarks += "BrandName-4 is Required" + ", "; }
                                    if (tblBrand1 != null && tblBrand2 != null && tblBrand3 != null && tblBrand4 != null)
                                    {
                                        if ((tblBrand1.Id == tblBrand2.Id || tblBrand2.Id == tblBrand3.Id || tblBrand1.Id == tblBrand3.Id)
                                            || (tblBrand1.Name != "Others" && tblBrand2.Name != "Others") && tblBrand3.Name != "Others" && tblBrand4.Name != "Others")
                                        {
                                            item.Remarks += "BrandName Must be Distinct or Select BrandName Others" + ", ";
                                        }
                                    }
                                    #endregion
                                    #region Product Amount Validations
                                    if (item.QuotePHigh < item.QuoteQHigh)
                                    {
                                        item.Remarks += "QuotePHigh field value must be greater then QuoteQHigh value" + ", ";
                                    }
                                    if (item.QuoteQHigh < item.QuoteRHigh)
                                    {
                                        item.Remarks += "QuoteQHigh field value must be greater then QuoteRHigh value" + ", ";
                                    }
                                    if (item.QuoteRHigh < item.QuoteSHigh)
                                    {
                                        item.Remarks += "QuoteRHigh field value must be greater then QuoteSHigh value" + ", ";
                                    }
                                    if (item.QuoteSHigh < item.QuoteRHigh)
                                    {
                                        item.Remarks += "QuoteSHigh field value must be less then QuoteRHigh value" + ", ";
                                    }
                                    if (item.QuoteP < item.QuoteQ)
                                    {
                                        item.Remarks += "QuoteP field value must be greater then QuoteQ value" + ", ";
                                    }
                                    if (item.QuoteQ < item.QuoteR)
                                    {
                                        item.Remarks += "QuoteQ field value must be greater then QuoteR value" + ", ";
                                    }
                                    if (item.QuoteR < item.QuoteS)
                                    {
                                        item.Remarks += "QuoteR field value must be greater then QuoteS value" + ", ";
                                    }
                                    if (item.QuoteS < item.QuoteR)
                                    {
                                        item.Remarks += "QuoteS field value must be less then QuoteR value" + ", ";
                                    }
                                    #endregion
                                    #region Date validations
                                    if (!String.IsNullOrWhiteSpace(item.PriceStartDate))
                                    {
                                        var priceStateDateStr = item.PriceStartDate;
                                        var priceEndDateStr = item.PriceEndDate;
                                        DateTime dtPriceStartDate; DateTime dtPriceEndDate;
                                        var CheckPriceStartDate = DateTime.TryParse(priceStateDateStr, out dtPriceStartDate);
                                        var CheckPriceEndDate = DateTime.TryParse(priceEndDateStr, out dtPriceStartDate);

                                        if (CheckPriceStartDate && CheckPriceEndDate)
                                        {
                                            var PriceStartDate = Convert.ToDateTime(priceStateDateStr);
                                            var PriceEndDate1 = Convert.ToDateTime(priceEndDateStr);
                                            DateTime priceStartDate = DateTime.Parse(priceStateDateStr);
                                            DateTime priceEndDate = DateTime.Parse(priceEndDateStr);
                                        }
                                        /*else
                                        {
                                            item.Remarks += "Invalid Date Format" + ", ";
                                        }
                                        var PriceEndDate = DateTime.TryParse(item.PriceStartDate, out dtPriceEndDate);
                                        if (PriceEndDate < PriceStartDate)
                                        {
                                            item.Remarks += "Price End Date must be Greater then Price Start Date" + ", ";
                                        }
                                        if (PriceEndDate < _currentDatetime.ToString("dd/MM/yyyy"))
                                        {
                                            item.Remarks += "Price End Date must be Greater then Current Date" + ", ";
                                        }*/
                                    }
                                    else { item.Remarks += "Price Start Date is Required" + ", "; }
                                    #endregion
                                    #region Save Price Master Details
                                    if (String.IsNullOrWhiteSpace(item.Remarks) && tblProductType != null && tblProductCat != null && tblBrand1 != null && tblBrand2 != null && tblBrand3 != null && tblBrand4 != null) 
                                    {
                                        tblPriceMaster = _priceMasterRepository.GetSingle(x=>x.IsActive == true && x.ExchPriceCode == login.PriceCode 
                                        && x.ProductCategoryId == tblProductTypeCode.ProductCatId && x.ProductTypeId == tblProductTypeCode.Id);
                                        if (tblPriceMaster != null && tblPriceMaster.Id > 0)
                                        {
                                            #region Initialize tblPriceMaster Details for Update
                                            tblPriceMaster.BrandName1 = tblBrand1.Name;
                                            tblPriceMaster.BrandName2 = tblBrand2.Name;
                                            tblPriceMaster.BrandName3 = tblBrand3.Name;
                                            tblPriceMaster.BrandName4 = tblBrand4.Name;
                                            tblPriceMaster.QuotePHigh = item.QuotePHigh.ToString();
                                            tblPriceMaster.QuoteQHigh = item.QuoteQHigh.ToString();
                                            tblPriceMaster.QuoteRHigh = item.QuoteRHigh.ToString();
                                            tblPriceMaster.QuoteSHigh = item.QuoteSHigh.ToString();
                                            tblPriceMaster.QuoteP = item.QuoteP.ToString();
                                            tblPriceMaster.QuoteQ = item.QuoteQ.ToString();
                                            tblPriceMaster.QuoteR = item.QuoteR.ToString();
                                            tblPriceMaster.QuoteS = item.QuoteS.ToString();
                                            tblPriceMaster.PriceStartDate = item.PriceStartDate;
                                            tblPriceMaster.PriceEndDate = item.PriceEndDate;
                                            tblPriceMaster.OtherBrand = item.OtherBrand;
                                            tblPriceMaster.IsActive = item.IsActive;
                                            tblPriceMaster.ModifiedBy = loggedInUserId;
                                            tblPriceMaster.ModifiedDate = _currentDatetime;
                                            #endregion
                                            _priceMasterRepository.Update(tblPriceMaster);
                                        }
                                        else
                                        {
                                            #region Initialize tblPriceMaster Details for Create
                                            tblPriceMaster = new TblPriceMaster();
                                            tblPriceMaster = _mapper.Map<PriceMasterVMExcel, TblPriceMaster>(item);
                                            tblPriceMaster.ExchPriceCode = login.PriceCode;
                                            tblPriceMaster.ProductCategoryId = tblProductTypeCode.ProductCatId;
                                            tblPriceMaster.ProductTypeId = tblProductTypeCode.Id;
                                            tblPriceMaster.CreatedBy = loggedInUserId;
                                            tblPriceMaster.CreatedDate = _currentDatetime;
                                            #endregion
                                            _priceMasterRepository.Create(tblPriceMaster);
                                        }
                                    }
                                    else
                                    {
                                        businessUnitVM.ExchangePriceMasterVMErrorList.Add(item);
                                    }
                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    item.Remarks += ex.Message + ", ";
                                    businessUnitVM.ExchangePriceMasterVMErrorList.Add(item);
                                }
                            }
                            result = _priceMasterRepository.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "ManageBusinessPartnerBulk", ex);
            }
            return businessUnitVM;
        }
        #endregion
        #endregion

        #region Get Count of Bu onboarding list data
        /// <summary>
        /// Method to get the BusinessPartner List Count and Exchange Price mater list Count by BuId 
        /// </summary>
        /// <param name="id">BuId</param>
        /// <returns>List<BusinessPartnerViewModel></returns>
        public CountBUOnboardingListDataVM GetCountBUOnboardingListData(int BUId)
        {
            CountBUOnboardingListDataVM CountListData = new CountBUOnboardingListDataVM();

            var tblBusinessPartnerList = 0;
            var tblPriceMasterList = 0;
            Login BuLoginPriceCode = null;
            var AbbPlanMasterList = 0;
            var AbbPriceMasterList = 0;
            try
            {
                tblBusinessPartnerList = _businessPartnerRepository.GetList(x => x.IsActive == true && x.BusinessUnitId == BUId).Count();
                BuLoginPriceCode = _loginRepository.GetSingle(x => x.SponsorId == BUId);
                AbbPlanMasterList = _aBBPlanMasterRepository.GetList(x => x.IsActive == true && x.BusinessUnitId == BUId).Count();
                AbbPriceMasterList = _aBBPriceMasterRepository.GetList(x => x.IsActive == true && x.BusinessUnitId == BUId).Count();

                if (BuLoginPriceCode != null && BuLoginPriceCode.PriceCode != null)
                {
                    tblPriceMasterList = _priceMasterRepository.GetList(x => x.IsActive == true && x.ExchPriceCode == BuLoginPriceCode.PriceCode).Count();
                }

                CountListData.BPListCount = tblBusinessPartnerList == null ? 0 : tblBusinessPartnerList;
                CountListData.ExchPriceMasterListCount = tblPriceMasterList == null ? 0 : tblPriceMasterList;
                CountListData.ABBPlanMasterListCount = AbbPlanMasterList == null ? 0 : AbbPlanMasterList;
                CountListData.ABBPriceMasterListCount = AbbPriceMasterList == null ? 0 : AbbPriceMasterList;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "GetCountBUOnboardingListData", ex);
            }
            return CountListData;
        }
        #endregion

        #region Store ABB and Exchange Product Category Mapping for BU (New Products only)
        /// <summary>
        /// Store ABB and Exchange Product Category Mapping for BU (New Products only)
        /// </summary>
        /// <param name="BusinessUnitVM"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public int SaveBuProductCategoriesForABBandExch(BusinessUnitViewModel BusinessUnitVM, int loggedInUserId)
        {
           
            int result = 0;
            TblBuproductCategoryMapping tblBuproductCategoryMapping = null;
            try
            {
                if (BusinessUnitVM != null && BusinessUnitVM.BusinessUnitId > 0 && BusinessUnitVM.ProductCategoryVMList != null && BusinessUnitVM.ProductCategoryVMList.Count > 0)
                {
                    foreach (var item in BusinessUnitVM.ProductCategoryVMList)
                    {
                        tblBuproductCategoryMapping = new TblBuproductCategoryMapping();
                        tblBuproductCategoryMapping = _bUProductCategoryMapping.GetSingle(x => x.IsActive == true && x.BusinessUnitId == BusinessUnitVM.BusinessUnitId && x.ProductCatId == item.Id);
                        if (tblBuproductCategoryMapping != null && item.Selected == false)
                        {
                            tblBuproductCategoryMapping.IsAbb = BusinessUnitVM.IsAbb;
                            tblBuproductCategoryMapping.IsExchange = BusinessUnitVM.IsExchange;
                            tblBuproductCategoryMapping.IsActive = false;
                            tblBuproductCategoryMapping.ModifiedBy = loggedInUserId;
                            tblBuproductCategoryMapping.ModifiedDate = _currentDatetime;
                            _bUProductCategoryMapping.Update(tblBuproductCategoryMapping);
                        }
                        else
                        {
                            if (item.Buid > 0 && item.Id > 0 && item.Selected == true)
                            {
                                tblBuproductCategoryMapping = new TblBuproductCategoryMapping();
                                tblBuproductCategoryMapping.IsAbb = BusinessUnitVM.IsAbb;
                                tblBuproductCategoryMapping.IsExchange = BusinessUnitVM.IsExchange;
                                tblBuproductCategoryMapping.ProductCatId = item.Id;
                                tblBuproductCategoryMapping.BusinessUnitId = item.Buid;
                                tblBuproductCategoryMapping.IsActive = true;
                                tblBuproductCategoryMapping.CreatedBy = loggedInUserId;
                                tblBuproductCategoryMapping.CreatedDate = _currentDatetime;
                                _bUProductCategoryMapping.Create(tblBuproductCategoryMapping);
                            }
                        }
                    }
                    result = _bUProductCategoryMapping.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "SaveBuProductCategoriesForABBandExch", ex);
            }
            return result;
        }
        #endregion

        #region Get Product Category list for Bu Product category Mapping
        /// <summary>
        /// Get Product Category list for Bu Product category Mapping
        /// </summary>     
        /// <returns>ProductCategoryViewModel</returns>
        public IList<ProductCategoryViewModel> GetAllProductCategoryBuMapping(int BUId)
        {
            IList<ProductCategoryViewModel> ProductCategoryVMList = null;
            List<TblProductCategory> TblProductCategorylist = new List<TblProductCategory>();
            List<TblBuproductCategoryMapping> tblBuproductCategoryMappings = null;
            List<TblBuproductCategoryMapping> tblBuproductCategoryMappings1 = null;
            // TblUseRole TblUseRole = null;
            try
            {
                TblProductCategorylist = _productCategoryRepository.GetList(x => x.IsActive == true).ToList();
                tblBuproductCategoryMappings = _bUProductCategoryMapping.GetList(x => x.IsActive == true && x.BusinessUnitId == BUId).ToList();

                //var catGroup = tblBuproductCategoryMappings.GroupBy(x=>x.ProductCat).Select(x=>x.ProductCatId).ToList();
                

                if (TblProductCategorylist != null && TblProductCategorylist.Count > 0)
                {
                    ProductCategoryVMList = _mapper.Map<IList<TblProductCategory>, IList<ProductCategoryViewModel>>(TblProductCategorylist);

                    if (tblBuproductCategoryMappings != null)
                    {
                        foreach (var item in tblBuproductCategoryMappings)
                        {
                            if (ProductCategoryVMList.FirstOrDefault(x => x.Id == item.ProductCatId) != null)
                            {
                                ProductCategoryVMList.FirstOrDefault(x => x.Id == item.ProductCatId).Selected = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "GetAllProductCategoryBuMapping", ex);
            }
            return ProductCategoryVMList;
        }
        #endregion

        #region Get Login Details for BU Api Configurations
        /// <summary>
        /// Get Login Details for BU Api Configurations
        /// </summary>
        /// <param name="BUId"></param>
        /// <returns></returns>
        public BusinessUnitLoginVM GetBULoginCredentials(int BUId)
        {
            BusinessUnitLoginVM BULoginVM = null;
            Login login = null;
            try
            {
                if (BUId > 0)
                {
                    login = _loginRepository.GetSingle(x=>x.SponsorId == BUId);
                    if (login != null)
                    {
                        BULoginVM = _mapper.Map<Login, BusinessUnitLoginVM > (login);
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "GetBULoginCredentials", ex);
            }
            return BULoginVM;
        }
        #endregion

        #region Store Login Creadentials for BU Dashboard, BU Api and Business Partner
        /// <summary>
        /// Store Login Creadentials for BU Dashboard, BU Api and Business Partner
        /// </summary>
        /// <param name="BusinessUnitVM"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public int SaveBUCredentials(BusinessUnitViewModel BusinessUnitVM, int loggedInUserId)
        {
            int result = 0;
            TblUser tblUser = null;
            TblRole tblRole = null;
            TblUserRole tblUserRole= null;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            Login login = null;
            UserViewModel userVM = null; int BuResult = 0;
            try
            {
                if (BusinessUnitVM != null && BusinessUnitVM.BusinessUnitId > 0 && BusinessUnitVM.BuLoginVM != null)
                {
                    tblBusinessUnit = _businessUnitRepository.GetSingle(x=>x.IsActive == true && x.BusinessUnitId == BusinessUnitVM.BusinessUnitId);
                    if (tblBusinessUnit != null)
                    {
                        login = _loginRepository.GetSingle(x => x.Id == BusinessUnitVM.BuLoginVM.Id && x.SponsorId == BusinessUnitVM.BusinessUnitId);
                        if (login != null)
                        {
                            login.Username = BusinessUnitVM.BuLoginVM.Username;
                            login.Password = BusinessUnitVM.BuLoginVM.Password;
                            login.PriceMasterNameId = BusinessUnitVM.BuLoginVM.PriceMasterNameId;
                            login.BusinessPartnerId = BusinessUnitVM.BuLoginVM.BusinessPartnerId;
                            _loginRepository.Update(login);
                        }
                        else
                        {
                            login = new Login();
                            login = _mapper.Map<BusinessUnitLoginVM, Login>(BusinessUnitVM.BuLoginVM);
                            _loginRepository.Create(login);
                        }
                        result = _loginRepository.SaveChanges();
                        if (result > 0)
                        {
                            #region Update Login Id into tblBusinessUnit
                            tblBusinessUnit.LoginId = login.Id;
                            _businessUnitRepository.Update(tblBusinessUnit);
                            _businessUnitRepository.SaveChanges();
                            #endregion
                            #region Create User and Role for Business Unit (Company Dashboard)
                            userVM = new UserViewModel();
                            userVM.Name = BusinessUnitVM.Name;
                            userVM.Email = BusinessUnitVM.Email;
                            userVM.Phone = BusinessUnitVM.CompanyVM.Phone;
                            userVM.Password = BusinessUnitVM.BuLoginVM.Password;
                            userVM.BusinessUnitId = BusinessUnitVM.BusinessUnitId;
                            userVM.RoleName = EnumHelper.DescriptionAttr(RoleEnum.SponsorAdmin);
                            userVM.MailTemplate = EmailTemplateConstant.NewUserAdded_User;
                            userVM.MailSubject = "Wellcome Mail";
                            BuResult = _userManager.ManageUserAndUserRole(userVM, loggedInUserId);
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "SaveBUCredentials", ex);
            }
            return result;
        }
        #endregion

        #region Save QR Code Image
        public bool GenerateBUQRCode(BusinessUnitLoginVM BULoginVM, string URL, string customFileName)
        {
            TblBusinessUnit tblBusinessUnit = null;
            try
            {
                if (BULoginVM != null && BULoginVM.SponsorId > 0)
                {
                    tblBusinessUnit = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == BULoginVM.SponsorId);
                    if (tblBusinessUnit != null)
                    {
                        QRCodeGenerator QrGenerator = new QRCodeGenerator();
                        QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(URL, QRCodeGenerator.ECCLevel.Q);
                        PngByteQRCode pngByteQRCode = new PngByteQRCode(QrCodeInfo);
                        byte[] qrCodeBytes = pngByteQRCode.GetGraphic(20);
                        string qrCodeString = Convert.ToBase64String(qrCodeBytes);



                        //byte[] BitmapArray = QrBitmap.BitmapToByteArray();
                        //string base64String = Convert.ToBase64String(BitmapArray);
                        string QrUri = string.Format("data:image/png;base64,{0}", qrCodeString);
                        var filename = customFileName + "_" + BULoginVM.SponsorId + ".jpg";
                        var filePath = EnumHelper.DescriptionAttr(FilePathEnum.QRImage);
                        bool QRSaved = _imageHelper.SaveFileFromBase64(qrCodeString, filePath, filename);
                        if (QRSaved)
                        {
                            tblBusinessUnit.QrcodeUrl = filename;
                            _businessUnitRepository.Update(tblBusinessUnit);
                            _businessUnitRepository.SaveChanges();
                            BULoginVM.QrCodeNameBU = filename;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "SaveBUCredentials", ex);
            }
            return true;
        }
        public bool GenerateBPBUQRCode(BusinessUnitLoginVM BULoginVM, string URL, string customFileName)
        {
            TblBusinessUnit tblBusinessUnit = null;
            TblBusinessPartner tblBusinessPartner = null;
            try
            {
                if (BULoginVM != null && BULoginVM.SponsorId > 0)
                {
                    tblBusinessUnit = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == BULoginVM.SponsorId);
                    tblBusinessPartner = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == BULoginVM.BusinessPartnerId);
                    if (tblBusinessUnit != null && tblBusinessPartner != null)
                    {
                        QRCodeGenerator QrGenerator = new QRCodeGenerator();
                        QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(URL, QRCodeGenerator.ECCLevel.Q);
                        PngByteQRCode pngByteQRCode = new PngByteQRCode(QrCodeInfo);
                        byte[] qrCodeBytes = pngByteQRCode.GetGraphic(20);
                        string qrCodeString = Convert.ToBase64String(qrCodeBytes);



                        //byte[] BitmapArray = QrBitmap.BitmapToByteArray();
                        //string base64String = Convert.ToBase64String(BitmapArray);
                        string QrUri = string.Format("data:image/png;base64,{0}", qrCodeString);
                        var filename = customFileName + "_" + BULoginVM.SponsorId + ".jpg";
                        var filePath = EnumHelper.DescriptionAttr(FilePathEnum.QRImage);
                        bool QRSaved = _imageHelper.SaveFileFromBase64(qrCodeString, filePath, filename);
                        if (QRSaved)
                        {
                            tblBusinessPartner.QrcodeUrl = filename;
                            _businessPartnerRepository.Update(tblBusinessPartner);
                            _businessPartnerRepository.SaveChanges();
                            BULoginVM.QrCodeNameBPBU = filename;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "SaveBUCredentials", ex);
            }
            return true;
        }

        #endregion

        #region Start Tab Pending Orders Reporting 

        #region Entry Point Pending Order List Scheduler Calling
        public bool? ReportingPendingOrders()
        {
            List<TblBusinessUnit>? businessUnitList = null;
            bool? flag = false;
            int errorResultCount = 0;
            try
            {
                businessUnitList = _businessUnitRepository.GetSponsorListForReporting();
                if (businessUnitList != null && businessUnitList.Count > 0)
                {
                    foreach (TblBusinessUnit tblBusinessUnit in businessUnitList)
                    {
                        flag = SendReportingMail(tblBusinessUnit);
                        if (flag == false)
                        {
                            errorResultCount++;
                        }
                    }
                }
                if (errorResultCount > 0)
                {
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "ReportingPendingOrders", ex);
            }
            return flag;
        }
        #endregion

        #region Create Html string for integrate pending orders reporting mail template Added by VK
        /// <summary>
        /// Create Html string for integrate pending orders reporting mail template Added by VK
        /// </summary>
        /// <param name="sponsorName"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns></returns>
        public string GetReportingMailHtmlString(string? sponsorName, string HtmlTemplateNameOnly)
        {
            #region Variable Declaration
            string htmlString = "";
            string fileName = HtmlTemplateNameOnly + ".html";
            string fileNameWithPath = "";
            #endregion

            try
            {
                if (sponsorName != null && HtmlTemplateNameOnly != null)
                {
                    string? supportEmail = _config.Value.SupportEmail;
                    //string supportConNo = "";
                    #region Get Html String Dynamically
                    var filePath = string.Concat(_webHostEnvironment.WebRootPath, @"\MailTemplates");
                    fileNameWithPath = string.Concat(filePath, "\\", fileName);
                    htmlString = File.ReadAllText(fileNameWithPath);
                    #endregion

                    if (HtmlTemplateNameOnly == TemplateConfigConstant.PendingOrderMailTempName)
                        htmlString = htmlString.Replace("[SponsorName]", sponsorName)
                            .Replace("[SupportEmail]", supportEmail);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "GetReportingMailHtmlString", ex);
            }
            return htmlString;
        }
        #endregion

        #region Send Reporting Mails for Pending Orders Added by VK
        /// <summary>
        /// Send Reporting Mails for Pending Orders Added by VK
        /// </summary>
        /// <param name="ABBRegistrationViewModel"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns></returns>
        public bool? SendReportingMail(TblBusinessUnit tblBusinessUnit)
        {
            #region Variable Declaration
            string mailTempName = "";
            string mailSubject = "";
            Task<bool>? IsMailSent = null;
            bool? flag = false;
            IList<TblConfiguration>? tblConfigurationList = null;
            string? ABB_Bcc = null;
            TemplateViewModel templateVM = new TemplateViewModel();
            OrderDetailsVMExcelList? orderDetailsListExch = null;
            OrderDetailsVMExcelList? orderDetailsListAbb = null;
            byte[]? byteArrayExch = null;
            byte[]? byteArrayAbb = null;
            #endregion

            try
            {
                mailTempName = TemplateConfigConstant.PendingOrderMailTempName;
                mailSubject = TemplateConfigConstant.PendingOrderMailSubject;

                if (tblBusinessUnit != null)
                {
                    if (tblBusinessUnit.IsReportingOn == true)
                    {
                        #region Get List of Pending Orders For Exchange
                        orderDetailsListExch = GetExchPendingOrdersList(tblBusinessUnit);
                        if (orderDetailsListExch != null)
                        {
                            byteArrayExch = OnPostExportExcel_PendingOrders(orderDetailsListExch);
                        }
                        #endregion

                        #region Get List of Pending Orders for ABB
                        orderDetailsListAbb = GetABBPendingOrdersList(tblBusinessUnit);
                        if (orderDetailsListAbb != null)
                        {
                            byteArrayAbb = OnPostExportExcel_PendingOrders(orderDetailsListAbb);
                        }
                        #endregion

                        #region Code for Get Data from TblConfiguration
                        tblConfigurationList = _templateConfigurationRepository.GetConfigurationList();
                        if (tblConfigurationList != null && tblConfigurationList.Count > 0)
                        {
                            var GetBcc = tblConfigurationList.FirstOrDefault(x => x.Name == ConfigurationEnum.ABB_Bcc.ToString());
                            if (GetBcc != null && GetBcc.Value != null)
                            {
                                ABB_Bcc = GetBcc.Value.Trim();
                            }
                        }
                        #endregion

                        #region Send Mail to Sponsor
                        if (byteArrayExch != null || byteArrayAbb != null)
                        {
                            var Date = System.DateTime.Now;
                            var dateTime = Date.ToString("MM-dd-yyyy_hh:mm");
                            templateVM.HtmlBody = GetReportingMailHtmlString(tblBusinessUnit.Name, mailTempName);
                            var FileNameExch = "PendingOrdersExc" + "_" + dateTime + ".xlsx";
                            var FileNameAbb = "PendingOrdersAbb" + "_" + dateTime + ".xlsx";
                            templateVM.AttachFileNameExch = FileNameExch;
                            templateVM.AttachFileNameAbb = FileNameAbb;
                            templateVM.byteArrayExch = byteArrayExch;
                            templateVM.byteArrayAbb = byteArrayAbb;
                            templateVM.To = tblBusinessUnit.Email;
                            templateVM.Cc = tblBusinessUnit.ReportEmails;
                            templateVM.Bcc = ABB_Bcc;
                            templateVM.Subject = mailSubject;
                            IsMailSent = _mailManager.JetMailSendWithAttachedFile(templateVM);
                            if (IsMailSent.Result)
                            {
                                flag = IsMailSent.Result;
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "SendReportingMail", ex);
            }
            return flag;
        }
        #endregion

        #region Export Data To Excel 
        public byte[]? OnPostExportExcel_PendingOrders(OrderDetailsVMExcelList? obj)
        {
            byte[]? data = null;
            try
            {
                if (obj != null)
                {
                    MemoryStream stream = ExcelExportHelper.MultiListExportToExcel(obj.pendingForQCList, obj.pendingForPriceAcceptanceList, obj.pendingForPickupList);
                    data = stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "ExportExcel_PendingOrders", ex);
            }
            return data;
        }
        #endregion
        
        #region Manage Pending Orders Reporting Exchange
        public OrderDetailsVMExcelList GetExchPendingOrdersList(TblBusinessUnit? tblBusinessUnit, int? spid = null)
        {
            #region Variable Declaration
            OrderDetailsVMExcelList? obj = new OrderDetailsVMExcelList();
            DateTime? DateTimeByElapsedHours = null;
            List<TblOrderTran>? obj1 = null;
            List<TblOrderTran>? obj2 = null;
            List<TblLogistic>? obj3 = null;
            int? ElapsedHrs = 0; int? buid = null;
            PendingForQCVMExcel? pendingForQCVMExcel = null;
            #endregion

            try
            {
                #region Elapsed Hours from the Current Dates
                if (tblBusinessUnit != null)
                {
                    DateTime DateTimeByElapsedHours1 = DateTime.Now;
                    ElapsedHrs = tblBusinessUnit.OrderPendingTimeH;
                    DateTimeByElapsedHours = DateTimeByElapsedHours1.AddHours(-(ElapsedHrs ?? 0));
                    buid = tblBusinessUnit.BusinessUnitId;
                }
                #endregion

                #region Order Status
                int statusId1 = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                int statusId2 = Convert.ToInt32(OrderStatusEnum.QCInProgress_3Q);
                int statusId3 = Convert.ToInt32(OrderStatusEnum.CallAndGoScheduledAppointmentTaken_3P);
                int statusId4 = Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled);
                int statusId5 = Convert.ToInt32(OrderStatusEnum.ReopenOrder);
                int statusId6 = Convert.ToInt32(OrderStatusEnum.InstalledbySponsor);
                int statusId7 = Convert.ToInt32(OrderStatusEnum.OrderWithDiagnostic);
                int statusId8 = Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval);
                int statusId9 = Convert.ToInt32(OrderStatusEnum.QCByPass);
                int statusId10 = Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated);
                #endregion

                if (buid > 0)
                {
                    obj.pendingForQCList = GetExchPendingQCList(buid, DateTimeByElapsedHours, statusId1
                        , statusId2, statusId3, statusId4, statusId5, statusId6, statusId7);
                    obj.pendingForPriceAcceptanceList = GetExchPendingPriceAcceptList(buid, DateTimeByElapsedHours, statusId8, statusId9);
                    obj.pendingForPickupList = GetExchPendingPickupList(buid, spid, DateTimeByElapsedHours, statusId10);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "GetExchPendingOrdersList", ex);
            }
            return obj;
        }
        #endregion

        #region Manage Pending Orders Reporting ABB
        public OrderDetailsVMExcelList GetABBPendingOrdersList(TblBusinessUnit? tblBusinessUnit, int? spid = null)
        {
            #region Variable Declaration
            OrderDetailsVMExcelList? obj = new OrderDetailsVMExcelList();
            DateTime? DateTimeByElapsedHours = null;
            List<TblOrderTran>? obj1 = null;
            List<TblOrderTran>? obj2 = null;
            List<TblLogistic>? obj3 = null;
            int? ElapsedHrs = 0; int? buid = null;
            #endregion

            try
            {
                #region Elapsed Hours from the Current Dates
                if (tblBusinessUnit != null)
                {
                    DateTime DateTimeByElapsedHours1 = DateTime.Now;
                    ElapsedHrs = tblBusinessUnit.OrderPendingTimeH;
                    DateTimeByElapsedHours = DateTimeByElapsedHours1.AddHours(-(ElapsedHrs ?? 0));
                    buid = tblBusinessUnit.BusinessUnitId;
                }
                #endregion

                #region Order Status
                int statusId1 = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                int statusId2 = Convert.ToInt32(OrderStatusEnum.QCInProgress_3Q);
                int statusId3 = Convert.ToInt32(OrderStatusEnum.CallAndGoScheduledAppointmentTaken_3P);
                int statusId4 = Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled);
                int statusId5 = Convert.ToInt32(OrderStatusEnum.ReopenOrder);
                int statusId6 = Convert.ToInt32(OrderStatusEnum.InstalledbySponsor);
                int statusId7 = Convert.ToInt32(OrderStatusEnum.OrderWithDiagnostic);
                int statusId8 = Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval);
                int statusId9 = Convert.ToInt32(OrderStatusEnum.QCByPass);
                int statusId10 = Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated);
                #endregion

                if (buid > 0)
                {
                    obj.pendingForQCList = GetAbbPendingQCList(buid, DateTimeByElapsedHours, statusId1
                        , statusId2, statusId3, statusId4, statusId5, statusId6, statusId7);
                    obj.pendingForPriceAcceptanceList = GetAbbPendingPriceAcceptList(buid, DateTimeByElapsedHours, statusId8, statusId9);
                    obj.pendingForPickupList = GetAbbPendingPickupList(buid, spid, DateTimeByElapsedHours, statusId10);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "GetABBPendingOrdersList", ex);
            }
            return obj;
        }
        #endregion

        #region Get List Pending for QC Exchange Orders list
        public List<PendingForQCVMExcel> GetExchPendingQCList(int? buid, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null, int? statusId2 = null, int? statusId3 = null, int? statusId4 = null,
            int? statusId5 = null, int? statusId6 = null, int? statusId7 = null)
        {
            #region Variable Declaration
            PendingForQCVMExcel? pendingForQCVMExcel = null;
            List<PendingForQCVMExcel>? pendingForQCVMList = new List<PendingForQCVMExcel>();
            List<TblOrderTran>? obj1 = null;
            bool? isFilterByModifiedDate = false;
            #endregion

            try
            {
                if (buid > 0)
                {
                    obj1 = _orderTransRepository.GetExchOrdersPendingList(buid, isFilterByModifiedDate, DateTimeByElapsedHours, statusId1
                        , statusId2, statusId3, statusId4, statusId5, statusId6, statusId7);
                    if (obj1 != null && obj1.Count > 0)
                    {
                        foreach (TblOrderTran tblOrderTran in obj1)
                        {
                            pendingForQCVMExcel = new PendingForQCVMExcel();
                            pendingForQCVMExcel.CompanyName = tblOrderTran?.Exchange?.CompanyName;
                            pendingForQCVMExcel.RegdNo = tblOrderTran?.RegdNo;
                            pendingForQCVMExcel.CustomerCity = tblOrderTran?.Exchange?.CustomerDetails?.City;
                            pendingForQCVMExcel.ProductCategory = tblOrderTran?.Exchange?.ProductType?.ProductCat?.Description;
                            pendingForQCVMExcel.ProductCondition = tblOrderTran?.Exchange?.ProductCondition;
                            pendingForQCVMExcel.StatusCode = tblOrderTran?.Status?.StatusCode;
                            pendingForQCVMExcel.OrderDueDateTime = tblOrderTran?.ModifiedDate != null ? Convert.ToDateTime(tblOrderTran?.ModifiedDate).ToString("dd/MM/yyyy hh:mm tt") : null;
                            pendingForQCVMExcel.OrderCreatedDate = tblOrderTran?.CreatedDate != null ? Convert.ToDateTime(tblOrderTran?.CreatedDate).ToString("dd/MM/yyyy") : null;
                            TblOrderQc tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran?.OrderTransId ?? 0);
                            if (tblOrderQc != null && tblOrderQc.ProposedQcdate != null)
                            {
                                pendingForQCVMExcel.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                pendingForQCVMExcel.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                            }
                            pendingForQCVMList.Add(pendingForQCVMExcel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "GetExchPendingOrdersList", ex);
            }
            return pendingForQCVMList;
        }
        #endregion

        #region Get List Pending for Price Acceptance Exchange Orders list
        public List<PendingForPriceAcceptVMExcel> GetExchPendingPriceAcceptList(int? buid, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null, int? statusId2 = null)
        {
            #region Variable Declaration
            PendingForPriceAcceptVMExcel? pendingForPriceAcceptVMExcel = null;
            List<PendingForPriceAcceptVMExcel>? pendingForPriceAcceptVMList = new List<PendingForPriceAcceptVMExcel>();
            List<TblOrderTran>? obj1 = null;
            bool? isFilterByModifiedDate = false;
            #endregion

            try
            {
                if (buid > 0)
                {
                    obj1 = _orderTransRepository.GetExchOrdersPendingList(buid, isFilterByModifiedDate, DateTimeByElapsedHours, statusId1
                        , statusId2);
                    if (obj1 != null && obj1.Count > 0)
                    {
                        foreach (TblOrderTran tblOrderTran in obj1)
                        {
                            pendingForPriceAcceptVMExcel = new PendingForPriceAcceptVMExcel();
                            pendingForPriceAcceptVMExcel.CompanyName = tblOrderTran?.Exchange?.CompanyName;
                            pendingForPriceAcceptVMExcel.RegdNo = tblOrderTran?.RegdNo;
                            pendingForPriceAcceptVMExcel.CustomerCity = tblOrderTran?.Exchange?.CustomerDetails?.City;
                            pendingForPriceAcceptVMExcel.ProductCategory = tblOrderTran?.Exchange?.ProductType?.ProductCat?.Description;
                            pendingForPriceAcceptVMExcel.ProductCondition = tblOrderTran?.Exchange?.ProductCondition;
                            pendingForPriceAcceptVMExcel.StatusCode = tblOrderTran?.Status?.StatusCode;
                            pendingForPriceAcceptVMExcel.OrderDueDateTime = tblOrderTran?.ModifiedDate != null ? Convert.ToDateTime(tblOrderTran?.ModifiedDate).ToString("dd/MM/yyyy hh:mm tt") : null;
                            pendingForPriceAcceptVMExcel.OrderCreatedDate = tblOrderTran?.CreatedDate != null ? Convert.ToDateTime(tblOrderTran?.CreatedDate).ToString("dd/MM/yyyy") : null;

                            //TblOrderQc tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran?.OrderTransId ?? 0);
                            //if (tblOrderQc != null && tblOrderQc.ProposedQcdate != null)
                            //{
                            //    pendingForPriceAcceptVMExcel.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                            //    pendingForPriceAcceptVMExcel.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                            //}
                            pendingForPriceAcceptVMList.Add(pendingForPriceAcceptVMExcel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "GetExchPendingOrdersList", ex);
            }
            return pendingForPriceAcceptVMList;
        }
        #endregion

        #region Get List Pending for Pickup Exchange Orders list
        public List<PendingForPickupVMExcel> GetExchPendingPickupList(int? buid, int? spid, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null)
        {
            #region Variable Declaration
            PendingForPickupVMExcel? pendingForPickupVMExcel = null;
            List<PendingForPickupVMExcel>? pendingForPickupVMList = new List<PendingForPickupVMExcel>();
            List<TblLogistic>? obj1 = null;
            #endregion

            try
            {
                if (buid > 0)
                {
                    obj1 = _orderTransRepository.GetExchOrdersPickupPendingList(buid, spid, DateTimeByElapsedHours, statusId1);
                    if (obj1 != null && obj1.Count > 0)
                    {
                        foreach (TblLogistic tblLogistic in obj1)
                        {
                            pendingForPickupVMExcel = new PendingForPickupVMExcel();
                            pendingForPickupVMExcel.CompanyName = tblLogistic?.OrderTrans?.Exchange?.CompanyName;
                            pendingForPickupVMExcel.RegdNo = tblLogistic?.RegdNo;
                            pendingForPickupVMExcel.ServicePartnerName = tblLogistic?.ServicePartner?.ServicePartnerBusinessName;
                            pendingForPickupVMExcel.CustomerCity = tblLogistic?.OrderTrans?.Exchange?.CustomerDetails?.City;
                            pendingForPickupVMExcel.ProductCategory = tblLogistic?.OrderTrans?.Exchange?.ProductType?.ProductCat?.Description;
                            pendingForPickupVMExcel.ProductCondition = tblLogistic?.OrderTrans?.Exchange?.ProductCondition;
                            pendingForPickupVMExcel.StatusCode = tblLogistic?.Status?.StatusCode;
                            //pendingForPickupVMExcel.OrderDueDateTime = tblLogistic?.OrderTrans?.CreatedDate != null ? tblLogistic?.OrderTrans?.CreatedDate.ToString() : null;
                            pendingForPickupVMExcel.TicketNumber = tblLogistic?.TicketNumber;
                            pendingForPickupVMExcel.PickupScheduleDate = tblLogistic?.PickupScheduleDate != null ? tblLogistic?.PickupScheduleDate.ToString() : null;
                            pendingForPickupVMExcel.OrderDueDateTime = tblLogistic?.CreatedDate != null ? Convert.ToDateTime(tblLogistic?.CreatedDate).ToString("dd/MM/yyyy hh:mm tt") : null;

                            pendingForPickupVMList.Add(pendingForPickupVMExcel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "GetExchPendingOrdersList", ex);
            }
            return pendingForPickupVMList;
        }
        #endregion

        #region Get List Pending for QC ABB Orders list
        public List<PendingForQCVMExcel> GetAbbPendingQCList(int? buid, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null, int? statusId2 = null, int? statusId3 = null, int? statusId4 = null,
            int? statusId5 = null, int? statusId6 = null, int? statusId7 = null)
        {
            #region Variable Declaration
            PendingForQCVMExcel? pendingForQCVMExcel = null;
            List<PendingForQCVMExcel>? pendingForQCVMList = new List<PendingForQCVMExcel>();
            List<TblOrderTran>? obj1 = null;
            bool? isFilterByModifiedDate = false;
            #endregion

            try
            {
                if (buid > 0)
                {
                    obj1 = _orderTransRepository.GetABBOrdersPendingList(buid, isFilterByModifiedDate, DateTimeByElapsedHours, statusId1
                        , statusId2, statusId3, statusId4, statusId5, statusId6, statusId7);
                    if (obj1 != null && obj1.Count > 0)
                    {
                        foreach (TblOrderTran tblOrderTran in obj1)
                        {
                            pendingForQCVMExcel = new PendingForQCVMExcel();
                            pendingForQCVMExcel.CompanyName = tblOrderTran?.Abbredemption?.Abbregistration?.BusinessUnit?.Name;
                            pendingForQCVMExcel.RegdNo = tblOrderTran?.RegdNo;
                            pendingForQCVMExcel.CustomerCity = tblOrderTran?.Abbredemption?.CustomerDetails?.City;
                            pendingForQCVMExcel.ProductCategory = tblOrderTran?.Abbredemption?.Abbregistration?.NewProductCategory?.DescriptionForAbb;
                            pendingForQCVMExcel.StatusCode = tblOrderTran?.Status?.StatusCode;
                            pendingForQCVMExcel.OrderDueDateTime = tblOrderTran?.ModifiedDate != null ? Convert.ToDateTime(tblOrderTran?.ModifiedDate).ToString("dd/MM/yyyy hh:mm tt") : null;
                            pendingForQCVMExcel.OrderCreatedDate = tblOrderTran?.CreatedDate != null ? Convert.ToDateTime(tblOrderTran?.CreatedDate).ToString("dd/MM/yyyy") : null;

                            TblOrderQc tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran?.OrderTransId ?? 0);
                            if (tblOrderQc != null && tblOrderQc.ProposedQcdate != null)
                            {
                                pendingForQCVMExcel.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                pendingForQCVMExcel.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                            }
                            pendingForQCVMExcel.ProductCondition = tblOrderQc?.QualityAfterQc;
                            pendingForQCVMList.Add(pendingForQCVMExcel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "GetAbbPendingOrdersList", ex);
            }
            return pendingForQCVMList;
        }
        #endregion

        #region Get List Pending for PriceAcceptance ABB Orders list
        public List<PendingForPriceAcceptVMExcel> GetAbbPendingPriceAcceptList(int? buid, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null, int? statusId2 = null)
        {
            #region Variable Declaration
            PendingForPriceAcceptVMExcel? pendingForPriceAcceptVMExcel = null;
            List<PendingForPriceAcceptVMExcel>? pendingForPriceAcceptVMList = new List<PendingForPriceAcceptVMExcel>();
            List<TblOrderTran>? obj1 = null;
            bool? isFilterByModifiedDate = false;
            #endregion

            try
            {
                if (buid > 0)
                {
                    obj1 = _orderTransRepository.GetABBOrdersPendingList(buid, isFilterByModifiedDate, DateTimeByElapsedHours, statusId1
                        , statusId2);
                    if (obj1 != null && obj1.Count > 0)
                    {
                        foreach (TblOrderTran tblOrderTran in obj1)
                        {
                            pendingForPriceAcceptVMExcel = new PendingForPriceAcceptVMExcel();
                            pendingForPriceAcceptVMExcel.CompanyName = tblOrderTran?.Abbredemption?.Abbregistration?.BusinessUnit?.Name;
                            pendingForPriceAcceptVMExcel.RegdNo = tblOrderTran?.RegdNo;
                            pendingForPriceAcceptVMExcel.CustomerCity = tblOrderTran?.Abbredemption?.CustomerDetails?.City;
                            pendingForPriceAcceptVMExcel.ProductCategory = tblOrderTran?.Abbredemption?.Abbregistration?.NewProductCategory?.DescriptionForAbb;
                            pendingForPriceAcceptVMExcel.StatusCode = tblOrderTran?.Status?.StatusCode;
                            pendingForPriceAcceptVMExcel.OrderDueDateTime = tblOrderTran?.ModifiedDate != null ? Convert.ToDateTime(tblOrderTran?.ModifiedDate).ToString("dd/MM/yyyy hh:mm tt") : null;

                            TblOrderQc tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran?.OrderTransId ?? 0);
                            //if (tblOrderQc != null && tblOrderQc.ProposedQcdate != null)
                            //{
                            //    pendingForPriceAcceptVMExcel.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                            //    pendingForPriceAcceptVMExcel.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                            //}
                            pendingForPriceAcceptVMExcel.ProductCondition = tblOrderQc?.QualityAfterQc;
                            pendingForPriceAcceptVMList.Add(pendingForPriceAcceptVMExcel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "GetAbbPendingOrdersList", ex);
            }
            return pendingForPriceAcceptVMList;
        }
        #endregion

        #region Get List Pending for Pickup ABB Orders list
        public List<PendingForPickupVMExcel> GetAbbPendingPickupList(int? buid, int? spid, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null)
        {
            #region Variable Declaration
            PendingForPickupVMExcel? pendingForPickupVMExcel = null;
            List<PendingForPickupVMExcel>? pendingForPickupVMList = new List<PendingForPickupVMExcel>();
            List<TblLogistic>? obj1 = null;
            #endregion

            try
            {
                if (buid > 0)
                {
                    obj1 = _orderTransRepository.GetABBOrdersPickupPendingList(buid, spid, DateTimeByElapsedHours, statusId1);
                    if (obj1 != null && obj1.Count > 0)
                    {
                        foreach (TblLogistic tblLogistic in obj1)
                        {
                            pendingForPickupVMExcel = new PendingForPickupVMExcel();
                            pendingForPickupVMExcel.CompanyName = tblLogistic?.OrderTrans?.Abbredemption?.Abbregistration?.BusinessUnit?.Name;
                            pendingForPickupVMExcel.RegdNo = tblLogistic?.RegdNo;
                            pendingForPickupVMExcel.ServicePartnerName = tblLogistic?.ServicePartner?.ServicePartnerBusinessName;
                            pendingForPickupVMExcel.CustomerCity = tblLogistic?.OrderTrans?.Abbredemption?.CustomerDetails?.City;
                            pendingForPickupVMExcel.ProductCategory = tblLogistic?.OrderTrans?.Abbredemption?.Abbregistration?.NewProductCategory?.DescriptionForAbb;
                            pendingForPickupVMExcel.StatusCode = tblLogistic?.Status?.StatusCode;
                            pendingForPickupVMExcel.TicketNumber = tblLogistic?.TicketNumber;
                            pendingForPickupVMExcel.PickupScheduleDate = tblLogistic?.PickupScheduleDate != null ? tblLogistic?.PickupScheduleDate.ToString() : null;
                            TblOrderQc tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblLogistic?.OrderTransId ?? 0);
                            pendingForPickupVMExcel.ProductCondition = tblOrderQc?.QualityAfterQc;
                            pendingForPickupVMExcel.OrderDueDateTime = tblLogistic?.CreatedDate != null ? Convert.ToDateTime(tblLogistic?.CreatedDate).ToString("dd/MM/yyyy hh:mm tt") : null;

                            pendingForPickupVMList.Add(pendingForPickupVMExcel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessUnitManager", "GetAbbPendingOrdersList", ex);
            }
            return pendingForPickupVMList;
        }

        public List<ProductCategoryViewModel> GetDataForCategoryAndType(int? Buid)
        {
            List<ProductCategoryViewModel> ProductCategoryAssociatedWithType = new List<ProductCategoryViewModel>();
            var result = _bUProductCategoryMapping.GetBUProdCatList(Buid);
            var iteration = result.Where(x=>x.ProductCatId != null).Select(x => x.ProductCatId).Distinct().ToList();

            foreach (var eachRecord in iteration)
            {
                ProductCategoryViewModel prodCatVM = new ProductCategoryViewModel();
                prodCatVM.Id = eachRecord.Value;
                var des = _productCategoryRepository.GetSingle(x => x.Id == prodCatVM.Id);
                prodCatVM.Description = des.Description;
                prodCatVM.Selected = true;
                prodCatVM.SelectedProduct = result.Where(x => x.ProductCatId == eachRecord).Select(x=>x.ProductTypeId).ToList();
                ProductCategoryAssociatedWithType.Add(prodCatVM);
            }   
            return ProductCategoryAssociatedWithType;
        }
        #endregion

        #endregion End Reporting Tab
    }
    //Extension method to convert Bitmap to Byte Array
    public static class BitmapExtension
    {
        public static byte[] BitmapToByteArray(this Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
