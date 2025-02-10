using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RDCELERP.Common.Constant;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.Base;
using RDCELERP.Model.ABBRedemption;
using RDCELERP.DAL.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using RDCELERP.DAL.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RDCELERP.Model.Company;
using Microsoft.Extensions.Options;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Model.LGC;
using RDCELERP.Model.OrderTrans;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using System.Data;
using System.IO;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Core.App.Helper;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.DealerDashBoard;
using ClosedXML.Excel;
using RDCELERP.BAL.Interface;
using System.Globalization;
using RDCELERP.Model;
using System.Drawing;
using RDCELERP.Model.Users;
using RDCELERP.Model.SearchFilters;
using RDCELERP.Model.QCComment;
using RDCELERP.Model.QC;
using MediaToolkit.Model;
using MediaToolkit.Options;
using MediaToolkit;
using static GoogleMaps.LocationServices.Directions;

namespace RDCELERP.Core.App.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExchangeOrdersController : ControllerBase
    {
        #region variable declaration
        private Digi2l_DevContext _context;
        private IMapper _mapper;
        private CustomDataProtection _protector;
        IExchangeOrderRepository _exchangeOrderRepository;
        private IOptions<ApplicationSettings> _config;
        string ExchangeImagesURL = string.Empty;
        string actionURL = string.Empty;
        IOrderTransRepository _orderTransRepository;
        TblOrderTran tblOrderTran = null;
        IVoucherRepository _voucherRepository;
        IOrderQCRepository _orderQCRepository;
        ICompanyRepository _companyRepository;
        IBusinessUnitRepository _businessUnitRepository;
        ICustomerDetailsRepository _customerDetailsRepository;
        IProductCategoryRepository _productCategoryRepository;
        IProductTypeRepository _productTypeRepository;
        IWhatsAppMessageRepository _whatsAppMessageRepository;
        IExchangeABBStatusHistoryRepository _exchangeABBStatusHistoryRepository;
        IUserRoleRepository _UserRoleRepository;
        IUserRepository _userRepository;
        IQCManager _QCManager;
        #endregion

        #region constructor
        public ExchangeOrdersController(IExchangeOrderRepository exchangeOrderRepository, IExchangeOrderRepository ExchangeOrderRepository, IMapper mapper, Digi2l_DevContext context, CustomDataProtection protector, IOptions<ApplicationSettings> config, IOrderTransRepository orderTransRepository, IVoucherRepository voucherRepository, IOrderQCRepository orderQCRepository, ICompanyRepository companyRepository, IBusinessUnitRepository businessUnitRepository, ICustomerDetailsRepository customerDetailsRepository, IProductTypeRepository productTypeRepository, IWhatsAppMessageRepository whatsAppMessageRepository, IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository, IProductCategoryRepository productCategoryRepository, IUserRoleRepository userRoleRepository, IUserRepository userRepository, IQCManager qCManager)
        {
            _context = context;
            _mapper = mapper;
            _protector = protector;
            _exchangeOrderRepository = exchangeOrderRepository;
            _config = config;
            _orderTransRepository = orderTransRepository;
            _voucherRepository = voucherRepository;
            _orderQCRepository = orderQCRepository;
            _companyRepository = companyRepository;
            _businessUnitRepository = businessUnitRepository;
            _customerDetailsRepository = customerDetailsRepository;
            _productTypeRepository = productTypeRepository;
            _whatsAppMessageRepository = whatsAppMessageRepository;
            _exchangeABBStatusHistoryRepository = exchangeABBStatusHistoryRepository;
            _productCategoryRepository = productCategoryRepository;
            _UserRoleRepository = userRoleRepository;
            _userRepository = userRepository;
            _QCManager = qCManager;
        }


        #endregion

        #region Exchange Order List added by Pooja Modified 28 Sep 2023
        [HttpPost]
        public async Task<ActionResult> ExchangeList(int companyId, DateTime? startDate, DateTime? endDate, int? productCatId,
            int? productTypeId, string? regdNo, string? phoneNo, string? pinCode, string? companyName)
        {
            #region Veriable declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            { phoneNo = phoneNo.Trim().ToLower(); }
            else { phoneNo = null; }
            if (!string.IsNullOrWhiteSpace(pinCode) && pinCode != "null")
            { pinCode = pinCode.Trim().ToLower(); }
            else { pinCode = null; }

            List<TblExchangeOrder> TblExchangeOrders = null;
            string? URL = _config.Value.URLPrefixforProd;
            string? MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string? MVCVoucherInvoiceimg = _config.Value.MVCBaseURLForExchangeInvoice;
            string? PODPdfUrl = _config.Value.PODPdfUrl;
            string? BaseURL = _config.Value.BaseURL;
            string? InvoiceimagURL = string.Empty;
            string? MVCBaseURL = _config.Value.MVCBaseURL;
            int count = 0;
            TblCompany? tblCompany = null;
            TblOrderQc? tblOrderQc = null;
            TblBusinessUnit? tblBusinessUnit = null;
            #endregion

            try
            {
                #region Datatable form variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                searchValue = searchValue.Trim();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                if (startDate != null && endDate != null)
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region table object Initialization
                count = _context.TblExchangeOrders
                               .Include(x => x.Brand)
                               .Include(x => x.Status)
                               .Include(x => x.CustomerDetails)
                               .Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Count(x => x.IsActive == true && (tblBusinessUnit == null || x.CompanyName == tblBusinessUnit.Name)
                               && (x.StatusId != Convert.ToInt32(OrderStatusEnum.CancelOrder)
                                     && x.StatusId != Convert.ToInt32(OrderStatusEnum.QCOrderCancel))
                               && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                               && (productCatId == null || (x.ProductType != null && x.ProductType.ProductCatId == productCatId))
                               && (productTypeId == null || x.ProductTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.CustomerDetails != null && x.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(pinCode) || (x.CustomerDetails != null && x.CustomerDetails.ZipCode == pinCode))
                               && (string.IsNullOrEmpty(companyName) || (x.CompanyName ?? "").ToLower() == companyName)
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               );
                if (count > 0)
                {
                    TblExchangeOrders = await _context.TblExchangeOrders
                               .Include(x => x.Brand)
                               .Include(x => x.Status)
                               .Include(x => x.CustomerDetails)
                               .Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Where(x => x.IsActive == true && (tblBusinessUnit == null || x.CompanyName == tblBusinessUnit.Name)
                               && (x.StatusId != Convert.ToInt32(OrderStatusEnum.CancelOrder)
                                     && x.StatusId != Convert.ToInt32(OrderStatusEnum.QCOrderCancel))
                               && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                               && (productCatId == null || (x.ProductType != null && x.ProductType.ProductCatId == productCatId))
                               && (productTypeId == null || x.ProductTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.CustomerDetails != null && x.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(pinCode) || (x.CustomerDetails != null && x.CustomerDetails.ZipCode == pinCode))
                               && (string.IsNullOrEmpty(companyName) || (x.CompanyName ?? "").ToLower() == companyName)
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.Id).Skip(skip).Take(pageSize).ToListAsync();

                    recordsTotal = count;
                }
                #endregion

                #region Data Initialization for Datatable from table to Model
                List<ExchangeOrderViewModel> ExchangeOrderList = _mapper.Map<List<TblExchangeOrder>, List<ExchangeOrderViewModel>>(TblExchangeOrders);

                foreach (ExchangeOrderViewModel item in ExchangeOrderList)
                {
                    if (item != null)
                    {
                        TblCustomerDetail customerDetail = TblExchangeOrders.FirstOrDefault(x => x.Id == item.Id).CustomerDetails;
                        if (customerDetail == null)
                        {
                            customerDetail = new TblCustomerDetail();
                        }
                        else
                        {
                            item.ZipCode = customerDetail.ZipCode;
                            item.CustFullname = customerDetail.FirstName + " " + customerDetail.LastName;
                            item.CustPhoneNumber = customerDetail.PhoneNumber;
                            item.CustEmail = customerDetail.Email;
                            item.CustAddress = customerDetail.Address1 + " " + customerDetail.Address2 != null ? customerDetail.Address2 : null;
                            item.CustCity = customerDetail.City != null ? customerDetail.City : null;
                            item.CustState = customerDetail.State != null ? customerDetail.State : null;
                        }

                        TblProductType productType = TblExchangeOrders.FirstOrDefault(x => x.Id == item.Id).ProductType;
                        if (productType == null)
                        {
                            productType = new TblProductType();
                        }
                        else
                        {
                            item.ProductDetail = productType.ProductCat.Name + "-" + productType.Name;
                        }

                        TblExchangeOrderStatus exchangeOrderStatus = TblExchangeOrders.FirstOrDefault(x => x.Id == item.Id).Status;
                        if (exchangeOrderStatus == null)
                        {
                            exchangeOrderStatus = new TblExchangeOrderStatus();
                        }

                        if (item.RegdNo != null)
                        {
                            TblEvcpoddetail evcPOD = _context.TblEvcpoddetails.FirstOrDefault(x => x.RegdNo != null && x.RegdNo.ToLower().Equals(item.RegdNo.ToLower()));
                            if (evcPOD != null)
                            {
                                //Rucha - PODPDFURL on appsetting changed and based on that old POD URL set
                                item.PodURL = "<a class='_target' target='_blank' href ='" + PODPdfUrl + evcPOD.Podurl + "' >POD</a>" + "<a class='_target' target='_blank' href ='" + BaseURL + EnumHelper.DescriptionAttr(FileAddressEnum.EVCPoD) + evcPOD.Podurl + "' >/ PODERP</a>";
                            }
                        }

                        TblVoucherVerfication voucherVerfication = _voucherRepository.GetSingle(x => x.ExchangeOrderId == item.Id);
                        if (voucherVerfication == null)
                        {
                            voucherVerfication = new TblVoucherVerfication();
                        }
                        else
                        {
                            if (voucherVerfication.InvoiceImageName != null)
                            {
                                InvoiceimagURL = MVCURL + voucherVerfication.InvoiceImageName;
                                ExchangeImagesURL = "<img src='" + InvoiceimagURL + "' class='img-responsive'/>";
                                item.InvoiceImageName = ExchangeImagesURL;
                            }
                        }

                        tblOrderTran = _orderTransRepository.GetQcDetailsByExchangeId(item.Id);
                        if (tblOrderTran != null)
                        {
                            item.OrderTransId = tblOrderTran.OrderTransId;
                            tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran.OrderTransId);
                            if (tblOrderQc != null)
                            {
                                item.UpiId = tblOrderQc.Upiid != null ? tblOrderQc.Upiid : null;
                            }
                            else
                            {
                                tblOrderQc = new TblOrderQc();
                            }
                        }
                        else
                        {
                            tblOrderTran = new TblOrderTran();
                        }

                        item.CreateDateString = item.CreatedDate != null ? Convert.ToDateTime(item.CreatedDate).ToString("MM/dd/yyyy") : "";
                        item.StatusName = exchangeOrderStatus != null ? exchangeOrderStatus.StatusCode : string.Empty;
                        item.FinalExchangePrice = item.FinalExchangePrice > 0 ? item.FinalExchangePrice : 0;
                    }
                    actionURL = " <div class='actionbtns'>";
                    actionURL = "<a href ='" + URL + "/Exchange/Manage?Exchangeorderid=" + _protector.Encode(item.Id) + "' ><button onclick='RecordView(" + item.Id + ")' class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                    actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                    actionURL = actionURL + "</div>";
                    item.Action = actionURL;

                    InvoiceimagURL = MVCURL + item.InvoiceImageName;
                    ExchangeImagesURL = "<img src='" + InvoiceimagURL + "' class='img-responsive'/>";
                    item.InvoiceImageName = ExchangeImagesURL;
                }
                #endregion

                var data = ExchangeOrderList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion       

        #region QC Order List (Pending for QC) added by VK for new Requirment Status Code(0,3Q,3P,3R,0R,6 and 5R added as per team Discussion)
        [HttpPost]
        public async Task<ActionResult> ExchangeListForOrdersQC(int companyId, int? userId, DateTime? orderStartDate,
            DateTime? orderEndDate, DateTime? resheduleStartDate, DateTime? resheduleEndDate, string? companyName, int? productCatId,
            int? productTypeId, string? regdNo, string? phoneNo, string? custCity)
        {
            #region Variable declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            { phoneNo = phoneNo.Trim().ToLower(); }
            else { phoneNo = null; }
            if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            { custCity = custCity.Trim().ToLower(); }
            else { custCity = null; }

            List<TblExchangeOrder> TblExchangeOrders = null;
            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            List<ExchangeOrderViewModel> ExchangeOrderList = null;
            List<ExchangeOrderViewModel> ExchangeOrderListFINAL = new List<ExchangeOrderViewModel>();

            int count = 0;
            #endregion
            try
            {
                #region Datatable form variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                if (resheduleStartDate != null && resheduleEndDate != null)
                {
                    resheduleStartDate = Convert.ToDateTime(resheduleStartDate).AddMinutes(-1);
                    resheduleEndDate = Convert.ToDateTime(resheduleEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion


                #region for filter of data for admin and user
                bool isUserAdmin = false;

                TblUserRole tblUserRole = _UserRoleRepository.GetUserRoleByUserId(userId);
                if (tblUserRole != null)
                {
                    string rolename = tblUserRole.Role != null ? tblUserRole.Role.RoleName : null;
                    if (rolename == EnumHelper.DescriptionAttr(RoleEnum.QcAdmin) || rolename == EnumHelper.DescriptionAttr(RoleEnum.SuperAdmin))
                    {
                        isUserAdmin = true;
                    }
                }
                #endregion

                #region table object Initialization
                if (isUserAdmin == false)
                {
                    count = _context.TblExchangeOrders.Include(x => x.Brand).Include(x => x.Status)
                               .Include(x => x.TblOrderTrans).ThenInclude(x => x.TblOrderQcs)
                               .Include(x => x.CustomerDetails).Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Count(x => x.IsActive == true && (tblBusinessUnit == null || x.CompanyName == tblBusinessUnit.Name)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCInProgress_3Q)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.CallAndGoScheduledAppointmentTaken_3P)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenOrder)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.InstalledbySponsor)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderWithDiagnostic))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))

                               && ((resheduleStartDate == null && resheduleEndDate == null)
                               || (x.TblOrderTrans.Count > 0 && x.TblOrderTrans.First().TblOrderQcs.Count > 0
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))

                               && ((isUserAdmin == false && userId > 0))
                               //&& (x.TblOrderTrans.Count > 0 && x.TblOrderTrans.First().TblOrderQcs.Count > 0
                               && ((isUserAdmin == false && x.TblOrderTrans.First().AssignTo == userId))

                               && (productCatId == null || (x.ProductType != null && x.ProductType.ProductCatId == productCatId))
                               && (productTypeId == null || x.ProductTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.CustomerDetails != null && x.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.CustomerDetails != null && (x.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.CompanyName ?? "").ToLower() == companyName)
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               //&& (userId == null || (x.CreatedBy != null && x.CreatedBy == userId))
                               );
                    if (count > 0)
                    {
                        TblExchangeOrders = await _context.TblExchangeOrders.Include(x => x.Brand).Include(x => x.Status)
                                   .Include(x => x.TblOrderTrans).ThenInclude(x => x.TblOrderQcs)
                                   .Include(x => x.CustomerDetails).Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                   .Where(x => x.IsActive == true && (tblBusinessUnit == null || x.CompanyName == tblBusinessUnit.Name)
                                   && (x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor)
                                    || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCInProgress_3Q)
                                    || x.StatusId == Convert.ToInt32(OrderStatusEnum.CallAndGoScheduledAppointmentTaken_3P)
                                    || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)
                                    || x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenOrder)
                                    || x.StatusId == Convert.ToInt32(OrderStatusEnum.InstalledbySponsor)
                                    || x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderWithDiagnostic))
                                   && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                                   && ((resheduleStartDate == null && resheduleEndDate == null)
                                   || (x.TblOrderTrans.Count > 0 && x.TblOrderTrans.First().TblOrderQcs.Count > 0
                                   && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                                   && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))

                                   && ((isUserAdmin == false && userId > 0))
                                   //&& (x.TblOrderTrans.Count > 0 && x.TblOrderTrans.First().TblOrderQcs.Count > 0
                                   && ((isUserAdmin == false && x.TblOrderTrans.First().AssignTo == userId))

                                   && (productCatId == null || (x.ProductType != null && x.ProductType.ProductCatId == productCatId))
                                   && (productTypeId == null || x.ProductTypeId == productTypeId)
                                   && (string.IsNullOrEmpty(phoneNo) || (x.CustomerDetails != null && x.CustomerDetails.PhoneNumber == phoneNo))
                                   && (string.IsNullOrEmpty(custCity) || (x.CustomerDetails != null && (x.CustomerDetails.City ?? "").ToLower() == custCity))
                                   && (string.IsNullOrEmpty(companyName) || (x.CompanyName ?? "").ToLower() == companyName)
                                   && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                                   //&& (userId == null || (x.CreatedBy != null && x.CreatedBy == userId))
                                   ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.Id).Skip(skip).Take(pageSize).ToListAsync();

                        recordsTotal = count;
                    }
                }
                else
                {
                    count = _context.TblExchangeOrders.Include(x => x.Brand).Include(x => x.Status)
                               .Include(x => x.TblOrderTrans).ThenInclude(x => x.TblOrderQcs)
                               .Include(x => x.CustomerDetails).Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Count(x => x.IsActive == true && (tblBusinessUnit == null || x.CompanyName == tblBusinessUnit.Name)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCInProgress_3Q)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.CallAndGoScheduledAppointmentTaken_3P)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenOrder)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.InstalledbySponsor)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderWithDiagnostic))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))

                               && ((resheduleStartDate == null && resheduleEndDate == null)
                               || (x.TblOrderTrans.Count > 0 && x.TblOrderTrans.First().TblOrderQcs.Count > 0
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))


                               && (productCatId == null || (x.ProductType != null && x.ProductType.ProductCatId == productCatId))
                               && (productTypeId == null || x.ProductTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.CustomerDetails != null && x.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.CustomerDetails != null && (x.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.CompanyName ?? "").ToLower() == companyName)
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               //&& (userId == null || (x.CreatedBy != null && x.CreatedBy == userId))
                               );
                    if (count > 0)
                    {
                        TblExchangeOrders = await _context.TblExchangeOrders.Include(x => x.Brand).Include(x => x.Status)
                                   .Include(x => x.TblOrderTrans).ThenInclude(x => x.TblOrderQcs)
                                   .Include(x => x.CustomerDetails).Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                   .Where(x => x.IsActive == true && (tblBusinessUnit == null || x.CompanyName == tblBusinessUnit.Name)
                                   && (x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor)
                                    || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCInProgress_3Q)
                                    || x.StatusId == Convert.ToInt32(OrderStatusEnum.CallAndGoScheduledAppointmentTaken_3P)
                                    || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)
                                    || x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenOrder)
                                    || x.StatusId == Convert.ToInt32(OrderStatusEnum.InstalledbySponsor)
                                    || x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderWithDiagnostic))
                                   && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                                   && ((resheduleStartDate == null && resheduleEndDate == null)
                                   || (x.TblOrderTrans.Count > 0 && x.TblOrderTrans.First().TblOrderQcs.Count > 0
                                   && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                                   && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))



                                   && (productCatId == null || (x.ProductType != null && x.ProductType.ProductCatId == productCatId))
                                   && (productTypeId == null || x.ProductTypeId == productTypeId)
                                   && (string.IsNullOrEmpty(phoneNo) || (x.CustomerDetails != null && x.CustomerDetails.PhoneNumber == phoneNo))
                                   && (string.IsNullOrEmpty(custCity) || (x.CustomerDetails != null && (x.CustomerDetails.City ?? "").ToLower() == custCity))
                                   && (string.IsNullOrEmpty(companyName) || (x.CompanyName ?? "").ToLower() == companyName)
                                   && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                                   //&& (userId == null || (x.CreatedBy != null && x.CreatedBy == userId))
                                   ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.Id).Skip(skip).Take(pageSize).ToListAsync();

                        recordsTotal = count;
                    }
                }


                #endregion

                #region Data Initialization for Datatable from table to Model
                if (TblExchangeOrders != null && TblExchangeOrders.Count > 0)
                {
                    ExchangeOrderList = _mapper.Map<List<TblExchangeOrder>, List<ExchangeOrderViewModel>>(TblExchangeOrders);
                    string actionURL = string.Empty;
                    TblOrderQc tblOrderQc = null;
                    //bool isUserAdmin = false;

                    //TblUserRole tblUserRole = _UserRoleRepository.GetUserRoleByUserId(userId);
                    //if (tblUserRole != null)
                    //{
                    //    string rolename = tblUserRole.Role !=null?tblUserRole.Role.RoleName : null;
                    //    if (rolename == EnumHelper.DescriptionAttr(RoleEnum.QcAdmin))
                    //    {
                    //        isUserAdmin = true;
                    //    }
                    //}

                    foreach (ExchangeOrderViewModel item in ExchangeOrderList)
                    {
                        item.OrderCreatedDate = item.CreatedDate;
                        if (item.CreatedDate != null)
                        {
                            item.OrderCreatedDateString = Convert.ToDateTime(item.CreatedDate).ToString("MM/dd/yyyy H:mm:ss");
                        }

                        if (isUserAdmin)
                        {
                            tblOrderTran = _orderTransRepository.GetQcDetailsByExchangeId(item.Id);
                        }
                        else
                        {
                            tblOrderTran = _orderTransRepository.GetQcDetailsByAssignUsers(item.Id, userId);
                        }

                        if (tblOrderTran != null)
                        {
                            item.OrderTransId = tblOrderTran.OrderTransId;
                            if (tblOrderTran.AssignBy != null && tblOrderTran.AssignBy > 0)
                            {
                                TblUser tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.UserId == tblOrderTran.AssignBy);
                                if (tblUser != null)
                                {
                                    string assignedBy = "";

                                    // Check if FirstName and LastName are not null before concatenating
                                    if (tblUser.FirstName != null && tblUser.LastName != null)
                                    {
                                        assignedBy = tblUser.FirstName + " " + tblUser.LastName;
                                    }
                                    else if (tblUser.FirstName != null)
                                    {
                                        assignedBy = tblUser.FirstName;
                                    }
                                    else if (tblUser.LastName != null)
                                    {
                                        assignedBy = tblUser.LastName;
                                    }
                                    else
                                    {
                                        assignedBy = "Unknown"; // or any other default value you prefer
                                    }

                                    // Then use assignedBy in your condition
                                    item.AssignedBy = assignedBy != null ? assignedBy : "Unknown";
                                }

                            }
                            if (tblOrderTran.AssignTo != null && tblOrderTran.AssignTo > 0)
                            {
                                TblUser tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.UserId == tblOrderTran.AssignTo);
                                if (tblUser != null)
                                {
                                    string assignedTo = "";

                                    // Check if FirstName and LastName are not null before concatenating
                                    if (tblUser.FirstName != null && tblUser.LastName != null)
                                    {
                                        assignedTo = tblUser.FirstName + " " + tblUser.LastName;
                                    }
                                    else if (tblUser.FirstName != null)
                                    {
                                        assignedTo = tblUser.FirstName;
                                    }
                                    else if (tblUser.LastName != null)
                                    {
                                        assignedTo = tblUser.LastName;
                                    }
                                    else
                                    {
                                        assignedTo = "Unknown"; // or any other default value you prefer
                                    }

                                    // Then use assignedBy in your condition
                                    item.AssignedTo = assignedTo != null ? assignedTo : "Unknown";
                                }
                                else
                                {
                                    item.AssignedTo = "Not Assigned";
                                }
                            }
                            //item.AssignBy = tblOrderTran.AssignBy!=null? tblOrderTran.AssignBy:0;
                            //item.AssignTo = tblOrderTran.AssignTo != null ? tblOrderTran.AssignTo : 0;
                            tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran.OrderTransId);
                            if (tblOrderQc != null)
                            {
                                if (tblOrderQc.ProposedQcdate != null && (item.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled) || item.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenforQC) || item.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenOrder)))
                                {
                                    item.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                    item.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                                }
                                item.Qccomments = tblOrderQc.Qccomments != null ? tblOrderQc.Qccomments : null;
                                item.QualityAfterQc = tblOrderQc.QualityAfterQc != null ? tblOrderQc.QualityAfterQc : null;
                                item.PriceAfterQc = tblOrderQc.PriceAfterQc != null ? null : tblOrderQc.PriceAfterQc;
                                item.Qcdate = tblOrderQc.Qcdate.ToString() != null ? tblOrderQc.Qcdate.ToString() : null;
                                item.QCStatusId = (int)(tblOrderQc.StatusId != null ? tblOrderQc.StatusId : 0);
                                item.CreatedBy = tblOrderQc.CreatedBy != null ? tblOrderQc.CreatedBy : null;
                                item.CreatedDate = (DateTime)(tblOrderQc.CreatedDate != null ? tblOrderQc.CreatedDate : null);
                                item.ModifiedBy = tblOrderQc.ModifiedBy != null ? tblOrderQc.ModifiedBy : null;
                                item.ModifiedDate = Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") : null;
                                item.IsPaymentConnected = tblOrderQc.IsPaymentConnected != null ? tblOrderQc.IsPaymentConnected : null;
                                item.CollectedAmount = tblOrderQc.CollectedAmount != null ? tblOrderQc.CollectedAmount : null;
                                item.IsActive = tblOrderQc.IsActive != null ? tblOrderQc.IsActive : null;
                                item.PreferredQCDate = tblOrderQc.ProposedQcdate != null ? tblOrderQc.ProposedQcdate : null;
                                item.PreferredQCDateString = Convert.ToDateTime(item.PreferredQCDate).ToString("MM/dd/yyyy");

                            }
                            ExchangeOrderListFINAL.Add(item);
                        }
                        else
                        {
                            //ExchangeOrderList.Remove(item);
                            //continue;
                        }

                        TblCustomerDetail customerDetail = TblExchangeOrders.FirstOrDefault(x => x.Id == item.Id).CustomerDetails;
                        if (customerDetail != null)
                        {
                            item.ZipCode = customerDetail.ZipCode;
                            item.CustFullname = customerDetail.FirstName + " " + customerDetail.LastName;
                            item.CustPhoneNumber = customerDetail.PhoneNumber;
                            item.CustEmail = customerDetail.Email;
                            item.CustAddress = customerDetail.Address1 + " " + customerDetail.Address2 != null ? customerDetail.Address2 : null;
                            item.CustCity = customerDetail.City != null ? customerDetail.City : null;
                            item.CustState = customerDetail.State != null ? customerDetail.State : null;
                        }

                        actionURL = " <div class='actionbtns'>";
                        actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>&nbsp;";
                        actionURL = actionURL + "<a href ='" + URL + "/Exchange/Manage?Exchangeorderid=" + _protector.Encode(item.Id) + "' onclick='RecordView(" + item.Id + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                        if (item.OrderCreatedDate != null)
                        {
                            int ElapsedHrs = 0;
                            DateTime orderDate = Convert.ToDateTime(item.OrderCreatedDate);
                            DateTime todaysdate = DateTime.Now;
                            TimeSpan variable = todaysdate - orderDate;
                            ElapsedHrs = Convert.ToInt32(variable.Days * 24);
                            ElapsedHrs = ElapsedHrs + variable.Hours;
                            if (ElapsedHrs >= 2)
                            {
                                actionURL = actionURL + "<i class='fas fa-flag' style='color: #f6331e;'></i>";
                            }
                        }

                        actionURL = actionURL + "</div>";
                        item.Action = actionURL;

                        //new added for checkbox 
                        //added for checkbox
                        if (isUserAdmin)
                        {
                            string actionURL1 = "<td class='actions'>";
                            actionURL1 += "<span><input type='checkbox' id='EVCAllocionCB' value=" + item.OrderTransId + " onclick='OnCheckBoxCheck();' class='checkboxinput' /></span>";
                            actionURL1 += "</td>";
                            item.ActionChkbox = actionURL1;
                        }

                        //end for check box
                        //end for checkbox

                        InvoiceimagURL = MVCURL + item.InvoiceImageName;
                        ExchangeImagesURL = "<img src='" + InvoiceimagURL + "' class='img-responsive' />";
                        item.InvoiceImageName = ExchangeImagesURL;

                        TblExchangeOrderStatus exchangeOrderStatuscode = _context.TblExchangeOrderStatuses.FirstOrDefault(x => x.Id == item.StatusId);
                        if (exchangeOrderStatuscode != null)
                        {
                            item.StatusCode = exchangeOrderStatuscode.StatusCode;
                        }

                        TblProductType productType = _productTypeRepository.GetBytypeid(item.ProductTypeId);
                        if (productType != null)
                        {
                            item.ProductType = productType.Description;
                            TblProductCategory productCategory = _productCategoryRepository.GeByid(productType.ProductCatId);
                            if (productCategory != null)
                            {
                                item.ProductCategory = productCategory.Description;
                            }
                        }
                        ExchangeOrderListFINAL.Add(item);
                    }
                }
                #endregion
                var data = ExchangeOrderList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region QC Order List (Self QC List) added by VK for new Requirment
        [HttpPost]
        public async Task<ActionResult> ExchangeListOfSelfQC(int companyId, DateTime? orderStartDate,
            DateTime? orderEndDate, DateTime? resheduleStartDate, DateTime? resheduleEndDate, string? companyName, int? productCatId,
            int? productTypeId, string? regdNo, string? phoneNo, string? custCity, int? userId)
        {
            #region Variable declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            { phoneNo = phoneNo.Trim().ToLower(); }
            else { phoneNo = null; }
            if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            { custCity = custCity.Trim().ToLower(); }
            else { custCity = null; }

            List<TblExchangeOrder> TblExchangeOrders = null;
            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            List<ExchangeOrderViewModel> ExchangeOrderList = null;
            int count = 0;
            #endregion

            try
            {
                #region Datatable form variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                if (resheduleStartDate != null && resheduleEndDate != null)
                {
                    resheduleStartDate = Convert.ToDateTime(resheduleStartDate).AddMinutes(-1);
                    resheduleEndDate = Convert.ToDateTime(resheduleEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region table object Initialization
                count = _context.TblExchangeOrders.Include(x => x.Brand).Include(x => x.Status)
                               .Include(x => x.CustomerDetails).Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Count(x => x.IsActive == true && (tblBusinessUnit == null || x.CompanyName == tblBusinessUnit.Name)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.SelfQCbyCustomer))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null)
                               || (x.TblOrderTrans.Count > 0 && x.TblOrderTrans.First().TblOrderQcs.Count > 0
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))
                               && (productCatId == null || (x.ProductType != null && x.ProductType.ProductCatId == productCatId))
                               && (productTypeId == null || x.ProductTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.CustomerDetails != null && x.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.CustomerDetails != null && (x.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.CompanyName ?? "").ToLower() == companyName)
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               && (userId == null || (x.CreatedBy != null && x.CreatedBy == userId))
                               );
                if (count > 0)
                {
                    TblExchangeOrders = await _context.TblExchangeOrders.Include(x => x.Brand).Include(x => x.Status)
                               .Include(x => x.CustomerDetails).Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Where(x => x.IsActive == true && (tblBusinessUnit == null || x.CompanyName == tblBusinessUnit.Name)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.SelfQCbyCustomer))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null)
                               || (x.TblOrderTrans.Count > 0 && x.TblOrderTrans.First().TblOrderQcs.Count > 0
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))
                               && (productCatId == null || (x.ProductType != null && x.ProductType.ProductCatId == productCatId))
                               && (productTypeId == null || x.ProductTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.CustomerDetails != null && x.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.CustomerDetails != null && (x.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.CompanyName ?? "").ToLower() == companyName)
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               && (userId == null || (x.CreatedBy != null && x.CreatedBy == userId))
                               ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.Id).Skip(skip).Take(pageSize).ToListAsync();
                    recordsTotal = count;
                }
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (TblExchangeOrders != null && TblExchangeOrders.Count > 0)
                {
                    ExchangeOrderList = _mapper.Map<List<TblExchangeOrder>, List<ExchangeOrderViewModel>>(TblExchangeOrders);
                    string actionURL = string.Empty;
                    TblOrderQc tblOrderQc = null;

                    foreach (ExchangeOrderViewModel item in ExchangeOrderList)
                    {
                        item.OrderCreatedDate = item.CreatedDate;
                        if (item.CreatedDate != null)
                        {
                            item.OrderCreatedDateString = Convert.ToDateTime(item.CreatedDate).ToString("MM/dd/yyyy H:mm:ss");
                        }
                        tblOrderTran = _orderTransRepository.GetQcDetailsByExchangeId(item.Id);
                        if (tblOrderTran != null)
                        {
                            item.OrderTransId = tblOrderTran.OrderTransId;
                            tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran.OrderTransId);
                            if (tblOrderQc != null)
                            {
                                item.Qccomments = tblOrderQc.Qccomments != null ? tblOrderQc.Qccomments : null;
                                item.QualityAfterQc = tblOrderQc.QualityAfterQc != null ? tblOrderQc.QualityAfterQc : null;
                                item.PriceAfterQc = tblOrderQc.PriceAfterQc != null ? null : tblOrderQc.PriceAfterQc;
                                item.Qcdate = tblOrderQc.Qcdate.ToString() != null ? tblOrderQc.Qcdate.ToString() : null;
                                item.QCStatusId = (int)(tblOrderQc.StatusId != null ? tblOrderQc.StatusId : 0);
                                item.CreatedBy = tblOrderQc.CreatedBy != null ? tblOrderQc.CreatedBy : null;
                                item.CreatedDate = (DateTime)(tblOrderQc.CreatedDate != null ? tblOrderQc.CreatedDate : null);
                                item.ModifiedBy = tblOrderQc.ModifiedBy != null ? tblOrderQc.ModifiedBy : null;
                                item.ModifiedDate = Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") : null;
                                item.IsPaymentConnected = tblOrderQc.IsPaymentConnected != null ? tblOrderQc.IsPaymentConnected : null;
                                item.CollectedAmount = tblOrderQc.CollectedAmount != null ? tblOrderQc.CollectedAmount : null;
                                item.IsActive = tblOrderQc.IsActive != null ? tblOrderQc.IsActive : null;
                                if (tblOrderQc.ProposedQcdate != null)
                                {
                                    item.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                    item.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                                    item.PreferredQCDate = tblOrderQc.ProposedQcdate != null ? tblOrderQc.ProposedQcdate : null;
                                    item.PreferredQCDateString = Convert.ToDateTime(item.PreferredQCDate).ToString("MM/dd/yyyy");
                                }
                            }
                        }

                        TblCustomerDetail customerDetail = TblExchangeOrders.FirstOrDefault(x => x.Id == item.Id).CustomerDetails;
                        if (customerDetail != null)
                        {
                            item.ZipCode = customerDetail.ZipCode;
                            item.CustFullname = customerDetail.FirstName + " " + customerDetail.LastName;
                            item.CustPhoneNumber = customerDetail.PhoneNumber;
                            item.CustEmail = customerDetail.Email;
                            item.CustAddress = customerDetail.Address1 + " " + customerDetail.Address2 != null ? customerDetail.Address2 : null;
                            item.CustCity = customerDetail.City != null ? customerDetail.City : null;
                            item.CustState = customerDetail.State != null ? customerDetail.State : null;
                        }

                        actionURL = " <div class='actionbtns'>";
                        actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>&nbsp;";
                        actionURL = actionURL + "<a href ='" + URL + "/Exchange/Manage?Exchangeorderid=" + _protector.Encode(item.Id) + "' onclick='RecordView(" + item.Id + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                        actionURL = actionURL + "</div>";
                        item.Action = actionURL;

                        InvoiceimagURL = MVCURL + item.InvoiceImageName;
                        ExchangeImagesURL = "<img src='" + InvoiceimagURL + "' class='img-responsive' />";
                        item.InvoiceImageName = ExchangeImagesURL;

                        TblExchangeOrderStatus exchangeOrderStatuscode = _context.TblExchangeOrderStatuses.FirstOrDefault(x => x.Id == item.StatusId);
                        if (exchangeOrderStatuscode != null)
                        {
                            item.StatusCode = exchangeOrderStatuscode.StatusCode;
                        }

                        TblProductType productType = _productTypeRepository.GetBytypeid(item.ProductTypeId);
                        if (productType != null)
                        {
                            item.ProductType = productType.Description;
                            TblProductCategory productCategory = _productCategoryRepository.GeByid(productType.ProductCatId);
                            if (productCategory != null)
                            {
                                item.ProductCategory = productCategory.Description;
                            }
                        }
                    }
                }
                var data = ExchangeOrderList;
                #endregion

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion

        #region All Rescheduled QC list (with Flag 3R & 3RA) added by VK
        [HttpPost]
        public async Task<IActionResult> AllRescheduledQC(int companyId, DateTime? orderStartDate,
              DateTime? orderEndDate, DateTime? resheduleStartDate, DateTime? resheduleEndDate, string? companyName, int? productCatId,
              int? productTypeId, string? regdNo, string? phoneNo, string? custCity, int? userId)
        {
            #region Variable Declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            { phoneNo = phoneNo.Trim().ToLower(); }
            else { phoneNo = null; }
            if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            { custCity = custCity.Trim().ToLower(); }
            else { custCity = null; }

            string URL = _config.Value.URLPrefixforProd;
            string BaseURL = _config.Value.BaseURL;
            List<TblOrderQc> TblOrderQcs = null;
            int count = 0;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            #endregion

            try
            {
                #region Datatable variable Initialization
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                if (resheduleStartDate != null && resheduleEndDate != null)
                {
                    resheduleStartDate = Convert.ToDateTime(resheduleStartDate).AddMinutes(-1);
                    resheduleEndDate = Convert.ToDateTime(resheduleEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region Get TblOrderQcs table data
                count = _context.TblOrderQcs
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                               .Count(x => x.IsActive == true && (tblBusinessUnit == null || tblBusinessUnit.Name == x.OrderTrans.Exchange.CompanyName)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)
                               || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentRescheduled_3RA))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null) || (x.ProposedQcdate >= resheduleStartDate && x.ProposedQcdate <= resheduleEndDate))
                               && (x.OrderTrans != null && x.OrderTrans.Exchange != null)
                               && (productCatId == null || (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId))
                               && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.City == custCity))
                               && (string.IsNullOrEmpty(companyName) || x.OrderTrans.Exchange.CompanyName == companyName)
                               && (string.IsNullOrEmpty(regdNo) || x.OrderTrans.RegdNo == regdNo)
                               && (userId == null || (x.CreatedBy != null && x.CreatedBy == userId))
                               );

                if (count > 0)
                {
                    TblOrderQcs = await _context.TblOrderQcs
                            .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                           .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                           .Where(x => x.IsActive == true && (tblBusinessUnit == null || tblBusinessUnit.Name == x.OrderTrans.Exchange.CompanyName)
                           && (x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)
                           || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentRescheduled_3RA))
                           && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                           && ((resheduleStartDate == null && resheduleEndDate == null) || (x.ProposedQcdate >= resheduleStartDate && x.ProposedQcdate <= resheduleEndDate))
                           && (x.OrderTrans != null && x.OrderTrans.Exchange != null)
                           && (productCatId == null || (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId))
                           && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId)
                           && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.PhoneNumber == phoneNo))
                           && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.City == custCity))
                           && (string.IsNullOrEmpty(companyName) || x.OrderTrans.Exchange.CompanyName == companyName)
                           && (string.IsNullOrEmpty(regdNo) || x.OrderTrans.RegdNo == regdNo)
                           && (userId == null || (x.CreatedBy != null && x.CreatedBy == userId))
                           ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.OrderQcid).Skip(skip).Take(pageSize).ToListAsync();
                }
                recordsTotal = count;
                #endregion

                #region Initialize Datatable
                List<QCCommentViewModel> QCCommentVMlist = _mapper.Map<List<TblOrderQc>, List<QCCommentViewModel>>(TblOrderQcs);
                string actionURL = string.Empty;
                string actionURL1 = string.Empty;

                foreach (QCCommentViewModel item in QCCommentVMlist)
                {
                    TblExchangeOrderStatus exchangeOrderStatuscode = _context.TblExchangeOrderStatuses.FirstOrDefault(x => x.Id == item.StatusId);
                    if (exchangeOrderStatuscode == null)
                    {
                        exchangeOrderStatuscode = new TblExchangeOrderStatus();
                    }
                    TblOrderTran tblOrderTran = _orderQCRepository.GetBytransId(item.OrderTransId);
                    if (tblOrderTran != null)
                    {
                        TblExchangeOrder tblExchangeOrder = _exchangeOrderRepository.GetExchangeorder(tblOrderTran.ExchangeId);
                        if (tblExchangeOrder != null)
                        {
                            item.OrderCreatedDate = tblExchangeOrder.CreatedDate;
                            item.ProductCondition = tblExchangeOrder.ProductCondition;
                            item.StatusCode = exchangeOrderStatuscode != null ? exchangeOrderStatuscode.StatusCode : "";
                            item.ExchangeId = tblExchangeOrder.Id;
                            item.CompanyName = tblExchangeOrder.CompanyName;
                            item.RegdNo = tblExchangeOrder.RegdNo;
                            item.RescheduleDate = Convert.ToDateTime(item.ProposedQcdate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(item.ProposedQcdate).ToString("dd/MM/yyyy") : null;

                            TblCustomerDetail customerDetail = _customerDetailsRepository.GetCustDetails(tblExchangeOrder.CustomerDetailsId);
                            {
                                item.ZipCode = customerDetail.ZipCode;
                                item.CustFullname = customerDetail.FirstName + " " + customerDetail.LastName != null ? customerDetail.LastName : "";
                                item.CustPhoneNumber = customerDetail.PhoneNumber;
                                item.CustEmail = customerDetail.Email;
                                item.CustAddress = customerDetail.Address1 + " " + customerDetail.Address2 != null ? customerDetail.Address2 : null;
                                item.CustCity = customerDetail.City != null ? customerDetail.City : null;
                                item.CustState = customerDetail.State != null ? customerDetail.State : null;
                            }

                            TblProductType productType = _productTypeRepository.GetBytypeid(tblExchangeOrder.ProductTypeId);
                            if (productType != null)
                            {
                                item.ProductType = productType.Description;
                                TblProductCategory productCategory = _productCategoryRepository.GeByid(productType.ProductCatId);
                                if (productCategory != null)
                                {
                                    item.ProductCategory = productCategory.Description;
                                }
                            }
                        }
                    }
                    actionURL = " <div class='actionbtns'>";
                    actionURL = "<a href ='" + URL + "/Exchange/Manage?Exchangeorderid=" + _protector.Encode(item.ExchangeId) + "' ><button onclick='RecordView(" + item.OrderQCId + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                    actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                    actionURL = actionURL + "</div>";
                    item.Action = actionURL;
                }

                var data = QCCommentVMlist;
                #endregion

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region All Rescheduled QC list (with Flag 3RA only) added by VK
        [HttpPost]
        public async Task<IActionResult> RescheduledAging(int companyId, DateTime? orderStartDate,
            DateTime? orderEndDate, DateTime? resheduleStartDate, DateTime? resheduleEndDate, string? companyName, int? productCatId,
            int? productTypeId, string? regdNo, string? phoneNo, string? custCity, int? userId)
        {
            #region Variable Declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            { phoneNo = phoneNo.Trim().ToLower(); }
            else { phoneNo = null; }
            if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            { custCity = custCity.Trim().ToLower(); }
            else { custCity = null; }

            string URL = _config.Value.URLPrefixforProd;
            string BaseURL = _config.Value.BaseURL;
            List<TblOrderQc> TblOrderQcs = null;
            int count = 0;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            #endregion

            try
            {
                #region Datatable variable Initialization
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                if (resheduleStartDate != null && resheduleEndDate != null)
                {
                    resheduleStartDate = Convert.ToDateTime(resheduleStartDate).AddMinutes(-1);
                    resheduleEndDate = Convert.ToDateTime(resheduleEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region Get TblOrderQcs table data
                count = _context.TblOrderQcs
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                               .Count(x => x.IsActive == true && (tblBusinessUnit == null || tblBusinessUnit.Name == x.OrderTrans.Exchange.CompanyName)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentRescheduled_3RA))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null) || (x.ProposedQcdate >= resheduleStartDate && x.ProposedQcdate <= resheduleEndDate))
                               && (x.OrderTrans != null && x.OrderTrans.Exchange != null)
                               && (productCatId == null || (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId))
                               && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.City == custCity))
                               && (string.IsNullOrEmpty(companyName) || x.OrderTrans.Exchange.CompanyName == companyName)
                               && (string.IsNullOrEmpty(regdNo) || x.OrderTrans.RegdNo == regdNo)
                               && (userId == null || (x.CreatedBy != null && x.CreatedBy == userId))
                           );

                if (count > 0)
                {
                    TblOrderQcs = await _context.TblOrderQcs
                            .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                           .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                           .Where(x => x.IsActive == true && (tblBusinessUnit == null || tblBusinessUnit.Name == x.OrderTrans.Exchange.CompanyName)
                           && (x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentRescheduled_3RA))
                           && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                           && ((resheduleStartDate == null && resheduleEndDate == null) || (x.ProposedQcdate >= resheduleStartDate && x.ProposedQcdate <= resheduleEndDate))
                           && (x.OrderTrans != null && x.OrderTrans.Exchange != null)
                           && (productCatId == null || (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId))
                           && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId)
                           && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.PhoneNumber == phoneNo))
                           && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.City == custCity))
                           && (string.IsNullOrEmpty(companyName) || x.OrderTrans.Exchange.CompanyName == companyName)
                           && (string.IsNullOrEmpty(regdNo) || x.OrderTrans.RegdNo == regdNo)
                           && (userId == null || (x.CreatedBy != null && x.CreatedBy == userId))
                           ).OrderByDescending(x => x.CreatedDate).Skip(skip).Take(pageSize).ToListAsync();
                }
                recordsTotal = count;
                #endregion

                #region Initialize Datatable
                List<QCCommentViewModel> QCCommentVMlist = _mapper.Map<List<TblOrderQc>, List<QCCommentViewModel>>(TblOrderQcs);
                string actionURL = string.Empty;
                string actionURL1 = string.Empty;

                foreach (QCCommentViewModel item in QCCommentVMlist)
                {
                    TblExchangeOrderStatus exchangeOrderStatuscode = _context.TblExchangeOrderStatuses.FirstOrDefault(x => x.Id == item.StatusId);
                    if (exchangeOrderStatuscode == null)
                    {
                        exchangeOrderStatuscode = new TblExchangeOrderStatus();
                    }
                    TblOrderTran tblOrderTran = _orderQCRepository.GetBytransId(item.OrderTransId);
                    if (tblOrderTran != null)
                    {
                        TblExchangeOrder tblExchangeOrder = _exchangeOrderRepository.GetExchangeorder(tblOrderTran.ExchangeId);
                        if (tblExchangeOrder != null)
                        {
                            item.OrderCreatedDate = tblExchangeOrder.CreatedDate;
                            item.ProductCondition = tblExchangeOrder.ProductCondition;
                            item.StatusCode = exchangeOrderStatuscode != null ? exchangeOrderStatuscode.StatusCode : "";
                            item.ExchangeId = tblExchangeOrder.Id;
                            item.CompanyName = tblExchangeOrder.CompanyName;
                            item.RegdNo = tblExchangeOrder.RegdNo;
                            item.RescheduleDate = Convert.ToDateTime(item.ProposedQcdate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(item.ProposedQcdate).ToString("dd/MM/yyyy") : null;

                            TblCustomerDetail customerDetail = _customerDetailsRepository.GetCustDetails(tblExchangeOrder.CustomerDetailsId);
                            {
                                item.ZipCode = customerDetail.ZipCode;
                                item.CustFullname = customerDetail.FirstName + " " + customerDetail.LastName != null ? customerDetail.LastName : "";
                                item.CustPhoneNumber = customerDetail.PhoneNumber;
                                item.CustEmail = customerDetail.Email;
                                item.CustAddress = customerDetail.Address1 + " " + customerDetail.Address2 != null ? customerDetail.Address2 : null;
                                item.CustCity = customerDetail.City != null ? customerDetail.City : null;
                                item.CustState = customerDetail.State != null ? customerDetail.State : null;
                            }

                            TblProductType productType = _productTypeRepository.GetBytypeid(tblExchangeOrder.ProductTypeId);
                            if (productType != null)
                            {
                                item.ProductType = productType.Description;
                                TblProductCategory productCategory = _productCategoryRepository.GeByid(productType.ProductCatId);
                                if (productCategory != null)
                                {
                                    item.ProductCategory = productCategory.Description;
                                }
                            }
                        }
                    }
                    actionURL = " <div class='actionbtns'>";
                    actionURL = "<a href ='" + URL + "/Exchange/Manage?Exchangeorderid=" + _protector.Encode(item.ExchangeId) + "' ><button onclick='RecordView(" + item.OrderQCId + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                    actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                    actionURL = actionURL + "</div>";
                    item.Action = actionURL;
                }

                var data = QCCommentVMlist;
                #endregion

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion

        #region Reschedule count 1 (with Flag 3R,OR,5R) added by VK
        [HttpPost]
        public async Task<IActionResult> QCRescheduleCountOne(int companyId, DateTime? orderStartDate,
            DateTime? orderEndDate, DateTime? resheduleStartDate, DateTime? resheduleEndDate, string? companyName, int? productCatId,
            int? productTypeId, string? regdNo, string? phoneNo, string? custCity, int? userId)
        {
            #region Variable Declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            { phoneNo = phoneNo.Trim().ToLower(); }
            else { phoneNo = null; }
            if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            { custCity = custCity.Trim().ToLower(); }
            else { custCity = null; }

            string URL = _config.Value.URLPrefixforProd;
            string BaseURL = _config.Value.BaseURL;
            List<TblOrderQc> TblOrderQcs = null;
            int count = 0;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            #endregion

            try
            {
                #region Datatable variable Initialization
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                if (resheduleStartDate != null && resheduleEndDate != null)
                {
                    resheduleStartDate = Convert.ToDateTime(resheduleStartDate).AddMinutes(-1);
                    resheduleEndDate = Convert.ToDateTime(resheduleEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region Get TblOrderQcs table data
                count = _context.TblOrderQcs
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                               .Count(x => x.IsActive == true && (tblBusinessUnit == null || tblBusinessUnit.Name == x.OrderTrans.Exchange.CompanyName)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled) && x.Reschedulecount == 1)
                               //&& ((x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled) || x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenOrder)
                               //|| x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenforQC)) && x.Reschedulecount == 1)
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null) || (x.ProposedQcdate >= resheduleStartDate && x.ProposedQcdate <= resheduleEndDate))
                               && (x.OrderTrans != null && x.OrderTrans.Exchange != null)
                               && (productCatId == null || (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId))
                               && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.City == custCity))
                               && (string.IsNullOrEmpty(companyName) || x.OrderTrans.Exchange.CompanyName == companyName)
                               && (string.IsNullOrEmpty(regdNo) || x.OrderTrans.RegdNo == regdNo)
                               && (userId == null || (x.CreatedBy != null && x.CreatedBy == userId))
                               );

                if (count > 0)
                {
                    TblOrderQcs = await _context.TblOrderQcs
                            .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                           .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                           .Where(x => x.IsActive == true && (tblBusinessUnit == null || tblBusinessUnit.Name == x.OrderTrans.Exchange.CompanyName)
                           && (x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled) && x.Reschedulecount == 1)
                           // && ((x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled) || x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenOrder)
                           //|| x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenforQC)) && x.Reschedulecount == 1)
                           && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                           && ((resheduleStartDate == null && resheduleEndDate == null) || (x.ProposedQcdate >= resheduleStartDate && x.ProposedQcdate <= resheduleEndDate))
                           && (x.OrderTrans != null && x.OrderTrans.Exchange != null)
                           && (productCatId == null || (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId))
                           && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId)
                           && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.PhoneNumber == phoneNo))
                           && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.City == custCity))
                           && (string.IsNullOrEmpty(companyName) || x.OrderTrans.Exchange.CompanyName == companyName)
                           && (string.IsNullOrEmpty(regdNo) || x.OrderTrans.RegdNo == regdNo)
                           && (userId == null || (x.CreatedBy != null && x.CreatedBy == userId))
                           ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.OrderQcid).Skip(skip).Take(pageSize).ToListAsync();
                }
                #endregion

                #region Set Pagination
                recordsTotal = count;
                #endregion

                #region Initialize Datatable
                List<QCCommentViewModel> QCCommentVMlist = _mapper.Map<List<TblOrderQc>, List<QCCommentViewModel>>(TblOrderQcs);
                string actionURL = string.Empty;
                string actionURL1 = string.Empty;

                foreach (QCCommentViewModel item in QCCommentVMlist)
                {
                    TblExchangeOrderStatus exchangeOrderStatuscode = _context.TblExchangeOrderStatuses.FirstOrDefault(x => x.Id == item.StatusId);
                    if (exchangeOrderStatuscode == null)
                    {
                        exchangeOrderStatuscode = new TblExchangeOrderStatus();
                    }
                    TblOrderTran tblOrderTran = _orderQCRepository.GetBytransId(item.OrderTransId);
                    if (tblOrderTran != null)
                    {
                        TblExchangeOrder tblExchangeOrder = _exchangeOrderRepository.GetExchangeorder(tblOrderTran.ExchangeId);
                        if (tblExchangeOrder != null)
                        {
                            item.OrderCreatedDate = tblExchangeOrder.CreatedDate;
                            item.ProductCondition = tblExchangeOrder.ProductCondition;
                            item.StatusCode = exchangeOrderStatuscode != null ? exchangeOrderStatuscode.StatusCode : "";
                            item.ExchangeId = tblExchangeOrder.Id;
                            item.CompanyName = tblExchangeOrder.CompanyName;
                            item.RegdNo = tblExchangeOrder.RegdNo;
                            item.RescheduleDate = Convert.ToDateTime(item.ProposedQcdate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(item.ProposedQcdate).ToString("dd/MM/yyyy") : null;

                            TblCustomerDetail customerDetail = _customerDetailsRepository.GetCustDetails(tblExchangeOrder.CustomerDetailsId);
                            {
                                item.ZipCode = customerDetail.ZipCode;
                                item.CustFullname = customerDetail.FirstName + " " + customerDetail.LastName != null ? customerDetail.LastName : "";
                                item.CustPhoneNumber = customerDetail.PhoneNumber;
                                item.CustEmail = customerDetail.Email;
                                item.CustAddress = customerDetail.Address1 + " " + customerDetail.Address2 != null ? customerDetail.Address2 : null;
                                item.CustCity = customerDetail.City != null ? customerDetail.City : null;
                                item.CustState = customerDetail.State != null ? customerDetail.State : null;
                            }

                            TblProductType productType = _productTypeRepository.GetBytypeid(tblExchangeOrder.ProductTypeId);
                            if (productType != null)
                            {
                                item.ProductType = productType.Description;
                                TblProductCategory productCategory = _productCategoryRepository.GeByid(productType.ProductCatId);
                                if (productCategory != null)
                                {
                                    item.ProductCategory = productCategory.Description;
                                }
                            }
                        }
                    }
                    actionURL = " <div class='actionbtns'>";
                    actionURL = "<a href ='" + URL + "/Exchange/Manage?Exchangeorderid=" + _protector.Encode(item.ExchangeId) + "' ><button onclick='RecordView(" + item.OrderQCId + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                    actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                    actionURL = actionURL + "</div>";
                    item.Action = actionURL;
                }

                var data = QCCommentVMlist;
                #endregion

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Reschedule count 2 (with Flag 3R,OR,5R) added by VK

        [HttpPost]
        public async Task<IActionResult> QCRescheduleCountTwo(int companyId, DateTime? orderStartDate,
            DateTime? orderEndDate, DateTime? resheduleStartDate, DateTime? resheduleEndDate, string? companyName, int? productCatId,
            int? productTypeId, string? regdNo, string? phoneNo, string? custCity, int? userId)
        {
            #region Variable Declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            { phoneNo = phoneNo.Trim().ToLower(); }
            else { phoneNo = null; }
            if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            { custCity = custCity.Trim().ToLower(); }
            else { custCity = null; }

            string URL = _config.Value.URLPrefixforProd;
            string BaseURL = _config.Value.BaseURL;
            List<TblOrderQc> TblOrderQcs = null;
            int count = 0;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            #endregion

            try
            {
                #region Datatable variable Initialization
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                if (resheduleStartDate != null && resheduleEndDate != null)
                {
                    resheduleStartDate = Convert.ToDateTime(resheduleStartDate).AddMinutes(-1);
                    resheduleEndDate = Convert.ToDateTime(resheduleEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region Get TblOrderQcs table data
                count = _context.TblOrderQcs
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                               .Count(x => x.IsActive == true && (tblBusinessUnit == null || tblBusinessUnit.Name == x.OrderTrans.Exchange.CompanyName)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled) && x.Reschedulecount == 2)
                               //&& ((x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled) || x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenOrder)
                               //|| x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenforQC)) && x.Reschedulecount == 2)
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null) || (x.ProposedQcdate >= resheduleStartDate && x.ProposedQcdate <= resheduleEndDate))
                               && (x.OrderTrans != null && x.OrderTrans.Exchange != null)
                               && (productCatId == null || (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId))
                               && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.City == custCity))
                               && (string.IsNullOrEmpty(companyName) || x.OrderTrans.Exchange.CompanyName == companyName)
                               && (string.IsNullOrEmpty(regdNo) || x.OrderTrans.RegdNo == regdNo)
                               && (userId == null || (x.CreatedBy != null && x.CreatedBy == userId))
                               );

                if (count > 0)
                {
                    TblOrderQcs = await _context.TblOrderQcs
                            .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                           .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                           .Where(x => x.IsActive == true && (tblBusinessUnit == null || tblBusinessUnit.Name == x.OrderTrans.Exchange.CompanyName)
                           && (x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled) && x.Reschedulecount == 2)
                           //&& ((x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled) || x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenOrder)
                           //|| x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenforQC)) && x.Reschedulecount == 2)
                           && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                           && ((resheduleStartDate == null && resheduleEndDate == null) || (x.ProposedQcdate >= resheduleStartDate && x.ProposedQcdate <= resheduleEndDate))
                           && (x.OrderTrans != null && x.OrderTrans.Exchange != null)
                           && (productCatId == null || (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId))
                           && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId)
                           && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.PhoneNumber == phoneNo))
                           && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.City == custCity))
                           && (string.IsNullOrEmpty(companyName) || x.OrderTrans.Exchange.CompanyName == companyName)
                           && (string.IsNullOrEmpty(regdNo) || x.OrderTrans.RegdNo == regdNo)
                           && (userId == null || (x.CreatedBy != null && x.CreatedBy == userId))
                           ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.OrderQcid).Skip(skip).Take(pageSize).ToListAsync();
                }
                #endregion

                #region Set Pagination
                recordsTotal = count;
                #endregion

                #region Initialize Datatable
                List<QCCommentViewModel> QCCommentVMlist = _mapper.Map<List<TblOrderQc>, List<QCCommentViewModel>>(TblOrderQcs);
                string actionURL = string.Empty;
                string actionURL1 = string.Empty;

                foreach (QCCommentViewModel item in QCCommentVMlist)
                {
                    TblExchangeOrderStatus exchangeOrderStatuscode = _context.TblExchangeOrderStatuses.FirstOrDefault(x => x.Id == item.StatusId);
                    if (exchangeOrderStatuscode == null)
                    {
                        exchangeOrderStatuscode = new TblExchangeOrderStatus();
                    }
                    TblOrderTran tblOrderTran = _orderQCRepository.GetBytransId(item.OrderTransId);
                    if (tblOrderTran != null)
                    {
                        TblExchangeOrder tblExchangeOrder = _exchangeOrderRepository.GetExchangeorder(tblOrderTran.ExchangeId);
                        if (tblExchangeOrder != null)
                        {
                            item.OrderCreatedDate = tblExchangeOrder.CreatedDate;
                            item.ProductCondition = tblExchangeOrder.ProductCondition;
                            item.StatusCode = exchangeOrderStatuscode != null ? exchangeOrderStatuscode.StatusCode : "";
                            item.ExchangeId = tblExchangeOrder.Id;
                            item.CompanyName = tblExchangeOrder.CompanyName;
                            item.RegdNo = tblExchangeOrder.RegdNo;
                            item.RescheduleDate = Convert.ToDateTime(item.ProposedQcdate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(item.ProposedQcdate).ToString("dd/MM/yyyy") : null;

                            TblCustomerDetail customerDetail = _customerDetailsRepository.GetCustDetails(tblExchangeOrder.CustomerDetailsId);
                            {
                                item.ZipCode = customerDetail.ZipCode;
                                item.CustFullname = customerDetail.FirstName + " " + customerDetail.LastName != null ? customerDetail.LastName : "";
                                item.CustPhoneNumber = customerDetail.PhoneNumber;
                                item.CustEmail = customerDetail.Email;
                                item.CustAddress = customerDetail.Address1 + " " + customerDetail.Address2 != null ? customerDetail.Address2 : null;
                                item.CustCity = customerDetail.City != null ? customerDetail.City : null;
                                item.CustState = customerDetail.State != null ? customerDetail.State : null;
                            }

                            TblProductType productType = _productTypeRepository.GetBytypeid(tblExchangeOrder.ProductTypeId);
                            if (productType != null)
                            {
                                item.ProductType = productType.Description;
                                TblProductCategory productCategory = _productCategoryRepository.GeByid(productType.ProductCatId);
                                if (productCategory != null)
                                {
                                    item.ProductCategory = productCategory.Description;
                                }
                            }
                        }
                    }
                    actionURL = " <div class='actionbtns'>";
                    actionURL = "<a href ='" + URL + "/Exchange/Manage?Exchangeorderid=" + _protector.Encode(item.ExchangeId) + "' ><button onclick='RecordView(" + item.OrderQCId + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                    actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                    actionURL = actionURL + "</div>";
                    item.Action = actionURL;
                }

                var data = QCCommentVMlist;
                #endregion

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Reschedule count 3 (with Flag 3R,OR,5R) added by VK
        [HttpPost]
        public async Task<IActionResult> QCRescheduleCountThree(int companyId, DateTime? orderStartDate,
            DateTime? orderEndDate, DateTime? resheduleStartDate, DateTime? resheduleEndDate, string? companyName, int? productCatId,
            int? productTypeId, string? regdNo, string? phoneNo, string? custCity, int? userId)
        {
            #region Variable Declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            { phoneNo = phoneNo.Trim().ToLower(); }
            else { phoneNo = null; }
            if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            { custCity = custCity.Trim().ToLower(); }
            else { custCity = null; }

            string URL = _config.Value.URLPrefixforProd;
            string BaseURL = _config.Value.BaseURL;
            List<TblOrderQc> TblOrderQcs = null;
            int count = 0;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            #endregion

            try
            {
                #region Datatable variable Initialization
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                if (resheduleStartDate != null && resheduleEndDate != null)
                {
                    resheduleStartDate = Convert.ToDateTime(resheduleStartDate).AddMinutes(-1);
                    resheduleEndDate = Convert.ToDateTime(resheduleEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region Get TblOrderQcs table data
                count = _context.TblOrderQcs
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                               .Count(x => x.IsActive == true && (tblBusinessUnit == null || tblBusinessUnit.Name == x.OrderTrans.Exchange.CompanyName)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled) && x.Reschedulecount == 3)
                               //&& ((x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled) || x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenOrder)
                               //|| x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenforQC)) && x.Reschedulecount == 3)
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null) || (x.ProposedQcdate >= resheduleStartDate && x.ProposedQcdate <= resheduleEndDate))
                               && (x.OrderTrans != null && x.OrderTrans.Exchange != null)
                               && (productCatId == null || (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId))
                               && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.City == custCity))
                               && (string.IsNullOrEmpty(companyName) || x.OrderTrans.Exchange.CompanyName == companyName)
                               && (string.IsNullOrEmpty(regdNo) || x.OrderTrans.RegdNo == regdNo)
                               && (userId == null || (x.CreatedBy != null && x.CreatedBy == userId))
                               );

                if (count > 0)
                {
                    TblOrderQcs = await _context.TblOrderQcs
                            .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                           .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                           .Where(x => x.IsActive == true && (tblBusinessUnit == null || tblBusinessUnit.Name == x.OrderTrans.Exchange.CompanyName)
                           && (x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled) && x.Reschedulecount == 3)
                           //&& ((x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled) || x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenOrder)
                           //|| x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenforQC)) && x.Reschedulecount == 3)
                           && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                           && ((resheduleStartDate == null && resheduleEndDate == null) || (x.ProposedQcdate >= resheduleStartDate && x.ProposedQcdate <= resheduleEndDate))
                           && (x.OrderTrans != null && x.OrderTrans.Exchange != null)
                           && (productCatId == null || (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId))
                           && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId)
                           && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.PhoneNumber == phoneNo))
                           && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.City == custCity))
                           && (string.IsNullOrEmpty(companyName) || x.OrderTrans.Exchange.CompanyName == companyName)
                           && (string.IsNullOrEmpty(regdNo) || x.OrderTrans.RegdNo == regdNo)
                           && (userId == null || (x.CreatedBy != null && x.CreatedBy == userId))
                           ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.OrderQcid).Skip(skip).Take(pageSize).ToListAsync();
                }
                #endregion

                #region Set Pagination
                recordsTotal = count;
                #endregion

                #region Initialize Datatable
                List<QCCommentViewModel> QCCommentVMlist = _mapper.Map<List<TblOrderQc>, List<QCCommentViewModel>>(TblOrderQcs);
                string actionURL = string.Empty;
                string actionURL1 = string.Empty;

                foreach (QCCommentViewModel item in QCCommentVMlist)
                {
                    TblExchangeOrderStatus exchangeOrderStatuscode = _context.TblExchangeOrderStatuses.FirstOrDefault(x => x.Id == item.StatusId);
                    if (exchangeOrderStatuscode == null)
                    {
                        exchangeOrderStatuscode = new TblExchangeOrderStatus();
                    }
                    TblOrderTran tblOrderTran = _orderQCRepository.GetBytransId(item.OrderTransId);
                    if (tblOrderTran != null)
                    {
                        TblExchangeOrder tblExchangeOrder = _exchangeOrderRepository.GetExchangeorder(tblOrderTran.ExchangeId);
                        if (tblExchangeOrder != null)
                        {
                            item.OrderCreatedDate = tblExchangeOrder.CreatedDate;
                            item.ProductCondition = tblExchangeOrder.ProductCondition;
                            item.StatusCode = exchangeOrderStatuscode != null ? exchangeOrderStatuscode.StatusCode : "";
                            item.ExchangeId = tblExchangeOrder.Id;
                            item.CompanyName = tblExchangeOrder.CompanyName;
                            item.RegdNo = tblExchangeOrder.RegdNo;
                            item.RescheduleDate = Convert.ToDateTime(item.ProposedQcdate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(item.ProposedQcdate).ToString("dd/MM/yyyy") : null;

                            TblCustomerDetail customerDetail = _customerDetailsRepository.GetCustDetails(tblExchangeOrder.CustomerDetailsId);
                            {
                                item.ZipCode = customerDetail.ZipCode;
                                item.CustFullname = customerDetail.FirstName + " " + customerDetail.LastName != null ? customerDetail.LastName : "";
                                item.CustPhoneNumber = customerDetail.PhoneNumber;
                                item.CustEmail = customerDetail.Email;
                                item.CustAddress = customerDetail.Address1 + " " + customerDetail.Address2 != null ? customerDetail.Address2 : null;
                                item.CustCity = customerDetail.City != null ? customerDetail.City : null;
                                item.CustState = customerDetail.State != null ? customerDetail.State : null;
                            }

                            TblProductType productType = _productTypeRepository.GetBytypeid(tblExchangeOrder.ProductTypeId);
                            if (productType != null)
                            {
                                item.ProductType = productType.Description;
                                TblProductCategory productCategory = _productCategoryRepository.GeByid(productType.ProductCatId);
                                if (productCategory != null)
                                {
                                    item.ProductCategory = productCategory.Description;
                                }
                            }
                        }
                    }
                    actionURL = " <div class='actionbtns'>";
                    actionURL = "<a href ='" + URL + "/Exchange/Manage?Exchangeorderid=" + _protector.Encode(item.ExchangeId) + "' ><button onclick='RecordView(" + item.OrderQCId + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                    actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                    actionURL = actionURL + "</div>";
                    item.Action = actionURL;
                }

                var data = QCCommentVMlist;
                #endregion

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Canceled Exchange Order List added by VK For Status Code(0X,3X,3C,3Y,5X)
        [HttpPost]
        public async Task<ActionResult> CancelledOrderListNew(int companyId, DateTime? orderStartDate,
              DateTime? orderEndDate, DateTime? resheduleStartDate, DateTime? resheduleEndDate, string? companyName, int? productCatId,
              int? productTypeId, string? regdNo, string? phoneNo, string? custCity, int? userId)
        {
            #region Variable Declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            { phoneNo = phoneNo.Trim().ToLower(); }
            else { phoneNo = null; }
            if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            { custCity = custCity.Trim().ToLower(); }
            else { custCity = null; }

            List<TblExchangeOrder> TblExchangeOrders = null;
            List<ExchangeOrderViewModel> ExchangeOrderList = null;

            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string MVCVoucherInvoiceimg = _config.Value.MVCBaseURLForExchangeInvoice;
            string PODPdfUrl = _config.Value.PODPdfUrl;
            string BaseURL = _config.Value.BaseURL;
            string InvoiceimagURL = string.Empty;
            string MVCBaseURL = _config.Value.MVCBaseURL;
            string actionURL1 = string.Empty;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            int count = 0;
            #endregion
            try
            {
                #region Datatable Variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                if (resheduleStartDate != null && resheduleEndDate != null)
                {
                    resheduleStartDate = Convert.ToDateTime(resheduleStartDate).AddMinutes(-1);
                    resheduleEndDate = Convert.ToDateTime(resheduleEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region TblExchangeOrders obj Initialization and Orders Count
                count = _context.TblExchangeOrders.Include(x => x.Status)
                               .Include(x => x.TblOrderTrans).ThenInclude(x => x.TblOrderQcs)
                               .Include(x => x.CustomerDetails).Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Count(x => x.IsActive == true && (tblBusinessUnit == null || x.CompanyName == tblBusinessUnit.Name)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.CancelOrder) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentDeclined)
                               || x.StatusId == Convert.ToInt32(OrderStatusEnum.CustomerNotResponding_3C) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCFail_3Y)
                               || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCOrderCancel))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null)
                               || (x.TblOrderTrans.Count > 0 && x.TblOrderTrans.First().TblOrderQcs.Count > 0
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))
                               && (productCatId == null || (x.ProductType != null && x.ProductType.ProductCatId == productCatId))
                               && (productTypeId == null || x.ProductTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.CustomerDetails != null && x.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.CustomerDetails != null && (x.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.CompanyName ?? "").ToLower() == companyName)
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               && (userId == null || (x.CreatedBy != null && x.CreatedBy == userId))
                               );
                if (count > 0)
                {
                    TblExchangeOrders = await _context.TblExchangeOrders.Include(x => x.Status)
                           .Include(x => x.TblOrderTrans).ThenInclude(x => x.TblOrderQcs)
                           .Include(x => x.CustomerDetails).Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                           .Where(x => x.IsActive == true && (tblBusinessUnit == null || x.CompanyName == tblBusinessUnit.Name)
                           && (x.StatusId == Convert.ToInt32(OrderStatusEnum.CancelOrder) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentDeclined)
                           || x.StatusId == Convert.ToInt32(OrderStatusEnum.CustomerNotResponding_3C) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCFail_3Y)
                           || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCOrderCancel))
                           && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                           && ((resheduleStartDate == null && resheduleEndDate == null)
                           || (x.TblOrderTrans.Count > 0 && x.TblOrderTrans.First().TblOrderQcs.Count > 0
                           && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                           && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))
                           && (productCatId == null || (x.ProductType != null && x.ProductType.ProductCatId == productCatId))
                           && (productTypeId == null || x.ProductTypeId == productTypeId)
                           && (string.IsNullOrEmpty(phoneNo) || (x.CustomerDetails != null && x.CustomerDetails.PhoneNumber == phoneNo))
                           && (string.IsNullOrEmpty(custCity) || (x.CustomerDetails != null && (x.CustomerDetails.City ?? "").ToLower() == custCity))
                           && (string.IsNullOrEmpty(companyName) || (x.CompanyName ?? "").ToLower() == companyName)
                           && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                           && (userId == null || (x.CreatedBy != null && x.CreatedBy == userId))
                           ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.Id).Skip(skip).Take(pageSize).ToListAsync();
                }
                recordsTotal = count;
                #endregion

                #region ExchangeOrderViewModel Initialization for Datatable
                if (TblExchangeOrders != null && TblExchangeOrders.Count > 0)
                {
                    ExchangeOrderList = _mapper.Map<List<TblExchangeOrder>, List<ExchangeOrderViewModel>>(TblExchangeOrders);
                    TblOrderQc tblOrderQc = null;
                    foreach (ExchangeOrderViewModel item in ExchangeOrderList)
                    {
                        if (item != null)
                        {
                            item.OrderCreatedDate = item.CreatedDate;
                            actionURL = " <td class='actions'>";
                            actionURL = actionURL + " <span><input type='checkbox' id=" + item.Id + " name ='orders'  value ='" + item.Id + "'   onclick='OnCheckBoxCheck();' class='checkboxinput' /></span>";
                            actionURL = actionURL + " </td>";
                            item.Action = actionURL;
                            TblCustomerDetail customerDetail = TblExchangeOrders.FirstOrDefault(x => x.Id == item.Id).CustomerDetails;
                            if (customerDetail == null)
                            {
                                customerDetail = new TblCustomerDetail();
                            }
                            else
                            {
                                item.ZipCode = customerDetail.ZipCode;
                                item.CustFullname = customerDetail.FirstName + " " + customerDetail.LastName != null ? customerDetail.LastName : "";
                                item.CustPhoneNumber = customerDetail.PhoneNumber;
                                item.CustEmail = customerDetail.Email;
                                item.CustAddress = customerDetail.Address1 + " " + customerDetail.Address2 != null ? customerDetail.Address2 : null;
                                item.CustCity = customerDetail.City != null ? customerDetail.City : null;
                                item.CustState = customerDetail.State != null ? customerDetail.State : null;
                            }

                            TblProductType productType = _productTypeRepository.GetBytypeid(item.ProductTypeId);
                            if (productType != null)
                            {
                                item.ProductType = productType.Description;
                                TblProductCategory productCategory = _productCategoryRepository.GeByid(productType.ProductCatId);
                                if (productCategory != null)
                                {
                                    item.ProductCategory = productCategory.Description;
                                }
                            }

                            TblExchangeOrderStatus exchangeOrderStatus = TblExchangeOrders.FirstOrDefault(x => x.Id == item.Id).Status;
                            if (exchangeOrderStatus == null)
                            {
                                exchangeOrderStatus = new TblExchangeOrderStatus();
                            }
                            if (item.RegdNo != null)
                            {
                                TblEvcpoddetail evcPOD = _context.TblEvcpoddetails.FirstOrDefault(x => x.RegdNo != null && x.RegdNo.ToLower().Equals(item.RegdNo.ToLower()));
                                if (evcPOD != null)
                                {
                                    //Rucha - PODPDFURL on appsetting changed and based on that old POD URL set
                                    item.PodURL = "<a class='_target' target='_blank' href ='" + PODPdfUrl + evcPOD.Podurl + "' >POD</a>" + "<a class='_target' target='_blank' href ='" + BaseURL + EnumHelper.DescriptionAttr(FileAddressEnum.EVCPoD) + evcPOD.Podurl + "' >/ PODERP</a>";
                                }
                            }
                            TblVoucherVerfication voucherVerfication = _voucherRepository.GetSingle(x => x.ExchangeOrderId == item.Id);
                            if (voucherVerfication == null)
                            {
                                voucherVerfication = new TblVoucherVerfication();
                            }
                            else
                            {
                                if (voucherVerfication.InvoiceImageName != null)
                                {
                                    InvoiceimagURL = MVCURL + voucherVerfication.InvoiceImageName;
                                    ExchangeImagesURL = "<img src='" + InvoiceimagURL + "' class='img-responsive'/>";
                                    item.InvoiceImageName = ExchangeImagesURL;
                                }
                            }
                            tblOrderTran = _orderTransRepository.GetQcDetailsByExchangeId(item.Id);
                            if (tblOrderTran != null)
                            {
                                item.OrderTransId = tblOrderTran.OrderTransId;
                                tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran.OrderTransId);
                                if (tblOrderQc != null)
                                {
                                    if (tblOrderQc.ProposedQcdate != null)
                                    {
                                        item.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                        item.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                                    }
                                    item.Qccomments = tblOrderQc.Qccomments != null ? tblOrderQc.Qccomments : null;
                                    item.QualityAfterQc = tblOrderQc.QualityAfterQc != null ? tblOrderQc.QualityAfterQc : null;
                                    item.PriceAfterQc = tblOrderQc.PriceAfterQc != null ? null : tblOrderQc.PriceAfterQc;
                                    item.Qcdate = tblOrderQc.Qcdate.ToString() != null ? tblOrderQc.Qcdate.ToString() : null;
                                    item.QCStatusId = (int)(tblOrderQc.StatusId != null ? tblOrderQc.StatusId : 0);
                                    item.CreatedBy = tblOrderQc.CreatedBy != null ? tblOrderQc.CreatedBy : null;
                                    item.CreatedDate = (DateTime)(tblOrderQc.CreatedDate != null ? tblOrderQc.CreatedDate : null);
                                    item.ModifiedBy = tblOrderQc.ModifiedBy != null ? tblOrderQc.ModifiedBy : null;
                                    item.ModifiedDate = Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") : null;
                                    item.IsPaymentConnected = tblOrderQc.IsPaymentConnected != null ? tblOrderQc.IsPaymentConnected : null;
                                    item.CollectedAmount = tblOrderQc.CollectedAmount != null ? tblOrderQc.CollectedAmount : null;
                                    item.IsActive = tblOrderQc.IsActive != null ? tblOrderQc.IsActive : null;
                                    if (tblOrderQc.ProposedQcdate != null)
                                    {
                                        item.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                        item.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                                        item.PreferredQCDate = tblOrderQc.ProposedQcdate != null ? tblOrderQc.ProposedQcdate : null;
                                        item.PreferredQCDateString = Convert.ToDateTime(item.PreferredQCDate).ToString("MM/dd/yyyy");
                                    }
                                }
                            }

                            item.CreateDateString = item.CreatedDate != null ? Convert.ToDateTime(item.CreatedDate).ToString("MM/dd/yyyy") : "";
                            item.StatusCode = exchangeOrderStatus != null ? exchangeOrderStatus.StatusCode : string.Empty;
                            item.FinalExchangePrice = item.FinalExchangePrice > 0 ? item.FinalExchangePrice : 0;
                        }

                        actionURL1 = "<div class='actionbtns'>";
                        actionURL1 = actionURL1 + " <a href ='" + URL + "/Exchange/Manage?Exchangeorderid=" + _protector.Encode(item.Id) + "' ><button onclick='RecordView(" + item.Id + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                        // actionURL1 = actionURL1 + " <a href ='" + URL + "/Exchange/Manage?id=" + _protector.Encode(item.Id) + "' ><button onclick='RecordView(" + item.Id + ")' class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='Reopen'><i class='fa-solid fa-arrow-rotate-left'></i></button></a>";
                        actionURL1 = actionURL1 + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                        actionURL1 = actionURL1 + "</div>";
                        item.Edit = actionURL1;

                        InvoiceimagURL = MVCURL + item.InvoiceImageName;
                        ExchangeImagesURL = "<img src='" + InvoiceimagURL + "' class='img-responsive'/>";
                        item.InvoiceImageName = ExchangeImagesURL;
                    }
                }
                else
                {
                    TblExchangeOrders = new List<TblExchangeOrder>();
                    ExchangeOrderList = new List<ExchangeOrderViewModel>();
                }
                #endregion
                var data = ExchangeOrderList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Canceled QC(Reopen QC) Order List added by VK  For Status Code(5X)
        [HttpPost]
        public async Task<ActionResult> CancelledQCListNew(int companyId, DateTime? orderStartDate,
              DateTime? orderEndDate, DateTime? resheduleStartDate, DateTime? resheduleEndDate, string? companyName, int? productCatId,
              int? productTypeId, string? regdNo, string? phoneNo, string? custCity, int? userId)
        {
            #region Variable Declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            { phoneNo = phoneNo.Trim().ToLower(); }
            else { phoneNo = null; }
            if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            { custCity = custCity.Trim().ToLower(); }
            else { custCity = null; }

            List<TblExchangeOrder> TblExchangeOrders = null;
            List<ExchangeOrderViewModel> ExchangeOrderList = null;

            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string MVCVoucherInvoiceimg = _config.Value.MVCBaseURLForExchangeInvoice;
            string PODPdfUrl = _config.Value.PODPdfUrl;
            string BaseURL = _config.Value.BaseURL;
            string InvoiceimagURL = string.Empty;
            string MVCBaseURL = _config.Value.MVCBaseURL;
            string actionURL1 = string.Empty;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            int count = 0;
            #endregion
            try
            {
                #region Datatable Variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                if (resheduleStartDate != null && resheduleEndDate != null)
                {
                    resheduleStartDate = Convert.ToDateTime(resheduleStartDate).AddMinutes(-1);
                    resheduleEndDate = Convert.ToDateTime(resheduleEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region TblExchangeOrders obj Initialization and Orders Count
                count = _context.TblExchangeOrders.Include(x => x.Status)
                               .Include(x => x.TblOrderTrans).ThenInclude(x => x.TblOrderQcs)
                               .Include(x => x.CustomerDetails).Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Count(x => x.IsActive == true && (tblBusinessUnit == null || x.CompanyName == tblBusinessUnit.Name)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.QCOrderCancel))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null)
                               || (x.TblOrderTrans.Count > 0 && x.TblOrderTrans.First().TblOrderQcs.Count > 0
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))
                               && (productCatId == null || (x.ProductType != null && x.ProductType.ProductCatId == productCatId))
                               && (productTypeId == null || x.ProductTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.CustomerDetails != null && x.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.CustomerDetails != null && (x.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.CompanyName ?? "").ToLower() == companyName)
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               && (userId == null || (x.CreatedBy != null && x.CreatedBy == userId))
                               );
                if (count > 0)
                {
                    TblExchangeOrders = await _context.TblExchangeOrders.Include(x => x.Status)
                           .Include(x => x.TblOrderTrans).ThenInclude(x => x.TblOrderQcs)
                           .Include(x => x.CustomerDetails).Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                           .Where(x => x.IsActive == true && (tblBusinessUnit == null || x.CompanyName == tblBusinessUnit.Name)
                           && (x.StatusId == Convert.ToInt32(OrderStatusEnum.QCOrderCancel))
                           && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                           && ((resheduleStartDate == null && resheduleEndDate == null)
                           || (x.TblOrderTrans.Count > 0 && x.TblOrderTrans.First().TblOrderQcs.Count > 0
                           && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                           && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))
                           && (productCatId == null || (x.ProductType != null && x.ProductType.ProductCatId == productCatId))
                           && (productTypeId == null || x.ProductTypeId == productTypeId)
                           && (string.IsNullOrEmpty(phoneNo) || (x.CustomerDetails != null && x.CustomerDetails.PhoneNumber == phoneNo))
                           && (string.IsNullOrEmpty(custCity) || (x.CustomerDetails != null && (x.CustomerDetails.City ?? "").ToLower() == custCity))
                           && (string.IsNullOrEmpty(companyName) || (x.CompanyName ?? "").ToLower() == companyName)
                           && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                           && (userId == null || (x.CreatedBy != null && x.CreatedBy == userId))
                           ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.Id).Skip(skip).Take(pageSize).ToListAsync();
                }
                recordsTotal = count;
                #endregion

                #region ExchangeOrderViewModel Initialization for Datatable
                if (TblExchangeOrders != null && TblExchangeOrders.Count > 0)
                {
                    ExchangeOrderList = _mapper.Map<List<TblExchangeOrder>, List<ExchangeOrderViewModel>>(TblExchangeOrders);
                    TblOrderQc tblOrderQc = null;
                    foreach (ExchangeOrderViewModel item in ExchangeOrderList)
                    {
                        if (item != null)
                        {
                            item.OrderCreatedDate = item.CreatedDate;
                            actionURL = " <td class='actions'>";
                            actionURL = actionURL + " <span><input type='checkbox' id=" + item.Id + " name ='orders'  value ='" + item.Id + "'   onclick='OnCheckBoxCheck();' class='checkboxinput' /></span>";
                            actionURL = actionURL + " </td>";
                            item.Action = actionURL;
                            TblCustomerDetail customerDetail = TblExchangeOrders.FirstOrDefault(x => x.Id == item.Id).CustomerDetails;
                            if (customerDetail == null)
                            {
                                customerDetail = new TblCustomerDetail();
                            }
                            else
                            {
                                item.ZipCode = customerDetail.ZipCode;
                                item.CustFullname = customerDetail.FirstName + " " + customerDetail.LastName != null ? customerDetail.LastName : "";
                                item.CustPhoneNumber = customerDetail.PhoneNumber;
                                item.CustEmail = customerDetail.Email;
                                item.CustAddress = customerDetail.Address1 + " " + customerDetail.Address2 != null ? customerDetail.Address2 : null;
                                item.CustCity = customerDetail.City != null ? customerDetail.City : null;
                                item.CustState = customerDetail.State != null ? customerDetail.State : null;
                            }

                            TblProductType productType = _productTypeRepository.GetBytypeid(item.ProductTypeId);
                            if (productType != null)
                            {
                                item.ProductType = productType.Description;
                                TblProductCategory productCategory = _productCategoryRepository.GeByid(productType.ProductCatId);
                                if (productCategory != null)
                                {
                                    item.ProductCategory = productCategory.Description;
                                }
                            }

                            TblExchangeOrderStatus exchangeOrderStatus = TblExchangeOrders.FirstOrDefault(x => x.Id == item.Id).Status;
                            if (exchangeOrderStatus == null)
                            {
                                exchangeOrderStatus = new TblExchangeOrderStatus();
                            }
                            if (item.RegdNo != null)
                            {
                                TblEvcpoddetail evcPOD = _context.TblEvcpoddetails.FirstOrDefault(x => x.RegdNo != null && x.RegdNo.ToLower().Equals(item.RegdNo.ToLower()));
                                if (evcPOD != null)
                                {
                                    //Rucha - PODPDFURL on appsetting changed and based on that old POD URL set
                                    item.PodURL = "<a class='_target' target='_blank' href ='" + PODPdfUrl + evcPOD.Podurl + "' >POD</a>" + "<a class='_target' target='_blank' href ='" + BaseURL + EnumHelper.DescriptionAttr(FileAddressEnum.EVCPoD) + evcPOD.Podurl + "' >/ PODERP</a>";
                                }
                            }
                            TblVoucherVerfication voucherVerfication = _voucherRepository.GetSingle(x => x.ExchangeOrderId == item.Id);
                            if (voucherVerfication == null)
                            {
                                voucherVerfication = new TblVoucherVerfication();
                            }
                            else
                            {
                                if (voucherVerfication.InvoiceImageName != null)
                                {
                                    InvoiceimagURL = MVCURL + voucherVerfication.InvoiceImageName;
                                    ExchangeImagesURL = "<img src='" + InvoiceimagURL + "' class='img-responsive'/>";
                                    item.InvoiceImageName = ExchangeImagesURL;
                                }
                            }
                            tblOrderTran = _orderTransRepository.GetQcDetailsByExchangeId(item.Id);
                            if (tblOrderTran != null)
                            {
                                item.OrderTransId = tblOrderTran.OrderTransId;
                                tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran.OrderTransId);
                                if (tblOrderQc != null)
                                {
                                    item.Qccomments = tblOrderQc.Qccomments != null ? tblOrderQc.Qccomments : null;
                                    item.QualityAfterQc = tblOrderQc.QualityAfterQc != null ? tblOrderQc.QualityAfterQc : null;
                                    item.PriceAfterQc = tblOrderQc.PriceAfterQc != null ? null : tblOrderQc.PriceAfterQc;
                                    item.Qcdate = tblOrderQc.Qcdate.ToString() != null ? tblOrderQc.Qcdate.ToString() : null;
                                    item.QCStatusId = (int)(tblOrderQc.StatusId != null ? tblOrderQc.StatusId : 0);
                                    item.CreatedBy = tblOrderQc.CreatedBy != null ? tblOrderQc.CreatedBy : null;
                                    item.CreatedDate = (DateTime)(tblOrderQc.CreatedDate != null ? tblOrderQc.CreatedDate : null);
                                    item.ModifiedBy = tblOrderQc.ModifiedBy != null ? tblOrderQc.ModifiedBy : null;
                                    item.ModifiedDate = Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") : null;
                                    item.IsPaymentConnected = tblOrderQc.IsPaymentConnected != null ? tblOrderQc.IsPaymentConnected : null;
                                    item.CollectedAmount = tblOrderQc.CollectedAmount != null ? tblOrderQc.CollectedAmount : null;
                                    item.IsActive = tblOrderQc.IsActive != null ? tblOrderQc.IsActive : null;
                                    if (tblOrderQc.ProposedQcdate != null)
                                    {
                                        item.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                        item.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                                        item.PreferredQCDate = tblOrderQc.ProposedQcdate != null ? tblOrderQc.ProposedQcdate : null;
                                        item.PreferredQCDateString = Convert.ToDateTime(item.PreferredQCDate).ToString("MM/dd/yyyy");
                                    }
                                }
                            }

                            item.CreateDateString = item.CreatedDate != null ? Convert.ToDateTime(item.CreatedDate).ToString("MM/dd/yyyy") : "";
                            item.StatusCode = exchangeOrderStatus != null ? exchangeOrderStatus.StatusCode : string.Empty;
                            item.FinalExchangePrice = item.FinalExchangePrice > 0 ? item.FinalExchangePrice : 0;
                        }

                        actionURL1 = "<div class='actionbtns'>";
                        actionURL1 = actionURL1 + " <a href ='" + URL + "/Exchange/Manage?Exchangeorderid=" + _protector.Encode(item.Id) + "' ><button onclick='RecordView(" + item.Id + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                        // actionURL1 = actionURL1 + " <a href ='" + URL + "/Exchange/Manage?id=" + _protector.Encode(item.Id) + "' ><button onclick='RecordView(" + item.Id + ")' class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='Reopen'><i class='fa-solid fa-arrow-rotate-left'></i></button></a>";
                        actionURL1 = actionURL1 + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                        actionURL1 = actionURL1 + "</div>";
                        item.Edit = actionURL1;

                        InvoiceimagURL = MVCURL + item.InvoiceImageName;
                        ExchangeImagesURL = "<img src='" + InvoiceimagURL + "' class='img-responsive'/>";
                        item.InvoiceImageName = ExchangeImagesURL;
                    }
                }
                else
                {
                    TblExchangeOrders = new List<TblExchangeOrder>();
                    ExchangeOrderList = new List<ExchangeOrderViewModel>();
                }
                #endregion
                var data = ExchangeOrderList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region QC FollowUp (with Flag 5W and 5Y) List(Price Quoted QC) After 48hrs added by Pooja Modified by VK
        [HttpPost]
        public async Task<ActionResult> FollowUpInAfter48hrsNew(int companyId, int userid, DateTime? orderStartDate,
            DateTime? orderEndDate, DateTime? resheduleStartDate, DateTime? resheduleEndDate, string? companyName, int? productCatId,
            int? productTypeId, string? regdNo, string? phoneNo, string? custCity)
        {
            #region Variable Declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            { phoneNo = phoneNo.Trim().ToLower(); }
            else { phoneNo = null; }
            if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            { custCity = custCity.Trim().ToLower(); }
            else { custCity = null; }

            List<TblExchangeOrder> TblExchangeOrders = null;
            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            List<ExchangeOrderViewModel> ExchangeOrderList = new List<ExchangeOrderViewModel>(); ;
            int count = 0;
            #endregion

            try
            {
                #region Datatable Variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                if (resheduleStartDate != null && resheduleEndDate != null)
                {
                    resheduleStartDate = Convert.ToDateTime(resheduleStartDate).AddMinutes(-1);
                    resheduleEndDate = Convert.ToDateTime(resheduleEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region table object Initialization
                TblExchangeOrders = await _context.TblExchangeOrders.Include(x => x.Brand).Include(x => x.Status)
                               .Include(x => x.TblOrderTrans).ThenInclude(x => x.TblOrderQcs)
                               .Include(x => x.CustomerDetails).Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Where(x => x.IsActive == true && (tblBusinessUnit == null || x.CompanyName == tblBusinessUnit.Name)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCByPass))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null)
                               || (x.TblOrderTrans.Count > 0 && x.TblOrderTrans.First().TblOrderQcs.Count > 0
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))
                               && (productCatId == null || (x.ProductType != null && x.ProductType.ProductCatId == productCatId))
                               && (productTypeId == null || x.ProductTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.CustomerDetails != null && x.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.CustomerDetails != null && (x.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.CompanyName ?? "").ToLower() == companyName)
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.Id).ToListAsync();
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (TblExchangeOrders != null && TblExchangeOrders.Count > 0)
                {
                    List<ExchangeOrderViewModel> ExchangeOrderListObj = null;
                    ExchangeOrderListObj = _mapper.Map<List<TblExchangeOrder>, List<ExchangeOrderViewModel>>(TblExchangeOrders);

                    string actionURL = string.Empty;
                    string actionURL1 = string.Empty;

                    foreach (ExchangeOrderViewModel item in ExchangeOrderListObj)
                    {
                        item.OrderCreatedDate = item.CreatedDate;
                        actionURL = " <td class='actions'>";
                        actionURL = actionURL + " <span><input type='checkbox' id=" + item.Id + " name ='orders'  value ='" + item.Id + "'   onclick='OnCheckBoxCheck();' class='checkboxinput' /></span>";
                        actionURL = actionURL + " </td>";
                        item.Action = actionURL;

                        int ElapsedHrs = 0;
                        TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = _exchangeABBStatusHistoryRepository.GetByRegdstatusno(item.RegdNo, item.StatusId);
                        if (tblExchangeAbbstatusHistory != null)
                        {
                            DateTime complaintDate = Convert.ToDateTime(tblExchangeAbbstatusHistory.CreatedDate);

                            DateTime todaysdate = DateTime.Now;

                            TimeSpan variable = todaysdate - complaintDate;
                            ElapsedHrs = Convert.ToInt32(variable.Days * 24);
                            ElapsedHrs = ElapsedHrs + variable.Hours;

                            if (ElapsedHrs >= 48)
                            {
                                TblExchangeOrderStatus exchangeOrderStatuscode = _context.TblExchangeOrderStatuses.FirstOrDefault(x => x.Id == item.StatusId);
                                if (exchangeOrderStatuscode == null)
                                {
                                    exchangeOrderStatuscode = new TblExchangeOrderStatus();
                                }
                                tblOrderTran = _orderTransRepository.GetQcDetailsByExchangeId(item.Id);
                                if (tblOrderTran != null)
                                {
                                    item.OrderTransId = tblOrderTran.OrderTransId;
                                }
                                if (tblOrderTran == null)
                                {
                                    tblOrderTran = new TblOrderTran();
                                }
                                TblOrderQc tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran.OrderTransId);
                                if (tblOrderQc == null)
                                {
                                    tblOrderQc = new TblOrderQc();
                                }
                                else
                                {
                                    item.Qccomments = tblOrderQc.Qccomments != null ? tblOrderQc.Qccomments : null;
                                    item.QualityAfterQc = tblOrderQc.QualityAfterQc != null ? tblOrderQc.QualityAfterQc : null;
                                    item.PriceAfterQc = tblOrderQc.PriceAfterQc != null ? null : tblOrderQc.PriceAfterQc;
                                    item.Qcdate = tblOrderQc.Qcdate.ToString() != null ? tblOrderQc.Qcdate.ToString() : null;
                                    item.QCStatusId = (int)(tblOrderQc.StatusId != null ? tblOrderQc.StatusId : null);
                                    item.CreatedBy = tblOrderQc.CreatedBy != null ? tblOrderQc.CreatedBy : null;
                                    item.CreatedDate = (DateTime)(tblOrderQc.CreatedDate != null ? tblOrderQc.CreatedDate : null);
                                    item.ModifiedBy = tblOrderQc.ModifiedBy != null ? tblOrderQc.ModifiedBy : null;
                                    item.ModifiedDate = Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") : null;
                                    item.IsPaymentConnected = tblOrderQc.IsPaymentConnected != null ? tblOrderQc.IsPaymentConnected : null;
                                    item.CollectedAmount = tblOrderQc.CollectedAmount != null ? tblOrderQc.CollectedAmount : null;
                                    item.IsActive = tblOrderQc.IsActive != null ? tblOrderQc.IsActive : null;
                                    if (tblOrderQc.ProposedQcdate != null)
                                    {
                                        item.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                        item.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                                        item.PreferredQCDate = tblOrderQc.ProposedQcdate != null ? tblOrderQc.ProposedQcdate : null;
                                        item.PreferredQCDateString = Convert.ToDateTime(item.PreferredQCDate).ToString("MM/dd/yyyy");
                                    }
                                }

                                actionURL1 = " <div class='actionbtns'>";
                                actionURL1 += "<a class='btn btn-primary btn-sm mx-1' target='_blank' href ='" + URL + "/PaymentDetails/ConfirmPaymentDetails?regdNo=" + item.RegdNo + "&userid=" + userid + "&exchangeid=" + item.Id + "&status=" + item.StatusId + "' title='Add UPI'>ADD UPI</a>&nbsp;";
                                actionURL1 += "<a href ='" + URL + "/Exchange/Manage?Exchangeorderid=" + _protector.Encode(item.Id) + "' ><button onclick='RecordView(" + item.Id + ")' class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>&nbsp;";
                                actionURL1 += " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                                actionURL1 += "</div>";
                                item.Edit = actionURL1;

                                InvoiceimagURL = MVCURL + item.InvoiceImageName;
                                ExchangeImagesURL = "<img src='" + InvoiceimagURL + "' class='img-responsive' />";
                                item.InvoiceImageName = ExchangeImagesURL;
                                item.ProductCondition = item.ProductCondition;
                                item.LinksendDate = Convert.ToDateTime(complaintDate).ToString("MM/dd/yyyy H:mm:ss");

                                item.StatusCode = exchangeOrderStatuscode != null ? exchangeOrderStatuscode.StatusCode : "";

                                TblCustomerDetail customerDetail = TblExchangeOrders.FirstOrDefault(x => x.Id == item.Id).CustomerDetails;
                                if (customerDetail != null)
                                {
                                    item.ZipCode = customerDetail.ZipCode;
                                    item.CustFullname = customerDetail.FirstName + " " + customerDetail.LastName != null ? customerDetail.LastName : "";
                                    item.CustPhoneNumber = customerDetail.PhoneNumber;
                                    item.CustEmail = customerDetail.Email;
                                    item.CustAddress = customerDetail.Address1 + " " + customerDetail.Address2 != null ? customerDetail.Address2 : null;
                                    item.CustCity = customerDetail.City != null ? customerDetail.City : null;
                                    item.CustState = customerDetail.State != null ? customerDetail.State : null;
                                }

                                TblProductType productType = _productTypeRepository.GetBytypeid(item.ProductTypeId);
                                if (productType != null)
                                {
                                    item.ProductType = productType.Description;
                                    TblProductCategory productCategory = _productCategoryRepository.GeByid(productType.ProductCatId);
                                    if (productCategory != null)
                                    {
                                        item.ProductCategory = productCategory.Description;
                                    }
                                }

                                if (tblOrderQc != null && item.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled))
                                {
                                    item.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                    item.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                                }
                                ExchangeOrderList.Add(item);
                            }
                        }
                    }
                }
                #endregion

                #region pagination
                recordsTotal = ExchangeOrderList != null ? ExchangeOrderList.Count : 0;
                if (ExchangeOrderList != null && ExchangeOrderList.Count > 0)
                {
                    ExchangeOrderList = ExchangeOrderList.Skip(skip).Take(pageSize).ToList();
                }
                #endregion

                var data = ExchangeOrderList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Vk Get List(Price Quoted QC)(with Flag 5W and 5Y) of QC Order Follow Up List In Between 48 hrs
        [HttpPost]
        public async Task<ActionResult> FollowUpInBetween48hrsNew(int companyId, int userid, DateTime? orderStartDate,
            DateTime? orderEndDate, DateTime? resheduleStartDate, DateTime? resheduleEndDate, string? companyName, int? productCatId,
            int? productTypeId, string? regdNo, string? phoneNo, string? custCity)
        {
            #region Variable Declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            { phoneNo = phoneNo.Trim().ToLower(); }
            else { phoneNo = null; }
            if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            { custCity = custCity.Trim().ToLower(); }
            else { custCity = null; }

            List<TblExchangeOrder> TblExchangeOrders = null;
            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            List<ExchangeOrderViewModel> ExchangeOrderList = new List<ExchangeOrderViewModel>();
            int count = 0;
            #endregion
            try
            {
                #region Datatable Variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                if (resheduleStartDate != null && resheduleEndDate != null)
                {
                    resheduleStartDate = Convert.ToDateTime(resheduleStartDate).AddMinutes(-1);
                    resheduleEndDate = Convert.ToDateTime(resheduleEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region table object Initialization
                TblExchangeOrders = await _context.TblExchangeOrders.Include(x => x.Brand).Include(x => x.Status)
                               .Include(x => x.TblOrderTrans).ThenInclude(x => x.TblOrderQcs)
                               .Include(x => x.CustomerDetails).Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Where(x => x.IsActive == true && (tblBusinessUnit == null || x.CompanyName == tblBusinessUnit.Name)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCByPass))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null)
                               || (x.TblOrderTrans.Count > 0 && x.TblOrderTrans.First().TblOrderQcs.Count > 0
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))
                               && (productCatId == null || (x.ProductType != null && x.ProductType.ProductCatId == productCatId))
                               && (productTypeId == null || x.ProductTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.CustomerDetails != null && x.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.CustomerDetails != null && (x.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.CompanyName ?? "").ToLower() == companyName)
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.Id).ToListAsync();
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (TblExchangeOrders != null && TblExchangeOrders.Count > 0)
                {
                    List<ExchangeOrderViewModel> ExchangeOrderListObj = new List<ExchangeOrderViewModel>();
                    ExchangeOrderListObj = _mapper.Map<List<TblExchangeOrder>, List<ExchangeOrderViewModel>>(TblExchangeOrders);

                    string actionURL = string.Empty;
                    string actionURL1 = string.Empty;

                    foreach (ExchangeOrderViewModel item in ExchangeOrderListObj)
                    {
                        item.OrderCreatedDate = item.CreatedDate;
                        int ElapsedHrs = 0;
                        TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = _exchangeABBStatusHistoryRepository.GetByRegdstatusno(item.RegdNo, item.StatusId);
                        if (tblExchangeAbbstatusHistory != null)
                        {
                            DateTime complaintDate = Convert.ToDateTime(tblExchangeAbbstatusHistory.CreatedDate);

                            DateTime todaysdate = DateTime.Now;

                            TimeSpan variable = todaysdate - complaintDate;
                            ElapsedHrs = Convert.ToInt32(variable.Days * 24);
                            ElapsedHrs = ElapsedHrs + variable.Hours;

                            if (ElapsedHrs < 48)
                            {
                                TblExchangeOrderStatus exchangeOrderStatuscode = _context.TblExchangeOrderStatuses.FirstOrDefault(x => x.Id == item.StatusId);
                                if (exchangeOrderStatuscode == null)
                                {
                                    exchangeOrderStatuscode = new TblExchangeOrderStatus();
                                }
                                tblOrderTran = _orderTransRepository.GetQcDetailsByExchangeId(item.Id);
                                if (tblOrderTran != null)
                                {
                                    item.OrderTransId = tblOrderTran.OrderTransId;
                                }
                                if (tblOrderTran == null)
                                {
                                    tblOrderTran = new TblOrderTran();
                                }
                                TblOrderQc tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran.OrderTransId);
                                if (tblOrderQc == null)
                                {
                                    tblOrderQc = new TblOrderQc();
                                }
                                else
                                {
                                    item.Qccomments = tblOrderQc.Qccomments != null ? tblOrderQc.Qccomments : null;
                                    item.QualityAfterQc = tblOrderQc.QualityAfterQc != null ? tblOrderQc.QualityAfterQc : null;
                                    item.PriceAfterQc = tblOrderQc.PriceAfterQc != null ? null : tblOrderQc.PriceAfterQc;
                                    item.Qcdate = tblOrderQc.Qcdate.ToString() != null ? tblOrderQc.Qcdate.ToString() : null;
                                    item.QCStatusId = (int)(tblOrderQc.StatusId != null ? tblOrderQc.StatusId : null);
                                    item.CreatedBy = tblOrderQc.CreatedBy != null ? tblOrderQc.CreatedBy : null;
                                    item.CreatedDate = (DateTime)(tblOrderQc.CreatedDate != null ? tblOrderQc.CreatedDate : null);
                                    item.ModifiedBy = tblOrderQc.ModifiedBy != null ? tblOrderQc.ModifiedBy : null;
                                    item.ModifiedDate = Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") : null;
                                    item.IsPaymentConnected = tblOrderQc.IsPaymentConnected != null ? tblOrderQc.IsPaymentConnected : null;
                                    item.CollectedAmount = tblOrderQc.CollectedAmount != null ? tblOrderQc.CollectedAmount : null;
                                    item.IsActive = tblOrderQc.IsActive != null ? tblOrderQc.IsActive : null;
                                    if (tblOrderQc.ProposedQcdate != null)
                                    {
                                        item.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                        item.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                                        item.PreferredQCDate = tblOrderQc.ProposedQcdate != null ? tblOrderQc.ProposedQcdate : null;
                                        item.PreferredQCDateString = Convert.ToDateTime(item.PreferredQCDate).ToString("MM/dd/yyyy");
                                    }
                                }
                                actionURL = " <div class='actionbtns'>";
                                actionURL += actionURL + "<button onclick='ResendUPILink(" + item.Id + ")' data-bs-toggle='tooltip' data-bs-placement='top' title='Resend UPI Verification Link' class='btn btn-primary btn-sm mx-1'>ResendLink</button>";
                                actionURL += "<a class='btn btn-primary btn-sm mx-1' target='_blank' href ='" + URL + "/PaymentDetails/ConfirmPaymentDetails?regdNo=" + item.RegdNo + "&userid=" + userid + "&exchangeid=" + item.Id + "&status=" + item.StatusId + "' title='Add UPI'>ADD UPI</a>&nbsp;";
                                actionURL += "<a href ='" + URL + "/Exchange/Manage?Exchangeorderid=" + _protector.Encode(item.Id) + "' ><button onclick='RecordView(" + item.Id + "," + ")' class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>&nbsp;";
                                actionURL += "<a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                                actionURL += "</div>";
                                item.Action = actionURL;

                                InvoiceimagURL = MVCURL + item.InvoiceImageName;
                                ExchangeImagesURL = "<img src='" + InvoiceimagURL + "' class='img-responsive' />";
                                item.InvoiceImageName = ExchangeImagesURL;
                                item.ProductCondition = item.ProductCondition;
                                item.LinksendDate = Convert.ToDateTime(complaintDate).ToString("MM/dd/yyyy H:mm:ss");

                                item.StatusCode = exchangeOrderStatuscode != null ? exchangeOrderStatuscode.StatusCode : "";

                                TblCustomerDetail customerDetail = TblExchangeOrders.FirstOrDefault(x => x.Id == item.Id).CustomerDetails;
                                if (customerDetail != null)
                                {
                                    item.ZipCode = customerDetail.ZipCode;
                                    item.CustFullname = customerDetail.FirstName + " " + customerDetail.LastName != null ? customerDetail.LastName : "";
                                    item.CustPhoneNumber = customerDetail.PhoneNumber;
                                    item.CustEmail = customerDetail.Email;
                                    item.CustAddress = customerDetail.Address1 + " " + customerDetail.Address2 != null ? customerDetail.Address2 : null;
                                    item.CustCity = customerDetail.City != null ? customerDetail.City : null;
                                    item.CustState = customerDetail.State != null ? customerDetail.State : null;
                                }

                                TblProductType productType = _productTypeRepository.GetBytypeid(item.ProductTypeId);
                                if (productType != null)
                                {
                                    item.ProductType = productType.Description;
                                    TblProductCategory productCategory = _productCategoryRepository.GeByid(productType.ProductCatId);
                                    if (productCategory != null)
                                    {
                                        item.ProductCategory = productCategory.Description;
                                    }
                                }

                                if (tblOrderQc != null && item.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled))
                                {
                                    item.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                    item.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                                }
                                ExchangeOrderList.Add(item);
                            }
                        }
                    }
                }
                #endregion

                #region pagination
                recordsTotal = ExchangeOrderList != null ? ExchangeOrderList.Count : 0;
                if (ExchangeOrderList != null && ExchangeOrderList.Count > 0)
                {
                    ExchangeOrderList = ExchangeOrderList.Skip(skip).Take(pageSize).ToList();
                }
                #endregion

                var data = ExchangeOrderList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Exchange Order List for Bulk Liquidation added by Kranti
        [HttpPost]
        public async Task<ActionResult> ExchangeBulkList(int companyId, int userid, DateTime? startDate, DateTime? endDate, int? productCatId,
            int? productTypeId, string? regdNo, string? sponsorNo, string? phoneNo, string? pinCode, string? companyName)
        {
            List<TblExchangeOrder> TblExchangeOrders = null;

            string URL = _config.Value.URLPrefixforProd;
            if (string.IsNullOrEmpty(regdNo) || regdNo == "null")
            { regdNo = "".Trim().ToLower(); }
            else
            { regdNo = regdNo.Trim().ToLower(); }
            if (string.IsNullOrEmpty(sponsorNo) || sponsorNo == "null")
            { sponsorNo = "".Trim().ToLower(); }
            else
            { sponsorNo = sponsorNo.Trim().ToLower(); }
            if (string.IsNullOrEmpty(phoneNo) || phoneNo == "null")
            { phoneNo = "".Trim().ToLower(); }
            else
            { phoneNo = phoneNo.Trim().ToLower(); }
            if (string.IsNullOrEmpty(pinCode) || pinCode == "null")
            { pinCode = "".Trim().ToLower(); }
            else
            { pinCode = pinCode.Trim().ToLower(); }
            if (string.IsNullOrEmpty(companyName) || companyName == "null")
            { companyName = "".Trim().ToLower(); }
            else
            { companyName = companyName.Trim().ToLower(); }
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string MVCVoucherInvoiceimg = _config.Value.MVCBaseURLForExchangeInvoice;
            string PODPdfUrl = _config.Value.PODPdfUrl;
            string BaseURL = _config.Value.BaseURL;
            string InvoiceimagURL = string.Empty;
            string MVCBaseURL = _config.Value.MVCBaseURL;

            int count = 0;
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                searchValue = searchValue.Trim();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                if (startDate != null && endDate != null)
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                }

                if (companyId > 0 && companyId != 1007)
                {
                    TblCompany tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        TblBusinessUnit tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                        {
                            if (startDate == null && endDate == null)
                            {
                                count = _context.TblExchangeOrders
                                    .Include(x => x.CustomerDetails)
                                    .Include(x => x.Brand)
                                    .Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                    .Count(x => x.IsActive == true && x.CompanyName == tblBusinessUnit.Name.ToLower() && x.StatusId != Convert.ToInt32(OrderStatusEnum.CancelOrder) && x.StatusId != Convert.ToInt32(OrderStatusEnum.QCOrderCancel)
                                     && (productCatId > 0 ? (x.ProductType.ProductCat.Id == 0 ? false : (x.ProductType.ProductCat.Id == productCatId)) : true)
                                     && (productTypeId > 0 ? (x.ProductTypeId == 0 ? false : (x.ProductTypeId == productTypeId)) : true)
                                     && x.RegdNo.ToLower().Contains(regdNo)
                                     && x.SponsorOrderNumber.ToLower().Contains(sponsorNo)
                                     && x.CustomerDetails.PhoneNumber.Contains(phoneNo)
                                     && x.CustomerDetails.ZipCode.Contains(pinCode)
                                     && x.CompanyName.Contains(companyName)
                                      && x.CreatedBy == userid
                                     );
                                if (count > 0)
                                {
                                    TblExchangeOrders = await _context.TblExchangeOrders
                                    .Include(x => x.Brand)
                                    .Include(x => x.Status)
                                    .Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                    .Include(x => x.CustomerDetails)
                                    .Where(x => x.IsActive == true && x.CompanyName == tblBusinessUnit.Name.ToLower()
                                     && x.StatusId != Convert.ToInt32(OrderStatusEnum.CancelOrder)
                                     && x.StatusId != Convert.ToInt32(OrderStatusEnum.QCOrderCancel)
                                     && (productCatId > 0 ? (x.ProductType.ProductCat.Id == 0 ? false : (x.ProductType.ProductCat.Id == productCatId)) : true)
                                     && (productTypeId > 0 ? (x.ProductTypeId == 0 ? false : (x.ProductTypeId == productTypeId)) : true)
                                     && x.RegdNo.ToLower().Contains(regdNo)
                                     && x.SponsorOrderNumber.ToLower().Contains(sponsorNo)
                                     && x.CustomerDetails.PhoneNumber.Contains(phoneNo)
                                     && x.CustomerDetails.ZipCode.Contains(pinCode)
                                     && x.CompanyName.Contains(companyName)
                                      && x.CreatedBy == userid
                                      ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.Id).Skip(skip).Take(pageSize).ToListAsync();
                                    //TblExchangeOrders.OrderByDescending(x => x.Id);
                                }
                            }
                            else
                            {

                                count = _context.TblExchangeOrders
                                    .Include(x => x.CustomerDetails)
                                    .Include(x => x.Brand)
                                    .Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                    .Count(x => x.IsActive == true
                                   && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                                    && x.CompanyName == tblBusinessUnit.Name.ToLower() && x.StatusId != Convert.ToInt32(OrderStatusEnum.CancelOrder) && x.StatusId != Convert.ToInt32(OrderStatusEnum.QCOrderCancel)
                                     && (productCatId > 0 ? (x.ProductType.ProductCat.Id == 0 ? false : (x.ProductType.ProductCat.Id == productCatId)) : true)
                                     && (productTypeId > 0 ? (x.ProductTypeId == 0 ? false : (x.ProductTypeId == productTypeId)) : true)
                                     && x.RegdNo.ToLower().Contains(regdNo)
                                     && x.SponsorOrderNumber.ToLower().Contains(sponsorNo)
                                     && x.CustomerDetails.PhoneNumber.Contains(phoneNo)
                                     && x.CustomerDetails.ZipCode.Contains(pinCode)
                                     && x.CompanyName.Contains(companyName)
                                      && x.CreatedBy == userid
                                     );

                                if (count > 0)
                                {
                                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                                    TblExchangeOrders = await _context.TblExchangeOrders
                                    .Include(x => x.Brand)
                                    .Include(x => x.Status)
                                    .Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                    .Include(x => x.CustomerDetails)
                                    .Where(x => x.IsActive == true && x.CompanyName == tblBusinessUnit.Name.ToLower()
                                     && x.StatusId != Convert.ToInt32(OrderStatusEnum.CancelOrder)
                                     && x.StatusId != Convert.ToInt32(OrderStatusEnum.QCOrderCancel)
                                     && (x.CreatedDate >= startDate && x.CreatedDate <= endDate)
                                     && (productCatId > 0 ? (x.ProductType.ProductCat.Id == 0 ? false : (x.ProductType.ProductCat.Id == productCatId)) : true)
                                     && (productTypeId > 0 ? (x.ProductTypeId == 0 ? false : (x.ProductTypeId == productTypeId)) : true)
                                     && x.RegdNo.ToLower().Contains(regdNo)

                                     && x.SponsorOrderNumber.ToLower().Contains(sponsorNo)
                                     && x.CustomerDetails.PhoneNumber.Contains(phoneNo)
                                     && x.CustomerDetails.ZipCode.Contains(pinCode)
                                     && x.CompanyName.Contains(companyName)
                                      && x.CreatedBy == userid
                                     ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.Id).Skip(skip).Take(pageSize).ToListAsync();
                                    //TblExchangeOrders.OrderByDescending(x => x.Id);
                                }
                            }

                        }
                    }
                }
                else
                {
                    if (startDate == null && endDate == null)
                    {
                        count = _context.TblExchangeOrders
                            .Include(x => x.CustomerDetails)
                            .Count(x => x.IsActive == true && x.StatusId != Convert.ToInt32(OrderStatusEnum.CancelOrder)
                            && x.StatusId != Convert.ToInt32(OrderStatusEnum.QCOrderCancel)
                            && (productCatId > 0 ? (x.ProductType.ProductCat.Id == 0 ? false : (x.ProductType.ProductCat.Id == productCatId)) : true)
                            && (productTypeId > 0 ? (x.ProductTypeId == 0 ? false : (x.ProductTypeId == productTypeId)) : true)
                            && x.RegdNo.ToLower().Contains(regdNo)
                            && x.SponsorOrderNumber.ToLower().Contains(sponsorNo)
                            && x.CustomerDetails.PhoneNumber.Contains(phoneNo)
                            && x.CustomerDetails.ZipCode.Contains(pinCode)
                            && x.CompanyName.Contains(companyName)
                            && x.CreatedBy == userid
                             );
                        if (count > 0)
                        {
                            TblExchangeOrders = await _context.TblExchangeOrders
                            .Include(x => x.Brand)
                            .Include(x => x.Status)
                            .Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                            .Include(x => x.CustomerDetails)
                            .Where(x => x.IsActive == true && x.StatusId != Convert.ToInt32(OrderStatusEnum.CancelOrder)
                            && x.StatusId != Convert.ToInt32(OrderStatusEnum.QCOrderCancel)
                            && (productCatId > 0 ? (x.ProductType.ProductCat.Id == 0 ? false : (x.ProductType.ProductCat.Id == productCatId)) : true)
                            && (productTypeId > 0 ? (x.ProductTypeId == 0 ? false : (x.ProductTypeId == productTypeId)) : true)
                            && x.RegdNo.ToLower().Contains(regdNo)
                            && x.SponsorOrderNumber.ToLower().Contains(sponsorNo)
                            && x.CustomerDetails.PhoneNumber.Contains(phoneNo)
                            && x.CustomerDetails.ZipCode.Contains(pinCode)
                            && x.CompanyName.Contains(companyName)
                            && x.CreatedBy == userid
                             ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.Id).Skip(skip).Take(pageSize).ToListAsync();

                        }
                    }
                    else
                    {
                        count = _context.TblExchangeOrders
                           .Include(x => x.CustomerDetails)
                           .Count(x => x.IsActive == true
                           && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                           && x.StatusId != Convert.ToInt32(OrderStatusEnum.CancelOrder)
                           && x.StatusId != Convert.ToInt32(OrderStatusEnum.QCOrderCancel)
                           && (productCatId > 0 ? (x.ProductType.ProductCat.Id == 0 ? false : (x.ProductType.ProductCat.Id == productCatId)) : true)
                           && (productTypeId > 0 ? (x.ProductTypeId == 0 ? false : (x.ProductTypeId == productTypeId)) : true)
                           && x.RegdNo.ToLower().Contains(regdNo)
                           && x.SponsorOrderNumber.ToLower().Contains(sponsorNo)
                           && x.CustomerDetails.PhoneNumber.Contains(phoneNo)
                           && x.CustomerDetails.ZipCode.Contains(pinCode)
                           && x.CompanyName.Contains(companyName)
                           && x.CreatedBy == userid
                            );
                        if (count > 0)
                        {
                            // to get date of same day
                            startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                            endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                            TblExchangeOrders = await _context.TblExchangeOrders
                            .Include(x => x.Brand)
                            .Include(x => x.Status)
                            .Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                            .Include(x => x.CustomerDetails)
                            .Where(x => x.IsActive == true
                            && x.StatusId != Convert.ToInt32(OrderStatusEnum.CancelOrder)
                            && x.StatusId != Convert.ToInt32(OrderStatusEnum.QCOrderCancel)
                            && (x.CreatedDate >= startDate && x.CreatedDate <= endDate)
                            && (productCatId > 0 ? (x.ProductType.ProductCat.Id == 0 ? false : (x.ProductType.ProductCat.Id == productCatId)) : true)
                            && (productTypeId > 0 ? (x.ProductTypeId == 0 ? false : (x.ProductTypeId == productTypeId)) : true)
                            && x.RegdNo.ToLower().Contains(regdNo)
                            && x.SponsorOrderNumber.ToLower().Contains(sponsorNo)
                            && x.CustomerDetails.PhoneNumber.Contains(phoneNo)
                            && x.CustomerDetails.ZipCode.Contains(pinCode)
                            && x.CompanyName.Contains(companyName)
                           && x.CreatedBy == userid
                             ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.Id).Skip(skip).Take(pageSize).ToListAsync();

                        }

                    }
                }

                recordsTotal = count;

                List<ExchangeOrderViewModel> ExchangeOrderList = _mapper.Map<List<TblExchangeOrder>, List<ExchangeOrderViewModel>>(TblExchangeOrders);

                foreach (ExchangeOrderViewModel item in ExchangeOrderList)
                {
                    if (item != null)
                    {
                        TblCustomerDetail customerDetail = TblExchangeOrders.FirstOrDefault(x => x.Id == item.Id).CustomerDetails;
                        if (customerDetail == null)
                        {
                            customerDetail = new TblCustomerDetail();
                        }
                        else
                        {
                            item.ZipCode = customerDetail.ZipCode;
                            item.CustFullname = customerDetail.FirstName + " " + customerDetail.LastName;
                            item.CustPhoneNumber = customerDetail.PhoneNumber;
                            item.CustEmail = customerDetail.Email;
                            item.CustAddress = customerDetail.Address1 + " " + customerDetail.Address2 != null ? customerDetail.Address2 : null;
                            item.CustCity = customerDetail.City != null ? customerDetail.City : null;
                            item.CustState = customerDetail.State != null ? customerDetail.State : null;
                        }
                        TblProductType productType = TblExchangeOrders.FirstOrDefault(x => x.Id == item.Id).ProductType;
                        if (productType == null)
                        {
                            productType = new TblProductType();
                        }
                        else
                        {
                            item.ProductDetail = productType.ProductCat.Name + "-" + productType.Name;
                        }
                        TblExchangeOrderStatus exchangeOrderStatus = TblExchangeOrders.FirstOrDefault(x => x.Id == item.Id).Status;
                        if (exchangeOrderStatus == null)
                        {
                            exchangeOrderStatus = new TblExchangeOrderStatus();
                        }
                        if (item.RegdNo != null)
                        {
                            TblEvcpoddetail evcPOD = _context.TblEvcpoddetails.FirstOrDefault(x => x.RegdNo != null && x.RegdNo.ToLower().Equals(item.RegdNo.ToLower()));
                            if (evcPOD != null)
                            {
                                //Rucha - PODPDFURL on appsetting changed and based on that old POD URL set
                                item.PodURL = "<a class='_target' target='_blank' href ='" + PODPdfUrl + evcPOD.Podurl + "' >POD</a>" + "<a class='_target' target='_blank' href ='" + BaseURL + EnumHelper.DescriptionAttr(FileAddressEnum.EVCPoD) + evcPOD.Podurl + "' >/ PODERP</a>";
                            }
                        }

                        TblVoucherVerfication voucherVerfication = _voucherRepository.GetSingle(x => x.ExchangeOrderId == item.Id);
                        if (voucherVerfication == null)
                        {
                            voucherVerfication = new TblVoucherVerfication();
                        }
                        else
                        {
                            if (voucherVerfication.InvoiceImageName != null)
                            {
                                InvoiceimagURL = MVCURL + voucherVerfication.InvoiceImageName;
                                ExchangeImagesURL = "<img src='" + InvoiceimagURL + "' class='img-responsive'/>";
                                item.InvoiceImageName = ExchangeImagesURL;
                            }
                        }
                        tblOrderTran = _orderTransRepository.GetQcDetailsByExchangeId(item.Id);
                        if (tblOrderTran != null)
                        {
                            item.OrderTransId = tblOrderTran.OrderTransId;
                        }
                        else
                        {
                            tblOrderTran = new TblOrderTran();
                        }

                        item.CreateDateString = item.CreatedDate != null ? Convert.ToDateTime(item.CreatedDate).ToString("MM/dd/yyyy") : "";
                        item.StatusName = exchangeOrderStatus != null ? exchangeOrderStatus.StatusCode : string.Empty;
                        item.FinalExchangePrice = item.FinalExchangePrice > 0 ? item.FinalExchangePrice : 0;
                    }

                    TblOrderQc tblOrderQc = _context.TblOrderQcs.Where(x => x.IsActive == true && x.OrderTransId == item.OrderTransId).FirstOrDefault();

                    if (tblOrderQc?.QualityAfterQc != null)
                    {
                        //if (tblOrderQc.QualityAfterQc == "Excellent")
                        //{
                        //    item.ProductGrade = "Working - A";
                        //}
                        if (tblOrderQc.QualityAfterQc == "Good" || tblOrderQc.QualityAfterQc == "Working")
                        {
                            item.ProductGrade = "Working - A";
                        }
                        if (tblOrderQc.QualityAfterQc == "Average" || tblOrderQc.QualityAfterQc == "Heavily Used")
                        {
                            item.ProductGrade = "Heavily Used - B";
                        }
                        if (tblOrderQc.QualityAfterQc == "Not_Working" || tblOrderQc.QualityAfterQc == "Not Working" || tblOrderQc.QualityAfterQc == "Non Working")
                        {
                            item.ProductGrade = "Not Working - C";
                        }
                    }

                    else
                    {
                        if (item.ProductCondition != null)
                        {
                            if (item.ProductCondition == "Working")
                            {
                                item.ProductGrade = "Working - A";
                            }
                            //if (item.ProductCondition == "Good")
                            //{
                            //    item.ProductGrade = "B";
                            //}
                            if (item.ProductCondition == "Heavily Used")
                            {
                                item.ProductGrade = "Heavily Used - B";
                            }
                            if (item.ProductCondition == "Not_Working" || item.ProductCondition == "Not Working")
                            {
                                item.ProductGrade = "Not Working - C";
                            }
                        }

                    }


                    if (item.StatusId == (int)OrderStatusEnum.Waitingforcustapproval || item.StatusId == (int)OrderStatusEnum.QCByPass)
                    {

                        actionURL = " <div class='actionbtns'>";
                        actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                        actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Bulk_Order/Acceptance?regdno=" + item.RegdNo + "' title='Accept_Price'><i class='fa-regular fa-circle-check'></i></a>";
                        actionURL = actionURL + "</div>";
                        item.Action = actionURL;
                    }
                    else
                    {


                        actionURL = " <div class='actionbtns'>";

                        actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                        actionURL = actionURL + " <a class='btn btn-sm btn-primary' target='_blank' href='" + URL + "/QCPortal/SelfQC?regdno=" + item.RegdNo + "' title='SelfQc'><i class='fa-solid fa-clipboard-check'></i></a>";

                        actionURL = actionURL + "</div>";
                        item.Action = actionURL;
                    }




                    InvoiceimagURL = MVCURL + item.InvoiceImageName;
                    ExchangeImagesURL = "<img src='" + InvoiceimagURL + "' class='img-responsive'/>";
                    item.InvoiceImageName = ExchangeImagesURL;
                }

                var data = ExchangeOrderList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion


        #region Self QC List for ratio reporting of exchange order done by customer added by Pratibha
        [HttpPost]
        public async Task<ActionResult> SelfQCRatioReportCustomer(int companyId, DateTime? orderStartDate,
            DateTime? orderEndDate, string? companyName, int? productCatId,
            int? productTypeId, string? regdNo, string? phoneNo, string? custCity, string? emailId)
        {
            #region Variable declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            { phoneNo = phoneNo.Trim().ToLower(); }
            else { phoneNo = null; }
            if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            { custCity = custCity.Trim().ToLower(); }
            else { custCity = null; }
            if (!string.IsNullOrWhiteSpace(emailId) && emailId != "null")
            {
                emailId = emailId.Trim().ToLower();
                emailId = SecurityHelper.EncryptString(emailId, _config.Value.SecurityKey);
            }
            else { emailId = null; }


            string URL = _config.Value.URLPrefixforProd;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            List<RatioReportSelfQcViewModel> ratioReportSelfQcViewModelList = new List<RatioReportSelfQcViewModel>();
            RatioReportSelfQcViewModel ratioReportSelfQcViewModel = null;
            int count = 0;
            string? keyString = _config.Value.SecurityKey;
            List<TblOrderTran>? tblOrderTran = null;
            #endregion

            try
            {
                #region Datatable form variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region table object Initialization

                count = _context.TblOrderTrans
       .Include(x => x.Exchange)
           .ThenInclude(x => x.TblSelfQcs).ThenInclude(x => x.User)
       .Include(x => x.Exchange)
           .ThenInclude(x => x.Status)
       .Include(x => x.Exchange)
           .ThenInclude(x => x.CustomerDetails)
       .Include(x => x.Exchange)
           .ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
       .Where(x => x.IsActive == true
           && (tblBusinessUnit == null || x.Exchange.CompanyName == tblBusinessUnit.Name)
           && ((orderStartDate == null && orderEndDate == null) || (x.Exchange.CreatedDate >= orderStartDate && x.Exchange.CreatedDate <= orderEndDate))
           && (productCatId == null || (x.Exchange.ProductType != null && x.Exchange.ProductType.ProductCat.Id == productCatId))
           && (productTypeId == null || x.Exchange.ProductType.Id == productTypeId)
           && (string.IsNullOrEmpty(phoneNo) || (x.Exchange.CustomerDetails != null && x.Exchange.CustomerDetails.PhoneNumber == phoneNo))
           && (string.IsNullOrEmpty(custCity) || (x.Exchange.CustomerDetails != null && (x.Exchange.CustomerDetails.City ?? "").ToLower() == custCity))
           && (string.IsNullOrEmpty(emailId) || x.Exchange.TblSelfQcs.Any(sq => sq.User.Email != null && sq.User.Email.ToLower() == emailId.ToLower()))
           && (string.IsNullOrEmpty(companyName) || (x.Exchange.CompanyName ?? "").ToLower() == companyName)
           && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
           && x.Exchange.TblOrderTrans.Any(ot => ot.SelfQclinkResendby == null)
           && x.Exchange.TblOrderTrans.Any(ot => ot.TblExchangeAbbstatusHistories.Any(eash => eash.StatusId == (int)OrderStatusEnum.SelfQCbyCustomer))
           && x.Exchange.TblSelfQcs.Any(sq => sq.User.UserId == 3)
       )
       .Count();

                if (count > 0)
                {
                    tblOrderTran = _context.TblOrderTrans
        .Include(x => x.Exchange)
            .ThenInclude(x => x.TblSelfQcs).ThenInclude(x => x.User)
        .Include(x => x.Exchange)
            .ThenInclude(x => x.Status)
        .Include(x => x.Exchange)
            .ThenInclude(x => x.CustomerDetails)
        .Include(x => x.Exchange)
            .ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
        .Where(x => x.IsActive == true
            && (tblBusinessUnit == null || x.Exchange.CompanyName == tblBusinessUnit.Name)
            && ((orderStartDate == null && orderEndDate == null) || (x.Exchange.CreatedDate >= orderStartDate && x.Exchange.CreatedDate <= orderEndDate))
            && (productCatId == null || (x.Exchange.ProductType != null && x.Exchange.ProductType.ProductCat.Id == productCatId))
            && (productTypeId == null || x.Exchange.ProductType.Id == productTypeId)
            && (string.IsNullOrEmpty(phoneNo) || (x.Exchange.CustomerDetails != null && x.Exchange.CustomerDetails.PhoneNumber == phoneNo))
            && (string.IsNullOrEmpty(custCity) || (x.Exchange.CustomerDetails != null && (x.Exchange.CustomerDetails.City ?? "").ToLower() == custCity))
            && (string.IsNullOrEmpty(emailId) || x.Exchange.TblSelfQcs.Any(sq => sq.User.Email != null && sq.User.Email.ToLower() == emailId.ToLower()))
            && (string.IsNullOrEmpty(companyName) || (x.Exchange.CompanyName ?? "").ToLower() == companyName)
            && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
            && x.Exchange.TblOrderTrans.Any(ot => ot.SelfQclinkResendby == null)
            && x.Exchange.TblOrderTrans.Any(ot => ot.TblExchangeAbbstatusHistories.Any(eash => eash.StatusId == (int)OrderStatusEnum.SelfQCbyCustomer))
            && x.Exchange.TblSelfQcs.Any(sq => sq.User.UserId == 3)).ToList();

                    tblOrderTran = tblOrderTran.OrderByDescending(x => x.Exchange.ModifiedDate).ThenByDescending(x => x.Exchange.Id).Skip(skip).Take(pageSize).ToList();
                    recordsTotal = count;
                    #region Data Initialization for Datatable from table to Model
                    if (tblOrderTran != null && tblOrderTran.Count > 0)
                    {
                        string actionURL = string.Empty;
                        TblOrderQc tblOrderQc = null;

                        foreach (var item in tblOrderTran)
                        {
                            ratioReportSelfQcViewModel = new RatioReportSelfQcViewModel();
                            if (item.Exchange != null)
                            {
                                ratioReportSelfQcViewModel.Id = item.Exchange.Id != null ? item.Exchange.Id : 0;
                                ratioReportSelfQcViewModel.CompanyName = item.Exchange.CompanyName != null ? item.Exchange.CompanyName : "";
                                ratioReportSelfQcViewModel.RegdNo = item.Exchange.RegdNo != null ? item.Exchange.RegdNo : "";
                                ratioReportSelfQcViewModel.ProductCondition = item.Exchange.ProductCondition != null ? item.Exchange.ProductCondition : "";
                                ratioReportSelfQcViewModel.OrderCreatedDate = Convert.ToDateTime(item.Exchange.CreatedDate).ToString("MM/dd/yyyy H:mm:ss");

                            }
                            else
                            {
                                ratioReportSelfQcViewModel.Id = 0;
                                ratioReportSelfQcViewModel.CompanyName = "";
                                ratioReportSelfQcViewModel.RegdNo = "";
                                ratioReportSelfQcViewModel.ProductCondition = "";
                                ratioReportSelfQcViewModel.OrderCreatedDate = "";

                            }
                            if (item.Exchange != null && item.Exchange.CustomerDetails != null)
                            {
                                ratioReportSelfQcViewModel.CustFullname = item.Exchange.CustomerDetails.FirstName + "" + item.Exchange.CustomerDetails.LastName;
                                ratioReportSelfQcViewModel.CustAddress = item.Exchange.CustomerDetails.Address1 + "" + item.Exchange.CustomerDetails.Address2;
                                ratioReportSelfQcViewModel.CustPincode = item.Exchange.CustomerDetails.ZipCode != null ? item.Exchange.CustomerDetails.ZipCode : "";
                                ratioReportSelfQcViewModel.CustCity = item.Exchange.CustomerDetails.City != null ? item.Exchange.CustomerDetails.City : "";
                                ratioReportSelfQcViewModel.CustState = item.Exchange.CustomerDetails.State != null ? item.Exchange.CustomerDetails.State : "";
                            }
                            else
                            {
                                ratioReportSelfQcViewModel.CustFullname = "";
                                ratioReportSelfQcViewModel.CustAddress = "";
                                ratioReportSelfQcViewModel.CustPincode = "";
                                ratioReportSelfQcViewModel.CustCity = "";
                                ratioReportSelfQcViewModel.CustState = "";
                            }
                            if (item.Exchange != null && item.Exchange.ProductType != null && item.Exchange.ProductType.ProductCat != null)
                            {
                                ratioReportSelfQcViewModel.ProductCategory = item.Exchange.ProductType.ProductCat.Description != null ? item.Exchange.ProductType.ProductCat.Description : "";
                            }
                            else
                            {
                                ratioReportSelfQcViewModel.ProductCategory = "";
                            }
                            if (item.Exchange != null && item.Exchange.Status != null)
                            {
                                ratioReportSelfQcViewModel.StatusCode = item.Exchange.Status.StatusCode != null ? item.Exchange.Status.StatusCode : "";
                            }
                            else
                            {
                                ratioReportSelfQcViewModel.StatusCode = "";
                            }
                            if (item.Exchange != null && item.Exchange.TblSelfQcs != null && item.Exchange.TblSelfQcs.First().User != null && item.Exchange.TblSelfQcs.First().User.Email != null)
                            {
                                ratioReportSelfQcViewModel.UserEmailId = SecurityHelper.DecryptString(item.Exchange.TblSelfQcs.First().User.Email, keyString);
                            }
                            else
                            {
                                ratioReportSelfQcViewModel.UserEmailId = "";

                            }

                            actionURL = " <div class='actionbtns'>";
                            actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>&nbsp;";
                            actionURL = actionURL + "<a href ='" + URL + "/Exchange/Manage?Exchangeorderid=" + _protector.Encode(item.Exchange.Id) + "' onclick='RecordView(" + item.Exchange.Id + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                            actionURL = actionURL + "</div>";
                            ratioReportSelfQcViewModel.Action = actionURL;
                            ratioReportSelfQcViewModelList.Add(ratioReportSelfQcViewModel);
                        }
                    }
                    #endregion
                }
                #endregion

                var data = ratioReportSelfQcViewModelList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion

        #region Self QC List for ratio reporting of exchange order done by internal team added by Pratibha
        [HttpPost]
        public async Task<ActionResult> SelfQCRatioReportInternalTeam(int companyId, DateTime? orderStartDate,
            DateTime? orderEndDate, string? companyName, int? productCatId,
            int? productTypeId, string? regdNo, string? phoneNo, string? custCity, string? emailId)
        {
            #region Variable declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            { phoneNo = phoneNo.Trim().ToLower(); }
            else { phoneNo = null; }
            if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            { custCity = custCity.Trim().ToLower(); }
            else { custCity = null; }
            if (!string.IsNullOrWhiteSpace(emailId) && emailId != "null")
            {
                emailId = emailId.Trim().ToLower();
                emailId = SecurityHelper.EncryptString(emailId, _config.Value.SecurityKey);
            }
            else { emailId = null; }

            string URL = _config.Value.URLPrefixforProd;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            List<RatioReportSelfQcViewModel> ratioReportSelfQcViewModelList = new List<RatioReportSelfQcViewModel>();
            RatioReportSelfQcViewModel ratioReportSelfQcViewModel = null;
            int count = 0;
            string? keyString = _config.Value.SecurityKey;
            List<TblOrderTran>? tblOrderTrans = null;
            #endregion

            try
            {
                #region Datatable form variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region table object Initialization

                count = _context.TblOrderTrans
        .Include(x => x.Exchange)
            .ThenInclude(x => x.TblSelfQcs).ThenInclude(x => x.User)
        .Include(x => x.Exchange)
            .ThenInclude(x => x.Status)
        .Include(x => x.Exchange)
            .ThenInclude(x => x.CustomerDetails)
        .Include(x => x.Exchange)
            .ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
        .Where(x => x.IsActive == true
            && (tblBusinessUnit == null || x.Exchange.CompanyName == tblBusinessUnit.Name)
            && ((orderStartDate == null && orderEndDate == null) || (x.Exchange.CreatedDate >= orderStartDate && x.Exchange.CreatedDate <= orderEndDate))
            && (productCatId == null || (x.Exchange.ProductType != null && x.Exchange.ProductType.ProductCat.Id == productCatId))
            && (productTypeId == null || x.Exchange.ProductType.Id == productTypeId)
            && (string.IsNullOrEmpty(phoneNo) || (x.Exchange.CustomerDetails != null && x.Exchange.CustomerDetails.PhoneNumber == phoneNo))
            && (string.IsNullOrEmpty(custCity) || (x.Exchange.CustomerDetails != null && (x.Exchange.CustomerDetails.City ?? "").ToLower() == custCity))
            && (string.IsNullOrEmpty(emailId) || x.Exchange.TblSelfQcs.Any(sq => sq.User.Email != null && sq.User.Email.ToLower() == emailId.ToLower()))
            && (string.IsNullOrEmpty(companyName) || (x.Exchange.CompanyName ?? "").ToLower() == companyName)
            && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
            && x.Exchange.TblOrderTrans.Any(ot => ot.TblExchangeAbbstatusHistories.Any(eash => eash.StatusId == (int)OrderStatusEnum.SelfQCbyCustomer))
            && ((x.Exchange.TblSelfQcs.Any(sq => sq.User.UserId == 3) && x.Exchange.TblOrderTrans.Any(ot => ot.SelfQclinkResendby != null)) || (x.Exchange.TblSelfQcs.Any(sq => sq.User.UserId != 3) && x.Exchange.TblOrderTrans.Any(ot => ot.SelfQclinkResendby != null)) || (x.Exchange.TblSelfQcs.Any(sq => sq.User.UserId != 3) && x.Exchange.TblOrderTrans.Any(ot => ot.SelfQclinkResendby == null)))
            ).Count();

                if (count > 0)
                {
                    tblOrderTrans = _context.TblOrderTrans
                 .Include(x => x.Exchange)
                 .ThenInclude(x => x.TblSelfQcs).ThenInclude(x => x.User)
                 .Include(x => x.Exchange)
                 .ThenInclude(x => x.Status)
                 .Include(x => x.Exchange)
                 .ThenInclude(x => x.CustomerDetails)
                 .Include(x => x.Exchange)
                 .ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
          .Where(x => x.IsActive == true
            && (tblBusinessUnit == null || x.Exchange.CompanyName == tblBusinessUnit.Name)
            && ((orderStartDate == null && orderEndDate == null) || (x.Exchange.CreatedDate >= orderStartDate && x.Exchange.CreatedDate <= orderEndDate))
            && (productCatId == null || (x.Exchange.ProductType != null && x.Exchange.ProductType.ProductCat.Id == productCatId))
            && (productTypeId == null || x.Exchange.ProductType.Id == productTypeId)
            && (string.IsNullOrEmpty(phoneNo) || (x.Exchange.CustomerDetails != null && x.Exchange.CustomerDetails.PhoneNumber == phoneNo))
            && (string.IsNullOrEmpty(custCity) || (x.Exchange.CustomerDetails != null && (x.Exchange.CustomerDetails.City ?? "").ToLower() == custCity))
            && (string.IsNullOrEmpty(emailId) || x.Exchange.TblSelfQcs.Any(sq => sq.User.Email != null && sq.User.Email.ToLower() == emailId.ToLower()))
            && (string.IsNullOrEmpty(companyName) || (x.Exchange.CompanyName ?? "").ToLower() == companyName)
            && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
            && x.Exchange.TblOrderTrans.Any(ot => ot.TblExchangeAbbstatusHistories.Any(eash => eash.StatusId == (int)OrderStatusEnum.SelfQCbyCustomer))
            && ((x.Exchange.TblSelfQcs.Any(sq => sq.User.UserId == 3) && x.Exchange.TblOrderTrans.Any(ot => ot.SelfQclinkResendby != null)) || (x.Exchange.TblSelfQcs.Any(sq => sq.User.UserId != 3) && x.Exchange.TblOrderTrans.Any(ot => ot.SelfQclinkResendby != null)) || (x.Exchange.TblSelfQcs.Any(sq => sq.User.UserId != 3) && x.Exchange.TblOrderTrans.Any(ot => ot.SelfQclinkResendby == null)))
                    ).ToList();

                    tblOrderTrans = tblOrderTrans.OrderByDescending(x => x.Exchange.ModifiedDate).ThenByDescending(x => x.Exchange.Id).Skip(skip).Take(pageSize).ToList();
                    recordsTotal = count;
                    #region Data Initialization for Datatable from table to Model
                    if (tblOrderTrans != null && tblOrderTrans.Count > 0)
                    {
                        string actionURL = string.Empty;
                        foreach (var item in tblOrderTrans)
                        {
                            ratioReportSelfQcViewModel = new RatioReportSelfQcViewModel();
                            if (item.Exchange != null)
                            {
                                ratioReportSelfQcViewModel.Id = item.Exchange.Id != null ? item.Exchange.Id : 0;
                                ratioReportSelfQcViewModel.CompanyName = item.Exchange.CompanyName != null ? item.Exchange.CompanyName : "";
                                ratioReportSelfQcViewModel.RegdNo = item.Exchange.RegdNo != null ? item.Exchange.RegdNo : "";
                                ratioReportSelfQcViewModel.ProductCondition = item.Exchange.ProductCondition != null ? item.Exchange.ProductCondition : "";
                                ratioReportSelfQcViewModel.OrderCreatedDate = Convert.ToDateTime(item.Exchange.CreatedDate).ToString("MM/dd/yyyy H:mm:ss");

                            }
                            else
                            {
                                ratioReportSelfQcViewModel.Id = 0;
                                ratioReportSelfQcViewModel.CompanyName = "";
                                ratioReportSelfQcViewModel.RegdNo = "";
                                ratioReportSelfQcViewModel.ProductCondition = "";
                                ratioReportSelfQcViewModel.OrderCreatedDate = "";

                            }
                            if (item.Exchange != null && item.Exchange.CustomerDetails != null)
                            {
                                ratioReportSelfQcViewModel.CustFullname = item.Exchange.CustomerDetails.FirstName + "" + item.Exchange.CustomerDetails.LastName;
                                ratioReportSelfQcViewModel.CustAddress = item.Exchange.CustomerDetails.Address1 + "" + item.Exchange.CustomerDetails.Address2;
                                ratioReportSelfQcViewModel.CustPincode = item.Exchange.CustomerDetails.ZipCode != null ? item.Exchange.CustomerDetails.ZipCode : "";
                                ratioReportSelfQcViewModel.CustCity = item.Exchange.CustomerDetails.City != null ? item.Exchange.CustomerDetails.City : "";
                                ratioReportSelfQcViewModel.CustState = item.Exchange.CustomerDetails.State != null ? item.Exchange.CustomerDetails.State : "";
                            }
                            else
                            {
                                ratioReportSelfQcViewModel.CustFullname = "";
                                ratioReportSelfQcViewModel.CustAddress = "";
                                ratioReportSelfQcViewModel.CustPincode = "";
                                ratioReportSelfQcViewModel.CustCity = "";
                                ratioReportSelfQcViewModel.CustState = "";
                            }
                            if (item.Exchange != null && item.Exchange.ProductType != null && item.Exchange.ProductType.ProductCat != null)
                            {
                                ratioReportSelfQcViewModel.ProductCategory = item.Exchange.ProductType.ProductCat.Description != null ? item.Exchange.ProductType.ProductCat.Description : "";
                            }
                            else
                            {
                                ratioReportSelfQcViewModel.ProductCategory = "";
                            }
                            if (item.Exchange != null && item.Exchange.Status != null)
                            {
                                ratioReportSelfQcViewModel.StatusCode = item.Exchange.Status.StatusCode != null ? item.Exchange.Status.StatusCode : "";
                            }
                            else
                            {
                                ratioReportSelfQcViewModel.StatusCode = "";
                            }
                            if (item.Exchange != null && item.Exchange.TblSelfQcs != null && item.Exchange.TblSelfQcs.First().User != null && item.Exchange.TblSelfQcs.First().User.Email != null)
                            {
                                ratioReportSelfQcViewModel.UserEmailId = SecurityHelper.DecryptString(item.Exchange.TblSelfQcs.First().User.Email, keyString);
                            }
                            else
                            {
                                ratioReportSelfQcViewModel.UserEmailId = "";

                            }


                            actionURL = " <div class='actionbtns'>";
                            actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>&nbsp;";
                            actionURL = actionURL + "<a href ='" + URL + "/Exchange/Manage?Exchangeorderid=" + _protector.Encode(item.Exchange.Id) + "' onclick='RecordView(" + item.Exchange.Id + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                            actionURL = actionURL + "</div>";
                            ratioReportSelfQcViewModel.Action = actionURL;
                            ratioReportSelfQcViewModelList.Add(ratioReportSelfQcViewModel);
                        }
                    }
                    #endregion
                }
                #endregion
                var data = ratioReportSelfQcViewModelList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion

        #region Video Compressor added by Pooja
        [HttpPost]
        public async Task<ActionResult> CompressVideo(IFormFile file, int? orderTransId, int? statusId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Video file is null");
            }
            try
            {
                string RegdNo = Request.Form["regdNo"].FirstOrDefault();
                bool IsMediaTypeVideo = bool.Parse(Request.Form["isMediaTypeVideo"].FirstOrDefault());
                int SrNum = int.Parse(Request.Form["srNum"].FirstOrDefault());
                int ImageLabelId = int.Parse(Request.Form["imageLabelId"].FirstOrDefault());

                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string tempInputFilePath = Path.Combine(documentsPath, "input_video.mp4");
                string outputFilePath = Path.Combine(documentsPath, "compressed_video.mp4");

                await using (var fileStream = new FileStream(tempInputFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
                {
                    await file.CopyToAsync(fileStream);
                }

                using (var engine = new Engine())
                {
                    var inputFile = new MediaFile { Filename = tempInputFilePath };
                    var outputFile = new MediaFile { Filename = outputFilePath };

                    engine.GetMetadata(inputFile);

                    var options = new ConversionOptions
                    {
                        VideoAspectRatio = VideoAspectRatio.R3_2,
                        VideoSize = VideoSize.Hd480,
                        AudioSampleRate = AudioSampleRate.Hz22050,
                        VideoBitRate = 620,
                        VideoFps = 54,
                    };

                    engine.Convert(inputFile, outputFile, options);
                }

                byte[] compressedfile = await System.IO.File.ReadAllBytesAsync(outputFilePath);

                // Delete files asynchronously
                await Task.WhenAll(
                    DeleteFileAsync(tempInputFilePath),
                    DeleteFileAsync(outputFilePath)
                );

                string bytedata = Convert.ToBase64String(compressedfile);

                if (!string.IsNullOrEmpty(RegdNo) && !string.IsNullOrEmpty(bytedata) && SrNum > 0)
                {
                    bool flag = _QCManager.SaveMediaFile(RegdNo, bytedata, IsMediaTypeVideo, SrNum, orderTransId, statusId, ImageLabelId);
                    if (!flag)
                    {
                        return new JsonResult("Failed to save compressed file.");
                    }
                }

                return new JsonResult(bytedata);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new JsonResult("An error occurred during processing.");
            }
        }

        private async Task DeleteFileAsync(string filePath)
        {
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    await Task.Run(() => System.IO.File.Delete(filePath));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
            }
        }
        #endregion

    }
}
