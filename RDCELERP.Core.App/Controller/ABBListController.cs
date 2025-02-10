using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RDCELERP.Common.Constant;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.AbbRegistration;
using RDCELERP.Model.Base;
using RDCELERP.Model.ABBRedemption;
using RDCELERP.DAL.IRepository;
using DocumentFormat.OpenXml.Spreadsheet;
using RDCELERP.Model.Program;
using Microsoft.AspNetCore.Mvc.Rendering;
using RDCELERP.DAL.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RDCELERP.Model.Company;
using Microsoft.Extensions.Options;
using RDCELERP.Common.Helper;
using RDCELERP.Common.Enums;
using RDCELERP.Model;
using RDCELERP.BAL.Helper;
using RDCELERP.Model.QC;

namespace RDCELERP.Core.App.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ABBListController : ControllerBase
    {
        #region Variable Declaration
        private Digi2l_DevContext _context;
        private IMapper _mapper;
        private CustomDataProtection _protector;
        IAbbRegistrationRepository _abbRegistrationRepository;
        IABBRedemptionRepository _abbRedemptionRepository;
        IBrandRepository _brandRepository;
        private IOptions<ApplicationSettings> _config;
        IBusinessPartnerRepository _businessPartnerRepository;
        IBusinessUnitRepository _businessUnitRepository;
        ICompanyRepository _companyRepository;
        IProductCategoryRepository _productCategoryRepository;
        ILogging _logging;
        IProductTypeRepository _productTypeRepository;
        IBrandSmartBuyRepository _brandSmartBuyRepository;
        IOrderTransRepository _orderTransRepository;
        IOrderQCRepository _orderQCRepository;
        TblOrderTran tblOrderTrans = null;
        IExchangeABBStatusHistoryRepository _exchangeABBStatusHistoryRepository;
        #endregion

        #region Model
        public AbbRegistrationModel AbbRegistrationModel { get; set; }
        string InvoiceImageURL = string.Empty;
        string actionURL = string.Empty;
        #endregion

        #region Constructor
        public ABBListController(IBrandRepository brandRepository, IAbbRegistrationRepository abbRegistrationRepository, IMapper mapper, Digi2l_DevContext context, CustomDataProtection protector, IOptions<ApplicationSettings> config, IBusinessPartnerRepository businessPartnerRepository, IBusinessUnitRepository businessUnitRepository, ICompanyRepository companyRepository, ILogging logging, IProductTypeRepository productTypeRepository, IProductCategoryRepository productCategoryRepository, IBrandSmartBuyRepository brandSmartBuyRepository, IABBRedemptionRepository aBBRedemptionRepository, IOrderTransRepository orderTransRepository, IOrderQCRepository orderQCRepository, IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository)
        {
            _context = context;
            _mapper = mapper;
            _protector = protector;
            _abbRegistrationRepository = abbRegistrationRepository;
            _brandRepository = brandRepository;
            _config = config;
            _businessPartnerRepository = businessPartnerRepository;
            _businessUnitRepository = businessUnitRepository;
            _companyRepository = companyRepository;
            _logging = logging;
            _productTypeRepository = productTypeRepository;
            _productCategoryRepository = productCategoryRepository;
            _brandSmartBuyRepository = brandSmartBuyRepository;
            _abbRedemptionRepository = aBBRedemptionRepository;
            _orderTransRepository = orderTransRepository;
            _orderQCRepository = orderQCRepository;
            _exchangeABBStatusHistoryRepository = exchangeABBStatusHistoryRepository;
        }
        #endregion

        #region ABB Registration Approve List
        [HttpPost]
        public async Task<ActionResult> ABBApproveList(int companyId, DateTime? startDate, DateTime? endDate,
            int? newProductCategoryType, string? regdNo, string? companyName, string? custFirstName)
        {
            #region Variable declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(custFirstName) && custFirstName != "null")
            { custFirstName = custFirstName.Trim().ToLower(); }
            else { custFirstName = null; }
            //if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            //{ custCity = custCity.Trim().ToLower(); }
            //else { custCity = null; }

            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            List<TblAbbregistration> TblABBRegistrations = null;
            string ERPAbbInvoiceUrl = _config.Value.BaseURL + EnumHelper.DescriptionAttr(FileAddressEnum.ABBInvoice);

            string InvoiceimagURL = string.Empty;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
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
                int searchByBusinessUnitId = 0;

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);

                        if (tblBusinessUnit != null)
                            searchByBusinessUnitId = tblBusinessUnit.BusinessUnitId;
                    }
                }

                if (startDate != null && endDate != null)
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region search by company Name filter
                if (!string.IsNullOrEmpty(companyName))
                {
                    tblBusinessUnit = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.Name.ToLower().Contains(companyName));
                    if (tblBusinessUnit != null)
                    {
                        searchByBusinessUnitId = tblBusinessUnit.BusinessUnitId;
                    }
                    else
                    {
                        TblABBRegistrations = new List<TblAbbregistration>();



                        List<AbbRegistrationModel> AbbRegistrationList1 = _mapper.Map<List<TblAbbregistration>, List<AbbRegistrationModel>>(TblABBRegistrations);


                        var data1 = AbbRegistrationList1;
                        var jsonData1 = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 };
                        return Ok(jsonData1);
                    }
                    
                }
                #endregion
                //#region search by ProductType Name filter
                //if (!string.IsNullOrEmpty(companyName))
                //{
                //    TblProductType tblProductType = 
                //    if (tblBusinessUnit != null)
                //    {
                //        searchByBusinessUnitId = tblBusinessUnit.BusinessUnitId;
                //    }
                //}
                //#endregion

                #region table object Initialization
                count = _context.TblAbbregistrations
                                .Include(x => x.NewProductCategory)
                                .Include(x => x.NewProductCategoryTypeNavigation).
                                Count(x => x.IsActive == true && (x.BusinessUnitId == null || x.BusinessUnitId == searchByBusinessUnitId)
                        && (x.AbbApprove == true) && (x.AbbReject == null || x.AbbReject == false)
                        && ((x.StatusId == null) || (x.StatusId != null && new[] { Convert.ToInt32(OrderStatusEnum.PaymentFailed), Convert.ToInt32(OrderStatusEnum.PaymentNotInitiated), Convert.ToInt32(OrderStatusEnum.PaymentSuccessful) }.Contains(x.StatusId.Value)))
                        && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                        && (newProductCategoryType == null || x.NewProductCategoryTypeId == newProductCategoryType)
                        && (string.IsNullOrEmpty(custFirstName) || x.CustFirstName == custFirstName)
                        //&& (string.IsNullOrEmpty(companyName) || x.BusinessUnitId == tblBusinessUnit.BusinessUnitId)
                        && (string.IsNullOrEmpty(regdNo)) || x.RegdNo == regdNo);

                if (count > 0)
                {
                    TblABBRegistrations = await _context.TblAbbregistrations
                                .Include(x => x.NewProductCategory)
                                .Include(x => x.NewProductCategoryTypeNavigation).Where
                                (x => x.IsActive == true && (tblBusinessUnit == null || x.BusinessUnitId == tblBusinessUnit.BusinessUnitId)
                        && (x.AbbApprove == true) && (x.AbbReject == null || x.AbbReject == false)
                        && ((x.StatusId == null) || (x.StatusId != null && new[] { Convert.ToInt32(OrderStatusEnum.PaymentFailed), Convert.ToInt32(OrderStatusEnum.PaymentNotInitiated), Convert.ToInt32(OrderStatusEnum.PaymentSuccessful) }.Contains(x.StatusId.Value)))
                        && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                        && (newProductCategoryType == null || x.NewProductCategoryTypeId == newProductCategoryType)
                        && (string.IsNullOrEmpty(custFirstName) || x.CustFirstName == custFirstName)
                        //&& (string.IsNullOrEmpty(companyName) || x.BusinessUnitId == tblBusinessUnit.BusinessUnitId)
                        && (string.IsNullOrEmpty(regdNo) || x.RegdNo == regdNo)).OrderByDescending(x => x.ModifiedDate).ToListAsync();

                    recordsTotal = count;
                }
                #endregion

                recordsTotal = TblABBRegistrations != null ? TblABBRegistrations.Count : 0;
                if (TblABBRegistrations != null)
                {
                    TblABBRegistrations = TblABBRegistrations.Skip(skip).Take(pageSize).ToList();
                }
                else
                    TblABBRegistrations = new List<TblAbbregistration>();



                List<AbbRegistrationModel> AbbRegistrationList = _mapper.Map<List<TblAbbregistration>, List<AbbRegistrationModel>>(TblABBRegistrations);



                foreach (AbbRegistrationModel item in AbbRegistrationList)
                {
                    actionURL = " <div class='actionbtns'>";
                    actionURL = "<a href ='" + URL + "/ABB/ABBRegistration?id=" + item.AbbregistrationId + "' ><button onclick='RecordView(" + item.AbbregistrationId + ")' class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                    actionURL = actionURL + "</div>";
                    item.Action = actionURL;

                    InvoiceimagURL = MVCURL + item.InvoiceImage;
                    InvoiceimagURL = "<img src='" + InvoiceimagURL + "' class='img-responsive'/>";
                    item.InvoiceImage = InvoiceimagURL;


                    TblAbbregistration Abbreg = TblABBRegistrations.FirstOrDefault(x => x.AbbregistrationId == item.AbbregistrationId);



                    item.CustFullName = Abbreg.CustFirstName + " " + Abbreg.CustLastName;

                    if (Abbreg.BusinessUnitId != null && Abbreg.BusinessUnitId > 0)
                    {
                        TblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == Abbreg.BusinessUnitId);
                        if (businessUnit != null)
                            item.CompanyName = businessUnit.Name;
                    }

                    if (Abbreg.NewProductCategoryTypeId != null && Abbreg.NewProductCategoryTypeId > 0)
                    {
                        TblProductType tblProductType = _productTypeRepository.GetSingle(x => x.IsActive == true && x.Id == Convert.ToInt32(Abbreg.NewProductCategoryTypeId));
                        if (tblProductType != null && !string.IsNullOrEmpty(tblProductType.Description))
                        {
                            item.NewProductCategoryType = tblProductType.Description.ToString();
                        }
                        else
                        {
                            item.NewProductCategoryType = "Not Found";
                        }
                    }

                    //item.CreatedDate =  ;
                    DateTime dt = Convert.ToDateTime(Abbreg.InvoiceDate);
                    item.RegistrationDate = dt.ToString("dd/MM/yyyy");

                }



                var data = AbbRegistrationList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ABBListController", "ABBApproveList", ex);
            }
            return Ok();


        }
        #endregion

        #region ABB Registration Not Approve List
        [HttpPost]
        public async Task<ActionResult> ABBDataList(int companyId, DateTime? startDate, DateTime? endDate,
            int? newProductCategoryType, string? regdNo, string? companyName, string? custFirstName)
        {
            #region Variable declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(custFirstName) && custFirstName != "null")
            { custFirstName = custFirstName.Trim().ToLower(); }
            else { custFirstName = null; }
            //if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            //{ custCity = custCity.Trim().ToLower(); }
            //else { custCity = null; }

            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            List<TblAbbregistration> TblABBRegistrations = null;
            string ERPAbbInvoiceUrl = _config.Value.BaseURL + EnumHelper.DescriptionAttr(FileAddressEnum.ABBInvoice);

            string InvoiceimagURL = string.Empty;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
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
                int searchByBusinessUnitId = 0;

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);

                        if (tblBusinessUnit != null)
                            searchByBusinessUnitId = tblBusinessUnit.BusinessUnitId;
                    }
                }

                if (startDate != null && endDate != null)
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region search by company Name filter
                if (!string.IsNullOrEmpty(companyName))
                {
                    tblBusinessUnit = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.Name.ToLower().Contains(companyName));
                    if (tblBusinessUnit != null)
                    {
                        searchByBusinessUnitId = tblBusinessUnit.BusinessUnitId;
                    }
                    else
                    {
                        TblABBRegistrations = new List<TblAbbregistration>();

                        List<AbbRegistrationModel> AbbRegistrationList1 = _mapper.Map<List<TblAbbregistration>, List<AbbRegistrationModel>>(TblABBRegistrations);

                        var data1 = AbbRegistrationList1;
                        var jsonData1 = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 };
                        return Ok(jsonData1);
                    }

                }
                #endregion
                //#region search by ProductType Name filter
                //if (!string.IsNullOrEmpty(companyName))
                //{
                //    TblProductType tblProductType = 
                //    if (tblBusinessUnit != null)
                //    {
                //        searchByBusinessUnitId = tblBusinessUnit.BusinessUnitId;
                //    }
                //}
                //#endregion

                #region table object Initialization
                count = _context.TblAbbregistrations
                                .Include(x => x.NewProductCategory)
                                .Include(x => x.NewProductCategoryTypeNavigation).
                                Count(x => x.IsActive == true && (tblBusinessUnit == null || x.BusinessUnitId == searchByBusinessUnitId)
                        && (x.AbbApprove == false || x.AbbApprove == null) && (x.AbbReject == null || x.AbbReject == false)
                        && (x.InvoiceDate!=null)
                        && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                        && (newProductCategoryType == null || x.NewProductCategoryTypeId == newProductCategoryType)
                        && (string.IsNullOrEmpty(custFirstName) || x.CustFirstName == custFirstName)
                        //&& (string.IsNullOrEmpty(companyName) || x.BusinessUnitId == tblBusinessUnit.BusinessUnitId)
                        && (string.IsNullOrEmpty(regdNo)) || x.RegdNo == regdNo);

                if (count > 0)
                {
                    TblABBRegistrations = await _context.TblAbbregistrations
                                .Include(x => x.NewProductCategory)
                                .Include(x => x.NewProductCategoryTypeNavigation).Where
                                (x => x.IsActive == true && (tblBusinessUnit == null || x.BusinessUnitId == tblBusinessUnit.BusinessUnitId)
                        && (x.AbbApprove == false || x.AbbApprove == null) && (x.AbbReject == null || x.AbbReject == false)
                        && (x.InvoiceDate != null)
                        && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                        && (newProductCategoryType == null || x.NewProductCategoryTypeId == newProductCategoryType)
                        && (string.IsNullOrEmpty(custFirstName) || x.CustFirstName == custFirstName)
                        //&& (string.IsNullOrEmpty(companyName) || x.BusinessUnitId == tblBusinessUnit.BusinessUnitId)
                        && (string.IsNullOrEmpty(regdNo) || x.RegdNo == regdNo)).OrderByDescending(x => x.ModifiedDate).ToListAsync();

                    recordsTotal = count;
                }
                #endregion

                recordsTotal = TblABBRegistrations != null ? TblABBRegistrations.Count : 0;
                if (TblABBRegistrations != null)
                {
                    TblABBRegistrations = TblABBRegistrations.Skip(skip).Take(pageSize).ToList();
                }
                else
                    TblABBRegistrations = new List<TblAbbregistration>();

                List<AbbRegistrationModel> AbbRegistrationList = _mapper.Map<List<TblAbbregistration>, List<AbbRegistrationModel>>(TblABBRegistrations);

                foreach (AbbRegistrationModel item in AbbRegistrationList)
                {
                    if (_config.Value.EditPageforAbbRegistration)
                    {
                        actionURL = " <div class='actionbtns'>";
                        actionURL = actionURL + "<a href='#' onclick='ABBApprove(" + item.AbbregistrationId + ")' class='btn btn-sm btn-success'  data-bs-toggle='tooltip' data-bs-placement='top' title='Approve'><i class='fa-solid fa-check text-white'></i></a>";

                        actionURL = actionURL + "<a class='mx-1' href ='" + URL + "/ABB/ABBRegistrationEdit?id=" + item.AbbregistrationId + "' >" +
                        "<button onclick='ABBEdit(" + item.AbbregistrationId + ")' class='btn btn-sm btn-primary'data-bs-toggle='tooltip' data-bs-placement='op' title='Edit'><i class='fa-solid fa-pen'></i></button></a>" +
                        "<button onclick='ABBReject(" + item.AbbregistrationId + ")' class='btn btn-sm btn-danger'data-bs-toggle='tooltip' data-bs-placement='top' title='Reject'><i class='fa-solid fa-xmark'></i></button>";
                        actionURL = actionURL + "</div>";
                    }
                    else
                    {
                        actionURL = " <div class='actionbtns'>";
                        actionURL = actionURL + "<a href='#' onclick='ABBApprove(" + item.AbbregistrationId + ")' class='btn btn-sm btn-success'  data-bs-toggle='tooltip' data-bs-placement='top' title='Approve'><i class='fa-solid fa-check text-white'></i></a>";

                        actionURL = actionURL + "<a class='mx-1' href ='" + URL + "/ABB/ABBRegistrationEdit?id=" + item.AbbregistrationId + "' >"  +
                        "<button onclick='ABBReject(" + item.AbbregistrationId + ")' class='btn btn-sm btn-danger'data-bs-toggle='tooltip' data-bs-placement='top' title='Reject'><i class='fa-solid fa-xmark'></i></button>";
                        actionURL = actionURL + "</div>";
                    }
                    
                    item.Action = actionURL;

                    TblAbbregistration Abbreg = TblABBRegistrations.FirstOrDefault(x => x.AbbregistrationId == item.AbbregistrationId);

                    item.CustFullName = Abbreg.CustFirstName + " " + Abbreg.CustLastName;

                    if (Abbreg.BusinessUnitId != null && Abbreg.BusinessUnitId > 0)
                    {
                        TblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == Abbreg.BusinessUnitId);
                        if (businessUnit != null)
                            item.CompanyName = businessUnit.Name;
                    }

                    if (Abbreg.NewProductCategoryTypeId != null && Abbreg.NewProductCategoryTypeId > 0)
                    {
                        TblProductType tblProductType = _productTypeRepository.GetSingle(x => x.IsActive == true && x.Id == Convert.ToInt32(Abbreg.NewProductCategoryTypeId));
                        if (tblProductType != null && !string.IsNullOrEmpty(tblProductType.Description))
                        {
                            item.NewProductCategoryType = tblProductType.Description.ToString();
                        }
                        else
                        {
                            item.NewProductCategoryType = "Not Found";
                        }
                    }

                    //item.CreatedDate =  ;
                    DateTime dt = Convert.ToDateTime(Abbreg.InvoiceDate);
                    item.RegistrationDate = dt.ToString("dd/MM/yyyy");

                }
                var data = AbbRegistrationList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ABBListController", "ABBDataList", ex);
            }
            return Ok();



        }
        #endregion

        #region ABB Registration Reject List
        public async Task<ActionResult> ABBRejectPageList(int companyId, DateTime? startDate, DateTime? endDate,
            int? newProductCategoryType, string? regdNo, string? companyName, string? custFirstName)
        {
            #region Variable declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(custFirstName) && custFirstName != "null")
            { custFirstName = custFirstName.Trim().ToLower(); }
            else { custFirstName = null; }
            //if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            //{ custCity = custCity.Trim().ToLower(); }
            //else { custCity = null; }

            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            List<TblAbbregistration> TblABBRegistrations = null;
            string ERPAbbInvoiceUrl = _config.Value.BaseURL + EnumHelper.DescriptionAttr(FileAddressEnum.ABBInvoice);

            string InvoiceimagURL = string.Empty;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
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
                int searchByBusinessUnitId = 0;

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);

                        if (tblBusinessUnit != null)
                            searchByBusinessUnitId = tblBusinessUnit.BusinessUnitId;
                    }
                }

                if (startDate != null && endDate != null)
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region search by company Name filter
                if (!string.IsNullOrEmpty(companyName))
                {
                    tblBusinessUnit = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.Name.ToLower().Contains(companyName));
                    if (tblBusinessUnit != null)
                    {
                        searchByBusinessUnitId = tblBusinessUnit.BusinessUnitId;
                    }
                    else
                    {
                        TblABBRegistrations = new List<TblAbbregistration>();



                        List<AbbRegistrationModel> AbbRegistrationList1 = _mapper.Map<List<TblAbbregistration>, List<AbbRegistrationModel>>(TblABBRegistrations);
                        var data1 = AbbRegistrationList1;
                        var jsonData1 = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 };
                        return Ok(jsonData1);

                    }
                }
                #endregion
                //#region search by ProductType Name filter
                //if (!string.IsNullOrEmpty(companyName))
                //{
                //    TblProductType tblProductType = 
                //    if (tblBusinessUnit != null)
                //    {
                //        searchByBusinessUnitId = tblBusinessUnit.BusinessUnitId;
                //    }
                //}
                //#endregion

                #region table object Initialization
                count = _context.TblAbbregistrations
                                .Include(x => x.NewProductCategory)
                                .Include(x => x.NewProductCategoryTypeNavigation).
                                Count(x => x.IsActive == true && (x.BusinessUnitId == null || x.BusinessUnitId == searchByBusinessUnitId)
                        && (x.AbbReject == true)
                        && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                        && (newProductCategoryType == null || x.NewProductCategoryTypeId == newProductCategoryType)
                        && (string.IsNullOrEmpty(custFirstName) || x.CustFirstName == custFirstName)
                        //&& (string.IsNullOrEmpty(companyName) || x.BusinessUnitId == tblBusinessUnit.BusinessUnitId)
                        && (string.IsNullOrEmpty(regdNo)) || x.RegdNo == regdNo);

                if (count > 0)
                {
                    TblABBRegistrations = await _context.TblAbbregistrations
                                .Include(x => x.NewProductCategory)
                                .Include(x => x.NewProductCategoryTypeNavigation).Where
                                (x => x.IsActive == true && (tblBusinessUnit == null || x.BusinessUnitId == tblBusinessUnit.BusinessUnitId)
                        && ( x.AbbReject == true)
                        && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                        && (newProductCategoryType == null || x.NewProductCategoryTypeId == newProductCategoryType)
                        && (string.IsNullOrEmpty(custFirstName) || x.CustFirstName == custFirstName)
                        //&& (string.IsNullOrEmpty(companyName) || x.BusinessUnitId == tblBusinessUnit.BusinessUnitId)
                        && (string.IsNullOrEmpty(regdNo) || x.RegdNo == regdNo)).OrderByDescending(x => x.AbbregistrationId).ToListAsync();

                    recordsTotal = count;
                }
                #endregion

                recordsTotal = TblABBRegistrations != null ? TblABBRegistrations.Count : 0;
                if (TblABBRegistrations != null)
                {
                    TblABBRegistrations = TblABBRegistrations.Skip(skip).Take(pageSize).ToList();
                }
                else
                    TblABBRegistrations = new List<TblAbbregistration>();



                List<AbbRegistrationModel> AbbRegistrationList = _mapper.Map<List<TblAbbregistration>, List<AbbRegistrationModel>>(TblABBRegistrations);



                foreach (AbbRegistrationModel item in AbbRegistrationList)
                {
                    actionURL = " <div class='actionbtns'>";
                    actionURL = "<a href ='" + URL + "/ABB/ABBRegistration?id=" + item.AbbregistrationId + "' ><button onclick='RecordView(" + item.AbbregistrationId + ")' class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                    actionURL = actionURL + "<button onclick='AbbReject(" + item.AbbregistrationId + ")' class='btn btn-primary btn-sm mx-1' data-bs-toggle='tooltip' data-bs-placement='op' title='Undo'><i class='fa-solid fa-undo'></i></button></a>";
                    actionURL = actionURL + "</div>";
                    item.Action = actionURL;

                    InvoiceimagURL = MVCURL + item.InvoiceImage;
                    InvoiceimagURL = "<img src='" + InvoiceimagURL + "' class='img-responsive'/>";
                    item.InvoiceImage = InvoiceimagURL;


                    TblAbbregistration Abbreg = TblABBRegistrations.FirstOrDefault(x => x.AbbregistrationId == item.AbbregistrationId);


                    item.CustFullName = Abbreg.CustFirstName + " " + Abbreg.CustLastName;

                    if (Abbreg.BusinessUnitId != null && Abbreg.BusinessUnitId > 0)
                    {
                        TblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == Abbreg.BusinessUnitId);
                        if (businessUnit != null)
                            item.CompanyName = businessUnit.Name;
                        else
                        {
                            item.CompanyName = "Not Found";
                        }
                    }
                    else
                    {
                        item.CompanyName = "Not Found";
                    }

                    if (Abbreg.NewProductCategoryTypeId != null && Abbreg.NewProductCategoryTypeId > 0)
                    {
                        TblProductType tblProductType = _productTypeRepository.GetSingle(x => x.IsActive == true && x.Id == Convert.ToInt32(Abbreg.NewProductCategoryTypeId));
                        if (tblProductType != null && !string.IsNullOrEmpty(tblProductType.Description))
                        {
                            item.NewProductCategoryType = tblProductType.Description.ToString();
                        }
                        else
                        {
                            item.NewProductCategoryType = "Not Found";
                        }
                    }

                    //item.CreatedDate =  ;
                    DateTime dt = Convert.ToDateTime(Abbreg.CreatedDate);
                    item.RegistrationDate = dt.ToString("dd/MM/yyyy");

                }



                var data = AbbRegistrationList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ABBListController", "ABBApproveList", ex);
            }
            return Ok();
        }
        #endregion

        #region ABB Redemption Record List
        public async Task<ActionResult> RedemptionRecordList()
        {
            List<TblAbbregistration> TblABBRegistrations = null;
            List<TblAbbredemption> TblAbbredemptions = null;
            string URL = _config.Value.URLPrefixforProd;
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                TblAbbredemptions = await _context.TblAbbredemptions.Where(x => x.IsActive == true
                        && (string.IsNullOrEmpty(searchValue) || x.RegdNo.ToLower().Contains(searchValue.ToLower()))).OrderByDescending(x => x.AbbregistrationId).ToListAsync();

                recordsTotal = TblAbbredemptions != null ? TblAbbredemptions.Count : 0;

                List<ABBRedemptionViewModel> AbbRedempList = _mapper.Map<List<TblAbbredemption>, List<ABBRedemptionViewModel>>(TblAbbredemptions);

                foreach (ABBRedemptionViewModel item in AbbRedempList)
                {
                    actionURL = " <ul class='actions'>";
                    actionURL = actionURL + "<a href ='" + URL + "/ABBRedemption/RecordEdit?id=" + item.RegdNo + "' ><button onclick='RecordEdit(" + item.RegdNo + ")'  class='btn btn-sm btn-primary'data-bs-toggle='tooltip' data-bs-placement='op' title='Edit'><i class='fa-solid fa-pen'></i></button></a>";
                    item.Action = actionURL;

                    TblABBRegistrations = await _context.TblAbbregistrations.Where((x => x.RegdNo.ToLower() == item.RegdNo)).ToListAsync();
                    List<AbbRegistrationModel> abbRegistrationModels = _mapper.Map<List<TblAbbregistration>, List<AbbRegistrationModel>>(TblABBRegistrations);

                    InvoiceImageURL = "<img src='/DBFiles/ABB/InvoiceImage/" + item.InvoiceImage + "' />";
                    item.InvoiceImage = InvoiceImageURL;

                    foreach (var items in abbRegistrationModels)
                    {
                        TblBrand tblBrand = _context.TblBrands.FirstOrDefault(x => x.Id == items.NewBrandId);
                        items.SponsorName = tblBrand != null ? tblBrand.Name : string.Empty;

                        TblProductCategory tblProductCategory = _context.TblProductCategories.FirstOrDefault(x => x.Id == items.NewProductCategoryId);
                        items.NewProductCategoryName = tblProductCategory != null ? tblProductCategory.Description : string.Empty;

                        TblProductType tblProductType = _context.TblProductTypes.FirstOrDefault(x => x.Id == Convert.ToInt32(items.NewProductCategoryTypeId));
                        items.NewProductCategoryType = tblProductType != null ? tblProductType.Description : string.Empty;

                        item.CustFirstName = items.CustFirstName;
                        item.CustMobile = items.CustMobile;
                        item.CustEmail = items.CustEmail;
                        item.CustAddress1 = items.CustAddress1;
                        item.CustAddress2 = items.CustAddress2;
                        item.CustPinCode = items.CustPinCode;
                        item.CustCity = items.CustCity;
                        item.StateId = items.StateId;
                        item.NewProductCategoryName = items.NewProductCategoryName.ToString();
                        item.NewProductCategoryType = items.NewProductCategoryType.ToString();
                        item.SponsorName = items.SponsorName;
                        item.NewSize = items.NewSize;
                        item.InvoiceImage = items.InvoiceImage != null ? items.InvoiceImage : string.Empty;
                        item.RegdNo = items.RegdNo;
                    }
                }
                var data = AbbRedempList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion

        #region Abb Dashboard ABB Registration Data List Added by Vishal choudhary
        public IActionResult ABBRegistrationList(int ?BusinessUnitId,string? regdNo,string? sponsorOrderNumber, string? EmployeeId,string? phoneNumber,string? Abbstorecode)
        {
            List<TblAbbregistration> TblABBRegistrations = null;
            List<ABBDashBoardViewModel> dashBoardViewModelList = new List<ABBDashBoardViewModel>();
            ABBDashBoardViewModel viewDataDC; new ABBDashBoardViewModel();
            string URL = _config.Value.URLPrefixforProd;

            string baseUrl = _config.Value.BaseURL;
            
            string InvoiceImageUrl = string.Empty;
            TblProductCategory produccategoryObj = new TblProductCategory();
            TblBrand BrandObj = new TblBrand();
            TblBrandSmartBuy BrandSmartBuyObj = new TblBrandSmartBuy();
            TblProductType ProductTypeObj = new TblProductType();
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            #region handeling of search filters  for null
            // Code for handeling null data in filters
            if (regdNo != null)
            {
                regdNo = regdNo.Trim();
            }
            if(!string.IsNullOrEmpty(regdNo) && regdNo != "null" && !string.IsNullOrWhiteSpace(regdNo))
            {
                regdNo = regdNo.Trim();
            }
            else
            {
                regdNo = null;
            }
            if (!string.IsNullOrEmpty(sponsorOrderNumber) && sponsorOrderNumber != "null" && !string.IsNullOrWhiteSpace(sponsorOrderNumber))
            {
                sponsorOrderNumber = sponsorOrderNumber.Trim();
            }
            else
            {
                sponsorOrderNumber = null;
            }
            if (!string.IsNullOrEmpty(EmployeeId) && EmployeeId != "null" && !string.IsNullOrWhiteSpace(EmployeeId))
            {
                EmployeeId = EmployeeId.Trim();
            }
            else
            {
                EmployeeId = null;
            }
            if (!string.IsNullOrEmpty(phoneNumber) && phoneNumber != "null" && !string.IsNullOrWhiteSpace(phoneNumber))
            {
                phoneNumber = phoneNumber.Trim();
            }
            else
            {
                phoneNumber = null;
            }
            if (!string.IsNullOrEmpty(Abbstorecode) && Abbstorecode != "null" && !string.IsNullOrWhiteSpace(Abbstorecode))
            {
                Abbstorecode = Abbstorecode.Trim();
            }
            else
            {
                Abbstorecode = null;
            }

            #endregion
            try
            {
                // data base method call for geting ABB data based on Business unit id and filters
                TblABBRegistrations= _abbRegistrationRepository.GetAllOrderList(BusinessUnitId, regdNo, sponsorOrderNumber, phoneNumber, EmployeeId, Abbstorecode);
                if (TblABBRegistrations != null)
                {
                    recordsTotal = TblABBRegistrations != null ? TblABBRegistrations.Count : 0;
                    TblABBRegistrations = TblABBRegistrations.Skip(skip).Take(pageSize).ToList();
                    string actionURL = string.Empty;

                    if (TblABBRegistrations.Count > 0)
                    {
                        string directoryPath = @"wwwroot/DBFiles/ABB/ABBApprovalCertificate/";
                        string[] files = Directory.Exists(directoryPath) ? Directory.GetFiles(directoryPath) : new string[0];
                        List<string> allfiles = new List<string>();

                        if (files.Count() > 0)
                        {
                            foreach (string eachPath in files)
                            {
                                var eachFileName = Path.GetFileName(eachPath);
                                allfiles.Add(eachFileName);
                            }
                        }

                        // Code to create list for data and pass on view page to show as list in data table
                        foreach (var item in TblABBRegistrations)
                        {
                            viewDataDC=new ABBDashBoardViewModel();
                            actionURL = " <ul class='actions'>";
                            actionURL = actionURL + "<a href ='" + URL + "/ABB/ABBRegistration?id=" + item.AbbregistrationId + "'><button onclick='RecordView(" + item.AbbregistrationId + ")'  class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>&nbsp;";

                            if (item?.BusinessUnit?.IsCertificateAvailable == true)
                            {
                                ValidateABBCertificatesOnServerPath.GetUrlsforCertificates(ref actionURL, URL, item?.RegdNo, allfiles);                                
                            }
                            viewDataDC.ActionUrl= actionURL;
                            InvoiceImageUrl = "<img src='/DBFiles/ABB/InvoiceImage/" + item.InvoiceImage + "' />";
                             viewDataDC.InvoiceImage = InvoiceImageUrl;
                             viewDataDC.RegdNo = item.RegdNo;
                             viewDataDC.SponsorOrderNo = item.SponsorOrderNo;
                             viewDataDC.ProductCategory = item.NewProductCategory.Description;
                             viewDataDC.AbbStoreCode = item.StoreCode;
                             viewDataDC.ProductType = item.NewProductCategoryTypeNavigation.Description;
                            if (item.NewBrandId > 0)
                            {
                                if (item.BusinessUnit.IsBumultiBrand == false)
                                {
                                    BrandObj = _brandRepository.GetBrand(item.NewBrandId);
                                    if (BrandObj != null)
                                    {
                                        viewDataDC.BrandName = BrandObj.Name;
                                    }
                                }
                                else
                                {
                                    BrandSmartBuyObj = _brandSmartBuyRepository.GetBrand(item.NewBrandId);
                                    if (BrandSmartBuyObj != null)
                                    {
                                        viewDataDC.BrandName = BrandSmartBuyObj.Name;
                                    }
                                }
                            }
                            viewDataDC.AbbregistrationId = item.AbbregistrationId;
                            viewDataDC.CustFirstName = item.CustFirstName;
                            viewDataDC.CustLastName = item.CustLastName;
                            viewDataDC.CustMobile = item.CustMobile;
                            viewDataDC.CustCity = item.CustCity;
                            viewDataDC.CustEmail = item.CustEmail;
                            viewDataDC.CustState = item.CustState;
                            viewDataDC.EmployeeId = item.EmployeeId;
                            viewDataDC.AbbApprove = item.AbbApprove;
                            viewDataDC.BusinessUnitName = item.BusinessUnit.Name;
                            if (viewDataDC.AbbApprove == true)
                            {
                                viewDataDC.AbbAproved = "Yes";
                            }
                            else
                            {
                                viewDataDC.AbbAproved = "No";
                            }
                            dashBoardViewModelList.Add(viewDataDC);
                        }
                    }
                    if (dashBoardViewModelList.Count > 0)
                    {
                        dashBoardViewModelList.OrderByDescending(x => x.AbbregistrationId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ABBListController", "ABBRegistrationList", ex);
            }
            var data = dashBoardViewModelList;
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return new JsonResult(jsonData);
        }


        #endregion

        #region ABB Redemption Data For DashBoard List Added by Vishal choudhary

        public IActionResult ABBRedemptionDataList(int? BusinessUnitId, string? regdNoRedemp, string? sponsorOrderNumber, string? phoneNumber, string? referenceId, string? storeCode)
        {
            List<TblAbbredemption> tblAbbredemptionsList = null;
            List<ABBRedemptionListViewModel> dashBoardViewModelList = new List<ABBRedemptionListViewModel>();
            ABBRedemptionListViewModel viewDataDC; 
            string URL = _config.Value.URLPrefixforProd;
            string InvoiceImageUrl = string.Empty;
            TblProductCategory produccategoryObj = new TblProductCategory();
            TblBrand BrandObj = new TblBrand();
            TblBrandSmartBuy BrandSmartBuyObj = new TblBrandSmartBuy();
            TblProductType ProductTypeObj = new TblProductType();
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            #region handeling of search filters  for null

            if (regdNoRedemp != null)
            {
                regdNoRedemp = regdNoRedemp.Trim();
            }
            if (!string.IsNullOrEmpty(regdNoRedemp) && regdNoRedemp != "null" && !string.IsNullOrWhiteSpace(regdNoRedemp))
            {
                regdNoRedemp = regdNoRedemp.Trim();
            }
            else
            {
                regdNoRedemp = null;
            }
            if (!string.IsNullOrEmpty(sponsorOrderNumber) && sponsorOrderNumber != "null" && !string.IsNullOrWhiteSpace(sponsorOrderNumber))
            {
                sponsorOrderNumber = sponsorOrderNumber.Trim();
            }
            else
            {
                sponsorOrderNumber = null;
            }
           
            if (!string.IsNullOrEmpty(phoneNumber) && phoneNumber != "null" && !string.IsNullOrWhiteSpace(phoneNumber))
            {
                phoneNumber = phoneNumber.Trim();
            }
            else
            {
                phoneNumber = null;
            }
            if (!string.IsNullOrEmpty(referenceId) && referenceId != "null" && !string.IsNullOrWhiteSpace(referenceId))
            {
                referenceId = referenceId.Trim();
            }
            else
            {
                referenceId = null;
            }
            if (!string.IsNullOrEmpty(storeCode) && storeCode != "null" && !string.IsNullOrWhiteSpace(storeCode))
            {
                storeCode = storeCode.Trim();
            }
            else
            {
                storeCode = null;
            }
            #endregion
            try
            {
                tblAbbredemptionsList = _abbRedemptionRepository.GetRedemptionDataList(BusinessUnitId, regdNoRedemp, sponsorOrderNumber, phoneNumber, referenceId, storeCode);
                if (tblAbbredemptionsList != null)
                {
                    recordsTotal = tblAbbredemptionsList != null ? tblAbbredemptionsList.Count : 0;
                    tblAbbredemptionsList = tblAbbredemptionsList.Skip(skip).Take(pageSize).ToList();
                    string actionURL = string.Empty;

                    if (tblAbbredemptionsList.Count > 0)
                    {
                        foreach (var item in tblAbbredemptionsList)
                        {
                            viewDataDC = new ABBRedemptionListViewModel();
                            actionURL = " <ul class='actions'>";
                            actionURL = actionURL + "<a href ='" + URL + "/ABBRedemption/Manage?regdNo=" + item.RegdNo + "' ><button onclick='RecordView(" + item.RegdNo + ")'  class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                            viewDataDC.ActionUrl = actionURL;
                            InvoiceImageUrl = "<img src='/DBFiles/ABB/InvoiceImage/" + item.InvoiceImage + "' />";
                            viewDataDC.InvoiceImage = InvoiceImageUrl;
                            viewDataDC.RegdNo = item.RegdNo;
                            viewDataDC.sponsorOrderNumber = item.Abbregistration.SponsorOrderNo;
                            viewDataDC.ProductCategory = item.Abbregistration.NewProductCategory.Description;
                            viewDataDC.ProductType = item.Abbregistration.NewProductCategoryTypeNavigation.Description;
                            viewDataDC.AbbregistrationId = item.AbbregistrationId;
                            viewDataDC.customerName = item.Abbregistration.CustFirstName+" "+ item.Abbregistration.CustLastName;
                            viewDataDC.customerPhoneNumber = item.Abbregistration.CustMobile;
                            if (item.RedemptionDate != null)
                            {
                                DateTime dt = Convert.ToDateTime(item.RedemptionDate);
                                viewDataDC.RedemptionDate = dt.ToString("dd-MMM-yyyy");
                               
                            }
                            viewDataDC.RedemptionId = item.RedemptionId;
                            viewDataDC.RedemptionPercentage = item.RedemptionPercentage;
                            viewDataDC.RedemptionPeriod = item.RedemptionPeriod;
                            viewDataDC.RedemptionValue = item.RedemptionValue;
                            viewDataDC.ReferenceId = item.ReferenceId;
                            viewDataDC.StoreCode = item.Abbregistration.StoreCode;
                            viewDataDC.BusinessUnitname = item.Abbregistration.BusinessUnit.Name;
                            dashBoardViewModelList.Add(viewDataDC);
                        }
                    }
                    if (dashBoardViewModelList.Count > 0)
                    {
                        dashBoardViewModelList.OrderByDescending(x => x.RedemptionId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ABBListController", "ABBRegistrationList", ex);
            }
            var data = dashBoardViewModelList;
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return new JsonResult(jsonData);
        }
        #endregion

        #region ABB Redemption Order List Added by shashi
        public IActionResult ABBRedemptionOrderList(int companyId, DateTime? startDate, DateTime? endDate, int? productCatId,
            int? productTypeId, string regdNo, string phoneNo, string pinCode, string companyName)
        {
            List<TblAbbredemption> TblAbbredemptions = null;
            List<ABBRedemptionViewModel> aBBRedemptionViewModelList = null;
            ABBRedemptionViewModel aBBRedemptionViewModel = null;
            TblOrderTran tblOrderTran = null;
            int count = 0;
            string URL = _config.Value.URLPrefixforProd;
            if (string.IsNullOrEmpty(regdNo) || regdNo == "null")
            { regdNo = "".Trim().ToLower(); }
            else
            { regdNo = regdNo.Trim().ToLower(); }
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


            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                #region Shashi
                if (companyId > 0 && companyId != 1007)
                {
                    TblCompany tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        TblBusinessUnit tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                        {
                            if (startDate == null && endDate == null)
                            {
                                count = _context.TblAbbredemptions
                                    .Include(x => x.CustomerDetails)
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.ModelNumber)
                                    .Count(x => x.IsActive == true && x.Abbregistration.BusinessUnit.Name == tblBusinessUnit.Name.ToLower() && x.StatusId != Convert.ToInt32(OrderStatusEnum.CancelOrder) && x.StatusId != Convert.ToInt32(OrderStatusEnum.QCOrderCancel)
                                     && (productCatId > 0 ? (x.Abbregistration.NewProductCategory.Id == 0 ? false : (x.Abbregistration.NewProductCategory.Id == productCatId)) : true)
                                     && (productTypeId > 0 ? (x.Abbregistration.NewProductCategoryTypeNavigation.Id == 0 ? false : (x.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId)) : true)
                                     && x.RegdNo.ToLower().Contains(regdNo)
                                     && x.CustomerDetails.PhoneNumber.Contains(phoneNo)
                                     && x.CustomerDetails.ZipCode.Contains(pinCode)
                                     && x.Abbregistration.BusinessUnit.Name.Contains(companyName)
                                     );
                                recordsTotal = count;
                                if (count > 0)
                                {
                                    TblAbbredemptions = _context.TblAbbredemptions
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                    .Include(x => x.Status)
                                    .Include(x => x.CustomerDetails)
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                                    .Where(x => x.IsActive == true && x.Abbregistration.BusinessUnit.Name == tblBusinessUnit.Name.ToLower()
                                     && x.StatusId != Convert.ToInt32(OrderStatusEnum.CancelOrder)
                                     && x.StatusId != Convert.ToInt32(OrderStatusEnum.QCOrderCancel)
                                     && (productCatId > 0 ? (x.Abbregistration.NewProductCategory.Id == 0 ? false : (x.Abbregistration.NewProductCategory.Id == productCatId)) : true)
                                     && (productTypeId > 0 ? (x.Abbregistration.NewProductCategoryTypeNavigation.Id == 0 ? false : (x.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId)) : true)
                                     && x.RegdNo.ToLower().Contains(regdNo)
                                     && x.CustomerDetails.PhoneNumber.Contains(phoneNo)
                                     && x.CustomerDetails.ZipCode.Contains(pinCode)
                                     && x.Abbregistration.BusinessUnit.Name.Contains(companyName)
                                      ).OrderByDescending(x => x.ModifiedDate).Skip(skip).Take(pageSize).ToList();

                                    TblAbbredemptions.OrderByDescending(x => x.RedemptionId);
                                }
                            }
                            else
                            {
                                count = _context.TblAbbredemptions.Count(x => x.IsActive == true);
                                recordsTotal = count;
                                if (count > 0)
                                {
                                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                                    TblAbbredemptions = _context.TblAbbredemptions
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                    .Include(x => x.Status)
                                    .Include(x => x.CustomerDetails)
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                                    .Where(x => x.IsActive == true && x.Abbregistration.BusinessUnit.Name == tblBusinessUnit.Name.ToLower()
                                     && x.StatusId != Convert.ToInt32(OrderStatusEnum.CancelOrder)
                                     && x.StatusId != Convert.ToInt32(OrderStatusEnum.QCOrderCancel)
                                     && (productCatId > 0 ? (x.Abbregistration.NewProductCategory.Id == 0 ? false : (x.Abbregistration.NewProductCategory.Id == productCatId)) : true)
                                     && (productTypeId > 0 ? (x.Abbregistration.NewProductCategoryTypeNavigation.Id == 0 ? false : (x.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId)) : true)
                                     && x.RegdNo.ToLower().Contains(regdNo)
                                     && x.CustomerDetails.PhoneNumber.Contains(phoneNo)
                                     && x.CustomerDetails.ZipCode.Contains(pinCode)
                                     && x.Abbregistration.BusinessUnit.Name.Contains(companyName)
                                      ).OrderByDescending(x => x.ModifiedDate).Skip(skip).Take(pageSize).ToList();

                                    TblAbbredemptions.OrderByDescending(x => x.RedemptionId);
                                }
                            }

                        }
                    }
                }
                else
                {
                    if (startDate == null && endDate == null)
                    {
                        count = _context.TblAbbredemptions
                                    .Include(x => x.CustomerDetails)
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.ModelNumber)
                                    .Count(x => x.IsActive == true && x.StatusId != Convert.ToInt32(OrderStatusEnum.CancelOrder) && x.StatusId != Convert.ToInt32(OrderStatusEnum.QCOrderCancel)
                                     && (productCatId > 0 ? (x.Abbregistration.NewProductCategory.Id == 0 ? false : (x.Abbregistration.NewProductCategory.Id == productCatId)) : true)
                                     && (productTypeId > 0 ? (x.Abbregistration.NewProductCategoryTypeNavigation.Id == 0 ? false : (x.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId)) : true)
                                     && x.RegdNo.ToLower().Contains(regdNo)
                                     && x.CustomerDetails.PhoneNumber.Contains(phoneNo)
                                     && x.CustomerDetails.ZipCode.Contains(pinCode)
                                     && x.Abbregistration.BusinessUnit.Name.Contains(companyName)
                                     );
                        recordsTotal = count;
                        if (count > 0)
                        {
                            TblAbbredemptions = _context.TblAbbredemptions
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                    .Include(x => x.Status)
                                    .Include(x => x.CustomerDetails)
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                                    .Where(x => x.IsActive == true && x.StatusId != Convert.ToInt32(OrderStatusEnum.CancelOrder)
                                     && x.StatusId != Convert.ToInt32(OrderStatusEnum.QCOrderCancel)
                                     && (productCatId > 0 ? (x.Abbregistration.NewProductCategory.Id == 0 ? false : (x.Abbregistration.NewProductCategory.Id == productCatId)) : true)
                                     && (productTypeId > 0 ? (x.Abbregistration.NewProductCategoryTypeNavigation.Id == 0 ? false : (x.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId)) : true)
                                     && x.RegdNo.ToLower().Contains(regdNo)
                                     && x.CustomerDetails.PhoneNumber.Contains(phoneNo)
                                     && x.CustomerDetails.ZipCode.Contains(pinCode)
                                     && x.Abbregistration.BusinessUnit.Name.Contains(companyName)
                                      ).OrderByDescending(x => x.ModifiedDate).Skip(skip).Take(pageSize).ToList();

                            TblAbbredemptions.OrderByDescending(x => x.RedemptionId);
                        }
                    }
                    else
                    {
                        count = _context.TblAbbredemptions.Count(x => x.IsActive == true);
                        recordsTotal = count;
                        if (count > 0)
                        {
                            startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                            endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                            TblAbbredemptions = _context.TblAbbredemptions
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                    .Include(x => x.Status)
                                    .Include(x => x.CustomerDetails)
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                                    .Where(x => x.IsActive == true && x.StatusId != Convert.ToInt32(OrderStatusEnum.CancelOrder)
                                     && x.StatusId != Convert.ToInt32(OrderStatusEnum.QCOrderCancel)
                                     && (productCatId > 0 ? (x.Abbregistration.NewProductCategory.Id == 0 ? false : (x.Abbregistration.NewProductCategory.Id == productCatId)) : true)
                                     && (productTypeId > 0 ? (x.Abbregistration.NewProductCategoryTypeNavigation.Id == 0 ? false : (x.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId)) : true)
                                     && x.RegdNo.ToLower().Contains(regdNo)
                                     && x.CustomerDetails.PhoneNumber.Contains(phoneNo)
                                     && x.CustomerDetails.ZipCode.Contains(pinCode)
                                     && x.Abbregistration.BusinessUnit.Name.Contains(companyName)
                                      ).OrderByDescending(x => x.ModifiedDate).Skip(skip).Take(pageSize).ToList();

                            TblAbbredemptions.OrderByDescending(x => x.ModifiedDate);
                        }
                    }
                }
                #endregion
                //recordsTotal = TblAbbredemptions != null ? TblAbbredemptions.Count : 0;
                aBBRedemptionViewModelList = new List<ABBRedemptionViewModel>();
                if (TblAbbredemptions != null)
                {
                    string actionURL = string.Empty;
                    foreach (var item in TblAbbredemptions)
                    {
                        aBBRedemptionViewModel = new ABBRedemptionViewModel();
                        tblOrderTran = _orderTransRepository.GetDetailsByRedemptionId(item.RedemptionId);
                        if (tblOrderTran != null)
                        {
                            aBBRedemptionViewModel.OrderTransId = tblOrderTran.OrderTransId;
                            actionURL = " <div class='actionbtns'>";
                            actionURL = "<a href ='" + URL + "/ABBRedemption/Manage?regdNo=" + item.RegdNo + "' ><button onclick='RecordView(" + item.RegdNo + ")' class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                            actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + aBBRedemptionViewModel.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                            actionURL = actionURL + "</div>";
                        }
                        else
                        {
                            tblOrderTran = new TblOrderTran();
                        }
                        aBBRedemptionViewModel.Action = actionURL;
                        aBBRedemptionViewModel.RedemptionId = item.RedemptionId;
                        aBBRedemptionViewModel.OrderDate = Convert.ToDateTime(item.CreatedDate).ToString("dd/MM/yyyy");
                        if (item.Abbregistration.BusinessUnit != null)
                        {
                            aBBRedemptionViewModel.CompanyName = item.Abbregistration.BusinessUnit.Name != null ? item.Abbregistration.BusinessUnit.Name : string.Empty;
                        }
                        else
                        {
                            aBBRedemptionViewModel.CompanyName = "";
                        }
                        aBBRedemptionViewModel.RegdNo = item.RegdNo;
                        if (item.CustomerDetails != null)
                        {
                            aBBRedemptionViewModel.CustFirstName = item.CustomerDetails.FirstName != null ? item.CustomerDetails.FirstName : string.Empty;
                            aBBRedemptionViewModel.CustLastName = item.CustomerDetails.LastName != null ? item.CustomerDetails.LastName : string.Empty;
                            aBBRedemptionViewModel.CustEmail = item.CustomerDetails.Email != null ? item.CustomerDetails.Email : string.Empty;
                            aBBRedemptionViewModel.CustCity = item.CustomerDetails.City != null ? item.CustomerDetails.City : string.Empty;
                            aBBRedemptionViewModel.CustPinCode = item.CustomerDetails.ZipCode != null ? item.CustomerDetails.ZipCode : string.Empty;
                            aBBRedemptionViewModel.CustAddress1 = item.CustomerDetails.Address1 != null ? item.CustomerDetails.Address1 : string.Empty;
                            aBBRedemptionViewModel.CustAddress2 = item.CustomerDetails.Address2 != null ? item.CustomerDetails.Address2 : string.Empty;
                            aBBRedemptionViewModel.CustMobile = item.CustomerDetails.PhoneNumber != null ? item.CustomerDetails.PhoneNumber : string.Empty;
                            aBBRedemptionViewModel.CustState = item.CustomerDetails.State != null ? item.CustomerDetails.State : string.Empty;
                        }
                        else
                        {
                            aBBRedemptionViewModel.CustFirstName = "";
                            aBBRedemptionViewModel.CustLastName = "";
                            aBBRedemptionViewModel.CustEmail = "";
                            aBBRedemptionViewModel.CustCity = "";
                            aBBRedemptionViewModel.CustPinCode = "";
                            aBBRedemptionViewModel.CustAddress1 = "";
                            aBBRedemptionViewModel.CustAddress2 = "";
                            aBBRedemptionViewModel.CustMobile = "";
                            aBBRedemptionViewModel.CustState = "";
                        }
                        if (item.Abbregistration.NewProductCategory != null)
                        {
                            aBBRedemptionViewModel.NewProductCategoryName = item.Abbregistration.NewProductCategory.Description != null ? item.Abbregistration.NewProductCategory.Description : string.Empty;
                        }
                        else
                        {
                            aBBRedemptionViewModel.NewProductCategoryName = "";
                        }
                        if (item.Abbregistration.NewProductCategory != null)
                        {
                            aBBRedemptionViewModel.NewProductCategoryType = item.Abbregistration.NewProductCategoryTypeNavigation.Description != null ? item.Abbregistration.NewProductCategoryTypeNavigation.Description : string.Empty;
                        }
                        else
                        {
                            aBBRedemptionViewModel.NewProductCategoryType = "";
                        }
                        if (item.Status != null)
                        {
                            aBBRedemptionViewModel.StatusCode = item.Status.StatusCode != null ? item.Status.StatusCode : string.Empty;
                        }
                        else
                        {
                            aBBRedemptionViewModel.StatusCode = "";
                        }

                        aBBRedemptionViewModelList.Add(aBBRedemptionViewModel);
                    }
                }

                var data = aBBRedemptionViewModelList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion

        #region ABB QC Order List (Self QC List) Added by shashi
        [HttpPost]
        public async Task<ActionResult> ExchangeListOfSelfQC(int companyId, DateTime? orderStartDate,
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

            List<TblAbbredemption> tblAbbredemptions = null;
            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.InvoiceImageURL;
            string InvoiceimagURL = string.Empty;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            List<ABBRedemptionViewModel> aBBRedemptionViewModelList = null;
            string ABBImagesURL = string.Empty;
            int count = 0;
            ABBRedemptionViewModel aBBRedemptionViewModel = null;
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
                count = _context.TblAbbredemptions.Include(x => x.Status)
                                .Include(x => x.TblOrderTrans).ThenInclude(x => x.TblOrderQcs)
                                .Include(x => x.Abbregistration).ThenInclude(x => x.Customer)
                                .Include(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                                .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                .Count(x => x.IsActive == true && (tblBusinessUnit == null || x.Abbregistration.BusinessUnit.Name == tblBusinessUnit.Name)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.SelfQCbyCustomer))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null)
                               || (x.TblOrderTrans.Count > 0 && x.TblOrderTrans.First().TblOrderQcs.Count > 0
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))
                               && (productCatId == null || (x.Abbregistration.NewProductCategory != null && x.Abbregistration.NewProductCategory.Id == productCatId))
                               && (productTypeId == null || x.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.CustomerDetails != null && x.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.CustomerDetails != null && (x.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName)
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               );
                if (count > 0)
                {
                    tblAbbredemptions = await _context.TblAbbredemptions.Include(x => x.Status)
                                .Include(x => x.TblOrderTrans).ThenInclude(x => x.TblOrderQcs)
                                .Include(x => x.Abbregistration).ThenInclude(x => x.Customer)
                                .Include(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                                .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                    .Where(x => x.IsActive == true && (tblBusinessUnit == null || x.Abbregistration.BusinessUnit.Name == tblBusinessUnit.Name)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.SelfQCbyCustomer))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null)
                               || (x.TblOrderTrans.Count > 0 && x.TblOrderTrans.First().TblOrderQcs.Count > 0
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))
                               && (productCatId == null || (x.Abbregistration.NewProductCategory != null && x.Abbregistration.NewProductCategory.Id == productCatId))
                               && (productTypeId == null || x.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.CustomerDetails != null && x.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.CustomerDetails != null && (x.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName)
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.AbbregistrationId).Skip(skip).Take(pageSize).ToListAsync();
                    recordsTotal = count;
                }
                #endregion

                aBBRedemptionViewModelList = new List<ABBRedemptionViewModel>();
                #region Data Initialization for Datatable from table to Model
                if (tblAbbredemptions != null && tblAbbredemptions.Count > 0)
                {
                    string actionURL = string.Empty;
                    TblOrderQc tblOrderQc = null;
                    foreach (var item in tblAbbredemptions)
                    {
                        aBBRedemptionViewModel = new ABBRedemptionViewModel();
                        tblOrderTrans = _orderTransRepository.GetQcDetailsByABBRedemptionId(item.RedemptionId);
                        if (tblOrderTrans != null)
                        {
                            aBBRedemptionViewModel.OrderTransId = tblOrderTrans.OrderTransId;
                            aBBRedemptionViewModel.RedemptionId = item.RedemptionId;
                            actionURL = " <div class='actionbtns'>";
                            actionURL = actionURL + " <a class='btn-sm btn-primary btn-sm' href='" + URL + "/Index1?orderTransId=" + tblOrderTrans.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>&nbsp;";
                            actionURL = actionURL + "<a href ='" + URL + "/ABBRedemption/Manage?regdNo=" + tblOrderTrans.RegdNo + "' onclick='RecordView(" + tblOrderTrans.RegdNo + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                            actionURL = actionURL + "</div>";
                            aBBRedemptionViewModel.Action = actionURL;

                            aBBRedemptionViewModel.OrderDate = Convert.ToDateTime(item.CreatedDate).ToString("dd/MM/yyyy");
                            if (item.Abbregistration.BusinessUnit != null)
                            {
                                aBBRedemptionViewModel.CompanyName = item.Abbregistration.BusinessUnit.Name != null ? item.Abbregistration.BusinessUnit.Name : string.Empty;
                            }
                            else
                            {
                                aBBRedemptionViewModel.CompanyName = "";
                            }
                            aBBRedemptionViewModel.RegdNo = item.RegdNo;
                            if (item.CustomerDetails != null)
                            {
                                aBBRedemptionViewModel.CustFullname = item.CustomerDetails.FirstName + "" + item.CustomerDetails.LastName;
                                aBBRedemptionViewModel.CustEmail = item.CustomerDetails.Email != null ? item.CustomerDetails.Email : string.Empty;
                                aBBRedemptionViewModel.CustCity = item.CustomerDetails.City != null ? item.CustomerDetails.City : string.Empty;
                                aBBRedemptionViewModel.CustPinCode = item.CustomerDetails.ZipCode != null ? item.CustomerDetails.ZipCode : string.Empty;
                                aBBRedemptionViewModel.CustAddress = item.CustomerDetails.Address1 + "" + item.CustomerDetails.Address2 != null ? item.CustomerDetails.Address2 : null;
                                aBBRedemptionViewModel.CustMobile = item.CustomerDetails.PhoneNumber != null ? item.CustomerDetails.PhoneNumber : string.Empty;
                                aBBRedemptionViewModel.CustState = item.CustomerDetails.State != null ? item.CustomerDetails.State : string.Empty;
                            }
                            else
                            {
                                aBBRedemptionViewModel.CustFullname = "";
                                aBBRedemptionViewModel.CustEmail = "";
                                aBBRedemptionViewModel.CustCity = "";
                                aBBRedemptionViewModel.CustPinCode = "";
                                aBBRedemptionViewModel.CustAddress = "";
                                aBBRedemptionViewModel.CustMobile = "";
                                aBBRedemptionViewModel.CustState = "";
                            }
                            if (item.Abbregistration.NewProductCategory != null)
                            {
                                aBBRedemptionViewModel.NewProductCategoryName = item.Abbregistration.NewProductCategory.Description != null ? item.Abbregistration.NewProductCategory.Description : string.Empty;
                            }
                            else
                            {
                                aBBRedemptionViewModel.NewProductCategoryName = "";
                            }
                            if (item.Abbregistration.NewProductCategory != null)
                            {
                                aBBRedemptionViewModel.NewProductCategoryType = item.Abbregistration.NewProductCategoryTypeNavigation.Description != null ? item.Abbregistration.NewProductCategoryTypeNavigation.Description : string.Empty;
                            }
                            else
                            {
                                aBBRedemptionViewModel.NewProductCategoryType = "";
                            }
                            if (item.Status != null)
                            {
                                aBBRedemptionViewModel.StatusCode = item.Status.StatusCode != null ? item.Status.StatusCode : string.Empty;
                            }
                            else
                            {
                                aBBRedemptionViewModel.StatusCode = "";
                            }
                            tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTrans.OrderTransId);
                            if (tblOrderQc != null)
                            {
                                aBBRedemptionViewModel.QCComments = tblOrderQc.Qccomments != null ? tblOrderQc.Qccomments : null;
                                aBBRedemptionViewModel.FinalRedemptionValue = tblOrderQc.PriceAfterQc != null ? null : tblOrderQc.PriceAfterQc;
                                aBBRedemptionViewModel.QcDate = tblOrderQc.Qcdate.ToString() != null ? tblOrderQc.Qcdate.ToString() : null;
                                aBBRedemptionViewModel.StatusId = (int)(tblOrderQc.StatusId != null ? tblOrderQc.StatusId : 0);
                                aBBRedemptionViewModel.CreatedBy = tblOrderQc.CreatedBy != null ? tblOrderQc.CreatedBy : null;
                                aBBRedemptionViewModel.CreatedDate = (DateTime)(tblOrderQc.CreatedDate != null ? tblOrderQc.CreatedDate : null);
                                aBBRedemptionViewModel.ModifiedBy = tblOrderQc.ModifiedBy != null ? tblOrderQc.ModifiedBy : null;
                                aBBRedemptionViewModel.ModifiedDateString = Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") : null;
                                aBBRedemptionViewModel.IsActive = tblOrderQc.IsActive != null ? tblOrderQc.IsActive : null;
                                if (tblOrderQc.ProposedQcdate != null)
                                {
                                    aBBRedemptionViewModel.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                    aBBRedemptionViewModel.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                                    aBBRedemptionViewModel.PreferredQCDate = tblOrderQc.ProposedQcdate != null ? tblOrderQc.ProposedQcdate : null;
                                    aBBRedemptionViewModel.PreferredQCDateString = Convert.ToDateTime(aBBRedemptionViewModel.PreferredQCDate).ToString("MM/dd/yyyy");
                                }
                            }
                        }

                        InvoiceimagURL = MVCURL + aBBRedemptionViewModel.InvoiceImageName;
                        ABBImagesURL = "<img src='" + InvoiceimagURL + "' class='img-responsive' />";
                        aBBRedemptionViewModel.InvoiceImageName = ABBImagesURL;
                        aBBRedemptionViewModelList.Add(aBBRedemptionViewModel);
                    }
                }
                var data = aBBRedemptionViewModelList;
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

        #region All Rescheduled QC list (with Flag 3R & 3RA) added by shashi
        [HttpPost]
        public async Task<IActionResult> AllRescheduledQC(int companyId, DateTime? orderStartDate,
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

            string URL = _config.Value.URLPrefixforProd;
            string BaseURL = _config.Value.BaseURL;
            List<TblOrderQc> TblOrderQcs = null;
            int count = 0;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            List<QCCommentViewModel> QCCommentVMlist = null;
            QCCommentViewModel qCCommentViewModel = null;
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
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Status)
                               .Count(x => x.IsActive == true && (tblBusinessUnit == null || tblBusinessUnit.Name == x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name)
                               && (x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)
                               || x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentRescheduled_3RA))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null) || (x.ProposedQcdate >= resheduleStartDate && x.ProposedQcdate <= resheduleEndDate))
                               && (x.OrderTrans != null && x.OrderTrans.Abbredemption != null)
                               && (productCatId == null || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Id == productCatId))
                               && (productTypeId == null || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId))
                               && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Abbredemption.Abbregistration.Customer != null && x.OrderTrans.Abbredemption.Abbregistration.Customer.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Abbredemption.Abbregistration.Customer != null && x.OrderTrans.Abbredemption.Abbregistration.Customer.City == custCity))
                               && (string.IsNullOrEmpty(companyName) || x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name == companyName)
                               && (string.IsNullOrEmpty(regdNo) || x.OrderTrans.RegdNo == regdNo)
                               );

                if (count > 0)
                {
                    TblOrderQcs = await _context.TblOrderQcs.Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Status)
                               .Where(x => x.IsActive == true && (tblBusinessUnit == null || tblBusinessUnit.Name == x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name)
                               && (x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)
                               || x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentRescheduled_3RA))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null) || (x.ProposedQcdate >= resheduleStartDate && x.ProposedQcdate <= resheduleEndDate))
                               && (x.OrderTrans != null && x.OrderTrans.Abbredemption != null)
                               && (productCatId == null || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Id == productCatId))
                               && (productTypeId == null || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId))
                               && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Abbredemption.Abbregistration.Customer != null && x.OrderTrans.Abbredemption.Abbregistration.Customer.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Abbredemption.Abbregistration.Customer != null && x.OrderTrans.Abbredemption.Abbregistration.Customer.City == custCity))
                               && (string.IsNullOrEmpty(companyName) || x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name == companyName)
                               && (string.IsNullOrEmpty(regdNo) || x.OrderTrans.RegdNo == regdNo)
                               ).OrderByDescending(x => x.ModifiedDate).OrderByDescending(x => x.OrderQcid).Skip(skip).Take(pageSize).ToListAsync();
                }
                recordsTotal = count;
                #endregion

                #region Initialize Datatable
                QCCommentVMlist = new List<QCCommentViewModel>();
                if (TblOrderQcs != null && TblOrderQcs.Count > 0)
                {
                    string actionURL = string.Empty;
                    foreach (var item in TblOrderQcs)
                    {
                        qCCommentViewModel = new QCCommentViewModel();
                        if (item.OrderTrans != null && item.OrderTrans.OrderTransId > 0)
                        {
                            actionURL = " <div class='actionbtns'>";
                            actionURL = "<a href ='" + URL + "/ABBRedemption/Manage?regdNo=" + item.OrderTrans.RegdNo + "' ><button onclick='RecordView(" + item.OrderTrans.RegdNo + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                            actionURL = actionURL + " <a class='btn-sm btn-primary btn-sm' href='" + URL + "/Index1?orderTransId=" + item.OrderTrans.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                            actionURL = actionURL + "</div>";
                            qCCommentViewModel.Action = actionURL;
                            qCCommentViewModel.RegdNo = item.OrderTrans.RegdNo != null ? item.OrderTrans.RegdNo : string.Empty;
                            qCCommentViewModel.AbbRedemptionId = (int)(item.OrderTrans.AbbredemptionId != null ? item.OrderTrans.AbbredemptionId : 0);
                        }
                        if (item.OrderTrans.Abbredemption.Abbregistration.BusinessUnit != null)
                        {
                            qCCommentViewModel.CompanyName = item.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name != null ? item.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.CompanyName = "";
                        }
                        if (item.OrderTrans.Abbredemption.CustomerDetails != null)
                        {
                            qCCommentViewModel.CustFullname = item.OrderTrans.Abbredemption.CustomerDetails.FirstName + "" + item.OrderTrans.Abbredemption.CustomerDetails.LastName;
                            qCCommentViewModel.CustEmail = item.OrderTrans.Abbredemption.CustomerDetails.Email != null ? item.OrderTrans.Abbredemption.CustomerDetails.Email : string.Empty;
                            qCCommentViewModel.CustCity = item.OrderTrans.Abbredemption.CustomerDetails.City != null ? item.OrderTrans.Abbredemption.CustomerDetails.City : string.Empty;
                            qCCommentViewModel.ZipCode = item.OrderTrans.Abbredemption.CustomerDetails.ZipCode != null ? item.OrderTrans.Abbredemption.CustomerDetails.ZipCode : string.Empty;
                            qCCommentViewModel.CustAddress = item.OrderTrans.Abbredemption.CustomerDetails.Address1 + "" + item.OrderTrans.Abbredemption.CustomerDetails.Address2 != null ? item.OrderTrans.Abbredemption.CustomerDetails.Address2 : null;
                            qCCommentViewModel.CustPhoneNumber = item.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber != null ? item.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber : string.Empty;
                            qCCommentViewModel.CustState = item.OrderTrans.Abbredemption.CustomerDetails.State != null ? item.OrderTrans.Abbredemption.CustomerDetails.State : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.CustFullname = "";
                            qCCommentViewModel.CustEmail = "";
                            qCCommentViewModel.CustCity = "";
                            qCCommentViewModel.ZipCode = "";
                            qCCommentViewModel.CustAddress = "";
                            qCCommentViewModel.CustPhoneNumber = "";
                            qCCommentViewModel.CustState = "";
                        }
                        if (item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                        {
                            qCCommentViewModel.ProductCategory = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description != null ? item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.ProductCategory = "";
                        }
                        if (item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                        {
                            qCCommentViewModel.ProductType = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description != null ? item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.ProductType = "";
                        }
                        if (item.OrderTrans.Status != null)
                        {
                            qCCommentViewModel.StatusCode = item.OrderTrans.Status.StatusCode != null ? item.OrderTrans.Status.StatusCode : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.StatusCode = "";
                        }
                        qCCommentViewModel.RescheduleDate = Convert.ToDateTime(item.ProposedQcdate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(item.ProposedQcdate).ToString("dd/MM/yyyy") : null;
                        qCCommentViewModel.Reschedulecount = (int)(item.Reschedulecount != null ? item.Reschedulecount : 0);
                        if (item.OrderTrans.Abbredemption != null)
                        {
                            qCCommentViewModel.OrderCreatedDate = item.OrderTrans.Abbredemption.CreatedDate != null ? item.OrderTrans.Abbredemption.CreatedDate : null;
                        }
                        QCCommentVMlist.Add(qCCommentViewModel);
                    }
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

        #region All Rescheduled QC list (with Flag 3RA only) added by shashi
        [HttpPost]
        public async Task<IActionResult> RescheduledAging(int companyId, DateTime? orderStartDate,
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

            string URL = _config.Value.URLPrefixforProd;
            string BaseURL = _config.Value.BaseURL;
            List<TblOrderQc> TblOrderQcs = null;
            int count = 0;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            List<QCCommentViewModel> QCCommentVMlist = null;
            QCCommentViewModel qCCommentViewModel = null;
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
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Status)
                               .Count(x => x.IsActive == true && (tblBusinessUnit == null || tblBusinessUnit.Name == x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name)
                               && (x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentRescheduled_3RA))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null) || (x.ProposedQcdate >= resheduleStartDate && x.ProposedQcdate <= resheduleEndDate))
                               && (x.OrderTrans != null && x.OrderTrans.Abbredemption != null)
                               && (productCatId == null || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Id == productCatId))
                               && (productTypeId == null || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId))
                               && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Abbredemption.Abbregistration.Customer != null && x.OrderTrans.Abbredemption.Abbregistration.Customer.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Abbredemption.Abbregistration.Customer != null && x.OrderTrans.Abbredemption.Abbregistration.Customer.City == custCity))
                               && (string.IsNullOrEmpty(companyName) || x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name == companyName)
                               && (string.IsNullOrEmpty(regdNo) || x.OrderTrans.RegdNo == regdNo)
                               );

                if (count > 0)
                {
                    TblOrderQcs = await _context.TblOrderQcs
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Status)
                           .Where(x => x.IsActive == true && (tblBusinessUnit == null || tblBusinessUnit.Name == x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name)
                           && (x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentRescheduled_3RA))
                           && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null) || (x.ProposedQcdate >= resheduleStartDate && x.ProposedQcdate <= resheduleEndDate))
                               && (x.OrderTrans != null && x.OrderTrans.Abbredemption != null)
                               && (productCatId == null || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Id == productCatId))
                               && (productTypeId == null || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId))
                               && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Abbredemption.Abbregistration.Customer != null && x.OrderTrans.Abbredemption.Abbregistration.Customer.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Abbredemption.Abbregistration.Customer != null && x.OrderTrans.Abbredemption.Abbregistration.Customer.City == custCity))
                               && (string.IsNullOrEmpty(companyName) || x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name == companyName)
                               && (string.IsNullOrEmpty(regdNo) || x.OrderTrans.RegdNo == regdNo)
                               ).OrderByDescending(x => x.ModifiedDate).OrderByDescending(x => x.OrderQcid).Skip(skip).Take(pageSize).ToListAsync();
                }
                recordsTotal = count;
                #endregion

                #region Initialize Datatable
                QCCommentVMlist = new List<QCCommentViewModel>();
                if (TblOrderQcs != null && TblOrderQcs.Count > 0)
                {
                    string actionURL = string.Empty;
                    foreach (var item in TblOrderQcs)
                    {
                        qCCommentViewModel = new QCCommentViewModel();
                        if (item.OrderTrans != null && item.OrderTrans.OrderTransId > 0)
                        {
                            actionURL = " <div class='actionbtns'>";
                            actionURL = "<a href ='" + URL + "/ABBRedemption/Manage?regdNo=" + item.OrderTrans.RegdNo + "' ><button onclick='RecordView(" + item.OrderTrans.RegdNo + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                            actionURL = actionURL + " <a class='btn-sm btn-primary btn-sm' href='" + URL + "/Index1?orderTransId=" + item.OrderTrans.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                            actionURL = actionURL + "</div>";
                            qCCommentViewModel.Action = actionURL;
                            qCCommentViewModel.RegdNo = item.OrderTrans.RegdNo != null ? item.OrderTrans.RegdNo : string.Empty;
                            qCCommentViewModel.AbbRedemptionId = (int)(item.OrderTrans.AbbredemptionId != null ? item.OrderTrans.AbbredemptionId : 0);
                        }
                        if (item.OrderTrans.Abbredemption.Abbregistration.BusinessUnit != null)
                        {
                            qCCommentViewModel.CompanyName = item.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name != null ? item.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.CompanyName = "";
                        }
                        if (item.OrderTrans.Abbredemption.CustomerDetails != null)
                        {
                            qCCommentViewModel.CustFullname = item.OrderTrans.Abbredemption.CustomerDetails.FirstName + "" + item.OrderTrans.Abbredemption.CustomerDetails.LastName;
                            qCCommentViewModel.CustEmail = item.OrderTrans.Abbredemption.CustomerDetails.Email != null ? item.OrderTrans.Abbredemption.CustomerDetails.Email : string.Empty;
                            qCCommentViewModel.CustCity = item.OrderTrans.Abbredemption.CustomerDetails.City != null ? item.OrderTrans.Abbredemption.CustomerDetails.City : string.Empty;
                            qCCommentViewModel.ZipCode = item.OrderTrans.Abbredemption.CustomerDetails.ZipCode != null ? item.OrderTrans.Abbredemption.CustomerDetails.ZipCode : string.Empty;
                            qCCommentViewModel.CustAddress = item.OrderTrans.Abbredemption.CustomerDetails.Address1 + "" + item.OrderTrans.Abbredemption.CustomerDetails.Address2 != null ? item.OrderTrans.Abbredemption.CustomerDetails.Address2 : null;
                            qCCommentViewModel.CustPhoneNumber = item.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber != null ? item.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber : string.Empty;
                            qCCommentViewModel.CustState = item.OrderTrans.Abbredemption.CustomerDetails.State != null ? item.OrderTrans.Abbredemption.CustomerDetails.State : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.CustFullname = "";
                            qCCommentViewModel.CustEmail = "";
                            qCCommentViewModel.CustCity = "";
                            qCCommentViewModel.ZipCode = "";
                            qCCommentViewModel.CustAddress = "";
                            qCCommentViewModel.CustPhoneNumber = "";
                            qCCommentViewModel.CustState = "";
                        }
                        if (item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                        {
                            qCCommentViewModel.ProductCategory = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description != null ? item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.ProductCategory = "";
                        }
                        if (item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                        {
                            qCCommentViewModel.ProductType = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description != null ? item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.ProductType = "";
                        }
                        if (item.OrderTrans.Status != null)
                        {
                            qCCommentViewModel.StatusCode = item.OrderTrans.Status.StatusCode != null ? item.OrderTrans.Status.StatusCode : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.StatusCode = "";
                        }
                        qCCommentViewModel.RescheduleDate = Convert.ToDateTime(item.ProposedQcdate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(item.ProposedQcdate).ToString("dd/MM/yyyy") : null;
                        qCCommentViewModel.Reschedulecount = (int)(item.Reschedulecount != null ? item.Reschedulecount : 0);
                        if (item.OrderTrans.Abbredemption != null)
                        {
                            qCCommentViewModel.OrderCreatedDate = item.OrderTrans.Abbredemption.CreatedDate != null ? item.OrderTrans.Abbredemption.CreatedDate : null;
                        }
                        QCCommentVMlist.Add(qCCommentViewModel);
                    }
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

        #region Reschedule count 1 (with Flag 3R) added by shashi
        [HttpPost]
        public async Task<IActionResult> QCRescheduleCountOne(int companyId, DateTime? orderStartDate,
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

            string URL = _config.Value.URLPrefixforProd;
            string BaseURL = _config.Value.BaseURL;
            List<TblOrderQc> TblOrderQcs = null;
            int count = 0;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            List<QCCommentViewModel> QCCommentVMlist = null;
            QCCommentViewModel qCCommentViewModel = null;
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
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Status)
                               .Count(x => x.IsActive == true && (tblBusinessUnit == null || tblBusinessUnit.Name == x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name)
                               && ((x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)) && x.Reschedulecount == 1)
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null) || (x.ProposedQcdate >= resheduleStartDate && x.ProposedQcdate <= resheduleEndDate))
                               && (x.OrderTrans != null && x.OrderTrans.Abbredemption != null)
                               && (productCatId == null || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Id == productCatId))
                               && (productTypeId == null || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId))
                               && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Abbredemption.Abbregistration.Customer != null && x.OrderTrans.Abbredemption.Abbregistration.Customer.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Abbredemption.Abbregistration.Customer != null && x.OrderTrans.Abbredemption.Abbregistration.Customer.City == custCity))
                               && (string.IsNullOrEmpty(companyName) || x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name == companyName)
                               && (string.IsNullOrEmpty(regdNo) || x.OrderTrans.RegdNo == regdNo)
                               );

                if (count > 0)
                {
                    TblOrderQcs = await _context.TblOrderQcs.Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Status)
                               .Where(x => x.IsActive == true && (tblBusinessUnit == null || tblBusinessUnit.Name == x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name)
                               && ((x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)) && x.Reschedulecount == 1)
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null) || (x.ProposedQcdate >= resheduleStartDate && x.ProposedQcdate <= resheduleEndDate))
                               && (x.OrderTrans != null && x.OrderTrans.Abbredemption != null)
                               && (productCatId == null || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Id == productCatId))
                               && (productTypeId == null || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId))
                               && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Abbredemption.Abbregistration.Customer != null && x.OrderTrans.Abbredemption.Abbregistration.Customer.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Abbredemption.Abbregistration.Customer != null && x.OrderTrans.Abbredemption.Abbregistration.Customer.City == custCity))
                               && (string.IsNullOrEmpty(companyName) || x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name == companyName)
                               && (string.IsNullOrEmpty(regdNo) || x.OrderTrans.RegdNo == regdNo)
                               ).OrderByDescending(x => x.ModifiedDate).OrderByDescending(x => x.OrderQcid).Skip(skip).Take(pageSize).ToListAsync();
                }
                #endregion

                #region Set Pagination
                recordsTotal = count;
                #endregion

                #region Initialize Datatable
                QCCommentVMlist = new List<QCCommentViewModel>();
                if (TblOrderQcs != null && TblOrderQcs.Count > 0)
                {
                    string actionURL = string.Empty;
                    foreach (var item in TblOrderQcs)
                    {
                        qCCommentViewModel = new QCCommentViewModel();
                        if (item.OrderTrans != null && item.OrderTrans.OrderTransId > 0)
                        {
                            actionURL = " <div class='actionbtns'>";
                            actionURL = "<a href ='" + URL + "/ABBRedemption/Manage?regdNo=" + item.OrderTrans.RegdNo + "' ><button onclick='RecordView(" + item.OrderTrans.RegdNo + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                            actionURL = actionURL + " <a class='btn-sm btn-primary btn-sm' href='" + URL + "/Index1?orderTransId=" + item.OrderTrans.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                            actionURL = actionURL + "</div>";
                            qCCommentViewModel.Action = actionURL;
                            qCCommentViewModel.RegdNo = item.OrderTrans.RegdNo != null ? item.OrderTrans.RegdNo : string.Empty;
                            qCCommentViewModel.AbbRedemptionId = (int)(item.OrderTrans.AbbredemptionId != null ? item.OrderTrans.AbbredemptionId : 0);
                        }
                        if (item.OrderTrans.Abbredemption.Abbregistration.BusinessUnit != null)
                        {
                            qCCommentViewModel.CompanyName = item.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name != null ? item.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.CompanyName = "";
                        }
                        if (item.OrderTrans.Abbredemption.CustomerDetails != null)
                        {
                            qCCommentViewModel.CustFullname = item.OrderTrans.Abbredemption.CustomerDetails.FirstName + "" + item.OrderTrans.Abbredemption.CustomerDetails.LastName;
                            qCCommentViewModel.CustEmail = item.OrderTrans.Abbredemption.CustomerDetails.Email != null ? item.OrderTrans.Abbredemption.CustomerDetails.Email : string.Empty;
                            qCCommentViewModel.CustCity = item.OrderTrans.Abbredemption.CustomerDetails.City != null ? item.OrderTrans.Abbredemption.CustomerDetails.City : string.Empty;
                            qCCommentViewModel.ZipCode = item.OrderTrans.Abbredemption.CustomerDetails.ZipCode != null ? item.OrderTrans.Abbredemption.CustomerDetails.ZipCode : string.Empty;
                            qCCommentViewModel.CustAddress = item.OrderTrans.Abbredemption.CustomerDetails.Address1 + "" + item.OrderTrans.Abbredemption.CustomerDetails.Address2 != null ? item.OrderTrans.Abbredemption.CustomerDetails.Address2 : null;
                            qCCommentViewModel.CustPhoneNumber = item.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber != null ? item.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber : string.Empty;
                            qCCommentViewModel.CustState = item.OrderTrans.Abbredemption.CustomerDetails.State != null ? item.OrderTrans.Abbredemption.CustomerDetails.State : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.CustFullname = "";
                            qCCommentViewModel.CustEmail = "";
                            qCCommentViewModel.CustCity = "";
                            qCCommentViewModel.ZipCode = "";
                            qCCommentViewModel.CustAddress = "";
                            qCCommentViewModel.CustPhoneNumber = "";
                            qCCommentViewModel.CustState = "";
                        }
                        if (item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                        {
                            qCCommentViewModel.ProductCategory = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description != null ? item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.ProductCategory = "";
                        }
                        if (item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                        {
                            qCCommentViewModel.ProductType = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description != null ? item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.ProductType = "";
                        }
                        if (item.OrderTrans.Status != null)
                        {
                            qCCommentViewModel.StatusCode = item.OrderTrans.Status.StatusCode != null ? item.OrderTrans.Status.StatusCode : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.StatusCode = "";
                        }
                        qCCommentViewModel.RescheduleDate = Convert.ToDateTime(item.ProposedQcdate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(item.ProposedQcdate).ToString("dd/MM/yyyy") : null;
                        qCCommentViewModel.Reschedulecount = (int)(item.Reschedulecount != null ? item.Reschedulecount : 0);
                        if (item.OrderTrans.Abbredemption != null)
                        {
                            qCCommentViewModel.OrderCreatedDate = item.OrderTrans.Abbredemption.CreatedDate != null ? item.OrderTrans.Abbredemption.CreatedDate : null;
                        }
                        QCCommentVMlist.Add(qCCommentViewModel);
                    }
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

        #region Reschedule count 2 (with Flag 3R) added by shashi

        [HttpPost]
        public async Task<IActionResult> QCRescheduleCountTwo(int companyId, DateTime? orderStartDate,
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

            string URL = _config.Value.URLPrefixforProd;
            string BaseURL = _config.Value.BaseURL;
            List<TblOrderQc> TblOrderQcs = null;
            int count = 0;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            List<QCCommentViewModel> QCCommentVMlist = null;
            QCCommentViewModel qCCommentViewModel = null;
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
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Status)
                               .Count(x => x.IsActive == true && (tblBusinessUnit == null || tblBusinessUnit.Name == x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name)
                               && ((x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)) && x.Reschedulecount == 2)
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null) || (x.ProposedQcdate >= resheduleStartDate && x.ProposedQcdate <= resheduleEndDate))
                               && (x.OrderTrans != null && x.OrderTrans.Abbredemption != null)
                               && (productCatId == null || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Id == productCatId))
                               && (productTypeId == null || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId))
                               && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Abbredemption.Abbregistration.Customer != null && x.OrderTrans.Abbredemption.Abbregistration.Customer.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Abbredemption.Abbregistration.Customer != null && x.OrderTrans.Abbredemption.Abbregistration.Customer.City == custCity))
                               && (string.IsNullOrEmpty(companyName) || x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name == companyName)
                               && (string.IsNullOrEmpty(regdNo) || x.OrderTrans.RegdNo == regdNo)
                               );

                if (count > 0)
                {
                    TblOrderQcs = await _context.TblOrderQcs.Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Status)
                               .Where(x => x.IsActive == true && (tblBusinessUnit == null || tblBusinessUnit.Name == x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name)
                                && ((x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)) && x.Reschedulecount == 2)
                                && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null) || (x.ProposedQcdate >= resheduleStartDate && x.ProposedQcdate <= resheduleEndDate))
                               && (x.OrderTrans != null && x.OrderTrans.Abbredemption != null)
                               && (productCatId == null || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Id == productCatId))
                               && (productTypeId == null || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId))
                               && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Abbredemption.Abbregistration.Customer != null && x.OrderTrans.Abbredemption.Abbregistration.Customer.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Abbredemption.Abbregistration.Customer != null && x.OrderTrans.Abbredemption.Abbregistration.Customer.City == custCity))
                               && (string.IsNullOrEmpty(companyName) || x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name == companyName)
                               && (string.IsNullOrEmpty(regdNo) || x.OrderTrans.RegdNo == regdNo)
                               ).OrderByDescending(x => x.ModifiedDate).OrderByDescending(x => x.OrderQcid).Skip(skip).Take(pageSize).ToListAsync();
                }
                #endregion

                #region Set Pagination
                recordsTotal = count;
                #endregion

                #region Initialize Datatable
                QCCommentVMlist = new List<QCCommentViewModel>();
                if (TblOrderQcs != null && TblOrderQcs.Count > 0)
                {
                    string actionURL = string.Empty;
                    foreach (var item in TblOrderQcs)
                    {
                        qCCommentViewModel = new QCCommentViewModel();
                        if (item.OrderTrans != null && item.OrderTrans.OrderTransId > 0)
                        {
                            actionURL = " <div class='actionbtns'>";
                            actionURL = "<a href ='" + URL + "/ABBRedemption/Manage?regdNo=" + item.OrderTrans.RegdNo + "' ><button onclick='RecordView(" + item.OrderTrans.RegdNo + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                            actionURL = actionURL + " <a class='btn-sm btn-primary btn-sm' href='" + URL + "/Index1?orderTransId=" + item.OrderTrans.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                            actionURL = actionURL + "</div>";
                            qCCommentViewModel.Action = actionURL;
                            qCCommentViewModel.RegdNo = item.OrderTrans.RegdNo != null ? item.OrderTrans.RegdNo : string.Empty;
                            qCCommentViewModel.AbbRedemptionId = (int)(item.OrderTrans.AbbredemptionId != null ? item.OrderTrans.AbbredemptionId : 0);
                        }
                        if (item.OrderTrans.Abbredemption.Abbregistration.BusinessUnit != null)
                        {
                            qCCommentViewModel.CompanyName = item.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name != null ? item.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.CompanyName = "";
                        }
                        if (item.OrderTrans.Abbredemption.CustomerDetails != null)
                        {
                            qCCommentViewModel.CustFullname = item.OrderTrans.Abbredemption.CustomerDetails.FirstName + "" + item.OrderTrans.Abbredemption.CustomerDetails.LastName;
                            qCCommentViewModel.CustEmail = item.OrderTrans.Abbredemption.CustomerDetails.Email != null ? item.OrderTrans.Abbredemption.CustomerDetails.Email : string.Empty;
                            qCCommentViewModel.CustCity = item.OrderTrans.Abbredemption.CustomerDetails.City != null ? item.OrderTrans.Abbredemption.CustomerDetails.City : string.Empty;
                            qCCommentViewModel.ZipCode = item.OrderTrans.Abbredemption.CustomerDetails.ZipCode != null ? item.OrderTrans.Abbredemption.CustomerDetails.ZipCode : string.Empty;
                            qCCommentViewModel.CustAddress = item.OrderTrans.Abbredemption.CustomerDetails.Address1 + "" + item.OrderTrans.Abbredemption.CustomerDetails.Address2 != null ? item.OrderTrans.Abbredemption.CustomerDetails.Address2 : null;
                            qCCommentViewModel.CustPhoneNumber = item.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber != null ? item.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber : string.Empty;
                            qCCommentViewModel.CustState = item.OrderTrans.Abbredemption.CustomerDetails.State != null ? item.OrderTrans.Abbredemption.CustomerDetails.State : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.CustFullname = "";
                            qCCommentViewModel.CustEmail = "";
                            qCCommentViewModel.CustCity = "";
                            qCCommentViewModel.ZipCode = "";
                            qCCommentViewModel.CustAddress = "";
                            qCCommentViewModel.CustPhoneNumber = "";
                            qCCommentViewModel.CustState = "";
                        }
                        if (item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                        {
                            qCCommentViewModel.ProductCategory = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description != null ? item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.ProductCategory = "";
                        }
                        if (item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                        {
                            qCCommentViewModel.ProductType = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description != null ? item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.ProductType = "";
                        }
                        if (item.OrderTrans.Status != null)
                        {
                            qCCommentViewModel.StatusCode = item.OrderTrans.Status.StatusCode != null ? item.OrderTrans.Status.StatusCode : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.StatusCode = "";
                        }
                        qCCommentViewModel.RescheduleDate = Convert.ToDateTime(item.ProposedQcdate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(item.ProposedQcdate).ToString("dd/MM/yyyy") : null;
                        qCCommentViewModel.Reschedulecount = (int)(item.Reschedulecount != null ? item.Reschedulecount : 0);
                        if (item.OrderTrans.Abbredemption != null)
                        {
                            qCCommentViewModel.OrderCreatedDate = item.OrderTrans.Abbredemption.CreatedDate != null ? item.OrderTrans.Abbredemption.CreatedDate : null;
                        }
                        QCCommentVMlist.Add(qCCommentViewModel);
                    }
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

        #region Reschedule count 3 (with Flag 3R) added by shashi
        [HttpPost]
        public async Task<IActionResult> QCRescheduleCountThree(int companyId, DateTime? orderStartDate,
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

            string URL = _config.Value.URLPrefixforProd;
            string BaseURL = _config.Value.BaseURL;
            List<TblOrderQc> TblOrderQcs = null;
            int count = 0;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            List<QCCommentViewModel> QCCommentVMlist = null;
            QCCommentViewModel qCCommentViewModel = null;
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
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Status)
                               .Count(x => x.IsActive == true && (tblBusinessUnit == null || tblBusinessUnit.Name == x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name)
                               && ((x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)) && x.Reschedulecount == 3)
                                && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null) || (x.ProposedQcdate >= resheduleStartDate && x.ProposedQcdate <= resheduleEndDate))
                               && (x.OrderTrans != null && x.OrderTrans.Abbredemption != null)
                               && (productCatId == null || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Id == productCatId))
                               && (productTypeId == null || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId))
                               && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Abbredemption.Abbregistration.Customer != null && x.OrderTrans.Abbredemption.Abbregistration.Customer.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Abbredemption.Abbregistration.Customer != null && x.OrderTrans.Abbredemption.Abbregistration.Customer.City == custCity))
                               && (string.IsNullOrEmpty(companyName) || x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name == companyName)
                               && (string.IsNullOrEmpty(regdNo) || x.OrderTrans.RegdNo == regdNo)
                               );

                if (count > 0)
                {
                    TblOrderQcs = await _context.TblOrderQcs.Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Status)
                               .Where(x => x.IsActive == true && (tblBusinessUnit == null || tblBusinessUnit.Name == x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name)
                           && ((x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)) && x.Reschedulecount == 3)
                           && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null) || (x.ProposedQcdate >= resheduleStartDate && x.ProposedQcdate <= resheduleEndDate))
                               && (x.OrderTrans != null && x.OrderTrans.Abbredemption != null)
                               && (productCatId == null || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Id == productCatId))
                               && (productTypeId == null || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId))
                               && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Abbredemption.Abbregistration.Customer != null && x.OrderTrans.Abbredemption.Abbregistration.Customer.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Abbredemption.Abbregistration.Customer != null && x.OrderTrans.Abbredemption.Abbregistration.Customer.City == custCity))
                               && (string.IsNullOrEmpty(companyName) || x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name == companyName)
                               && (string.IsNullOrEmpty(regdNo) || x.OrderTrans.RegdNo == regdNo)
                               ).OrderByDescending(x => x.ModifiedDate).OrderByDescending(x => x.OrderQcid).Skip(skip).Take(pageSize).ToListAsync();
                }
                #endregion

                #region Set Pagination
                recordsTotal = count;
                #endregion

                #region Initialize Datatable
                QCCommentVMlist = new List<QCCommentViewModel>();
                if (TblOrderQcs != null && TblOrderQcs.Count > 0)
                {
                    string actionURL = string.Empty;
                    foreach (var item in TblOrderQcs)
                    {
                        qCCommentViewModel = new QCCommentViewModel();
                        if (item.OrderTrans != null && item.OrderTrans.OrderTransId > 0)
                        {
                            actionURL = " <div class='actionbtns'>";
                            actionURL = "<a href ='" + URL + "/ABBRedemption/Manage?regdNo=" + item.OrderTrans.RegdNo + "' ><button onclick='RecordView(" + item.OrderTrans.RegdNo + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                            actionURL = actionURL + " <a class='btn-sm btn-primary btn-sm' href='" + URL + "/Index1?orderTransId=" + item.OrderTrans.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                            actionURL = actionURL + "</div>";
                            qCCommentViewModel.Action = actionURL;
                            qCCommentViewModel.RegdNo = item.OrderTrans.RegdNo != null ? item.OrderTrans.RegdNo : string.Empty;
                            qCCommentViewModel.AbbRedemptionId = (int)(item.OrderTrans.AbbredemptionId != null ? item.OrderTrans.AbbredemptionId : 0);
                        }
                        if (item.OrderTrans.Abbredemption.Abbregistration.BusinessUnit != null)
                        {
                            qCCommentViewModel.CompanyName = item.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name != null ? item.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.CompanyName = "";
                        }
                        if (item.OrderTrans.Abbredemption.CustomerDetails != null)
                        {
                            qCCommentViewModel.CustFullname = item.OrderTrans.Abbredemption.CustomerDetails.FirstName + "" + item.OrderTrans.Abbredemption.CustomerDetails.LastName;
                            qCCommentViewModel.CustEmail = item.OrderTrans.Abbredemption.CustomerDetails.Email != null ? item.OrderTrans.Abbredemption.CustomerDetails.Email : string.Empty;
                            qCCommentViewModel.CustCity = item.OrderTrans.Abbredemption.CustomerDetails.City != null ? item.OrderTrans.Abbredemption.CustomerDetails.City : string.Empty;
                            qCCommentViewModel.ZipCode = item.OrderTrans.Abbredemption.CustomerDetails.ZipCode != null ? item.OrderTrans.Abbredemption.CustomerDetails.ZipCode : string.Empty;
                            qCCommentViewModel.CustAddress = item.OrderTrans.Abbredemption.CustomerDetails.Address1 + "" + item.OrderTrans.Abbredemption.CustomerDetails.Address2 != null ? item.OrderTrans.Abbredemption.CustomerDetails.Address2 : null;
                            qCCommentViewModel.CustPhoneNumber = item.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber != null ? item.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber : string.Empty;
                            qCCommentViewModel.CustState = item.OrderTrans.Abbredemption.CustomerDetails.State != null ? item.OrderTrans.Abbredemption.CustomerDetails.State : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.CustFullname = "";
                            qCCommentViewModel.CustEmail = "";
                            qCCommentViewModel.CustCity = "";
                            qCCommentViewModel.ZipCode = "";
                            qCCommentViewModel.CustAddress = "";
                            qCCommentViewModel.CustPhoneNumber = "";
                            qCCommentViewModel.CustState = "";
                        }
                        if (item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                        {
                            qCCommentViewModel.ProductCategory = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description != null ? item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.ProductCategory = "";
                        }
                        if (item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                        {
                            qCCommentViewModel.ProductType = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description != null ? item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.ProductType = "";
                        }
                        if (item.OrderTrans.Status != null)
                        {
                            qCCommentViewModel.StatusCode = item.OrderTrans.Status.StatusCode != null ? item.OrderTrans.Status.StatusCode : string.Empty;
                        }
                        else
                        {
                            qCCommentViewModel.StatusCode = "";
                        }
                        qCCommentViewModel.RescheduleDate = Convert.ToDateTime(item.ProposedQcdate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(item.ProposedQcdate).ToString("dd/MM/yyyy") : null;
                        qCCommentViewModel.Reschedulecount = (int)(item.Reschedulecount != null ? item.Reschedulecount : 0);
                        if (item.OrderTrans.Abbredemption != null)
                        {
                            qCCommentViewModel.OrderCreatedDate = item.OrderTrans.Abbredemption.CreatedDate != null ? item.OrderTrans.Abbredemption.CreatedDate : null;
                        }
                        QCCommentVMlist.Add(qCCommentViewModel);
                    }
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

        #region Canceled OdrerList added by Shashi For Status Code(0X,3X,3C,3Y,5X)
        [HttpPost]
        public async Task<ActionResult> CancelledOrderListNew(int companyId, DateTime? orderStartDate,
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


            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.InvoiceImageURL;
            string InvoiceimagURL = string.Empty;
            string ABBImagesURL = string.Empty;
            string actionURL1 = string.Empty;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            int count = 0;
            List<TblAbbredemption> tblAbbredemptionList = null;
            List<ABBRedemptionViewModel> aBBRedemptionViewModelList = null;
            ABBRedemptionViewModel aBBRedemptionViewModel = null;
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
                count = _context.TblAbbredemptions.Include(x => x.Status)
                               .Include(x => x.TblOrderTrans).ThenInclude(x => x.TblOrderQcs)
                               .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                               .Include(x => x.Abbregistration).ThenInclude(x => x.BusinessPartner)
                               .Include(x => x.CustomerDetails)
                               .Count(x => x.IsActive == true && (tblBusinessUnit == null || x.Abbregistration.BusinessUnit.Name == tblBusinessUnit.Name)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.CancelOrder) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentDeclined)
                               || x.StatusId == Convert.ToInt32(OrderStatusEnum.CustomerNotResponding_3C) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCFail_3Y)
                               || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCOrderCancel))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null)
                               || (x.TblOrderTrans.Count > 0 && x.TblOrderTrans.First().TblOrderQcs.Count > 0
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))
                               && (productCatId == null || (x.Abbregistration.NewProductCategory != null && x.Abbregistration.NewProductCategory.Id == productCatId))
                               && (productTypeId == null || (x.Abbregistration.NewProductCategoryTypeNavigation != null && x.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId))
                               && (string.IsNullOrEmpty(phoneNo) || (x.CustomerDetails != null && x.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.CustomerDetails != null && (x.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName)
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               );
                if (count > 0)
                {
                    tblAbbredemptionList = await _context.TblAbbredemptions.Include(x => x.Status)
                               .Include(x => x.TblOrderTrans).ThenInclude(x => x.TblOrderQcs)
                               .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                               .Include(x => x.Abbregistration).ThenInclude(x => x.BusinessPartner)
                               .Include(x => x.CustomerDetails)
                               .Where(x => x.IsActive == true && (tblBusinessUnit == null || x.Abbregistration.BusinessUnit.Name == tblBusinessUnit.Name)
                                && (x.StatusId == Convert.ToInt32(OrderStatusEnum.CancelOrder) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentDeclined)
                               || x.StatusId == Convert.ToInt32(OrderStatusEnum.CustomerNotResponding_3C) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCFail_3Y)
                               || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCOrderCancel))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null)
                               || (x.TblOrderTrans.Count > 0 && x.TblOrderTrans.First().TblOrderQcs.Count > 0
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                               && x.TblOrderTrans.First().TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))
                               && (productCatId == null || (x.Abbregistration.NewProductCategory != null && x.Abbregistration.NewProductCategory.Id == productCatId))
                               && (productTypeId == null || (x.Abbregistration.NewProductCategoryTypeNavigation != null && x.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId))
                               && (string.IsNullOrEmpty(phoneNo) || (x.CustomerDetails != null && x.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.CustomerDetails != null && (x.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName)
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.RedemptionId).Skip(skip).Take(pageSize).ToListAsync();
                }
                recordsTotal = count;
                #endregion

                #region ExchangeOrderViewModel Initialization for Datatable
                aBBRedemptionViewModelList = new List<ABBRedemptionViewModel>();
                if (tblAbbredemptionList != null && tblAbbredemptionList.Count > 0)
                {
                    TblOrderQc tblOrderQc = null;
                    foreach (var item in tblAbbredemptionList)
                    {
                        aBBRedemptionViewModel = new ABBRedemptionViewModel();
                        tblOrderTrans = _orderTransRepository.GetQcDetailsByABBRedemptionId(item.RedemptionId);
                        if (item != null)
                        {
                            aBBRedemptionViewModel.RedemptionId = item.RedemptionId;
                            aBBRedemptionViewModel.CreatedDate = item.CreatedDate;
                            actionURL = " <td class='actions'>";
                            actionURL = actionURL + " <span><input type='checkbox' id=" + item.RedemptionId + " name ='orders'  value ='" + item.RedemptionId + "'   onclick='OnCheckBoxCheck();' class='checkboxinput' /></span>";
                            actionURL = actionURL + " </td>";
                            aBBRedemptionViewModel.Action = actionURL;
                            aBBRedemptionViewModel.RegdNo = item.RegdNo;
                            TblCustomerDetail customerDetail = tblAbbredemptionList.FirstOrDefault(x => x.RedemptionId == item.RedemptionId && x.IsActive == true).CustomerDetails;
                            if (customerDetail == null)
                            {
                                customerDetail = new TblCustomerDetail();
                            }
                            else
                            {
                                aBBRedemptionViewModel.CustPinCode = customerDetail.ZipCode;
                                aBBRedemptionViewModel.CustFullname = customerDetail.FirstName + "" + customerDetail.LastName != null ? customerDetail.LastName : "";
                                aBBRedemptionViewModel.CustPhoneNumber = customerDetail.PhoneNumber;
                                aBBRedemptionViewModel.CustEmail = customerDetail.Email;
                                aBBRedemptionViewModel.CustAddress = customerDetail.Address1 + "" + customerDetail.Address2 != null ? customerDetail.Address2 : null;
                                aBBRedemptionViewModel.CustCity = customerDetail.City != null ? customerDetail.City : null;
                                aBBRedemptionViewModel.CustState = customerDetail.State != null ? customerDetail.State : null;
                            }
                            if (item.Abbregistration.BusinessUnit != null)
                            {
                                aBBRedemptionViewModel.CompanyName = item.Abbregistration.BusinessUnit.Name != null ? item.Abbregistration.BusinessUnit.Name : string.Empty;
                            }
                            else
                            {
                                aBBRedemptionViewModel.CompanyName = "";
                            }
                            if (item.Abbregistration.NewProductCategory != null)
                            {
                                aBBRedemptionViewModel.NewProductCategoryName = item.Abbregistration.NewProductCategory.Description != null ? item.Abbregistration.NewProductCategory.Description : string.Empty;
                            }
                            else
                            {
                                aBBRedemptionViewModel.NewProductCategoryName = "";
                            }
                            if (item.Abbregistration.NewProductCategory != null)
                            {
                                aBBRedemptionViewModel.NewProductCategoryType = item.Abbregistration.NewProductCategoryTypeNavigation.Description != null ? item.Abbregistration.NewProductCategoryTypeNavigation.Description : string.Empty;
                            }
                            else
                            {
                                aBBRedemptionViewModel.NewProductCategoryType = "";
                            }
                            if (item.Status != null)
                            {
                                aBBRedemptionViewModel.StatusCode = item.Status.StatusCode != null ? item.Status.StatusCode : string.Empty;
                            }
                            else
                            {
                                aBBRedemptionViewModel.StatusCode = "";
                            }

                            tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTrans.OrderTransId);
                            if (tblOrderQc != null)
                            {
                                aBBRedemptionViewModel.QCComments = tblOrderQc.Qccomments != null ? tblOrderQc.Qccomments : null;
                                aBBRedemptionViewModel.FinalRedemptionValue = tblOrderQc.PriceAfterQc != null ? null : tblOrderQc.PriceAfterQc;
                                aBBRedemptionViewModel.QcDate = tblOrderQc.Qcdate.ToString() != null ? tblOrderQc.Qcdate.ToString() : null;
                                aBBRedemptionViewModel.StatusId = (int)(tblOrderQc.StatusId != null ? tblOrderQc.StatusId : 0);
                                aBBRedemptionViewModel.CreatedBy = tblOrderQc.CreatedBy != null ? tblOrderQc.CreatedBy : null;
                                aBBRedemptionViewModel.CreatedDate = (DateTime)(tblOrderQc.CreatedDate != null ? tblOrderQc.CreatedDate : null);
                                aBBRedemptionViewModel.ModifiedBy = tblOrderQc.ModifiedBy != null ? tblOrderQc.ModifiedBy : null;
                                aBBRedemptionViewModel.ModifiedDateString = Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") : null;
                                aBBRedemptionViewModel.IsActive = tblOrderQc.IsActive != null ? tblOrderQc.IsActive : null;
                                if (tblOrderQc.ProposedQcdate != null)
                                {
                                    aBBRedemptionViewModel.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                    aBBRedemptionViewModel.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                                    aBBRedemptionViewModel.PreferredQCDate = tblOrderQc.ProposedQcdate != null ? tblOrderQc.ProposedQcdate : null;
                                    aBBRedemptionViewModel.PreferredQCDateString = Convert.ToDateTime(aBBRedemptionViewModel.PreferredQCDate).ToString("MM/dd/yyyy");
                                }
                            }

                            if (item.Abbregistration.BusinessPartner != null)
                            {
                                aBBRedemptionViewModel.StoreCode = item.Abbregistration.BusinessPartner.StoreCode != null ? item.Abbregistration.BusinessPartner.StoreCode : string.Empty;
                                aBBRedemptionViewModel.StoreName = item.Abbregistration.BusinessPartner.Name != null ? item.Abbregistration.BusinessPartner.Name : string.Empty;
                                aBBRedemptionViewModel.StorePhoneNumber = item.Abbregistration.BusinessPartner.PhoneNumber != null ? item.Abbregistration.BusinessPartner.PhoneNumber : string.Empty;
                            }
                            else
                            {
                                aBBRedemptionViewModel.StoreCode = "";
                                aBBRedemptionViewModel.StoreName = "";
                                aBBRedemptionViewModel.StorePhoneNumber = "";
                            }
                        }

                        actionURL1 = "<div class='actionbtns'>";
                        actionURL1 = actionURL1 + " <a href ='" + URL + "/ABBRedemption/Manage?regdNo=" + item.RegdNo + "' ><button onclick='RecordView(" + item.RedemptionId + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                        actionURL1 = actionURL1 + " <a class='btn-sm btn-primary btn-sm' href='" + URL + "/Index1?orderTransId=" + tblOrderTrans.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                        actionURL1 = actionURL1 + "</div>";
                        aBBRedemptionViewModel.Edit = actionURL1;

                        InvoiceimagURL = MVCURL + aBBRedemptionViewModel.InvoiceImageName;
                        ABBImagesURL = "<img src='" + InvoiceimagURL + "' class='img-responsive' />";
                        aBBRedemptionViewModel.InvoiceImageName = ABBImagesURL;
                        aBBRedemptionViewModelList.Add(aBBRedemptionViewModel);
                    }
                }
                else
                {
                    tblAbbredemptionList = new List<TblAbbredemption>();
                    aBBRedemptionViewModelList = new List<ABBRedemptionViewModel>();
                }
                #endregion
                var data = aBBRedemptionViewModelList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region QC Order List added by VK for ABB Redemption Start Date 27-June-2023

        #region QC Order List (Pending for QC) added by VK for new Requirment Status Code(0,3Q,3P,3R,0R,6 and 5R added as per team Discussion)
        [HttpPost]
        public async Task<ActionResult> ABBOrdersForQCList(int companyId, DateTime? orderStartDate,
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

            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            int count = 0;

            List<TblAbbredemption> tblAbbredemptionList = null;
            List<ABBRedemptionViewModel> aBBRedemptionVMList = null;
            List<TblOrderTran> tblOrderTransList = null;
            TblOrderTran tblOrderTrans = null;
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
                count = _context.TblOrderTrans.Include(x => x.Status)
                               .Include(x => x.TblOrderQcs)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Count(x => x.IsActive == true && x.Abbredemption != null && x.Abbredemption.Abbregistration != null && x.Abbredemption.Abbregistration.BusinessUnit != null
                               && (tblBusinessUnit == null || x.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCInProgress_3Q)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.CallAndGoScheduledAppointmentTaken_3P)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenOrder)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenforQC)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.InstalledbySponsor)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderWithDiagnostic))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null)
                               || (x.TblOrderQcs.Count > 0
                               && x.TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                               && x.TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))
                               && (productCatId == null || (x.Abbredemption.Abbregistration.NewProductCategoryId != null && x.Abbredemption.Abbregistration.NewProductCategoryId == productCatId))
                               && (productTypeId == null || x.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.Abbredemption.CustomerDetails != null && x.Abbredemption.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.Abbredemption.CustomerDetails != null && (x.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.Abbredemption.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName)
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               );
                if (count > 0)
                {
                    tblOrderTransList = await _context.TblOrderTrans.Include(x => x.Status)
                               .Include(x => x.TblOrderQcs)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Where(x => x.IsActive == true && x.Abbredemption != null && x.Abbredemption.Abbregistration != null && x.Abbredemption.Abbregistration.BusinessUnit != null
                               && (tblBusinessUnit == null || x.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCInProgress_3Q)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.CallAndGoScheduledAppointmentTaken_3P)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenOrder)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenforQC)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.InstalledbySponsor)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderWithDiagnostic))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null)
                               || (x.TblOrderQcs.Count > 0
                               && x.TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                               && x.TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))
                               && (productCatId == null || (x.Abbredemption.Abbregistration.NewProductCategoryId != null && x.Abbredemption.Abbregistration.NewProductCategoryId == productCatId))
                               && (productTypeId == null || x.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.Abbredemption.CustomerDetails != null && x.Abbredemption.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.Abbredemption.CustomerDetails != null && (x.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.Abbredemption.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName)
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.AbbredemptionId).Skip(skip).Take(pageSize).ToListAsync();

                    recordsTotal = count;
                }
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (tblOrderTransList != null && tblOrderTransList.Count > 0)
                {
                    tblAbbredemptionList = tblOrderTransList.Select(x => x.Abbredemption).ToList();
                    aBBRedemptionVMList = _mapper.Map<List<TblAbbredemption>, List<ABBRedemptionViewModel>>(tblAbbredemptionList);
                    string actionURL = string.Empty;

                    foreach (ABBRedemptionViewModel item in aBBRedemptionVMList)
                    {
                        #region Variables
                        TblCustomerDetail customerDetail = null;
                        TblOrderQc tblOrderQc = null;
                        #endregion

                        item.OrderCreatedDate = item.CreatedDate;
                        if (item.CreatedDate != null)
                        {
                            item.OrderCreatedDateString = Convert.ToDateTime(item.CreatedDate).ToString("MM/dd/yyyy H:mm:ss");
                        }
                        tblOrderTrans = tblOrderTransList.FirstOrDefault(x => x.RegdNo == item.RegdNo);
                        if (tblOrderTrans != null)
                        {
                            item.OrderTransId = tblOrderTrans.OrderTransId;
                            #region ABB common Configuraion Add by VK
                            if (tblOrderTrans.Abbredemption != null && tblOrderTrans.Abbredemption.Abbregistration != null)
                            {
                                regdNo = tblOrderTrans.RegdNo;
                                item.NewProductCategoryType = tblOrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation?.DescriptionForAbb;
                                item.NewProductCategoryName = tblOrderTrans.Abbredemption.Abbregistration.NewProductCategory?.DescriptionForAbb;
                                item.CompanyName = tblOrderTrans.Abbredemption.Abbregistration.BusinessUnit?.Name;
                                #region Fill Customer Details 
                                if (tblOrderTrans.Abbredemption.CustomerDetails != null)
                                {
                                    customerDetail = tblOrderTrans.Abbredemption.CustomerDetails;
                                    item.CustPinCode = customerDetail.ZipCode;
                                    item.CustFullname = customerDetail.FirstName + "" + customerDetail.LastName;
                                    item.CustPhoneNumber = customerDetail.PhoneNumber;
                                    item.CustEmail = customerDetail.Email;
                                    item.CustAddress = customerDetail.Address1 + "" + customerDetail.Address2 != null ? customerDetail.Address2 : null;
                                    item.CustCity = customerDetail.City != null ? customerDetail.City : null;
                                    item.CustState = customerDetail.State != null ? customerDetail.State : null;
                                }
                                #endregion
                            }
                            #endregion

                            #region Fill TblOrderQC Data
                            if (tblOrderTrans.TblOrderQcs.Count > 0)
                            {
                                tblOrderQc = tblOrderTrans.TblOrderQcs.FirstOrDefault();
                            }
                            if (tblOrderQc != null)
                            {
                                item.QCComments = tblOrderQc.Qccomments != null ? tblOrderQc.Qccomments : null;
                                item.FinalRedemptionValue = tblOrderQc.PriceAfterQc != null ? null : tblOrderQc.PriceAfterQc;
                                item.QcDate = tblOrderQc.Qcdate.ToString() != null ? tblOrderQc.Qcdate.ToString() : null;
                                item.StatusId = (int)(tblOrderQc.StatusId != null ? tblOrderQc.StatusId : 0);
                                item.CreatedBy = tblOrderQc.CreatedBy != null ? tblOrderQc.CreatedBy : null;
                                item.CreatedDate = (DateTime)(tblOrderQc.CreatedDate != null ? tblOrderQc.CreatedDate : null);
                                item.ModifiedBy = tblOrderQc.ModifiedBy != null ? tblOrderQc.ModifiedBy : null;
                                item.ModifiedDateString = Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") : null;
                                item.IsActive = tblOrderQc.IsActive != null ? tblOrderQc.IsActive : null;
                                if (tblOrderQc.ProposedQcdate != null)
                                {
                                    item.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                    item.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                                    item.PreferredQCDate = tblOrderQc.ProposedQcdate != null ? tblOrderQc.ProposedQcdate : null;
                                    item.PreferredQCDateString = Convert.ToDateTime(item.PreferredQCDate).ToString("MM/dd/yyyy");
                                }
                            }
                            #endregion

                            if (tblOrderTrans.Status != null)
                            {
                                item.StatusCode = tblOrderTrans.Status.StatusCode;
                            }
                        }

                        actionURL = " <div class='actionbtns'>";
                        actionURL = actionURL + " <a class='btn-sm btn-primary btn-sm' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>&nbsp;";
                        actionURL = actionURL + "<a href ='" + URL + "/ABBRedemption/Manage?regdNo=" + item.RegdNo + "'  class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";

                        #region Logic for Get Elepesed Hours Greater then 2 and Mark Red Flag
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
                        #endregion

                        actionURL = actionURL + "</div>";
                        item.Action = actionURL;
                    }
                }
                #endregion
                var data = aBBRedemptionVMList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Canceled QC(Reopen QC) OdrerList added by VK  For Status Code(5X)
        [HttpPost]
        public async Task<ActionResult> CancelledQCList(int companyId, DateTime? orderStartDate,
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

            List<TblAbbredemption> tblAbbredemptionList = null;
            List<ABBRedemptionViewModel> aBBRedemptionVMList = null;
            List<TblOrderTran> tblOrderTransList = null;
            TblOrderTran tblOrderTrans = null;

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

                #region tblOrderTransList obj Initialization and Orders Count
                count = _context.TblOrderTrans.Include(x => x.Status)
                               .Include(x => x.TblOrderQcs)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Count(x => x.IsActive == true && x.Abbredemption != null && x.Abbredemption.Abbregistration != null && x.Abbredemption.Abbregistration.BusinessUnit != null
                               && (tblBusinessUnit == null || x.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.QCOrderCancel))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null)
                               || (x.TblOrderQcs.Count > 0
                               && x.TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                               && x.TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))
                               && (productCatId == null || (x.Abbredemption.Abbregistration.NewProductCategoryId != null && x.Abbredemption.Abbregistration.NewProductCategoryId == productCatId))
                               && (productTypeId == null || x.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.Abbredemption.CustomerDetails != null && x.Abbredemption.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.Abbredemption.CustomerDetails != null && (x.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.Abbredemption.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName)
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               );
                if (count > 0)
                {
                    tblOrderTransList = await _context.TblOrderTrans.Include(x => x.Status)
                               .Include(x => x.TblOrderQcs)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Where(x => x.IsActive == true && x.Abbredemption != null && x.Abbredemption.Abbregistration != null && x.Abbredemption.Abbregistration.BusinessUnit != null
                               && (tblBusinessUnit == null || x.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.QCOrderCancel))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null)
                               || (x.TblOrderQcs.Count > 0
                               && x.TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                               && x.TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))
                               && (productCatId == null || (x.Abbredemption.Abbregistration.NewProductCategoryId != null && x.Abbredemption.Abbregistration.NewProductCategoryId == productCatId))
                               && (productTypeId == null || x.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.Abbredemption.CustomerDetails != null && x.Abbredemption.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.Abbredemption.CustomerDetails != null && (x.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.Abbredemption.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName)
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.OrderTransId).Skip(skip).Take(pageSize).ToListAsync();
                }
                recordsTotal = count;
                #endregion

                #region ABBRedemptionViewModel Initialization for Datatable
                if (tblOrderTransList != null && tblOrderTransList.Count > 0)
                {
                    tblAbbredemptionList = tblOrderTransList.Select(x => x.Abbredemption).ToList();
                    aBBRedemptionVMList = _mapper.Map<List<TblAbbredemption>, List<ABBRedemptionViewModel>>(tblAbbredemptionList);
                    foreach (ABBRedemptionViewModel item in aBBRedemptionVMList)
                    {
                        if (item != null)
                        {
                            #region Variables
                            TblCustomerDetail customerDetail = null;
                            TblOrderQc tblOrderQc = null;
                            #endregion

                            item.OrderCreatedDate = item.CreatedDate;
                            if (item.CreatedDate != null)
                            {
                                item.OrderCreatedDateString = Convert.ToDateTime(item.CreatedDate).ToString("MM/dd/yyyy H:mm:ss");
                            }
                            tblOrderTrans = tblOrderTransList.FirstOrDefault(x => x.RegdNo == item.RegdNo);
                            if (tblOrderTrans != null)
                            {
                                item.OrderTransId = tblOrderTrans.OrderTransId;
                                #region ABB common Configuraion Add by VK
                                if (tblOrderTrans.Abbredemption != null && tblOrderTrans.Abbredemption.Abbregistration != null)
                                {
                                    regdNo = tblOrderTrans.RegdNo;
                                    item.NewProductCategoryType = tblOrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation?.DescriptionForAbb;
                                    item.NewProductCategoryName = tblOrderTrans.Abbredemption.Abbregistration.NewProductCategory?.DescriptionForAbb;
                                    item.CompanyName = tblOrderTrans.Abbredemption.Abbregistration.BusinessUnit?.Name;
                                    #region Fill Customer Details 
                                    if (tblOrderTrans.Abbredemption.CustomerDetails != null)
                                    {
                                        customerDetail = tblOrderTrans.Abbredemption.CustomerDetails;
                                        item.CustPinCode = customerDetail.ZipCode;
                                        item.CustFullname = customerDetail.FirstName + "" + customerDetail.LastName;
                                        item.CustPhoneNumber = customerDetail.PhoneNumber;
                                        item.CustEmail = customerDetail.Email;
                                        item.CustAddress = customerDetail.Address1 + "" + customerDetail.Address2 != null ? customerDetail.Address2 : null;
                                        item.CustCity = customerDetail.City != null ? customerDetail.City : null;
                                        item.CustState = customerDetail.State != null ? customerDetail.State : null;
                                    }
                                    #endregion
                                }
                                #endregion

                                #region Fill TblOrderQC Data
                                if (tblOrderTrans.TblOrderQcs.Count > 0)
                                {
                                    tblOrderQc = tblOrderTrans.TblOrderQcs.FirstOrDefault();
                                }
                                if (tblOrderQc != null)
                                {
                                    item.QCComments = tblOrderQc.Qccomments != null ? tblOrderQc.Qccomments : null;
                                    item.FinalRedemptionValue = tblOrderQc.PriceAfterQc != null ? null : tblOrderQc.PriceAfterQc;
                                    item.QcDate = tblOrderQc.Qcdate.ToString() != null ? tblOrderQc.Qcdate.ToString() : null;
                                    item.StatusId = (int)(tblOrderQc.StatusId != null ? tblOrderQc.StatusId : 0);
                                    item.CreatedBy = tblOrderQc.CreatedBy != null ? tblOrderQc.CreatedBy : null;
                                    item.CreatedDate = (DateTime)(tblOrderQc.CreatedDate != null ? tblOrderQc.CreatedDate : null);
                                    item.ModifiedBy = tblOrderQc.ModifiedBy != null ? tblOrderQc.ModifiedBy : null;
                                    item.ModifiedDateString = Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") : null;
                                    item.IsActive = tblOrderQc.IsActive != null ? tblOrderQc.IsActive : null;
                                    if (tblOrderQc.ProposedQcdate != null)
                                    {
                                        item.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                        item.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                                        item.PreferredQCDate = tblOrderQc.ProposedQcdate != null ? tblOrderQc.ProposedQcdate : null;
                                        item.PreferredQCDateString = Convert.ToDateTime(item.PreferredQCDate).ToString("MM/dd/yyyy");
                                    }
                                }
                                #endregion

                                if (tblOrderTrans.Status != null)
                                {
                                    item.StatusCode = tblOrderTrans.Status.StatusCode;
                                }
                            }
                        }

                        #region Action Button for CheckBox
                        actionURL = " <td class='actions'>";
                        actionURL = actionURL + " <span><input type='checkbox' id=" + item.RedemptionId + " name ='orders'  value ='" + item.RedemptionId + "'   onclick='OnCheckBoxCheck();' class='checkboxinput' /></span>";
                        actionURL = actionURL + " </td>";
                        item.Action = actionURL;
                        #endregion

                        #region Action Button
                        actionURL1 = "<div class='actionbtns'>";
                        actionURL1 = actionURL1 + " <a href ='" + URL + "/ABBRedemption/Manage?regdNo=" + item.RegdNo + "' ><button onclick='RecordView(" + item.RegdNo + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                        // actionURL1 = actionURL1 + " <a href ='" + URL + "/Exchange/Manage?id=" + _protector.Encode(item.Id) + "' ><button onclick='RecordView(" + item.Id + ")' class='btn btn-primary btn-sm' data-bs-toggle='tooltip' data-bs-placement='top' title='Reopen'><i class='fa-solid fa-arrow-rotate-left'></i></button></a>";
                        actionURL1 = actionURL1 + " <a class='btn-sm btn-primary btn-sm' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                        actionURL1 = actionURL1 + "</div>";
                        item.Edit = actionURL1;
                        #endregion

                        InvoiceimagURL = MVCURL + item.InvoiceImageName;
                        var ABBImagesURL = "<img src='" + InvoiceimagURL + "' class='img-responsive' />";
                        item.InvoiceImageName = ABBImagesURL;
                    }
                }
                #endregion

                var data = aBBRedemptionVMList;
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

            List<TblAbbredemption> tblAbbredemptionList = null;
            List<ABBRedemptionViewModel> aBBRedemptionVMList = new List<ABBRedemptionViewModel>();
            List<TblOrderTran> tblOrderTransList = null;
            TblOrderTran tblOrderTrans = null;
            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
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

                #region table object Initialization
                tblOrderTransList = await _context.TblOrderTrans.Include(x => x.Status)
                               .Include(x => x.TblOrderQcs)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Where(x => x.IsActive == true && x.Abbredemption != null && x.Abbredemption.Abbregistration != null && x.Abbredemption.Abbregistration.BusinessUnit != null
                               && (tblBusinessUnit == null || x.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCByPass))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null)
                               || (x.TblOrderQcs.Count > 0
                               && x.TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                               && x.TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))
                               && (productCatId == null || (x.Abbredemption.Abbregistration.NewProductCategoryId != null && x.Abbredemption.Abbregistration.NewProductCategoryId == productCatId))
                               && (productTypeId == null || x.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.Abbredemption.CustomerDetails != null && x.Abbredemption.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.Abbredemption.CustomerDetails != null && (x.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.Abbredemption.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName)
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.OrderTransId).ToListAsync();
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (tblOrderTransList != null && tblOrderTransList.Count > 0)
                {
                    tblAbbredemptionList = tblOrderTransList.Select(x => x.Abbredemption).ToList();

                    List<ABBRedemptionViewModel> ABBRedemptionOrderListObj = new List<ABBRedemptionViewModel>();
                    ABBRedemptionOrderListObj = _mapper.Map<List<TblAbbredemption>, List<ABBRedemptionViewModel>>(tblAbbredemptionList);

                    string actionURL = string.Empty;
                    string actionURL1 = string.Empty;

                    foreach (ABBRedemptionViewModel item in ABBRedemptionOrderListObj)
                    {
                        #region Variables
                        TblCustomerDetail customerDetail = null;
                        TblOrderQc tblOrderQc = null;
                        int ElapsedHrs = 0;
                        #endregion

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
                                item.OrderCreatedDate = item.CreatedDate;
                                if (item.CreatedDate != null)
                                {
                                    item.OrderCreatedDateString = Convert.ToDateTime(item.CreatedDate).ToString("MM/dd/yyyy H:mm:ss");
                                }
                                tblOrderTrans = tblOrderTransList.FirstOrDefault(x => x.RegdNo == item.RegdNo);
                                if (tblOrderTrans != null)
                                {
                                    item.OrderTransId = tblOrderTrans.OrderTransId;
                                    #region ABB common Configuraion Add by VK
                                    if (tblOrderTrans.Abbredemption != null && tblOrderTrans.Abbredemption.Abbregistration != null)
                                    {
                                        regdNo = tblOrderTrans.RegdNo;
                                        item.NewProductCategoryType = tblOrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation?.DescriptionForAbb;
                                        item.NewProductCategoryName = tblOrderTrans.Abbredemption.Abbregistration.NewProductCategory?.DescriptionForAbb;
                                        item.CompanyName = tblOrderTrans.Abbredemption.Abbregistration.BusinessUnit?.Name;
                                        #region Fill Customer Details 
                                        if (tblOrderTrans.Abbredemption.CustomerDetails != null)
                                        {
                                            customerDetail = tblOrderTrans.Abbredemption.CustomerDetails;
                                            item.CustPinCode = customerDetail.ZipCode;
                                            item.CustFullname = customerDetail.FirstName + "" + customerDetail.LastName;
                                            item.CustPhoneNumber = customerDetail.PhoneNumber;
                                            item.CustEmail = customerDetail.Email;
                                            item.CustAddress = customerDetail.Address1 + "" + customerDetail.Address2 != null ? customerDetail.Address2 : null;
                                            item.CustCity = customerDetail.City != null ? customerDetail.City : null;
                                            item.CustState = customerDetail.State != null ? customerDetail.State : null;
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    #region Fill TblOrderQC Data
                                    if (tblOrderTrans.TblOrderQcs.Count > 0)
                                    {
                                        tblOrderQc = tblOrderTrans.TblOrderQcs.FirstOrDefault();
                                    }
                                    if (tblOrderQc != null)
                                    {
                                        item.QCComments = tblOrderQc.Qccomments != null ? tblOrderQc.Qccomments : null;
                                        item.FinalRedemptionValue = tblOrderQc.PriceAfterQc != null ? null : tblOrderQc.PriceAfterQc;
                                        item.QcDate = tblOrderQc.Qcdate.ToString() != null ? tblOrderQc.Qcdate.ToString() : null;
                                        item.StatusId = (int)(tblOrderQc.StatusId != null ? tblOrderQc.StatusId : 0);
                                        item.CreatedBy = tblOrderQc.CreatedBy != null ? tblOrderQc.CreatedBy : null;
                                        item.CreatedDate = (DateTime)(tblOrderQc.CreatedDate != null ? tblOrderQc.CreatedDate : null);
                                        item.ModifiedBy = tblOrderQc.ModifiedBy != null ? tblOrderQc.ModifiedBy : null;
                                        item.ModifiedDateString = Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") : null;
                                        item.IsActive = tblOrderQc.IsActive != null ? tblOrderQc.IsActive : null;
                                        if (tblOrderQc.ProposedQcdate != null)
                                        {
                                            item.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                            item.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                                            item.PreferredQCDate = tblOrderQc.ProposedQcdate != null ? tblOrderQc.ProposedQcdate : null;
                                            item.PreferredQCDateString = Convert.ToDateTime(item.PreferredQCDate).ToString("MM/dd/yyyy");
                                        }
                                    }
                                    #endregion

                                    if (tblOrderTrans.Status != null)
                                    {
                                        item.StatusCode = tblOrderTrans.Status.StatusCode;
                                    }
                                }

                                #region Action buttons
                                actionURL = " <div class='actionbtns'>";
                                actionURL += actionURL + "<button onclick='ResendUPILink(" + item.RedemptionId + ")' data-bs-toggle='tooltip' data-bs-placement='top' title='Resend UPI Verification Link' class='btn btn-primary btn-sm mx-1'>ResendLink</button>";
                                actionURL += "<a class='btn btn-primary btn-sm mx-1' target='_blank' href ='" + URL + "/PaymentDetails/ConfirmPaymentDetails?regdNo=" + item.RegdNo + "&userid=" + userid + "&AbbRedemptionid=" + item.RedemptionId + "&status=" + item.StatusId + "' title='Add UPI'>ADD UPI</a>&nbsp;";
                                actionURL += "<a href ='" + URL + "/ABBRedemption/Manage?regdNo=" + item.RegdNo + "' ><button onclick='RecordView(" + item.RegdNo + "," + ")' class='btn btn-primary btn-sm' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>&nbsp;";
                                actionURL += "<a class='btn btn-primary btn-sm' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                                actionURL += "</div>";
                                item.Action = actionURL;
                                #endregion

                                InvoiceimagURL = MVCURL + item.InvoiceImageName;
                                var ABBImagesURL = "<img src='" + InvoiceimagURL + "' class='img-responsive' />";
                                item.InvoiceImageName = ABBImagesURL;

                                item.LinksendDate = Convert.ToDateTime(complaintDate).ToString("MM/dd/yyyy H:mm:ss");

                                aBBRedemptionVMList.Add(item);
                            }
                        }
                    }
                }
                #endregion

                #region pagination
                recordsTotal = aBBRedemptionVMList != null ? aBBRedemptionVMList.Count : 0;
                if (aBBRedemptionVMList != null && aBBRedemptionVMList.Count > 0)
                {
                    aBBRedemptionVMList = aBBRedemptionVMList.Skip(skip).Take(pageSize).ToList();
                }
                #endregion

                var data = aBBRedemptionVMList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region QC FollowUp (with Flag 5W and 5Y) List(Price Quoted QC) After 48hrs added by VK
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

            List<TblAbbredemption> tblAbbredemptionList = null;
            List<ABBRedemptionViewModel> aBBRedemptionVMList = new List<ABBRedemptionViewModel>();
            List<TblOrderTran> tblOrderTransList = null;
            TblOrderTran tblOrderTrans = null;
            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
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

                #region table object Initialization
                tblOrderTransList = await _context.TblOrderTrans.Include(x => x.Status)
                               .Include(x => x.TblOrderQcs)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Where(x => x.IsActive == true && x.Abbredemption != null && x.Abbredemption.Abbregistration != null && x.Abbredemption.Abbregistration.BusinessUnit != null
                               && (tblBusinessUnit == null || x.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCByPass))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((resheduleStartDate == null && resheduleEndDate == null)
                               || (x.TblOrderQcs.Count > 0
                               && x.TblOrderQcs.First().ProposedQcdate >= resheduleStartDate
                               && x.TblOrderQcs.First().ProposedQcdate <= resheduleEndDate))
                               && (productCatId == null || (x.Abbredemption.Abbregistration.NewProductCategoryId != null && x.Abbredemption.Abbregistration.NewProductCategoryId == productCatId))
                               && (productTypeId == null || x.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId)
                               && (string.IsNullOrEmpty(phoneNo) || (x.Abbredemption.CustomerDetails != null && x.Abbredemption.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.Abbredemption.CustomerDetails != null && (x.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.Abbredemption.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName)
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.OrderTransId).ToListAsync();
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (tblOrderTransList != null && tblOrderTransList.Count > 0)
                {
                    tblAbbredemptionList = tblOrderTransList.Select(x => x.Abbredemption).ToList();

                    List<ABBRedemptionViewModel> ABBRedemptionOrderListObj = new List<ABBRedemptionViewModel>();
                    ABBRedemptionOrderListObj = _mapper.Map<List<TblAbbredemption>, List<ABBRedemptionViewModel>>(tblAbbredemptionList);

                    string actionURL = string.Empty;
                    string actionURL1 = string.Empty;

                    foreach (ABBRedemptionViewModel item in ABBRedemptionOrderListObj)
                    {
                        #region Variables
                        TblCustomerDetail customerDetail = null;
                        TblOrderQc tblOrderQc = null;
                        int ElapsedHrs = 0;
                        #endregion

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
                                item.OrderCreatedDate = item.CreatedDate;
                                if (item.CreatedDate != null)
                                {
                                    item.OrderCreatedDateString = Convert.ToDateTime(item.CreatedDate).ToString("MM/dd/yyyy H:mm:ss");
                                }
                                tblOrderTrans = tblOrderTransList.FirstOrDefault(x => x.RegdNo == item.RegdNo);
                                if (tblOrderTrans != null)
                                {
                                    item.OrderTransId = tblOrderTrans.OrderTransId;
                                    #region ABB common Configuraion Add by VK
                                    if (tblOrderTrans.Abbredemption != null && tblOrderTrans.Abbredemption.Abbregistration != null)
                                    {
                                        regdNo = tblOrderTrans.RegdNo;
                                        item.NewProductCategoryType = tblOrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation?.DescriptionForAbb;
                                        item.NewProductCategoryName = tblOrderTrans.Abbredemption.Abbregistration.NewProductCategory?.DescriptionForAbb;
                                        item.CompanyName = tblOrderTrans.Abbredemption.Abbregistration.BusinessUnit?.Name;
                                        #region Fill Customer Details 
                                        if (tblOrderTrans.Abbredemption.CustomerDetails != null)
                                        {
                                            customerDetail = tblOrderTrans.Abbredemption.CustomerDetails;
                                            item.CustPinCode = customerDetail.ZipCode;
                                            item.CustFullname = customerDetail.FirstName + "" + customerDetail.LastName;
                                            item.CustPhoneNumber = customerDetail.PhoneNumber;
                                            item.CustEmail = customerDetail.Email;
                                            item.CustAddress = customerDetail.Address1 + "" + customerDetail.Address2 != null ? customerDetail.Address2 : null;
                                            item.CustCity = customerDetail.City != null ? customerDetail.City : null;
                                            item.CustState = customerDetail.State != null ? customerDetail.State : null;
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    #region Fill TblOrderQC Data
                                    if (tblOrderTrans.TblOrderQcs.Count > 0)
                                    {
                                        tblOrderQc = tblOrderTrans.TblOrderQcs.FirstOrDefault();
                                    }
                                    if (tblOrderQc != null)
                                    {
                                        item.QCComments = tblOrderQc.Qccomments != null ? tblOrderQc.Qccomments : null;
                                        item.FinalRedemptionValue = tblOrderQc.PriceAfterQc != null ? null : tblOrderQc.PriceAfterQc;
                                        item.QcDate = tblOrderQc.Qcdate.ToString() != null ? tblOrderQc.Qcdate.ToString() : null;
                                        item.StatusId = (int)(tblOrderQc.StatusId != null ? tblOrderQc.StatusId : 0);
                                        item.CreatedBy = tblOrderQc.CreatedBy != null ? tblOrderQc.CreatedBy : null;
                                        item.CreatedDate = (DateTime)(tblOrderQc.CreatedDate != null ? tblOrderQc.CreatedDate : null);
                                        item.ModifiedBy = tblOrderQc.ModifiedBy != null ? tblOrderQc.ModifiedBy : null;
                                        item.ModifiedDateString = Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") != null ? Convert.ToDateTime(tblOrderQc.ModifiedDate).ToString("dd/MM/yyyy") : null;
                                        item.IsActive = tblOrderQc.IsActive != null ? tblOrderQc.IsActive : null;
                                        if (tblOrderQc.ProposedQcdate != null)
                                        {
                                            item.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                            item.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                                            item.PreferredQCDate = tblOrderQc.ProposedQcdate != null ? tblOrderQc.ProposedQcdate : null;
                                            item.PreferredQCDateString = Convert.ToDateTime(item.PreferredQCDate).ToString("MM/dd/yyyy");
                                        }
                                    }
                                    #endregion

                                    if (tblOrderTrans.Status != null)
                                    {
                                        item.StatusCode = tblOrderTrans.Status.StatusCode;
                                    }
                                }

                                #region Action Button for CheckBoxes
                                actionURL = " <td class='actions'>";
                                actionURL = actionURL + " <span><input type='checkbox' id=" + item.RedemptionId + " name ='orders'  value ='" + item.RedemptionId + "'   onclick='OnCheckBoxCheck();' class='checkboxinput' /></span>";
                                actionURL = actionURL + " </td>";
                                item.Action = actionURL;
                                #endregion

                                #region Action buttons
                                actionURL1 = " <div class='actionbtns'>";
                                actionURL1 += "<a class='btn btn-primary btn-sm mx-1' target='_blank' href ='" + URL + "/PaymentDetails/ConfirmPaymentDetails?regdNo=" + item.RegdNo + "&userid=" + userid + "&AbbRedemptionid=" + item.RedemptionId + "&status=" + item.StatusId + "' title='Add UPI'>ADD UPI</a>&nbsp;";
                                actionURL1 += "<a href ='" + URL + "/ABBRedemption/Manage?id=" + _protector.Encode(item.RedemptionId) + "' ><button onclick='RecordView(" + item.RedemptionId + ")' class='btn btn-primary btn-sm' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>&nbsp;";
                                actionURL1 += " <a class='btn-sm btn-primary btn-sm' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                                actionURL1 += "</div>";
                                item.Edit = actionURL1;
                                #endregion

                                InvoiceimagURL = MVCURL + item.InvoiceImageName;
                                var ABBImagesURL = "<img src='" + InvoiceimagURL + "' class='img-responsive' />";
                                item.InvoiceImageName = ABBImagesURL;

                                item.LinksendDate = Convert.ToDateTime(complaintDate).ToString("MM/dd/yyyy H:mm:ss");

                                aBBRedemptionVMList.Add(item);
                            }
                        }
                    }
                }
                #endregion

                #region pagination
                recordsTotal = aBBRedemptionVMList != null ? aBBRedemptionVMList.Count : 0;
                if (aBBRedemptionVMList != null && aBBRedemptionVMList.Count > 0)
                {
                    aBBRedemptionVMList = aBBRedemptionVMList.Skip(skip).Take(pageSize).ToList();
                }
                #endregion

                var data = aBBRedemptionVMList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #endregion

        #region Self Qc List for ratio reporting of ABB Orders done by Customer added by Pratibha
        [HttpPost]
        public async Task<ActionResult> ABBListOfSelfQCByCustomer(int companyId, DateTime? orderStartDate,
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

                count = _context.TblOrderTrans.Include(x => x.Abbredemption).ThenInclude(x => x.TblSelfQcs).ThenInclude(x => x.User)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Status)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                        .Where(x => x.IsActive == true
                        && (tblBusinessUnit == null || x.Abbredemption.Abbregistration.BusinessUnit.Name == tblBusinessUnit.Name)
                        && ((orderStartDate == null && orderEndDate == null) || (x.Abbredemption.CreatedDate >= orderStartDate && x.Abbredemption.CreatedDate <= orderEndDate))
                        && (productCatId == null || (x.Abbredemption.Abbregistration.NewProductCategory != null && x.Abbredemption.Abbregistration.NewProductCategory.Id == productCatId))
                        && (productTypeId == null || x.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId)
                        && (string.IsNullOrEmpty(phoneNo) || (x.Abbredemption.CustomerDetails != null && x.Abbredemption.CustomerDetails.PhoneNumber == phoneNo))
                        && (string.IsNullOrEmpty(custCity) || (x.Abbredemption.CustomerDetails != null && (x.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity))
                        && (string.IsNullOrEmpty(emailId) || x.Abbredemption.TblSelfQcs.Any(sq => sq.User.Email != null && sq.User.Email.ToLower() == emailId.ToLower()))
                        && (string.IsNullOrEmpty(companyName) || (x.Abbredemption.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName)
                        && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                        && x.Abbredemption.TblOrderTrans.Any(ot => ot.SelfQclinkResendby == null)
                        && x.Abbredemption.TblOrderTrans.Any(ot => ot.TblExchangeAbbstatusHistories.Any(eash => eash.StatusId == (int)OrderStatusEnum.SelfQCbyCustomer))
                        && x.Abbredemption.TblSelfQcs.Any(sq => sq.User.UserId == 3)).Count();

                if (count > 0)
                {
                    tblOrderTran = _context.TblOrderTrans.Include(x => x.Abbredemption).ThenInclude(x => x.TblSelfQcs).ThenInclude(x => x.User)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Status)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                        .Where(x => x.IsActive == true
                        && (tblBusinessUnit == null || x.Abbredemption.Abbregistration.BusinessUnit.Name == tblBusinessUnit.Name)
                        && ((orderStartDate == null && orderEndDate == null) || (x.Abbredemption.CreatedDate >= orderStartDate && x.Abbredemption.CreatedDate <= orderEndDate))
                        && (productCatId == null || (x.Abbredemption.Abbregistration.NewProductCategory != null && x.Abbredemption.Abbregistration.NewProductCategory.Id == productCatId))
                        && (productTypeId == null || x.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId)
                        && (string.IsNullOrEmpty(phoneNo) || (x.Abbredemption.CustomerDetails != null && x.Abbredemption.CustomerDetails.PhoneNumber == phoneNo))
                        && (string.IsNullOrEmpty(custCity) || (x.Abbredemption.CustomerDetails != null && (x.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity))
                        && (string.IsNullOrEmpty(emailId) || x.Abbredemption.TblSelfQcs.Any(sq => sq.User.Email != null && sq.User.Email.ToLower() == emailId.ToLower()))
                        && (string.IsNullOrEmpty(companyName) || (x.Abbredemption.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName)
                        && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                        && x.Abbredemption.TblOrderTrans.Any(ot => ot.SelfQclinkResendby == null)
                        && x.Abbredemption.TblOrderTrans.Any(ot => ot.TblExchangeAbbstatusHistories.Any(eash => eash.StatusId == (int)OrderStatusEnum.SelfQCbyCustomer))
                        && x.Abbredemption.TblSelfQcs.Any(sq => sq.User.UserId == 3)).ToList();


                    tblOrderTran = tblOrderTran.OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.Abbredemption.RedemptionId).Skip(skip).Take(pageSize).ToList();
                    recordsTotal = count;
                    #region Data Initialization for Datatable from table to Model
                    if (tblOrderTran != null && tblOrderTran.Count > 0)
                    {
                        string actionURL = string.Empty;
                        foreach (var item in tblOrderTran)
                        {
                            ratioReportSelfQcViewModel = new RatioReportSelfQcViewModel();
                            ratioReportSelfQcViewModel.Id = item.Abbredemption.RedemptionId != null ? item.Abbredemption.RedemptionId : 0;
                            ratioReportSelfQcViewModel.CompanyName = item.Abbredemption.Abbregistration.BusinessUnit.Name != null ? item.Abbredemption.Abbregistration.BusinessUnit.Name : "";
                            ratioReportSelfQcViewModel.RegdNo = item.RegdNo != null ? item.RegdNo : "";
                            ratioReportSelfQcViewModel.ProductCategory = item.Abbredemption.Abbregistration.NewProductCategory.Description != null ? item.Abbredemption.Abbregistration.NewProductCategory.Description : "";
                            ratioReportSelfQcViewModel.OrderCreatedDate = Convert.ToDateTime(item.CreatedDate).ToString("MM/dd/yyyy H:mm:ss");
                            ratioReportSelfQcViewModel.CustFullname = item.Abbredemption.CustomerDetails.FirstName + "" + item.Abbredemption.CustomerDetails.LastName;
                            ratioReportSelfQcViewModel.CustAddress = item.Abbredemption.CustomerDetails.Address1 + "" + item.Abbredemption.CustomerDetails.Address2;
                            ratioReportSelfQcViewModel.CustPincode = item.Abbredemption.CustomerDetails.ZipCode != null ? item.Abbredemption.CustomerDetails.ZipCode : "";
                            ratioReportSelfQcViewModel.CustCity = item.Abbredemption.CustomerDetails.City != null ? item.Abbredemption.CustomerDetails.City : "";
                            ratioReportSelfQcViewModel.CustState = item.Abbredemption.CustomerDetails.State != null ? item.Abbredemption.CustomerDetails.State : "";
                            ratioReportSelfQcViewModel.StatusCode = item.Abbredemption.Status.StatusCode != null ? item.Abbredemption.Status.StatusCode : "";
                            ratioReportSelfQcViewModel.UserEmailId = SecurityHelper.DecryptString(item.Abbredemption.TblSelfQcs.First().User.Email, keyString);

                            actionURL = " <div class='actionbtns'>";
                            actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>&nbsp;";
                            actionURL = actionURL + "<a href ='" + URL + "/ABBRedemption/Manage?regdNo=" + item.RegdNo + "' onclick='RecordView(" + item.RegdNo + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
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


        #region Self Qc List for ratio reporting of ABB Orders done by Internal Team added by Pratibha
        [HttpPost]
        public async Task<ActionResult> ABBListOfSelfQCByInternalTeam(int companyId, DateTime? orderStartDate,
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

                count = _context.TblOrderTrans.Include(x => x.Abbredemption).ThenInclude(x => x.TblSelfQcs).ThenInclude(x => x.User)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Status)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                        .Where(x => x.IsActive == true
                        && (tblBusinessUnit == null || x.Abbredemption.Abbregistration.BusinessUnit.Name == tblBusinessUnit.Name)
                        && ((orderStartDate == null && orderEndDate == null) || (x.Abbredemption.CreatedDate >= orderStartDate && x.Abbredemption.CreatedDate <= orderEndDate))
                        && (productCatId == null || (x.Abbredemption.Abbregistration.NewProductCategory != null && x.Abbredemption.Abbregistration.NewProductCategory.Id == productCatId))
                        && (productTypeId == null || x.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId)
                        && (string.IsNullOrEmpty(phoneNo) || (x.Abbredemption.CustomerDetails != null && x.Abbredemption.CustomerDetails.PhoneNumber == phoneNo))
                        && (string.IsNullOrEmpty(custCity) || (x.Abbredemption.CustomerDetails != null && (x.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity))
                        && (string.IsNullOrEmpty(emailId) || x.Abbredemption.TblSelfQcs.Any(sq => sq.User.Email != null && sq.User.Email.ToLower() == emailId.ToLower()))
                        && (string.IsNullOrEmpty(companyName) || (x.Abbredemption.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName)
                        && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                        && x.Abbredemption.TblOrderTrans.Any(ot => ot.TblExchangeAbbstatusHistories.Any(eash => eash.StatusId == (int)OrderStatusEnum.SelfQCbyCustomer))
                        && ((x.Abbredemption.TblSelfQcs.Any(sq => sq.User.UserId == 3) && x.Abbredemption.TblOrderTrans.Any(ot => ot.SelfQclinkResendby != null)) || (x.Abbredemption.TblSelfQcs.Any(sq => sq.User.UserId != 3) && x.Abbredemption.TblOrderTrans.Any(ot => ot.SelfQclinkResendby != null)) || (x.Abbredemption.TblSelfQcs.Any(sq => sq.User.UserId != 3) && x.Abbredemption.TblOrderTrans.Any(ot => ot.SelfQclinkResendby == null)))).Count();


                if (count > 0)
                {

                    tblOrderTran = _context.TblOrderTrans.Include(x => x.Abbredemption).ThenInclude(x => x.TblSelfQcs).ThenInclude(x => x.User)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Status)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                        .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                        .Where(x => x.IsActive == true
                        && (tblBusinessUnit == null || x.Abbredemption.Abbregistration.BusinessUnit.Name == tblBusinessUnit.Name)
                        && ((orderStartDate == null && orderEndDate == null) || (x.Abbredemption.CreatedDate >= orderStartDate && x.Abbredemption.CreatedDate <= orderEndDate))
                        && (productCatId == null || (x.Abbredemption.Abbregistration.NewProductCategory != null && x.Abbredemption.Abbregistration.NewProductCategory.Id == productCatId))
                        && (productTypeId == null || x.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Id == productTypeId)
                        && (string.IsNullOrEmpty(phoneNo) || (x.Abbredemption.CustomerDetails != null && x.Abbredemption.CustomerDetails.PhoneNumber == phoneNo))
                        && (string.IsNullOrEmpty(custCity) || (x.Abbredemption.CustomerDetails != null && (x.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity))
                        && (string.IsNullOrEmpty(emailId) || x.Abbredemption.TblSelfQcs.Any(sq => sq.User.Email != null && sq.User.Email.ToLower() == emailId.ToLower()))
                        && (string.IsNullOrEmpty(companyName) || (x.Abbredemption.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName)
                        && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                        && x.Abbredemption.TblOrderTrans.Any(ot => ot.TblExchangeAbbstatusHistories.Any(eash => eash.StatusId == (int)OrderStatusEnum.SelfQCbyCustomer))
                        && ((x.Abbredemption.TblSelfQcs.Any(sq => sq.User.UserId == 3) && x.Abbredemption.TblOrderTrans.Any(ot => ot.SelfQclinkResendby != null)) || (x.Abbredemption.TblSelfQcs.Any(sq => sq.User.UserId != 3) && x.Abbredemption.TblOrderTrans.Any(ot => ot.SelfQclinkResendby != null)) || (x.Abbredemption.TblSelfQcs.Any(sq => sq.User.UserId != 3) && x.Abbredemption.TblOrderTrans.Any(ot => ot.SelfQclinkResendby == null)))).ToList();

                    tblOrderTran = tblOrderTran.OrderByDescending(x => x.Abbredemption.ModifiedDate).ThenByDescending(x => x.Abbredemption.RedemptionId).Skip(skip).Take(pageSize).ToList();
                    recordsTotal = count;
                    #region Data Initialization for Datatable from table to Model
                    if (tblOrderTran != null && tblOrderTran.Count > 0)
                    {
                        string actionURL = string.Empty;
                        TblOrderQc tblOrderQc = null;

                        foreach (var item in tblOrderTran)
                        {
                            ratioReportSelfQcViewModel = new RatioReportSelfQcViewModel();
                            ratioReportSelfQcViewModel.Id = item.Abbredemption.RedemptionId != null ? item.Abbredemption.RedemptionId : 0;
                            ratioReportSelfQcViewModel.CompanyName = item.Abbredemption.Abbregistration.BusinessUnit.Name != null ? item.Abbredemption.Abbregistration.BusinessUnit.Name : "";
                            ratioReportSelfQcViewModel.RegdNo = item.RegdNo != null ? item.RegdNo : "";
                            ratioReportSelfQcViewModel.ProductCategory = item.Abbredemption.Abbregistration.NewProductCategory.Description != null ? item.Abbredemption.Abbregistration.NewProductCategory.Description : "";
                            ratioReportSelfQcViewModel.OrderCreatedDate = Convert.ToDateTime(item.CreatedDate).ToString("MM/dd/yyyy H:mm:ss");
                            ratioReportSelfQcViewModel.CustFullname = item.Abbredemption.CustomerDetails.FirstName + "" + item.Abbredemption.CustomerDetails.LastName;
                            ratioReportSelfQcViewModel.CustAddress = item.Abbredemption.CustomerDetails.Address1 + "" + item.Abbredemption.CustomerDetails.Address2;
                            ratioReportSelfQcViewModel.CustPincode = item.Abbredemption.CustomerDetails.ZipCode != null ? item.Abbredemption.CustomerDetails.ZipCode : "";
                            ratioReportSelfQcViewModel.CustCity = item.Abbredemption.CustomerDetails.City != null ? item.Abbredemption.CustomerDetails.City : "";
                            ratioReportSelfQcViewModel.CustState = item.Abbredemption.CustomerDetails.State != null ? item.Abbredemption.CustomerDetails.State : "";
                            ratioReportSelfQcViewModel.StatusCode = item.Abbredemption.Status.StatusCode != null ? item.Abbredemption.Status.StatusCode : "";
                            ratioReportSelfQcViewModel.UserEmailId = SecurityHelper.DecryptString(item.Abbredemption.TblSelfQcs.First().User.Email, keyString);

                            actionURL = " <div class='actionbtns'>";
                            actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>&nbsp;";
                            actionURL = actionURL + "<a href ='" + URL + "/ABBRedemption/Manage?regdNo=" + item.RegdNo + "' onclick='RecordView(" + item.RegdNo + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
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



    }
}
