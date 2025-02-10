using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.LGC;
using static RDCELERP.Common.Helper.MessageHelper;
using RDCELERP.Model.Users;

namespace RDCELERP.CoreWebApi.Controllers.Mobile
{
    [Route("api/Mobile/[controller]")]
    [ApiController]
    public class DriverController : Controller
    {
        #region Variables
        ILogging _logging;
        private readonly ILogin_MobileRepository _login_MobileRepository;
        private readonly IServicePartnerManager _servicePartnerManager;
        private readonly IDriverDetailsManager _driverDetailsManager;
        private readonly IUserRepository _userRepository;
        private readonly IServicePartnerRepository _servicePartnerRepository;
        private CustomDataProtection _protector;
        IOptions<ApplicationSettings> _config;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IDriverDetailsRepository _driverDetailsRepository;
        private readonly IVehicleJourneyTrackingDetailsRepository _journeyTrackingDetailsRepository;
        private readonly IVehicleJourneyTrackingRepository _journeyTrackingRepository;
        private readonly IOrderTransRepository _orderTransRepository;
        private readonly INotificationManager _notificationManager;
        private readonly IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _WhatsAppMessageRepository;
        IEVCRepository _eVCRepository;
        ILogisticManager _logisticManager;
        IPushNotificationManager _pushNotificationManager;


        #endregion

        #region Constructor
        public DriverController(ILogging logging, IServicePartnerManager servicePartnerManager, ILogin_MobileRepository login_MobileRepository, IDriverDetailsManager driverDetailsManager, IUserRepository userRepository, IServicePartnerRepository servicePartnerRepository, CustomDataProtection protector, IOptions<ApplicationSettings> config, IUserRoleRepository userRoleRepository, IRoleRepository roleRepository, IDriverDetailsRepository driverDetailsRepository, IVehicleJourneyTrackingDetailsRepository journeyTrackingDetailsRepository, IVehicleJourneyTrackingRepository journeyTrackingRepository, IOrderTransRepository orderTransRepository, Digi2l_DevContext digi2L_DevContext, INotificationManager notificationManager, IWhatsappNotificationManager whatsappNotificationManager, IWhatsAppMessageRepository whatsAppMessageRepository, IEVCRepository eVCRepository, ILogisticManager logisticManager, IPushNotificationManager pushNotificationManager)
        {
            _logging = logging;
            _servicePartnerManager = servicePartnerManager;
            _login_MobileRepository = login_MobileRepository;
            _driverDetailsManager = driverDetailsManager;
            _userRepository = userRepository;
            _servicePartnerRepository = servicePartnerRepository;
            _protector = protector;
            _config = config;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _driverDetailsRepository = driverDetailsRepository;
            _journeyTrackingDetailsRepository = journeyTrackingDetailsRepository;
            _journeyTrackingRepository = journeyTrackingRepository;
            _orderTransRepository = orderTransRepository;
            _notificationManager = notificationManager;
            _whatsappNotificationManager = whatsappNotificationManager;
            _WhatsAppMessageRepository = whatsAppMessageRepository;
            _eVCRepository = eVCRepository;
            _logisticManager = logisticManager;
            _pushNotificationManager = pushNotificationManager;
        }
        #endregion

        #region Test DriverController Api
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
                _logging.WriteErrorToDB("DriverController", "GetTest", ex);
            }
            return responseResult;
        }
        #endregion

        #region Api for Driver Registeration
        /// <summary>
        /// Api Used To Register Vehicle using mobile app
        /// created by priyanshi
        /// </summary>
        /// <param name="data"></param>
        /// <returns>executionResult</returns>
        /// []
        [Authorize]
        [HttpPost]
        [Route("RegisterDriver")]
        public ResponseResult RegisterDriver([FromForm] DriverDataModel data)
        {
            ResponseResult responseMessage = new ResponseResult();
            string username = string.Empty;
            string userRole = string.Empty;
            DAL.Entities.TblUser tblUser = null;

            try
            {
                if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;

                    //userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.LGC_Admin).ToString();
                    userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();

                    TblServicePartner tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerIsApprovrd == true && x.ServicePartnerEmailId == username.ToString() || x.ServicePartnerMobileNumber == username);

                    if (tblServicePartner != null && tblServicePartner.ServicePartnerEmailId != null)
                    {

                        var Email = SecurityHelper.EncryptString(tblServicePartner.ServicePartnerEmailId, _config.Value.SecurityKey);
                        tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.Email == Email);
                        if (tblUser != null)
                        {
                            TblUserRole tblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == tblUser.UserId);
                            if (tblUserRole != null)
                            {
                                TblRole tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == tblUserRole.RoleId);
                                if (tblRole != null && tblRole.RoleName == userRole)
                                {
                                    if (data != null)
                                    {
                                        //Manager call
                                        //int? userId =  tblLoginMobile.UserId;
                                        responseMessage = _servicePartnerManager.AddDriver(data);
                                        if (responseMessage.Status == false && responseMessage.message == string.Empty)
                                        {
                                            responseMessage.message = "Registration Failed";
                                            responseMessage.Status = false;
                                            responseMessage.Status_Code = HttpStatusCode.OK;
                                            return responseMessage;
                                        }
                                        else
                                        {
                                            return responseMessage;
                                        }
                                    }
                                    else
                                    {
                                        responseMessage = new ResponseResult();
                                        responseMessage.message = " Request object should not be null or greater than five";
                                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                                        return responseMessage;
                                    }
                                }
                                else
                                {
                                    responseMessage.message = "Not A Valid User";
                                    responseMessage.Status = false;
                                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                                }

                            }
                            else
                            {
                                responseMessage.message = "Not A Valid User";
                                responseMessage.Status = false;
                                responseMessage.Status_Code = HttpStatusCode.BadRequest;
                            }


                        }

                        else
                        {
                            responseMessage.message = "Not A Valid User";
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseMessage.message = "Service partner not a valid user";
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseMessage.message = "Not A Valid Request";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                responseMessage = new ResponseResult();
                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #region Api for Update Driver
        /// <summary>
        /// Api Used To Update Driver
        /// created by priyanshi
        /// </summary>
        /// <param name="data"></param>
        /// <returns>executionResult</returns>
        [Authorize]
        [HttpPost]
        [Route("UpdateDriver")]
        public ResponseResult UpdateDriver([FromForm] UpdateDriverDataModel data)
        {
            ResponseResult responseMessage = new ResponseResult();
            string username = string.Empty;
            string userRole = string.Empty;
            DAL.Entities.TblUser tblUser = null;
            try
            {
                if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;

                    //userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.LGC_Admin).ToString();
                    userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();

                    TblServicePartner tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerIsApprovrd == true && x.ServicePartnerEmailId == username.ToString() || x.ServicePartnerMobileNumber == username);

                    if (tblServicePartner != null && tblServicePartner.ServicePartnerEmailId != null)
                    {

                        var Email = SecurityHelper.EncryptString(tblServicePartner.ServicePartnerEmailId, _config.Value.SecurityKey);
                        tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.Email == Email);
                        if (tblUser != null)
                        {
                            TblUserRole tblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == tblUser.UserId);
                            if (tblUserRole != null)
                            {
                                TblRole tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == tblUserRole.RoleId);
                                if (tblRole != null && tblRole.RoleName == userRole)
                                {
                                    if (data != null)
                                    {
                                        //Manager call
                                     
                                        responseMessage = _servicePartnerManager.UpdateDriver(data);
                                        if (responseMessage.Status == true)
                                        {
                                            responseMessage.Status_Code = HttpStatusCode.OK;
                                            return responseMessage;
                                        }
                                        else
                                        {
                                            if (responseMessage.Status == false)
                                            {
                                                return responseMessage;
                                            }
                                            else
                                            {
                                                responseMessage.Status_Code = HttpStatusCode.BadRequest;
                                                responseMessage.Status = false;
                                                responseMessage.message = "Not valid Request";
                                                return responseMessage;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        responseMessage = new ResponseResult();
                                        responseMessage.message = " Request object should not be null or greater than five";
                                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                                        return responseMessage;
                                    }
                                }
                                else
                                {
                                    responseMessage.message = "Not A Valid User";
                                    responseMessage.Status = false;
                                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                                }

                            }
                            else
                            {
                                responseMessage.message = "Not A Valid User";
                                responseMessage.Status = false;
                                responseMessage.Status_Code = HttpStatusCode.BadRequest;
                            }


                        }

                        else
                        {
                            responseMessage.message = "Not A Valid User";
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseMessage.message = "Service partner not a valid user";
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseMessage.message = "Not A Valid Request";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                }

            }
            catch (Exception ex)
            {
                responseMessage = new ResponseResult();
                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #region Api for GetDriverList by servicepartnerId 
        /// <summary>
        /// Api for VehicleList  by servicepartnerId
        /// take Details from tblservicepartner 
        /// added by Kranti
        /// </summary>
        /// <param name="date,servicepartnerId"></param>
        /// <returns>responseMessage</returns>

        [HttpGet]
        [Route("GetDriverList")]
        public ResponseResult GetDriverList(int servicepartnerId, string? searchTerm = "", int? pageNumber = 1, int? pageSize = 10)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            string username = string.Empty;
            string userRole = string.Empty;

            try
            {
                if (servicepartnerId > 0)
                {
                    if (pageNumber == 0 || pageSize == 0)
                    {
                        pageNumber = 1;
                        pageSize = 10;
                    }

                    responseMessage = _driverDetailsManager.GetDriverList(servicepartnerId, searchTerm, pageNumber, pageSize);

                    if (responseMessage.Status == false && responseMessage.message == string.Empty)
                    {
                        responseMessage.message = "Invalid Request";
                        responseMessage.Status = true;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        return responseMessage;
                    }
                    else
                    {
                        return responseMessage;
                    }
                }
                else
                {
                    responseMessage.Status = false;
                    responseMessage.message = "Invalid UserId";
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DriverController", "GetDriverList", ex);

                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #region Api for GetDriverListforVehicle by servicepartnerId 
        /// <summary>
        /// Api for VehicleList  by servicepartnerId
        /// take Details from tblservicepartner 
        /// added by Kranti
        /// </summary>
        /// <param name="date,servicepartnerId"></param>
        /// <returns>responseMessage</returns>

        [HttpGet]
        [Route("GetDriverListforVehicle")]
        public ResponseResult GetDriverListforVehicle(int servicepartnerId,string cityName, bool isJourneyPlannedForToday, string? searchTerm = "", int? pageNumber = 1, int? pageSize = 10)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            string username = string.Empty;
            string userRole = string.Empty;

            try
            {
                if (servicepartnerId > 0)
                {
                    if (pageNumber == 0 || pageSize == 0)
                    {
                        pageNumber = 1;
                        pageSize = 10;
                    }

                    responseMessage = _driverDetailsManager.GetAssignDriverToVehicleList(servicepartnerId,cityName, searchTerm, pageNumber, pageSize, isJourneyPlannedForToday);

                    if (responseMessage.Status == false && responseMessage.message == string.Empty)
                    {
                        responseMessage.message = "Invalid Request";
                        responseMessage.Status = true;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        return responseMessage;
                    }
                    else
                    {
                        return responseMessage;
                    }
                }
                else
                {
                    responseMessage.Status = false;
                    responseMessage.message = "Invalid UserId";
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DriverController", "GetDriverList", ex);

                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #region Api for UpdateDriverforVehicle
        /// <summary>
        /// Api Used To Update Driver
        /// created by priyanshi
        /// </summary>
        /// <param name="data"></param>
        /// <returns>executionResult</returns>
        [Authorize]
        [HttpPost]
        [Route("UpdateDriverforVehicle")]
        public ResponseResult UpdateDriverforVehicle (ReqUpdateDriverforVehicle request)
        {
            ResponseResult responseMessage = new ResponseResult();
            string username = string.Empty;
            string userRole = string.Empty;
            DAL.Entities.TblUser tblUser = null;
            try
            {
                if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;

                    //userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.LGC_Admin).ToString();
                    userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();

                    TblServicePartner tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerIsApprovrd == true && x.ServicePartnerEmailId == username.ToString() || x.ServicePartnerMobileNumber == username);

                    if (tblServicePartner != null && tblServicePartner.ServicePartnerEmailId != null)
                    {

                        var Email = SecurityHelper.EncryptString(tblServicePartner.ServicePartnerEmailId, _config.Value.SecurityKey);
                        tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.Email == Email);
                        if (tblUser != null)
                        {
                            TblUserRole tblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == tblUser.UserId);
                            if (tblUserRole != null)
                            {
                                TblRole tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == tblUserRole.RoleId);
                                if (tblRole != null && tblRole.RoleName == userRole)
                                {
                                    if (request != null)
                                    {
                                        //Manager call

                                        responseMessage = _driverDetailsManager.UpdateDriverforVehicle(request);
                                        if (responseMessage.Status == true)
                                        {
                                            responseMessage.Status_Code = HttpStatusCode.OK;
                                            return responseMessage;
                                        }
                                        else
                                        {
                                            if (responseMessage.Status == false)
                                            {
                                                return responseMessage;
                                            }
                                            else
                                            {
                                                responseMessage.Status_Code = HttpStatusCode.BadRequest;
                                                responseMessage.Status = false;
                                                responseMessage.message = "Not valid Request";
                                                return responseMessage;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        responseMessage = new ResponseResult();
                                        responseMessage.message = " Request object should not be null or greater than five";
                                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                                        return responseMessage;
                                    }
                                }
                                else
                                {
                                    responseMessage.message = "Not A Valid User";
                                    responseMessage.Status = false;
                                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                                }

                            }
                            else
                            {
                                responseMessage.message = "Not A Valid User";
                                responseMessage.Status = false;
                                responseMessage.Status_Code = HttpStatusCode.BadRequest;
                            }


                        }

                        else
                        {
                            responseMessage.message = "Not A Valid User";
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseMessage.message = "Service partner not a valid user";
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseMessage.message = "Not A Valid Request";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                }

            }
            catch (Exception ex)
            {
                responseMessage = new ResponseResult();
                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #region Api for Disable Driver
        /// <summary>
        /// Api Used To Disable Driver
        /// created by priyanshi
        /// </summary>
        /// <param name="data"></param>
        /// <returns>executionResult</returns>
        [Authorize]
        [HttpPost]
        [Route("DisableDriver")]
        public ResponseResult DisableDriver(DisableDriverDataModel data)
        {
            ResponseResult responseMessage = new ResponseResult();
            string username = string.Empty;
            string userRole = string.Empty;
            DAL.Entities.TblUser tblUser = null;
            try
            {
                if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;

                    //userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.LGC_Admin).ToString();
                    userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();

                    TblServicePartner tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerIsApprovrd == true && x.ServicePartnerEmailId == username.ToString() || x.ServicePartnerMobileNumber == username);

                    if (tblServicePartner != null && tblServicePartner.ServicePartnerEmailId != null)
                    {

                        var Email = SecurityHelper.EncryptString(tblServicePartner.ServicePartnerEmailId, _config.Value.SecurityKey);
                        tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.Email == Email);
                        if (tblUser != null)
                        {
                            TblUserRole tblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == tblUser.UserId);
                            if (tblUserRole != null)
                            {
                                TblRole tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == tblUserRole.RoleId);
                                if (tblRole != null && tblRole.RoleName == userRole)
                                {
                                    if (data != null)
                                    {
                                        //Manager call

                                        responseMessage = _servicePartnerManager.DisableDriver(data);
                                        if (responseMessage.Status == true)
                                        {
                                            responseMessage.Status_Code = HttpStatusCode.OK;
                                            return responseMessage;
                                        }
                                        else
                                        {
                                            if (responseMessage.Status == false)
                                            {
                                                return responseMessage;
                                            }
                                            else
                                            {
                                                responseMessage.Status_Code = HttpStatusCode.BadRequest;
                                                responseMessage.Status = false;
                                                responseMessage.message = "Not valid Request";
                                                return responseMessage;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        responseMessage = new ResponseResult();
                                        responseMessage.message = " Request object should not be null or greater than five";
                                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                                        return responseMessage;
                                    }
                                }
                                else
                                {
                                    responseMessage.message = "Not A Valid User";
                                    responseMessage.Status = false;
                                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                                }

                            }
                            else
                            {
                                responseMessage.message = "Not A Valid User";
                                responseMessage.Status = false;
                                responseMessage.Status_Code = HttpStatusCode.BadRequest;
                            }


                        }

                        else
                        {
                            responseMessage.message = "Not A Valid User";
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseMessage.message = "Service partner not a valid user";
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseMessage.message = "Not A Valid Request";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                }

            }
            catch (Exception ex)
            {
                responseMessage = new ResponseResult();
                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion
    }
}
