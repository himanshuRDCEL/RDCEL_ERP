using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.EVC;

namespace RDCELERP.CoreWebApi.Controllers.Mobile
{
    
    [Route("api/Mobile/[controller]")]
    [ApiController]
    public class EVCController : ControllerBase
    {
        private readonly IEVCManager _evcManager;
        private readonly IEVCRepository _evcRepository;
        ILogging _logging;
        public EVCController(IEVCRepository evcRepository, IEVCManager evcManager, ILogging logging)
        {
            _evcRepository = evcRepository;
            _evcManager = evcManager;
            _logging = logging;
        }

        [HttpGet]
        [Route("Test")]
        [Authorize]
        public ExecutionResult GetTest()
        {

            ExecutionResult executionResult = new ExecutionResult();

            InfoMessage structObj = new InfoMessage(true, "API Working..", null);

            executionResult.StatusCode = HttpStatusCode.Accepted;
            executionResult.Details.Add(structObj);

            return executionResult;
        }

        [HttpGet("{id}")]
        [Route("GetEvcById")]
        public ExecutionResult GetEvcById(int id)
        {
            EVC_RegistrationModel eVC_RegistrationModels = null;
            ExecutionResult executionResult = new ExecutionResult();

            try
            {

                eVC_RegistrationModels = _evcManager.GetEvcByUserId(id);
                InfoMessage structObj = new InfoMessage(true, "Success", eVC_RegistrationModels);

                executionResult.StatusCode = HttpStatusCode.OK;

                executionResult.Details.Add(structObj);

            }
            catch (Exception ex)
            {

            }





            return executionResult;



        }
        /// <summary>
        /// Api Used To Register EVC/Reseller using mobile app
        /// created by ashwin
        /// </summary>
        /// <param name="data"></param>
        /// <returns>executionResult</returns>
        [HttpPost]
        [Route("EVCRegisteration")]
        public ExecutionResult EVCRegisteration(EVC_RegistrationModel data)
        {
            
            ExecutionResult executionResult = new ExecutionResult();
            
            ResponseResult responseMessage = new ResponseResult();
            try
            {
                if(data != null)
                {
                    responseMessage = _evcManager.RegisterEVC(data);
                    if (responseMessage.Status == true)
                    {
                        
                        InfoMessage structObj = new InfoMessage(true, "Success", responseMessage.message);
                        
                        executionResult.StatusCode = HttpStatusCode.OK;

                        executionResult.Details.Add(structObj);
                    }
                    else
                    {
                        
                        
                        InfoMessage structObj = new InfoMessage(false, "Not Success", responseMessage.message);

                        executionResult.StatusCode = HttpStatusCode.BadRequest;

                        executionResult.Details.Add(structObj);
                    }
                }
                else
                {
                    responseMessage.message = "Object Not Found";
                    InfoMessage structObj = new InfoMessage(false, "Not Success", responseMessage.message);

                    executionResult.StatusCode = HttpStatusCode.BadRequest;

                    executionResult.Details.Add(structObj);
                }

            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                InfoMessage structObj = new InfoMessage(false, "Internal server error", responseMessage.message);

                executionResult.StatusCode = HttpStatusCode.InternalServerError;

                executionResult.Details.Add(structObj);
            }
            return executionResult;
        }

        public class Error
        {
            public Error(string key, string message)
            {
                Key = key;
                Message = message;
            }

            public string Key { get; set; }
            public string Message { get; set; }
        }

    }
}
