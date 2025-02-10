using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.CashfreeModel;
using RDCELERP.Model.DriverDetails;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.LGC;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.LGC;
using RDCELERP.Model.Users;

namespace RDCELERP.BAL.Interface
{
    public interface IDriverDetailsManager
    {
        public ResponseResult VehicleDetailsList(string userName);
        public DriverResponseViewModal GetDriverDetailsById(int id);
        public ResponseResult VehiclelistbyLGCIdandCityId(int lgcId, string cityName, bool isJourneyPlannedForToday, int pageNumber, int pageSize, string? filterBy = null);
        public ResponseResult GetAssignOrderListByIdDriverDetailsId(int DriverId,bool isJourneyPlannedForToday);
        public ResponseResult RejectOrderbyVehicle(int OrdertransId, int DriverDetailId, string RejectComment);
        public ResponseResult AcceptOrderbyVehicle(AcceptOrderbyVehicleRequest request);
        public ResponseResult GetAcceptOrderListByDriverId(int DriverId, DateTime journeyDate,int StatusId);
        public ResponseResult StartVehicleJourney(StartVehicleJourney request);
        public ResponseResult PickupDonebyVehicle([FromForm] pickupDoneReq request);
        public string GetCustDeclartionhtmlstring(TblOrderTran tblOrderTran, string HtmlTemplateNameOnly);
        public ResponseResult DropDonebyVehicle([FromForm] DropDoneReq request);
        public ResponseResult savePaymentResponce(SavePaymentResponce savePaymentResponce);

        #region DriverDetails By DriverDetails userId
        /// <summary>
        /// DriverDetails By DriverDetails userId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DriverDetailsViewModel GetDriverDetailsByDriverId(int id);
        #endregion

        #region Manage Driver Details
        public ResponseResult ManageVehicle(DriverDetailsViewModel dataModel, int? loggedInUserId);
        #endregion

        #region ManageServicePartnerDriverUser
        /// <summary>
        /// Create User For Service Partner Driver
        /// </summary>
        /// <param name="tblServicePartner"></param>
        /// <returns></returns>
        public int ManageServicePartnerDriverUser(TblDriverDetail tblDriverDetail);
        #endregion

        public ResponseResult GetJourneyDetailsByServicePIdorDriverId(int? driverId, int servicePartnerId, int? page, int? pagesize,int? vehicleId,DateTime? Journeydate);
        public ResponseResult GetDriverList(int servicePartnerId, string? searchTerm, int? pageNumber, int? pageSize);
        public ResponseResult GetAssignDriverToVehicleList(int servicePartnerId, string cityName, string? searchTerm, int? pageNumber, int? pageSize, bool isJourneyPlannedForToday);
        public ResponseResult UpdateDriverforVehicle(ReqUpdateDriverforVehicle request);
    }
}
