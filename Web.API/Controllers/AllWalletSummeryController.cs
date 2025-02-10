using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.InfoMessage;

namespace UTC.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AllWalletSummeryController : ControllerBase
    {
        private readonly IEVCRepository _evcRepository;
        private readonly IEVCManager _evcManager;

        public AllWalletSummeryController(IEVCRepository evcRepository, IEVCManager evcManager)
        {
            _evcRepository = evcRepository;
            _evcManager = evcManager;

        }
        [HttpGet]
        public ExecutionResult AllWalletSummery()
        {
            return _evcManager.GetAllWalletSummery();
        }
    }
}
