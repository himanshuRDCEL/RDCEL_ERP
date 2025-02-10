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
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.Users;

namespace Web.API
{
    [Authorize]

    [Route("api/[controller]")]
    [ApiController]
    public class ForogotPasswordController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserManager _userManager;
        public readonly IOptions<ApplicationSettings> _baseConfig;


        public ForogotPasswordController(IUserRepository userRepository, IUserManager userManager, IOptions<ApplicationSettings> baseConfig)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _baseConfig = baseConfig;

        }

        [HttpPost]
        public ExecutionResult UpdateUserPassword(string email)
        {

           string pwd = StringHelper.RandomStrByLength(6);
            string encpwd = SecurityHelper.EncryptString(pwd, _baseConfig.Value.SecurityKey);
            string encpwd1 = SecurityHelper.EncryptString(email, _baseConfig.Value.SecurityKey);

            return _userManager.UpdateUserPassword(encpwd1, encpwd, pwd);
        }

    }
}

                



