using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using static Org.BouncyCastle.Math.EC.ECCurve;
using RDCELERP.Model.OrderDetails;
using RDCELERP.Model.TemplateConfiguration;

namespace RDCELERP.Reporting.Scheduler.SponsorReporting
{
    public interface IPendingOrderManager : IDisposable
    {
        //public void Dispose();
        #region Start Tab Pending Orders Reporting 

        #region Entry Point Pending Order List Scheduler Calling
        public void ReportingPendingOrders();
        #endregion

        #region Create Html string for integrate pending orders reporting mail template Added by VK
        /// <summary>
        /// Create Html string for integrate pending orders reporting mail template Added by VK
        /// </summary>
        /// <param name="sponsorName"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns></returns>
        public string GetReportingMailHtmlString(string? sponsorName, string HtmlTemplateNameOnly);
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

        #region Export Data To Excel 
        public byte[]? OnPostExportExcel_PendingOrders(OrderDetailsVMExcelList? obj);
        #endregion

        #region Manage Pending Orders Reporting Exchange
        public Task<OrderDetailsVMExcelList> GetExchPendingOrdersListAsync(TblBusinessUnit? tblBusinessUnit, int? spid = null);
        #endregion

        #region Manage Pending Orders Reporting ABB
        public Task<OrderDetailsVMExcelList> GetABBPendingOrdersListAsync(TblBusinessUnit? tblBusinessUnit, int? spid = null);
        #endregion

        #region Get List Pending for QC Exchange Orders list
        public List<PendingForQCVMExcel> GetExchPendingQCList(int? buid, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null, int? statusId2 = null, int? statusId3 = null, int? statusId4 = null,
            int? statusId5 = null, int? statusId6 = null, int? statusId7 = null);
        #endregion

        #region Get List Pending for Price Acceptance Exchange Orders list
        public List<PendingForPriceAcceptVMExcel> GetExchPendingPriceAcceptList(int? buid, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null, int? statusId2 = null);
        #endregion

        #region Get List Pending for Pickup Exchange Orders list
        public List<PendingForPickupVMExcel> GetExchPendingPickupList(int? buid, int? spid, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null);
        #endregion

        #region Get List Pending for QC ABB Orders list
        public List<PendingForQCVMExcel> GetAbbPendingQCList(int? buid, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null, int? statusId2 = null, int? statusId3 = null, int? statusId4 = null,
            int? statusId5 = null, int? statusId6 = null, int? statusId7 = null);
        #endregion

        #region Get List Pending for PriceAcceptance ABB Orders list
        public List<PendingForPriceAcceptVMExcel> GetAbbPendingPriceAcceptList(int? buid, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null, int? statusId2 = null);
        #endregion

        #region Get List Pending for Pickup ABB Orders list
        public List<PendingForPickupVMExcel> GetAbbPendingPickupList(int? buid, int? spid, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null);
        #endregion
        #endregion End Reporting Tab
        
    }
}
