using CorePush.Google;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.LGC;
using RDCELERP.Model.PushNotification;
using RDCELERP.Model.ResponseModel;
using RDCELERP.Model.ServicePartner;
using static RDCELERP.Model.PushNotification.GoogleNotification;

namespace RDCELERP.BAL.MasterManager
{
    public class PushNotificationManager : IPushNotificationManager
    {
        public readonly IOptions<ApplicationSettings> _baseConfig;
        public readonly IMapLoginUserDeviceRepository _mapLoginUserDeviceRepository;
        public readonly IDriverDetailsRepository _driverDetailsRepository;
        public readonly IServicePartnerRepository _servicePartnerRepository;
        IPushNotificationMessageDetailRepository _pushNotificationMessageDetailRepository;
        IPushNotificationSavedDetailsRepository _pushNotificationSavedDetailsRepository;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IUserRepository _userRepository;
        public PushNotificationManager(IOptions<ApplicationSettings> baseConfig, IMapLoginUserDeviceRepository mapLoginUserDeviceRepository, ILogging logging, IPushNotificationMessageDetailRepository pushNotificationMessageDetailRepository, IDriverDetailsRepository driverDetailsRepository, IServicePartnerRepository servicePartnerRepository, IPushNotificationSavedDetailsRepository pushNotificationSavedDetailsRepository, IUserRepository userRepository)
        {
            _baseConfig = baseConfig;
            _mapLoginUserDeviceRepository = mapLoginUserDeviceRepository;
            _logging = logging;
            _pushNotificationMessageDetailRepository = pushNotificationMessageDetailRepository;
            _servicePartnerRepository = servicePartnerRepository;
            _driverDetailsRepository = driverDetailsRepository;
            _pushNotificationSavedDetailsRepository = pushNotificationSavedDetailsRepository;
            _userRepository = userRepository;
        }


        #region Method to Send Push Notification By Kranti Silawat
        /// <summary>
        /// Method to Send Push Notification
        /// </summary>
        /// <param name="DeviceId">device Id</param>
        /// <param name="UserId">User Id</param>
        /// <returns>bool</returns>
        public ResponseModel SendNotification(int? ServicePartnerId, int? DriverId, string? Title, string? commonvariabale, string? regdNo)
        {
            ResponseModel response = new ResponseModel();
            MapLoginUserDevice mapLoginUserDevice = new MapLoginUserDevice();
            TblPushNotificationSavedDetail PushNotificationSavedDetails = new TblPushNotificationSavedDetail();
            string? deviceToken=null;
            try
            {
                TblPushNotificationMessageDetail tblPushNotificationMessageDetail = _pushNotificationMessageDetailRepository.GetSingle(x =>x.Title == Title && x.IsActive == true);
                TblServicePartner tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerId == ServicePartnerId);
                TblDriverDetail? tbldriverDetail = _driverDetailsRepository.GetSingle(x => x.IsActive == true && x.DriverDetailsId == DriverId);
                if (tblPushNotificationMessageDetail != null)
                {
                    /* FCM Sender (Android Device) */
                    FcmSettings settings = new FcmSettings()
                    {
                        SenderId = _baseConfig.Value.SenderId,
                        ServerKey = _baseConfig.Value.ServerKey
                    };
                    HttpClient httpClient = new HttpClient();

                    string authorizationKey = string.Format("keyy={0}", settings.ServerKey);
                   
                    mapLoginUserDevice = _mapLoginUserDeviceRepository.GetSingle(x => x.IsActive == true && x.UserId == tblServicePartner.UserId);
                    if(mapLoginUserDevice?.UserDeviceId != null && tblPushNotificationMessageDetail?.UserType == "ServicePartner")
                    {
                        deviceToken = mapLoginUserDevice.UserDeviceId;
                    }
                    else
                    {
                        mapLoginUserDevice = _mapLoginUserDeviceRepository.GetSingle(x => x.IsActive == true && x.UserId == tbldriverDetail?.UserId && x.UserDeviceId != null);

                        deviceToken = mapLoginUserDevice?.UserDeviceId;
                    }
                 
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                    httpClient.DefaultRequestHeaders.Accept
                            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    DataPayload dataPayload = new DataPayload();
                    
                    dataPayload.Title = tblPushNotificationMessageDetail.Title;

                    if(tblPushNotificationMessageDetail.Title == EnumHelper.DescriptionAttr(NotificationEnum.EVCChangedbyAdmin))
                    {
                        string Body = tblPushNotificationMessageDetail.Body.Replace("[user]", tbldriverDetail.DriverName);
                        if (commonvariabale != null && regdNo != null)
                        {
                            string Body1 = Body.Replace("[RegNo]", regdNo);
                            dataPayload.Body = Body1.Replace("[NewEVC]", commonvariabale);
                           
                        }
                        else
                        {
                            dataPayload.Body = Body.Replace("[NewEVC]", "new EVC");
                        }
                    }
                    else
                    {
                        if (tblPushNotificationMessageDetail.UserType == "Driver")
                        {
                            string Body = tblPushNotificationMessageDetail.Body.Replace("[user]", tbldriverDetail.DriverName);
                            if (commonvariabale != null)
                            {
                                string Body1 = Body.Replace("[ordercount]", commonvariabale);
                                dataPayload.Body = Body1.Replace("[ServicePartner]", tblServicePartner.ServicePartnerName);
                            }
                            else
                            {
                                dataPayload.Body = Body.Replace("[ServicePartner]", tblServicePartner.ServicePartnerName);
                            }

                        }
                        else
                        {
                            string Body = tblPushNotificationMessageDetail.Body.Replace("[user]", tblServicePartner.ServicePartnerName);
                            if (commonvariabale != null && tbldriverDetail != null)
                            {
                                string Body1 = Body.Replace("[RegNo]", commonvariabale);
                                dataPayload.Body = Body1.Replace("[Driver]", tbldriverDetail?.DriverName);
                            }
                            else if (commonvariabale != null && tbldriverDetail == null)
                            {
                                dataPayload.Body = Body.Replace("[ordercount]", commonvariabale);

                            }
                            else
                            {
                                dataPayload.Body = Body.Replace("[Driver]", tbldriverDetail?.DriverName);
                            }

                        }
                    }

                    if (tblPushNotificationMessageDetail.Title == EnumHelper.DescriptionAttr(NotificationEnum.PickUpRescheduledByCustomer))
                    {
                        string Body = tblPushNotificationMessageDetail.Body.Replace("[user]", tblServicePartner.ServicePartnerName);
                        if (commonvariabale != null && regdNo != null)
                        {
                            string Body1 = Body.Replace("[RegNo]", regdNo);
                            string Body2 = Body1.Replace("[Driver]", tbldriverDetail?.DriverName);
                            dataPayload.Body = Body2.Replace("[DateTime]", commonvariabale);
                        }
                    }

                    if (tblPushNotificationMessageDetail.Title == EnumHelper.DescriptionAttr(NotificationEnum.NewJourneyAssigned))
                    {
                        string Body = tblPushNotificationMessageDetail.Body.Replace("[User]", tbldriverDetail.DriverName);
                        if (commonvariabale != null && regdNo != null)
                        {
                            string Body1 = Body.Replace("[VehicleNumber]", commonvariabale);
                            dataPayload.Body = Body1.Replace("[Date]", regdNo);
                             
                        }
                    }
                    if (tblPushNotificationMessageDetail.Title == EnumHelper.DescriptionAttr(NotificationEnum.Assignmentiscancelled))
                    {
                        string Body = tblPushNotificationMessageDetail.Body.Replace("[User]", tbldriverDetail.DriverName);
                        if (commonvariabale != null )
                        {                          
                            dataPayload.Body = Body.Replace("[Date]", commonvariabale);
                        }
                    }


                    dataPayload.Click_Action = "FLUTTER_NOTIFICATION_CLICK";
                    dataPayload.Tag = tblPushNotificationMessageDetail.UserType;

                    dataPayload.AndroidAttributes = new DataPayload.android(); // Create an instance
                    dataPayload.AndroidAttributes.Click_Action = "FLUTTER_NOTIFICATION_CLICK";
                    //dataPayload.AndroidAttributes.Small_Icon = notificationModel.Small_Icon;
                    dataPayload.AndroidAttributes.Tag = tblPushNotificationMessageDetail.UserType;

                    GoogleNotification notification = new GoogleNotification();
                    notification.Data = dataPayload;
                    notification.Notification = dataPayload;

                    var fcm = new CorePush.Google.FcmSender(settings, httpClient);
                    if (deviceToken != null)
                    {
                        var fcmSendResponse = fcm.SendAsync(deviceToken, notification);
                        //Code to Insert the details on tblPushNotificationSavedDetails
                        PushNotificationSavedDetails.SentUserId = mapLoginUserDevice.UserId;
                        PushNotificationSavedDetails.Title = dataPayload.Title;
                        PushNotificationSavedDetails.Body = dataPayload.Body;
                        PushNotificationSavedDetails.IsActive = true;
                        PushNotificationSavedDetails.CreatedDate = _currentDatetime;
                        PushNotificationSavedDetails.CreatedBy = mapLoginUserDevice.UserId;
                        _pushNotificationSavedDetailsRepository.Create(PushNotificationSavedDetails);
                        _pushNotificationSavedDetailsRepository.SaveChanges();

                        if (fcmSendResponse.Result.IsSuccess())
                        {
                            response.IsSuccess = true;
                            response.Message = "Notification sent successfully";
                            return response;
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "Notification not sent";
                            return response;
                        }
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Notification not sent";
                        return response;
                    }

                }
                else
                {
                    /* Code here for APN Sender (iOS Device) */
                    //var apn = new ApnSender(apnSettings, httpClient);
                    //await apn.SendAsync(notification, deviceToken);
                }
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Something went wrong";
                return response;
            }
        }
        #endregion



        #region Method to Save DeviceId By Kranti Silawat
        /// <summary>
        /// Method to t Save DeviceId
        /// </summary>
        /// <param name="DeviceId">device Id</param>
        /// <param name="UserId">User Id</param>
        /// <returns>bool</returns>
        public bool SaveDeviceId(string deviceId, string deviceType, int userId)
        {
            string response = string.Empty;
            string Message = string.Empty;
            bool flag = false;
            MapLoginUserDevice userDevice = new MapLoginUserDevice();

            try
            {
                userDevice = _mapLoginUserDeviceRepository.GetSingle(x => x.UserId.Equals(userId) && x.IsActive == true);
                DateTime start = DateTime.Now;
                if (userDevice != null)
                {
                    userDevice.UserDeviceId = deviceId;
                    userDevice.UserId = userId;
                    userDevice.DeviceType = deviceType;
                   
                    userDevice.ModifiedBy = userId;
                    userDevice.ModifiedDate = _currentDatetime;
                    _mapLoginUserDeviceRepository.Update(userDevice);
                    _mapLoginUserDeviceRepository.SaveChanges();

                   
                }

                else
                {
                    userDevice = new MapLoginUserDevice();
                    userDevice.UserDeviceId = deviceId;
                    userDevice.UserId = userId;
                    userDevice.IsActive = true;
                    userDevice.DeviceType = deviceType;
                    userDevice.CreatedBy = userId;
                    userDevice.CreatedDate = _currentDatetime;
                    _mapLoginUserDeviceRepository.Create(userDevice);
                    _mapLoginUserDeviceRepository.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("NotoficationManager", "ValidateOTP", ex);
            }
            return flag;
        }
        #endregion


        #region Method to Get Notification List By Id
        public ResponseResult GetNotificationListById(int Id, int? page, int? pageSize)
        {
            try
            {
                TblUser tblUser = new TblUser();
                if(Id <= 0)
                {
                    return new ResponseResult
                    {
                        Status = false,
                        Status_Code = HttpStatusCode.OK,
                        message = "Invalid Id "
                    };
                }

                tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.UserId == Id);

                if (tblUser == null || tblUser.UserId <= 0)
                {
                    return new ResponseResult
                    {
                        Status = false,
                        Status_Code = HttpStatusCode.OK,
                        message = "User id not found"
                    };
                }

                List<TblPushNotificationSavedDetail> tblPushNotificationSavedDetails;
                tblPushNotificationSavedDetails = _pushNotificationSavedDetailsRepository.GetNotificationListById(Id);

                if (page.HasValue && pageSize.HasValue && page > 0 && pageSize > 0)
                {
                    tblPushNotificationSavedDetails = tblPushNotificationSavedDetails
                        .OrderByDescending(x => x.CreatedDate)
                       .Skip((page.Value - 1) * pageSize.Value)
                       .Take(pageSize.Value)
                       .ToList();
                }
                else
                {
                    tblPushNotificationSavedDetails = tblPushNotificationSavedDetails.ToList();
                }

                if (tblPushNotificationSavedDetails == null || tblPushNotificationSavedDetails.Count == 0)
                {
                    return new ResponseResult
                    {
                        Status = false,
                        Status_Code = HttpStatusCode.OK,
                        message = "No data found"
                    };
                }

                List<NotificationListViewModel> notificationList = new List<NotificationListViewModel>();
                foreach(var item in tblPushNotificationSavedDetails)
                {
                    NotificationListViewModel notificationListViewModel = new NotificationListViewModel();
                    notificationListViewModel.Title = item.Title;
                    notificationListViewModel.Body = item.Body;
                    notificationListViewModel.CreatedDate = item.CreatedDate;
                    notificationListViewModel.CreatedBy = item.CreatedBy;
                    notificationList.Add(notificationListViewModel);
                }
                
                if(notificationList.Count > 0)
                {
                    return new ResponseResult
                    {
                        Status = true,
                        Status_Code = HttpStatusCode.OK,
                        Data = notificationList,
                        message = "Success"
                    };
                }
                else
                {
                    return new ResponseResult
                    {
                        Status = false,
                        Status_Code = HttpStatusCode.OK,
                        message = "No data found"
                    };
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PushNotificationManager", "GetNotificationListById", ex);

                return new ResponseResult
                {
                    Status = false,
                    Status_Code = HttpStatusCode.InternalServerError,
                    message = ex.Message
                };
            }
        }
        #endregion
    }
}
   
