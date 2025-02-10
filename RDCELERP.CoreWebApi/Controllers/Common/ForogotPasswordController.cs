using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.Users;

namespace RDCELERP.CoreWebApi.Controllers.Common
{
    [Route("api/Common/[controller]")]
    [ApiController]
    public class ForogotPasswordController : ControllerBase
    {
        #region Variables Declaration
        private readonly IUserRepository _userRepository;
        private readonly IUserManager _userManager;
        public readonly IOptions<ApplicationSettings> _baseConfig;
        ILogging _logging;
        #endregion

        #region Constructor
        public ForogotPasswordController(ILogging logging,IUserRepository userRepository, IUserManager userManager, IOptions<ApplicationSettings> baseConfig)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _baseConfig = baseConfig;
            _logging = logging;
        }
        #endregion

        #region Test ForgotPasswordController Api's
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
                _logging.WriteErrorToDB("ForogotPasswordController", "GetTest", ex);
            }
            return responseResult;
        }
        #endregion

        #region UpdateUserPassword Api
        [HttpPost]
        public ResponseResult UpdateUserPassword(string email)
        {
            ResponseResult result = new ResponseResult();
            string pwd = string.Empty;
            string encpwd = string.Empty;
            string encpwd1 = string.Empty;
            try
            {
                if (email != string.Empty || email != null)
                {
                    pwd = StringHelper.RandomStrByLength(6);
                    encpwd = SecurityHelper.EncryptString(pwd, _baseConfig.Value.SecurityKey);
                    encpwd1 = SecurityHelper.EncryptString(email.ToString(), _baseConfig.Value.SecurityKey);
                    
                    if (pwd!=string.Empty && encpwd != string.Empty && encpwd1!=string.Empty)
                    {
                        result = _userManager.UpdatePassword(encpwd1, encpwd, pwd);
                    }
                    else
                    {
                        result.message = "Encryting string failed";
                        result.Status = false;
                        result.Status_Code = HttpStatusCode.BadRequest;                       
                    }
                }
                else
                {
                    result.Status = false;
                    result.Status_Code = HttpStatusCode.BadRequest;
                    result.message = "Not Success,email should not be empty";
                }
                return result;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ForogotPasswordController", "UpdateUserPassword", ex);
                result.Status = false;
                result.Status_Code = HttpStatusCode.InternalServerError;
                result.message = ex.Message;
                return result;
            }
        }
        #endregion
    }
}