using AutoMapper;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.EVC;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC;
using RDCELERP.Model.EVC_Allocated;
using RDCELERP.Model.EVCdispute;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.LGC;
using RDCELERP.Model.LGCMobileApp;
using RDCELERP.Model.QCComment;
using static Org.BouncyCastle.Math.EC.ECCurve;
using static RDCELERP.Model.Whatsapp.WhatsappRescheduleViewModel;

namespace RDCELERP.Core.App.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LGCListController : ControllerBase
    {
        #region veriable decleration 
        private Digi2l_DevContext _context;
        private IMapper _mapper;
        private CustomDataProtection _protector;
        private readonly IOptions<ApplicationSettings> _config;
        ICompanyRepository _companyRepository;
        IBusinessUnitRepository _businessUnitRepository;
        #endregion

        #region Controller 
        public LGCListController(IMapper mapper, Digi2l_DevContext context, CustomDataProtection protector, IOptions<ApplicationSettings> config, ICompanyRepository companyRepository, IBusinessUnitRepository businessUnitRepository)
        {
            _context = context;
            _mapper = mapper;
            _protector = protector;
            _config = config;
            _companyRepository = companyRepository;
            _businessUnitRepository = businessUnitRepository;
        }
        #endregion

        #region ADDED BY Priyanshi Sahu---- Get All Assign Order Record for Ticket Generation (Optimized and Modified by VK for ABB Redemption Date : 20-June-2023)
        public IActionResult GetAssignOrderListforTicketGenretion(int TableNo,int companyId, DateTime? orderStartDate,
            DateTime? orderEndDate, string? companyName, int? productCatId,
            int? productTypeId, string? regdNo, string? phoneNo, string? custCity, string? evcregdNo, string? evcstoreCode, int? userId)

        {
            #region Variable Declaration
            List<TblOrderTran> TblOrderTrans = null;
            List<TblWalletTransaction> tblWalletTransactions = null;
            IQueryable<TblWalletTransaction> tblWalletTransQueryable = null;
            List<TblWalletTransaction> tblWalletTransactionforlists = null;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            //int count = 0;
            decimal? IsProgress = 0;
            decimal? IsDeleverd = 0;
            decimal? RuningBlns = 0;
            string? URL = _config.Value.URLPrefixforProd;
            #region Trim string values from parameters
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
            if (!string.IsNullOrWhiteSpace(evcregdNo) && evcregdNo != "null")
            { evcregdNo = evcregdNo.Trim().ToLower(); }
            else { evcregdNo = null; }
            if (!string.IsNullOrWhiteSpace(evcstoreCode) && evcstoreCode != "null")
            { evcstoreCode = evcstoreCode.Trim().ToLower(); }
            else { evcstoreCode = null; }
            #endregion
            #endregion

            try
            {
                #region Datatable variables
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

                #region TblWalletTransaction initialization Modified by Vk
                if (TableNo == 1)
                {
                    tblWalletTransQueryable = _context.TblWalletTransactions
                   .Include(x => x.Evcregistration)
                   .Include(x => x.Evcpartner)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.Status)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                  //Changes for ABB Redemption by Vk 
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Status)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    //Changes for ABB Redemption by Vk
                    .Where(x => x.IsActive == true && x.OrderTrans != null 
                    && x.StatusId != null && x.Evcregistration != null
                    && ((orderStartDate == null && orderEndDate == null) || (x.OrderofAssignDate >= orderStartDate && x.OrderofAssignDate <= orderEndDate))
                    && x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted)
                    && (userId == null || x.OrderTrans.CreatedBy == userId)
                    && x.OrderTrans.RegdNo != null && x.OrderTrans.FinalPriceAfterQc != null
                    && (((x.OrderTrans.Exchange != null)
                    && (tblBusinessUnit == null || x.OrderTrans.Exchange.CompanyName == tblBusinessUnit.Name)
                    && x.OrderTrans.Exchange.FinalExchangePrice != null
                    ) || ((x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null)
                    && (tblBusinessUnit == null || x.OrderTrans.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId)
                    )));
                }
                else if (TableNo == 2)
                {
                    tblWalletTransQueryable = _context.TblWalletTransactions
                   .Include(x => x.Evcregistration)
                   .Include(x => x.Evcpartner)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.Status)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                  //Changes for ABB Redemption by Vk 
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Status)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    //Changes for ABB Redemption by Vk
                    .Where(x => x.IsActive == true && x.OrderTrans != null
                    && x.StatusId != null && x.Evcregistration != null
                    && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= orderStartDate && x.ModifiedDate <= orderEndDate))
                    && x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenforLogistics)
                    && (userId == null || x.OrderTrans.CreatedBy == userId)
                    && x.OrderTrans.RegdNo != null && x.OrderTrans.FinalPriceAfterQc != null
                    && (((x.OrderTrans.Exchange != null)
                    && (tblBusinessUnit == null || x.OrderTrans.Exchange.CompanyName == tblBusinessUnit.Name)
                    && x.OrderTrans.Exchange.FinalExchangePrice != null
                    ) || ((x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null)
                    && (tblBusinessUnit == null || x.OrderTrans.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId)
                    )));
                }
                else if (TableNo == 3)
                {
                    tblWalletTransQueryable = _context.TblWalletTransactions
                   .Include(x => x.Evcregistration)
                   .Include(x => x.Evcpartner)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.Status)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                    //Changes for ABB Redemption by Vk 
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Status)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    //Changes for ABB Redemption by Vk
                    .Where(x => x.IsActive == true && x.OrderTrans != null
                    && x.StatusId != null && x.Evcregistration != null
                    && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= orderStartDate && x.ModifiedDate <= orderEndDate))
                    && x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.PickupReschedule)
                    && (userId == null || x.OrderTrans.CreatedBy == userId)
                    && x.OrderTrans.RegdNo != null && x.OrderTrans.FinalPriceAfterQc != null
                    && (((x.OrderTrans.Exchange != null)
                    && (tblBusinessUnit == null || x.OrderTrans.Exchange.CompanyName == tblBusinessUnit.Name)
                    && x.OrderTrans.Exchange.FinalExchangePrice != null
                    ) || ((x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null)
                    && (tblBusinessUnit == null || x.OrderTrans.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId)
                    )));
                }
                else if (TableNo == 4)
                {
                    tblWalletTransQueryable = _context.TblWalletTransactions
                   .Include(x => x.Evcregistration)
                   .Include(x => x.Evcpartner)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.Status)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                    //Changes for ABB Redemption by Vk 
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Status)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    //Changes for ABB Redemption by Vk
                    .Where(x => x.IsActive == true && x.OrderTrans != null
                    && x.StatusId != null && x.Evcregistration != null
                    && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= orderStartDate && x.ModifiedDate <= orderEndDate))
                    && x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.PickupTicketcancellationbyUTC)
                    && (userId == null || x.OrderTrans.CreatedBy == userId)
                    && x.OrderTrans.RegdNo != null && x.OrderTrans.FinalPriceAfterQc != null
                    && (((x.OrderTrans.Exchange != null)
                    && (tblBusinessUnit == null || x.OrderTrans.Exchange.CompanyName == tblBusinessUnit.Name)
                    && x.OrderTrans.Exchange.FinalExchangePrice != null
                    ) || ((x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null)
                    && (tblBusinessUnit == null || x.OrderTrans.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId)
                    )));
                }

                #region Search Filter Old
                // tblWalletTransQueryable = tblWalletTransQueryable.Where(x =>
                //    x.RegdNo.ToLower().Contains(searchValue.ToLower().Trim())
                //|| (x.Evcregistration.EvcregdNo != null ? x.Evcregistration.EvcregdNo.ToLower().Contains(searchValue.ToLower().Trim()) : false)
                //|| (x.Evcregistration.EvcwalletAmount != null ? x.Evcregistration.EvcwalletAmount.ToString().Contains(searchValue.ToLower().Trim()) : false)
                //|| (x.Evcregistration.BussinessName != null ? x.Evcregistration.BussinessName.ToLower().Contains(searchValue.ToLower().Trim()) : false)
                ////Changes for ABB Redemption by Vk
                //|| (((x.OrderTrans.Exchange != null)
                //&& ((x.OrderTrans.Exchange.Brand != null ? (x.OrderTrans.Exchange.Brand.Name ?? "").ToLower().Contains(searchValue.ToLower().Trim()) : false)
                //|| (x.OrderTrans.Exchange.CustomerDetails != null ? (x.OrderTrans.Exchange.CustomerDetails.ZipCode ?? "").ToLower().Contains(searchValue.ToLower().Trim()) : false)
                //|| (x.OrderTrans.Exchange.CustomerDetails != null ? (x.OrderTrans.Exchange.CustomerDetails.FirstName ?? "").ToLower().Contains(searchValue.ToLower().Trim()) : false)
                //|| (x.OrderTrans.Exchange.CustomerDetails != null ? (x.OrderTrans.Exchange.CustomerDetails.City ?? "").ToLower().Contains(searchValue.ToLower().Trim()) : false)
                //|| (x.OrderTrans.Exchange.CustomerDetails != null ? (x.OrderTrans.Exchange.CustomerDetails.PhoneNumber ?? "").ToLower().Contains(searchValue.ToLower().Trim()) : false)
                //|| (x.OrderTrans.Exchange.ProductType != null ? (x.OrderTrans.Exchange.ProductType.Description ?? "").ToLower().Contains(searchValue.ToLower().Trim()) : false)
                //|| (x.OrderTrans.Exchange.ProductType.ProductCat != null ? (x.OrderTrans.Exchange.ProductType.ProductCat.Description ?? "").ToLower().Contains(searchValue.ToLower().Trim()) : false)
                //|| (x.OrderTrans.Exchange.CustomerDetails != null ? (x.OrderTrans.Exchange.CustomerDetails.State ?? "").ToLower().Contains(searchValue.ToLower().Trim()) : false)
                //|| (x.OrderTrans.Exchange.Status.StatusCode != null ? (x.OrderTrans.Exchange.Status.StatusCode ?? "").ToLower().Contains(searchValue.ToLower().Trim()) : false)
                //|| (x.OrderTrans.Exchange.CustomerDetails != null ? (x.OrderTrans.Exchange.CustomerDetails.State ?? "").ToLower().Contains(searchValue.ToLower().Trim()) : false)
                //))
                //|| ((x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null)
                //&& ((x.OrderTrans.Abbredemption.CustomerDetails != null ? (x.OrderTrans.Abbredemption.CustomerDetails.ZipCode ?? "").ToLower().Contains(searchValue.ToLower().Trim()) : false)
                //|| (x.OrderTrans.Abbredemption.CustomerDetails != null ? (x.OrderTrans.Abbredemption.CustomerDetails.FirstName ?? "").ToLower().Contains(searchValue.ToLower().Trim()) : false)
                //|| (x.OrderTrans.Abbredemption.CustomerDetails != null ? (x.OrderTrans.Abbredemption.CustomerDetails.City ?? "").ToLower().Contains(searchValue.ToLower().Trim()) : false)
                //|| (x.OrderTrans.Abbredemption.CustomerDetails != null ? (x.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber ?? "").ToLower().Contains(searchValue.ToLower().Trim()) : false)
                //|| (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null ? (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description ?? "").ToLower().Contains(searchValue.ToLower().Trim()) : false)
                //|| (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null ? (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description ?? "").ToLower().Contains(searchValue.ToLower().Trim()) : false)
                //|| (x.OrderTrans.Abbredemption.CustomerDetails != null ? (x.OrderTrans.Abbredemption.CustomerDetails.State ?? "").ToLower().Contains(searchValue.ToLower().Trim()) : false)
                //|| (x.OrderTrans.Abbredemption.Status.StatusCode != null ? (x.OrderTrans.Abbredemption.Status.StatusCode ?? "").ToLower().Contains(searchValue.ToLower().Trim()) : false)
                //|| (x.OrderTrans.Abbredemption.CustomerDetails != null ? (x.OrderTrans.Abbredemption.CustomerDetails.State ?? "").ToLower().Contains(searchValue.ToLower().Trim()) : false)
                //))));
                #endregion

                #region Advanced Filters
                if (tblWalletTransQueryable != null && tblWalletTransQueryable.Count() > 0)
                {
                    tblWalletTransQueryable = tblWalletTransQueryable.Where(x =>
                   (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                   //&& (string.IsNullOrEmpty(evcregdNo) || (x.Evcregistration != null && (x.Evcregistration.EvcregdNo ?? "".Concat("-") + x.Evcregistration.BussinessName ?? "").ToLower().Contains(evcregdNo)))
                   && (string.IsNullOrEmpty(evcregdNo) || (x.Evcregistration != null && ((x.Evcregistration.EvcregdNo ?? "").Trim() +"-"+ (x.Evcregistration.BussinessName ?? "").Trim()).ToLower().Contains(evcregdNo)))
                   && (string.IsNullOrEmpty(evcstoreCode) || (x.Evcpartner.EvcStoreCode ?? "").ToLower().Contains(evcstoreCode))
                   && (x.OrderTrans != null &&
                   ((x.OrderTrans.Exchange != null
                   && (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCat != null)
                   && (productCatId == null || x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId)
                   && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId)
                   && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.PhoneNumber == phoneNo))
                   && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Exchange.CustomerDetails != null && (x.OrderTrans.Exchange.CustomerDetails.City ?? "").ToLower() == custCity))
                   && (string.IsNullOrEmpty(companyName) || (x.OrderTrans.Exchange.CompanyName ?? "").ToLower() == companyName)
                   )
                   || (x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null
                   && (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                   && (productCatId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryId == productCatId)
                   && (productTypeId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId)
                   && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Abbredemption.CustomerDetails != null && x.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber == phoneNo))
                   && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Abbredemption.CustomerDetails != null && (x.OrderTrans.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity))
                   && (string.IsNullOrEmpty(companyName) || (x.OrderTrans.Abbredemption.Abbregistration.BusinessUnitId != null && (x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName))
                   ))));
                }
                #endregion
                #endregion

                #region Sorting and Pagination Modified by Vk
                //Code for Pagination
                recordsTotal = tblWalletTransQueryable != null ? tblWalletTransQueryable.Count() : 0;
                tblWalletTransactions = tblWalletTransQueryable
                    .OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.OrderTransId)
                    .Skip(skip).Take(pageSize).ToList();
                if (tblWalletTransactions != null)
                {
                    tblWalletTransactions = sortColumnDirection.Equals(SortingOrder.ASCENDING)
    ? tblWalletTransactions.OrderBy(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
                          .ToList()
    : tblWalletTransactions.OrderByDescending(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
                          .ToList();
                }
                else
                {
                    tblWalletTransactions = new List<TblWalletTransaction>();
                }
                #endregion

                #region TblWalletTransaction to model mapping Modified by Vk
                List<AssignOrderViewModel> assignOrderViewModels = _mapper.Map<List<TblWalletTransaction>, List<AssignOrderViewModel>>(tblWalletTransactions);

                string actionURL = string.Empty;
                string actionURL1 = string.Empty;

                foreach (AssignOrderViewModel item in assignOrderViewModels)
                {
                    TblOrderTran tblOrderTrans = _context.TblOrderTrans.Where(x => x.IsActive == true && x.OrderTransId == item.OrderTransId).FirstOrDefault();
                    actionURL = " <td class='actions'>";
                    actionURL = actionURL + "<a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a> &nbsp;";
                    //actionURL = actionURL + "<a class='btn btn-sm btn-primary' target='_blanck'  href='" + _config.Value.MVCBaseURL + "/Home/CreateTicketForLogistics?RegdNo=" + item.RegdNo + "' title='Ticket Genrate'><i class='fa-solid fa-ticket'></i></a> &nbsp;";
                    
                    if (tblOrderTrans.StatusId != 34)
                    {
                        actionURL = actionURL + "<a href ='" + URL + "/LGC_Admin/OrderViewPage?OrderTransId=" + (item.OrderTransId) + "' ><button onclick='View(" + item.OrderTransId + ")' class='btn btn-sm btn-primary'><i class='fa-solid fa-eye'></i></button></a>" + "&nbsp;<button onclick='ReassignOrder(" + item.OrderTransId + ")' class='btn btn-primary btn-sm ml-1'>Change EVC</button> &nbsp;";
                    }
                   

                    actionURL1 = " <td class='actions'>";
                    actionURL1 = actionURL1 + " <span><input type='checkbox' id='EVCAllocionCB' value=" + item.RegdNo + " onclick='OnCheckBoxCheck();' class='checkboxinput' /></span>";
                    actionURL1 = actionURL1 + " </td>";
                    item.Edit = actionURL1;

                    TblWalletTransaction OrdeDetails = tblWalletTransactions.FirstOrDefault(x => x.OrderTransId == item.OrderTransId);
                    item.OrderTransId = OrdeDetails.OrderTransId;

                    string? regdNoVar = null; string? productTypeDesc = null; string? productCatDesc = null; string? statusCode = null;
                    string? custFirstName = null; string? custLastName = null; string? custCityVar = null; string? custPincode = null; decimal? finalPrice = null;
                    if (OrdeDetails != null && OrdeDetails.OrderTrans != null)
                    {
                        #region Exchange and ABB common Configuraion Add by VK
                        if (OrdeDetails.OrderTrans.Exchange != null)
                        {
                            regdNoVar = OrdeDetails.OrderTrans.Exchange.RegdNo;
                            finalPrice = OrdeDetails.OrderTrans.Exchange.FinalExchangePrice;
                            if (OrdeDetails.OrderTrans.Exchange.ProductType != null)
                            {
                                productTypeDesc = OrdeDetails.OrderTrans.Exchange.ProductType.Description;
                                if (OrdeDetails.OrderTrans.Exchange.ProductType.ProductCat != null)
                                {
                                    productCatDesc = OrdeDetails.OrderTrans.Exchange.ProductType.ProductCat.Description;
                                }
                            }
                            if (OrdeDetails.OrderTrans.Exchange.Status != null)
                            {
                                statusCode = OrdeDetails.OrderTrans.Exchange.Status.StatusCode;
                            }
                            if (OrdeDetails.OrderTrans.Exchange.CustomerDetails != null)
                            {
                                custFirstName = OrdeDetails.OrderTrans.Exchange.CustomerDetails.FirstName;
                                custLastName = OrdeDetails.OrderTrans.Exchange.CustomerDetails.LastName;
                                custCityVar = OrdeDetails.OrderTrans.Exchange.CustomerDetails.City;
                                custPincode = OrdeDetails.OrderTrans.Exchange.CustomerDetails.ZipCode;
                            }
                        }
                        else if (OrdeDetails.OrderTrans.Abbredemption != null && OrdeDetails.OrderTrans.Abbredemption.Abbregistration != null)
                        {
                            regdNoVar = OrdeDetails.OrderTrans.RegdNo;
                            finalPrice = OrdeDetails.OrderTrans.FinalPriceAfterQc;
                            if (OrdeDetails.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null)
                            {
                                productTypeDesc = OrdeDetails.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description;
                            }
                            if (OrdeDetails.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                            {
                                productCatDesc = OrdeDetails.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description;
                            }
                            if (OrdeDetails.OrderTrans.Status != null)
                            {
                                statusCode = OrdeDetails.OrderTrans.Status.StatusCode;
                            }
                            if (OrdeDetails.OrderTrans.Abbredemption.CustomerDetails != null)
                            {
                                custFirstName = OrdeDetails.OrderTrans.Abbredemption.CustomerDetails.FirstName;
                                custLastName = OrdeDetails.OrderTrans.Abbredemption.CustomerDetails.LastName;
                                custCityVar = OrdeDetails.OrderTrans.Abbredemption.CustomerDetails.City;
                                custPincode = OrdeDetails.OrderTrans.Abbredemption.CustomerDetails.ZipCode;
                            }
                        }
                        #endregion

                        item.EvcRate = OrdeDetails.OrderAmount > 0 ? OrdeDetails.OrderAmount : 0;
                        item.RegdNo = regdNoVar;
                        item.OldProdType = productTypeDesc;
                        item.ExchProdGroup = productCatDesc;
                        item.StatusCode = statusCode;
                        item.FirstName = custFirstName;
                        item.LastName = custLastName;
                        item.CustCity = custCityVar;
                        item.CustPin = custPincode;
                        item.FinalExchangePrice = finalPrice;

                        if (OrdeDetails.Evcregistration != null)
                        {
                            item.EvcRegNo = OrdeDetails.Evcregistration.EvcregdNo != null ? OrdeDetails.Evcregistration.EvcregdNo + "-" + OrdeDetails.Evcregistration.BussinessName : String.Empty;
                        }
                        if(OrdeDetails.Evcpartner != null)
                        {
                            item.EvcStoreCode = OrdeDetails.Evcpartner != null ? OrdeDetails.Evcpartner.EvcStoreCode : String.Empty;
                        }
                        if (OrdeDetails.OrderofAssignDate != null && OrdeDetails.OrderofAssignDate != null)
                        {
                            var Date = (DateTime)OrdeDetails.OrderofAssignDate;
                            item.Date = Date.ToShortDateString();
                        }
                        if (OrdeDetails.ModifiedDate != null && OrdeDetails.ModifiedDate != null)
                        {
                            var Date = (DateTime)OrdeDetails.ModifiedDate;
                            item.ReOpenDate = Date.ToShortDateString();
                        }
                        if (OrdeDetails.IsPrimeProductId != null)
                        {
                            if (OrdeDetails.IsPrimeProductId == true)
                            {
                                item.IsPrimeProductId = "Yes";
                            }
                            else
                            {
                                item.IsPrimeProductId = "No";
                            }
                        }

                        if (OrdeDetails.Evcregistration.EvcwalletAmount != null )
                        {
                            item.EvcWalletAmount = OrdeDetails.Evcregistration.EvcwalletAmount != null ? OrdeDetails.Evcregistration.EvcwalletAmount : 0;
                            tblWalletTransactionforlists = _context.TblWalletTransactions.Where(x => x.EvcregistrationId == OrdeDetails.EvcregistrationId && x.IsActive == true && x.StatusId != 26.ToString() && x.StatusId != 21.ToString() && x.StatusId != 44.ToString()).ToList();

                            if (tblWalletTransactionforlists != null)
                            {
                                TblWalletTransaction tblWalletTransactionslist = new TblWalletTransaction();
                                foreach (var items in tblWalletTransactionforlists)
                                {
                                    if (items.OrderofAssignDate != null && items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate == null && items.OrderOfCompleteDate == null && items.StatusId != 26.ToString() && items.StatusId != 21.ToString() && items.StatusId != 44.ToString())
                                    {
                                        if (items.OrderAmount != null)
                                        {
                                            IsProgress += items.OrderAmount;
                                        }
                                    }
                                    if (items.OrderofAssignDate != null && items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate != null && items.OrderOfCompleteDate == null && items.StatusId != 26.ToString() && items.StatusId != 21.ToString() && items.StatusId != 44.ToString())
                                    {
                                        if (items.OrderAmount != null)
                                        {
                                            IsDeleverd += items.OrderAmount;
                                        }
                                    }
                                }

                                RuningBlns = OrdeDetails.Evcregistration.EvcwalletAmount - (IsProgress + IsDeleverd);
                                item.clearBalance = (decimal)RuningBlns;
                                RuningBlns = 0;
                                IsProgress = 0;
                                IsDeleverd = 0;
                            }
                        }
                        if (item.clearBalance == 0 || item.clearBalance < 0 || item.clearBalance < item.EvcRate)
                        {
                            if (tblOrderTrans.StatusId == 34)
                            {
                                TblCreditRequest? tblCreditRequest = _context.TblCreditRequests.Where(x => x.WalletTransactionId == item.WalletTransactionId && x.IsActive == true ).FirstOrDefault();
                                if (tblCreditRequest == null)
                                {
                                    actionURL = actionURL + "&nbsp;<button onclick='GenrateCreditRequest(" + item.WalletTransactionId + ")' class='btn btn-primary btn-sm ml-1'>Credit Request</button> &nbsp;";
                                }
                                else if (tblCreditRequest.IsCreditRequest == true && tblCreditRequest.IsCreditRequestApproved == false)
                                {
                                    actionURL += "<p class='btn btn-sm btn-danger'>Waiting for Approval</p> &nbsp;";
                                }
                                else
                                { 
                                    actionURL = actionURL + "&nbsp;<button  class='btn btn-success btn-sm ml-1'>Request Approve</button> &nbsp;";
                                }
                            }
                            
                        }
                        actionURL = actionURL + " </td>";
                        item.Action = actionURL;
                        if (TableNo == 3)
                        {
                            var logistic = _context.TblLogistics.Where(x => x.IsActive == false && x.Rescheduledate != null && x.OrderTransId == OrdeDetails.OrderTransId).ToList().LastOrDefault();
                            if (logistic != null)
                            {
                                var RescheduleDate1 = (DateTime)logistic.Rescheduledate;
                                item.RescheduleDate = RescheduleDate1.ToShortDateString();
                                item.RescheduleTime= RescheduleDate1.ToShortTimeString();
                            }
                        }
                        TblOrderQc? tblOrderQc1 = _context.TblOrderQcs.Where(x => x.OrderTransId == item.OrderTransId && x.IsActive==true && x.PreferredPickupDate!=null && x.PickupStartTime!=null && x.PickupEndTime !=null ).FirstOrDefault();
                        if (tblOrderQc1 != null)
                        {
                            var pickupScheduleDate1 = (DateTime)tblOrderQc1.PreferredPickupDate;
                            item.pickupScheduleDate = pickupScheduleDate1.ToShortDateString();
                            item.pickupScheduletime = tblOrderQc1.PickupStartTime+" - "+ tblOrderQc1.PickupEndTime;
                        }
                    }
                }
                #endregion

                var data = assignOrderViewModels;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region ADDED BY Priyanshi Sahu---- Get All Ready for Pickup List (Optimized and Modified by VK for ABB Redemption Date : 21-June-2023)
        public async Task<ActionResult> ReadyforPickup(int? ServicePartnerId, DateTime? startDate, DateTime? endDate)
        {
            #region Variable Declaration
            List<TblLogistic> tblLogistic = null;
            TblServicePartner tblServicePartner = null;
            List<PickupOrderViewModel> lGCOrderList = null;
            PickupOrderViewModel lGCOrderViewModel = null;
            string URL = _config.Value.URLPrefixforProd;
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
                searchValue = searchValue.ToLower().Trim();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                int count = 0;
                #endregion

                #region Date Filter and ServicePartner
                if (startDate != null && endDate != null)
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                }
                if (ServicePartnerId == 0)
                {
                    ServicePartnerId = null;
                }
                #endregion

                //tblServicePartner = _context.TblServicePartners.Where(x => x.UserId == userId && x.IsActive == true).FirstOrDefault();
                #region tblLogistic Implementation
                count = _context.TblLogistics
                            .Include(x => x.TblOrderLgcs)
                             .Include(x => x.ServicePartner)
                             .Include(x => x.Status)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                                        //Changes for ABB Redemption by Vk 
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                                        //Changes for ABB Redemption by Vk
                                        .Count(x => x.IsActive == true && x.OrderTrans != null && x.OrderTrans.IsActive == true
                                        && x.StatusId == 18 && x.OrderTrans.StatusId == 18
                                        && (ServicePartnerId == null || (x.ServicePartner != null && x.ServicePartnerId == ServicePartnerId))
                                        && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                                        && ((string.IsNullOrEmpty(searchValue)
                                        || (x.RegdNo ?? "").ToLower().Contains(searchValue)
                                        || (x.TicketNumber ?? "").ToLower().Contains(searchValue)
                                        || x.ServicePartner.ServicePartnerDescription.ToLower().Contains(searchValue)
                                        || (x.Status.StatusCode ?? "").ToLower().Contains(searchValue)
                                        || (x.OrderTrans.Exchange == null ? false :
                                        ((x.OrderTrans.Exchange.ProductType == null ? false : (x.OrderTrans.Exchange.ProductType.Description ?? "").ToLower().Contains(searchValue))
                                        || (x.OrderTrans.Exchange.ProductType.ProductCat == null ? false : (x.OrderTrans.Exchange.ProductType.ProductCat.Description ?? "").ToLower().Contains(searchValue))
                                        || (x.OrderTrans.Exchange.CustomerDetails == null ? false : (x.OrderTrans.Exchange.CustomerDetails.City ?? "").ToLower().Contains(searchValue))
                                        || (x.OrderTrans.Exchange.CustomerDetails == null ? false : (x.OrderTrans.Exchange.CustomerDetails.PhoneNumber ?? "").ToLower().Contains(searchValue))
                                        )) // For ABB Redumption
                                        || ((x.OrderTrans.Abbredemption == null && x.OrderTrans.Abbredemption.Abbregistration == null) ? false :
                                         ((x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null ? (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description ?? "").ToLower().Contains(searchValue) : false)
                                         || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null ? (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description ?? "").ToLower().Contains(searchValue) : false)
                                         || (x.OrderTrans.Abbredemption.CustomerDetails == null ? false : (x.OrderTrans.Abbredemption.CustomerDetails.City ?? "").ToLower().Contains(searchValue))
                                         || (x.OrderTrans.Abbredemption.CustomerDetails == null ? false : (x.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber ?? "").ToLower().Contains(searchValue))
                                        ))
                                        )));
                if (count > 0)
                {
                    tblLogistic = _context.TblLogistics
                            .Include(x => x.TblOrderLgcs)
                             .Include(x => x.ServicePartner)
                             .Include(x => x.Status)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                                        //Changes for ABB Redemption by Vk 
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                                        //Changes for ABB Redemption by Vk
                                        .Where(x => x.IsActive == true && x.OrderTrans != null && x.OrderTrans.IsActive == true
                                        && x.StatusId == 18 && x.OrderTrans.StatusId == 18
                                        && (ServicePartnerId == null || (x.ServicePartner != null && x.ServicePartnerId == ServicePartnerId))
                                        && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                                        && ((string.IsNullOrEmpty(searchValue)
                                        || x.RegdNo.ToLower().Contains(searchValue)
                                        || x.TicketNumber.ToLower().Contains(searchValue)
                                        || x.ServicePartner.ServicePartnerDescription.ToLower().Contains(searchValue)
                                        || x.Status.StatusCode.ToLower().Contains(searchValue)
                                        || (x.OrderTrans.Exchange == null ? false :
                                        ((x.OrderTrans.Exchange.ProductType == null ? false : (x.OrderTrans.Exchange.ProductType.Description ?? "").ToLower().Contains(searchValue))
                                        || (x.OrderTrans.Exchange.ProductType.ProductCat == null ? false : (x.OrderTrans.Exchange.ProductType.ProductCat.Description ?? "").ToLower().Contains(searchValue))
                                        || (x.OrderTrans.Exchange.CustomerDetails == null ? false : (x.OrderTrans.Exchange.CustomerDetails.City ?? "").ToLower().Contains(searchValue))
                                        || (x.OrderTrans.Exchange.CustomerDetails == null ? false : (x.OrderTrans.Exchange.CustomerDetails.PhoneNumber ?? "").ToLower().Contains(searchValue))
                                        )) // For ABB Redumption
                                        || ((x.OrderTrans.Abbredemption == null && x.OrderTrans.Abbredemption.Abbregistration == null) ? false :
                                         ((x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null ? (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description ?? "").ToLower().Contains(searchValue) : false)
                                         || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null ? (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description ?? "").ToLower().Contains(searchValue) : false)
                                         || (x.OrderTrans.Abbredemption.CustomerDetails == null ? false : (x.OrderTrans.Abbredemption.CustomerDetails.City ?? "").ToLower().Contains(searchValue))
                                         || (x.OrderTrans.Abbredemption.CustomerDetails == null ? false : (x.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber ?? "").ToLower().Contains(searchValue))
                                        ))
                                        ))).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.LogisticId).Skip(skip).Take(pageSize).ToList();
                }
                recordsTotal = count;
                #endregion

                #region Sorting
                if (tblLogistic != null)
                {
                    tblLogistic = sortColumnDirection.Equals(SortingOrder.ASCENDING)
 ? tblLogistic.OrderBy(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
                       .ToList()
 : tblLogistic.OrderByDescending(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
                       .ToList();
                }
                #endregion

                #region table to model mapping
                lGCOrderList = new List<PickupOrderViewModel>();
                if (tblLogistic != null && tblLogistic.Count > 0)
                {
                    foreach (var item in tblLogistic)
                    {
                        string actionURL = string.Empty;
                        actionURL = " <ul class='actions'>";
                        actionURL = "<a href ='" + URL + "/LGC_Admin/OrderViewPage?OrderTransId=" + (item.OrderTransId) + "' ><button onclick='View(" + item.OrderTransId + ")' class='btn btn-sm btn-primary'><i class='fa-solid fa-eye'></i></button></a>" + "&nbsp;<button onclick='ReassignOrder(" + item.OrderTransId + ")' class='btn btn-primary btn-sm ml-1'>Change EVC</button>" + " <button onclick = 'CancelTicket(" + item.OrderTransId + ")' class='btn btn-sm btn-primary'>Change LGC</button> &nbsp;";
                        actionURL = actionURL + "</ul>";

                        string productTypeDesc = null; string productCatDesc = null; string statusCode = null;
                        string servicePartnerName = null; string custCity = null;
                        if (item != null)
                        {
                            #region Exchange and ABB common Configuraion Add by VK
                            if (item.OrderTrans.Exchange != null)
                            {
                                if (item.OrderTrans.Exchange.ProductType != null)
                                {
                                    productTypeDesc = item.OrderTrans.Exchange.ProductType.Description;
                                    if (item.OrderTrans.Exchange.ProductType.ProductCat != null)
                                    {
                                        productCatDesc = item.OrderTrans.Exchange.ProductType.ProductCat.Description;
                                    }
                                }
                                if (item.OrderTrans.Exchange.CustomerDetails != null)
                                {
                                    custCity = item.OrderTrans.Exchange.CustomerDetails.City;
                                }
                            }
                            else if (item.OrderTrans.Abbredemption != null && item.OrderTrans.Abbredemption.Abbregistration != null)
                            {
                                if (item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null)
                                {
                                    productTypeDesc = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description;
                                }
                                if (item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                                {
                                    productCatDesc = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description;
                                }
                                if (item.OrderTrans.Abbredemption.CustomerDetails != null)
                                {
                                    custCity = item.OrderTrans.Abbredemption.CustomerDetails.City;
                                }
                            }
                            if (item.Status != null)
                            {
                                statusCode = item.Status.StatusCode;
                            }
                            if (item.ServicePartner != null)
                            {
                                servicePartnerName = item.ServicePartner.ServicePartnerName;
                            }
                            #endregion

                            lGCOrderViewModel = new PickupOrderViewModel();
                            lGCOrderViewModel.Action = actionURL;
                            lGCOrderViewModel.Id = item.LogisticId > 0 ? item.LogisticId : 0;
                            lGCOrderViewModel.RegdNo = item.RegdNo != null ? item.RegdNo : String.Empty;
                            lGCOrderViewModel.TicketNumber = item.TicketNumber != null ? item.TicketNumber : String.Empty;
                            lGCOrderViewModel.ProductCategory = productCatDesc;
                            lGCOrderViewModel.ProductType = productTypeDesc;
                            lGCOrderViewModel.AmountPayableThroughLGC = Convert.ToDecimal(item.AmtPaybleThroughLgc);
                            lGCOrderViewModel.City = custCity;
                            lGCOrderViewModel.ServicePartnerName = servicePartnerName;
                            lGCOrderViewModel.StatusCode = statusCode;
                            TblOrderQc? tblOrderQc1 = _context.TblOrderQcs.Where(x => x.OrderTransId == item.OrderTransId && x.IsActive == true && x.PreferredPickupDate != null && x.PickupStartTime != null && x.PickupEndTime != null).FirstOrDefault();
                            if (tblOrderQc1 != null)
                            {
                               // var pickupScheduleDate1 = (DateTime)tblOrderQc1.PreferredPickupDate;
                                lGCOrderViewModel.PickupScheduleTime = tblOrderQc1.PickupStartTime + " - " + tblOrderQc1.PickupEndTime;
                            }
                            lGCOrderViewModel.PickupScheduleDate = item.PickupScheduleDate != null
                            ? Convert.ToDateTime(item.PickupScheduleDate).ToString("MM/dd/yyyy")
                            : Convert.ToDateTime(item.CreatedDate).ToString("MM/dd/yyyy");

                            if (item.CreatedDate != null)
                            {
                                var Date = (DateTime)item.CreatedDate;
                                lGCOrderViewModel.Date = Date.ToShortDateString();
                            }
                            lGCOrderList.Add(lGCOrderViewModel);
                        }
                    }
                }
                #endregion

                var data = lGCOrderList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region ADDED BY Priyanshi Sahu---- Get All Pickup Done List (Optimized and Modified by VK for ABB Redemption Date : 21-June-2023)
        public async Task<ActionResult> PickupDone(int? ServicePartnerId, DateTime? startDate, DateTime? endDate)
        {
            #region Variable Declaration
            List<TblOrderLgc> tblOrderLgcs = null;
            List<TblLogistic> tblLogistic = null;
            TblServicePartner tblServicePartner = null;
            List<PickupOrderViewModel> lGCOrderList = null;
            PickupOrderViewModel lGCOrderViewModel = null;
            string URL = _config.Value.URLPrefixforProd;
            #endregion

            try
            {
                #region Datatable Variable Declaration
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                searchValue = searchValue.ToLower().Trim();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                int count = 0;
                #endregion

                #region Date Filter and ServicePartner
                if (startDate != null && endDate != null)
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                }
                if (ServicePartnerId == 0)
                {
                    ServicePartnerId = null;
                }
                #endregion

                // tblServicePartner = _context.TblServicePartners.Where(x => x.UserId == userId && x.IsActive == true).FirstOrDefault();
                #region TblOrderLgc Implementation
                count = _context.TblOrderLgcs
                             .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner)
                             .Include(x => x.Evcregistration).Include(x => x.Evcpartner)
                             .Include(x => x.Logistic).ThenInclude(x => x.Status)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                                        //Changes for ABB Redemption by Vk 
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                                        //Changes for ABB Redemption by Vk
                                        .Count(x => x.IsActive == true && x.Logistic != null && x.Logistic.IsActive == true && x.OrderTrans != null && x.OrderTrans.IsActive == true && x.DriverDetailsId == null
                                        && x.Logistic.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup) && x.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup) && x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup)
                                        && (ServicePartnerId == null || (x.Logistic.ServicePartner != null && x.Logistic.ServicePartnerId == ServicePartnerId))
                                        && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                                        && (string.IsNullOrEmpty(searchValue)
                                        || (x.Logistic.RegdNo ?? "").ToLower().Contains(searchValue)
                                        || (x.Logistic.ServicePartner.ServicePartnerDescription ?? "").ToLower().Contains(searchValue)
                                        || (x.Logistic.TicketNumber ?? "").ToLower().Contains(searchValue)
                                        || (x.Logistic.Status.StatusCode ?? "").ToLower().Contains(searchValue)
                                        || (x.Evcpartner != null && (x.Evcpartner.EvcStoreCode ?? "").ToLower().Contains(searchValue))
                                        || (x.OrderTrans.Exchange == null ? false :
                                        ((x.OrderTrans.Exchange.ProductType == null ? false : (x.OrderTrans.Exchange.ProductType.Description ?? "").ToLower().Contains(searchValue))
                                        || (x.OrderTrans.Exchange.ProductType.ProductCat == null ? false : (x.OrderTrans.Exchange.ProductType.ProductCat.Description ?? "").ToLower().Contains(searchValue))
                                        || (x.OrderTrans.Exchange.CustomerDetails == null ? false : (x.OrderTrans.Exchange.CustomerDetails.City ?? "").ToLower().Contains(searchValue))
                                        || (x.OrderTrans.Exchange.CustomerDetails == null ? false : (x.OrderTrans.Exchange.CustomerDetails.PhoneNumber ?? "").ToLower().Contains(searchValue))
                                        )) // For ABB Redumption
                                        || ((x.OrderTrans.Abbredemption == null && x.OrderTrans.Abbredemption.Abbregistration == null) ? false :
                                         ((x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null ? (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description ?? "").ToLower().Contains(searchValue) : false)
                                         || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null ? (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description ?? "").ToLower().Contains(searchValue) : false)
                                         || (x.OrderTrans.Abbredemption.CustomerDetails == null ? false : (x.OrderTrans.Abbredemption.CustomerDetails.City ?? "").ToLower().Contains(searchValue))
                                         || (x.OrderTrans.Abbredemption.CustomerDetails == null ? false : (x.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber ?? "").ToLower().Contains(searchValue))
                                        ))
                                        ));
                if (count > 0)
                {
                    tblOrderLgcs = _context.TblOrderLgcs
                             .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner)
                             .Include(x => x.Evcregistration).Include(x => x.Evcpartner)
                             .Include(x => x.Logistic).ThenInclude(x => x.Status)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                                        //Changes for ABB Redemption by Vk 
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                                        //Changes for ABB Redemption by Vk
                                        .Where(x => x.IsActive == true && x.Logistic.IsActive == true && x.OrderTrans != null 
                                        && x.OrderTrans.IsActive == true && x.DriverDetailsId == null 
                                        && x.Logistic.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup) && x.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup) && x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup)
                                        && (ServicePartnerId == null || (x.Logistic.ServicePartner != null && x.Logistic.ServicePartnerId == ServicePartnerId))
                                        && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                                        && (string.IsNullOrEmpty(searchValue)
                                        || (x.Logistic.RegdNo ?? "").ToLower().Contains(searchValue)
                                        || (x.Logistic.ServicePartner.ServicePartnerDescription ?? "").ToLower().Contains(searchValue)
                                        || (x.Logistic.TicketNumber ?? "").ToLower().Contains(searchValue)
                                        || (x.Logistic.Status.StatusCode ?? "").ToLower().Contains(searchValue)
                                        || (x.Evcpartner != null && (x.Evcpartner.EvcStoreCode ?? "").ToLower().Contains(searchValue))
                                        || (x.OrderTrans.Exchange == null ? false :
                                        ((x.OrderTrans.Exchange.ProductType == null ? false : (x.OrderTrans.Exchange.ProductType.Description ?? "").ToLower().Contains(searchValue))
                                        || (x.OrderTrans.Exchange.ProductType.ProductCat == null ? false : (x.OrderTrans.Exchange.ProductType.ProductCat.Description ?? "").ToLower().Contains(searchValue))
                                        || (x.OrderTrans.Exchange.CustomerDetails == null ? false : (x.OrderTrans.Exchange.CustomerDetails.City ?? "").ToLower().Contains(searchValue))
                                        || (x.OrderTrans.Exchange.CustomerDetails == null ? false : (x.OrderTrans.Exchange.CustomerDetails.PhoneNumber ?? "").ToLower().Contains(searchValue))
                                        )) // For ABB Redumption
                                        || ((x.OrderTrans.Abbredemption == null && x.OrderTrans.Abbredemption.Abbregistration == null) ? false :
                                         ((x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null ? (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description ?? "").ToLower().Contains(searchValue) : false)
                                         || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null ? (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description ?? "").ToLower().Contains(searchValue) : false)
                                         || (x.OrderTrans.Abbredemption.CustomerDetails == null ? false : (x.OrderTrans.Abbredemption.CustomerDetails.City ?? "").ToLower().Contains(searchValue))
                                         || (x.OrderTrans.Abbredemption.CustomerDetails == null ? false : (x.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber ?? "").ToLower().Contains(searchValue))
                                        ))
                                        )).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.OrderLgcid).Skip(skip).Take(pageSize).ToList();
                }
                recordsTotal = count;
                #endregion

                #region Sorting
                if (tblOrderLgcs != null)
                {
                    tblOrderLgcs = sortColumnDirection.Equals(SortingOrder.ASCENDING)
? tblOrderLgcs.OrderBy(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
    .ToList()
: tblOrderLgcs.OrderByDescending(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
    .ToList();
                }
                #endregion

                #region tblOrderLgc to model Mapping
                lGCOrderList = new List<PickupOrderViewModel>();
                if (tblOrderLgcs != null && tblOrderLgcs.Count > 0)
                {
                    foreach (var item in tblOrderLgcs)
                    {
                        string actionURL = string.Empty;
                        var regdno = item.Logistic.RegdNo;
                        actionURL = " <ul class='actions'>";
                        actionURL = "<a href ='" + URL + "/LGC_Admin/OrderViewPage?OrderTransId=" + (item.OrderTransId) + "' ><button onclick='View(" + item.OrderTransId + ")' class='btn btn-sm btn-primary'><i class='fa-solid fa-eye'></i></button></a>" + "&nbsp;<button onclick='ReassignOrder(" + item.OrderTransId + ")' class='btn btn-primary btn-sm ml-1'>Change EVC</button>";
                        actionURL = actionURL + "</ul>";

                        string productTypeDesc = null; string productCatDesc = null;
                        string custCity = null;
                        if (tblOrderLgcs != null)
                        {
                            #region Exchange and ABB common Configuraion Add by VK
                            if (item.OrderTrans.Exchange != null)
                            {
                                if (item.OrderTrans.Exchange.ProductType != null)
                                {
                                    productTypeDesc = item.OrderTrans.Exchange.ProductType.Description;
                                    if (item.OrderTrans.Exchange.ProductType.ProductCat != null)
                                    {
                                        productCatDesc = item.OrderTrans.Exchange.ProductType.ProductCat.Description;
                                    }
                                }
                                if (item.OrderTrans.Exchange.CustomerDetails != null)
                                {
                                    custCity = item.OrderTrans.Exchange.CustomerDetails.City;
                                }
                            }
                            else if (item.OrderTrans.Abbredemption != null && item.OrderTrans.Abbredemption.Abbregistration != null)
                            {
                                if (item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null)
                                {
                                    productTypeDesc = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description;
                                }
                                if (item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                                {
                                    productCatDesc = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description;
                                }
                                if (item.OrderTrans.Abbredemption.CustomerDetails != null)
                                {
                                    custCity = item.OrderTrans.Abbredemption.CustomerDetails.City;
                                }
                            }
                            #endregion

                            lGCOrderViewModel = new PickupOrderViewModel();
                            lGCOrderViewModel.Action = actionURL;
                            lGCOrderViewModel.Id = item.LogisticId != null ? item.LogisticId : 0;
                            lGCOrderViewModel.ProductCategory = productCatDesc;
                            lGCOrderViewModel.ProductType = productTypeDesc;
                            lGCOrderViewModel.City = custCity;
                            lGCOrderViewModel.EVCBusinessName = item.Evcregistration != null && item.Evcregistration.EvcregdNo != null && item.Evcregistration.BussinessName != null ? item.Evcregistration.EvcregdNo + "-" + item.Evcregistration.BussinessName : string.Empty;
                            lGCOrderViewModel.EvcStoreCode = item.Evcpartner?.EvcStoreCode;
                            if (item.Logistic != null)
                            {
                                lGCOrderViewModel.RegdNo = item.Logistic.RegdNo;
                                lGCOrderViewModel.TicketNumber = item.Logistic.TicketNumber;
                                lGCOrderViewModel.AmountPayableThroughLGC = Convert.ToDecimal(item.Logistic.AmtPaybleThroughLgc);
                                if (item.Logistic.Status != null)
                                {
                                    lGCOrderViewModel.StatusCode = item.Logistic.Status.StatusCode;
                                }
                                if (item.Logistic.ServicePartner != null)
                                {
                                    lGCOrderViewModel.ServicePartnerName = item.Logistic.ServicePartner.ServicePartnerName;
                                }
                            }
                            if (item.ActualPickupDate != null)
                            {
                                var Date = (DateTime)item.ActualPickupDate;
                                lGCOrderViewModel.Date = Date.ToShortDateString();
                            }
                            lGCOrderList.Add(lGCOrderViewModel);
                        }
                    }
                }
                #endregion

                var data = lGCOrderList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region ADDED BY Priyanshi Sahu---Get All Load Done List (Optimized and Modified by VK Date : 21-June-2023)
        public IActionResult GetLoadDoneList(int? ServicePartnerId, DateTime? startDate, DateTime? endDate)
        {
            #region Variable Declaration
            List<TblOrderLgc> tblOrderLgc = null;
            List<PickupOrderViewModel> lGCOrderList = null;
            PickupOrderViewModel lGCOrderViewModel = null;
            string URL = _config.Value.URLPrefixforProd;
            #endregion

            try
            {
                #region Variable Declaration
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
                int count = 0;
                #endregion

                #region Date Filter and ServicePartner
                if (startDate != null && endDate != null)
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                }
                if (ServicePartnerId == 0)
                {
                    ServicePartnerId = null;
                }
                #endregion

                #region TblOrderLgc Implementation
                count = _context.TblOrderLgcs
                            .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner)
                            .Include(x => x.Evcpartner)
                            .Include(x => x.Logistic).ThenInclude(x => x.OrderTrans).ThenInclude(x => x.Status)
                            .Include(x => x.DriverDetails).Include(x => x.Evcregistration).ThenInclude(x => x.City)
                            .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner)
                            .Count(x => x.IsActive == true && x.Logistic != null && x.Logistic.IsActive == true && x.OrderTrans != null && x.OrderTrans.IsActive == true && x.DriverDetailsId != null
                            && x.Logistic.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup) && x.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup) && x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup)
                            && (ServicePartnerId == null || (x.Logistic.ServicePartner != null && x.Logistic.ServicePartnerId == ServicePartnerId))
                            && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                            && (string.IsNullOrEmpty(searchValue)
                            || x.Logistic.RegdNo.ToLower().Contains(searchValue.ToLower().Trim())
                            || x.Logistic.Status.StatusCode.ToLower().Contains(searchValue.ToLower().Trim())
                            || x.Logistic.ServicePartner.ServicePartnerDescription.ToLower().Contains(searchValue.ToLower().Trim())
                            || (x.Evcpartner != null && (x.Evcpartner.EvcStoreCode ?? "").ToLower().Contains(searchValue.ToLower().Trim()))
                            || (x.DriverDetails == null ? false :
                            (x.DriverDetails.DriverName.ToLower().Contains(searchValue.ToLower().Trim())
                            || x.DriverDetails.DriverPhoneNumber.ToLower().Contains(searchValue.ToLower().Trim())
                            || x.DriverDetails.VehicleNumber.ToLower().Contains(searchValue.ToLower().Trim())
                            || x.DriverDetails.City.ToLower().Contains(searchValue.ToLower().Trim())
                            ))
                            || (x.Evcregistration == null ? false :
                            (x.Evcregistration.BussinessName.ToLower().Contains(searchValue.ToLower().Trim())))
                            ));
                if (count > 0)
                {
                    tblOrderLgc = _context.TblOrderLgcs
                            .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner)
                            .Include(x => x.Evcpartner)
                            .Include(x => x.Logistic).ThenInclude(x => x.OrderTrans).ThenInclude(x => x.Status)
                            .Include(x => x.DriverDetails).Include(x => x.Evcregistration).ThenInclude(x => x.City)
                            .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner)
                            .Where(x => x.IsActive == true && x.Logistic != null && x.Logistic.IsActive == true && x.OrderTrans != null && x.OrderTrans.IsActive == true && x.DriverDetailsId != null
                            && x.Logistic.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup) && x.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup) && x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup)
                            && (ServicePartnerId == null || (x.Logistic.ServicePartner != null && x.Logistic.ServicePartnerId == ServicePartnerId))
                            && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                            && (string.IsNullOrEmpty(searchValue)
                            || x.Logistic.RegdNo.ToLower().Contains(searchValue.ToLower().Trim())
                            || x.Logistic.Status.StatusCode.ToLower().Contains(searchValue.ToLower().Trim())
                            || x.Logistic.ServicePartner.ServicePartnerDescription.ToLower().Contains(searchValue.ToLower().Trim())
                            || (x.Evcpartner != null && (x.Evcpartner.EvcStoreCode ?? "").ToLower().Contains(searchValue.ToLower().Trim()))
                            || (x.DriverDetails == null ? false :
                            (x.DriverDetails.DriverName.ToLower().Contains(searchValue.ToLower().Trim())
                            || x.DriverDetails.DriverPhoneNumber.ToLower().Contains(searchValue.ToLower().Trim())
                            || x.DriverDetails.VehicleNumber.ToLower().Contains(searchValue.ToLower().Trim())
                            || x.DriverDetails.City.ToLower().Contains(searchValue.ToLower().Trim())
                            ))
                            || (x.Evcregistration == null ? false :
                            (x.Evcregistration.BussinessName.ToLower().Contains(searchValue.ToLower().Trim())))
                            )).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.OrderLgcid).Skip(skip).Take(pageSize).ToList();
                }
                recordsTotal = count;
                #endregion

                #region Sorting
                if (tblOrderLgc != null)
                {
                    tblOrderLgc = sortColumnDirection.Equals(SortingOrder.ASCENDING)
? tblOrderLgc.OrderBy(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
.ToList()
: tblOrderLgc.OrderByDescending(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
.ToList();
                }
                #endregion

                #region TblOrderLgc to model Mapping
                lGCOrderList = new List<PickupOrderViewModel>();
                if (tblOrderLgc != null)
                {
                    foreach (var item in tblOrderLgc)
                    {
                        string actionURL = string.Empty;
                        var regdno = item.Logistic.RegdNo;
                        actionURL = " <ul class='actions'>";
                        actionURL = "<a href ='" + URL + "/LGC_Admin/OrderViewPage?OrderTransId=" + (item.OrderTransId) + "' ><button onclick='View(" + item.OrderTransId + ")' class='btn btn-sm btn-primary'><i class='fa-solid fa-eye'></i></button></a>" + "&nbsp;<button onclick='ReassignOrder(" + item.OrderTransId + ")' class='btn btn-primary btn-sm ml-1'>Change EVC</button>";
                        actionURL = actionURL + "</ul>";

                        if (item != null)
                        {
                            lGCOrderViewModel = new PickupOrderViewModel();
                            lGCOrderViewModel.Id = item.EvcregistrationId != null ? item.EvcregistrationId : 0;
                            lGCOrderViewModel.Action = actionURL;
                            lGCOrderViewModel.DriverName = item.DriverDetails == null ? item.DriverDetails.DriverName : String.Empty;
                            lGCOrderViewModel.DriverPhoneNumber = item.DriverDetails != null ? item.DriverDetails.DriverPhoneNumber : String.Empty;
                            lGCOrderViewModel.RegdNo = item.Logistic.RegdNo != null ? item.Logistic.RegdNo : String.Empty;
                            lGCOrderViewModel.DriverName = item.DriverDetails != null ? item.DriverDetails.DriverName : String.Empty;
                            lGCOrderViewModel.DriverPhoneNumber = item.DriverDetails != null ? item.DriverDetails.DriverPhoneNumber : String.Empty;
                            lGCOrderViewModel.VehicleNumber = item.DriverDetails != null ? item.DriverDetails.VehicleNumber : String.Empty;
                            lGCOrderViewModel.City = item.Evcregistration != null ? item.Evcregistration.City.Name : String.Empty;
                            // lGCOrderViewModel.EVCBusinessName = item.Evcregistration.BussinessName != null ? item.Evcregistration.BussinessName : String.Empty;
                            lGCOrderViewModel.EVCBusinessName = item.Evcregistration != null && item.Evcregistration.EvcregdNo != null && item.Evcregistration.BussinessName != null ? item.Evcregistration.EvcregdNo + "-" + item.Evcregistration.BussinessName : string.Empty;
                            lGCOrderViewModel.EvcStoreCode = item.Evcpartner?.EvcStoreCode;
                            if (item.ModifiedDate != null)
                            {
                                lGCOrderViewModel.ModifiedDate = item.ModifiedDate;
                            }
                            lGCOrderViewModel.ServicePartnerName = item.Logistic?.ServicePartner?.ServicePartnerName != null ? item.Logistic.ServicePartner.ServicePartnerName : null;
                            lGCOrderViewModel.StatusCode = item.OrderTrans.Status.StatusCode != null ? item.OrderTrans.Status.StatusCode : String.Empty;
                            if (item.ModifiedDate != null)
                            {
                                var Date = (DateTime)item.ModifiedDate;
                                lGCOrderViewModel.Date = Date.ToShortDateString();
                            }
                            lGCOrderList.Add(lGCOrderViewModel);
                        }
                    }
                }
                #endregion
                var data = lGCOrderList.Distinct().ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region ADDED BY Priyanshi Sahu---- Get All Pickup decline Record (Optimized and Modified by VK for Abb Redemption Date : 22-June-2023)
        public async Task<ActionResult> PickupDecline(int? ServicePartnerId, DateTime? startDate, DateTime? endDate)
        {
            #region Variable Declaration
            List<TblOrderLgc> tblOrderLgcs = null;
            List<TblLogistic> tblLogistic = null;
            TblServicePartner tblServicePartner = null;
            List<PickupOrderViewModel> lGCOrderList = null;
            PickupOrderViewModel lGCOrderViewModel = null;
            string URL = _config.Value.URLPrefixforProd;
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
                searchValue = searchValue.Trim();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                int count = 0;
                #endregion

                #region Date Filter and ServicePartner
                if (startDate != null && endDate != null)
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                }
                if (ServicePartnerId == 0)
                {
                    ServicePartnerId = null;
                }
                #endregion

                // tblServicePartner = _context.TblServicePartners.Where(x => x.UserId == userId && x.IsActive == true).FirstOrDefault();
                #region tblOrderLgc Implementaion
                count = _context.TblOrderLgcs
                     .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner)
                     .Include(x => x.Evcpartner)
                     .Include(x => x.Evcregistration).Include(x => x.Logistic).ThenInclude(x => x.Status)
                          .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                          .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                           //Changes for ABB Redemption by Vk 
                           .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                           .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                           .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                          //Changes for ABB Redemption by Vk             
                          .Count(x => x.IsActive == true && x.Logistic != null && x.Logistic.IsActive == false && x.OrderTrans.IsActive == true
                          && x.Logistic.StatusId == Convert.ToInt32(OrderStatusEnum.PickupDecline) && x.StatusId == Convert.ToInt32(OrderStatusEnum.PickupDecline) && x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.PickupDecline)
                          && (ServicePartnerId == null || (x.Logistic.ServicePartner != null && x.Logistic.ServicePartnerId == ServicePartnerId))
                          && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                          && (string.IsNullOrEmpty(searchValue)
                                || (x.Logistic.RegdNo ?? "").ToLower().Contains(searchValue)
                                || (x.Logistic.ServicePartner.ServicePartnerDescription ?? "").ToLower().Contains(searchValue)
                                || (x.Logistic.TicketNumber ?? "").ToLower().Contains(searchValue)
                                || (x.Logistic.Status.StatusCode ?? "").ToLower().Contains(searchValue)
                                || (x.Evcpartner != null && (x.Evcpartner.EvcStoreCode ?? "").ToLower().Contains(searchValue))
                                || (x.OrderTrans.Exchange == null ? false :
                                ((x.OrderTrans.Exchange.ProductType == null ? false : (x.OrderTrans.Exchange.ProductType.Description ?? "").ToLower().Contains(searchValue))
                                || (x.OrderTrans.Exchange.ProductType.ProductCat == null ? false : (x.OrderTrans.Exchange.ProductType.ProductCat.Description ?? "").ToLower().Contains(searchValue))
                                || (x.OrderTrans.Exchange.CustomerDetails == null ? false : (x.OrderTrans.Exchange.CustomerDetails.City ?? "").ToLower().Contains(searchValue))
                                || (x.OrderTrans.Exchange.CustomerDetails == null ? false : (x.OrderTrans.Exchange.CustomerDetails.PhoneNumber ?? "").ToLower().Contains(searchValue))
                                )) // For ABB Redumption
                                || ((x.OrderTrans.Abbredemption == null && x.OrderTrans.Abbredemption.Abbregistration == null) ? false :
                                 ((x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null ? (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description ?? "").ToLower().Contains(searchValue) : false)
                                 || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null ? (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description ?? "").ToLower().Contains(searchValue) : false)
                                 || (x.OrderTrans.Abbredemption.CustomerDetails == null ? false : (x.OrderTrans.Abbredemption.CustomerDetails.City ?? "").ToLower().Contains(searchValue))
                                 || (x.OrderTrans.Abbredemption.CustomerDetails == null ? false : (x.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber ?? "").ToLower().Contains(searchValue))
                                ))));
                if (count > 0)
                {
                    tblOrderLgcs = _context.TblOrderLgcs
                     .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner)
                     .Include(x => x.Evcpartner)
                     .Include(x => x.Evcregistration).Include(x => x.Logistic).ThenInclude(x => x.Status)
                          .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                          .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                           //Changes for ABB Redemption by Vk 
                           .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                           .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                           .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                          //Changes for ABB Redemption by Vk             
                          .Where(x => x.IsActive == true && x.Logistic != null && x.Logistic.IsActive == false && x.OrderTrans.IsActive == true
                          && x.Logistic.StatusId == Convert.ToInt32(OrderStatusEnum.PickupDecline) && x.StatusId == Convert.ToInt32(OrderStatusEnum.PickupDecline) && x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.PickupDecline)
                          && (ServicePartnerId == null || (x.Logistic.ServicePartner != null && x.Logistic.ServicePartnerId == ServicePartnerId))
                          && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                          && (string.IsNullOrEmpty(searchValue)
                                || (x.Logistic.RegdNo ?? "").ToLower().Contains(searchValue)
                                || (x.Logistic.ServicePartner.ServicePartnerDescription ?? "").ToLower().Contains(searchValue)
                                || (x.Logistic.TicketNumber ?? "").ToLower().Contains(searchValue)
                                || (x.Logistic.Status.StatusCode ?? "").ToLower().Contains(searchValue)
                                || (x.Evcpartner != null && (x.Evcpartner.EvcStoreCode ?? "").ToLower().Contains(searchValue))
                                || (x.OrderTrans.Exchange == null ? false :
                                ((x.OrderTrans.Exchange.ProductType == null ? false : (x.OrderTrans.Exchange.ProductType.Description ?? "").ToLower().Contains(searchValue))
                                || (x.OrderTrans.Exchange.ProductType.ProductCat == null ? false : (x.OrderTrans.Exchange.ProductType.ProductCat.Description ?? "").ToLower().Contains(searchValue))
                                || (x.OrderTrans.Exchange.CustomerDetails == null ? false : (x.OrderTrans.Exchange.CustomerDetails.City ?? "").ToLower().Contains(searchValue))
                                || (x.OrderTrans.Exchange.CustomerDetails == null ? false : (x.OrderTrans.Exchange.CustomerDetails.PhoneNumber ?? "").ToLower().Contains(searchValue))
                                )) // For ABB Redumption
                                || ((x.OrderTrans.Abbredemption == null && x.OrderTrans.Abbredemption.Abbregistration == null) ? false :
                                 ((x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null ? (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description ?? "").ToLower().Contains(searchValue) : false)
                                 || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null ? (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description ?? "").ToLower().Contains(searchValue) : false)
                                 || (x.OrderTrans.Abbredemption.CustomerDetails == null ? false : (x.OrderTrans.Abbredemption.CustomerDetails.City ?? "").ToLower().Contains(searchValue))
                                 || (x.OrderTrans.Abbredemption.CustomerDetails == null ? false : (x.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber ?? "").ToLower().Contains(searchValue))
                                )))).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.OrderLgcid).Skip(skip).Take(pageSize).ToList();
                }
                recordsTotal = count;
                #endregion

                #region Sorting
                if (tblOrderLgcs != null)
                {
                    tblOrderLgcs = sortColumnDirection.Equals(SortingOrder.ASCENDING)
? tblOrderLgcs.OrderBy(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
.ToList()
: tblOrderLgcs.OrderByDescending(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
.ToList();
                }
                #endregion

                #region tblOrderLgc to model Mapping
                lGCOrderList = new List<PickupOrderViewModel>();
                if (tblOrderLgcs != null && tblOrderLgcs.Count > 0)
                {
                    foreach (var item in tblOrderLgcs)
                    {
                        string actionURL = string.Empty;
                        actionURL = " <ul class='actions'>";
                        actionURL = "<a href ='" + URL + "/LGC_Admin/OrderViewPage?OrderTransId=" + (item.OrderTransId) + "' ><button onclick='View(" + item.OrderTransId + ")' class='btn btn-primary btn'>View</button></a> &nbsp;";
                        actionURL = actionURL + " <button onclick = 'ReOpenTicket(" + item.OrderTransId + ")' class='btn btn-sm btn-primary'>Re-Open</button> &nbsp;";
                        actionURL = actionURL + "</ul>";

                        string productTypeDesc = null; string productCatDesc = null; string custCity = null;
                        if (item != null)
                        {
                            #region Exchange and ABB common Configuraion Add by VK
                            if (item.OrderTrans.Exchange != null)
                            {
                                if (item.OrderTrans.Exchange.ProductType != null)
                                {
                                    productTypeDesc = item.OrderTrans.Exchange.ProductType.Description;
                                    if (item.OrderTrans.Exchange.ProductType.ProductCat != null)
                                    {
                                        productCatDesc = item.OrderTrans.Exchange.ProductType.ProductCat.Description;
                                    }
                                }
                                if (item.OrderTrans.Exchange.CustomerDetails != null)
                                {
                                    custCity = item.OrderTrans.Exchange.CustomerDetails.City;
                                }
                            }
                            else if (item.OrderTrans.Abbredemption != null && item.OrderTrans.Abbredemption.Abbregistration != null)
                            {
                                if (item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null)
                                {
                                    productTypeDesc = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description;
                                }
                                if (item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                                {
                                    productCatDesc = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description;
                                }
                                if (item.OrderTrans.Abbredemption.CustomerDetails != null)
                                {
                                    custCity = item.OrderTrans.Abbredemption.CustomerDetails.City;
                                }
                            }
                            #endregion

                            lGCOrderViewModel = new PickupOrderViewModel();
                            lGCOrderViewModel.Action = actionURL;
                            lGCOrderViewModel.Id = item.LogisticId != null ? item.LogisticId : 0;
                            lGCOrderViewModel.ProductCategory = productCatDesc;
                            lGCOrderViewModel.ProductType = productTypeDesc;
                            lGCOrderViewModel.City = custCity;
                            lGCOrderViewModel.EVCBusinessName = item.Evcregistration != null && item.Evcregistration.EvcregdNo != null && item.Evcregistration.BussinessName != null ? item.Evcregistration.EvcregdNo + "-" + item.Evcregistration.BussinessName : string.Empty;
                            lGCOrderViewModel.EvcStoreCode = item.Evcpartner?.EvcStoreCode;
                            if (item.Logistic != null)
                            {
                                lGCOrderViewModel.RegdNo = item.Logistic.RegdNo;
                                lGCOrderViewModel.TicketNumber = item.Logistic.TicketNumber;
                                lGCOrderViewModel.AmountPayableThroughLGC = Convert.ToDecimal(item.Logistic.AmtPaybleThroughLgc);
                                if (item.Logistic.Status != null)
                                {
                                    lGCOrderViewModel.StatusCode = item.Logistic.Status.StatusCode;
                                }
                                if (item.Logistic.ServicePartner != null)
                                {
                                    lGCOrderViewModel.ServicePartnerName = item.Logistic.ServicePartner.ServicePartnerName;
                                }
                            }
                            if (item.CreatedDate != null)
                            {
                                var Date = (DateTime)item.CreatedDate != null ? (DateTime)item.CreatedDate : DateTime.MinValue;
                                lGCOrderViewModel.Date = Date.ToShortDateString();
                            }
                            lGCOrderList.Add(lGCOrderViewModel);
                        }
                    }
                }
                #endregion

                var data = lGCOrderList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region ADDED BY Priyanshi Sahu---Get All Drop List (Optimized and Modified by VK Date : 22-June-2023)
        public IActionResult DropList(int? ServicePartnerId, DateTime? startDate, DateTime? endDate)
        {
            #region Variable Declaration
            List<TblOrderLgc> tblOrderLgc = null;
            List<PickupOrderViewModel> lGCOrderList = null;
            PickupOrderViewModel lGCOrderViewModel = null;
            string URL = _config.Value.URLPrefixforProd;
            #endregion

            try
            {
                #region Datatable Variable Declaration
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
                int count = 0;
                #endregion

                #region Date Filter and ServicePartner
                if (startDate != null && endDate != null)
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                }
                if (ServicePartnerId == 0)
                {
                    ServicePartnerId = null;
                }
                #endregion

                #region TblOrderLgcs Implementation
                count = _context.TblOrderLgcs
                    .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner)
                    .Include(x => x.Evcregistration).Include(x => x.Evcpartner)
                    .Include(x => x.Logistic).ThenInclude(x => x.OrderTrans).ThenInclude(x => x.Status)
                    .Include(x => x.DriverDetails).Include(x => x.Evcregistration).ThenInclude(x => x.City)
                    .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner)
                    .Count(x => x.IsActive == true && x.OrderTrans != null && x.OrderTrans.IsActive == true && x.Logistic != null && x.Logistic.IsActive == true && x.DriverDetails != null && x.Evcregistration != null
                    && x.Logistic.StatusId == Convert.ToInt32(OrderStatusEnum.Posted) && x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.Posted) && x.StatusId == Convert.ToInt32(OrderStatusEnum.Posted)
                    && (ServicePartnerId == null || (x.Logistic.ServicePartner != null && x.Logistic.ServicePartnerId == ServicePartnerId))
                    && ((startDate == null && endDate == null) || (x.ActualDropDate >= startDate && x.ActualDropDate <= endDate))
                    && (string.IsNullOrEmpty(searchValue)
                    || x.Logistic.RegdNo.ToLower().Contains(searchValue.ToLower().Trim())
                    || x.Logistic.Status.StatusCode.ToLower().Contains(searchValue.ToLower().Trim())
                    || (x.Evcpartner != null && (x.Evcpartner.EvcStoreCode ?? "").ToLower().Contains(searchValue.ToLower().Trim()))
                    || (x.DriverDetails == null ? false :
                    (x.DriverDetails.DriverName.ToLower().Contains(searchValue.ToLower().Trim())
                    || x.Logistic.ServicePartner.ServicePartnerDescription.ToLower().Contains(searchValue.ToLower().Trim())
                    || x.DriverDetails.DriverPhoneNumber.ToLower().Contains(searchValue.ToLower().Trim())
                    || x.DriverDetails.VehicleNumber.ToLower().Contains(searchValue.ToLower().Trim())
                    || x.DriverDetails.City.ToLower().Contains(searchValue.ToLower().Trim())
                    ))
                    || (x.Evcregistration == null ? false :
                    (x.Evcregistration.BussinessName.ToLower().Contains(searchValue.ToLower().Trim())))
                    ));
                if (count > 0)
                {
                    tblOrderLgc = _context.TblOrderLgcs
                    .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner)
                    .Include(x => x.Evcregistration).Include(x => x.Evcpartner)
                    .Include(x => x.Logistic).ThenInclude(x => x.OrderTrans).ThenInclude(x => x.Status)
                    .Include(x => x.DriverDetails).Include(x => x.Evcregistration).ThenInclude(x => x.City)
                    .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner)
                    .Where(x => x.IsActive == true && x.OrderTrans != null && x.OrderTrans.IsActive == true && x.Logistic != null && x.Logistic.IsActive == true && x.DriverDetails != null && x.Evcregistration != null
                    && x.Logistic.StatusId == Convert.ToInt32(OrderStatusEnum.Posted) && x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.Posted) && x.StatusId == Convert.ToInt32(OrderStatusEnum.Posted)
                    && (ServicePartnerId == null || (x.Logistic.ServicePartner != null && x.Logistic.ServicePartnerId == ServicePartnerId))
                    && ((startDate == null && endDate == null) || (x.ActualDropDate >= startDate && x.ActualDropDate <= endDate))
                    && (string.IsNullOrEmpty(searchValue)
                    || x.Logistic.RegdNo.ToLower().Contains(searchValue.ToLower().Trim())
                    || x.Logistic.Status.StatusCode.ToLower().Contains(searchValue.ToLower().Trim())
                    || (x.Evcpartner != null && (x.Evcpartner.EvcStoreCode ?? "").ToLower().Contains(searchValue.ToLower().Trim()))
                    || (x.DriverDetails == null ? false :
                    (x.DriverDetails.DriverName.ToLower().Contains(searchValue.ToLower().Trim())
                    || x.Logistic.ServicePartner.ServicePartnerDescription.ToLower().Contains(searchValue.ToLower().Trim())
                    || x.DriverDetails.DriverPhoneNumber.ToLower().Contains(searchValue.ToLower().Trim())
                    || x.DriverDetails.VehicleNumber.ToLower().Contains(searchValue.ToLower().Trim())
                    || x.DriverDetails.City.ToLower().Contains(searchValue.ToLower().Trim())
                    ))
                    || (x.Evcregistration == null ? false :
                    (x.Evcregistration.BussinessName.ToLower().Contains(searchValue.ToLower().Trim())))
                    )).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.OrderLgcid).Skip(skip).Take(pageSize).ToList();
                }
                recordsTotal = count;
                #endregion

                #region Sorting
                if (tblOrderLgc != null)
                {
                    tblOrderLgc = sortColumnDirection.Equals(SortingOrder.ASCENDING)
? tblOrderLgc.OrderBy(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
.ToList()
: tblOrderLgc.OrderByDescending(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
.ToList();
                }
                #endregion

                #region tblOrderLgc to Model mapping
                lGCOrderList = new List<PickupOrderViewModel>();
                if (tblOrderLgc != null && tblOrderLgc.Count > 0)
                {
                    foreach (var item in tblOrderLgc)
                    {
                        string actionURL = string.Empty;
                        var regdno = item.Logistic.RegdNo;
                        actionURL = " <ul class='actions'>";
                        actionURL = "<a href ='" + URL + "/LGC_Admin/OrderViewPage?OrderTransId=" + (item.OrderTransId) + "' ><button onclick='View(" + item.OrderTransId + ")' class='btn btn-primary btn'>View</button></a>";
                        actionURL = actionURL + "</ul>";

                        if (item != null)
                        {
                            lGCOrderViewModel = new PickupOrderViewModel();
                            lGCOrderViewModel.Id = item.EvcregistrationId != null ? item.EvcregistrationId : 0;
                            lGCOrderViewModel.Action = actionURL;
                            lGCOrderViewModel.DriverName = item.DriverDetails == null ? item.DriverDetails.DriverName : String.Empty;
                            lGCOrderViewModel.DriverPhoneNumber = item.DriverDetails != null ? item.DriverDetails.DriverPhoneNumber : String.Empty;
                            lGCOrderViewModel.RegdNo = item.Logistic.RegdNo != null ? item.Logistic.RegdNo : String.Empty;
                            lGCOrderViewModel.DriverName = item.DriverDetails != null ? item.DriverDetails.DriverName : String.Empty;
                            lGCOrderViewModel.DriverPhoneNumber = item.DriverDetails != null ? item.DriverDetails.DriverPhoneNumber : String.Empty;
                            lGCOrderViewModel.VehicleNumber = item.DriverDetails != null ? item.DriverDetails.VehicleNumber : String.Empty;
                            lGCOrderViewModel.City = item.Evcregistration != null ? item.Evcregistration.City.Name : String.Empty;
                            lGCOrderViewModel.EVCBusinessName = item.Evcregistration != null && item.Evcregistration.EvcregdNo != null && item.Evcregistration.BussinessName != null ? item.Evcregistration.EvcregdNo + "-" + item.Evcregistration.BussinessName : string.Empty;
                            lGCOrderViewModel.EvcStoreCode = item.Evcpartner?.EvcStoreCode;
                            // lGCOrderViewModel.EVCBusinessName = item.Evcregistration.BussinessName != null ? item.Evcregistration.BussinessName : String.Empty;
                            if (item.ModifiedDate != null)
                            {
                                lGCOrderViewModel.ModifiedDate = item.ModifiedDate;
                            }
                            lGCOrderViewModel.ServicePartnerName = item.Logistic.ServicePartner.ServicePartnerName != null ? item.Logistic.ServicePartner.ServicePartnerName : null;
                            lGCOrderViewModel.StatusCode = item.OrderTrans.Status.StatusCode != null ? item.OrderTrans.Status.StatusCode : String.Empty;
                            if (item.ActualDropDate != null)
                            {
                                var Date = (DateTime)item.ActualDropDate;
                                lGCOrderViewModel.Date = Date.ToShortDateString();
                            }
                            lGCOrderList.Add(lGCOrderViewModel);
                        }
                    }
                }
                #endregion

                lGCOrderList = lGCOrderList.Distinct().ToList();
                //lGCOrderList = lGCOrderList.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();
                var data = lGCOrderList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region  ADDED BY Priyanshi Sahu---Get All Not Approverd ServicePartner List (Optimized and Modified by VK Date : 24-June-2023)
        [HttpPost]
        public async Task<ActionResult> GetServicePartner_NotApprovedList(DateTime? startDate, DateTime? endDate)
        {
            #region Variable Declaration
            string URL = _config.Value.URLPrefixforProd;
            List<TblServicePartner> TblServicePartners = null;
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
                int count = 0;
                // _context = new Digi2l_DevContext();
                #endregion

                #region Date Filter and ServicePartner
                if (startDate != null && endDate != null)
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region tblServicePartner Implementation
                count = _context.TblServicePartners
                                        .Include(x => x.ServicePartnerCity)
                                        .Count(x => x.IsActive == true
                                            && x.ServicePartnerIsApprovrd == false
                                            && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                                            && (string.IsNullOrEmpty(searchValue)
                                                || x.ServicePartnerRegdNo.ToLower().Contains(searchValue.ToLower().Trim())
                                                || (x.ServicePartnerName != null ? x.ServicePartnerName.ToLower().Contains(searchValue.ToLower().Trim()) : false)
                                                || (x.ServicePartnerCity.Name != null ? x.ServicePartnerCity.Name.ToLower().Contains(searchValue.ToLower().Trim()) : false)
                                                || (x.ServicePartnerDescription != null ? x.ServicePartnerDescription.ToLower().Contains(searchValue.ToLower().Trim()) : false)
                                                ));
                if (count > 0)
                {
                    TblServicePartners = await _context.TblServicePartners
                                        .Include(x => x.ServicePartnerCity)
                                        .Where(x => x.IsActive == true
                                            && x.ServicePartnerIsApprovrd == false
                                            && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                                            && (string.IsNullOrEmpty(searchValue)
                                                || x.ServicePartnerRegdNo.ToLower().Contains(searchValue.ToLower().Trim())
                                                || (x.ServicePartnerName != null ? x.ServicePartnerName.ToLower().Contains(searchValue.ToLower().Trim()) : false)
                                                || (x.ServicePartnerCity.Name != null ? x.ServicePartnerCity.Name.ToLower().Contains(searchValue.ToLower().Trim()) : false)
                                                || (x.ServicePartnerDescription != null ? x.ServicePartnerDescription.ToLower().Contains(searchValue.ToLower().Trim()) : false)
                                                )).OrderByDescending(x => x.CreatedDate).ThenByDescending(x => x.ServicePartnerId).Skip(skip).Take(pageSize).ToListAsync();
                }
                recordsTotal = count;
                #endregion

                #region Sorting
                if (TblServicePartners != null)
                {
                    if (TblServicePartners != null && pageSize == -1)
                    {
                        TblServicePartners = sortColumnDirection.Equals(SortingOrder.ASCENDING)
? TblServicePartners.OrderBy(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
.ToList()
: TblServicePartners.OrderByDescending(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
.ToList();
                    }
                    else
                    {
                        TblServicePartners = sortColumnDirection.Equals(SortingOrder.ASCENDING)
? TblServicePartners.OrderBy(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
.ToList()
: TblServicePartners.OrderByDescending(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
.ToList();
                    }
                }
                else
                    TblServicePartners = new List<TblServicePartner>();
                #endregion

                #region TblServicePartners to Model mapping
                List<LGC_NotapprovedViewModel> LGC_NotApporvedViewList = _mapper.Map<List<TblServicePartner>, List<LGC_NotapprovedViewModel>>(TblServicePartners);
                string actionURL = string.Empty;

                foreach (LGC_NotapprovedViewModel item in LGC_NotApporvedViewList)
                {
                    actionURL = " <div class='actionbtns'>";
                    actionURL = actionURL + "<button onclick='ApprovedServicePartner(" + item.ServicePartnerId + ")' class='btn btn-sm btn-primary'>Approve</button>";

                    //actionURL = actionURL + "<a class='mx-1 fas fa-edit' href='" + URL + "/EVC/EVC_Registration?id=" + _protector.Encode(item.EvcregistrationId) + "&AFlag=2' title='Edit' ></a>" +
                    //    "<a  href='javascript: void(0)' onclick='deleteConfirm( " + item.EvcregistrationId + " )' class='fas fa-trash' data-toggle='tooltip' data-placement='top' title='Delete''></a>";
                    //actionURL = actionURL + "</div>";

                    actionURL = actionURL + "<a  href='javascript: void(0)' onclick='deleteConfirm( " + item.ServicePartnerId + " )' class='fas fa-trash' data-toggle='tooltip' data-placement='top' title='Delete''></a>";
                    actionURL = actionURL + "</div>";
                    item.Action = actionURL;


                    //Take Data is More Tbl 
                    TblServicePartner ServicePartnerDetails = TblServicePartners.FirstOrDefault(x => x.ServicePartnerId == item.ServicePartnerId);
                    if (ServicePartnerDetails != null)
                    {
                        if (ServicePartnerDetails.ServicePartnerCity != null)
                        {
                            item.CityName = ServicePartnerDetails.ServicePartnerCity.Name != null ? ServicePartnerDetails.ServicePartnerCity.Name : String.Empty;
                        }

                        if (ServicePartnerDetails.CreatedDate != null && ServicePartnerDetails.CreatedDate != null)
                        {
                            var Date = (DateTime)ServicePartnerDetails.CreatedDate;
                            item.Date = Date.ToShortDateString();

                        }
                    }
                }
                #endregion

                var data = LGC_NotApporvedViewList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region ADDED BY Priyanshi Sahu---Get All Approverd ServicePartner List (Optimized and Modified by VK Date : 24-June-2023)
        [HttpPost]
        public async Task<ActionResult> GetServicePartner_ApprovedList(DateTime? startDate, DateTime? endDate)
        {
            #region Variable Declaration
            string URL = _config.Value.URLPrefixforProd;
            List<TblServicePartner> TblServicePartners = null;
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
                searchValue = searchValue.ToLower().Trim();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                int count = 0;
                // _context = new Digi2l_DevContext();
                #endregion

                #region Date Filter and ServicePartner
                if (startDate != null && endDate != null)
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region  TblServicePartners Implementation
                count = _context.TblServicePartners
                .Include(x => x.ServicePartnerCity)
                .Count(x => x.IsActive == true
                    && x.ServicePartnerIsApprovrd == true
                    && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                    && (string.IsNullOrEmpty(searchValue)
                        || (x.ServicePartnerRegdNo ?? "").ToLower().Contains(searchValue)
                        || (x.ServicePartnerName != null ? (x.ServicePartnerName ?? "").ToLower().Contains(searchValue) : false)
                         || (x.ServicePartnerCity.Name != null ? (x.ServicePartnerCity.Name ?? "").ToLower().Contains(searchValue) : false)
                        || (x.ServicePartnerDescription != null ? (x.ServicePartnerDescription ?? "").ToLower().Contains(searchValue) : false)
                        ));
                if (count > 0)
                {
                    TblServicePartners = await _context.TblServicePartners
                .Include(x => x.ServicePartnerCity)
                .Where(x => x.IsActive == true
                    && x.ServicePartnerIsApprovrd == true
                    && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                    && (string.IsNullOrEmpty(searchValue)
                        || (x.ServicePartnerRegdNo ?? "").ToLower().Contains(searchValue)
                        || (x.ServicePartnerName != null ? (x.ServicePartnerName ?? "").ToLower().Contains(searchValue) : false)
                         || (x.ServicePartnerCity.Name != null ? (x.ServicePartnerCity.Name ?? "").ToLower().Contains(searchValue) : false)
                        || (x.ServicePartnerDescription != null ? (x.ServicePartnerDescription ?? "").ToLower().Contains(searchValue) : false)
                        )).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.ServicePartnerId).Skip(skip).Take(pageSize).ToListAsync();
                }
                recordsTotal = count;
                #endregion

                #region Sorting
                if (TblServicePartners != null)
                {
                    if (TblServicePartners != null && pageSize == -1)
                    {
                        TblServicePartners = sortColumnDirection.Equals(SortingOrder.ASCENDING)
? TblServicePartners.OrderBy(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
.ToList()
: TblServicePartners.OrderByDescending(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
.ToList();
                    }
                    else
                    {
                        TblServicePartners = sortColumnDirection.Equals(SortingOrder.ASCENDING)
? TblServicePartners.OrderBy(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
.ToList()
: TblServicePartners.OrderByDescending(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
.ToList();
                    }
                }
                else
                    TblServicePartners = new List<TblServicePartner>();
                #endregion

                #region tblServicePartner to Model mapping
                List<LGC_ApprovedViewModel> LGC_ApporvedViewList = _mapper.Map<List<TblServicePartner>, List<LGC_ApprovedViewModel>>(TblServicePartners);
                string actionURL = string.Empty;

                foreach (LGC_ApprovedViewModel item in LGC_ApporvedViewList)
                {
                    actionURL = " <div class='actionbtns'>";
                    //actionURL = actionURL + "<button onclick='ApprovedServicePartner(" + item.ServicePartnerId + ")' class='btn btn-sm btn-primary'>Approve</button>";

                    //actionURL = actionURL + "<a class='mx-1 fas fa-edit' href='" + URL + "/EVC/EVC_Registration?id=" + _protector.Encode(item.EvcregistrationId) + "&AFlag=2' title='Edit' ></a>" +
                    //    "<a  href='javascript: void(0)' onclick='deleteConfirm( " + item.EvcregistrationId + " )' class='fas fa-trash' data-toggle='tooltip' data-placement='top' title='Delete''></a>";
                    //actionURL = actionURL + "</div>";

                    
                    actionURL = actionURL + "<a href ='" + URL + "/LGCOrderTracking/LGCVehicleList?ServicePartnerId=" + item.ServicePartnerId + "' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                    actionURL = actionURL + "<a  href='javascript: void(0)' onclick='deleteConfirm( " + item.ServicePartnerId + " )' class='fas fa-trash' data-toggle='tooltip' data-placement='top' title='Delete''></a>";
                    actionURL = actionURL + "</div>";
                    item.Action = actionURL;

                    //Take Data is More Tbl 
                    TblServicePartner ServicePartnerDetails = TblServicePartners.FirstOrDefault(x => x.ServicePartnerId == item.ServicePartnerId);
                    if (ServicePartnerDetails != null)
                    {
                        if (ServicePartnerDetails.ServicePartnerCity != null)
                        {
                            item.CityName = ServicePartnerDetails.ServicePartnerCity.Name != null ? ServicePartnerDetails.ServicePartnerCity.Name : String.Empty;
                        }

                        if (ServicePartnerDetails.CreatedDate != null && ServicePartnerDetails.CreatedDate != null)
                        {
                            var Date = (DateTime)ServicePartnerDetails.CreatedDate;
                            item.Date = Date.ToShortDateString();

                        }
                    }

                }
                #endregion

                var data = LGC_ApporvedViewList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion

        #region ADDED BY Priyanshi Sahu---- Get All Status List for LGC (Optimized and Modified by VK for ABB Redemption Date : 23-June-2023)
        public IActionResult CommanStatusListforLGC(int? ServicePartnerId, DateTime? startDate, DateTime? endDate, int? statusId)
        {
            #region Variable Declaration
            List<TblLogistic> tblLogistic = null;
            List<PickupOrderViewModel> lGCOrderList = null;
            PickupOrderViewModel lGCOrderViewModel = null;
            string URL = _config.Value.URLPrefixforProd;
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
                searchValue = searchValue.ToLower().Trim();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                int count = 0;
                #endregion

                #region Date Filter and ServicePartner
                if (startDate != null && endDate != null)
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                }
                if (ServicePartnerId == 0)
                {
                    ServicePartnerId = null;
                }
                #endregion

                #region TblLogistics Implementation
                count = _context.TblLogistics
                                    .Include(x => x.TblOrderLgcs).ThenInclude(x => x.Evcregistration)
                             .Include(x => x.ServicePartner)
                             .Include(x => x.Status)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                                        //Changes for ABB Redemption by Vk 
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                                        //Changes for ABB Redemption by Vk
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.TblWalletTransactions).ThenInclude(x => x.Evcregistration)
                                        .Count(x => x.IsActive == true
                                        && (statusId > 0 ? (x.StatusId == 0 ? false : (x.StatusId == statusId)) : (x.StatusId == Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated) || x.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup) || x.StatusId == Convert.ToInt32(OrderStatusEnum.Posted)))
                                        && (ServicePartnerId == null || (x.ServicePartner != null && x.ServicePartnerId == ServicePartnerId))
                                        && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                                        && (string.IsNullOrEmpty(searchValue)
                                        || (x.RegdNo ?? "").ToLower().Contains(searchValue)
                                        || (x.TicketNumber ?? "").ToLower().Contains(searchValue)
                                        || (x.ServicePartner.ServicePartnerDescription ?? "").ToLower().Contains(searchValue)
                                        || (x.Status.StatusCode ?? "").ToLower().Contains(searchValue)
                                        || (x.OrderTrans != null
                                        && ((x.OrderTrans.Exchange == null ? false :
                                        ((x.OrderTrans.Exchange.ProductType == null ? false : (x.OrderTrans.Exchange.ProductType.Description ?? "").ToLower().Contains(searchValue))
                                        || (x.OrderTrans.Exchange.ProductType.ProductCat == null ? false : (x.OrderTrans.Exchange.ProductType.ProductCat.Description ?? "").ToLower().Contains(searchValue))
                                        || (x.OrderTrans.Exchange.CustomerDetails == null ? false : (x.OrderTrans.Exchange.CustomerDetails.City ?? "").ToLower().Contains(searchValue))
                                        || (x.OrderTrans.Exchange.CustomerDetails == null ? false : (x.OrderTrans.Exchange.CustomerDetails.PhoneNumber ?? "").ToLower().Contains(searchValue))
                                        )) // For ABB Redumption
                                        || ((x.OrderTrans.Abbredemption == null && x.OrderTrans.Abbredemption.Abbregistration == null) ? false :
                                         ((x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null ? (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description ?? "").ToLower().Contains(searchValue) : false)
                                         || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null ? (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description ?? "").ToLower().Contains(searchValue) : false)
                                         || (x.OrderTrans.Abbredemption.CustomerDetails == null ? false : (x.OrderTrans.Abbredemption.CustomerDetails.City ?? "").ToLower().Contains(searchValue))
                                         || (x.OrderTrans.Abbredemption.CustomerDetails == null ? false : (x.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber ?? "").ToLower().Contains(searchValue))
                                        )))
                                        )));
                if (count > 0)
                {
                    tblLogistic = _context.TblLogistics
                            .Include(x => x.TblOrderLgcs).ThenInclude(x => x.Evcregistration)
                             .Include(x => x.ServicePartner)
                             .Include(x => x.Status)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                                        //Changes for ABB Redemption by Vk 
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                                        //Changes for ABB Redemption by Vk
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.TblWalletTransactions).ThenInclude(x => x.Evcregistration)
                                        .Where(x => x.IsActive == true
                                        && (statusId > 0 ? (x.StatusId == 0 ? false : (x.StatusId == statusId)) : (x.StatusId == Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated) || x.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup) || x.StatusId == Convert.ToInt32(OrderStatusEnum.Posted)))
                                        && (ServicePartnerId == null || (x.ServicePartner != null && x.ServicePartnerId == ServicePartnerId))
                                        && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                                        && (string.IsNullOrEmpty(searchValue)
                                        || (x.RegdNo ?? "").ToLower().Contains(searchValue)
                                        || (x.TicketNumber ?? "").ToLower().Contains(searchValue)
                                        || (x.ServicePartner.ServicePartnerDescription ?? "").ToLower().Contains(searchValue)
                                        || (x.Status.StatusCode ?? "").ToLower().Contains(searchValue)
                                        || (x.OrderTrans != null
                                        && ((x.OrderTrans.Exchange == null ? false :
                                        ((x.OrderTrans.Exchange.ProductType == null ? false : (x.OrderTrans.Exchange.ProductType.Description ?? "").ToLower().Contains(searchValue))
                                        || (x.OrderTrans.Exchange.ProductType.ProductCat == null ? false : (x.OrderTrans.Exchange.ProductType.ProductCat.Description ?? "").ToLower().Contains(searchValue))
                                        || (x.OrderTrans.Exchange.CustomerDetails == null ? false : (x.OrderTrans.Exchange.CustomerDetails.City ?? "").ToLower().Contains(searchValue))
                                        || (x.OrderTrans.Exchange.CustomerDetails == null ? false : (x.OrderTrans.Exchange.CustomerDetails.PhoneNumber ?? "").ToLower().Contains(searchValue))
                                        )) // For ABB Redumption
                                        || ((x.OrderTrans.Abbredemption == null && x.OrderTrans.Abbredemption.Abbregistration == null) ? false :
                                         ((x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null ? (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description ?? "").ToLower().Contains(searchValue) : false)
                                         || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null ? (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description ?? "").ToLower().Contains(searchValue) : false)
                                         || (x.OrderTrans.Abbredemption.CustomerDetails == null ? false : (x.OrderTrans.Abbredemption.CustomerDetails.City ?? "").ToLower().Contains(searchValue))
                                         || (x.OrderTrans.Abbredemption.CustomerDetails == null ? false : (x.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber ?? "").ToLower().Contains(searchValue))
                                        )))
                                       // ))).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.LogisticId).Skip(skip).Take(pageSize).ToList();
                                       ))).OrderByDescending(x => ((x.ModifiedDate == null || x.ModifiedDate != null) ? x.CreatedDate : x.ModifiedDate)).Skip(skip).Take(pageSize).ToList();
                }
                recordsTotal = count;
                #endregion

                #region Sorting
                if (tblLogistic != null)
                {
                    tblLogistic = sortColumnDirection.Equals(SortingOrder.ASCENDING)
? tblLogistic.OrderBy(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
.ToList()
: tblLogistic.OrderByDescending(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
.ToList();
                }
                #endregion

                #region tblLogistic to Model mapping
                lGCOrderList = new List<PickupOrderViewModel>();
                if (tblLogistic != null && tblLogistic.Count > 0)
                {
                    foreach (var item in tblLogistic)
                    {
                        string actionURL = string.Empty;
                        actionURL = " <ul class='actions'>";
                        actionURL = "<a href ='" + URL + "/EVC_Portal/OrderdetailsViewPageForEVC_PortAL?OrderTransId=" + (item.OrderTransId) + "' ><button onclick='View(" + item.OrderTransId + ")' class='btn btn-primary btn'>View</button></a>";
                        actionURL = actionURL + "</ul>";
                        string productTypeDesc = null; string productCatDesc = null; string custCity = null;
                        if (item != null)
                        {
                            #region Exchange and ABB common Configuraion Add by VK
                            if (item.OrderTrans.Exchange != null)
                            {
                                if (item.OrderTrans.Exchange.ProductType != null)
                                {
                                    productTypeDesc = item.OrderTrans.Exchange.ProductType.Description;
                                    if (item.OrderTrans.Exchange.ProductType.ProductCat != null)
                                    {
                                        productCatDesc = item.OrderTrans.Exchange.ProductType.ProductCat.Description;
                                    }
                                }
                                if (item.OrderTrans.Exchange.CustomerDetails != null)
                                {
                                    custCity = item.OrderTrans.Exchange.CustomerDetails.City;
                                }
                            }
                            else if (item.OrderTrans.Abbredemption != null && item.OrderTrans.Abbredemption.Abbregistration != null)
                            {
                                if (item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null)
                                {
                                    productTypeDesc = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description;
                                }
                                if (item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                                {
                                    productCatDesc = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description;
                                }
                                if (item.OrderTrans.Abbredemption.CustomerDetails != null)
                                {
                                    custCity = item.OrderTrans.Abbredemption.CustomerDetails.City;
                                }
                            }
                            #endregion

                            lGCOrderViewModel = new PickupOrderViewModel();
                            lGCOrderViewModel.Action = actionURL;
                            lGCOrderViewModel.Id = item.LogisticId != null ? item.LogisticId : 0;
                            lGCOrderViewModel.RegdNo = item.RegdNo != null ? item.RegdNo : String.Empty;
                            lGCOrderViewModel.TicketNumber = item.TicketNumber != null ? item.TicketNumber : String.Empty;
                            lGCOrderViewModel.AmountPayableThroughLGC = Convert.ToDecimal(item.AmtPaybleThroughLgc);

                            lGCOrderViewModel.ProductCategory = productCatDesc;
                            lGCOrderViewModel.ProductType = productTypeDesc;
                            lGCOrderViewModel.City = custCity;
                            if (item.ServicePartner != null)
                            {
                                lGCOrderViewModel.ServicePartnerName = item.ServicePartner.ServicePartnerName != null ? item.ServicePartner.ServicePartnerName : String.Empty;
                            }
                            lGCOrderViewModel.StatusCode = item.Status.StatusCode != null ? item.Status.StatusCode : String.Empty;
                            if (item.OrderTrans != null && item.OrderTrans.TblWalletTransactions != null && item.OrderTrans.TblWalletTransactions.Count > 0)
                            {
                                var Xyz = item.OrderTrans.TblWalletTransactions.FirstOrDefault(x => (x.OrderTransId ?? 0) == item.OrderTransId && x.EvcregistrationId != null).Evcregistration.EvcregdNo;
                                if (Xyz != null)
                                {
                                    lGCOrderViewModel.EvcName = Xyz;
                                }
                            }
                            if (item.CreatedDate != null)
                            {
                                var Date = (DateTime)item.CreatedDate;
                                lGCOrderViewModel.Date = Date.ToShortDateString();
                            }
                            if (item.Status.StatusCode != null)
                            {
                                if (item.StatusId == 18)
                                {
                                    lGCOrderViewModel.OrderState = "Ready for Pickup";
                                }
                                else if (item.StatusId == 23 && item.DriverDetailsId == null)
                                {
                                    lGCOrderViewModel.OrderState = "Pickup Done";

                                }
                                else if (item.StatusId == 23 && item.DriverDetailsId != null)
                                {
                                    lGCOrderViewModel.OrderState = "Load Complete";

                                }
                                else if (item.StatusId == 30)
                                {
                                    lGCOrderViewModel.OrderState = "Drop Done";
                                }
                                else
                                {
                                    lGCOrderViewModel.OrderState = item.Status.StatusCode != null ? item.Status.StatusDescription : String.Empty;
                                }
                            }
                            lGCOrderList.Add(lGCOrderViewModel);
                        }
                    }
                }
                #endregion

                var data = lGCOrderList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Assign Order to Driver by Service partner list Added by Pooja Jatav
        [HttpPost]
        //orderStartDate, orderEndDate, regdNo, servicepartnerName, driverName, driverphoneNo, vehicleno, drivercity, ticketnumber, actionorder
        public async Task<ActionResult> OrderAssignbySPlist(int companyId, DateTime? orderStartDate, DateTime? orderEndDate, string? regdNo, string? driverphoneNo, string? servicepartnerName, string? driverName, string? vehicleno, string? driverCity, string? ticketnumber)
        {
            #region Variable declaration
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }

            if (!string.IsNullOrWhiteSpace(driverphoneNo) && driverphoneNo != "null")
            { driverphoneNo = driverphoneNo.Trim().ToLower(); }
            else { driverphoneNo = null; }

            if (!string.IsNullOrWhiteSpace(servicepartnerName) && servicepartnerName != "null")
            { servicepartnerName = servicepartnerName.Trim().ToLower(); }
            else { servicepartnerName = null; }

            if (!string.IsNullOrWhiteSpace(driverName) && driverName != "null")
            { driverName = driverName.Trim().ToLower(); }
            else { driverName = null; }

            if (!string.IsNullOrWhiteSpace(vehicleno) && vehicleno != "null")
            { vehicleno = vehicleno.Trim().ToLower(); }
            else { vehicleno = null; }

            if (!string.IsNullOrWhiteSpace(driverCity) && driverCity != "null")
            { driverCity = driverCity.Trim().ToLower(); }
            else { driverCity = null; }

            if (!string.IsNullOrWhiteSpace(ticketnumber) && ticketnumber != "null")
            { ticketnumber = ticketnumber.Trim().ToLower(); }
            else { ticketnumber = null; }


            List<TblLogistic>? TblLogistics = null;
            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
            TblServicePartner? tblServicePartner = null;
            TblDriverDetail? tblDriverDetail = null;
            TblCity? tblCity = null;
            List<MobileAppLogisticViewModel> MobileAppLogisticList = null;
            int count = 0;
            TblCompany? tblCompany = null;
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
                count = _context.TblLogistics
                               .Include(x => x.ServicePartner)
                               .Include(x => x.DriverDetails)
                               .Count(x => x.IsActive == true
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner))
                               && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= Convert.ToDateTime(orderStartDate) && x.ModifiedDate <= Convert.ToDateTime(orderEndDate))
                               && (string.IsNullOrEmpty(servicepartnerName) || (x.ServicePartner != null && x.ServicePartner.ServicePartnerBusinessName.Contains(servicepartnerName)))
                               && (string.IsNullOrEmpty(driverName) || (x.DriverDetails != null && x.DriverDetails.DriverName.Contains(driverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.DriverDetails != null && x.DriverDetails.VehicleNumber.Contains(vehicleno)))
                               && (string.IsNullOrEmpty(ticketnumber) || (x.TicketNumber != null && x.TicketNumber.Contains(ticketnumber)))
                               && (string.IsNullOrEmpty(driverCity) || (x.DriverDetails != null && (x.DriverDetails.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || (x.DriverDetails != null && (x.DriverDetails.DriverPhoneNumber ?? "").ToLower() == driverphoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               ));
                if (count > 0)
                {
                    TblLogistics = await _context.TblLogistics
                               .Include(x => x.ServicePartner)
                               .Include(x => x.DriverDetails)
                               .Where(x => x.IsActive == true
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner))
                               && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= Convert.ToDateTime(orderStartDate) && x.ModifiedDate <= Convert.ToDateTime(orderEndDate)))
                               && (string.IsNullOrEmpty(servicepartnerName) || (x.ServicePartner != null && x.ServicePartner.ServicePartnerBusinessName.Contains(servicepartnerName)))
                               && (string.IsNullOrEmpty(driverName) || (x.DriverDetails != null && x.DriverDetails.DriverName.Contains(driverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.DriverDetails != null && x.DriverDetails.VehicleNumber.Contains(vehicleno)))
                               && (string.IsNullOrEmpty(driverCity) || (x.DriverDetails != null && (x.DriverDetails.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || (x.DriverDetails != null && (x.DriverDetails.DriverPhoneNumber ?? "").ToLower() == driverphoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               && (string.IsNullOrEmpty(ticketnumber) || (x.TicketNumber ?? "").ToLower() == ticketnumber)
                               ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.LogisticId).Skip(skip).Take(pageSize).ToListAsync();

                    recordsTotal = count;
                }
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (TblLogistics != null && TblLogistics.Count > 0)
                {
                    
                    MobileAppLogisticList = _mapper.Map<List<TblLogistic>, List<MobileAppLogisticViewModel>>(TblLogistics);
                    //string actionURL = string.Empty;

                    foreach (MobileAppLogisticViewModel item in MobileAppLogisticList)
                    {
                        item.LogisticId = item.LogisticId > 0 ? item.LogisticId : 0;
                        item.TicketNumber = item.TicketNumber != null ? item.TicketNumber.ToString() : string.Empty;
                        item.RegdNo = item.RegdNo != null ? item.RegdNo.ToString() : string.Empty;
                        item.ModifiedDate = item.ModifiedDate != null ? item.ModifiedDate : null;
                        //item.ServicePartnerName = item.ServicePartner != null ? item.ServicePartner.ServicePartnerName : string.Empty;
                        //item.DriverName = item.DriverDetails != null ? item.DriverDetails.DriverName : string.Empty;
                        //item.DriverPhoneNo = item.DriverDetails != null ? item.DriverDetails.DriverPhoneNumber : string.Empty;
                     
                        if (item.ServicePartnerId > 0)
                        {
                            tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == item.ServicePartnerId).FirstOrDefault();
                            item.ServicePartnerName = tblServicePartner != null ? tblServicePartner.ServicePartnerName : null;
                        }
                        if (item.driverDetailsId > 0)
                        {
                            tblDriverDetail = _context.TblDriverDetails.Where(x => x.IsActive == true && x.DriverDetailsId == item.driverDetailsId).FirstOrDefault();
                            item.DriverName = tblDriverDetail != null ? tblDriverDetail.DriverName : null;
                            item.VehicleNo = tblDriverDetail != null ? tblDriverDetail.VehicleNumber : null;
                            item.DriverPhoneNo = tblDriverDetail != null ? tblDriverDetail.DriverPhoneNumber : null;
                            if (tblDriverDetail?.CityId > 0)
                            {
                                tblCity = _context.TblCities.Where(x => x.IsActive == true && x.CityId == tblDriverDetail.CityId).FirstOrDefault();
                                item.DriverCity = tblCity != null ? tblCity.Name : null;
                            }
                        }


                        //actionURL = " <div class='actionbtns'>";
                        //actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>&nbsp;";
                        //actionURL = actionURL + "<a href ='" + URL + "/Exchange/Manage?id=" + _protector.Encode(item.Id) + "' onclick='RecordView(" + item.Id + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";

                        //TblExchangeOrderStatus exchangeOrderStatuscode = _context.TblExchangeOrderStatuses.FirstOrDefault(x => x.Id == item.StatusId);
                        //if (exchangeOrderStatuscode != null)
                        //{
                        //    item.StatusCode = exchangeOrderStatuscode.StatusCode;
                        //}

                    }
                }
                #endregion
                var data = MobileAppLogisticList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Order Accept and Reject By Driver list Added by Pooja Jatav
        [HttpPost]
        public async Task<ActionResult> AcceptRejectbyDriverlist(int companyId, DateTime? orderStartDate, DateTime? orderEndDate, string? regdNo, string? driverphoneNo, string? servicepartnerName, string? driverName, string? vehicleno, string? driverCity, string? ticketnumber)
        {
            #region Variable declaration
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }

            if (!string.IsNullOrWhiteSpace(driverphoneNo) && driverphoneNo != "null")
            { driverphoneNo = driverphoneNo.Trim().ToLower(); }
            else { driverphoneNo = null; }

            if (!string.IsNullOrWhiteSpace(servicepartnerName) && servicepartnerName != "null")
            { servicepartnerName = servicepartnerName.Trim().ToLower(); }
            else { servicepartnerName = null; }

            if (!string.IsNullOrWhiteSpace(driverName) && driverName != "null")
            { driverName = driverName.Trim().ToLower(); }
            else { driverName = null; }

            if (!string.IsNullOrWhiteSpace(vehicleno) && vehicleno != "null")
            { vehicleno = vehicleno.Trim().ToLower(); }
            else { vehicleno = null; }

            if (!string.IsNullOrWhiteSpace(driverCity) && driverCity != "null")
            { driverCity = driverCity.Trim().ToLower(); }
            else { driverCity = null; }

            if (!string.IsNullOrWhiteSpace(ticketnumber) && ticketnumber != "null")
            { ticketnumber = ticketnumber.Trim().ToLower(); }
            else { ticketnumber = null; }


            List<TblLogistic>? TblLogistics = null;
            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
            TblServicePartner? tblServicePartner = null;
            TblDriverDetail? tblDriverDetail = null;
            TblCity? tblCity = null;
            List<MobileAppLogisticViewModel> MobileAppLogisticList = null;
            int count = 0;
            TblCompany? tblCompany = null;
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
                count = _context.TblLogistics
                               .Include(x => x.ServicePartner)
                               .Include(x => x.DriverDetails)
                               .Count(x => x.IsActive == true
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner))
                               && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= Convert.ToDateTime(orderStartDate) && x.ModifiedDate <= Convert.ToDateTime(orderEndDate))
                               && (string.IsNullOrEmpty(servicepartnerName) || (x.ServicePartner != null && x.ServicePartner.ServicePartnerBusinessName.Contains(servicepartnerName)))
                               && (string.IsNullOrEmpty(driverName) || (x.DriverDetails != null && x.DriverDetails.DriverName.Contains(driverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.DriverDetails != null && x.DriverDetails.VehicleNumber.Contains(vehicleno)))
                               && (string.IsNullOrEmpty(ticketnumber) || (x.TicketNumber != null && x.TicketNumber.Contains(ticketnumber)))
                               && (string.IsNullOrEmpty(driverCity) || (x.DriverDetails != null && (x.DriverDetails.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || (x.DriverDetails != null && (x.DriverDetails.DriverPhoneNumber ?? "").ToLower() == driverphoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               ));
                if (count > 0)
                {
                    TblLogistics = await _context.TblLogistics
                               .Include(x => x.ServicePartner)
                               .Include(x => x.DriverDetails)
                               .Where(x => x.IsActive == true
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner))
                               && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= Convert.ToDateTime(orderStartDate) && x.ModifiedDate <= Convert.ToDateTime(orderEndDate)))
                               && (string.IsNullOrEmpty(servicepartnerName) || (x.ServicePartner != null && x.ServicePartner.ServicePartnerBusinessName.Contains(servicepartnerName)))
                               && (string.IsNullOrEmpty(driverName) || (x.DriverDetails != null && x.DriverDetails.DriverName.Contains(driverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.DriverDetails != null && x.DriverDetails.VehicleNumber.Contains(vehicleno)))
                               && (string.IsNullOrEmpty(driverCity) || (x.DriverDetails != null && (x.DriverDetails.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || (x.DriverDetails != null && (x.DriverDetails.DriverPhoneNumber ?? "").ToLower() == driverphoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               && (string.IsNullOrEmpty(ticketnumber) || (x.TicketNumber ?? "").ToLower() == ticketnumber)
                               ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.LogisticId).Skip(skip).Take(pageSize).ToListAsync();

                    recordsTotal = count;
                }
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (TblLogistics != null && TblLogistics.Count > 0)
                {
                    MobileAppLogisticList = _mapper.Map<List<TblLogistic>, List<MobileAppLogisticViewModel>>(TblLogistics);             

                    foreach (MobileAppLogisticViewModel item in MobileAppLogisticList)
                    {
                        item.LogisticId = item.LogisticId > 0 ? item.LogisticId : 0;
                        item.TicketNumber = item.TicketNumber != null ? item.TicketNumber.ToString() : string.Empty;
                        item.RegdNo = item.RegdNo != null ? item.RegdNo.ToString() : string.Empty;
                        item.ModifiedDate = item.ModifiedDate != null ? item.ModifiedDate : null;
                     
                        if (item.ServicePartnerId > 0)
                        {
                            tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == item.ServicePartnerId).FirstOrDefault();
                            item.ServicePartnerName = tblServicePartner != null ? tblServicePartner.ServicePartnerName : null;
                        }
                        if (item.driverDetailsId > 0)
                        {
                            tblDriverDetail = _context.TblDriverDetails.Where(x => x.IsActive == true && x.DriverDetailsId == item.driverDetailsId).FirstOrDefault();
                            item.DriverName = tblDriverDetail != null ? tblDriverDetail.DriverName : null;
                            item.VehicleNo = tblDriverDetail != null ? tblDriverDetail.VehicleNumber : null;
                            item.DriverPhoneNo = tblDriverDetail != null ? tblDriverDetail.DriverPhoneNumber : null;
                            if (tblDriverDetail?.CityId > 0)
                            {
                                tblCity = _context.TblCities.Where(x => x.IsActive == true && x.CityId == tblDriverDetail.CityId).FirstOrDefault();
                                item.DriverCity = tblCity != null ? tblCity.Name : null;
                            }
                        }

                    }
                }
                #endregion
                var data = MobileAppLogisticList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

    }
}
