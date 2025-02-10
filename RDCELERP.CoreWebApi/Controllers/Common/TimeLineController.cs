using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using RDCELERP.Common.Helper;
using RDCELERP.Model.Base;
using RDCELERP.Model.MobileApplicationModel;

namespace RDCELERP.CoreWebApi.Controllers.Common
{
    [Route("api/Common/[controller]")]
    [ApiController]
    public class TimeLineController : ControllerBase
    {
        IOptions<ApplicationSettings> _config;
        ILogging _logging;
        public TimeLineController(IOptions<ApplicationSettings> config, ILogging logging)
        {
            _config = config;
            _logging = logging;
        }

        #region Test TimeLine API
        [HttpGet]
        [Route("Test")]
        public ResponseResult GetTest()
        {

            ResponseResult responseResult = new ResponseResult();
            try
            {
                responseResult.message = "API Working..";
                responseResult.Status = true;
                responseResult.Status_Code = HttpStatusCode.Accepted;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("TimeLineController", "GetTest", ex);
            }
            return responseResult;
        }
        #endregion

        #region API to Get TimeLine by RegdNo
        [HttpGet]
        [Route("GetTimeLineByRegdNo")]
        public IActionResult GetTimeLineByRegdNo(string regdNo)
        {
            string baseUrl = _config.Value.ERPBaseURL;
            return Redirect(baseUrl + "TimeLineIndependent?regdNo=" + regdNo);
        }
        #endregion
    }
}
