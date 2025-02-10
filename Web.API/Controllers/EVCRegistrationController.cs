using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.EVC;
using RDCELERP.Model.InfoMessage;

namespace Web.API.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EVCRegistrationController : ControllerBase
    {
        private readonly IEVCRepository _evcRepository;
        private readonly IEVCManager _evcManager;


        public EVCRegistrationController(IEVCRepository evcRepository, IEVCManager evcManager)
        {
            _evcRepository = evcRepository;
            _evcManager = evcManager;

        }

        [HttpGet("{id}")]
        public ExecutionResult EvcByEvcregistrationId(int id)
        {
            
            return _evcManager.EvcByEvcregistrationId(id);
        }
    }
}
    

