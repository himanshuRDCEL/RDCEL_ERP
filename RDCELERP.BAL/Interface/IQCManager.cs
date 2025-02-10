using System.Collections.Generic;
using RDCELERP.Model.ImageLabel;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.QC;

namespace RDCELERP.BAL.Interface
{
    public interface IQCManager
    {
        public List<ImageLabelViewModel> SelfQC(string regno);
        //public bool AddSelfQCImageToDB(List<ImageLabelViewModel> imageLabelViewModels);
        public bool AddSelfQCImageToDB(SelfQcVideoImageViewModel selfQcVideoImageViewModel);
        public List<SelfQCViewModel> GetImageUploadedByCustomer(string regno);
        public bool saveCustomerUploadedImage(string regno);
        public SelfQCExchangeDetailsViewModel getOrderDetailsbyRegdno(string regdno);
        public bool verifyDuplicateSelfQC(string regdno);
        public List<ImageLabelViewModel> SelfQCFlipkart(string regdno);

        #region Self QC Enhancements
        #region Get User Id Which user is Send the Self QC Link to the Customer.
        /// <summary>
        /// Get User Id Which user is Send the Self QC Link to the Customer.
        /// </summary>
        /// <param name="regdno"></param>
        /// <returns>UserId</returns>
        public int? GetSelfLinkSenderUserId(string regdno);
        #endregion

        #region Save Media Files into the Folder
        /// <summary>
        /// Save Media Files into the Folder
        /// </summary>
        /// <param name="regdNo"></param>
        /// <param name="base64String"></param>
        /// <param name="isMediaTypeVideo"></param>
        /// <param name="srNum"></param>
        /// <param name="orderTransId"></param>
        /// <param name="statusId"></param>
        /// <param name="imageLabelId"></param>
        /// <returns></returns>
        public bool SaveMediaFile(string? regdNo, string? base64String, bool isMediaTypeVideo, int srNum, int? orderTransId, int? statusId, int? imageLabelId);
        #endregion

        #region Delete Media Files
        /// <summary>
        /// Delete Media Files
        /// </summary>
        /// <param name="regdNo"></param>
        /// <param name="base64String"></param>
        /// <param name="isMediaTypeVideo"></param>
        /// <param name="srNum"></param>
        /// <returns></returns>
        public bool DeleteMediaFile(string? regdNo, bool isMediaTypeVideo, int srNum);
        #endregion

        #region Save the customer provided images to DB
        /// <summary>
        /// Save the customer provided images to DB
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="imageLabelViewModel"></param>
        /// <returns>bool</returns>
        public bool UpdateSelfQCImageToDB(SelfQcVideoImageViewModel selfQcVideoImageViewModel);
        #endregion

        #region Common method to Get Image Labels from Image Label Master by RegdNo
        /// <summary>
        /// Common method to Get Image Labels from Image Label Master by RegdNo
        /// </summary>
        /// <param name="regdno"></param>
        /// <returns></returns>
        public List<ImageLabelViewModel> GetQCImageLabels(string regdno);
        #endregion
        #endregion
    }
}
