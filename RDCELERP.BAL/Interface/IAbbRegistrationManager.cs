using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.ABBRedemption;
using RDCELERP.Model.AbbRegistration;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.Company;
using RDCELERP.Model.Master;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.PinCode;
using RDCELERP.Model.Product;
using RDCELERP.Model.TemplateConfiguration;

namespace RDCELERP.BAL.Interface
{
    public interface IAbbRegistrationManager
    {       
        int SaveABBRegDetails(AbbRegistrationModel abbRegistrationModel);      
        public AbbRegistrationModel GetABBRegistrationId(int id);
        AbbRegistrationModel GetAllRegdNoList();
        IList<AbbRegistrationModel> GetAllABBRegistrationDetails();
        public AbbRegistrationModel GetCustDetailsByMob(string custmob);
        public BusinessPartnerViewModel GetStoremanageemail(string storecode);
        public IList<ProductTypeViewModel> GetProductTypeBycategory(int NewProductCategoryId);       
        //public IList<PinCodeViewModel> GetPincodeonselectedCity(string CustCity);        
        public IList<PinCodeViewModel> GetStatebyPincode(int CustPinCode);      
        public List<TblAbbregistration> GetRnoAutoComplete(string regdNum);
        public int SaveExchangeABBHistory(string regdno, int UserId, int approvestatus);
        public List<TblModelNumber> GetModelNoAutoComplete(string modelno);
        public List<TblModelNumber> GetModelNumListByProdCatAndProdType(int? prodCatId, int? prodTypeId, int BuId);

        public ResponseResult CreateAbbOrder(AbbRegistrationModel RequestAbbRegistrationModel, string username);

        public List<BuProductCatDataModel> GetAllProductCategoryByAbbPlanMaster();
        public IList<ProductTypeViewModel> GetAllProductTypeByAbbPlanMaster(int NewProductCategoryId);
        public ABBRegistrationViewModel ManageABBBulk(ABBRegistrationViewModel AbbVM, int userId);

        public List<BrandViewModel> GetAllBrandForAbbD2c(int NewProductCategoryId);

        #region Create pdf string for ABB Approval Certificate for Customer Added by VK
        /// <summary>
        /// Create pdf string for ABB Approval Certificate for Customer Added by VK
        /// </summary>
        /// <param name="ABBRegistrationViewModel"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns></returns>
        public string GetCertificateHtmlString(ABBRegistrationViewModel abbRegVM, string HtmlTemplateNameOnly);
        #endregion

        #region Send ABB Approval Certificate Added by VK
        /// <summary>
        /// Create pdf string for ABB Approval Certificate for Customer Added by VK
        /// </summary>
        /// <param name="ABBRegistrationViewModel"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns></returns>
        public bool? SendABBWelcomeMailToCust(ABBRegistrationViewModel aBBRegistrationVM, TemplateViewModel templateVM);
        #endregion

        #region ABB Approve
        /// <summary>
        /// ABB Approve
        /// </summary>
        /// <param name="ABBRegistrationViewModel"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns></returns>
        public bool? ABBApproveById(int? ABBregistrationId, int? loggedInUserId);
        #endregion

        #region Send ABB Email
        /// <summary>
        /// Send ABB Email
        /// </summary>
        /// <param name="tblAbbregistrations"></param>
        /// <returns></returns>
        bool? SendABBEmail(TblAbbregistration tblAbbregistrations, int? loggedInUserId);
        #endregion


    }
}
