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
using RDCELERP.Model.State;

namespace Web.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IStateRepository _stateRepository;
        private readonly IStateManager _stateManager;


        public StateController(IStateRepository stateRepository, IStateManager stateManager)
        {
            _stateManager = stateManager;
            _stateRepository = stateRepository;

        }

        [HttpGet]
        public ExecutionResult GetAllStateList()
        {
            ExecutionResult executionResult = new ExecutionResult();
           //return _stateManager.GetState();
            return executionResult;
        }

        [HttpGet("{id}")]
        public ExecutionResult GetStateById(int id)
        {
            return _stateManager.StateById(id);
        }
    }
}
