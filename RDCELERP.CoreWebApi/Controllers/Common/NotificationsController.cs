using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.Common;

namespace RDCELERP.CoreWebApi.Controllers.Common
{
    [Route("api/Common/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        #region Variable Declaration
        private readonly ICityManager _cityManager;
        private readonly ICityRepository _cityRepository;
        INotificationManager _notificationManager;
        ILogging _logging;
        ILoginRepository _loginRepository;
        IBusinessUnitRepository _businessUnitRepository;
        #endregion

        #region Constructor
        public NotificationsController(ILogging logging, ICityRepository cityRepository, ICityManager cityManager, INotificationManager notificationManager, ILoginRepository loginRepository, IBusinessUnitRepository businessUnitRepository)
        {
            _cityRepository = cityRepository;
            _cityManager = cityManager;
            _notificationManager = notificationManager;
            _logging = logging;
            _loginRepository = loginRepository;
            _businessUnitRepository = businessUnitRepository;
        }
        #endregion

        #region Test NotificationsController Api
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
                _logging.WriteErrorToDB("NotificationsController", "GetTest", ex);
            }
            return responseResult;
        }
        #endregion

        #region SendOtp_Mobile
        /// <summary>
        /// Send Otp to service partner at time of registeration through mobile api
        /// added by ashwin
        /// </summary>
        /// <param name="mobileOtpVerification"></param>
        /// <returns>responseResult</returns>
        [HttpPost]
        [Route("SendOtp")]
        public ResponseResult SendOtp_Mobile([FromForm] MobileOtpVerification mobileOtpVerification)
        {
            ResponseResult responseResult = new ResponseResult();           
            try
            {
                if (mobileOtpVerification.mobileNumber != string.Empty || mobileOtpVerification.requestFor != string.Empty )
                {
                    bool flag = false;
                    string message = string.Empty;
                    string OTPValue = UniqueString.RandomNumber();
                    if (mobileOtpVerification.requestFor.Equals("SMS_LGCPickup_OTP"))
                    {
                        message = NotificationConstants.SMS_LGCPickup_OTP.Replace("[OTP]", OTPValue);

                        flag = _notificationManager.SendNotificationSMS(mobileOtpVerification.mobileNumber, message, OTPValue);
                        if (flag)
                        {
                            responseResult.Status = true;
                            responseResult.Status_Code = HttpStatusCode.OK;
                            responseResult.message = "otp successfully send to " + mobileOtpVerification.mobileNumber;
                            return responseResult;
                        }
                        else
                        {
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            responseResult.message = "invalid mobile number system not allow send otp to " + mobileOtpVerification.mobileNumber;
                        }
                    }
                    else
                    {
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        responseResult.message = "not valid request type " + mobileOtpVerification.requestFor;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    responseResult.message = "Invalid request parameters,please use valid mobileNumber and required requestFor";
                }
            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("NotificationsController", "SendOtp_Mobile", ex);
            }
            return responseResult;
        }
        #endregion

        #region VerifyOtp_Mobile
        /// <summary>
        /// verify otp at time of service partner registeration
        /// </summary>
        /// <param name="mobileOtpVerification"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VerifyOtp")]
        public ResponseResult VerifyOtp_Mobile([FromForm] VerifyOtp verify)
        {
            ResponseResult responseResult = new ResponseResult();
            try
            {
                if (verify.mobileNumber != string.Empty || verify.Otp != string.Empty)
                {
                    if (verify.Otp != null )
                    {
                        bool flag = _notificationManager.ValidateOTP(verify.mobileNumber, verify.Otp);
                        if (flag)
                        {
                            responseResult.Status = true;
                            responseResult.Status_Code = HttpStatusCode.OK;
                            responseResult.message = "OTP verified";
                            return responseResult;
                        }
                        else
                        {
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            responseResult.message = "invalid OTP " + verify.Otp;
                        }
                    }
                    else
                    {
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        responseResult.message = "invalid OTP";
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    responseResult.message = "Invalid request parameters,please use valid mobileNumber and required otp";
                }
            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("NotificationsController", "VerifyOtp_Mobile", ex);
            }
            return responseResult;
        }
        #endregion

        #region OtpSend BU Based Template Integration
        /// <summary>
        /// Send Otp to customer for verify mobile number according to login BUID
        /// Used in Questioner's 
        /// added by ashwin
        /// </summary>
        /// <param name="mobileOtpVerification"></param>
        /// <returns>responseResult</returns>
        [Authorize]
        [HttpPost]
        [Route("OtpSend")]
        public ResponseResult OtpSend([FromForm] OTPSent oTPSent)
        {
            ResponseResult responseResult = new ResponseResult();
            try
            {
                string userName = string.Empty;

                if (HttpContext.User != null && HttpContext.User != null
                        && HttpContext.User.Identity.Name != null)
                {
                    userName = HttpContext.User.Identity.Name;
                    Login login = _loginRepository.GetSingle(x => x.SponsorId > 0 && !string.IsNullOrEmpty(x.Username) && x.Username.ToLower().Equals(userName.ToLower()));
                    
                    if (oTPSent!=null && !string.IsNullOrEmpty(oTPSent.mobileNumber) && login!=null)
                    {
                        bool flag = false;
                        string message = string.Empty;
                        string OTPValue = UniqueString.RandomNumber();
                        TblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == login.SponsorId);
                        if(businessUnit!=null && (!string.IsNullOrEmpty(businessUnit.Name)))
                        {
                            message = NotificationConstants.SMS_ExchangeOtp.Replace("[OTP]", OTPValue).Replace("[BrandName]", businessUnit.Name);

                            flag = _notificationManager.SendNotificationSMS(oTPSent.mobileNumber, message, OTPValue);
                            if (flag)
                            {
                                responseResult.Status = true;
                                responseResult.Status_Code = HttpStatusCode.OK;
                                responseResult.message = "otp successfully send to " + oTPSent.mobileNumber;
                                return responseResult;
                            }
                            else
                            {
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.BadRequest;
                                responseResult.message = "invalid mobile number system not allow send otp to " + oTPSent.mobileNumber;
                            }
                        }
                        else
                        {
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            responseResult.message = "Bussiness Unit Not Found for login credential " + oTPSent.mobileNumber;
                        }
                    }
                    else
                    {
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        responseResult.message = "login details not found to send otp";
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.Unauthorized;
                    responseResult.message = "Authorization Failed";
                }


            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("NotificationsController", "SendOtp_Mobile", ex);
            }
            return responseResult;
        }
        #endregion

    }
}
