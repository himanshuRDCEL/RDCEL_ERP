using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Helper;
using RDCELERP.CoreWebApi.Configuration;
using RDCELERP.CoreWebApi.Models;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.Common;
using RDCELERP.Model.Product;
using RDCELERP.Model.Users;
using static RDCELERP.Common.Helper.MessageHelper;

namespace RDCELERP.CoreWebApi.Controllers.Common
{

    [Route("api/Common/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        #region Variables Declaration
        private readonly JwtBearerTokenSettings jwtBearerTokenSettings;
        private readonly IUserManager _userManager;
        public readonly IOptions<ApplicationSettings> _baseConfig;
        INotificationManager _notificationManager;
        IPushNotificationManager _pushnotificationManager;
        private readonly IServicePartnerManager _servicePartnerManager;
        ILogging _logging;
        IServicePartnerRepository _servicePartnerRepository;

        #endregion

        #region Constructors
        public LoginController(IServicePartnerManager servicePartnerManager, INotificationManager notificationManager, ILogging logging, IUserManager userManager, IOptions<ApplicationSettings> baseConfig, IOptions<JwtBearerTokenSettings> jwtTokenOptions, IPushNotificationManager pushnotificationManager,IServicePartnerRepository servicePartnerRepository)
        {
            _userManager = userManager;
            _baseConfig = baseConfig;
            this.jwtBearerTokenSettings = jwtTokenOptions.Value;
            _logging = logging;
            _notificationManager = notificationManager;
            _servicePartnerManager = servicePartnerManager;
            _pushnotificationManager = pushnotificationManager;
            _servicePartnerRepository = servicePartnerRepository;
        }
        #endregion

        #region Test LoginController Api
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
                _logging.WriteErrorToDB("LoginController", "GetTest", ex);
            }
            return responseResult;
        }
        #endregion

        #region UserLogin
        [HttpPost]
        [Route("UserLogin")]
        public ExecutionResult GetUserByLoginPost(string username, string password)
        {
            string pwd = SecurityHelper.EncryptString(password.ToString(), _baseConfig.Value.SecurityKey);
            string email = SecurityHelper.EncryptString(username.ToString(), _baseConfig.Value.SecurityKey);
            return _userManager.UserByLogin(email, pwd);
        }
        #endregion

        #region MobileUserLogin
        [HttpPost]
        [Route("MobileUserLogin")]
        public ResultResponse UserLogin(MobileUserLoginDataModel loginDetails)
        {
            ResultResponse resultResponse = new ResultResponse();
            ResponseResult responseResult = new ResponseResult();
            OtpWithUserInfo otpWithUserInfo = new OtpWithUserInfo();
            string pwd = string.Empty;
            string email = string.Empty;
            try
            {
                if (loginDetails != null)
                {
                    #region login by email and password
                    if (loginDetails.loginByNumber == false && loginDetails.username != null && loginDetails.username.Length > 0 && loginDetails.password != null && loginDetails.password.Length > 0)
                    {
                        responseResult = _servicePartnerManager.GetServicePartnerByUserId(loginDetails.username, loginDetails.password);
                        email = loginDetails.username;
                        pwd = loginDetails.password;
                    }
                    #endregion

                    #region login by phone number
                    if (loginDetails.loginByNumber == true && loginDetails.MobileNumber.Length > 0 && loginDetails.Otp == null)
                    {
                        #region send Otp to mobile number for login
                        bool flag = false;

                        MobileOtpVerification mobileOtpVerification = new MobileOtpVerification();
                        mobileOtpVerification.mobileNumber = loginDetails.MobileNumber;
                        mobileOtpVerification.requestFor = "SMS_LGCPickup_OTP";

                        string message = string.Empty;
                        string OTPValue = UniqueString.RandomNumber();
                        if (mobileOtpVerification.requestFor.Equals("SMS_LGCPickup_OTP"))
                        {
                            message = NotificationConstants.SMS_LGCPickup_OTP.Replace("[OTP]", OTPValue);
                            otpWithUserInfo.UserRoleName = string.Empty;
                            otpWithUserInfo = _servicePartnerManager.IsValidMobileNumber(loginDetails.MobileNumber);
                            if (otpWithUserInfo != null && otpWithUserInfo.UserId > 0)
                            {
                                flag = _notificationManager.SendNotificationSMS(mobileOtpVerification.mobileNumber, message, OTPValue);
                                if (flag)
                                {
                                    resultResponse.Data = otpWithUserInfo;
                                    resultResponse.Status = true;
                                    resultResponse.Status_Code = HttpStatusCode.OK;
                                    resultResponse.message = "otp successfully send to " + mobileOtpVerification.mobileNumber;
                                    return resultResponse;
                                }
                                else
                                {

                                    resultResponse.Status = false;
                                    resultResponse.Status_Code = HttpStatusCode.BadRequest;
                                    resultResponse.message = "invalid mobile number system not allow send otp to " + mobileOtpVerification.mobileNumber;
                                    return resultResponse;
                                }
                            }
                            else if (otpWithUserInfo != null && otpWithUserInfo.UserId == 0)
                            {
                                resultResponse.Status = false;
                                resultResponse.Status_Code = HttpStatusCode.BadRequest;
                                //add error message in variable userrolename 
                                resultResponse.message = otpWithUserInfo.UserRoleName;
                                return resultResponse;
                            }
                            else
                            {
                                resultResponse.Status = false;
                                resultResponse.Status_Code = HttpStatusCode.BadRequest;
                                resultResponse.message = mobileOtpVerification.mobileNumber + " Mobile Number is not Register.";
                                return resultResponse;
                            }
                        }
                        #endregion

                    }
                    else if (loginDetails.Otp != null && loginDetails.Otp.Length > 1 && loginDetails.MobileNumber != null
                        && loginDetails.MobileNumber.Length > 0 && loginDetails.UserRoleName != null && loginDetails.loginByNumber == true)
                    {
                        if (loginDetails.Otp != null)
                        {
                            bool flag = _notificationManager.ValidateOTP(loginDetails.MobileNumber, loginDetails.Otp);
                            if (flag)
                            {
                                responseResult = _servicePartnerManager.GetLoginUserDetails(loginDetails.MobileNumber, loginDetails.UserRoleName, Convert.ToInt32(loginDetails.UserId));
                                if (responseResult != null)
                                {
                                    bool updatetblLoginMobile = _servicePartnerManager.UpdateMobileLogindetails(loginDetails.DeviceType, loginDetails.DeviceId, Convert.ToInt32(loginDetails.UserId));
                                    email = loginDetails.MobileNumber;
                                    pwd = loginDetails.MobileNumber;
                                }
                                else
                                {
                                    responseResult = new ResponseResult();
                                    responseResult.Status = false;
                                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                                    responseResult.message = "User Data Not Found";
                                }
                            }
                            else
                            {
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.BadRequest;
                                responseResult.message = "invalid OTP " + loginDetails.Otp;
                            }
                        }
                        else
                        {
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            responseResult.message = "invalid OTP";
                        }
                    }

                    #endregion

                    #region genrating token
                    if (responseResult != null && responseResult.Status == true)
                    {
                        LoginModel loginModel = new LoginModel();
                        loginModel.Username = email;
                        loginModel.Password = pwd;
                        loginModel.email = email;

                        var token = GenerateToken(loginModel);

                        resultResponse.message = responseResult.message;
                        resultResponse.Status = responseResult.Status;
                        resultResponse.Status_Code = responseResult.Status_Code;

                        resultResponse.Data = responseResult.Data;
                        if (token != null)
                        {
                            resultResponse.token = token;
                        }
                        else
                        {
                            resultResponse = new ResultResponse();
                            resultResponse.Status = false;
                            resultResponse.Status_Code = HttpStatusCode.NotFound;
                            resultResponse.message = "token not generated";
                        }

                        return resultResponse;
                    }
                    else
                    {
                        if (responseResult.Status == false && responseResult.Status_Code > 0)
                        {
                            resultResponse.message = responseResult.message;
                            resultResponse.Status = responseResult.Status;
                            resultResponse.Status_Code = responseResult.Status_Code;
                            return resultResponse;
                        }
                        else
                        {
                            resultResponse.Status = false;
                            resultResponse.message = "Not Valid Request";
                            resultResponse.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    #endregion
                }
                else
                {
                    resultResponse.Status = false;
                    resultResponse.message = "Not success,due to invalid model object";
                    resultResponse.Status_Code = HttpStatusCode.BadRequest;
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LoginController", "UserLogin", ex);
                resultResponse = new ResultResponse();
                resultResponse.message = ex.Message;
                resultResponse.Status = false;
                resultResponse.Status_Code = HttpStatusCode.InternalServerError;
            }
            return resultResponse;
        }
        #endregion

        #region Login by Email & Password
        [HttpPost]
        [Route("Login")]
        public ResultResponse Login(LoginByEmail loginDetails)
        {
            ResultResponse resultResponse = new ResultResponse();
            ResponseResult responseResult = new ResponseResult();
            OtpWithUserInfo otpWithUserInfo = new OtpWithUserInfo();
            string pwd = string.Empty;
            string email = string.Empty;
            try
            {
                if (loginDetails != null)
                {
                    pwd = SecurityHelper.EncryptString(loginDetails.password, _baseConfig.Value.SecurityKey);
                    email = SecurityHelper.EncryptString(loginDetails.email, _baseConfig.Value.SecurityKey);

                    UserViewModel userViewModel = _userManager.GetUserByEmailandPasswordLogin(email, pwd);

                    if (userViewModel == null || userViewModel.UserId == 0)
                    {
                        resultResponse.Status = false;
                        resultResponse.Status_Code = HttpStatusCode.BadRequest;
                        resultResponse.message = "User data not found";
                        return resultResponse;
                    }

                    if (loginDetails.DeviceId != null)
                    {
                        bool isDeviceIdSaved = _pushnotificationManager.SaveDeviceId(loginDetails.DeviceId, loginDetails.DeviceType, userViewModel.UserId);
                    }

                    #region login by email and password
                    if (loginDetails.email != null && loginDetails.email.Length > 0 && loginDetails.password != null && loginDetails.password.Length > 0)
                    {
                        userDataViewModal userDetails = new userDataViewModal();
                        userDetails.Email = email;
                        userDetails.Password = pwd;
                        resultResponse = _userManager.LoginUserDetails(userDetails);
                        email = loginDetails.email;
                        pwd = loginDetails.password;
                    }
                    #endregion

                    

                    #region genrating token
                    if (resultResponse != null && resultResponse.Status == true)
                    {
                        LoginModel loginModel = new LoginModel();
                        loginModel.Username = email;
                        loginModel.Password = pwd;
                        loginModel.email = email;

                        var token = GenerateToken(loginModel);

                        if (token != null)
                        {
                            resultResponse.token = token;
                        }
                        else
                        {
                            resultResponse = new ResultResponse();
                            resultResponse.Status = false;
                            resultResponse.Status_Code = HttpStatusCode.NotFound;
                            resultResponse.message = "token not generated";
                        }

                        return resultResponse;
                    }
                    else
                    {
                        if (responseResult.Status == false && responseResult.Status_Code > 0)
                        {
                            resultResponse.message = responseResult.message;
                            resultResponse.Status = responseResult.Status;
                            resultResponse.Status_Code = responseResult.Status_Code;
                            return resultResponse;
                        }
                        else
                        {
                            resultResponse.Status = false;
                            resultResponse.message = "Not Valid Request";
                            resultResponse.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    #endregion
                }
                else
                {
                    resultResponse.Status = false;
                    resultResponse.message = "Not success,due to invalid model object";
                    resultResponse.Status_Code = HttpStatusCode.BadRequest;
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LoginController", "UserLogin", ex);
                resultResponse = new ResultResponse();
                resultResponse.message = ex.Message;
                resultResponse.Status = false;
                resultResponse.Status_Code = HttpStatusCode.InternalServerError;
            }
            return resultResponse;
        }
        #endregion

        #region OTP LOGIN SEND OTP
        [HttpPost]
        [Route("OTPLOGIN")]
        public ResultResponse OTPlogin(OTPLOGIN loginDetails)
        {
            ResultResponse resultResponse = new ResultResponse();
            string number = string.Empty;
            //bool isMobileNumberValid = false;
            try
            {
                if (loginDetails != null && loginDetails.MobileNumber.Length == 10)
                {
                    number = SecurityHelper.EncryptString(loginDetails.MobileNumber, _baseConfig.Value.SecurityKey);

                    TblServicePartner servicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive==true && x.ServicePartnerMobileNumber == loginDetails.MobileNumber);
                    if (servicePartner != null)
                    {
                        if (servicePartner.UserId == null)
                        {                       
                                resultResponse.Status = false;
                                resultResponse.Status_Code = HttpStatusCode.OK;
                                resultResponse.message = loginDetails.MobileNumber + " Mobile Number is NotActive,If you Already Register please wait for approval";
                                return resultResponse;                           
                        }
                    }
                    userDataViewModal userDetail = _userManager.GetUserByNumber(number);                    
                    if (userDetail == null || userDetail.UserId == 0)
                    {
                        resultResponse.Status = false;
                        resultResponse.Status_Code = HttpStatusCode.OK;
                        resultResponse.message = "User Not found";
                        return resultResponse;
                    }
                    #region send Otp to mobile number for login
                    bool flag = false;

                    MobileOtpVerification mobileOtpVerification = new MobileOtpVerification();
                    mobileOtpVerification.mobileNumber = loginDetails.MobileNumber;
                    mobileOtpVerification.requestFor = "SMS_Login_OTP";

                    string message = string.Empty;
                    string OTPValue = UniqueString.RandomNumber();
                    if (mobileOtpVerification.requestFor.Equals("SMS_Login_OTP"))
                    {
                        message = NotificationConstants.SMS_Login_OTP.Replace("[OTP]", OTPValue);
                        number = SecurityHelper.EncryptString(loginDetails.MobileNumber, _baseConfig.Value.SecurityKey);
                        userDataViewModal userDetails = new userDataViewModal();
                        userDetails = _userManager.GetUserByNumber(number);
                        if (userDetails != null && userDetails.UserId > 0)
                        {
                            flag = _notificationManager.SendNotificationSMS(mobileOtpVerification.mobileNumber, message, OTPValue);
                            if (flag)
                            {
                                resultResponse.Status = true;
                                resultResponse.Status_Code = HttpStatusCode.OK;
                                resultResponse.message = "otp successfully send to " + mobileOtpVerification.mobileNumber;
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
                            resultResponse.message = mobileOtpVerification.mobileNumber + " Mobile Number is NotActive,If you Already Register please wait for approval";
                            return resultResponse;
                        }
                    }
                    else
                    {
                        resultResponse.Status = false;
                        resultResponse.Status_Code = HttpStatusCode.BadRequest;
                        resultResponse.message = mobileOtpVerification.mobileNumber + " invalid request type";
                        return resultResponse;
                    }
                    #endregion
                }
                else
                {
                    resultResponse.Status = false;
                    resultResponse.message = "Not success,due to invalid model object";
                    resultResponse.Status_Code = HttpStatusCode.BadRequest;
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LoginController", "OTPlogin", ex);
                resultResponse = new ResultResponse();
                resultResponse.message = ex.Message;
                resultResponse.Status = false;
                resultResponse.Status_Code = HttpStatusCode.InternalServerError;
            }
            return resultResponse;
        }


        #endregion

        #region OTP login Verfication
        [HttpPost]
        [Route("OTPloginVerfication")]
        public ResultResponse OtpLoginVerification(OTPLOGIN loginDetails)
        {
            ResultResponse resultResponse = new ResultResponse();
            ResponseResult responseResult = new ResponseResult();
            string number = string.Empty;
            string email = string.Empty;

            try
            {
                if (loginDetails == null)
                {
                    resultResponse.Status = false;
                    resultResponse.message = "Invalid model object";
                    resultResponse.Status_Code = HttpStatusCode.BadRequest;
                    return resultResponse;
                }

                if (loginDetails.MobileNumber.Length != 10 || string.IsNullOrEmpty(loginDetails.OTP))
                {
                    resultResponse.Status = false;
                    resultResponse.Status_Code = HttpStatusCode.BadRequest;
                    resultResponse.message = "Invalid OTP";
                    return resultResponse;
                }

                bool isOTPValid = _notificationManager.ValidateOTP(loginDetails.MobileNumber, loginDetails.OTP);
                if (!isOTPValid)
                {
                    resultResponse.Status = false;
                    resultResponse.Status_Code = HttpStatusCode.BadRequest;
                    resultResponse.message = "Invalid OTP: " + loginDetails.OTP;
                    return resultResponse;
                }

                number = SecurityHelper.EncryptString(loginDetails.MobileNumber, _baseConfig.Value.SecurityKey);
                userDataViewModal userDetails = _userManager.GetUserByNumber(number);

                if (userDetails == null || userDetails.UserId == 0)
                {
                    resultResponse.Status = false;
                    resultResponse.Status_Code = HttpStatusCode.BadRequest;
                    resultResponse.message = "User data not found";
                    return resultResponse;
                }

                resultResponse = _userManager.LoginUserDetails(userDetails);
                if(loginDetails.DeviceId != null)
                {
                    bool isDeviceIdSaved = _pushnotificationManager.SaveDeviceId(loginDetails.DeviceId, loginDetails.DeviceType, userDetails.UserId);
                }
                
                email = loginDetails.MobileNumber;

                if (resultResponse != null && resultResponse.Status)
                {
                    LoginModel loginModel = new LoginModel();
                    loginModel.Username = email;
                    loginModel.Password = loginDetails.MobileNumber;
                    loginModel.email = email;

                    var token = GenerateToken(loginModel);

                    if (token != null)
                    {
                        resultResponse.token = token;
                    }
                    else
                    {
                        resultResponse.Status = false;
                        resultResponse.message = "Token not generated";
                        resultResponse.Status_Code = HttpStatusCode.NotFound;
                    }
                }
                else
                {
                    resultResponse.Status = false;
                    resultResponse.message = "Not a valid request";
                    resultResponse.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LoginController", "OtpLoginVerification", ex);
                resultResponse.Status = false;
                resultResponse.message = ex.Message;
                resultResponse.Status_Code = HttpStatusCode.InternalServerError;
            }

            return resultResponse;
        }

        #endregion
        //#region  Token Generate Method
        //private object GenerateToken(LoginModel identityUser)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim(ClaimTypes.Name, identityUser.Username.ToString()),
        //            new Claim(ClaimTypes.Email, identityUser.Username)
        //        }),

        //        Expires = DateTime.UtcNow.AddSeconds(jwtBearerTokenSettings.ExpiryTimeInSeconds),
        //        //Expires = DateTime.UtcNow.AddYears(25),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        //        Audience = jwtBearerTokenSettings.Audience,
        //        Issuer = jwtBearerTokenSettings.Issuer
        //    };

        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}
        //#endregion

        #region  Token Generate Method
        private object GenerateToken(LoginModel identityUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, identityUser.Username.ToString()),
                    new Claim(ClaimTypes.Email, identityUser.Username)
                }),

                Expires = DateTime.UtcNow.AddSeconds(jwtBearerTokenSettings.ExpiryTimeInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtBearerTokenSettings.Audience,
                Issuer = jwtBearerTokenSettings.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        #endregion

        #region Logout Api
        [HttpPost]
        [Authorize]
        [Route("LogOut")]
        public ResponseResult LogOut(int userId)
        {
            ResponseResult responseResult = new ResponseResult();

            try
            {
                if(userId > 0)
                {
                    responseResult = _userManager.UpdateDeviceId(userId);
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.message = "Invalid UserId";
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LoginController", "OtpLoginVerification", ex);
                responseResult.Status = false;
                responseResult.message = ex.Message;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }
        #endregion

    }
}