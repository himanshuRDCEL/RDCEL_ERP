using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using RDCELERP.BAL.Helper;
using RDCELERP.Common.Constant;
using RDCELERP.Model.MobileApplicationModel.Common;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using Microsoft.AspNetCore.Authorization;
using RDCELERP.BAL.MasterManager;

namespace RDCELERP.CoreWebApi.Controllers.Common
{
   [Route("api/Common/[controller]")]
    [ApiController]
    public class RecommerceD2CController : ControllerBase
    {

        INotificationManager _notificationManager;
        ILogging _logging;

         
        public RecommerceD2CController(INotificationManager notificationManager,ILogging logging)
        {
           _notificationManager = notificationManager;
            _logging = logging;
        }

        [Authorize]
        [HttpPost]
        [Route("SendOtp")]
        public ResponseResult SendOtp_SmartBuy(string MobileNumber)
        {
            ResponseResult responseResult = new ResponseResult();
            try
            {
                if (MobileNumber != string.Empty )
                {
                    bool flag = false;
                    string message = string.Empty;
                    string OTPValue = UniqueString.RandomNumber();
                   
                        message = NotificationConstants.SmartBuy_otp_30012024.Replace("[OTP]", OTPValue);
                    

                        flag = _notificationManager.SendNotificationSMS(MobileNumber, message, OTPValue);
                        if (flag)
                        {
                            responseResult.Status = true;
                            responseResult.Status_Code = HttpStatusCode.OK;
                            responseResult.message = "otp successfully send to " + MobileNumber;
                            return responseResult;
                        }
                        else
                        {
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            responseResult.message = "invalid mobile number system not allow send otp to " + MobileNumber;
                        }
                  
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    responseResult.message = "Invalid request parameters,please use valid mobile Number and required requestFor";
                }
            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("RecommerceD2CController", "SendOtp_SmartBuy", ex);
            }
            return responseResult;
        }
    }
}
