using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Common.Enums;
using RDCELERP.DAL.Entities;
using RDCELERP.Model;
using RDCELERP.Model.Dashboards;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.LGC;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.QC;
using RDCELERP.Model.QCComment;
using RDCELERP.Model.UniversalPriceMaster;
using static NPOI.HSSF.Util.HSSFColor;

namespace RDCELERP.BAL.Interface
{
    public interface IQCCommentManager
    {
        public List<ExchangeOrderViewModel> GetAllRegdNo();
        public QCCommentViewModel GetQCByExchangeId(int Id);
        public int ManageQCComment(QCCommentViewModel QCCommentVM, IList<ImageLabelViewModel> imageLabelViewModels, int UserId);
        QCCommentViewModel GetQCCommentById(int id);
        bool DeletQCCommentById(int id);
        List<ExchangeOrderStatusViewModel> GetQcFlag();
        List<decimal> GetPriceAfterQc(string ExchangeOrderId, string Quailty, string custQuality, int UserId);

        QCCommentViewModel GetQcDetails(int Id);
        QCCommentViewModel GetLGCDetails(int Id);
        VoucherDetailsViewModel GetVoucherDetails(int CustomerId);
        bool Rescheduled(QCCommentViewModel QCCommentVM, int UserId);
        //QCCommentViewModel Rescheduled(QCCommentViewModel QCCommentVM, int UserId);      
        public IList<SelfQCViewModel> GetImagesUploadedBySelfQC(string regdNo);
        public IList<OrderImageUploadViewModel> GetImagesUploadedByVideoQC(string regdNo);
        public IList<OrderImageUploadViewModel> GetImagesUploadedByLGCQC(string regdNo);
        bool GetQCCancelById(string RegdNo, string CancelComment, int UserId);
        //ExchangeOrderStatusViewModel GetQCCancelById(string RegdNo, string CancelComment, int UserId);
        public MessageResponseViewModel saveSelfQCImageAsFinalImage(string RegdNo, int UserId);
        //public void GetCashVoucheronQc(string ExchangeOrderId, string QcQuailty, decimal QCDeclareprice, int UserId);
        public void GetCashVoucheronQc(string ExchangeOrderId, string QcQuailty, decimal QCDeclareprice);
        //public void GenerateVoucher(TblExchangeOrder exchangeOrderDetails, decimal QCDeclareprice, int UserId);
        public void GenerateVoucher(TblExchangeOrder exchangeOrderDetails, decimal QCDeclareprice);
        public int SaveUpino(UPINoViewModel UpinoViewModel);
        public QuestionerViewModel GetProductDetailsByRegdNo(string regdNo);
        public decimal GetASP(int productTypeId, int techId, int brandId);
        public decimal GetNonWorkingPrice(int productTypeId, int techId);
        public List<QCRatingViewModel> GetDynamicQuestionbyProdCatId(int prodCatId);
        public List<double> GetQuotedPrice(List<QCRatingViewModel> qCRatingViewModels);
        public double FinalPriceAfterQCBonus(int bonusCap, double quotedPrice);
        public bool saveForFinalCap(QuestionerViewModel questionerViewModel, List<QCRatingViewModel> qCRatingViewModelList, int? userId);
        public int saveAndSubmit(QuestionerViewModel questionerViewModel, List<QCRatingViewModel> qCRatingViewModelList, int? userId, bool flag = false);
        public decimal saveAnswersbyQuestionId(List<QCRatingViewModel> qCRatingViewModelList, int orderTransId, int? userId, bool flag = false);
        public AdminBonusCapViewModel GetOrderDetailsPendingForUpperCap(int orderTransId);
        public List<QCRatingViewModel> GetQuestionerReport(int orderTransId);
        public bool SaveBonusDetailByAdimn(AdminBonusCapViewModel adminBonusCapViewModel, List<QCRatingViewModel> qCRatingViewModelList, int UserId);
        public ExchangeOrderViewModel GetOrderDetailsById(int id);
        public string FinalQCImagesUploadedByQC(IList<ImageLabelViewModel> imageLabelViewModels, ExchangeOrderViewModel exchangeOrderViewModel, int userId);
        public ResponseResult GetQuestionswithLov(int prodCatId);
        /// <summary>
        /// For add UPI Id in tblorderQc but not update status in any table(Call from beneficiary btn)
        /// </summary>
        /// <param name="UpinoViewModel"></param>
        /// <returns></returns>
        public int SaveUPIIdByExchangemanage(UPINoViewModel UpinoViewModel);
        public string CheckUPIandBeneficiary(string regdno);
        public QCDashboardViewModel GetQCFlagBasedCount(int companyId);
        public TblOrderTran GetOrderDetailsByOrderTransId(int orderTransId);
        public List<QCRatingViewModel> GetQuestionerReportByQCTeam(int orderTransId);
        public QCCommentViewModel GetQCDetailsByOrderTransId(int orderTransId);
        public bool RescheduledQCOrder(QCCommentViewModel QCComment, int UserId);
        //public QCCommentViewModel RescheduledQCOrder(QCCommentViewModel QCComment, int UserId);
        public bool CancelQCOrder(string RegdNo, string CancelComment, int UserId);
        public QCCommentViewModel GetAbbOrderDetailsByTransId(int orderTransId);
        public List<ImageLabelViewModel> GetImageLabelUploadByProductCat(string regdNo);
        public bool SaveAbbQcOrder(QCCommentViewModel qCCommentViewModel, IList<ImageLabelViewModel> imageLabelViewModels, int UserId);
        public string CheckUPIandBeneficiaryForAbb(string regdno);

        public UniversalPMViewModel ValidationbasedGetPriceAfterQc(string exchangerno, int ProdConditionId, string custQuality, int UserId, bool InvoiceV, bool Instollestion, int newbrandid, int Modelnoid);

        public List<BUBasedSweetnerValidation> GetBUSweetnerValidationQuestions(int businessUnitId);
        public bool SaveStatuIdForQcDetails(ExchangeOrderViewModel exchangeOrderViewModel, int userId);
        //public ProductPriceViewModel GetProductPriceforQC(string exchangerno, int QcQuailtyId, string custQuality, int UserId);
        public UniversalPMViewModel GetProductPrice(QCProductPriceDetails QCdetails);
        public UniversalPMViewModel GetBasePrice(QCProductPriceDetails details);

        public ABBVoucherDetailsViewModel GetABBVoucherDetails(int redemptionId);

        #region Diagnose v2
        #region Get ASP by productTypeId, TechId and brandId
        /// <summary>
        /// Get ASP by productTypeId, TechId and brandId
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <param name="techId"></param>
        /// <param name="brandId"></param>
        /// <returns></returns>
        //public decimal GetASPV2(int productTypeId, int techId, int brandId);
        public ProdCatBrandMapViewModel GetASPV2(int productTypeId, int techId, int brandId);
        #endregion

        #region GetNewQuestionswithLov V2 --yash rathore
        public ResponseResult GetNewQuestionswithLov(int prodCatId, int prodtypeid, int prodtechid);

        #endregion

        #region Get age Que v2--yash R
        ResponseResult GetAgeQuestionwithLov(int catid);
        #endregion

        #region GetNewQuotedPrice
        public List<double> GetNewQuotedPrice(List<QCRatingViewModel> qCRatingViewModels);
        #endregion

        #region
        public double GetnewQueWeighatgeV2(int condition, int qcRatingId, int ProductCatId);
        #region getquev2 for erp
        //public List<QCRatingViewModel> GetDynamicQuestsionbyProdCatIdV2(int prodCatId);

        public List<QCRatingViewModel> GetDynamicQuestsionbyProdCatIdV2(int? prodTypeId, int? prodTechId);
        #endregion
        #region saveAnswersbyQuestionIdV2 -YR
        public decimal saveAnswersbyQuestionIdV2(List<QCRatingViewModel> qCRatingViewModelList, int orderTransId, int? userId, bool flag = false);
        #endregion
        #endregion


        #region Get Media Files for QC
        public string[] OnPostGetMediaFiles(string? regdNo);

        #endregion
    }
    #endregion
}
