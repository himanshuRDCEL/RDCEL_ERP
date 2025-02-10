using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.City;
using RDCELERP.Model.InfoMessage;

namespace Web.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityRepository _cityRepository;
        private readonly ICityManager _cityManager;


        public CityController(ICityRepository cityRepository, ICityManager cityManager)
        {
            _cityRepository = cityRepository;
            _cityManager = cityManager;

        }

        [HttpGet]
        public ExecutionResult GetAllCityList()
        {
            ExecutionResult executionResult = new ExecutionResult();
            return executionResult;
        }

        [HttpGet("{id}")]
        public ExecutionResult GetCityById(int id)
        {
            return _cityManager.CityById(id);
        }
    }
}
