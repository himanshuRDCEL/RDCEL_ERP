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
using RDCELERP.Model.PinCode;

namespace Web.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PinCodeController : ControllerBase
    {
        private readonly IPinCodeRepository _pinCodeRepository;
        private readonly IPinCodeManager _pinCodeManager;


        public PinCodeController(IPinCodeRepository pinCodeRepository, IPinCodeManager pinCodeManager)
        {
            _pinCodeRepository = pinCodeRepository;
            _pinCodeManager = pinCodeManager;

        }

        [HttpGet]
        public ExecutionResult GetAllPinCodeList()
        {
            return _pinCodeManager.GetPinCode();
        }

        [HttpGet("{id}")]
        public ExecutionResult GetPinCodeById(int id)
        {
            return _pinCodeManager.PinCodeById(id);
        }
    }
}
