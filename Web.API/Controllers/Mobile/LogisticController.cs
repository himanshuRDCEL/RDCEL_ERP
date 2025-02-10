using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.LGC;

namespace UTC.API.Controllers.Mobile
{

    [Route("api/Mobile/[controller]")]
    [ApiController]
    public class LogisticController : ControllerBase
    {
        ILogging _logging;
        private readonly ILogisticManager _logisticManager;
        private readonly IServicePartnerManager _servicePartnerManager;
        public LogisticController( ILogging logging, ILogisticManager logisticManager, IServicePartnerManager servicePartnerManager)
        {
            _logisticManager = logisticManager;
            _logging = logging;
            _servicePartnerManager = servicePartnerManager;
        }

        
        [HttpGet]
        [Route("Test")]
        public ExecutionResult GetTest()
        {

            ExecutionResult executionResult = new ExecutionResult();

            InfoMessage structObj = new InfoMessage(true, "API Working..", null);

            executionResult.StatusCode = HttpStatusCode.Accepted;
            executionResult.Details.Add(structObj);

            return executionResult;
        }


        /// <summary>
        /// Api Used To Register LGC/Service Partner using mobile app
        /// created by ashwin
        /// </summary>
        /// <param name="data"></param>
        /// <returns>executionResult</returns>
        [HttpPost]
        [Route("LGCRegisteration")]
        public ResponseResult LGCRegisteration(RegisterServicePartnerDataModel data)
        {

            ExecutionResult executionResult = new ExecutionResult();
            //EVCResellerRegisterationModel registerationModel = null;
            //ErrorMessage errorMessage = new ErrorMessage();
            ResponseResult responseMessage = null;
            try
            {
                if (data != null)
                {
                    responseMessage = new ResponseResult();
                    responseMessage = _servicePartnerManager.LGCRegister(data);
                    if (responseMessage.Status == true)
                    {
                        responseMessage.Status_Code = HttpStatusCode.OK;
                        return responseMessage;
                        
                    }
                    else
                    {
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        return responseMessage;
                    }
                }
                else
                {
                    responseMessage = new ResponseResult();
                    responseMessage.message = " Request Object should not be null ";
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
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

    }
}
