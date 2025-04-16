using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RDCELERP.BAL.Interface;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.BusinessCustomer;
using RDCELERP.Model.Zoho;
using AutoMapper;
using ZXing;

namespace RDCELERP.Core.App.Controller
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class BusinessCustomerController : ControllerBase
    {
        private readonly IBusinessCustomerManager _businessCustomerManager;
        private readonly IBussinessCustomerRepository _businessCustomerRepository;


        public BusinessCustomerController(IBusinessCustomerManager businessCustomerManager, IBussinessCustomerRepository businessCustomerRepository)
        {
            _businessCustomerManager = businessCustomerManager;
            _businessCustomerRepository = businessCustomerRepository;
        }

        [HttpPost]
        public IActionResult ManageBusinessCustomer(BusinessCustomerViewModel busniessCustomerVM)
        {
            int result = 0;
            if (busniessCustomerVM.PhoneNo!=null)
            {

                 result = _businessCustomerManager.ManageBusinessCustomer(busniessCustomerVM);
            
            }
            if (result > 0)
            {
                return Ok(new { message = "Created successfully", success = true });
            }
            else
            {
                return BadRequest(new { message = "Failed to create", success = false });
            }

        }

    }
}
