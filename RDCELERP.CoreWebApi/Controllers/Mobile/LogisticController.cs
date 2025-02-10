using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Dml.Diagram;
using Org.BouncyCastle.Asn1.Ocsp;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.City;
using RDCELERP.Model.DriverDetails;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.Common;
using RDCELERP.Model.MobileApplicationModel.LGC;
using RDCELERP.Model.Paginated;
using RDCELERP.Model.ServicePartner;
using RDCELERP.Model.Users;
using static RDCELERP.Common.Helper.MessageHelper;
using TblUser = RDCELERP.DAL.Entities.TblUser;
using static RDCELERP.Model.Whatsapp.WhatsappLgcDropViewModel;
using UserDetails = RDCELERP.Model.Whatsapp.WhatsappLgcDropViewModel.UserDetails;
using WhatsappTemplate = RDCELERP.Model.Whatsapp.WhatsappLgcDropViewModel.WhatsappTemplate;
using RDCELERP.Model.PinCode;
using DocumentFormat.OpenXml.Drawing.Charts;
using Irony.Parsing;
using System.Security.Policy;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.EntityFrameworkCore;
using RDCELERP.Model.TicketGenrateModel;

namespace RDCELERP.CoreWebApi.Controllers.Mobile
{
    [Route("api/Mobile/[controller]")]
    [ApiController]
    public class LogisticController : ControllerBase
    {
        #region Variables
        ILogging _logging;
        private readonly ILogisticManager _logisticManager;
        private readonly ILogin_MobileRepository _login_MobileRepository;
        private readonly IServicePartnerManager _servicePartnerManager;
        private readonly IServicePartnerRepository _servicePartnerRepository;
        private readonly Digi2l_DevContext _context;
        private readonly ICityRepository _CityRepository;
        IOptions<ApplicationSettings> _config;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogisticsRepository _logisticsRepository;
        private readonly INotificationManager _notificationManager;
        private readonly IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _WhatsAppMessageRepository;
        IEVCRepository _eVCRepository;
        IEVCPartnerRepository _evCPartnerRepository;
        private readonly ICommonManager _commonManager;
        private readonly IPushNotificationManager _pushNotificationManager;

        #endregion

        #region Constructor
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        public LogisticController(Digi2l_DevContext context, ILogging logging, ILogisticManager logisticManager, IServicePartnerManager servicePartnerManager, ILogin_MobileRepository login_MobileRepository, IServicePartnerRepository servicePartnerRepository, ICityRepository cityRepository, IOptions<ApplicationSettings> config, IUserRoleRepository userRoleRepository, IRoleRepository roleRepository, IUserRepository userRepository, ILogisticsRepository logisticsRepository, INotificationManager notificationManager, IWhatsappNotificationManager whatsappNotificationManager, IWhatsAppMessageRepository whatsAppMessageRepository, IEVCRepository eVCRepository, ICommonManager commonManager, IPushNotificationManager pushNotificationManager, IEVCPartnerRepository eVCPartnerRepository)
        {
            _logisticManager = logisticManager;
            _logging = logging;
            _servicePartnerManager = servicePartnerManager;
            _context = context;
            _login_MobileRepository = login_MobileRepository;
            _servicePartnerRepository = servicePartnerRepository;
            _CityRepository = cityRepository;
            _config = config;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _logisticsRepository = logisticsRepository;
            _notificationManager = notificationManager;
            _whatsappNotificationManager = whatsappNotificationManager;
            _WhatsAppMessageRepository = whatsAppMessageRepository;
            _eVCRepository = eVCRepository;
            _commonManager = commonManager;
            _pushNotificationManager = pushNotificationManager;
            _evCPartnerRepository = eVCPartnerRepository;
        }
        #endregion

        #region TestLogistic Controller Api
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
                _logging.WriteErrorToDB("LogisticController", "GetTest", ex);
            }
            return responseResult;
        }
        #endregion

        #region used API in Mobile app (Added by Priyanshi/Kranti)
        #region (V-2Done) Check Mobile Number is exists or not
        [HttpPost]
        [Route("CheckMobileNumber")]
        public ResponseResult CheckMobileNumber(IsNumberorEmailExits isNumberExits)
        {
            ResponseResult responseResult = new ResponseResult();
            bool isExists = false;

            try
            {
                if (isNumberExits != null)
                {
                    isExists = _servicePartnerManager.CheckNumberOrEmail(isNumberExits);
                    if (isExists == true)
                    {
                        if (isNumberExits.isMobileNumber == true)
                        {
                            responseResult.message = "Mobile Number is Already Register";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                        else if (isNumberExits.isMobileNumber == false && isNumberExits.Email.Length > 0)
                        {
                            responseResult.message = "Email is Already Register";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                        else
                        {
                            responseResult.message = "invalid request type";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseResult.message = "Accepted";
                        responseResult.Status = true;
                        responseResult.Status_Code = HttpStatusCode.OK;
                    }
                }
                else
                {
                    responseResult.message = "invalid request type";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticController", "GetTest", ex);
            }
            return responseResult;
        }
        #endregion

        #region (V-2Done) Api for LGC/Service Partner Registeration
        /// <summary>
        /// Api Used To Register LGC/Service Partner using mobile app
        /// created by ashwin
        /// </summary>
        /// <param name="data"></param>
        /// <returns>executionResult</returns>
        /// []
        [HttpPost]
        [Route("LGCRegisteration")]
        public ResponseResult LGCRegisteration([FromForm] RegisterServicePartnerDataModel data)
        {
            ResponseResult responseMessage = null;
            try
            {
                if (data != null)
                {
                    responseMessage = new ResponseResult();

                    //Manager call
                    responseMessage = _servicePartnerManager.LGCRegister(data);
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
                    responseMessage.message = " Request object should not be null ";
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
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

        #region (V-2Done) Api for Vehicle Registeration
        /// <summary>
        /// Api Used To Register Vehicle using mobile app
        /// created by ashwin and update by priyanshi
        /// </summary>
        /// <param name="data"></param>
        /// <returns>executionResult</returns>
        /// []
        [Authorize]
        [HttpPost]
        [Route("RegisterVehicle")]
        public ResponseResult RegisterVehicle([FromForm] VehicleDataModel data)
        {
            ResponseResult responseMessage = new ResponseResult();
            string username = string.Empty;
            string userRole = string.Empty;
            TblUser tblUser = null;

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
                                        responseMessage = _servicePartnerManager.AddVehicle(data);
                                        if (responseMessage.Status == false && responseMessage.message == string.Empty)
                                        {
                                            responseMessage.message = "Registration Failed";
                                            responseMessage.Status = false;
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

        #region (V-2Done) Api for LGC/Service Partner UserDetails
        /// <summary>
        /// Api for get UserDetails
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("GetUserDetails")]
        public ResponseResult UserDetails(int UserId)
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.message = string.Empty;
            TblUser tblUser = null;
            string username = string.Empty;
            string ServicepartnerRole = string.Empty;
            string DriverRole = string.Empty;

            ServicepartnerRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();
            DriverRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner_Driver).ToString();
            try
            {
                if (UserId > 0)
                {
                    if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity.Name != null)
                    {
                        tblUser = _userRepository.GetSingle(x => x.UserId == UserId && x.IsActive == true);
                        if (tblUser == null)
                        {
                            responseResult.message = "Not A Valid User";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            return responseResult;
                        }
                        TblUserRole tblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == tblUser.UserId);

                        if (tblUserRole == null)
                        {
                            responseResult.message = "Not A Valid Role";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            return responseResult;
                        }
                        TblRole tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == tblUserRole.RoleId);
                        if (tblRole == null)
                        {
                            responseResult.message = "Not A Valid Role";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            return responseResult;
                        }
                        if (tblRole != null && (tblRole.RoleName == ServicepartnerRole || tblRole.RoleName == DriverRole))
                        {

                            responseResult = _servicePartnerManager.UserProfileDetails(UserId);
                        }

                    }
                    else
                    {
                        responseResult.message = "Not A Valid Request";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }

                else
                {
                    responseResult.message = "Invalid Request,parameter should be greater than zero";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticController", "AssignVehiclebyLGC", ex);
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }
        #endregion

        #region (V-2Done) Api for GetOrderListById  by ServicePartner Id
        /// <summary>
        /// Api for GetOrderListById  by ServicePartner Id
        /// take order list from tbllogistic
        /// added by ashwin
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>responseMessage</returns>
        [Authorize]
        [HttpGet]
        [Route("GetOrderListById")]
        public ResponseResult GetOrderListById(int Id, int page, string? CityName, bool? Alldata, int? DriverId)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            string username = string.Empty;
            string userRole = string.Empty;
            var pagesize = Convert.ToInt32(PaginationEnum.Pagesize);
            var status = 0;
            try
            {
                if (Id > 0)
                {
                    if (page <= 0)
                    {
                        page = 1; // Set default page to 1 if it's less than or equal to 0
                    }
                    if (pagesize <= 0)
                    {
                        pagesize = 10; // Set default page size to 10 if it's less than or equal to 0
                    }
                    if (Alldata == true)
                    {
                        status = 0;
                    }
                    else
                    {
                        status = Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated);
                    }
                    responseMessage = _servicePartnerManager.GetOrderListById(Id, status, page, pagesize, CityName, DriverId);

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
                _logging.WriteErrorToDB("LogisticController", "GetLGCOrderDetails", ex);

                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #region (V-2Done) Api for Get Order city list by LGC Id
        /// <summary>
        /// Api for Get Order city list by LGC Id
        /// Api for Get Order city list by LGC Id
        /// added by Priyanshi
        /// </summary>
        /// <param name="Regdno"></param>
        /// <param name="Id"></param>
        /// <returns>responseResult</returns>

        [Authorize]
        [HttpGet]
        [Route("GetOrdercitylistbyLgcId")]
        public ResponseResult GetOrdercitylistbyLgcId(int Id, bool? IsRejectedOrder)
        {
            ResponseResult responseMessage = new ResponseResult();
            ResponseResult responseResult = new ResponseResult();
            responseMessage.message = string.Empty;
            string username = string.Empty;
            string userRole = string.Empty;
            int status = 0;
            try
            {
                if (Id > 0)
                {
                    // Retrieve the order list by ServicePartner ID
                    if (IsRejectedOrder == true)
                    {
                        status = Convert.ToInt32(OrderStatusEnum.OrderRejectedbyVehicle);
                    }
                    else
                    {
                        status = Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated);
                    }
                    responseMessage = _servicePartnerManager.GetOrderListById(Id, status, null, null, null, null);

                    if (responseMessage.Status && responseMessage.Data != null)
                    {
                        AllOrderList orderDetails = (AllOrderList)responseMessage.Data;

                        List<CityList> cityLists = new List<CityList>();
                        List<string> uniqueCityNames = orderDetails.AllOrderlistViewModels.Select(order => (order.City ?? "").ToLower().Trim()).Distinct().ToList();
                        foreach (var item in uniqueCityNames)
                        {
                            TblCity? tblCity = _context.TblCities.Where(x => x.IsActive == true && x.Name.ToLower() == item.ToLower()).FirstOrDefault();
                            // TblCity tblCity = _CityRepository.GetSingle(x => x.IsActive == true && x.Name == item.ToLower().Trim());
                            if (tblCity != null)
                            {
                                CityList city = new CityList
                                {
                                    Name = tblCity.Name,
                                    CityId = tblCity.CityId,
                                    StateId = (int)tblCity.StateId
                                };

                                cityLists.Add(city);
                            }
                        }
                        if (cityLists.Count > 0)
                        {
                            responseResult.Status = true;
                            responseResult.Status_Code = HttpStatusCode.OK;
                            responseResult.Data = cityLists;
                            responseResult.message = "Success";
                        }
                        else
                        {
                            responseResult.Status = true;
                            responseResult.Status_Code = HttpStatusCode.OK;
                            responseResult.Data = cityLists;
                            responseResult.message = "Data not Found";
                        }
                    }
                    else
                    {
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        responseResult.message = responseMessage.message;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    responseResult.message = "Invalid Parameter";
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticController", "GetOrdercitylistbyLgcId", ex);
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
            }

            return responseResult;
        }
        #endregion

        #region (V-2Done) Api for Order Assign to Vehicle by LGC/Service Partner
        /// <summary>
        /// Assign Order To Vehicle by LGC partner
        /// Api  Order Assign to Vehicle by LGC/Service Partner
        /// added by Priyanshi
        /// </summary>
        /// <param name="OrdertransId"></param>
        /// <param name="LGCId"></param>
        /// <param name="driverDetailsId"></param>
        /// 
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("AssignOrdertoVehiclebyLGC")]
        public AssignOrderResponse AssignOrdertoVehiclebyLGC(AssignOrderRequest request)
        {
            AssignOrderResponse response = new AssignOrderResponse();
            response.Data = new List<ResponseResult>();
            response.Status = false;
            response.Status_Code = 0;
            response.message = null;
            response.SucssesCount = 0;
            var count = 0;
            string userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();
            try
            {
                // Your existing code...

                if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity.Name != null)
                {
                    TblServicePartner tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerIsApprovrd == true && x.ServicePartnerId == request.LGCId);

                    userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();
                    if (tblServicePartner != null && tblServicePartner.ServicePartnerEmailId != null)
                    {
                        var email = SecurityHelper.EncryptString(tblServicePartner.ServicePartnerEmailId, _config.Value.SecurityKey);
                        TblUser tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.Email == email);

                        if (tblUser != null)
                        {
                            TblUserRole tblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == tblUser.UserId);
                            if (tblUserRole != null)
                            {
                                TblRole tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == tblUserRole.RoleId);

                                if (tblRole != null && tblRole.RoleName == userRole)
                                {

                                    if (request == null)
                                    {
                                        ResponseResult responseResult = new ResponseResult
                                        {
                                            message = "Not a valid request",
                                            Status = false,
                                            Status_Code = HttpStatusCode.BadRequest
                                        };
                                        response.Data.Add(responseResult);
                                    }
                                    if (request.DriverId != null && request.DriverId > 0 && request.LGCId != null && request.LGCId > 0 && request.vehicleId != null && request.vehicleId > 0)
                                    {
                                        DateTime currentDate = _currentDatetime.Date;
                                        DateTime nextDate = currentDate.AddDays(1);

                                        TblDriverDetail? tblDriverDetailDriverChack = _context.TblDriverDetails
                                         .Include(x => x.Driver).ThenInclude(x => x.City)
                                         .FirstOrDefault(x =>
                                         x.IsActive == true &&
                                         x.DriverId == request.DriverId &&
                                         x.VehicleId != null &&
                                             x.ServicePartnerId == request.LGCId &&
                                             (request.isJourneyPlannedForToday ? x.JourneyPlanDate.Value.Date == currentDate : x.JourneyPlanDate.Value.Date == nextDate)
                                         );
                                        TblDriverDetail? tblDriverDetailVehicleChack = _context.TblDriverDetails
                                         .Include(x => x.Driver).ThenInclude(x => x.City)
                                         .FirstOrDefault(x =>
                                         x.IsActive == true &&
                                         x.DriverId != null &&
                                         x.VehicleId == request.vehicleId &&
                                             x.ServicePartnerId == request.LGCId &&
                                             (request.isJourneyPlannedForToday ? x.JourneyPlanDate.Value.Date == currentDate : x.JourneyPlanDate.Value.Date == nextDate)
                                         );



                                        if (tblDriverDetailDriverChack == null && tblDriverDetailVehicleChack == null)
                                        {
                                            TblDriverList? tblDriverList = _context.TblDriverLists.Where(x => x.DriverId == request.DriverId && x.IsActive == true).FirstOrDefault();
                                            TblVehicleList? tblVehicleList = _context.TblVehicleLists.Where(x => x.VehicleId == request.vehicleId && x.IsActive == true).FirstOrDefault();

                                            // If driver detail is not found, create a new entry
                                            tblDriverDetailDriverChack = new TblDriverDetail
                                            {
                                                // Assign values according to your model
                                                DriverName = tblDriverList.DriverName,
                                                DriverPhoneNumber = tblDriverList.DriverPhoneNumber,
                                                VehicleNumber = tblVehicleList.VehicleNumber,
                                                CreatedBy = tblServicePartner.UserId,
                                                CreatedDate = _currentDatetime,
                                                DriverId = request.DriverId,
                                                VehicleId = request.vehicleId,
                                                ServicePartnerId = request.LGCId,
                                                JourneyPlanDate = request.isJourneyPlannedForToday ? currentDate : nextDate,
                                                IsActive = true,
                                                CityId = tblDriverList.CityId,
                                                UserId = tblDriverList.UserId,
                                            };

                                            // Add the new entry to the context and save changes
                                            _context.TblDriverDetails.Add(tblDriverDetailDriverChack);
                                            var result = _context.SaveChanges();
                                            request.DriverDetailsId = tblDriverDetailDriverChack.DriverDetailsId;
                                        }
                                        else
                                        {
                                            if (request.vehicleId != tblDriverDetailDriverChack? .VehicleId || request.DriverId != tblDriverDetailVehicleChack?.DriverId)
                                            {
                                                response.Data.Add(new ResponseResult
                                                {
                                                    message = "This driver and vehicle is associated with another driver and vehicle.",
                                                    Status = false,
                                                    Status_Code = HttpStatusCode.OK
                                                });
                                                return response;
                                            }
                                            else
                                            {
                                                request.DriverDetailsId = tblDriverDetailDriverChack.DriverDetailsId;
                                            }

                                        }

                                    }
                                    else
                                    {
                                        ResponseResult responseResult = new ResponseResult
                                        {
                                            message = "Not a valid request",
                                            Status = false,
                                            Status_Code = HttpStatusCode.BadRequest
                                        };
                                        response.Data.Add(responseResult);
                                    }


                                    var status = Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner);
                                    count = _logisticsRepository.GetDriverAssignOrderCount(request.DriverDetailsId, status, request.LGCId);
                                    var remainingOrderCount = _config.Value.DriverOrderCount - count;

                                    if (count >= 0 && count < _config.Value.DriverOrderCount && remainingOrderCount >= request.OrdertransId.Count)
                                    {
                                        foreach (var item in request.OrdertransId)
                                        {
                                            ResponseResult responseResult = _servicePartnerManager.OrderAssignLGCtoDriverid(item, request.LGCId, request.DriverDetailsId);
                                            if (responseResult.Status)
                                            {

                                                // Increment the success count and update the response message
                                                response.SucssesCount += 1;
                                                response.Status = true;
                                                response.Status_Code = HttpStatusCode.OK;
                                                response.message = response.SucssesCount + " Orders Are Successfully Assigned";
                                            }
                                            else
                                            {
                                                // If a response is a failure, add it to the response data
                                                response.Data.Add(responseResult);
                                            }
                                        }

                                        // Check if all orders were successfully assigned
                                        if (response.Data.Count == request.OrdertransId.Count)
                                        {
                                            response.Status = false;
                                            response.Status_Code = HttpStatusCode.OK;
                                            response.message = "All Order failed ";
                                        }
                                        if (response.SucssesCount > 0)
                                        {
                                            string SucssesCountPush = response.SucssesCount.ToString();
                                            var result = _pushNotificationManager.SendNotification(request.LGCId, request.DriverDetailsId, EnumHelper.DescriptionAttr(NotificationEnum.OrderAssignedbyServicePartner), SucssesCountPush, null);
                                        }
                                    }
                                    else
                                    {
                                        ResponseResult responseResult = new ResponseResult
                                        {
                                            message = "Now you can only assign " + remainingOrderCount + " order(s) to this vehicle.",
                                            Status = false,
                                            Status_Code = HttpStatusCode.BadRequest
                                        };
                                        response.Data.Add(responseResult);
                                    }
                                }
                                else
                                {
                                    ResponseResult responseResult = new ResponseResult
                                    {
                                        message = "Not a valid user",
                                        Status = false,
                                        Status_Code = HttpStatusCode.BadRequest
                                    };
                                    response.Data.Add(responseResult);
                                }
                            }
                            else
                            {
                                ResponseResult responseResult = new ResponseResult
                                {
                                    message = "Service partner is not a valid user",
                                    Status = false,
                                    Status_Code = HttpStatusCode.BadRequest
                                };
                                response.Data.Add(responseResult);
                            }
                        }
                        else
                        {
                            ResponseResult responseResult = new ResponseResult
                            {
                                message = "Not a valid request",
                                Status = false,
                                Status_Code = HttpStatusCode.BadRequest
                            };
                            response.Data.Add(responseResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticController", "AssignVehiclebyLGC", ex);
                ResponseResult responseResult = new ResponseResult
                {
                    message = ex.Message,
                    Status = false,
                    Status_Code = HttpStatusCode.InternalServerError
                };
                response.Data.Add(responseResult);
            }

            return response;
        }
        #endregion

        #region (V-2Done)  Api for Order reject  by LGC/Service Partner
        /// <summary>
        /// Assign Order To Vehicle by LGC partner
        /// Api  Order Assign to Vehicle by LGC/Service Partner
        /// added by Priyanshi
        /// </summary>
        /// <param name="Regdno"></param>
        /// <param name="DriverDetailsId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("RejectOrderbyLGC")]
        public ResponseResult RejectOrderbyLGC(RejectLGCOrderRequest request)
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.message = string.Empty;
            TblUser tblUser = null;
            string username = string.Empty;
            string userRole = string.Empty;

            try
            {
                if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity.Name != null)
                {
                    TblServicePartner tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerIsApprovrd == true && x.ServicePartnerId == request.LGCId);

                    userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();
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

                                    responseResult = _servicePartnerManager.RejectOrderbyLGC(request.OrdertransId, request.LGCId, request.RejectComment);
                                }
                                else
                                {
                                    responseResult.message = "Not A Valid User";
                                    responseResult.Status = false;
                                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                                }

                            }
                            else
                            {
                                responseResult.message = "Not A Valid User";
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.BadRequest;
                            }


                        }

                        else
                        {
                            responseResult.message = "Not A Valid User";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseResult.message = "Service partner not a valid user";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.message = "Not A Valid Request";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticController", "AssignVehiclebyLGC", ex);
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }
        #endregion

        #region  Api for GetAllVehicleRejectOrderListById  by ServicePartner Id
        /// <summary>
        /// Api for GetAllVehicleRejectOrderListById  by ServicePartner Id
        /// take order list from tbllogistic
        /// added by Priyanshi
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>responseMessage</returns>
        [Authorize]
        [HttpGet]
        [Route("GetAllVehicleRejectOrderListById")]
        public ResponseResult GetAllVehicleRejectOrderListById(int Id, int? page, string? CityName, int? DriverId)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            string username = string.Empty;
            string userRole = string.Empty;
            var pagesize = Convert.ToInt32(PaginationEnum.Pagesize);
            try
            {
                if (Id > 0)
                {
                    if (page == null)
                    {
                        page = 1;
                    }

                    var status = Convert.ToInt32(OrderStatusEnum.OrderRejectedbyVehicle);
                    responseMessage = _servicePartnerManager.GetOrderListById(Id, status, page, pagesize, CityName, DriverId);

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
                _logging.WriteErrorToDB("LogisticController", "GetLGCOrderDetails", ex);

                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #region (V-2Done) Api for GetAllOrderDetailsByOrdertransId  by OrdertransId
        /// <summary>
        /// Api for GetAllOrderDetailsByOrdertransId  by OrdertransId
        /// take order Details from tblordertrans
        /// added by priyanshi
        /// </summary>
        /// <param name="OrdertransId"></param>
        /// <returns>responseMessage</returns>
        [Authorize]
        [HttpGet]
        [Route("GetOrderDetailsByOrdertransId")]
        public ResponseResult GetOrderDetailsByOrdertransId(int OrdertransId)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            string username = string.Empty;
            string userRole = string.Empty;

            try
            {
                if (OrdertransId > 0)
                {

                    responseMessage = _servicePartnerManager.GetOrderDetailsByOTId(OrdertransId);

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
                _logging.WriteErrorToDB("LogisticController", "GetLGCOrderDetails", ex);

                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #region Vehicle list at Service Partner end
        /// <summary>
        /// Api for Vehicle list at Service Partner end  by ServicePartner Id and Journey Plan Date
        /// take order list from TblVehicleJourneyTracking
        /// added by Priyanshi
        /// </summary>
        /// <param name="ServiceP"></param>
        /// <param name="JourneyPlanDate"></param>
        /// <returns>responseMessage</returns>
        [Authorize]
        [HttpGet]
        [Route("StartJournyVehicleListbyServiceP")]
        public ResponseResult StartJournyVehicleListbyServiceP(int ServicePartnerId, DateTime journeyDate)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;

            try
            {
                if (ServicePartnerId > 0 && journeyDate != DateTime.MinValue)
                {
                    // var status = Convert.ToInt32(OrderStatusEnum.OrderAcceptedbyVehicle);
                    responseMessage = _servicePartnerManager.StartJournyVehicleListbyServiceP(ServicePartnerId, journeyDate);

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
                _logging.WriteErrorToDB("VehicleDetailsController", "GetAssignOrderListByvehicle", ex);

                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #region GetCurrentLetLogforVehicle
        /// <summary>
        /// Api for Vehicle list at Service Partner end  by ServicePartner Id and Journey Plan Date
        /// take order list from TblVehicleJourneyTracking
        /// added by Priyanshi
        /// </summary>
        /// <param name="ServiceP"></param>
        /// <param name="DriverDetailsId"></param>
        /// <param name="JourneyPlanDate"></param>
        /// <returns>responseMessage</returns>
        [Authorize]
        [HttpGet]
        [Route("GetCurrentLetLogforVehicle")]
        public ResponseResult GetCurrentLetLogforVehicle(int DriverDetailsId, int ServicePartnerId, DateTime journeyDate)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;

            try
            {
                if (DriverDetailsId > 0 && ServicePartnerId > 0 && journeyDate != DateTime.MinValue)
                {
                    // var status = Convert.ToInt32(OrderStatusEnum.OrderAcceptedbyVehicle);
                    responseMessage = _servicePartnerManager.GetCurrentLetLogforVehicle(DriverDetailsId, ServicePartnerId, journeyDate);

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
                _logging.WriteErrorToDB("VehicleDetailsController", "GetAssignOrderListByvehicle", ex);

                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #region Api for SPDashboard & DriverDashboard  by date, servicepartnerId & driverId
        /// <summary>
        /// Api for ServicePartnerDashboard  by date, servicepartnerId & driverId
        /// take Details from tblVehiclejourneyTrackingDetails
        /// added by Kranti
        /// </summary>
        /// <param name="date,servicepartnerId"></param>
        /// <returns>responseMessage</returns>
        [Authorize]
        [HttpGet]
        [Route("GetSPDashboard")]
        public ResponseResult GetSPDashboard(DateTime? Date, int servicepartnerId, int? driverId)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            string username = string.Empty;
            string userRole = string.Empty;

            try
            {
                if (servicepartnerId > 0)
                {

                    responseMessage = _servicePartnerManager.ServicePartnerDashboard(Date, servicepartnerId, driverId);

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
                _logging.WriteErrorToDB("LogisticController", "GetServicePartnerDashboard", ex);

                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #region Api for WalletSummeryList & DriverWalletSummeryList Dashboard by date, servicepartnerId & driverId
        /// <summary>
        /// Api for ServicePartnerDashboard  by date, servicepartnerId & driverId
        /// take Details from tblVehiclejourneyTrackingDetails
        /// added by Kranti
        /// </summary>
        /// <param name="date,servicepartnerId"></param>
        /// <returns>responseMessage</returns>
        [Authorize]
        [HttpGet]
        [Route("GetSPWalletSummeryList")]
        public ResponseResult GetSPWalletSummeryList(DateTime? Date, int servicepartnerId, int? driverId)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            string username = string.Empty;
            string userRole = string.Empty;

            try
            {
                if (servicepartnerId > 0)
                {

                    responseMessage = _servicePartnerManager.SPWalletSummeryList(Date, servicepartnerId, driverId);

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
                _logging.WriteErrorToDB("LogisticController", "GetServicePartnerDashboard", ex);

                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #region Api for VehicleList by servicepartnerId 
        /// <summary>
        /// Api for VehicleList  by servicepartnerId
        /// take Details from tblservicepartner 
        /// added by Kranti
        /// </summary>
        /// <param name="date,servicepartnerId"></param>
        /// <returns>responseMessage</returns>

        [HttpGet]
        [Route("GetVehicleList")]
        public ResponseResult GetVehicleList(int servicepartnerId, string? searchTerm = "", int? pageNumber = 1, int? pageSize = 10)
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

                    responseMessage = _servicePartnerManager.VehicleList(servicepartnerId, searchTerm, pageNumber, pageSize);

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
                _logging.WriteErrorToDB("LogisticController", "GetVehicleList", ex);

                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #region Api for SPDashboard & DriverDashboard  by date, servicepartnerId & driverId
        /// <summary>
        /// Api for ServicePartnerDashboard  by date, servicepartnerId & driverId
        /// take Details from tblVehiclejourneyTrackingDetails
        /// added by Kranti
        /// </summary>
        /// <param name="date,servicepartnerId"></param>
        /// <returns>responseMessage</returns>
        [Authorize]
        [HttpGet]
        [Route("GetSPWalletSummeryCount")]
        public ResponseResult GetSPWalletSummeryCount(DateTime Date, int servicepartnerId, int? driverId)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            string username = string.Empty;
            string userRole = string.Empty;

            try
            {
                if (servicepartnerId > 0)
                {

                    responseMessage = _servicePartnerManager.SPWalletSummeryCount(Date, servicepartnerId, driverId);

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
                _logging.WriteErrorToDB("LogisticController", "GetServicePartnerDashboard", ex);

                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #region send OTP in Drop done

        [HttpPost]
        [Route("DropDoneOTP")]
        public ResultResponse DropDoneOTP(DropDoneOTPReq request)
        {
            ResultResponse resultResponse = new ResultResponse();
            string number = string.Empty;
            WhatasappResponse whatasappResponse = new WhatasappResponse();
            TblWhatsAppMessage? tblwhatsappmessage = null;

            string tempaltename;
            string mobnumber;
            //bool isMobileNumberValid = false;
            try
            {
                if (request != null && request.EVCId > 0 && request.EvcPartnerId > 0)
                {
                    #region send Otp to Customer mobile number for login
                    bool flag = false;
                    //TblEvcregistration tblEvcregistration = _eVCRepository.GetEVCDetailsById(request.EVCId);
                    TblEvcPartner tblEvcpartner = _evCPartnerRepository.GetEVCPartnerDetails(request.EvcPartnerId);
                    if (tblEvcpartner != null)
                    {
                        if (tblEvcpartner.ContactNumber != null)
                        {
                            MobileOtpVerification mobileOtpVerification = new MobileOtpVerification();
                            mobnumber = tblEvcpartner.ContactNumber;
                            tempaltename = "SMS_Drop_OTP";
                            string OTPValue = UniqueString.RandomNumber();
                            string message = string.Empty;

                            if (tempaltename.Equals("SMS_Drop_OTP"))
                                message = NotificationConstants.SMS_Drop_OTP.Replace("[OTP]", OTPValue);
                            flag = _notificationManager.SendNotificationSMS(mobnumber, message, OTPValue);

                            #region code to send OTP on whatsappNotification for lgc Drop
                            WhatsappTemplate whatsappObj = new WhatsappTemplate();
                            whatsappObj.userDetails = new UserDetails();
                            whatsappObj.notification = new LogiDrop();
                            whatsappObj.notification.@params = new SendOTP();
                            whatsappObj.userDetails.number = mobnumber;
                            whatsappObj.notification.sender = _config.Value.YelloaiSenderNumber;
                            whatsappObj.notification.type = _config.Value.YellowaiMesssaheType;
                            whatsappObj.notification.templateId = NotificationConstants.Logi_Drop;
                            whatsappObj.notification.@params.OTP = OTPValue;
                            string url = _config.Value.YellowAiUrl;
                            RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                            if (response.Content != null)
                            {
                                whatasappResponse = JsonConvert.DeserializeObject<WhatasappResponse>(response.Content);
                                tblwhatsappmessage = new TblWhatsAppMessage();
                                tblwhatsappmessage.TemplateName = NotificationConstants.Logi_Drop;
                                tblwhatsappmessage.IsActive = true;
                                tblwhatsappmessage.PhoneNumber = mobnumber;
                                tblwhatsappmessage.SendDate = DateTime.Now;
                                tblwhatsappmessage.MsgId = whatasappResponse.msgId;
                                tblwhatsappmessage.Code = OTPValue;
                                _WhatsAppMessageRepository.Create(tblwhatsappmessage);
                                _WhatsAppMessageRepository.SaveChanges();
                            }
                            #endregion

                            if (flag)
                            {
                                resultResponse.Status = true;
                                resultResponse.Status_Code = HttpStatusCode.OK;
                                resultResponse.message = "otp successfully send to " + mobnumber;
                                return resultResponse;
                            }
                            else
                            {

                                resultResponse.Status = false;
                                resultResponse.Status_Code = HttpStatusCode.OK;
                                resultResponse.message = "invalid mobile number system not allow send otp to " + mobileOtpVerification.mobileNumber;
                                return resultResponse;
                            }

                        }
                        else
                        {
                            resultResponse.Status = false;
                            resultResponse.Status_Code = HttpStatusCode.OK;
                            resultResponse.message = "EVC Mobile Number Not Available";
                            return resultResponse;
                        }
                    }
                    else
                    {
                        resultResponse.Status = false;
                        resultResponse.Status_Code = HttpStatusCode.BadRequest;
                        resultResponse.message = "Order Not  Avalible";
                        return resultResponse;
                    }
                    #endregion

                }
                else
                {
                    resultResponse.Status = false;
                    resultResponse.Status_Code = HttpStatusCode.BadRequest;
                    resultResponse.message = "Not a valid request";
                    return resultResponse;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticController", "DropDoneOTP", ex);
                resultResponse = new ResultResponse();
                resultResponse.message = ex.Message;
                resultResponse.Status = false;
                resultResponse.Status_Code = HttpStatusCode.InternalServerError;
            }
            return resultResponse;
        }
        #endregion

        #region OTP in Droppdone Verfication
        [HttpPost]
        [Route("DropdoneOTPVerfication")]
        public ResponseResult DropdoneOTPVerfication(DropDoneOTPVerficationReq request)
        {
            ResponseResult resultResponse = new ResponseResult();

            string number = string.Empty;
            string email = string.Empty;
            string? mobnumber;
            try
            {
                if (request == null)
                {
                    resultResponse.Status = false;
                    resultResponse.message = "Invalid model object";
                    resultResponse.Status_Code = HttpStatusCode.BadRequest;
                    return resultResponse;
                }

                if (request.EVCId <= 0 || request.EvcPartnerId <= 0 || string.IsNullOrEmpty(request.OTP) || request.DriverDetailsId <= 0)
                {
                    resultResponse.Status = false;
                    resultResponse.Status_Code = HttpStatusCode.BadRequest;
                    resultResponse.message = "Invalid OTP";
                    return resultResponse;
                }

                //TblEvcregistration? tblEvcregistration = _eVCRepository.GetEVCDetailsById(request.EVCId);
                TblEvcPartner? tblEvcPartner = _evCPartnerRepository.GetEVCPartnerDetails(request.EvcPartnerId);
                if (tblEvcPartner != null)
                {
                    mobnumber = tblEvcPartner?.ContactNumber;
                    bool isOTPValid = _notificationManager.ValidateOTP(mobnumber, request.OTP);

                    if (!isOTPValid)
                    {
                        resultResponse.Status = false;
                        resultResponse.Status_Code = HttpStatusCode.BadRequest;
                        resultResponse.message = "Invalid OTP: " + request.OTP;
                        return resultResponse;
                    }
                    else
                    {

                        resultResponse.Status = true;
                        resultResponse.Status_Code = HttpStatusCode.OK;
                        resultResponse.message = "Valid OTP ";
                        return resultResponse;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticController", "DropdoneOTPVerfication", ex);
                resultResponse.Status = false;
                resultResponse.message = ex.Message;
                resultResponse.Status_Code = HttpStatusCode.InternalServerError;
            }

            return resultResponse;
        }

        #endregion

        #region Api for Update LGC/Service Partner Registeration
        /// <summary>
        /// Api Used To Update LGC/Service Partner using mobile app
        /// created by Kranti
        /// </summary>
        /// <param name="data"></param>
        /// <returns>executionResult</returns>
        [HttpPost]
        [Route("UpdatePincodesInServicePartner")]
        public ResponseResult UpdatePincodesInServicePartner(UpdatePinCodeServicePartner data)
        {
            ResponseResult? responseMessage = null;
            try
            {
                if (data.ServicePartnerId > 0)
                {
                    responseMessage = new ResponseResult();

                    //Manager call
                    responseMessage = _servicePartnerManager.UpdatePincodesServicePartner(data);
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
                    responseMessage.message = " Request object should not be null ";
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
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

        #region Api for Update LGC/Service Partner Registeration
        /// <summary>
        /// Api Used To Update LGC/Service Partner using mobile app
        /// created by Kranti
        /// </summary>
        /// <param name="data"></param>
        /// <returns>executionResult</returns>
        [HttpPost]
        [Route("UpdateServicePartner")]
        public ResponseResult UpdateServicePartner([FromForm] UpdateServicePartnerDataModel data)
        {
            ResponseResult? responseMessage = null;
            try
            {
                if (data.ServicePartnerId > 0)
                {
                    responseMessage = new ResponseResult();

                    //Manager call
                    responseMessage = _servicePartnerManager.UpdateServicePartner(data);
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
                    responseMessage.message = " Request object should not be null ";
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
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
        /// created by Kranti
        /// </summary>
        /// <param name="data"></param>
        /// <returns>executionResult</returns>

        [HttpPost]
        [Route("UpdateVehicle")]
        public ResponseResult UpdateVehicle([FromForm] UpdateVehicleDataModel data)
        {
            ResponseResult responseMessage = new ResponseResult();
            try
            {
                if (data != null && data.VehicleId > 0 && data.ServicePartnerId > 0)
                {
                    responseMessage = new ResponseResult();

                    //Manager call
                    responseMessage = _servicePartnerManager.UpdateVehicle(data);
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
                    responseMessage.message = " Request object should not be null ";
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
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

        #region API for GetOrderListOfUninitiatedPaymentByUserId
        [HttpGet]
        [Authorize]
        [Route("GetOrderListOfUninitiatedPaymentByUserId")]
        public ResponseResult GetOrderListOfUninitiatedPaymentByUserId(int userId, int page)
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.message = string.Empty;
            var pageSize = Convert.ToInt32(PaginationEnum.Pagesize);
            try
            {
                if (userId > 0)
                {
                    if (page <= 0)
                    {
                        page = 1; // Set default page to 1 if it's less than or equal to 0
                    }
                    if (pageSize <= 0)
                    {
                        pageSize = 10; // Set default page size to 10 if it's less than or equal to 0
                    }

                    responseResult = _servicePartnerManager.GetOrderListOfUninitiatedPaymentByUserId(userId, page, pageSize);
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.message = "Invalid UserId";
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticController", "GetOrderListOfUninitiatedPaymentByUserId", ex);
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
            }

            return responseResult;
        }
        #endregion

        #region Api for PlanJournyList
        /// <summary>
        /// Api Used To Disable Driver
        /// created by priyanshi
        /// </summary>
        /// <param name="data"></param>
        /// <returns>executionResult</returns>
        [Authorize]
        [HttpPost]
        [Route("PlanJourneyList")]
        public ResponseResult PlanJourneyList(PlanJourneyListDataModel data)
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

                                        responseMessage = _servicePartnerManager.PlanJourneyList(data);
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
        #endregion

        #region Not Used API (Added by ashwin)
        #region Api for GetAllOrderbyDriverId
        /// <summary>
        /// Api for get all order assgin to particular driver
        /// added by ashwin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("GetAllOrderbyDriverId")]
        public ResponseResult GetLGCOrderDetails(int id)
        {
            ResponseResult responseMessage = new ResponseResult();
            try
            {
                if (id > 0)
                {
                    responseMessage = _servicePartnerManager.OrderRegdNoByDriverId(id);
                    if (responseMessage.Status == true)
                    {
                        responseMessage.Status_Code = HttpStatusCode.OK;
                        return responseMessage;
                    }
                    else
                    {
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        return responseMessage;
                    }
                }
                else
                {
                    responseMessage = new ResponseResult();
                    responseMessage.Status = false;
                    responseMessage.message = "Request Parameter should not be null ";
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticController", "GetLGCOrderDetails", ex);

                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #region Api for Accept/Reject orders by driver
        /// <summary>
        /// Api for Accept/Reject order by driver
        /// added by ashwin
        /// </summary>
        /// <param name="Regdno"></param>
        /// <param name="driverId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("AcceptOrderByDriver")]
        public ResponseResult AcceptOrderByDriver(string RegdNo, bool isAccepted)
        {
            ResponseResult responseMessage = new ResponseResult();
            try
            {
                if (RegdNo != string.Empty && RegdNo != null)
                {

                    responseMessage = _servicePartnerManager.AcceptOrder(RegdNo, isAccepted);
                    if (responseMessage.Status == true)
                    {
                        responseMessage.Status_Code = HttpStatusCode.OK;
                        return responseMessage;
                    }
                    else
                    {
                        responseMessage.Status = false;
                        //responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        return responseMessage;
                    }
                }
                else
                {
                    responseMessage = new ResponseResult();
                    responseMessage.Status = false;
                    responseMessage.message = " Request object should not be null ";
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticController", "GetLGCOrderDetails", ex);

                responseMessage.message = ex.Message;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseMessage;
        }
        #endregion

        #region Api for Reject Order by LGC/Service Partner////this Api Use for Driver Reject Order
        ///// <summary>
        ///// Api  Reject Order by LGC/Service Partner
        ///// added by ashwin
        ///// </summary>
        ///// <param name="Regdno"></param>
        ///// <param name="servicePartnerId"></param>
        ///// <returns></returns>
        //[Authorize]
        //[HttpPost]
        //[Route("RejectOrderbyLGC")]
        //public ResponseResult RejectOrderbyLGC(string RegdNo, string Comment)
        //{
        //    ResponseResult responseMessage = new ResponseResult();
        //    responseMessage.message = string.Empty;
        //    string username = string.Empty;
        //    string userRole = string.Empty;
        //    TblLoginMobile tblLoginMobile = null;
        //    try
        //    {
        //        if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity.Name != null)
        //        {
        //            username = HttpContext.User.Identity.Name;
        //            userRole = EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString();

        //            tblLoginMobile = _login_MobileRepository.GetSingle(x => x.IsActive == true && x.Username == username.ToString());
        //            if (tblLoginMobile != null && tblLoginMobile.UserRoleName == userRole)
        //            {
        //                responseMessage = _servicePartnerManager.RejectOrderbyLGC(RegdNo, Comment, username);
        //            }
        //            else
        //            {
        //                responseMessage.message = "invalid user Role";
        //                responseMessage.Status = false;
        //                responseMessage.Status_Code = HttpStatusCode.BadRequest;
        //                return responseMessage;
        //            }
        //        }
        //        else
        //        {
        //            responseMessage.message = "invalid user";
        //            responseMessage.Status = false;
        //            responseMessage.Status_Code = HttpStatusCode.BadRequest;
        //            return responseMessage;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.WriteErrorToDB("LogisticController", "GetLGCOrderDetails", ex);

        //        responseMessage.message = ex.Message;
        //        responseMessage.Status_Code = HttpStatusCode.InternalServerError;
        //    }
        //    return responseMessage;
        //}
        #endregion

        #region Number of Vehicle Register by servicepartnerID
        [Authorize]
        [HttpGet]
        [Route("NumberOfRegisterVehicles")]
        public ResponseResult NumberOfRegisterVehicles()
        {
            ResponseResult? responseResult = new ResponseResult();
            string username = string.Empty;
            try
            {
                if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;

                    if (!string.IsNullOrEmpty(username))
                    {
                        responseResult = _servicePartnerManager.GetNumberofVehicle(username);
                        if (responseResult != null && responseResult.Status == false && responseResult.message == string.Empty)
                        {
                            responseResult.message = " invalid request";
                            responseResult.Status = true;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            return responseResult;
                        }
                        else
                        {
                            return responseResult;
                        }
                    }
                    else
                    {
                        responseResult.message = "User Not Found";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.message = "UnAutorized";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.Unauthorized;
                }

            }
            catch (Exception ex)
            {
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.BadRequest;
                _logging.WriteErrorToDB("LogisticController", "NumberOfRegisterVehicles", ex);
            }
            return responseResult;
        }
        #endregion
        #endregion
    }
}
