using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text.Json;
using RDCELERP.BAL.Enum;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.CashfreeModel;
using RDCELERP.Model.Company;
using RDCELERP.Model.DriverDetails;
using RDCELERP.Model.EVC_Allocated;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.LGC;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.EVC;
using RDCELERP.Model.MobileApplicationModel.LGC;
using RDCELERP.Model.ServicePartner;
using RDCELERP.Model.TicketGenrateModel;
using RDCELERP.Model.Users;

namespace RDCELERP.BAL.MasterManager
{
    public class DriverDetailsManager : IDriverDetailsManager
    {
        #region variable declartion
        IExchangeOrderRepository _exchangeOrderRepository;
        IServicePartnerRepository _servicePartnerRepository;
        IOrderTransRepository _orderTransRepository;
        IOrderLGCRepository _orderLGCRepository;
        ILovRepository _lovRepository;
        IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        ILogisticsRepository _logisticsRepository;
        IExchangeOrderStatusRepository _exchangeOrderStatusRepository;
        IHtmlToPDFConverterHelper _htmlToPdfConverterHelper;
        IExchangeABBStatusHistoryRepository _exchangeABBStatusHistoryRepository;
        IDriverDetailsRepository _driverDetailsRepository;
        Digi2l_DevContext _digi2L_DevContext;
        public readonly IOptions<ApplicationSettings> _baseConfig;
        ICommonManager _commonManager;
        IWalletTransactionRepository _walletTransactionRepository;
        private readonly IVehicleJourneyTrackingDetailsRepository _journeyTrackingDetailsRepository;
        private readonly IVehicleJourneyTrackingRepository _journeyTrackingRepository;
        IImageHelper _imageHelper;
        IOrderImageUploadRepository _orderImageUploadRepository;
        IBusinessPartnerRepository _businessPartnerRepository;
        IVoucherRepository _voucherRepository;
        IABBRedemptionRepository _abbRedemptionRepository;
        IBrandSmartBuyRepository _brandSmartBuyRepository;
        IBrandRepository _brandRepository;
        IBusinessUnitRepository _businessUnitRepository;
        IABBRedemptionRepository _aBBRedemptionRepository;
        //ILogisticManager _logisticManager;
        IEVCPODDetailsRepository _evcPoDDetailsRepository;
        IEVCRepository _eVCRepository;
        IEVCPartnerRepository _evCPartnerRepository;
        IEVCWalletHistoryRepository _eVCWalletHistoryRepository;
        private DAL.Entities.Digi2l_DevContext _context;
        IPaymentLeaser _paymentLeaserRepository;
        private readonly IPushNotificationManager _pushNotificationManager;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IDriverListRepository _driverListRepository;
        private readonly IVehicleListRepository _vehicleListRepository;

        #endregion

        #region Constructor
        public DriverDetailsManager(IExchangeOrderRepository exchangeOrderRepository, ILogging logging, IServicePartnerRepository servicePartnerRepository, ILovRepository lovRepository, IOrderImageUploadRepository orderImageUploadRepository, IMapper mapper, IOrderTransRepository orderTransRepository, IOrderLGCRepository orderLGC, ILogisticsRepository logisticsRepository, IExchangeOrderStatusRepository exchangeOrderStatusRepository, IHtmlToPDFConverterHelper htmlToPdfConverterHelper, IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository, IDriverDetailsRepository driverDetailsRepository,
            IOptions<ApplicationSettings> baseConfig, Digi2l_DevContext digi2L_DevContext, ICommonManager commonManager, IWalletTransactionRepository walletTransactionRepository, IVehicleJourneyTrackingDetailsRepository journeyTrackingDetailsRepository, IVehicleJourneyTrackingRepository journeyTrackingRepository, IImageHelper imageHelper, IBusinessPartnerRepository businessPartnerRepository, IVoucherRepository voucherRepository, IABBRedemptionRepository abbRedemptionRepository, IBrandSmartBuyRepository brandSmartBuyRepository
        , IBrandRepository brandRepository, IBusinessUnitRepository businessUnitRepository, IABBRedemptionRepository aBBRedemptionRepository, IEVCPODDetailsRepository eVCPODDetailsRepository, IEVCRepository eVCRepository, IEVCWalletHistoryRepository eVCWalletHistoryRepository, DAL.Entities.Digi2l_DevContext context, IPaymentLeaser paymentLeaserRepository, IPushNotificationManager pushNotificationManager, IUserRepository userRepository, IUserRoleRepository userRoleRepository, IRoleRepository roleRepository, ICompanyRepository companyRepository, IEVCPartnerRepository evCPartnerRepository, IDriverListRepository driverListRepository, IVehicleListRepository vehicleListRepository)

        {
            _exchangeOrderRepository = exchangeOrderRepository;
            _logging = logging;
            _servicePartnerRepository = servicePartnerRepository;
            _lovRepository = lovRepository;
            _mapper = mapper;
            _orderTransRepository = orderTransRepository;
            _orderLGCRepository = orderLGC;
            _logisticsRepository = logisticsRepository;
            _exchangeOrderStatusRepository = exchangeOrderStatusRepository;
            _htmlToPdfConverterHelper = htmlToPdfConverterHelper;
            _exchangeABBStatusHistoryRepository = exchangeABBStatusHistoryRepository;
            _digi2L_DevContext = digi2L_DevContext;
            _driverDetailsRepository = driverDetailsRepository;
            _baseConfig = baseConfig;
            _digi2L_DevContext = digi2L_DevContext;
            _commonManager = commonManager;
            _walletTransactionRepository = walletTransactionRepository;
            _journeyTrackingDetailsRepository = journeyTrackingDetailsRepository;
            _journeyTrackingRepository = journeyTrackingRepository;
            _imageHelper = imageHelper;
            _orderImageUploadRepository = orderImageUploadRepository;
            _businessPartnerRepository = businessPartnerRepository;
            _voucherRepository = voucherRepository;
            _abbRedemptionRepository = abbRedemptionRepository;
            _brandSmartBuyRepository = brandSmartBuyRepository;
            _brandRepository = brandRepository;
            _businessUnitRepository = businessUnitRepository;
            _aBBRedemptionRepository = abbRedemptionRepository;
            _evcPoDDetailsRepository = eVCPODDetailsRepository;
            _eVCRepository = eVCRepository;
            _eVCWalletHistoryRepository = eVCWalletHistoryRepository;
            _context = context;
            _paymentLeaserRepository = paymentLeaserRepository;
            _pushNotificationManager = pushNotificationManager;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _companyRepository = companyRepository;
            _evCPartnerRepository = evCPartnerRepository;
            _driverListRepository = driverListRepository;
            _vehicleListRepository = vehicleListRepository;
            //_logisticManager = logisticManager;

        }
        #endregion

        #region use method 
        #region VehiclelistbyLGCIdandCityId by login ServicePartner/LGC and CityName
        /// <summary>
        /// Api method VehiclelistbyLGCIdandCityId by login ServicePartner/LGC
        /// Added by Priyanshi
        /// </summary>
        /// <param name="LgcId">LGC ID</param>
        /// <param name="CityName">City Name</param>
        /// <returns>ResponseResult</returns>
        public ResponseResult VehiclelistbyLGCIdandCityId(int lgcId, string cityName, bool isJourneyPlannedForToday, int pageNumber, int pageSize, string? filterBy = null)
        {
            ResponseResult responseMessage = new ResponseResult
            {
                message = string.Empty
            };

            try
            {
                if (lgcId <= 0)
                {
                    responseMessage.message = "Service Partner Id Not Found";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }

                if (string.IsNullOrEmpty(cityName))
                {
                    responseMessage.message = "City Not Found";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }

                TblServicePartner tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerId == lgcId);

                if (tblServicePartner == null)
                {
                    responseMessage.message = "Invalid login credentials";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }

                IQueryable<TblVehicleList> query = _context.TblVehicleLists
                    .Include(x => x.City)
                    .Where(x => x.IsActive == true && x.City.Name.ToLower() == cityName.ToLower() && x.ServicePartnerId == lgcId);

                if (!string.IsNullOrEmpty(filterBy))
                {
                    query = query.Where(x => x.VehicleNumber.Contains(filterBy));
                }

                List<VehicleListByCityResponse> vehicleDetailsListFinal = query
                    .OrderBy(x => x.VehicleId)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(vehicle => new VehicleListByCityResponse
                    {
                        City = vehicle.City.Name,
                        CityId = vehicle.CityId ?? 0,
                        VehicleId = vehicle.VehicleId,
                        ServicePartnerId = vehicle.ServicePartnerId,
                        VehicleNumber = vehicle.VehicleNumber
                    }).ToList();

                foreach (var vehicle in vehicleDetailsListFinal)
                {
                    DateTime currentDate = _currentDatetime.Date;
                    DateTime nextDate = currentDate.AddDays(1);

                    TblDriverDetail? tblDriverDetail = _context.TblDriverDetails
                       .Include(x => x.Driver).ThenInclude(x => x.City)
                       .FirstOrDefault(x =>
                           x.IsActive == true &&
                           x.VehicleId == vehicle.VehicleId &&
                           x.ServicePartnerId == vehicle.ServicePartnerId &&
                           (isJourneyPlannedForToday ? x.JourneyPlanDate.Value.Date == currentDate : x.JourneyPlanDate.Value.Date == nextDate)
                       );

                    if (tblDriverDetail != null)
                    {
                        vehicle.DriverDetail = _mapper.Map<TblDriverDetail, DriverResponseViewModal>(tblDriverDetail);
                        vehicle.DriverDetail.cityName = tblDriverDetail.Driver.City.Name;
                        vehicle.DriverDetail.CityId = tblDriverDetail.Driver.CityId;

                        if (!string.IsNullOrWhiteSpace(tblDriverDetail.Driver.DriverLicenseImage))
                        {
                            vehicle.DriverDetail.DriverlicenseImage = _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail.Driver.DriverLicenseImage;
                        }

                        if (!string.IsNullOrWhiteSpace(tblDriverDetail.Driver.ProfilePicture))
                        {
                            vehicle.DriverDetail.ProfilePicture = _baseConfig.Value.PostERPImagePath + _baseConfig.Value.ProfilePicture + tblDriverDetail.Driver.ProfilePicture;
                        }

                        int status = (int)OrderStatusEnum.VehicleAssignbyServicePartner;
                        int count = _logisticsRepository.GetDriverAssignOrderCount(tblDriverDetail.DriverDetailsId, status, lgcId);

                        if (count < _baseConfig.Value.DriverOrderCount)
                        {
                            vehicle.AcceptedOrderCount = count;
                            vehicle.MaxAcceptableOrderCount = (int)_baseConfig.Value.DriverOrderCount;
                        }
                        else
                        {
                            vehicle.AcceptedOrderCount = 0;
                            vehicle.MaxAcceptableOrderCount = (int)_baseConfig.Value.DriverOrderCount;
                        }
                    }
                    else
                    {
                        vehicle.AcceptedOrderCount = 0;
                        vehicle.MaxAcceptableOrderCount = (int)_baseConfig.Value.DriverOrderCount;
                    }
                }

                if (vehicleDetailsListFinal.Count > 0)
                {
                    responseMessage.Data = vehicleDetailsListFinal;
                    responseMessage.message = "Success";
                    responseMessage.Status = true;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                }
                else
                {
                    responseMessage.message = "No Records found";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("DriverDetailsManager", "GetNumberofVehicle", ex);
            }

            return responseMessage;
        }
        #endregion

        #region DriverDetails By DriverDetails userId
        /// <summary>
        /// Get driver Details by userId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DriverResponseViewModal GetDriverDetailsById(int id)
        {
            DriverResponseViewModal? driverDetails = null;
            TblDriverList? tblDriverList = null;
            try
            {
                if (id > 0)
                {
                    tblDriverList = _driverListRepository.GetSingle(x => x.IsActive == true && x.UserId == id);
                    if (tblDriverList != null)
                    {
                        driverDetails = new DriverResponseViewModal();
                        driverDetails = _mapper.Map<TblDriverList, DriverResponseViewModal>(tblDriverList);

                        UpdateDriverImagePaths(driverDetails);
                        return driverDetails;
                    }
                    else
                    {
                        return driverDetails;
                    }
                }
                else
                {
                    return driverDetails;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DriverDetailsManager", "GetDriverDetailsById", ex);
            }

            return driverDetails;
        }
        private void UpdateDriverImagePaths(DriverResponseViewModal model)
        {

            if (!string.IsNullOrEmpty(model.DriverlicenseImage))
            {
                model.DriverlicenseImage = GetImagePath(_baseConfig.Value.DriverlicenseImage, model.DriverlicenseImage);
            }

            //if (!string.IsNullOrEmpty(model.VehiclefitnessCertificate))
            //{
            //    model.VehiclefitnessCertificate = GetImagePath(_baseConfig.Value.VehiclefitnessCertificate, model.VehiclefitnessCertificate);
            //}

            //if (!string.IsNullOrEmpty(model.VehicleInsuranceCertificate))
            //{
            //    model.VehicleInsuranceCertificate = GetImagePath(_baseConfig.Value.VehicleInsuranceCertificate, model.VehicleInsuranceCertificate);
            //}
            //if (!string.IsNullOrEmpty(model.VehicleRcCertificate))
            //{
            //    model.VehicleRcCertificate = GetImagePath(_baseConfig.Value.VehicleRcCertificate, model.VehicleRcCertificate);
            //}
            if (!string.IsNullOrEmpty(model.ProfilePicture))
            {
                model.ProfilePicture = GetImagePath(_baseConfig.Value.ProfilePicture, model.ProfilePicture);
            }
        }
        private string GetImagePath(string basePath, string imageName)
        {
            return string.IsNullOrEmpty(imageName) ? string.Empty : $"{_baseConfig.Value.PostERPImagePath}{basePath}{imageName}";
        }
        #endregion

        #region GetOrderListById  assign to ServicePartner
        /// <summary>
        /// GetOrderListById  assign to ServicePartner
        /// added by ashwin
        /// </summary>
        /// <param name="id"></param>
        /// <returns>responseResult</returns>
        public ResponseResult GetAssignOrderListByIdDriverDetailsId(int DriverId, bool isJourneyPlannedForToday)
        {
            ResponseResult responseResult = new ResponseResult();

            try
            {
                if (DriverId <= 0)
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    responseResult.message = "UserId not found";
                    return responseResult;
                }
                // Check if the driver is already associated with another vehicle for the same day
                var currentDate = _currentDatetime.Date;
                var nextDate = currentDate.AddDays(1);

                TblDriverDetail? existingDriverDetail = _context.TblDriverDetails
 .Include(x => x.Driver).ThenInclude(x => x.City)
 .FirstOrDefault(x =>
 x.IsActive == true &&
 x.DriverId == DriverId &&
     (isJourneyPlannedForToday ? x.JourneyPlanDate.Value.Date == currentDate : x.JourneyPlanDate.Value.Date == nextDate)
 );
                if (existingDriverDetail != null)
                {
                    var DriverDetailsId = existingDriverDetail.DriverDetailsId;
                    List<TblLogistic> tblLogisticObj = _logisticsRepository.GetList(x => x.IsActive == true && x.DriverDetailsId == DriverDetailsId && x.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner)).ToList();

                    if (tblLogisticObj == null || tblLogisticObj.Count == 0)
                    {
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.OK;
                        responseResult.message = "No data found";
                        return responseResult;
                    }

                    List<AllAssignOrderlistByVehicleViewModel> orderListsViewModal = new List<AllAssignOrderlistByVehicleViewModel>();

                    foreach (var item in tblLogisticObj)
                    {
                        TblLogistic tbllogistic = _logisticsRepository.GetExchangeDetailsByRegdno(item.RegdNo);
                        TblWalletTransaction tblWalletTransaction = _logisticsRepository.GetEvcDetailsByOrdertranshId((int)item.OrderTransId);

                        if (tbllogistic != null)
                        {
                            if (tbllogistic != null)
                            {
                                AllAssignOrderlistByVehicleViewModel orderDetail = new AllAssignOrderlistByVehicleViewModel
                                {
                                    OrderTransId = tbllogistic.OrderTransId,
                                    RegdNo = tbllogistic.OrderTrans?.RegdNo,
                                    TicketNumber = tbllogistic.TicketNumber,
                                    PickupScheduleDate = tbllogistic.PickupScheduleDate.ToString(),
                                    //JourneyPlanDate = _currentDatetime.Date.AddDays(1),
                                    DriverId = (int)tbllogistic.DriverDetailsId,
                                    servicePartnerId = (int)tbllogistic.ServicePartnerId
                                };

                                if (tbllogistic.OrderTrans?.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                                {
                                    orderDetail.ProductCategory = tbllogistic.OrderTrans?.Exchange?.ProductType?.ProductCat?.Description;
                                    orderDetail.ProductType = tbllogistic.OrderTrans?.Exchange?.ProductType?.DescriptionForAbb;
                                    orderDetail.CustomerName = $"{tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.FirstName} {tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.LastName}";
                                    orderDetail.FirstName = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.FirstName;
                                    orderDetail.LastName = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.LastName;
                                    orderDetail.Email = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.Email;
                                    orderDetail.City = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.City;
                                    orderDetail.ZipCode = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.ZipCode;
                                    orderDetail.Address1 = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.Address1;
                                    orderDetail.Address2 = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.Address2 ?? "";
                                    orderDetail.PhoneNumber = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.PhoneNumber;
                                    orderDetail.State = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.State;
                                }

                                if (tbllogistic.OrderTrans?.OrderType == Convert.ToInt32(OrderTypeEnum.ABB))
                                {
                                    orderDetail.ProductCategory = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.NewProductCategory?.Description;
                                    orderDetail.ProductType = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation?.Description;
                                    orderDetail.CustomerName = $"{tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustFirstName} {tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustLastName}";
                                    orderDetail.FirstName = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustFirstName;
                                    orderDetail.LastName = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustLastName;
                                    orderDetail.Email = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustEmail;
                                    orderDetail.City = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustCity;
                                    orderDetail.ZipCode = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustPinCode;
                                    orderDetail.Address1 = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustAddress1;
                                    orderDetail.Address2 = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustAddress2 ?? "";
                                    orderDetail.PhoneNumber = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustMobile;
                                    orderDetail.State = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustState;
                                }


                                if (tblWalletTransaction != null && tblWalletTransaction.Evcregistration != null)
                                {
                                    orderDetail.EvcData = new EVCResellerModel
                                    {
                                        EvcregistrationId = tblWalletTransaction.Evcregistration.EvcregistrationId,
                                        EvcPartnerId = tblWalletTransaction.Evcpartner.EvcPartnerId,
                                        EVCStoreCode = tblWalletTransaction.Evcpartner.EvcStoreCode,
                                        BussinessName = tblWalletTransaction.Evcregistration.BussinessName,
                                        ContactPerson = tblWalletTransaction.Evcregistration.ContactPerson,
                                        EvcmobileNumber = tblWalletTransaction.Evcpartner.ContactNumber,
                                        AlternateMobileNumber = tblWalletTransaction.Evcregistration.EvcmobileNumber ?? "",
                                        EmailId = tblWalletTransaction.Evcpartner.EmailId,
                                        RegdAddressLine1 = tblWalletTransaction.Evcpartner.Address1,
                                        RegdAddressLine2 = tblWalletTransaction.Evcpartner.Address2,
                                        City = tblWalletTransaction.Evcpartner.City?.Name,
                                        State = tblWalletTransaction.Evcpartner.State?.Name,
                                        PinCode = tblWalletTransaction.Evcpartner.PinCode
                                    };

                                }
                                if (tbllogistic?.DriverDetailsId > 0)
                                {
                                    TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetDriverDetailsById((int)tbllogistic.DriverDetailsId);

                                    orderDetail.DriverData = new DriverDetailsModel
                                    {

                                       
                                        DriverDetailsId = tblDriverDetail?.DriverDetailsId,
                                        DriverName = tblDriverDetail?.Driver ==null? tblDriverDetail?.DriverName: tblDriverDetail?.Driver?.DriverName,
                                        DriverPhoneNumber = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverPhoneNumber : tblDriverDetail?.Driver?.DriverPhoneNumber,
                                        VehicleNumber = tblDriverDetail?.Vehicle == null ? tblDriverDetail?.VehicleNumber : tblDriverDetail?.Vehicle?.VehicleNumber,
                                        City = tblDriverDetail?.Driver == null ? tblDriverDetail?.City : tblDriverDetail?.Driver?.City?.Name,
                                        VehicleRcNumber = tblDriverDetail?.Vehicle == null ? tblDriverDetail?.VehicleRcNumber : tblDriverDetail?.Vehicle?.VehicleRcNumber,
                                        DriverlicenseNumber = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverlicenseNumber : tblDriverDetail?.Driver?.DriverLicenseNumber,
                                        CityId = tblDriverDetail?.Driver == null ? tblDriverDetail?.CityId : tblDriverDetail?.Driver?.CityId,
                                        ServicePartnerId = tblDriverDetail?.ServicePartnerId,
                                        CreatedDate = tblDriverDetail?.CreatedDate,
                                        ModifiedDate = tblDriverDetail?.ModifiedDate,
                                        DriverId = tblDriverDetail?.DriverId,
                                        VehicleId = tblDriverDetail?.VehicleId,
                                        JourneyPlannedDate = (tblDriverDetail?.JourneyPlanDate!=null? (DateTime)tblDriverDetail?.JourneyPlanDate:DateTime.MinValue),
                                        ProfilePicture = tblDriverDetail?.Driver!=null?_baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Driver?.ProfilePicture:string.Empty,
                                        DriverlicenseImage = tblDriverDetail?.Driver!=null? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Driver?.DriverLicenseImage:string.Empty,
                                        VehicleInsuranceCertificate = tblDriverDetail?.Vehicle!=null?_baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehicleInsuranceCertificate:string.Empty,
                                        VehiclefitnessCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehiclefitnessCertificate:string.Empty,
                                        VehicleRcCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehicleRcCertificate:string.Empty,
                                    
                                    };
                                }
                                orderListsViewModal.Add(orderDetail);
                            }
                        }
                    }

                    if (orderListsViewModal.Count > 0)
                    {
                        AllAssignOrderlistByVehicle allOrderList = new AllAssignOrderlistByVehicle
                        {
                            AllOrderlistViewModels = orderListsViewModal
                        };

                        return new ResponseResult
                        {
                            Status = true,
                            Status_Code = HttpStatusCode.OK,
                            Data = allOrderList,
                            message = "Success"
                        };
                    }
                    else
                    {
                        return new ResponseResult
                        {
                            Status = false,
                            Status_Code = HttpStatusCode.BadRequest,
                            message = "No data found"
                        };
                    }
                }
                else
                {
                    return new ResponseResult
                    {
                        Status = false,
                        Status_Code = HttpStatusCode.OK,
                        message = "Driver data not found"
                    };
                }
            }

            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DriverDetailsManager", "GetOrderListById", ex);

                return new ResponseResult
                {
                    Status = false,
                    Status_Code = HttpStatusCode.InternalServerError,
                    message = ex.Message
                };
            }
        }

        #endregion

        #region GetAcceptOrderListByDriverId  Accept Order by Driver
        /// <summary>
        /// GetAcceptOrderListByDriverId  Accept Order by Driver
        /// added by Priyanshi
        /// </summary>
        /// <param name="DriverDetailsId"></param>
        /// <param name="journeyDate"></param>
        /// <returns>responseResult</returns>
        public ResponseResult GetAcceptOrderListByDriverId(int DriverId, DateTime journeyDate, int StatusId)
        {
            ResponseResult responseResult = new ResponseResult();
            List<TblVehicleJourneyTrackingDetail> tblVehicleJourneyTrackingDetails = new List<TblVehicleJourneyTrackingDetail>();
            List<TblVehicleJourneyTrackingDetail> tblVehicleJourneyTrackingDetails1 = new List<TblVehicleJourneyTrackingDetail>();
            TblVehicleJourneyTracking tblVehicleJourneyTracking = new TblVehicleJourneyTracking();
            // journeyDate = Convert.ToDateTime(journeyDate).AddMinutes(-1).AddSeconds(-1);
            //string Date1 = journeyDate.ToString();
            //Date = Date1.Split()[0];
            journeyDate = journeyDate.Date;

            try
            {
                if (DriverId <= 0)
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.OK;
                    responseResult.message = "UserId not found";
                    return responseResult;
                }

                TblDriverDetail? existingDriverDetail = _context.TblDriverDetails
 .Include(x => x.Driver).ThenInclude(x => x.City)
 .FirstOrDefault(x =>
 x.IsActive == true &&x.DriverId == DriverId &&(x.JourneyPlanDate.Value.Date == journeyDate.Date));
                if (existingDriverDetail != null)
                {
                    var DriverDetailsId = existingDriverDetail.DriverDetailsId;

                    if (journeyDate == DateTime.MinValue)
                    {
                        tblVehicleJourneyTrackingDetails = _journeyTrackingDetailsRepository.GetList(x => x.IsActive == true && x.DriverId == DriverDetailsId && x.StatusId == StatusId && x.JourneyPlanDate.Value.Date == _currentDatetime.Date /*(x.JourneyPlanDate != null && Convert.ToDateTime(x.JourneyPlanDate).ToShortDateString() == journeyDate.ToShortDateString())*/).ToList();
                    }
                    else
                    {
                        tblVehicleJourneyTrackingDetails = _journeyTrackingDetailsRepository.GetList(x => x.IsActive == true && x.DriverId == DriverDetailsId && x.StatusId == StatusId && x.JourneyPlanDate.Value.Date == journeyDate.Date).ToList();
                    }
                    if (tblVehicleJourneyTrackingDetails == null || tblVehicleJourneyTrackingDetails.Count == 0)
                    {
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.OK;
                        responseResult.message = "Order Not found";
                        return responseResult;
                    }

                    List<AllAcceptOrderlistByVehicleViewModel> orderListsViewModal = new List<AllAcceptOrderlistByVehicleViewModel>();
                    List<AllEVCDetails> EVCListViewModal = new List<AllEVCDetails>();

                    foreach (var item in tblVehicleJourneyTrackingDetails)
                    {
                        TblLogistic tbllogistic = _logisticsRepository.GetExchangeDetailsByOID((int)item.OrderTransId);
                        TblWalletTransaction tblWalletTransaction = _logisticsRepository.GetEvcDetailsByOrdertranshId((int)item.OrderTransId);

                        if (tbllogistic != null)
                        {
                            AllAcceptOrderlistByVehicleViewModel orderDetail = new AllAcceptOrderlistByVehicleViewModel
                            {
                                OrderTransId = tbllogistic.OrderTransId,
                                RegdNo = tbllogistic.OrderTrans?.RegdNo,
                                TicketNumber = tbllogistic.TicketNumber,
                                PickupScheduleDate = tbllogistic.PickupScheduleDate.ToString(),
                                JourneyPlanDate = (DateTime)item.JourneyPlanDate,
                                DriverDetailsId = (int)tbllogistic.DriverDetailsId,
                                servicePartnerId = (int)(tbllogistic.ServicePartnerId ?? 0),
                                AmtPaybleThroughLGC = (int)(tbllogistic.AmtPaybleThroughLgc ?? 0),
                                TrackingId = item.TrackingId ?? 0,
                            };
                            if (tbllogistic.OrderTrans?.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                            {
                                orderDetail.ProductCategory = tbllogistic.OrderTrans?.Exchange?.ProductType?.ProductCat?.Description;
                                orderDetail.ProductType = tbllogistic.OrderTrans?.Exchange?.ProductType?.DescriptionForAbb;
                                orderDetail.IsDefferedSettlement = tbllogistic.OrderTrans?.Exchange?.IsDefferedSettlement == true ? true : false;
                                orderDetail.pickupDetails = new PickupDetails();
                                orderDetail.pickupDetails.CustomerName = $"{tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.FirstName} {tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.LastName}";
                                orderDetail.pickupDetails.FirstName = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.FirstName;
                                orderDetail.pickupDetails.LastName = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.LastName;
                                orderDetail.pickupDetails.Email = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.Email;
                                orderDetail.pickupDetails.City = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.City;
                                orderDetail.pickupDetails.ZipCode = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.ZipCode;
                                orderDetail.pickupDetails.Address1 = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.Address1;
                                orderDetail.pickupDetails.Address2 = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.Address2 ?? "";
                                orderDetail.pickupDetails.PhoneNumber = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.PhoneNumber;
                                orderDetail.pickupDetails.State = tbllogistic.OrderTrans?.Exchange?.CustomerDetails?.State;
                                orderDetail.pickupDetails.PickupLatt = item.PickupLatt;
                                orderDetail.pickupDetails.PickupLong = item.PickupLong;
                            }

                            if (tbllogistic.OrderTrans?.OrderType == Convert.ToInt32(OrderTypeEnum.ABB))
                            {
                                orderDetail.ProductCategory = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.NewProductCategory?.Description;
                                orderDetail.ProductType = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation?.Description;
                                orderDetail.IsDefferedSettlement = tbllogistic.OrderTrans?.Abbredemption?.IsDefferedSettelment == true ? true : false;
                                orderDetail.pickupDetails = new PickupDetails();
                                orderDetail.pickupDetails.CustomerName = $"{tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustFirstName} {tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustLastName}";
                                orderDetail.pickupDetails.FirstName = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustFirstName;
                                orderDetail.pickupDetails.LastName = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustLastName;
                                orderDetail.pickupDetails.Email = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustEmail;
                                orderDetail.pickupDetails.City = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustCity;
                                orderDetail.pickupDetails.ZipCode = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustPinCode;
                                orderDetail.pickupDetails.Address1 = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustAddress1;
                                orderDetail.pickupDetails.Address2 = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustAddress2 ?? "";
                                orderDetail.pickupDetails.PhoneNumber = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustMobile;
                                orderDetail.pickupDetails.State = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.CustState;
                                orderDetail.pickupDetails.PickupLatt = item.PickupLatt;
                                orderDetail.pickupDetails.PickupLong = item.PickupLong;
                            }

                            if (tblWalletTransaction != null && tblWalletTransaction.Evcregistration != null)
                            {
                                orderDetail.dropDetails = new DropDetails
                                {
                                    EvcregistrationId = tblWalletTransaction.Evcregistration.EvcregistrationId,
                                    EvcPartnerId = tblWalletTransaction.Evcpartner.EvcPartnerId,
                                    EvcStoreCode = tblWalletTransaction.Evcpartner.EvcStoreCode,
                                    BussinessName = tblWalletTransaction.Evcregistration.BussinessName,
                                    ContactPerson = tblWalletTransaction.Evcregistration.ContactPerson,
                                    EvcmobileNumber = tblWalletTransaction.Evcpartner.ContactNumber,
                                    AlternateMobileNumber = tblWalletTransaction.Evcregistration.EvcmobileNumber ?? "",
                                    EmailId = tblWalletTransaction.Evcpartner.EmailId,
                                    RegdAddressLine1 = tblWalletTransaction.Evcpartner.Address1,
                                    RegdAddressLine2 = tblWalletTransaction.Evcpartner.Address2,
                                    City = tblWalletTransaction.Evcpartner.City?.Name,
                                    State = tblWalletTransaction.Evcpartner.State?.Name,
                                    PinCode = tblWalletTransaction.Evcpartner.PinCode,
                                    DropLatt = item.DropLatt,
                                    DropLong = item.DropLong
                                };

                            }

                            orderListsViewModal.Add(orderDetail);
                        }
                    }
                    if (journeyDate != DateTime.MinValue)
                    {
                        tblVehicleJourneyTrackingDetails1 = _journeyTrackingDetailsRepository.GetList(x => x.IsActive == true && x.DriverId == DriverDetailsId && (x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderAcceptedbyVehicle) || x.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup)) && (x.JourneyPlanDate != null && Convert.ToDateTime(x.JourneyPlanDate).ToShortDateString() == journeyDate.ToShortDateString())).ToList();

                        tblVehicleJourneyTracking = _journeyTrackingRepository.GetSingle(x => x.IsActive == true && x.DriverId == DriverDetailsId && (x.JourneyPlanDate != null && Convert.ToDateTime(x.JourneyPlanDate).ToShortDateString() == journeyDate.ToShortDateString()));
                    }
                    else
                    {
                        tblVehicleJourneyTrackingDetails1 = _journeyTrackingDetailsRepository.GetList(x => x.IsActive == true && x.DriverId == DriverDetailsId && (x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderAcceptedbyVehicle) || x.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup)) && x.JourneyPlanDate == _currentDatetime.Date).ToList();

                        tblVehicleJourneyTracking = _journeyTrackingRepository.GetSingle(x => x.IsActive == true && x.DriverId == DriverDetailsId && x.JourneyPlanDate == _currentDatetime.Date);
                    }

                    int TotalPickedUpCount = tblVehicleJourneyTrackingDetails1.Where(x => x.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup)).Count();

                    var commonEVCGroups = tblVehicleJourneyTrackingDetails1
       .GroupBy(x => x.Evcid)
       .Select(g => g.Key) // Select only the distinct common EVC IDs
       .Distinct();
                    foreach (var item1 in commonEVCGroups)
                    {
                        var firstGroup = tblVehicleJourneyTrackingDetails1
                            .Where(x => x.Evcid == item1)
                            .FirstOrDefault();

                        if (firstGroup != null)
                        {
                            TblLogistic tblLogistic = _logisticsRepository.GetExchangeDetailsByOID((int)firstGroup.OrderTransId);
                            TblWalletTransaction tblWalletTransaction = _logisticsRepository.GetEvcDetailsByOrdertranshId((int)firstGroup.OrderTransId);


                            if (tblLogistic != null && tblWalletTransaction != null && tblWalletTransaction.Evcregistration != null)
                            {
                                AllEVCDetails eVCDetails = new AllEVCDetails
                                {
                                    EvcregistrationId = tblWalletTransaction.Evcregistration.EvcregistrationId,
                                    BussinessName = tblWalletTransaction.Evcregistration.BussinessName,
                                    ContactPerson = tblWalletTransaction.Evcregistration.ContactPerson,
                                    EvcmobileNumber = tblWalletTransaction.Evcpartner.ContactNumber,
                                    AlternateMobileNumber = tblWalletTransaction.Evcregistration.EvcmobileNumber ?? "",
                                    EmailId = tblWalletTransaction.Evcpartner.EmailId,
                                    RegdAddressLine1 = tblWalletTransaction.Evcpartner.Address1,
                                    RegdAddressLine2 = tblWalletTransaction.Evcpartner.Address2,
                                    City = tblWalletTransaction.Evcpartner.City?.Name ?? "",
                                    State = tblWalletTransaction.Evcpartner.State?.Name ?? "",
                                    PinCode = tblWalletTransaction.Evcpartner.PinCode,
                                    DropLatt = firstGroup.DropLatt,
                                    DropLong = firstGroup.DropLong
                                };
                                EVCListViewModal.Add(eVCDetails);
                            }
                        }
                    }

                    if (orderListsViewModal.Count > 0)
                    {
                        AllAcceptOrderlistByVehicle allOrderList = new AllAcceptOrderlistByVehicle
                        {
                            TotalOrderCount = tblVehicleJourneyTrackingDetails1.Count > 0 ? tblVehicleJourneyTrackingDetails1.Count : 0,
                            TotalPickedUpCount = TotalPickedUpCount > 0 ? TotalPickedUpCount : 0,
                            StartJourneyTime = (DateTime?)tblVehicleJourneyTracking?.JourneyStartDatetime,
                            AllAcceptOrderlistViewModels = orderListsViewModal,
                            AllEVCDetail = EVCListViewModal
                        };

                        return new ResponseResult
                        {
                            Status = true,
                            Status_Code = HttpStatusCode.OK,
                            Data = allOrderList,
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
                else
                {
                    return new ResponseResult
                    {
                        Status = false,
                        Status_Code = HttpStatusCode.OK,
                        message = "Driver data not found"
                    };
                }
            }

            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DriverDetailsManager", "GetOrderListById", ex);

                return new ResponseResult
                {
                    Status = false,
                    Status_Code = HttpStatusCode.InternalServerError,
                    message = ex.Message
                };
            }
        }
        #endregion

        #region Order Accept Order by Vehicle
        /// <summary>
        /// Api method for Accept Order by Vehicle
        /// Added by Priyanshi
        /// </summary>
        /// <param name="">OrdertransId</param>
        /// <param name="">DriverDetailId</param>       
        /// <returns></returns>
        public ResponseResult AcceptOrderbyVehicle(AcceptOrderbyVehicleRequest request)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            TblAbbredemption? tblAbbredemption = null;
            TblExchangeAbbstatusHistory? tblExchangeAbbstatusHistory = null;
            int TrackingId = 0;
            try
            {
                if (request == null || request.DriverDetailsId <= 0 || request.OrdertransId <= 0)
                {
                    responseMessage.message = "Not Valid login credential";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }

                TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetSingle(x => x.IsActive == true && x.DriverDetailsId == request.DriverDetailsId);
                if (tblDriverDetail == null)
                {
                    responseMessage.message = "Not Valid login credential";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }

                TblLogistic tblLogistic = _logisticsRepository.GetSingle(x => x.OrderTransId == request.OrdertransId && x.IsActive == true && x.DriverDetailsId == request.DriverDetailsId && x.StatusId == (int)OrderStatusEnum.VehicleAssignbyServicePartner);
                if (tblLogistic == null)
                {
                    responseMessage.message = "Invalid RegdNo";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }

                TblOrderTran tblOrderTran = _orderTransRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == tblLogistic.OrderTransId);
                if (tblOrderTran == null || tblOrderTran.OrderTransId <= 0)
                {
                    responseMessage.message = "OrderTransId Not Found for RegdNo";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }

                int OrderStatusId = (int)OrderStatusEnum.OrderAcceptedbyVehicle;
                #region Create/update in journeyTrackingRepository
                if (request.ServicePartnerId > 0 && request.JourneyPlanDate != null)
                {
                    TblVehicleJourneyTracking tblVehicleJourneyTracking = _journeyTrackingRepository.GetSingle(x => x.DriverId == request.DriverDetailsId && x.ServicePartnerId == request.ServicePartnerId && x.JourneyPlanDate == request.JourneyPlanDate && x.IsActive == true);

                    if (tblVehicleJourneyTracking != null)
                    {
                        tblVehicleJourneyTracking.ModifiedDate = _currentDatetime;
                        tblVehicleJourneyTracking.ModifiedBy = tblDriverDetail.UserId;
                        _journeyTrackingRepository.Update(tblVehicleJourneyTracking);
                        _journeyTrackingRepository.SaveChanges();
                        TrackingId = tblVehicleJourneyTracking.TrackingId;

                    }
                    else
                    {
                        TblVehicleJourneyTracking tblVehicleJourneyTracking1 = new TblVehicleJourneyTracking();
                        tblVehicleJourneyTracking1.ServicePartnerId = request.ServicePartnerId;
                        tblVehicleJourneyTracking1.DriverId = request.DriverDetailsId;
                        tblVehicleJourneyTracking1.JourneyPlanDate = request.JourneyPlanDate;
                        tblVehicleJourneyTracking1.CreatedBy = tblDriverDetail.UserId;
                        tblVehicleJourneyTracking1.CreatedDate = _currentDatetime;
                        tblVehicleJourneyTracking1.IsActive = true;
                        _journeyTrackingRepository.Create(tblVehicleJourneyTracking1);
                        _journeyTrackingRepository.SaveChanges();
                        TrackingId = tblVehicleJourneyTracking1.TrackingId;
                    }
                }
                #endregion

                #region Create in JourneyTrackingDetailsRepository
                if (request.JourneyPlanDate != null && request.PickupLatt != null && request.PickupLong != null && request.DropLatt != null && request.DropLong != null)
                {
                    TblVehicleJourneyTrackingDetail tblVehicleJourneyTrackingDetail = _journeyTrackingDetailsRepository.GetSingle(x => x.OrderTransId == request.OrdertransId && x.DriverId == request.DriverDetailsId && x.ServicePartnerId == request.ServicePartnerId && x.JourneyPlanDate == request.JourneyPlanDate && x.IsActive == true);

                    if (tblVehicleJourneyTrackingDetail != null)
                    {
                        tblVehicleJourneyTrackingDetail.ModifiedDate = _currentDatetime;
                        tblVehicleJourneyTrackingDetail.ModifiedBy = tblDriverDetail.UserId;
                        _journeyTrackingDetailsRepository.Update(tblVehicleJourneyTrackingDetail);
                        _journeyTrackingDetailsRepository.SaveChanges();


                    }
                    else
                    {
                        TblVehicleJourneyTrackingDetail tblVehicleJourneyTrackingDetail1 = new TblVehicleJourneyTrackingDetail();
                        tblVehicleJourneyTrackingDetail1.ServicePartnerId = request.ServicePartnerId;
                        tblVehicleJourneyTrackingDetail1.DriverId = request.DriverDetailsId;
                        tblVehicleJourneyTrackingDetail1.JourneyPlanDate = request.JourneyPlanDate;
                        tblVehicleJourneyTrackingDetail1.CreatedBy = tblDriverDetail.UserId;
                        tblVehicleJourneyTrackingDetail1.CreatedDate = _currentDatetime;
                        tblVehicleJourneyTrackingDetail1.IsActive = true;
                        tblVehicleJourneyTrackingDetail1.TrackingId = TrackingId;
                        tblVehicleJourneyTrackingDetail1.Evcid = request.EVCId;
                        tblVehicleJourneyTrackingDetail1.EvcpartnerId = request.EvcPartnerId;
                        tblVehicleJourneyTrackingDetail1.OrderTransId = request.OrdertransId;
                        tblVehicleJourneyTrackingDetail1.JourneyPlanDate = request.JourneyPlanDate;
                        tblVehicleJourneyTrackingDetail1.PickupLatt = request.PickupLatt;
                        tblVehicleJourneyTrackingDetail1.PickupLong = request.PickupLong;
                        tblVehicleJourneyTrackingDetail1.DropLatt = request.DropLatt;
                        tblVehicleJourneyTrackingDetail1.DropLong = request.DropLong;
                        tblVehicleJourneyTrackingDetail1.StatusId = OrderStatusId;

                        _journeyTrackingDetailsRepository.Create(tblVehicleJourneyTrackingDetail1);
                        _journeyTrackingDetailsRepository.SaveChanges();

                        bool pickupflag = _commonManager.CalculateDriverIncentive((int)request.OrdertransId);


                    }
                }
                #endregion

                #region update statusId in tblOrderTrans
                var Ordertype = _orderTransRepository.UpdateTransRecordStatus(tblOrderTran.OrderTransId, OrderStatusId, tblDriverDetail.UserId);
                //tblOrderTran.StatusId = OrderStatusId;
                //tblOrderTran.ModifiedBy = tblDriverDetail.UserId;
                //tblOrderTran.ModifiedDate = _currentDatetime;
                //_orderTransRepository.Update(tblOrderTran);
                //_orderTransRepository.SaveChanges();
                #endregion

                #region insert/update into tblOrderLgc
                TblOrderLgc tblOrderLgc = _orderLGCRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == tblOrderTran.OrderTransId);
                if (tblOrderLgc != null && tblOrderLgc.OrderLgcid > 0)
                {
                    tblOrderLgc.StatusId = OrderStatusId;
                    tblOrderLgc.ModifiedBy = tblDriverDetail.UserId;
                    tblOrderLgc.ModifiedDate = _currentDatetime;
                    tblOrderLgc.LogisticId = tblLogistic.LogisticId;
                    _orderLGCRepository.Update(tblOrderLgc);
                    _orderLGCRepository.SaveChanges();
                }
                #endregion

                #region Update StatusId in Tbl WalletTransection 
                TblWalletTransaction tblWalletTransaction = _walletTransactionRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == request.OrdertransId);
                if (tblWalletTransaction != null)
                {
                    tblWalletTransaction.StatusId = OrderStatusId.ToString();
                    tblWalletTransaction.ModifiedBy = tblDriverDetail.UserId;
                    tblWalletTransaction.ModifiedDate = _currentDatetime;
                    _walletTransactionRepository.Update(tblWalletTransaction);
                    _walletTransactionRepository.SaveChanges();
                }
                #endregion

                #region update statusid in tbllogistics
                tblLogistic.IsOrderAcceptedByDriver = true;
                tblLogistic.IsOrderRejectedByDriver = false;
                tblLogistic.StatusId = OrderStatusId;
                tblLogistic.IsActive = true;
                tblLogistic.Modifiedby = tblDriverDetail.UserId;
                tblLogistic.ModifiedDate = _currentDatetime;
                _logisticsRepository.Update(tblLogistic);
                _logisticsRepository.SaveChanges();
                #endregion

                if (tblOrderTran.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                {
                    #region update statusId in tblexchangeOrder
                    TblExchangeOrder tblExchangeOrder = _exchangeOrderRepository.GetSingle(x => x.IsActive == true && x.Id == tblOrderTran.ExchangeId);
                    if (tblExchangeOrder != null)
                    {
                        int Exresult = _exchangeOrderRepository.UpdateExchangeRecordStatus(tblOrderTran.RegdNo, OrderStatusId, tblDriverDetail.UserId);
                    }
                    #endregion

                    #region Insert into tblexchangeabbhistory
                    tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                    tblExchangeAbbstatusHistory.OrderType = (int)tblOrderTran.OrderType;
                    tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                    tblExchangeAbbstatusHistory.RegdNo = tblExchangeOrder.RegdNo;
                    tblExchangeAbbstatusHistory.ZohoSponsorId = tblExchangeOrder.ZohoSponsorOrderId ?? string.Empty;
                    tblExchangeAbbstatusHistory.CustId = tblExchangeOrder.CustomerDetailsId;
                    tblExchangeAbbstatusHistory.StatusId = OrderStatusId;
                    tblExchangeAbbstatusHistory.IsActive = true;
                    tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                    tblExchangeAbbstatusHistory.CreatedBy = tblDriverDetail.UserId;
                    tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                    tblExchangeAbbstatusHistory.ServicepartnerId = request.ServicePartnerId;
                    tblExchangeAbbstatusHistory.DriverDetailId = request.DriverDetailsId;


                    _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                    #endregion

                }
                if (tblOrderTran.OrderType == Convert.ToInt32(OrderTypeEnum.ABB))
                {
                    #region update statusId in tblAbbredemtion
                    tblAbbredemption = _abbRedemptionRepository.GetSingle(x => x.IsActive == true && x.RedemptionId == tblOrderTran.AbbredemptionId);
                    if (tblAbbredemption != null)
                    {
                        TblAbbredemption OtblAbbredemption = new TblAbbredemption();
                        OtblAbbredemption = _abbRedemptionRepository.UpdateABBOrderStatus(tblOrderTran.RegdNo, OrderStatusId, tblDriverDetail.UserId, null);
                    }
                    #endregion

                    #region Insert into tblexchangeabbhistory
                    tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                    tblExchangeAbbstatusHistory.OrderType = (int)tblOrderTran.OrderType;
                    //  tblExchangeAbbstatusHistory.SponsorOrderNumber = tblOrderTran.Abbredemption.SponsorOrderNumber;
                    tblExchangeAbbstatusHistory.RegdNo = tblOrderTran.RegdNo;
                    // tblExchangeAbbstatusHistory.ZohoSponsorId = tblOrderTran.Exchange.ZohoSponsorOrderId != null ? TblExchangeOrders.ZohoSponsorOrderId : string.Empty;
                    tblExchangeAbbstatusHistory.CustId = tblOrderTran?.Abbredemption?.CustomerDetailsId;
                    tblExchangeAbbstatusHistory.StatusId = OrderStatusId;
                    tblExchangeAbbstatusHistory.IsActive = true;
                    tblExchangeAbbstatusHistory.CreatedBy = tblDriverDetail.UserId;
                    tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                    tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                    tblExchangeAbbstatusHistory.ServicepartnerId = request.ServicePartnerId;
                    tblExchangeAbbstatusHistory.DriverDetailId = request.DriverDetailsId;

                    _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                    //_exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                    //_exchangeABBStatusHistoryRepository.SaveChanges();
                    #endregion
                }


                responseMessage.message = "Accept Order Successfully";
                responseMessage.Status = true;
                responseMessage.Status_Code = HttpStatusCode.OK;
                var Notification = _pushNotificationManager.SendNotification(tblLogistic.ServicePartnerId, tblLogistic.DriverDetailsId, EnumHelper.DescriptionAttr(NotificationEnum.OrderAcceptedbyDriver), null, null);
            }

            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("DriverDetailsManager", "AcceptOrderbyVehicle", ex);
            }
            return responseMessage;
        }
        #endregion

        #region Order Reject by Vehicle
        /// <summary>
        /// Api method for Reject Order by Vehicle
        /// Added by Priyanshi
        /// </summary>
        /// <param name="">OrdertransId</param>
        /// <param name="">DriverDetailId</param>
        /// <param name="">RejectComment</param>
        /// <returns></returns>
        public ResponseResult RejectOrderbyVehicle(int OrdertransId, int DriverDetailId, string RejectComment)
        {
            TblLogistic? tblLogistic = null;
            int OrderStatusId = Convert.ToInt32(OrderStatusEnum.OrderRejectedbyVehicle);
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            TblOrderLgc? tblOrderLgc = null;
            TblExchangeOrder? tblExchangeOrder = null;
            TblExchangeAbbstatusHistory? tblExchangeAbbstatusHistory = null;
            TblDriverDetail? tblDriverDetail = null;
            TblAbbredemption? tblAbbredemption = null;
            try
            {
                tblDriverDetail = _driverDetailsRepository.GetSingle(x => x.IsActive == true && x.DriverDetailsId == DriverDetailId);

                if (DriverDetailId > 0 && tblDriverDetail != null && OrdertransId > 0)
                {

                    tblLogistic = _logisticsRepository.GetSingle(x => x.OrderTransId == OrdertransId && x.IsActive == true && x.DriverDetailsId == DriverDetailId && x.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner));

                    if (tblLogistic != null)
                    {

                        TblOrderTran tblOrderTran = _orderTransRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == tblLogistic.OrderTransId);

                        if (tblOrderTran != null && tblOrderTran.OrderTransId > 0)
                        {
                            #region update statusId in tblOrderTrans
                            tblOrderTran.StatusId = OrderStatusId;
                            tblOrderTran.ModifiedBy = tblDriverDetail.UserId;
                            tblOrderTran.ModifiedDate = _currentDatetime;
                            _orderTransRepository.Update(tblOrderTran);
                            _orderTransRepository.SaveChanges();
                            #endregion

                            #region update statusid in tbllogistics
                            tblLogistic.IsOrderAcceptedByDriver = false;
                            tblLogistic.IsOrderRejectedByDriver = true;
                            tblLogistic.StatusId = OrderStatusId;
                            tblLogistic.IsActive = true;
                            tblLogistic.Modifiedby = tblDriverDetail.UserId;
                            tblLogistic.ModifiedDate = _currentDatetime;
                            _logisticsRepository.Update(tblLogistic);
                            _logisticsRepository.SaveChanges();
                            #endregion

                            #region insert/update into tblOrderLgc
                            tblOrderLgc = _orderLGCRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == tblOrderTran.OrderTransId);
                            if (tblOrderLgc != null && tblOrderLgc.OrderLgcid > 0)
                            {
                                tblOrderLgc.StatusId = OrderStatusId;
                                tblOrderLgc.ModifiedBy = tblDriverDetail.UserId;
                                tblOrderLgc.ModifiedDate = _currentDatetime;
                                tblOrderLgc.LogisticId = tblLogistic.LogisticId;
                                if (RejectComment.Length > 0)
                                {
                                    tblOrderLgc.Lgccomments = RejectComment;
                                }
                                _orderLGCRepository.Update(tblOrderLgc);
                                _orderLGCRepository.SaveChanges();
                            }

                            #endregion

                            #region Update StatusId in Tbl WalletTransection 
                            TblWalletTransaction tblWalletTransaction = _walletTransactionRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == OrdertransId);
                            if (tblWalletTransaction != null)
                            {
                                tblWalletTransaction.StatusId = OrderStatusId.ToString();
                                tblWalletTransaction.ModifiedBy = tblDriverDetail.UserId;
                                tblWalletTransaction.ModifiedDate = _currentDatetime;
                                _walletTransactionRepository.Update(tblWalletTransaction);
                                _walletTransactionRepository.SaveChanges();
                            }
                            #endregion


                            if (tblOrderTran.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                            {
                                #region update statusId in tblexchangeOrder
                                tblExchangeOrder = _exchangeOrderRepository.GetSingle(x => x.IsActive == true && x.Id == tblOrderTran.ExchangeId);
                                if (tblExchangeOrder != null)
                                {
                                    tblExchangeOrder.StatusId = OrderStatusId;
                                    tblExchangeOrder.ModifiedBy = tblDriverDetail.UserId;
                                    tblExchangeOrder.ModifiedDate = _currentDatetime;
                                    _exchangeOrderRepository.Update(tblExchangeOrder);
                                    _exchangeOrderRepository.SaveChanges();
                                }
                                #endregion

                                #region Insert into tblexchangeabbhistory
                                tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                tblExchangeAbbstatusHistory.OrderType = 17;
                                tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                                tblExchangeAbbstatusHistory.RegdNo = tblExchangeOrder.RegdNo;
                                tblExchangeAbbstatusHistory.ZohoSponsorId = tblExchangeOrder.ZohoSponsorOrderId != null ? tblExchangeOrder.ZohoSponsorOrderId : string.Empty; ;
                                tblExchangeAbbstatusHistory.CustId = tblExchangeOrder.CustomerDetailsId;
                                tblExchangeAbbstatusHistory.StatusId = OrderStatusId;
                                tblExchangeAbbstatusHistory.IsActive = true;
                                tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                tblExchangeAbbstatusHistory.CreatedBy = tblDriverDetail.UserId;
                                tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                                tblExchangeAbbstatusHistory.Comment = RejectComment != null ? "TicketNo-" + tblLogistic.TicketNumber + "Comment -" + RejectComment : string.Empty;
                                tblExchangeAbbstatusHistory.DriverDetailId = DriverDetailId;

                                _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                                #endregion
                            }
                            if (tblOrderTran.OrderType == Convert.ToInt32(OrderTypeEnum.ABB))
                            {
                                #region update statusId in tblAbbredemtion
                                tblAbbredemption = _abbRedemptionRepository.GetSingle(x => x.IsActive == true && x.RedemptionId == tblOrderTran.AbbredemptionId);
                                if (tblAbbredemption != null)
                                {
                                    tblAbbredemption.StatusId = OrderStatusId;
                                    tblAbbredemption.ModifiedBy = tblDriverDetail.UserId;
                                    tblAbbredemption.ModifiedDate = _currentDatetime;
                                    _abbRedemptionRepository.Update(tblAbbredemption);
                                    _abbRedemptionRepository.SaveChanges();
                                }
                                #endregion

                                #region Insert into tblexchangeabbhistory
                                tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                tblExchangeAbbstatusHistory.OrderType = (int)tblOrderTran.OrderType;
                                //  tblExchangeAbbstatusHistory.SponsorOrderNumber = tblOrderTran.Abbredemption.SponsorOrderNumber;
                                tblExchangeAbbstatusHistory.RegdNo = tblOrderTran.RegdNo;
                                // tblExchangeAbbstatusHistory.ZohoSponsorId = tblOrderTran.Exchange.ZohoSponsorOrderId != null ? TblExchangeOrders.ZohoSponsorOrderId : string.Empty;
                                tblExchangeAbbstatusHistory.CustId = tblAbbredemption.CustomerDetailsId;
                                tblExchangeAbbstatusHistory.StatusId = OrderStatusId;
                                tblExchangeAbbstatusHistory.IsActive = true;
                                tblExchangeAbbstatusHistory.CreatedBy = tblDriverDetail.UserId;
                                tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                                tblExchangeAbbstatusHistory.ServicepartnerId = DriverDetailId;

                                _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                                //_exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                                //_exchangeABBStatusHistoryRepository.SaveChanges();
                                #endregion
                            }




                            responseMessage.message = "Reject Order Succesfully";
                            responseMessage.Status = true;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                            var Notification = _pushNotificationManager.SendNotification(tblLogistic.ServicePartnerId, tblLogistic.DriverDetailsId, EnumHelper.DescriptionAttr(NotificationEnum.OrderRejectedbyDriver), null, null);
                        }
                        else
                        {
                            responseMessage.message = "OrderTransId Not Found for RegdNo";
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {

                        responseMessage.message = "Invalid order ";
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;

                    }
                }
                else
                {
                    responseMessage.message = "Not Valid login credential";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                }

            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("DriverDetailsManager", "RejectOrderbyVehicle", ex);
            }
            return responseMessage;
        }
        #endregion

        #region Order StartVehicleJourney by Vehicle
        /// <summary>
        /// Api method for Order StartVehicleJourney by Vehicle
        /// Added by Priyanshi
        /// </summary>
        /// <param name="">StartVehicleJourney</param>           
        /// <returns></returns>
        public ResponseResult StartVehicleJourney(StartVehicleJourney request)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            int TrackingId = 0;
            try
            {
                if (request == null || request.DriverDetailsId <= 0 || request.ServicePartnerId <= 0)
                {
                    responseMessage.message = "Not Valid login credential";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }
                TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetSingle(x => x.IsActive == true && x.DriverDetailsId == request.DriverDetailsId);
                if (tblDriverDetail != null)
                {
                    #region Create/update in journeyTrackingRepository
                    if (request.ServicePartnerId > 0 && request.JourneyPlanDate != null)
                    {
                        TblVehicleJourneyTracking tblVehicleJourneyTracking = _journeyTrackingRepository.GetSingle(x => x.DriverId == request.DriverDetailsId && x.ServicePartnerId == request.ServicePartnerId && x.JourneyPlanDate.Value.Date == request.JourneyPlanDate.Value.Date && x.IsActive == true);
                        if (tblVehicleJourneyTracking != null)
                        {
                            if (tblVehicleJourneyTracking.CurrentVehicleLatt == null && tblVehicleJourneyTracking.CurrentVehicleLong == null && tblVehicleJourneyTracking.JourneyStartDatetime == null)
                            {
                                tblVehicleJourneyTracking.CurrentVehicleLatt = request.CurrentVehicleLatt;
                                tblVehicleJourneyTracking.CurrentVehicleLong = request.CurrentVehicleLong;
                                tblVehicleJourneyTracking.JourneyStartDatetime = _currentDatetime;
                                tblVehicleJourneyTracking.ModifiedDate = _currentDatetime;
                                tblVehicleJourneyTracking.ModifiedBy = tblDriverDetail.UserId;
                                _journeyTrackingRepository.Update(tblVehicleJourneyTracking);
                                _journeyTrackingRepository.SaveChanges();
                                TrackingId = tblVehicleJourneyTracking.TrackingId;

                                #region update TblVehicleJourneyTrackingDetail
                                List<TblVehicleJourneyTrackingDetail> tblVehicleJourneyTrackingDetail = _journeyTrackingDetailsRepository.GetList(x => x.DriverId == request.DriverDetailsId && x.ServicePartnerId == request.ServicePartnerId && x.JourneyPlanDate.Value.Date == request.JourneyPlanDate.Value.Date && x.IsActive == true && x.TrackingId == TrackingId).ToList();
                                if (tblVehicleJourneyTrackingDetail != null)
                                {
                                    foreach (var item in tblVehicleJourneyTrackingDetail)
                                    {
                                        TblVehicleJourneyTrackingDetail tblVehicleJourneyTrackingDetailItem = null;

                                        tblVehicleJourneyTrackingDetailItem = _journeyTrackingDetailsRepository.GetSingle(x => x.DriverId == request.DriverDetailsId && x.ServicePartnerId == request.ServicePartnerId && x.JourneyPlanDate.Value.Date == request.JourneyPlanDate.Value.Date && x.IsActive == true && x.TrackingId == TrackingId && x.TrackingDetailsId == item.TrackingDetailsId);

                                        if (tblVehicleJourneyTrackingDetailItem != null)
                                        {
                                            tblVehicleJourneyTrackingDetailItem.PickupStartDatetime = tblVehicleJourneyTracking.JourneyStartDatetime;
                                            tblVehicleJourneyTrackingDetailItem.ModifiedDate = _currentDatetime;
                                            tblVehicleJourneyTrackingDetailItem.ModifiedBy = tblDriverDetail.UserId;
                                            _journeyTrackingDetailsRepository.Update(tblVehicleJourneyTrackingDetailItem);
                                            _journeyTrackingDetailsRepository.SaveChanges();
                                        }
                                        else
                                        {
                                            continue;
                                        }

                                    }
                                }
                                var Notification = _pushNotificationManager.SendNotification(request.ServicePartnerId, request.DriverDetailsId, EnumHelper.DescriptionAttr(NotificationEnum.JourneyStartedbyDriver), null, null);
                                #endregion
                            }
                            else
                            {
                                tblVehicleJourneyTracking.CurrentVehicleLatt = request.CurrentVehicleLatt;
                                tblVehicleJourneyTracking.CurrentVehicleLong = request.CurrentVehicleLong;
                                tblVehicleJourneyTracking.ModifiedDate = _currentDatetime;
                                tblVehicleJourneyTracking.ModifiedBy = tblDriverDetail.UserId;
                                _journeyTrackingRepository.Update(tblVehicleJourneyTracking);
                                _journeyTrackingRepository.SaveChanges();
                                TrackingId = tblVehicleJourneyTracking.TrackingId;
                            }
                        }
                        else
                        {
                            responseMessage.message = "Data not found for to day";
                            responseMessage.Status = true;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                            return responseMessage;
                        }
                    }
                    #endregion
                    responseMessage.message = "Start Vehicle Journey Successfully";
                    responseMessage.Status = true;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                }
                else
                {
                    responseMessage.message = "Vehicle not available";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    return responseMessage;
                }
            }

            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("DriverDetailsManager", "StartVehicleJourney", ex);
            }
            return responseMessage;
        }
        #endregion

        #region Order Pickup Done by Vehicle
        /// <summary>
        /// Api method for Order Pickup Done by Vehicle
        /// Added by Priyanshi
        /// </summary>
        /// <param name="">Data</param>       
        /// <returns></returns>
        public ResponseResult PickupDonebyVehicle([FromForm] pickupDoneReq request)
        {
            TblOrderLgc tblOrderLgc = null;
            TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = null;

            bool flag = false;
            string LGCType = "Pickup";
            int saveOrderLgc = 0;
            TblOrderImageUpload tblOrderImageUpload = null;
            TblBusinessPartner tblBusinessPartner = null;
            TblVoucherVerfication tblVoucherVerfication = null;

            #region Common Implementations for (ABB or Exchange)
            TblExchangeOrder tblExchangeOrder = null;
            int? setStatusId = Convert.ToInt32(OrderStatusEnum.LGCPickup);
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;

            #endregion
            try
            {
                #region check
                if (request == null || request.DriverDetailsId <= 0 || request.OrdertransId <= 0)
                {
                    responseMessage.message = "Not Valid login credential";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }

                TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetSingle(x => x.IsActive == true && x.DriverDetailsId == request.DriverDetailsId);
                if (tblDriverDetail == null)
                {
                    responseMessage.message = "Not Valid login credential";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    return responseMessage;
                }

                TblLogistic tblLogistic = _logisticsRepository.GetSingle(x => x.OrderTransId == request.OrdertransId && x.IsActive == true && x.DriverDetailsId == request.DriverDetailsId && x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderAcceptedbyVehicle));
                if (tblLogistic == null)
                {
                    responseMessage.message = "Invalid RegdNo";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    return responseMessage;
                }

                TblOrderTran tblOrderTran = _orderTransRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == tblLogistic.OrderTransId);
                if (tblOrderTran == null || tblOrderTran.OrderTransId <= 0)
                {
                    responseMessage.message = "OrderTransId Not Found for RegdNo";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }
                #endregion
                #region Common Implementations for (ABB or Exchange)
                TblOrderTran tblOrderTrans = _orderTransRepository.GetSingleOrderWithExchangereference(request.OrdertransId);

                #endregion

                // TblLogistic tblLogistic = _logisticsRepository.GetSingle(x => x.IsActive == true && x.RegdNo == regdNo);
                TblLoV tblLoV = _lovRepository.GetSingle(x => x.IsActive == true && x.LoVname.ToLower().Equals(LGCType.ToLower()));
                #region save Image U_Image_One
                if (request.U_Image_One != null)
                {
                    string fileName = string.Empty;

                    fileName = tblOrderTran.RegdNo + "_" + "PickupImage" + "U_Image_One" + ".jpg";
                    string filePath = _baseConfig.Value.WebCoreBaseUrl + "LGC\\LGCPickup";
                    flag = _imageHelper.SaveFileDefRoot(request.U_Image_One, filePath, fileName);
                    if (flag)
                    {
                        #region Upload images in TblOrderImageUpload
                        tblOrderImageUpload = new TblOrderImageUpload();
                        tblOrderImageUpload.OrderTransId = tblOrderTrans.OrderTransId;
                        tblOrderImageUpload.ImageName = fileName;
                        tblOrderImageUpload.ImageUploadby = tblLoV.ParentId;
                        tblOrderImageUpload.LgcpickDrop = tblLoV.LoVname;
                        tblOrderImageUpload.IsActive = true;
                        tblOrderImageUpload.CreatedBy = tblDriverDetail.UserId;
                        tblOrderImageUpload.CreatedDate = DateTime.Now;
                        _orderImageUploadRepository.Create(tblOrderImageUpload);
                        _orderImageUploadRepository.SaveChanges();
                        #endregion
                    }
                }
                #endregion

                #region save Image U_Image_Two
                if (request.U_Image_Two != null)
                {
                    string fileName = string.Empty;

                    fileName = tblOrderTran.RegdNo + "_" + "PickupImage" + "U_Image_Two" + ".jpg";
                    string filePath = _baseConfig.Value.WebCoreBaseUrl + "LGC\\LGCPickup";
                    flag = _imageHelper.SaveFileDefRoot(request.U_Image_Two, filePath, fileName);
                    if (flag)
                    {
                        #region Upload images in TblOrderImageUpload
                        tblOrderImageUpload = new TblOrderImageUpload();
                        tblOrderImageUpload.OrderTransId = tblOrderTrans.OrderTransId;
                        tblOrderImageUpload.ImageName = fileName;
                        tblOrderImageUpload.ImageUploadby = tblLoV.ParentId;
                        tblOrderImageUpload.LgcpickDrop = tblLoV.LoVname;
                        tblOrderImageUpload.IsActive = true;
                        tblOrderImageUpload.CreatedBy = tblDriverDetail.UserId;
                        tblOrderImageUpload.CreatedDate = DateTime.Now;
                        _orderImageUploadRepository.Create(tblOrderImageUpload);
                        _orderImageUploadRepository.SaveChanges();
                        #endregion
                    }
                }
                #endregion

                #region save Image U_Image_three
                if (request.U_Image_three != null)
                {
                    string fileName = string.Empty;

                    fileName = tblOrderTran.RegdNo + "_" + "PickupImage" + "U_Image_three" + ".jpg";
                    string filePath = _baseConfig.Value.WebCoreBaseUrl + "LGC\\LGCPickup";
                    flag = _imageHelper.SaveFileDefRoot(request.U_Image_three, filePath, fileName);
                    if (flag)
                    {
                        #region Upload images in TblOrderImageUpload
                        tblOrderImageUpload = new TblOrderImageUpload();
                        tblOrderImageUpload.OrderTransId = tblOrderTrans.OrderTransId;
                        tblOrderImageUpload.ImageName = fileName;
                        tblOrderImageUpload.ImageUploadby = tblLoV.ParentId;
                        tblOrderImageUpload.LgcpickDrop = tblLoV.LoVname;
                        tblOrderImageUpload.IsActive = true;
                        tblOrderImageUpload.CreatedBy = tblDriverDetail.UserId;
                        tblOrderImageUpload.CreatedDate = DateTime.Now;
                        _orderImageUploadRepository.Create(tblOrderImageUpload);
                        _orderImageUploadRepository.SaveChanges();
                        #endregion
                    }
                }
                #endregion

                #region save Image U_Image_for
                if (request.U_Image_four != null)
                {
                    string fileName = string.Empty;

                    fileName = tblOrderTran.RegdNo + "_" + "PickupImage" + "U_Image_four" + ".jpg";
                    string filePath = _baseConfig.Value.WebCoreBaseUrl + "LGC\\LGCPickup";
                    flag = _imageHelper.SaveFileDefRoot(request.U_Image_four, filePath, fileName);
                    if (flag)
                    {
                        #region Upload images in TblOrderImageUpload
                        tblOrderImageUpload = new TblOrderImageUpload();
                        tblOrderImageUpload.OrderTransId = tblOrderTrans.OrderTransId;
                        tblOrderImageUpload.ImageName = fileName;
                        tblOrderImageUpload.ImageUploadby = tblLoV.ParentId;
                        tblOrderImageUpload.LgcpickDrop = tblLoV.LoVname;
                        tblOrderImageUpload.IsActive = true;
                        tblOrderImageUpload.CreatedBy = tblDriverDetail.UserId;
                        tblOrderImageUpload.CreatedDate = DateTime.Now;
                        _orderImageUploadRepository.Create(tblOrderImageUpload);
                        _orderImageUploadRepository.SaveChanges();
                        #endregion
                    }
                }
                #endregion

                #region save Image P_Image_five
                if (request.P_Image_five != null)
                {
                    string fileName = string.Empty;

                    fileName = tblOrderTran.RegdNo + "_" + "PickupImage" + "P_Image_five" + ".jpg";
                    string filePath = _baseConfig.Value.WebCoreBaseUrl + "LGC\\LGCPickup";
                    flag = _imageHelper.SaveFileDefRoot(request.P_Image_five, filePath, fileName);
                    if (flag)
                    {
                        #region Upload images in TblOrderImageUpload
                        tblOrderImageUpload = new TblOrderImageUpload();
                        tblOrderImageUpload.OrderTransId = tblOrderTrans.OrderTransId;
                        tblOrderImageUpload.ImageName = fileName;
                        tblOrderImageUpload.ImageUploadby = tblLoV.ParentId;
                        tblOrderImageUpload.LgcpickDrop = tblLoV.LoVname;
                        tblOrderImageUpload.IsActive = true;
                        tblOrderImageUpload.CreatedBy = tblDriverDetail.UserId;
                        tblOrderImageUpload.CreatedDate = DateTime.Now;
                        _orderImageUploadRepository.Create(tblOrderImageUpload);
                        _orderImageUploadRepository.SaveChanges();
                        #endregion
                    }
                }
                #endregion

                #region insert details in tblorderlgc
                if (tblOrderTrans != null && tblOrderTrans.OrderTransId > 0)
                {
                    tblOrderLgc = _orderLGCRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == tblOrderTrans.OrderTransId);
                }
                if (tblOrderLgc != null && tblOrderLgc.OrderLgcid > 0)
                {
                    tblOrderLgc.Lgccomments = request.LGCComment != null ? request.LGCComment : string.Empty;
                    tblOrderLgc.ActualPickupDate = DateTime.Now;
                    tblOrderLgc.StatusId = Convert.ToInt32(OrderStatusEnum.LGCPickup);
                    tblOrderLgc.IsActive = true;
                    tblOrderLgc.ModifiedBy = tblDriverDetail.UserId;
                    tblOrderLgc.ModifiedDate = DateTime.Now;
                    tblOrderLgc.LogisticId = tblLogistic.LogisticId;
                    tblOrderLgc.EvcregistrationId = request.EVCId;
                    tblOrderLgc.EvcpartnerId = request.EvcPartnerId;
                    tblOrderLgc.DriverDetailsId = request.DriverDetailsId;
                    _orderLGCRepository.Update(tblOrderLgc);
                    saveOrderLgc = _orderLGCRepository.SaveChanges();
                }
                else
                {
                    tblOrderLgc = new TblOrderLgc();
                    tblOrderLgc.OrderTransId = tblOrderTrans.OrderTransId;
                    tblOrderLgc.Lgccomments = request.LGCComment != null ? request.LGCComment : string.Empty;
                    tblOrderLgc.ActualPickupDate = DateTime.Now;
                    tblOrderLgc.StatusId = setStatusId;
                    tblOrderLgc.IsActive = true;
                    tblOrderLgc.CreatedBy = tblDriverDetail.UserId;
                    tblOrderLgc.CreatedDate = DateTime.Now;
                    tblOrderLgc.LogisticId = tblLogistic.LogisticId;
                    tblOrderLgc.EvcregistrationId = request.EVCId;
                    tblOrderLgc.EvcpartnerId = request.EvcPartnerId;
                    tblOrderLgc.DriverDetailsId = request.DriverDetailsId;
                    _orderLGCRepository.Create(tblOrderLgc);
                    saveOrderLgc = _orderLGCRepository.SaveChanges();
                }
                #endregion
                #region update statusId in Base tbl Exchange or ABB
                if (tblOrderTrans.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                {
                    tblExchangeOrder = _exchangeOrderRepository.GetSingle(x => x.IsActive == true && x.Id == tblOrderTrans.ExchangeId);
                    int Exresult = _exchangeOrderRepository.UpdateExchangeRecordStatus(tblOrderTrans.RegdNo, setStatusId, tblDriverDetail.UserId);

                }
                else
                {
                    var abbRedemption = _aBBRedemptionRepository.UpdateABBOrderStatus(tblOrderTrans.RegdNo, (int)setStatusId, (int)tblDriverDetail.UserId, EnumHelper.DescriptionAttr(OrderStatusEnum.LGCPickup));
                    //#region update status on tblAbbRedemption
                    //tblAbbredemption.StatusId = setStatusId;
                    //tblAbbredemption.AbbredemptionStatus = "Pickup";
                    //tblAbbredemption.ModifiedBy = tblDriverDetail.UserId;
                    //tblAbbredemption.ModifiedDate = _currentDatetime;
                    //_abbRedemptionRepository.Update(tblAbbredemption);
                    //_abbRedemptionRepository.SaveChanges();
                    //#endregion
                }
                #endregion
                #region Redeemed Mark on tblVoucherVerification if Order case is Deffered 
                if (saveOrderLgc > 0 && tblExchangeOrder != null && tblExchangeOrder.IsDefferedSettlement == true)
                {
                    tblBusinessPartner = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == tblExchangeOrder.BusinessPartnerId);
                    if (tblBusinessPartner != null && tblBusinessPartner.IsDefferedSettlement == true && tblBusinessPartner.IsVoucher == true && tblBusinessPartner.VoucherType == Convert.ToInt32(BusinessPartnerVoucherTypeEnum.Cash))
                    {
                        tblVoucherVerfication = _voucherRepository.GetSingle(x => x.IsActive == true && x.ExchangeOrderId == tblExchangeOrder.Id && x.BusinessPartnerId == tblBusinessPartner.BusinessPartnerId);
                        if (tblVoucherVerfication != null && tblVoucherVerfication.VoucherStatusId == Convert.ToInt32(VoucherStatusEnum.Captured))
                        {
                            tblVoucherVerfication.VoucherStatusId = Convert.ToInt32(VoucherStatusEnum.Redeemed);
                            tblVoucherVerfication.ModifiedBy = tblDriverDetail.UserId;
                            tblVoucherVerfication.ModifiedDate = _currentDatetime;
                            _voucherRepository.Update(tblVoucherVerfication);
                            _voucherRepository.SaveChanges();
                        }
                    }
                }
                #endregion

                #region update statusId in tblOrderTrans
                tblOrderTrans.StatusId = setStatusId;
                tblOrderTrans.ModifiedBy = tblDriverDetail.UserId;
                tblOrderTrans.ModifiedDate = _currentDatetime;
                _orderTransRepository.Update(tblOrderTrans);
                _orderTransRepository.SaveChanges();
                #endregion

                #region Insert into tblexchangeabbhistory ABB/Exchange
                if (tblOrderTrans != null)
                {
                    if (tblOrderTrans.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                    {
                        #region Insert into tblexchangeabbhistory
                        tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                        tblExchangeAbbstatusHistory.OrderType = (int)tblOrderTrans.OrderType;
                        tblExchangeAbbstatusHistory.SponsorOrderNumber = tblOrderTrans.Exchange.SponsorOrderNumber;
                        tblExchangeAbbstatusHistory.RegdNo = tblOrderTrans.Exchange.RegdNo;
                        tblExchangeAbbstatusHistory.ZohoSponsorId = tblOrderTrans.Exchange.ZohoSponsorOrderId != null ? tblOrderTrans.Exchange.ZohoSponsorOrderId : string.Empty;
                        tblExchangeAbbstatusHistory.CustId = tblOrderTrans.Exchange.CustomerDetailsId;
                        tblExchangeAbbstatusHistory.StatusId = Convert.ToInt32(OrderStatusEnum.LGCPickup);
                        tblExchangeAbbstatusHistory.IsActive = true;
                        tblExchangeAbbstatusHistory.CreatedBy = tblDriverDetail.UserId;
                        tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                        tblExchangeAbbstatusHistory.OrderTransId = tblOrderTrans.OrderTransId;
                        tblExchangeAbbstatusHistory.Evcid = request.EVCId;
                        _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                        //_exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                        //_exchangeABBStatusHistoryRepository.SaveChanges();
                        #endregion

                    }
                    else
                    {
                        #region Insert into tblexchangeabbhistory
                        tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                        tblExchangeAbbstatusHistory.OrderType = (int)tblOrderTran.OrderType;
                        //  tblExchangeAbbstatusHistory.SponsorOrderNumber = tblOrderTran.Abbredemption.SponsorOrderNumber;
                        tblExchangeAbbstatusHistory.RegdNo = tblOrderTran.RegdNo;
                        // tblExchangeAbbstatusHistory.ZohoSponsorId = tblOrderTran.Exchange.ZohoSponsorOrderId != null ? TblExchangeOrders.ZohoSponsorOrderId : string.Empty;
                        tblExchangeAbbstatusHistory.CustId = tblOrderTrans.Abbredemption.CustomerDetailsId;
                        tblExchangeAbbstatusHistory.StatusId = Convert.ToInt32(OrderStatusEnum.LGCPickup);
                        tblExchangeAbbstatusHistory.IsActive = true;
                        tblExchangeAbbstatusHistory.CreatedBy = tblDriverDetail.UserId;
                        tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                        tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                        tblExchangeAbbstatusHistory.Evcid = request.EVCId;
                        _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                        //_exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                        //_exchangeABBStatusHistoryRepository.SaveChanges();
                        #endregion

                    }
                }
                #endregion

                #region Update StatusId in Tbl WalletTransection 
                TblWalletTransaction tblWalletTransaction = _walletTransactionRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == request.OrdertransId);
                tblWalletTransaction.StatusId = setStatusId.ToString();
                tblWalletTransaction.ModifiedBy = tblDriverDetail.UserId;
                tblWalletTransaction.ModifiedDate = _currentDatetime;
                _walletTransactionRepository.Update(tblWalletTransaction);
                _walletTransactionRepository.SaveChanges();

                #endregion

                TblVehicleJourneyTrackingDetail tblVehicleJourneyTrackingDetail = _journeyTrackingDetailsRepository.GetSingle(x => x.OrderTransId == request.OrdertransId && x.DriverId == request.DriverDetailsId && x.ServicePartnerId == request.ServicePartnerId && x.IsActive == true);
                if (tblVehicleJourneyTrackingDetail != null)
                {
                    //DateTime pickupStartDatetime = (DateTime)tblVehicleJourneyTrackingDetail.PickupStartDatetime;
                    //DateTime pickupEndDatetime = _currentDatetime;

                    //TimeSpan? pickupTat = pickupEndDatetime - pickupStartDatetime;
                    //double hours = pickupTat.TotalHours;
                    //int roundedHours = (int)Math.Round(hours);
                    if (request.P_Image_five != null)
                    {
                        tblVehicleJourneyTrackingDetail.IsPackedImg = true;
                    }
                    else
                    {
                        tblVehicleJourneyTrackingDetail.IsPackedImg = false;
                    }
                    tblVehicleJourneyTrackingDetail.StatusId = setStatusId;
                    tblVehicleJourneyTrackingDetail.PickupEndDatetime = _currentDatetime;
                    tblVehicleJourneyTrackingDetail.ModifiedDate = _currentDatetime;
                    tblVehicleJourneyTrackingDetail.ModifiedBy = tblDriverDetail.UserId;
                    // tblVehicleJourneyTrackingDetail.PickupTat = pickupTat;                                    
                    _context.Entry(tblVehicleJourneyTrackingDetail).State = EntityState.Modified;
                    _context.SaveChanges();
                    _context.Entry(tblVehicleJourneyTrackingDetail).State = EntityState.Detached;

                    bool pickupflag = _commonManager.CalculateDriverIncentive((int)tblVehicleJourneyTrackingDetail.OrderTransId);
                }
                #region update statusId in tbllogistic
                tblLogistic.StatusId = Convert.ToInt32(OrderStatusEnum.LGCPickup);
                tblLogistic.Modifiedby = tblDriverDetail.UserId;
                tblLogistic.ModifiedDate = _currentDatetime;
                _logisticsRepository.Update(tblLogistic);
                _logisticsRepository.SaveChanges();
                #endregion
                #region Genreate Customer Decleration 
                if (request != null)
                {
                    string custDeclartionpdfname = tblOrderTrans.RegdNo + "_" + "custdeclarationpdf" + ".pdf";
                    string custDeclartionfilepath = _baseConfig.Value.WebCoreBaseUrl + "evc\\CustomerDeclaration";
                    string custDeclartionhtmlstring = GetCustDeclartionhtmlstring(tblOrderTrans, "customer_declaration");
                    bool podpdfsave = _htmlToPdfConverterHelper.GeneratePDFAPI(custDeclartionhtmlstring, custDeclartionfilepath, custDeclartionpdfname);
                    tblOrderLgc.CustDeclartionpdfname = custDeclartionpdfname != null ? custDeclartionpdfname : string.Empty;
                    _orderLGCRepository.Update(tblOrderLgc);
                    _orderLGCRepository.SaveChanges();
                }
                #endregion


                responseMessage.message = "Pickup Order Successfully";
                responseMessage.Status = true;
                responseMessage.Status_Code = HttpStatusCode.OK;

                var Notification = _pushNotificationManager.SendNotification(tblVehicleJourneyTrackingDetail.ServicePartnerId, tblVehicleJourneyTrackingDetail.DriverId, EnumHelper.DescriptionAttr(NotificationEnum.OrderPickupedbyDriver), tblOrderTrans.RegdNo, null);
            }

            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("DriverDetailsManager", "AcceptOrderbyVehicle", ex);
            }
            return responseMessage;
        }
        #endregion

        #region create pdf string for Customer Declartion
        /// <summary>
        /// create pdf string for Customer Declartion
        /// </summary>
        /// <param name="orderLGCViewModel"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns></returns>
        public string GetCustDeclartionhtmlstring(TblOrderTran tblOrderTran, string HtmlTemplateNameOnly)
        {
            var DateTime = System.DateTime.Now;
            string FinalDate = DateTime.Date.ToShortDateString();
            string htmlString = "";
            string fileName = HtmlTemplateNameOnly + ".html";
            string fileNameWithPath = "";
            string AbbOrderBrand = "";
            try
            {
                var filePath = _baseConfig.Value.WebCoreBaseUrlCD + "\\PdfTemplates";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                fileNameWithPath = string.Concat(filePath, "\\", fileName);
                htmlString = File.ReadAllText(fileNameWithPath);
                var EVCDetails = _walletTransactionRepository.GetSingleEvcDetails(tblOrderTran.OrderTransId);



                if (tblOrderTran.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                {
                    if (HtmlTemplateNameOnly == "customer_declaration")
                        htmlString = htmlString.Replace("[Regd_No]", tblOrderTran.RegdNo)
                            .Replace("[Customer_Name]", tblOrderTran.Exchange.CustomerDetails.FirstName)
                            .Replace("[Customer_Address_1]", tblOrderTran.Exchange.CustomerDetails.Address1)
                            .Replace("[Customer_City]", tblOrderTran.Exchange.CustomerDetails.City)
                            .Replace("[Customer_Pincode]", tblOrderTran.Exchange.CustomerDetails.ZipCode.ToString())
                            .Replace("[Short_Number]", tblOrderTran.Exchange.CustomerDetails.PhoneNumber)
                            .Replace("[Sponsor_Name]", tblOrderTran.Exchange.SponsorOrderNumber)
                            .Replace("[New_Prod_Group]", tblOrderTran.Exchange.ProductType.ProductCat.Description)
                            .Replace("[New_Product_Type]", tblOrderTran.Exchange.ProductType.Description)
                            .Replace("[EVC_Bussiness_Name]", EVCDetails.Evcregistration.BussinessName)
                            .Replace("[Old_Brand_Name]", tblOrderTran.Exchange.Brand.Name)
                            .Replace("[date]", FinalDate);
                }
                else
                {
                    TblBusinessUnit tblBuID = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == tblOrderTran.Abbredemption.Abbregistration.BusinessUnitId && x.IsBumultiBrand == true);
                    if (tblBuID != null)
                    {
                        TblBrandSmartBuy tblBrandSmart = _brandSmartBuyRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == tblBuID.BusinessUnitId && x.ProductCategoryId == tblOrderTran.Abbredemption.Abbregistration.NewProductCategoryId && x.Id == tblOrderTran.Abbredemption.Abbregistration.NewBrandId);
                        if (tblBrandSmart != null)
                        {
                            AbbOrderBrand = tblBrandSmart.Name;
                        }

                    }
                    else
                    {
                        TblBrand? tblBrand = _brandRepository.GetSingle(x => x.IsActive == true && x.Id == tblOrderTran?.Abbredemption?.Abbregistration?.NewBrandId);
                        if (tblBrand != null)
                        {
                            AbbOrderBrand = tblBrand?.Name;
                        }
                    }
                    if (HtmlTemplateNameOnly == "customer_declaration")
                        htmlString = htmlString.Replace("[Regd_No]", tblOrderTran.RegdNo)
                            .Replace("[Customer_Name]", tblOrderTran?.Abbredemption?.CustomerDetails?.FirstName)
                            .Replace("[Customer_Address_1]", tblOrderTran?.Abbredemption?.CustomerDetails?.Address1)
                            .Replace("[Customer_City]", tblOrderTran?.Abbredemption?.CustomerDetails?.City)
                            .Replace("[Customer_Pincode]", tblOrderTran?.Abbredemption?.CustomerDetails?.ToString())
                            .Replace("[Short_Number]", tblOrderTran?.Abbredemption?.CustomerDetails?.PhoneNumber)
                            .Replace("[Sponsor_Name]", tblOrderTran?.Abbredemption?.Sponsor)
                            .Replace("[New_Prod_Group]", tblOrderTran?.Abbredemption?.Abbregistration?.NewProductCategory?.Description)
                            .Replace("[New_Product_Type]", tblOrderTran?.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation?.Description)
                            .Replace("[EVC_Bussiness_Name]", EVCDetails?.Evcregistration?.BussinessName)
                            .Replace("[Old_Brand_Name]", AbbOrderBrand)
                            .Replace("[date]", FinalDate);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "GetCustDeclartionhtmlstring", ex);
            }
            return htmlString;
        }
        #endregion

        #region Order Drop Done by Vehicle
        /// <summary>
        /// Api method for Order Pickup Done by Vehicle
        /// Added by Priyanshi
        /// </summary>
        /// <param name="">Data</param>       
        /// <returns></returns>
        public ResponseResult DropDonebyVehicle([FromForm] DropDoneReq request)
        {

            bool flag = false;
            string LGCType = "Drop";
            TblOrderImageUpload tblOrderImageUpload = null;
            #region Common Implementations for (ABB or Exchange)            
            int? setStatusId = Convert.ToInt32(OrderStatusEnum.LGCDrop);
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;

            List<OrdertranList> OrderTransList = new List<OrdertranList>();
            PODViewModel podVM = new PODViewModel();
            EVCDetailsViewModel eVCDetailsViewModel = new EVCDetailsViewModel();
            podVM.evcDetailsVM = new EVCDetailsViewModel();

            #endregion
            try
            {
                #region check
                if (request == null || request.DriverDetailsId <= 0 || request.ServicePartnerId <= 0)
                {
                    responseMessage.message = "Not Valid login credential";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    return responseMessage;
                }

                TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetSingle(x => x.IsActive == true && x.DriverDetailsId == request.DriverDetailsId);
                if (tblDriverDetail == null)
                {
                    responseMessage.message = "Not Valid login credential";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }
                #endregion
                #region save Image
                foreach (var item in request.OrderTransList)
                {
                    TblLogistic tblLogistic = _logisticsRepository.GetSingle(x => x.OrderTransId == item.OrdertransId && x.IsActive == true && x.DriverDetailsId == request.DriverDetailsId && x.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup));

                    OrdertranList OrderTransListO = new OrdertranList();
                    if (tblLogistic == null)
                    {
                        responseMessage.message = "Invalid RegdNo - " + item.OrdertransId;
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.OK;
                        return responseMessage;
                    }

                    #region Common Implementations for (ABB or Exchange)
                    TblOrderTran tblOrderTran = _orderTransRepository.GetSingleOrderWithExchangereference(item.OrdertransId);

                    #endregion
                    if (tblOrderTran == null || tblOrderTran.OrderTransId <= 0)
                    {
                        responseMessage.message = "OrderTransId Not Found for RegdNo" + item.OrdertransId;
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        return responseMessage;
                    }

                    TblLoV tblLoV = _lovRepository.GetSingle(x => x.IsActive == true && x.LoVname.ToLower().Equals(LGCType.ToLower()));
                    #region save Image U_Image_One
                    if (request.U_Image_One != null)
                    {
                        string fileName = string.Empty;

                        fileName = tblOrderTran.RegdNo + "_" + "DropImage" + "U_Image_One" + ".jpg";
                        string filePath = _baseConfig.Value.WebCoreBaseUrl + "LGC\\LGCDrop";
                        flag = _imageHelper.SaveFileDefRoot(request.U_Image_One, filePath, fileName);
                        if (flag)
                        {
                            #region Upload images in TblOrderImageUpload
                            tblOrderImageUpload = new TblOrderImageUpload();
                            tblOrderImageUpload.OrderTransId = tblOrderTran.OrderTransId;
                            tblOrderImageUpload.ImageName = fileName;
                            tblOrderImageUpload.ImageUploadby = tblLoV.ParentId;
                            tblOrderImageUpload.LgcpickDrop = tblLoV.LoVname;
                            tblOrderImageUpload.IsActive = true;
                            tblOrderImageUpload.CreatedBy = tblDriverDetail.UserId;
                            tblOrderImageUpload.CreatedDate = DateTime.Now;
                            _orderImageUploadRepository.Create(tblOrderImageUpload);
                            _orderImageUploadRepository.SaveChanges();
                            #endregion
                        }
                    }
                    #endregion

                    #region save Image U_Image_Two
                    if (request.U_Image_Two != null)
                    {
                        string fileName = string.Empty;

                        fileName = tblOrderTran.RegdNo + "_" + "DropImage" + "U_Image_Two" + ".jpg";
                        string filePath = _baseConfig.Value.WebCoreBaseUrl + "LGC\\LGCDrop";
                        flag = _imageHelper.SaveFileDefRoot(request.U_Image_Two, filePath, fileName);
                        if (flag)
                        {
                            #region Upload images in TblOrderImageUpload
                            tblOrderImageUpload = new TblOrderImageUpload();
                            tblOrderImageUpload.OrderTransId = tblOrderTran.OrderTransId;
                            tblOrderImageUpload.ImageName = fileName;
                            tblOrderImageUpload.ImageUploadby = tblLoV.ParentId;
                            tblOrderImageUpload.LgcpickDrop = tblLoV.LoVname;
                            tblOrderImageUpload.IsActive = true;
                            tblOrderImageUpload.CreatedBy = tblDriverDetail.UserId;
                            tblOrderImageUpload.CreatedDate = DateTime.Now;
                            _orderImageUploadRepository.Create(tblOrderImageUpload);
                            _orderImageUploadRepository.SaveChanges();
                            #endregion
                        }
                    }
                    #endregion

                    #region save Image U_Image_three
                    if (request.U_Image_three != null)
                    {
                        string fileName = string.Empty;

                        fileName = tblOrderTran.RegdNo + "_" + "DropImage" + "U_Image_three" + ".jpg";
                        string filePath = _baseConfig.Value.WebCoreBaseUrl + "LGC\\LGCDrop";
                        flag = _imageHelper.SaveFileDefRoot(request.U_Image_three, filePath, fileName);
                        if (flag)
                        {
                            #region Upload images in TblOrderImageUpload
                            tblOrderImageUpload = new TblOrderImageUpload();
                            tblOrderImageUpload.OrderTransId = tblOrderTran.OrderTransId;
                            tblOrderImageUpload.ImageName = fileName;
                            tblOrderImageUpload.ImageUploadby = tblLoV.ParentId;
                            tblOrderImageUpload.LgcpickDrop = tblLoV.LoVname;
                            tblOrderImageUpload.IsActive = true;
                            tblOrderImageUpload.CreatedBy = tblDriverDetail.UserId;
                            tblOrderImageUpload.CreatedDate = DateTime.Now;
                            _orderImageUploadRepository.Create(tblOrderImageUpload);
                            _orderImageUploadRepository.SaveChanges();
                            #endregion
                        }
                    }
                    #endregion

                    #region save Image U_Image_four
                    if (request.U_Image_four != null)
                    {
                        string fileName = string.Empty;

                        fileName = tblOrderTran.RegdNo + "_" + "DropImage" + "U_Image_four" + ".jpg";
                        string filePath = _baseConfig.Value.WebCoreBaseUrl + "LGC\\LGCDrop";
                        flag = _imageHelper.SaveFileDefRoot(request.U_Image_four, filePath, fileName);
                        if (flag)
                        {
                            #region Upload images in TblOrderImageUpload
                            tblOrderImageUpload = new TblOrderImageUpload();
                            tblOrderImageUpload.OrderTransId = tblOrderTran.OrderTransId;
                            tblOrderImageUpload.ImageName = fileName;
                            tblOrderImageUpload.ImageUploadby = tblLoV.ParentId;
                            tblOrderImageUpload.LgcpickDrop = tblLoV.LoVname;
                            tblOrderImageUpload.IsActive = true;
                            tblOrderImageUpload.CreatedBy = tblDriverDetail.UserId;
                            tblOrderImageUpload.CreatedDate = DateTime.Now;
                            _orderImageUploadRepository.Create(tblOrderImageUpload);
                            _orderImageUploadRepository.SaveChanges();
                            #endregion
                        }
                    }
                    #endregion

                    #region save Image P_Image_five
                    if (request.P_Image_five != null)
                    {
                        string fileName = string.Empty;

                        fileName = tblOrderTran.RegdNo + "_" + "DropImage" + "P_Image_five" + ".jpg";
                        string filePath = _baseConfig.Value.WebCoreBaseUrl + "LGC\\LGCDrop";
                        flag = _imageHelper.SaveFileDefRoot(request.P_Image_five, filePath, fileName);
                        if (flag)
                        {
                            #region Upload images in TblOrderImageUpload
                            tblOrderImageUpload = new TblOrderImageUpload();
                            tblOrderImageUpload.OrderTransId = tblOrderTran.OrderTransId;
                            tblOrderImageUpload.ImageName = fileName;
                            tblOrderImageUpload.ImageUploadby = tblLoV.ParentId;
                            tblOrderImageUpload.LgcpickDrop = tblLoV.LoVname;
                            tblOrderImageUpload.IsActive = true;
                            tblOrderImageUpload.CreatedBy = tblDriverDetail.UserId;
                            tblOrderImageUpload.CreatedDate = DateTime.Now;
                            _orderImageUploadRepository.Create(tblOrderImageUpload);
                            _orderImageUploadRepository.SaveChanges();
                            #endregion
                        }
                    }
                    #endregion

                    OrderTransListO.OrdertransId = item.OrdertransId;
                    OrderTransList.Add(OrderTransListO);
                }
                if (OrderTransList.Count > 0)
                {
                    podVM.EVCId = request.EVCId;
                    podVM.EvcPartnerId = request.EvcPartnerId;
                    podVM.DriverId = request.DriverDetailsId;
                    podVM.OrderTransListO = OrderTransList;

                    TblEvcregistration tblEvcregistration = _eVCRepository.GetEVCDetailsById((int)podVM.EVCId);
                    TblEvcPartner tblEvcPartner = _evCPartnerRepository.GetEVCPartnerDetails((int)podVM.EvcPartnerId);
                    if (tblEvcregistration != null)
                    {
                        podVM.evcDetailsVM.Id = tblEvcregistration.EvcregistrationId;
                        podVM.evcDetailsVM.Name = tblEvcregistration.BussinessName + "-" + tblEvcPartner.EvcStoreCode;
                        podVM.evcDetailsVM.State = tblEvcPartner.State.Name;
                        podVM.evcDetailsVM.City = tblEvcPartner.City.Name;
                        podVM.evcDetailsVM.Pincode = tblEvcPartner.PinCode;
                        podVM.evcDetailsVM.Address = tblEvcPartner.Address1;
                        podVM.evcDetailsVM.EvcStoreCode = tblEvcPartner.EvcStoreCode;

                    }

                    flag = SaveLGCDropStatus(podVM, (int)tblDriverDetail.UserId);
                    if (flag == true)
                    {
                        responseMessage.message = "Drop Done Successfully";
                        responseMessage.Status = true;
                        responseMessage.Status_Code = HttpStatusCode.OK;
                        var Notification = _pushNotificationManager.SendNotification(request.ServicePartnerId, request.DriverDetailsId, EnumHelper.DescriptionAttr(NotificationEnum.OrderDropedbyDriver), (request.OrderTransList.Count).ToString(), null);
                        SaveJourneyEndTime(request.TrackingId, (int)tblDriverDetail.UserId);
                    }
                    else
                    {
                        responseMessage.message = "failed";
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.OK;

                    }
                }
                #endregion
            }

            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("DriverDetailsManager", "AcceptOrderbyVehicle", ex);
            }
            return responseMessage;
        }

        #region Save Journey End Time
        public void SaveJourneyEndTime(int TrackingId, int loggedUserId)
        {
            List<TblVehicleJourneyTrackingDetail> tblVehicleJourneyTrackingDetail = _journeyTrackingDetailsRepository.GetVehicleJourneyTrackingDetailByTrackingId(TrackingId);
            bool result = false;
            int count = 0;
            try
            {
                if (tblVehicleJourneyTrackingDetail != null)
                {
                    foreach (var item in tblVehicleJourneyTrackingDetail)
                    {
                        if (!(item.StatusId == Convert.ToInt32(OrderStatusEnum.Posted) || item.StatusId == Convert.ToInt32(OrderStatusEnum.PickupDecline) || item.StatusId == Convert.ToInt32(OrderStatusEnum.PickupReschedule)))
                        {
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
                if (result)
                {
                    TblVehicleJourneyTracking tblVehicleJourneyTracking = new TblVehicleJourneyTracking();
                    tblVehicleJourneyTracking = _journeyTrackingRepository.GetVehicleJourneyTrackingById(TrackingId);
                    if (tblVehicleJourneyTracking != null)
                    {
                        tblVehicleJourneyTracking.JourneyEndTime = _currentDatetime;
                        tblVehicleJourneyTracking.ModifiedBy = loggedUserId;
                        tblVehicleJourneyTracking.ModifiedDate = _currentDatetime;
                        _journeyTrackingRepository.UpdateVehicleJourneyTracking(tblVehicleJourneyTracking);
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DriverDetailsManager", "SaveJourneyEndTime", ex);
            }
        }
        #endregion

        #region Manage LGC Drop Methods and update drop status on all associated tables
        /// <summary>
        /// Create POD, DebitNote and update drop status on all respective tables
        /// </summary>
        /// <param name="podVM"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public bool SaveLGCDropStatus(PODViewModel podVM, int loggedUserId)
        {
            #region Variable Declaration
            List<TblOrderLgc> tblOrderLgcListAll = null;
            podVM.podVMList = new List<PODViewModel>();
            PODViewModel podVMtemp = null;
            bool flag = false;
            string podfilePath = null;
            string podHtmlString = null;
            bool podPDFSave = false;
            string podPdfName = null;
            string debitNotePdfName = null;
            bool debitNoteSaved = false;
            string debitNotefilePath = null;
            string debitNoteHtmlString = null;
            decimal? finalAmountDN = 0;
            //Generate Invoice
            int MaxInvSrNum = 0;
            int InvSrNumFromConfig = 0;
            string FinancialYear = "";
            decimal? finalAmountInv = 0;
            string invoicePdfName = null;
            bool invoiceSaved = false;
            string invoicefilePath = null;
            string invoiceHtmlString = null;
            List<TblConfiguration> tblConfigurationList = null;
            //Generate Invoice
            TblEvcpoddetail tblEvcpoddetailObj = null;
            #endregion

            OrderLGCViewModel orderLGC = new OrderLGCViewModel();
            try
            {
                if (podVM != null && podVM.DriverId > 0 && podVM.EVCId > 0 && podVM.EvcPartnerId > 0)
                {
                    #region Code for Get Data from TblConfiguration
                    tblConfigurationList = _digi2L_DevContext.TblConfigurations.Where(x => x.IsActive == true).ToList();
                    if (tblConfigurationList != null && tblConfigurationList.Count > 0)
                    {
                        // MaxInvSrNum = tblEvcpoddetail.Max(x => x.InvSrNum).Value;
                        var startInvoiceSrNum = tblConfigurationList.FirstOrDefault(x => x.Name == ConfigurationEnum.StartInvoiceSrNum.ToString());
                        if (startInvoiceSrNum != null && startInvoiceSrNum.Value != null)
                        {
                            InvSrNumFromConfig = Convert.ToInt32(startInvoiceSrNum.Value.Trim());
                        }
                        var financialYear = tblConfigurationList.FirstOrDefault(x => x.Name == ConfigurationEnum.FinancialYear.ToString());
                        if (financialYear != null && financialYear.Value != null)
                        {
                            FinancialYear = financialYear.Value.Trim();
                        }
                    }
                    #endregion

                    #region Code for get Max InvSrNum from TblEvcpoddetails
                    // tblEvcpoddetail = _evcPoDDetailsRepository.GetList(x => x.IsActive == true && x.InvSrNum != null && x.InvSrNum > 0).ToList();
                    tblEvcpoddetailObj = _evcPoDDetailsRepository.GetList(x => x.IsActive == true && x.InvSrNum != null && x.InvSrNum != null && x.InvSrNum > 0 && x.FinancialYear == FinancialYear).OrderByDescending(x => x.InvSrNum).FirstOrDefault();

                    if (tblEvcpoddetailObj != null && tblEvcpoddetailObj.InvSrNum <= InvSrNumFromConfig)
                    {
                        MaxInvSrNum = InvSrNumFromConfig;
                    }
                    else if (tblEvcpoddetailObj != null && tblEvcpoddetailObj.InvSrNum >= InvSrNumFromConfig)
                    {
                        MaxInvSrNum = Convert.ToInt32(tblEvcpoddetailObj.InvSrNum);
                    }
                    else if (tblEvcpoddetailObj == null && InvSrNumFromConfig > 0)
                    {
                        MaxInvSrNum = InvSrNumFromConfig;
                    }
                    #endregion

                    tblOrderLgcListAll = _orderLGCRepository.GetOrderLGCListByDriverIdEVCPId(podVM.DriverId, podVM.EvcPartnerId)
                        .Where(x => x.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup)).ToList();

                    List<TblOrderLgc> tblOrderLgcListAll1 = new List<TblOrderLgc>();
                    if (podVM != null && podVM.OrderTransListO != null && podVM.OrderTransListO.Count > 0)
                    {
                        foreach (var item in podVM.OrderTransListO)
                        {
                            var item1 = tblOrderLgcListAll.FirstOrDefault(x => x.IsActive == true && x.OrderTransId == item.OrdertransId);
                            if (item1 != null)
                            {
                                tblOrderLgcListAll1.Add(item1);
                            }
                        }
                    }

                    tblOrderLgcListAll = tblOrderLgcListAll1;

                    if (tblOrderLgcListAll != null && tblOrderLgcListAll.Count > 0)
                    {

                        var totalOrderRecords = 0;
                        int pageSize = 8;
                        int skip = 0;
                        int totalPages = 1;
                        int reminder = 0;

                        #region if Orders List is greater than [pageSize] records per invoice
                        totalOrderRecords = tblOrderLgcListAll.Count;
                        if (totalOrderRecords > pageSize)
                        {
                            totalPages = Convert.ToInt32(totalOrderRecords / pageSize);
                            reminder = Convert.ToInt32(totalOrderRecords % pageSize);
                            if (reminder > 0) { totalPages++; }
                        }
                        for (int i = 1; i <= totalPages; i++)
                        {
                            finalAmountDN = 0; finalAmountInv = 0; bool DropCompleted = false; bool PostedCompleted = false;
                            podVM.podVMList = new List<PODViewModel>();
                            MaxInvSrNum++;
                            skip = (i - 1) * pageSize;
                            List<TblOrderLgc> orderList = tblOrderLgcListAll.Skip(skip).Take(pageSize).ToList();

                            #region Set Counter Sr. Number 
                            string InvSrNum = String.Format("{0:D6}", MaxInvSrNum);
                            podPdfName = "POD-" + FinancialYear.Replace("/", "-") + "-" + InvSrNum + ".pdf";
                            podVM.PoDPdfName = podPdfName;
                            #endregion

                            #region Initialize POD and Debit Note Data
                            foreach (var item1 in orderList)
                            {
                                if (item1 != null && item1.Evcregistration != null && item1.Evcpartner != null && item1.Logistic != null && item1.OrderTrans != null
                                    && (item1.OrderTrans.Exchange != null || item1.OrderTrans.Abbredemption != null) && item1.OrderTrans.TblWalletTransactions != null && item1.OrderTrans.TblWalletTransactions.Count > 0)
                                {
                                    var tblWalletTransObj = item1.OrderTrans.TblWalletTransactions.FirstOrDefault(x => x.OrderTransId == item1.OrderTransId);
                                    if (tblWalletTransObj != null)
                                    {
                                        #region Common Variable Declaration for (ABB or Exchange)
                                        TblExchangeOrder tblExchangeOrder = null; TblCustomerDetail tblCustomerDetail = null;
                                        TblAbbredemption tblAbbredemption = null;
                                        TblAbbregistration tblAbbregistration = null;
                                        string productTypeDesc = null; string productCatDesc = null; decimal FinalExchPriceWithoutSweetner = 0;
                                        #endregion

                                        #region Common Implementations for (ABB or Exchange)
                                        if (item1.OrderTrans.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                                        {
                                            tblExchangeOrder = item1.OrderTrans.Exchange;
                                            tblCustomerDetail = tblExchangeOrder?.CustomerDetails;
                                            productTypeDesc = tblExchangeOrder?.ProductType?.Name;
                                            productCatDesc = tblExchangeOrder?.ProductType?.ProductCat?.Name;
                                            FinalExchPriceWithoutSweetner = Convert.ToDecimal(tblExchangeOrder?.FinalExchangePrice) - Convert.ToDecimal(tblExchangeOrder?.Sweetener);
                                        }
                                        else if (item1.OrderTrans.OrderType == Convert.ToInt32(LoVEnum.ABB))
                                        {
                                            tblAbbredemption = item1.OrderTrans.Abbredemption;
                                            tblAbbregistration = tblAbbredemption?.Abbregistration;
                                            tblCustomerDetail = item1.OrderTrans.Abbredemption?.CustomerDetails;
                                            productTypeDesc = tblAbbregistration?.NewProductCategoryTypeNavigation?.Name;
                                            productCatDesc = tblAbbregistration?.NewProductCategory?.Name;
                                            FinalExchPriceWithoutSweetner = Convert.ToDecimal(item1.OrderTrans?.FinalPriceAfterQc);
                                        }
                                        #endregion

                                        podVMtemp = new PODViewModel();
                                        podVMtemp.EVCBussinessName = item1.Evcregistration.BussinessName + "-" + item1.Evcpartner.EvcStoreCode;
                                        podVMtemp.EVCRegdNo = item1.Evcregistration.EvcregdNo;
                                        podVMtemp.ServicePartnerName = item1.Logistic.ServicePartner.ServicePartnerName;
                                        podVMtemp.TicketNo = podVMtemp.ServicePartnerName + "-" + item1.Logistic.TicketNumber;
                                        podVMtemp.RegdNo = item1.Logistic.RegdNo;
                                        podVMtemp.ProductCatName = productCatDesc;
                                        if (tblCustomerDetail != null)
                                        {
                                            podVMtemp.CustName = tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName;
                                            podVMtemp.CustPincode = tblCustomerDetail.ZipCode;
                                        }
                                        podVMtemp.Podurl = EnumHelper.DescriptionAttr(FileAddressEnum.EVCPoD) + podPdfName;
                                        podVMtemp.OrderAmtForEVCDN = FinalExchPriceWithoutSweetner; //item1.OrderTrans.TblWalletTransactions.FirstOrDefault(x=>x.OrderTransId == item1.OrderTransId).OrderAmount;
                                        finalAmountDN += Convert.ToDecimal(podVMtemp.OrderAmtForEVCDN);

                                        #region Initialize Distinct data for invoice
                                        decimal? orderAmt = 0;
                                        orderAmt = tblWalletTransObj.OrderAmount;
                                        podVMtemp.OrderAmtForEVCInv = Convert.ToDecimal(orderAmt) - Convert.ToDecimal(FinalExchPriceWithoutSweetner);
                                        finalAmountInv += Convert.ToDecimal(podVMtemp.OrderAmtForEVCInv);
                                        podVM.podVMList.Add(podVMtemp);
                                        #endregion
                                    }
                                }
                            }
                            #endregion
                            if (podVM != null && podVM.podVMList != null && podVM.podVMList.Count > 0)
                            {
                                #region Generate and Save PoD
                                podVM.FinancialYear = FinancialYear;
                                // podfilePath = EnumHelper.DescriptionAttr(FilePathEnum.EVCPoD);
                                podfilePath = _baseConfig.Value.WebCoreBaseUrl + "EVC\\POD";
                                podHtmlString = GetPoDHtmlString(podVM, "POD");
                                podPDFSave = _htmlToPdfConverterHelper.GeneratePDFAPI(podHtmlString, podfilePath, podPdfName);
                                #endregion

                                #region Generate and Save Debit Note
                                string DNBillNumber = "DN-" + FinancialYear + "-" + InvSrNum;
                                podVM.evcDetailsVM.BillNumberDN = DNBillNumber;
                                debitNotePdfName = "DN-" + FinancialYear.Replace("/", "-") + "-" + InvSrNum + ".pdf";
                                podVM.evcDetailsVM.FinalPriceDN = finalAmountDN;
                                //debitNotefilePath = EnumHelper.DescriptionAttr(FilePathEnum.EVCDebitNote);
                                debitNotefilePath = _baseConfig.Value.WebCoreBaseUrl + "EVC\\DebitNote";
                                debitNoteHtmlString = GetPoDHtmlString(podVM, "EVC_Debit_Note");
                                debitNoteSaved = _htmlToPdfConverterHelper.GeneratePDFAPI(debitNoteHtmlString, debitNotefilePath, debitNotePdfName);
                                #region Update Drop Status In DB  
                                if (debitNoteSaved)
                                {
                                    podVM.DebitNotePdfName = debitNotePdfName;
                                    podVM.DnsrNum = MaxInvSrNum;
                                    podVM.InvSrNum = MaxInvSrNum;
                                    podVM.DebitNoteAmount = finalAmountDN;
                                    DropCompleted = UpdateLGCDropStatus(orderList, podVM, loggedUserId);
                                }
                                #endregion
                                #endregion

                                #region Generate Invoice Pdf
                                if (DropCompleted)
                                {
                                    string InvBillNumber = "INV-" + FinancialYear + "/" + InvSrNum;
                                    podVM.evcDetailsVM.BillNumberInv = InvBillNumber;
                                    invoicePdfName = InvBillNumber.Replace("/", "-") + ".pdf";
                                    podVM.evcDetailsVM.FinalPriceInv = finalAmountInv;
                                    //invoicefilePath = EnumHelper.DescriptionAttr(FilePathEnum.EVCInvoice);
                                    invoicefilePath = _baseConfig.Value.WebCoreBaseUrl + "EVC\\Invoice";
                                    invoiceHtmlString = GetPoDHtmlString(podVM, "EVC_Invoice");
                                    invoiceSaved = _htmlToPdfConverterHelper.GeneratePDFAPI(invoiceHtmlString, invoicefilePath, invoicePdfName);
                                    #region Update Drop Status In DB  
                                    if (invoiceSaved)
                                    {
                                        podVM.InvoicePdfName = invoicePdfName;
                                        podVM.InvSrNum = MaxInvSrNum;
                                        podVM.InvoiceAmount = finalAmountInv;
                                        PostedCompleted = UpdateInvoicePostedStatus(orderList, podVM, loggedUserId);
                                        flag = PostedCompleted;
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "SaveLGCDropStatus", ex);
            }
            return flag;
        }
        #endregion
        public bool UpdateLGCDropStatus(List<TblOrderLgc> tblOrderLgcList, PODViewModel podVM, int loggedUserId)
        {
            #region Variable Declaration
            TblExchangeAbbstatusHistory? tblExchangeAbbstatusHistory = null;
            TblEvcwalletHistory? tblEvcwalletHistory = null;
            TblEvcregistration? tblEvcregistration = null;
            TblWalletTransaction? tblWalletTransaction = null;
            TblLogistic? tblLogistic = null;

            int resultSavedPoDId = 0;
            bool flag = false;
            int setStatusId = Convert.ToInt32(OrderStatusEnum.LGCDrop);
            #endregion

            OrderLGCViewModel orderLGC = new OrderLGCViewModel();
            try
            {
                if (podVM != null && podVM.DriverId > 0 && podVM.EVCId > 0 && podVM.EvcPartnerId > 0)
                {
                    if (tblOrderLgcList != null && tblOrderLgcList.Count > 0)
                    {
                        foreach (var item in tblOrderLgcList)
                        {
                            if (item != null && item.Evcregistration != null && item.Evcpartner != null && item.Logistic != null && item.OrderTrans != null
                                    && (item.OrderTrans.Exchange != null || item.OrderTrans.Abbredemption != null) && item.OrderTrans.TblWalletTransactions != null
                                    && item.OrderTrans.TblWalletTransactions.Count > 0)
                            {
                                var podVMIsExist = podVM.podVMList.FirstOrDefault(x => x.RegdNo == item.Logistic.RegdNo);
                                if (podVMIsExist != null)
                                {
                                    #region Common Implementations for (ABB or Exchange)
                                    TblExchangeOrder tblExchangeOrder = null; TblOrderTran tblOrderTran = null;
                                    TblAbbredemption tblAbbredemption = null;
                                    TblAbbregistration tblAbbregistration = null;
                                    decimal FinalPriceWithoutSweetner = 0; int? customerDetailsId = null; string sponsorOrderNo = null;
                                    #endregion

                                    #region tblInitialization
                                    tblOrderTran = item.OrderTrans;
                                    tblWalletTransaction = tblOrderTran.TblWalletTransactions.FirstOrDefault(x => x.OrderTransId == tblOrderTran.OrderTransId);
                                    tblExchangeOrder = tblOrderTran.Exchange;
                                    tblEvcregistration = item.Evcregistration;
                                    tblLogistic = item.Logistic;
                                    #endregion

                                    if (tblWalletTransaction != null)
                                    {
                                        #region Common Implementations for (ABB or Exchange)
                                        if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                                        {
                                            tblExchangeOrder = tblOrderTran.Exchange;
                                            customerDetailsId = tblExchangeOrder?.CustomerDetailsId;
                                            sponsorOrderNo = tblExchangeOrder?.SponsorOrderNumber;
                                            FinalPriceWithoutSweetner = Convert.ToDecimal(tblExchangeOrder?.FinalExchangePrice) - Convert.ToDecimal(tblExchangeOrder?.Sweetener);
                                        }
                                        else if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.ABB))
                                        {
                                            tblAbbredemption = tblOrderTran.Abbredemption;
                                            tblAbbregistration = tblAbbredemption?.Abbregistration;
                                            customerDetailsId = tblAbbredemption?.CustomerDetailsId;
                                            sponsorOrderNo = tblAbbregistration?.SponsorOrderNo;
                                            FinalPriceWithoutSweetner = Convert.ToDecimal(tblOrderTran?.FinalPriceAfterQc);
                                        }
                                        #endregion

                                        #region Save PoD and Debit Note Details
                                        podVM.RegdNo = tblOrderTran.RegdNo;
                                        podVM.EVCId = tblEvcregistration.EvcregistrationId;
                                        podVM.EvcPartnerId = item.EvcpartnerId;
                                        if (tblExchangeOrder != null && tblExchangeOrder.Id > 0)
                                        {
                                            podVM.ExchangeId = tblExchangeOrder.Id;
                                            podVM.AbbredemptionId = null;
                                        }
                                        else if (tblAbbredemption != null && tblAbbredemption.RedemptionId > 0)
                                        {
                                            podVM.AbbredemptionId = tblAbbredemption.RedemptionId;
                                            podVM.ExchangeId = null;
                                        }

                                        podVM.OrderTransId = tblOrderTran.OrderTransId;
                                        podVM.DebitNoteDate = _currentDatetime;
                                        resultSavedPoDId = ManageExchangePOD(podVM, loggedUserId);
                                        #endregion

                                        #region code to Update drop status in order lgc
                                        item.ActualDropDate = _currentDatetime;
                                        item.StatusId = setStatusId;
                                        item.Evcpodid = resultSavedPoDId;

                                        _orderLGCRepository.Update(item);
                                        _context.Entry(item).State = EntityState.Detached;
                                        _orderLGCRepository.SaveChanges();
                                        #endregion

                                        #region Create EVC Wallet History 
                                        tblEvcwalletHistory = new TblEvcwalletHistory();
                                        tblEvcwalletHistory.EvcregistrationId = (int)tblWalletTransaction.EvcregistrationId;
                                        tblEvcwalletHistory.EvcpartnerId = (int)tblWalletTransaction.EvcpartnerId;
                                        tblEvcwalletHistory.OrderTransId = tblWalletTransaction.OrderTransId;
                                        tblEvcwalletHistory.CurrentWalletAmount = tblEvcregistration.EvcwalletAmount;
                                        tblEvcwalletHistory.FinalOrderAmount = tblWalletTransaction.OrderAmount;
                                        tblEvcwalletHistory.AmountdeductionFlag = true;
                                        tblEvcwalletHistory.BalanceWalletAmount = tblEvcwalletHistory.CurrentWalletAmount - FinalPriceWithoutSweetner; //tblEvcwalletHistory.FinalOrderAmount;
                                        tblEvcwalletHistory.IsActive = true;
                                        tblEvcwalletHistory.CreatedBy = loggedUserId;
                                        tblEvcwalletHistory.CreatedDate = _currentDatetime;
                                        _eVCWalletHistoryRepository.Create(tblEvcwalletHistory);
                                        _eVCWalletHistoryRepository.SaveChanges();
                                        #endregion

                                        #region Update Tbl EVC Registration 
                                        tblEvcregistration.EvcwalletAmount = tblEvcregistration.EvcwalletAmount - FinalPriceWithoutSweetner; //tblWalletTransaction.OrderAmount;
                                        tblEvcregistration.ModifiedBy = loggedUserId;
                                        tblEvcregistration.ModifiedDate = _currentDatetime;
                                        _eVCRepository.Update(tblEvcregistration);
                                        _context.Entry(tblEvcregistration).State = EntityState.Detached;
                                        _eVCRepository.SaveChanges();
                                        #endregion

                                        #region update tblwallettranscation 
                                        tblWalletTransaction.OrderOfDeliverdDate = _currentDatetime;
                                        //Modification Pending: Update OrderOfCompleteDate at the time invoice generate
                                        tblWalletTransaction.StatusId = setStatusId.ToString();
                                        tblWalletTransaction.ModifiedBy = loggedUserId;
                                        tblWalletTransaction.ModifiedDate = _currentDatetime;
                                        _walletTransactionRepository.Update(tblWalletTransaction);
                                        _context.Entry(tblWalletTransaction).State = EntityState.Detached;
                                        _walletTransactionRepository.SaveChanges();
                                        #endregion

                                        #region update statusId in Base tbl Exchange or ABB
                                        if (tblExchangeOrder != null)
                                        {
                                            tblExchangeOrder.StatusId = setStatusId;
                                            tblExchangeOrder.OrderStatus = "Delivered";
                                            tblExchangeOrder.ModifiedBy = loggedUserId;
                                            tblExchangeOrder.ModifiedDate = _currentDatetime;
                                            _exchangeOrderRepository.Update(tblExchangeOrder);
                                            _context.Entry(tblExchangeOrder).State = EntityState.Detached;
                                            _exchangeOrderRepository.SaveChanges();
                                        }
                                        else if (tblAbbredemption != null && tblAbbregistration != null)
                                        {
                                            #region update status on tblAbbRedemption
                                            tblAbbredemption.StatusId = setStatusId;
                                            tblAbbredemption.AbbredemptionStatus = "Delivered";
                                            tblAbbredemption.ModifiedBy = loggedUserId;
                                            tblAbbredemption.ModifiedDate = _currentDatetime;
                                            _abbRedemptionRepository.Update(tblAbbredemption);
                                            _context.Entry(tblAbbredemption).State = EntityState.Detached;
                                            _abbRedemptionRepository.SaveChanges();
                                            #endregion
                                        }
                                        #endregion

                                        #region Update Status Id in tblLogistic
                                        tblLogistic.StatusId = setStatusId;
                                        tblLogistic.Modifiedby = loggedUserId;
                                        tblLogistic.ModifiedDate = _currentDatetime;
                                        _logisticsRepository.Update(tblLogistic);
                                        _context.Entry(tblLogistic).State = EntityState.Detached;
                                        _logisticsRepository.SaveChanges();
                                        #endregion

                                        #region update statusId in tblOrderTrans
                                        tblOrderTran.StatusId = setStatusId;
                                        tblOrderTran.ModifiedBy = loggedUserId;
                                        tblOrderTran.ModifiedDate = _currentDatetime;
                                        _orderTransRepository.Update(tblOrderTran);
                                        _context.Entry(tblOrderTran).State = EntityState.Detached;
                                        _orderTransRepository.SaveChanges();
                                        #endregion

                                        #region Insert into tblexchangeabbhistory
                                        tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                        tblExchangeAbbstatusHistory.OrderType = (int)tblOrderTran.OrderType;
                                        tblExchangeAbbstatusHistory.SponsorOrderNumber = sponsorOrderNo;
                                        tblExchangeAbbstatusHistory.RegdNo = tblOrderTran.RegdNo;
                                        tblExchangeAbbstatusHistory.CustId = customerDetailsId;
                                        tblExchangeAbbstatusHistory.StatusId = setStatusId;
                                        tblExchangeAbbstatusHistory.IsActive = true;
                                        tblExchangeAbbstatusHistory.CreatedBy = loggedUserId;
                                        tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                        tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                                        _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                                        #endregion

                                        #region update TblVehicleJourneyTrackingDetail
                                        int result = _journeyTrackingDetailsRepository.UpdateVehicleJourney(tblOrderTran.OrderTransId, setStatusId, loggedUserId);

                                        #endregion
                                        flag = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "SaveLGCDropStatus", ex);
            }
            return flag;
        }
        public bool UpdateInvoicePostedStatus(List<TblOrderLgc> tblOrderLgcList, PODViewModel podVM, int? loggedUserId)
        {
            TblEvcwalletHistory tblEvcwalletHistory = null;
            TblLogistic tblLogistic = null;
            TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = null;
            bool flag = false;
            List<TblOrderLgc> lgcOrderList = null;
            int setStatusId = Convert.ToInt32(OrderStatusEnum.Posted);

            try
            {
                lgcOrderList = tblOrderLgcList;
                if (lgcOrderList != null && lgcOrderList.Count > 0)
                {
                    #region Update Invoice Details In DB  
                    foreach (var tblOrderLgc in lgcOrderList)
                    {
                        if (tblOrderLgc != null && tblOrderLgc.Evcregistration != null && tblOrderLgc.Evcpartner != null && tblOrderLgc.Logistic != null && tblOrderLgc.OrderTrans != null
                                    && (tblOrderLgc.OrderTrans.Exchange != null || tblOrderLgc.OrderTrans.Abbredemption != null) && tblOrderLgc.OrderTrans.TblWalletTransactions != null
                                    && tblOrderLgc.OrderTrans.TblWalletTransactions.Count > 0)
                        {
                            var podVMIsExist = podVM.podVMList.FirstOrDefault(x => x.RegdNo == tblOrderLgc.Logistic.RegdNo);
                            if (podVMIsExist != null)
                            {
                                #region Common Implementations for (ABB or Exchange)
                                TblExchangeOrder tblExchangeOrder = null; TblOrderTran tblOrderTran = null;
                                TblAbbredemption tblAbbredemption = null;
                                TblAbbregistration tblAbbregistration = null;
                                decimal FinalPriceWithoutSweetner = 0; int? customerDetailsId = null; string sponsorOrderNo = null;
                                #endregion

                                #region Variable Initialization
                                tblOrderTran = tblOrderLgc.OrderTrans;
                                TblWalletTransaction tblWalletTransection = tblOrderTran.TblWalletTransactions.FirstOrDefault(x => x.OrderTransId == tblOrderLgc.OrderTransId);
                                TblEvcpoddetail? tblEvcpoddetail = new TblEvcpoddetail();
                                //tblEvcpoddetail = _evcPoDDetailsRepository.GetSingle(x => x.IsActive == true && x.Id == tblOrderLgc.Evcpodid);
                                //tblEvcpoddetail = _evcPoDDetailsRepository.GetEVCPODDetailsById(tblOrderLgc.Evcpodid);
                                tblEvcpoddetail = _evcPoDDetailsRepository.GetEVCPODDetailsByOrderTransId(tblOrderTran.OrderTransId);
                                //tblEvcpoddetail = tblOrderLgc?.Evcpod;
                                #endregion
                                if (tblWalletTransection != null && tblEvcpoddetail != null)
                                {
                                    #region Common Implementations for (ABB or Exchange)
                                    if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                                    {
                                        tblExchangeOrder = tblOrderTran.Exchange;
                                        customerDetailsId = tblExchangeOrder?.CustomerDetailsId;
                                        sponsorOrderNo = tblExchangeOrder?.SponsorOrderNumber;
                                        FinalPriceWithoutSweetner = Convert.ToDecimal(tblExchangeOrder?.FinalExchangePrice) - Convert.ToDecimal(tblExchangeOrder?.Sweetener);
                                    }
                                    else if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.ABB))
                                    {
                                        tblAbbredemption = tblOrderTran.Abbredemption;
                                        tblAbbregistration = tblAbbredemption?.Abbregistration;
                                        customerDetailsId = tblAbbredemption?.CustomerDetailsId;
                                        sponsorOrderNo = tblAbbregistration?.SponsorOrderNo;
                                        FinalPriceWithoutSweetner = Convert.ToDecimal(tblOrderTran?.FinalPriceAfterQc);
                                    }
                                    #endregion

                                    #region Update TblEVCPODDetails for Invoice
                                    tblEvcpoddetail.InvoicePdfName = podVM.InvoicePdfName;
                                    tblEvcpoddetail.InvSrNum = podVM.InvSrNum;
                                    tblEvcpoddetail.FinancialYear = podVM.FinancialYear;
                                    tblEvcpoddetail.InvoiceDate = _currentDatetime;
                                    tblEvcpoddetail.InvoiceAmount = podVM.InvoiceAmount;
                                    tblEvcpoddetail.ModifiedBy = loggedUserId;
                                    tblEvcpoddetail.ModifiedDate = _currentDatetime;
                                    _evcPoDDetailsRepository.Update(tblEvcpoddetail);

                                    // set Modified flag in your entry
                                    //_context.Entry(tblEvcpoddetail).State = EntityState.Modified;
                                    // save 
                                    _context.SaveChanges();

                                    #endregion

                                    #region Create EVC Wallet History 
                                    tblEvcwalletHistory = new TblEvcwalletHistory();
                                    tblEvcwalletHistory.EvcregistrationId = (int)tblOrderLgc.EvcregistrationId;
                                    tblEvcwalletHistory.EvcpartnerId = (int)tblOrderLgc.EvcpartnerId;
                                    tblEvcwalletHistory.OrderTransId = tblOrderLgc.OrderTransId;
                                    tblEvcwalletHistory.CurrentWalletAmount = tblOrderLgc.Evcregistration.EvcwalletAmount;
                                    tblEvcwalletHistory.FinalOrderAmount = tblWalletTransection.OrderAmount;
                                    tblEvcwalletHistory.AmountdeductionFlag = true;
                                    decimal? EVCPlateformFee = tblEvcwalletHistory.FinalOrderAmount - FinalPriceWithoutSweetner;
                                    tblEvcwalletHistory.BalanceWalletAmount = tblEvcwalletHistory.CurrentWalletAmount - EVCPlateformFee; //tblEvcwalletHistory.FinalOrderAmount;
                                    tblEvcwalletHistory.IsActive = true;
                                    tblEvcwalletHistory.CreatedBy = loggedUserId;
                                    tblEvcwalletHistory.CreatedDate = _currentDatetime;
                                    _eVCWalletHistoryRepository.Create(tblEvcwalletHistory);
                                    _eVCWalletHistoryRepository.SaveChanges();
                                    #endregion

                                    #region Update TblEVCRegistration 
                                    tblOrderLgc.Evcregistration.EvcwalletAmount = tblOrderLgc.Evcregistration.EvcwalletAmount - EVCPlateformFee;
                                    tblOrderLgc.Evcregistration.ModifiedBy = loggedUserId;
                                    tblOrderLgc.Evcregistration.ModifiedDate = _currentDatetime;
                                    _eVCRepository.Update(tblOrderLgc.Evcregistration);
                                    _eVCRepository.SaveChanges();
                                    #endregion

                                    #region update statusId in Base tbl Exchange or ABB
                                    if (tblExchangeOrder != null)
                                    {
                                        tblExchangeOrder.StatusId = setStatusId;
                                        tblExchangeOrder.OrderStatus = "Posted";
                                        tblExchangeOrder.ModifiedBy = loggedUserId;
                                        tblExchangeOrder.ModifiedDate = _currentDatetime;
                                        _exchangeOrderRepository.Update(tblExchangeOrder);
                                        _exchangeOrderRepository.SaveChanges();
                                    }
                                    else if (tblAbbredemption != null && tblAbbregistration != null)
                                    {
                                        #region update status on tblAbbRedemption
                                        tblAbbredemption.StatusId = setStatusId;
                                        tblAbbredemption.AbbredemptionStatus = "Posted";
                                        tblAbbredemption.ModifiedBy = loggedUserId;
                                        tblAbbredemption.ModifiedDate = _currentDatetime;
                                        _abbRedemptionRepository.Update(tblAbbredemption);
                                        _abbRedemptionRepository.SaveChanges();
                                        #endregion
                                    }
                                    #endregion

                                    #region update status in tblordertrans added by PJ  
                                    _orderTransRepository.UpdateTransRecordStatus(tblOrderLgc.OrderTransId, (int)OrderStatusEnum.Posted, loggedUserId);
                                    #endregion

                                    #region update status in tbllogistics added by PJ                                      
                                    tblLogistic = tblOrderLgc.Logistic;
                                    tblLogistic.StatusId = setStatusId;
                                    tblLogistic.Modifiedby = loggedUserId;
                                    tblLogistic.ModifiedDate = _currentDatetime;
                                    _logisticsRepository.Update(tblLogistic);
                                    _logisticsRepository.SaveChanges();
                                    #endregion

                                    #region update tblwallettranscation 
                                    tblWalletTransection.OrderOfCompleteDate = _currentDatetime;
                                    tblWalletTransection.StatusId = setStatusId.ToString();
                                    tblWalletTransection.ModifiedBy = loggedUserId;
                                    tblWalletTransection.ModifiedDate = _currentDatetime;
                                    _walletTransactionRepository.Update(tblWalletTransection);
                                    _walletTransactionRepository.SaveChanges();
                                    #endregion

                                    #region Update TblOrderLgc for Invoice 
                                    tblOrderLgc.IsInvoiceGenerated = true;
                                    tblOrderLgc.StatusId = setStatusId;
                                    tblOrderLgc.ModifiedBy = loggedUserId;
                                    tblOrderLgc.ModifiedDate = _currentDatetime;
                                    _orderLGCRepository.Update(tblOrderLgc);
                                    _orderLGCRepository.SaveChanges();
                                    #endregion

                                    #region Insert into tblexchangeabbhistory
                                    tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                    tblExchangeAbbstatusHistory.OrderType = (int)tblOrderTran.OrderType;
                                    tblExchangeAbbstatusHistory.SponsorOrderNumber = sponsorOrderNo;
                                    tblExchangeAbbstatusHistory.RegdNo = tblOrderTran.RegdNo;
                                    tblExchangeAbbstatusHistory.CustId = customerDetailsId;
                                    tblExchangeAbbstatusHistory.StatusId = setStatusId;
                                    tblExchangeAbbstatusHistory.IsActive = true;
                                    tblExchangeAbbstatusHistory.CreatedBy = loggedUserId;
                                    tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                    tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                                    _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                                    #endregion

                                    #region update TblVehicleJourneyTrackingDetail
                                    //TblVehicleJourneyTrackingDetail OtblVehicleJourneyTrackingDetail = _journeyTrackingDetailsRepository.GetSingle(x => x.OrderTransId == tblOrderTran.OrderTransId && x.IsActive == true);
                                    //int result = _journeyTrackingDetailsRepository.UpdateVehicleJourney(tblOrderTran.OrderTransId, setStatusId, loggedUserId);
                                    TblVehicleJourneyTrackingDetail tblVehicleJourneyTrackingDetail = _context.TblVehicleJourneyTrackingDetails.Where(x => x.IsActive == true && x.OrderTransId == tblOrderTran.OrderTransId).FirstOrDefault();
                                    if (tblVehicleJourneyTrackingDetail != null)
                                    {
                                        tblVehicleJourneyTrackingDetail.OrderDropTime = DateTime.Now;
                                        tblVehicleJourneyTrackingDetail.ModifiedBy = loggedUserId;
                                        tblVehicleJourneyTrackingDetail.ModifiedDate = DateTime.Now;
                                        tblVehicleJourneyTrackingDetail.StatusId = setStatusId;
                                        _context.Update(tblVehicleJourneyTrackingDetail);
                                        _context.SaveChanges();
                                        _context.Entry(tblVehicleJourneyTrackingDetail).State = EntityState.Detached;

                                    }
                                    bool Dropflag = _commonManager.CalculateDriverIncentive((int)tblVehicleJourneyTrackingDetail.OrderTransId);





                                    #endregion
                                    flag = true;
                                }
                            }
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "UpdateInvoicePostedStatus", ex);
            }
            return flag;
        }
        #endregion

        #region Create pdf String for DebitNote, Invoice, POD New Method with Bunch of Rnum
        /// <summary> 
        /// Create pdf String for DebitNote, Invoice, POD
        /// </summary>
        /// <param name="podVM"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns>htmlString</returns>
        public string GetPoDHtmlString(PODViewModel podVM, string HtmlTemplateNameOnly)
        {
            #region Variable Initialization and Price Calculation
            string htmlString = "";
            string fileName = HtmlTemplateNameOnly + ".html";
            string fileNameWithPath = "";
            string baseUrl = _baseConfig.Value.BaseURL;
            var item = podVM.evcDetailsVM;
            podVM.UtcSeel_INV = baseUrl + EnumHelper.DescriptionAttr(FileAddressEnum.UTCACSeel);
            item.PlaceOfSupply = item.State;
            item.CurrentDate = Convert.ToDateTime(_currentDatetime).ToString("dd/MM/yyyy");
            podVM.CurrentDate = item.CurrentDate;
            //DebitNote
            decimal? finalPriceDN = item.FinalPriceDN;
            decimal? gstDN = finalPriceDN * Convert.ToDecimal(RDCELERP.Common.Constant.GeneralConstant.GSTPercentage);
            gstDN = Math.Round((gstDN ?? 0), 2);
            decimal? finalPriceWithGSTDN = finalPriceDN;
            string finalPiceInWordsDN = NumberToWordsConverterHelper.ConvertAmount(Convert.ToDecimal(finalPriceWithGSTDN));
            //Invoice
            decimal? finalPriceInv = item.FinalPriceInv;

            decimal? basePriceInv = 0;

            bool isGSTInclusive = true; // Get this value from BU table. For now its is kept as true;

            if (isGSTInclusive)
            {
                basePriceInv = finalPriceInv / Convert.ToDecimal(RDCELERP.Common.Constant.GeneralConstant.GSTPercentage);
            }
            else
            {
                basePriceInv = finalPriceInv;
            }

            basePriceInv = Math.Round((basePriceInv ?? 0), 2);

            decimal? gstInv = basePriceInv * Convert.ToDecimal(RDCELERP.Common.Constant.GeneralConstant.CGST);
            gstInv = Math.Round((gstInv ?? 0), 2);
            decimal? finalPriceWithGSTInv = finalPriceInv;
            string finalPiceInWordsInv = NumberToWordsConverterHelper.ConvertAmount(Convert.ToDecimal(finalPriceWithGSTInv));
            #endregion
            try
            {
                #region Get Html String Dynamically

                var filePath = _baseConfig.Value.WebCoreBaseUrlCD + "PdfTemplates";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath); //Create directory if it doesn't exist
                }
                fileNameWithPath = string.Concat(filePath, "\\", fileName);
                //D:\Jan2023\Project\RDCELERP.Core.App\wwwroot\DBFiles\EVC\POD
                htmlString = File.ReadAllText(fileNameWithPath);
                #endregion

                #region Code for Replace EVC Details
                if (HtmlTemplateNameOnly == "EVC_Debit_Note" || HtmlTemplateNameOnly == "EVC_Invoice")
                {
                    htmlString = htmlString.Replace("demoEVCName", podVM.evcDetailsVM.Name + "-" + podVM.evcDetailsVM.EvcStoreCode)
                            .Replace("demoAddress", podVM.evcDetailsVM.Address).Replace("demoCity", podVM.evcDetailsVM.City)
                            .Replace("demoState", podVM.evcDetailsVM.State).Replace("demoPincode", podVM.evcDetailsVM.Pincode)
                            .Replace("demoPlaceOfSupply", podVM.evcDetailsVM.PlaceOfSupply).Replace("demoCurrentDate", podVM.evcDetailsVM.CurrentDate)
                            .Replace("demoUtcSeel_INV", podVM.UtcSeel_INV);
                }
                #endregion

                #region Code for Create Bunch of Rnum for PoD, DebitNote and Invoice template
                string RnumBunch = "";
                if (podVM.podVMList != null && podVM.podVMList.Count > 0)
                {
                    if (HtmlTemplateNameOnly == "POD")
                    {
                        foreach (var podItem in podVM.podVMList)
                        {
                            RnumBunch += "<tr>" +
                                "<td> " + podItem.EVCBussinessName + " </td>" +
                        "<td> " + podItem.EVCRegdNo + " </td>" +
                        "<td> " + podItem.TicketNo + " </td>" +
                        "<td> " + podItem.RegdNo + " </td>" +
                     "<td> " + podItem.ProductCatName + " </td>" +
                     "<td> " + podItem.CustName + " </td>" +
                     "<td> " + podItem.CustPincode + " </td>" +
                     "<td> Delivered </td>" +
                     "<td> " + podVM.CurrentDate + " </td><td></td><tr>";
                        }
                    }
                    else if (HtmlTemplateNameOnly == "EVC_Debit_Note")
                    {
                        int srNum = 1;
                        foreach (var podItem in podVM.podVMList)
                        {
                            podItem.FullPoDUrl = baseUrl + podItem.Podurl;
                            RnumBunch += "<tr><td> " + srNum + " </td>" +
                            "<td> " + podItem.RegdNo + " <a href=" + podItem.FullPoDUrl + " target='_blanck'>" + podItem.FullPoDUrl + "</a> " + " </td>" +
                            "<td>1.00</td>" +
                            "<td> " + podItem.OrderAmtForEVCDN + " </td>" +
                            "<td> " + podItem.OrderAmtForEVCDN + " </td><tr>";
                            srNum++;
                        }
                    }
                    else if (HtmlTemplateNameOnly == "EVC_Invoice")
                    {
                        int srNum = 1;
                        foreach (var podItem in podVM.podVMList)
                        {
                            podItem.FullPoDUrl = baseUrl + podItem.Podurl;
                            //UTC Platform fee for EVC
                            decimal? PlatformFee_PerOrder = podItem.OrderAmtForEVCInv;
                            decimal? PlatformFee_BasePerOrder = 0;
                            if (isGSTInclusive)
                            {
                                PlatformFee_BasePerOrder = PlatformFee_PerOrder / Convert.ToDecimal(RDCELERP.Common.Constant.GeneralConstant.GSTPercentage);
                            }
                            else
                            {
                                PlatformFee_BasePerOrder = PlatformFee_PerOrder;
                            }
                            PlatformFee_BasePerOrder = Math.Round((PlatformFee_BasePerOrder ?? 0), 2);
                            PlatformFee_PerOrder = Math.Round((PlatformFee_PerOrder ?? 0), 2);
                            // decimal? GST_PerOrder_PlatformFee = podItem.OrderAmtForEVCInv * Convert.ToDecimal(0.09);
                            decimal? GST_PerOrder_PlatformFee = PlatformFee_BasePerOrder * Convert.ToDecimal(RDCELERP.Common.Constant.GeneralConstant.CGST);
                            GST_PerOrder_PlatformFee = Math.Round((GST_PerOrder_PlatformFee ?? 0), 2);
                            // decimal? PerOrder_PlatformFee_With_GST = podItem.OrderAmtForEVCInv + GST_PerOrder_PlatformFee + GST_PerOrder_PlatformFee;
                            decimal? PerOrder_PlatformFee_With_GST = PlatformFee_BasePerOrder + GST_PerOrder_PlatformFee + GST_PerOrder_PlatformFee;
                            //Create List with Multiple orders
                            RnumBunch += "<tr><td> " + srNum + " </td>" +
                               "<td><span>UTC Bridge Service Fees For Regd No : </span>" + podItem.RegdNo + " <a href=" + podItem.FullPoDUrl + " target='_blanck'>" + podItem.FullPoDUrl + "</a> " + " </td>" +
                                "<td>9961</td>" +
                                "<td>1.00</td>" +
                                "<td>" + Math.Round((PlatformFee_BasePerOrder ?? 0), 2) + "</td>" +
                                "<td>9%</td>" +
                                "<td>" + GST_PerOrder_PlatformFee + "</td>" +
                                "<td>9%</td>" +
                                "<td>" + GST_PerOrder_PlatformFee + "</td>" +
                                "<td>" + PlatformFee_PerOrder + " </td><tr>";
                            srNum++;
                        }
                    }
                    htmlString = htmlString.Replace("demoRnumList", RnumBunch);
                }
                #endregion

                #region Replace final price and BillNumber in Debit Note Or Invoice
                if (HtmlTemplateNameOnly == "EVC_Debit_Note")
                {
                    htmlString = htmlString.Replace("demoBillNumber", podVM.evcDetailsVM.BillNumberDN)
                        .Replace("demoFinalPrice", finalPriceDN.ToString())
                        .Replace("demoFinalAmtInWords", finalPiceInWordsDN);
                }
                else if (HtmlTemplateNameOnly == "EVC_Invoice")
                {
                    htmlString = htmlString.Replace("demoBillNumber", podVM.evcDetailsVM.BillNumberInv)
                        .Replace("demoPriceBeforeAllGST", basePriceInv.ToString()).Replace("demoCGSTAmt", gstInv.ToString())
                        .Replace("demoSGSTAmt", gstInv.ToString()).Replace("demoPriceAfterAllGST", finalPriceWithGSTInv.ToString())
                        .Replace("demoSubtotal", finalPriceWithGSTInv.ToString()).Replace("demoFinalPrice", finalPriceWithGSTInv.ToString())
                        .Replace("demoFinalAmtInWords", finalPiceInWordsInv);
                }
                #endregion

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "GetPoDHtmlString", ex);
            }
            return htmlString;
        }
        #endregion

        #region Store Exchange POD Details in database
        /// <summary>
        ///Exchange POD Details in database
        /// </summary>       
        /// <returns></returns>   
        public int ManageExchangePOD(PODViewModel podVM, int loggedUserId)
        {
            TblEvcpoddetail tblEVCPODDetailObj = new TblEvcpoddetail();
            string fileName = null;
            if (podVM.Podurl != null)
            {
                fileName = podVM.Podurl.Split('/').LastOrDefault();
            }
            else if (podVM.PoDPdfName != null)
            {
                fileName = podVM.PoDPdfName;
            }
            int result = 0;
            int evcPoDId = 0;
            try
            {
                if (podVM.RegdNo != null && (podVM.Podurl != null || podVM.PoDPdfName != null))
                {
                    tblEVCPODDetailObj = _evcPoDDetailsRepository.GetSingle(x => x.IsActive == true && x.RegdNo == podVM.RegdNo);
                    if (tblEVCPODDetailObj != null)
                    {
                        tblEVCPODDetailObj.Podurl = fileName;
                        if (podVM.ExchangeId > 0)
                        {
                            tblEVCPODDetailObj.ExchangeId = podVM.ExchangeId;
                        }
                        if (podVM.AbbredemptionId > 0)
                        {
                            tblEVCPODDetailObj.AbbredemptionId = podVM.AbbredemptionId;
                        }
                        tblEVCPODDetailObj.Evcid = podVM.EVCId;
                        tblEVCPODDetailObj.EvcpartnerId = podVM.EvcPartnerId;
                        tblEVCPODDetailObj.OrderTransId = podVM.OrderTransId;
                        tblEVCPODDetailObj.DebitNotePdfName = podVM.DebitNotePdfName;
                        tblEVCPODDetailObj.DebitNoteDate = podVM.DebitNoteDate;
                        tblEVCPODDetailObj.DebitNoteAmount = podVM.DebitNoteAmount;
                        tblEVCPODDetailObj.DnsrNum = podVM.DnsrNum;
                        tblEVCPODDetailObj.ModifiedBy = loggedUserId;
                        tblEVCPODDetailObj.ModifiedDate = _currentDatetime;
                        _evcPoDDetailsRepository.Update(tblEVCPODDetailObj);
                        //_context.Entry(tblEVCPODDetailObj).State = EntityState.Detached;

                    }
                    else
                    {
                        tblEVCPODDetailObj = new TblEvcpoddetail();
                        tblEVCPODDetailObj = _mapper.Map<PODViewModel, TblEvcpoddetail>(podVM);
                        tblEVCPODDetailObj.Podurl = fileName;
                        tblEVCPODDetailObj.IsActive = true;
                        tblEVCPODDetailObj.CreatedBy = loggedUserId;
                        tblEVCPODDetailObj.CreatedDate = _currentDatetime;
                        _evcPoDDetailsRepository.Create(tblEVCPODDetailObj);
                    }
                    result = _evcPoDDetailsRepository.SaveChanges();
                    if (result > 0)
                    {
                        evcPoDId = tblEVCPODDetailObj.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "ManageExchangePOD", ex);
            }

            return evcPoDId;
        }
        #endregion


        public ResponseResult GetDriverList(int servicePartnerId, string? searchTerm, int? pageNumber, int? pageSize)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            string imagepath = string.Empty;

            try
            {
                if (servicePartnerId > 0)
                {
                    var servicePartner = _servicePartnerRepository.GetSingle(where: x => x.ServicePartnerId == servicePartnerId && x.IsActive == true && x.ServicePartnerIsApprovrd == true);
                    if (servicePartner != null)
                    {
                        List<TblDriverList> query = _context.TblDriverLists.Include(x => x.City).Where(x => x.IsActive == true && x.ServicePartnerId == servicePartnerId && x.IsApproved == true).ToList();

                        // Apply filtering
                        if (!string.IsNullOrWhiteSpace(searchTerm))
                        {
                            query = query.Where(x => x.DriverPhoneNumber.Contains(searchTerm) || x.DriverName.Contains(searchTerm) || x.City.Name.Contains(searchTerm)).ToList();
                        }

                        // Get total count for pagination
                        int totalCount = query.Count();

                        // Apply pagination
                        List<TblDriverList> driverDetails = query.OrderBy(x => x.CreatedDate)
                                                                   .Skip((int)((pageNumber - 1) * pageSize))
                                                                   .Take((int)pageSize)
                                                                   .ToList();

                        // Map to view model
                        List<DriverResponseViewModal> DriverListDetails = new List<DriverResponseViewModal>();
                        foreach (var item in driverDetails)
                        {
                            DriverResponseViewModal DriverlistViewModel = new DriverResponseViewModal();
                            DriverlistViewModel.DriverId = item.DriverId;
                            DriverlistViewModel.DriverName = item.DriverName;
                            DriverlistViewModel.DriverPhoneNumber = item.DriverPhoneNumber;
                            DriverlistViewModel.DriverlicenseNumber = item.DriverLicenseNumber;
                            DriverlistViewModel.ServicePartnerId = item.ServicePartnerId;
                            DriverlistViewModel.CityId = item.CityId;
                            //  TblCity tblCity = _context.TblCities.Where(x=>x.CityId== VehiclelistViewModel.CityId).FirstOrDefault();
                            DriverlistViewModel.cityName = item.City.Name;
                            DriverlistViewModel.CreatedDate = item.CreatedDate;
                            DriverlistViewModel.ModifiedDate = item.ModifiedDate;

                            // Add images path
                            if (!string.IsNullOrWhiteSpace(item.DriverLicenseImage))
                            {
                                imagepath = _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + item.DriverLicenseImage;
                                DriverlistViewModel.DriverlicenseImage = imagepath;
                            }
                            if (!string.IsNullOrWhiteSpace(item.ProfilePicture))
                            {
                                imagepath = _baseConfig.Value.PostERPImagePath + _baseConfig.Value.ProfilePicture + item.ProfilePicture;
                                DriverlistViewModel.ProfilePicture = imagepath;
                            }


                            DriverListDetails.Add(DriverlistViewModel);
                        }

                        // Prepare response
                        DriverListResponseList allDriverList = new DriverListResponseList
                        {
                            DriverLists = DriverListDetails
                        };

                        responseMessage.message = "Details retrieved successfully.";
                        responseMessage.Status = true;
                        responseMessage.Data = allDriverList;
                        //responseMessage.TotalItems = totalCount;
                        //responseMessage.PageNumber = pageNumber;
                        //responseMessage.PageSize = pageSize;
                        responseMessage.Status_Code = HttpStatusCode.OK;
                    }
                    else
                    {
                        responseMessage.message = "Service Partner not found.";
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.OK;
                    }
                }
                else
                {
                    responseMessage.message = "Service Partner ID must be greater than 0.";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("LogisticManager", "ServicePartnerDashboard", ex);
            }

            return responseMessage;
        }


        #endregion

        #region Order savePaymentResponce
        /// <summary>
        /// Api method for savePaymentResponce
        /// Added by Priyanshi
        /// </summary>
        /// <param name="">savePaymentResponce</param>       
        /// <returns></returns>
        public ResponseResult savePaymentResponce(SavePaymentResponce savePaymentResponce)
        {
            TblPaymentLeaser? paymentLedger = null;
            TransactionResponseCashfree transactionResponse = new TransactionResponseCashfree();

            string? subcode = null;
            string? PaymentMode = null;
            string? ModuleType = null;
            string? TransactionType = null;
            int payledger = 0;

            ResponseResult responseResult = new ResponseResult();
            try
            {
                subcode = Convert.ToInt32(CashfreeEnum.Succcess).ToString();
                PaymentMode = EnumHelper.DescriptionAttr(CashfreeEnum.upi);

                TblOrderTran? tblOrderTrans = _orderTransRepository.GetOrderTransByRegdno(savePaymentResponce.Regdno);
                if (tblOrderTrans.OrderType == Convert.ToInt32(OrderTypeEnum.ABB))
                {
                    ModuleType = EnumHelper.DescriptionAttr(CashfreeEnum.ABB);
                }
                else
                {
                    ModuleType = EnumHelper.DescriptionAttr(CashfreeEnum.Exchange);
                }

                TransactionType = EnumHelper.DescriptionAttr(CashfreeEnum.TransactionType);
                TransactionResponseCashfree? transactionResponseCashfree = JsonSerializer.Deserialize<TransactionResponseCashfree>(savePaymentResponce.JsonResponce);

                if (transactionResponseCashfree != null)
                {
                    paymentLedger = _context.TblPaymentLeasers.FirstOrDefault(x => x.RegdNo == savePaymentResponce.Regdno && x.ModuleType == ModuleType && x.IsActive == true && x.PaymentStatus == true);
                    if (paymentLedger == null)
                    {

                        TblPaymentLeaser paymentLedgeradd = new TblPaymentLeaser();
                        paymentLedgeradd.RegdNo = savePaymentResponce.Regdno;
                        paymentLedgeradd.Amount = Convert.ToDecimal(tblOrderTrans?.FinalPriceAfterQc);
                        paymentLedgeradd.PaymentMode = PaymentMode;
                        paymentLedgeradd.OrderId = savePaymentResponce.Regdno;
                        paymentLedgeradd.PaymentDate = DateTime.Now;
                        paymentLedgeradd.ResponseDescription = transactionResponseCashfree.message;
                        paymentLedgeradd.PaymentResponse = transactionResponseCashfree.message;
                        paymentLedgeradd.TransactionId = transactionResponseCashfree.data?.utr != null ? transactionResponseCashfree.data?.utr : string.Empty;
                        paymentLedgeradd.ModuleType = ModuleType;
                        paymentLedgeradd.IsActive = true;
                        paymentLedgeradd.PaymentStatus = true;
                        paymentLedgeradd.ResponseCode = subcode;
                        paymentLedgeradd.TransactionType = TransactionType;
                        paymentLedgeradd.CreatedBy = tblOrderTrans?.ExchangeId;
                        paymentLedgeradd.CreatedDate = DateTime.Now;
                        paymentLedgeradd.ModuleReferenceId = tblOrderTrans?.ExchangeId;
                        _paymentLeaserRepository.Create(paymentLedgeradd);
                        payledger = _paymentLeaserRepository.SaveChanges();

                        if (paymentLedgeradd.Id >= 0)
                        {
                            #region For sending Notification to customer for succesfull payment

                            #endregion

                            responseResult.Status = true;
                            responseResult.message = "Transaction Updated Sucssefully";
                            responseResult.Status_Code = HttpStatusCode.OK;



                        }
                        else
                        {
                            responseResult.Status = true;
                            responseResult.message = "Something Went wrong";
                            responseResult.Status_Code = HttpStatusCode.OK;

                        }

                    }
                    else
                    {
                        responseResult.Status = true;
                        responseResult.message = "Amount= " + paymentLedger.Amount + "/- already paid transaction id=" + paymentLedger.TransactionId;
                        responseResult.Status_Code = HttpStatusCode.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("DriverDetailsManager", "savePaymentResponce", ex);
            }
            return responseResult;
        }
        #endregion

        #region GetVehicleDetails by login ServicePartner/LGC
        /// <summary>
        /// Api method GetVehicleDetails by login ServicePartner/LGC
        /// Added by ashwin
        /// </summary>
        /// <param name="">username</param>
        /// <param name=""></param>
        /// <returns></returns>
        public ResponseResult VehicleDetailsList(string userName)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            string imagepath = string.Empty;
            TblServicePartner tblServicePartner = null;
            List<TblDriverDetail> tblDriverDetail = null;
            List<DriverDetailsResponseViewModal> driverDetailsList = new List<DriverDetailsResponseViewModal>();
            try
            {
                tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerEmailId == userName || x.ServicePartnerMobileNumber == userName);

                if (tblServicePartner != null && tblServicePartner.ServicePartnerId > 0)
                {
                    tblDriverDetail = _driverDetailsRepository.GetList(x => x.IsActive == true && x.CreatedBy == tblServicePartner.UserId).ToList();
                    if (tblDriverDetail != null && tblDriverDetail.Count > 0)
                    {
                        driverDetailsList = _mapper.Map<List<TblDriverDetail>, List<DriverDetailsResponseViewModal>>(tblDriverDetail);
                        if (driverDetailsList != null && driverDetailsList.Count > 0)
                        {
                            foreach (DriverDetailsResponseViewModal item in driverDetailsList)
                            {
                                #region Add images path
                                if (item.DriverlicenseImage != null && item.DriverlicenseImage != string.Empty)
                                {
                                    imagepath = _baseConfig.Value.BaseURL + _baseConfig.Value.ServicePartnerAadhar + item.DriverlicenseImage;

                                    if (imagepath != string.Empty && imagepath != null)
                                    {
                                        item.DriverlicenseImage = "";
                                        item.DriverlicenseImage = imagepath;
                                    }
                                    imagepath = string.Empty;
                                }

                                if (item.VehiclefitnessCertificate != null && item.VehiclefitnessCertificate != string.Empty)
                                {
                                    imagepath = _baseConfig.Value.BaseURL + _baseConfig.Value.ServicePartnerAadhar + item.VehiclefitnessCertificate;

                                    if (imagepath != string.Empty && imagepath != null)
                                    {
                                        item.VehiclefitnessCertificate = "";
                                        item.VehiclefitnessCertificate = imagepath;
                                    }
                                    imagepath = string.Empty;
                                }

                                if (item.VehicleRcCertificate != null && item.VehicleRcCertificate != string.Empty)
                                {
                                    imagepath = _baseConfig.Value.BaseURL + _baseConfig.Value.ServicePartnerCancelledCheque + item.VehicleRcCertificate;

                                    if (imagepath != string.Empty && imagepath != null)
                                    {
                                        item.VehicleRcCertificate = "";
                                        item.VehicleRcCertificate = imagepath;
                                    }
                                    imagepath = string.Empty;
                                }

                                //if (item.DriverInsuranceImage != null && item.DriverInsuranceImage != string.Empty)
                                //{
                                //    imagepath = _baseConfig.Value.BaseURL + _baseConfig.Value.ServicePartnerGST + item.DriverInsuranceImage;

                                //    if (imagepath != string.Empty && imagepath != null)
                                //    {
                                //        item.DriverInsuranceImage = "";
                                //        item.DriverInsuranceImage = imagepath;
                                //    }
                                //    imagepath = string.Empty;
                                //}
                                #endregion
                            }
                            if (driverDetailsList.Count > 0)
                            {
                                responseMessage.Data = driverDetailsList;
                                responseMessage.message = "Success";
                                responseMessage.Status = true;
                                responseMessage.Status_Code = HttpStatusCode.OK;
                            }
                            else
                            {
                                //responseMessage.Data = driverDetailsList;
                                responseMessage.message = "No Record found";
                                responseMessage.Status = false;
                                responseMessage.Status_Code = HttpStatusCode.BadRequest;
                            }

                        }
                        else
                        {
                            responseMessage.message = "No Record found";
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseMessage.message = "Data not map properly";
                        responseMessage.Status = false;
                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseMessage.message = "Not Valid login credential";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("DriverDetailsManager", "GetNumberofVehicle", ex);
            }
            return responseMessage;
        }
        #endregion

        #region DriverDetails By DriverDetails userId
        /// <summary>
        /// Get driver Details by userId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DriverDetailsViewModel GetDriverDetailsByDriverId(int id)
        {
            DriverDetailsViewModel? driverDetailsVM = null;
            TblDriverDetail? tblDriverDetail = null;
            TblServicePartner? tblServicePartner = null;
            try
            {
                if (id > 0)
                {
                    tblDriverDetail = _driverDetailsRepository.GetDriverDetailsById(id);
                    if (tblDriverDetail != null)
                    {
                        driverDetailsVM = _mapper.Map<TblDriverDetail, DriverDetailsViewModel>(tblDriverDetail);

                        driverDetailsVM.DriverlicenseImgSrc = _imageHelper.GetImageSrc(_baseConfig.Value.DriverlicenseImage, tblDriverDetail.DriverlicenseImage);
                        driverDetailsVM.VehiclefitnessCertificateImgSrc = _imageHelper.GetImageSrc(_baseConfig.Value.VehiclefitnessCertificate, tblDriverDetail.VehiclefitnessCertificate);
                        driverDetailsVM.VehicleInsuranceCertificateImgSrc = _imageHelper.GetImageSrc(_baseConfig.Value.VehicleInsuranceCertificate, tblDriverDetail.VehicleInsuranceCertificate);
                        driverDetailsVM.VehicleRcCertificateImgSrc = _imageHelper.GetImageSrc(_baseConfig.Value.VehicleRcCertificate, tblDriverDetail.VehicleRcCertificate);
                        driverDetailsVM.ProfilePictureImgSrc = _imageHelper.GetImageSrc(_baseConfig.Value.ProfilePicture, tblDriverDetail.ProfilePicture);
                        tblServicePartner = _servicePartnerRepository.GetServicePartnerById(driverDetailsVM.ServicePartnerId ?? 0);
                        if (tblServicePartner != null)
                            driverDetailsVM.ServicePartnerBusinessName = tblServicePartner.ServicePartnerBusinessName;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DriverDetailsManager", "GetDriverDetailsByDriverId", ex);
            }
            return driverDetailsVM;
        }
        #endregion

        #region Manage Driver Details
        public ResponseResult ManageVehicle(DriverDetailsViewModel dataModel, int? loggedInUserId)
        {
            TblDriverDetail tblDriverDetail = null;
            bool cancelCheck = false;
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            List<TblDriverDetail>? tblDriverList = null;
            try
            {
                if (dataModel != null)
                {
                    tblDriverDetail = _driverDetailsRepository.GetDriverDetailsById(dataModel.DriverDetailsId);
                    if (tblDriverDetail != null)
                    {
                        #region Add Image in Folder 
                        // DriverlicenseImage
                        if (dataModel.DriverlicenseImageString != null)
                        {
                            string fileName = string.Empty;
                            if (tblDriverDetail.DriverlicenseNumber != null)
                            {
                                fileName = tblDriverDetail.DriverlicenseNumber + "DriverlicenseImage" + ".jpg";
                            }
                            else
                            {
                                fileName = tblDriverDetail.DriverPhoneNumber + "DriverlicenseImage" + ".jpg";
                            }
                            string? filePath = _baseConfig.Value.DriverlicenseImagePath;
                            cancelCheck = _imageHelper.SaveFileFromBase64(dataModel.DriverlicenseImageString, filePath, fileName);
                            if (cancelCheck)
                            {
                                tblDriverDetail.DriverlicenseImage = fileName;
                                cancelCheck = false;
                            }
                        }
                        // VehicleRcCertificate
                        if (dataModel.VehicleRcCertificateString != null)
                        {
                            string fileName = string.Empty;
                            if (tblDriverDetail.VehicleRcNumber != null)
                            {
                                fileName = tblDriverDetail.VehicleRcNumber + "VehicleRcCertificate" + ".jpg";
                            }
                            else
                            {
                                fileName = tblDriverDetail.VehicleNumber + "VehicleRcCertificate" + ".jpg";
                            }
                            string? filePath = _baseConfig.Value.VehicleRcCertificatePath;
                            cancelCheck = _imageHelper.SaveFileFromBase64(dataModel.VehicleRcCertificateString, filePath, fileName);
                            if (cancelCheck)
                            {
                                tblDriverDetail.VehicleRcCertificate = fileName;
                                cancelCheck = false;
                            }
                        }
                        // VehiclefitnessCertificate
                        if (dataModel.VehiclefitnessCertificateString != null)
                        {
                            string fileName = string.Empty;
                            if (tblDriverDetail.VehicleNumber != null)
                            {
                                fileName = tblDriverDetail.VehicleNumber + "VehicleFitnessCertificate" + ".jpg";
                            }
                            else
                            {
                                fileName = tblDriverDetail.VehicleRcNumber + "VehicleFitnessCertificate" + ".jpg";
                            }
                            string? filePath = _baseConfig.Value.VehiclefitnessCertificatePath;
                            cancelCheck = _imageHelper.SaveFileFromBase64(dataModel.VehiclefitnessCertificateString, filePath, fileName);
                            if (cancelCheck)
                            {
                                tblDriverDetail.VehiclefitnessCertificate = fileName;
                                cancelCheck = false;
                            }
                        }
                        // VehicleInsuranceCertificate
                        if (dataModel.VehicleInsuranceCertificateString != null)
                        {
                            string fileName = string.Empty;
                            if (tblDriverDetail.VehicleInsuranceCertificate != null)
                            {
                                fileName = tblDriverDetail.VehicleRcNumber + "VehicleInsuranceCertificate" + ".jpg";
                            }
                            else
                            {

                                fileName = tblDriverDetail.VehicleNumber + "VehicleInsuranceCertificate" + ".jpg";
                            }
                            string? filePath = _baseConfig.Value.VehicleInsuranceCertificatePath;
                            cancelCheck = _imageHelper.SaveFileFromBase64(dataModel.VehicleInsuranceCertificateString, filePath, fileName);
                            if (cancelCheck)
                            {
                                tblDriverDetail.VehicleInsuranceCertificate = fileName;
                                cancelCheck = false;
                            }
                        }
                        // ProfilePicture
                        if (dataModel.ProfilePictureString != null)
                        {
                            string fileName = string.Empty;
                            if (tblDriverDetail.ProfilePicture != null)
                            {
                                fileName = tblDriverDetail.VehicleNumber + "DriverProfilePicture" + ".jpg";
                            }
                            else
                            {
                                fileName = tblDriverDetail.VehicleNumber + "DriverProfilePicture" + ".jpg";
                            }
                            string? filePath = _baseConfig.Value.ProfilePicturePath;
                            cancelCheck = _imageHelper.SaveFileFromBase64(dataModel.ProfilePictureString, filePath, fileName);
                            if (cancelCheck)
                            {
                                tblDriverDetail.ProfilePicture = fileName;
                                cancelCheck = false;
                            }
                        }
                        #endregion

                        tblDriverDetail.DriverName = dataModel.DriverName;
                        tblDriverDetail.DriverPhoneNumber = dataModel.DriverPhoneNumber;
                        tblDriverDetail.DriverlicenseNumber = dataModel.DriverlicenseNumber;
                        tblDriverDetail.VehicleNumber = dataModel.VehicleNumber;
                        tblDriverDetail.VehicleRcNumber = dataModel.VehicleRcNumber;
                        tblDriverDetail.CityId = dataModel.CityId;
                        tblDriverDetail.City = dataModel.City;
                        tblDriverDetail.Modifiedby = loggedInUserId;
                        tblDriverDetail.ModifiedDate = _currentDatetime;
                        _driverDetailsRepository.Update(tblDriverDetail);
                        int result = _driverDetailsRepository.SaveChanges();
                        if (result > 0)
                        {
                            responseMessage.message = "Updated Sucessfully";
                            responseMessage.Status = true;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                        }
                        else
                        {
                            responseMessage.message = "Driver Details Not Updated";
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        tblDriverDetail = _mapper.Map<DriverDetailsViewModel, TblDriverDetail>(dataModel);
                        tblDriverList = _driverDetailsRepository.GetAllDriverDetailsList();
                        if (tblDriverDetail != null)
                        {
                            if (tblDriverList.Exists(p => p.DriverPhoneNumber == tblDriverDetail.DriverPhoneNumber))
                            {
                                responseMessage.message = "This Mobile Number is Already Exists";
                                responseMessage.Status = false;
                                responseMessage.Status_Code = HttpStatusCode.BadRequest;
                                return responseMessage;
                            }
                            else
                            {
                                TblDriverDetail tblDriver = _driverDetailsRepository.GetSingle(x => x.IsActive == true && x.VehicleNumber == dataModel.VehicleNumber || x.VehicleRcNumber == dataModel.VehicleRcNumber);
                                if (tblDriver == null)
                                {
                                    #region Add Image in Folder 
                                    // DriverlicenseImage
                                    if (dataModel.DriverlicenseImageString != null)
                                    {
                                        string fileName = string.Empty;
                                        if (tblDriverDetail.DriverlicenseNumber != null)
                                        {
                                            fileName = tblDriverDetail.DriverlicenseNumber + "DriverlicenseImage" + ".jpg";
                                        }
                                        else
                                        {
                                            fileName = tblDriverDetail.DriverPhoneNumber + "DriverlicenseImage" + ".jpg";
                                        }
                                        string? filePath = _baseConfig.Value.DriverlicenseImagePath;
                                        cancelCheck = _imageHelper.SaveFileFromBase64(dataModel.DriverlicenseImageString, filePath, fileName);
                                        if (cancelCheck)
                                        {
                                            tblDriverDetail.DriverlicenseImage = fileName;
                                            cancelCheck = false;
                                        }
                                    }
                                    // VehicleRcCertificate
                                    if (dataModel.VehicleRcCertificateString != null)
                                    {
                                        string fileName = string.Empty;
                                        if (tblDriverDetail.VehicleRcNumber != null)
                                        {
                                            fileName = tblDriverDetail.VehicleRcNumber + "VehicleRcCertificate" + ".jpg";
                                        }
                                        else
                                        {
                                            fileName = tblDriverDetail.VehicleNumber + "VehicleRcCertificate" + ".jpg";
                                        }
                                        string? filePath = _baseConfig.Value.VehicleRcCertificatePath;
                                        cancelCheck = _imageHelper.SaveFileFromBase64(dataModel.VehicleRcCertificateString, filePath, fileName);
                                        if (cancelCheck)
                                        {
                                            tblDriverDetail.VehicleRcCertificate = fileName;
                                            cancelCheck = false;
                                        }
                                    }
                                    // VehiclefitnessCertificate
                                    if (dataModel.VehiclefitnessCertificateString != null)
                                    {
                                        string fileName = string.Empty;
                                        if (tblDriverDetail.VehicleNumber != null)
                                        {
                                            fileName = tblDriverDetail.VehicleNumber + "VehicleFitnessCertificate" + ".jpg";
                                        }
                                        else
                                        {
                                            fileName = tblDriverDetail.VehicleRcNumber + "VehicleFitnessCertificate" + ".jpg";
                                        }
                                        string? filePath = _baseConfig.Value.VehiclefitnessCertificatePath;
                                        cancelCheck = _imageHelper.SaveFileFromBase64(dataModel.VehiclefitnessCertificateString, filePath, fileName);
                                        if (cancelCheck)
                                        {
                                            tblDriverDetail.VehiclefitnessCertificate = fileName;
                                            cancelCheck = false;
                                        }
                                    }
                                    // VehicleInsuranceCertificate
                                    if (dataModel.VehicleInsuranceCertificateString != null)
                                    {
                                        string fileName = string.Empty;
                                        if (tblDriverDetail.VehicleInsuranceCertificate != null)
                                        {
                                            fileName = tblDriverDetail.VehicleRcNumber + "VehicleInsuranceCertificate" + ".jpg";
                                        }
                                        else
                                        {

                                            fileName = tblDriverDetail.VehicleNumber + "VehicleInsuranceCertificate" + ".jpg";
                                        }
                                        string? filePath = _baseConfig.Value.VehicleInsuranceCertificatePath;
                                        cancelCheck = _imageHelper.SaveFileFromBase64(dataModel.VehicleInsuranceCertificateString, filePath, fileName);
                                        if (cancelCheck)
                                        {
                                            tblDriverDetail.VehicleInsuranceCertificate = fileName;
                                            cancelCheck = false;
                                        }
                                    }
                                    // ProfilePicture
                                    if (dataModel.ProfilePictureString != null)
                                    {
                                        string fileName = string.Empty;
                                        if (tblDriverDetail.ProfilePicture != null)
                                        {
                                            fileName = tblDriverDetail.VehicleNumber + "DriverProfilePicture" + ".jpg";
                                        }
                                        else
                                        {
                                            fileName = tblDriverDetail.VehicleNumber + "DriverProfilePicture" + ".jpg";
                                        }
                                        string? filePath = _baseConfig.Value.ProfilePicturePath;
                                        cancelCheck = _imageHelper.SaveFileFromBase64(dataModel.ProfilePictureString, filePath, fileName);
                                        if (cancelCheck)
                                        {
                                            tblDriverDetail.ProfilePicture = fileName;
                                            cancelCheck = false;
                                        }
                                    }
                                    #endregion

                                    tblDriverDetail.IsActive = true;
                                    tblDriverDetail.CreatedDate = _currentDatetime;
                                    tblDriverDetail.CreatedBy = loggedInUserId;
                                    tblDriverDetail.IsApproved = true;
                                    tblDriverDetail.ApprovedBy = loggedInUserId;

                                    _driverDetailsRepository.Create(tblDriverDetail);
                                    int result = _driverDetailsRepository.SaveChanges();

                                    if (result > 0)
                                    {
                                        int responseUserId = ManageServicePartnerDriverUser(tblDriverDetail);

                                        // Update userID in DriverDetails
                                        if (responseUserId > 0)
                                        {
                                            tblDriverDetail.UserId = responseUserId;
                                            tblDriverDetail.DriverPhoneNumber = SecurityHelper.DecryptString(tblDriverDetail.DriverPhoneNumber, _baseConfig.Value.SecurityKey);
                                            _driverDetailsRepository.Update(tblDriverDetail);
                                            int updateResult = _driverDetailsRepository.SaveChanges();

                                            if (updateResult > 0)
                                            {
                                                responseMessage.message = "Added Success";
                                                responseMessage.Status = true;
                                                responseMessage.Status_Code = HttpStatusCode.OK;
                                            }
                                            else
                                            {
                                                responseMessage.message = "Driver Added but User not Created";
                                                responseMessage.Status = true;
                                                responseMessage.Status_Code = HttpStatusCode.OK;
                                            }
                                        }
                                        else
                                        {
                                            responseMessage.Status = false;
                                            responseMessage.Status_Code = HttpStatusCode.BadRequest;
                                            responseMessage.message = "Registration Failed";
                                        }
                                    }
                                    else
                                    {
                                        responseMessage.Status = false;
                                        responseMessage.Status_Code = HttpStatusCode.BadRequest;
                                        responseMessage.message = "Registration Failed";
                                    }
                                }
                                else
                                {
                                    if (tblDriver.VehicleRcNumber == dataModel.VehicleRcNumber)
                                    {
                                        responseMessage.Status = false;
                                        responseMessage.Status_Code = HttpStatusCode.OK;
                                        responseMessage.message = "Vehicle RC number already exist";
                                    }
                                    else
                                    {
                                        responseMessage.Status = false;
                                        responseMessage.Status_Code = HttpStatusCode.OK;
                                        responseMessage.message = "Vehicle number already exist";
                                    }
                                }
                            }
                        }
                        else
                        {
                            responseMessage.Status = false;
                            responseMessage.Status_Code = HttpStatusCode.OK;
                            responseMessage.message = "Registration Failed";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("DriverDetailsManager", "AddVehicle", ex);
            }

            return responseMessage;
        }
        #endregion

        #region ManageServicePartnerDriverUser
        /// <summary>
        /// Create User For Service Partner Driver
        /// </summary>
        /// <param name="tblServicePartner"></param>
        /// <returns></returns>
        public int ManageServicePartnerDriverUser(TblDriverDetail tblDriverDetail)
        {
            int userId = 0;

            UserViewModel userVM = new UserViewModel();
            TblCompany tblCompany = null;
            TblRole tblRole = null;
            UserRoleViewModel userRoleVM = new UserRoleViewModel();
            try
            {
                if (tblDriverDetail != null)
                {
                    string key1 = _baseConfig.Value.SecurityKey;
                    string phoneNumber = tblDriverDetail.DriverPhoneNumber;
                    tblDriverDetail.DriverPhoneNumber = SecurityHelper.EncryptString(tblDriverDetail.DriverPhoneNumber, key1);

                    TblUser tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.Phone == tblDriverDetail.DriverPhoneNumber);

                    tblCompany = _companyRepository.GetSingle(x => x.IsActive == true && x.CompanyName == EnumHelper.DescriptionAttr(CompanyNameenum.UTC).ToString());
                    tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleName == EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner_Driver).ToString());

                    if (tblUser != null && tblUser.UserId > 0 && tblCompany != null && tblRole != null)
                    {
                        userId = tblUser.UserId;


                        //Code to Insert the object in tbluserrole
                        TblUserRole tblUserRole = new TblUserRole();
                        tblUserRole.RoleId = tblRole.RoleId;
                        tblUserRole.UserId = tblUser.UserId;
                        tblUserRole.CompanyId = tblCompany.CompanyId;
                        tblUserRole.IsActive = true;
                        tblUserRole.CreatedBy = tblDriverDetail.CreatedBy;
                        tblUserRole.CreatedDate = _currentDatetime;

                        _userRoleRepository.Create(tblUserRole);
                        _userRoleRepository.SaveChanges();
                    }
                    else
                    {
                        tblUser = new TblUser();
                        //Code to Insert the object in tbluser
                        tblUser.Phone = tblDriverDetail.DriverPhoneNumber;
                        //tblUser.Phone = SecurityHelper.EncryptString(tblDriverDetail.DriverPhoneNumber, _baseConfig.Value.SecurityKey);                        
                        tblUser.FirstName = tblDriverDetail.DriverName;
                        tblUser.ImageName = tblDriverDetail.ProfilePicture;
                        tblUser.IsActive = true;
                        tblUser.CreatedDate = _currentDatetime;
                        tblUser.CreatedBy = tblDriverDetail.CreatedBy;
                        tblUser.CompanyId = tblCompany.CompanyId;
                        tblUser.UserStatus = "Active";

                        _userRepository.Create(tblUser);
                        _userRepository.SaveChanges();

                        userId = tblUser.UserId;

                        //Code to Insert the object in tbluserrole

                        TblUserRole tblUserRole = new TblUserRole();
                        tblUserRole.RoleId = tblRole.RoleId;
                        tblUserRole.UserId = userId;
                        tblUserRole.CompanyId = tblCompany.CompanyId;
                        tblUserRole.IsActive = true;
                        tblUserRole.CreatedBy = tblDriverDetail.CreatedBy;
                        tblUserRole.CreatedDate = _currentDatetime;

                        _userRoleRepository.Create(tblUserRole);
                        _userRoleRepository.SaveChanges();
                    }

                }
                else
                {
                    userId = 0;
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DriverDetailsManager", "ManageServicePartnerDriverUser", ex);
            }
            return userId;
        }
        #endregion

        #region Get Journey Details by driverId

        public ResponseResult GetJourneyDetailsByServicePIdorDriverId(int? driverId, int servicePartnerId, int? page, int? pageSize, int? vehicleId, DateTime? Journeydate)
        {
            ResponseResult responseResult = new ResponseResult();
            List<StartJournyVehicleListbyServicePResponse> responseList = new List<StartJournyVehicleListbyServicePResponse>();

            try
            {
                IQueryable<TblDriverDetail> tblDriverDetailsList = _context.TblDriverDetails
                    .Include(x => x.Driver).ThenInclude(x => x.City)
                    .Include(x => x.Driver).ThenInclude(x => x.ServicePartner)
                    .Include(x => x.Vehicle).ThenInclude(x => x.City)
                    .Where(x => x.IsActive == true &&
                    ((driverId == null || driverId == 0) ? (x.ServicePartnerId == servicePartnerId) : (x.DriverId == driverId && x.ServicePartnerId == servicePartnerId)));

                if (vehicleId > 0)
                {
                    tblDriverDetailsList = tblDriverDetailsList.Where(x => x.VehicleId == vehicleId);
                }

                if (Journeydate != null &&Journeydate != DateTime.MinValue)
                {
                    tblDriverDetailsList = tblDriverDetailsList.Where(x => x.JourneyPlanDate.Value.Date == Journeydate.Value.Date);
                }

                var tblDriverDetailsListFetched = tblDriverDetailsList.ToList();

                if (tblDriverDetailsListFetched != null && tblDriverDetailsListFetched.Any())
                {
                    foreach (var tblDriverDetail in tblDriverDetailsListFetched)
                    {
                        var tblVehicleJourneyTrackings = _journeyTrackingRepository.GetJourneyDetailsByServicePIdorDriverId(tblDriverDetail.DriverDetailsId, servicePartnerId).ToList();

                        if (tblVehicleJourneyTrackings != null && tblVehicleJourneyTrackings.Any())
                        {
                            foreach (var journey in tblVehicleJourneyTrackings)
                            {
                                responseList.Add(new StartJournyVehicleListbyServicePResponse
                                {
                                    TrackingId = journey.TrackingId,
                                    ServicePartnerId = journey.ServicePartnerId,
                                    DriverDetailsId = journey.DriverId,
                                    JourneyPlanDate = journey.JourneyPlanDate,
                                    CurrentVehicleLatt = journey.CurrentVehicleLatt,
                                    CurrentVehicleLong = journey.CurrentVehicleLong,
                                    JourneyStartDatetime = journey.JourneyStartDatetime,
                                    JourneyEndTime = journey.JourneyEndTime,
                                    driverDetails = new DriverDetailsModel
                                    {
                                        DriverDetailsId = tblDriverDetail?.DriverDetailsId,
                                        DriverName = tblDriverDetail?.Driver ==null? tblDriverDetail?.DriverName: tblDriverDetail?.Driver?.DriverName,
                                        DriverPhoneNumber = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverPhoneNumber : tblDriverDetail?.Driver?.DriverPhoneNumber,
                                        VehicleNumber = tblDriverDetail?.Vehicle == null ? tblDriverDetail?.VehicleNumber : tblDriverDetail?.Vehicle?.VehicleNumber,
                                        City = tblDriverDetail?.Driver == null ? tblDriverDetail?.City : tblDriverDetail?.Driver?.City?.Name,
                                        VehicleRcNumber = tblDriverDetail?.Vehicle == null ? tblDriverDetail?.VehicleRcNumber : tblDriverDetail?.Vehicle?.VehicleRcNumber,
                                        DriverlicenseNumber = tblDriverDetail?.Driver == null ? tblDriverDetail?.DriverlicenseNumber : tblDriverDetail?.Driver?.DriverLicenseNumber,
                                        CityId = tblDriverDetail?.Driver == null ? tblDriverDetail?.CityId : tblDriverDetail?.Driver?.CityId,
                                        ServicePartnerId = tblDriverDetail?.ServicePartnerId,
                                        CreatedDate = tblDriverDetail?.CreatedDate,
                                        ModifiedDate = tblDriverDetail?.ModifiedDate,
                                        DriverId = tblDriverDetail?.DriverId,
                                        VehicleId = tblDriverDetail?.VehicleId,
                                        JourneyPlannedDate = (tblDriverDetail?.JourneyPlanDate!=null? (DateTime)tblDriverDetail?.JourneyPlanDate:DateTime.MinValue),
                                        ProfilePicture = tblDriverDetail?.Driver!=null?_baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Driver?.ProfilePicture:string.Empty,
                                        DriverlicenseImage = tblDriverDetail?.Driver!=null? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Driver?.DriverLicenseImage:string.Empty,
                                        VehicleInsuranceCertificate = tblDriverDetail?.Vehicle!=null?_baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehicleInsuranceCertificate:string.Empty,
                                        VehiclefitnessCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehiclefitnessCertificate:string.Empty,
                                        VehicleRcCertificate = tblDriverDetail?.Vehicle != null ? _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + tblDriverDetail?.Vehicle?.VehicleRcCertificate:string.Empty,
                                    }
                                });
                            }
                        }
                    }

                    if (page.HasValue && pageSize.HasValue && page > 0 && pageSize > 0)
                    {
                        responseList = responseList
                            .OrderByDescending(x => x.JourneyPlanDate)
                            .Skip((page.Value - 1) * pageSize.Value)
                            .Take(pageSize.Value)
                            .ToList();
                    }

                    if (responseList != null && responseList.Any())
                    {
                        responseResult.Status = true;
                        responseResult.Status_Code = HttpStatusCode.OK;
                        responseResult.message = "Details found";
                        responseResult.Data = responseList;
                    }
                    else
                    {
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        responseResult.message = "Details not found";
                        return responseResult;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DriverDetailsManager", "GetJourneyDetailsByServicePIdorDriverId", ex);

                return new ResponseResult
                {
                    Status = false,
                    Status_Code = HttpStatusCode.InternalServerError,
                    message = ex.Message
                };
            }

            return responseResult;
        }



        #endregion

        #region GetAssignDriverToVehicleList
        public ResponseResult GetAssignDriverToVehicleList(int servicePartnerId, string cityName, string? searchTerm, int? pageNumber, int? pageSize, bool isJourneyPlannedForToday)
        {
            ResponseResult responseMessage = new ResponseResult
            {
                message = string.Empty
            };

            try
            {
                if (servicePartnerId <= 0)
                {
                    responseMessage.message = "Service Partner Id Not Found";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    return responseMessage;
                }

                if (string.IsNullOrEmpty(cityName))
                {
                    responseMessage.message = "City Not Found";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    return responseMessage;
                }

                TblServicePartner tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerId == servicePartnerId);

                if (tblServicePartner == null)
                {
                    responseMessage.message = "Invalid login credentials";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    return responseMessage;
                }

                IQueryable<TblDriverList> query = _context.TblDriverLists
                    .Include(x => x.City)
                    .Where(x => x.IsActive == true && x.City.Name.ToLower() == cityName.ToLower() && x.ServicePartnerId == servicePartnerId);

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(x => x.DriverPhoneNumber.Contains(searchTerm) || x.DriverName
                    .Contains(searchTerm));
                }

                List<DriverResponseViewModal> DriverDetailsListFinal = query
                    .OrderBy(x => x.DriverId)
                    .Skip((int)((pageNumber - 1) * pageSize))
                    .Take((int)pageSize)
                    .Select(Driver => new DriverResponseViewModal
                    {
                        cityName = Driver.City.Name,
                        CityId = Driver.CityId ?? 0,
                        DriverId = Driver.DriverId,
                        ServicePartnerId = Driver.ServicePartnerId,
                        DriverPhoneNumber = Driver.DriverPhoneNumber,
                        DriverName = Driver.DriverName,
                        DriverlicenseImage = _baseConfig.Value.PostERPImagePath + _baseConfig.Value.DriverlicenseImage + Driver.DriverLicenseImage,
                        DriverlicenseNumber = Driver.DriverLicenseNumber,
                        ProfilePicture = _baseConfig.Value.PostERPImagePath + _baseConfig.Value.ProfilePicture + Driver.ProfilePicture,
                        CreatedDate = Driver.CreatedDate,
                        ModifiedDate = Driver.ModifiedDate,
                    }).ToList();
                List<DriverResponseViewModal> DriverDetailsListFinal2 = new List<DriverResponseViewModal>();
                foreach (var Driver in DriverDetailsListFinal)
                {
                    DateTime currentDate = _currentDatetime.Date;
                    DateTime nextDate = currentDate.AddDays(1);

                    TblDriverDetail? tblDriverDetail = _context.TblDriverDetails
                       .Include(x => x.Driver).ThenInclude(x => x.City)
                       .FirstOrDefault(x =>
                           x.IsActive == true &&
                           x.DriverId == Driver.DriverId &&
                           x.VehicleId != null &&
                           x.ServicePartnerId == Driver.ServicePartnerId &&
                           (isJourneyPlannedForToday ? x.JourneyPlanDate.Value.Date == currentDate : x.JourneyPlanDate.Value.Date == nextDate)
                       );

                    if (tblDriverDetail == null)
                    {
                        DriverDetailsListFinal2.Add(Driver);
                    }
                }

                if (DriverDetailsListFinal2.Count > 0)
                {
                    responseMessage.Data = DriverDetailsListFinal2;
                    responseMessage.message = "Success";
                    responseMessage.Status = true;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                }
                else
                {
                    responseMessage.message = "No Records found";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("DriverDetailsManager", "GetAssignDriverToVehicleList", ex);
            }

            return responseMessage;
        }
        #endregion

        #region Order UpdateDriverforVehicle by Vehicle
        /// <summary>
        /// Api method for Order StartVehicleJourney by Vehicle
        /// Added by Priyanshi
        /// </summary>
        /// <param name="request">StartVehicleJourney</param>           
        /// <returns></returns>
        public ResponseResult UpdateDriverforVehicle(ReqUpdateDriverforVehicle request)
        {
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            try
            {
                if (request == null || request.DriverId <= 0 || request.servicePartnerId <= 0 || request.vehicleId <= 0)
                {
                    responseMessage.message = "Invalid request. Please provide valid Driver ID, Service Partner ID, and Vehicle ID.";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.BadRequest;
                    return responseMessage;
                }

                // Check if the driver is already associated with another vehicle for the same day
                var currentDate = _currentDatetime.Date;
                var nextDate = currentDate.AddDays(1);


                TblDriverDetail? existingvehicleDetail = _context.TblDriverDetails
                 .Include(x => x.Driver).ThenInclude(x => x.City)
                 .FirstOrDefault(x =>
                 x.IsActive == true &&
                 x.VehicleId == request.vehicleId &&
                     x.ServicePartnerId == request.servicePartnerId &&
                     (request.isJourneyPlannedForToday ? x.JourneyPlanDate.Value.Date == currentDate : x.JourneyPlanDate.Value.Date == nextDate)
                 );
                if (existingvehicleDetail == null)
                {
                    responseMessage.message = "Vehicle not avalible";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    return responseMessage;
                }

                TblDriverDetail? existingDriverDetail = _context.TblDriverDetails
                 .Include(x => x.Driver).ThenInclude(x => x.City)
                 .FirstOrDefault(x =>
                 x.IsActive == true &&
                 x.DriverId == request.DriverId &&
                     x.ServicePartnerId == request.servicePartnerId &&
                     (request.isJourneyPlannedForToday ? x.JourneyPlanDate.Value.Date == currentDate : x.JourneyPlanDate.Value.Date == nextDate)
                 );


                if (existingDriverDetail != null)
                {
                    responseMessage.message = "The driver is already associated with another vehicle for the day.";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                    return responseMessage;
                }

                // Update the driver ID for the vehicle

                TblDriverList? tblDriverList = _context.TblDriverLists.Where(x => x.DriverId == request.DriverId && x.IsActive == true).FirstOrDefault();

                if (tblDriverList != null && existingvehicleDetail != null && existingDriverDetail == null)
                {
                    string journydate = existingvehicleDetail.JourneyPlanDate.Value.Date.ToString();
                    var resultO = _pushNotificationManager.SendNotification(request.servicePartnerId, existingvehicleDetail.DriverDetailsId, EnumHelper.DescriptionAttr(NotificationEnum.Assignmentiscancelled), journydate, null);

                    // Update existing driver detail with new values
                    existingvehicleDetail.DriverName = tblDriverList.DriverName;
                    existingvehicleDetail.DriverPhoneNumber = tblDriverList.DriverPhoneNumber;
                    existingvehicleDetail.Modifiedby = existingvehicleDetail.CreatedBy;
                    existingvehicleDetail.ModifiedDate = _currentDatetime;
                    existingvehicleDetail.DriverId = request.DriverId;
                    existingvehicleDetail.IsActive = true;
                    existingvehicleDetail.UserId = tblDriverList.UserId;

                    _context.TblDriverDetails.Update(existingvehicleDetail);
                    var result = _context.SaveChanges();
                    if (result > 0)
                    {
                        responseMessage.message = "Driver details updated successfully.";
                        responseMessage.Status = true;
                        responseMessage.Status_Code = HttpStatusCode.OK;

                        var resultN = _pushNotificationManager.SendNotification(request.servicePartnerId, existingvehicleDetail.DriverDetailsId, EnumHelper.DescriptionAttr(NotificationEnum.NewJourneyAssigned), existingvehicleDetail.VehicleNumber, journydate.ToString());
                    }
                }
                else
                {
                    responseMessage.message = "Driver details not found.";
                    responseMessage.Status = false;
                    responseMessage.Status_Code = HttpStatusCode.OK;
                }

            }
            catch (Exception ex)
            {
                responseMessage.message = "An error occurred while updating driver ID for the vehicle.";
                responseMessage.Status = false;
                responseMessage.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("DriverDetailsManager", "UpdateDriverforVehicle", ex);
            }
            return responseMessage;
        }


        #endregion

    }
}
