using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
using System.Net.Http;
using RDCELERP.Model.Base;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace UTC.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSlotController : ControllerBase
    {

        public TimeSlotController()
        {

        }

        [HttpGet]
        [Route("Test")]
        public ExecutionResult GetTest()
        {

            ExecutionResult executionResult = new ExecutionResult();

            InfoMessage structObj = new InfoMessage(true, "API Working..", null);

            executionResult.StatusCode = HttpStatusCode.Accepted;
            executionResult.Details.Add(structObj);

            return executionResult;
        }


    }
}
