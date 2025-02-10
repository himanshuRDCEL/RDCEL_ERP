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
using RDCELERP.Model.Product;
using RDCELERP.Model.Users;

namespace Web.API.Controllers
{
   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserManager _userManager;
        public readonly IOptions<ApplicationSettings> _baseConfig;


        public LoginController(IUserRepository userRepository, IUserManager userManager, IOptions<ApplicationSettings> baseConfig)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _baseConfig = baseConfig;

        }

        TblUser TblUser = new TblUser();

        [HttpPost]
        public ExecutionResult GetUserByLoginPost(string username, string password)
        {
            string pwd = SecurityHelper.EncryptString(password, _baseConfig.Value.SecurityKey);
            string email = SecurityHelper.EncryptString(username, _baseConfig.Value.SecurityKey);
            return _userManager.UserByLogin(email, pwd);
        }
    }
}

   

