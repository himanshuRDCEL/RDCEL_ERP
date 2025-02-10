using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.BusinessUnit;
using RDCELERP.Model.Master;
using RDCELERP.Model.OrderDetails;
using RDCELERP.Model.OrderTrans;
using RDCELERP.Model.TemplateConfiguration;

namespace RDCELERP.BAL.Interface
{
    public interface IBusinessUnitManager
    {
        /// <summary>
        /// Method to manage (Add/Edit) Business Unit 
        /// </summary>
        /// <param name="BusinessUnitVM">BusinessUnitVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageBusinessUnit(BusinessUnitViewModel BusinessUnitVM, int userId);

        /// <summary>
        /// Method to get the Business Unit by id 
        /// </summary>
        /// <param name="id">BusinessUnitId</param>
        /// <returns>BusinessUnitViewModel</returns>
        public BusinessUnitViewModel GetBusinessUnitById(int id);

        /// <summary>
        /// Method to delete Business Unit by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletBusinessUnitById(int id);

        /// <summary>
        /// Method to get the All Business Unit
        /// </summary>     
        /// <returns>BusinessUnitViewModel</returns>
        public IList<BusinessUnitViewModel> GetAllBusinessUnit();
        public BusinessUnitViewModel GetDefaultProductByModelBaised(int id);

        /// <summary>
        /// Method to manage (Add/Edit) Business Unit in Bulk
        /// </summary>
        /// <param name="BusinessUnitVM">BusinessUnitVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public BusinessUnitViewModel ManageBusinessPartnerBulk(BusinessUnitViewModel businessUnitVM, int loggedInUserId);
        #region Manage Price Master Upload By Excel
        /// <summary>
        /// Method to manage (Add/Edit) BusinessPartner
        /// </summary>
        /// <param name="BusinessPartnerVM">BusinessPartnerVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public BusinessUnitViewModel ManageExchangePriceMasterBulk(BusinessUnitViewModel businessUnitVM, int loggedInUserId);
        #endregion

        /// <summary>
        /// Method to get the BusinessPartner List Count and Exchange Price mater list Count by BuId 
        /// </summary>
        /// <param name="id">BuId</param>
        /// <returns>List<BusinessPartnerViewModel></returns>
        public CountBUOnboardingListDataVM GetCountBUOnboardingListData(int BUId);
        /// <summary>
        /// Store ABB and Exchange Product Category Mapping for BU (New Products only)
        /// </summary>
        /// <param name="BusinessUnitVM"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public int SaveBuProductCategoriesForABBandExch(BusinessUnitViewModel BusinessUnitVM, int loggedInUserId);
        /// <summary>
        /// Get Product Category list for Bu Product category Mapping
        /// </summary>     
        /// <returns>ProductCategoryViewModel</returns>
        public IList<ProductCategoryViewModel> GetAllProductCategoryBuMapping(int BUId);

        /// <summary>
        /// Store Login Creadentials for BU Dashboard, BU Api and Business Partner
        /// </summary>
        /// <param name="BusinessUnitVM"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public int SaveBUCredentials(BusinessUnitViewModel BusinessUnitVM, int loggedInUserId);

        /// <summary>
        /// Get Login Details for BU Api Configurations
        /// </summary>
        /// <param name="BUId"></param>
        /// <returns></returns>
        public BusinessUnitLoginVM GetBULoginCredentials(int BUId);
        public bool GenerateBUQRCode(BusinessUnitLoginVM BULoginVM, string URL, string customFileName);
        public bool GenerateBPBUQRCode(BusinessUnitLoginVM BULoginVM, string? URL, string? customFileName);
        
        #region Manage Pending Orders Reporting
        public OrderDetailsVMExcelList GetExchPendingOrdersList(TblBusinessUnit? tblBusinessUnit, int? spid = null);
        #endregion

        #region Send Reporting Mails for Pending Orders Added by VK
        /// <summary>
        /// Send Reporting Mails for Pending Orders Added by VK
        /// </summary>
        /// <param name="ABBRegistrationViewModel"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns></returns>
        public bool? SendReportingMail(TblBusinessUnit tblBusinessUnit);
        #endregion
        #region Get Pending Order List For Scheduler Calling
        public bool? ReportingPendingOrders();
        #endregion

        #region Get Product Category associated with Product Types
        public List<ProductCategoryViewModel> GetDataForCategoryAndType(int? Buid);
        #endregion
    }
}
