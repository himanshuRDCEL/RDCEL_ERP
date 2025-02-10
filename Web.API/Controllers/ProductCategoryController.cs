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
using RDCELERP.Model.Company;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.Master;

namespace Web.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {

        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductCategoryManager _productCategoryManager;


        public ProductCategoryController(IProductCategoryRepository productCategoryRepository, IProductCategoryManager productCategoryManager)
        {
            _productCategoryRepository = productCategoryRepository;
            _productCategoryManager = productCategoryManager;

        }

        [HttpGet]
        public ExecutionResult GetAllProductCategoryList()
        {
            return _productCategoryManager.GetProductCategory();
        }

        [HttpGet("{id}")]
        public ExecutionResult GetProductCategoryById(int id)
        {
            return _productCategoryManager.ProductCategoryById(id);
        }
    }

}

