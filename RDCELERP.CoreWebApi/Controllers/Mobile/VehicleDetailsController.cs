using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.CoreWebApi.Models;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.CashfreeModel;
using RDCELERP.Model.DriverDetails;
using RDCELERP.Model.LGC;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.Common;
using RDCELERP.Model.MobileApplicationModel.LGC;
using RDCELERP.Model.ServicePartner;
using RDCELERP.Model.Users;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;
using static Org.BouncyCastle.Math.EC.ECCurve;
using static RDCELERP.Model.Whatsapp.WhatsappLgcPickupViewModel;
using UserDetails = RDCELERP.Model.Whatsapp.WhatsappLgcPickupViewModel.UserDetails;
using WhatsappTemplate = RDCELERP.Model.Whatsapp.WhatsappLgcPickupViewModel.WhatsappTemplate;



namespace RDCELERP.CoreWebApi.Controllers.Mobile
{
    [Route("api/Mobile/[controller]")]
    [ApiController]
    public class VehicleDetailsController : ControllerBase
    {
        #region Variables
        ILogging _logging;
        private readonly ILogin_MobileRepository _login_MobileRepository;
        private readonly IServicePartnerManager _servicePartnerManager;
        private readonly IDriverDetailsManager _driverDetailsManager;
        private readonly IUserRepository _userRepository;
        private readonly IServicePartnerRepository _servicePartnerRepository;
        private CustomDataProtection _protector;
        IOptions<ApplicationSettings> _config;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IDriverDetailsRepository _driverDetailsRepository;
        private readonly IVehicleJourneyTrackingDetailsRepository _journeyTrackingDetailsRepository;
        private readonly IVehicleJourneyTrackingRepository _journeyTrackingRepository;
        private readonly IOrderTransRepository _orderTransRepository;
        private readonly INotificationManager _notificationManager;
        private readonly IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _WhatsAppMessageRepository;
        IEVCRepository _eVCRepository;
        ILogisticManager _logisticManager;
        IPushNotificationManager _pushNotificationManager;


        #endregion

        #region Constructor
        public VehicleDetailsController(ILogging logging, IServicePartnerManager servicePartnerManager, ILogin_MobileRepository login_MobileRepository, IDriverDetailsManager driverDetailsManager, IUserRepository userRepository, IServicePartnerRepository servicePartnerRepository, CustomDataProtection protector, IOptions<ApplicationSettings> config, IUserRoleRepository userRoleRepository, IRoleRepository roleRepository, IDriverDetailsRepository driverDetailsRepository, IVehicleJourneyTrackingDetailsRepository journeyTrackingDetailsRepository, IVehicleJourneyTrackingRepository journeyTrackingRepository, IOrderTransRepository orderTransRepository, Digi2l_DevContext digi2L_DevContext, INotificationManager notificationManager, IWhatsappNotificationManager whatsappNotificationManager, IWhatsAppMessageRepository whatsAppMessageRepository, IEVCRepository eVCRepository, ILogisticManager logisticManager, IPushNotificationManager pushNotificationManager  ) 
        {
            _logging = logging;
            _servicePartnerManager = servicePartnerManager;
            _login_MobileRepository = login_MobileRepository;
            _driverDetailsManager = driverDetailsManager;
            _userRepository = userRepository;
            _servicePartnerRepository = servicePartnerRepository;
            _protector = protector;
            _config = config;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _driverDetailsRepository = driverDetailsRepository;
            _journeyTrackingDetailsRepository = journeyTrackingDetailsRepository;
            _journeyTrackingRepository = journeyTrackingRepository;
            _orderTransRepository = orderTransRepository;
            _notificationManager = notificationManager;
            _whatsappNotificationManager = whatsappNotificationManager;
            _WhatsAppMessageRepository = whatsAppMessageRepository;
            _eVCRepository = eVCRepository;
            _logisticManager = logisticManager;
            _pushNotificationManager = pushNotificationManager;
        }
        #endregion

        #region Test VehicleDetails Controller Api
        [HttpGet]
        [Route("Test")]
        public ResponseResult GetTest()
        {
            ResponseResult responseResult = new ResponseResult();
            try
            {
                responseResult.message = "API Working..";
                responseResult.Status = true;
                responseResult.Status_Code = HttpStatusCode.Accepted;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleDetailsController", "GetTest", ex);
            }
            return responseResult;
        }
        #endregion

        #region used in Mobile app
        #region Get vehicle list by service partner Id and City Name
        [HttpGet]
        [Authorize]
        [Route("GetvehiclelistByLGCIdandCityID")]
        public ResponseResult GetvehiclelistByLGCIdandCityID(int LgcId, string CityName,bool isJourneyPlannedForToday, int pageNumber=1, int pageSize=10, string? filterBy = null)
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.message = string.Empty;

            string username = string.Empty;
            string userRole = string.Empty;
            string userRole1 = string.Empty;

            try
            {

                if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity.Name != null)
                {
                    TblServicePartner tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerIsApprovrd == true && x.ServicePartnerId == LgcId);

                    userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();
                    userRole1 = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner_Driver).ToString();
                    if (tblServicePartner != null && tblServicePartner.ServicePartnerEmailId != null)
                    {

                        var Email = SecurityHelper.EncryptString(tblServicePartner.ServicePartnerEmailId, _config.Value.SecurityKey);
                        TblUser tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.Email == Email);

                        if (tblUser != null)
                        {
                            TblUserRole tblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == tblUser.UserId);
                            if (tblUserRole != null)
                            {
                                TblRole tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == tblUserRole.RoleId);
                                if (tblRole != null && (tblRole.RoleName == userRole || tblRole.RoleName == userRole1 || tblRole.RoleName == EnumHelper.DescriptionAttr(RoleEnum.EVCLGC)))
                                {
                                    responseResult = _driverDetailsManager.VehiclelistbyLGCIdandCityId(LgcId, CityName,  isJourneyPlannedForToday, pageNumber,pageSize,filterBy);
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
                else
                {
                    responseResult.message = "Not A Valid Request";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleDetailsController", "GetvehiclelistByLGCIdandCityID", ex);
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }
        #endregion

        #region Get Assign Order list by DriverDetails Id 
        [HttpGet]
        [Authorize]
        [Route("GetAssignOrderListByvehicle")]
        public ResponseResult GetAssignOrderListByvehicle(int DriverId, bool isJourneyPlannedForToday)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;

            try
            {
                if (DriverId > 0)
                {
                    responseMessage = _driverDetailsManager.GetAssignOrderListByIdDriverDetailsId(DriverId, isJourneyPlannedForToday);

                    if (responseMessage.Status == false && responseMessage.message == string.Empty)
                    {
                        responseMessage.message = "Invalid Request";
                        responseMessage.Status = true;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        return responseMessage;
                    }
                    else
                    {
                        return responseMessage;
                    }
                }
                else
                {
                    responseMessage.Status = false;
                    responseMessage.message = "Invalid UserId";
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleDetailsController", "GetAssignOrderListByvehicle", ex);

                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #region Api for Order reject  by Vehicle
        /// <summary>
        ///  Order Reject by Vehicle 
        /// Api for Order reject  by Vehicle
        /// added by Priyanshi
        /// </summary>
        /// <param name="OrdertransId"></param>
        /// <param name="DriverDetailsId"></param>
        /// <param name="RejectComment"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("RejectOrderbyVehicle")]
        public ResponseResult RejectOrderbyVehicle(RejectVehicleOrderRequest request)
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.message = string.Empty;
            TblUser tblUser = null;
            string username = string.Empty;
            string userRole = string.Empty;
            string userRole1 = string.Empty;

            try
            {
                if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity.Name != null)
                {
                    TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetSingle(x => x.IsActive == true && x.DriverDetailsId == request.DriverDetailsId);

                    userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();
                    userRole1 = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner_Driver).ToString();
                    if (tblDriverDetail != null && tblDriverDetail.DriverPhoneNumber != null)
                    {

                        var DriverPhoneNumber = SecurityHelper.EncryptString(tblDriverDetail.DriverPhoneNumber, _config.Value.SecurityKey);
                        tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.Phone == DriverPhoneNumber);

                        if (tblUser != null)
                        {
                            TblUserRole tblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == tblUser.UserId);
                            if (tblUserRole != null)
                            {
                                TblRole tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == tblUserRole.RoleId);
                                if (tblRole != null && (tblRole.RoleName == userRole || tblRole.RoleName == userRole1 || tblRole.RoleName == EnumHelper.DescriptionAttr(RoleEnum.EVCLGC)))
                                {

                                    responseResult = _driverDetailsManager.RejectOrderbyVehicle(request.OrdertransId, request.DriverDetailsId, request.RejectComment);
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
                else
                {
                    responseResult.message = "Not A Valid Request";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleDetailsController", "AssignVehiclebyLGC", ex);
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }
        #endregion

        #region Api for Order Accept  by Vehicle
        /// <summary>
        ///  Order Accept  by Vehicle
        /// Api for Order Accept  by Vehicle
        /// added by Priyanshi
        /// </summary>
        /// <param name="OrdertransId"></param>
        /// <param name="DriverDetailsId"></param>       
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("AcceptOrderbyVehicle")]
        public ResponseResult AcceptOrderbyVehicle(AcceptOrderbyVehicleRequest request)
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.message = string.Empty;
            TblUser tblUser = null;
            string username = string.Empty;
            string userRole = string.Empty;
            string userRole1 = string.Empty;

            try
            {
                if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity.Name != null)
                {
                    TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetSingle(x => x.IsActive == true && x.DriverDetailsId == request.DriverDetailsId);

                    userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();
                    userRole1 = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner_Driver).ToString();
                    if (tblDriverDetail != null && tblDriverDetail.DriverPhoneNumber != null)
                    {

                        var DriverPhoneNumber = SecurityHelper.EncryptString(tblDriverDetail.DriverPhoneNumber, _config.Value.SecurityKey);
                        tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.Phone == DriverPhoneNumber);

                        if (tblUser != null)
                        {
                            TblUserRole tblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == tblUser.UserId);
                            if (tblUserRole != null)
                            {
                                TblRole tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == tblUserRole.RoleId);
                                if (tblRole != null || (tblRole.RoleName == userRole || tblRole.RoleName == userRole1 || tblRole.RoleName == EnumHelper.DescriptionAttr(RoleEnum.EVCLGC)))
                                {

                                    responseResult = _driverDetailsManager.AcceptOrderbyVehicle(request);
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
                else
                {
                    responseResult.message = "Not A Valid Request";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleDetailsController", "AcceptOrderbyVehicle", ex);
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }
        #endregion

        #region Get Accept Order list by Driver Id 
        [HttpGet]
        [Authorize]
        [Route("GetAcceptOrderListByvehicle")]
        public ResponseResult GetAcceptOrderListByvehicle(int DriverId, DateTime journeyDate)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;

            try
            {
                if (DriverId > 0)
                {
                    var status = Convert.ToInt32(OrderStatusEnum.OrderAcceptedbyVehicle);

                    responseMessage = _driverDetailsManager.GetAcceptOrderListByDriverId(DriverId, journeyDate, status);

                    if (responseMessage.Status == false && responseMessage.message == string.Empty)
                    {
                        responseMessage.message = "Invalid Request";
                        responseMessage.Status = true;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        return responseMessage;
                    }
                    else
                    {
                        return responseMessage;
                    }
                }
                else
                {
                    responseMessage.Status = false;
                    responseMessage.message = "Invalid UserId";
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleDetailsController", "GetAssignOrderListByvehicle", ex);

                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #region Api for Order StartVehicleJourney  by Vehicle
        /// <summary>
        ///  Order StartVehicleJourney  by Vehicle
        /// Api for Order StartVehicleJourney  by Vehicle
        /// added by Priyanshi
        /// </summary>
        /// <param name="StartVehicleJourney"></param>              
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("StartVehicleJourney")]
        public ResponseResult StartVehicleJourney(StartVehicleJourney request)
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.message = string.Empty;
            TblUser tblUser = null;
            string username = string.Empty;
            string userRole = string.Empty;
            string userRole1 = string.Empty;

            try
            {
                if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity.Name != null)
                {
                    TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetSingle(x => x.IsActive == true && x.DriverDetailsId == request.DriverDetailsId);

                    userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();
                    userRole1 = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner_Driver).ToString();
                    if (tblDriverDetail != null && tblDriverDetail.DriverPhoneNumber != null)
                    {

                        var DriverPhoneNumber = SecurityHelper.EncryptString(tblDriverDetail.DriverPhoneNumber, _config.Value.SecurityKey);
                        tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.Phone == DriverPhoneNumber);

                        if (tblUser != null)
                        {
                            TblUserRole tblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == tblUser.UserId);
                            if (tblUserRole != null)
                            {
                                TblRole tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == tblUserRole.RoleId);
                                if (tblRole != null && (tblRole.RoleName == userRole || tblRole.RoleName == userRole1 || tblRole.RoleName == EnumHelper.DescriptionAttr(RoleEnum.EVCLGC)))
                                {
                                    responseResult = _driverDetailsManager.StartVehicleJourney(request);

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
                else
                {
                    responseResult.message = "Not A Valid Request";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleDetailsController", "AcceptOrderbyVehicle", ex);
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }
        #endregion

        #region send OTP in pickup done

        [HttpPost]
        [Route("PickupDoneOTP")]
        public ResultResponse PickupDoneOTP(PickupDoneOTPReq request)
        {
            ResultResponse resultResponse = new ResultResponse();
            string number = string.Empty;
            WhatasappResponse whatasappResponse = new WhatasappResponse();
            TblWhatsAppMessage tblwhatsappmessage = null;

            string tempaltename;
            string mobnumber;
            //bool isMobileNumberValid = false;
            try
            {
                if (request != null && request.OrdertransId > 0)
                {
                    #region send Otp to Customer mobile number for login
                    bool flag = false;
                    TblOrderTran tblOrderTran = _orderTransRepository.GetOrderDetailsByOrderTransId(request.OrdertransId);
                    if (tblOrderTran != null)
                    {
                        if (tblOrderTran.StatusId == Convert.ToInt32(OrderStatusEnum.OrderAcceptedbyVehicle))
                        {
                            MobileOtpVerification mobileOtpVerification = new MobileOtpVerification();
                            mobnumber = tblOrderTran?.Exchange != null ? tblOrderTran?.Exchange?.CustomerDetails?.PhoneNumber : tblOrderTran?.Abbredemption?.CustomerDetails?.PhoneNumber;
                            tempaltename = "SMS_LGCPickup_OTP";
                            string OTPValue = UniqueString.RandomNumber();
                            string message = string.Empty;

                            if (tempaltename.Equals("SMS_LGCPickup_OTP"))
                                message = NotificationConstants.SMS_LGCPickup_OTP.Replace("[OTP]", OTPValue);
                            flag = _notificationManager.SendNotificationSMS(mobnumber, message, OTPValue);

                            #region code to send OTP on whatsappNotification for lgc Pickup
                            WhatsappTemplate whatsappObj = new WhatsappTemplate();
                            whatsappObj.userDetails = new UserDetails();
                            whatsappObj.notification = new LogiPickup();
                            whatsappObj.notification.@params = new SendOTP();
                            whatsappObj.userDetails.number = mobnumber;
                            whatsappObj.notification.sender = _config.Value.YelloaiSenderNumber;
                            whatsappObj.notification.type = _config.Value.YellowaiMesssaheType;
                            whatsappObj.notification.templateId = NotificationConstants.Logi_Pickup;
                            whatsappObj.notification.@params.OTP = OTPValue;
                            string url = _config.Value.YellowAiUrl;
                            RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                            if (response.Content != null)
                            {
                                whatasappResponse = JsonConvert.DeserializeObject<WhatasappResponse>(response.Content);
                                tblwhatsappmessage = new TblWhatsAppMessage();
                                tblwhatsappmessage.TemplateName = NotificationConstants.Logi_Drop;
                                tblwhatsappmessage.IsActive = true;
                                tblwhatsappmessage.PhoneNumber = mobnumber;
                                tblwhatsappmessage.SendDate = DateTime.Now;
                                tblwhatsappmessage.MsgId = whatasappResponse.msgId;
                                tblwhatsappmessage.Code = OTPValue;
                                _WhatsAppMessageRepository.Create(tblwhatsappmessage);
                                _WhatsAppMessageRepository.SaveChanges();
                            }
                            #endregion

                            if (flag)
                            {
                                resultResponse.Status = true;
                                resultResponse.Status_Code = HttpStatusCode.OK;
                                resultResponse.message = "otp successfully send to " + mobnumber;
                                return resultResponse;
                            }
                            else
                            {

                                resultResponse.Status = false;
                                resultResponse.Status_Code = HttpStatusCode.OK;
                                resultResponse.message = "invalid mobile number system not allow send otp to " + mobileOtpVerification.mobileNumber;
                                return resultResponse;
                            }

                        }
                        else
                        {
                            resultResponse.Status = false;
                            resultResponse.Status_Code = HttpStatusCode.OK;
                            resultResponse.message = "This Regno Not Valid";
                            return resultResponse;
                        }
                    }
                    else
                    {
                        resultResponse.Status = false;
                        resultResponse.Status_Code = HttpStatusCode.BadRequest;
                        resultResponse.message = "Order Not  Avalible";
                        return resultResponse;
                    }
                    #endregion

                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleDetailsController", "OTPlogin", ex);
                resultResponse = new ResultResponse();
                resultResponse.message = ex.Message;
                resultResponse.Status = false;
                resultResponse.Status_Code = HttpStatusCode.InternalServerError;
            }
            return resultResponse;
        }
        #endregion

        #region OTP in pickupdone Verfication
        [HttpPost]
        [Route("PickupdoneOTPVerfication")]
        public ResponseResult PickupdoneOTPVerfication(PickupDoneOTPVerficationReq request)
        {
            ResponseResult resultResponse = new ResponseResult();

            string number = string.Empty;
            string email = string.Empty;
            string mobnumber;
            try
            {
                if (request == null)
                {
                    resultResponse.Status = false;
                    resultResponse.message = "Invalid model object";
                    resultResponse.Status_Code = HttpStatusCode.BadRequest;
                    return resultResponse;
                }

                if (request.OrdertransId <= 0 || string.IsNullOrEmpty(request.OTP) || request.DriverDetailsId <= 0)
                {
                    resultResponse.Status = false;
                    resultResponse.Status_Code = HttpStatusCode.BadRequest;
                    resultResponse.message = "Invalid OTP";
                    return resultResponse;
                }

                TblOrderTran tblOrderTran = _orderTransRepository.GetOrderDetailsByOrderTransId(request.OrdertransId);
                if (tblOrderTran != null)
                {
                    mobnumber = tblOrderTran?.Exchange != null ? tblOrderTran?.Exchange?.CustomerDetails?.PhoneNumber : tblOrderTran?.Abbredemption?.CustomerDetails?.PhoneNumber;
                    bool isOTPValid = _notificationManager.ValidateOTP(mobnumber, request.OTP);

                    if (!isOTPValid)
                    {
                        resultResponse.Status = false;
                        resultResponse.Status_Code = HttpStatusCode.BadRequest;
                        resultResponse.message = "Invalid OTP: " + request.OTP;
                        return resultResponse;
                    }
                    else
                    {

                        resultResponse.Status = true;
                        resultResponse.Status_Code = HttpStatusCode.OK;
                        resultResponse.message = "Valid OTP ";
                        return resultResponse;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleDetailsController", "PickupdoneOTPVerfication", ex);
                resultResponse.Status = false;
                resultResponse.message = ex.Message;
                resultResponse.Status_Code = HttpStatusCode.InternalServerError;
            }

            return resultResponse;
        }

        #endregion

        #region Api for Pickup Done 
        /// <summary>
        /// Api Used To Pickup Done using mobile app
        /// created by Priyanshi 
        /// </summary>
        /// <param name="data"></param>
        /// <returns>executionResult</returns>
        /// []
        [Authorize]
        [HttpPost]
        [Route("PickupDone")]
        public ResponseResult PickupDone([FromForm] pickupDoneReq request)
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.message = string.Empty;
            TblUser tblUser = null;
            string username = string.Empty;
            string userRole = string.Empty;
            string userRole1 = string.Empty;

            try
            {
                if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity.Name != null)
                {
                    TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetSingle(x => x.IsActive == true && x.DriverDetailsId == request.DriverDetailsId);

                    userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();
                    userRole1 = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner_Driver).ToString();
                    if (tblDriverDetail != null && tblDriverDetail.DriverPhoneNumber != null)
                    {

                        var DriverPhoneNumber = SecurityHelper.EncryptString(tblDriverDetail.DriverPhoneNumber, _config.Value.SecurityKey);
                        tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.Phone == DriverPhoneNumber);

                        if (tblUser != null)
                        {
                            TblUserRole tblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == tblUser.UserId);
                            if (tblUserRole != null)
                            {
                                TblRole tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == tblUserRole.RoleId);
                                if (tblRole != null && (tblRole.RoleName == userRole || tblRole.RoleName == userRole1 || tblRole.RoleName == EnumHelper.DescriptionAttr(RoleEnum.EVCLGC)))
                                {

                                    responseResult = _driverDetailsManager.PickupDonebyVehicle(request);
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
                else
                {
                    responseResult.message = "Not A Valid Request";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleDetailsController", "AcceptOrderbyVehicle", ex);
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }
        #endregion

        #region Api for Drop Done 
        /// <summary>
        /// Api Used To Drop Done using mobile app
        /// created by Priyanshi 
        /// </summary>
        /// <param name="data"></param>
        /// <returns>executionResult</returns>
        /// []
        [Authorize]
        [HttpPost]
        [Route("DropDone")]
        public ResponseResult DropDone([FromForm] DropDoneReq request)
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.message = string.Empty;
            TblUser tblUser = null;
            string username = string.Empty;
            string userRole = string.Empty;
            string userRole1 = string.Empty;

            try
            {
                if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity.Name != null)
                {
                    if (request == null || request.DriverDetailsId <= 0 || request.ServicePartnerId <= 0)
                    {

                        responseResult.message = "Not Valid login credential";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;

                    }

                    TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetSingle(x => x.IsActive == true && x.DriverDetailsId == request.DriverDetailsId);

                    userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();
                    userRole1 = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner_Driver).ToString();
                    if (tblDriverDetail != null && tblDriverDetail.DriverPhoneNumber != null)
                    {

                        var DriverPhoneNumber = SecurityHelper.EncryptString(tblDriverDetail.DriverPhoneNumber, _config.Value.SecurityKey);
                        tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.Phone == DriverPhoneNumber);

                        if (tblUser != null)
                        {
                            TblUserRole tblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == tblUser.UserId);
                            if (tblUserRole != null)
                            {
                                TblRole tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == tblUserRole.RoleId);
                                if (tblRole != null && (tblRole.RoleName == userRole || tblRole.RoleName == userRole1 || tblRole.RoleName == EnumHelper.DescriptionAttr(RoleEnum.EVCLGC)))
                                {

                                    responseResult = _driverDetailsManager.DropDonebyVehicle(request);


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
                else
                {
                    responseResult.message = "Not A Valid Request";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleDetailsController", "AcceptOrderbyVehicle", ex);
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }
        #endregion

        #region Get Pickup Done Order list by DriverDetails Id 
        [HttpGet]
        [Authorize]
        [Route("GetPickupDoneOrderListByvehicle")]
        public ResponseResult GetPickupDoneOrderListByvehicle(int DriverId, DateTime journeyDate)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;

            try
            {
                if (DriverId > 0)
                {
                    var status = Convert.ToInt32(OrderStatusEnum.LGCPickup);

                    responseMessage = _driverDetailsManager.GetAcceptOrderListByDriverId(DriverId, journeyDate, status);

                    if (responseMessage.Status == false && responseMessage.message == string.Empty)
                    {
                        responseMessage.message = "Invalid Request";
                        responseMessage.Status = true;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        return responseMessage;
                    }
                    else
                    {
                        return responseMessage;
                    }
                }
                else
                {
                    responseMessage.Status = false;
                    responseMessage.message = "Invalid UserId";
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleDetailsController", "GetAssignOrderListByvehicle", ex);

                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #region payment save details 

        /// <summary>
        /// Api Used To SavePaymentRespone using mobile app
        /// created by Priyanshi 
        /// </summary>
        /// <param name="data"></param>
        /// <returns>executionResult</returns>
        /// []       
        [HttpPost]
        [Route("SavePaymentRespone")]

        public ResponseResult SavePaymentRespone(SavePaymentResponce savePaymentResponce)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            try
            {

                if (savePaymentResponce != null)
                {
                    if (savePaymentResponce.Regdno != null)
                    {
                        TblOrderTran tblOrderTrans = _orderTransRepository.GetOrderTransByRegdno(savePaymentResponce.Regdno);
                        if (tblOrderTrans != null)
                        {
                            responseMessage = _driverDetailsManager.savePaymentResponce(savePaymentResponce);
                        }
                        else
                        {
                            responseMessage.Status = true;
                            responseMessage.message = "Invalid Regdno";
                            responseMessage.Status_Code = HttpStatusCode.OK;
                            return responseMessage;
                        }
                    }
                    else
                    {
                        responseMessage.Status = true;
                        responseMessage.message = "Invalid Regdno";
                        responseMessage.Status_Code = HttpStatusCode.OK;
                        return responseMessage;
                    }
                }
                else
                {
                    responseMessage.Status = true;
                    responseMessage.message = "Invalid Objest";
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    return responseMessage;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleDetailsController", "SavePaymentRespone", ex);

                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #region Api for pickup Decline by Driver
        /// <summary>
        /// pickup Decline by Driver
        /// Api  Order pickup decline  by Driver
        /// added by Priyanshi
        /// </summary>
        /// <param name="Order trans Id"></param>
        /// <param name="DriverDetailsId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("PickupDecline")]
        public ResponseResult PickupDecline(PickupDeclineOrderRequest request)
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.message = string.Empty;
            TblUser tblUser = null;
            string username = string.Empty;
            string userRole = string.Empty;
            string userRole1 = string.Empty;

            try
            {
                if (request == null)
                {
                    responseResult.message = "Not A Valid Request";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    return responseResult;
                }
                if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity.Name != null)
                {
                    TblServicePartner tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerIsApprovrd == true && x.ServicePartnerId == request.LGCId);

                    userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();
                    userRole1 = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner_Driver).ToString();

                    if (tblServicePartner != null && tblServicePartner.ServicePartnerEmailId != null)
                    {

                        var Email = SecurityHelper.EncryptString(tblServicePartner.ServicePartnerEmailId, _config.Value.SecurityKey);
                        tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.Email == Email);

                        if (tblUser != null)
                        {
                            TblUserRole tblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == tblUser.UserId);
                            if (tblUserRole != null)
                            {
                                TblRole tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == tblUserRole.RoleId);
                                if (tblRole != null && (tblRole.RoleName == userRole|| tblRole.RoleName == userRole1))
                                {
                                    TblDriverDetail driverDetail = _driverDetailsRepository.GetSingle(x => x.IsActive == true && x.DriverDetailsId == request.DriverDetailsId);
                                    if (driverDetail != null)
                                    {
                                        if (request.OrdertransId > 0)
                                        {
                                            TblOrderTran tblOrderTran = new TblOrderTran();
                                            tblOrderTran = _orderTransRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == request.OrdertransId);
                                            if (tblOrderTran != null)
                                            {
                                                var result = _logisticManager.AddRejectedOrderStatusToDB(tblOrderTran.RegdNo, request.DeclineComment, (int)driverDetail.UserId);
                                                if (result)
                                                {
                                                    responseResult.message = "Successfully Pickup Decline";
                                                    responseResult.Status = true;
                                                    responseResult.Status_Code = HttpStatusCode.OK;
                                                    var Notification = _pushNotificationManager.SendNotification(request.LGCId, request.DriverDetailsId, EnumHelper.DescriptionAttr(NotificationEnum.PickUpDeclinedByCustomer), tblOrderTran.RegdNo, null);
                                                }
                                                else
                                                {
                                                    responseResult.message = "Faild";
                                                    responseResult.Status = false;
                                                    responseResult.Status_Code = HttpStatusCode.OK;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            responseResult.message = "Invalid RegdNo";
                                            responseResult.Status = false;
                                            responseResult.Status_Code = HttpStatusCode.OK;
                                        }
                                    }
                                    else
                                    {
                                        responseResult.message = "Not A Valid User";
                                        responseResult.Status = false;
                                        responseResult.Status_Code = HttpStatusCode.OK;
                                    }
                                }
                                else
                                {
                                    responseResult.message = "Not A Valid User";
                                    responseResult.Status = false;
                                    responseResult.Status_Code = HttpStatusCode.OK;
                                }

                            }
                            else
                            {
                                responseResult.message = "Not A Valid User";
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.OK;
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
                else
                {
                    responseResult.message = "Not A Valid Request";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleDetailsController", "PickupDecline", ex);
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }
        #endregion

        #region Api for pickup Reschedule by Driver
        /// <summary>
        /// PickupReschedule by Driver
        /// Api  Order PickupReschedule by Driver
        /// added by Priyanshi
        /// </summary>
        /// <param name="Order trans Id"></param>
        /// <param name="DriverDetailsId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("PickupReschedule")]
        public ResponseResult PickupReschedule(PickupRescheduleOrderRequest request)
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.message = string.Empty;
            TblUser tblUser = null;
            string username = string.Empty;
            string userRole = string.Empty;
            string userRole1 = string.Empty;

            try
            {
                if (request == null)
                {
                    responseResult.message = "Not A Valid Request";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    return responseResult;
                }

                if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity.Name != null)
                {
                    TblServicePartner tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerIsApprovrd == true && x.ServicePartnerId == request.LGCId);

                    userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();
                    userRole1 = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner_Driver).ToString();

                    if (tblServicePartner != null && tblServicePartner.ServicePartnerEmailId != null)
                    {

                        var Email = SecurityHelper.EncryptString(tblServicePartner.ServicePartnerEmailId, _config.Value.SecurityKey);
                        tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.Email == Email);

                        if (tblUser != null)
                        {
                            TblUserRole tblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == tblUser.UserId);
                            if (tblUserRole != null)
                            {
                                TblRole tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == tblUserRole.RoleId);
                                if (tblRole != null && (tblRole.RoleName == userRole || tblRole.RoleName == userRole1))
                                {

                                    TblDriverDetail driverDetail = _driverDetailsRepository.GetSingle(x => x.IsActive == true && x.DriverDetailsId == request.DriverDetailsId);
                                    if (driverDetail != null)
                                    {
                                        if (request.OrdertransId > 0)
                                        {
                                            TblOrderTran tblOrderTran = new TblOrderTran();
                                            tblOrderTran = _orderTransRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == request.OrdertransId);
                                            if (tblOrderTran != null)
                                            {
                                                var result = _logisticManager.RescheduledLGC(tblOrderTran.RegdNo, request.RescheduleComment,request.RescheduleDate, (int)driverDetail.UserId);
                                                if (result==1)
                                                {
                                                    responseResult.message = "Successfully Pickup Reschedule";
                                                    responseResult.Status = true;
                                                    responseResult.Status_Code = HttpStatusCode.OK;
                                                    var Notification = _pushNotificationManager.SendNotification(request.LGCId, request.DriverDetailsId, EnumHelper.DescriptionAttr(NotificationEnum.PickUpRescheduledByCustomer), request.RescheduleDate?.ToString(), tblOrderTran.RegdNo);
                                                }
                                                else
                                                {
                                                    responseResult.message = "Faild";
                                                    responseResult.Status = false;
                                                    responseResult.Status_Code = HttpStatusCode.OK;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            responseResult.message = "Invalid RegdNo";
                                            responseResult.Status = false;
                                            responseResult.Status_Code = HttpStatusCode.OK;
                                        }
                                    }
                                    else
                                    {
                                        responseResult.message = "Not A Valid User";
                                        responseResult.Status = false;
                                        responseResult.Status_Code = HttpStatusCode.OK;
                                    }
                                }
                                else
                                {
                                    responseResult.message = "Not A Valid User";
                                    responseResult.Status = false;
                                    responseResult.Status_Code = HttpStatusCode.OK;
                                }

                            }
                            else
                            {
                                responseResult.message = "Not A Valid User";
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.OK;
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
                else
                {
                    responseResult.message = "Not A Valid Request";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleDetailsController", "PickupReschedule", ex);
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }
        #endregion

        #region Api for Get Journey details by driverId or servicePartnerId
        [HttpGet]
        [Authorize]
        [Route("GetJourneyDetailsByServicePIdorDriverId")]
        public ResponseResult GetJourneyDetailsByServicePIdorDriverId(int? driverId, int servicePartnerId, int? pagenumber,int? Pagesize,int? vehicleId,DateTime? Journeydate)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            //var pagesize = Convert.ToInt32(PaginationEnum.Pagesize);

            try
            {
                if (servicePartnerId > 0)
                {
                    if (pagenumber <= 0)
                    {
                        pagenumber = 1; // Set default page to 1 if it's less than or equal to 0
                    }
                    if (Pagesize <= 0)
                    {
                        Pagesize = 10; // Set default page size to 10 if it's less than or equal to 0
                    }

                    if(driverId > 0)
                    {
                        driverId = driverId;
                    }
                    else
                    {
                        driverId = null;
                    }
                    
                    responseMessage = _driverDetailsManager.GetJourneyDetailsByServicePIdorDriverId(driverId, servicePartnerId, pagenumber, Pagesize,vehicleId,Journeydate);

                    if (responseMessage.Status == false && responseMessage.message == string.Empty)
                    {
                        responseMessage.message = "Invalid Request";
                        responseMessage.Status = true;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        return responseMessage;
                    }
                    else
                    {
                        return responseMessage;
                    }
                }
                else
                {
                    responseMessage.Status = false;
                    responseMessage.message = "Invalid UserId";
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticController", "GetJourneyDetailsByDriverId", ex);

                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }

        #endregion

        #region Api for Disable Vehicle
        /// <summary>
        /// Api Used To Disable Vehicle
        /// created by priyanshi
        /// </summary>
        /// <param name="data"></param>
        /// <returns>executionResult</returns>
        [Authorize]
        [HttpPost]
        [Route("DisableVehicle")]
        public ResponseResult DisableVehicle(DisableVehicleDataModel data)
        {
            ResponseResult responseMessage = new ResponseResult();
            string username = string.Empty;
            string userRole = string.Empty;
            DAL.Entities.TblUser tblUser = null;
            try
            {
                if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;

                    //userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.LGC_Admin).ToString();
                    userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();

                    TblServicePartner tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerIsApprovrd == true && x.ServicePartnerEmailId == username.ToString() || x.ServicePartnerMobileNumber == username);

                    if (tblServicePartner != null && tblServicePartner.ServicePartnerEmailId != null)
                    {

                        var Email = SecurityHelper.EncryptString(tblServicePartner.ServicePartnerEmailId, _config.Value.SecurityKey);
                        tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.Email == Email);
                        if (tblUser != null)
                        {
                            TblUserRole tblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == tblUser.UserId);
                            if (tblUserRole != null)
                            {
                                TblRole tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == tblUserRole.RoleId);
                                if (tblRole != null && tblRole.RoleName == userRole)
                                {
                                    if (data != null)
                                    {
                                        //Manager call

                                        responseMessage = _servicePartnerManager.DisableVehicle(data);
                                        if (responseMessage.Status == true)
                                        {
                                            responseMessage.Status_Code = HttpStatusCode.OK;
                                            return responseMessage;
                                        }
                                        else
                                        {
                                            if (responseMessage.Status == false)
                                            {
                                                return responseMessage;
                                            }
                                            else
                                            {
                                                responseMessage.Status_Code = HttpStatusCode.BadRequest;
                                                responseMessage.Status = false;
                                                responseMessage.message = "Not valid Request";
                                                return responseMessage;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        responseMessage = new ResponseResult();
                                        responseMessage.message = " Request object should not be null or greater than five";
                                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                                        return responseMessage;
                                    }
                                }
                                else
                                {
                                    responseMessage.message = "Not A Valid User";
                                    responseMessage.Status = false;
                                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                                }

                            }
                            else
                            {
                                responseMessage.message = "Not A Valid User";
                                responseMessage.Status = false;
                                responseMessage.Status_Code = HttpStatusCode.BadRequest;
                            }


                        }

                        else
                        {
                            responseMessage.message = "Not A Valid User";
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseMessage.message = "Service partner not a valid user";
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseMessage.message = "Not A Valid Request";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                }

            }
            catch (Exception ex)
            {
                responseMessage = new ResponseResult();
                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #endregion

        #region not use in Mobile app
        #region Get vehicle details by login service partner
        [HttpGet]
        [Authorize]
        [Route("GetVehicleDetails")]
        public ResponseResult GetVehicleDetails()
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.message = string.Empty;
            string username = string.Empty;
            string userRole = string.Empty;
            string userRole1 = string.Empty;

            TblLoginMobile tblLoginMobile = null;
            try
            {
                if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;
                    userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();
                    userRole1 = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner_Driver).ToString();
                    tblLoginMobile = _login_MobileRepository.GetSingle(x => x.IsActive == true && x.Username == username.ToString());
                    if (tblLoginMobile != null && (tblLoginMobile.UserRoleName == userRole || tblLoginMobile.UserRoleName == userRole1))
                    {
                        responseResult = _driverDetailsManager.VehicleDetailsList(username);
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
                    responseResult.message = "Not A Valid Request";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleDetailsController", "GetVehicleDetails", ex);
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }
        #endregion

        #region Get vehicle details by login service partner
        [HttpGet]
        [Authorize]
        [Route("GetCititis")]
        public ResponseResult GetCititis()
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.message = string.Empty;
            string username = string.Empty;
            string userRole = string.Empty;
            string userRole1 = string.Empty;

            TblLoginMobile tblLoginMobile = null;
            try
            {
                if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;
                    userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();
                    userRole1 = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner_Driver).ToString();
                    tblLoginMobile = _login_MobileRepository.GetSingle(x => x.IsActive == true && x.Username == username.ToString());
                    if (tblLoginMobile != null && (tblLoginMobile.UserRoleName == userRole || tblLoginMobile.UserRoleName == userRole1))
                    {
                        responseResult = _driverDetailsManager.VehicleDetailsList(username);
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
                    responseResult.message = "Not A Valid Request";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleDetailsController", "GetVehicleDetails", ex);
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }
        #endregion
        #endregion
    }
}

