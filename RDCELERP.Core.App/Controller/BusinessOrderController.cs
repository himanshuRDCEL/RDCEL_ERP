using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RDCELERP.BAL.Interface;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.BusinessCustomer;

namespace RDCELERP.Core.App.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class BusinessOrderController : ControllerBase
    {
        private readonly IItemBookingManager _bookingItemManager;


        public BusinessOrderController(IItemBookingManager bookingItemManager)
        { 
            _bookingItemManager = bookingItemManager;
        }
        [HttpPost]
        public IActionResult ManageOrderStatus(UpdateBookingItemViewModel bookingitemVM)
        {
            int result = 0;
            if (bookingitemVM.SyncOrderNo != null)
            {

                result = _bookingItemManager.ManageOrderStatus(bookingitemVM);

            }
            else
            {
                return Ok(new { message = "Please Provide Order No.", success = false });

            }
            if (result > 0)
            {
                return Ok(new { message = "Order Status Update successfully", success = true });
            }
            if (result==-1)
            {
                return Ok(new { message = "Incorrect Order No.", success = true });
            }
            else
            {
                return BadRequest(new { message = "Failed to Update", success = false });
            }

        }


    }
}
