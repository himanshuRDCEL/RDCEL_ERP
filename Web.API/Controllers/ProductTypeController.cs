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
using RDCELERP.Model.Product;

namespace Web.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IProductTypeManager _productTypeManager;
        

        public ProductController(IProductTypeRepository productTypeRepository, IProductTypeManager productTypeManager)
        {
            _productTypeRepository = productTypeRepository;
            _productTypeManager = productTypeManager;
            
        }

        [HttpGet]
        public ExecutionResult GetAllProductTypeList()
        {
            return _productTypeManager.GetProductType();
        }

        [HttpGet("{id}")]
        public ExecutionResult GetProductTypeById(int id)
        {
            return _productTypeManager.ProductTypeById(id);
        }

       
    }
}



