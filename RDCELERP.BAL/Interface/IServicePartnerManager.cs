using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.City;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.Common;
using RDCELERP.Model.MobileApplicationModel.LGC;
using RDCELERP.Model.PinCode;
using RDCELERP.Model.ServicePartner;


namespace RDCELERP.BAL.Interface
{
    public interface IServicePartnerManager
    {
        /// <summary>
        /// Method to manage (Add/Edit) ServicePartner 
        /// </summary>
        /// <param name="ServicePartnerVM">ServicePartnerVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageSevicePartner(ServicePartnerViewModel SevicePartnerVM, int userId);

        /// <summary>
        /// Method to get the Service Partner by id 
        /// </summary>
        /// <param name="id">ServicePartnerId</param>
        /// <returns>ServicePartnerViewModel</returns>
        public ServicePartnerViewModel GetServicePartnerById(int id);

        /// <summary>
        /// Method to delete Service Partner by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletSevicePartnerById(int id);

        /// <summary>
        /// Method to get the All Service Partner
        /// </summary>     
        /// <returns>ServicePartnerViewModel</returns>
        public IList<ServicePartnerViewModel> GetAllServicePartner();

        public ResponseResult LGCRegister(RegisterServicePartnerDataModel LGCRegisterationModel);

        public ResponseResult AddVehicle(VehicleDataModel dataModel);

        public ResponseResult OrderRegdNoByDriverId(int id);

        public ResponseResult AcceptOrder(string OrderRegdNo, bool isAccepted);

        public ResponseResult ServicePartnerDetails(int id);

        public OtpWithUserInfo IsValidMobileNumber(string mobNumber);

        public ResponseResult GetLoginUserDetails(string mobNumber, string UserRoleName, int userId);

        public bool UpdateMobileLogindetails(string DeviceType, string DeviceId, int userId);

        public ResponseResult GetServicePartnerByUserId(string email, string password);
        public bool CheckNumberOrEmail(IsNumberorEmailExits isNumberExits);

        public ResponseResult GetOrderListById(int id, int? status, int? page, int? pageSize, string? CityName, int? DriverId);

        public ResponseResult RejectOrderbyLGC(int OrdertransId, int LGCId, string commentbyLGC);

        public ResponseResult GetNumberofVehicle(string userName);

        public ResponseResult OrderAssignLGCtoDriverid(int OrdertransId, int LGCId, int DriverDetailId);
        public ResponseResult GetOrderDetailsByOTId(int OrdertransId);
        public ServicePartnerViewModel ManageSevicePartnerBulk(ServicePartnerViewModel SevicePartnerVM, int userId);
        public ResponseResult UserProfileDetails(int userid);

        public ResponseResult StartJournyVehicleListbyServiceP(int ServicePartnerId, DateTime journeyDate);

        public ResponseResult GetCurrentLetLogforVehicle(int DriverDetailsId, int servicePartnerId, DateTime journeyDate);
        public ResponseResult ServicePartnerDashboard(DateTime? date, int servicePartnerId, int? driverId);
        public ResponseResult SPWalletSummeryList(DateTime? date, int servicePartnerId, int? driverId);
        public ResponseResult VehicleList(int servicePartnerId, string? searchTerm, int? pageNumber, int? pageSize);
        public ResponseResult SPWalletSummeryCount(DateTime date, int servicePartnerId, int? driverId);
        public ResponseResult UpdatePincodesServicePartner([FromForm] UpdatePinCodeServicePartner data);
        public ResponseResult UpdateServicePartner([FromForm] UpdateServicePartnerDataModel LGCRegisterationModel);
        public ResponseResult UpdateVehicle([FromForm] UpdateVehicleDataModel dataModel);
        //public ResponseResult ServicePartnerOrderList(int ServicePartner);

        #region Add LGC Vehicle by Service partner
        /// <summary>
        /// Add LGC Vehicle by Service partner
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public ResponseResult AddVehicleBySP([FromForm] DriverDetailsDataModel dataModel);
        #endregion

        #region Order Assign to Vehicle by LGC/Service Partner
        /// <summary>
        /// Assign Order To Vehicle by LGC partner
        /// Api  Order Assign to Vehicle by LGC/Service Partner
        /// added by VK
        /// </summary>
        /// <param name="OrdertransId"></param>
        /// <param name="LGCId"></param>
        /// <param name="driverDetailsId"></param>
        /// <returns></returns>
        public AssignOrderResponse AssignOrdertoVehiclebyLGC(AssignOrderRequest request);
        #endregion

        #region Order reject  by LGC/Service Partner
        /// <summary>
        /// Order reject  by LGC/Service Partner
        /// </summary>
        /// <param name="Regdno"></param>
        /// <param name="DriverDetailsId"></param>
        /// <returns></returns>
        public ResponseResult RejectOrderbyLGC(RejectLGCOrderRequest request);
        #endregion

        #region Get Order city list by LGC Id
        /// <summary>
        /// Get Order city list by LGC Id
        /// added by VK
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>responseResult</returns>
        public ResponseResult GetOrdercitylistbyLgcId(int LGCId);
        #endregion
        public ResponseResult GetOrderListBySPId(int LGCId);

        #region Get Order specific Eraning Details of Driver by Order Tracking Details Id
        /// <summary>
        /// Get Order specific Eraning Details of Driver by Order Tracking Details Id
        /// </summary>
        /// <param name="trackingDetailsId"></param>
        /// <returns></returns>
        public ResponseResult GetDriverEarningDetailsById(int? trackingDetailsId, int servicePartnerId = 0);
        #endregion

        public ResponseResult GetOrderListOfUninitiatedPaymentByUserId(int userId, int? page, int? pageSize);
        public ResponseResult AddDriver([FromForm] DriverDataModel dataModel);
        public ResponseResult UpdateDriver([FromForm] UpdateDriverDataModel dataModel);
        public ResponseResult DisableDriver(DisableDriverDataModel dataModel);
        public ResponseResult DisableVehicle(DisableVehicleDataModel dataModel);
        public ResponseResult PlanJourneyList(PlanJourneyListDataModel dataModel);
    }
}
