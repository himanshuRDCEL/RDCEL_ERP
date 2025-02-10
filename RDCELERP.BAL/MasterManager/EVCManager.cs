using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using GoogleMaps.LocationServices;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model;
using RDCELERP.Model.Base;
using RDCELERP.Model.CommonModel;
using RDCELERP.Model.EVC;
using RDCELERP.Model.EVC_Allocated;
using RDCELERP.Model.EVC_Portal;
using RDCELERP.Model.EVCdispute;
using RDCELERP.Model.EVCPriceMaster;
using RDCELERP.Model.ExchangeBulkLiquidation;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.LGC;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.LGC;
using RDCELERP.Model.Paymant;
using RDCELERP.Model.Users;
using static RDCELERP.Model.ABBRedemption.LoVViewModel;
using static RDCELERP.Model.Whatsapp.AssignEVCViewModel;
using static RDCELERP.Model.Whatsapp.WhatsappEvcAutoAllocation;
using static RDCELERP.Model.Whatsapp.WhatsappLgcPickupViewModel;

namespace RDCELERP.BAL.MasterManager
{
    public class EVCManager : IEVCManager
    {
        #region  Variable Declaration
        IUserRepository _userRepository;
        IUserRoleRepository _userRoleRepository;
        IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        private DAL.Entities.Digi2l_DevContext _context;
        IEVCRepository _evcRepository;
        IStateRepository _stateRepository;
        IEntityRepository _entityRepository;
        IWalletTransactionRepository _walletTransactionRepository;
        ICityRepository _cityRepository;
        IExchangeOrderRepository _exchangeOrderRepository;
        IEVCWalletAdditionRepository _EVCWalletAdditionRepository;
        ICustomerDetailsRepository _customerDetailsRepository;
        IEVCDisputeRepository _eVCDisputeRepository;
        IProductTypeRepository _productTypeRepository;
        IProductCategoryRepository _productCategoryRepository;
        IOrderTransRepository _OrderTransRepository;
        ILovRepository _lovRepository;
        IOrderImageUploadRepository _orderImageUploadRepository;
        IImageHelper _imageHelper;
        IExchangeABBStatusHistoryRepository _exchangeABBStatusHistoryRepository;
        IEVCWalletHistoryRepository _eVCWalletHistoryRepository;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        private readonly IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _WhatsAppMessageRepository;
        private readonly INotificationManager _notificationManager;
        public readonly IOptions<ApplicationSettings> _baseConfig;
        ICommonManager _commonManager;
        IOrderLGCRepository _orderLGCRepository;
        IPaymentLeaser _paymentLeaser;
        IABBRedemptionRepository _aBBRedemptionRepository;
        IAbbRegistrationRepository _abbRegistrationRepository;
        IAreaLocalityRepository _areaLocalityRepository;
        IVehicleJourneyTrackingDetailsRepository _vehicleJourneyTrackingDetails;
        IPushNotificationManager _pushNotificationManager;
        IEVCPartnerRepository _eVCPartnerRepository;
        IEvcPartnerPreferenceRepository _eVCPartnerPreferenceRepository;
        IOrderTransactionManager _orderTransactionManager;
        IOrderQCRepository _orderQCRepository;

        #endregion

        #region Constructor

        public EVCManager(IProductCategoryRepository productCategoryRepository, IOrderTransRepository orderTransRepository, IEVCRepository evcRepository, DAL.Entities.Digi2l_DevContext context,
            IWalletTransactionRepository walletTransactionRepository,
            IUserRoleRepository userRoleRepository, IRoleRepository roleRepository,
            IMapper mapper, ILogging logging, IStateRepository stateRepository,
            IEntityRepository entityRepository, ICityRepository cityRepository,
            IEVCWalletAdditionRepository eVCWalletAdditionRepository,
            IExchangeOrderRepository exchangeOrderRepository,
            ICustomerDetailsRepository customerDetailsRepository,
            IEVCDisputeRepository eVCDisputeRepository,
            IProductTypeRepository productTypeRepository,
            ILovRepository lovRepository,
            IOrderImageUploadRepository orderImageUploadRepository,
            IImageHelper imageHelper,
            IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository,
            IEVCWalletHistoryRepository eVCWalletHistoryRepository,
            INotificationManager notificationManager,
            IWhatsappNotificationManager whatsappNotificationManager,
            IWhatsAppMessageRepository whatsAppMessageRepository,
            IOptions<ApplicationSettings> baseConfig,
            ICommonManager commonManager,
            IOrderLGCRepository orderLGCRepository,
            IPaymentLeaser paymentLeaser,
            IABBRedemptionRepository aBBRedemptionRepository,
            IAbbRegistrationRepository abbRegistrationRepository,
            IAreaLocalityRepository areaLocalityRepository,
            IVehicleJourneyTrackingDetailsRepository vehicleJourneyTrackingDetails,
            IPushNotificationManager pushNotificationManager,
            IEVCPartnerRepository eVCPartnerRepository,
            IEvcPartnerPreferenceRepository eVCPartnerPreferenceRepository,
            IOrderTransactionManager orderTransactionManager, IOrderQCRepository orderQCRepository)
        {
            _productCategoryRepository = productCategoryRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logging = logging;
            _evcRepository = evcRepository;
            _stateRepository = stateRepository;
            _OrderTransRepository = orderTransRepository;
            _entityRepository = entityRepository;
            _walletTransactionRepository = walletTransactionRepository;
            _cityRepository = cityRepository;
            _context = context;
            _EVCWalletAdditionRepository = eVCWalletAdditionRepository;
            _exchangeOrderRepository = exchangeOrderRepository;
            _customerDetailsRepository = customerDetailsRepository;
            _eVCDisputeRepository = eVCDisputeRepository;
            _productTypeRepository = productTypeRepository;
            _lovRepository = lovRepository;
            _orderImageUploadRepository = orderImageUploadRepository;
            _imageHelper = imageHelper;
            _exchangeABBStatusHistoryRepository = exchangeABBStatusHistoryRepository;
            _eVCWalletHistoryRepository = eVCWalletHistoryRepository;
            _notificationManager = notificationManager;
            _whatsappNotificationManager = whatsappNotificationManager;
            _WhatsAppMessageRepository = whatsAppMessageRepository;
            _baseConfig = baseConfig;
            _commonManager = commonManager;
            _orderLGCRepository = orderLGCRepository;
            _paymentLeaser = paymentLeaser;
            _aBBRedemptionRepository = aBBRedemptionRepository;
            _abbRegistrationRepository = abbRegistrationRepository;
            _areaLocalityRepository = areaLocalityRepository;
            _vehicleJourneyTrackingDetails = vehicleJourneyTrackingDetails;
            _pushNotificationManager = pushNotificationManager;
            _eVCPartnerRepository = eVCPartnerRepository;
            _eVCPartnerPreferenceRepository = eVCPartnerPreferenceRepository;
            _orderTransactionManager = orderTransactionManager;
            _orderQCRepository = orderQCRepository;
        }
        #endregion

        #region Get All User list 
        /// <summary>
        ///Get All User list 
        /// </summary>
        /// <returns>TblUser UserVMList</returns>
        public IList<UserViewModel> GetallEmployeeId()
        {
            IList<UserViewModel> UserVMList = null;
            IList<TblUser> TblUserList = null;
            IList<TblUserRole> TblUserRoleList = null;

            try
            {
                if (TblUserRoleList != null && TblUserRoleList.Count > 0)
                {
                    TblUserList = _userRepository.GetList(x => x.IsActive == true).ToList();

                    TblUserList = (from al in TblUserRoleList
                                   join ral in TblUserList on al.UserId equals ral.UserId
                                   select new TblUser
                                   {
                                       UserId = ral.UserId,
                                       FirstName = ral.FirstName,
                                       LastName = ral.LastName,

                                   }).ToList();
                }
                if (TblUserList != null && TblUserList.Count > 0)
                {
                    UserVMList = _mapper.Map<IList<TblUser>, IList<UserViewModel>>(TblUserList);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "GetSalesSUserList", ex);
            }
            return UserVMList;
        }
        #endregion

        #region ADDED BY Priyanshi _Save/Edit EvcRegistation From
        /// <summary>
        /// ADDED BY Priyanshi_ Save/Edit EvcRegistation From
        /// </summary>
        /// <param name="evc_RegistrationModel"></param>
        /// <param name="userId"></param>
        /// <returns>TblEvcregistration.EvcregistrationId</returns>
        public int SaveEVCDetails(EVC_RegistrationModel evc_RegistrationModel, int userId)
        {
            TblEvcregistration TblEvcregistration = new TblEvcregistration();
            TblEvcPartner tblEvcPartner = new TblEvcPartner();
            EVC_StoreRegistrastionViewModel eVC_StoreRegistrastionViewModels = new EVC_StoreRegistrastionViewModel();
            int response = 0;

            bool cencelCheck = false;
            try
            {

                if (evc_RegistrationModel.CopyofCancelledChequeLinkURLBase64string != null)
                {
                    string fileName = Guid.NewGuid().ToString("N") + "CancelledCheck" + ".jpg";
                    string filePath = @"\DBFiles\EVC\CancelledCheque";
                    cencelCheck = _imageHelper.SaveFileFromBase64(evc_RegistrationModel.CopyofCancelledChequeLinkURLBase64string, filePath, fileName);
                    if (cencelCheck)
                    {
                        evc_RegistrationModel.CopyofCancelledCheque = fileName;
                        cencelCheck = false;
                    }

                }

                if (evc_RegistrationModel.UploadGSTRegistrationLinkURLBase64string != null)
                {
                    string fileName = Guid.NewGuid().ToString("N") + "GSTRegistration" + ".jpg";
                    string filePath = @"\DBFiles\EVC\GSTRegistration";
                    cencelCheck = _imageHelper.SaveFileFromBase64(evc_RegistrationModel.UploadGSTRegistrationLinkURLBase64string, filePath, fileName);
                    if (cencelCheck)
                    {
                        evc_RegistrationModel.UploadGSTRegistration = fileName;
                        cencelCheck = false;
                    }

                }
                if (evc_RegistrationModel.EWasteCertificateLinkURLBase64string != null)
                {
                    string fileName = Guid.NewGuid().ToString("N") + "EWasteCertificate" + ".jpg";
                    string filePath = @"\DBFiles\EVC\EWasteCertificate";
                    cencelCheck = _imageHelper.SaveFileFromBase64(evc_RegistrationModel.EWasteCertificateLinkURLBase64string, filePath, fileName);
                    if (cencelCheck)
                    {
                        evc_RegistrationModel.EWasteCertificate = fileName;
                        cencelCheck = false;
                    }

                }
                if (evc_RegistrationModel.AadharfrontImageLinkURLBase64string != null)
                {
                    string fileName = Guid.NewGuid().ToString("N") + "Aadharfront" + ".jpg";
                    string filePath = @"\DBFiles\EVC\AadharfrontImage";
                    cencelCheck = _imageHelper.SaveFileFromBase64(evc_RegistrationModel.AadharfrontImageLinkURLBase64string, filePath, fileName);
                    if (cencelCheck)
                    {
                        evc_RegistrationModel.AadharfrontImage = fileName;
                        cencelCheck = false;
                    }

                }
                if (evc_RegistrationModel.AadharBackImageLinkURLBase64string != null)
                {
                    string fileName = Guid.NewGuid().ToString("N") + "CancelledCheck" + ".jpg";
                    string filePath = @"\DBFiles\EVC\AadharBackImage";
                    cencelCheck = _imageHelper.SaveFileFromBase64(evc_RegistrationModel.AadharBackImageLinkURLBase64string, filePath, fileName);
                    if (cencelCheck)
                    {
                        evc_RegistrationModel.AadharBackImage = fileName;
                        cencelCheck = false;
                    }

                }
                if (evc_RegistrationModel.ProfilePicLinkURLBase64string != null)
                {
                    string fileName = Guid.NewGuid().ToString("N") + "CancelledCheck" + ".jpg";
                    string filePath = @"\DBFiles\EVC\EVCProfilePic";
                    cencelCheck = _imageHelper.SaveFileFromBase64(evc_RegistrationModel.ProfilePicLinkURLBase64string, filePath, fileName);
                    if (cencelCheck)
                    {
                        evc_RegistrationModel.ProfilePic = fileName;
                        cencelCheck = false;
                    }

                }

                if (evc_RegistrationModel != null)
                {

                    TblEvcregistration = _mapper.Map<EVC_RegistrationModel, TblEvcregistration>(evc_RegistrationModel);
                    if (TblEvcregistration.EvcregistrationId > 0)
                    {
                        //Code to update the object
                        TblEvcregistration.IconfirmTermsCondition = true;
                        TblEvcregistration.Date = _currentDatetime;
                        TblEvcregistration.ModifiedBy = userId;
                        TblEvcregistration.ModifiedDate = _currentDatetime;
                        _evcRepository.Update(TblEvcregistration);
                        _evcRepository.SaveChanges();
                        response = 2;

                    }
                    else
                    {
                        var Check = _evcRepository.GetSingle(x => x.EmailId == evc_RegistrationModel.EmailId);
                        if (Check == null)
                        {
                            //Code to Insert the object 
                            TblEvcregistration.EvcwalletAmount = 0;
                            TblEvcregistration.EvcapprovalStatusId = userId;
                            TblEvcregistration.Isevcapprovrd = false;
                            TblEvcregistration.IsInHouse = false;
                            TblEvcregistration.IsActive = true;
                            TblEvcregistration.CreatedDate = _currentDatetime;
                            TblEvcregistration.CreatedBy = userId;
                            TblEvcregistration.Date = _currentDatetime;
                            TblEvcregistration.IconfirmTermsCondition = true;
                            if (TblEvcregistration.EmployeeId == null)
                            {
                                TblEvcregistration.EmployeeId = userId;
                            }
                            var getcitycode = _cityRepository.GetSingle(x => x.CityId == evc_RegistrationModel.cityId);
                            TblEvcregistration.EvcregdNo = getcitycode.CityCode + UniqueString.RandomNumber();
                            TblEvcregistration.EvczohoBookName = TblEvcregistration.EvcregdNo + "-" + evc_RegistrationModel.BussinessName;
                            _evcRepository.Create(TblEvcregistration);
                            var xyz = _evcRepository.SaveChanges();

                            #region save TblEvcPartner details
                            eVC_StoreRegistrastionViewModels.EvcregistrationId = TblEvcregistration.EvcregistrationId;
                            eVC_StoreRegistrastionViewModels.StateId = (int)TblEvcregistration.StateId;
                            eVC_StoreRegistrastionViewModels.CityId = (int)TblEvcregistration.CityId;
                            eVC_StoreRegistrastionViewModels.PinCode = TblEvcregistration.PinCode;
                            eVC_StoreRegistrastionViewModels.EmailId = TblEvcregistration.EmailId;
                            eVC_StoreRegistrastionViewModels.ContactNumber = TblEvcregistration.EvcmobileNumber;
                            eVC_StoreRegistrastionViewModels.Address1 = TblEvcregistration.RegdAddressLine1;
                            eVC_StoreRegistrastionViewModels.Address2 = TblEvcregistration.RegdAddressLine2;


                            var result = SaveEVCPartnerDetails(eVC_StoreRegistrastionViewModels, userId);
                            #endregion

                            response = 1;
                        }
                        else
                        {
                            response = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "SaveEVCDetails", ex);
            }
            return response;
        }
        #endregion

        #region Vendor Registration

        public bool SaveVendorDetails(VendorRegistrationModel vendorRegistrationModel, int userId)
        {
            TblEvcregistration TblEvcregistration = new TblEvcregistration();
            TblEvcPartner tblEvcPartner = new TblEvcPartner();
            EVC_StoreRegistrastionViewModel EvcPartner = new EVC_StoreRegistrastionViewModel();
            int response = 0;
            bool IsSuccess = false;

            try
            {
                if (vendorRegistrationModel != null)
                {
                    TblEvcregistration.BussinessName = vendorRegistrationModel.VendorName;
                    TblEvcregistration.NatureOfBusiness = vendorRegistrationModel.NatureOfBusiness;
                    TblEvcregistration.ContactPerson = vendorRegistrationModel.contactPerson;
                    TblEvcregistration.RegdAddressLine1 = vendorRegistrationModel.Address;
                    TblEvcregistration.RegdAddressLine2 = vendorRegistrationModel.Address;
                    TblEvcregistration.Country = vendorRegistrationModel.Country;
                    TblEvcregistration.StateId = vendorRegistrationModel.StateId;
                    TblEvcregistration.CityId = vendorRegistrationModel.cityId;
                    TblEvcregistration.PinCode = vendorRegistrationModel.PostalCode;
                    TblEvcregistration.AlternateMobileNumber = vendorRegistrationModel.Telephone;
                    TblEvcregistration.EvcmobileNumber = vendorRegistrationModel.Mobile;
                    TblEvcregistration.Gstno = vendorRegistrationModel.gstin;
                    TblEvcregistration.PanNo = vendorRegistrationModel.panNo;
                    TblEvcregistration.EmailId = vendorRegistrationModel.email;
                    TblEvcregistration.CompanyRegNo = vendorRegistrationModel.companyRegNo;

                    //Bank details mapping...
                    TblEvcregistration.AccountHolder = vendorRegistrationModel.accountHolder;
                    TblEvcregistration.BankName = vendorRegistrationModel.bankName;
                    TblEvcregistration.Branch = vendorRegistrationModel.branch;
                    TblEvcregistration.BankAccountNo = vendorRegistrationModel.accountNo;
                    TblEvcregistration.Ifsccode = vendorRegistrationModel.ifscCode;

                    //UTC Employee details mapping...
                    TblEvcregistration.UtcEmployeeName = vendorRegistrationModel.utcEmployeeName;
                    TblEvcregistration.UtcEmployeeEmail = vendorRegistrationModel.utcEmployeeEmail;
                    TblEvcregistration.UtcEmployeeContact = vendorRegistrationModel.utcEmployeeContact;
                    TblEvcregistration.ApproverName = vendorRegistrationModel.approverName;
                    TblEvcregistration.UnitDepartment = vendorRegistrationModel.unitDepartment;
                    TblEvcregistration.ManagerEmail = vendorRegistrationModel.managerEmail;
                    TblEvcregistration.ManagerContact = vendorRegistrationModel.managerContact;

                    TblEvcregistration.ContactPersonAddress = vendorRegistrationModel.Address;
                    TblEvcregistration.Date = _currentDatetime;

                    if (TblEvcregistration != null)
                    {
                        var Check = _evcRepository.GetSingle(x => x.EmailId == vendorRegistrationModel.email);
                        if (Check == null)
                        {
                            //Code to Insert the object 
                            TblEvcregistration.EvcwalletAmount = 0;
                            //TblEvcregistration.EvcapprovalStatusId = userId;
                            TblEvcregistration.Isevcapprovrd = false;
                            TblEvcregistration.IsInHouse = true;
                            TblEvcregistration.IsActive = true;
                            TblEvcregistration.CreatedDate = _currentDatetime;
                            TblEvcregistration.CreatedBy = userId;
                            TblEvcregistration.Date = _currentDatetime;
                            TblEvcregistration.IconfirmTermsCondition = true;
                            if (TblEvcregistration.EmployeeId == null)
                            {
                                TblEvcregistration.EmployeeId = userId;
                            }
                            var getcitycode = _cityRepository.GetSingle(x => x.CityId == vendorRegistrationModel.cityId);
                            TblEvcregistration.EvcregdNo = getcitycode.CityCode + UniqueString.RandomNumber();
                            TblEvcregistration.EvczohoBookName = TblEvcregistration.EvcregdNo + "-" + vendorRegistrationModel.NatureOfBusiness;
                            _evcRepository.Create(TblEvcregistration);
                            var res = _evcRepository.SaveChanges();

                            // save TblEvcPartner details

                            EvcPartner.EvcregistrationId = TblEvcregistration.EvcregistrationId;
                            EvcPartner.StateId = Convert.ToInt32(TblEvcregistration.StateId);
                            EvcPartner.CityId = Convert.ToInt32(TblEvcregistration.CityId);
                            EvcPartner.PinCode = TblEvcregistration.PinCode;
                            EvcPartner.EmailId = TblEvcregistration.EmailId;
                            EvcPartner.ContactNumber = TblEvcregistration.EvcmobileNumber;
                            EvcPartner.Address1 = TblEvcregistration.RegdAddressLine1;
                            EvcPartner.Address2 = TblEvcregistration.RegdAddressLine2;

                            var result = SaveEVCPartnerDetails(EvcPartner, userId);

                            if (result == 1 && res == 1)
                            {
                                IsSuccess = true;
                            }

                            response = 1;
                        }
                        else
                        {
                            response = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "SaveVendorDetails", ex);
            }
            return IsSuccess;
        }

        #endregion


        #region Added by Priyanshi _ Approved Evc(update 2 colom in tblEvcRegistraion) 
        /// <summary>
        /// Added by Priyanshi _ Approved Evc 
        /// </summary>
        /// <param name="EvcregistrationId"></param>
        /// <returns></returns>
        public int ApprovedEVC(int EvcregistrationId)
        {
            TblEvcregistration TblEvcRegistration = new TblEvcregistration();
            try
            {
                var Check = _evcRepository.GetSingle(x => x.EvcregistrationId == EvcregistrationId);
                if (Check != null)
                {
                    //TblEvcRegistration.EvcapprovalStatusId = 1;
                    TblEvcRegistration.Isevcapprovrd = true;
                    _evcRepository.Update(items: TblEvcRegistration);
                    _evcRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "ApprovedEVC", ex);
            }
            return TblEvcRegistration.EvcregistrationId;
        }
        #endregion

        #region Added by Priyanshi_ Get EVC Details by EvcregistrationId use of EVC_Registration Razor Page 
        /// <summary>
        ///  Added by Priyanshi_ Get EVC Details by EvcregistrationId use of EVC_Registration Razor Page 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TblEvcregistration eVC_RegistrationModels</returns>
        public EVC_RegistrationModel GetEvcByEvcregistrationId(int id)
        {
            EVC_RegistrationModel eVC_RegistrationModels = null;
            TblEvcregistration TblEvcregistration = null;
            try
            {
                TblEvcregistration = _evcRepository.GetSingle(where: x => x.IsActive == true && x.EvcregistrationId == id);
                if (TblEvcregistration != null)
                {
                    eVC_RegistrationModels = _mapper.Map<TblEvcregistration, EVC_RegistrationModel>(TblEvcregistration);
                    if (eVC_RegistrationModels != null)
                    {
                        TblUser tblUser = _context.TblUsers.Where(x => x.IsActive == true && x.UserId == eVC_RegistrationModels.EmployeeId).FirstOrDefault();
                        eVC_RegistrationModels.EmployeeIdName = tblUser != null ? tblUser.UserId + " " + tblUser.FirstName : string.Empty;

                        TblState tblState = _context.TblStates.Where(x => x.IsActive == true && x.StateId == eVC_RegistrationModels.StateId).FirstOrDefault();
                        eVC_RegistrationModels.StateIdName = tblState != null ? tblState.Name : string.Empty;
                        TblCity tblCity = _context.TblCities.Where(x => x.IsActive == true && x.CityId == eVC_RegistrationModels.cityId).FirstOrDefault();
                        eVC_RegistrationModels.CityIdName = tblCity != null ? tblCity.Name : string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "GetEvcByEvcregistrationId", ex);
            }
            return eVC_RegistrationModels;
        }
        #endregion

        #region Added by Priyanshi - Save EVC Wallet Details by EVCAdmin 
        /// <summary>
        /// Added by Priyanshi - Save EVC Wallet Details by EVCAdmin 
        /// </summary>
        /// <param name="eVCWalletAdditionViewModels"></param>
        /// <returns></returns>
        public int SaveEVCWalletDetails(EVCWalletAdditionViewModel eVCWalletAdditionViewModels, int userId)
        {
            TblEvcwalletAddition evcwalletAddition = new TblEvcwalletAddition();
            TblEvcwalletHistory evcwalletHistory = new TblEvcwalletHistory();
            List<TblEvcwalletAddition> tblEvcwalletAddition = new List<TblEvcwalletAddition>();
            decimal Amount = 0;
            try
            {
                if (eVCWalletAdditionViewModels != null)
                {
                    // evcwalletAddition = _mapper.Map<EVCWalletAdditionViewModel, TblEvcwalletAddition>(eVCWalletAdditionViewModels);
                    evcwalletAddition.EvcregistrationId = eVCWalletAdditionViewModels.EvcregistrationId;
                    evcwalletAddition.CreatedBy = userId;
                    evcwalletAddition.Rechargeby = Convert.ToInt32(LoVEnum.EVCAdmin);
                    evcwalletAddition.CreatedDate = _currentDatetime;
                    evcwalletAddition.IsActive = true;
                    evcwalletAddition.IsCreaditNote = eVCWalletAdditionViewModels.IsCreaditNote;
                    if (eVCWalletAdditionViewModels.IsCreaditNote == true)
                    {
                        evcwalletAddition.Amount = (decimal)eVCWalletAdditionViewModels.CreditAmount;
                        evcwalletAddition.TransactionId = eVCWalletAdditionViewModels.CreditTransactionId;
                        evcwalletAddition.TransactionDate = eVCWalletAdditionViewModels.CreditTransactionDate;
                        evcwalletAddition.Comments = eVCWalletAdditionViewModels.CreditComments;
                    }
                    else
                    {
                        evcwalletAddition.Amount = (decimal)eVCWalletAdditionViewModels.Amount;
                        evcwalletAddition.TransactionId = eVCWalletAdditionViewModels.TransactionId;
                        evcwalletAddition.TransactionDate = eVCWalletAdditionViewModels.TransactionDate;
                        evcwalletAddition.InvoiceImage = eVCWalletAdditionViewModels.InvoiceImage;
                    }

                    _EVCWalletAdditionRepository.Create(evcwalletAddition);
                    _EVCWalletAdditionRepository.SaveChanges();
                    Amount = evcwalletAddition.Amount;

                }
                if (eVCWalletAdditionViewModels != null)
                {
                    evcwalletHistory.EvcregistrationId = eVCWalletAdditionViewModels.EvcregistrationId;
                    var cAmount = _context.TblEvcregistrations.Where(X => X.EvcregistrationId == evcwalletHistory.EvcregistrationId).FirstOrDefault();
                    evcwalletHistory.CurrentWalletAmount = cAmount.EvcwalletAmount > 0 || cAmount.EvcwalletAmount != null ? cAmount.EvcwalletAmount : 0;
                    evcwalletHistory.AddAmount = Amount;
                    evcwalletHistory.BalanceWalletAmount = evcwalletHistory.CurrentWalletAmount + evcwalletHistory.AddAmount;
                    evcwalletHistory.AmountAdditionFlag = true;
                    evcwalletHistory.IsActive = true;
                    evcwalletHistory.CreatedDate = _currentDatetime;
                    evcwalletHistory.CreatedBy = userId;
                    _eVCWalletHistoryRepository.Create(evcwalletHistory);
                    _eVCWalletHistoryRepository.SaveChanges();


                    TblEvcregistration tblEvcregistrations = _context.TblEvcregistrations.Where(x => x.EvcregistrationId == evcwalletHistory.EvcregistrationId).FirstOrDefault();
                    if (tblEvcregistrations != null)
                    {

                        tblEvcregistrations.EvcwalletAmount = (tblEvcregistrations.EvcwalletAmount == null ? tblEvcregistrations.EvcwalletAmount = 0 : tblEvcregistrations.EvcwalletAmount) + Amount;
                        tblEvcregistrations.ModifiedBy = userId;
                        tblEvcregistrations.ModifiedDate = _currentDatetime;

                        _evcRepository.Update(tblEvcregistrations);
                        _evcRepository.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "SaveEVCWalletDetails", ex);
            }
            return (int)evcwalletAddition.Amount;
        }
        #endregion

        #region Added by priyanshi _ Get List of all Evc
        /// <summary>
        /// Added by priyanshi _ Get List of all Evc
        /// </summary>
        /// <returns>TblEvcregistration -EVC_RegistrationVMList</returns>
        public IList<EVC_RegistrationModel> GetAllEVCRegistration()
        {
            IList<EVC_RegistrationModel> EVC_RegistrationVMList = null;
            List<TblEvcregistration> TblEvcregistrationlist = new List<TblEvcregistration>();
            try
            {
                TblEvcregistrationlist = _evcRepository.GetList(x => x.IsActive == true).ToList();
                if (TblEvcregistrationlist != null && TblEvcregistrationlist.Count > 0)
                {
                    EVC_RegistrationVMList = _mapper.Map<IList<TblEvcregistration>, IList<EVC_RegistrationModel>>(TblEvcregistrationlist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "GetAllUser", ex);
            }
            return EVC_RegistrationVMList;
        }
        #endregion

        #region ADDED BY Priyansi Sahu---this Method Use For EVC AssigeMent --Get EVC BY City//state(Asking Rucha) 
        public List<Allocate_EVCFromViewModel> ListOfEVCBycity(string ids)
        {
            List<Allocate_EVCFromViewModel> Allocate_EVCFromViewModels = new List<Allocate_EVCFromViewModel>();
            List<EVC_PartnerViewModel> eVCPartnerViewModels = new List<EVC_PartnerViewModel>();
            List<TblEvcPartner> tblEvcPartnerList = new List<TblEvcPartner>();
            try
            {
                // Split the input ids into an array
                string[] OrderTranIDList = ids.Split(",");

                foreach (var itemId in OrderTranIDList)
                {
                    Allocate_EVCFromViewModel allocate = new Allocate_EVCFromViewModel();

                    // Retrieve the TblOrderTran using the itemId
                    // TblOrderTran tblOrderTran = _OrderTransRepository.GetSingleOrderWithExchangereference(Convert.ToInt32(itemId));

                    allocate = _orderTransactionManager.GetOrderDetailsByOrderTransId(Convert.ToInt32(itemId));
                    if (!string.IsNullOrEmpty(allocate.CustPin) && allocate.ProductCatId > 0)
                    {
                        List<TblConfiguration> tblConfiguration = _context.TblConfigurations.Where(x => x.IsActive == true).ToList();

                        if (tblConfiguration != null)
                        {
                            TblConfiguration? EVCAssignbyState = tblConfiguration.Where(x => x.Name == EnumHelper.DescriptionAttr(ConfigurationEnum.EVCAssignbyState) && x.Value == "1").FirstOrDefault();
                            if (EVCAssignbyState != null)
                            {

                                if (!string.IsNullOrEmpty(allocate.Custstate) || !string.IsNullOrEmpty(allocate.CustCity) || !string.IsNullOrEmpty(allocate.CustPin))
                                {
                                    List<EVcList> EVcLists1 = (List<EVcList>)GetEVCListbycityAndpin(allocate.Custstate, allocate.CustCity, allocate.CustPin);
                                    if (EVcLists1 != null && EVcLists1.Count > 0)
                                    {
                                        List<EVcList> EvcList1 = new List<EVcList>();
                                        foreach (var item in EvcList1)
                                        {
                                            // Create an EVcList object and add it to the EVCList
                                            EVcList evclist = new EVcList
                                            {
                                                EvcregistrationId = item.EvcregistrationId,
                                                EvcregdNo = item.EvcregdNo,
                                                BussinessName = item.BussinessName
                                            };
                                            if (evclist != null)
                                            {
                                                EvcList1.Add(evclist);
                                            }
                                        }
                                        allocate.EVCLists = EVcLists1;
                                    }
                                }

                            }
                            TblConfiguration? EVCAssignbypartner = tblConfiguration.Where(x => x.Name == EnumHelper.DescriptionAttr(ConfigurationEnum.EVCAssignbypartner) && x.Value == "1").FirstOrDefault();
                            if (EVCAssignbypartner != null)
                            {

                                if (!string.IsNullOrEmpty(allocate.CustPin))
                                {
                                    tblEvcPartnerList = _eVCPartnerRepository.GetAllEvcPartnerListByPincode(allocate.CustPin, allocate.ProductCatId, allocate.ActualProdQltyAtQc, allocate.Ordertype);
                                    if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
                                    {
                                        List<EVC_PartnerViewModel> eVCPartnerViewModels1 = new List<EVC_PartnerViewModel>();
                                        foreach (var item in tblEvcPartnerList)
                                        {
                                            // Create an EVcList object and add it to the EVCList
                                            EVC_PartnerViewModel evclist = new EVC_PartnerViewModel
                                            {
                                                EvcPartnerId = item.EvcPartnerId,
                                                EvcregistrationId = item.EvcregistrationId,
                                                EvcregdNo = item.Evcregistration.EvcregdNo,
                                                BussinessName = item.Evcregistration.BussinessName,
                                                IsSweetenerAmtInclude = item?.Evcregistration?.IsSweetenerAmtInclude,
                                                GSTTypeId = item?.Evcregistration?.GsttypeId,
                                            };
                                            if (evclist != null)
                                            {
                                                eVCPartnerViewModels1.Add(evclist);
                                            }
                                        }
                                        allocate.eVCPartnerViewModels = eVCPartnerViewModels1;
                                    }
                                }

                                TblConfiguration? EVCAssignbypartnerandWallet = tblConfiguration.Where(x => x.Name == EnumHelper.DescriptionAttr(ConfigurationEnum.EVCAssignbypartnerandWallet) && x.Value == "1").FirstOrDefault();
                                if (EVCAssignbypartnerandWallet != null)
                                {
                                    if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
                                    {
                                        // tblEvcPartnerList = _eVCPartnerRepository.GetEvcPartnerListHavingClearBalance(tblEvcPartnerList, allocate.ExpectedPrice);
                                        tblEvcPartnerList = _commonManager.GetEVCPartnerListHavingClearBalance(allocate.orderTransId, tblEvcPartnerList);
                                        if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
                                        {
                                            List<EVC_PartnerViewModel> eVCPartnerViewModels1 = new List<EVC_PartnerViewModel>();
                                            foreach (var item in tblEvcPartnerList)
                                            {
                                                // Create an EVcList object and add it to the EVCList
                                                EVC_PartnerViewModel evclist = new EVC_PartnerViewModel
                                                {
                                                    EvcPartnerId = item.EvcPartnerId,
                                                    EvcregistrationId = item.EvcregistrationId,
                                                    EvcregdNo = item.Evcregistration.EvcregdNo,
                                                    BussinessName = item.Evcregistration.BussinessName,
                                                    IsSweetenerAmtInclude = item?.Evcregistration?.IsSweetenerAmtInclude,
                                                    GSTTypeId = item?.Evcregistration?.GsttypeId,
                                                };
                                                if (evclist != null)
                                                {
                                                    eVCPartnerViewModels1.Add(evclist);
                                                }
                                            }
                                            allocate.eVCPartnerViewModels = eVCPartnerViewModels1;

                                        }

                                    }

                                }

                                TblConfiguration? EVCAssignbyPartnerandWalletandlastTran = tblConfiguration.Where(x => x.Name == EnumHelper.DescriptionAttr(ConfigurationEnum.EVCAssignbyPartnerandWalletandlastTran) && x.Value == "1").FirstOrDefault();
                                if (EVCAssignbyPartnerandWalletandlastTran != null)
                                {

                                    if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
                                    {
                                        tblEvcPartnerList = _eVCPartnerRepository.GetEvcPartnerListHavingOldRecharge(tblEvcPartnerList);
                                        if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
                                        {
                                            List<EVC_PartnerViewModel> eVCPartnerViewModels1 = new List<EVC_PartnerViewModel>();
                                            foreach (var item in tblEvcPartnerList)
                                            {
                                                // Create an EVcList object and add it to the EVCList
                                                EVC_PartnerViewModel evclist = new EVC_PartnerViewModel
                                                {
                                                    EvcPartnerId = item.EvcPartnerId,
                                                    EvcregistrationId = item.EvcregistrationId,
                                                    EvcregdNo = item.Evcregistration.EvcregdNo,
                                                    BussinessName = item.Evcregistration.BussinessName,
                                                    IsSweetenerAmtInclude = item?.Evcregistration?.IsSweetenerAmtInclude,
                                                    GSTTypeId = item?.Evcregistration?.GsttypeId,
                                                };
                                                if (evclist != null)
                                                {
                                                    eVCPartnerViewModels1.Add(evclist);
                                                }
                                            }
                                            allocate.eVCPartnerViewModels = eVCPartnerViewModels1;

                                        }
                                    }
                                }

                            }

                        }
                    }

                    Allocate_EVCFromViewModels.Add(allocate);


                }
                return Allocate_EVCFromViewModels;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "GetAllApprovedEVC", ex);
                // Handle exception or return an empty list
            }

            return Allocate_EVCFromViewModels;
        }

        #endregion

        #region ADDED BY Priyanshi Sahu ------ This Method use for EVC AssigeMent ----Save EVC Assign Data 
        public string AllocateEVCByOrder(List<Allocate_EVCFromViewModel> allocate_EVCFromViewModels, int userId)
        {
            TblExchangeOrder? TblExchangeOrders = new TblExchangeOrder();
            TblProductCategory tblProductCategory = new TblProductCategory();
            TblWalletTransaction tblWalletTransaction = new TblWalletTransaction();
            TblProductType tblProductType = new TblProductType();
            TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = null;
            //WhatasappResponse whatasappResponse = new WhatasappResponse();
            TblWhatsAppMessage tblwhatsappmessage = null;
            List<string> NotAssignRNumber = new List<string>();
            TblAbbredemption tblAbbredemption = new TblAbbredemption();
            TblAbbregistration tblAbbregistration = new TblAbbregistration();
            string commaSeparatedValues = null;
            int LGCCost = 0;
            decimal UTCCost = 0;
            decimal GstAmt = 0;
            EVCPriceViewModel? eVCPriceVM = null;
            TblEvcregistration? tblEvcReg = null;
            try
            {
                if (allocate_EVCFromViewModels != null)
                {
                    foreach (Allocate_EVCFromViewModel item in allocate_EVCFromViewModels)
                    {

                        if (item.SelectEVCId > 0 && item.ExpectedPrice > 0)
                        {
                            TblOrderTran tblOrderTran = _OrderTransRepository.GetSingleOrderWithExchangereference(item.orderTransId);
                            if (tblOrderTran != null)
                            {
                                TblEvcPartner tblEvcPartner = _eVCPartnerRepository.GetEVCPartnerDetails((int)item.SelectEVCId);
                                if (tblEvcPartner != null)
                                {
                                    tblEvcReg = tblEvcPartner?.Evcregistration;
                                    #region create tblWallet Tranction
                                    TblWalletTransaction tblWallet = _walletTransactionRepository.GetSingle(x => x.OrderTransId == item.orderTransId && x.IsActive == true);
                                    if (tblWallet == null)
                                    {
                                        tblWalletTransaction = new TblWalletTransaction();
                                        //tblWalletTransaction.SponserOrderNo = tblOrderTran.Exchange.SponsorOrderNumber;
                                        tblWalletTransaction.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted).ToString();
                                        tblWalletTransaction.OrderofAssignDate = _currentDatetime;
                                        tblWalletTransaction.RegdNo = tblOrderTran.RegdNo;
                                        //order Amount
                                        #region Old Scenerio code
                                        //if (item.ExpectedPrice != null && item.ExpectedPrice > 0)
                                        //{
                                        //    tblWalletTransaction.OrderAmount = item.ExpectedPrice;
                                        //}
                                        //else
                                        //{
                                        //    if (tblOrderTran.FinalPriceAfterQc != null)
                                        //    {
                                        //        tblWalletTransaction.OrderAmount = tblOrderTran.FinalPriceAfterQc;
                                        //    }
                                        //    tblWalletTransaction.OrderAmount = 0;
                                        //}
                                        #endregion

                                        #region Set Order Amount on the basis of EVC Seetener flag
                                        if (tblEvcReg?.IsSweetenerAmtInclude == true)
                                        {
                                            #region Get LGC Cost, UTC Cost and GST
                                            eVCPriceVM = _commonManager.CalculateEVCPriceDetailed(tblOrderTran, tblEvcReg?.IsSweetenerAmtInclude, tblEvcReg?.GsttypeId ?? 0);
                                            #endregion
                                            tblWalletTransaction.IsOrderAmtWithSweetener = true;
                                            tblWalletTransaction.SweetenerAmt = eVCPriceVM.SweetenerAmt;
                                            tblWalletTransaction.Lgccost = eVCPriceVM.LGCCost;
                                            tblWalletTransaction.OrderAmount = eVCPriceVM.EVCAmount;
                                        }
                                        else
                                        {
                                            #region Get LGC Cost, UTC Cost and GST
                                            eVCPriceVM = _commonManager.CalculateEVCPriceDetailed(tblOrderTran, tblEvcReg?.IsSweetenerAmtInclude, Convert.ToInt32(LoVEnum.GSTInclusive));
                                            #endregion
                                            tblWalletTransaction.OrderAmount = eVCPriceVM.EVCAmount;
                                            tblWalletTransaction.IsOrderAmtWithSweetener = false;
                                        }
                                        tblWalletTransaction.BaseValue = eVCPriceVM.BaseValue;
                                        tblWalletTransaction.GsttypeId = eVCPriceVM.GstTypeId;
                                        tblWalletTransaction.Cgstamt = eVCPriceVM.CGSTAmount;
                                        tblWalletTransaction.Sgstamt = eVCPriceVM.SGSTAmount;
                                        #endregion

                                        tblWalletTransaction.OrderType = tblOrderTran.OrderType.ToString();//Get from Enum 
                                        tblWalletTransaction.IsActive = true;
                                        tblWalletTransaction.CreatedBy = userId;//Loging user
                                        tblWalletTransaction.CreatedDate = _currentDatetime;
                                        tblWalletTransaction.EvcregistrationId = tblEvcPartner?.EvcregistrationId;
                                        tblWalletTransaction.OrderTransId = tblOrderTran.OrderTransId;
                                        tblWalletTransaction.IsPrimeProductId = false;
                                        tblWalletTransaction.ReassignCount = 0;
                                        tblWalletTransaction.ModifiedBy = userId;//Loging user
                                        tblWalletTransaction.ModifiedDate = _currentDatetime;
                                        tblWalletTransaction.EvcpartnerId = item.SelectEVCId;
                                        _walletTransactionRepository.Create(tblWalletTransaction);
                                        _walletTransactionRepository.SaveChanges();
                                    }
                                    else
                                    {

                                        //tblWalletTransaction.SponserOrderNo = tblOrderTran.Exchange.SponsorOrderNumber;
                                        tblWallet.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted).ToString();
                                        tblWallet.OrderofAssignDate = _currentDatetime;
                                        tblWallet.RegdNo = tblOrderTran.RegdNo;
                                        //order Amount
                                        #region old Scenerio 
                                        //if (item.ExpectedPrice != null && item.ExpectedPrice > 0)
                                        //{
                                        //    tblWallet.OrderAmount = item.ExpectedPrice;
                                        //}
                                        //else
                                        //{
                                        //    if (tblOrderTran.FinalPriceAfterQc != null)
                                        //    {
                                        //        tblWallet.OrderAmount = tblOrderTran.FinalPriceAfterQc;
                                        //    }
                                        //    tblWallet.OrderAmount = 0;
                                        //}
                                        #endregion

                                        #region Set Order Amount on the basis of EVC Seetener flag
                                        if (tblEvcReg?.IsSweetenerAmtInclude == true)
                                        {
                                            #region Get LGC Cost, UTC Cost and GST
                                            eVCPriceVM = _commonManager.CalculateEVCPriceDetailed(tblOrderTran, tblEvcReg?.IsSweetenerAmtInclude, tblEvcReg?.GsttypeId ?? 0);
                                            #endregion
                                            tblWalletTransaction.IsOrderAmtWithSweetener = true;
                                            tblWalletTransaction.SweetenerAmt = eVCPriceVM.SweetenerAmt;
                                            tblWalletTransaction.Lgccost = eVCPriceVM.LGCCost;
                                            tblWalletTransaction.OrderAmount = eVCPriceVM.EVCAmount;
                                        }
                                        else
                                        {
                                            #region Get LGC Cost, UTC Cost and GST
                                            eVCPriceVM = _commonManager.CalculateEVCPriceDetailed(tblOrderTran, tblEvcReg?.IsSweetenerAmtInclude, Convert.ToInt32(LoVEnum.GSTInclusive));
                                            #endregion
                                            tblWalletTransaction.OrderAmount = eVCPriceVM.EVCAmount;
                                            tblWalletTransaction.IsOrderAmtWithSweetener = false;
                                        }
                                        tblWalletTransaction.BaseValue = eVCPriceVM.BaseValue;
                                        tblWalletTransaction.GsttypeId = eVCPriceVM.GstTypeId;
                                        tblWalletTransaction.Cgstamt = eVCPriceVM.CGSTAmount;
                                        tblWalletTransaction.Sgstamt = eVCPriceVM.SGSTAmount;
                                        #endregion

                                        tblWallet.OrderType = tblOrderTran.OrderType.ToString();//Get from Enum 
                                        tblWallet.IsActive = true;
                                        tblWallet.ModifiedBy = userId;//Loging user
                                        tblWallet.ModifiedDate = _currentDatetime;
                                        tblWallet.EvcpartnerId = item.SelectEVCId;
                                        tblWallet.OrderTransId = tblOrderTran.OrderTransId;
                                        tblWallet.IsPrimeProductId = false;
                                        tblWallet.ReassignCount = 0;
                                        tblWallet.EvcregistrationId = tblEvcPartner?.EvcregistrationId;
                                        _walletTransactionRepository.Update(tblWallet);
                                        _walletTransactionRepository.SaveChanges();
                                    }
                                    #endregion

                                    #region Update into TblOrderTrans
                                    var Ordertype = _OrderTransRepository.UpdateTransRecordStatus(item.orderTransId, Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted), userId);
                                    #endregion

                                    if (tblOrderTran != null && tblOrderTran.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                                    {
                                        #region Update into TblExchangeOrders
                                        TblExchangeOrders = _exchangeOrderRepository.GetExchOrderByRegdNo(tblOrderTran.RegdNo);
                                        if (TblExchangeOrders != null)
                                        {
                                            TblExchangeOrders.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted);
                                            TblExchangeOrders.OrderStatus = "EVC_Assign";
                                            TblExchangeOrders.ModifiedBy = userId;
                                            TblExchangeOrders.ModifiedDate = _currentDatetime;
                                            _exchangeOrderRepository.Update(TblExchangeOrders);
                                            _exchangeOrderRepository.SaveChanges();
                                        }
                                        #endregion

                                        #region Insert into tblexchangeabbhistory
                                        tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                        tblExchangeAbbstatusHistory.OrderType = (int)tblOrderTran.OrderType;
                                        tblExchangeAbbstatusHistory.SponsorOrderNumber = tblOrderTran.Exchange.SponsorOrderNumber;
                                        tblExchangeAbbstatusHistory.RegdNo = tblOrderTran.Exchange.RegdNo;
                                        tblExchangeAbbstatusHistory.ZohoSponsorId = tblOrderTran.Exchange.ZohoSponsorOrderId != null ? TblExchangeOrders.ZohoSponsorOrderId : string.Empty;
                                        tblExchangeAbbstatusHistory.CustId = tblOrderTran.Exchange.CustomerDetailsId;
                                        tblExchangeAbbstatusHistory.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted);
                                        tblExchangeAbbstatusHistory.IsActive = true;
                                        tblExchangeAbbstatusHistory.CreatedBy = userId;
                                        tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                        tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                                        tblExchangeAbbstatusHistory.Evcid = tblWalletTransaction.EvcregistrationId;
                                        _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                                        //_exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                                        //_exchangeABBStatusHistoryRepository.SaveChanges();
                                        #endregion

                                        #region send whatsapp notification to EVC Partner
                                        TblWhatsAppMessage tblwhatsappmessage1 = null;
                                        WhatasappEvcAutoAllocationResponse whatasappResponse = new WhatasappEvcAutoAllocationResponse();
                                        WhatsappEvcAutoAllocationTemplate whatsappObj = new WhatsappEvcAutoAllocationTemplate();
                                        whatsappObj.userDetails = new UserDetailsAutoAllocation();
                                        whatsappObj.notification = new AutoAllocation();
                                        whatsappObj.notification.@params = new AutoAllocationURL();
                                        whatsappObj.userDetails.number = tblEvcPartner.ContactNumber;
                                        whatsappObj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
                                        whatsappObj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
                                        whatsappObj.notification.templateId = NotificationConstants.EvcAutoAllocation;
                                        whatsappObj.notification.@params.Customername = tblOrderTran.Exchange.CustomerDetails.FirstName;
                                        whatsappObj.notification.@params.OrderNumber = tblOrderTran.Exchange.RegdNo;
                                        whatsappObj.notification.@params.EvcPrice = item.ExpectedPrice;
                                        whatsappObj.notification.@params.ProductCategory = tblOrderTran.Exchange.ProductType.ProductCat.Description;
                                        whatsappObj.notification.@params.ProductType = tblOrderTran.Exchange.ProductType.Description;
                                        string url = _baseConfig.Value.YellowAiUrl;
                                        RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                                        int statusCode = Convert.ToInt32(response.StatusCode);
                                        if (response.Content != null && statusCode == 202)
                                        {
                                            whatasappResponse = JsonConvert.DeserializeObject<WhatasappEvcAutoAllocationResponse>(response.Content);
                                            tblwhatsappmessage1 = new TblWhatsAppMessage();
                                            tblwhatsappmessage1.TemplateName = NotificationConstants.EvcAutoAllocation;
                                            tblwhatsappmessage1.IsActive = true;
                                            tblwhatsappmessage1.PhoneNumber = tblEvcPartner.ContactNumber;
                                            tblwhatsappmessage1.SendDate = DateTime.Now;
                                            tblwhatsappmessage1.MsgId = whatasappResponse.msgId;
                                            _WhatsAppMessageRepository.Create(tblwhatsappmessage1);
                                            _WhatsAppMessageRepository.SaveChanges();
                                        }
                                        #endregion
                                    }

                                    else
                                    {
                                        #region Update into TblAbbredmption
                                        tblAbbredemption = _aBBRedemptionRepository.GetAbbOrderDetails(tblOrderTran.RegdNo);
                                        if (tblAbbredemption != null)
                                        {
                                            tblAbbredemption.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted);
                                            tblAbbredemption.ModifiedBy = userId;
                                            tblAbbredemption.ModifiedDate = _currentDatetime;
                                            _aBBRedemptionRepository.Update(tblAbbredemption);
                                            _aBBRedemptionRepository.SaveChanges();
                                        }
                                        #endregion
                                        #region Update into TblAbbregistration Commited
                                        //tblAbbregistration = _abbRegistrationRepository.GetSingle(x => x.IsActive == true && x.RegdNo == tblOrderTran.RegdNo);
                                        //if (tblAbbregistration != null)
                                        //{
                                        //    tblAbbregistration.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted);
                                        //    tblAbbregistration.ModifiedBy = userId;
                                        //    tblAbbregistration.ModifiedDate = _currentDatetime;
                                        //    _abbRegistrationRepository.Update(tblAbbregistration);
                                        //    _abbRegistrationRepository.SaveChanges();
                                        //}
                                        #endregion
                                        #region Insert into tblexchangeabbhistory
                                        tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                        tblExchangeAbbstatusHistory.OrderType = (int)tblOrderTran.OrderType;
                                        //  tblExchangeAbbstatusHistory.SponsorOrderNumber = tblOrderTran.Abbredemption.SponsorOrderNumber;
                                        tblExchangeAbbstatusHistory.RegdNo = tblOrderTran.RegdNo;
                                        // tblExchangeAbbstatusHistory.ZohoSponsorId = tblOrderTran.Exchange.ZohoSponsorOrderId != null ? TblExchangeOrders.ZohoSponsorOrderId : string.Empty;
                                        tblExchangeAbbstatusHistory.CustId = tblOrderTran.Abbredemption.CustomerDetailsId;
                                        tblExchangeAbbstatusHistory.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted);
                                        tblExchangeAbbstatusHistory.IsActive = true;
                                        tblExchangeAbbstatusHistory.CreatedBy = userId;
                                        tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                        tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                                        tblExchangeAbbstatusHistory.Evcid = tblWalletTransaction.EvcregistrationId;
                                        _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                                        //_exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                                        //_exchangeABBStatusHistoryRepository.SaveChanges();
                                        #endregion

                                        #region send whatsapp notification to EVC Partner
                                        TblWhatsAppMessage tblwhatsappmessage2 = null;
                                        WhatasappEvcAutoAllocationResponse whatasappResponse = new WhatasappEvcAutoAllocationResponse();
                                        WhatsappEvcAutoAllocationTemplate whatsappObj = new WhatsappEvcAutoAllocationTemplate();
                                        whatsappObj.userDetails = new UserDetailsAutoAllocation();
                                        whatsappObj.notification = new AutoAllocation();
                                        whatsappObj.notification.@params = new AutoAllocationURL();
                                        whatsappObj.userDetails.number = tblEvcPartner.ContactNumber;
                                        whatsappObj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
                                        whatsappObj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
                                        whatsappObj.notification.templateId = NotificationConstants.EvcAutoAllocation;
                                        whatsappObj.notification.@params.Customername = tblOrderTran.Abbredemption.Abbregistration.Customer.FirstName;
                                        whatsappObj.notification.@params.OrderNumber = tblOrderTran.Abbredemption.RegdNo;
                                        whatsappObj.notification.@params.EvcPrice = item.ExpectedPrice;
                                        whatsappObj.notification.@params.ProductCategory = tblOrderTran.Abbredemption.Abbregistration.NewProductCategory.Description;
                                        whatsappObj.notification.@params.ProductType = tblOrderTran.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description;
                                        string url = _baseConfig.Value.YellowAiUrl;
                                        RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                                        int statusCode = Convert.ToInt32(response.StatusCode);
                                        if (response.Content != null && statusCode == 202)
                                        {
                                            whatasappResponse = JsonConvert.DeserializeObject<WhatasappEvcAutoAllocationResponse>(response.Content);
                                            tblwhatsappmessage2 = new TblWhatsAppMessage();
                                            tblwhatsappmessage2.TemplateName = NotificationConstants.EvcAutoAllocation;
                                            tblwhatsappmessage2.IsActive = true;
                                            tblwhatsappmessage2.PhoneNumber = tblEvcPartner.ContactNumber;
                                            tblwhatsappmessage2.SendDate = DateTime.Now;
                                            tblwhatsappmessage2.MsgId = whatasappResponse.msgId;
                                            _WhatsAppMessageRepository.Create(tblwhatsappmessage2);
                                            _WhatsAppMessageRepository.SaveChanges();
                                        }
                                        #endregion
                                    }
                                    #region Price Calculation 
                                    //     TblOrderTran tblOrderTran = _context.TblOrderTrans.Include(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                    //.Include(x => x.Exchange).ThenInclude(x => x.BusinessPartner).ThenInclude(x => x.BusinessUnit)
                                    //.Where(x => x.IsActive == true && x.OrderTransId == item.orderTransId && x.Exchange.FinalExchangePrice > 0 && x.Exchange.BusinessPartner != null && x.Exchange.BusinessPartner.BusinessUnit != null).FirstOrDefault();



                                    //TblOrderTran tblOrderTran = _OrderTransRepository.GetSingle(u => u.OrderTransId == item.orderTransId);
                                    //TblExchangeOrders = _exchangeOrderRepository.GetSingle(u => u.Id == tblOrderTran.ExchangeId);
                                    //tblProductType = _productTypeRepository.GetSingle(u => u.Id == TblExchangeOrders.ProductTypeId);
                                    //tblProductCategory = _productCategoryRepository.GetSingle(u => u.Id == tblProductType.ProductCatId);
                                    ////var PriceAfterQC = TblExchangeOrders.FinalExchangePrice;
                                    ///
                                    //var per = 0;
                                    //TblEvcpriceRangeMaster tblEvcpriceRangeMaster = _context.TblEvcpriceRangeMasters.Where(x => x.IsActive == true && x.PriceStartRange <= PriceAfterQC && x.PriceEndRange >= PriceAfterQC).FirstOrDefault();
                                    //if (tblEvcpriceRangeMaster != null)
                                    //{
                                    //    per = (int)tblEvcpriceRangeMaster.EvcApplicablePercentage;
                                    //    item.ExpectedPrice = (((int?)(PriceAfterQC + ((PriceAfterQC / 100) * per))));
                                    //}
                                    //else
                                    //{
                                    //    item.ExpectedPrice = (int?)TblExchangeOrders.FinalExchangePrice;
                                    //}
                                    #endregion                                   
                                }
                                else
                                {
                                    //Handal error
                                }
                            }
                            else
                            {
                                //Handal error
                            }
                        }
                        else
                        {
                            NotAssignRNumber.Add(item?.RegdNo);
                            commaSeparatedValues = String.Join(",", NotAssignRNumber);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "GetEVCListbycityAndpin", ex);
            }
            return commaSeparatedValues;
        }
        #endregion

        #region ADDED BY Priyanshi Sahu----This Method Use for Prime Oder EVC Assignment-------Save  EVC Prime Order Data 
        public int AllocateEVCByPrimeOrder(Allocate_EVCFromViewModel allocate_EVCFromViewModels, int userId)
        {
            TblExchangeOrder TblExchangeOrders = new TblExchangeOrder();
            TblProductCategory tblProductCategory = new TblProductCategory();
            TblWalletTransaction tblWalletTransaction = new TblWalletTransaction();
            TblProductType tblProductType = new TblProductType();
            TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = null;
            //WhatasappResponse whatasappResponse = new WhatasappResponse();
            List<string> NotAssignRNumber = new List<string>();
            TblAbbredemption tblAbbredemption = new TblAbbredemption();
            TblAbbregistration tblAbbregistration = new TblAbbregistration();
            EVCPriceViewModel? eVCPriceVM = null;
            try
            {
                if (allocate_EVCFromViewModels != null)
                {
                    TblOrderTran tblOrderTran = _OrderTransRepository.GetSingleOrderWithExchangereference(allocate_EVCFromViewModels.orderTransId);
                    if (tblOrderTran != null)
                    {
                        TblEvcPartner tblEvcPartner = _eVCPartnerRepository.GetEVCPartnerDetails(allocate_EVCFromViewModels.EvcPartnerId ?? 0);
                        if (tblEvcPartner != null && tblEvcPartner.Evcregistration != null)
                        {
                            #region create tblWallet Tranction
                            TblWalletTransaction tblWallet = _walletTransactionRepository.GetSingle(x => x.OrderTransId == allocate_EVCFromViewModels.orderTransId && x.IsActive == true);
                            if (tblWallet == null)
                            {
                                tblWalletTransaction = new TblWalletTransaction();

                                #region Set Order Amount on the basis of EVC Seetener flag
                                if (tblEvcPartner.Evcregistration.IsSweetenerAmtInclude == true)
                                {
                                    #region Get LGC Cost, UTC Cost and GST
                                    eVCPriceVM = _commonManager.CalculateEVCPriceDetailed(tblOrderTran, tblEvcPartner.Evcregistration.IsSweetenerAmtInclude, tblEvcPartner.Evcregistration.GsttypeId ?? 0);
                                    #endregion
                                    tblWalletTransaction.IsOrderAmtWithSweetener = true;
                                    tblWalletTransaction.SweetenerAmt = eVCPriceVM.SweetenerAmt;
                                    tblWalletTransaction.Lgccost = eVCPriceVM.LGCCost;
                                }
                                else
                                {
                                    #region Get LGC Cost, UTC Cost and GST
                                    eVCPriceVM = _commonManager.CalculateEVCPriceDetailed(tblOrderTran, tblEvcPartner.Evcregistration.IsSweetenerAmtInclude, Convert.ToInt32(LoVEnum.GSTInclusive));
                                    #endregion
                                    tblWalletTransaction.IsOrderAmtWithSweetener = false;
                                }
                                tblWalletTransaction.OrderAmount = eVCPriceVM.EVCAmount;
                                tblWalletTransaction.BaseValue = eVCPriceVM.BaseValue;
                                tblWalletTransaction.GsttypeId = eVCPriceVM.GstTypeId;
                                tblWalletTransaction.Cgstamt = eVCPriceVM.CGSTAmount;
                                tblWalletTransaction.Sgstamt = eVCPriceVM.SGSTAmount;
                                #endregion

                                #region Set Difference Amt for Prime Product
                                if ((allocate_EVCFromViewModels.NewExpectedPrice ?? 0) > (tblWalletTransaction.OrderAmount ?? 0))
                                {
                                    tblWalletTransaction.PrimeProductDiffAmt = allocate_EVCFromViewModels.NewExpectedPrice - tblWalletTransaction.OrderAmount;
                                    tblWalletTransaction.OrderAmount = allocate_EVCFromViewModels.NewExpectedPrice;
                                }
                                #endregion 

                                tblWalletTransaction.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted).ToString();
                                tblWalletTransaction.OrderofAssignDate = _currentDatetime;
                                tblWalletTransaction.RegdNo = tblOrderTran.RegdNo;
                                //order Amount
                                //if (allocate_EVCFromViewModels.NewExpectedPrice != null)
                                //{
                                //    tblWalletTransaction.OrderAmount = allocate_EVCFromViewModels.NewExpectedPrice;
                                //}
                                //else
                                //{
                                //    tblWalletTransaction.OrderAmount = allocate_EVCFromViewModels.ExpectedPrice;
                                //}
                                tblWalletTransaction.OrderType = tblOrderTran.OrderType.ToString();//Get from Enum 
                                tblWalletTransaction.IsActive = true;
                                tblWalletTransaction.CreatedBy = userId;//Loging user
                                tblWalletTransaction.CreatedDate = _currentDatetime;
                                tblWalletTransaction.EvcpartnerId = allocate_EVCFromViewModels.EvcPartnerId;
                                tblWalletTransaction.EvcregistrationId = tblEvcPartner.EvcregistrationId;
                                tblWalletTransaction.OrderTransId = tblOrderTran.OrderTransId;
                                tblWalletTransaction.IsPrimeProductId = true;
                                tblWalletTransaction.ReassignCount = 0;
                                tblWalletTransaction.ModifiedDate = _currentDatetime;
                                tblWalletTransaction.ModifiedBy = userId;
                                _walletTransactionRepository.Create(tblWalletTransaction);
                                _walletTransactionRepository.SaveChanges();
                            }
                            else
                            {
                                #region Set Order Amount on the basis of EVC Seetener flag
                                if (tblEvcPartner.Evcregistration.IsSweetenerAmtInclude == true)
                                {
                                    #region Get LGC Cost, UTC Cost and GST
                                    eVCPriceVM = _commonManager.CalculateEVCPriceDetailed(tblOrderTran, tblEvcPartner.Evcregistration.IsSweetenerAmtInclude, tblEvcPartner.Evcregistration.GsttypeId ?? 0);
                                    #endregion
                                    tblWalletTransaction.IsOrderAmtWithSweetener = true;
                                    tblWalletTransaction.SweetenerAmt = eVCPriceVM.SweetenerAmt;
                                    tblWalletTransaction.Lgccost = eVCPriceVM.LGCCost;
                                    tblWalletTransaction.OrderAmount = eVCPriceVM.EVCAmount;
                                }
                                else
                                {
                                    #region Get LGC Cost, UTC Cost and GST
                                    eVCPriceVM = _commonManager.CalculateEVCPriceDetailed(tblOrderTran, tblEvcPartner.Evcregistration.IsSweetenerAmtInclude, Convert.ToInt32(LoVEnum.GSTInclusive));
                                    #endregion
                                    tblWalletTransaction.OrderAmount = eVCPriceVM.EVCAmount;
                                    tblWalletTransaction.IsOrderAmtWithSweetener = false;
                                }
                                tblWalletTransaction.BaseValue = eVCPriceVM.BaseValue;
                                tblWalletTransaction.GsttypeId = eVCPriceVM.GstTypeId;
                                tblWalletTransaction.Cgstamt = eVCPriceVM.CGSTAmount;
                                tblWalletTransaction.Sgstamt = eVCPriceVM.SGSTAmount;
                                #endregion

                                #region Set Difference Amt for Prime Product
                                if ((allocate_EVCFromViewModels.NewExpectedPrice ?? 0) > (tblWalletTransaction.OrderAmount ?? 0))
                                {
                                    tblWalletTransaction.PrimeProductDiffAmt = allocate_EVCFromViewModels.NewExpectedPrice - tblWalletTransaction.OrderAmount;
                                    tblWalletTransaction.OrderAmount = allocate_EVCFromViewModels.NewExpectedPrice;
                                }
                                #endregion

                                //tblWalletTransaction.SponserOrderNo = tblOrderTran.Exchange.SponsorOrderNumber;
                                tblWallet.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted).ToString();
                                tblWallet.OrderofAssignDate = _currentDatetime;
                                tblWallet.RegdNo = tblOrderTran.RegdNo;
                                //order Amount
                                //if (allocate_EVCFromViewModels.NewExpectedPrice != null)
                                //{
                                //    tblWalletTransaction.OrderAmount = allocate_EVCFromViewModels.NewExpectedPrice;
                                //}
                                //else
                                //{
                                //    tblWalletTransaction.OrderAmount = allocate_EVCFromViewModels.ExpectedPrice;
                                //}

                                tblWallet.OrderType = tblOrderTran.OrderType.ToString();//Get from Enum 
                                tblWallet.IsActive = true;
                                tblWallet.ModifiedBy = userId;//Loging user
                                tblWallet.ModifiedDate = _currentDatetime;
                                tblWalletTransaction.EvcpartnerId = allocate_EVCFromViewModels.EvcPartnerId;
                                tblWalletTransaction.EvcregistrationId = tblEvcPartner.EvcregistrationId;
                                tblWallet.OrderTransId = tblOrderTran.OrderTransId;
                                tblWallet.IsPrimeProductId = true;
                                tblWallet.ReassignCount = 0;
                                _walletTransactionRepository.Update(tblWallet);
                                _walletTransactionRepository.SaveChanges();
                            }
                            #endregion
                            #region Update into TblOrderTrans
                            var Ordertype = _OrderTransRepository.UpdateTransRecordStatus(allocate_EVCFromViewModels.orderTransId, Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted), userId);
                            #endregion

                            if (Ordertype == Convert.ToInt32(OrderTypeEnum.Exchange))
                            {
                                #region Update into TblExchangeOrders
                                TblExchangeOrders = _exchangeOrderRepository.GetSingle(x => x.IsActive == true && x.RegdNo == tblOrderTran.RegdNo);
                                if (TblExchangeOrders != null)
                                {
                                    TblExchangeOrders.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted);
                                    TblExchangeOrders.OrderStatus = "EVC_Assign";
                                    TblExchangeOrders.ModifiedBy = userId;
                                    TblExchangeOrders.ModifiedDate = _currentDatetime;
                                    _exchangeOrderRepository.Update(TblExchangeOrders);
                                    _exchangeOrderRepository.SaveChanges();
                                }
                                #endregion
                                #region Insert into tblexchangeabbhistory
                                tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                tblExchangeAbbstatusHistory.OrderType = (int)tblOrderTran.OrderType;
                                tblExchangeAbbstatusHistory.SponsorOrderNumber = tblOrderTran.Exchange.SponsorOrderNumber;
                                tblExchangeAbbstatusHistory.RegdNo = tblOrderTran.Exchange.RegdNo;
                                tblExchangeAbbstatusHistory.ZohoSponsorId = tblOrderTran.Exchange.ZohoSponsorOrderId != null ? TblExchangeOrders.ZohoSponsorOrderId : string.Empty;
                                tblExchangeAbbstatusHistory.CustId = tblOrderTran.Exchange.CustomerDetailsId;
                                tblExchangeAbbstatusHistory.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted);
                                tblExchangeAbbstatusHistory.IsActive = true;
                                tblExchangeAbbstatusHistory.CreatedBy = userId;
                                tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                                tblExchangeAbbstatusHistory.Evcid = tblEvcPartner.EvcregistrationId;
                                _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                                //_exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                                //_exchangeABBStatusHistoryRepository.SaveChanges();
                                #endregion
                            }
                            else
                            {
                                #region Update into TblAbbredmption
                                tblAbbredemption = _aBBRedemptionRepository.GetSingle(x => x.IsActive == true && x.RegdNo == tblOrderTran.RegdNo);
                                if (tblAbbredemption != null)
                                {
                                    tblAbbredemption.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted);
                                    tblAbbredemption.ModifiedBy = userId;
                                    tblAbbredemption.ModifiedDate = _currentDatetime;
                                    _aBBRedemptionRepository.Update(tblAbbredemption);
                                    _aBBRedemptionRepository.SaveChanges();
                                }
                                #endregion
                                #region Update into TblAbbregistration commite
                                //tblAbbregistration = _abbRegistrationRepository.GetSingle(x => x.IsActive == true && x.RegdNo == tblOrderTran.RegdNo);
                                //if (tblAbbregistration != null)
                                //{
                                //    tblAbbregistration.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted);
                                //    tblAbbregistration.ModifiedBy = userId;
                                //    tblAbbregistration.ModifiedDate = _currentDatetime;
                                //    _abbRegistrationRepository.Update(tblAbbregistration);
                                //    _abbRegistrationRepository.SaveChanges();
                                //}
                                #endregion
                                #region Insert into tblexchangeabbhistory
                                tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                tblExchangeAbbstatusHistory.OrderType = (int)tblOrderTran.OrderType;
                                //  tblExchangeAbbstatusHistory.SponsorOrderNumber = tblOrderTran.Abbredemption.SponsorOrderNumber;
                                tblExchangeAbbstatusHistory.RegdNo = tblOrderTran.RegdNo;
                                // tblExchangeAbbstatusHistory.ZohoSponsorId = tblOrderTran.Exchange.ZohoSponsorOrderId != null ? TblExchangeOrders.ZohoSponsorOrderId : string.Empty;
                                tblExchangeAbbstatusHistory.CustId = tblOrderTran.Abbredemption.CustomerDetailsId;
                                tblExchangeAbbstatusHistory.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted);
                                tblExchangeAbbstatusHistory.IsActive = true;
                                tblExchangeAbbstatusHistory.CreatedBy = userId;
                                tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                                tblExchangeAbbstatusHistory.Evcid = tblEvcPartner.EvcregistrationId;
                                _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                                //_exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                                //_exchangeABBStatusHistoryRepository.SaveChanges();
                                #endregion
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "AllocateEVCByPrimeOrder", ex);
            }
            return tblWalletTransaction.WalletTransactionId;
        }
        #endregion

        #region ***********ADDED BY Priyanshi Sahu----- this Method use for EVC Assignment --select DropDown EVC --get All Details For EVC 
        public EVC_PartnerViewModel GetEVCByEVCRegNo(int evcPartnerId, int? orderTransId)
        {

            EVC_PartnerViewModel eVcPartnerObj = new EVC_PartnerViewModel();
            List<Allocate_EVCFromViewModel> Allocate_EVCFromViewModels = new List<Allocate_EVCFromViewModel>();
            Allocate_EVCFromViewModel Allocate_EVCFromViewModels1 = new Allocate_EVCFromViewModel();
            EVCClearBalanceViewModel? eVCClearBalanceViewModel = new EVCClearBalanceViewModel();

            try
            {
                //TblEvcregistrations = _evcRepository.GetSingle(x => x.IsActive == true && x.EvcregistrationId == evcregdId);
                TblEvcPartner tblEvcPartner = _eVCPartnerRepository.GetEVCPartnerDetails(evcPartnerId);
                if (tblEvcPartner != null)
                {
                    eVcPartnerObj.EvcregdNo = tblEvcPartner.Evcregistration.EvcregdNo + " - " + tblEvcPartner.Evcregistration.BussinessName;

                    eVcPartnerObj.Address = tblEvcPartner.Address1 + " " + tblEvcPartner.Address2;
                    eVcPartnerObj.PinCode = tblEvcPartner.PinCode;
                    eVcPartnerObj.EvcwalletAmount = tblEvcPartner.Evcregistration.EvcwalletAmount > 0 ? tblEvcPartner.Evcregistration.EvcwalletAmount : 0;
                    eVcPartnerObj.EvcStoreCode = tblEvcPartner.EvcStoreCode;
                    Allocate_EVCFromViewModels1.EVCLists = new List<EVcList>();
                    eVCClearBalanceViewModel = _commonManager.CalculateEVCClearBalance(tblEvcPartner.Evcregistration.EvcregistrationId);
                    eVcPartnerObj.RuningBalance = eVCClearBalanceViewModel.clearBalance ?? 0;
                    int expectedEVCPrice = _commonManager.CalculateEVCPriceNew(orderTransId ?? 0, tblEvcPartner?.Evcregistration?.IsSweetenerAmtInclude, tblEvcPartner?.Evcregistration?.GsttypeId ?? 0);
                    eVcPartnerObj.ExpectedPrice = expectedEVCPrice;
                    eVcPartnerObj.StateName = tblEvcPartner.State.Name;
                    eVcPartnerObj.CityName = tblEvcPartner.City.Name;
                }
                return eVcPartnerObj;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "GetEVCByEVCRegNo", ex);
            }
            return eVcPartnerObj;
        }
        #endregion

        #region ADDED BY Priyanshi Sahu---this method use for EVCDispute--Admin /Portal ---use for get valid order by EvcREgId 
        public IList<EVCWalletTransaction> GetOrderByEvcregistrationId(int EvcregistrationId)
        {
            IList<EVCWalletTransaction> EVCWalletTransactions = null;
            List<TblEvcdispute> tblEvcdisputes = new List<TblEvcdispute>();
            List<TblWalletTransaction> tblWalletTransactionList = new List<TblWalletTransaction>();
            List<TblWalletTransaction> tblWalletTransactionList1 = new List<TblWalletTransaction>();
            try
            {

                tblWalletTransactionList = _walletTransactionRepository.GetList(x => x.IsActive == true
                && x.EvcregistrationId == EvcregistrationId && x.OrderTransId != null && x.OrderOfInprogressDate != null
                && x.OrderofAssignDate != null && x.OrderOfDeliverdDate != null && x.StatusId == (Convert.ToInt32(OrderStatusEnum.Posted)).ToString()
                && x.OrderOfCompleteDate > DateTime.Now.AddDays(-2)).ToList();

                foreach (var item in tblWalletTransactionList)
                {
                    tblEvcdisputes = _eVCDisputeRepository.GetList(x => x.IsActive == true && x.OrderTransId == item.OrderTransId).ToList();
                    if (tblEvcdisputes == null || tblEvcdisputes.Count == 0)
                    {
                        tblWalletTransactionList1.Add(item);
                    }
                }
                if (tblWalletTransactionList1 != null && tblWalletTransactionList1.Count > 0)
                {
                    EVCWalletTransactions = _mapper.Map<IList<TblWalletTransaction>, IList<EVCWalletTransaction>>(tblWalletTransactionList1);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "GetOrderByEvcregistrationId", ex);
            }
            return EVCWalletTransactions;
        }
        #endregion

        #region ADDED BY Priyanshi sahu---- this method use for Primeproduct Assignment---Get EVC BY City and state and pin 
        public IList<EVcList> GetEVCListbycityAndpin(string state, string city, string pin)
        {
            IList<EVcList> eVcLists = null;
            List<TblEvcregistration> tblEvcregistrations = new List<TblEvcregistration>();
            // TblUseRole TblUseRole = null;
            try
            {
                // TblCity TblCitys = _cityRepository.GetSingle(x => x.IsActive == true && x.Name == city);
                TblCity TblCitys = _context.TblCities.Where(x => x.IsActive == true && x.Name == city).FirstOrDefault();
                TblState tblState = _context.TblStates.Where(x => x.IsActive == true && x.Name == state).FirstOrDefault();

                if (tblState != null)
                {
                    tblEvcregistrations = _evcRepository.GetList(x => x.IsActive == true && (x.StateId == tblState.StateId)
                        && x.Isevcapprovrd == true).ToList();
                }
                else if (TblCitys != null && tblEvcregistrations.Count == 0)
                {
                    tblEvcregistrations = _evcRepository.GetList(x => x.IsActive == true && (x.CityId == TblCitys.CityId)
                            && x.Isevcapprovrd == true).ToList();
                }
                else if (pin != null && tblEvcregistrations.Count == 0)
                {
                    tblEvcregistrations = _evcRepository.GetList(x => x.IsActive == true && (x.PinCode == pin)
                               && x.Isevcapprovrd == true).ToList();
                }

                if (tblEvcregistrations != null && tblEvcregistrations.Count > 0)
                {
                    eVcLists = _mapper.Map<IList<TblEvcregistration>, IList<EVcList>>(tblEvcregistrations);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "GetEVCListbycityAndpin", ex);
            }
            return eVcLists;
        }
        #endregion

        #region ADDED BY Priyanshi sahu---- Reassign get all detalis for from get method
        public ReassignFromViewModel GetReassignEVC(int OId)
        {
            ReassignFromViewModel reassignFromViewModel = new ReassignFromViewModel();
            List<TblWalletTransaction> TblWalletTransactions = new List<TblWalletTransaction>();
            //decimal? IsProgress = 0;
            //decimal? IsDeleverd = 0;
            //decimal? RuningBlns = 0;
            TblAreaLocality tblAreaLocality = null;
            TblOrderQc tblOrderQc = null;
            //order details
            TblOrderTran tblOrderTran = _OrderTransRepository.GetSingleOrderWithExchangereference(Convert.ToInt32(OId));
            reassignFromViewModel.RegdNo = tblOrderTran.RegdNo != null ? tblOrderTran.RegdNo : string.Empty;
            reassignFromViewModel.StatusId = tblOrderTran.StatusId != null && tblOrderTran.StatusId > 0 ? tblOrderTran.StatusId : 0;
            reassignFromViewModel.ActualBasePrice = tblOrderTran.FinalPriceAfterQc > 0 ? tblOrderTran.FinalPriceAfterQc : 0;
            reassignFromViewModel.CustCity = (tblOrderTran.OrderType == (Convert.ToInt32(OrderTypeEnum.Exchange)) ? (tblOrderTran?.Exchange?.CustomerDetails?.City) : tblOrderTran.Abbredemption.CustomerDetails.City);
            reassignFromViewModel.CustPin = (tblOrderTran.OrderType == (Convert.ToInt32(OrderTypeEnum.Exchange)) ? (tblOrderTran?.Exchange?.CustomerDetails?.ZipCode) : tblOrderTran.Abbredemption.CustomerDetails.ZipCode);
            reassignFromViewModel.ExchAreaLocalityId = (tblOrderTran.OrderType == (Convert.ToInt32(OrderTypeEnum.Exchange)) ? Convert.ToInt32(tblOrderTran?.Exchange?.CustomerDetails?.AreaLocalityId) : Convert.ToInt32(tblOrderTran.Abbredemption.CustomerDetails.AreaLocalityId));
            reassignFromViewModel.ABBAreaLocalityId = (tblOrderTran.OrderType == (Convert.ToInt32(OrderTypeEnum.ABB)) ? Convert.ToInt32(tblOrderTran?.Abbredemption?.Abbregistration?.Customer?.AreaLocalityId) : Convert.ToInt32(tblOrderTran?.Abbredemption?.Abbregistration?.Customer?.AreaLocalityId));
            if (reassignFromViewModel.ExchAreaLocalityId > 0)
            {
                tblAreaLocality = _areaLocalityRepository.GetArealocalityById(tblOrderTran.Exchange.CustomerDetails.AreaLocalityId);
                if (tblAreaLocality != null)
                {
                    reassignFromViewModel.AreaLocality = tblAreaLocality.AreaLocality;
                }
            }
            if (reassignFromViewModel.ABBAreaLocalityId > 0)
            {
                tblAreaLocality = _areaLocalityRepository.GetArealocalityById(tblOrderTran.Abbredemption.Abbregistration.Customer.AreaLocalityId);
                if (tblAreaLocality != null)
                {
                    reassignFromViewModel.AreaLocality = tblAreaLocality.AreaLocality;
                }
            }

            reassignFromViewModel.ExchProdGroup = (tblOrderTran.OrderType == (Convert.ToInt32(OrderTypeEnum.Exchange)) ?
(tblOrderTran?.Exchange?.ProductType?.ProductCat?.Description) : tblOrderTran.Abbredemption.Abbregistration.NewProductCategory.Description);

            reassignFromViewModel.ProductCatId = (tblOrderTran.OrderType == (Convert.ToInt32(OrderTypeEnum.Exchange)) ?
(tblOrderTran?.Exchange?.ProductType?.ProductCatId) : tblOrderTran.Abbredemption.Abbregistration.NewProductCategoryId);

            reassignFromViewModel.OldProdType = (tblOrderTran.OrderType == (Convert.ToInt32(OrderTypeEnum.Exchange)) ?
(tblOrderTran?.Exchange?.ProductType?.Description) : tblOrderTran.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description);

            reassignFromViewModel.orderTransId = tblOrderTran.OrderTransId;
            reassignFromViewModel.Custstate = (tblOrderTran.OrderType == (Convert.ToInt32(OrderTypeEnum.Exchange)) ? (tblOrderTran?.Exchange?.CustomerDetails?.State
          ) : tblOrderTran.Abbredemption.CustomerDetails.State);
            tblOrderQc = _orderQCRepository.GetQcorderBytransId(Convert.ToInt32(OId));
            if (tblOrderQc != null)
            {
                reassignFromViewModel.ActualProdQltyAtQc = tblOrderQc.QualityAfterQc;
            }
            else
            {
                reassignFromViewModel.ActualProdQltyAtQc = string.Empty;
            }
            reassignFromViewModel.ordertype = (int)tblOrderTran.OrderType;

            //Old EvcDetails
            TblWalletTransaction tblWalletTransaction = _walletTransactionRepository.GetSingleEvcDetails(OId);
            if (tblWalletTransaction != null)
            {
                reassignFromViewModel.OldEvcregistrationId = (int)tblWalletTransaction.EvcregistrationId > 0 ? (int)tblWalletTransaction.EvcregistrationId : 0; ;
                reassignFromViewModel.OldEvcregdNo = tblWalletTransaction?.Evcregistration?.EvcregdNo != null ? tblWalletTransaction.Evcregistration.EvcregdNo + "-" + tblWalletTransaction.Evcregistration.BussinessName : string.Empty;
                reassignFromViewModel.OldEvcName = tblWalletTransaction?.Evcregistration?.ContactPerson != null ? tblWalletTransaction.Evcregistration.ContactPerson : string.Empty;
                reassignFromViewModel.OldEVCState = tblWalletTransaction?.Evcpartner?.State?.Name != null ? tblWalletTransaction?.Evcpartner?.State.Name : string.Empty;
                reassignFromViewModel.OldEVCCity = tblWalletTransaction?.Evcpartner?.City?.Name != null ? tblWalletTransaction?.Evcpartner?.City.Name : string.Empty;
                reassignFromViewModel.OldEVCPinCode = tblWalletTransaction?.Evcpartner?.PinCode != null ? tblWalletTransaction?.Evcpartner?.PinCode : string.Empty;
                reassignFromViewModel.OldEVCEvcwalletAmount = tblWalletTransaction?.Evcregistration?.EvcwalletAmount > 0 ? tblWalletTransaction.Evcregistration.EvcwalletAmount : 0;
                reassignFromViewModel.OldEvcExpectedPrice = (int?)(tblWalletTransaction?.OrderAmount > 0 ? tblWalletTransaction.OrderAmount : 0);
                reassignFromViewModel.OldEVCStoreCode = tblWalletTransaction?.Evcpartner?.EvcStoreCode != null ? tblWalletTransaction?.Evcpartner?.EvcStoreCode : string.Empty;
                if (tblWalletTransaction.IsPrimeProductId != null)
                {
                    if (tblWalletTransaction.IsPrimeProductId == true)
                    {
                        reassignFromViewModel.PrimeProduct = "yes";
                    }
                    else
                    {
                        reassignFromViewModel.PrimeProduct = "No";
                    }
                }
                //calculate clear balance 
                EVCClearBalanceViewModel? eVCClearBalanceViewModel = new EVCClearBalanceViewModel();
                eVCClearBalanceViewModel = _commonManager.CalculateEVCClearBalance(reassignFromViewModel.OldEvcregistrationId);
                if (eVCClearBalanceViewModel != null)
                {
                    reassignFromViewModel.OldEVCClearBalance = eVCClearBalanceViewModel.clearBalance ;
                }

            }
            return reassignFromViewModel;
        }
        #endregion

        #region ADDED BY Priyanshi sahu---- this method use for REassign Assignment---Get EVC BY City and state and pin 
        //public IList<EVcListRessign> GetEVCListforEVCReassign(string? ActualProdQltyAtQc, int ProductCatId, string? pin, int EVCId, int? statusId, int OrdertransId, int ExpectedPrice, int ordertype)
        //{
        //    IList<EVcListRessign> eVcLists = new List<EVcListRessign>();
        //    List<TblEvcregistration> tblEvcregistrations = new List<TblEvcregistration>();
        //    List<TblWalletTransaction> TblWalletTransactions = new List<TblWalletTransaction>();
        //    IList<TblEvcPartner> tblEvcPartnerList = new List<TblEvcPartner>();

        //    try
        //    {
        //        if (!string.IsNullOrEmpty(pin) && ProductCatId > 0)
        //        {
        //            if (statusId == Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted))
        //            {
        //                if (!string.IsNullOrEmpty(pin))
        //                {
        //                    tblEvcPartnerList = _eVCPartnerRepository.GetAllEvcPartnerListByPincode(pin, ProductCatId, ActualProdQltyAtQc, ordertype);
        //                }
        //            }
        //            else
        //            {
        //                if (!string.IsNullOrEmpty(pin) && ProductCatId > 0)
        //                {
        //                    if (!string.IsNullOrEmpty(pin))
        //                    {
        //                        tblEvcPartnerList = _eVCPartnerRepository.GetAllEvcPartnerListByPincode(pin, ProductCatId, ActualProdQltyAtQc, ordertype);
        //                    }

        //                    if (statusId != Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted))
        //                    {
        //                        if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
        //                        {
        //                            tblEvcPartnerList = _eVCPartnerRepository.GetEvcPartnerListHavingClearBalance((List<TblEvcPartner>)tblEvcPartnerList, ExpectedPrice);
        //                        }
        //                    }
        //                }
        //            }
        //            if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
        //            {
        //                tblEvcPartnerList = tblEvcPartnerList.Where(x => x.EvcregistrationId != EVCId).ToList();
        //                if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
        //                {
        //                    List<EVC_PartnerViewModel> eVCPartnerViewModels1 = new List<EVC_PartnerViewModel>();
        //                    foreach (var item in tblEvcPartnerList)
        //                    {
        //                        // Create an EVcList object and add it to the EVCList
        //                        EVcListRessign evclist = new EVcListRessign
        //                        {
        //                            EvcPartnerId = item.EvcPartnerId,
        //                            EvcregistrationId = item.EvcregistrationId,
        //                            EvcregdNo = item.Evcregistration.EvcregdNo,
        //                            BussinessName = item.Evcregistration.BussinessName
        //                        };
        //                        if (evclist != null)
        //                        {
        //                            eVcLists.Add(evclist);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.WriteErrorToDB("EVCManager", "GetEVCListbycityAndpin", ex);
        //    }
        //    return eVcLists;
        //}

        #endregion

        #region ADDED BY Priyanshi Sahu----This Method Use for Reassign EVC EVC Assignment-------update Reassign EVC
        public int SaveReassignEVC(ReassignFromViewModel reassignFromViewModel, int userId)
        {

            TblWalletTransaction tblWalletTransaction = new TblWalletTransaction();
            TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = null;
            TblOrderLgc tblOrderLgc = null;
            EVCPriceViewModel? eVCPriceVM = null;

            int? result = 0;

            if (reassignFromViewModel != null)
            {
                tblWalletTransaction = _context.TblWalletTransactions.Where(x => x.OrderTransId == reassignFromViewModel.orderTransId).FirstOrDefault();

                TblOrderTran tblOrderTran = _OrderTransRepository.GetSingleOrderWithExchangereference(reassignFromViewModel.orderTransId);

                if (tblOrderTran != null && tblWalletTransaction != null)
                {
                    #region tblWalletTransaction update
                    TblEvcPartner tblEvcPartner = _eVCPartnerRepository.GetEVCPartnerDetails(reassignFromViewModel.newEVCPartnerId);
                    if (tblEvcPartner != null && tblEvcPartner.Evcregistration != null)
                    {
                        #region Set Order Amount on the basis of EVC Seetener flag
                        if (tblEvcPartner.Evcregistration.IsSweetenerAmtInclude == true)
                        {
                            #region Get LGC Cost, UTC Cost and GST
                            eVCPriceVM = _commonManager.CalculateEVCPriceDetailed(tblOrderTran, tblEvcPartner.Evcregistration.IsSweetenerAmtInclude, tblEvcPartner.Evcregistration.GsttypeId ?? 0);
                            #endregion
                            tblWalletTransaction.IsOrderAmtWithSweetener = true;
                            tblWalletTransaction.SweetenerAmt = eVCPriceVM.SweetenerAmt;
                            tblWalletTransaction.Lgccost = eVCPriceVM.LGCCost;
                            tblWalletTransaction.OrderAmount = eVCPriceVM.EVCAmount;
                        }
                        else
                        {
                            #region Get LGC Cost, UTC Cost and GST
                            eVCPriceVM = _commonManager.CalculateEVCPriceDetailed(tblOrderTran, tblEvcPartner.Evcregistration.IsSweetenerAmtInclude, Convert.ToInt32(LoVEnum.GSTInclusive));
                            #endregion
                            tblWalletTransaction.OrderAmount = eVCPriceVM.EVCAmount;
                            tblWalletTransaction.IsOrderAmtWithSweetener = false;
                        }
                        tblWalletTransaction.BaseValue = eVCPriceVM.BaseValue;
                        tblWalletTransaction.GsttypeId = eVCPriceVM.GstTypeId;
                        tblWalletTransaction.Cgstamt = eVCPriceVM.CGSTAmount;
                        tblWalletTransaction.Sgstamt = eVCPriceVM.SGSTAmount;
                        #endregion

                        tblWalletTransaction.OldEvcid = tblWalletTransaction.EvcregistrationId;
                        tblWalletTransaction.NewEvcid = tblEvcPartner.EvcregistrationId;
                        tblWalletTransaction.EvcregistrationId = tblEvcPartner.EvcregistrationId;
                        tblWalletTransaction.ReassignCount = (tblWalletTransaction.ReassignCount ?? 0) + 1;
                        tblWalletTransaction.EvcpartnerId = reassignFromViewModel.newEVCPartnerId;
                        if (reassignFromViewModel.StatusId == Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted))
                        {
                            tblWalletTransaction.OrderofAssignDate = _currentDatetime;
                            tblWalletTransaction.OrderOfInprogressDate = null;
                            tblWalletTransaction.OrderOfDeliverdDate = null;
                            tblWalletTransaction.OrderOfCompleteDate = null;
                        }
                        tblWalletTransaction.ModifiedBy = userId;
                        tblWalletTransaction.ModifiedDate = _currentDatetime;

                        _walletTransactionRepository.Update(tblWalletTransaction);
                        _walletTransactionRepository.SaveChanges();

                        #endregion

                        #region tblOrderLGC
                        if (reassignFromViewModel.StatusId != null && reassignFromViewModel.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup) || reassignFromViewModel.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenforLogistics) || reassignFromViewModel.StatusId == Convert.ToInt32(OrderStatusEnum.PickupDecline))
                        {
                            tblOrderLgc = _context.TblOrderLgcs.Where(x => x.IsActive == true && x.OrderTransId == tblOrderTran.OrderTransId).FirstOrDefault();
                            if (tblOrderLgc != null && tblOrderLgc.OrderLgcid > 0)
                            {
                                tblOrderLgc.EvcregistrationId = tblEvcPartner.EvcregistrationId;
                                tblOrderLgc.EvcpartnerId = reassignFromViewModel.newEVCPartnerId;
                                tblOrderLgc.ModifiedBy = userId;
                                tblOrderLgc.ModifiedDate = _currentDatetime;
                                _orderLGCRepository.Update(tblOrderLgc);
                                _orderLGCRepository.SaveChanges();
                            }
                        }
                        #endregion

                        #region Insert into tblexchangeabbhistory
                        tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                        tblExchangeAbbstatusHistory.OrderType = (int)tblOrderTran.OrderType;
                        tblExchangeAbbstatusHistory.RegdNo = tblOrderTran.RegdNo;
                        if (tblOrderTran.OrderType != null && tblOrderTran.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                        {
                            tblExchangeAbbstatusHistory.CustId = tblOrderTran.Exchange.CustomerDetailsId;
                            tblExchangeAbbstatusHistory.SponsorOrderNumber = tblOrderTran.SponsorOrderNumber;
                        }
                        else
                        {
                            tblExchangeAbbstatusHistory.CustId = tblOrderTran.Abbredemption.CustomerDetailsId;
                        }
                        tblExchangeAbbstatusHistory.StatusId = reassignFromViewModel.StatusId != null ? reassignFromViewModel.StatusId : 0; /*Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted);*/
                        tblExchangeAbbstatusHistory.IsActive = true;
                        tblExchangeAbbstatusHistory.CreatedBy = userId;
                        tblExchangeAbbstatusHistory.Evcid = tblEvcPartner.EvcregistrationId;
                        tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                        tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;

                        tblExchangeAbbstatusHistory.Comment = reassignFromViewModel.ReallocationComment != null ? reassignFromViewModel.ReallocationComment : string.Empty;
                        _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);

                        #endregion

                        #region update TblVehicleJourneyTrackingDetails by Kranti date 24/08/2023
                        TblVehicleJourneyTrackingDetail? tblVehicleJourneyTrackingDetail = _vehicleJourneyTrackingDetails.GetSingle(x => x.IsActive == true && x.OrderTransId == tblOrderTran.OrderTransId);
                        if (tblVehicleJourneyTrackingDetail != null)
                        {
                            tblVehicleJourneyTrackingDetail.DropLatt = null;
                            tblVehicleJourneyTrackingDetail.DropLong = null;
                            tblVehicleJourneyTrackingDetail.Evcid = tblEvcPartner.EvcregistrationId;
                            tblVehicleJourneyTrackingDetail.ModifiedBy = userId;
                            tblVehicleJourneyTrackingDetail.ModifiedDate = _currentDatetime;
                            tblVehicleJourneyTrackingDetail.EvcpartnerId = reassignFromViewModel.newEVCPartnerId;
                            _vehicleJourneyTrackingDetails.Update(tblVehicleJourneyTrackingDetail);
                            _vehicleJourneyTrackingDetails.SaveChanges();

                            if (_baseConfig.Value.SendPushNotification == true)
                            {
                                if (tblVehicleJourneyTrackingDetail.Evcid > 0)
                                {
                                    TblEvcregistration? tblEvcregistration = _evcRepository.GetSingle(x => x.IsActive == true && x.EvcregistrationId == tblVehicleJourneyTrackingDetail.Evcid);
                                    if (tblEvcregistration != null)
                                    {
                                        var Notification = _pushNotificationManager.SendNotification(tblVehicleJourneyTrackingDetail.ServicePartnerId, tblVehicleJourneyTrackingDetail.DriverId, EnumHelper.DescriptionAttr(NotificationEnum.EVCChangedbyAdmin), tblEvcregistration.BussinessName, tblOrderTran.RegdNo);
                                    }
                                }
                            }
                            #region added latt long of new EVC Partner
                            EvcLattLongModel evcLattLong = CalculateLattLong(reassignFromViewModel.newEVCPartnerId);
                            if (evcLattLong != null)
                            {
                                TblVehicleJourneyTrackingDetail? tblVehicleJourneyTrackingDetailO = _vehicleJourneyTrackingDetails.GetVehicleJourneyTrackingDetail(tblOrderTran.OrderTransId);

                                tblVehicleJourneyTrackingDetailO.DropLatt = evcLattLong.EvcPartnerlatitude.ToString();
                                tblVehicleJourneyTrackingDetailO.DropLong = evcLattLong.EvcPartnerlongitude.ToString();
                                tblVehicleJourneyTrackingDetailO.ModifiedBy = userId;
                                tblVehicleJourneyTrackingDetailO.ModifiedDate = _currentDatetime;
                                _vehicleJourneyTrackingDetails.UpdateLattLong(tblVehicleJourneyTrackingDetailO);

                            }
                            #endregion
                        }
                        #endregion
                    }
                }
            }
            result = reassignFromViewModel.StatusId != null ? reassignFromViewModel.StatusId : 0;
            return (int)result;
        }
        #endregion

        #region Calculate latt, long of new EVC Partner
        public EvcLattLongModel CalculateLattLong(int EvcpartnerId)
        {
            string APIkey = _baseConfig.Value.LattLongKey;
            TblEvcPartner tblEvcPartner = new TblEvcPartner();
            tblEvcPartner = _eVCPartnerRepository.GetEVCPartnerDetails(EvcpartnerId);
            var address = tblEvcPartner.Address1 + " " + tblEvcPartner.Address2 + " " + tblEvcPartner.PinCode + " " + tblEvcPartner.City.Name + " " + tblEvcPartner.State.Name;
            var locationService = new GoogleLocationService(APIkey);
            var point = locationService.GetLatLongFromAddress(address);

            EvcLattLongModel evcLattLongModel = new EvcLattLongModel();
            evcLattLongModel.EvcPartnerlatitude = point.Latitude;
            evcLattLongModel.EvcPartnerlongitude = point.Longitude;

            return evcLattLongModel;
        }
        #endregion

        #region  ADDED BY ASHWIN  _ THIS METHOD TO GET DETAILS OF LOGIN USER WITH HELP OF LOGINSESSION LOGLIN ID
        /// <summary>
        /// ADDED BY ASHWIN  _ THIS METHOD TO GET DETAILS OF LOGIN USER WITH HELP OF LOGINSESSION LOGLIN ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EVC_DashboardViewModel EvcByUserId(int id)
        {
            TblEvcregistration TblEvcregistration = null;
            EVC_RegistrationPortalViewModel eVC_RegistrationPortalViewModel = null;
            List<TblWalletTransaction> TblWalletTransactions = null;
            EVC_DashboardViewModel eVC_DashboardViewModel = new EVC_DashboardViewModel();

            try
            {
                TblEvcregistration = _evcRepository.GetSingle(where: x => x.IsActive == true && x.UserId == id);
                if (TblEvcregistration != null)
                {
                    eVC_RegistrationPortalViewModel = _mapper.Map<TblEvcregistration, EVC_RegistrationPortalViewModel>(TblEvcregistration);
                    TblWalletTransactions = _context.TblWalletTransactions.Where(x => x.EvcregistrationId == TblEvcregistration.EvcregistrationId && x.IsActive == true && x.StatusId != 26.ToString()).ToList();
                    eVC_DashboardViewModel.walletAmount = eVC_RegistrationPortalViewModel.EvcwalletAmount;
                    decimal? IsProgress = 0;
                    decimal? IsDeleverd = 0;
                    decimal? RuningBlns = 0;

                    if (TblWalletTransactions != null)
                    {
                        EVC_DashboardViewModel eVC_DashboardViewModel1 = new EVC_DashboardViewModel();
                        EVCClearBalanceViewModel? eVCClearBalanceViewModel = new EVCClearBalanceViewModel();
                        eVCClearBalanceViewModel = _commonManager.CalculateEVCClearBalance(TblEvcregistration.EvcregistrationId);
                        if (eVCClearBalanceViewModel != null)
                        {
                            eVC_DashboardViewModel.clearBalance =  eVCClearBalanceViewModel.clearBalance;
                            eVC_DashboardViewModel.Allocation = eVCClearBalanceViewModel.InProgresAmount + eVCClearBalanceViewModel.DeliverdAmount;

                        }




                        //foreach (var items in TblWalletTransactions)
                        //{
                        //    if (items.OrderofAssignDate != null && items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate == null && items.OrderOfCompleteDate == null && items.StatusId != 26.ToString())
                        //    {
                        //        if (items.OrderAmount != null)
                        //        {
                        //            IsProgress += items.OrderAmount;
                        //        }
                        //    }
                        //    if (items.OrderofAssignDate != null && items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate != null && items.OrderOfCompleteDate == null && items.StatusId != 26.ToString())
                        //    {
                        //        if (items.OrderAmount != null)
                        //        {
                        //            IsDeleverd += items.OrderAmount;
                        //        }
                        //    }
                        //}
                        //RuningBlns = eVC_RegistrationPortalViewModel.EvcwalletAmount - (IsProgress + IsDeleverd);
                        //eVC_DashboardViewModel.Allocation = IsProgress + IsDeleverd;



                        //eVC_DashboardViewModel.clearBalance = RuningBlns;

                        eVC_DashboardViewModel.totalAssignOrder = TblWalletTransactions.Count(items => items.OrderofAssignDate != null && items.OrderOfInprogressDate == null && items.OrderOfDeliverdDate == null && items.OrderOfCompleteDate == null && items.StatusId != 26.ToString());

                        eVC_DashboardViewModel.totalAllocateOrder = TblWalletTransactions.Count(items => items.OrderofAssignDate != null && items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate == null && items.OrderOfCompleteDate == null && items.StatusId != 26.ToString());

                        eVC_DashboardViewModel.totalDeliveredOrder = TblWalletTransactions.Count(items => items.OrderofAssignDate != null && items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate != null && items.OrderOfCompleteDate == null && items.StatusId != 26.ToString());

                        eVC_DashboardViewModel.totalCompleteOrder = TblWalletTransactions.Count(items => items.OrderofAssignDate != null && items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate != null && items.OrderOfCompleteDate != null && items.StatusId != 26.ToString());

                        eVC_DashboardViewModel.evcAllocation = eVC_DashboardViewModel.totalAssignOrder + eVC_DashboardViewModel.totalAllocateOrder + eVC_DashboardViewModel.totalDeliveredOrder + eVC_DashboardViewModel.totalCompleteOrder;



                    }


                    eVC_DashboardViewModel.evcRegistrationId = eVC_RegistrationPortalViewModel.EvcregistrationId;
                    return eVC_DashboardViewModel;
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "EvcByUserId", ex);
            }
            return eVC_DashboardViewModel;
        }
        #endregion

        #region  ADDED BY ASHWIN  _ THIS METHOD IS USED IN TO SHOW LATTEST ALLOCATION TO EVC USER ON EVC PORTAL DASHBOARD TO SHOW DASHBOARD DETAILS
        /// <summary>
        /// ADDED BY ASHWIN  _ THIS METHOD IS USED IN TO SHOW LATTEST ALLOCATION TO EVC USER ON EVC PORTAL DASHBOARD TO SHOW DASHBOARD DETAILS
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<lattestAllocationViewModel> GetListOFLattestAllocation(int id, bool getassignorder)
        {
            List<lattestAllocationViewModel> lattestAllocationViewModel = new List<lattestAllocationViewModel>();
            TblEvcregistration TblEvcregistration = null;
            EVC_RegistrationPortalViewModel eVC_RegistrationPortalViewModel = null;
            List<TblWalletTransaction> TblWalletTransactions = null;
            try
            {
                //TblEvcregistration = _evcRepository.GetSingle(where: x => x.IsActive == true && x.UserId == id && x.Isevcapprovrd == true);
                TblEvcregistration = _evcRepository.GetSingle(where: x => x.IsActive == true && x.UserId == id);
                if (TblEvcregistration != null)
                {
                    eVC_RegistrationPortalViewModel = _mapper.Map<TblEvcregistration, EVC_RegistrationPortalViewModel>(TblEvcregistration);
                    TblWalletTransactions = _context.TblWalletTransactions
.Where(x => x.IsActive == true && x.EvcregistrationId == TblEvcregistration.EvcregistrationId && x.OrderTrans != null && x.OrderTrans.OrderTransId > 0 && x.OrderofAssignDate != null && /*x.OrderOfInprogressDate != null &&  x.OrderOfDeliverdDate == null && x.OrderOfCompleteDate == null &&*/ x.StatusId != Convert.ToInt32(OrderStatusEnum.PickupDecline).ToString())
.Include(x => x.Evcregistration)
.Include(x => x.Evcpartner)
.Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
.Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
.Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation).ToList();

                }
                if (getassignorder == true)
                {
                    TblWalletTransactions = TblWalletTransactions.Where(x => x.OrderOfInprogressDate == null).ToList();
                }
                else
                {
                    TblWalletTransactions = TblWalletTransactions.Where(x => x.OrderOfInprogressDate != null).ToList();

                }
                if (TblWalletTransactions != null)
                {
                    foreach (var items in TblWalletTransactions)
                    {
                        lattestAllocationViewModel lattestAllocationViewModels = new lattestAllocationViewModel();

                        lattestAllocationViewModels.regdNo = items.RegdNo != null ? items.RegdNo : String.Empty;
                        lattestAllocationViewModels.storecode = items.Evcpartner?.EvcStoreCode != null ? items.Evcpartner?.EvcStoreCode : String.Empty;
                        lattestAllocationViewModels.orderAmount = items.OrderAmount != null && items.OrderAmount > 0 ? items.OrderAmount : 0;
                        lattestAllocationViewModels.OrdertransId = items.OrderTrans.OrderTransId > 0 ? items.OrderTrans.OrderTransId : 0;
                        if (items.OrderTrans.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                        {
                            lattestAllocationViewModels.productTypeName = items.OrderTrans?.Exchange?.ProductType != null ? items.OrderTrans.Exchange.ProductType.Description : string.Empty;
                            lattestAllocationViewModels.productCategoryName = items.OrderTrans?.Exchange?.ProductType?.ProductCat != null ? items.OrderTrans.Exchange.ProductType.ProductCat.Description : string.Empty; ;
                        }
                        else
                        {
                            lattestAllocationViewModels.productTypeName = items.OrderTrans?.Abbredemption?.Abbregistration?.NewProductCategory?.DescriptionForAbb;
                            lattestAllocationViewModels.productCategoryName = items.OrderTrans?.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation?.DescriptionForAbb;
                        }
                        if (items.OrderofAssignDate != null)
                        {
                            lattestAllocationViewModels.AssignDate = items.OrderofAssignDate != null ? items.OrderofAssignDate : DateTime.MinValue;
                            var finaldate = (DateTime)items.OrderofAssignDate;
                            lattestAllocationViewModels.FinalDate = finaldate.Date.ToShortDateString();
                            if (items.OrderOfCompleteDate != null)
                            {
                                var completeDate = (DateTime)items.OrderOfCompleteDate;
                                lattestAllocationViewModels.dateComplete = completeDate.Date.ToShortDateString();
                            }

                            if (items.OrderOfInprogressDate != null)
                            {
                                var inProgressDate = (DateTime)items.OrderOfInprogressDate;
                                lattestAllocationViewModels.dateInProg = inProgressDate.Date.ToShortDateString();
                            }
                            //lattestAllocationViewModels.dateInProg = items.OrderOfInprogressDate.ToString();
                            //lattestAllocationViewModels.dateComplete = items.OrderOfCompleteDate.ToString();

                        }
                        lattestAllocationViewModel.Add(lattestAllocationViewModels);
                    }
                }
                lattestAllocationViewModel = lattestAllocationViewModel.GetRange(0, 5);
                lattestAllocationViewModel = lattestAllocationViewModel.OrderByDescending(o => o.FinalDate).ToList();

                return lattestAllocationViewModel;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "GetListOFLattestAllocation", ex);
            }

            return lattestAllocationViewModel;
        }
        #endregion

        #region ADDED BY ASHWIN  _ THIS METHOD IS USED IN TO EVC USERPROFILE TO SHOW LOGIN PROFILE DETAILS OF EVC
        /// <summary>
        /// ADDED BY ASHWIN  _ THIS METHOD IS USED IN TO EVC USERPROFILE TO SHOW LOGIN PROFILE DETAILS OF EVC
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EVC_RegistrationModel GetEvcByUserId(int id)
        {
            EVC_RegistrationModel? eVC_RegistrationModels = null;
            TblEvcregistration? TblEvcregistration = null;


            try
            {
                TblEvcregistration = _evcRepository.GetSingle(where: x => x.IsActive == true && x.UserId == id);
                if (TblEvcregistration != null)
                {
                    eVC_RegistrationModels = _mapper.Map<TblEvcregistration, EVC_RegistrationModel>(TblEvcregistration);

                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "GetEvcByUserId", ex);
            }
            return eVC_RegistrationModels;
        }
        #endregion

        #region ADDED By Ashwin---EvcUserWalletAdditionHistory
        public List<MyWalletSummaryAdditionViewModel> EvcUserWalletAdditionHistory(int id)
        {
            List<MyWalletSummaryAdditionViewModel> myWalletSummaryAddition = new List<MyWalletSummaryAdditionViewModel>();
            TblEvcregistration TblEvcregistration = null;
            List<TblEvcwalletAddition> tblEvcwalletAddition = null;
            TblUserRole tblUserRole = null;
            TblRole tblRole = null;
            try
            {

                TblEvcregistration = _evcRepository.GetSingle(where: x => x.IsActive == true && x.UserId == id);
                if (TblEvcregistration != null)
                {
                    tblEvcwalletAddition = _context.TblEvcwalletAdditions.Where(x => x.EvcregistrationId == TblEvcregistration.EvcregistrationId && x.IsActive == true).ToList();

                }
                if (tblEvcwalletAddition != null)
                {

                    //int count = 0;
                    foreach (var items in tblEvcwalletAddition)
                    {
                        //count++;
                        MyWalletSummaryAdditionViewModel myWalletSummaryAddition1 = new MyWalletSummaryAdditionViewModel();
                        lattestAllocationViewModel lattestAllocationViewModels = new lattestAllocationViewModel();

                        tblUserRole = _userRoleRepository.GetSingle(where: x => x.UserId == TblEvcregistration.UserId);
                        tblRole = _roleRepository.GetSingle(where: x => x.RoleId == tblUserRole.RoleId);
                        if (items.CreatedDate != null && items.Amount > 0)
                        {
                            myWalletSummaryAddition1.evcRegistrationId = items.EvcregistrationId;
                            myWalletSummaryAddition1.Amount = items.Amount;
                            myWalletSummaryAddition1.AddedBy = tblRole.RoleName;
                            myWalletSummaryAddition1.CreatedDate = items.CreatedDate;
                            if (items.IsCreaditNote == true)
                            {
                                myWalletSummaryAddition1.type = "Credit Recharge";

                            }
                            else {
                                myWalletSummaryAddition1.type = "Wallet Recharge";
                            }
                            var finaldate = (DateTime)myWalletSummaryAddition1.CreatedDate;
                            myWalletSummaryAddition1.FinalDate = finaldate.Date.ToShortDateString();

                            //myWalletSummaryAddition1.CreatedDate = items.CreatedDate;

                        }
                        myWalletSummaryAddition.Add(myWalletSummaryAddition1);
                        //if (count <= 5)
                        //{
                        //    myWalletSummaryAddition.Add(myWalletSummaryAddition1);
                        //}
                        //else
                        //    break;

                    }

                }
                myWalletSummaryAddition = myWalletSummaryAddition.OrderByDescending(O => O.FinalDate).ToList().GetRange(0, 5);
                //return new ExecutionResult(new InfoMessage(true, "Success", eVC_RegistrationPortalViewModel));
                //myWalletSummaryAddition = myWalletSummaryAddition.Take(5);
                return myWalletSummaryAddition;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "EvcUserWalletAdditionHistory", ex);
            }
            return myWalletSummaryAddition;
        }
        #endregion

        #region Added by Kranti _Get EVC Details by EvcregistrationId use in  WebApi 
        /// <summary>
        /// Added by Kranti _Get EVC Details by EvcregistrationId use in  WebApi
        /// </summary>
        /// <param name="id"></param>
        /// <returns> TblEvcregistration -eVC_RegistrationModels</returns>
        public ExecutionResult EvcByEvcregistrationId(int id)
        {
            EVC_RegistrationModel eVC_RegistrationModels = null;
            TblEvcregistration TblEvcregistration = null;

            try
            {
                TblEvcregistration = _evcRepository.GetSingle(where: x => x.IsActive == true && x.EvcregistrationId == id);
                if (TblEvcregistration != null)
                {
                    eVC_RegistrationModels = _mapper.Map<TblEvcregistration, EVC_RegistrationModel>(TblEvcregistration);
                    return new ExecutionResult(new InfoMessage(true, "Success", eVC_RegistrationModels));
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "GetEvcByEvcregistrationId", ex);
            }
            return new ExecutionResult(new InfoMessage(true, "No data found"));
        }
        #endregion

        #region Added by Kranti _ Get All EVC Wallet Summary List  use in WebApi 
        /// <summary>
        /// Added by Kranti _ Get All EVC Wallet Summary List 
        /// </summary>
        /// <returns>allWalletSummaryViewModels</returns>
        public ExecutionResult GetAllWalletSummery()
        {
            List<TblEvcregistration> TblEvcregistrations = null;
            List<TblWalletTransaction> TblWalletTransactions = null;
            TblEvcregistrations = new List<TblEvcregistration>();
            // TblUseRole TblUseRole = null;
            try
            {
                TblEvcregistrations = _evcRepository.GetList(x => x.IsActive == true).ToList();
                List<AllWalletSummaryViewModel> allWalletSummaryViewModels = _mapper.Map<List<TblEvcregistration>, List<AllWalletSummaryViewModel>>(TblEvcregistrations);
                foreach (AllWalletSummaryViewModel item in allWalletSummaryViewModels)
                {
                    TblEvcregistration EVC_Reg = TblEvcregistrations.FirstOrDefault(x => x.EvcregistrationId == item.EvcregistrationId);

                    if (TblEvcregistrations != null && TblEvcregistrations.Count > 0)
                    {
                        EVCClearBalanceViewModel? eVCClearBalanceViewModel = new EVCClearBalanceViewModel();
                        eVCClearBalanceViewModel = _commonManager.CalculateEVCClearBalance(item.EvcregistrationId);
                        if (eVCClearBalanceViewModel != null)
                        {
                            item.TotalofInprogress = eVCClearBalanceViewModel.InProgresAmount > 0 ? eVCClearBalanceViewModel.InProgresAmount : 0;
                            item.TotalofDeliverd = eVCClearBalanceViewModel.DeliverdAmount > 0 ? eVCClearBalanceViewModel.DeliverdAmount : 0;
                            item.RuningBalance = eVCClearBalanceViewModel.clearBalance ;
                        }
                        item.EvcName = item.EvcregdNo + '-' + item.BussinessName;


                        //TblWalletTransactions = _context.TblWalletTransactions.Where(x => x.EvcregistrationId == EVC_Reg.EvcregistrationId && x.IsActive == true && x.StatusId != 26.ToString()).ToList();

                        //if (TblWalletTransactions != null)
                        //{
                        //    TblWalletTransaction tblWalletTransaction = new TblWalletTransaction();
                        //    AllWalletSummaryViewModel allWalletSummaryViewModel = new AllWalletSummaryViewModel();
                        //    foreach (var items in TblWalletTransactions)
                        //    {
                        //        if (items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate == null && items.OrderOfCompleteDate == null && items.StatusId != 26.ToString())
                        //        {
                        //            allWalletSummaryViewModel.TotalofInprogress += items.OrderAmount;
                        //        }
                        //        if (items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate != null && items.OrderOfCompleteDate == null && items.StatusId != 26.ToString())
                        //        {
                        //            allWalletSummaryViewModel.TotalofDeliverd += items.OrderAmount;
                        //        }
                        //    }
                        //    item.TotalofInprogress = allWalletSummaryViewModel.TotalofInprogress == null ? 0 : allWalletSummaryViewModel.TotalofInprogress;
                        //    item.TotalofDeliverd = allWalletSummaryViewModel.TotalofDeliverd == null ? 0 : allWalletSummaryViewModel.TotalofDeliverd;
                        //    item.RuningBalance = item.EvcwalletAmount - (item.TotalofInprogress + item.TotalofDeliverd);

                        //}


                    }
                    if (EVC_Reg != null && EVC_Reg.EmployeeId > 0)
                    {
                        TblUser tblUser = _context.TblUsers.FirstOrDefault(x => x.UserId == EVC_Reg.EmployeeId);
                        item.EmployeeName = tblUser != null ? tblUser.FirstName : string.Empty;
                        var data = allWalletSummaryViewModels;
                        return new ExecutionResult(new InfoMessage(true, "Success", allWalletSummaryViewModels));
                    }
                }
            }


            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "GetAllUser", ex);
            }
            return new ExecutionResult(new InfoMessage(true, "No data found"));

        }
        #endregion

        #region Added By Kranti _Get One EVC Wallet summary use in Api 
        /// <summary>
        /// Added By Kranti _Get One EVC Wallet summary use in Api
        /// </summary>
        /// <param name="id"></param>
        /// <returns>allWalletSummaryViewModels</returns>
        public ExecutionResult GetWalletSummeryByEVC(int id)
        {
            TblEvcregistration TblEvcregistrations = null;
            TblWalletTransaction TblWalletTransactions = null;
            TblEvcregistrations = new TblEvcregistration();
            // TblUseRole TblUseRole = null;
            try
            {
                TblEvcregistrations = _evcRepository.GetSingle(where: x => x.IsActive == true && x.EvcregistrationId == id);
                AllWalletSummaryViewModel allWalletSummaryViewModels = _mapper.Map<TblEvcregistration, AllWalletSummaryViewModel>(TblEvcregistrations);

                TblEvcregistration EVC_Reg = _evcRepository.GetSingle(x => x.EvcregistrationId == id);

                if (TblEvcregistrations != null)
                {
                    TblWalletTransactions = _walletTransactionRepository.GetSingle(where: x => x.EvcregistrationId == EVC_Reg.EvcregistrationId);

                    if (TblWalletTransactions != null)
                    {
                        TblWalletTransaction tblWalletTransaction = new TblWalletTransaction();
                        AllWalletSummaryViewModel allWalletSummaryViewModel = new AllWalletSummaryViewModel();

                        if (tblWalletTransaction.OrderOfInprogressDate != null && tblWalletTransaction.OrderOfDeliverdDate == null && tblWalletTransaction.OrderOfCompleteDate == null && tblWalletTransaction.StatusId != 26.ToString())
                        {
                            allWalletSummaryViewModel.TotalofInprogress += tblWalletTransaction.OrderAmount;
                        }
                        if (tblWalletTransaction.OrderOfInprogressDate != null && tblWalletTransaction.OrderOfDeliverdDate != null && tblWalletTransaction.OrderOfCompleteDate == null && tblWalletTransaction.StatusId != 26.ToString())
                        {
                            allWalletSummaryViewModel.TotalofDeliverd += tblWalletTransaction.OrderAmount;
                        }
                    }
                    allWalletSummaryViewModels.TotalofInprogress = allWalletSummaryViewModels.TotalofInprogress == null ? null : allWalletSummaryViewModels.TotalofInprogress;
                    allWalletSummaryViewModels.TotalofDeliverd = allWalletSummaryViewModels.TotalofDeliverd == null ? null : allWalletSummaryViewModels.TotalofDeliverd;
                    allWalletSummaryViewModels.RuningBalance = allWalletSummaryViewModels.EvcwalletAmount - (allWalletSummaryViewModels.TotalofInprogress + allWalletSummaryViewModels.TotalofDeliverd);
                    allWalletSummaryViewModels.EvcName = allWalletSummaryViewModels.EvcregdNo + '-' + allWalletSummaryViewModels.BussinessName;
                }
                if (EVC_Reg != null && EVC_Reg.EmployeeId > 0)
                {
                    TblUser tblUser = _context.TblUsers.FirstOrDefault(x => x.UserId == EVC_Reg.EmployeeId);
                    allWalletSummaryViewModels.EmployeeName = tblUser != null ? tblUser.FirstName : string.Empty;
                    var data = allWalletSummaryViewModels;
                    return new ExecutionResult(new InfoMessage(true, "Success", allWalletSummaryViewModels));
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "GetAllUser", ex);
            }
            return new ExecutionResult(new InfoMessage(true, "No data found"));
        }
        #endregion

        #region Added By Kranti_ its wrong method ---Priyanshi  
        public ExecutionResult GetApprovedByEVC(int id)
        {
            TblEvcregistration TblEvcregistrations = null;
            try
            {
                TblEvcregistrations = _evcRepository.GetSingle(where: x => x.IsActive == true && x.EvcregistrationId == id && x.Isevcapprovrd == true);
                if (TblEvcregistrations != null)
                {
                    EVC_ApprovedViewModel EVC_ApporvedViewList = _mapper.Map<TblEvcregistration, EVC_ApprovedViewModel>(TblEvcregistrations);
                    return new ExecutionResult(new InfoMessage(true, "Approved", EVC_ApporvedViewList));
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "GetApprovedByEVC", ex);
            }
            return new ExecutionResult(new InfoMessage(true, "NotApproved", "No data found"));
        }
        #endregion

        #region Added By Kranti 
        public ExecutionResult GetEVCAllocation(int id)
        {
            TblEvcregistration TblEvcregistrations = null;
            TblWalletTransaction TblWalletTransactions = null;
            try
            {
                TblEvcregistrations = _evcRepository.GetSingle(where: x => x.IsActive == true && x.EvcregistrationId == id);
                AllWalletSummaryViewModel allWalletSummaryViewModels = _mapper.Map<TblEvcregistration, AllWalletSummaryViewModel>(TblEvcregistrations);


                TblEvcregistration EVC_Reg = _evcRepository.GetSingle(x => x.EvcregistrationId == id);

                if (TblEvcregistrations != null)
                {
                    TblWalletTransactions = _walletTransactionRepository.GetSingle(where: x => x.EvcregistrationId == EVC_Reg.EvcregistrationId);

                    if (TblWalletTransactions != null)
                    {

                        if (TblWalletTransactions.OrderOfInprogressDate != null && TblWalletTransactions.OrderOfDeliverdDate == null && TblWalletTransactions.OrderOfCompleteDate == null && TblWalletTransactions.StatusId != 26.ToString())
                        {
                            return new ExecutionResult(new InfoMessage(true, "InProgress", allWalletSummaryViewModels.TotalofInprogress));
                        }

                        if (TblWalletTransactions.OrderOfInprogressDate != null && TblWalletTransactions.OrderOfDeliverdDate != null && TblWalletTransactions.OrderOfCompleteDate == null && TblWalletTransactions.StatusId != 26.ToString())
                        {
                            return new ExecutionResult(new InfoMessage(true, "Delivered", allWalletSummaryViewModels.TotalofDeliverd));
                        }

                        if (TblWalletTransactions.OrderOfInprogressDate != null && TblWalletTransactions.OrderOfDeliverdDate != null && TblWalletTransactions.OrderOfCompleteDate != null && TblWalletTransactions.StatusId != 26.ToString())
                        {
                            return new ExecutionResult(new InfoMessage(true, "Completed", allWalletSummaryViewModels.TotalofCompleted));
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "GetApprovedByEVC", ex);
            }
            return new ExecutionResult(new InfoMessage(true, "Success", "No data found"));

        }
        #endregion

        #region Added BY Vishal Khare ---EVC Order Details Page --Get All Images --use for EVC Portal
        /// <summary>
        ///Added BY Vishal Khare ---EVC Order Details Page --Get All Images --use for EVC Portal
        /// </summary>
        /// <param name="orderTransId"></param>
        /// <returns>GetAllImagesByTransId</returns>
        public IList<OrderImageUploadViewModel> GetAllImagesByTransId(int orderTransId)
        {
            List<OrderImageUploadViewModel> orderImageUploadViewModels = null;
            List<TblOrderImageUpload> tblOrderImageUpload = new List<TblOrderImageUpload>();
            try
            {
                TblOrderTran tblOrderTran = _OrderTransRepository.GetSingle(x => x.IsActive == true && x.OrderTransId.Equals(orderTransId));
                if (tblOrderTran != null)
                {
                    tblOrderImageUpload = _orderImageUploadRepository.GetList(x => x.IsActive == true && x.OrderTransId == orderTransId).ToList();
                    orderImageUploadViewModels = _mapper.Map<List<TblOrderImageUpload>, List<OrderImageUploadViewModel>>(tblOrderImageUpload);
                    return orderImageUploadViewModels;
                }
                return orderImageUploadViewModels;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "GetAllImagesByTransId", ex);
            }
            return orderImageUploadViewModels;
        }
        #endregion

        #region ADDED BY Ashwin _Save/Edit EvcRegistation  
        /// <summary>
        /// Used in api of EVC Registeration in mobile development
        /// </summary>
        /// <param name="evc_RegistrationModel"></param>
        /// <param name="userId"></param>
        /// <returns>TblEvcregistration.EvcregistrationId</returns>
        public ResponseResult RegisterEVC(EVC_RegistrationModel evc_RegistrationModel)
        {
            TblEvcregistration TblEvcregistration = new TblEvcregistration();
            bool response = false;
            int userId = 3;
            bool cencelCheck = false;
            ResponseResult responseMessage = new ResponseResult();
            try
            {

                if (evc_RegistrationModel.CopyofCancelledChequeLinkURLBase64string != null)
                {
                    string fileName = Guid.NewGuid().ToString("N") + "CancelledCheck" + ".jpg";
                    string filePath = @"\DBFiles\EVC\CancelledCheque";
                    cencelCheck = _imageHelper.SaveFileFromBase64(evc_RegistrationModel.CopyofCancelledChequeLinkURLBase64string, filePath, fileName);
                    if (cencelCheck)
                    {
                        evc_RegistrationModel.CopyofCancelledCheque = fileName;
                        cencelCheck = false;
                    }

                }

                if (evc_RegistrationModel.UploadGSTRegistrationLinkURLBase64string != null)
                {
                    string fileName = Guid.NewGuid().ToString("N") + "GSTRegistration" + ".jpg";
                    string filePath = @"\DBFiles\EVC\GSTRegistration";
                    cencelCheck = _imageHelper.SaveFileFromBase64(evc_RegistrationModel.UploadGSTRegistrationLinkURLBase64string, filePath, fileName);
                    if (cencelCheck)
                    {
                        evc_RegistrationModel.UploadGSTRegistration = fileName;
                        cencelCheck = false;
                    }

                }
                if (evc_RegistrationModel.EWasteCertificateLinkURLBase64string != null)
                {
                    string fileName = Guid.NewGuid().ToString("N") + "EWasteCertificate" + ".jpg";
                    string filePath = @"\DBFiles\EVC\EWasteCertificate";
                    cencelCheck = _imageHelper.SaveFileFromBase64(evc_RegistrationModel.EWasteCertificateLinkURLBase64string, filePath, fileName);
                    if (cencelCheck)
                    {
                        evc_RegistrationModel.EWasteCertificate = fileName;
                        cencelCheck = false;
                    }

                }
                if (evc_RegistrationModel.AadharfrontImageLinkURLBase64string != null)
                {
                    string fileName = Guid.NewGuid().ToString("N") + "Aadharfront" + ".jpg";
                    string filePath = @"\DBFiles\EVC\AadharfrontImage";
                    cencelCheck = _imageHelper.SaveFileFromBase64(evc_RegistrationModel.AadharfrontImageLinkURLBase64string, filePath, fileName);
                    if (cencelCheck)
                    {
                        evc_RegistrationModel.AadharfrontImage = fileName;
                        cencelCheck = false;
                    }

                }
                if (evc_RegistrationModel.AadharBackImageLinkURLBase64string != null)
                {
                    string fileName = Guid.NewGuid().ToString("N") + "CancelledCheck" + ".jpg";
                    string filePath = @"\DBFiles\EVC\AadharBackImage";
                    cencelCheck = _imageHelper.SaveFileFromBase64(evc_RegistrationModel.AadharBackImageLinkURLBase64string, filePath, fileName);
                    if (cencelCheck)
                    {
                        evc_RegistrationModel.AadharBackImage = fileName;
                        cencelCheck = false;
                    }

                }
                if (evc_RegistrationModel.ProfilePicLinkURLBase64string != null)
                {
                    string fileName = Guid.NewGuid().ToString("N") + "CancelledCheck" + ".jpg";
                    string filePath = @"\DBFiles\EVC\EVCProfilePic";
                    cencelCheck = _imageHelper.SaveFileFromBase64(evc_RegistrationModel.ProfilePicLinkURLBase64string, filePath, fileName);
                    if (cencelCheck)
                    {
                        evc_RegistrationModel.ProfilePic = fileName;
                        cencelCheck = false;
                    }

                }

                if (evc_RegistrationModel != null)
                {

                    TblEvcregistration = _mapper.Map<EVC_RegistrationModel, TblEvcregistration>(evc_RegistrationModel);

                    var Check = _evcRepository.GetSingle(x => x.EmailId == evc_RegistrationModel.EmailId);
                    if (Check == null)
                    {
                        //Code to Insert the object 
                        TblEvcregistration.EvcwalletAmount = 0;
                        TblEvcregistration.EvcapprovalStatusId = userId;
                        TblEvcregistration.Isevcapprovrd = false;
                        TblEvcregistration.IsActive = true;
                        TblEvcregistration.CreatedDate = _currentDatetime;
                        TblEvcregistration.CreatedBy = userId;
                        TblEvcregistration.Date = _currentDatetime;
                        TblEvcregistration.IconfirmTermsCondition = true;
                        if (TblEvcregistration.EmployeeId == null)
                        {
                            TblEvcregistration.EmployeeId = userId;
                        }
                        var getcitycode = _cityRepository.GetSingle(x => x.CityId == evc_RegistrationModel.cityId);
                        TblEvcregistration.EvcregdNo = getcitycode.CityCode + UniqueString.RandomNumber();
                        TblEvcregistration.EvczohoBookName = TblEvcregistration.EvcregdNo + "-" + evc_RegistrationModel.BussinessName;
                        _evcRepository.Create(TblEvcregistration);
                        int result = _evcRepository.SaveChanges();
                        if (result == 1)
                        {
                            response = true;
                            responseMessage.message = "Regisiteration Success";
                            responseMessage.Status = true;
                        }
                        else
                        {
                            responseMessage.message = "Regisiteration failed";
                            responseMessage.Status = false;
                        }

                    }
                    else
                    {
                        responseMessage.message = "EmailID is already Register";
                        responseMessage.Status = false;
                    }

                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                _logging.WriteErrorToDB("EVCManager", "RegisterEVC", ex);
            }
            return responseMessage;
        }
        #endregion

        #region Payment update in EVC
        public string EVCPaymentstatusUpdate(PaymentResponseModel paymentReponse)
        {

            TblPaymentLeaser planPaymentObj = new TblPaymentLeaser();

            string response = string.Empty;
            string ZohoPushFlag = string.Empty;
            string SuccessResponse = null;
            string OrderStatus = null;
            try
            {
                if (paymentReponse != null)
                {
                    TblEvcregistration tblEVCRegistration = _evcRepository.GetSingle(x => x.EvcregdNo == paymentReponse.RegdNo);
                    if (tblEVCRegistration != null)
                    {
                        SuccessResponse = EnumHelper.DescriptionAttr(PluralEnum.PaymentStatus);

                        OrderStatus = EnumHelper.DescriptionAttr(PluralEnum.OrderStatus);

                        if (paymentReponse.status == SuccessResponse && paymentReponse.orderStatus == OrderStatus)
                        {
                            #region table tblEVCWalletAddition and tblEVCWalletHistory Insert and Update TblEvcRegistration
                            TblEvcwalletAddition tblEVCWalletAddition = new TblEvcwalletAddition();
                            tblEVCWalletAddition.EvcregistrationId = tblEVCRegistration.EvcregistrationId;
                            tblEVCWalletAddition.Amount = paymentReponse.amount;
                            tblEVCWalletAddition.TransactionId = paymentReponse.transactionId;
                            tblEVCWalletAddition.IsActive = true;
                            tblEVCWalletAddition.CreatedDate = DateTime.Now; ;
                            tblEVCWalletAddition.CreatedBy = tblEVCRegistration.UserId;
                            tblEVCWalletAddition.IsCreaditNote = false;
                            _EVCWalletAdditionRepository.Create(tblEVCWalletAddition);
                            _EVCWalletAdditionRepository.SaveChanges();

                            TblEvcwalletHistory tblEVCWalletHistory = new TblEvcwalletHistory();
                            tblEVCWalletHistory.EvcregistrationId = tblEVCRegistration.EvcregistrationId;
                            var cAmount = _evcRepository.GetSingle(X => X.EvcregistrationId == tblEVCWalletHistory.EvcregistrationId);
                            tblEVCWalletHistory.CurrentWalletAmount = cAmount.EvcwalletAmount > 0 || cAmount.EvcwalletAmount != null ? cAmount.EvcwalletAmount : 0;
                            tblEVCWalletHistory.AddAmount = tblEVCWalletAddition.Amount;
                            tblEVCWalletHistory.BalanceWalletAmount = tblEVCWalletHistory.CurrentWalletAmount + tblEVCWalletHistory.AddAmount;
                            tblEVCWalletHistory.AmountAdditionFlag = true;
                            tblEVCWalletHistory.IsActive = true;
                            tblEVCWalletHistory.TransactionId = paymentReponse.transactionId;
                            tblEVCWalletHistory.CreatedDate = DateTime.Now; ;
                            tblEVCWalletHistory.CreatedBy = tblEVCRegistration.UserId;
                            _eVCWalletHistoryRepository.Create(tblEVCWalletHistory);
                            _eVCWalletHistoryRepository.SaveChanges();

                            tblEVCRegistration.EvcwalletAmount = tblEVCWalletHistory.BalanceWalletAmount;
                            tblEVCRegistration.ModifiedBy = tblEVCRegistration.UserId;
                            tblEVCRegistration.ModifiedDate = DateTime.Now;
                            _evcRepository.Update(tblEVCRegistration);
                            _evcRepository.SaveChanges();
                            #endregion

                        }
                        planPaymentObj.RegdNo = tblEVCRegistration.EvcregdNo;
                        planPaymentObj.OrderId = paymentReponse.OrderId;
                        planPaymentObj.PaymentDate = DateTime.Now;
                        planPaymentObj.IsActive = true;
                        planPaymentObj.TransactionId = paymentReponse.transactionId;
                        planPaymentObj.ResponseDescription = paymentReponse.responseDescription;
                        planPaymentObj.ResponseCode = paymentReponse.responseCode.ToString();
                        planPaymentObj.CardId = paymentReponse.cardId;
                        planPaymentObj.CardHashId = paymentReponse.cardhashId;
                        planPaymentObj.CardScheme = paymentReponse.cardScheme;
                        planPaymentObj.CardToken = paymentReponse.cardToken;
                        planPaymentObj.Bank = paymentReponse.bank;
                        planPaymentObj.BankId = paymentReponse.bankid;
                        planPaymentObj.Amount = paymentReponse.amount;
                        planPaymentObj.CheckSum = paymentReponse.checksum;
                        planPaymentObj.PaymentMode = paymentReponse.paymentMode;
                        string transactionType = "Cr";
                        planPaymentObj.TransactionType = transactionType;
                        string moduleType = "EVC";
                        planPaymentObj.ModuleType = moduleType;
                        planPaymentObj.ModuleReferenceId = tblEVCRegistration.EvcregistrationId;
                        planPaymentObj.CreatedBy = tblEVCRegistration.UserId;
                        planPaymentObj.CreatedDate = DateTime.Now;
                        planPaymentObj.GatewayTransactioId = paymentReponse.gatewayTransactionId;
                        planPaymentObj.OrderStatus = paymentReponse.orderStatus;

                        if (paymentReponse.status == SuccessResponse)
                        {
                            planPaymentObj.PaymentStatus = true;
                        }
                        else
                        {
                            planPaymentObj.PaymentStatus = false;
                        }

                        _paymentLeaser.Create(planPaymentObj);
                        _paymentLeaser.SaveChanges();

                        response = "success";
                    }
                    else
                    {
                        planPaymentObj.RegdNo = paymentReponse.RegdNo;
                        planPaymentObj.OrderId = paymentReponse.OrderId;
                        planPaymentObj.PaymentDate = DateTime.Now;
                        planPaymentObj.IsActive = true;
                        if (paymentReponse.responseCode == 100)
                        {
                            planPaymentObj.PaymentStatus = true;
                        }
                        else
                        {
                            planPaymentObj.PaymentStatus = false;
                        }
                        planPaymentObj.TransactionId = paymentReponse.transactionId;
                        planPaymentObj.ResponseDescription = paymentReponse.responseDescription;
                        planPaymentObj.ResponseCode = paymentReponse.responseCode.ToString();
                        planPaymentObj.CardId = paymentReponse.cardId;
                        planPaymentObj.CardHashId = paymentReponse.cardhashId;
                        planPaymentObj.CardScheme = paymentReponse.cardScheme;
                        planPaymentObj.CardToken = paymentReponse.cardToken;
                        planPaymentObj.Bank = paymentReponse.bank;
                        planPaymentObj.BankId = paymentReponse.bankid;
                        planPaymentObj.Amount = paymentReponse.amount;
                        planPaymentObj.CheckSum = paymentReponse.checksum;
                        planPaymentObj.PaymentMode = paymentReponse.paymentMode;
                        _paymentLeaser.Create(planPaymentObj);
                        _paymentLeaser.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ERPManager", "EVCPaymentstatusUpdate", ex);
            }
            return response;
        }
        #region to get String Value Of Enum
        public static string GetEnumDescription(RoleEnum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
        #endregion
        #endregion

        #region ADDED BY VK ---- this method use for EVCDispute ---Admin/Portal--- Product details by OrdertansID---
        /// <summary>
        /// this method use for EVCDispute ---Admin/Portal--- Product details by OrdertansID
        /// </summary>
        /// <param name="orderTransId"></param>
        /// <returns></returns>
        public EVCDisputeViewModel GetProductDetailsByTransId(int orderTransId)
        {
            EVCDisputeViewModel eVCDisputeView = new EVCDisputeViewModel();
            #region Common Implementations for (ABB or Exchange)
            TblExchangeOrder tblExchangeOrder = null;
            TblAbbredemption tblAbbredemption = null;
            TblAbbregistration tblAbbregistration = null;
            string productCatDesc = null; string productTypeDesc = null;
            TblOrderTran tblOrderTrans = null;
            #endregion
            try
            {
                if (orderTransId != 0 && orderTransId > 0)
                {
                    #region Common Implementations for (ABB or Exchange)
                    tblOrderTrans = _OrderTransRepository.GetOrderDetailsByOrderTransId(orderTransId);
                    if (tblOrderTrans != null)
                    {
                        if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                        {
                            tblExchangeOrder = tblOrderTrans.Exchange;
                            if (tblExchangeOrder != null)
                            {
                                productCatDesc = tblExchangeOrder.ProductType?.ProductCat?.Description;
                                productTypeDesc = tblExchangeOrder.ProductType?.Description;
                            }
                        }
                        else if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.ABB))
                        {
                            tblAbbredemption = tblOrderTrans.Abbredemption;
                            if (tblAbbredemption != null)
                            {
                                tblAbbregistration = tblAbbredemption.Abbregistration;
                                if (tblAbbregistration != null)
                                {
                                    productCatDesc = tblAbbregistration.NewProductCategory.DescriptionForAbb;
                                    productTypeDesc = tblAbbregistration.NewProductCategoryTypeNavigation?.DescriptionForAbb;
                                }
                            }
                        }
                    }
                    #endregion

                    eVCDisputeView.ProductTypeName = productTypeDesc;
                    eVCDisputeView.ProductCatName = productCatDesc;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "GetOrderDetailsByTransId", ex);
            }
            return eVCDisputeView;
        }
        #endregion

        #region Method to get the EVC by nearest stateid,cityid or pincode and having maximum balance -- Added by SK
        public List<Allocate_EVCFromViewModel> GetEvcForAutoAllocation(int? OrderTransId)
        {
            List<Allocate_EVCFromViewModel> Allocate_EVCFromViewModels = new List<Allocate_EVCFromViewModel>();
            try
            {
                Allocate_EVCFromViewModel allocate = new Allocate_EVCFromViewModel();
                TblOrderTran tblOrderTran = _OrderTransRepository.GetSingleOrderWithExchangereference(Convert.ToInt32(OrderTransId));
                if (tblOrderTran != null)
                {
                    allocate.ActualBasePrice = tblOrderTran.FinalPriceAfterQc > 0 ? tblOrderTran.FinalPriceAfterQc : 0;
                    if (tblOrderTran.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                    {
                        // Set properties for Exchange order type
                        allocate.ActualBasePrice = tblOrderTran.Exchange.FinalExchangePrice > 0 ? tblOrderTran.Exchange.FinalExchangePrice : 0;
                        allocate.Bonus = tblOrderTran.Exchange.Bonus;
                        allocate.RegdNo = tblOrderTran.Exchange.RegdNo;
                        allocate.orderTransId = tblOrderTran.OrderTransId;

                        if (tblOrderTran.Exchange.CustomerDetails != null)
                        {
                            allocate.FirstName = tblOrderTran.Exchange.CustomerDetails.FirstName;
                            allocate.CustCity = tblOrderTran.Exchange.CustomerDetails.City;
                            allocate.CustPin = tblOrderTran.Exchange.CustomerDetails.ZipCode;
                            allocate.Custstate = tblOrderTran.Exchange.CustomerDetails.State;
                        }

                        if (tblOrderTran.Exchange.ProductType != null)
                        {
                            allocate.OldProdType = !string.IsNullOrEmpty(tblOrderTran.Exchange.ProductType.Description) ? tblOrderTran.Exchange.ProductType.Description : string.Empty;

                            if (tblOrderTran.Exchange.ProductType.ProductCat != null)
                            {
                                allocate.ExchProdGroup = !string.IsNullOrEmpty(tblOrderTran.Exchange.ProductType.ProductCat.Description) ? tblOrderTran.Exchange.ProductType.ProductCat.Description : string.Empty;
                            }
                        }
                    }
                    else if (tblOrderTran.OrderType == Convert.ToInt32(OrderTypeEnum.ABB))
                    {
                        // Set properties for ABB order type
                        allocate.RegdNo = tblOrderTran.RegdNo;
                        allocate.orderTransId = tblOrderTran.OrderTransId;

                        if (tblOrderTran.Abbredemption?.Abbregistration != null)
                        {
                            allocate.FirstName = tblOrderTran.Abbredemption.Abbregistration.CustFirstName;
                            allocate.CustCity = tblOrderTran.Abbredemption.Abbregistration.CustCity;
                            allocate.CustPin = tblOrderTran.Abbredemption.Abbregistration.CustPinCode;
                            allocate.OldProdType = tblOrderTran.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description;

                        }
                        if (tblOrderTran.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation != null)
                        {
                            allocate.OldProdType = tblOrderTran.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description;

                            if (tblOrderTran.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.ProductCat != null)
                            {
                                allocate.ExchProdGroup = tblOrderTran.Abbredemption.Abbregistration.NewProductCategory.Description;
                            }
                        }
                    }

                    // Calculate the ExpectedPrice if ActualBasePrice is greater than 0 and not null
                    if (allocate.ActualBasePrice > 0 && allocate.ActualBasePrice != null)
                    {
                        var result = _commonManager.CalculateEVCPrice(tblOrderTran.OrderTransId);
                        if (result > 0)
                        {
                            allocate.ExpectedPrice = result;
                        }
                        else
                        {
                            // Handle error or display a message
                        }
                    }

                    if (!string.IsNullOrEmpty(allocate.Custstate) || !string.IsNullOrEmpty(allocate.CustCity) || !string.IsNullOrEmpty(allocate.CustPin))
                    {
                        var EVcLists1 = GetEVCListbycityAndpin(allocate.Custstate, allocate.CustCity, allocate.CustPin);
                        if (EVcLists1 != null && EVcLists1.Count > 0)
                        {
                            foreach (var evc in EVcLists1)
                            {
                                List<TblWalletTransaction> TblWalletTransactions = _context.TblWalletTransactions
                                    .Where(x => x.EvcregistrationId == evc.EvcregistrationId && x.IsActive == true && x.StatusId != "26")
                                    .ToList();

                                decimal? IsProgress = 0;
                                decimal? IsDeleverd = 0;
                                decimal? RunningBlns = 0;

                                if (TblWalletTransactions != null)
                                {
                                    foreach (var walletTransaction in TblWalletTransactions)
                                    {
                                        // Calculate IsProgress, IsDeleverd, and RunningBlns based on walletTransaction properties
                                        if (walletTransaction.OrderOfInprogressDate != null && walletTransaction.OrderOfDeliverdDate == null && walletTransaction.OrderOfCompleteDate == null && walletTransaction.StatusId != "26")
                                        {
                                            if (walletTransaction.OrderAmount != null)
                                            {
                                                IsProgress += walletTransaction.OrderAmount;
                                            }
                                        }
                                        if (walletTransaction.OrderOfInprogressDate != null && walletTransaction.OrderOfDeliverdDate != null && walletTransaction.OrderOfCompleteDate == null && walletTransaction.StatusId != "26")
                                        {
                                            if (walletTransaction.OrderAmount != null)
                                            {
                                                IsDeleverd += walletTransaction.OrderAmount;
                                            }
                                        }
                                    }

                                    RunningBlns = evc.EvcwalletAmount - (IsProgress + IsDeleverd);
                                    if (RunningBlns > 0 && RunningBlns > allocate.ExpectedPrice)
                                    {
                                        allocate.SelectEVCId = evc.EvcregistrationId;
                                        break;
                                    }
                                }
                            }
                            Allocate_EVCFromViewModels.Add(allocate);
                        }
                    }
                }
                return Allocate_EVCFromViewModels;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "GetEvcForAutoAllocation", ex);
                // Handle exception or return an empty list
            }
            return Allocate_EVCFromViewModels;
        }
        #endregion

        #region Method to get the in-house evc list
        /// <summary>
        /// Method to get the in-house evc list
        /// </summary>
        /// <param name="custstate"></param>
        /// <param name="custCity"></param>
        /// <param name="custPin"></param>
        /// <returns></returns>
        public List<EVC_PartnerViewModel> GetListOfInHouseEvc(Allocate_EVCFromViewModel allocate)
        {
            List<EVC_PartnerViewModel> eVCPartnerViewModels = new List<EVC_PartnerViewModel>();
            List<TblEvcPartner> tblEvcPartnerList = new List<TblEvcPartner>();
            try
            {
                if (!string.IsNullOrEmpty(allocate.CustPin))
                {
                    tblEvcPartnerList = _eVCPartnerRepository.GetEvcPartnerListByPincode(allocate.CustPin, allocate.ProductCatId, allocate.ActualProdQltyAtQc, allocate.Ordertype);
                    if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
                    {
                        //tblEvcPartnerList = _eVCPartnerRepository.GetEvcPartnerListHavingClearBalance(tblEvcPartnerList, allocate.ExpectedPrice);
                        tblEvcPartnerList = _commonManager.GetEVCPartnerListHavingClearBalance(allocate.orderTransId, tblEvcPartnerList);
                        if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
                        {
                            tblEvcPartnerList = _eVCPartnerRepository.GetEvcPartnerListHavingOldRecharge(tblEvcPartnerList);
                            if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
                            {
                                eVCPartnerViewModels = _mapper.Map<List<TblEvcPartner>, List<EVC_PartnerViewModel>>(tblEvcPartnerList);
                                foreach (var item in eVCPartnerViewModels)
                                {
                                    item.IsSweetenerAmtInclude = tblEvcPartnerList.Where(x => x.EvcPartnerId == item.EvcPartnerId)?.FirstOrDefault()?.Evcregistration?.IsSweetenerAmtInclude;
                                    item.GSTTypeId = tblEvcPartnerList.Where(x => x.EvcPartnerId == item.EvcPartnerId)?.FirstOrDefault()?.Evcregistration?.GsttypeId;
                                }
                                return eVCPartnerViewModels;
                            }
                        }
                    }
                }
                else
                {
                    return eVCPartnerViewModels;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "GetListOfInHouseEvc", ex);
            }
            return eVCPartnerViewModels;
        }
        #endregion

        #region Method to get the other than in-house evc partner list
        /// <summary>
        /// Method to get the in-house evc list
        /// </summary>
        /// <param name="custstate"></param>
        /// <param name="custCity"></param>
        /// <param name="custPin"></param>
        /// <returns></returns>
        public List<EVC_PartnerViewModel> GetListOfEvcOtherThanInHouse(Allocate_EVCFromViewModel allocate)
        {
            List<EVC_PartnerViewModel> eVCPartnerViewModels = new List<EVC_PartnerViewModel>();
            List<TblEvcPartner> tblEvcPartnerList = new List<TblEvcPartner>();
            try
            {
                if (!string.IsNullOrEmpty(allocate.CustPin))
                {
                    tblEvcPartnerList = _eVCPartnerRepository.GetNonInHouseEvcPartnerListByPincode(allocate.CustPin, allocate.ProductCatId, allocate.ActualProdQltyAtQc, allocate.Ordertype);
                    if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
                    {
                        //tblEvcPartnerList = _eVCPartnerRepository.GetEvcPartnerListHavingClearBalance(tblEvcPartnerList, allocate.ExpectedPrice);
                        tblEvcPartnerList = _commonManager.GetEVCPartnerListHavingClearBalance(allocate.orderTransId, tblEvcPartnerList);
                        if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
                        {
                            tblEvcPartnerList = _eVCPartnerRepository.GetEvcPartnerListHavingOldRecharge(tblEvcPartnerList);
                            if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
                            {
                                eVCPartnerViewModels = _mapper.Map<List<TblEvcPartner>, List<EVC_PartnerViewModel>>(tblEvcPartnerList);
                                foreach (var item in eVCPartnerViewModels)
                                {
                                    item.IsSweetenerAmtInclude = tblEvcPartnerList.Where(x => x.EvcPartnerId == item.EvcPartnerId)?.FirstOrDefault()?.Evcregistration?.IsSweetenerAmtInclude;
                                    item.GSTTypeId = tblEvcPartnerList.Where(x => x.EvcPartnerId == item.EvcPartnerId)?.FirstOrDefault()?.Evcregistration?.GsttypeId;
                                }
                                return eVCPartnerViewModels;
                            }
                        }
                    }
                }
                else
                {
                    return eVCPartnerViewModels;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "GetListOfEvcOtherThanInHouse", ex);
            }
            return eVCPartnerViewModels;
        }
        #endregion

        #region Method to assign evc to order and maintain all details in DB
        /// <summary>
        /// Method to assign evc to order and maintain all details in DB
        /// </summary>
        /// <param name="eVCPartnerViewModels"></param>
        /// <returns>flag</returns>
        public bool AssignEVCByPartnerId(Allocate_EVCFromViewModel allocate, List<EVC_PartnerViewModel> eVCPartnerViewModels, int? orderTransId)
        {
            bool flag = false;
            TblWalletTransaction? tblWalletTransaction = null;
            int LGCCost = 0;
            decimal UTCCost = 0;
            decimal GstAmt = 0;
            EVCPriceViewModel? eVCPriceVM = null;
            try
            {
                if (allocate != null && orderTransId > 0)
                {
                    foreach (var item in eVCPartnerViewModels)
                    {
                        TblOrderTran tblOrderTran = _OrderTransRepository.GetSingleOrderWithExchangereference(Convert.ToInt32(orderTransId));
                        if (tblOrderTran != null)
                        {
                            #region Changes in wallettranscation table
                            tblWalletTransaction = _walletTransactionRepository.GetSingle(x => x.OrderTransId == orderTransId && x.IsActive == true);
                            if (tblWalletTransaction != null)
                            {
                                #region Set Order Amount on the basis of EVC Seetener flag
                                if (item.IsSweetenerAmtInclude == true)
                                {
                                    #region Get LGC Cost, UTC Cost and GST
                                    eVCPriceVM = _commonManager.CalculateEVCPriceDetailed(tblOrderTran, item.IsSweetenerAmtInclude, item.GSTTypeId ?? 0);
                                    #endregion
                                    tblWalletTransaction.IsOrderAmtWithSweetener = true;
                                    tblWalletTransaction.SweetenerAmt = allocate.SweetenerAmt;
                                    tblWalletTransaction.Lgccost = eVCPriceVM.LGCCost;
                                    tblWalletTransaction.OrderAmount = allocate.ExpectedPriceWithSweetener;
                                }
                                else
                                {
                                    #region Get LGC Cost, UTC Cost and GST
                                    eVCPriceVM = _commonManager.CalculateEVCPriceDetailed(tblOrderTran, item.IsSweetenerAmtInclude, Convert.ToInt32(LoVEnum.GSTInclusive));
                                    #endregion
                                    tblWalletTransaction.OrderAmount = allocate.ExpectedPrice;
                                    tblWalletTransaction.IsOrderAmtWithSweetener = false;
                                }
                                tblWalletTransaction.BaseValue = eVCPriceVM.BaseValue;
                                tblWalletTransaction.GsttypeId = eVCPriceVM.GstTypeId;
                                tblWalletTransaction.Cgstamt = eVCPriceVM.CGSTAmount;
                                tblWalletTransaction.Sgstamt = eVCPriceVM.SGSTAmount;
                                #endregion

                                tblWalletTransaction.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted).ToString();
                                tblWalletTransaction.OrderofAssignDate = _currentDatetime;
                                tblWalletTransaction.RegdNo = tblOrderTran.RegdNo;
                                tblWalletTransaction.OrderType = tblOrderTran.OrderType.ToString();
                                tblWalletTransaction.IsActive = true;
                                tblWalletTransaction.ModifiedBy = 3;
                                tblWalletTransaction.ModifiedDate = _currentDatetime;
                                tblWalletTransaction.EvcregistrationId = item.EvcregistrationId;
                                tblWalletTransaction.OrderTransId = tblOrderTran.OrderTransId;
                                tblWalletTransaction.IsPrimeProductId = false;
                                tblWalletTransaction.ReassignCount = 0;
                                tblWalletTransaction.EvcpartnerId = item.EvcPartnerId;
                                _walletTransactionRepository.Update(tblWalletTransaction);
                                _walletTransactionRepository.SaveChanges();
                            }
                            else
                            {
                                tblWalletTransaction = new TblWalletTransaction();
                                tblWalletTransaction.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted).ToString();
                                tblWalletTransaction.OrderofAssignDate = _currentDatetime;
                                tblWalletTransaction.RegdNo = tblOrderTran.RegdNo;
                                #region Old Synerio
                                //order Amount
                                //if (allocate.ExpectedPrice != null && allocate.ExpectedPrice > 0)
                                //{
                                //    tblWalletTransaction.OrderAmount = allocate.ExpectedPrice;
                                //}
                                //else
                                //{
                                //    if (tblOrderTran.FinalPriceAfterQc != null)
                                //    {
                                //        tblWalletTransaction.OrderAmount = tblOrderTran.FinalPriceAfterQc;
                                //    }
                                //    tblWalletTransaction.OrderAmount = 0;
                                //}
                                #endregion

                                #region Set Order Amount on the basis of EVC Seetener flag
                                if (item.IsSweetenerAmtInclude == true)
                                {
                                    #region Get LGC Cost, UTC Cost and GST
                                    eVCPriceVM = _commonManager.CalculateEVCPriceDetailed(tblOrderTran, item.IsSweetenerAmtInclude, item.GSTTypeId ?? 0);
                                    #endregion
                                    tblWalletTransaction.IsOrderAmtWithSweetener = true;
                                    tblWalletTransaction.SweetenerAmt = allocate.SweetenerAmt;
                                    tblWalletTransaction.Lgccost = eVCPriceVM.LGCCost;
                                    tblWalletTransaction.OrderAmount = allocate.ExpectedPriceWithSweetener;
                                }
                                else
                                {
                                    #region Get LGC Cost, UTC Cost and GST
                                    eVCPriceVM = _commonManager.CalculateEVCPriceDetailed(tblOrderTran, item.IsSweetenerAmtInclude, Convert.ToInt32(LoVEnum.GSTInclusive));
                                    #endregion
                                    tblWalletTransaction.OrderAmount = allocate.ExpectedPrice;
                                    tblWalletTransaction.IsOrderAmtWithSweetener = false;
                                }
                                tblWalletTransaction.BaseValue = eVCPriceVM.BaseValue;
                                tblWalletTransaction.GsttypeId = eVCPriceVM.GstTypeId;
                                tblWalletTransaction.Cgstamt = eVCPriceVM.CGSTAmount;
                                tblWalletTransaction.Sgstamt = eVCPriceVM.SGSTAmount;
                                #endregion

                                tblWalletTransaction.OrderType = tblOrderTran.OrderType.ToString();
                                tblWalletTransaction.IsActive = true;
                                tblWalletTransaction.CreatedBy = 3;
                                tblWalletTransaction.CreatedDate = _currentDatetime;
                                tblWalletTransaction.EvcregistrationId = item.EvcregistrationId;
                                tblWalletTransaction.OrderTransId = tblOrderTran.OrderTransId;
                                tblWalletTransaction.IsPrimeProductId = false;
                                tblWalletTransaction.ReassignCount = 0;
                                tblWalletTransaction.ModifiedBy = 3;
                                tblWalletTransaction.ModifiedDate = _currentDatetime;
                                tblWalletTransaction.EvcpartnerId = item.EvcPartnerId;
                                _walletTransactionRepository.Create(tblWalletTransaction);
                                _walletTransactionRepository.SaveChanges();

                            }
                            #endregion

                            #region Update into TblOrderTrans
                            tblOrderTran.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted);
                            tblOrderTran.ModifiedBy = 3;
                            tblOrderTran.ModifiedDate = _currentDatetime;
                            _OrderTransRepository.Update(tblOrderTran);
                            _OrderTransRepository.SaveChanges();
                            #endregion

                            #region update in exchange or abb order
                            if (tblOrderTran != null && tblOrderTran.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                            {
                                #region Update into TblExchangeOrders
                                TblExchangeOrder TblExchangeOrders = _exchangeOrderRepository.GetExchOrderByRegdNo(tblOrderTran.RegdNo);
                                if (TblExchangeOrders != null)
                                {
                                    TblExchangeOrders.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted);
                                    TblExchangeOrders.OrderStatus = "EVC_Assign";
                                    TblExchangeOrders.ModifiedBy = 3;
                                    TblExchangeOrders.ModifiedDate = _currentDatetime;
                                    _exchangeOrderRepository.Update(TblExchangeOrders);
                                    _exchangeOrderRepository.SaveChanges();
                                }
                                #endregion

                                #region Insert into tblexchangeabbhistory
                                TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                tblExchangeAbbstatusHistory.OrderType = (int)tblOrderTran.OrderType;
                                tblExchangeAbbstatusHistory.SponsorOrderNumber = tblOrderTran.Exchange.SponsorOrderNumber;
                                tblExchangeAbbstatusHistory.RegdNo = tblOrderTran.Exchange.RegdNo;
                                tblExchangeAbbstatusHistory.ZohoSponsorId = tblOrderTran.Exchange.ZohoSponsorOrderId != null ? TblExchangeOrders.ZohoSponsorOrderId : string.Empty;
                                tblExchangeAbbstatusHistory.CustId = tblOrderTran.Exchange.CustomerDetailsId;
                                tblExchangeAbbstatusHistory.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted);
                                tblExchangeAbbstatusHistory.IsActive = true;
                                tblExchangeAbbstatusHistory.CreatedBy = 3;
                                tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                                tblExchangeAbbstatusHistory.Evcid = tblWalletTransaction.EvcregistrationId;
                                _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                                #endregion

                                #region send whatsapp notification to EVC Partner
                                TblWhatsAppMessage tblwhatsappmessage = null;
                                WhatasappEvcAutoAllocationResponse whatasappResponse = new WhatasappEvcAutoAllocationResponse();
                                WhatsappEvcAutoAllocationTemplate whatsappObj = new WhatsappEvcAutoAllocationTemplate();
                                whatsappObj.userDetails = new UserDetailsAutoAllocation();
                                whatsappObj.notification = new AutoAllocation();
                                whatsappObj.notification.@params = new AutoAllocationURL();
                                whatsappObj.userDetails.number = item.ContactNumber;
                                whatsappObj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
                                whatsappObj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
                                whatsappObj.notification.templateId = NotificationConstants.EvcAutoAllocation;
                                whatsappObj.notification.@params.Customername = tblOrderTran.Exchange.CustomerDetails.FirstName;
                                whatsappObj.notification.@params.OrderNumber = tblOrderTran.Exchange.RegdNo;
                                whatsappObj.notification.@params.EvcPrice = allocate.ExpectedPrice;
                                whatsappObj.notification.@params.ProductCategory = tblOrderTran.Exchange.ProductType.ProductCat.Description;
                                whatsappObj.notification.@params.ProductType = tblOrderTran.Exchange.ProductType.Description;
                                string url = _baseConfig.Value.YellowAiUrl;
                                RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                                int statusCode = Convert.ToInt32(response.StatusCode);
                                if (response.Content != null && statusCode == 202)
                                {
                                    whatasappResponse = JsonConvert.DeserializeObject<WhatasappEvcAutoAllocationResponse>(response.Content);
                                    tblwhatsappmessage = new TblWhatsAppMessage();
                                    tblwhatsappmessage.TemplateName = NotificationConstants.EvcAutoAllocation;
                                    tblwhatsappmessage.IsActive = true;
                                    tblwhatsappmessage.PhoneNumber = item.ContactNumber;
                                    tblwhatsappmessage.SendDate = DateTime.Now;
                                    tblwhatsappmessage.MsgId = whatasappResponse.msgId;
                                    _WhatsAppMessageRepository.Create(tblwhatsappmessage);
                                    _WhatsAppMessageRepository.SaveChanges();
                                }
                                #endregion
                            }
                            else
                            {
                                #region Update into TblAbbredmption
                                TblAbbredemption tblAbbredemption = _aBBRedemptionRepository.GetAbbOrderDetails(tblOrderTran.RegdNo);
                                if (tblAbbredemption != null)
                                {
                                    tblAbbredemption.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted);
                                    tblAbbredemption.ModifiedBy = 3;
                                    tblAbbredemption.ModifiedDate = _currentDatetime;
                                    _aBBRedemptionRepository.Update(tblAbbredemption);
                                    _aBBRedemptionRepository.SaveChanges();
                                }
                                #endregion

                                #region Insert into tblexchangeabbhistory
                                TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                tblExchangeAbbstatusHistory.OrderType = (int)tblOrderTran.OrderType;
                                tblExchangeAbbstatusHistory.RegdNo = tblOrderTran.RegdNo;
                                tblExchangeAbbstatusHistory.CustId = tblOrderTran.Abbredemption.CustomerDetailsId;
                                tblExchangeAbbstatusHistory.StatusId = Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted);
                                tblExchangeAbbstatusHistory.IsActive = true;
                                tblExchangeAbbstatusHistory.CreatedBy = 3;
                                tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                                tblExchangeAbbstatusHistory.Evcid = tblWalletTransaction.EvcregistrationId;
                                _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                                #endregion

                                #region send whatsapp notification to EVC Partner
                                TblWhatsAppMessage tblwhatsappmessage = null;
                                WhatasappEvcAutoAllocationResponse whatasappResponse = new WhatasappEvcAutoAllocationResponse();
                                WhatsappEvcAutoAllocationTemplate whatsappObj = new WhatsappEvcAutoAllocationTemplate();
                                whatsappObj.userDetails = new UserDetailsAutoAllocation();
                                whatsappObj.notification = new AutoAllocation();
                                whatsappObj.notification.@params = new AutoAllocationURL();
                                whatsappObj.userDetails.number = item.ContactNumber;
                                whatsappObj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
                                whatsappObj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
                                whatsappObj.notification.templateId = NotificationConstants.EvcAutoAllocation;
                                whatsappObj.notification.@params.Customername = tblOrderTran.Abbredemption.Abbregistration.Customer.FirstName;
                                whatsappObj.notification.@params.OrderNumber = tblOrderTran.Abbredemption.RegdNo;
                                whatsappObj.notification.@params.EvcPrice = allocate.ExpectedPrice;
                                whatsappObj.notification.@params.ProductCategory = tblOrderTran.Abbredemption.Abbregistration.NewProductCategory.Description;
                                whatsappObj.notification.@params.ProductType = tblOrderTran.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description;
                                string url = _baseConfig.Value.YellowAiUrl;
                                RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                                int statusCode = Convert.ToInt32(response.StatusCode);
                                if (response.Content != null && statusCode == 202)
                                {
                                    whatasappResponse = JsonConvert.DeserializeObject<WhatasappEvcAutoAllocationResponse>(response.Content);
                                    tblwhatsappmessage = new TblWhatsAppMessage();
                                    tblwhatsappmessage.TemplateName = NotificationConstants.EvcAutoAllocation;
                                    tblwhatsappmessage.IsActive = true;
                                    tblwhatsappmessage.PhoneNumber = item.ContactNumber;
                                    tblwhatsappmessage.SendDate = DateTime.Now;
                                    tblwhatsappmessage.MsgId = whatasappResponse.msgId;
                                    _WhatsAppMessageRepository.Create(tblwhatsappmessage);
                                    _WhatsAppMessageRepository.SaveChanges();
                                }
                                #endregion
                            }

                            #endregion
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "AssignEVCByPartnerId", ex);
            }
            return flag;
        }
        #endregion

        #region Save EVC Partner Details Added by Priyanshi
        /// <summary>
        /// Added by Priyanshi : Add EVC Partner details
        /// </summary>
        /// <param name="EVCPartnerViewModels"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int SaveEVCPartnerDetails(EVC_StoreRegistrastionViewModel eVC_StoreRegistrastionViewModels, int userId)
        {
            #region Variable Declaration
            TblEvcPartner tblEvcPartner = new TblEvcPartner();
            int result = 0;
            #endregion

            try
            {
                if (eVC_StoreRegistrastionViewModels != null)
                {
                    if (eVC_StoreRegistrastionViewModels.EvcPartnerId > 0)
                    {
                        TblEvcPartner tblEvcPartner1 = _eVCPartnerRepository.GetSingle(x => x.IsActive == true && x.EvcPartnerId == eVC_StoreRegistrastionViewModels.EvcPartnerId);
                        //Code to update the object
                        tblEvcPartner1.StateId = eVC_StoreRegistrastionViewModels.StateId;
                        tblEvcPartner1.CityId = eVC_StoreRegistrastionViewModels.CityId;
                        tblEvcPartner1.PinCode = eVC_StoreRegistrastionViewModels.PinCode;
                        tblEvcPartner1.EmailId = eVC_StoreRegistrastionViewModels.EmailId;
                        tblEvcPartner1.ContactNumber = eVC_StoreRegistrastionViewModels.ContactNumber;
                        tblEvcPartner1.Address1 = eVC_StoreRegistrastionViewModels.Address1;
                        tblEvcPartner1.Address2 = eVC_StoreRegistrastionViewModels.Address2;
                        tblEvcPartner1.ModifiedBy = userId;
                        tblEvcPartner1.ModifiedDate = _currentDatetime;
                        tblEvcPartner1.ListOfPincode = eVC_StoreRegistrastionViewModels.ListOfPincode;
                        tblEvcPartner1.IsApprove = eVC_StoreRegistrastionViewModels.IsApprove;
                        _eVCPartnerRepository.Update(tblEvcPartner1);
                        _eVCPartnerRepository.SaveChanges();
                        result = 4;
                    }
                    else
                    {
                        var Check = _eVCPartnerRepository.GetSingle(x => x.EvcregistrationId == eVC_StoreRegistrastionViewModels.EvcregistrationId && x.IsActive == true && x.PinCode == eVC_StoreRegistrastionViewModels.PinCode);
                        if (Check == null)
                        {
                            var getcitycode = _cityRepository.GetSingle(x => x.CityId == eVC_StoreRegistrastionViewModels.CityId);

                            var GetEvcDetails = _evcRepository.GetSingle(x => x.IsActive == true && x.EvcregistrationId == eVC_StoreRegistrastionViewModels.EvcregistrationId);
                            if (GetEvcDetails != null)
                            {
                                //Code to Insert the object                             
                                tblEvcPartner.IsActive = true;
                                tblEvcPartner.CreatedDate = _currentDatetime;
                                tblEvcPartner.Createdby = userId;
                                tblEvcPartner.EvcregistrationId = eVC_StoreRegistrastionViewModels.EvcregistrationId;
                                tblEvcPartner.StateId = eVC_StoreRegistrastionViewModels.StateId;
                                tblEvcPartner.CityId = eVC_StoreRegistrastionViewModels.CityId;
                                tblEvcPartner.PinCode = eVC_StoreRegistrastionViewModels.PinCode;
                                tblEvcPartner.EmailId = eVC_StoreRegistrastionViewModels.EmailId;
                                tblEvcPartner.ContactNumber = eVC_StoreRegistrastionViewModels.ContactNumber;
                                tblEvcPartner.Address1 = eVC_StoreRegistrastionViewModels.Address1;
                                tblEvcPartner.Address2 = eVC_StoreRegistrastionViewModels.Address2;
                                tblEvcPartner.ModifiedBy = userId;
                                tblEvcPartner.ModifiedDate = _currentDatetime;
                                tblEvcPartner.IsApprove = false;
                                tblEvcPartner.EvcStoreCode = GenerateEVCStoreCode(GetEvcDetails.EvcregdNo, getcitycode.CityCode);
                                //tblEvcPartner.EvcStoreCode = "EHYTMF007";
                                _eVCPartnerRepository.Create(tblEvcPartner);
                                _eVCPartnerRepository.SaveChanges();
                                result = 1;
                            }
                            else
                            {
                                result = 2;
                            }

                        }
                        else
                        {
                            result = 3;
                        }
                    }
                }
                else
                {
                    result = 0;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "SaveEVCPartnerDetails", ex);
                result = 0;
            }
            return result;
        }
        #endregion

        #region Save EVC Partner Store_Specification Details Added by Priyanshi
        /// <summary>
        /// Added by Priyanshi : Add EVC Partner details
        /// </summary>
        /// <param name="EVCPartnerViewModels"></param>
        /// <param name="userId"></param>
        /// <returns></returns>       
        public int SaveStoreSpecificationDetails(EVCStore_SpecificationViewModel eVCStore_SpecificationViewModel, int userId)
        {
            #region Variable Declaration
            TblEvcPartner tblEvcPartner = new TblEvcPartner();
            int result = 0;
            #endregion

            try
            {
                if (eVCStore_SpecificationViewModel != null)
                {
                    if (eVCStore_SpecificationViewModel.EvcPartnerpreferenceId > 0)
                    {
                        // Existing preference found, update it
                        //TblEvcPartnerPreference tblEvcPartnerPreference = _eVCPartnerPreferenceRepository.GetSingle(x => x.IsActive == true && x.EvcpartnerId == eVCStore_SpecificationViewModel.EvcPartnerId && x.EvcPartnerpreferenceId == eVCStore_SpecificationViewModel.EvcPartnerpreferenceId);
                        TblEvcPartnerPreference tblEvcPartnerPreference = _context.TblEvcPartnerPreferences.Where(x => x.IsActive == true && x.EvcpartnerId == eVCStore_SpecificationViewModel.EvcPartnerId && x.EvcPartnerpreferenceId == eVCStore_SpecificationViewModel.EvcPartnerpreferenceId).FirstOrDefault();
                        if (tblEvcPartnerPreference != null)
                        {
                            tblEvcPartnerPreference.ModifiedBy = userId;
                            tblEvcPartnerPreference.ModifiedDate = _currentDatetime;
                            _eVCPartnerPreferenceRepository.Update(tblEvcPartnerPreference);
                            _context.Entry(tblEvcPartnerPreference).State = EntityState.Detached;
                            _eVCPartnerPreferenceRepository.SaveChanges();
                        }
                        else
                        {
                            // Handle the case when preference is not found
                            result = 4; // You can define your custom error code
                        }
                    }
                    else
                    {
                        // Convert the Quality values from a string array to a list of integers
                        List<int> qualityList = eVCStore_SpecificationViewModel.Quality
                            .Select(q => int.TryParse(q, out int parsedQuality) ? parsedQuality : 0)
                            .ToList();

                        if (qualityList.Any())
                        {
                            // Delete existing preferences for the same EvcpartnerId and productcatId
                            List<TblEvcPartnerPreference> tblEvcPartnerPreferencesToDelete = _eVCPartnerPreferenceRepository.GetList(x => x.EvcpartnerId == eVCStore_SpecificationViewModel.EvcPartnerId && x.ProductCatId == eVCStore_SpecificationViewModel.productcatId).ToList();
                            foreach (var preferenceToDelete in tblEvcPartnerPreferencesToDelete)
                            {
                                preferenceToDelete.IsActive = false;
                                preferenceToDelete.ModifiedBy = userId;
                                preferenceToDelete.ModifiedDate = _currentDatetime;
                                _eVCPartnerPreferenceRepository.Update(preferenceToDelete);
                                _context.Entry(preferenceToDelete).State = EntityState.Detached;
                                _eVCPartnerPreferenceRepository.SaveChanges();

                            }

                            // Create new preferences for selected qualities
                            foreach (var qualityId in qualityList)
                            {
                                TblEvcPartnerPreference? tblEvcPartnerPreference1 = null;
                                //TblEvcPartnerPreference tblEvcPartnerPreference1 = _eVCPartnerPreferenceRepository.GetSingle(x => x.EvcpartnerId == eVCStore_SpecificationViewModel.EvcPartnerId && x.ProductCatId== eVCStore_SpecificationViewModel.productcatId&&x.ProductQualityId== qualityId);
                                tblEvcPartnerPreference1 = _context.TblEvcPartnerPreferences.Where(x => x.EvcpartnerId == eVCStore_SpecificationViewModel.EvcPartnerId && x.ProductCatId == eVCStore_SpecificationViewModel.productcatId && x.ProductQualityId == qualityId).FirstOrDefault();


                                if (tblEvcPartnerPreference1 != null)
                                {
                                    var Result = _eVCPartnerPreferenceRepository.Updatedetails(tblEvcPartnerPreference1.EvcPartnerpreferenceId, userId);
                                }
                                else
                                {
                                    TblEvcPartnerPreference evcPartnerPreference = new TblEvcPartnerPreference();
                                    evcPartnerPreference.IsActive = true;
                                    evcPartnerPreference.CreatedDate = _currentDatetime;
                                    evcPartnerPreference.Createdby = userId;
                                    evcPartnerPreference.EvcpartnerId = eVCStore_SpecificationViewModel.EvcPartnerId;
                                    evcPartnerPreference.ProductCatId = eVCStore_SpecificationViewModel.productcatId;
                                    evcPartnerPreference.ProductQualityId = qualityId;
                                    evcPartnerPreference.ModifiedBy = userId;
                                    evcPartnerPreference.ModifiedDate = _currentDatetime;
                                    _eVCPartnerPreferenceRepository.Create(evcPartnerPreference);
                                    _eVCPartnerPreferenceRepository.SaveChanges();
                                }
                            }

                            result = 1;
                        }
                        else
                        {
                            // No quality selected, delete all preferences for the same EvcpartnerId and productcatId

                            result = 2;
                        }
                    }
                }
                else
                {
                    result = 0;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "SaveEVCPartnerDetails", ex);
                result = 0;
            }
            return result;
        }

        #endregion

        #region Method to Generate EVC Store Code
        public string GenerateEVCStoreCode(string EVCRegdNo, string CityCode)
        {
            string updatedValue = null;
            TblEvcPartner tblEvcPartner = _context.TblEvcPartners.Where(x => x.EvcStoreCode != null).ToList().LastOrDefault();
            if (tblEvcPartner != null)
            {
                string[] res = tblEvcPartner.EvcStoreCode.Split('_');
                Console.WriteLine(res[1]);
                int val = Convert.ToInt32(res[1]);
                val++;
                updatedValue = "_00" + val;
            }
            string EVCStoreCode = EVCRegdNo + CityCode + updatedValue;
            return EVCStoreCode;
        }
        #endregion

        #region ADDED BY Priyanshi sahu---- this method use for REassign Assignment---Get EVC BY City and state and pin 
        public IList<EVcListRessign> GetEVCListforEVCReassign(string? ActualProdQltyAtQc, int ProductCatId, string? pin, int EVCId, int? statusId, int OrdertransId, int ExpectedPrice, int ordertype)
        {
            IList<EVcListRessign> eVcLists = new List<EVcListRessign>();
            List<TblEvcregistration> tblEvcregistrations = new List<TblEvcregistration>();
            List<TblWalletTransaction> TblWalletTransactions = new List<TblWalletTransaction>();
            List<TblEvcPartner> tblEvcPartnerList = new List<TblEvcPartner>();
            try
            {
                if (!string.IsNullOrEmpty(pin) && ProductCatId > 0)
                {
                    if (!string.IsNullOrEmpty(pin))
                    {
                        tblEvcPartnerList = _eVCPartnerRepository.GetAllEvcPartnerListByPincode(pin, ProductCatId, ActualProdQltyAtQc, ordertype);
                        //tblEvcPartnerList1 = tblEvcPartnerList 
                    }
                    if (statusId != Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted))
                    {
                        if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
                        {
                            //#region Get Expected EVC Price
                            //// Get Expected EVC Price With Sweetener Implementation
                            //int EVCPriceWithSweetener = _commonManager.CalculateEVCPriceNew(OrdertransId,true,Convert.ToInt32(LoVEnum.GSTExclusive));
                            //int EVCPriceWithoutSweetener = _commonManager.CalculateEVCPriceNew(OrdertransId, false, Convert.ToInt32(LoVEnum.GSTInclusive));
                            //#endregion
                            tblEvcPartnerList = _commonManager.GetEVCPartnerListHavingClearBalance(OrdertransId, tblEvcPartnerList);
                        }
                    }
                    if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
                    {
                        tblEvcPartnerList = tblEvcPartnerList.Where(x => x.EvcregistrationId != EVCId).ToList();
                        if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
                        {
                            List<EVC_PartnerViewModel> eVCPartnerViewModels1 = new List<EVC_PartnerViewModel>();
                            foreach (var item in tblEvcPartnerList)
                            {
                                // Create an EVcList object and add it to the EVCList
                                EVcListRessign evclist = new EVcListRessign
                                {
                                    EvcPartnerId = item.EvcPartnerId,
                                    EvcregistrationId = item.EvcregistrationId,
                                    EvcregdNo = item.Evcregistration.EvcregdNo,
                                    BussinessName = item.Evcregistration.BussinessName
                                };
                                if (evclist != null)
                                {
                                    eVcLists.Add(evclist);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "GetEVCListbycityAndpin", ex);
            }
            return eVcLists;
        }

        #endregion


    }
}





