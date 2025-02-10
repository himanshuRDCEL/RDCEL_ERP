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
using RDCELERP.DAL.Repository;
using RDCELERP.DAL.IRepository;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using NPOI.SS.Formula.Functions;
using Mailjet.Client.TransactionalEmails.Response;
using Mailjet.Client.TransactionalEmails;
using Mailjet.Client;
using Mailjet.Client.Resources;

namespace RDCELERP.Reporting.Scheduler.SponsorReporting
{
    internal class PendingOrderManager : IPendingOrderManager
    {
        private IBusinessUnitRepository _businessUnitRepository;
        private ILogging _logging;
        private IOrderTransRepository _orderTransRepository;
        private IWebHostEnvironment _webHostEnvironment;
        private IOptions<ApplicationSettings> _config;
        private ITemplateConfigurationRepository _templateConfigurationRepository;
        private IOrderQCRepository _orderQCRepository;
        
        public PendingOrderManager(IBusinessUnitRepository businessUnitRepository, ILogging logging, IOrderTransRepository orderTransRepository, ITemplateConfigurationRepository templateConfigurationRepository, IOrderQCRepository orderQCRepository, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config)
        {
            _businessUnitRepository = businessUnitRepository;
            _logging = logging;
            _orderTransRepository = orderTransRepository;
            _templateConfigurationRepository = templateConfigurationRepository;
            _orderQCRepository = orderQCRepository;
            _webHostEnvironment = webHostEnvironment;
            _config = config;
        }
       
        public void Dispose()
        {

        }
        #region Start Tab Pending Orders Reporting 

        #region Entry Point Pending Order List Scheduler Calling
        public void ReportingPendingOrders()
        {
            List<TblBusinessUnit>? businessUnitList = null;
            bool? flag = false;
            int errorResultCount = 0;
            try
            {
                businessUnitList = _businessUnitRepository.GetSponsorListForReporting();
                if (businessUnitList != null && businessUnitList.Count > 0)
                {
                    foreach (TblBusinessUnit tblBusinessUnit in businessUnitList)
                    {
                        flag = SendReportingMail(tblBusinessUnit);
                        if (flag == false)
                        {
                            errorResultCount++;
                        }
                    }
                }
                if (errorResultCount > 0)
                {
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PendingOrderManager", "ReportingPendingOrders", ex);
            }
            //return flag;
        }
        #endregion

        #region Create Html string for integrate pending orders reporting mail template Added by VK
        /// <summary>
        /// Create Html string for integrate pending orders reporting mail template Added by VK
        /// </summary>
        /// <param name="sponsorName"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns></returns>
        public string GetReportingMailHtmlString(string? sponsorName, string HtmlTemplateNameOnly)
        {
            #region Variable Declaration
            string htmlString = "";
            string fileName = HtmlTemplateNameOnly + ".html";
            string fileNameWithPath = "";
            #endregion

            try
            {
                if (sponsorName != null && HtmlTemplateNameOnly != null)
                {
                    string? supportEmail = _config.Value.SupportEmail;
                    //string supportConNo = "";
                    #region Get Html String Dynamically
                    var filePath = string.Concat(_webHostEnvironment.WebRootPath, @"\MailTemplates");
                    fileNameWithPath = string.Concat(filePath, "\\", fileName);
                    htmlString = File.ReadAllText(fileNameWithPath);
                    #endregion

                    if (HtmlTemplateNameOnly == TemplateConfigConstant.PendingOrderMailTempName)
                        htmlString = htmlString.Replace("[SponsorName]", sponsorName)
                            .Replace("[SupportEmail]", supportEmail);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PendingOrderManager", "GetReportingMailHtmlString", ex);
            }
            return htmlString;
        }
        #endregion

        #region Send Reporting Mails for Pending Orders Added by VK
        /// <summary>
        /// Send Reporting Mails for Pending Orders Added by VK
        /// </summary>
        /// <param name="ABBRegistrationViewModel"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns></returns>
        public bool? SendReportingMail(TblBusinessUnit tblBusinessUnit)
        {
            #region Variable Declaration
            string mailTempName = "";
            string mailSubject = "";
            Task<bool>? IsMailSent = null;
            bool? flag = false;
            IList<TblConfiguration>? tblConfigurationList = null;
            string? ABB_Bcc = null;
            TemplateViewModel templateVM = new TemplateViewModel();
            //OrderDetailsVMExcelList? orderDetailsListExch = null;
            //OrderDetailsVMExcelList? orderDetailsListAbb = null;

            Task<OrderDetailsVMExcelList>? orderDetailsListExch = null;
            Task<OrderDetailsVMExcelList>? orderDetailsListAbb = null;


            byte[]? byteArrayExch = null;
            byte[]? byteArrayAbb = null;
            #endregion

            try
            {
                mailTempName = TemplateConfigConstant.PendingOrderMailTempName;
                mailSubject = TemplateConfigConstant.PendingOrderMailSubject;

                if (tblBusinessUnit != null)
                {
                    if (tblBusinessUnit.IsReportingOn == true)
                    {
                        #region Get List of Pending Orders For Exchange
                        orderDetailsListExch = GetExchPendingOrdersListAsync(tblBusinessUnit);
                        //if (orderDetailsListExch != null)
                        //{
                        //    byteArrayExch = OnPostExportExcel_PendingOrders(orderDetailsListExch);
                        //}
                        #endregion

                        #region Get List of Pending Orders for ABB
                        orderDetailsListAbb = GetABBPendingOrdersListAsync(tblBusinessUnit);
                        //if (orderDetailsListAbb != null)
                        //{
                        //    byteArrayAbb = OnPostExportExcel_PendingOrders(orderDetailsListAbb);
                        //}
                        #endregion

                        #region wait for the list
                        if (orderDetailsListExch != null && orderDetailsListExch.IsCompleted)
                        {
                            byteArrayExch = OnPostExportExcel_PendingOrders(orderDetailsListExch.Result);
                        }
                        if (orderDetailsListAbb != null && orderDetailsListAbb.IsCompleted)
                        {
                            byteArrayAbb = OnPostExportExcel_PendingOrders(orderDetailsListAbb.Result);
                        }
                        #endregion


                        #region Code for Get Data from TblConfiguration
                        tblConfigurationList = _templateConfigurationRepository.GetConfigurationList();
                        if (tblConfigurationList != null && tblConfigurationList.Count > 0)
                        {
                            var GetBcc = tblConfigurationList.FirstOrDefault(x => x.Name == ConfigurationEnum.ABB_Bcc.ToString());
                            if (GetBcc != null && GetBcc.Value != null)
                            {
                                ABB_Bcc = GetBcc.Value.Trim();
                            }
                        }
                        #endregion

                        #region Send Mail to Sponsor
                        if (byteArrayExch != null || byteArrayAbb != null)
                        {
                            var Date = System.DateTime.Now;
                            var dateTime = Date.ToString("MM-dd-yyyy_hh:mm");
                            templateVM.HtmlBody = GetReportingMailHtmlString(tblBusinessUnit.Name, mailTempName);
                            var FileNameExch = "PendingOrdersExc" + "_" + dateTime + ".xlsx";
                            var FileNameAbb = "PendingOrdersAbb" + "_" + dateTime + ".xlsx";
                            templateVM.AttachFileNameExch = FileNameExch;
                            templateVM.AttachFileNameAbb = FileNameAbb;
                            templateVM.byteArrayExch = byteArrayExch;
                            templateVM.byteArrayAbb = byteArrayAbb;
                            templateVM.To = tblBusinessUnit.Email;
                            templateVM.Cc = tblBusinessUnit.ReportEmails;
                            templateVM.Bcc = ABB_Bcc;
                            templateVM.Subject = mailSubject;
                            IsMailSent = JetMailSendWithAttachedFile(templateVM);
                            if (IsMailSent.Result)
                            {
                                flag = IsMailSent.Result;
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PendingOrderManager", "SendReportingMail", ex);
            }
            return flag;
        }
        #endregion

        #region Export Data To Excel 
        public byte[]? OnPostExportExcel_PendingOrders(OrderDetailsVMExcelList? obj)
        {
            byte[]? data = null;
            try
            {
                if (obj != null)
                {
                    MemoryStream stream = ExcelExportHelper.MultiListExportToExcel(obj.pendingForQCList, obj.pendingForPriceAcceptanceList, obj.pendingForPickupList);
                    data = stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PendingOrderManager", "ExportExcel_PendingOrders", ex);
            }
            return data;
        }
        #endregion

        #region Manage Pending Orders Reporting Exchange
        public async Task<OrderDetailsVMExcelList>  GetExchPendingOrdersListAsync(TblBusinessUnit? tblBusinessUnit, int? spid = null)
        {
            #region Variable Declaration
            OrderDetailsVMExcelList? obj = new OrderDetailsVMExcelList();
            DateTime? DateTimeByElapsedHours = null;
            List<TblOrderTran>? obj1 = null;
            List<TblOrderTran>? obj2 = null;
            List<TblLogistic>? obj3 = null;
            int? ElapsedHrs = 0; int? buid = null;
            PendingForQCVMExcel? pendingForQCVMExcel = null;
            #endregion

            try
            {
                #region Elapsed Hours from the Current Dates
                if (tblBusinessUnit != null)
                {
                    DateTime DateTimeByElapsedHours1 = DateTime.Now;
                    ElapsedHrs = tblBusinessUnit.OrderPendingTimeH;
                    DateTimeByElapsedHours = DateTimeByElapsedHours1.AddHours(-(ElapsedHrs ?? 0));
                    buid = tblBusinessUnit.BusinessUnitId;
                }
                #endregion

                #region Order Status
                int statusId1 = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                int statusId2 = Convert.ToInt32(OrderStatusEnum.QCInProgress_3Q);
                int statusId3 = Convert.ToInt32(OrderStatusEnum.CallAndGoScheduledAppointmentTaken_3P);
                int statusId4 = Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled);
                int statusId5 = Convert.ToInt32(OrderStatusEnum.ReopenOrder);
                int statusId6 = Convert.ToInt32(OrderStatusEnum.InstalledbySponsor);
                int statusId7 = Convert.ToInt32(OrderStatusEnum.OrderWithDiagnostic);
                int statusId8 = Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval);
                int statusId9 = Convert.ToInt32(OrderStatusEnum.QCByPass);
                int statusId10 = Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated);
                #endregion

                if (buid > 0)
                {
                    obj.pendingForQCList = GetExchPendingQCList(buid, DateTimeByElapsedHours, statusId1
                        , statusId2, statusId3, statusId4, statusId5, statusId6, statusId7);
                    obj.pendingForPriceAcceptanceList = GetExchPendingPriceAcceptList(buid, DateTimeByElapsedHours, statusId8, statusId9);
                    obj.pendingForPickupList = GetExchPendingPickupList(buid, spid, DateTimeByElapsedHours, statusId10);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PendingOrderManager", "GetExchPendingOrdersList", ex);
            }
            return obj;
        }
        #endregion

        #region Manage Pending Orders Reporting ABB
        public async Task<OrderDetailsVMExcelList> GetABBPendingOrdersListAsync(TblBusinessUnit? tblBusinessUnit, int? spid = null)
        {
            #region Variable Declaration
            OrderDetailsVMExcelList? obj = new OrderDetailsVMExcelList();
            DateTime? DateTimeByElapsedHours = null;
            List<TblOrderTran>? obj1 = null;
            List<TblOrderTran>? obj2 = null;
            List<TblLogistic>? obj3 = null;
            int? ElapsedHrs = 0; int? buid = null;
            #endregion

            try
            {
                #region Elapsed Hours from the Current Dates
                if (tblBusinessUnit != null)
                {
                    DateTime DateTimeByElapsedHours1 = DateTime.Now;
                    ElapsedHrs = tblBusinessUnit.OrderPendingTimeH;
                    DateTimeByElapsedHours = DateTimeByElapsedHours1.AddHours(-(ElapsedHrs ?? 0));
                    buid = tblBusinessUnit.BusinessUnitId;
                }
                #endregion

                #region Order Status
                int statusId1 = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                int statusId2 = Convert.ToInt32(OrderStatusEnum.QCInProgress_3Q);
                int statusId3 = Convert.ToInt32(OrderStatusEnum.CallAndGoScheduledAppointmentTaken_3P);
                int statusId4 = Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled);
                int statusId5 = Convert.ToInt32(OrderStatusEnum.ReopenOrder);
                int statusId6 = Convert.ToInt32(OrderStatusEnum.InstalledbySponsor);
                int statusId7 = Convert.ToInt32(OrderStatusEnum.OrderWithDiagnostic);
                int statusId8 = Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval);
                int statusId9 = Convert.ToInt32(OrderStatusEnum.QCByPass);
                int statusId10 = Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated);
                #endregion

                if (buid > 0)
                {
                    obj.pendingForQCList = GetAbbPendingQCList(buid, DateTimeByElapsedHours, statusId1
                        , statusId2, statusId3, statusId4, statusId5, statusId6, statusId7);
                    obj.pendingForPriceAcceptanceList = GetAbbPendingPriceAcceptList(buid, DateTimeByElapsedHours, statusId8, statusId9);
                    obj.pendingForPickupList = GetAbbPendingPickupList(buid, spid, DateTimeByElapsedHours, statusId10);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PendingOrderManager", "GetABBPendingOrdersList", ex);
            }
            return obj;
        }
        #endregion

        #region Get List Pending for QC Exchange Orders list
        public List<PendingForQCVMExcel> GetExchPendingQCList(int? buid, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null, int? statusId2 = null, int? statusId3 = null, int? statusId4 = null,
            int? statusId5 = null, int? statusId6 = null, int? statusId7 = null)
        {
            #region Variable Declaration
            PendingForQCVMExcel? pendingForQCVMExcel = null;
            List<PendingForQCVMExcel>? pendingForQCVMList = new List<PendingForQCVMExcel>();
            List<TblOrderTran>? obj1 = null;
            bool? isFilterByModifiedDate = false;
            #endregion

            try
            {
                if (buid > 0)
                {
                    obj1 = _orderTransRepository.GetExchOrdersPendingList(buid, isFilterByModifiedDate, DateTimeByElapsedHours, statusId1
                        , statusId2, statusId3, statusId4, statusId5, statusId6, statusId7);
                    if (obj1 != null && obj1.Count > 0)
                    {
                        foreach (TblOrderTran tblOrderTran in obj1)
                        {
                            pendingForQCVMExcel = new PendingForQCVMExcel();
                            pendingForQCVMExcel.CompanyName = tblOrderTran?.Exchange?.CompanyName;
                            pendingForQCVMExcel.RegdNo = tblOrderTran?.RegdNo;
                            pendingForQCVMExcel.CustomerCity = tblOrderTran?.Exchange?.CustomerDetails?.City;
                            pendingForQCVMExcel.ProductCategory = tblOrderTran?.Exchange?.ProductType?.ProductCat?.Description;
                            pendingForQCVMExcel.ProductCondition = tblOrderTran?.Exchange?.ProductCondition;
                            pendingForQCVMExcel.StatusCode = tblOrderTran?.Status?.StatusCode;
                            pendingForQCVMExcel.OrderDueDateTime = tblOrderTran?.ModifiedDate != null ? Convert.ToDateTime(tblOrderTran?.ModifiedDate).ToString("dd/MM/yyyy hh:mm tt") : null;
                            pendingForQCVMExcel.OrderCreatedDate = tblOrderTran?.CreatedDate != null ? Convert.ToDateTime(tblOrderTran?.CreatedDate).ToString("dd/MM/yyyy") : null;
                            TblOrderQc tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran?.OrderTransId ?? 0);
                            if (tblOrderQc != null && tblOrderQc.ProposedQcdate != null)
                            {
                                pendingForQCVMExcel.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                pendingForQCVMExcel.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                            }
                            pendingForQCVMList.Add(pendingForQCVMExcel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PendingOrderManager", "GetExchPendingOrdersList", ex);
            }
            return pendingForQCVMList;
        }
        #endregion

        #region Get List Pending for Price Acceptance Exchange Orders list
        public List<PendingForPriceAcceptVMExcel> GetExchPendingPriceAcceptList(int? buid, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null, int? statusId2 = null)
        {
            #region Variable Declaration
            PendingForPriceAcceptVMExcel? pendingForPriceAcceptVMExcel = null;
            List<PendingForPriceAcceptVMExcel>? pendingForPriceAcceptVMList = new List<PendingForPriceAcceptVMExcel>();
            List<TblOrderTran>? obj1 = null;
            bool? isFilterByModifiedDate = false;
            #endregion

            try
            {
                if (buid > 0)
                {
                    obj1 = _orderTransRepository.GetExchOrdersPendingList(buid, isFilterByModifiedDate, DateTimeByElapsedHours, statusId1
                        , statusId2);
                    if (obj1 != null && obj1.Count > 0)
                    {
                        foreach (TblOrderTran tblOrderTran in obj1)
                        {
                            pendingForPriceAcceptVMExcel = new PendingForPriceAcceptVMExcel();
                            pendingForPriceAcceptVMExcel.CompanyName = tblOrderTran?.Exchange?.CompanyName;
                            pendingForPriceAcceptVMExcel.RegdNo = tblOrderTran?.RegdNo;
                            pendingForPriceAcceptVMExcel.CustomerCity = tblOrderTran?.Exchange?.CustomerDetails?.City;
                            pendingForPriceAcceptVMExcel.ProductCategory = tblOrderTran?.Exchange?.ProductType?.ProductCat?.Description;
                            pendingForPriceAcceptVMExcel.ProductCondition = tblOrderTran?.Exchange?.ProductCondition;
                            pendingForPriceAcceptVMExcel.StatusCode = tblOrderTran?.Status?.StatusCode;
                            pendingForPriceAcceptVMExcel.OrderDueDateTime = tblOrderTran?.ModifiedDate != null ? Convert.ToDateTime(tblOrderTran?.ModifiedDate).ToString("dd/MM/yyyy hh:mm tt") : null;
                            pendingForPriceAcceptVMExcel.OrderCreatedDate = tblOrderTran?.CreatedDate != null ? Convert.ToDateTime(tblOrderTran?.CreatedDate).ToString("dd/MM/yyyy") : null;

                            //TblOrderQc tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran?.OrderTransId ?? 0);
                            //if (tblOrderQc != null && tblOrderQc.ProposedQcdate != null)
                            //{
                            //    pendingForPriceAcceptVMExcel.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                            //    pendingForPriceAcceptVMExcel.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                            //}
                            pendingForPriceAcceptVMList.Add(pendingForPriceAcceptVMExcel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PendingOrderManager", "GetExchPendingPriceAcceptList", ex);
            }
            return pendingForPriceAcceptVMList;
        }
        #endregion

        #region Get List Pending for Pickup Exchange Orders list
        public List<PendingForPickupVMExcel> GetExchPendingPickupList(int? buid, int? spid, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null)
        {
            #region Variable Declaration
            PendingForPickupVMExcel? pendingForPickupVMExcel = null;
            List<PendingForPickupVMExcel>? pendingForPickupVMList = new List<PendingForPickupVMExcel>();
            List<TblLogistic>? obj1 = null;
            #endregion

            try
            {
                if (buid > 0)
                {
                    obj1 = _orderTransRepository.GetExchOrdersPickupPendingList(buid, spid, DateTimeByElapsedHours, statusId1);
                    if (obj1 != null && obj1.Count > 0)
                    {
                        foreach (TblLogistic tblLogistic in obj1)
                        {
                            pendingForPickupVMExcel = new PendingForPickupVMExcel();
                            pendingForPickupVMExcel.CompanyName = tblLogistic?.OrderTrans?.Exchange?.CompanyName;
                            pendingForPickupVMExcel.RegdNo = tblLogistic?.RegdNo;
                            pendingForPickupVMExcel.ServicePartnerName = tblLogistic?.ServicePartner?.ServicePartnerBusinessName;
                            pendingForPickupVMExcel.CustomerCity = tblLogistic?.OrderTrans?.Exchange?.CustomerDetails?.City;
                            pendingForPickupVMExcel.ProductCategory = tblLogistic?.OrderTrans?.Exchange?.ProductType?.ProductCat?.Description;
                            pendingForPickupVMExcel.ProductCondition = tblLogistic?.OrderTrans?.Exchange?.ProductCondition;
                            pendingForPickupVMExcel.StatusCode = tblLogistic?.Status?.StatusCode;
                            //pendingForPickupVMExcel.OrderDueDateTime = tblLogistic?.OrderTrans?.CreatedDate != null ? tblLogistic?.OrderTrans?.CreatedDate.ToString() : null;
                            pendingForPickupVMExcel.TicketNumber = tblLogistic?.TicketNumber;
                            pendingForPickupVMExcel.PickupScheduleDate = tblLogistic?.PickupScheduleDate != null ? tblLogistic?.PickupScheduleDate.ToString() : null;
                            pendingForPickupVMExcel.OrderDueDateTime = tblLogistic?.CreatedDate != null ? Convert.ToDateTime(tblLogistic?.CreatedDate).ToString("dd/MM/yyyy hh:mm tt") : null;

                            pendingForPickupVMList.Add(pendingForPickupVMExcel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PendingOrderManager", "GetExchPendingPickupList", ex);
            }
            return pendingForPickupVMList;
        }
        #endregion

        #region Get List Pending for QC ABB Orders list
        public List<PendingForQCVMExcel> GetAbbPendingQCList(int? buid, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null, int? statusId2 = null, int? statusId3 = null, int? statusId4 = null,
            int? statusId5 = null, int? statusId6 = null, int? statusId7 = null)
        {
            #region Variable Declaration
            PendingForQCVMExcel? pendingForQCVMExcel = null;
            List<PendingForQCVMExcel>? pendingForQCVMList = new List<PendingForQCVMExcel>();
            List<TblOrderTran>? obj1 = null;
            bool? isFilterByModifiedDate = false;
            #endregion

            try
            {
                if (buid > 0)
                {
                    obj1 = _orderTransRepository.GetABBOrdersPendingList(buid, isFilterByModifiedDate, DateTimeByElapsedHours, statusId1
                        , statusId2, statusId3, statusId4, statusId5, statusId6, statusId7);
                    if (obj1 != null && obj1.Count > 0)
                    {
                        foreach (TblOrderTran tblOrderTran in obj1)
                        {
                            pendingForQCVMExcel = new PendingForQCVMExcel();
                            pendingForQCVMExcel.CompanyName = tblOrderTran?.Abbredemption?.Abbregistration?.BusinessUnit?.Name;
                            pendingForQCVMExcel.RegdNo = tblOrderTran?.RegdNo;
                            pendingForQCVMExcel.CustomerCity = tblOrderTran?.Abbredemption?.CustomerDetails?.City;
                            pendingForQCVMExcel.ProductCategory = tblOrderTran?.Abbredemption?.Abbregistration?.NewProductCategory?.DescriptionForAbb;
                            pendingForQCVMExcel.StatusCode = tblOrderTran?.Status?.StatusCode;
                            pendingForQCVMExcel.OrderDueDateTime = tblOrderTran?.ModifiedDate != null ? Convert.ToDateTime(tblOrderTran?.ModifiedDate).ToString("dd/MM/yyyy hh:mm tt") : null;
                            pendingForQCVMExcel.OrderCreatedDate = tblOrderTran?.CreatedDate != null ? Convert.ToDateTime(tblOrderTran?.CreatedDate).ToString("dd/MM/yyyy") : null;

                            TblOrderQc tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran?.OrderTransId ?? 0);
                            if (tblOrderQc != null && tblOrderQc.ProposedQcdate != null)
                            {
                                pendingForQCVMExcel.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                pendingForQCVMExcel.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                            }
                            pendingForQCVMExcel.ProductCondition = tblOrderQc?.QualityAfterQc;
                            pendingForQCVMList.Add(pendingForQCVMExcel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PendingOrderManager", "GetAbbPendingQCList", ex);
            }
            return pendingForQCVMList;
        }
        #endregion

        #region Get List Pending for PriceAcceptance ABB Orders list
        public List<PendingForPriceAcceptVMExcel> GetAbbPendingPriceAcceptList(int? buid, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null, int? statusId2 = null)
        {
            #region Variable Declaration
            PendingForPriceAcceptVMExcel? pendingForPriceAcceptVMExcel = null;
            List<PendingForPriceAcceptVMExcel>? pendingForPriceAcceptVMList = new List<PendingForPriceAcceptVMExcel>();
            List<TblOrderTran>? obj1 = null;
            bool? isFilterByModifiedDate = false;
            #endregion

            try
            {
                if (buid > 0)
                {
                    obj1 = _orderTransRepository.GetABBOrdersPendingList(buid, isFilterByModifiedDate, DateTimeByElapsedHours, statusId1
                        , statusId2);
                    if (obj1 != null && obj1.Count > 0)
                    {
                        foreach (TblOrderTran tblOrderTran in obj1)
                        {
                            pendingForPriceAcceptVMExcel = new PendingForPriceAcceptVMExcel();
                            pendingForPriceAcceptVMExcel.CompanyName = tblOrderTran?.Abbredemption?.Abbregistration?.BusinessUnit?.Name;
                            pendingForPriceAcceptVMExcel.RegdNo = tblOrderTran?.RegdNo;
                            pendingForPriceAcceptVMExcel.CustomerCity = tblOrderTran?.Abbredemption?.CustomerDetails?.City;
                            pendingForPriceAcceptVMExcel.ProductCategory = tblOrderTran?.Abbredemption?.Abbregistration?.NewProductCategory?.DescriptionForAbb;
                            pendingForPriceAcceptVMExcel.StatusCode = tblOrderTran?.Status?.StatusCode;
                            pendingForPriceAcceptVMExcel.OrderDueDateTime = tblOrderTran?.ModifiedDate != null ? Convert.ToDateTime(tblOrderTran?.ModifiedDate).ToString("dd/MM/yyyy hh:mm tt") : null;

                            TblOrderQc tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran?.OrderTransId ?? 0);
                            //if (tblOrderQc != null && tblOrderQc.ProposedQcdate != null)
                            //{
                            //    pendingForPriceAcceptVMExcel.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                            //    pendingForPriceAcceptVMExcel.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                            //}
                            pendingForPriceAcceptVMExcel.ProductCondition = tblOrderQc?.QualityAfterQc;
                            pendingForPriceAcceptVMList.Add(pendingForPriceAcceptVMExcel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PendingOrderManager", "GetAbbPendingPriceAcceptList", ex);
            }
            return pendingForPriceAcceptVMList;
        }
        #endregion

        #region Get List Pending for Pickup ABB Orders list
        public List<PendingForPickupVMExcel> GetAbbPendingPickupList(int? buid, int? spid, DateTime? DateTimeByElapsedHours = null,
             int? statusId1 = null)
        {
            #region Variable Declaration
            PendingForPickupVMExcel? pendingForPickupVMExcel = null;
            List<PendingForPickupVMExcel>? pendingForPickupVMList = new List<PendingForPickupVMExcel>();
            List<TblLogistic>? obj1 = null;
            #endregion

            try
            {
                if (buid > 0)
                {
                    obj1 = _orderTransRepository.GetABBOrdersPickupPendingList(buid, spid, DateTimeByElapsedHours, statusId1);
                    if (obj1 != null && obj1.Count > 0)
                    {
                        foreach (TblLogistic tblLogistic in obj1)
                        {
                            pendingForPickupVMExcel = new PendingForPickupVMExcel();
                            pendingForPickupVMExcel.CompanyName = tblLogistic?.OrderTrans?.Abbredemption?.Abbregistration?.BusinessUnit?.Name;
                            pendingForPickupVMExcel.RegdNo = tblLogistic?.RegdNo;
                            pendingForPickupVMExcel.ServicePartnerName = tblLogistic?.ServicePartner?.ServicePartnerBusinessName;
                            pendingForPickupVMExcel.CustomerCity = tblLogistic?.OrderTrans?.Abbredemption?.CustomerDetails?.City;
                            pendingForPickupVMExcel.ProductCategory = tblLogistic?.OrderTrans?.Abbredemption?.Abbregistration?.NewProductCategory?.DescriptionForAbb;
                            pendingForPickupVMExcel.StatusCode = tblLogistic?.Status?.StatusCode;
                            pendingForPickupVMExcel.TicketNumber = tblLogistic?.TicketNumber;
                            pendingForPickupVMExcel.PickupScheduleDate = tblLogistic?.PickupScheduleDate != null ? tblLogistic?.PickupScheduleDate.ToString() : null;
                            TblOrderQc tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblLogistic?.OrderTransId ?? 0);
                            pendingForPickupVMExcel.ProductCondition = tblOrderQc?.QualityAfterQc;
                            pendingForPickupVMExcel.OrderDueDateTime = tblLogistic?.CreatedDate != null ? Convert.ToDateTime(tblLogistic?.CreatedDate).ToString("dd/MM/yyyy hh:mm tt") : null;

                            pendingForPickupVMList.Add(pendingForPickupVMExcel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PendingOrderManager", "GetAbbPendingPickupList", ex);
            }
            return pendingForPickupVMList;
        }
        #endregion

        #region Mail Send with Attatchment using JetMail Added By VK
        public async Task<bool> JetMailSendWithAttachedFile(TemplateViewModel templateVM)
        {
            bool flag = false;
            TransactionalEmailResponse response = new TransactionalEmailResponse();

            if (templateVM != null && templateVM.To.Length > 0)
            {
                try
                {
                    #region Mail Send To
                    List<SendContact> contactList = new List<SendContact>();
                    templateVM.To = templateVM.To.Replace(",", ";");
                    String[] recipient = templateVM.To.Split(';');
                    for (int n = 0; n < recipient.Length; n++)
                        contactList.Add(new SendContact(recipient[n].Trim().ToString()));
                    #endregion

                    #region Add Cc
                    List<SendContact> ccContactList = new List<SendContact>();
                    if (templateVM.Cc != null && templateVM.Cc.Length > 0)
                    {
                        templateVM.Cc = templateVM.Cc.Replace(",", ";");
                        String[] ccEmailAddresses = templateVM.Cc.Split(';');
                        for (int n = 0; n < ccEmailAddresses.Length; n++)
                            ccContactList.Add(new SendContact(ccEmailAddresses[n].Trim().ToString()));
                    }
                    #endregion 

                    #region Add Bcc
                    List<SendContact> bccContactList = new List<SendContact>();
                    if (templateVM.Bcc != null)
                    {
                        templateVM.Bcc = templateVM.Bcc.Replace(",", ";");
                        String[] BccEmailAddresses = templateVM.Bcc.Split(';');
                        for (int n = 0; n < BccEmailAddresses.Length; n++)
                            bccContactList.Add(new SendContact(BccEmailAddresses[n].Trim().ToString()));
                    }
                    #endregion

                    List<Mailjet.Client.TransactionalEmails.Attachment> AttachmentsList = new List<Mailjet.Client.TransactionalEmails.Attachment>();
                    #region Add Direct File Attachment
                    if (templateVM.byteArrayExch != null)
                    {
                        string fileNameExch = templateVM.AttachFileNameExch ?? "PendingOrdersExch";
                        Byte[]? bytes = templateVM.byteArrayExch;
                        String file = Convert.ToBase64String(bytes);
                        AttachmentsList.Add(new Mailjet.Client.TransactionalEmails.Attachment(fileNameExch, ".xls", file));
                    }
                    if (templateVM.byteArrayAbb != null)
                    {
                        string fileNameAbb = templateVM.AttachFileNameAbb ?? "PendingOrdersAbb";
                        Byte[]? bytes = templateVM.byteArrayAbb;
                        String file = Convert.ToBase64String(bytes);
                        AttachmentsList.Add(new Mailjet.Client.TransactionalEmails.Attachment(fileNameAbb, ".xls", file));
                    }
                    #endregion

                    #region Mail Configurations
                    var email = new TransactionalEmail();
                    email.From = new SendContact(_config.Value.FromEmail, _config.Value.FromDisplayName);
                    email.ReplyTo = new SendContact(_config.Value.FromEmail, _config.Value.FromDisplayName);
                    email.To = contactList;

                    if (AttachmentsList != null && AttachmentsList.Count > 0)
                    {
                        email.Attachments = AttachmentsList;
                    }

                    if (ccContactList != null && ccContactList.Count > 0)
                        email.Cc = ccContactList;

                    if (bccContactList != null && bccContactList.Count > 0)
                        email.Bcc = bccContactList;

                    email.Subject = templateVM.Subject;
                    email.HTMLPart = templateVM.HtmlBody;
                    #endregion

                    #region Invoke API to send email
                    MailjetClient client = new MailjetClient(_config.Value.MailjetAPIKey, _config.Value.MailjetAPISecret);
                    MailjetRequest request = new MailjetRequest
                    {
                        Resource = Send.Resource
                    };
                    response = await client.SendTransactionalEmailAsync(email);
                    if (response != null && response.Messages != null && response.Messages.Length > 0 && !string.IsNullOrEmpty(response.Messages[0].Status) && response.Messages[0].Status.ToLower().Equals("success"))
                        flag = true;
                    #endregion
                }
                catch (Exception ex)
                {
                    flag = false;
                    _logging.WriteErrorToDB("PendingOrderManager", "JetMailSendWithAttachedFile", ex);
                }
            }
            return flag;
        }
        #endregion
        #endregion End Reporting Tab


    }
}
