using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.PushNotification;

namespace RDCELERP.CoreWebApi.Controllers.Mobile
{
    [Route("api/Mobile/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IPushNotificationManager _pushNotificationManager;
        ILogging _logging;

        public NotificationController(IPushNotificationManager pushNotificationManager, ILogging logging)
        {
            _pushNotificationManager = pushNotificationManager;
            _logging = logging;
        }

        #region TestNotification Controller Api
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
                _logging.WriteErrorToDB("NotificationController", "GetTest", ex);
            }
            return responseResult;
        }
        #endregion

        #region Api for GetNotificationListById  by User Id
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="page"></param>
        /// <param name="Alldata"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("GetNotificationListById")]
        public ResponseResult GetNotificationListById(int Id, int page, bool? Alldata)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            var pagesize = Convert.ToInt32(PaginationEnum.Pagesize);
            try
            {
                if (Id > 0)
                {
                    if (page <= 0)
                    {
                        page = 1; // Set default page to 1 if it's less than or equal to 0
                    }
                    if (pagesize <= 0)
                    {
                        pagesize = 10; // Set default page size to 10 if it's less than or equal to 0
                    }

                    responseMessage = _pushNotificationManager.GetNotificationListById(Id, page, pagesize);

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
                _logging.WriteErrorToDB("NotificationController", "GetNotificationById", ex);

                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        //[Route("SendNotification")]
        //[HttpPost]
        //public async Task<IActionResult> SendNotification(PushNotificationViewModel notificationModel)
        //{
        //    var result =  _pushNotificationManager.SendNotification(notificationModel);
        //    return Ok(result);
        //}
    }
}