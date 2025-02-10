using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.City;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.State;

namespace RDCELERP.CoreWebApi.Controllers.Common
{
    
    [Route("api/Common/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        #region Variable
        private readonly ICityRepository _cityRepository;
        private readonly ICityManager _cityManager;
        ILogging _logging;
        #endregion

        #region Constructor
        public CityController(ICityRepository cityRepository, ICityManager cityManager, ILogging logging)
        {
            _cityRepository = cityRepository;
            _cityManager = cityManager;
            _logging = logging;
        }
        #endregion

        #region Test CityController Api
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
                _logging.WriteErrorToDB("CityController", "GetTest", ex);
            }
            return responseResult;
        }
        #endregion

        #region List of All City
        [HttpGet]
        [Route("GetAllCityList")]
        public ResponseResult GetAllCityList()
        {
            ResponseResult responseResult = null;
            
            try
            {
                responseResult = _cityManager.GetCity();
                if (responseResult != null && responseResult.Status==true)
                {
                    //responseResult.Status_Code = HttpStatusCode.OK;
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
                        responseResult = new ResponseResult();
                        responseResult.message = "Not Valid Request ";
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                    
                }
            }
            catch(Exception ex)
            {
                _logging.WriteErrorToDB("CityController", "GetAllCityList", ex);
                responseResult = new ResponseResult();
                responseResult.Status_Code = HttpStatusCode.InternalServerError;    
            }
            return responseResult;
        }
        #endregion

        #region GetCityByID 
        [HttpGet]
        [Route("GetCityByID")]
        public ExecutionResult GetCityById(int id)
        {
            ExecutionResult executionResult = null;
            try
            {
                executionResult = _cityManager.CityById(id);
                if(executionResult != null)
                {
                    executionResult.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    executionResult = new ExecutionResult();
                    executionResult.StatusCode = HttpStatusCode.BadRequest;
                }
            }
            catch(Exception ex)
            {
                _logging.WriteErrorToDB("CityController", "GetCityById", ex);
                executionResult = new ExecutionResult();
                executionResult.StatusCode = HttpStatusCode.InternalServerError;
            }
            return executionResult;
        }
        #endregion

        #region GetCitiesByStateID
        [HttpGet]
        [Route("GetCitiesByStateID")]
        public ResponseResult GetCitiesByStateID(int id)
        {
            ResponseResult responseResult = new ResponseResult();
            ExecutionResult executionResult = null;
            try
            {
                responseResult = _cityManager.CityByStateId(id);
                if (executionResult != null)
                {
                    executionResult.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    executionResult = new ExecutionResult();
                    executionResult.StatusCode = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CityController", "GetCitiesByStateID",ex);
                executionResult = new ExecutionResult();
                
                executionResult.StatusCode = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }
        #endregion

        #region Api for GetCities By List of State Id's
        [HttpPost]
        [Route("GetCitiesByStateList")]
        public ResponseResult GetCitiesByStateList(StateList stateList)
        {
            ResponseResult responseResult = new ResponseResult();
            try
            {
                if (ModelState.IsValid && stateList.stateIdLists.Count>0)
                {
                   
                    responseResult = _cityManager.CityByStateLists(stateList);
                    if (responseResult != null && responseResult.Status == true)
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
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            responseResult.message = "No data found";
                        }
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    responseResult.message = "failed,Request parameter should not be null or not contains zero";
                }
                
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CityController", "GetCitiesByStateList", ex);
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }

        #endregion

        #region GetCities by LgcId 
        [Authorize]
        [HttpGet]
        [Route("GetCitiesbyLgcId")]
        public ResponseResult GetCitiesbyLgcId(int lgcId)
        {
            ResponseResult responseResult = new ResponseResult();
            string username = string.Empty;
            try
            {
                if (lgcId > 0)
                {
                    responseResult = _cityManager.GetCitiesbyLgcId(lgcId);
                    if (responseResult.Status == false && responseResult.message == string.Empty)
                    {
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        responseResult.message = "Invalid Request";
                        return responseResult;
                    }
                    else
                    {
                        return responseResult;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    responseResult.message = "Invalid Parameter";
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CityController", "GetCitiesbyLgcId", ex);
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
            }
            return responseResult;
        }
        #endregion
    }


}
