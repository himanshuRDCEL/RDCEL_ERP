using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.InfoMessage;

namespace Web.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChangePasswordController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserManager _userManager;
        public readonly IOptions<ApplicationSettings> _baseConfig;

        public ChangePasswordController(IUserRepository userRepository, IUserManager userManager, IOptions<ApplicationSettings> baseConfig)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _baseConfig = baseConfig;

        }

        [HttpPost]
        public ExecutionResult UpdateUserChangePasswordPost(int userid, string oldPassword, string NewPassword, string ConfirmPassword)
        {
            //string pwd = SecurityHelper.EncryptString(oldPassword, _baseConfig.Value.SecurityKey);
            return _userManager.ChangePassword(userid, oldPassword, NewPassword, ConfirmPassword);
            

        }
    }
}
