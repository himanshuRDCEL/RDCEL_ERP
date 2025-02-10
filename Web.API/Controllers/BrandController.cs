using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.Company;
using RDCELERP.Model.InfoMessage;

namespace Web.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IBrandManager _brandManager;
        
        public BrandController(IBrandRepository brandRepository, IBrandManager brandManager)
        {
            _brandRepository = brandRepository;
            _brandManager = brandManager;
        }

        [HttpGet]
        public ExecutionResult GetAllBrandList()
        {
            return _brandManager.GetBrand();
        }

        //[HttpGet("{id}")]
        //public ExecutionResult GetBrandById(int id)
        //{
        //    return _brandManager.BrandById(id);
        //}

    }
}
