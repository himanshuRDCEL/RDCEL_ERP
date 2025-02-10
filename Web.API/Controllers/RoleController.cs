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

namespace UTC.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleBaisedLoginController : ControllerBase
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserManager _userManager;
        public readonly IOptions<ApplicationSettings> _baseConfig;

        public RoleBaisedLoginController(IUserRoleRepository userRoleRepository, IUserManager userManager, IOptions<ApplicationSettings> baseConfig)
        {
            _userRoleRepository = userRoleRepository;
            _userManager = userManager;
            _baseConfig = baseConfig;
        }

        [HttpPost]
        public ExecutionResult PostRoleBaisedLogin(string username, string password, int roleId, int userId)
        {
            string email = SecurityHelper.EncryptString(username, _baseConfig.Value.SecurityKey);
            string pwd = SecurityHelper.EncryptString(password, _baseConfig.Value.SecurityKey);
            return _userManager.RoleByLogin(email, pwd, roleId, userId);

        }
    }
}
