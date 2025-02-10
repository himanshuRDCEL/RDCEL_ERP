using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.State;

namespace RDCELERP.CoreWebApi.Controllers.Common
{
    
    [Route("api/Common/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {

        #region Variables
        private readonly IStateRepository _stateRepository;
        private readonly IStateManager _stateManager;
        ILogging _logging;
        #endregion

        #region Constructor
        public StateController(ILogging logging,IStateRepository stateRepository, IStateManager stateManager)
        {
            _stateManager = stateManager;
            _stateRepository = stateRepository;
            _logging = logging;
        }
        #endregion

        #region Test StateController Api
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
                _logging.WriteErrorToDB("StateController", "GetTest", ex);
            }
            return responseResult;
        }
        #endregion

        #region GetAllState List
        [HttpGet]
        [Route("GetAllState")]
        public ResponseResult GetAllStateList()
        {
            ResponseResult responseResult = null;
            ExecutionResult executionResult = null;
            try
            {
                responseResult = _stateManager.GetState();
                if (responseResult != null && responseResult.Status==true)
                {
                    return responseResult;
                }
                else
                {
                    if (responseResult.Status == false)
                    {
                        return responseResult;
                    }
                    else
                    {
                        responseResult.Status = false;
                        responseResult.message = "Not Valid Request";
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
            }
            catch(Exception ex)
            {
                executionResult = new ExecutionResult();
                executionResult.StatusCode = HttpStatusCode.BadRequest;
            }
            return responseResult;
        }
        #endregion

        #region GetStateById 
        [HttpGet("{id}")]
        [Route("GetStateById")]
        public ExecutionResult GetStateById(int id)
        {
            return _stateManager.StateById(id);
        }
        #endregion
    }
}
