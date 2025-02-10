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
using RDCELERP.Model.ProductQuality;

namespace Web.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductQualityIndexController : ControllerBase
    {
        private readonly IProductQualityIndexRepository _productQualityIndexRepository;
        private readonly IProductQualityIndexManager _productQualityIndexManager;


        public ProductQualityIndexController(IProductQualityIndexRepository productQualityIndexRepository, IProductQualityIndexManager productQualityIndexManager)
        {
            _productQualityIndexRepository = productQualityIndexRepository;
            _productQualityIndexManager = productQualityIndexManager;

        }

        [HttpGet]
        public ExecutionResult GetAllProductQualityIndexList()
        {
            return _productQualityIndexManager.GetProductQualityIndex();
        }

        [HttpGet("{id}")]
        public ExecutionResult GetProductQualityIndexId(int id)
        {
            return _productQualityIndexManager.ProductQualityIndexById(id);
        }
    }
}
