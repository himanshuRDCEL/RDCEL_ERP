using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.InfoMessage;

namespace Web.API.Controllers
{
    namespace Web.API.Controllers
    {
        [Authorize]
        [Route("api/[controller]")]
        [ApiController]
        public class BusinessPartnerController : ControllerBase
        {
            private readonly IBusinessPartnerRepository _businessPartnerRepository;
            private readonly IBusinessPartnerManager _businessPartnerManager;


            public BusinessPartnerController(IBusinessPartnerRepository businessPartnerRepository, IBusinessPartnerManager businessPartnerManager)
            {
                _businessPartnerRepository = businessPartnerRepository;
                _businessPartnerManager = businessPartnerManager;

            }

            [HttpGet]
            public ExecutionResult GetAllBusinessPartnerList()
            {
                return _businessPartnerManager.GetBusinessPartner();
            }

            [HttpGet("{id}")]
            public ExecutionResult BusinesspartnerById(int id)
            {
                return _businessPartnerManager.BusinessPartnerById(id);
            }

        }
    }
}