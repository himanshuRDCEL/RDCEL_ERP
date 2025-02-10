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
using RDCELERP.Model.PinCode;

namespace RDCELERP.CoreWebApi.Controllers.Common
{
    //[Authorize]
    [Route("api/Common/[controller]")]
    [ApiController]
    public class PinCodeController : ControllerBase
    {
        #region Variables
        private readonly IPinCodeRepository _pinCodeRepository;
        private readonly IPinCodeManager _pinCodeManager;
        ILogging _logging;
        #endregion

        #region Constructor
        public PinCodeController(IPinCodeRepository pinCodeRepository, IPinCodeManager pinCodeManager, ILogging logging)
        {
            _pinCodeRepository = pinCodeRepository;
            _pinCodeManager = pinCodeManager;
            _logging = logging;
        }
        #endregion

        #region Test PinCodeController Api
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
                _logging.WriteErrorToDB("PinCodeController", "GetTest", ex);
            }
            return responseResult;
        }
        #endregion

        #region GetAllPinCodeList
        /// <summary>
        /// old Api
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ExecutionResult GetAllPinCodeList()
        {
            return _pinCodeManager.GetPinCode();
        }
        #endregion

        #region GetPinCodeById 
        [HttpGet("{id}")]
        public ExecutionResult GetPinCodeById(int id)
        {
            return _pinCodeManager.PinCodeById(id);
        }
        #endregion

        #region  Api for GetPinCodes By List of City Id's
        /// <summary>
        ///  Get List of Pincodes by list of City Id's
        /// </summary>
        /// <param name="Cityid"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PinCodesByCitites")]
        public ResponseResult GetPinCodesByCitites(CitiesID CityIdlist)
        {
            ResponseResult responseResult = new ResponseResult();
            try
            {
                if (CityIdlist != null && CityIdlist.cityIdlists!=null)
                {
                    
                    responseResult = _pinCodeManager.PinCodesByCities(CityIdlist);
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
                            responseResult.message = "no data found";
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
                _logging.WriteErrorToDB("PinCodeController", "GetPinCodesByCitites", ex);
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
            }
            return responseResult;
        }
        #endregion


        #region  Api for GetPinCodes for Service Partner Registration By City Id's & Service Partner Id
        /// <summary>
        ///  Get List of Pincodes by City Id's & Service Partner Id
        /// </summary>
        /// <param name="Cityid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPinCodesByServicePartner")]
        public ResponseResult GetPinCodesByServicePartner(int cityId, int ServicePartnerId)
        {
            ResponseResult responseResult = new ResponseResult();
            try
            {
                if (cityId > 0 && ServicePartnerId > 0)
                {

                    responseResult = _pinCodeManager.PinCodesByServicePartner(cityId, ServicePartnerId);
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
                            responseResult.message = "no data found";
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
                _logging.WriteErrorToDB("PinCodeController", "GetPinCodesByCitites", ex);
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
            }
            return responseResult;
        }
        #endregion
    }
}
