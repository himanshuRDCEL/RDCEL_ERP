using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.DriverDetails;
using RDCELERP.Model.EVC_Portal;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.LGC;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.LGC;
using RDCELERP.Model.ServicePartner;

namespace RDCELERP.BAL.Interface
{
    public interface ILogisticManager
    {
        public TblServicePartner GetServicePartnerDetails(int userId);
        public IList<OrderImageUploadViewModel> GetImagesUploadedFromLGCPickup(string regdNo);
        public int ManageOrderLGC(OrderLGCViewModel orderLGCVM, int loggedUserId);
        public List<ImageLabelViewModel> GetImageLabelUploadByProductCat(string regdNo);
        public List<OrderImageUploadViewModel> GetImageUploadedByQC(string regdNo);
        public bool AddFinalQCImageToDB(IList<ImageLabelViewModel> imageLabelViewModels, LGCOrderViewModel lgcOrderViewModel, int userId);
        public bool AddRejectedOrderStatusToDB(string regdNo, string Commant, int userId);
        public int ManageExchangePOD(PODViewModel podVM, int loggedUserId);
        public string GetPoDHtmlString(PODViewModel podVM, string HtmlTemplateNameOnly);
        public List<TblOrderLgc> GetOrderLGCListByDriverIdEVCId(int? DriverId, int? EVCRegistrationId);
        /*public bool SaveLGCDropImages(IList<ImageLabelViewModel> imageLabelViewModels, PODViewModel podVM, int loggedUserId);*/
        public bool SaveLGCDropStatus(PODViewModel podVM, int loggedUserId);

        public List<TblOrderLgc> GetCityAndEvcList(int UserId);
        public bool saveListOfLoads(DriverDetailsViewModel driverDetailsViewModel, int LoggedInUserId);
        public JsonResult getEvcListByCityId(int cityId);
        public List<TblOrderLgc> GenerateInvoiceForEVC(int? EvcRegistrationId, int? StatusId);
        public IList<TblServicePartner> SelectServicePartner();
        public IList<TblExchangeOrderStatus> GetExchangeOrderStatusByLGCDepartment();

        ServicePartnerDashboardViewModel servicePartnerDashboard(int? ServicePartnerId);

        public ResponseResult LGCRegister(RegisterServicePartnerDataModel LGCRegisterationModel);
        public bool ReOpenLGCOrder(int OrderTransId,int loggedInUserId,string Comment);
        int RescheduledLGC(string RegdNo, String RescheduleComment, DateTime? RescheduleDate, int UserId);

        bool LGCPayNow(string RegdNo);
        public bool CancelTicketByUTC(int OrderTransId, int loggedInUserId, string Comment);

        /// <summary>
        /// Get LGC Pickup Order Details by RegdNum
        /// </summary>
        /// <param name="regdNo"></param>
        /// <returns></returns>
        public LGCOrderViewModel GetLGCPickupOrderDetailsByRegdNo(string regdNo);

        /// <summary>
        /// Get BU based PayNow redirection Link
        /// </summary>
        /// <param name="RegdNo"></param>
        /// <returns></returns>
        public string GetLGCPayNowLinkBasedOnBU(string RegdNo);

        #region Get Driver Details by DriverDetailsId
        /// <summary>
        /// Get Driver Details by DriverDetailsId
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns>DriverDetailsViewModel</returns>
        public DriverDetailsViewModel GetDriverDetailsById(int driverId);
        #endregion

        #region Get details of service partner by userid
        /// <summary>
        /// Get details of service partner by userid
        /// </summary>
        /// <returns>tblServicePartner<LGCOrderViewModel></returns>
        public ServicePartnerViewModel GetServicePartnerByUserId(int userId);
        #endregion

        #region Get Driver Details by TrackingId
        public DriverDetailsViewModel GetDriverDetailsByTrackingId(int trackingId);
        #endregion

        #region Get Service Partner Details by TrackingId
        /// <summary>
        /// Get Service Partner Details by TrackingId
        /// </summary>
        /// <param name="trackingId"></param>
        /// <returns></returns>
        public ServicePartnerViewModel GetSPDetailsByTrackingId(int trackingId);
        #endregion

        #region Get Service Partner Details by Id
        /// <summary>
        /// Get Service Partner Details by Id
        /// </summary>
        /// <param name="servicePartnerId"></param>
        /// <returns></returns>
        public ServicePartnerViewModel GetSPDetailsById(int servicePartnerId);
        #endregion

        #region Get LGC Drop Order Details by EVC Partner Id
        /// <summary>
        /// Get LGC Drop Order Details by Driver Id and EVC Partner Id
        /// </summary>
        /// <param name="evcPartnerId"></param>
        /// <returns></returns>
        public LGCOrderViewModel GetLGCDropDetails(int? evcPartnerId);
        #endregion
    }
}
