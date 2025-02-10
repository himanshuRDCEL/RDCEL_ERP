using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.LGC;
using RDCELERP.Model.ServicePartner;
using RDCELERP.BAL.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.Model.City;
using RDCELERP.Model.MobileApplicationModel.Common;
using Microsoft.AspNetCore.Hosting;
using RDCELERP.Model.MapSerVicePartner;
using RDCELERP.Common.Enums;
using RDCELERP.Model.PinCode;
using RDCELERP.Model.Users;
using RDCELERP.Common.Constant;
using RDCELERP.Model.LGC;
using System.Text.Json;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.MobileApplicationModel.EVC;
using static RDCELERP.Model.Whatsapp.WhatsappRescheduleViewModel;
using System.Drawing;
using RDCELERP.Model.ImagLabel;
using System.IO;
using System.Security.Policy;
using RDCELERP.Model.DriverDetails;
using RDCELERP.Model.Dashboards;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.Office2010.Excel;
using RDCELERP.Model.VehicleJourneyViewModel;

namespace RDCELERP.BAL.MasterManager
{
    public class SevicePartnerManager : IServicePartnerManager
    {
        #region  Variable Declaration
        IServicePartnerRepository _servicePartnerRepository;
        ImapServicePartnerCityStateRepository _mapServicePartnerCityStateRepository;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IUserRoleRepository _userRoleRepository;
        IImageHelper _imageHelper;
        IManageUserForServicePartnerManager _manageUserForServicePartnerManager;
        ICityRepository _cityRepository;
        IDriverDetailsRepository _driverDetailsRepository;
        ILogisticsRepository _logisticsRepository;
        IExchangeOrderRepository _exchangeOrderRepository;
        public readonly IOptions<ApplicationSettings> _baseConfig;
        ICustomerDetailsRepository _customerDetailsRepository;
        IUserRepository _userRepository;
        private readonly Digi2l_DevContext _context;
        ImapServicePartnerCityStateRepository _imapServicePartnerCityStateRepository;
        ILogin_MobileRepository _login_MobileRepository;
        IOrderTransRepository _orderTransRepository;
        IOrderLGCRepository _orderLGCRepository;
        IExchangeABBStatusHistoryRepository _exchangeABBStatusHistoryRepository;
        IWalletTransactionRepository _walletTransactionRepository;
        IExchangeABBStatusHistoryManager _exchangeABBStatusHistoryManager;
        IPinCodeRepository _pinCodeRepository;
        IDriverDetailsManager _driverDetailsManager;
        //UserManager _userManager ;
        ICompanyRepository _companyRepository;
        IRoleRepository _roleRepository;
        ICommonManager _commonManager;
        IEVCManager _eVCManager;
        private readonly IVehicleJourneyTrackingDetailsRepository _journeyTrackingDetailsRepository;
        private readonly IVehicleJourneyTrackingRepository _journeyTrackingRepository;
        IVehicleJourneyTrackingDetailsRepository _vehicleJourneyTrackingDetailsRepository;
        IEVCRepository _eVCRepository;
        IEVCPartnerRepository _evCPartnerRepository;
        private readonly IPushNotificationManager _pushNotificationManager;
        IABBRedemptionRepository _abbRedemptionRepository;
        IBusinessUnitRepository _businessUnitRepository;
        IPaymentLeaser _paymentLeaser;
        IDriverListRepository _driverListRepository;
        IVehicleListRepository _vehicleListRepository;

        #endregion

        #region Constructor
        public SevicePartnerManager(ILogin_MobileRepository login_MobileRepository, IManageUserForServicePartnerManager manageUserForServicePartnerManager, ImapServicePartnerCityStateRepository mapServicePartnerCityStateRepository, IWebHostEnvironment webHostEnvironment, ImapServicePartnerCityStateRepository imapServicePartnerCityStateRepository, Digi2l_DevContext context, IUserRepository userRepository, ICustomerDetailsRepository customerDetailsRepository, IOptions<ApplicationSettings> baseConfig, IExchangeOrderRepository exchangeOrderRepository, ILogisticsRepository logisticsRepository, IDriverDetailsRepository driverDetailsRepository, IServicePartnerRepository servicePartnerRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging, IImageHelper imageHelper, ICityRepository cityRepository, IOptions<ApplicationSettings> config, IOrderTransRepository orderTransRepository, IOrderLGCRepository orderLGCRepository, IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository, IWalletTransactionRepository walletTransactionRepository, IExchangeABBStatusHistoryManager exchangeABBStatusHistoryManager, IPinCodeRepository pinCodeRepository, IDriverDetailsManager driverDetailsManager, ICompanyRepository companyRepository, IRoleRepository roleRepository, ICommonManager commonManager, IEVCManager eVCManager, IVehicleJourneyTrackingDetailsRepository journeyTrackingDetailsRepository, IVehicleJourneyTrackingRepository journeyTrackingRepository, IVehicleJourneyTrackingDetailsRepository vehicleJourneyTrackingDetailsRepository, IEVCRepository eVCRepository, IPushNotificationManager pushNotificationManager, IABBRedemptionRepository aBBRedemptionRepository, IBusinessUnitRepository businessUnitRepository, IEVCPartnerRepository eVCPartnerRepository, IPaymentLeaser paymentLeaser, IDriverListRepository driverListRepository, IVehicleListRepository vehicleListRepository)
        {
            _servicePartnerRepository = servicePartnerRepository;
            _mapServicePartnerCityStateRepository = mapServicePartnerCityStateRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
            _manageUserForServicePartnerManager = manageUserForServicePartnerManager;
            _webHostEnvironment = webHostEnvironment;
            _logging = logging;
            _imageHelper = imageHelper;
            _cityRepository = cityRepository;
            _driverDetailsRepository = driverDetailsRepository;
            _logisticsRepository = logisticsRepository;
            _exchangeOrderRepository = exchangeOrderRepository;
            _baseConfig = baseConfig;
            _customerDetailsRepository = customerDetailsRepository;
            _userRepository = userRepository;
            _context = context;
            _imapServicePartnerCityStateRepository = imapServicePartnerCityStateRepository;
            _login_MobileRepository = login_MobileRepository;
            _orderTransRepository = orderTransRepository;
            _orderLGCRepository = orderLGCRepository;
            _exchangeABBStatusHistoryRepository = exchangeABBStatusHistoryRepository;
            _walletTransactionRepository = walletTransactionRepository;
            _exchangeABBStatusHistoryManager = exchangeABBStatusHistoryManager;
            _pinCodeRepository = pinCodeRepository;
            _driverDetailsManager = driverDetailsManager;
            _companyRepository = companyRepository;
            _roleRepository = roleRepository;
            _commonManager = commonManager;
            _eVCManager = eVCManager;
            _journeyTrackingDetailsRepository = journeyTrackingDetailsRepository;
            _journeyTrackingRepository = journeyTrackingRepository;
            _vehicleJourneyTrackingDetailsRepository = vehicleJourneyTrackingDetailsRepository;
            _eVCRepository = eVCRepository;
            _pushNotificationManager = pushNotificationManager;
            _abbRedemptionRepository = aBBRedemptionRepository;
            _businessUnitRepository = businessUnitRepository;
            _evCPartnerRepository = eVCPartnerRepository;
            _paymentLeaser = paymentLeaser;
            _driverListRepository = driverListRepository;
            _vehicleListRepository = vehicleListRepository;
        }
        #endregion

        #region used in MobileAPP Related API 
        #region Verify Mobile Number and EmailId at time of Registeration
        /// <summary>
        ///  Verify Mobile Number and EmailId at time of Registeration
        ///  added by ashwin
        /// </summary>
        /// <param name="isNumberExits"></param>
        /// <returns></returns>
        //public bool CheckNumberOrEmail(IsNumberorEmailExits isNumberExits)
        //{
        //    bool flag = false;
        //    try
        //    {
        //        if (isNumberExits != null)
        //        {
        //            if (isNumberExits.isMobileNumber == true && isNumberExits.MobileNumber.Length == 10)
        //            {
        //                if (isNumberExits != null && isNumberExits.MobileNumber.Length == 10 && isNumberExits.isServicePartner == true)
        //                {
        //                    TblServicePartner tblServicePartner = null;
        //                    tblServicePartner = _servicePartnerRepository.GetSingle(x => x.ServicePartnerMobileNumber == isNumberExits.MobileNumber);
        //                    if (tblServicePartner != null && tblServicePartner.ServicePartnerId > 0)
        //                    {
        //                        flag = true;
        //                    }
        //                }
        //                else if (isNumberExits != null && isNumberExits.MobileNumber.Length == 10 && isNumberExits.isServicePartnerDriver == true)
        //                {
        //                    TblDriverDetail tblDriverDetails = null;
        //                    tblDriverDetails = _driverDetailsRepository.GetSingle(x => x.DriverPhoneNumber == isNumberExits.MobileNumber);
        //                    if (tblDriverDetails != null && tblDriverDetails.DriverDetailsId > 0)
        //                    {
        //                        flag = true;
        //                    }
        //                }
        //                else
        //                {
        //                    return flag;
        //                }
        //            }
        //            if (isNumberExits.isMobileNumber == false)
        //            {
        //                if (isNumberExits != null && isNumberExits.Email != null && isNumberExits.Email.Length > 0 && isNumberExits.isServicePartner == true)
        //                {
        //                    TblServicePartner tblServicePartner = null;
        //                    tblServicePartner = _servicePartnerRepository.GetSingle(x => x.ServicePartnerEmailId == isNumberExits.Email);
        //                    if (tblServicePartner != null && tblServicePartner.ServicePartnerId > 0)
        //                    {
        //                        flag = true;
        //                    }
        //                }
        //                else
        //                {
        //                    return flag;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            return flag;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.WriteErrorToDB("SevicePartnerManager", "CheckNumber", ex);
        //    }
        //    return flag;
        //}

        #endregion

        #region Verify Mobile Number and EmailId at time of Registeration
        public bool CheckNumberOrEmail(IsNumberorEmailExits isNumberExits)
        {
            bool flag = false;
            try
            {
                if (isNumberExits != null)
                {
                    var TBlUser = _context.TblUsers.Where(x => x.IsActive == true).ToList();
                    if (isNumberExits.MobileNumber != null)
                    {
                        var number = SecurityHelper.EncryptString(isNumberExits.MobileNumber, _baseConfig.Value.SecurityKey);
                        bool NumberFlagUser = TBlUser.Exists(p => p.Phone == number);
                        if (NumberFlagUser)
                        {
                            flag = true;
                        }
                    }
                    if (isNumberExits.Email != null)
                    {
                        var email = SecurityHelper.EncryptString(isNumberExits.Email, _baseConfig.Value.SecurityKey);
                        bool emailFlagUser = TBlUser.Exists(p => p.Email == email);
                        if (emailFlagUser)
                        {
                            flag = true;
                        }
                    }
                }
                else
                {
                    return flag;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("SevicePartnerManager", "CheckNumber", ex);
            }
            return flag;
        }
        #endregion

        #region ServicePartner/LGC Registation   ADDED BY Ashwin /Modify by priyanshi 
        /// <summary>
        /// Add ServicePartner inti TblServicePartner
        /// Used in api of LGC Registeration in mobile development
        /// </summary>
        /// <param name="LGCRegisterationModel"></param>
        /// <param name=""></param>
        /// <returns>tblServicePartner</returns>
        public ResponseResult LGCRegister([FromForm] RegisterServicePartnerDataModel LGCRegisterationModel)
        {
            TblEvcregistration TblEvcregistration = new TblEvcregistration();
            TblServicePartner tblServicePartner = new TblServicePartner();

            int userId = 3;
            bool cencelCheck = false;
            ResponseResult responseMessage = new ResponseResult();
            try
            {
                if (LGCRegisterationModel != null)
                {
                    tblServicePartner = _mapper.Map<RegisterServicePartnerDataModel, TblServicePartner>(LGCRegisterationModel);

                    if (tblServicePartner != null)
                    {

                        #region Check email & mobile number is exists or not
                        var checkemail = _context.TblServicePartners.Where(x => x.IsActive == true).ToList();
                        bool emailflag = checkemail.Exists(p => p.ServicePartnerEmailId == tblServicePartner.ServicePartnerEmailId);

                        if (emailflag == true)
                        {
                            responseMessage.message = "This Email is Already Exists";
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                            return responseMessage;
                        }
                        else
                        {
                            var checkemailinUser = _context.TblUsers.Where(x => x.IsActive == true).ToList();
                            var Email = SecurityHelper.EncryptString(tblServicePartner.ServicePartnerEmailId, _baseConfig.Value.SecurityKey);
                            bool emailUserflag = checkemailinUser.Exists(p => p.Email == Email);
                            if (emailUserflag == true)
                            {
                                responseMessage.message = "This Email is Already Exists for another user";
                                responseMessage.Status = false;
                                responseMessage.Status_Code = HttpStatusCode.OK;
                                return responseMessage;
                            }
                            else
                            {
                                bool numberflag = checkemail.Exists(p => p.ServicePartnerMobileNumber == tblServicePartner.ServicePartnerMobileNumber);
                                if (numberflag == true)
                                {
                                    responseMessage.message = "This Mobile Number is Already Exists";
                                    responseMessage.Status = false;
                                    responseMessage.Status_Code = HttpStatusCode.OK;
                                    return responseMessage;
                                }
                                else
                                {
                                    var numberflagUser = _context.TblUsers.Where(x => x.IsActive == true).ToList();
                                    var number = SecurityHelper.EncryptString(tblServicePartner.ServicePartnerMobileNumber, _baseConfig.Value.SecurityKey);
                                    bool NumberUserflag = numberflagUser.Exists(p => p.Phone == number);
                                    if (NumberUserflag == true)
                                    {
                                        responseMessage.message = "This Mobile Number is Already Exists for another user";
                                        responseMessage.Status = false;
                                        responseMessage.Status_Code = HttpStatusCode.OK;
                                        return responseMessage;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region ADD ADD LGC REGDNO
                        var getcitycode = _cityRepository.GetSingle(x => x.CityId == LGCRegisterationModel.ServicePartnerCityId);


                        tblServicePartner.ServicePartnerRegdNo = "SP" + UniqueString.RandomNumber();


                        #endregion

                        #region add images to folder


                        if (LGCRegisterationModel.ServicePartnerCancelledCheque != null)
                        {
                            string fileName = string.Empty;
                            if (tblServicePartner.ServicePartnerRegdNo != null)
                            {
                                fileName = tblServicePartner.ServicePartnerRegdNo + "CancelledCheck" + ".jpg";
                            }
                            else
                            {

                                fileName = tblServicePartner.ServicePartnerId + "CancelledCheck" + ".jpg";
                            }


                            string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\CancelledCheque";

                            cencelCheck = _imageHelper.SaveFileDefRoot(LGCRegisterationModel.ServicePartnerCancelledCheque, filePath, fileName);
                            if (cencelCheck)
                            {
                                tblServicePartner.ServicePartnerCancelledCheque = fileName;
                                cencelCheck = false;
                            }
                        }
                        if (LGCRegisterationModel.ServicePartnerGstregisteration != null)
                        {
                            string fileName = string.Empty;
                            if (tblServicePartner.ServicePartnerRegdNo != null)
                            {
                                fileName = tblServicePartner.ServicePartnerRegdNo + "ServicePartner_GST" + ".jpg";
                            }
                            else
                            {
                                fileName = tblServicePartner.ServicePartnerId + "ServicePartner_GST" + ".jpg";
                            }

                            //string fileName = tblServicePartner.ServicePartnerRegdNo + "ServicePartner_GST" + ".jpg";
                            string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\GST";
                            cencelCheck = _imageHelper.SaveFileDefRoot(LGCRegisterationModel.ServicePartnerGstregisteration, filePath, fileName);
                            if (cencelCheck)
                            {
                                tblServicePartner.ServicePartnerGstregisteration = fileName;
                                cencelCheck = false;
                            }
                        }
                        if (LGCRegisterationModel.ServicePartnerAadharfrontImage != null)
                        {
                            string fileName = string.Empty;
                            if (tblServicePartner.ServicePartnerRegdNo != null)
                            {
                                fileName = tblServicePartner.ServicePartnerRegdNo + "AadharFront" + ".jpg";
                            }
                            else
                            {
                                fileName = tblServicePartner.ServicePartnerId + "AadharFront" + ".jpg";
                            }
                            //string fileName = tblServicePartner.ServicePartnerRegdNo + "AadharFront" + ".jpg";
                            string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\Aadhar";
                            cencelCheck = _imageHelper.SaveFileDefRoot(LGCRegisterationModel.ServicePartnerAadharfrontImage, filePath, fileName);
                            if (cencelCheck)
                            {
                                tblServicePartner.ServicePartnerAadharfrontImage = fileName;
                                cencelCheck = false;
                            }
                        }
                        if (LGCRegisterationModel.ServicePartnerAadharBackImage != null)
                        {
                            string fileName = string.Empty;
                            if (tblServicePartner.ServicePartnerRegdNo != null)
                            {
                                fileName = tblServicePartner.ServicePartnerRegdNo + "AadharBack" + ".jpg";
                            }
                            else
                            {
                                fileName = tblServicePartner.ServicePartnerId + "AadharBack" + ".jpg";
                            }
                            //string fileName = tblServicePartner.ServicePartnerRegdNo + "AadharBack" + ".jpg";
                            string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\Aadhar";
                            cencelCheck = _imageHelper.SaveFileDefRoot(LGCRegisterationModel.ServicePartnerAadharBackImage, filePath, fileName);
                            if (cencelCheck)
                            {
                                tblServicePartner.ServicePartnerAadharBackImage = fileName;
                                cencelCheck = false;
                            }
                        }
                        if (LGCRegisterationModel.ServicePartnerProfilePic != null)
                        {
                            string fileName = string.Empty;
                            if (tblServicePartner.ServicePartnerRegdNo != null)
                            {
                                fileName = tblServicePartner.ServicePartnerRegdNo + "ProfilePic" + ".jpg";
                            }
                            else
                            {
                                fileName = tblServicePartner.ServicePartnerId + "ProfilePic" + ".jpg";
                            }
                            //string fileName = "Profile" + ".jpg";
                            string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\ProfilePic";
                            cencelCheck = _imageHelper.SaveFileDefRoot(LGCRegisterationModel.ServicePartnerProfilePic, filePath, fileName);
                            if (cencelCheck)
                            {
                                tblServicePartner.ServicePartnerProfilePic = fileName;
                                cencelCheck = false;
                            }
                        }
                        if (LGCRegisterationModel.ServicePartnerGstregisteration != null)
                        {
                            string fileName = string.Empty;
                            if (tblServicePartner.ServicePartnerGstregisteration != null)
                            {
                                fileName = tblServicePartner.ServicePartnerRegdNo + "ServicePartner_GST" + ".jpg";
                            }
                            else
                            {
                                fileName = tblServicePartner.ServicePartnerId + "ServicePartner_GST" + ".jpg";
                            }

                            //string fileName = tblServicePartner.ServicePartnerRegdNo + "ServicePartner_GST" + ".jpg";
                            string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\GST";
                            cencelCheck = _imageHelper.SaveFileDefRoot(LGCRegisterationModel.ServicePartnerGstregisteration, filePath, fileName);
                            if (cencelCheck)
                            {
                                tblServicePartner.ServicePartnerGstregisteration = fileName;
                                cencelCheck = false;
                            }
                        }
                        #endregion

                        //Code to Insert the object 
                        tblServicePartner.ServicePartnerIsApprovrd = false;
                        tblServicePartner.IsActive = true;
                        tblServicePartner.CreatedDate = _currentDatetime;
                        tblServicePartner.CreatedBy = userId;

                        _servicePartnerRepository.Create(tblServicePartner);
                        int result = _servicePartnerRepository.SaveChanges();
                        if (result == 1)
                        {
                            responseMessage.message = "Regisiteration Success";
                            responseMessage.Status = true;
                            responseMessage.Status_Code = HttpStatusCode.OK;

                            #region Add City Pincode
                            if (LGCRegisterationModel.addPincodes.Count > 0)
                            {
                                int insertResult = 0;
                                foreach (var item in LGCRegisterationModel.addPincodes)
                                {
                                    List<TblPinCode> tblPinCodes = _pinCodeRepository.GetList(x => x.IsActive == true && x.CityId == item.CityId).ToList();
                                    if (tblPinCodes != null && tblPinCodes.Count > 0)
                                    {
                                        List<mappingZipCode> mappingZipCode = new List<mappingZipCode>();
                                        mappingZipCode = _mapper.Map<List<TblPinCode>, List<mappingZipCode>>(tblPinCodes).ToList();

                                        string str = string.Empty;

                                        //loop to add comma separated zipcode
                                        foreach (var lst in mappingZipCode)
                                            str = str + lst.ZipCode + ",";

                                        str = str.Remove(str.Length - 1);

                                        item.ListOfPincodes = str;
                                        item.StateId = item.StateId;
                                        item.ServicePartnerId = tblServicePartner.ServicePartnerId;
                                        item.CreatedBy = 3;
                                        //item.ModifiedBy = null;
                                        item.CreatedDate = _currentDatetime;
                                        item.ModifiedDate = _currentDatetime;
                                        item.IsActive = true;
                                        //item.PincodeId = null;
                                        MapServicePartnerCityState mapServicePartnerCityState1 = new MapServicePartnerCityState();
                                        mapServicePartnerCityState1 = _mapper.Map<AddPincode, MapServicePartnerCityState>(item);
                                        if (!string.IsNullOrEmpty(item.ListOfPincodes) && mapServicePartnerCityState1 != null)
                                        {
                                            _imapServicePartnerCityStateRepository.Create(mapServicePartnerCityState1);

                                            insertResult = _imapServicePartnerCityStateRepository.SaveChanges();
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        item.StateId = item.StateId;
                                        item.ServicePartnerId = tblServicePartner.ServicePartnerId;
                                        item.CreatedBy = 3;
                                        //item.ModifiedBy = null;
                                        item.CreatedDate = _currentDatetime;
                                        item.ModifiedDate = _currentDatetime;
                                        item.IsActive = true;
                                        //item.PincodeId = null;
                                        MapServicePartnerCityState mapServicePartnerCityState1 = new MapServicePartnerCityState();
                                        mapServicePartnerCityState1 = _mapper.Map<AddPincode, MapServicePartnerCityState>(item);
                                        if (mapServicePartnerCityState1 != null)
                                        {
                                            _imapServicePartnerCityStateRepository.Create(mapServicePartnerCityState1);

                                            insertResult = _imapServicePartnerCityStateRepository.SaveChanges();
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                responseMessage.message = "Regisiteration Success,but pincodes not added";
                                responseMessage.Status = true;
                                responseMessage.Status_Code = HttpStatusCode.OK;
                            }
                            #endregion

                        }
                        else
                        {
                            responseMessage.message = "Data Not Map properply";
                            responseMessage.Status = false;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                _logging.WriteErrorToDB("LogisticManager", "LGCRegister", ex);
                //return responseMessage;
            }
            return responseMessage;
        }

        #endregion

        #region Add LGC Vehicle by api
        /// <summary>
        /// Register LGC Vehichle
        /// Used in api of RegisterVehicle in mobile development
        /// Added by priyanshi
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public ResponseResult AddVehicle([FromForm] VehicleDataModel dataModel)
        {
            TblVehicleList tblVehicleList = null;
            bool cancelCheck = false;
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;

            try
            {
                if (dataModel != null)
                {
                    tblVehicleList = _mapper.Map<VehicleDataModel, TblVehicleList>(dataModel);
                    TblVehicleList tblVehicle = _vehicleListRepository.GetSingle(x => x.IsActive == true && x.VehicleNumber == dataModel.VehicleNumber || x.VehicleRcNumber == dataModel.VehicleRcNumber);
                    if (tblVehicle == null)
                    {
                        if (tblVehicleList != null)
                        {
                            // Add Image in Folder

                            // VehicleRcCertificate
                            if (dataModel.VehicleRcCertificate != null)
                            {
                                string fileName = string.Empty;
                                if (tblVehicleList.VehicleRcNumber != null)
                                {
                                    fileName = tblVehicleList.VehicleRcNumber + "VehicleRcCertificate" + ".jpg";
                                }
                                else
                                {
                                    fileName = tblVehicleList.VehicleNumber + "VehicleRcCertificate" + ".jpg";
                                }
                                string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\Driver\\VehicleRcCertificate";
                                cancelCheck = _imageHelper.SaveFileDefRoot(dataModel.VehicleRcCertificate, filePath, fileName);
                                if (cancelCheck)
                                {
                                    tblVehicleList.VehicleRcCertificate = fileName;
                                    cancelCheck = false;
                                }
                            }

                            // VehiclefitnessCertificate
                            if (dataModel.VehiclefitnessCertificate != null)
                            {
                                string fileName = string.Empty;
                                if (tblVehicleList.VehicleNumber != null)
                                {
                                    fileName = tblVehicleList.VehicleNumber + "VehicleFitnessCertificate" + ".jpg";
                                }
                                else
                                {
                                    fileName = tblVehicleList.VehicleRcNumber + "VehicleFitnessCertificate" + ".jpg";
                                }
                                string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\Driver\\DriverFitnessCerti";
                                cancelCheck = _imageHelper.SaveFileDefRoot(dataModel.VehiclefitnessCertificate, filePath, fileName);
                                if (cancelCheck)
                                {
                                    tblVehicleList.VehiclefitnessCertificate = fileName;
                                    cancelCheck = false;
                                }
                            }

                            // VehicleInsuranceCertificate
                            if (dataModel.VehicleInsuranceCertificate != null)
                            {
                                string fileName = string.Empty;
                                if (tblVehicleList.VehicleInsuranceCertificate != null)
                                {
                                    fileName = tblVehicleList.VehicleRcNumber + "VehicleInsuranceCertificate" + ".jpg";
                                }
                                else
                                {

                                    fileName = tblVehicleList.VehicleNumber + "VehicleInsuranceCertificate" + ".jpg";
                                }

                                string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\Driver\\DriverInsuranceCerti";
                                cancelCheck = _imageHelper.SaveFileDefRoot(dataModel.VehicleInsuranceCertificate, filePath, fileName);
                                if (cancelCheck)
                                {
                                    tblVehicleList.VehicleInsuranceCertificate = fileName;
                                    cancelCheck = false;
                                }
                            }


                            tblVehicleList.IsActive = true;
                            tblVehicleList.CreatedDate = _currentDatetime;
                            tblVehicleList.CreatedBy = dataModel.SPuserId;
                            tblVehicleList.IsApproved = true;
                            tblVehicleList.ApprovedBy = dataModel.SPuserId;

                            _vehicleListRepository.Create(tblVehicleList);
                            int result = _vehicleListRepository.SaveChanges();
                            if (result > 0)
                            {
                                responseMessage.message = "Added Success";
                                responseMessage.Status = true;
                                responseMessage.Status_Code = HttpStatusCode.OK;
                            }
                            else
                            {
                                responseMessage.Status = false;
                                responseMessage.Status_Code = HttpStatusCode.OK;
                                responseMessage.message = "Registration Failed";
                            }
                        }
                        else
                        {
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                            responseMessage.message = "Registration Failed";
                        }
                    }
                    else
                    {
                        if (tblVehicle.VehicleRcNumber == dataModel.VehicleRcNumber)
                        {
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                            responseMessage.message = "Vehicle RC number already exist";
                        }
                        else
                        {
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                            responseMessage.message = "Vehicle number already exist";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("LogisticManager", "AddVehicle", ex);
            }

            return responseMessage;
        }
        #endregion

        #region Api Method Get UserProfileDetails
        /// <summary>
        /// get details of UserProfileDetails on basis of user id
        /// added by priyanshi
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ResponseResult UserProfileDetails(int userid)
        {
            ResponseResult responseResult = new ResponseResult();
            LoginUserDetailsDataViewModal loginUserDetailsDataViewModal = new LoginUserDetailsDataViewModal();

            try
            {
                if (userid > 0)
                {
                    TblUser tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.UserId == userid);
                    if (tblUser != null)
                    {
                        UserDetailsDataModel userDetails = _mapper.Map<TblUser, UserDetailsDataModel>(tblUser);

                        if (userDetails != null)
                        {
                            TblServicePartner tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.UserId == tblUser.UserId);
                            TblDriverList tblDriverList = _driverListRepository.GetSingle(x => x.IsActive == true && x.UserId == tblUser.UserId);
                            if (tblServicePartner != null && tblDriverList != null)
                            {
                                LGCUserViewDataModel lGCUserViewDataModel = _mapper.Map<TblServicePartner, LGCUserViewDataModel>(tblServicePartner);

                                lGCUserViewDataModel.ServicePartnerFirstName = tblServicePartner.ServicePartnerFirstName;
                                lGCUserViewDataModel.ServicePartnerLastName = tblServicePartner.ServicePartnerLastName;
                                if (lGCUserViewDataModel?.ServicePartnerId > 0)
                                {
                                    lGCUserViewDataModel.NumofVehicle = _vehicleListRepository.GetList(x => x.IsActive == true && x.IsApproved == true && x.ApprovedBy == tblServicePartner.UserId).Count();
                                    UpdateServicePartnerImagePaths(lGCUserViewDataModel);
                                    loginUserDetailsDataViewModal.userDetails = userDetails;
                                    loginUserDetailsDataViewModal.servicePatnerDetails = lGCUserViewDataModel;

                                }
                                DriverResponseViewModal driverResponseViewModal = _mapper.Map<TblDriverList, DriverResponseViewModal>(tblDriverList);

                                if (driverResponseViewModal?.DriverId > 0)
                                {

                                    UpdateDriverImagePaths(driverResponseViewModal);
                                    loginUserDetailsDataViewModal.userDetails = userDetails;
                                    loginUserDetailsDataViewModal.driverDetails = driverResponseViewModal;
                                }
                                responseResult.Data = loginUserDetailsDataViewModal;
                                responseResult.message = "Success";
                                responseResult.Status_Code = HttpStatusCode.OK;
                                responseResult.Status = true;
                                return responseResult;
                            }
                            if (tblServicePartner != null)
                            {

                                LGCUserViewDataModel lGCUserViewDataModel = _mapper.Map<TblServicePartner, LGCUserViewDataModel>(tblServicePartner);

                                if (lGCUserViewDataModel?.ServicePartnerId > 0)
                                {
                                    lGCUserViewDataModel.NumofVehicle = _vehicleListRepository.GetList(x => x.IsActive == true && x.IsApproved == true && x.ApprovedBy == tblServicePartner.UserId).Count();
                                    UpdateServicePartnerImagePaths(lGCUserViewDataModel);
                                    loginUserDetailsDataViewModal.userDetails = userDetails;
                                    loginUserDetailsDataViewModal.servicePatnerDetails = lGCUserViewDataModel;

                                    responseResult.Data = loginUserDetailsDataViewModal;
                                    responseResult.message = "Success";
                                    responseResult.Status_Code = HttpStatusCode.OK;
                                    responseResult.Status = true;
                                    return responseResult;
                                }
                                else
                                {
                                    responseResult.message = "Not Success, error occurs while mapping the servicePartner data";
                                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                                    responseResult.Status = false;
                                    return responseResult;
                                }
                            }
                            else
                            {
                                if (tblDriverList != null)
                                {
                                    DriverResponseViewModal driverResponseViewModal = _mapper.Map<TblDriverList, DriverResponseViewModal>(tblDriverList);

                                    if (driverResponseViewModal?.DriverId > 0)
                                    {

                                        UpdateDriverImagePaths(driverResponseViewModal);
                                        loginUserDetailsDataViewModal.userDetails = userDetails;
                                        loginUserDetailsDataViewModal.driverDetails = driverResponseViewModal;

                                        responseResult.Data = loginUserDetailsDataViewModal;
                                        responseResult.message = "Success";
                                        responseResult.Status_Code = HttpStatusCode.OK;
                                        responseResult.Status = true;
                                        return responseResult;
                                    }
                                    else
                                    {
                                        responseResult.message = "Not Success, error occurs while mapping the Driver data";
                                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                                        responseResult.Status = false;
                                        return responseResult;
                                    }
                                }
                                else
                                {
                                    responseResult.message = "Not Success, error occurs while mapping  the user data";
                                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                                    responseResult.Status = false;
                                    return responseResult;
                                }
                            }
                        }
                        else
                        {
                            responseResult.message = "Not Success, User Not Found";
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            responseResult.Status = false;
                        }
                    }
                    else
                    {
                        responseResult.message = "Invalid Id";
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        responseResult.Status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("SevicePartnerManager", "ServicePartnerDetails", ex);
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                return responseResult;
            }
            return responseResult;
        }
        private void UpdateServicePartnerImagePaths(LGCUserViewDataModel model)
        {
            if (!string.IsNullOrEmpty(model.ServicePartnerAadharfrontImage))
            {
                model.ServicePartnerAadharfrontImage = GetImagePath(_baseConfig.Value.ServicePartnerAadhar, model.ServicePartnerAadharfrontImage);
            }

            if (!string.IsNullOrEmpty(model.ServicePartnerAadharBackImage))
            {
                model.ServicePartnerAadharBackImage = GetImagePath(_baseConfig.Value.ServicePartnerAadhar, model.ServicePartnerAadharBackImage);
            }

            if (!string.IsNullOrEmpty(model.ServicePartnerCancelledCheque))
            {
                model.ServicePartnerCancelledCheque = GetImagePath(_baseConfig.Value.ServicePartnerCancelledCheque, model.ServicePartnerCancelledCheque);
            }

            if (!string.IsNullOrEmpty(model.ServicePartnerGstregisteration))
            {
                model.ServicePartnerGstregisteration = GetImagePath(_baseConfig.Value.ServicePartnerGST, model.ServicePartnerGstregisteration);
            }
            if (!string.IsNullOrEmpty(model.ServicePartnerProfilePic))
            {
                model.ServicePartnerProfilePic = GetImagePath(_baseConfig.Value.ServicePartnerProfilePic, model.ServicePartnerProfilePic);
            }
        }
        private void UpdateDriverImagePaths(DriverResponseViewModal model)
        {

            if (!string.IsNullOrEmpty(model.DriverlicenseImage))
            {
                model.DriverlicenseImage = GetImagePath(_baseConfig.Value.DriverlicenseImage, model.DriverlicenseImage);
            }

            //if (!string.IsNullOrEmpty(model.VehiclefitnessCertificate))
            //{
            //    model.VehiclefitnessCertificate = GetImagePath(_baseConfig.Value.VehiclefitnessCertificate, model.VehiclefitnessCertificate);
            //}

            //if (!string.IsNullOrEmpty(model.VehicleInsuranceCertificate))
            //{
            //    model.VehicleInsuranceCertificate = GetImagePath(_baseConfig.Value.VehicleInsuranceCertificate, model.VehicleInsuranceCertificate);
            //}
            //if (!string.IsNullOrEmpty(model.VehicleRcCertificate))
            //{
            //    model.VehicleRcCertificate = GetImagePath(_baseConfig.Value.VehicleRcCertificate, model.VehicleRcCertificate);
            //}
            if (!string.IsNullOrEmpty(model.ProfilePicture))
            {
                model.ProfilePicture = GetImagePath(_baseConfig.Value.ProfilePicture, model.ProfilePicture);
            }
        }
        private string GetImagePath(string basePath, string imageName)
        {
            return string.IsNullOrEmpty(imageName) ? string.Empty : $"{_baseConfig.Value.PostERPImagePath}{basePath}{imageName}";
        }
        #endregion

        #region GetOrderListById  assign to ServicePartner
        /// <summary>
        /// GetOrderListById  assign to ServicePartner
        /// added by ashwin
        /// </summary>
        /// <param name="id"></param>
        /// <returns>responseResult</returns>
        public ResponseResult GetOrderListById(int id, int? status, int? page, int? pageSize, string? CityName, int? DriverId)
        {

            try
            {
                TblServicePartner tblServicePartner = new TblServicePartner();
                if (id <= 0)
                {
                    return new ResponseResult
                    {
                        Status = true,
                        Status_Code = HttpStatusCode.OK,
                        message = "Invalid Id "
                    };
                }

                tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerId == id);

                if (tblServicePartner == null || tblServicePartner.ServicePartnerId <= 0)
                {
                    return new ResponseResult
                    {
                        Status = true,
                        Status_Code = HttpStatusCode.OK,
                        message = "Service Partner not found"
                    };
                }
                //get order list
                List<TblLogistic> tblLogisticObj;
                tblLogisticObj = _logisticsRepository.GetOrderListByStatus(status);
                //data filter by service partner Id
                tblLogisticObj = tblLogisticObj.Where(x => x.ServicePartnerId == tblServicePartner.ServicePartnerId).ToList();
                if (DriverId != null)
                {
                    tblLogisticObj = tblLogisticObj.Where(x => x.ServicePartnerId == tblServicePartner.ServicePartnerId && x.DriverDetailsId == DriverId).ToList();
                }
                //data filter by
                //

                if (CityName != null && CityName != string.Empty)
                {
                    tblLogisticObj = tblLogisticObj.Where(x => x.OrderTrans.Exchange != null && x.OrderTrans?.Exchange?.CustomerDetails?.City.ToLower() == CityName.ToLower().Trim() || x.OrderTrans.Abbredemption != null && x.OrderTrans?.Abbredemption?.CustomerDetails?.City.ToLower() == CityName.ToLower().Trim()).ToList();

                }
                if (page.HasValue && pageSize.HasValue && page > 0 && pageSize > 0)
                {
                    //tblLogisticObj = tblLogisticObj.OrderByDescending(x=>x.ModifiedDate).ThenByDescending(x=>x.CreatedDate).ToList();
                    tblLogisticObj = tblLogisticObj
                        .OrderByDescending(x => x.PickupScheduleDate)
                       .Skip((page.Value - 1) * pageSize.Value)
                       .Take(pageSize.Value)
                       .ToList();
                }
                else
                {
                    tblLogisticObj = tblLogisticObj.ToList();
                }

                if (tblLogisticObj == null || tblLogisticObj.Count == 0)
                {
                    return new ResponseResult
                    {
                        Status = true,
                        Status_Code = HttpStatusCode.OK,
                        message = "No data found"
                    };
                }

                List<AllOrderlistViewModel> orderListsViewModal = new List<AllOrderlistViewModel>();

                foreach (TblLogistic item in tblLogisticObj)
                {
                    TblLogistic? tbllogistic = _logisticsRepository.GetExchangeDetailsByRegdno(item.RegdNo);
                    TblWalletTransaction? tblWalletTransaction = _logisticsRepository.GetEvcDetailsByOrdertranshId((int)item.OrderTransId);


                    if (tbllogistic != null)
                    {
                        AllOrderlistViewModel orderDetail = new AllOrderlistViewModel
                        {
                            OrderTransId = tbllogistic?.OrderTransId,
                            RegdNo = tbllogistic?.OrderTrans?.RegdNo,
                            StatusDesc = tbllogistic?.Status?.StatusDescription,
                            StatusId = tbllogistic?.StatusId,
                            AmtPaybleThroughLGC = (int?)(tbllogistic?.AmtPaybleThroughLgc ?? 0),
                            TicketNumber = tbllogistic?.TicketNumber,
                            PickupScheduleDate = tbllogistic?.PickupScheduleDate?.ToString(),
                        };
                        if (tbllogistic?.OrderTrans?.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                        {
                            orderDetail.ProductCategory = tbllogistic.OrderTrans?.Exchange?.ProductType?.ProductCat?.Description;
                            orderDetail.ProductType = tbllogistic.OrderTrans?.Exchange?.ProductType?.DescriptionForAbb;
                            orderDetail.CustomerName = $"{tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.FirstName} {tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.LastName}";
                            orderDetail.FirstName = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.FirstName;
                            orderDetail.LastName = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.LastName;
                            orderDetail.Email = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.Email;
                            orderDetail.City = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.City;
                            orderDetail.ZipCode = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.ZipCode;
                            orderDetail.Address1 = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.Address1;
                            orderDetail.Address2 = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.Address2 ?? "";
                            orderDetail.PhoneNumber = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.PhoneNumber;
                            orderDetail.State = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.State;
                            orderDetail.IsDefferedSettlement = tbllogistic?.OrderTrans?.Exchange?.IsDefferedSettlement == true ? true : false;
                        }

                        if (tbllogistic?.OrderTrans?.OrderType == Convert.ToInt32(OrderTypeEnum.ABB))
                        {
                            orderDetail.ProductCategory = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.NewProductCategory?.Description;
                            orderDetail.ProductType = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation?.Description;
                            orderDetail.CustomerName = $"{tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustFirstName} {tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustLastName}";
                            orderDetail.FirstName = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustFirstName;
                            orderDetail.LastName = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustLastName;
                            orderDetail.Email = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustEmail;
                            orderDetail.City = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustCity;
                            orderDetail.ZipCode = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustPinCode;
                            orderDetail.Address1 = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustAddress1;
                            orderDetail.Address2 = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustAddress2 ?? "";
                            orderDetail.PhoneNumber = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustMobile;
                            orderDetail.State = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustState;
                            orderDetail.IsDefferedSettlement = tbllogistic?.OrderTrans?.Abbredemption?.IsDefferedSettelment == true ? true : false;

                        }

                        if (tblWalletTransaction != null && tblWalletTransaction.Evcregistration != null)
                        {
                            orderDetail.EvcData = new EVCResellerModel
                            {
                                EvcregistrationId = tblWalletTransaction?.Evcregistration?.EvcregistrationId,
                                EvcPartnerId = tblWalletTransaction?.Evcpartner?.EvcPartnerId,
                                EVCStoreCode = tblWalletTransaction?.Evcpartner?.EvcStoreCode,
                                BussinessName = tblWalletTransaction?.Evcregistration?.BussinessName,
                                ContactPerson = tblWalletTransaction?.Evcregistration?.ContactPerson,
                                EvcmobileNumber = tblWalletTransaction?.Evcpartner?.ContactNumber,
                                AlternateMobileNumber = tblWalletTransaction?.Evcregistration?.EvcmobileNumber ?? "",
                                EmailId = tblWalletTransaction?.Evcpartner?.EmailId,
                                RegdAddressLine1 = tblWalletTransaction?.Evcpartner?.Address1,
                                RegdAddressLine2 = tblWalletTransaction?.Evcpartner?.Address2,
                                City = tblWalletTransaction?.Evcpartner?.City?.Name,
                                State = tblWalletTransaction?.Evcpartner?.State?.Name,
                                PinCode = tblWalletTransaction?.Evcpartner?.PinCode
                            };
                        }

                        if (tbllogistic?.DriverDetailsId > 0)
                        {
                            TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetDriverDetailsById((int)tbllogistic.DriverDetailsId);

                            orderDetail.DriverData = new DriverDetailsModel
                            {
                                DriverDetailsId = tblDriverDetail?.DriverDetailsId,
                                DriverName = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverName : tblDriverDetail?.Driver?.DriverName,
                                DriverPhoneNumber = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverPhoneNumber : tblDriverDetail?.Driver?.DriverPhoneNumber,
                                VehicleNumber = tblDriverDetail?.Vehicle == null ? tblDriverDetail?.VehicleNumber : tblDriverDetail?.Vehicle?.VehicleNumber,
                                City = tblDriverDetail?.Driver == null ? tblDriverDetail?.City : tblDriverDetail?.Driver?.City?.Name,
                                VehicleRcNumber = tblDriverDetail?.Vehicle == null ? tblDriverDetail?.VehicleRcNumber : tblDriverDetail?.Vehicle?.VehicleRcNumber,
                                DriverlicenseNumber = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverlicenseNumber : tblDriverDetail?.Driver?.DriverLicenseNumber,
                                CityId = tblDriverDetail?.Driver == null ? tblDriverDetail?.CityId : tblDriverDetail?.Driver?.CityId,
                                ServicePartnerId = tblDriverDetail?.ServicePartnerId,
                                CreatedDate = tblDriverDetail?.CreatedDate,
                                ModifiedDate = tblDriverDetail?.ModifiedDate,
                                DriverId = tblDriverDetail?.DriverId,
                                VehicleId = tblDriverDetail?.VehicleId,
                                JourneyPlannedDate = (tblDriverDetail?.JourneyPlanDate != null ? (DateTime)tblDriverDetail?.JourneyPlanDate : DateTime.MinValue),
                                ProfilePicture = tblDriverDetail?.Driver != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Driver?.ProfilePicture : string.Empty,
                                DriverlicenseImage = tblDriverDetail?.Driver != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Driver?.DriverLicenseImage : string.Empty,
                                VehicleInsuranceCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehicleInsuranceCertificate : string.Empty,
                                VehiclefitnessCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehiclefitnessCertificate : string.Empty,
                                VehicleRcCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehicleRcCertificate : string.Empty,
                            };
                        }
                        orderListsViewModal.Add(orderDetail);
                    }
                }

                if (orderListsViewModal.Count > 0)
                {
                    AllOrderList allOrderList = new AllOrderList
                    {
                        AllOrderlistViewModels = orderListsViewModal
                    };

                    return new ResponseResult
                    {
                        Status = true,
                        Status_Code = HttpStatusCode.OK,
                        Data = allOrderList,
                        message = "Success"
                    };
                }
                else
                {
                    return new ResponseResult
                    {
                        Status = true,
                        Status_Code = HttpStatusCode.OK,
                        message = "No data found"
                    };
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ServicePartnerManager", "GetOrderListById", ex);

                return new ResponseResult
                {
                    Status = false,
                    Status_Code = HttpStatusCode.InternalServerError,
                    message = ex.Message
                };
            }
        }

        #endregion

        #region Order Assign to Vehicle by ServicePartner/LGC
        /// <summary>
        /// Api method for Order Assign to Vehicle by ServicePartner/LGC
        /// Added by Priyanshi
        /// </summary>
        /// <param name="">username</param>
        /// <param name="">regdno</param>
        /// <returns></returns>
        public ResponseResult OrderAssignLGCtoDriverid(int OrdertransId, int LGCId, int DriverDetailId)
        {
            TblLogistic tblLogistic = null;
            int OrderStatusId = Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner);
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            TblServicePartner? tblServicePartner = null;
            TblOrderLgc? tblOrderLgc = null;
            TblExchangeOrder? tblExchangeOrder = null;
            TblExchangeAbbstatusHistory? tblExchangeAbbstatusHistory = null;
            AssignOrderfailedRespone? assignOrderfailedRespone = new AssignOrderfailedRespone();
            TblOrderTran? tblOrderTran = null;
            #region Variables for (ABB or Exchange)
            TblAbbredemption tblAbbredemption = null;
            TblAbbregistration tblAbbregistration = null; int orderType = 0;
            string regdNo = null; int? customerDetailsId = null; string sponsorOrderNumber = null;
            #endregion
            try
            {
                tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerId == LGCId);
                if (tblServicePartner != null && tblServicePartner.ServicePartnerId > 0 && OrdertransId > 0)
                {
                    tblLogistic = _logisticsRepository.GetSingle(x => x.OrderTransId == OrdertransId && x.IsActive == true && x.ServicePartnerId == LGCId && (x.StatusId == Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated) || x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderRejectedbyVehicle)));

                    if (tblLogistic != null)
                    {
                        tblOrderTran = _orderTransRepository.GetOrderDetailsByOrderTransId(OrdertransId);

                        if (tblOrderTran != null && tblOrderTran.OrderTransId > 0)
                        {
                            orderType = (int)tblOrderTran.OrderType;
                            if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                            {
                                tblExchangeOrder = tblOrderTran.Exchange;
                                if (tblExchangeOrder != null)
                                {
                                    regdNo = tblExchangeOrder.RegdNo;
                                    customerDetailsId = tblExchangeOrder.CustomerDetailsId;
                                    sponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                                }
                            }
                            else if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.ABB))
                            {
                                tblAbbredemption = tblOrderTran.Abbredemption;
                                if (tblAbbredemption != null)
                                {
                                    customerDetailsId = tblAbbredemption.CustomerDetailsId;
                                    tblAbbregistration = tblAbbredemption.Abbregistration;
                                    if (tblAbbregistration != null)
                                    {
                                        regdNo = tblAbbregistration.RegdNo;
                                        sponsorOrderNumber = tblAbbregistration.SponsorOrderNo;
                                    }
                                }
                            }



                            #region update statusId in Base tbl Exchange or ABB
                            if (tblExchangeOrder != null)
                            {
                                tblExchangeOrder.StatusId = OrderStatusId;
                                tblExchangeOrder.OrderStatus = "Vehicle Assign by Service Partner";
                                tblExchangeOrder.ModifiedBy = tblServicePartner.UserId;
                                tblExchangeOrder.ModifiedDate = _currentDatetime;
                                _exchangeOrderRepository.Update(tblExchangeOrder);
                                _exchangeOrderRepository.SaveChanges();
                            }
                            else if (tblAbbredemption != null && tblAbbregistration != null)
                            {
                                #region update status on tblAbbRedemption
                                tblAbbredemption.StatusId = OrderStatusId;
                                tblAbbredemption.AbbredemptionStatus = "Vehicle Assign by Service Partner";
                                tblAbbredemption.ModifiedBy = tblServicePartner.UserId;
                                tblAbbredemption.ModifiedDate = _currentDatetime;
                                _abbRedemptionRepository.Update(tblAbbredemption);
                                _abbRedemptionRepository.SaveChanges();
                                #endregion
                            }
                            #endregion

                            #region update statusId in tblOrderTrans
                            tblOrderTran.StatusId = OrderStatusId;
                            tblOrderTran.ModifiedBy = tblServicePartner.UserId;
                            tblOrderTran.ModifiedDate = _currentDatetime;
                            _orderTransRepository.Update(tblOrderTran);
                            _orderTransRepository.SaveChanges();
                            #endregion

                            #region update statusid in tbllogistics
                            tblLogistic.OrderTransId = OrdertransId;
                            tblLogistic.StatusId = OrderStatusId;
                            tblLogistic.IsActive = true;
                            tblLogistic.DriverDetailsId = DriverDetailId;
                            tblLogistic.Modifiedby = tblServicePartner.UserId;
                            tblLogistic.ModifiedDate = _currentDatetime;
                            var Logisticresult = _logisticsRepository.UpdateLogiticStatus(tblLogistic);

                            #endregion

                            #region Update StatusId in TblWalletTransection 
                            var walletTransactionresult = _walletTransactionRepository.UpdateWalletTransStatus(OrdertransId, OrderStatusId, tblServicePartner.UserId);
                            #endregion

                            #region Update TBLOrderLGC
                            if (tblOrderLgc != null)
                            {
                                //used for LGC Drop
                                tblOrderLgc.StatusId = OrderStatusId;
                                tblOrderLgc.ModifiedBy = tblServicePartner.UserId;
                                tblOrderLgc.ModifiedDate = _currentDatetime;
                                _orderLGCRepository.Update(tblOrderLgc);
                                _orderLGCRepository.SaveChanges();
                            }
                            #endregion

                            #region Insert into tblexchangeabbhistory
                            tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                            tblExchangeAbbstatusHistory.OrderType = orderType;
                            tblExchangeAbbstatusHistory.SponsorOrderNumber = sponsorOrderNumber;
                            tblExchangeAbbstatusHistory.RegdNo = tblOrderTran.RegdNo;
                            //tblExchangeAbbstatusHistory.ZohoSponsorId = tblExchangeOrder.ZohoSponsorOrderId != null ? tblExchangeOrder.ZohoSponsorOrderId : string.Empty; ;
                            tblExchangeAbbstatusHistory.CustId = customerDetailsId;
                            tblExchangeAbbstatusHistory.StatusId = OrderStatusId;
                            tblExchangeAbbstatusHistory.IsActive = true;
                            tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                            tblExchangeAbbstatusHistory.CreatedBy = tblServicePartner.UserId;
                            tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                            tblExchangeAbbstatusHistory.ServicepartnerId = LGCId;
                            tblExchangeAbbstatusHistory.DriverDetailId = DriverDetailId;
                            _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);

                            #endregion

                            responseMessage.message = "Assign Order Successfully";
                            responseMessage.Status = true;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                        }
                        else
                        {
                            responseMessage.message = "OrderTransId Not Found for RegdNo";
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseMessage.message = "Invalid RegdNo";
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseMessage.message = "Not Valid login credential";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                }
                if (!responseMessage.Status)
                {
                    assignOrderfailedRespone.OrdertransId = OrdertransId;
                    responseMessage.Data = assignOrderfailedRespone;
                }

            }

            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("LogisticManager", "RejectOrderbyLGC", ex);
            }
            return responseMessage;
        }
        #endregion

        #region Order Reject by ServicePartner/LGC
        /// <summary>
        /// Api method for Reject Order by ServicePartner/LGC
        /// Added by Priyanshi
        /// </summary>
        /// <param name="">OrdertransId</param>
        /// <param name="">LGCId</param>
        /// <param name="">commentbyLGC</param>
        /// <returns></returns>
        public ResponseResult RejectOrderbyLGC(int OrdertransId, int LGCId, string commentbyLGC)
        {
            #region Variable Declaration
            TblExchangeOrder? tblExchangeOrder = null;
            TblExchangeAbbstatusHistory? tblExchangeAbbstatusHistory = null;
            TblWalletTransaction? tblWalletTransaction = null;
            TblOrderTran? tblOrderTrans = null;
            TblLogistic? tblLogistic = null;
            TblServicePartner? tblServicePartner = null;
            //TblLogistic tblLogistic = null;
            // TblOrderLgc tblOrderLGC = null;
            int statusId = Convert.ToInt32(OrderStatusEnum.PickupTicketcancellationbyUTC);
            bool flag = false;
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;

            #endregion

            #region Variables for (ABB or Exchange)
            TblAbbredemption? tblAbbredemption = null;
            TblAbbregistration? tblAbbregistration = null; int orderType = 0;
            string? regdNo = null; int? customerDetailsId = null; string? sponsorOrderNumber = null;
            #endregion
            try
            {
                tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerId == LGCId);

                if (tblServicePartner != null && tblServicePartner.ServicePartnerId > 0 && OrdertransId > 0)
                {
                    var a = Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated);
                    tblLogistic = _logisticsRepository.GetSingle(x => x.OrderTransId == OrdertransId && x.IsActive == true && x.ServicePartnerId == LGCId && (x.StatusId == Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated) || x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderRejectedbyVehicle)));

                    if (tblLogistic != null)
                    {

                        #region Common Implementations for (ABB or Exchange)
                        tblOrderTrans = _orderTransRepository.GetOrderDetailsByOrderTransId(OrdertransId);
                        if (tblOrderTrans != null)
                        {
                            orderType = (int)tblOrderTrans.OrderType;
                            if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                            {
                                tblExchangeOrder = tblOrderTrans.Exchange;
                                if (tblExchangeOrder != null)
                                {
                                    regdNo = tblExchangeOrder.RegdNo;
                                    customerDetailsId = tblExchangeOrder.CustomerDetailsId;
                                    sponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                                }
                            }
                            else if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.ABB))
                            {
                                tblAbbredemption = tblOrderTrans.Abbredemption;
                                if (tblAbbredemption != null)
                                {
                                    customerDetailsId = tblAbbredemption.CustomerDetailsId;
                                    tblAbbregistration = tblAbbredemption.Abbregistration;
                                    if (tblAbbregistration != null)
                                    {
                                        regdNo = tblAbbregistration.RegdNo;
                                        sponsorOrderNumber = tblAbbregistration.SponsorOrderNo;
                                    }
                                }
                            }

                            #endregion


                            #region update statusid in tbllogistics
                            tblLogistic.StatusId = statusId;
                            //Priyanshi for use (LGC Reopen Task)
                            tblLogistic.IsActive = false;
                            tblLogistic.RescheduleComment = commentbyLGC;
                            tblLogistic.Modifiedby = tblServicePartner.UserId;
                            tblLogistic.ModifiedDate = _currentDatetime;
                            _logisticsRepository.Update(tblLogistic);
                            _logisticsRepository.SaveChanges();
                            #endregion

                            #region update statusId in tblOrderTrans
                            tblOrderTrans.StatusId = statusId;
                            tblOrderTrans.ModifiedBy = tblServicePartner.UserId;
                            tblOrderTrans.ModifiedDate = _currentDatetime;
                            _orderTransRepository.Update(tblOrderTrans);
                            _orderTransRepository.SaveChanges();
                            #endregion

                            #region update statusId in Base tbl Exchange or ABB
                            if (tblExchangeOrder != null)
                            {
                                tblExchangeOrder.StatusId = statusId;
                                tblExchangeOrder.OrderStatus = "Pickup Ticket cancellation by UTC";
                                tblExchangeOrder.ModifiedBy = tblServicePartner.UserId;
                                tblExchangeOrder.ModifiedDate = _currentDatetime;
                                _exchangeOrderRepository.Update(tblExchangeOrder);
                                _exchangeOrderRepository.SaveChanges();
                            }
                            else if (tblAbbredemption != null && tblAbbregistration != null)
                            {
                                #region update status on tblAbbRedemption
                                tblAbbredemption.StatusId = statusId;
                                tblAbbredemption.AbbredemptionStatus = "Pickup Ticket cancellation by UTC";
                                tblAbbredemption.ModifiedBy = tblServicePartner.UserId;
                                tblAbbredemption.ModifiedDate = _currentDatetime;
                                _abbRedemptionRepository.Update(tblAbbredemption);
                                _abbRedemptionRepository.SaveChanges();
                                #endregion
                            }
                            #endregion

                            #region Insert into tblexchangeabbhistory
                            tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                            tblExchangeAbbstatusHistory.OrderType = orderType;
                            tblExchangeAbbstatusHistory.SponsorOrderNumber = sponsorOrderNumber;
                            tblExchangeAbbstatusHistory.RegdNo = regdNo;
                            tblExchangeAbbstatusHistory.CustId = customerDetailsId;
                            tblExchangeAbbstatusHistory.StatusId = statusId;
                            tblExchangeAbbstatusHistory.IsActive = true;
                            tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                            tblExchangeAbbstatusHistory.CreatedBy = tblServicePartner.UserId;
                            tblExchangeAbbstatusHistory.OrderTransId = OrdertransId;
                            tblExchangeAbbstatusHistory.Comment = commentbyLGC != null ? "TicketNo-" + tblLogistic.TicketNumber + "- Service Partner Name -" + tblServicePartner.ServicePartnerDescription + "Comment -" + commentbyLGC : string.Empty;
                            _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                            #endregion

                            #region Update StatusId in Tbl WalletTransection 
                            tblWalletTransaction = _walletTransactionRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == OrdertransId);
                            if (tblWalletTransaction != null)
                            {
                                tblWalletTransaction.StatusId = statusId.ToString();
                                tblWalletTransaction.ModifiedBy = tblServicePartner.UserId;
                                tblWalletTransaction.ModifiedDate = _currentDatetime;
                                _walletTransactionRepository.Update(tblWalletTransaction);
                                _walletTransactionRepository.SaveChanges();
                            }
                            #endregion

                            responseMessage.message = "Reject Order Succesfully";
                            responseMessage.Status = true;
                            responseMessage.Status_Code = HttpStatusCode.OK;

                        }
                        else
                        {
                            responseMessage.message = "OrderTransId Not Found for RegdNo";
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseMessage.message = "Invalid RegdNo";
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseMessage.message = "Not Valid login credential";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                }

            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("LogisticManager", "RejectOrderbyLGC", ex);
            }
            return responseMessage;
        }
        #endregion

        #region GetAllOrderDetailsByOTId  assign to ServicePartner
        /// <summary>
        /// GetAllOrderDetailsByOTId  by OrdertransId
        /// added by priyanshi
        /// </summary>
        /// <param name="OrdertransId"></param>
        /// <returns>responseResult</returns>
        public ResponseResult GetOrderDetailsByOTId(int orderTransId)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            BrandViewModel? brandVM = null;
            try
            {
                TblOrderTran tblOrderTran = _orderTransRepository.GetSingleOrderWithExchangereference(orderTransId);
                TblLogistic tblLogistic = _logisticsRepository.GetExchangeDetailsByOrdertransId(orderTransId);
                TblWalletTransaction tblWalletTransaction = _logisticsRepository.GetEvcDetailsByOrdertranshId(orderTransId);
                string url = _baseConfig.Value.PostERPImagePath;

                if (tblOrderTran == null)
                {
                    responseMessage.message = "Order details not found.";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }

                OrderDetail orderDetail = new OrderDetail();
                orderDetail.RegdNo = tblOrderTran.RegdNo;
                if (tblLogistic != null)
                {
                    orderDetail.TicketNumber = tblLogistic.TicketNumber;
                    orderDetail.PickupScheduleDate = tblLogistic.PickupScheduleDate?.ToString();
                    orderDetail.AmtPaybleThroughLGC = (int)tblLogistic.AmtPaybleThroughLgc;

                }
                #region For Exchange
                if (tblOrderTran.Exchange != null)
                {

                    orderDetail.ProductCategory = tblOrderTran.Exchange.ProductType?.ProductCat?.Description;
                    orderDetail.ProductType = tblOrderTran.Exchange.ProductType?.DescriptionForAbb;
                    orderDetail.SponsorName = tblOrderTran.Exchange.CompanyName;
                    orderDetail.BrandName = tblOrderTran.Exchange.Brand?.Name;
                    orderDetail.OrderTransId = tblOrderTran.OrderTransId;
                    orderDetail.CreatedDate = tblOrderTran.CreatedDate?.ToString();
                    orderDetail.CustomerName = $"{tblOrderTran.Exchange.CustomerDetails?.FirstName} {tblOrderTran.Exchange.CustomerDetails?.LastName}";
                    orderDetail.IsDefferedSettlement = tblOrderTran.Exchange.IsDefferedSettlement == true ? true : false;


                    if (tblOrderTran.Exchange.CustomerDetails != null)
                    {
                        orderDetail.customerData = new customerDetailsviewModel
                        {
                            FirstName = tblOrderTran.Exchange.CustomerDetails.FirstName,
                            LastName = tblOrderTran.Exchange.CustomerDetails.LastName,
                            Email = tblOrderTran.Exchange.CustomerDetails.Email,
                            PhoneNumber = tblOrderTran.Exchange.CustomerDetails.PhoneNumber,
                            Address1 = tblOrderTran.Exchange.CustomerDetails.Address1,
                            Address2 = tblOrderTran.Exchange.CustomerDetails.Address2 ?? "",
                            City = tblOrderTran.Exchange.CustomerDetails.City,
                            State = tblOrderTran.Exchange.CustomerDetails.State ?? "",
                            ZipCode = tblOrderTran.Exchange.CustomerDetails.ZipCode
                        };
                    }
                }
                #endregion

                #region For ABB
                if (tblOrderTran.Abbredemption != null)
                {

                    orderDetail.ProductCategory = tblOrderTran.Abbredemption?.Abbregistration?.NewProductCategory?.Description;
                    orderDetail.ProductType = tblOrderTran.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation?.Description;
                    orderDetail.SponsorName = tblOrderTran.Abbredemption?.Abbregistration?.BusinessUnit?.Name;

                    orderDetail.OrderTransId = tblOrderTran.OrderTransId;
                    orderDetail.CreatedDate = tblOrderTran.CreatedDate?.ToString();
                    orderDetail.CustomerName = $"{tblOrderTran.Abbredemption.CustomerDetails?.FirstName} {tblOrderTran.Abbredemption.CustomerDetails?.LastName}";
                    orderDetail.IsDefferedSettlement = tblOrderTran.Abbredemption.IsDefferedSettelment == true ? true : false;

                    #region Check BusinessUnit configuration

                    TblBusinessUnit tblBusinessUnit = _businessUnitRepository.GetBUDetailsByTransId(tblOrderTran.OrderTransId);
                    if (tblBusinessUnit != null)
                    {
                        int? newBrandId = tblOrderTran?.Abbredemption?.Abbregistration?.NewBrandId;
                        brandVM = _commonManager.GetAbbBrandDetailsById(tblBusinessUnit.IsBumultiBrand, newBrandId);
                        orderDetail.BrandName = brandVM?.Name;
                    }
                    #endregion

                    if (tblOrderTran?.Abbredemption?.CustomerDetails != null)
                    {
                        orderDetail.customerData = new customerDetailsviewModel
                        {
                            FirstName = tblOrderTran.Abbredemption.CustomerDetails.FirstName,
                            LastName = tblOrderTran.Abbredemption.CustomerDetails.LastName,
                            Email = tblOrderTran.Abbredemption.CustomerDetails.Email,
                            PhoneNumber = tblOrderTran.Abbredemption.CustomerDetails.PhoneNumber,
                            Address1 = tblOrderTran.Abbredemption.CustomerDetails.Address1,
                            Address2 = tblOrderTran.Abbredemption.CustomerDetails.Address2 ?? "",
                            City = tblOrderTran.Abbredemption.CustomerDetails.City,
                            State = tblOrderTran.Abbredemption.CustomerDetails.State ?? "",
                            ZipCode = tblOrderTran.Abbredemption.CustomerDetails.ZipCode
                        };
                    }
                }
                #endregion


                if (tblWalletTransaction != null && tblWalletTransaction.Evcregistration != null)
                {
                    orderDetail.EvcData = new EVCResellerModel
                    {
                        EvcregistrationId = tblWalletTransaction.Evcregistration.EvcregistrationId,
                        EvcPartnerId = tblWalletTransaction.Evcpartner.EvcPartnerId,
                        EVCStoreCode = tblWalletTransaction.Evcpartner.EvcStoreCode,
                        BussinessName = tblWalletTransaction.Evcregistration.BussinessName,
                        ContactPerson = tblWalletTransaction.Evcregistration.ContactPerson,
                        EvcmobileNumber = tblWalletTransaction.Evcpartner.ContactNumber,
                        AlternateMobileNumber = tblWalletTransaction.Evcregistration.EvcmobileNumber ?? "",
                        EmailId = tblWalletTransaction.Evcpartner.EmailId,
                        RegdAddressLine1 = tblWalletTransaction.Evcpartner.Address1,
                        RegdAddressLine2 = tblWalletTransaction.Evcpartner.Address2,
                        City = tblWalletTransaction.Evcpartner.City?.Name,
                        State = tblWalletTransaction.Evcpartner.State?.Name,
                        PinCode = tblWalletTransaction.Evcpartner.PinCode
                    };
                }

                if (tblLogistic?.DriverDetailsId > 0)
                {
                    TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetDriverDetailsById((int)tblLogistic.DriverDetailsId);

                    orderDetail.DriverData = new DriverDetailsModel
                    {


                        DriverDetailsId = tblDriverDetail?.DriverDetailsId,
                        DriverName = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverName : tblDriverDetail?.Driver?.DriverName,
                        DriverPhoneNumber = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverPhoneNumber : tblDriverDetail?.Driver?.DriverPhoneNumber,
                        VehicleNumber = tblDriverDetail?.Vehicle == null ? tblDriverDetail?.VehicleNumber : tblDriverDetail?.Vehicle?.VehicleNumber,
                        City = tblDriverDetail?.Driver == null ? tblDriverDetail?.City : tblDriverDetail?.Driver?.City?.Name,
                        VehicleRcNumber = tblDriverDetail?.Vehicle == null ? tblDriverDetail?.VehicleRcNumber : tblDriverDetail?.Vehicle?.VehicleRcNumber,
                        DriverlicenseNumber = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverlicenseNumber : tblDriverDetail?.Driver?.DriverLicenseNumber,
                        CityId = tblDriverDetail?.Driver == null ? tblDriverDetail?.CityId : tblDriverDetail?.Driver?.CityId,
                        ServicePartnerId = tblDriverDetail?.ServicePartnerId,
                        CreatedDate = tblDriverDetail?.CreatedDate,
                        ModifiedDate = tblDriverDetail?.ModifiedDate,
                        DriverId = tblDriverDetail?.DriverId,
                        VehicleId = tblDriverDetail?.VehicleId,
                        JourneyPlannedDate = (tblDriverDetail?.JourneyPlanDate != null ? (DateTime)tblDriverDetail?.JourneyPlanDate : DateTime.MinValue),
                        ProfilePicture = tblDriverDetail?.Driver != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Driver?.ProfilePicture : string.Empty,
                        DriverlicenseImage = tblDriverDetail?.Driver != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Driver?.DriverLicenseImage : string.Empty,
                        VehicleInsuranceCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehicleInsuranceCertificate : string.Empty,
                        VehiclefitnessCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehiclefitnessCertificate : string.Empty,
                        VehicleRcCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehicleRcCertificate : string.Empty,

                    };
                }

                List<OrderImageUploadViewModel> orderPickupImages = new List<OrderImageUploadViewModel>();
                List<OrderImageUploadViewModel> orderDropImages = new List<OrderImageUploadViewModel>();
                List<OrderImageUploadViewModel> orderQCImages = new List<OrderImageUploadViewModel>();


                List<OrderImageUploadViewModel> orderImageUploadViewModels = new List<OrderImageUploadViewModel>();
                if (orderTransId > 0)
                {
                    orderImageUploadViewModels = (List<OrderImageUploadViewModel>)_eVCManager.GetAllImagesByTransId(orderTransId);
                    if (orderImageUploadViewModels != null && orderImageUploadViewModels.Count > 0)
                    {
                        foreach (var item in orderImageUploadViewModels)
                        {
                            if (item.LgcpickDrop == "Pickup")
                            {
                                item.FilePath = "DBFiles/LGC/LGCPickup/";
                                item.ImageWithPath = url + item.FilePath + item.ImageName;
                                orderPickupImages.Add(item);
                            }
                            else if (item.LgcpickDrop == "Drop")
                            {
                                item.FilePath = "DBFiles/LGC/LGCDrop/";
                                item.ImageWithPath = url + item.FilePath + item.ImageName;
                                orderDropImages.Add(item);
                            }
                            else
                            {
                                item.FilePath = "DBFiles/QC/VideoQC/";
                                item.ImageWithPath = url + item.FilePath + item.ImageName;
                                orderQCImages.Add(item);
                            }
                        }
                    }
                }


                OrderList orderList = new OrderList
                {
                    orderDetail = orderDetail,
                    OrderPickupImages = orderPickupImages,
                    OrderDropImages = orderDropImages,
                    OrderQCImages = orderQCImages
                };
                responseMessage.message = "Order details retrieved successfully.";
                responseMessage.Status = true;
                responseMessage.Data = orderList;
                responseMessage.Status_Code = HttpStatusCode.OK;

            }

            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("LogisticManager", "GetOrderDetailsByOTId", ex);
            }
            return responseMessage;
        }
        #endregion

        #region StartJournyVehicleListbyServiceP  
        /// <summary>
        /// StartJournyVehicleListbyServiceP  
        /// added by Priyanshi
        /// </summary>
        /// <param name="servicePartnerId"></param>
        /// <param name="journeyDate"></param>
        /// <returns>responseResult</returns>
        public ResponseResult StartJournyVehicleListbyServiceP(int servicePartnerId, DateTime journeyDate)
        {
            ResponseResult responseResult = new ResponseResult();
            journeyDate = journeyDate.Date;

            try
            {
                if (servicePartnerId <= 0)
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    responseResult.message = "UserId not found";
                    return responseResult;
                }

                List<TblVehicleJourneyTracking> tblVehicleJourneyTracking;
                if (journeyDate != DateTime.MinValue)
                {
                    tblVehicleJourneyTracking = _journeyTrackingRepository.GetList(x => x.IsActive == true && x.JourneyPlanDate != null && Convert.ToDateTime(x.JourneyPlanDate).ToShortDateString() == journeyDate.ToShortDateString() && x.ServicePartnerId == servicePartnerId && x.JourneyStartDatetime != null && x.JourneyEndTime == null).ToList();
                }
                else
                {
                    tblVehicleJourneyTracking = _journeyTrackingRepository.GetList(x => x.IsActive == true && x.JourneyPlanDate != null && Convert.ToDateTime(x.JourneyPlanDate).ToShortDateString() == _currentDatetime.ToShortDateString() && x.ServicePartnerId == servicePartnerId && x.JourneyStartDatetime != null && x.JourneyEndTime == null).ToList();
                }

                if (tblVehicleJourneyTracking == null || tblVehicleJourneyTracking.Count == 0)
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.OK;
                    responseResult.message = "Vehicle Not found";
                    return responseResult;
                }

                List<StartJournyVehicleListbyServicePResponse> vehicleListsViewModal = new List<StartJournyVehicleListbyServicePResponse>();

                foreach (var item in tblVehicleJourneyTracking)
                {
                    StartJournyVehicleListbyServicePResponse vehicleList = new StartJournyVehicleListbyServicePResponse
                    {
                        TrackingId = item.TrackingId,
                        ServicePartnerId = item.ServicePartnerId,
                        DriverDetailsId = item.DriverId,
                        JourneyPlanDate = item.JourneyPlanDate,
                        CurrentVehicleLatt = item.CurrentVehicleLatt,
                        CurrentVehicleLong = item.CurrentVehicleLong,
                        JourneyStartDatetime = item.JourneyStartDatetime,
                        JourneyEndTime = item.JourneyEndTime

                    };
                    TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetDriverDetailsById((int)item.DriverId);

                    // TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetSingle(x => x.DriverDetailsId == item.DriverId && x.IsActive == true);
                    if (tblDriverDetail != null)
                    {
                        vehicleList.driverDetails = new DriverDetailsModel
                        {
                            DriverDetailsId = tblDriverDetail?.DriverDetailsId,
                            DriverName = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverName : tblDriverDetail?.Driver?.DriverName,
                            DriverPhoneNumber = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverPhoneNumber : tblDriverDetail?.Driver?.DriverPhoneNumber,
                            VehicleNumber = tblDriverDetail?.Vehicle == null ? tblDriverDetail?.VehicleNumber : tblDriverDetail?.Vehicle?.VehicleNumber,
                            City = tblDriverDetail?.Driver == null ? tblDriverDetail?.City : tblDriverDetail?.Driver?.City?.Name,
                            VehicleRcNumber = tblDriverDetail?.Vehicle == null ? tblDriverDetail?.VehicleRcNumber : tblDriverDetail?.Vehicle?.VehicleRcNumber,
                            DriverlicenseNumber = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverlicenseNumber : tblDriverDetail?.Driver?.DriverLicenseNumber,
                            CityId = tblDriverDetail?.Driver == null ? tblDriverDetail?.CityId : tblDriverDetail?.Driver?.CityId,
                            ServicePartnerId = tblDriverDetail?.ServicePartnerId,
                            CreatedDate = tblDriverDetail?.CreatedDate,
                            ModifiedDate = tblDriverDetail?.ModifiedDate,
                            DriverId = tblDriverDetail?.DriverId,
                            VehicleId = tblDriverDetail?.VehicleId,
                            JourneyPlannedDate = (tblDriverDetail?.JourneyPlanDate != null ? (DateTime)tblDriverDetail?.JourneyPlanDate : DateTime.MinValue),
                            ProfilePicture = tblDriverDetail?.Driver != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Driver?.ProfilePicture : string.Empty,
                            DriverlicenseImage = tblDriverDetail?.Driver != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Driver?.DriverLicenseImage : string.Empty,
                            VehicleInsuranceCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehicleInsuranceCertificate : string.Empty,
                            VehiclefitnessCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehiclefitnessCertificate : string.Empty,
                            VehicleRcCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehicleRcCertificate : string.Empty,
                        };
                    }
                    vehicleListsViewModal.Add(vehicleList);
                }

                if (vehicleListsViewModal.Count > 0)
                {
                    var allOrderList = new StartJournyVehicleList
                    {
                        startJournyVehicleList = vehicleListsViewModal
                    };

                    return new ResponseResult
                    {
                        Status = true,
                        Status_Code = HttpStatusCode.OK,
                        Data = allOrderList,
                        message = "Success"
                    };
                }
                else
                {
                    return new ResponseResult
                    {
                        Status = false,
                        Status_Code = HttpStatusCode.BadRequest,
                        message = "No data found"
                    };
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DriverDetailsManager", "GetOrderListById", ex);

                return new ResponseResult
                {
                    Status = false,
                    Status_Code = HttpStatusCode.InternalServerError,
                    message = ex.Message
                };
            }
        }

        #endregion

        #region GetCurrentLetLogforVehicle  
        /// <summary>
        /// GetCurrentLetLogforVehicle  
        /// added by Priyanshi
        /// </summary>
        /// <param name="servicePartnerId"></param>
        /// <param name="journeyDate"></param>
        /// <returns>responseResult</returns>
        public ResponseResult GetCurrentLetLogforVehicle(int driverDetailsId, int servicePartnerId, DateTime journeyDate)
        {
            ResponseResult responseResult = new ResponseResult();
            journeyDate = journeyDate.Date;

            try
            {
                if (servicePartnerId <= 0)
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.OK;
                    responseResult.message = "servicePartnerId not found";
                    return responseResult;
                }

                if (driverDetailsId <= 0)
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.OK;
                    responseResult.message = "DriverDetailsId not found";
                    return responseResult;
                }

                if (journeyDate == DateTime.MinValue)
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.OK;
                    responseResult.message = "Date not valid";
                    return responseResult;
                }

                TblVehicleJourneyTracking tblVehicleJourneyTracking = _journeyTrackingRepository.GetSingle(x => x.IsActive == true && x.JourneyPlanDate != null && Convert.ToDateTime(x.JourneyPlanDate).ToShortDateString() == journeyDate.ToShortDateString() && x.ServicePartnerId == servicePartnerId && x.JourneyStartDatetime != null && x.DriverId == driverDetailsId);

                if (tblVehicleJourneyTracking == null)
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.OK;
                    responseResult.message = "Vehicle Not found";
                    return responseResult;
                }

                var vehicleList = new StartJournyVehicleListbyServicePResponse
                {
                    TrackingId = tblVehicleJourneyTracking.TrackingId,
                    ServicePartnerId = tblVehicleJourneyTracking.ServicePartnerId,
                    DriverDetailsId = tblVehicleJourneyTracking.DriverId,
                    JourneyPlanDate = tblVehicleJourneyTracking.JourneyPlanDate,
                    CurrentVehicleLatt = tblVehicleJourneyTracking.CurrentVehicleLatt,
                    CurrentVehicleLong = tblVehicleJourneyTracking.CurrentVehicleLong,
                    JourneyStartDatetime = tblVehicleJourneyTracking.JourneyStartDatetime,
                    JourneyEndTime = tblVehicleJourneyTracking.JourneyEndTime
                };
                TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetDriverDetailsById((int)tblVehicleJourneyTracking.DriverId);

                // TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetSingle(x => x.DriverDetailsId == item.DriverId && x.IsActive == true);
                if (tblDriverDetail != null)
                {
                    vehicleList.driverDetails = new DriverDetailsModel
                    {
                        DriverDetailsId = tblDriverDetail?.DriverDetailsId,
                        DriverName = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverName : tblDriverDetail?.Driver?.DriverName,
                        DriverPhoneNumber = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverPhoneNumber : tblDriverDetail?.Driver?.DriverPhoneNumber,
                        VehicleNumber = tblDriverDetail?.Vehicle == null ? tblDriverDetail?.VehicleNumber : tblDriverDetail?.Vehicle?.VehicleNumber,
                        City = tblDriverDetail?.Driver == null ? tblDriverDetail?.City : tblDriverDetail?.Driver?.City?.Name,
                        VehicleRcNumber = tblDriverDetail?.Vehicle == null ? tblDriverDetail?.VehicleRcNumber : tblDriverDetail?.Vehicle?.VehicleRcNumber,
                        DriverlicenseNumber = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverlicenseNumber : tblDriverDetail?.Driver?.DriverLicenseNumber,
                        CityId = tblDriverDetail?.Driver == null ? tblDriverDetail?.CityId : tblDriverDetail?.Driver?.CityId,
                        ServicePartnerId = tblDriverDetail?.ServicePartnerId,
                        CreatedDate = tblDriverDetail?.CreatedDate,
                        ModifiedDate = tblDriverDetail?.ModifiedDate,
                        DriverId = tblDriverDetail?.DriverId,
                        VehicleId = tblDriverDetail?.VehicleId,
                        JourneyPlannedDate = (tblDriverDetail?.JourneyPlanDate != null ? (DateTime)tblDriverDetail?.JourneyPlanDate : DateTime.MinValue),
                        ProfilePicture = tblDriverDetail?.Driver != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Driver?.ProfilePicture : string.Empty,
                        DriverlicenseImage = tblDriverDetail?.Driver != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Driver?.DriverLicenseImage : string.Empty,
                        VehicleInsuranceCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehicleInsuranceCertificate : string.Empty,
                        VehiclefitnessCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehiclefitnessCertificate : string.Empty,
                        VehicleRcCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehicleRcCertificate : string.Empty,
                    };
                }
                return new ResponseResult
                {
                    Status = true,
                    Status_Code = HttpStatusCode.OK,
                    Data = vehicleList,
                    message = "Success"
                };
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DriverDetailsManager", "GetOrderListById", ex);

                return new ResponseResult
                {
                    Status = false,
                    Status_Code = HttpStatusCode.InternalServerError,
                    message = ex.Message
                };
            }
        }


        #endregion

        #region Api Method Get ServicePartnerDashboard & DriverDashboard
        /// <summary>
        /// get dashboard of servicepartner on basis of servicepartnerid and date
        /// added by Kranti
        /// </summary>
        /// <param name="servicepartnerid"></param>
        /// <returns></returns>
        /// 

        public ResponseResult ServicePartnerDashboard(DateTime? date, int servicePartnerId, int? driverId)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            ServicePartnerDasboardViewModel dasboardViewModel = new ServicePartnerDasboardViewModel();
            try
            {
                if (servicePartnerId > 0)
                {
                    DateTime currentDate = DateTime.Now.Date;
                    if (date == null)
                        date = currentDate.Date;

                    var vehicleJourneyListQuery = _vehicleJourneyTrackingDetailsRepository.GetList(where: x => x.ServicePartnerId == servicePartnerId && x.IsActive == true);
                    if (driverId != null)
                    {

                        TblDriverDetail? existingDriverDetail = _context.TblDriverDetails
         .Include(x => x.Driver).ThenInclude(x => x.City)
         .FirstOrDefault(x =>
         x.IsActive == true && x.DriverId == driverId && (x.JourneyPlanDate.Value.Date == date.Value.Date));
                        if (existingDriverDetail != null)
                        {
                            vehicleJourneyListQuery = vehicleJourneyListQuery.Where(x => x.DriverId == existingDriverDetail.DriverDetailsId);
                            List<TblVehicleJourneyTrackingDetail> tblVehicleJourneyTrackingDetails1 = _journeyTrackingDetailsRepository.GetList(x => x.IsActive == true && x.DriverId == existingDriverDetail.DriverDetailsId && (x.StatusId != Convert.ToInt32(OrderStatusEnum.PickupDecline) || x.StatusId == Convert.ToInt32(OrderStatusEnum.PickupReschedule)) && (date == DateTime.MinValue ? x.JourneyPlanDate == _currentDatetime.Date : x.JourneyPlanDate == date.Value.Date)).ToList();
                            int TotalPickedUpCount = tblVehicleJourneyTrackingDetails1.Where(x => x.PickupEndDatetime != null).Count();
                            dasboardViewModel.TotalOrderCount = tblVehicleJourneyTrackingDetails1.Count > 0 ? tblVehicleJourneyTrackingDetails1.Count : 0;
                            dasboardViewModel.TotalPickedUpCount = TotalPickedUpCount > 0 ? TotalPickedUpCount : 0;

                        }
                    }
                    if (date != DateTime.MinValue)
                    {
                        vehicleJourneyListQuery = vehicleJourneyListQuery.Where(x => x.JourneyPlanDate == date);
                    }
                    var vehicleJourneyList = vehicleJourneyListQuery.ToList();
                    if (vehicleJourneyList == null || vehicleJourneyList.Count == 0)
                    {
                        responseMessage.message = "Details not found.";
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        return responseMessage;
                    }

                    dasboardViewModel.TodayActiveOrders = vehicleJourneyList.Count();


                    List<TblVehicleJourneyTracking> tblVehicleJourneyTracking = new List<TblVehicleJourneyTracking>();

                    if (date != DateTime.MinValue)
                    {
                        tblVehicleJourneyTracking = _journeyTrackingRepository.GetList(x => x.IsActive == true && x.JourneyPlanDate == date && x.ServicePartnerId == servicePartnerId && x.JourneyStartDatetime != null).ToList();
                    }

                    //var activeVehicle = vehicleJourneyList.Where(x => x.DriverId != null&&x.PickupStartDatetime!=null).ToList();

                    dasboardViewModel.TodayActiveVehicle = tblVehicleJourneyTracking.Count;

                    foreach (var item in vehicleJourneyList)
                    {
                        if (item.Total != null)
                            dasboardViewModel.TodayTotalEarning += (decimal)item.Total;
                        if (item.EstimateEarning != null)
                            dasboardViewModel.TodayEstimateEarning += (decimal)item.EstimateEarning;

                    }
                    var previousDate = currentDate.AddDays(-1);
                    var previousEarning = _vehicleJourneyTrackingDetailsRepository.GetList(where: x => x.JourneyPlanDate == previousDate && x.IsActive == true && x.PickupEndDatetime != null && x.OrderDropTime != null);
                    foreach (var item2 in previousEarning)
                    {
                        if (previousEarning != null && item2.Total != null)

                            dasboardViewModel.PrevoiusEarning += (decimal)item2.Total;

                    }
                    var tilldateEarning = _vehicleJourneyTrackingDetailsRepository.GetList(where: x => x.IsActive == true && x.DriverId == driverId && x.PickupEndDatetime != null && x.OrderDropTime != null);
                    if (driverId != null)
                    {
                        tilldateEarning = _vehicleJourneyTrackingDetailsRepository.GetList(where: x => x.IsActive == true && x.ServicePartnerId == servicePartnerId && x.PickupEndDatetime != null && x.OrderDropTime != null);
                    }
                    foreach (var item3 in tilldateEarning)
                    {
                        if (previousEarning != null && item3.Total != null)

                            dasboardViewModel.TillDateEarning += (decimal)item3.Total;

                    }
                    responseMessage.message = "Details retrieved successfully.";
                    responseMessage.Status = true;
                    responseMessage.Data = dasboardViewModel;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                }
                else
                {
                    responseMessage.message = "ServicePartner not found.";
                    responseMessage.Status = true;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    return responseMessage;
                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("LogisticManager", "ServicePartnerDashboard", ex);
            }
            return responseMessage;
        }
        #endregion

        #region Api Method Get SPWalletSummeryList & DriverWalletSummeryList
        /// <summary>
        /// get dashboard of servicepartner on basis of servicepartnerid and date
        /// added by Kranti
        /// </summary>
        /// <param name="servicepartnerid"></param>
        /// <returns></returns>
        public ResponseResult SPWalletSummeryList(DateTime? date, int servicePartnerId, int? driverId)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            List<TblVehicleJourneyTrackingDetail> VehicleDetails = new List<TblVehicleJourneyTrackingDetail>();
            List<TblVehicleJourneyTrackingDetail> VehicleJourneyList = new List<TblVehicleJourneyTrackingDetail>();
            List<TblVehicleJourneyTrackingDetail> ActiveVehicle = new List<TblVehicleJourneyTrackingDetail>();
            List<ServicePartnerDashboardListViewModel> dasboardListViewModel = new List<ServicePartnerDashboardListViewModel>();

            try
            {
                if (servicePartnerId > 0)
                {
                    if (date == null)
                    {
                        date = _currentDatetime.Date;
                    }
                    if (driverId != null)
                    {
                        VehicleJourneyList = _vehicleJourneyTrackingDetailsRepository.GetList(where: x => x.JourneyPlanDate == date && x.ServicePartnerId == servicePartnerId && x.IsActive == true && x.DriverId == driverId && x.PickupStartDatetime != null && x.PickupEndDatetime != null).ToList();
                    }
                    else
                    {
                        VehicleJourneyList = _vehicleJourneyTrackingDetailsRepository.GetList(where: x => x.ServicePartnerId == servicePartnerId && x.JourneyPlanDate == date && x.IsActive == true && x.PickupStartDatetime != null && x.PickupEndDatetime != null).ToList();
                    }

                    if (VehicleJourneyList == null)
                    {
                        responseMessage.message = "Details not found.";
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        return responseMessage;
                    }
                    foreach (var item in VehicleJourneyList)
                    {
                        ServicePartnerDashboardListViewModel dasboardViewModel = new ServicePartnerDashboardListViewModel();
                        dasboardViewModel.EstimateEarning = item.EstimateEarning;
                        dasboardViewModel.JourneyPlanDate = item.JourneyPlanDate;
                        dasboardViewModel.PickupStartDatetime = item.PickupStartDatetime;
                        dasboardViewModel.PickupEndDatetime = item.PickupEndDatetime;
                        dasboardViewModel.OrderDropTime = item.OrderDropTime;
                        dasboardViewModel.PickupTatinHr = item.PickupTat;
                        dasboardViewModel.DropTatinHr = item.DropTat;
                        dasboardViewModel.BasePrice = item.BasePrice;
                        dasboardViewModel.PickupIncAmount = item.PickupInc;
                        dasboardViewModel.PackagingIncentive = item.PackingInc;
                        dasboardViewModel.DropIncAmount = item.DropInc;
                        dasboardViewModel.DropImageInc = item.DropImageInc;
                        dasboardViewModel.Total = item.Total;

                        var tblOrderTrans = _orderTransRepository.GetSingle(where: x => x.OrderTransId == item.OrderTransId && x.IsActive == true);
                        if (tblOrderTrans != null)
                        {
                            dasboardViewModel.RegdNo = tblOrderTrans.RegdNo;
                            dasboardViewModel.OrderTrans = item.OrderTransId;

                        }
                        var tblEVC = _eVCRepository.GetSingle(where: x => x.EvcregistrationId == item.Evcid && x.IsActive == true && x.Isevcapprovrd == true);
                        if (tblEVC != null)
                        {
                            dasboardViewModel.EVCName = tblEVC.BussinessName;
                        }
                        var tblEvcPartner = _evCPartnerRepository.GetEVCPartnerDetails((int)item.EvcpartnerId);
                        if (tblEvcPartner != null)
                        {
                            dasboardViewModel.EvcStoreCode = tblEvcPartner.EvcStoreCode;
                        }
                        dasboardListViewModel.Add(dasboardViewModel);
                    }

                    if (dasboardListViewModel.Count > 0)
                    {
                        ServicePartnerDashboardList allOrderList = new ServicePartnerDashboardList
                        {
                            AllSPOrderlistViewModel = dasboardListViewModel
                        };

                        responseMessage.message = "Details retrieved successfully.";
                        responseMessage.Status = true;
                        responseMessage.Data = allOrderList;
                        responseMessage.Status_Code = HttpStatusCode.OK;
                    }
                }
                else
                {
                    responseMessage.message = "Service Partner not found.";
                    responseMessage.Status = true;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    return responseMessage;
                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("LogisticManager", "ServicePartnerDashboard", ex);
            }
            return responseMessage;
        }

        #endregion

        #region Api Method Get SPWalletSummeryCount
        /// <summary>
        /// get dashboard of servicepartner on basis of servicepartnerid and date
        /// added by Kranti
        /// </summary>
        /// <param name="servicepartnerid"></param>
        /// <returns></returns>
        public ResponseResult SPWalletSummeryCount(DateTime date, int servicePartnerId, int? driverId)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            SPWalletSummeryViewModel walletsummeryViewModel = new SPWalletSummeryViewModel();
            DateTime currentDate;

            try
            {
                if (servicePartnerId > 0)
                {
                    if (date != DateTime.MinValue)
                    {
                        currentDate = date;

                    }
                    else
                    {
                        currentDate = DateTime.Now.Date;
                    }
                    var vehicleJourneyListQuery = _vehicleJourneyTrackingDetailsRepository.GetList(where: x => x.ServicePartnerId == servicePartnerId && x.IsActive == true);

                    if (driverId != null)
                    {
                        vehicleJourneyListQuery = vehicleJourneyListQuery.Where(x => x.DriverId == driverId);
                    }
                    if (date != DateTime.MinValue)
                    {
                        vehicleJourneyListQuery = vehicleJourneyListQuery.Where(x => x.JourneyPlanDate == currentDate && x.PickupStartDatetime != null && x.PickupEndDatetime != null);
                    }
                    var vehicleJourneyList = vehicleJourneyListQuery.ToList();
                    if (vehicleJourneyList == null || vehicleJourneyList.Count == 0)
                    {
                        responseMessage.message = "Details not found.";
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;

                    }

                    foreach (var item in vehicleJourneyList)
                    {
                        if (item.Total != null)
                        {
                            walletsummeryViewModel.TotalEarning += (decimal)item.Total;
                        }
                    }
                    var previousDate = date.AddDays(-1);
                    IEnumerable<TblVehicleJourneyTrackingDetail>? previousEarning;
                    if (driverId != null)
                    {
                        previousEarning = _vehicleJourneyTrackingDetailsRepository.GetList(where: x => x.JourneyPlanDate == previousDate && x.IsActive == true && x.PickupEndDatetime != null && x.OrderDropTime != null && x.ServicePartnerId == servicePartnerId && (x.DriverId == (driverId > 0 ? driverId : null)));
                    }
                    else
                    {
                        previousEarning = _vehicleJourneyTrackingDetailsRepository.GetList(where: x => x.JourneyPlanDate == previousDate && x.IsActive == true && x.PickupEndDatetime != null && x.OrderDropTime != null && x.ServicePartnerId == servicePartnerId);
                    }
                    foreach (var item2 in previousEarning)
                    {
                        if (previousEarning != null && item2.Total != null)
                            walletsummeryViewModel.PrevoiusEarning += (decimal)item2.Total;
                    }
                    IEnumerable<TblVehicleJourneyTrackingDetail>? tilldateEarning;
                    if (driverId != null)
                    {
                        tilldateEarning = _vehicleJourneyTrackingDetailsRepository.GetList(where: x => x.IsActive == true && x.ServicePartnerId == servicePartnerId && (x.DriverId == (driverId > 0 ? driverId : null)) && x.PickupEndDatetime != null && x.OrderDropTime != null);

                    }
                    else
                    {
                        tilldateEarning = _vehicleJourneyTrackingDetailsRepository.GetList(where: x => x.IsActive == true && x.ServicePartnerId == servicePartnerId && x.PickupEndDatetime != null && x.OrderDropTime != null);

                    }
                    foreach (var item3 in tilldateEarning)
                    {
                        if (previousEarning != null && item3.Total != null)
                            walletsummeryViewModel.TillDateEarning += (decimal)item3.Total;
                    }

                    responseMessage.message = "Details retrieved successfully.";
                    responseMessage.Status = true;
                    responseMessage.Data = walletsummeryViewModel;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                }
                else
                {
                    responseMessage.message = "Service Partner not found.";
                    responseMessage.Status = true;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    return responseMessage;
                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("LogisticManager", "ServicePartnerDashboard", ex);
            }
            return responseMessage;
        }

        #endregion

        #region VehicleList
        public ResponseResult VehicleList(int servicePartnerId, string? searchTerm, int? pageNumber, int? pageSize)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            string imagepath = string.Empty;

            try
            {
                if (servicePartnerId > 0)
                {
                    var servicePartner = _servicePartnerRepository.GetSingle(where: x => x.ServicePartnerId == servicePartnerId && x.IsActive == true && x.ServicePartnerIsApprovrd == true);
                    if (servicePartner != null)
                    {
                        List<TblVehicleList> query = _context.TblVehicleLists.Include(x => x.City).Where(x => x.IsActive == true && x.ServicePartnerId == servicePartnerId).ToList();

                        // Apply filtering
                        if (!string.IsNullOrWhiteSpace(searchTerm))
                        {
                            query = query.Where(x => x.VehicleNumber.Contains(searchTerm) || x.VehicleRcNumber.Contains(searchTerm) || x.City.Name.Contains(searchTerm)).ToList();
                        }

                        // Get total count for pagination
                        int totalCount = query.Count();

                        // Apply pagination
                        List<TblVehicleList> vehicleDetails = query.OrderBy(x => x.CreatedDate)
                                                                   .Skip((int)((pageNumber - 1) * pageSize))
                                                                   .Take((int)pageSize)
                                                                   .ToList();

                        // Map to view model
                        List<vehicleList> vehicleListDetails = new List<vehicleList>();
                        foreach (var item in vehicleDetails)
                        {
                            vehicleList VehiclelistViewModel = new vehicleList();
                            VehiclelistViewModel.VehicleId = item.VehicleId;

                            VehiclelistViewModel.VehicleNumber = item.VehicleNumber;
                            VehiclelistViewModel.VehicleRcNumber = item.VehicleRcNumber;
                            VehiclelistViewModel.ServicePartnerId = item.ServicePartnerId;
                            VehiclelistViewModel.CityId = item.CityId;
                            //  TblCity tblCity = _context.TblCities.Where(x=>x.CityId== VehiclelistViewModel.CityId).FirstOrDefault();
                            VehiclelistViewModel.CityName = item.City.Name;
                            VehiclelistViewModel.CreatedDate = item.CreatedDate;
                            VehiclelistViewModel.ModifiedDate = item.ModifiedDate;

                            // Add images path
                            if (!string.IsNullOrWhiteSpace(item.VehicleInsuranceCertificate))
                            {
                                imagepath = _baseConfig.Value.PostERPImagePath + _baseConfig.Value.VehicleInsuranceCertificate + item.VehicleInsuranceCertificate;
                                VehiclelistViewModel.VehicleInsuranceCertificate = imagepath;
                            }
                            if (!string.IsNullOrWhiteSpace(item.VehiclefitnessCertificate))
                            {
                                imagepath = _baseConfig.Value.PostERPImagePath + _baseConfig.Value.VehiclefitnessCertificate + item.VehiclefitnessCertificate;
                                VehiclelistViewModel.VehiclefitnessCertificate = imagepath;
                            }
                            if (!string.IsNullOrWhiteSpace(item.VehicleRcCertificate))
                            {
                                imagepath = _baseConfig.Value.PostERPImagePath + _baseConfig.Value.VehicleRcCertificate + item.VehicleRcCertificate;
                                VehiclelistViewModel.VehicleRcCertificate = imagepath;
                            }

                            vehicleListDetails.Add(VehiclelistViewModel);
                        }

                        // Prepare response
                        vehicleListResponseList allVehicleList = new vehicleListResponseList
                        {
                            vehicleLists = vehicleListDetails
                        };

                        responseMessage.message = "Details retrieved successfully.";
                        responseMessage.Status = true;
                        responseMessage.Data = allVehicleList;
                        //responseMessage.TotalItems = totalCount;
                        //responseMessage.PageNumber = pageNumber;
                        //responseMessage.PageSize = pageSize;
                        responseMessage.Status_Code = HttpStatusCode.OK;
                    }
                    else
                    {
                        responseMessage.message = "Service Partner not found.";
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.OK;
                    }
                }
                else
                {
                    responseMessage.message = "Service Partner ID must be greater than 0.";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("LogisticManager", "ServicePartnerDashboard", ex);
            }

            return responseMessage;
        }
        #endregion

        #region Update pincodes ServicePartner/LGC Registation   ADDED BY Kranti
        /// <summary>
        /// Add ServicePartner inti TblServicePartner
        /// Used in api update of pincodes LGC Registeration in mobile development
        /// </summary>
        /// <param name="UpdateLGCRegisterationModel"></param>
        /// <param name=""></param>
        /// <returns>tblServicePartner</returns>
        public ResponseResult UpdatePincodesServicePartner(UpdatePinCodeServicePartner data)
        {
            ResponseResult responseMessage = new ResponseResult();
            try
            {
                if (data.ServicePartnerId > 0)
                {
                    List<MapServicePartnerCityState> mapServicePartnerCityStates = _mapServicePartnerCityStateRepository.GetList(x => x.IsActive == true && x.ServicePartnerId == data.ServicePartnerId).ToList();
                    bool cityExists = mapServicePartnerCityStates.Any(x => x.CityId == data.CityId);
                    if (cityExists)
                    {
                        MapServicePartnerCityState mapServicePartnerCityState = _mapServicePartnerCityStateRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerId == data.ServicePartnerId && x.CityId == data.CityId);
                        if (data.Pincodes != null)
                        {
                            TblCity city = _context.TblCities.Where(x => x.CityId == data.CityId).FirstOrDefault();
                            if (city != null)
                            {
                                TblState state = _context.TblStates.Where(x => x.StateId == city.StateId).FirstOrDefault();
                                mapServicePartnerCityState.StateId = state.StateId;
                            }
                            mapServicePartnerCityState.ServicePartnerId = data.ServicePartnerId;
                            mapServicePartnerCityState.CityId = data.CityId;
                            mapServicePartnerCityState.ListOfPincodes = data.Pincodes;

                            mapServicePartnerCityState.ModifiedDate = _currentDatetime;
                            mapServicePartnerCityState.ListOfPincodes = data.Pincodes;
                            _mapServicePartnerCityStateRepository.Update(mapServicePartnerCityState);
                            _mapServicePartnerCityStateRepository.SaveChanges();

                            responseMessage.message = "City, State & Pincodes updated successfully";
                            responseMessage.Status = true;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                        }

                        else
                        {
                            responseMessage.message = "Please Provide Pincodes";
                            responseMessage.Status = true;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                        }

                    }
                    else
                    {
                        if (data.Pincodes != null)
                        {
                            MapServicePartnerCityState mapServicePartnerCityState = new MapServicePartnerCityState();
                            TblCity city = _context.TblCities.Where(x => x.CityId == data.CityId).FirstOrDefault();
                            if (city != null)
                            {
                                TblState state = _context.TblStates.Where(x => x.StateId == city.StateId).FirstOrDefault();
                                mapServicePartnerCityState.StateId = state.StateId;
                            }

                            mapServicePartnerCityState.ServicePartnerId = data.ServicePartnerId;
                            mapServicePartnerCityState.CityId = data.CityId;
                            mapServicePartnerCityState.ListOfPincodes = data.Pincodes;
                            mapServicePartnerCityState.IsActive = true;
                            mapServicePartnerCityState.CreatedDate = _currentDatetime;
                            _mapServicePartnerCityStateRepository.Create(mapServicePartnerCityState);
                            _mapServicePartnerCityStateRepository.SaveChanges();

                            responseMessage.message = "City, State & Pincodes added successfully";
                            responseMessage.Status = true;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                        }
                        else
                        {
                            responseMessage.message = "Please Provide Pincodes";
                            responseMessage.Status = true;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                        }
                    }
                }
                else
                {
                    responseMessage.message = "Please provide Service Partner Id";
                    responseMessage.Status = false;
                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                _logging.WriteErrorToDB("LogisticManager", "LGCRegister", ex);
                //return responseMessage;
            }
            return responseMessage;
        }
        #endregion

        #region Update Service Partner API ADDED BY KRANTI
        /// <summary>
        /// Add ServicePartner inti TblServicePartner
        /// Used in api of LGC Registeration in mobile development
        /// </summary>
        /// <param name="LGCRegisterationModel"></param>
        /// <param name=""></param>
        /// <returns>tblServicePartner</returns>
        public ResponseResult UpdateServicePartner([FromForm] UpdateServicePartnerDataModel LGCRegisterationModel)
        {
            TblEvcregistration TblEvcregistration = new TblEvcregistration();
            TblServicePartner tblServicePartner = new TblServicePartner();

            int userId = 3;
            bool cencelCheck = false;
            ResponseResult responseMessage = new ResponseResult();
            try
            {
                if (LGCRegisterationModel != null)
                {
                    TblServicePartner OServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerId == LGCRegisterationModel.ServicePartnerId);
                    tblServicePartner = _mapper.Map<UpdateServicePartnerDataModel, TblServicePartner>(LGCRegisterationModel);

                    if (tblServicePartner != null && tblServicePartner.ServicePartnerId > 0)
                    {

                        #region add images to folder

                        if (LGCRegisterationModel.ServicePartnerCancelledCheque != null)
                        {
                            string fileName = string.Empty;
                            if (OServicePartner.ServicePartnerRegdNo != null)
                            {
                                fileName = OServicePartner.ServicePartnerRegdNo + "CancelledCheck" + ".jpg";
                            }
                            else
                            {

                                fileName = OServicePartner.ServicePartnerId + "CancelledCheck" + ".jpg";
                            }


                            string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\CancelledCheque";

                            cencelCheck = _imageHelper.SaveFileDefRoot(LGCRegisterationModel.ServicePartnerCancelledCheque, filePath, fileName);
                            if (cencelCheck)
                            {
                                OServicePartner.ServicePartnerCancelledCheque = fileName;
                                cencelCheck = false;
                            }
                        }

                        if (LGCRegisterationModel.ServicePartnerGstregisteration != null)
                        {
                            string fileName = string.Empty;
                            if (OServicePartner.ServicePartnerRegdNo != null)
                            {
                                fileName = OServicePartner.ServicePartnerRegdNo + "ServicePartner_GST" + ".jpg";
                            }
                            else
                            {
                                fileName = OServicePartner.ServicePartnerId + "ServicePartner_GST" + ".jpg";
                            }


                            string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\GST";
                            cencelCheck = _imageHelper.SaveFileDefRoot(LGCRegisterationModel.ServicePartnerGstregisteration, filePath, fileName);
                            if (cencelCheck)
                            {
                                OServicePartner.ServicePartnerGstregisteration = fileName;
                                cencelCheck = false;
                            }
                        }

                        if (LGCRegisterationModel.ServicePartnerAadharfrontImage != null)
                        {
                            string fileName = string.Empty;
                            if (OServicePartner.ServicePartnerRegdNo != null)
                            {
                                fileName = OServicePartner.ServicePartnerRegdNo + "AadharFront" + ".jpg";
                            }
                            else
                            {
                                fileName = OServicePartner.ServicePartnerId + "AadharFront" + ".jpg";
                            }
                            //string fileName = tblServicePartner.ServicePartnerRegdNo + "AadharFront" + ".jpg";
                            string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\Aadhar";
                            cencelCheck = _imageHelper.SaveFileDefRoot(LGCRegisterationModel.ServicePartnerAadharfrontImage, filePath, fileName);
                            if (cencelCheck)
                            {
                                OServicePartner.ServicePartnerAadharfrontImage = fileName;
                                cencelCheck = false;
                            }
                        }
                        if (LGCRegisterationModel.ServicePartnerAadharBackImage != null)
                        {
                            string fileName = string.Empty;
                            if (OServicePartner.ServicePartnerRegdNo != null)
                            {
                                fileName = OServicePartner.ServicePartnerRegdNo + "AadharBack" + ".jpg";
                            }
                            else
                            {
                                fileName = OServicePartner.ServicePartnerId + "AadharBack" + ".jpg";
                            }
                            //string fileName = tblServicePartner.ServicePartnerRegdNo + "AadharBack" + ".jpg";
                            string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\Aadhar";
                            cencelCheck = _imageHelper.SaveFileDefRoot(LGCRegisterationModel.ServicePartnerAadharBackImage, filePath, fileName);
                            if (cencelCheck)
                            {
                                OServicePartner.ServicePartnerAadharBackImage = fileName;
                                cencelCheck = false;
                            }
                        }
                        if (LGCRegisterationModel.ServicePartnerProfilePic != null)
                        {
                            string fileName = string.Empty;
                            if (OServicePartner.ServicePartnerRegdNo != null)
                            {
                                fileName = OServicePartner.ServicePartnerRegdNo + "ProfilePic" + ".jpg";
                            }
                            else
                            {
                                fileName = OServicePartner.ServicePartnerId + "ProfilePic" + ".jpg";
                            }
                            //string fileName = "Profile" + ".jpg";
                            string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\ProfilePic";
                            cencelCheck = _imageHelper.SaveFileDefRoot(LGCRegisterationModel.ServicePartnerProfilePic, filePath, fileName);
                            if (cencelCheck)
                            {
                                OServicePartner.ServicePartnerProfilePic = fileName;
                                cencelCheck = false;
                            }
                        }

                        #endregion

                        //Code to update the object 
                        if (LGCRegisterationModel.ServicePartnerName != null)
                        {
                            OServicePartner.ServicePartnerName = LGCRegisterationModel.ServicePartnerName;
                        }
                        else
                        {
                            OServicePartner.ServicePartnerName = OServicePartner.ServicePartnerName;
                        }
                        if (LGCRegisterationModel.ServicePartnerDescription != null)
                        {
                            OServicePartner.ServicePartnerDescription = LGCRegisterationModel.ServicePartnerDescription;
                        }
                        else
                        {
                            OServicePartner.ServicePartnerDescription = OServicePartner.ServicePartnerDescription;
                        }
                        if (LGCRegisterationModel.IsActive != null)
                        {
                            OServicePartner.IsActive = LGCRegisterationModel.IsActive;
                        }
                        else
                        {
                            OServicePartner.IsActive = OServicePartner.IsActive;
                        }
                        if (LGCRegisterationModel.CreatedBy != null)
                        {
                            OServicePartner.CreatedBy = LGCRegisterationModel.CreatedBy;
                        }
                        else
                        {
                            OServicePartner.CreatedBy = OServicePartner.CreatedBy;
                        }
                        if (LGCRegisterationModel.CreatedDate != null)
                        {
                            OServicePartner.CreatedDate = LGCRegisterationModel.CreatedDate;
                        }
                        else
                        {
                            OServicePartner.CreatedDate = OServicePartner.CreatedDate;
                        }
                        if (LGCRegisterationModel.IsServicePartnerLocal != null)
                        {
                            OServicePartner.IsServicePartnerLocal = LGCRegisterationModel.IsServicePartnerLocal;
                        }
                        else
                        {
                            OServicePartner.IsServicePartnerLocal = OServicePartner.IsServicePartnerLocal;
                        }
                        if (LGCRegisterationModel.UserId != null)
                        {
                            OServicePartner.UserId = LGCRegisterationModel.UserId;
                        }
                        else
                        {
                            OServicePartner.UserId = OServicePartner.UserId;
                        }
                        if (LGCRegisterationModel.ServicePartnerRegdNo != null)
                        {
                            OServicePartner.ServicePartnerRegdNo = LGCRegisterationModel.ServicePartnerRegdNo;
                        }
                        else
                        {
                            OServicePartner.ServicePartnerRegdNo = OServicePartner.ServicePartnerRegdNo;
                        }
                        if (LGCRegisterationModel.ServicePartnerMobileNumber != null)
                        {
                            OServicePartner.ServicePartnerMobileNumber = LGCRegisterationModel.ServicePartnerMobileNumber;
                        }
                        else
                        {
                            OServicePartner.ServicePartnerMobileNumber = OServicePartner.ServicePartnerMobileNumber;
                        }
                        if (LGCRegisterationModel.ServicePartnerAlternateMobileNumber != null)
                        {
                            OServicePartner.ServicePartnerAlternateMobileNumber = LGCRegisterationModel.ServicePartnerAlternateMobileNumber;
                        }
                        else
                        {
                            OServicePartner.ServicePartnerAlternateMobileNumber = OServicePartner.ServicePartnerAlternateMobileNumber;
                        }
                        if (LGCRegisterationModel.ServicePartnerAlternateMobileNumber != null)
                        {
                            OServicePartner.ServicePartnerAlternateMobileNumber = LGCRegisterationModel.ServicePartnerAlternateMobileNumber;
                        }
                        else
                        {
                            OServicePartner.ServicePartnerAlternateMobileNumber = OServicePartner.ServicePartnerAlternateMobileNumber;
                        }
                        if (LGCRegisterationModel.ServicePartnerEmailId != null)
                        {
                            OServicePartner.ServicePartnerEmailId = LGCRegisterationModel.ServicePartnerEmailId;
                        }
                        else
                        {
                            OServicePartner.ServicePartnerEmailId = OServicePartner.ServicePartnerEmailId;
                        }
                        if (LGCRegisterationModel.ServicePartnerAddressLine1 != null)
                        {
                            OServicePartner.ServicePartnerAddressLine1 = LGCRegisterationModel.ServicePartnerAddressLine1;
                        }
                        else
                        {
                            OServicePartner.ServicePartnerAddressLine1 = OServicePartner.ServicePartnerAddressLine1;
                        }
                        if (LGCRegisterationModel.ServicePartnerAddressLine2 != null)
                        {
                            OServicePartner.ServicePartnerAddressLine2 = LGCRegisterationModel.ServicePartnerAddressLine2;
                        }
                        else
                        {
                            OServicePartner.ServicePartnerAddressLine2 = OServicePartner.ServicePartnerAddressLine2;
                        }
                        if (LGCRegisterationModel.ServicePartnerAddressLine2 != null)
                        {
                            OServicePartner.ServicePartnerAddressLine2 = LGCRegisterationModel.ServicePartnerAddressLine2;
                        }
                        else
                        {
                            OServicePartner.ServicePartnerAddressLine2 = OServicePartner.ServicePartnerAddressLine2;
                        }
                        if (LGCRegisterationModel.ServicePartnerGstno != null)
                        {
                            OServicePartner.ServicePartnerGstno = LGCRegisterationModel.ServicePartnerGstno;
                        }
                        else
                        {
                            OServicePartner.ServicePartnerGstno = OServicePartner.ServicePartnerGstno;
                        }
                        if (LGCRegisterationModel.ServicePartnerBankName != null)
                        {
                            OServicePartner.ServicePartnerBankName = LGCRegisterationModel.ServicePartnerBankName;
                        }
                        else
                        {
                            OServicePartner.ServicePartnerBankName = OServicePartner.ServicePartnerBankName;
                        }
                        if (LGCRegisterationModel.ServicePartnerBankAccountNo != null)
                        {
                            OServicePartner.ServicePartnerBankAccountNo = LGCRegisterationModel.ServicePartnerBankAccountNo;
                        }
                        else
                        {
                            OServicePartner.ServicePartnerBankAccountNo = OServicePartner.ServicePartnerBankAccountNo;
                        }
                        if (LGCRegisterationModel.ServicePartnerIfsccode != null)
                        {
                            OServicePartner.ServicePartnerIfsccode = LGCRegisterationModel.ServicePartnerIfsccode;
                        }
                        else
                        {
                            OServicePartner.ServicePartnerIfsccode = OServicePartner.ServicePartnerIfsccode;
                        }
                        if (LGCRegisterationModel.ServicePartnerInsertOtp != null)
                        {
                            OServicePartner.ServicePartnerInsertOtp = LGCRegisterationModel.ServicePartnerInsertOtp;
                        }
                        else
                        {
                            OServicePartner.ServicePartnerInsertOtp = OServicePartner.ServicePartnerInsertOtp;
                        }
                        if (LGCRegisterationModel.ServicePartnerLoginId != null)
                        {
                            OServicePartner.ServicePartnerLoginId = LGCRegisterationModel.ServicePartnerLoginId;
                        }
                        else
                        {
                            OServicePartner.ServicePartnerLoginId = OServicePartner.ServicePartnerLoginId;
                        }
                        if (LGCRegisterationModel.ServicePartnerIsApprovrd != null)
                        {
                            OServicePartner.ServicePartnerIsApprovrd = LGCRegisterationModel.ServicePartnerIsApprovrd;
                        }
                        else
                        {
                            OServicePartner.ServicePartnerIsApprovrd = OServicePartner.ServicePartnerIsApprovrd;
                        }
                        if (LGCRegisterationModel.IconfirmTermsCondition != null)
                        {
                            OServicePartner.IconfirmTermsCondition = LGCRegisterationModel.IconfirmTermsCondition;
                        }
                        else
                        {
                            OServicePartner.IconfirmTermsCondition = OServicePartner.IconfirmTermsCondition;
                        }
                        if (LGCRegisterationModel.ServicePartnerFirstName != null)
                        {
                            OServicePartner.ServicePartnerFirstName = LGCRegisterationModel.ServicePartnerFirstName;
                        }
                        else
                        {
                            OServicePartner.ServicePartnerFirstName = OServicePartner.ServicePartnerFirstName;
                        }
                        if (LGCRegisterationModel.ServicePartnerLastName != null)
                        {
                            OServicePartner.ServicePartnerLastName = LGCRegisterationModel.ServicePartnerLastName;
                        }
                        else
                        {
                            OServicePartner.ServicePartnerLastName = OServicePartner.ServicePartnerLastName;
                        }
                        if (LGCRegisterationModel.ServicePartnerBusinessName != null)
                        {
                            OServicePartner.ServicePartnerBusinessName = LGCRegisterationModel.ServicePartnerBusinessName;
                        }
                        else
                        {
                            OServicePartner.ServicePartnerBusinessName = OServicePartner.ServicePartnerBusinessName;
                        }


                        OServicePartner.ModifiedDate = _currentDatetime;
                        OServicePartner.Modifiedby = userId;

                        _context.Entry(OServicePartner).State = EntityState.Modified;

                        int result = _context.SaveChanges();
                        if (result == 1)
                        {
                            responseMessage.message = "Details Updated Successfully";
                            responseMessage.Status = true;
                            responseMessage.Status_Code = HttpStatusCode.OK;


                        }
                        else
                        {
                            responseMessage.Status_Code = HttpStatusCode.OK;
                            responseMessage.message = "Data Not Map properply";
                            responseMessage.Status = false;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                _logging.WriteErrorToDB("LogisticManager", "LGCRegister", ex);
                //return responseMessage;
            }
            return responseMessage;
        }
        #endregion

        #region Update Vehicle(Driver Deatils) by api Added by Kranti
        /// <summary>
        /// Update LGC Vehichle
        /// Used in api of UpdateVehicle in mobile development
        /// Added by Kranti
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public ResponseResult UpdateVehicle([FromForm] UpdateVehicleDataModel dataModel)
        {
            TblVehicleList tblVehicleList = null;
            bool cancelCheck = false;
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;

            try
            {
                if (dataModel != null)
                {
                    TblVehicleList objtblVehicleList = _vehicleListRepository.GetSingle(x => x.IsActive == true && x.VehicleId == dataModel.VehicleId && x.ServicePartnerId == dataModel.ServicePartnerId);
                    // tblVehicleList = _mapper.Map<UpdateVehicleDataModel, TblVehicleList>(dataModel);

                    if (objtblVehicleList != null && objtblVehicleList.VehicleId > 0)
                    {
                        //Code to update the object 
                        if (dataModel.CityId != null)
                        {
                            objtblVehicleList.CityId = dataModel.CityId;
                        }
                        else
                        {
                            objtblVehicleList.CityId = objtblVehicleList.CityId;
                        }

                        // Add Image in Folder                      
                        // VehicleRcCertificate
                        if (dataModel.VehicleRcCertificate != null)
                        {
                            string fileName = string.Empty;
                            if (objtblVehicleList.VehicleRcNumber != null)
                            {
                                fileName = objtblVehicleList.VehicleRcNumber + "VehicleRcCertificate" + ".jpg";
                            }
                            else
                            {
                                fileName = objtblVehicleList.VehicleNumber + "VehicleRcCertificate" + ".jpg";
                            }
                            string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\Driver\\VehicleRcCertificate";
                            cancelCheck = _imageHelper.SaveFileDefRoot(dataModel.VehicleRcCertificate, filePath, fileName);
                            if (cancelCheck)
                            {
                                objtblVehicleList.VehicleRcCertificate = fileName;
                                cancelCheck = false;
                            }
                        }

                        // VehiclefitnessCertificate
                        if (dataModel.VehiclefitnessCertificate != null)
                        {
                            string fileName = string.Empty;
                            if (objtblVehicleList.VehicleNumber != null)
                            {
                                fileName = objtblVehicleList.VehicleNumber + "VehicleFitnessCertificate" + ".jpg";
                            }
                            else
                            {
                                fileName = objtblVehicleList.VehicleRcNumber + "VehicleFitnessCertificate" + ".jpg";
                            }
                            string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\Driver\\DriverFitnessCerti";
                            cancelCheck = _imageHelper.SaveFileDefRoot(dataModel.VehiclefitnessCertificate, filePath, fileName);
                            if (cancelCheck)
                            {
                                objtblVehicleList.VehiclefitnessCertificate = fileName;
                                cancelCheck = false;
                            }
                        }

                        // VehicleInsuranceCertificate
                        if (dataModel.VehicleInsuranceCertificate != null)
                        {
                            string fileName = string.Empty;
                            if (objtblVehicleList.VehicleInsuranceCertificate != null)
                            {
                                fileName = objtblVehicleList.VehicleRcNumber + "VehicleInsuranceCertificate" + ".jpg";
                            }
                            else
                            {

                                fileName = objtblVehicleList.VehicleNumber + "VehicleInsuranceCertificate" + ".jpg";
                            }

                            string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\Driver\\DriverInsuranceCerti";
                            cancelCheck = _imageHelper.SaveFileDefRoot(dataModel.VehicleInsuranceCertificate, filePath, fileName);
                            if (cancelCheck)
                            {
                                objtblVehicleList.VehicleInsuranceCertificate = fileName;
                                cancelCheck = false;
                            }
                        }
                        objtblVehicleList.IsActive = true;
                        objtblVehicleList.ModifiedDate = _currentDatetime;
                        objtblVehicleList.ModifiedBy = dataModel.SPuserId;

                        _context.Entry(objtblVehicleList).State = EntityState.Modified;
                        int result = _context.SaveChanges();

                        if (result > 0)
                        {
                            responseMessage.message = "Update Success";
                            responseMessage.Status = true;
                            responseMessage.Status_Code = HttpStatusCode.OK;

                        }
                        else
                        {
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.BadRequest;
                            responseMessage.message = "Update Failed";
                        }
                    }
                    else
                    {
                        responseMessage.message = "Vehicle not found";
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.OK;
                        return responseMessage;
                    }

                }
                else
                {
                    responseMessage.message = "Data model is null";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    return responseMessage;
                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("LogisticManager", "UpdateVehicle", ex);
            }

            return responseMessage;
        }
        #endregion

        #region ManageServicePartnerDriverUser
        /// <summary>
        /// Create User For Service Partner Driver
        /// </summary>
        /// <param name="tblServicePartner"></param>
        /// <returns></returns>
        public int ManageServicePartnerDriverUser(TblDriverList tblDriverDetail)
        {
            int userId = 0;

            UserViewModel userVM = new UserViewModel();
            TblCompany tblCompany = null;
            TblRole tblRole = null;
            UserRoleViewModel userRoleVM = new UserRoleViewModel();
            try
            {
                if (tblDriverDetail != null)
                {
                    string key1 = _baseConfig.Value.SecurityKey;
                    string phoneNumber = tblDriverDetail.DriverPhoneNumber;
                    tblDriverDetail.DriverPhoneNumber = SecurityHelper.EncryptString(tblDriverDetail.DriverPhoneNumber, key1);

                    TblUser tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.Phone == tblDriverDetail.DriverPhoneNumber);

                    tblCompany = _companyRepository.GetSingle(x => x.IsActive == true && x.CompanyName == EnumHelper.DescriptionAttr(CompanyNameenum.UTC).ToString());
                    tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleName == EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner_Driver).ToString());

                    if (tblUser != null && tblUser.UserId > 0 && tblCompany != null && tblRole != null)
                    {
                        userId = tblUser.UserId;


                        //Code to Insert the object in tbluserrole
                        TblUserRole tblUserRole = new TblUserRole();
                        tblUserRole.RoleId = tblRole.RoleId;
                        tblUserRole.UserId = tblUser.UserId;
                        tblUserRole.CompanyId = tblCompany.CompanyId;
                        tblUserRole.IsActive = true;
                        tblUserRole.CreatedBy = tblDriverDetail.CreatedBy;
                        tblUserRole.CreatedDate = _currentDatetime;


                        _userRoleRepository.Create(tblUserRole);
                        _userRoleRepository.SaveChanges();
                    }
                    else
                    {
                        tblUser = new TblUser();
                        //Code to Insert the object in tbluser
                        tblUser.Phone = tblDriverDetail.DriverPhoneNumber;
                        //tblUser.Phone = SecurityHelper.EncryptString(tblDriverDetail.DriverPhoneNumber, _baseConfig.Value.SecurityKey);                        
                        tblUser.FirstName = tblDriverDetail.DriverName;
                        tblUser.ImageName = tblDriverDetail.ProfilePicture;
                        tblUser.IsActive = true;
                        tblUser.CreatedDate = _currentDatetime;
                        tblUser.CreatedBy = tblDriverDetail.CreatedBy;
                        tblUser.CompanyId = tblCompany?.CompanyId;
                        tblUser.UserStatus = "Active";


                        _userRepository.Create(tblUser);
                        _userRepository.SaveChanges();

                        userId = tblUser.UserId;

                        //Code to Insert the object in tbluserrole

                        TblUserRole tblUserRole = new TblUserRole();
                        tblUserRole.RoleId = tblRole.RoleId;
                        tblUserRole.UserId = userId;
                        tblUserRole.CompanyId = tblCompany.CompanyId;
                        tblUserRole.IsActive = true;
                        tblUserRole.CreatedBy = tblDriverDetail.CreatedBy;
                        tblUserRole.CreatedDate = _currentDatetime;

                        _userRoleRepository.Create(tblUserRole);
                        _userRoleRepository.SaveChanges();
                    }

                }
                else
                {
                    userId = 0;
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("SevicePartnerManager", "ManageServicePartnerDriverUser", ex);
            }
            return userId;
        }
        #endregion

        #region Check mobile number existance
        /// <summary>
        /// Check mobile number existance in loginMobile tbl
        /// added by ashwin
        /// </summary>
        /// <param name="mobNumber"></param>
        /// <returns></returns>
        public OtpWithUserInfo IsValidMobileNumber(string mobNumber)
        {
            bool flag = false;
            //string userRoleName = "false";
            object userRoleInfo = null;
            OtpWithUserInfo otpWithUser = null;
            try
            {
                if (mobNumber != null && mobNumber.Length == 10)
                {
                    var checkemail = _context.TblLoginMobiles.ToList();
                    flag = checkemail.Exists(p => p.MobileNumber == mobNumber);

                    if (flag)
                    {
                        otpWithUser = new OtpWithUserInfo();
                        TblLoginMobile data = _login_MobileRepository.GetSingle(x => x.MobileNumber == mobNumber);
                        if (data != null && data.Id > 0 && data.IsActive == true)
                        {
                            userRoleInfo = data;
                            otpWithUser.UserId = data.UserId;
                            otpWithUser.UserRoleName = data.UserRoleName;
                            //otpWithUser.message = string.Empty;
                        }
                        else if (data != null && data.Id > 0 && data.IsActive == false)
                        {
                            otpWithUser.UserId = 0;
                            otpWithUser.UserRoleName = "Your Registration is in progress, please wait for approval";
                        }
                        else
                            return otpWithUser;
                    }
                    else
                        return otpWithUser;
                }
                else
                    return otpWithUser;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("SevicePartnerManager", "IsValidMobileNumber", ex);
            }

            return otpWithUser;
        }

        #endregion

        #region GetNumberofVehicle by ServicePartner/LGC
        /// <summary>
        /// Api method GetNumberofVehicle for by ServicePartner/LGC
        /// Added by ashwin
        /// </summary>
        /// <param name="">username</param>
        /// <param name=""></param>
        /// <returns></returns>
        public ResponseResult GetNumberofVehicle(string userName)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            TblServicePartner tblServicePartner = null;
            List<TblDriverDetail> tblDriverDetail = null;
            try
            {
                tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerEmailId == userName || x.ServicePartnerMobileNumber == userName);

                if (tblServicePartner != null && tblServicePartner.ServicePartnerId > 0)
                {
                    TotalVehicleCounts totalVehicleCounts = new TotalVehicleCounts();

                    tblDriverDetail = _driverDetailsRepository.GetList(x => x.IsActive == true && x.CreatedBy == tblServicePartner.UserId).ToList();
                    if (tblDriverDetail != null && tblDriverDetail.Count > 0)
                    {
                        totalVehicleCounts.ActiveVehicles = tblDriverDetail.Count;

                    }

                    responseMessage.Data = totalVehicleCounts;
                    responseMessage.message = "Success";
                    responseMessage.Status = true;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                }
                else
                {
                    responseMessage.message = "Not Valid login credential";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                }

            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("LogisticManager", "GetNumberofVehicle", ex);
            }
            return responseMessage;
        }
        #endregion

        #region get login UserId by mobile number
        /// <summary>
        /// get details of loginUser by mobile number
        /// added by ashwin
        /// </summary>
        /// <param name="mobNumber"></param>
        /// <param name="UserRoleName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResponseResult GetLoginUserDetails(string mobNumber, string UserRoleName, int userId)
        {
            int id = 0;
            ResponseResult responseResult = null;
            try
            {
                if (mobNumber != null && mobNumber.Length == 10 && UserRoleName == "Service Partner")
                {
                    TblServicePartner tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.UserId == userId && x.ServicePartnerMobileNumber == mobNumber);
                    if (tblServicePartner != null)
                    {
                        id = (int)tblServicePartner.ServicePartnerId;
                        responseResult = new ResponseResult();
                        responseResult = ServicePartnerDetails(id);
                    }
                }
                else if (mobNumber != null && mobNumber.Length == 10 && UserRoleName == "Service Partner Driver")
                {
                    TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetSingle(x => x.IsActive == true && x.DriverPhoneNumber == mobNumber);
                    if (tblDriverDetail != null)
                    {
                        id = tblDriverDetail.DriverDetailsId;
                        responseResult = new ResponseResult();
                        responseResult.Data = tblDriverDetail;
                        responseResult.message = "Success";
                        responseResult.Status_Code = HttpStatusCode.OK;
                        responseResult.Status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("SevicePartnerManager", "GetLoginUserDetails", ex);
            }
            return responseResult;
        }
        #endregion

        #region Check email existance and get details of login user's
        /// <summary>
        /// Check email existance in loginMobile tbl and get details of login user's
        /// added by ashwin
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ResponseResult GetServicePartnerByUserId(string email, string password)
        {
            bool flagemail = false;
            bool flagpassword = false;
            ResponseResult responseResult = new ResponseResult();
            try
            {
                #region check email is user is exits or not
                if (email != null && password.Length > 0)
                {
                    var checkemail = _context.TblLoginMobiles.ToList();

                    flagemail = checkemail.Exists(p => p.Username == email);
                    flagpassword = checkemail.Exists(p => p.Password == password);
                    if (flagemail == true && flagpassword == true)
                    {
                        TblLoginMobile data = _login_MobileRepository.GetSingle(x => x.Username == email && x.Password == password);
                        if (data.IsActive == true && data.UserId > 0)
                        {
                            #region filled data of user
                            if (data.UserId > 0 && data.UserRoleName == "Service Partner")
                            {
                                TblServicePartner tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.UserId == data.UserId);
                                if (tblServicePartner.ServicePartnerId > 0)
                                {
                                    responseResult = ServicePartnerDetails(tblServicePartner.ServicePartnerId);
                                }
                                else
                                {
                                    responseResult.message = "User details not found";
                                    responseResult.Status = false;
                                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                                }
                            }
                            else if (data.UserId > 0 && data.UserRoleName == "Service Partner Driver")
                            {
                                TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetSingle(x => x.IsActive == true);
                                if (tblDriverDetail != null && tblDriverDetail.DriverDetailsId > 0)
                                {
                                    responseResult.Data = tblDriverDetail;
                                    responseResult.message = "Success";
                                    responseResult.Status = true;
                                    responseResult.Status_Code = HttpStatusCode.OK;
                                }
                                else
                                {
                                    responseResult.message = "User details not found";
                                    responseResult.Status = false;
                                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                                }
                            }
                            else
                            {
                                responseResult.message = "User details not found";
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.BadRequest;
                            }
                            #endregion
                        }
                        else if (data.IsActive == false && data.Id > 0 && data.UserRoleName != null)
                        {
                            responseResult.message = "Please wait for approval";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                        else
                        {
                            responseResult.message = "Invalid Username and Password";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseResult.message = "Invalid Username and Password";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("SevicePartnerManager", "IsValidMobileNumber", ex);
            }

            return responseResult;
        }

        #endregion

        #region Api Method Get ServicePartner Details
        /// <summary>
        /// get details of servicepartner on basis of service partner id
        /// added by ashwin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseResult ServicePartnerDetails(int id)
        {
            ResponseResult responseResult = new ResponseResult();
            TblServicePartner tblServicePartner = null;

            LGCUserViewDataModel lGCUserViewDataModel = new LGCUserViewDataModel();
            string imagepath = string.Empty;
            try
            {
                if (id > 0)
                {
                    tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerId == id);
                    if (tblServicePartner != null && tblServicePartner.ServicePartnerId > 0)
                    {
                        lGCUserViewDataModel = _mapper.Map<TblServicePartner, LGCUserViewDataModel>(tblServicePartner);
                        if (lGCUserViewDataModel != null && lGCUserViewDataModel.ServicePartnerId > 0)
                        {
                            #region Add images path
                            if (lGCUserViewDataModel.ServicePartnerAadharfrontImage != null && lGCUserViewDataModel.ServicePartnerAadharfrontImage != string.Empty)
                            {
                                imagepath = _baseConfig.Value.PostERPImagePath + _baseConfig.Value.ServicePartnerAadhar + lGCUserViewDataModel.ServicePartnerAadharfrontImage;

                                if (imagepath != string.Empty && imagepath != null)
                                {
                                    lGCUserViewDataModel.ServicePartnerAadharfrontImage = "";
                                    lGCUserViewDataModel.ServicePartnerAadharfrontImage = imagepath;
                                }
                                imagepath = string.Empty;
                            }

                            if (lGCUserViewDataModel.ServicePartnerAadharBackImage != null && lGCUserViewDataModel.ServicePartnerAadharBackImage != string.Empty)
                            {
                                imagepath = _baseConfig.Value.PostERPImagePath + _baseConfig.Value.ServicePartnerAadhar + lGCUserViewDataModel.ServicePartnerAadharBackImage;

                                if (imagepath != string.Empty && imagepath != null)
                                {
                                    lGCUserViewDataModel.ServicePartnerAadharBackImage = "";
                                    lGCUserViewDataModel.ServicePartnerAadharBackImage = imagepath;
                                }
                                imagepath = string.Empty;
                            }

                            if (lGCUserViewDataModel.ServicePartnerCancelledCheque != null && lGCUserViewDataModel.ServicePartnerCancelledCheque != string.Empty)
                            {
                                imagepath = _baseConfig.Value.PostERPImagePath + _baseConfig.Value.ServicePartnerCancelledCheque + lGCUserViewDataModel.ServicePartnerCancelledCheque;

                                if (imagepath != string.Empty && imagepath != null)
                                {
                                    lGCUserViewDataModel.ServicePartnerCancelledCheque = "";
                                    lGCUserViewDataModel.ServicePartnerCancelledCheque = imagepath;
                                }
                                imagepath = string.Empty;
                            }

                            if (lGCUserViewDataModel.ServicePartnerGstregisteration != null && lGCUserViewDataModel.ServicePartnerGstregisteration != string.Empty)
                            {
                                imagepath = _baseConfig.Value.PostERPImagePath + _baseConfig.Value.ServicePartnerGST + lGCUserViewDataModel.ServicePartnerGstregisteration;

                                if (imagepath != string.Empty && imagepath != null)
                                {
                                    lGCUserViewDataModel.ServicePartnerGstregisteration = "";
                                    lGCUserViewDataModel.ServicePartnerGstregisteration = imagepath;
                                }
                                imagepath = string.Empty;
                            }

                            if (lGCUserViewDataModel.ServicePartnerProfilePic != null && lGCUserViewDataModel.ServicePartnerProfilePic != string.Empty)
                            {
                                imagepath = _baseConfig.Value.PostERPImagePath + _baseConfig.Value.ServicePartnerProfilePic + lGCUserViewDataModel.ServicePartnerProfilePic;
                                if (imagepath != string.Empty && imagepath != null)
                                {
                                    lGCUserViewDataModel.ServicePartnerProfilePic = "";
                                    lGCUserViewDataModel.ServicePartnerProfilePic = imagepath;
                                }
                                imagepath = string.Empty;
                            }
                            lGCUserViewDataModel.NumofVehicle = _vehicleListRepository.GetList(x => x.IsActive == true && x.IsApproved == true && x.ApprovedBy == tblServicePartner.UserId).Count();
                            #endregion

                            responseResult.Data = lGCUserViewDataModel;
                            responseResult.message = "Success";
                            responseResult.Status_Code = HttpStatusCode.OK;
                            responseResult.Status = true;
                        }
                        else
                        {
                            responseResult.message = "Not Success,error occurs while mapping the data";
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            responseResult.Status = false;
                        }

                    }
                    else
                    {
                        responseResult.message = "Not Success,User Not Found";
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        responseResult.Status = false;
                    }
                }
                else
                {
                    responseResult.message = "Invalid Id";
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    responseResult.Status = false;
                }
                return responseResult;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("SevicePartnerManager", "ServicePartnerDetails", ex);
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                return responseResult;
            }
        }
        #endregion

        #region API for GetOrderListOfUninitiatedPaymentByUserId
        public ResponseResult GetOrderListOfUninitiatedPaymentByUserId(int userId, int? page, int? pageSize)
        {
            ResponseResult responseResult = new ResponseResult();
            TblPaymentLeaser tblPaymentLeaser = new TblPaymentLeaser();
            List<AllOrderlistViewModel> allOrderlistViewModels = new List<AllOrderlistViewModel>();
            try
            {
                TblDriverDetail tblDriverDetail = new TblDriverDetail();
                if (userId <= 0)
                {
                    responseResult.message = "Invalid Id";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.OK;
                    return responseResult;
                }

                tblDriverDetail = _driverDetailsRepository.GetSingle(x => x.IsActive == true && x.UserId == userId);
                if (tblDriverDetail == null || tblDriverDetail.UserId <= 0)
                {
                    responseResult.message = "User id not found";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.OK;
                    return responseResult;
                }

                List<TblOrderTran> tblOrderTrans = new List<TblOrderTran>();
                tblOrderTrans = _orderTransRepository.GetOrderDetailsByUserId(userId);

                if (page.HasValue && pageSize.HasValue && page > 0 && pageSize > 0)
                {
                    tblOrderTrans = tblOrderTrans
                       .OrderByDescending(x => x.ModifiedDate)
                       .Skip((page.Value - 1) * pageSize.Value)
                       .Take(pageSize.Value)
                       .ToList();
                }
                else
                {
                    tblOrderTrans = tblOrderTrans.ToList();
                }

                if (tblOrderTrans == null || tblOrderTrans.Count == 0)
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.OK;
                    responseResult.message = "No data found";
                    return responseResult;
                }

                foreach (var item in tblOrderTrans)
                {
                    if ((item.AmountPaidToCustomer == null || item.AmountPaidToCustomer == false) && item.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup))
                    {
                        tblPaymentLeaser = _paymentLeaser.GetPaymentdetails(item.RegdNo);
                        TblLogistic? tbllogistic = _logisticsRepository.GetExchangeDetailsByRegdno(item.RegdNo);
                        TblWalletTransaction? tblWalletTransaction = _logisticsRepository.GetEvcDetailsByOrdertranshId((int)item.OrderTransId);
                        if (tblPaymentLeaser == null)
                        {
                            AllOrderlistViewModel orderDetail = new AllOrderlistViewModel
                            {
                                OrderTransId = tbllogistic?.OrderTransId,
                                RegdNo = tbllogistic?.OrderTrans?.RegdNo,
                                StatusDesc = tbllogistic?.Status?.StatusDescription,
                                StatusId = tbllogistic?.StatusId,
                                AmtPaybleThroughLGC = (int?)(tbllogistic?.AmtPaybleThroughLgc ?? 0),
                                TicketNumber = tbllogistic?.TicketNumber,
                                PickupScheduleDate = tbllogistic?.PickupScheduleDate?.ToString(),
                            };
                            if (tbllogistic?.OrderTrans?.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                            {
                                orderDetail.ProductCategory = tbllogistic.OrderTrans?.Exchange?.ProductType?.ProductCat?.Description;
                                orderDetail.ProductType = tbllogistic.OrderTrans?.Exchange?.ProductType?.DescriptionForAbb;
                                orderDetail.CustomerName = $"{tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.FirstName} {tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.LastName}";
                                orderDetail.FirstName = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.FirstName;
                                orderDetail.LastName = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.LastName;
                                orderDetail.Email = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.Email;
                                orderDetail.City = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.City;
                                orderDetail.ZipCode = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.ZipCode;
                                orderDetail.Address1 = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.Address1;
                                orderDetail.Address2 = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.Address2 ?? "";
                                orderDetail.PhoneNumber = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.PhoneNumber;
                                orderDetail.State = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.State;
                                orderDetail.IsDefferedSettlement = tbllogistic?.OrderTrans?.Exchange?.IsDefferedSettlement == true ? true : false;
                            }

                            if (tbllogistic?.OrderTrans?.OrderType == Convert.ToInt32(OrderTypeEnum.ABB))
                            {
                                orderDetail.ProductCategory = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.NewProductCategory?.Description;
                                orderDetail.ProductType = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation?.Description;
                                orderDetail.CustomerName = $"{tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustFirstName} {tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustLastName}";
                                orderDetail.FirstName = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustFirstName;
                                orderDetail.LastName = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustLastName;
                                orderDetail.Email = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustEmail;
                                orderDetail.City = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustCity;
                                orderDetail.ZipCode = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustPinCode;
                                orderDetail.Address1 = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustAddress1;
                                orderDetail.Address2 = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustAddress2 ?? "";
                                orderDetail.PhoneNumber = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustMobile;
                                orderDetail.State = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustState;
                                orderDetail.IsDefferedSettlement = tbllogistic?.OrderTrans?.Abbredemption?.IsDefferedSettelment == true ? true : false;

                            }

                            if (tblWalletTransaction != null && tblWalletTransaction.Evcregistration != null)
                            {
                                orderDetail.EvcData = new EVCResellerModel
                                {
                                    EvcregistrationId = tblWalletTransaction?.Evcregistration?.EvcregistrationId,
                                    EvcPartnerId = tblWalletTransaction?.Evcpartner?.EvcPartnerId,
                                    EVCStoreCode = tblWalletTransaction?.Evcpartner?.EvcStoreCode,
                                    BussinessName = tblWalletTransaction?.Evcregistration?.BussinessName,
                                    ContactPerson = tblWalletTransaction?.Evcregistration?.ContactPerson,
                                    EvcmobileNumber = tblWalletTransaction?.Evcpartner?.ContactNumber,
                                    AlternateMobileNumber = tblWalletTransaction?.Evcregistration?.EvcmobileNumber ?? "",
                                    EmailId = tblWalletTransaction?.Evcpartner?.EmailId,
                                    RegdAddressLine1 = tblWalletTransaction?.Evcpartner?.Address1,
                                    RegdAddressLine2 = tblWalletTransaction?.Evcpartner?.Address2,
                                    City = tblWalletTransaction?.Evcpartner?.City?.Name,
                                    State = tblWalletTransaction?.Evcpartner?.State?.Name,
                                    PinCode = tblWalletTransaction?.Evcpartner?.PinCode
                                };
                            }

                            if (tbllogistic?.DriverDetailsId > 0)
                            {
                                TblDriverDetail tblDriverDetail1 = _driverDetailsRepository.GetSingle(x => x.IsActive == true && x.DriverDetailsId == tbllogistic.DriverDetailsId);

                                orderDetail.DriverData = new DriverDetailsModel
                                {
                                    DriverDetailsId = tblDriverDetail?.DriverDetailsId,
                                    DriverName = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverName : tblDriverDetail?.Driver?.DriverName,
                                    DriverPhoneNumber = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverPhoneNumber : tblDriverDetail?.Driver?.DriverPhoneNumber,
                                    VehicleNumber = tblDriverDetail?.Vehicle == null ? tblDriverDetail?.VehicleNumber : tblDriverDetail?.Vehicle?.VehicleNumber,
                                    City = tblDriverDetail?.Driver == null ? tblDriverDetail?.City : tblDriverDetail?.Driver?.City?.Name,
                                    VehicleRcNumber = tblDriverDetail?.Vehicle == null ? tblDriverDetail?.VehicleRcNumber : tblDriverDetail?.Vehicle?.VehicleRcNumber,
                                    DriverlicenseNumber = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverlicenseNumber : tblDriverDetail?.Driver?.DriverLicenseNumber,
                                    CityId = tblDriverDetail?.Driver == null ? tblDriverDetail?.CityId : tblDriverDetail?.Driver?.CityId,
                                    ServicePartnerId = tblDriverDetail?.ServicePartnerId,
                                    CreatedDate = tblDriverDetail?.CreatedDate,
                                    ModifiedDate = tblDriverDetail?.ModifiedDate,
                                    DriverId = tblDriverDetail?.DriverId,
                                    VehicleId = tblDriverDetail?.VehicleId,
                                    JourneyPlannedDate = (tblDriverDetail?.JourneyPlanDate != null ? (DateTime)tblDriverDetail?.JourneyPlanDate : DateTime.MinValue),
                                    ProfilePicture = tblDriverDetail?.Driver != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Driver?.ProfilePicture : string.Empty,
                                    DriverlicenseImage = tblDriverDetail?.Driver != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Driver?.DriverLicenseImage : string.Empty,
                                    VehicleInsuranceCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehicleInsuranceCertificate : string.Empty,
                                    VehiclefitnessCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehiclefitnessCertificate : string.Empty,
                                    VehicleRcCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehicleRcCertificate : string.Empty,
                                };
                            }
                            if (orderDetail.IsDefferedSettlement == true)
                            {
                                allOrderlistViewModels.Add(orderDetail);
                            }
                        }
                    }
                }
                if (allOrderlistViewModels.Count > 0)
                {
                    AllOrderList allOrderList = new AllOrderList
                    {
                        AllOrderlistViewModels = allOrderlistViewModels
                    };
                    responseResult.message = "Details found";
                    responseResult.Status = true;
                    responseResult.Status_Code = HttpStatusCode.OK;
                    responseResult.Data = allOrderList;
                    return responseResult;
                }
                else
                {
                    responseResult.message = "No data found";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.OK;
                    return responseResult;
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ServicePartnerManager", "GetOrderListOfUninitiatedPaymentByUserId", ex);
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                return responseResult;
            }
            return responseResult;
        }
        #endregion

        #region Add LGC Driver by api
        /// <summary>
        /// Register LGC Driver
        /// Used in api of RegisterVehicle in mobile development
        /// Added by ashwin
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public ResponseResult AddDriver([FromForm] DriverDataModel dataModel)
        {
            TblDriverList tblDriverList = null;
            bool cancelCheck = false;
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;

            try
            {
                if (dataModel != null)
                {
                    tblDriverList = _mapper.Map<DriverDataModel, TblDriverList>(dataModel);
                    //  TblDriverList tblDriver = _driverListRepository.GetSingle(x => x.IsActive == true && x.DriverPhoneNumber == dataModel.DriverPhoneNumber);

                    if (tblDriverList != null)
                    {
                        // Check email & mobile number is exists or not
                        var checkEmail = _context.TblDriverDetails.ToList();
                        bool numberFlag = checkEmail.Exists(p => p.DriverPhoneNumber == dataModel.DriverPhoneNumber);

                        if (numberFlag)
                        {
                            responseMessage.message = "This Mobile Number is Already Exists";
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                            return responseMessage;
                        }
                        else
                        {
                            var TBlUser = _context.TblUsers.Where(x => x.IsActive == true).ToList();
                            var number = SecurityHelper.EncryptString(dataModel.DriverPhoneNumber, _baseConfig.Value.SecurityKey);
                            bool NumberFlagUser = TBlUser.Exists(p => p.Phone == number);
                            if (NumberFlagUser)
                            {
                                TblServicePartner tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerIsApprovrd == true && x.ServicePartnerId == dataModel.ServicePartnerId && x.ServicePartnerMobileNumber == dataModel.DriverPhoneNumber).FirstOrDefault();
                                if (tblServicePartner == null)
                                {
                                    responseMessage.message = "This Mobile Number is Already Exists";
                                    responseMessage.Status = false;
                                    responseMessage.Status_Code = HttpStatusCode.OK;
                                    return responseMessage;
                                }
                            }
                        }

                        // Add Image in Folder
                        // DriverlicenseImage
                        if (dataModel.DriverlicenseImage != null)
                        {
                            string fileName = string.Empty;
                            if (tblDriverList.DriverLicenseImage != null)
                            {
                                fileName = tblDriverList.DriverLicenseNumber + "DriverlicenseImage" + ".jpg";
                            }
                            else
                            {
                                fileName = tblDriverList.DriverPhoneNumber + "DriverlicenseImage" + ".jpg";
                            }
                            string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\Driver\\DriversLiscense";
                            cancelCheck = _imageHelper.SaveFileDefRoot(dataModel.DriverlicenseImage, filePath, fileName);
                            if (cancelCheck)
                            {
                                tblDriverList.DriverLicenseImage = fileName;
                                cancelCheck = false;
                            }
                        }

                        // ProfilePicture
                        if (dataModel.ProfilePicture != null)
                        {
                            string fileName = string.Empty;
                            if (tblDriverList.ProfilePicture != null)
                            {
                                fileName = tblDriverList.DriverPhoneNumber + "DriverProfilePicture" + ".jpg";
                            }
                            else
                            {
                                fileName = tblDriverList.DriverPhoneNumber + "DriverProfilePicture" + ".jpg";
                            }

                            string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\Driver\\ProfilePic";
                            cancelCheck = _imageHelper.SaveFileDefRoot(dataModel.ProfilePicture, filePath, fileName);
                            if (cancelCheck)
                            {
                                tblDriverList.ProfilePicture = fileName;
                                cancelCheck = false;
                            }
                        }

                        tblDriverList.IsActive = true;
                        tblDriverList.CreatedDate = _currentDatetime;
                        tblDriverList.CreatedBy = dataModel.SPuserId;
                        tblDriverList.IsApproved = true;
                        tblDriverList.ApprovedBy = dataModel.SPuserId;

                        _driverListRepository.Create(tblDriverList);
                        int result = _driverListRepository.SaveChanges();

                        if (result > 0)
                        {
                            int responseUserId = ManageServicePartnerDriverUser(tblDriverList);

                            // Update userID in DriverDetails
                            if (responseUserId > 0)
                            {
                                tblDriverList.UserId = responseUserId;
                                tblDriverList.DriverPhoneNumber = SecurityHelper.DecryptString(tblDriverList.DriverPhoneNumber, _baseConfig.Value.SecurityKey);
                                _driverListRepository.Update(tblDriverList);
                                int updateResult = _driverListRepository.SaveChanges();

                                if (updateResult > 0)
                                {
                                    responseMessage.message = "Added Success";
                                    responseMessage.Status = true;
                                    responseMessage.Status_Code = HttpStatusCode.OK;
                                }
                                else
                                {
                                    responseMessage.message = "Driver Added but User not Created";
                                    responseMessage.Status = false;
                                    responseMessage.Status_Code = HttpStatusCode.OK;
                                }
                            }
                            else
                            {
                                responseMessage.Status = false;
                                responseMessage.Status_Code = HttpStatusCode.OK;
                                responseMessage.message = "Registration Failed";
                            }
                        }
                        else
                        {
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                            responseMessage.message = "Registration Failed";
                        }
                    }
                    else
                    {
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.OK;
                        responseMessage.message = "Registration Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("LogisticManager", "AddVehicle", ex);
            }

            return responseMessage;
        }
        #endregion

        #region Update UpdateDriver by api Added by Kranti
        /// <summary>
        /// Update LGC Vehichle
        /// Used in api of UpdateVehicle in mobile development
        /// Added by Kranti
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public ResponseResult UpdateDriver([FromForm] UpdateDriverDataModel dataModel)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            bool cancelCheck = false;
            try
            {
                if (dataModel == null)
                {
                    responseMessage.message = "Data model is null";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }

                TblDriverList ObjDriverDetail = _driverListRepository.GetSingle(x => x.IsActive == true && x.DriverId == dataModel.DriverId && x.ServicePartnerId == dataModel.ServicePartnerId);
                if (ObjDriverDetail == null)
                {
                    responseMessage.message = "User not found";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    return responseMessage;
                }

                // Check if driver phone number is being updated and if it already exists
                if (dataModel.DriverPhoneNumber != null && dataModel.ServicePartnerId > 0 && dataModel.DriverPhoneNumber != ObjDriverDetail.DriverPhoneNumber)
                {
                    bool numberExists = _context.TblDriverLists.Any(x => x.DriverId != ObjDriverDetail.DriverId && x.DriverPhoneNumber == dataModel.DriverPhoneNumber);
                    if (numberExists)
                    {
                        responseMessage.message = "This Mobile Number is Already Exists";
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.OK;
                        return responseMessage;
                    }

                    var encryptedPhoneNumber = SecurityHelper.EncryptString(dataModel.DriverPhoneNumber, _baseConfig.Value.SecurityKey);
                    bool numberExistsInUsers = _context.TblUsers.Any(x => x.IsActive == true && x.Phone == encryptedPhoneNumber);
                    if (numberExistsInUsers)
                    {
                        var servicePartner = _context.TblServicePartners.FirstOrDefault(x => x.IsActive == true && x.ServicePartnerIsApprovrd == true && x.ServicePartnerId == dataModel.ServicePartnerId && x.ServicePartnerMobileNumber == dataModel.DriverPhoneNumber);
                        if (servicePartner == null)
                        {
                            responseMessage.message = "This Mobile Number is Already Exists";
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                            return responseMessage;
                        }
                    }
                }

                // Update driver details
                if (dataModel.DriverName != null)
                    ObjDriverDetail.DriverName = dataModel.DriverName;

                if (dataModel.DriverPhoneNumber != null)
                    ObjDriverDetail.DriverPhoneNumber = dataModel.DriverPhoneNumber;

                if (dataModel.CityId != null)
                    ObjDriverDetail.CityId = dataModel.CityId;

                if (dataModel.DriverlicenseNumber != null)
                    ObjDriverDetail.DriverLicenseNumber = dataModel.DriverlicenseNumber;

                // Update images
                if (dataModel.DriverlicenseImage != null)
                {
                    string fileName = string.Empty;
                    if (ObjDriverDetail.DriverLicenseNumber != null)
                    {
                        fileName = ObjDriverDetail.DriverLicenseNumber + "DriverlicenseImage" + ".jpg";
                    }
                    else
                    {
                        fileName = ObjDriverDetail.DriverPhoneNumber + "DriverlicenseImage" + ".jpg";
                    }
                    string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\Driver\\DriversLiscense";
                    cancelCheck = _imageHelper.SaveFileDefRoot(dataModel.DriverlicenseImage, filePath, fileName);
                    if (cancelCheck)
                    {
                        ObjDriverDetail.DriverLicenseImage = fileName;
                        cancelCheck = false;
                    }
                }

                if (dataModel.ProfilePicture != null)
                {
                    string fileName = string.Empty;
                    if (ObjDriverDetail.ProfilePicture != null)
                    {
                        fileName = ObjDriverDetail.DriverPhoneNumber + "DriverProfilePicture" + ".jpg";
                    }
                    else
                    {
                        fileName = ObjDriverDetail.DriverPhoneNumber + "DriverProfilePicture" + ".jpg";
                    }
                    string filePath = _baseConfig.Value.WebCoreBaseUrl + "ServicePartner\\Driver\\ProfilePic";
                    cancelCheck = _imageHelper.SaveFileDefRoot(dataModel.ProfilePicture, filePath, fileName);
                    if (cancelCheck)
                    {
                        ObjDriverDetail.ProfilePicture = fileName;
                        cancelCheck = false;
                    }
                }

                ObjDriverDetail.IsActive = true;
                ObjDriverDetail.ModifiedDate = _currentDatetime;

                _context.Entry(ObjDriverDetail).State = EntityState.Modified;
                int result = _context.SaveChanges();

                // Update user phone number if necessary
                if (result > 0 && dataModel.DriverPhoneNumber != null)
                {
                    var userId = ObjDriverDetail.UserId;
                    var phoneNumber = dataModel.DriverPhoneNumber;
                    var tblUser = _context.TblUsers.FirstOrDefault(x => x.UserId == userId && x.IsActive == true);
                    if (tblUser != null)
                    {
                        tblUser.Phone = SecurityHelper.EncryptString(phoneNumber, _baseConfig.Value.SecurityKey);
                        _userRepository.Update(tblUser);
                        _userRepository.SaveChanges();
                    }
                }

                responseMessage.message = result > 0 ? "Update Success" : "Update Failed";
                responseMessage.Status = result > 0;
                responseMessage.Status_Code = result > 0 ? HttpStatusCode.OK : HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("LogisticManager", "UpdateVehicle", ex);
            }

            return responseMessage;
        }

        #endregion

        #endregion

        #region use in ERP Added by Kranti 

        //Added by Kranti
        /// <summary>
        /// Method to manage (Add/Edit) ServicePartner 
        /// </summary>
        /// <param name="ServicePartnerVM">ServicePartnerVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageSevicePartner(ServicePartnerViewModel SevicePartnerVM, int userId)
        {
            TblServicePartner TblServicePartner = new TblServicePartner();
            List<TblPinCode> TblPinCode = new List<TblPinCode>();
            TblCity TblCities = new TblCity();
            UserViewModel userVM = null;
            List<MapServicePartnerCityState> MapServicePartnerCityStateId = new List<MapServicePartnerCityState>();
            TblState TblStates = new TblState();

            try
            {
                if (SevicePartnerVM != null)
                {
                    TblServicePartner = _mapper.Map<ServicePartnerViewModel, TblServicePartner>(SevicePartnerVM);

                    if (TblServicePartner.ServicePartnerId > 0)
                    {


                        //Code to update the object
                        TblServicePartner.ServicePartnerName = SevicePartnerVM.ServicePartnerFirstName + " " + SevicePartnerVM.ServicePartnerLastName;

                        TblServicePartner.Modifiedby = userId;
                        TblServicePartner.ModifiedDate = _currentDatetime;
                        TblServicePartner = TrimHelper.TrimAllValuesInModel(TblServicePartner);
                        _servicePartnerRepository.Update(TblServicePartner);
                        _servicePartnerRepository.SaveChanges();

                        // Code to update the object in MapServicePartnerCityState Table
                        //Added by Kranti

                        string Values = string.Join(",", Array.ConvertAll(SevicePartnerVM.ServicePartnerCities, x => x.ToString()));
                        string[] ValueList = Values.Split(",");
                        string ValuesState = string.Join(",", Array.ConvertAll(SevicePartnerVM.ServicePartnerStates, x => x.ToString()));
                        string[] ValueListState = ValuesState.Split(",");


                        MapServicePartnerCityState mapServicePartners = new MapServicePartnerCityState();
                        for (int i = 0; i < ValueList.Count(); i++)
                        {

                            if (ValueList.Count() > 0)
                            {
                                TblCities = _context.TblCities.Where(x => x.Name == ValueList[i]).FirstOrDefault();
                                TblPinCode = _context.TblPinCodes.Where(x => x.CityId == TblCities.CityId).ToList();
                                TblStates = _context.TblStates.Where(x => x.StateId == TblCities.StateId).FirstOrDefault();
                                IList<MapServicePartnerCityState> MapServicePartnerCityStatelist = null;
                                MapServicePartnerCityStatelist = _context.MapServicePartnerCityStates.Where(x => x.ServicePartnerId == TblServicePartner.ServicePartnerId).ToList();

                                //int CityMap = (int)MapServicePartnerCityStatelist[i].CityId;
                                int CityId = TblCities.CityId;
                                bool cityExists = MapServicePartnerCityStatelist.Any(city => city.CityId == CityId);

                                if (!cityExists)
                                {
                                    if (TblPinCode.Count > 0)
                                    {

                                        string str = string.Empty;

                                        //loop to add comma separated zipcode
                                        foreach (var lst in TblPinCode)
                                            str = str + lst.ZipCode + ",";

                                        str = str.Remove(str.Length - 1);

                                        mapServicePartners.ListOfPincodes = str;
                                    }
                                    else
                                    {

                                        mapServicePartners.ListOfPincodes = null;
                                    }
                                    MapServicePartnerCityState mapServicePartnerCityStates = new MapServicePartnerCityState();


                                    mapServicePartnerCityStates.ServicePartnerId = TblServicePartner.ServicePartnerId;
                                    mapServicePartnerCityStates.CityId = TblCities.CityId;
                                    mapServicePartnerCityStates.StateId = TblStates.StateId;
                                    mapServicePartnerCityStates.ListOfPincodes = mapServicePartners.ListOfPincodes;
                                    mapServicePartnerCityStates.IsActive = true;
                                    mapServicePartnerCityStates.CreatedBy = userId;
                                    mapServicePartnerCityStates.CreatedDate = _currentDatetime;

                                    _mapServicePartnerCityStateRepository.Create(mapServicePartnerCityStates);
                                    _mapServicePartnerCityStateRepository.SaveChanges();


                                }
                                else
                                {
                                    if (TblPinCode.Count > 0)
                                    {

                                        string str = string.Empty;

                                        //loop to add comma separated zipcode
                                        foreach (var lst in TblPinCode)
                                            str = str + lst.ZipCode + ",";

                                        str = str.Remove(str.Length - 1);

                                        mapServicePartners.ListOfPincodes = str;
                                    }
                                    else
                                    {

                                        mapServicePartners.ListOfPincodes = null;
                                    }
                                    MapServicePartnerCityState mapServicePartnerCityState = new MapServicePartnerCityState();
                                    mapServicePartnerCityState.ServicePartnerId = TblServicePartner.ServicePartnerId;
                                    mapServicePartnerCityState.CityId = TblCities.CityId;
                                    mapServicePartnerCityState.StateId = TblStates.StateId;
                                    mapServicePartnerCityState.ListOfPincodes = mapServicePartners.ListOfPincodes;
                                    mapServicePartnerCityState.IsActive = true;
                                    mapServicePartnerCityState.ModifiedBy = userId;
                                    mapServicePartnerCityState.ModifiedDate = _currentDatetime;

                                    mapServicePartnerCityState.MapServicePartnerCityStateId = MapServicePartnerCityStatelist[i].MapServicePartnerCityStateId;
                                    _mapServicePartnerCityStateRepository.Update(mapServicePartnerCityState);
                                    _mapServicePartnerCityStateRepository.SaveChanges();

                                }


                            }
                        }

                    }
                    else
                    {

                        // Code to Insert the object in Service Partner Table
                        //Added by Kranti


                        TblServicePartner.ServicePartnerName = SevicePartnerVM.ServicePartnerFirstName + " " + SevicePartnerVM.ServicePartnerLastName;
                        TblServicePartner.ServicePartnerIsApprovrd = true;
                        TblServicePartner.IsActive = true;
                        TblServicePartner.CreatedDate = _currentDatetime;
                        TblServicePartner.CreatedBy = userId;
                        TblServicePartner = TrimHelper.TrimAllValuesInModel(TblServicePartner);
                        _servicePartnerRepository.Create(TblServicePartner);

                        userVM = new UserViewModel();
                        #region Create User and Role for Service Partner 
                        userVM = new UserViewModel();
                        userVM.Name = TblServicePartner.ServicePartnerName;
                        userVM.Email = TblServicePartner.ServicePartnerEmailId;
                        userVM.Phone = TblServicePartner.ServicePartnerMobileNumber;
                        userVM.Password = _baseConfig.Value.LGCLoginPossword;
                        if (SevicePartnerVM.Selected == true)
                        {
                            userVM.RoleName = EnumHelper.DescriptionAttr(RoleEnum.EVCLGC);
                        }
                        else
                        {
                            userVM.RoleName = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner);
                        }


                        userVM.MailTemplate = EmailTemplateConstant.ServicePartnerUser;
                        userVM.MailSubject = "Welcome Mail";
                        int BpResult = _manageUserForServicePartnerManager.ManageUserAndUserRole(userVM, userId);
                        TblServicePartner.UserId = BpResult;
                        _servicePartnerRepository.SaveChanges();
                        #endregion


                        // Code to insert the object in MapServicePartnerCityState Table
                        //Added by Kranti
                        if (SevicePartnerVM.ServicePartnerCities != null && SevicePartnerVM.ServicePartnerId == 0)
                        {
                            string ValuesCity = string.Join(",", Array.ConvertAll(SevicePartnerVM.ServicePartnerCities, x => x.ToString()));
                            string[] ValuesCityList = ValuesCity.Split(",");
                            string ValuesStates = string.Join(",", Array.ConvertAll(SevicePartnerVM.ServicePartnerStates, x => x.ToString()));
                            string[] ValueListStateList = ValuesStates.Split(",");
                            MapServicePartnerCityState mapServicePartner = new MapServicePartnerCityState();
                            for (int i = 0; i < ValuesCityList.Count(); i++)
                            {
                                TblCities = _context.TblCities.Where(x => x.Name == ValuesCityList[i]).FirstOrDefault();
                                TblPinCode = _context.TblPinCodes.Where(x => x.CityId == TblCities.CityId).ToList();
                                TblStates = _context.TblStates.Where(x => x.StateId == TblCities.StateId).FirstOrDefault();

                                if (ValuesCityList.Count() > 0)
                                {
                                    if (TblPinCode.Count > 0)
                                    {

                                        string str = string.Empty;

                                        //loop to add comma separated zipcode
                                        foreach (var lst in TblPinCode)
                                            str = str + lst.ZipCode + ",";

                                        str = str.Remove(str.Length - 1);

                                        mapServicePartner.ListOfPincodes = str;
                                    }
                                    else
                                    {

                                        mapServicePartner.ListOfPincodes = null;
                                    }

                                    MapServicePartnerCityState mapServicePartnerCityStates = new MapServicePartnerCityState();
                                    mapServicePartnerCityStates.ServicePartnerId = TblServicePartner.ServicePartnerId;
                                    mapServicePartnerCityStates.CityId = TblCities.CityId;
                                    mapServicePartnerCityStates.StateId = TblStates.StateId;

                                    mapServicePartnerCityStates.ListOfPincodes = mapServicePartner.ListOfPincodes;

                                    mapServicePartnerCityStates.IsActive = true;
                                    mapServicePartnerCityStates.CreatedBy = userId;
                                    mapServicePartnerCityStates.CreatedDate = _currentDatetime;

                                    _mapServicePartnerCityStateRepository.Create(mapServicePartnerCityStates);
                                    _mapServicePartnerCityStateRepository.SaveChanges();

                                }
                            }



                        }



                    }
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ServicePartnerManager", "ManageServicePartner", ex);
            }

            return TblServicePartner.ServicePartnerId;
        }

        //Added by Kranti

        // Method to Bulk Upload (Add/Edit) ServicePartner 

        public ServicePartnerViewModel ManageSevicePartnerBulk(ServicePartnerViewModel SevicePartnerVM, int userId)
        {
            TblServicePartner TblServicePartner = new TblServicePartner();
            List<TblPinCode> TblPinCode = new List<TblPinCode>();
            TblCity TblCities = new TblCity();
            UserViewModel userVM = null;
            List<MapServicePartnerCityState> MapServicePartnerCityStateId = new List<MapServicePartnerCityState>();
            TblState TblStates = new TblState();
            int BpResult = 0;
            if (SevicePartnerVM != null && SevicePartnerVM.ServicePartnerVMList != null && SevicePartnerVM.ServicePartnerVMList.Count > 0)
            {
                var ValidatedServicePartnerList = SevicePartnerVM.ServicePartnerVMList.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
                SevicePartnerVM.ServicePartnerVMErrorList = SevicePartnerVM.ServicePartnerVMList.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();
                TblServicePartner tblServicePartner = new TblServicePartner();
                //tblBusinessPartner = _mapper.Map<List<BusinessPartnerVMExcelModel>, List<TblBusinessPartner>>(ValidatedBusinessPartnerList);
                if (ValidatedServicePartnerList != null && ValidatedServicePartnerList.Count > 0)
                {
                    foreach (var item in ValidatedServicePartnerList)
                    {
                        try
                        {
                            if (SevicePartnerVM != null)
                            {

                                if (item.ServicePartnerId > 0)
                                {

                                    //Code to update the object
                                    tblServicePartner.ServicePartnerName = item.ServicePartnerFirstName + " " + item.ServicePartnerLastName;
                                    tblServicePartner.ServicePartnerBusinessName = item.ServicePartnerBusinessName;
                                    tblServicePartner.ServicePartnerFirstName = item.ServicePartnerFirstName;
                                    tblServicePartner.ServicePartnerLastName = item.ServicePartnerLastName;
                                    tblServicePartner.ServicePartnerGstno = item.ServicePartnerGstno;
                                    tblServicePartner.ServicePartnerDescription = item.ServicePartnerDescription;
                                    tblServicePartner.ServicePartnerEmailId = item.ServicePartnerEmailId;
                                    tblServicePartner.ServicePartnerMobileNumber = item.ServicePartnerMobileNumber;
                                    tblServicePartner.ServicePartnerAlternateMobileNumber = item.ServicePartnerAlternateMobileNumber;
                                    tblServicePartner.ServicePartnerAddressLine1 = item.ServicePartnerAddressLine1;
                                    tblServicePartner.ServicePartnerAddressLine2 = item.ServicePartnerAddressLine2;
                                    tblServicePartner.IsServicePartnerLocal = item.IsServicePartnerLocal;
                                    tblServicePartner.IconfirmTermsCondition = item.IconfirmTermsCondition;
                                    tblServicePartner.IsActive = item.IsActive;
                                    tblServicePartner.Modifiedby = userId;
                                    tblServicePartner.ModifiedDate = _currentDatetime;
                                    _servicePartnerRepository.Update(tblServicePartner);
                                    _servicePartnerRepository.SaveChanges();

                                    // Code to update the object in MapServicePartnerCityState Table
                                    //Added by Kranti

                                    string Values = item.ServicePartnerCities;
                                    string[] ValueList = Values.Split(",");
                                    string ValuesState = item.ServicePartnerStates;
                                    string[] ValueListState = ValuesState.Split(",");


                                    MapServicePartnerCityState mapServicePartners = new MapServicePartnerCityState();
                                    for (int i = 0; i < ValueList.Count(); i++)
                                    {

                                        if (ValueList.Count() > 0)
                                        {
                                            TblCities = _context.TblCities.Where(x => x.Name == ValueList[i]).FirstOrDefault();
                                            TblPinCode = _context.TblPinCodes.Where(x => x.CityId == TblCities.CityId).ToList();
                                            TblStates = _context.TblStates.Where(x => x.StateId == TblCities.StateId).FirstOrDefault();
                                            IList<MapServicePartnerCityState> MapServicePartnerCityStatelist = null;
                                            MapServicePartnerCityStatelist = _context.MapServicePartnerCityStates.Where(x => x.ServicePartnerId == TblServicePartner.ServicePartnerId).ToList();

                                            int CityMap = (int)MapServicePartnerCityStatelist[i].CityId;
                                            int CityId = TblCities.CityId;
                                            bool cityExists = MapServicePartnerCityStatelist.Any(city => city.CityId == CityId);

                                            if (!cityExists)
                                            {
                                                if (TblPinCode.Count > 0)
                                                {

                                                    string str = string.Empty;

                                                    //loop to add comma separated zipcode
                                                    foreach (var lst in TblPinCode)
                                                        str = str + lst.ZipCode + ",";

                                                    str = str.Remove(str.Length - 1);

                                                    mapServicePartners.ListOfPincodes = str;
                                                }
                                                else
                                                {

                                                    mapServicePartners.ListOfPincodes = null;
                                                }
                                                MapServicePartnerCityState mapServicePartnerCityStates = new MapServicePartnerCityState();
                                                mapServicePartnerCityStates.ServicePartnerId = tblServicePartner.ServicePartnerId;
                                                mapServicePartnerCityStates.CityId = TblCities.CityId;
                                                mapServicePartnerCityStates.StateId = TblStates.StateId;
                                                mapServicePartnerCityStates.ListOfPincodes = mapServicePartners.ListOfPincodes;
                                                mapServicePartnerCityStates.IsActive = true;
                                                mapServicePartnerCityStates.CreatedBy = userId;
                                                mapServicePartnerCityStates.CreatedDate = _currentDatetime;

                                                _mapServicePartnerCityStateRepository.Create(mapServicePartnerCityStates);
                                                _mapServicePartnerCityStateRepository.SaveChanges();


                                            }
                                            else
                                            {
                                                if (TblPinCode.Count > 0)
                                                {

                                                    string str = string.Empty;

                                                    //loop to add comma separated zipcode
                                                    foreach (var lst in TblPinCode)
                                                        str = str + lst.ZipCode + ",";

                                                    str = str.Remove(str.Length - 1);

                                                    mapServicePartners.ListOfPincodes = str;
                                                }
                                                else
                                                {

                                                    mapServicePartners.ListOfPincodes = null;
                                                }
                                                MapServicePartnerCityState mapServicePartnerCityState = new MapServicePartnerCityState();
                                                mapServicePartnerCityState.ServicePartnerId = tblServicePartner.ServicePartnerId;
                                                mapServicePartnerCityState.CityId = TblCities.CityId;
                                                mapServicePartnerCityState.StateId = TblStates.StateId;
                                                mapServicePartnerCityState.ListOfPincodes = mapServicePartners.ListOfPincodes;
                                                mapServicePartnerCityState.IsActive = true;
                                                mapServicePartnerCityState.ModifiedBy = userId;
                                                mapServicePartnerCityState.ModifiedDate = _currentDatetime;

                                                mapServicePartnerCityState.MapServicePartnerCityStateId = MapServicePartnerCityStatelist[i].MapServicePartnerCityStateId;
                                                _mapServicePartnerCityStateRepository.Update(mapServicePartnerCityState);
                                                _mapServicePartnerCityStateRepository.SaveChanges();

                                            }


                                        }
                                    }

                                }
                                else
                                {

                                    // Code to Insert the object in Service Partner Table
                                    //Added by Kranti
                                    var Check = _servicePartnerRepository.GetSingle(x => x.ServicePartnerEmailId == SevicePartnerVM.ServicePartnerEmailId);
                                    if (Check == null)
                                    {
                                        var Check1 = _servicePartnerRepository.GetSingle(x => x.ServicePartnerMobileNumber == SevicePartnerVM.ServicePartnerMobileNumber);
                                        if (Check1 == null)
                                        {
                                            tblServicePartner.ServicePartnerRegdNo = "SP" + UniqueString.RandomNumber();
                                            tblServicePartner.ServicePartnerName = item.ServicePartnerFirstName + " " + item.ServicePartnerLastName;
                                            tblServicePartner.ServicePartnerBusinessName = item.ServicePartnerBusinessName;
                                            tblServicePartner.ServicePartnerFirstName = item.ServicePartnerFirstName;
                                            tblServicePartner.ServicePartnerLastName = item.ServicePartnerLastName;
                                            tblServicePartner.ServicePartnerGstno = item.ServicePartnerGstno;
                                            tblServicePartner.ServicePartnerDescription = item.ServicePartnerDescription;
                                            tblServicePartner.ServicePartnerEmailId = item.ServicePartnerEmailId;
                                            tblServicePartner.ServicePartnerMobileNumber = item.ServicePartnerMobileNumber;
                                            tblServicePartner.ServicePartnerAlternateMobileNumber = item.ServicePartnerAlternateMobileNumber;
                                            tblServicePartner.ServicePartnerAddressLine1 = item.ServicePartnerAddressLine1;
                                            tblServicePartner.ServicePartnerAddressLine2 = item.ServicePartnerAddressLine2;
                                            tblServicePartner.IsServicePartnerLocal = item.IsServicePartnerLocal;
                                            tblServicePartner.IconfirmTermsCondition = item.IconfirmTermsCondition;
                                            tblServicePartner.IsActive = item.IsActive;
                                            tblServicePartner.CreatedDate = _currentDatetime;
                                            tblServicePartner.CreatedBy = userId;
                                            _servicePartnerRepository.Create(tblServicePartner);

                                            userVM = new UserViewModel();
                                            #region Create User and Role for Service Partner 
                                            userVM = new UserViewModel();
                                            userVM.Name = tblServicePartner.ServicePartnerName;
                                            userVM.Email = tblServicePartner.ServicePartnerEmailId;
                                            userVM.Phone = tblServicePartner.ServicePartnerMobileNumber;
                                            userVM.Password = "Digi2L@123";
                                            if (item.IsServicePartnerEVCalso == true)
                                            {
                                                userVM.RoleName = EnumHelper.DescriptionAttr(RoleEnum.EVCLGC);
                                            }
                                            else
                                            {
                                                userVM.RoleName = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner);
                                            }

                                            userVM.MailTemplate = EmailTemplateConstant.ServicePartnerUser;
                                            userVM.MailSubject = "Welcome Mail";
                                            BpResult = _manageUserForServicePartnerManager.ManageUserAndUserRole(userVM, userId);
                                            tblServicePartner.UserId = BpResult;
                                            _servicePartnerRepository.SaveChanges();
                                            #endregion
                                        }
                                    }



                                }




                                // Code to insert the object in MapServicePartnerCityState Table
                                //Added by Kranti
                                if (item.ServicePartnerCities != null && item.ServicePartnerId == 0)
                                {
                                    string ValuesCity = item.ServicePartnerCities;
                                    string[] ValuesCityList = ValuesCity.Split(",");
                                    string ValuesStates = item.ServicePartnerStates;
                                    string[] ValueListStateList = ValuesStates.Split(",");
                                    MapServicePartnerCityState mapServicePartner = new MapServicePartnerCityState();
                                    for (int i = 0; i < ValuesCityList.Count(); i++)
                                    {
                                        TblCities = _context.TblCities.Where(x => x.Name == ValuesCityList[i].Trim().ToLower()).FirstOrDefault();
                                        TblPinCode = _context.TblPinCodes.Where(x => x.CityId == TblCities.CityId).ToList();
                                        TblStates = _context.TblStates.Where(x => x.StateId == TblCities.StateId).FirstOrDefault();

                                        if (ValuesCityList.Count() > 0)
                                        {
                                            if (TblPinCode.Count > 0)
                                            {

                                                string str = string.Empty;

                                                //loop to add comma separated zipcode
                                                foreach (var lst in TblPinCode)
                                                    str = str + lst.ZipCode + ",";

                                                str = str.Remove(str.Length - 1);

                                                mapServicePartner.ListOfPincodes = str;
                                            }
                                            else
                                            {

                                                mapServicePartner.ListOfPincodes = null;
                                            }

                                            MapServicePartnerCityState mapServicePartnerCityStates = new MapServicePartnerCityState();
                                            mapServicePartnerCityStates.ServicePartnerId = tblServicePartner.ServicePartnerId;
                                            mapServicePartnerCityStates.CityId = TblCities.CityId;
                                            mapServicePartnerCityStates.StateId = TblStates.StateId;

                                            mapServicePartnerCityStates.ListOfPincodes = mapServicePartner.ListOfPincodes;

                                            mapServicePartnerCityStates.IsActive = true;
                                            mapServicePartnerCityStates.CreatedBy = userId;
                                            mapServicePartnerCityStates.CreatedDate = _currentDatetime;

                                            _mapServicePartnerCityStateRepository.Create(mapServicePartnerCityStates);
                                            _mapServicePartnerCityStateRepository.SaveChanges();

                                        }
                                    }


                                }

                            }

                        }


                        catch (Exception ex)
                        {
                            _logging.WriteErrorToDB("ServicePartnerManager", "ManageServicePartner", ex);
                        }


                    }
                }
            }
            return SevicePartnerVM;
        }

        /// <summary>
        /// Method to get the Service Partner by id 
        /// </summary>
        /// <param name="id">ServicePartnerId</param>
        /// <returns>ServicePartnerViewModel</returns>
        public ServicePartnerViewModel GetServicePartnerById(int id)
        {
            ServicePartnerViewModel SevicePartnerVM = null;
            TblServicePartner TblServicePartner = null;

            try
            {
                TblServicePartner = _servicePartnerRepository.GetSingle(where: x => x.IsActive == true && x.ServicePartnerId == id);
                if (TblServicePartner != null)
                {
                    SevicePartnerVM = _mapper.Map<TblServicePartner, ServicePartnerViewModel>(TblServicePartner);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ServicePartnerManager", "GetServicepartnerById", ex);
            }
            return SevicePartnerVM;
        }

        /// <summary>
        /// Method to get the MapServicePartnerCityState by id 
        /// </summary>
        /// <param name="id">ServicePartnerId</param>
        /// <returns>ServicePartnerViewModel</returns>
        public MapServicePartnerViewModel GetMapServicePartnerCityStateById(int id)
        {
            MapServicePartnerViewModel MapSevicePartnerVM = null;
            MapServicePartnerCityState MapServicePartnerCityState = null;

            try
            {
                MapServicePartnerCityState = _mapServicePartnerCityStateRepository.GetSingle(where: x => x.IsActive == true && x.MapServicePartnerCityStateId == id);
                if (MapServicePartnerCityState != null)
                {
                    MapSevicePartnerVM = _mapper.Map<MapServicePartnerCityState, MapServicePartnerViewModel>(MapServicePartnerCityState);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ServicePartnerManager", "GetServicepartnerById", ex);
            }
            return MapSevicePartnerVM;
        }

        /// <summary>
        /// Method to delete Service Partner by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletSevicePartnerById(int id)
        {
            bool flag = false;
            try
            {
                TblServicePartner TblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerId == id);
                if (TblServicePartner != null)
                {
                    TblServicePartner.IsActive = false;
                    _servicePartnerRepository.Update(TblServicePartner);
                    _servicePartnerRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ServicePartnerManager", "DeleteServicePartnerById", ex);
            }
            return flag;
        }

        /// <summary>
        /// Method to get the All Service Partner
        /// </summary>     
        /// <returns>ServicePartnerViewModel</returns>
        public IList<ServicePartnerViewModel> GetAllServicePartner()
        {
            IList<ServicePartnerViewModel> ServicePartnerVMList = null;
            List<TblServicePartner> TblServicePartnerlist = new List<TblServicePartner>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblServicePartnerlist = _servicePartnerRepository.GetList(x => x.IsActive == true).ToList();

                if (TblServicePartnerlist != null && TblServicePartnerlist.Count > 0)
                {
                    ServicePartnerVMList = _mapper.Map<IList<TblServicePartner>, IList<ServicePartnerViewModel>>(TblServicePartnerlist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ServicePartnerManager", "GetAllServicePartner", ex);
            }
            return ServicePartnerVMList;
        }
        #region Disable Driver by api Added by priyanshi
        /// <summary>
        /// Update LGC Vehichle
        /// Used in api of UpdateVehicle in mobile development
        /// Added by Kranti
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public ResponseResult DisableDriver(DisableDriverDataModel dataModel)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            bool cancelCheck = false;
            try
            {
                if (dataModel == null)
                {
                    responseMessage.message = "Data model is null";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }

                TblDriverList ObjDriverDetail = _driverListRepository.GetSingle(x => x.IsActive == true && x.DriverId == dataModel.DriverId && x.ServicePartnerId == dataModel.ServicePartnerId);
                if (ObjDriverDetail == null)
                {
                    responseMessage.message = "User not found";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    return responseMessage;
                }

                //bool isValid = _context.TblLogistics
                //       .Include(x => x.DriverDetails).ThenInclude(x => x.Driver)
                //       .Include(x => x.DriverDetails).ThenInclude(x => x.Vehicle)
                //       .Any(x => x.DriverDetails.DriverId == dataModel.DriverId && x.DriverDetailsId == x.DriverDetails.DriverDetailsId && x.StatusId != 32 && x.StatusId != 55 && x.StatusId != 22&&x.StatusId !=21 && x.StatusId != 26);

                List<TblLogistic> tblLogistics = _context.TblLogistics
                      .Include(x => x.DriverDetails).ThenInclude(x => x.Driver)
                      .Include(x => x.DriverDetails).ThenInclude(x => x.Vehicle)
                      .Where(x => x.DriverDetails.DriverId == dataModel.DriverId && x.DriverDetailsId == x.DriverDetails.DriverDetailsId && x.StatusId != 32 && x.StatusId != 55 && x.StatusId != 22 && x.StatusId != 21 && x.StatusId != 26).ToList();

                if (tblLogistics.Count > 0)
                {
                    List<DriverDetailsModel> DriverDetailsViewModal = new List<DriverDetailsModel>();
                    HashSet<int> addedDriverDetailsIds = new HashSet<int>();

                    foreach (TblLogistic item in tblLogistics)
                    {
                        TblLogistic? tbllogistic = _logisticsRepository.GetExchangeDetailsByRegdno(item.RegdNo);

                        if (tbllogistic != null && tbllogistic.DriverDetailsId > 0 && !addedDriverDetailsIds.Contains((int)tbllogistic.DriverDetailsId))
                        {
                            TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetDriverDetailsById((int)tbllogistic.DriverDetailsId);

                            DriverDetailsModel driverDetails = new DriverDetailsModel
                            {
                                DriverDetailsId = tblDriverDetail?.DriverDetailsId,
                                DriverName = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverName : tblDriverDetail?.Driver?.DriverName,
                                DriverPhoneNumber = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverPhoneNumber : tblDriverDetail?.Driver?.DriverPhoneNumber,
                                VehicleNumber = tblDriverDetail?.Vehicle == null ? tblDriverDetail?.VehicleNumber : tblDriverDetail?.Vehicle?.VehicleNumber,
                                City = tblDriverDetail?.Driver == null ? tblDriverDetail?.City : tblDriverDetail?.Driver?.City?.Name,
                                VehicleRcNumber = tblDriverDetail?.Vehicle == null ? tblDriverDetail?.VehicleRcNumber : tblDriverDetail?.Vehicle?.VehicleRcNumber,
                                DriverlicenseNumber = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverlicenseNumber : tblDriverDetail?.Driver?.DriverLicenseNumber,
                                CityId = tblDriverDetail?.Driver == null ? tblDriverDetail?.CityId : tblDriverDetail?.Driver?.CityId,
                                ServicePartnerId = tblDriverDetail?.ServicePartnerId,
                                CreatedDate = tblDriverDetail?.CreatedDate,
                                ModifiedDate = tblDriverDetail?.ModifiedDate,
                                DriverId = tblDriverDetail?.DriverId,
                                VehicleId = tblDriverDetail?.VehicleId,
                                JourneyPlannedDate = tblDriverDetail?.JourneyPlanDate ?? DateTime.MinValue,
                                ProfilePicture = tblDriverDetail?.Driver != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Driver?.ProfilePicture : string.Empty,
                                DriverlicenseImage = tblDriverDetail?.Driver != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Driver?.DriverLicenseImage : string.Empty,
                                VehicleInsuranceCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehicleInsuranceCertificate : string.Empty,
                                VehiclefitnessCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehiclefitnessCertificate : string.Empty,
                                VehicleRcCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehicleRcCertificate : string.Empty
                            };

                            DriverDetailsViewModal.Add(driverDetails);
                            addedDriverDetailsIds.Add((int)tbllogistic.DriverDetailsId);
                        }
                    }

                    AllDriverDetailslist allDriverDetailsList = new AllDriverDetailslist
                    {
                        AllDriverDetailslists = DriverDetailsViewModal
                    };

                    responseMessage.message = "Please clear the Driver bucket orders";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    responseMessage.Data = allDriverDetailsList;
                    return responseMessage;
                }
                else
                {
                    ObjDriverDetail.IsActive = false;
                    ObjDriverDetail.IsApproved = false;
                    ObjDriverDetail.ModifiedDate = _currentDatetime;

                    _context.Entry(ObjDriverDetail).State = EntityState.Modified;
                    int result = _context.SaveChanges();

                    responseMessage.message = result > 0 ? "Driver Disable successfully " : "Disable Failed";
                    responseMessage.Status = result > 0;
                    responseMessage.Status_Code = result > 0 ? HttpStatusCode.OK : HttpStatusCode.OK;

                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("ServicePartnerManager", "DisableDriver", ex);
            }

            return responseMessage;
        }

        #endregion


        #region Disable Vehicle by api Added by priyanshi
        /// <summary>
        /// Update LGC Vehichle
        /// Used in api of UpdateVehicle in mobile development
        /// Added by Priyanshi
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public ResponseResult DisableVehicle(DisableVehicleDataModel dataModel)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            bool cancelCheck = false;
            try
            {
                if (dataModel == null)
                {
                    responseMessage.message = "Data model is null";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }

                TblVehicleList ObjVehicleDetail = _vehicleListRepository.GetSingle(x => x.IsActive == true && x.VehicleId == dataModel.vehicleId && x.ServicePartnerId == dataModel.ServicePartnerId);
                if (ObjVehicleDetail == null)
                {
                    responseMessage.message = "vehicle not found";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    return responseMessage;
                }

                bool isValid = _context.TblLogistics
                       .Include(x => x.DriverDetails).ThenInclude(x => x.Driver)
                       .Include(x => x.DriverDetails).ThenInclude(x => x.Vehicle)
                       .Any(x => x.DriverDetails.VehicleId == dataModel.vehicleId && x.DriverDetailsId == x.DriverDetails.DriverDetailsId && x.StatusId != 32 && x.StatusId != 55 && x.StatusId != 22 && x.StatusId != 21 && x.StatusId != 26);

                if (isValid == true)
                {
                    responseMessage.message = "Please clear the bucket orders in order to get the new ones ";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    return responseMessage;
                }
                else
                {
                    ObjVehicleDetail.IsActive = false;
                    ObjVehicleDetail.IsApproved = false;
                    ObjVehicleDetail.ModifiedDate = _currentDatetime;

                    _context.Entry(ObjVehicleDetail).State = EntityState.Modified;
                    int result = _context.SaveChanges();

                    responseMessage.message = result > 0 ? "Vehicle Disable successfully " : "Disable Failed";
                    responseMessage.Status = result > 0;
                    responseMessage.Status_Code = result > 0 ? HttpStatusCode.OK : HttpStatusCode.OK;

                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("ServicePartnerManager", "DisableVehicle", ex);
            }

            return responseMessage;
        }

        #endregion

        #region PlanJournyList by api Added by priyanshi
        /// <summary>
        /// Update LGC Vehichle
        /// Used in api of UpdateVehicle in mobile development
        /// Added by Kranti
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public ResponseResult PlanJourneyList(PlanJourneyListDataModel dataModel)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            bool cancelCheck = false;
            try
            {
                if (dataModel == null)
                {
                    responseMessage.message = "Data model is null";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }
                if (dataModel.ServicePartnerId > 0 && dataModel.DriverId > 0)
                {
                    TblDriverList ObjDriverDetail = _driverListRepository.GetSingle(x => x.IsActive == true && x.DriverId == dataModel.DriverId && x.ServicePartnerId == dataModel.ServicePartnerId);
                    if (ObjDriverDetail == null)
                    {
                        responseMessage.message = "User not found";
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.OK;
                        return responseMessage;
                    }
                }

                var tblDriverDetailsQuery = _context.TblDriverDetails
            .Include(x => x.Driver)
                      .Include(x => x.Vehicle)
            .Where(x => x.ServicePartnerId == dataModel.ServicePartnerId);



                if (dataModel.DriverId > 0)
                {
                    tblDriverDetailsQuery = tblDriverDetailsQuery.Where(x => x.DriverId == dataModel.DriverId);
                }

                if (dataModel.startdate.HasValue)
                {
                    tblDriverDetailsQuery = tblDriverDetailsQuery.Where(x => x.JourneyPlanDate.Value.Date >= dataModel.startdate.Value.Date);
                }

                if (dataModel.enddate.HasValue)
                {
                    tblDriverDetailsQuery = tblDriverDetailsQuery.Where(x => x.JourneyPlanDate.Value.Date <= dataModel.enddate.Value.Date);
                }

                var tbldriverdetails = tblDriverDetailsQuery
                    .Skip((int)((dataModel.pagenumber - 1) * dataModel.pagesize))
                    .Take((int)dataModel.pagesize)
                    .OrderByDescending(x=>x.JourneyPlanDate)
                    .ToList();


                if (tbldriverdetails.Count > 0)
                {
                    List<DriverDetailsModel> DriverDetailsViewModal = new List<DriverDetailsModel>();
                    HashSet<int> addedDriverDetailsIds = new HashSet<int>();

                    foreach (TblDriverDetail item in tbldriverdetails)
                    {
                        // TblLogistic? tbllogistic = _logisticsRepository.GetExchangeDetailsByRegdno(item.RegdNo);

                        if (item.DriverDetailsId > 0 && !addedDriverDetailsIds.Contains((int)item.DriverDetailsId))
                        {
                            TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetDriverDetailsById((int)item.DriverDetailsId);

                            DriverDetailsModel driverDetails = new DriverDetailsModel
                            {
                                DriverDetailsId = tblDriverDetail?.DriverDetailsId,
                                DriverName = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverName : tblDriverDetail?.Driver?.DriverName,
                                DriverPhoneNumber = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverPhoneNumber : tblDriverDetail?.Driver?.DriverPhoneNumber,
                                VehicleNumber = tblDriverDetail?.Vehicle == null ? tblDriverDetail?.VehicleNumber : tblDriverDetail?.Vehicle?.VehicleNumber,
                                City = tblDriverDetail?.Driver == null ? tblDriverDetail?.City : tblDriverDetail?.Driver?.City?.Name,
                                VehicleRcNumber = tblDriverDetail?.Vehicle == null ? tblDriverDetail?.VehicleRcNumber : tblDriverDetail?.Vehicle?.VehicleRcNumber,
                                DriverlicenseNumber = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverlicenseNumber : tblDriverDetail?.Driver?.DriverLicenseNumber,
                                CityId = tblDriverDetail?.Driver == null ? tblDriverDetail?.CityId : tblDriverDetail?.Driver?.CityId,
                                ServicePartnerId = tblDriverDetail?.ServicePartnerId,
                                CreatedDate = tblDriverDetail?.CreatedDate,
                                ModifiedDate = tblDriverDetail?.ModifiedDate,
                                DriverId = tblDriverDetail?.DriverId,
                                VehicleId = tblDriverDetail?.VehicleId,
                                JourneyPlannedDate = tblDriverDetail?.JourneyPlanDate ?? DateTime.MinValue,
                                ProfilePicture = tblDriverDetail?.Driver != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Driver?.ProfilePicture : string.Empty,
                                DriverlicenseImage = tblDriverDetail?.Driver != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Driver?.DriverLicenseImage : string.Empty,
                                VehicleInsuranceCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehicleInsuranceCertificate : string.Empty,
                                VehiclefitnessCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehiclefitnessCertificate : string.Empty,
                                VehicleRcCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehicleRcCertificate : string.Empty
                            };

                            DriverDetailsViewModal.Add(driverDetails);
                            addedDriverDetailsIds.Add((int)item.DriverDetailsId);
                        }
                    }

                    AllDriverDetailslist allDriverDetailsList = new AllDriverDetailslist
                    {
                        AllDriverDetailslists = DriverDetailsViewModal
                    };

                    responseMessage.message = "Success";
                    responseMessage.Status = true;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    responseMessage.Data = allDriverDetailsList;
                    return responseMessage;
                }
                else
                {
                    responseMessage.message = "Details not found";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    return responseMessage;

                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("ServicePartnerManager", "PlanJourneyList", ex);
            }

            return responseMessage;
        }

        #endregion

        #endregion

        #region Not Used Method (Added by ashwin)

        #region get all Orders RegdNo list assign to drivers
        /// <summary>
        /// Get all orders whose ticket generated and assgin to logistic driver ID
        /// added by ashwin
        /// </summary>
        /// <param name="id"></param>
        /// <returns>responseResult</returns>
        public ResponseResult OrderRegdNoByDriverId(int id)
        {
            ResponseResult responseResult = new ResponseResult();
            List<TblLogistic> tblLogistic = null;
            List<orderRegdnolist> orderRegdnolists = new List<orderRegdnolist>();
            customerOrderDetailsDataModel OrderDetails = new customerOrderDetailsDataModel();
            try
            {
                tblLogistic = _logisticsRepository.GetList(x => x.IsActive == true && x.DriverDetailsId == id).ToList();

                if (tblLogistic != null && tblLogistic.Count > 0)
                {
                    //foreach (var custID in tblLogistic)
                    //{
                    //    OrderDetails = CustomerDetails(custID.RegdNo);
                    //    if (OrderDetails != null && OrderDetails.CustomerName !=null)
                    //    {

                    //    }
                    //}

                    orderRegdnolists = _mapper.Map<List<TblLogistic>, List<orderRegdnolist>>(tblLogistic);

                    if (orderRegdnolists.Count > 0)
                    {
                        responseResult.Status = true;
                        responseResult.Status_Code = HttpStatusCode.OK;
                        responseResult.Data = orderRegdnolists;
                        responseResult.message = "Success";
                    }
                    else
                    {
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        responseResult.message = "Error occurs while mapping the data";
                    }
                }
                else
                {
                    responseResult.Status = true;
                    responseResult.Status_Code = HttpStatusCode.OK;
                    responseResult.message = "no data found";
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("SevicePartnerManager", "OrderListByDriverId", ex);

                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
            }
            return responseResult;
        }
        #endregion

        #region Order Accept/Reject by driver
        /// <summary>
        /// Api method for Order Accept/Reject by driver 
        /// Added by ashwin
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public ResponseResult AcceptOrder(string OrderRegdNo, bool isAccepted)
        {
            TblLogistic tblLogistic = null;

            ResponseResult responseMessage = new ResponseResult();
            try
            {
                if (OrderRegdNo != null)
                {
                    tblLogistic = _logisticsRepository.GetSingle(x => x.RegdNo == OrderRegdNo && x.IsActive == true);
                    if (tblLogistic != null && tblLogistic.RegdNo != string.Empty)
                    {
                        if (isAccepted == true)
                        {
                            tblLogistic.IsOrderAcceptedByDriver = true;
                        }
                        else if (isAccepted == false)
                        {
                            tblLogistic.IsOrderAcceptedByDriver = false;
                        }
                        else
                        {
                            responseMessage.message = "failed,invalid flag request";
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.BadRequest;
                            return responseMessage;
                        }

                        tblLogistic.ModifiedDate = DateTime.Now;

                        //tblLogistic.Modifiedby = tblLogistic.DriverDetailsId;

                        _logisticsRepository.Update(tblLogistic);
                        int result = _logisticsRepository.SaveChanges();
                        if (result == 1)
                        {

                            //response = true;
                            responseMessage.message = "Success";
                            responseMessage.Status = true;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                        }
                        else
                        {
                            responseMessage.message = "failed";
                            responseMessage.Status_Code = HttpStatusCode.BadRequest;
                            responseMessage.Status = false;
                        }

                    }
                    else
                    {
                        responseMessage.message = "failed,Data Not found for given RegdNo";
                        responseMessage.Status = false;
                    }

                }
                else
                {
                    responseMessage.message = "failed,RegdNo Not found";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("LogisticManager", "AcceptOrder", ex);
            }
            return responseMessage;
        }
        #endregion

        #region OrderDetatils By Regdno 
        /// <summary>
        /// OrderDetatils By Regdno 
        /// added by ashwin
        /// </summary>
        /// <param name="orderRegdNo"></param>
        /// <returns></returns>
        public customerOrderDetailsDataModel CustomerDetails(string orderRegdNo)
        {
            customerOrderDetailsDataModel orderDetails = new customerOrderDetailsDataModel();
            try
            {
                if (orderRegdNo != null && orderRegdNo != string.Empty && orderRegdNo.Length > 0)
                {
                    TblExchangeOrder tblExchangeOrder = _exchangeOrderRepository.GetSingle(x => x.IsActive == true && x.RegdNo == orderRegdNo);
                    if (tblExchangeOrder != null && tblExchangeOrder.CustomerDetailsId > 0 && tblExchangeOrder.CustomerDetailsId != null)
                    {
                        TblCustomerDetail tblCustomerDetails = _customerDetailsRepository.GetSingle(x => x.IsActive == true && x.Id == tblExchangeOrder.CustomerDetailsId);
                        if (tblCustomerDetails != null && tblCustomerDetails.Id > 0)
                        {
                            //orderDetails.OrderId = tblExchangeOrder.id
                            orderDetails.RegdNo = tblExchangeOrder.RegdNo;
                            orderDetails.OrderId = tblExchangeOrder.Id;
                            orderDetails.Location = tblCustomerDetails.Address1.ToString() + tblCustomerDetails.Address2.ToString();
                            orderDetails.CustomerName = tblCustomerDetails.FirstName.ToString() + tblCustomerDetails.LastName.ToString();
                            orderDetails.MobileNumber = tblCustomerDetails.PhoneNumber.ToString();
                        }
                        else
                        {
                            orderDetails.responseMessage = "Customer Details Not Found";
                        }
                    }
                    else
                    {
                        orderDetails.responseMessage = "OrderNotFound";
                    }
                }
                else
                {
                    orderDetails.responseMessage = "RegdNoNotFound";
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("SevicePartnerManager", "CustomerDetails", ex);
            }
            return orderDetails;
        }
        #endregion

        #region Update Login_Mobile Table at time of login by mobile number
        /// <summary>
        /// update the loginMobile table at time of user login by mobileNumber
        /// </summary>
        /// <param name="DeviceType"></param>
        /// <param name="DeviceId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateMobileLogindetails(string DeviceType, string DeviceId, int userId)
        {
            bool flag = false;
            try
            {
                if (DeviceType != null && DeviceId != null && userId > 0)
                {
                    TblLoginMobile tblLoginMobile = _login_MobileRepository.GetSingle(x => x.IsActive == true && x.UserId == userId);
                    tblLoginMobile.DeviceType = DeviceType;
                    tblLoginMobile.UserDeviceId = DeviceId;
                    _login_MobileRepository.Update(tblLoginMobile);
                    _login_MobileRepository.SaveChanges();
                }
                else
                {
                    return flag;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("SevicePartnerManager", "UpdateMobileLogindetails", ex);
            }
            return flag;

        }

        #endregion

        #region Order Assign to Vehicle by ServicePartner/LGC
        /// <summary>
        /// Api method for Order Assign to Vehicle by ServicePartner/LGC
        /// Added by ashwin
        /// </summary>
        /// <param name="">username</param>
        /// <param name="">regdno</param>
        /// <returns></returns>
        public ResponseResult OrderAssigntoVehicle(string OrderRegdNo, string commentbyLGC, string userName)
        {
            TblLogistic tblLogistic = null;
            int OrderStatusId = Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner);
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            TblServicePartner tblServicePartner = null;
            TblOrderLgc? tblOrderLgc = null;
            TblExchangeOrder tblExchangeOrder = null;
            TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = null;

            try
            {
                tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerEmailId == userName);

                if (tblServicePartner != null && tblServicePartner.ServicePartnerId > 0 && OrderRegdNo != null)
                {
                    tblLogistic = _logisticsRepository.GetSingle(x => x.RegdNo == OrderRegdNo && x.IsActive == true && x.ServicePartnerId == tblServicePartner.ServicePartnerId && x.StatusId == Convert.ToInt32(OrderStatusEnum.CallAssignedforPickup));

                    if (tblLogistic != null)
                    {
                        TblOrderTran tblOrderTran = _orderTransRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == tblLogistic.OrderTransId);

                        if (tblOrderTran != null && tblOrderTran.OrderTransId > 0)
                        {
                            #region insert/update into tblOrderLgc
                            tblOrderLgc = _orderLGCRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == tblOrderTran.OrderTransId);
                            if (tblOrderLgc != null && tblOrderLgc.OrderLgcid > 0)
                            {
                                tblOrderLgc.StatusId = OrderStatusId;
                                tblOrderLgc.ModifiedBy = tblServicePartner.UserId;
                                tblOrderLgc.ModifiedDate = _currentDatetime;
                                tblOrderLgc.LogisticId = tblLogistic.LogisticId;
                                if (commentbyLGC.Length > 0)
                                {
                                    tblOrderLgc.Lgccomments = commentbyLGC;
                                }
                                _orderLGCRepository.Update(tblOrderLgc);
                            }
                            else
                            {
                                tblOrderLgc = new TblOrderLgc();
                                tblOrderLgc.OrderTransId = tblOrderTran.OrderTransId;
                                tblOrderLgc.StatusId = OrderStatusId;
                                tblOrderLgc.IsActive = true;
                                tblOrderLgc.CreatedBy = tblServicePartner.UserId;
                                tblOrderLgc.CreatedDate = _currentDatetime;
                                tblOrderLgc.LogisticId = tblLogistic.LogisticId;
                                if (commentbyLGC.Length > 0)
                                {
                                    tblOrderLgc.Lgccomments = commentbyLGC;
                                }
                                _orderLGCRepository.Create(tblOrderLgc);
                            }
                            _orderLGCRepository.SaveChanges();
                            #endregion

                            #region update statusid in tbllogistics
                            tblLogistic.StatusId = OrderStatusId;
                            tblLogistic.IsActive = true;
                            tblLogistic.Modifiedby = tblServicePartner.UserId;
                            tblLogistic.ModifiedDate = _currentDatetime;
                            _logisticsRepository.Update(tblLogistic);
                            _logisticsRepository.SaveChanges();
                            #endregion

                            #region Insert into tblexchangeabbhistory

                            tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                            tblExchangeAbbstatusHistory.OrderType = 17;
                            tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                            tblExchangeAbbstatusHistory.RegdNo = tblExchangeOrder.RegdNo;
                            tblExchangeAbbstatusHistory.ZohoSponsorId = tblExchangeOrder.ZohoSponsorOrderId != null ? tblExchangeOrder.ZohoSponsorOrderId : string.Empty; ;
                            tblExchangeAbbstatusHistory.CustId = tblExchangeOrder.CustomerDetailsId;
                            tblExchangeAbbstatusHistory.StatusId = OrderStatusId;
                            tblExchangeAbbstatusHistory.IsActive = true;
                            tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                            tblExchangeAbbstatusHistory.CreatedBy = tblServicePartner.UserId;
                            tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                            tblExchangeAbbstatusHistory.Comment = commentbyLGC != null ? "TicketNo-" + tblLogistic.TicketNumber + "- Service Partner Name -" + tblServicePartner.ServicePartnerDescription + "Comment -" + commentbyLGC : string.Empty;
                            _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                            _exchangeABBStatusHistoryRepository.SaveChanges();
                            #endregion
                        }
                        else
                        {
                            responseMessage.message = "OrderTransId Not Found for RegdNo";
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseMessage.message = "Invalid RegdNo";
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseMessage.message = "Not Valid login credential";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                }

            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("LogisticManager", "RejectOrderbyLGC", ex);
            }
            return responseMessage;
        }
        #endregion

        #endregion

        #region Added by Vk for LGC Mobibie App ERP Connectivity
        #region Add LGC Vehicle by Service partner
        /// <summary>
        /// Add LGC Vehicle by Service partner
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public ResponseResult AddVehicleBySP([FromForm] DriverDetailsDataModel dataModel)
        {
            TblDriverDetail tblDriverDetail = null;
            bool cancelCheck = false;
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;

            try
            {
                if (dataModel != null)
                {
                    tblDriverDetail = _mapper.Map<DriverDetailsDataModel, TblDriverDetail>(dataModel);
                    TblDriverDetail tblDriver = _driverDetailsRepository.GetSingle(x => x.IsActive == true && x.VehicleNumber == dataModel.VehicleNumber || x.VehicleRcNumber == dataModel.VehicleRcNumber);
                    if (tblDriver == null)
                    {
                        if (tblDriverDetail != null)
                        {
                            // Check email & mobile number is exists or not
                            var checkEmail = _context.TblDriverDetails.ToList();
                            bool numberFlag = checkEmail.Exists(p => p.DriverPhoneNumber == dataModel.DriverPhoneNumber);

                            if (numberFlag)
                            {
                                responseMessage.message = "This Mobile Number is Already Exists";
                                responseMessage.Status = false;
                                responseMessage.Status_Code = HttpStatusCode.BadRequest;
                                return responseMessage;
                            }

                            // Add Image in Folder
                            // DriverlicenseImage
                            if (dataModel.DriverlicenseImage != null)
                            {
                                string fileName = string.Empty;
                                if (tblDriverDetail.DriverlicenseNumber != null)
                                {
                                    fileName = tblDriverDetail.DriverlicenseNumber + "DriverlicenseImage" + ".jpg";
                                }
                                else
                                {
                                    fileName = tblDriverDetail.DriverPhoneNumber + "DriverlicenseImage" + ".jpg";
                                }
                                string filePath = "ServicePartner\\Driver\\DriversLiscense";
                                cancelCheck = _imageHelper.SaveFileFromBase64(dataModel.DriverlicenseImageString, filePath, fileName);
                                if (cancelCheck)
                                {
                                    tblDriverDetail.DriverlicenseImage = fileName;
                                    cancelCheck = false;
                                }
                            }

                            // VehicleRcCertificate
                            if (dataModel.VehicleRcCertificate != null)
                            {
                                string fileName = string.Empty;
                                if (tblDriverDetail.VehicleRcNumber != null)
                                {
                                    fileName = tblDriverDetail.VehicleRcNumber + "VehicleRcCertificate" + ".jpg";
                                }
                                else
                                {
                                    fileName = tblDriverDetail.VehicleNumber + "VehicleRcCertificate" + ".jpg";
                                }
                                string filePath = "ServicePartner\\Driver\\VehicleRcCertificate";
                                cancelCheck = _imageHelper.SaveFileFromBase64(dataModel.VehicleRcCertificateString, filePath, fileName);
                                if (cancelCheck)
                                {
                                    tblDriverDetail.VehicleRcCertificate = fileName;
                                    cancelCheck = false;
                                }
                            }

                            // VehiclefitnessCertificate
                            if (dataModel.VehiclefitnessCertificate != null)
                            {
                                string fileName = string.Empty;
                                if (tblDriverDetail.VehicleNumber != null)
                                {
                                    fileName = tblDriverDetail.VehicleNumber + "VehicleFitnessCertificate" + ".jpg";
                                }
                                else
                                {
                                    fileName = tblDriverDetail.VehicleRcNumber + "VehicleFitnessCertificate" + ".jpg";
                                }
                                string filePath = "ServicePartner\\Driver\\DriverFitnessCerti";
                                cancelCheck = _imageHelper.SaveFileFromBase64(dataModel.VehiclefitnessCertificateString, filePath, fileName);
                                if (cancelCheck)
                                {
                                    tblDriverDetail.VehiclefitnessCertificate = fileName;
                                    cancelCheck = false;
                                }
                            }

                            // VehicleInsuranceCertificate
                            if (dataModel.VehicleInsuranceCertificate != null)
                            {
                                string fileName = string.Empty;
                                if (tblDriverDetail.VehicleInsuranceCertificate != null)
                                {
                                    fileName = tblDriverDetail.VehicleRcNumber + "VehicleInsuranceCertificate" + ".jpg";
                                }
                                else
                                {

                                    fileName = tblDriverDetail.VehicleNumber + "VehicleInsuranceCertificate" + ".jpg";
                                }

                                string filePath = "ServicePartner\\Driver\\DriverInsuranceCerti";
                                cancelCheck = _imageHelper.SaveFileFromBase64(dataModel.VehicleInsuranceCertificateString, filePath, fileName);
                                if (cancelCheck)
                                {
                                    tblDriverDetail.VehicleInsuranceCertificate = fileName;
                                    cancelCheck = false;
                                }
                            }

                            // ProfilePicture
                            if (dataModel.ProfilePicture != null)
                            {
                                string fileName = string.Empty;
                                if (tblDriverDetail.ProfilePicture != null)
                                {
                                    fileName = tblDriverDetail.VehicleNumber + "DriverProfilePicture" + ".jpg";
                                }
                                else
                                {
                                    fileName = tblDriverDetail.VehicleNumber + "DriverProfilePicture" + ".jpg";
                                }

                                string filePath = "ServicePartner\\Driver\\ProfilePic";
                                cancelCheck = _imageHelper.SaveFileFromBase64(dataModel.ProfilePictureString, filePath, fileName);
                                if (cancelCheck)
                                {
                                    tblDriverDetail.ProfilePicture = fileName;
                                    cancelCheck = false;
                                }
                            }

                            tblDriverDetail.IsActive = true;
                            tblDriverDetail.CreatedDate = _currentDatetime;
                            tblDriverDetail.CreatedBy = dataModel.CreatedBy;
                            tblDriverDetail.IsApproved = true;
                            tblDriverDetail.ApprovedBy = tblDriverDetail.CreatedBy;

                            _driverDetailsRepository.Create(tblDriverDetail);
                            int result = _driverDetailsRepository.SaveChanges();

                            //if (result > 0)
                            //{
                            //    int responseUserId = ManageServicePartnerDriverUser(tblDriverDetail);

                            //    // Update userID in DriverDetails
                            //    if (responseUserId > 0)
                            //    {
                            //        tblDriverDetail.UserId = responseUserId;
                            //        tblDriverDetail.DriverPhoneNumber = SecurityHelper.DecryptString(tblDriverDetail.DriverPhoneNumber, _baseConfig.Value.SecurityKey);
                            //        _driverDetailsRepository.Update(tblDriverDetail);
                            //        int updateResult = _driverDetailsRepository.SaveChanges();

                            //        if (updateResult > 0)
                            //        {
                            //            responseMessage.message = "Added Success";
                            //            responseMessage.Status = true;
                            //            responseMessage.Status_Code = HttpStatusCode.OK;
                            //        }
                            //        else
                            //        {
                            //            responseMessage.message = "Driver Added but User not Created";
                            //            responseMessage.Status = true;
                            //            responseMessage.Status_Code = HttpStatusCode.OK;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        responseMessage.Status = false;
                            //        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                            //        responseMessage.message = "Registration Failed";
                            //    }
                            //}
                            //else
                            //{
                            //    responseMessage.Status = false;
                            //    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                            //    responseMessage.message = "Registration Failed";
                            //}
                        }
                        else
                        {
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                            responseMessage.message = "Registration Failed";
                        }
                    }
                    else
                    {
                        if (tblDriver.VehicleRcNumber == dataModel.VehicleRcNumber)
                        {
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                            responseMessage.message = "Vehicle RC number already exist";
                        }
                        else
                        {
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                            responseMessage.message = "Vehicle number already exist";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("LogisticManager", "AddVehicle", ex);
            }

            return responseMessage;
        }
        #endregion

        //#region Assign Order to Driver
        //public ResponseResult AssignOrderToDriver(DriverDetailsViewModel driverDetailsVM)
        //{
        //    ResponseResult responseResult = new ResponseResult();
        //    try
        //    {
        //        string mystring = driverDetailsVM.LogisticIdList;
        //        int driverId = driverDetailsVM.DriverDetailsId;
        //        if (mystring != null)
        //        {
        //            var query = from val in mystring.Split(',')
        //                        select int.Parse(val);
        //            foreach (int num in query)
        //            {
        //                TblLogistic tblLogistic = _logisticsRepository.GetSingle(x => x.LogisticId == num);
        //                if (tblLogistic != null)
        //                {
        //                    TblOrderTran tblOrderTrans = _orderTransRepository.GetSingle(x => x.RegdNo == tblLogistic.RegdNo);
        //                    if (tblOrderTrans != null)
        //                    {
        //                        #region Exchange Order
        //                        tblExchangeOrders.IsActive = true;
        //                        tblExchangeOrders.OrderStatus = "Cancel Order for QC";
        //                        tblExchangeOrders.StatusId = Convert.ToInt32(OrderStatusEnum.QCOrderCancel);
        //                        tblExchangeOrders.Comment1 = comment;
        //                        tblExchangeOrders.ModifiedBy = _loginSession.UserViewModel.UserId;
        //                        tblExchangeOrders.ModifiedDate = _currentDatetime;
        //                        _ExchangeOrderRepository.Update(tblExchangeOrders);
        //                        _ExchangeOrderRepository.SaveChanges();
        //                        TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();

        //                        #endregion

        //                        tblOrderTrans.StatusId = Convert.ToInt32(OrderStatusEnum.QCOrderCancel);
        //                        tblOrderTrans.ModifiedBy = _loginSession.UserViewModel.UserId;
        //                        tblOrderTrans.ModifiedDate = _currentDatetime;
        //                        _orderTransRepository.Update(tblOrderTrans);
        //                        _orderTransRepository.SaveChanges();

        //                        TblOrderQc tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTrans.OrderTransId);
        //                        if (tblOrderQc != null)
        //                        {
        //                            tblOrderQc.StatusId = Convert.ToInt32(OrderStatusEnum.QCOrderCancel);
        //                            tblOrderQc.Qccomments = comment;
        //                            tblOrderQc.ModifiedBy = _loginSession.UserViewModel.UserId;
        //                            tblOrderQc.ModifiedDate = _currentDatetime;
        //                            _orderQCRepository.Update(tblOrderQc);
        //                            _orderQCRepository.SaveChanges();
        //                        }

        //                        #region Update tblExchangeAbbstatusHistory
        //                        tblExchangeAbbstatusHistory.OrderTransId = tblOrderTrans.OrderTransId;
        //                        tblExchangeAbbstatusHistory.OrderType = 17;
        //                        tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeOrders.SponsorOrderNumber;
        //                        tblExchangeAbbstatusHistory.CustId = tblExchangeOrders.CustomerDetailsId;
        //                        tblExchangeAbbstatusHistory.RegdNo = tblExchangeOrders.RegdNo;
        //                        tblExchangeAbbstatusHistory.StatusId = Convert.ToInt32(OrderStatusEnum.QCOrderCancel);
        //                        tblExchangeAbbstatusHistory.Comment = comment;
        //                        tblExchangeAbbstatusHistory.IsActive = true;
        //                        tblExchangeAbbstatusHistory.CreatedBy = _loginSession.UserViewModel.UserId;
        //                        tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
        //                        _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
        //                        _exchangeABBStatusHistoryRepository.SaveChanges();
        //                        #endregion
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        //#endregion

        #region Order Assign to Vehicle by LGC/Service Partner
        /// <summary>
        /// Assign Order To Vehicle by LGC partner
        /// added by VK
        /// </summary>
        /// <param name="OrdertransId"></param>
        /// <param name="LGCId"></param>
        /// <param name="driverDetailsId"></param>
        /// <returns></returns>
        public AssignOrderResponse AssignOrdertoVehiclebyLGC(AssignOrderRequest request)
        {
            AssignOrderResponse response = new AssignOrderResponse();
            response.Data = new List<ResponseResult>();
            response.Status = false;
            response.Status_Code = 0;
            response.message = null;
            response.SucssesCount = 0;
            var count = 0;
            string userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();
            try
            {
                if (request != null)
                {
                    TblServicePartner tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerIsApprovrd == true && x.ServicePartnerId == request.LGCId);

                    userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();
                    if (tblServicePartner != null && tblServicePartner.ServicePartnerEmailId != null)
                    {
                        var email = SecurityHelper.EncryptString(tblServicePartner.ServicePartnerEmailId, _baseConfig.Value.SecurityKey);
                        TblUser tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.Email == email);

                        if (tblUser != null)
                        {
                            TblUserRole tblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == tblUser.UserId);
                            if (tblUserRole != null)
                            {
                                TblRole tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == tblUserRole.RoleId);

                                if (tblRole != null && tblRole.RoleName == userRole)
                                {
                                    var status = Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner);
                                    count = _logisticsRepository.GetDriverAssignOrderCount(request.DriverDetailsId, status, request.LGCId);
                                    var remainingOrderCount = _baseConfig.Value.DriverOrderCount - count;

                                    if (count >= 0 && count < _baseConfig.Value.DriverOrderCount && remainingOrderCount >= request.OrdertransId.Count)
                                    {
                                        foreach (var item in request.OrdertransId)
                                        {
                                            ResponseResult responseResult = OrderAssignLGCtoDriverid(item, request.LGCId, request.DriverDetailsId);
                                            if (responseResult.Status)
                                            {

                                                // Increment the success count and update the response message
                                                response.SucssesCount += 1;
                                                response.Status = true;
                                                response.Status_Code = HttpStatusCode.OK;
                                                response.message = response.SucssesCount + " Orders Are Successfully Assigned";
                                            }
                                            else
                                            {
                                                // If a response is a failure, add it to the response data
                                                response.Data.Add(responseResult);
                                            }
                                        }

                                        // Check if all orders were successfully assigned
                                        if (response.Data.Count == request.OrdertransId.Count)
                                        {
                                            response.Status = false;
                                            response.Status_Code = HttpStatusCode.OK;
                                            response.message = "All Order failed ";
                                        }
                                        if (response.SucssesCount > 0)
                                        {
                                            string SucssesCountPush = response.SucssesCount.ToString();
                                            var result = _pushNotificationManager.SendNotification(request.LGCId, request.DriverDetailsId, EnumHelper.DescriptionAttr(NotificationEnum.OrderAssignedbyServicePartner), SucssesCountPush, null);
                                        }
                                    }
                                    else
                                    {
                                        ResponseResult responseResult = new ResponseResult
                                        {
                                            message = "Now you can only assign " + remainingOrderCount + " order(s) to this vehicle.",
                                            Status = false,
                                            Status_Code = HttpStatusCode.BadRequest
                                        };
                                        response.Data.Add(responseResult);
                                    }
                                }
                                else
                                {
                                    ResponseResult responseResult = new ResponseResult
                                    {
                                        message = "Not a valid user",
                                        Status = false,
                                        Status_Code = HttpStatusCode.BadRequest
                                    };
                                    response.Data.Add(responseResult);
                                }
                            }
                            else
                            {
                                ResponseResult responseResult = new ResponseResult
                                {
                                    message = "Service partner is not a valid user",
                                    Status = false,
                                    Status_Code = HttpStatusCode.BadRequest
                                };
                                response.Data.Add(responseResult);
                            }
                        }
                        else
                        {
                            ResponseResult responseResult = new ResponseResult
                            {
                                message = "Not a valid request",
                                Status = false,
                                Status_Code = HttpStatusCode.BadRequest
                            };
                            response.Data.Add(responseResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticController", "AssignVehiclebyLGC", ex);
                ResponseResult responseResult = new ResponseResult
                {
                    message = ex.Message,
                    Status = false,
                    Status_Code = HttpStatusCode.InternalServerError
                };
                response.Data.Add(responseResult);
            }
            return response;
        }
        #endregion

        #region Order reject  by LGC/Service Partner
        /// <summary>
        /// Order reject  by LGC/Service Partner
        /// </summary>
        /// <param name="Regdno"></param>
        /// <param name="DriverDetailsId"></param>
        /// <returns></returns>
        public ResponseResult RejectOrderbyLGC(RejectLGCOrderRequest request)
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.message = string.Empty;
            TblUser tblUser = null;
            string username = string.Empty;
            string userRole = string.Empty;

            try
            {
                TblServicePartner tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerIsApprovrd == true && x.ServicePartnerId == request.LGCId);

                userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();
                if (tblServicePartner != null && tblServicePartner.ServicePartnerEmailId != null)
                {

                    var Email = SecurityHelper.EncryptString(tblServicePartner.ServicePartnerEmailId, _baseConfig.Value.SecurityKey);
                    tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.Email == Email);

                    if (tblUser != null)
                    {
                        TblUserRole tblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == tblUser.UserId);
                        if (tblUserRole != null)
                        {
                            TblRole tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == tblUserRole.RoleId);
                            if (tblRole != null && tblRole.RoleName == userRole)
                            {
                                responseResult = RejectOrderbyLGC(request.OrdertransId, request.LGCId, request.RejectComment);
                            }
                            else
                            {
                                responseResult.message = "Not A Valid User";
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.BadRequest;
                            }

                        }
                        else
                        {
                            responseResult.message = "Not A Valid User";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }


                    }

                    else
                    {
                        responseResult.message = "Not A Valid User";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.message = "Service partner not a valid user";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticController", "AssignVehiclebyLGC", ex);
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }
        #endregion

        //#region Get Order city list by LGC Id
        ///// <summary>
        ///// Get Order city list by LGC Id
        ///// added by VK
        ///// </summary>
        ///// <param name="Regdno"></param>
        ///// <param name="Id"></param>
        ///// <returns>responseResult</returns>
        //public ResponseResult GetOrdercitylistbyLgcId1(int Id, bool? IsRejectedOrder)
        //{
        //    ResponseResult responseMessage = new ResponseResult();
        //    ResponseResult responseResult = new ResponseResult();
        //    responseMessage.message = string.Empty;
        //    string username = string.Empty;
        //    string userRole = string.Empty;
        //    int status = 0;
        //    try
        //    {
        //        if (Id > 0)
        //        {
        //            // Retrieve the order list by ServicePartner ID
        //            if (IsRejectedOrder == true)
        //            {
        //                status = Convert.ToInt32(OrderStatusEnum.OrderRejectedbyVehicle);
        //            }
        //            else
        //            {
        //                status = Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated);
        //            }
        //            responseMessage = GetOrderListById(Id, status, null, null, null, null);

        //            if (responseMessage.Status && responseMessage.Data != null)
        //            {
        //                AllOrderList orderDetails = (AllOrderList)responseMessage.Data;

        //                List<CityList> cityLists = new List<CityList>();
        //                if(orderDetails != null && orderDetails.AllOrderlistViewModels != null && orderDetails.AllOrderlistViewModels.Count > 0)
        //                {
        //                    List<string>? uniqueCityNames = orderDetails.AllOrderlistViewModels.Where(x=>x.City != null).Select(order => order.City).Distinct().ToList();
        //                    if (uniqueCityNames != null && uniqueCityNames.Count > 0)
        //                    {
        //                        foreach (var item in uniqueCityNames)
        //                        {
        //                            if (item != null)
        //                            {
        //                                TblCity tblCity = _cityRepository.GetSingle(x => x.IsActive == true && x.Name == item);
        //                                if (tblCity != null)
        //                                {
        //                                    CityList city = new CityList
        //                                    {
        //                                        Name = tblCity?.Name,
        //                                        CityId = tblCity?.CityId ?? 0,
        //                                        StateId = (int)(tblCity?.StateId??0)
        //                                    };
        //                                    cityLists.Add(city);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //                if (cityLists.Count > 0)
        //                {
        //                    responseResult.Status = true;
        //                    responseResult.Status_Code = HttpStatusCode.OK;
        //                    responseResult.Data = cityLists;
        //                    responseResult.message = "Success";
        //                }
        //                else
        //                {
        //                    responseResult.Status = true;
        //                    responseResult.Status_Code = HttpStatusCode.OK;
        //                    responseResult.Data = cityLists;
        //                    responseResult.message = "Data not Found";
        //                }
        //            }
        //            else
        //            {
        //                responseResult.Status = false;
        //                responseResult.Status_Code = HttpStatusCode.BadRequest;
        //                responseResult.message = responseMessage.message;
        //            }
        //        }
        //        else
        //        {
        //            responseResult.Status = false;
        //            responseResult.Status_Code = HttpStatusCode.BadRequest;
        //            responseResult.message = "Invalid Parameter";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.WriteErrorToDB("ServicPartnerManager", "GetOrdercitylistbyLgcId", ex);
        //        responseResult.Status = false;
        //        responseResult.Status_Code = HttpStatusCode.InternalServerError;
        //        responseResult.message = ex.Message;
        //    }

        //    return responseResult;
        //}
        //#endregion

        #region Get Order city list by LGC Id
        /// <summary>
        /// Get Order city list by LGC Id
        /// added by VK
        /// </summary>
        /// <param name="Regdno"></param>
        /// <param name="Id"></param>
        /// <returns>responseResult</returns>
        public ResponseResult GetOrdercitylistbyLgcId(int LGCId)
        {
            ResponseResult responseMessage = new ResponseResult();
            ResponseResult responseResult = new ResponseResult();
            responseMessage.message = string.Empty;
            string username = string.Empty;
            string userRole = string.Empty;
            int status = 0;
            try
            {
                responseMessage = GetOrderListBySPId(LGCId);

                if (responseMessage.Status && responseMessage.Data != null)
                {
                    AllOrderList orderDetails = (AllOrderList)responseMessage.Data;

                    List<CityList> cityLists = new List<CityList>();
                    if (orderDetails != null && orderDetails.AllOrderlistViewModels != null && orderDetails.AllOrderlistViewModels.Count > 0)
                    {
                        List<string>? uniqueCityNames = orderDetails.AllOrderlistViewModels.Where(x => x.City != null).Select(order => order.City).Distinct().ToList();
                        if (uniqueCityNames != null && uniqueCityNames.Count > 0)
                        {
                            foreach (var item in uniqueCityNames)
                            {
                                if (item != null)
                                {
                                    TblCity tblCity = _cityRepository.GetSingle(x => x.IsActive == true && x.Name == item);
                                    if (tblCity != null)
                                    {
                                        CityList city = new CityList
                                        {
                                            Name = tblCity?.Name,
                                            CityId = tblCity?.CityId ?? 0,
                                            StateId = (int)(tblCity?.StateId ?? 0)
                                        };
                                        cityLists.Add(city);
                                    }
                                }
                            }
                        }
                    }

                    if (cityLists.Count > 0)
                    {
                        responseResult.Status = true;
                        responseResult.Status_Code = HttpStatusCode.OK;
                        responseResult.Data = cityLists;
                        responseResult.message = "Success";
                    }
                    else
                    {
                        responseResult.Status = true;
                        responseResult.Status_Code = HttpStatusCode.OK;
                        responseResult.Data = cityLists;
                        responseResult.message = "Data not Found";
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    responseResult.message = responseMessage.message;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ServicPartnerManager", "GetOrdercitylistbyLgcId", ex);
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
            }

            return responseResult;
        }
        #endregion

        #region GetOrderListById  assign to ServicePartner
        /// <summary>
        /// GetOrderListById  assign to ServicePartner
        /// added by VK
        /// </summary>
        /// <param name="id"></param>
        /// <returns>responseResult</returns>
        public ResponseResult GetOrderListBySPId(int LGCId)
        {
            //get order list
            List<TblLogistic> tblLogisticObj = null;
            try
            {
                TblServicePartner tblServicePartner = new TblServicePartner();
                tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerId == LGCId);
                if (tblServicePartner == null || tblServicePartner.ServicePartnerId <= 0)
                {
                    return new ResponseResult
                    {
                        Status = true,
                        Status_Code = HttpStatusCode.OK,
                        message = "Service Partner not found"
                    };
                }
                tblLogisticObj = _logisticsRepository.GetOrderListBySPIdAndStatus(LGCId, Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated), Convert.ToInt32(OrderStatusEnum.OrderRejectedbyVehicle));

                List<AllOrderlistViewModel> orderListsViewModal = new List<AllOrderlistViewModel>();
                if (tblLogisticObj != null && tblLogisticObj.Count > 0)
                {
                    foreach (var item in tblLogisticObj)
                    {
                        if (item != null && item.LogisticId > 0)
                        {
                            AllOrderlistViewModel orderDetail = new AllOrderlistViewModel();
                            if (item?.OrderTrans?.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                            {
                                orderDetail.City = item.OrderTrans?.Exchange?.CustomerDetails?.City;
                                orderDetail.State = item.OrderTrans?.Exchange?.CustomerDetails?.State;
                            }

                            if (item?.OrderTrans?.OrderType == Convert.ToInt32(OrderTypeEnum.ABB))
                            {
                                orderDetail.City = item.OrderTrans?.Abbredemption?.Abbregistration?.CustCity;
                                orderDetail.State = item.OrderTrans?.Abbredemption?.Abbregistration?.CustState;
                            }
                            orderListsViewModal.Add(orderDetail);
                        }
                    }
                }
                else
                {
                    return new ResponseResult
                    {
                        Status = true,
                        Status_Code = HttpStatusCode.OK,
                        message = "No data found"
                    };
                }

                if (orderListsViewModal.Count > 0)
                {
                    AllOrderList allOrderList = new AllOrderList
                    {
                        AllOrderlistViewModels = orderListsViewModal
                    };

                    return new ResponseResult
                    {
                        Status = true,
                        Status_Code = HttpStatusCode.OK,
                        Data = allOrderList,
                        message = "Success"
                    };
                }
                else
                {
                    return new ResponseResult
                    {
                        Status = true,
                        Status_Code = HttpStatusCode.OK,
                        message = "No data found"
                    };
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ServicePartnerManager", "GetOrderListById", ex);

                return new ResponseResult
                {
                    Status = false,
                    Status_Code = HttpStatusCode.InternalServerError,
                    message = ex.Message
                };
            }
        }

        #endregion

        #region Get Order specific Eraning Details of Driver by Order Tracking Details Id and ServicePartnerId
        public ResponseResult GetDriverEarningDetailsById(int? trackingDetailsId, int servicePartnerId = 0)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            TblVehicleJourneyTrackingDetail? tblTrackingDetails = null;
            VehicleJourneyTrackDetailsModel? trackingDetailsVM = null;
            try
            {
                if (trackingDetailsId > 0)
                {
                    tblTrackingDetails = _vehicleJourneyTrackingDetailsRepository.GetDriverEarningDetail(trackingDetailsId ?? 0, servicePartnerId);
                    if (tblTrackingDetails != null)
                    {
                        trackingDetailsVM = _mapper.Map<TblVehicleJourneyTrackingDetail, VehicleJourneyTrackDetailsModel>(tblTrackingDetails);
                        trackingDetailsVM.RegdNo = tblTrackingDetails.OrderTrans?.RegdNo;
                        trackingDetailsVM.EVCName = tblTrackingDetails.Evc?.BussinessName;
                        responseMessage.message = "Details retrieved successfully.";
                        responseMessage.Status = true;
                        responseMessage.Data = trackingDetailsVM;
                        responseMessage.Status_Code = HttpStatusCode.OK;
                    }
                    else
                    {
                        responseMessage.message = "Details not found.";
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        return responseMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("ServicePartnerManager", "DriverEarningDetailsById", ex);
            }
            return responseMessage;
        }
        #endregion
        #endregion
    }
}