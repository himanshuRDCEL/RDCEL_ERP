using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Enum;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model;
using RDCELERP.Model.Base;
using RDCELERP.Model.Company;
using RDCELERP.Model.DriverDetails;
using RDCELERP.Model.EVC_Allocated;
using RDCELERP.Model.EVC_Portal;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.LGC;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.LGC;
using RDCELERP.Model.ServicePartner;
using RDCELERP.Model.TicketGenrateModel;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;
using static Org.BouncyCastle.Math.EC.ECCurve;
using static RDCELERP.Model.ABBRedemption.LoVViewModel;
using ResponseData = RDCELERP.Model.TicketGenrateModel.ResponseData;

namespace RDCELERP.BAL.MasterManager
{
    public class LogisticManager : ILogisticManager
    {
        #region variable declartion
        IExchangeOrderRepository _exchangeOrderRepository;
        IServicePartnerRepository _servicePartnerRepository;
        IOrderTransRepository _orderTransRepository;
        IOrderImageUploadRepository _orderImageUploadRepository;
        IImageLabelRepository _imageLabelRepository;
        IProductTypeRepository _productTypeRepository;
        IOrderLGCRepository _orderLGCRepository;
        IWebHostEnvironment _webHostEnvironment;
        ILovRepository _lovRepository;
        IMapper _mapper;
        IImageHelper _imageHelper;
        ILogging _logging;
        IEVCPODDetailsRepository _evcPoDDetailsRepository;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        ILogisticsRepository _logisticsRepository;
        IExchangeOrderStatusRepository _exchangeOrderStatusRepository;
        IHtmlToPDFConverterHelper _htmlToPdfConverterHelper;
        IExchangeABBStatusHistoryRepository _exchangeABBStatusHistoryRepository;
        IWalletTransactionRepository _walletTransactionRepository;
        IEVCWalletHistoryRepository _eVCWalletHistoryRepository;
        IEVCRepository _eVCRepository;
        IDriverDetailsRepository _driverDetailsRepository;
        Digi2l_DevContext _digi2L_DevContext;
        public readonly IOptions<ApplicationSettings> _baseConfig;
        IRoleRepository _roleRepository;
        IUserRoleRepository _userRoleRepository;
        ICityRepository _cityRepository;
        IBusinessPartnerRepository _businessPartnerRepository;
        IVoucherRepository _voucherRepository;
        private DAL.Entities.Digi2l_DevContext _context;
        ICommonManager _commonManager;
        ITicketGenrateManager _ticketGenrateManager;
        IABBRedemptionRepository _abbRedemptionRepository;
        IAbbRegistrationRepository _abbRegistrationRepository;
        IBusinessUnitRepository _businessUnitRepository;
        IAreaLocalityRepository _AreaLocalityRepository;

        IVehicleJourneyTrackingRepository _vehicleJourneyTrackingRepository;
        IVehicleJourneyTrackingDetailsRepository _vehicleJourneyTrackingDetails;
        IEVCPartnerRepository _evcpartnerRepository;
        #endregion

        #region Constructor
        public LogisticManager(IExchangeOrderRepository exchangeOrderRepository, ILogging logging, DAL.Entities.Digi2l_DevContext context, IServicePartnerRepository servicePartnerRepository, ILovRepository lovRepository, IOrderImageUploadRepository orderImageUploadRepository, IMapper mapper, IOrderTransRepository orderTransRepository, IEVCPODDetailsRepository evcPoDDetailsRepository, IOrderLGCRepository orderLGC, IImageLabelRepository imageLabelRepository, IProductTypeRepository productTypeRepository, IImageHelper imageHelper, IWebHostEnvironment webHostEnvironment, ILogisticsRepository logisticsRepository, IExchangeOrderStatusRepository exchangeOrderStatusRepository, IHtmlToPDFConverterHelper htmlToPdfConverterHelper, IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository, IWalletTransactionRepository walletTransactionRepository, IEVCWalletHistoryRepository eVCWalletHistoryRepository, IEVCRepository eVCRepository, Digi2l_DevContext digi2L_DevContext, IDriverDetailsRepository driverDetailsRepository, IOptions<ApplicationSettings> baseConfig, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, ICityRepository cityRepository, IBusinessPartnerRepository businessPartnerRepository, IVoucherRepository voucherRepository, ICommonManager commonManager, ITicketGenrateManager ticketGenrateManager, IABBRedemptionRepository abbRedemptionRepository, IAbbRegistrationRepository abbRegistrationRepository, IBusinessUnitRepository businessUnitRepository, IAreaLocalityRepository areaLocalityRepository, IVehicleJourneyTrackingRepository vehicleJourneyTrackingRepository, IVehicleJourneyTrackingDetailsRepository vehicleJourneyTrackingDetailsRepository, IEVCPartnerRepository evcpartnerRepository)
        {
            _exchangeOrderRepository = exchangeOrderRepository;
            _logging = logging;
            _servicePartnerRepository = servicePartnerRepository;
            _lovRepository = lovRepository;
            _orderImageUploadRepository = orderImageUploadRepository;
            _mapper = mapper;
            _orderTransRepository = orderTransRepository;
            _evcPoDDetailsRepository = evcPoDDetailsRepository;
            _orderLGCRepository = orderLGC;
            _imageLabelRepository = imageLabelRepository;
            _productTypeRepository = productTypeRepository;
            _imageHelper = imageHelper;
            _webHostEnvironment = webHostEnvironment;
            _logisticsRepository = logisticsRepository;
            _exchangeOrderStatusRepository = exchangeOrderStatusRepository;
            _htmlToPdfConverterHelper = htmlToPdfConverterHelper;
            _exchangeABBStatusHistoryRepository = exchangeABBStatusHistoryRepository;
            _walletTransactionRepository = walletTransactionRepository;
            _eVCWalletHistoryRepository = eVCWalletHistoryRepository;
            _eVCRepository = eVCRepository;
            _digi2L_DevContext = digi2L_DevContext;
            _driverDetailsRepository = driverDetailsRepository;
            _baseConfig = baseConfig;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _cityRepository = cityRepository;
            _businessPartnerRepository = businessPartnerRepository;
            _voucherRepository = voucherRepository;
            _context = context;
            _commonManager = commonManager;
            _ticketGenrateManager = ticketGenrateManager;
            _abbRedemptionRepository = abbRedemptionRepository;
            _abbRegistrationRepository = abbRegistrationRepository;
            _businessUnitRepository = businessUnitRepository;
            _AreaLocalityRepository = areaLocalityRepository;
            _vehicleJourneyTrackingRepository = vehicleJourneyTrackingRepository;
            _vehicleJourneyTrackingDetails = vehicleJourneyTrackingDetailsRepository;
            _evcpartnerRepository = evcpartnerRepository;
        }
        
        public LogisticManager()
        {

        }
        #endregion

        #region get details of service partner
        /// <summary>
        /// get details of service partner
        /// </summary>
        /// <returns>tblServicePartner<LGCOrderViewModel></returns>
        public TblServicePartner GetServicePartnerDetails(int userId)
        {
            TblServicePartner tblServicePartner = null;
            try
            {
                tblServicePartner = _servicePartnerRepository.GetServicePartnerByUserId(userId);
                if (tblServicePartner != null)
                {
                    return tblServicePartner;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "GetServicePartnerDetails", ex);
            }
            return tblServicePartner;
        }
        #endregion

        #region get image uplodedby LGC at time of pickup
        /// <summary>
        /// get image uplodedby LGC at time of pickup
        /// </summary>
        /// <param name="regdNo"></param>
        /// <returns>orderImageUploadVMList</returns>
        public IList<OrderImageUploadViewModel> GetImagesUploadedFromLGCPickup(string regdNo)
        {
            string imageUplodedBy = "Pickup";
            List<OrderImageUploadViewModel> orderImageUploadViewModels = null;
            List<TblOrderImageUpload> tblOrderImageUpload = new List<TblOrderImageUpload>();
            try
            {
                TblOrderTran tblOrderTran = _orderTransRepository.GetSingle(x => x.IsActive == true && x.RegdNo == regdNo);
                if (tblOrderTran != null)
                {
                    TblLoV tblLoV = _lovRepository.GetSingle(x => x.IsActive == true && x.LoVname.ToLower().Equals(imageUplodedBy.ToLower()));
                    if (tblLoV != null)
                    {
                        tblOrderImageUpload = _orderImageUploadRepository.GetList(x => x.IsActive == true && x.ImageUploadby == tblLoV.ParentId && x.LgcpickDrop == tblLoV.LoVname && x.OrderTransId == tblOrderTran.OrderTransId).ToList();
                        orderImageUploadViewModels = _mapper.Map<List<TblOrderImageUpload>, List<OrderImageUploadViewModel>>(tblOrderImageUpload);
                        return orderImageUploadViewModels;
                    }
                    return orderImageUploadViewModels;

                }
                return orderImageUploadViewModels;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "GetImagesUploadedFromLGCPickup", ex);
            }
            return orderImageUploadViewModels;
        }
        #endregion

        #region Manage LGC Drop Methods and update drop status on all associated tables
        /// <summary>
        /// Create POD, DebitNote and update drop status on all respective tables
        /// </summary>
        /// <param name="podVM"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public bool SaveLGCDropStatus(PODViewModel podVM, int loggedUserId)
        {
            #region Variable Declaration
            List<TblOrderLgc>? tblOrderLgcListAll = null;
            bool flag = false;
            //Generate Invoice
            int MaxInvSrNum = 0;
            int InvSrNumFromConfig = 0;
            string FinancialYear = "";
            List<TblConfiguration>? tblConfigurationList = null;
            TblEvcpoddetail? tblEvcpoddetailObj = null;
            List<TblOrderLgc>? NewList = null;
            List <TblOrderLgc>? ExistingList = null;
            #endregion

            try
            {
                if (podVM != null && podVM.DriverId > 0 && podVM.EVCRegistrationId > 0 && podVM.EvcPartnerId > 0)
                {
                    tblOrderLgcListAll = _orderLGCRepository.GetOrderLGCListByDriverIdEVCPId(podVM.DriverId, podVM.EvcPartnerId)
                        .Where(x => x.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup)).ToList();

                    ExistingList = tblOrderLgcListAll.Where(x=>x.OrderTrans.TblWalletTransactions.All(x=>x.IsOrderAmtWithSweetener == null || x.IsOrderAmtWithSweetener == false)).ToList();
                    NewList = tblOrderLgcListAll.Where(x => x.OrderTrans.TblWalletTransactions.All(x => x.IsOrderAmtWithSweetener != null && x.IsOrderAmtWithSweetener == true)).ToList();

                    #region Code for Get Data from TblConfiguration
                    tblConfigurationList = _digi2L_DevContext.TblConfigurations.Where(x => x.IsActive == true).ToList();
                    if (tblConfigurationList != null && tblConfigurationList.Count > 0)
                    {
                        // MaxInvSrNum = tblEvcpoddetail.Max(x => x.InvSrNum).Value;
                        var startInvoiceSrNum = tblConfigurationList.FirstOrDefault(x => x.Name == ConfigurationEnum.StartInvoiceSrNum.ToString());
                        if (startInvoiceSrNum != null && startInvoiceSrNum.Value != null)
                        {
                            InvSrNumFromConfig = Convert.ToInt32(startInvoiceSrNum.Value.Trim());
                        }
                        var financialYear = tblConfigurationList.FirstOrDefault(x => x.Name == ConfigurationEnum.FinancialYear.ToString());
                        if (financialYear != null && financialYear.Value != null)
                        {
                            FinancialYear = financialYear.Value.Trim();
                        }
                    }
                    podVM.FinancialYear = FinancialYear;
                    #endregion

                    #region Code for get Max InvSrNum from TblEvcpoddetails
                    // tblEvcpoddetail = _evcPoDDetailsRepository.GetList(x => x.IsActive == true && x.InvSrNum != null && x.InvSrNum > 0).ToList();
                    tblEvcpoddetailObj = _evcPoDDetailsRepository.GetList(x => x.IsActive == true && x.InvSrNum != null && x.InvSrNum != null && x.InvSrNum > 0 && x.FinancialYear == FinancialYear).OrderByDescending(x => x.InvSrNum).FirstOrDefault();

                    if (tblEvcpoddetailObj != null && tblEvcpoddetailObj.InvSrNum <= InvSrNumFromConfig)
                    {
                        MaxInvSrNum = InvSrNumFromConfig;
                    }
                    else if (tblEvcpoddetailObj != null && tblEvcpoddetailObj.InvSrNum >= InvSrNumFromConfig)
                    {
                        MaxInvSrNum = Convert.ToInt32(tblEvcpoddetailObj.InvSrNum);
                    }
                    else if (tblEvcpoddetailObj == null && InvSrNumFromConfig > 0)
                    {
                        MaxInvSrNum = InvSrNumFromConfig;
                    }
                    podVM.MaxInvSrNum = MaxInvSrNum;
                    #endregion

                    if (NewList != null && NewList.Count > 0)
                    {
                        podVM.IsOrderAmtWithSweetener = true;
                        podVM.GstType = GeneralConstant.GSTExclusiveLabel;
                        flag = GenerateAndStorePDFData(NewList, podVM, loggedUserId);
                    }
                    if (ExistingList != null && ExistingList.Count > 0)
                    {
                        podVM.IsOrderAmtWithSweetener = false;
                        podVM.GstType = GeneralConstant.GSTInclusiveLabel;
                        flag = GenerateAndStorePDFData(ExistingList, podVM, loggedUserId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "SaveLGCDropStatus", ex);
            }
            return flag;
        }

        #region Old Data 
        //public bool SaveLGCDropStatus2(PODViewModel podVM, int loggedUserId)
        //{
        //    #region Variable Declaration
        //    List<TblOrderLgc>? tblOrderLgcListAll = null;
        //    podVM.podVMList = new List<PODViewModel>();
        //    PODViewModel? podVMtemp = null;
        //    bool flag = false;
        //    string? podfilePath = null;
        //    string? podHtmlString = null;
        //    bool podPDFSave = false;
        //    string? podPdfName = null;
        //    decimal? finalAmountDN = 0;
        //    //Generate Invoice
        //    int MaxInvSrNum = 0;
        //    int InvSrNumFromConfig = 0;
        //    string FinancialYear = "";
        //    decimal? finalAmountInv = 0;
        //    List<TblConfiguration>? tblConfigurationList = null;
        //    decimal totalSweetenerAmt = 0;
        //    decimal totalLgcCost = 0;
        //    decimal totalCgst = 0;
        //    decimal totalSgst = 0;
        //    //Generate Invoice

        //    TblEvcpoddetail? tblEvcpoddetailObj = null;

        //    #endregion

        //    OrderLGCViewModel orderLGC = new OrderLGCViewModel();
        //    try
        //    {
        //        if (podVM != null && podVM.DriverId > 0 && podVM.EVCRegistrationId > 0 && podVM.EvcPartnerId > 0)
        //        {
        //            #region Code for Get Data from TblConfiguration
        //            tblConfigurationList = _digi2L_DevContext.TblConfigurations.Where(x => x.IsActive == true).ToList();
        //            if (tblConfigurationList != null && tblConfigurationList.Count > 0)
        //            {
        //                // MaxInvSrNum = tblEvcpoddetail.Max(x => x.InvSrNum).Value;
        //                var startInvoiceSrNum = tblConfigurationList.FirstOrDefault(x => x.Name == ConfigurationEnum.StartInvoiceSrNum.ToString());
        //                if (startInvoiceSrNum != null && startInvoiceSrNum.Value != null)
        //                {
        //                    InvSrNumFromConfig = Convert.ToInt32(startInvoiceSrNum.Value.Trim());
        //                }
        //                var financialYear = tblConfigurationList.FirstOrDefault(x => x.Name == ConfigurationEnum.FinancialYear.ToString());
        //                if (financialYear != null && financialYear.Value != null)
        //                {
        //                    FinancialYear = financialYear.Value.Trim();
        //                }
        //            }
        //            #endregion

        //            #region Code for get Max InvSrNum from TblEvcpoddetails
        //            // tblEvcpoddetail = _evcPoDDetailsRepository.GetList(x => x.IsActive == true && x.InvSrNum != null && x.InvSrNum > 0).ToList();
        //            tblEvcpoddetailObj = _evcPoDDetailsRepository.GetList(x => x.IsActive == true && x.InvSrNum != null && x.InvSrNum != null && x.InvSrNum > 0 && x.FinancialYear == FinancialYear).OrderByDescending(x => x.InvSrNum).FirstOrDefault();

        //            if (tblEvcpoddetailObj != null && tblEvcpoddetailObj.InvSrNum <= InvSrNumFromConfig)
        //            {
        //                MaxInvSrNum = InvSrNumFromConfig;
        //            }
        //            else if (tblEvcpoddetailObj != null && tblEvcpoddetailObj.InvSrNum >= InvSrNumFromConfig)
        //            {
        //                MaxInvSrNum = Convert.ToInt32(tblEvcpoddetailObj.InvSrNum);
        //            }
        //            else if (tblEvcpoddetailObj == null && InvSrNumFromConfig > 0)
        //            {
        //                MaxInvSrNum = InvSrNumFromConfig;
        //            }
        //            #endregion

        //            tblOrderLgcListAll = _orderLGCRepository.GetOrderLGCListByDriverIdEVCPId(podVM.DriverId, podVM.EvcPartnerId)
        //                .Where(x => x.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup)).ToList();

        //            if (tblOrderLgcListAll != null && tblOrderLgcListAll.Count > 0)
        //            {
        //                var totalOrderRecords = 0;
        //                int pageSize = 8;
        //                int skip = 0;
        //                int totalPages = 1;
        //                int reminder = 0;

        //                #region if Orders List is greater than [pageSize] records per invoice
        //                totalOrderRecords = tblOrderLgcListAll.Count;
        //                if (totalOrderRecords > pageSize)
        //                {
        //                    totalPages = Convert.ToInt32(totalOrderRecords / pageSize);
        //                    reminder = Convert.ToInt32(totalOrderRecords % pageSize);
        //                    if (reminder > 0) { totalPages++; }
        //                }

        //                for (int i = 1; i <= totalPages; i++)
        //                {
        //                    podVM.DNOrderCount = 0; podVM.InvOrderCount = 0;
        //                    finalAmountDN = 0; finalAmountInv = 0; bool DropCompleted = false; bool PostedCompleted = false;
        //                    podVM.podVMList = new List<PODViewModel>();
        //                    MaxInvSrNum++;
        //                    skip = (i - 1) * pageSize;
        //                    List<TblOrderLgc> orderList = tblOrderLgcListAll.Skip(skip).Take(pageSize).ToList();

        //                    #region Set Counter Sr. Number 
        //                    podVM.BillCounterNum = String.Format("{0:D6}", MaxInvSrNum);
        //                    podPdfName = "POD-" + FinancialYear.Replace("/", "-") + "-" + podVM.BillCounterNum + ".pdf";
        //                    podVM.PoDPdfName = podPdfName;
        //                    #endregion

        //                    #region Initialize POD and Debit Note Data 
        //                    foreach (var item1 in orderList)
        //                    {
        //                        if (item1 != null && item1.Evcregistration != null && item1.Evcpartner != null && item1.Logistic != null && item1.OrderTrans != null
        //                            && (item1.OrderTrans.Exchange != null || item1.OrderTrans.Abbredemption != null) && item1.OrderTrans.TblWalletTransactions != null && item1.OrderTrans.TblWalletTransactions.Count > 0)
        //                        {
        //                            var tblWalletTransObj = item1.OrderTrans.TblWalletTransactions.FirstOrDefault(x => x.OrderTransId == item1.OrderTransId);
        //                            if (tblWalletTransObj != null)
        //                            {
        //                                #region Variable Declaration
        //                                TblExchangeOrder? tblExchangeOrder = null; TblCustomerDetail? tblCustomerDetail = null;
        //                                TblAbbredemption? tblAbbredemption = null;
        //                                TblAbbregistration? tblAbbregistration = null; bool IsFEPWithoutSweetener = false; decimal SweetenerAmt = 0;
        //                                string? productTypeDesc = null; string? productCatDesc = null; decimal FinalExchPriceWithoutSweetner = 0;
        //                                podVMtemp = new PODViewModel();
        //                                #endregion

        //                                #region Common Implementations for (ABB or Exchange)
        //                                if (item1.OrderTrans.OrderType == Convert.ToInt32(LoVEnum.Exchange))
        //                                {
        //                                    tblExchangeOrder = item1.OrderTrans.Exchange;
        //                                    tblCustomerDetail = tblExchangeOrder?.CustomerDetails;
        //                                    productTypeDesc = tblExchangeOrder?.ProductType?.Name;
        //                                    productCatDesc = tblExchangeOrder?.ProductType?.ProductCat?.Name;
        //                                    SweetenerAmt = Convert.ToDecimal(tblExchangeOrder?.Sweetener);
        //                                    #region Condition based set Final Exchange Price
        //                                    IsFEPWithoutSweetener = Convert.ToBoolean(tblExchangeOrder?.IsFinalExchangePriceWithoutSweetner ?? false);
        //                                    if (IsFEPWithoutSweetener)
        //                                    {
        //                                        FinalExchPriceWithoutSweetner = Convert.ToDecimal(tblExchangeOrder?.FinalExchangePrice);
        //                                    }
        //                                    else
        //                                    {
        //                                        FinalExchPriceWithoutSweetner = Convert.ToDecimal(tblExchangeOrder?.FinalExchangePrice) - Convert.ToDecimal(tblExchangeOrder?.Sweetener);
        //                                    }
        //                                    #endregion
        //                                }
        //                                else if (item1.OrderTrans.OrderType == Convert.ToInt32(LoVEnum.ABB))
        //                                {
        //                                    tblAbbredemption = item1.OrderTrans.Abbredemption;
        //                                    tblAbbregistration = tblAbbredemption?.Abbregistration;
        //                                    tblCustomerDetail = item1.OrderTrans.Abbredemption?.CustomerDetails;
        //                                    productTypeDesc = tblAbbregistration?.NewProductCategoryTypeNavigation?.Name;
        //                                    productCatDesc = tblAbbregistration?.NewProductCategory?.Name;
        //                                    FinalExchPriceWithoutSweetner = Convert.ToDecimal(item1.OrderTrans?.FinalPriceAfterQc);
        //                                }
        //                                #endregion

        //                                #region Initialize the Common Details
        //                                podVMtemp.EVCBussinessName = item1.Evcregistration.BussinessName + "-" + item1.Evcpartner.EvcStoreCode;
        //                                podVMtemp.EVCRegdNo = item1.Evcregistration.EvcregdNo;
        //                                podVMtemp.ServicePartnerName = item1.Logistic.ServicePartner.ServicePartnerName;
        //                                podVMtemp.TicketNo = podVMtemp.ServicePartnerName + "-" + item1.Logistic.TicketNumber;
        //                                podVMtemp.RegdNo = item1.Logistic.RegdNo;
        //                                podVMtemp.ProductCatName = productCatDesc;
        //                                if (tblCustomerDetail != null)
        //                                {
        //                                    podVMtemp.CustName = tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName;
        //                                    podVMtemp.CustPincode = tblCustomerDetail.ZipCode;
        //                                }
        //                                podVMtemp.Podurl = EnumHelper.DescriptionAttr(FileAddressEnum.EVCPoD) + podPdfName;
        //                                #endregion

        //                                #region Check BusinessUnit configuration
        //                                podVMtemp.IsDebitNoteSkiped = _commonManager.CheckBUCongigByKey(item1.OrderTransId, BUConfigKeyEnum.IsDebitNoteSkiped.ToString());
        //                                #endregion

        //                                #region EVC Based Configuration for Invoice and Debit Note
        //                                decimal orderAmt = tblWalletTransObj.OrderAmount ?? 0;

        //                                #region Bulk Liquidation
        //                                if (Convert.ToBoolean(podVMtemp.IsDebitNoteSkiped ?? false))
        //                                {
        //                                    podVM.InvOrderCount = ++podVM.InvOrderCount;
        //                                    podVMtemp.OrderAmtForEVCInv = Convert.ToDecimal(orderAmt);
        //                                }
        //                                else
        //                                {
        //                                    podVM.DNOrderCount = ++podVM.DNOrderCount;
        //                                    podVM.InvOrderCount = ++podVM.InvOrderCount;
        //                                    podVMtemp.OrderAmtForEVCDN = FinalExchPriceWithoutSweetner; //item1.OrderTrans.TblWalletTransactions.FirstOrDefault(x=>x.OrderTransId == item1.OrderTransId).OrderAmount;
        //                                    finalAmountDN += Convert.ToDecimal(podVMtemp.OrderAmtForEVCDN);
        //                                    podVMtemp.OrderAmtForEVCInv = Convert.ToDecimal(orderAmt) - Convert.ToDecimal(FinalExchPriceWithoutSweetner);
        //                                }
        //                                #endregion

        //                                if (tblWalletTransObj.IsOrderAmtWithSweetener != null)
        //                                {
        //                                    podVMtemp.SweetenerAmt = 0;

        //                                    #region Debit Note and Invoice Change Algo V2
        //                                    if (tblWalletTransObj.IsOrderAmtWithSweetener == true)
        //                                    {
        //                                        if (!IsFEPWithoutSweetener && tblWalletTransObj.IsOrderAmtWithSweetener == true)
        //                                        {
        //                                            finalAmountDN += SweetenerAmt;
        //                                        }
        //                                        podVMtemp.SweetenerAmt = tblWalletTransObj.SweetenerAmt;
        //                                        totalSweetenerAmt += tblWalletTransObj.SweetenerAmt ?? 0;
        //                                        podVMtemp.Lgccost = tblWalletTransObj.Lgccost;
        //                                        totalLgcCost += tblWalletTransObj.Lgccost ?? 0;
        //                                    }
        //                                    #endregion

        //                                    podVMtemp.BaseValue = tblWalletTransObj.BaseValue;
        //                                    podVMtemp.Cgstamt = tblWalletTransObj.Cgstamt;
        //                                    podVMtemp.Sgstamt = tblWalletTransObj.Sgstamt;
        //                                    totalCgst += tblWalletTransObj.Cgstamt ?? 0;
        //                                    totalSgst += tblWalletTransObj.Sgstamt ?? 0;
        //                                    //podVM.IsOrderAmtWithSweetener = true;
        //                                }
        //                                else
        //                                {
        //                                    #region Get Base Amount and GST Calculation according to V1
        //                                    decimal? BaseAmount = 0;
        //                                    bool isGSTInclusive = true;
        //                                    if (podVMtemp.OrderAmtForEVCInv > 0)
        //                                    {
        //                                        if (isGSTInclusive)
        //                                        {
        //                                            BaseAmount = podVMtemp.OrderAmtForEVCInv / Convert.ToDecimal(GeneralConstant.GSTPercentage);
        //                                        }
        //                                        else
        //                                        {
        //                                            BaseAmount = podVMtemp.OrderAmtForEVCInv;
        //                                        }
        //                                        BaseAmount = Math.Round((BaseAmount ?? 0), 2);
        //                                        podVMtemp.OrderAmtForEVCInv = Math.Round((podVMtemp.OrderAmtForEVCInv ?? 0), 2);
        //                                        decimal? GstAmount = BaseAmount * Convert.ToDecimal(GeneralConstant.CGST);
        //                                        GstAmount = Math.Round((GstAmount ?? 0), 2);

        //                                        podVMtemp.BaseValue = BaseAmount;
        //                                        podVMtemp.Cgstamt = GstAmount;
        //                                        podVMtemp.Sgstamt = GstAmount;
        //                                        totalCgst += GstAmount ?? 0;
        //                                        //podVM.IsOrderAmtWithSweetener = true;
        //                                    }
        //                                    #endregion
        //                                }
        //                                finalAmountInv += Convert.ToDecimal(podVMtemp.OrderAmtForEVCInv);
        //                                podVM.podVMList.Add(podVMtemp);
        //                                #endregion
        //                            }
        //                        }
        //                    }
        //                    #endregion

        //                    if (podVM != null && podVM.podVMList != null && podVM.podVMList.Count > 0)
        //                    {
        //                        #region Generate PoD
        //                        podVM.FinancialYear = FinancialYear;
        //                        podVM.InvSrNum = MaxInvSrNum;
        //                        podfilePath = EnumHelper.DescriptionAttr(FilePathEnum.EVCPoD);
        //                        podHtmlString = GetPoDHtmlString(podVM, "POD");
        //                        podPDFSave = _htmlToPdfConverterHelper.GeneratePDF(podHtmlString, podfilePath, podPdfName);
        //                        #endregion

        //                        #region Generate and Save Debit Note
        //                        if (podVM.DNOrderCount > 0)
        //                        {
        //                            podVM.evcDetailsVM.FinalPriceDN = finalAmountDN;
        //                            podVM.evcDetailsVM.TotalSweetenerAmt = totalSweetenerAmt;
        //                            DropCompleted = GenerateAndSaveDebitNote(orderList, podVM, loggedUserId);
        //                        }
        //                        #endregion

        //                        #region Generate and Save Invoice Pdf
        //                        if (podVM.InvOrderCount > 0)
        //                        {
        //                            podVM.evcDetailsVM.FinalPriceInv = finalAmountInv;
        //                            podVM.evcDetailsVM.TotalCgstamt = totalCgst;
        //                            podVM.evcDetailsVM.TotalSgstamt = totalSgst;
        //                            podVM.evcDetailsVM.TotalLgccost = totalLgcCost;
        //                            flag = GenerateAndSaveInvoice(orderList, podVM, loggedUserId);
        //                        }
        //                        #endregion
        //                    }
        //                }
        //                #endregion
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.WriteErrorToDB("LogisticManager", "SaveLGCDropStatus", ex);
        //    }
        //    return flag;
        //}
        #endregion


        public bool GenerateAndStorePDFData(List<TblOrderLgc> tblOrderLgcListAll, PODViewModel podVM, int loggedUserId)
        {
            #region Variable Declaration
            podVM.podVMList = new List<PODViewModel>();
            PODViewModel? podVMtemp = null;
            bool flag = false;
            string? podfilePath = null;
            string? podHtmlString = null;
            bool podPDFSave = false;
            string? podPdfName = null;
            decimal? finalAmountDN = 0;
            decimal? finalAmountInv = 0;
            decimal totalSweetenerAmt = 0;
            int totalLgcCost = 0;
            decimal totalCgst = 0;
            decimal totalSgst = 0;
            decimal totalPrimeProductAmt = 0;
            #endregion

            try
            {
                if (podVM != null && podVM.DriverId > 0 && podVM.EVCRegistrationId > 0 && podVM.EvcPartnerId > 0)
                {
                    int MaxInvSrNum = podVM.MaxInvSrNum ?? 0;
                    string? FinancialYear = podVM?.FinancialYear;
                    if (tblOrderLgcListAll != null && tblOrderLgcListAll.Count > 0)
                    {
                        #region Set Pagination for Generate PDF
                        var totalOrderRecords = 0;
                        int pageSize = 8;
                        int skip = 0;
                        int totalPages = 1;
                        int reminder = 0;
                        totalOrderRecords = tblOrderLgcListAll.Count;
                        if (totalOrderRecords > pageSize)
                        {
                            totalPages = Convert.ToInt32(totalOrderRecords / pageSize);
                            reminder = Convert.ToInt32(totalOrderRecords % pageSize);
                            if (reminder > 0) { totalPages++; }
                        }
                        #endregion

                        for (int i = 1; i <= totalPages; i++)
                        {
                            podVM.DNOrderCount = 0; podVM.InvOrderCount = 0; totalSweetenerAmt = 0; totalLgcCost = 0;
                            totalCgst = 0; totalSgst = 0; totalPrimeProductAmt = 0;
                            finalAmountDN = 0; finalAmountInv = 0; bool DropCompleted = false; bool PostedCompleted = false;
                            podVM.podVMList = new List<PODViewModel>();
                            MaxInvSrNum++;
                            //insert + field 
                            //carry id
                            podVM.MaxInvSrNum++;
                            skip = (i - 1) * pageSize;
                            List<TblOrderLgc> orderList = tblOrderLgcListAll.Skip(skip).Take(pageSize).ToList();

                            #region Set Counter Sr. Number 
                            podVM.BillCounterNum = String.Format("{0:D6}", MaxInvSrNum);
                            podPdfName = "POD-" + FinancialYear?.Replace("/", "-") + "-" + podVM.BillCounterNum + ".pdf";
                            podVM.PoDPdfName = podPdfName;
                            #endregion

                            #region Initialize POD and Debit Note Data 
                            foreach (var item1 in orderList)
                            {
                                if (item1 != null && item1.Evcregistration != null && item1.Evcpartner != null && item1.Logistic != null && item1.OrderTrans != null
                                    && (item1.OrderTrans.Exchange != null || item1.OrderTrans.Abbredemption != null) && item1.OrderTrans.TblWalletTransactions != null && item1.OrderTrans.TblWalletTransactions.Count > 0)
                                {
                                    var tblWalletTransObj = item1.OrderTrans.TblWalletTransactions.FirstOrDefault(x => x.OrderTransId == item1.OrderTransId);
                                    if (tblWalletTransObj != null)
                                    {
                                        #region Variable Declaration
                                        TblExchangeOrder? tblExchangeOrder = null; TblCustomerDetail? tblCustomerDetail = null;
                                        TblAbbredemption? tblAbbredemption = null;
                                        TblAbbregistration? tblAbbregistration = null; bool IsFEPWithoutSweetener = false; decimal SweetenerAmt = 0;
                                        string? productTypeDesc = null; string? productCatDesc = null; decimal FinalExchPriceWithoutSweetner = 0;
                                        podVMtemp = new PODViewModel();
                                        #endregion

                                        #region Common Implementations for (ABB or Exchange)
                                        if (item1.OrderTrans.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                                        {
                                            tblExchangeOrder = item1.OrderTrans.Exchange;
                                            tblCustomerDetail = tblExchangeOrder?.CustomerDetails;
                                            productTypeDesc = tblExchangeOrder?.ProductType?.Name;
                                            productCatDesc = tblExchangeOrder?.ProductType?.ProductCat?.Name;
                                            SweetenerAmt = Convert.ToDecimal(tblExchangeOrder?.Sweetener);
                                            #region Condition based set Final Exchange Price
                                            IsFEPWithoutSweetener = Convert.ToBoolean(tblExchangeOrder?.IsFinalExchangePriceWithoutSweetner ?? false);
                                            if (IsFEPWithoutSweetener)
                                            {
                                                FinalExchPriceWithoutSweetner = Convert.ToDecimal(tblExchangeOrder?.FinalExchangePrice);
                                            }
                                            else
                                            {
                                                FinalExchPriceWithoutSweetner = Convert.ToDecimal(tblExchangeOrder?.FinalExchangePrice) - Convert.ToDecimal(tblExchangeOrder?.Sweetener);
                                            }
                                            #endregion
                                        }
                                        else if (item1.OrderTrans.OrderType == Convert.ToInt32(LoVEnum.ABB))
                                        {
                                            tblAbbredemption = item1.OrderTrans.Abbredemption;
                                            tblAbbregistration = tblAbbredemption?.Abbregistration;
                                            tblCustomerDetail = item1.OrderTrans.Abbredemption?.CustomerDetails;
                                            productTypeDesc = tblAbbregistration?.NewProductCategoryTypeNavigation?.Name;
                                            productCatDesc = tblAbbregistration?.NewProductCategory?.Name;
                                            FinalExchPriceWithoutSweetner = Convert.ToDecimal(item1.OrderTrans?.FinalPriceAfterQc);
                                        }
                                        #endregion

                                        #region Initialize the Common Details
                                        podVMtemp.EVCBussinessName = item1.Evcregistration.BussinessName + "-" + item1.Evcpartner.EvcStoreCode;
                                        podVMtemp.EVCRegdNo = item1.Evcregistration.EvcregdNo;
                                        podVMtemp.ServicePartnerName = item1.Logistic.ServicePartner.ServicePartnerName;
                                        podVMtemp.TicketNo = podVMtemp.ServicePartnerName + "-" + item1.Logistic.TicketNumber;
                                        podVMtemp.RegdNo = item1.Logistic.RegdNo;
                                        podVMtemp.ProductCatName = productCatDesc;
                                        if (tblCustomerDetail != null)
                                        {
                                            podVMtemp.CustName = tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName;
                                            podVMtemp.CustPincode = tblCustomerDetail.ZipCode;
                                        }
                                        podVMtemp.Podurl = EnumHelper.DescriptionAttr(FileAddressEnum.EVCPoD) + podPdfName;
                                        #endregion

                                        #region Check BusinessUnit configuration Bulk Liquidation
                                        podVMtemp.IsDebitNoteSkiped = _commonManager.CheckBUCongigByKey(item1.OrderTransId, BUConfigKeyEnum.IsDebitNoteSkiped.ToString());
                                        #endregion

                                        #region EVC Based Configuration for Invoice and Debit Note
                                        decimal orderAmt = tblWalletTransObj.OrderAmount ?? 0;
                                        #region Bulk Liquidation
                                        if (Convert.ToBoolean(podVMtemp.IsDebitNoteSkiped ?? false))
                                        {
                                            podVM.InvOrderCount = ++podVM.InvOrderCount;
                                            podVMtemp.OrderAmtForEVCInv = Convert.ToDecimal(orderAmt);
                                        }
                                        else
                                        {
                                            podVM.DNOrderCount = ++podVM.DNOrderCount;
                                            podVM.InvOrderCount = ++podVM.InvOrderCount;
                                            podVMtemp.OrderAmtForEVCDN = FinalExchPriceWithoutSweetner; //item1.OrderTrans.TblWalletTransactions.FirstOrDefault(x=>x.OrderTransId == item1.OrderTransId).OrderAmount;
                                            finalAmountDN += Convert.ToDecimal(podVMtemp.OrderAmtForEVCDN);
                                            podVMtemp.OrderAmtForEVCInv = Convert.ToDecimal(orderAmt) - Convert.ToDecimal(FinalExchPriceWithoutSweetner);
                                        }
                                        #endregion

                                        if (tblWalletTransObj.IsOrderAmtWithSweetener != null)
                                        {
                                            podVMtemp.SweetenerAmt = 0; var isGSTInclusive = true;

                                            #region Debit Note and Invoice Revised Algo V2
                                            if (tblWalletTransObj.IsOrderAmtWithSweetener == true)
                                            {
                                                if (!IsFEPWithoutSweetener && tblWalletTransObj.IsOrderAmtWithSweetener == true)
                                                {
                                                    finalAmountDN += SweetenerAmt;
                                                    podVMtemp.OrderAmtForEVCInv = podVMtemp.OrderAmtForEVCInv - (tblWalletTransObj?.SweetenerAmt??0);
                                                }
                                                podVMtemp.OrderAmtForEVCInv = podVMtemp.OrderAmtForEVCInv - (tblWalletTransObj?.Lgccost ?? 0);
                                                podVMtemp.SweetenerAmt = tblWalletTransObj.SweetenerAmt;
                                                totalSweetenerAmt += tblWalletTransObj.SweetenerAmt ?? 0;
                                                podVMtemp.Lgccost = tblWalletTransObj.Lgccost;
                                                totalLgcCost += tblWalletTransObj.Lgccost ?? 0;
                                                isGSTInclusive = false;
                                            }
                                            #endregion

                                            #region Prime Product case with new scenerio
                                            if (tblWalletTransObj.IsPrimeProductId == true)
                                            {
                                                podVMtemp.OrderAmtForEVCInv = podVMtemp.OrderAmtForEVCInv - (tblWalletTransObj?.PrimeProductDiffAmt ?? 0);
                                                podVMtemp.PrimeProductDiffAmt = tblWalletTransObj?.PrimeProductDiffAmt;
                                                totalPrimeProductAmt += (tblWalletTransObj.PrimeProductDiffAmt ?? 0);
                                            }
                                            #endregion

                                            #region Set Base Value and GST Amount (With Check the Bulk Liquidation Case)
                                            if (podVMtemp.IsDebitNoteSkiped == true)
                                            {
                                                podVMtemp = SetBaseValueAndGst(podVMtemp, isGSTInclusive);
                                                totalCgst += podVMtemp.Cgstamt ?? 0;
                                                totalSgst += podVMtemp.Sgstamt ?? 0;
                                            }
                                            else
                                            {
                                                podVMtemp.BaseValue = tblWalletTransObj.BaseValue;
                                                podVMtemp.Cgstamt = tblWalletTransObj.Cgstamt;
                                                podVMtemp.Sgstamt = tblWalletTransObj.Sgstamt;
                                                totalCgst += tblWalletTransObj.Cgstamt ?? 0;
                                                totalSgst += tblWalletTransObj.Sgstamt ?? 0;
                                            }
                                            #endregion

                                            //podVMtemp.BaseValue = tblWalletTransObj.BaseValue;
                                            //podVMtemp.Cgstamt = tblWalletTransObj.Cgstamt;
                                            //podVMtemp.Sgstamt = tblWalletTransObj.Sgstamt;
                                            //totalCgst += tblWalletTransObj.Cgstamt ?? 0;
                                            //totalSgst += tblWalletTransObj.Sgstamt ?? 0;
                                            //podVM.IsOrderAmtWithSweetener = true;
                                        }
                                        else
                                        {
                                            #region Get Base Amount and GST Calculation according to V1
                                            bool isGSTInclusive = true;
                                            if (podVMtemp.OrderAmtForEVCInv > 0)
                                            {
                                                podVMtemp = SetBaseValueAndGst(podVMtemp, isGSTInclusive);
                                                totalCgst += podVMtemp.Cgstamt ?? 0;
                                                totalSgst += podVMtemp.Sgstamt ?? 0;
                                                //podVM.IsOrderAmtWithSweetener = true;
                                            }
                                            #endregion
                                        }
                                        finalAmountInv += Convert.ToDecimal(podVMtemp.OrderAmtForEVCInv);
                                        podVM.podVMList.Add(podVMtemp);
                                        #endregion
                                    }
                                }
                            }
                            #endregion

                            if (podVM != null && podVM.podVMList != null && podVM.podVMList.Count > 0)
                            {
                                #region Generate PoD
                                podVM.FinancialYear = FinancialYear;
                                podVM.InvSrNum = MaxInvSrNum;
                                podfilePath = EnumHelper.DescriptionAttr(FilePathEnum.EVCPoD);
                                podHtmlString = GetPoDHtmlString(podVM, "POD");
                                podPDFSave = _htmlToPdfConverterHelper.GeneratePDF(podHtmlString, podfilePath, podPdfName);
                                #endregion

                                #region Generate and Save Debit Note
                                if (podVM.DNOrderCount > 0)
                                {
                                    podVM.evcDetailsVM.FinalPriceDN = finalAmountDN;
                                    podVM.evcDetailsVM.TotalSweetenerAmt = totalSweetenerAmt;
                                    DropCompleted = GenerateAndSaveDebitNote(orderList, podVM, loggedUserId);
                                }
                                #endregion

                                #region Generate and Save Invoice Pdf
                                if (podVM.InvOrderCount > 0)
                                {
                                    podVM.evcDetailsVM.FinalPriceInv = finalAmountInv + totalLgcCost + totalPrimeProductAmt;
                                    podVM.evcDetailsVM.TotalCgstamt = totalCgst;
                                    podVM.evcDetailsVM.TotalSgstamt = totalCgst;
                                    podVM.evcDetailsVM.TotalLgccost = totalLgcCost;
                                    podVM.evcDetailsVM.TotalPrimeProductDiffAmt = totalPrimeProductAmt;
                                    flag = GenerateAndSaveInvoice(orderList, podVM, loggedUserId);
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "GenerateAndStorePDFData", ex);
            }
            return flag;
        }

        
        public PODViewModel SetBaseValueAndGst(PODViewModel podVMtemp, bool isGSTInclusive)
        {
            decimal? BaseAmount = 0;
            try
            {
                if (podVMtemp != null)
                {
                    if (podVMtemp.OrderAmtForEVCInv > 0)
                    {
                        if (isGSTInclusive)
                        {
                            BaseAmount = podVMtemp.OrderAmtForEVCInv / Convert.ToDecimal(GeneralConstant.GSTPercentage);
                        }
                        else
                        {
                            BaseAmount = podVMtemp.OrderAmtForEVCInv;
                        }
                        BaseAmount = Math.Round((BaseAmount ?? 0), 2);
                        podVMtemp.OrderAmtForEVCInv = Math.Round((podVMtemp.OrderAmtForEVCInv ?? 0), 2);
                        decimal? GstAmount = BaseAmount * Convert.ToDecimal(GeneralConstant.CGST);
                        GstAmount = Math.Round((GstAmount ?? 0), 2);

                        podVMtemp.BaseValue = BaseAmount;
                        podVMtemp.Cgstamt = GstAmount;
                        podVMtemp.Sgstamt = GstAmount;
                        //podVM.IsOrderAmtWithSweetener = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "SetBaseValueAndGst", ex);
            }
            return podVMtemp;
        }

        public bool UpdateLGCDropStatus(List<TblOrderLgc> tblOrderLgcList, PODViewModel podVM, int loggedUserId)
        {
            #region Variable Declaration
            TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = null;
            TblEvcwalletHistory tblEvcwalletHistory = null;
            TblEvcregistration tblEvcregistration = null;
            TblWalletTransaction tblWalletTransaction = null;
            TblLogistic tblLogistic = null;

            int resultSavedPoDId = 0;
            bool flag = false;
            int setStatusId = Convert.ToInt32(OrderStatusEnum.LGCDrop);
            #endregion

            OrderLGCViewModel orderLGC = new OrderLGCViewModel();
            try
            {
                if (podVM != null && podVM.DriverId > 0 && podVM.EVCRegistrationId > 0)
                {
                    if (tblOrderLgcList != null && tblOrderLgcList.Count > 0)
                    {
                        foreach (var item in tblOrderLgcList)
                        {
                            if (item != null && item.Evcregistration != null && item.Evcpartner != null && item.Logistic != null && item.OrderTrans != null
                                    && (item.OrderTrans.Exchange != null || item.OrderTrans.Abbredemption != null) && item.OrderTrans.TblWalletTransactions != null
                                    && item.OrderTrans.TblWalletTransactions.Count > 0)
                            {
                                var podVMIsExist = podVM.podVMList.FirstOrDefault(x => x.RegdNo == item.Logistic.RegdNo);
                                if (podVMIsExist != null)
                                {
                                    #region Common Implementations for (ABB or Exchange)
                                    TblExchangeOrder tblExchangeOrder = null; TblOrderTran tblOrderTran = null;
                                    TblAbbredemption tblAbbredemption = null;
                                    TblAbbregistration tblAbbregistration = null;
                                    decimal FinalPriceWithoutSweetner = 0; int? customerDetailsId = null; string sponsorOrderNo = null;
                                    bool? IsFEPWithoutSweetener = null; decimal SweetenerAmt = 0;
                                    #endregion

                                    #region tblInitialization
                                    tblOrderTran = item.OrderTrans;
                                    tblWalletTransaction = tblOrderTran.TblWalletTransactions.FirstOrDefault(x => x.OrderTransId == tblOrderTran.OrderTransId);
                                    tblExchangeOrder = tblOrderTran.Exchange;
                                    tblEvcregistration = item.Evcregistration;
                                    tblLogistic = item.Logistic;
                                    #endregion

                                    if (tblWalletTransaction != null)
                                    {
                                        #region Common Implementations for (ABB or Exchange)
                                        if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                                        {
                                            tblExchangeOrder = tblOrderTran?.Exchange;
                                            customerDetailsId = tblExchangeOrder?.CustomerDetailsId;
                                            sponsorOrderNo = tblExchangeOrder?.SponsorOrderNumber;
                                            #region Condition based set Final Exchange Price
                                            IsFEPWithoutSweetener = Convert.ToBoolean(tblExchangeOrder?.IsFinalExchangePriceWithoutSweetner ?? false);
                                            if (IsFEPWithoutSweetener == true)
                                            {
                                                FinalPriceWithoutSweetner = Convert.ToDecimal(tblExchangeOrder?.FinalExchangePrice);
                                            }
                                            else
                                            {
                                                FinalPriceWithoutSweetner = Convert.ToDecimal(tblExchangeOrder?.FinalExchangePrice) - Convert.ToDecimal(tblExchangeOrder?.Sweetener);
                                                SweetenerAmt = Convert.ToDecimal(tblExchangeOrder?.Sweetener);
                                            }
                                            #endregion
                                        }
                                        else if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.ABB))
                                        {
                                            tblAbbredemption = tblOrderTran?.Abbredemption;
                                            tblAbbregistration = tblAbbredemption?.Abbregistration;
                                            customerDetailsId = tblAbbredemption?.CustomerDetailsId;
                                            sponsorOrderNo = tblAbbregistration?.SponsorOrderNo;
                                            FinalPriceWithoutSweetner = Convert.ToDecimal(tblOrderTran?.FinalPriceAfterQc);
                                        }
                                        #endregion

                                        #region Set Final Debit Note Amount to be debited
                                        decimal FinalDNAmtPerOrder = 0;
                                        if (podVMIsExist.IsOrderAmtWithSweetener == true && IsFEPWithoutSweetener != true && SweetenerAmt > 0)
                                        {
                                            FinalDNAmtPerOrder = FinalPriceWithoutSweetner + SweetenerAmt;
                                        }
                                        else
                                        {
                                            FinalDNAmtPerOrder = FinalPriceWithoutSweetner;
                                        }
                                        #endregion

                                        #region Save PoD and Debit Note Details
                                        podVM.RegdNo = tblOrderTran.RegdNo;
                                        podVM.EVCId = tblEvcregistration.EvcregistrationId;
                                        if (tblExchangeOrder != null && tblExchangeOrder.Id > 0)
                                        {
                                            podVM.ExchangeId = tblExchangeOrder.Id;
                                            podVM.AbbredemptionId = null;
                                        }
                                        else if (tblAbbredemption != null && tblAbbredemption.RedemptionId > 0)
                                        {
                                            podVM.AbbredemptionId = tblAbbredemption.RedemptionId;
                                            podVM.ExchangeId = null;
                                        }

                                        podVM.OrderTransId = tblOrderTran.OrderTransId;
                                        podVM.DebitNoteDate = _currentDatetime;
                                        resultSavedPoDId = ManageExchangePOD(podVM, loggedUserId);
                                        #endregion

                                        #region code to Update drop status in order lgc
                                        item.ActualDropDate = _currentDatetime;
                                        item.StatusId = setStatusId;
                                        item.Evcpodid = resultSavedPoDId;
                                        _orderLGCRepository.Update(item);
                                        _orderLGCRepository.SaveChanges();
                                        #endregion

                                        #region Create EVC Wallet History 
                                        tblEvcwalletHistory = new TblEvcwalletHistory();
                                        tblEvcwalletHistory.EvcregistrationId = (int)tblWalletTransaction.EvcregistrationId;
                                        tblEvcwalletHistory.EvcpartnerId = tblWalletTransaction.EvcpartnerId;
                                        tblEvcwalletHistory.OrderTransId = tblWalletTransaction.OrderTransId;
                                        tblEvcwalletHistory.CurrentWalletAmount = tblEvcregistration.EvcwalletAmount;
                                        tblEvcwalletHistory.FinalOrderAmount = tblWalletTransaction.OrderAmount;
                                        tblEvcwalletHistory.AmountdeductionFlag = true;
                                        tblEvcwalletHistory.BalanceWalletAmount = tblEvcwalletHistory.CurrentWalletAmount - FinalDNAmtPerOrder; //tblEvcwalletHistory.FinalOrderAmount;
                                        tblEvcwalletHistory.IsActive = true;
                                        tblEvcwalletHistory.CreatedBy = loggedUserId;
                                        tblEvcwalletHistory.CreatedDate = _currentDatetime;
                                        _eVCWalletHistoryRepository.Create(tblEvcwalletHistory);
                                        _eVCWalletHistoryRepository.SaveChanges();
                                        #endregion

                                        #region Update Tbl EVC Registration 
                                        tblEvcregistration.EvcwalletAmount = tblEvcregistration.EvcwalletAmount - FinalDNAmtPerOrder; //tblWalletTransaction.OrderAmount;
                                        tblEvcregistration.ModifiedBy = loggedUserId;
                                        tblEvcregistration.ModifiedDate = _currentDatetime;
                                        _eVCRepository.Update(tblEvcregistration);
                                        _eVCRepository.SaveChanges();
                                        #endregion

                                        #region update tblwallettranscation 
                                        tblWalletTransaction.OrderOfDeliverdDate = _currentDatetime;
                                        //Modification Pending: Update OrderOfCompleteDate at the time invoice generate
                                        tblWalletTransaction.StatusId = setStatusId.ToString();
                                        tblWalletTransaction.ModifiedBy = loggedUserId;
                                        tblWalletTransaction.ModifiedDate = _currentDatetime;
                                        _walletTransactionRepository.Update(tblWalletTransaction);
                                        _walletTransactionRepository.SaveChanges();
                                        #endregion

                                        #region update statusId in Base tbl Exchange or ABB
                                        if (tblExchangeOrder != null)
                                        {
                                            tblExchangeOrder.StatusId = setStatusId;
                                            tblExchangeOrder.OrderStatus = "Delivered";
                                            tblExchangeOrder.ModifiedBy = loggedUserId;
                                            tblExchangeOrder.ModifiedDate = _currentDatetime;
                                            _exchangeOrderRepository.Update(tblExchangeOrder);
                                            _exchangeOrderRepository.SaveChanges();
                                        }
                                        else if (tblAbbredemption != null && tblAbbregistration != null)
                                        {
                                            #region update status on tblAbbRedemption
                                            tblAbbredemption.StatusId = setStatusId;
                                            tblAbbredemption.AbbredemptionStatus = "Delivered";
                                            tblAbbredemption.ModifiedBy = loggedUserId;
                                            tblAbbredemption.ModifiedDate = _currentDatetime;
                                            _abbRedemptionRepository.Update(tblAbbredemption);
                                            _abbRedemptionRepository.SaveChanges();
                                            #endregion
                                        }
                                        #endregion

                                        #region Update Status Id in tblLogistic
                                        tblLogistic.StatusId = setStatusId;
                                        tblLogistic.Modifiedby = loggedUserId;
                                        tblLogistic.ModifiedDate = _currentDatetime;
                                        _logisticsRepository.Update(tblLogistic);
                                        _logisticsRepository.SaveChanges();
                                        #endregion

                                        #region update statusId in tblOrderTrans
                                        tblOrderTran.StatusId = setStatusId;
                                        tblOrderTran.ModifiedBy = loggedUserId;
                                        tblOrderTran.ModifiedDate = _currentDatetime;
                                        _orderTransRepository.Update(tblOrderTran);
                                        _orderTransRepository.SaveChanges();
                                        #endregion

                                        #region Insert into tblexchangeabbhistory
                                        tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                        tblExchangeAbbstatusHistory.OrderType = (int)tblOrderTran.OrderType;
                                        tblExchangeAbbstatusHistory.SponsorOrderNumber = sponsorOrderNo;
                                        tblExchangeAbbstatusHistory.RegdNo = tblOrderTran.RegdNo;
                                        tblExchangeAbbstatusHistory.CustId = customerDetailsId;
                                        tblExchangeAbbstatusHistory.StatusId = setStatusId;
                                        tblExchangeAbbstatusHistory.IsActive = true;
                                        tblExchangeAbbstatusHistory.CreatedBy = loggedUserId;
                                        tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                        tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                                        _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                                        #endregion
                                        flag = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "SaveLGCDropStatus", ex);
            }
            return flag;
        }
        public bool UpdateInvoicePostedStatus(List<TblOrderLgc> tblOrderLgcList, PODViewModel podVM, int? loggedUserId)
        {
            TblEvcwalletHistory tblEvcwalletHistory = null;
            TblLogistic tblLogistic = null;
            TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = null;
            bool flag = false; int resultSavedPoDId = 0;
            List<TblOrderLgc> lgcOrderList = null;
            try
            {
                int setStatusId = Convert.ToInt32(OrderStatusEnum.Posted);
                int DropStatusId = Convert.ToInt32(OrderStatusEnum.LGCDrop);
                lgcOrderList = tblOrderLgcList;
                if (lgcOrderList != null && lgcOrderList.Count > 0)
                {
                    #region Update Invoice Details In DB  
                    foreach (var tblOrderLgc in lgcOrderList)
                    {
                        if (tblOrderLgc != null && tblOrderLgc.Evcregistration != null && tblOrderLgc.Evcpartner != null && tblOrderLgc.Logistic != null && tblOrderLgc.OrderTrans != null
                                    && (tblOrderLgc.OrderTrans.Exchange != null || tblOrderLgc.OrderTrans.Abbredemption != null) && tblOrderLgc.OrderTrans.TblWalletTransactions != null
                                    && tblOrderLgc.OrderTrans.TblWalletTransactions.Count > 0)
                        {
                            var podVMIsExist = podVM.podVMList.FirstOrDefault(x => x.RegdNo == tblOrderLgc.Logistic.RegdNo);
                            if (podVMIsExist != null)
                            {
                                #region Common Implementations for (ABB or Exchange)
                                TblExchangeOrder tblExchangeOrder = null; TblOrderTran tblOrderTran = null;
                                TblAbbredemption tblAbbredemption = null;
                                TblAbbregistration tblAbbregistration = null;
                                decimal FinalPriceWithoutSweetner = 0; int? customerDetailsId = null; string sponsorOrderNo = null;
                                bool? IsFEPWithoutSweetener = null; decimal SweetenerAmt = 0;
                                #endregion

                                #region Variable Initialization
                                tblOrderTran = tblOrderLgc.OrderTrans;
                                TblWalletTransaction tblWalletTransection = tblOrderTran.TblWalletTransactions.FirstOrDefault(x => x.OrderTransId == tblOrderLgc.OrderTransId);
                                //TblEvcpoddetail tblEvcpoddetail = tblOrderLgc.Evcpod;
                                TblEvcpoddetail tblEvcpoddetail = _evcPoDDetailsRepository.GetEVCPODDetailsById(tblOrderLgc.Evcpodid);
                                //tblEvcpoddetail = tblOrderLgc.Evcpod;

                                #endregion
                                if (tblWalletTransection != null)
                                {
                                    #region Common Implementations for (ABB or Exchange)
                                    if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                                    {
                                        tblExchangeOrder = tblOrderTran.Exchange;
                                        customerDetailsId = tblExchangeOrder?.CustomerDetailsId;
                                        sponsorOrderNo = tblExchangeOrder?.SponsorOrderNumber;
                                        #region Condition based set Final Exchange Price
                                        IsFEPWithoutSweetener = Convert.ToBoolean(tblExchangeOrder?.IsFinalExchangePriceWithoutSweetner ?? false);
                                        if (IsFEPWithoutSweetener == true)
                                        {
                                            FinalPriceWithoutSweetner = Convert.ToDecimal(tblExchangeOrder?.FinalExchangePrice);
                                        }
                                        else
                                        {
                                            FinalPriceWithoutSweetner = Convert.ToDecimal(tblExchangeOrder?.FinalExchangePrice) - Convert.ToDecimal(tblExchangeOrder?.Sweetener);
                                            SweetenerAmt = Convert.ToDecimal(tblExchangeOrder?.Sweetener);
                                        }
                                        #endregion
                                    }
                                    else if (tblOrderTran.OrderType == Convert.ToInt32(LoVEnum.ABB))
                                    {
                                        tblAbbredemption = tblOrderTran.Abbredemption;
                                        tblAbbregistration = tblAbbredemption?.Abbregistration;
                                        customerDetailsId = tblAbbredemption?.CustomerDetailsId;
                                        sponsorOrderNo = tblAbbregistration?.SponsorOrderNo;
                                        FinalPriceWithoutSweetner = Convert.ToDecimal(tblOrderTran?.FinalPriceAfterQc);
                                    }
                                    #endregion

                                    #region Set Debitnote amount that has been debited
                                    decimal FinalDNAmtPerOrder = 0;
                                    if (podVMIsExist.IsOrderAmtWithSweetener == true && IsFEPWithoutSweetener != true && SweetenerAmt > 0)
                                    {
                                        FinalDNAmtPerOrder = FinalPriceWithoutSweetner + SweetenerAmt;
                                    }
                                    else
                                    {
                                        FinalDNAmtPerOrder = FinalPriceWithoutSweetner;
                                    }
                                    #endregion

                                    #region Insert Data on tblEVCPODDetails
                                    if (tblEvcpoddetail == null)
                                    {
                                        podVM.RegdNo = tblOrderTran.RegdNo;
                                        podVM.EVCId = tblOrderLgc.Evcregistration.EvcregistrationId;
                                        if (tblExchangeOrder != null && tblExchangeOrder.Id > 0)
                                        {
                                            podVM.ExchangeId = tblExchangeOrder.Id;
                                            podVM.AbbredemptionId = null;
                                        }
                                        else if (tblAbbredemption != null && tblAbbredemption.RedemptionId > 0)
                                        {
                                            podVM.AbbredemptionId = tblAbbredemption.RedemptionId;
                                            podVM.ExchangeId = null;
                                        }
                                        podVM.OrderTransId = tblOrderTran.OrderTransId;

                                        #region Update TblEVCPODDetails for Invoice
                                        podVM.InvoiceDate = _currentDatetime;
                                        #endregion
                                        resultSavedPoDId = ManageExchangePOD(podVM, loggedUserId ?? 0);
                                    }
                                    else
                                    {
                                        #region Update TblEVCPODDetails for Invoice
                                        tblEvcpoddetail.InvoicePdfName = podVM.InvoicePdfName;
                                        tblEvcpoddetail.InvSrNum = podVM.InvSrNum;
                                        tblEvcpoddetail.FinancialYear = podVM.FinancialYear;
                                        tblEvcpoddetail.InvoiceDate = _currentDatetime;
                                        tblEvcpoddetail.InvoiceAmount = podVM.InvoiceAmount;
                                        tblEvcpoddetail.ModifiedBy = loggedUserId;
                                        tblEvcpoddetail.ModifiedDate = _currentDatetime;
                                        /*_evcPoDDetailsRepository.Update(tblEvcpoddetail);*/

                                        // set Modified flag in your entry
                                        _context.Entry(tblEvcpoddetail).State = EntityState.Modified;
                                        // save 
                                        _context.SaveChanges();
                                        #endregion
                                    }
                                    #endregion

                                    #region Update TblOrderLgc for Invoice 
                                    if (resultSavedPoDId > 0) { tblOrderLgc.Evcpodid = resultSavedPoDId; }
                                    tblOrderLgc.IsInvoiceGenerated = true;
                                    tblOrderLgc.StatusId = setStatusId;
                                    tblOrderLgc.ModifiedBy = loggedUserId;
                                    tblOrderLgc.ModifiedDate = _currentDatetime;
                                    tblOrderLgc.ActualDropDate = _currentDatetime;
                                    _orderLGCRepository.Update(tblOrderLgc);
                                    _orderLGCRepository.SaveChanges();
                                    #endregion

                                    #region Create EVC Wallet History 
                                    tblEvcwalletHistory = new TblEvcwalletHistory();
                                    tblEvcwalletHistory.EvcregistrationId = (int)tblOrderLgc.EvcregistrationId;
                                    tblEvcwalletHistory.EvcpartnerId = tblOrderLgc.EvcpartnerId;
                                    tblEvcwalletHistory.OrderTransId = tblOrderLgc.OrderTransId;
                                    tblEvcwalletHistory.CurrentWalletAmount = tblOrderLgc.Evcregistration.EvcwalletAmount;
                                    tblEvcwalletHistory.FinalOrderAmount = tblWalletTransection.OrderAmount;
                                    tblEvcwalletHistory.AmountdeductionFlag = true;
                                    decimal? EVCPlateformFee = tblEvcwalletHistory.FinalOrderAmount - FinalDNAmtPerOrder;
                                    tblEvcwalletHistory.BalanceWalletAmount = tblEvcwalletHistory.CurrentWalletAmount - EVCPlateformFee; //tblEvcwalletHistory.FinalOrderAmount;
                                    tblEvcwalletHistory.IsActive = true;
                                    tblEvcwalletHistory.CreatedBy = loggedUserId;
                                    tblEvcwalletHistory.CreatedDate = _currentDatetime;
                                    _eVCWalletHistoryRepository.Create(tblEvcwalletHistory);
                                    _eVCWalletHistoryRepository.SaveChanges();
                                    #endregion

                                    #region Update TblEVCRegistration 
                                    tblOrderLgc.Evcregistration.EvcwalletAmount = tblOrderLgc.Evcregistration.EvcwalletAmount - EVCPlateformFee;
                                    tblOrderLgc.Evcregistration.ModifiedBy = loggedUserId;
                                    tblOrderLgc.Evcregistration.ModifiedDate = _currentDatetime;
                                    _eVCRepository.Update(tblOrderLgc.Evcregistration);
                                    _eVCRepository.SaveChanges();
                                    #endregion

                                    #region update statusId in Base tbl Exchange or ABB
                                    if (tblExchangeOrder != null)
                                    {
                                        tblExchangeOrder.StatusId = setStatusId;
                                        tblExchangeOrder.OrderStatus = "Posted";
                                        tblExchangeOrder.ModifiedBy = loggedUserId;
                                        tblExchangeOrder.ModifiedDate = _currentDatetime;
                                        _exchangeOrderRepository.Update(tblExchangeOrder);
                                        _exchangeOrderRepository.SaveChanges();
                                    }
                                    else if (tblAbbredemption != null && tblAbbregistration != null)
                                    {
                                        #region update status on tblAbbRedemption
                                        tblAbbredemption.StatusId = setStatusId;
                                        tblAbbredemption.AbbredemptionStatus = "Posted";
                                        tblAbbredemption.ModifiedBy = loggedUserId;
                                        tblAbbredemption.ModifiedDate = _currentDatetime;
                                        _abbRedemptionRepository.Update(tblAbbredemption);
                                        _abbRedemptionRepository.SaveChanges();
                                        #endregion
                                    }
                                    #endregion

                                    #region update status in tblordertrans added by PJ  
                                    _orderTransRepository.UpdateTransRecordStatus(tblOrderLgc.OrderTransId, (int)OrderStatusEnum.Posted, loggedUserId);
                                    #endregion

                                    #region update status in tbllogistics added by PJ                                      
                                    tblLogistic = tblOrderLgc.Logistic;
                                    tblLogistic.StatusId = setStatusId;
                                    tblLogistic.Modifiedby = loggedUserId;
                                    tblLogistic.ModifiedDate = _currentDatetime;
                                    _logisticsRepository.Update(tblLogistic);
                                    _logisticsRepository.SaveChanges();
                                    #endregion

                                    #region update tblwallettranscation 
                                    if (podVMIsExist?.IsDebitNoteSkiped == true)
                                    {
                                        tblWalletTransection.OrderOfDeliverdDate = _currentDatetime;
                                    }
                                    tblWalletTransection.OrderOfCompleteDate = _currentDatetime;
                                    tblWalletTransection.StatusId = setStatusId.ToString();
                                    tblWalletTransection.ModifiedBy = loggedUserId;
                                    tblWalletTransection.ModifiedDate = _currentDatetime;
                                    _walletTransactionRepository.Update(tblWalletTransection);
                                    _walletTransactionRepository.SaveChanges();
                                    #endregion

                                    #region Mapping History data Create History
                                    tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                    tblExchangeAbbstatusHistory.OrderType = (int)tblOrderTran.OrderType;
                                    tblExchangeAbbstatusHistory.SponsorOrderNumber = sponsorOrderNo;
                                    tblExchangeAbbstatusHistory.RegdNo = tblOrderTran.RegdNo;
                                    tblExchangeAbbstatusHistory.CustId = customerDetailsId;
                                    tblExchangeAbbstatusHistory.IsActive = true;
                                    tblExchangeAbbstatusHistory.CreatedBy = loggedUserId;
                                    tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                    tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;

                                    #region Create Drop Status History
                                    if (podVMIsExist.IsDebitNoteSkiped == true)
                                    {
                                        tblExchangeAbbstatusHistory.StatusId = DropStatusId;
                                        _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                                    }
                                    #endregion

                                    #region Create Invoice History
                                    tblExchangeAbbstatusHistory.StatusId = setStatusId;
                                    _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                                    #endregion
                                    #endregion
                                    flag = true;
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "UpdateInvoicePostedStatus", ex);
            }
            return flag;
        }
        #endregion

        #region GenerateInvoiceForEVC Method for Generate Invoice by Console Application
        public List<TblOrderLgc> GenerateInvoiceForEVC(int? EvcRegistrationId, int? loggedUserId)
        {
            #region Variable Declaration
            List<TblOrderLgc> tblOrderLgcList = new List<TblOrderLgc>();
            PODViewModel podVM = new PODViewModel();
            PODViewModel podVMtemp = new PODViewModel();
            TblEvcwalletHistory tblEvcwalletHistory = null;            
            List<TblConfiguration> tblConfigurationList = null;
            decimal? finalAmountInv = 0;
            string invoicePdfName = null;
            bool invoiceSaved = false;
            string invoicefilePath = null;
            string invoiceHtmlString = null;
            int MaxInvSrNum = 0;
            int InvSrNumFromConfig = 0;
            string FinancialYear = "";
            TblEvcpoddetail tblEvcpoddetailObj = null;
            TblExchangeOrder tblExchangeOrder = null;
            TblLogistic tblLogistic = null;
            TblOrderTran tblOrderTran = null;
            TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = null;
            #endregion
            try
            {
                if (loggedUserId == null)
                {
                    //User Role whitch have a uniq value    
                    TblRole tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleName == EnumHelper.DescriptionAttr(RoleEnum.SuperAdmin));
                    if (tblRole != null)
                        loggedUserId = _userRoleRepository.GetSingle(x => x.IsActive == true && x.RoleId == tblRole.RoleId).UserId;
                }

                #region Code for Get Data from TblConfiguration
                tblConfigurationList = _digi2L_DevContext.TblConfigurations.Where(x => x.IsActive == true).ToList();
                if (tblConfigurationList != null && tblConfigurationList.Count > 0)
                {
                    // MaxInvSrNum = tblEvcpoddetail.Max(x => x.InvSrNum).Value;
                    var startInvoiceSrNum = tblConfigurationList.FirstOrDefault(x => x.Name == ConfigurationEnum.StartInvoiceSrNum.ToString());
                    if (startInvoiceSrNum != null && startInvoiceSrNum.Value != null)
                    {
                        InvSrNumFromConfig = Convert.ToInt32(startInvoiceSrNum.Value.Trim());
                    }
                    var financialYear = tblConfigurationList.FirstOrDefault(x => x.Name == ConfigurationEnum.FinancialYear.ToString());
                    if (financialYear != null && financialYear.Value != null)
                    {
                        FinancialYear = financialYear.Value.Trim();
                    }
                }
                #endregion

                #region Code for get Max InvSrNum from TblEvcpoddetails
                tblEvcpoddetailObj = _evcPoDDetailsRepository.GetList(x => x.IsActive == true && x.InvSrNum != null && x.InvSrNum != null && x.InvSrNum > 0 && x.FinancialYear == FinancialYear).OrderByDescending(x => x.InvSrNum).FirstOrDefault();

                if (tblEvcpoddetailObj != null && tblEvcpoddetailObj.InvSrNum <= InvSrNumFromConfig)
                {
                    MaxInvSrNum = InvSrNumFromConfig;
                }
                else if (tblEvcpoddetailObj != null && tblEvcpoddetailObj.InvSrNum >= InvSrNumFromConfig)
                {
                    MaxInvSrNum = Convert.ToInt32(tblEvcpoddetailObj.InvSrNum);
                }
                else if (tblEvcpoddetailObj == null && InvSrNumFromConfig > 0)
                {
                    MaxInvSrNum = InvSrNumFromConfig;
                }
                #endregion

                tblOrderLgcList = _orderLGCRepository.GetOrderLGCListForGenerateInvoice(EvcRegistrationId,Convert.ToInt32(OrderStatusEnum.LGCDrop)).ToList();
                if (tblOrderLgcList != null && tblOrderLgcList.Count > 0)
                {
                    var groupByEVC = tblOrderLgcList
                         .GroupBy(x => x.EvcregistrationId).Select(g => new
                         {
                             EVCDetails = g.Select(x => x.Evcregistration).FirstOrDefault(),
                             OrderList = g.ToList()
                         });

                    foreach (var groupItem in groupByEVC)
                    {
                        if (groupItem != null && groupItem.EVCDetails != null && groupItem.OrderList != null && groupItem.OrderList.Count > 0)
                        {
                            podVM.evcDetailsVM = new EVCDetailsViewModel();

                            #region EVC Details and per invoice
                            podVM.evcDetailsVM.Name = groupItem.EVCDetails.BussinessName;
                            podVM.evcDetailsVM.Address = groupItem.EVCDetails.RegdAddressLine1;
                            podVM.evcDetailsVM.City = groupItem.EVCDetails.City.Name;
                            podVM.evcDetailsVM.State = groupItem.EVCDetails.City.State.Name;
                            podVM.evcDetailsVM.Pincode = groupItem.EVCDetails.PinCode;
                            #endregion

                            var totalOrderRecords = 0;
                            int pageSize = 8;
                            int skip = 0;
                            int totalPages = 1;
                            int reminder = 0;

                            #region if Orders List is greater than [pageSize] records per invoice
                            if (groupItem.OrderList != null && groupItem.OrderList.Count > 0)
                            {
                                totalOrderRecords = groupItem.OrderList.Count;
                                if (totalOrderRecords > pageSize)
                                {
                                    totalPages = Convert.ToInt32(totalOrderRecords / pageSize);
                                    reminder = Convert.ToInt32(totalOrderRecords % pageSize);
                                    if (reminder > 0) { totalPages++; }
                                }
                                for (int i = 1; i <= totalPages; i++)
                                {
                                    podVM.podVMList = new List<PODViewModel>();
                                    finalAmountInv = 0;
                                    MaxInvSrNum++;
                                    skip = (i - 1) * pageSize;
                                    var orderList = groupItem.OrderList.Skip(skip).Take(pageSize);

                                    #region EVC Price Calculation per order for Invoice 
                                    foreach (TblOrderLgc orderObj in orderList)
                                    {
                                        if (orderObj != null && orderObj.Evcpod != null && orderObj.Evcpod.InvoicePdfName == null)
                                        {
                                            podVMtemp = new PODViewModel();
                                            podVMtemp.RegdNo = orderObj.OrderTrans.RegdNo;
                                            decimal? orderAmt = orderObj.OrderTrans.TblWalletTransactions.FirstOrDefault(x => x.OrderTransId == orderObj.OrderTransId).OrderAmount;
                                            decimal FinalExchPriceWithoutSweetner = Convert.ToDecimal(orderObj.OrderTrans.Exchange.FinalExchangePrice) - Convert.ToDecimal(orderObj.OrderTrans.Exchange.Sweetener);
                                            podVMtemp.OrderAmtForEVCInv = Convert.ToDecimal(orderAmt) - Convert.ToDecimal(FinalExchPriceWithoutSweetner);
                                            finalAmountInv += Convert.ToDecimal(podVMtemp.OrderAmtForEVCInv);
                                            podVMtemp.Podurl = EnumHelper.DescriptionAttr(FileAddressEnum.EVCPoD) + orderObj.Evcpod.Podurl;
                                            podVM.podVMList.Add(podVMtemp);
                                        }
                                    }
                                    #endregion
                                    if (podVM != null && podVM.podVMList != null && podVM.podVMList.Count > 0)
                                    {
                                        finalAmountInv = Math.Round((finalAmountInv ?? 0), 2);
                                        #region Set EVC invoice Final Price per Invoice
                                        podVM.evcDetailsVM.FinalPriceInv = finalAmountInv;
                                        #endregion

                                        #region Generate Invoice Pdf
                                        string InvSrNum = String.Format("{0:D6}", MaxInvSrNum);
                                        string InvBillNumber = "INV-" + FinancialYear + "/" + InvSrNum;
                                        podVM.evcDetailsVM.BillNumberInv = InvBillNumber;
                                        invoicePdfName = InvBillNumber.Replace("/", "-") + ".pdf";
                                        invoicefilePath = EnumHelper.DescriptionAttr(FilePathEnum.EVCInvoice);
                                        invoiceHtmlString = GetPoDHtmlString(podVM, "EVC_Invoice");
                                        invoiceSaved = _htmlToPdfConverterHelper.GeneratePDF(invoiceHtmlString, invoicefilePath, invoicePdfName);
                                        #endregion
                                    }
                                    #region Update Invoice Details In DB  
                                    if (invoiceSaved)
                                    {
                                        foreach (TblOrderLgc tblOrderLgc in orderList)
                                        {
                                            if (tblOrderLgc != null && tblOrderLgc.Evcpod != null && tblOrderLgc.Evcpod.InvoicePdfName == null)
                                            {
                                                var podVMIsExist = podVM.podVMList.FirstOrDefault(x => x.RegdNo == tblOrderLgc.Logistic.RegdNo);
                                                if (podVMIsExist != null)
                                                {
                                                    #region Variable Initialization
                                                    TblWalletTransaction tblWalletTransection = tblOrderLgc.OrderTrans.TblWalletTransactions.FirstOrDefault(x => x.OrderTransId == tblOrderLgc.OrderTransId);
                                                    decimal FinalExchPriceWithoutSweetner = Convert.ToDecimal(tblOrderLgc.OrderTrans.Exchange.FinalExchangePrice) - Convert.ToDecimal(tblOrderLgc.OrderTrans.Exchange.Sweetener);
                                                    #endregion
                                                    if (tblWalletTransection != null)
                                                    {
                                                        #region Update TblEVCPODDetails for Invoice
                                                        tblOrderLgc.Evcpod.InvoicePdfName = invoicePdfName;
                                                        tblOrderLgc.Evcpod.InvSrNum = MaxInvSrNum;
                                                        tblOrderLgc.Evcpod.FinancialYear = FinancialYear;
                                                        tblOrderLgc.Evcpod.InvoiceDate = _currentDatetime;

                                                        /*var totalGST = finalAmountInv * Convert.ToDecimal(0.18);
                                                        tblOrderLgc.Evcpod.InvoiceAmount = finalAmountInv + totalGST;*/
                                                        tblOrderLgc.Evcpod.InvoiceAmount = finalAmountInv;
                                                        tblOrderLgc.Evcpod.ModifiedBy = loggedUserId;
                                                        tblOrderLgc.Evcpod.ModifiedDate = _currentDatetime;
                                                        _evcPoDDetailsRepository.Update(tblOrderLgc.Evcpod);
                                                        _evcPoDDetailsRepository.SaveChanges();
                                                        #endregion

                                                        #region Create EVC Wallet History 
                                                        tblEvcwalletHistory = new TblEvcwalletHistory();
                                                        tblEvcwalletHistory.EvcregistrationId = (int)tblOrderLgc.EvcregistrationId;
                                                        tblEvcwalletHistory.OrderTransId = tblOrderLgc.OrderTransId;
                                                        tblEvcwalletHistory.CurrentWalletAmount = tblOrderLgc.Evcregistration.EvcwalletAmount;
                                                        tblEvcwalletHistory.FinalOrderAmount = tblWalletTransection.OrderAmount;
                                                        tblEvcwalletHistory.AmountdeductionFlag = true;
                                                        decimal? EVCPlateformFee = tblEvcwalletHistory.FinalOrderAmount - FinalExchPriceWithoutSweetner;
                                                        tblEvcwalletHistory.BalanceWalletAmount = tblEvcwalletHistory.CurrentWalletAmount - EVCPlateformFee; //tblEvcwalletHistory.FinalOrderAmount;
                                                        tblEvcwalletHistory.IsActive = true;
                                                        tblEvcwalletHistory.CreatedBy = loggedUserId;
                                                        tblEvcwalletHistory.CreatedDate = _currentDatetime;
                                                        _eVCWalletHistoryRepository.Create(tblEvcwalletHistory);
                                                        _eVCWalletHistoryRepository.SaveChanges();
                                                        #endregion

                                                        #region Update TblEVCRegistration 
                                                        tblOrderLgc.Evcregistration.EvcwalletAmount = tblOrderLgc.Evcregistration.EvcwalletAmount - EVCPlateformFee;
                                                        tblOrderLgc.Evcregistration.ModifiedBy = loggedUserId;
                                                        tblOrderLgc.Evcregistration.ModifiedDate = _currentDatetime;
                                                        _eVCRepository.Update(tblOrderLgc.Evcregistration);
                                                        _eVCRepository.SaveChanges();
                                                        #endregion

                                                        #region update tblexchangeorder added by PJ                                       
                                                        tblExchangeOrder = tblOrderLgc.OrderTrans.Exchange;
                                                        tblExchangeOrder.StatusId = (int)OrderStatusEnum.Posted;
                                                        tblExchangeOrder.OrderStatus = "Posted";
                                                        tblExchangeOrder.ModifiedBy = loggedUserId;
                                                        tblExchangeOrder.ModifiedDate = _currentDatetime;
                                                        _exchangeOrderRepository.Update(tblExchangeOrder);
                                                        _exchangeOrderRepository.SaveChanges();
                                                        #endregion

                                                        #region update status in tblordertrans added by PJ                                      
                                                        tblOrderTran = tblOrderLgc.OrderTrans;
                                                        tblOrderTran.StatusId = (int)OrderStatusEnum.Posted;
                                                        tblOrderTran.ModifiedBy = loggedUserId;
                                                        tblOrderTran.ModifiedDate = _currentDatetime;
                                                        _orderTransRepository.Update(tblOrderTran);
                                                        _orderTransRepository.SaveChanges();
                                                        #endregion

                                                        #region Insert into tblexchangeabbhistory added by PJ
                                                        tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                                        tblExchangeAbbstatusHistory.OrderType = 17;
                                                        tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                                                        tblExchangeAbbstatusHistory.RegdNo = tblExchangeOrder.RegdNo;
                                                        tblExchangeAbbstatusHistory.ZohoSponsorId = tblExchangeOrder.ZohoSponsorOrderId;
                                                        tblExchangeAbbstatusHistory.CustId = tblExchangeOrder.CustomerDetailsId;
                                                        tblExchangeAbbstatusHistory.StatusId = tblExchangeOrder.StatusId;
                                                        tblExchangeAbbstatusHistory.IsActive = true;
                                                        tblExchangeAbbstatusHistory.CreatedBy = loggedUserId;
                                                        tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                                        tblExchangeAbbstatusHistory.OrderTransId = tblOrderTran.OrderTransId;
                                                        _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                                                        _exchangeABBStatusHistoryRepository.SaveChanges();
                                                        #endregion

                                                        #region update status in tbllogistics added by PJ                                      
                                                        tblLogistic = tblOrderLgc.Logistic;
                                                        tblLogistic.StatusId = (int)OrderStatusEnum.Posted;
                                                        tblLogistic.Modifiedby = loggedUserId;
                                                        tblLogistic.ModifiedDate = _currentDatetime;
                                                        _logisticsRepository.Update(tblLogistic);
                                                        _logisticsRepository.SaveChanges();
                                                        #endregion

                                                        #region update tblwallettranscation 
                                                        tblWalletTransection.OrderOfCompleteDate = _currentDatetime;
                                                        tblWalletTransection.StatusId = Convert.ToInt32(OrderStatusEnum.Posted).ToString();
                                                        tblWalletTransection.ModifiedBy = loggedUserId;
                                                        tblWalletTransection.ModifiedDate = _currentDatetime;
                                                        _walletTransactionRepository.Update(tblWalletTransection);
                                                        _walletTransactionRepository.SaveChanges();
                                                        #endregion

                                                        #region Update TblOrderLgc for Invoice 
                                                        tblOrderLgc.IsInvoiceGenerated = true;
                                                        tblOrderLgc.StatusId = (int)OrderStatusEnum.Posted;
                                                        tblOrderLgc.ModifiedBy = loggedUserId;
                                                        tblOrderLgc.ModifiedDate = _currentDatetime;
                                                        _orderLGCRepository.Update(tblOrderLgc);
                                                        _orderLGCRepository.SaveChanges();
                                                        #endregion
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "GenerateInvoiceForEVC", ex);
            }
            return tblOrderLgcList;
        }
        #endregion

        #region Store Exchange POD Details in database
        /// <summary>
        /// Exchange POD Details in database
        /// </summary>       
        /// <returns></returns>   
        public int ManageExchangePOD(PODViewModel podVM, int loggedUserId)
        {
            TblEvcpoddetail tblEVCPODDetailObj = new TblEvcpoddetail();
            string fileName = null;
            if (podVM.Podurl != null)
            {
                fileName = podVM.Podurl.Split('/').LastOrDefault();
            }
            else if (podVM.PoDPdfName != null)
            {
                fileName = podVM.PoDPdfName;
            }
            int result = 0;
            int evcPoDId = 0;
            try
            {
                if (podVM.RegdNo != null && (podVM.Podurl != null || podVM.PoDPdfName != null))
                {
                    tblEVCPODDetailObj = _evcPoDDetailsRepository.GetSingle(x => x.IsActive == true && x.RegdNo == podVM.RegdNo);
                    if (tblEVCPODDetailObj != null)
                    {
                        tblEVCPODDetailObj.Podurl = fileName;
                        if (podVM.ExchangeId > 0)
                        {
                            tblEVCPODDetailObj.ExchangeId = podVM.ExchangeId;
                        }
                        if (podVM.AbbredemptionId > 0)
                        {
                            tblEVCPODDetailObj.AbbredemptionId = podVM.AbbredemptionId;
                        }
                        tblEVCPODDetailObj.Evcid = podVM.EVCRegistrationId;
                        tblEVCPODDetailObj.EvcpartnerId = podVM.EvcPartnerId;
                        tblEVCPODDetailObj.OrderTransId = podVM.OrderTransId;
                        tblEVCPODDetailObj.DebitNotePdfName = podVM.DebitNotePdfName;
                        tblEVCPODDetailObj.DebitNoteDate = podVM.DebitNoteDate;
                        tblEVCPODDetailObj.DebitNoteAmount = podVM.DebitNoteAmount;
                        tblEVCPODDetailObj.DnsrNum = podVM.DnsrNum;
                        tblEVCPODDetailObj.ModifiedBy = loggedUserId;
                        tblEVCPODDetailObj.ModifiedDate = _currentDatetime;
                        _evcPoDDetailsRepository.Update(tblEVCPODDetailObj);
                    }
                    else
                    {
                        tblEVCPODDetailObj = new TblEvcpoddetail();
                        tblEVCPODDetailObj = _mapper.Map<PODViewModel, TblEvcpoddetail>(podVM);
                        tblEVCPODDetailObj.Podurl = fileName;
                        tblEVCPODDetailObj.IsActive = true;
                        tblEVCPODDetailObj.CreatedBy = loggedUserId;
                        tblEVCPODDetailObj.CreatedDate = _currentDatetime;
                        _evcPoDDetailsRepository.Create(tblEVCPODDetailObj);
                    }
                    result = _evcPoDDetailsRepository.SaveChanges();
                    if (result > 0)
                    {
                        evcPoDId = tblEVCPODDetailObj.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "ManageExchangePOD", ex);
            }
            return evcPoDId;
        }
        #endregion

        #region Store Logistic details of Exchange order database tblOrderLGC
        /// <summary>
        ///Exchange POD Details in database
        /// </summary>       
        /// <returns></returns>   
        public int ManageOrderLGC(OrderLGCViewModel orderLGCVM, int loggedUserId)
        {
            //TblOrderLgc tblOrderLgc = new TblOrderLgc();
            TblOrderLgc tblOrderLgc = null;
            TblExchangeOrder tblExchangeOrderObj = new TblExchangeOrder();
            int result = 0;
            try
            {
                if (orderLGCVM != null)
                {
                    if (orderLGCVM.OrderLgcid > 0)
                    {
                        tblOrderLgc = _orderLGCRepository.GetSingle(x => x.IsActive == true && x.OrderLgcid == orderLGCVM.OrderLgcid);
                    }
                    if (tblOrderLgc != null)
                    {
                        //used for LGC Drop
                        tblOrderLgc.StatusId = orderLGCVM.StatusId;
                        tblOrderLgc.ActualDropDate = _currentDatetime;
                        tblOrderLgc.Evcpodid = orderLGCVM.Evcpodid;
                        tblOrderLgc.ModifiedBy = loggedUserId;
                        tblOrderLgc.ModifiedDate = _currentDatetime;
                        _orderLGCRepository.Update(tblOrderLgc);
                    }
                    else
                    {
                        tblOrderLgc = new TblOrderLgc();
                        tblOrderLgc = _mapper.Map<OrderLGCViewModel, TblOrderLgc>(orderLGCVM);
                        tblOrderLgc.IsActive = true;
                        tblOrderLgc.CreatedBy = loggedUserId;
                        tblOrderLgc.CreatedDate = _currentDatetime;
                        _orderLGCRepository.Create(tblOrderLgc);
                    }
                    result = _orderLGCRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "ManageOrderLGC", ex);
            }

            return result;
        }
        #endregion

        #region get no. of image to be uploded by lgc pickup
        /// <summary> 
        /// get no. of image to be uploded by lgc pickup
        /// </summary>
        /// <param name="regdNo"></param>
        /// <returns>tblImageLabel</returns>
        public List<ImageLabelViewModel> GetImageLabelUploadByProductCat(string regdNo)
        {
            TblProductType tblProductType = null;
            List<ImageLabelViewModel> imageLabelViewModels = new List<ImageLabelViewModel>();
            List<TblImageLabelMaster> tblImageLabel = new List<TblImageLabelMaster>();
            #region Common Implementations for (ABB or Exchange)
            TblExchangeOrder tblExchangeOrder = null;
            TblAbbredemption tblAbbredemption = null;
            TblAbbregistration tblAbbregistration = null;
            int? productTypeId = null; 
            #endregion
            try
            {
                #region Common Implementations for (ABB or Exchange)
                TblOrderTran tblOrderTrans = _orderTransRepository.GetOrderTransByRegdno(regdNo);
                if (tblOrderTrans != null)
                {
                    if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                    {
                        tblExchangeOrder = tblOrderTrans.Exchange;
                        if (tblExchangeOrder != null)
                        {
                            productTypeId = tblExchangeOrder.ProductTypeId;
                        }
                    }
                    else if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.ABB))
                    {
                        tblAbbredemption = tblOrderTrans.Abbredemption;
                        if (tblAbbredemption != null)
                        {
                            tblAbbregistration = tblAbbredemption.Abbregistration;
                            if (tblAbbregistration != null)
                            {
                                productTypeId = tblAbbregistration.NewProductCategoryTypeId;
                            }
                        }
                    }
                }
                #endregion
                tblProductType = _productTypeRepository.GetSingle(x => x.Id == productTypeId && x.IsActive == true);
                if (tblProductType != null)
                {
                    tblImageLabel = _imageLabelRepository.GetList(x => x.ProductCatId == tblProductType.ProductCatId && x.IsActive == true).ToList();
                    if (tblImageLabel != null && tblImageLabel.Count > 0)
                    {
                        imageLabelViewModels = _mapper.Map<List<TblImageLabelMaster>, List<ImageLabelViewModel>>(tblImageLabel);
                    }
                }
                return imageLabelViewModels;
            }
            catch (Exception ex)
            {

                _logging.WriteErrorToDB("LogisticManager", "GetImageLabelUploadByProductCat", ex);
            }
            return imageLabelViewModels;
        }
        #endregion

        #region get images uploaded by QC team
        /// <summary>
        /// get images uploaded by QC team
        /// </summary>
        /// <param name="regdNo"></param>
        /// <returns></returns>
        public List<OrderImageUploadViewModel> GetImageUploadedByQC(string regdNo)
        {
            string imageUplodedBy = "QC Team";
            List<OrderImageUploadViewModel> orderImageUploadViewModels = null;
            List<TblOrderImageUpload> tblOrderImageUpload = new List<TblOrderImageUpload>();
            try
            {
                TblOrderTran tblOrderTran = _orderTransRepository.GetSingle(x => x.IsActive == true && x.RegdNo == regdNo);

                if (tblOrderTran != null)
                {
                    TblLoV tblLoV = _lovRepository.GetSingle(x => x.IsActive == true && x.LoVname.ToLower().Equals(imageUplodedBy.ToLower()));
                    if (tblLoV != null)
                    {
                        tblOrderImageUpload = _orderImageUploadRepository.GetList(x => x.IsActive == true && x.ImageUploadby == tblLoV.LoVid && x.OrderTransId == tblOrderTran.OrderTransId).ToList();
                        orderImageUploadViewModels = _mapper.Map<List<TblOrderImageUpload>, List<OrderImageUploadViewModel>>(tblOrderImageUpload);
                        return orderImageUploadViewModels;
                    }
                    return orderImageUploadViewModels;

                }
                return orderImageUploadViewModels;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "GetImageUploadedByQC", ex);
            }
            return orderImageUploadViewModels;
        }
        #endregion

        #region save details by lgc pickup
        /// <summary>
        /// save details by lgc pickup
        /// </summary>
        /// <param name="regdNo"></param>
        /// <returns></returns>
        public bool AddFinalQCImageToDB(IList<ImageLabelViewModel> imageLabelViewModels, LGCOrderViewModel lgcOrderViewModel, int loggedInUserId)
        {
            #region Variable Declaration
            TblOrderLgc? tblOrderLgc = null;
            TblExchangeAbbstatusHistory? tblExchangeAbbstatusHistory = null;
            int totalCount = 0;
            bool flag = false;
            string LGCType = "Pickup";
            int saveOrderLgc = 0;
            int? businessPartnerId = 0;
            TblOrderImageUpload? tblOrderImageUpload = null;
            TblBusinessPartner? tblBusinessPartner = null;
            TblVoucherVerfication? tblVoucherVerfication = null;
            #endregion

            #region Common Implementations for (ABB or Exchange)
            TblExchangeOrder tblExchangeOrder = null;
            TblAbbredemption tblAbbredemption = null;
            TblAbbregistration tblAbbregistration = null;
            string? regdNo = lgcOrderViewModel?.RegdNo; int? customerDetailsId = null; string sponsorOrderNumber = null;
            int? setStatusId = Convert.ToInt32(OrderStatusEnum.LGCPickup);
            //int? 
            #endregion

            try
            {

                #region Common Implementations for (ABB or Exchange)
                TblOrderTran tblOrderTrans = _orderTransRepository.GetOrderTransByRegdno(regdNo);
                if (tblOrderTrans != null)
                {
                    if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                    {
                        tblExchangeOrder = tblOrderTrans.Exchange;
                        if (tblExchangeOrder != null)
                        {
                            customerDetailsId = tblExchangeOrder.CustomerDetailsId;
                            sponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                            businessPartnerId = tblExchangeOrder.BusinessPartnerId;
                        }
                    }
                    else if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.ABB))
                    {
                        tblAbbredemption = tblOrderTrans.Abbredemption;
                        if (tblAbbredemption != null)
                        {
                            customerDetailsId = tblAbbredemption.CustomerDetailsId;
                            tblAbbregistration = tblAbbredemption.Abbregistration;
                            if (tblAbbregistration != null)
                            {
                                sponsorOrderNumber = tblAbbregistration.SponsorOrderNo;
                                businessPartnerId = tblAbbregistration.BusinessPartnerId;
                            }
                        }
                    }
                }
                #endregion

                TblLogistic tblLogistic = _logisticsRepository.GetSingle(x => x.IsActive == true && x.RegdNo == regdNo);
                TblLoV tblLoV = _lovRepository.GetSingle(x => x.IsActive == true && x.LoVname.ToLower().Equals(LGCType.ToLower()));
                if (tblLogistic != null)
                {
                    foreach (var items in imageLabelViewModels)
                    {
                        #region Save Images
                        items.FileName = items.RegdNo + "_" + "PickupImage" + totalCount + ".jpg";
                        if (items.Base64StringValue != null)
                        {
                            var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"\DBFiles\LGC\LGCPickup");
                            if (!Directory.Exists(filePath))
                            {
                                Directory.CreateDirectory(filePath);
                            }
                            var fileNameWithPath = string.Concat(filePath, "\\", items.FileName);
                            byte[] imageBytes = Convert.FromBase64String(items.Base64StringValue);
                            File.WriteAllBytes(fileNameWithPath, imageBytes);


                            #region Upload images in TblOrderImageUpload
                            tblOrderImageUpload = new TblOrderImageUpload();
                            tblOrderImageUpload.OrderTransId = tblOrderTrans.OrderTransId;
                            tblOrderImageUpload.ImageName = items.FileName;
                            tblOrderImageUpload.ImageUploadby = tblLoV.ParentId;
                            tblOrderImageUpload.LgcpickDrop = tblLoV.LoVname;
                            tblOrderImageUpload.IsActive = true;
                            tblOrderImageUpload.CreatedBy = loggedInUserId;
                            tblOrderImageUpload.CreatedDate = DateTime.Now;
                            _orderImageUploadRepository.Create(tblOrderImageUpload);
                            _orderImageUploadRepository.SaveChanges();
                            #endregion
                        }
                        #endregion


                        totalCount += 1;
                    }

                    #region insert details in tblorderlgc
                    if (tblOrderTrans != null && tblOrderTrans.OrderTransId > 0)
                    {
                        tblOrderLgc = _orderLGCRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == tblOrderTrans.OrderTransId);
                    }
                    if (tblOrderLgc != null && tblOrderLgc.OrderLgcid > 0)
                    {
                        tblOrderLgc.Lgccomments = lgcOrderViewModel.Lgccomments;
                        tblOrderLgc.ActualPickupDate = DateTime.Now;
                        tblOrderLgc.StatusId = Convert.ToInt32(OrderStatusEnum.LGCPickup);
                        tblOrderLgc.IsActive = true;
                        tblOrderLgc.ModifiedBy = loggedInUserId;
                        tblOrderLgc.ModifiedDate = DateTime.Now;
                        tblOrderLgc.LogisticId = tblLogistic.LogisticId;
                        tblOrderLgc.EvcregistrationId = lgcOrderViewModel.EVCRegistrationId;
                        tblOrderLgc.EvcpartnerId = lgcOrderViewModel.EvcpartnerId;
                        _orderLGCRepository.Update(tblOrderLgc);
                        saveOrderLgc = _orderLGCRepository.SaveChanges();
                    }
                    else
                    {
                        tblOrderLgc = new TblOrderLgc();
                        tblOrderLgc.OrderTransId = tblOrderTrans.OrderTransId;
                        tblOrderLgc.Lgccomments = lgcOrderViewModel.Lgccomments;
                        tblOrderLgc.ActualPickupDate = DateTime.Now;
                        tblOrderLgc.StatusId = setStatusId;
                        tblOrderLgc.IsActive = true;
                        tblOrderLgc.CreatedBy = loggedInUserId;
                        tblOrderLgc.CreatedDate = DateTime.Now;
                        //start Added By Priyanshi - for get data in descending order
                        tblOrderLgc.ModifiedBy = loggedInUserId;
                        tblOrderLgc.ModifiedDate = DateTime.Now;
                        //end
                        tblOrderLgc.LogisticId = tblLogistic.LogisticId;
                        tblOrderLgc.EvcregistrationId = lgcOrderViewModel.EVCRegistrationId;
                        tblOrderLgc.EvcpartnerId = lgcOrderViewModel.EvcpartnerId;
                        _orderLGCRepository.Create(tblOrderLgc);
                        saveOrderLgc = _orderLGCRepository.SaveChanges();
                    }
                    #endregion

                    #region Redeemed Mark on tblVoucherVerification if Order case is Deffered 
                    if (saveOrderLgc > 0 && tblExchangeOrder != null && tblExchangeOrder.IsDefferedSettlement == true)
                    {
                        tblBusinessPartner = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == tblExchangeOrder.BusinessPartnerId);
                        if (tblBusinessPartner != null && tblBusinessPartner.IsDefferedSettlement == true && tblBusinessPartner.IsVoucher == true && tblBusinessPartner.VoucherType == Convert.ToInt32(BusinessPartnerVoucherTypeEnum.Cash))
                        {
                            tblVoucherVerfication = _voucherRepository.GetSingle(x => x.IsActive == true && x.ExchangeOrderId == tblExchangeOrder.Id && x.BusinessPartnerId == tblBusinessPartner.BusinessPartnerId);
                            if (tblVoucherVerfication != null && tblVoucherVerfication.VoucherStatusId == Convert.ToInt32(VoucherStatusEnum.Captured))
                            {
                                tblVoucherVerfication.VoucherStatusId = Convert.ToInt32(VoucherStatusEnum.Redeemed);
                                tblVoucherVerfication.ModifiedBy = loggedInUserId;
                                tblVoucherVerfication.ModifiedDate = _currentDatetime;
                                _voucherRepository.Update(tblVoucherVerfication);
                                _voucherRepository.SaveChanges();
                            }
                        }
                    }
                    #endregion

                    #region update statusId in tbllogistic
                    tblLogistic.StatusId = Convert.ToInt32(OrderStatusEnum.LGCPickup);
                    tblLogistic.Modifiedby = loggedInUserId;
                    tblLogistic.ModifiedDate = _currentDatetime;
                    _logisticsRepository.Update(tblLogistic);
                    _logisticsRepository.SaveChanges();
                    #endregion

                    #region update statusId in Base tbl Exchange or ABB
                    if (tblExchangeOrder != null)
                    {
                        tblExchangeOrder.StatusId = Convert.ToInt32(OrderStatusEnum.LGCPickup);
                        tblExchangeOrder.OrderStatus = "Pickup";
                        tblExchangeOrder.ModifiedBy = loggedInUserId;
                        tblExchangeOrder.ModifiedDate = _currentDatetime;
                        _exchangeOrderRepository.Update(tblExchangeOrder);
                        _exchangeOrderRepository.SaveChanges();
                    }
                    else if (tblAbbredemption != null && tblAbbregistration != null)
                    {
                        #region update status on tblAbbRedemption
                        tblAbbredemption.StatusId = setStatusId;
                        tblAbbredemption.AbbredemptionStatus = "Pickup";
                        tblAbbredemption.ModifiedBy = loggedInUserId;
                        tblAbbredemption.ModifiedDate = _currentDatetime;
                        _abbRedemptionRepository.Update(tblAbbredemption);
                        _abbRedemptionRepository.SaveChanges();
                        #endregion
                    }
                    #endregion

                    #region update statusId in tblOrderTrans
                    tblOrderTrans.StatusId = setStatusId;
                    tblOrderTrans.ModifiedBy = loggedInUserId;
                    tblOrderTrans.ModifiedDate = _currentDatetime;
                    _orderTransRepository.Update(tblOrderTrans);
                    _orderTransRepository.SaveChanges();
                    #endregion

                    #region Insert into tblexchangeabbhistory
                    tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                    tblExchangeAbbstatusHistory.OrderType = (int)tblOrderTrans.OrderType;
                    tblExchangeAbbstatusHistory.SponsorOrderNumber = sponsorOrderNumber;
                    tblExchangeAbbstatusHistory.RegdNo = regdNo;
                    tblExchangeAbbstatusHistory.CustId = customerDetailsId;
                    tblExchangeAbbstatusHistory.StatusId = setStatusId;
                    tblExchangeAbbstatusHistory.IsActive = true;
                    tblExchangeAbbstatusHistory.CreatedBy = loggedInUserId;
                    tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                    tblExchangeAbbstatusHistory.OrderTransId = tblOrderTrans.OrderTransId;
                    _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                    #endregion

                    #region Update StatusId in Tbl WalletTransection 
                    TblWalletTransaction tblWalletTransaction = _walletTransactionRepository.GetSingle(x => x.IsActive == true && x.RegdNo == regdNo);
                    tblWalletTransaction.StatusId = setStatusId.ToString();
                    tblWalletTransaction.ModifiedBy = loggedInUserId;
                    tblWalletTransaction.ModifiedDate = _currentDatetime;
                    _walletTransactionRepository.Update(tblWalletTransaction);
                    _walletTransactionRepository.SaveChanges();
                    #endregion

                    #region Generate and Save Customer Declaration

                    tblBusinessPartner = _businessPartnerRepository.GetById(businessPartnerId);
                    if(tblBusinessPartner != null)
                    {
                        if(tblBusinessPartner.IsDefaultPickupAddress == null)
                        {
                            if (lgcOrderViewModel != null)
                            {
                                string custDeclartionpdfname = lgcOrderViewModel.RegdNo + "_" + "custdeclarationpdf" + ".pdf";
                                string custDeclartionfilepath = @"\dbfiles\evc\CustomerDeclaration";
                                string custDeclartionhtmlstring = GetCustDeclartionhtmlstring(lgcOrderViewModel, "customer_declaration");
                                bool podpdfsave = _htmlToPdfConverterHelper.GeneratePDF(custDeclartionhtmlstring, custDeclartionfilepath, custDeclartionpdfname);
                                tblOrderLgc.CustDeclartionpdfname = custDeclartionpdfname != null ? custDeclartionpdfname : string.Empty;
                                _orderLGCRepository.Update(tblOrderLgc);
                                _orderLGCRepository.SaveChanges();
                            }
                        }
                    }
                    
                    #endregion

                    return flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "AddFinalQCImageToDB", ex);
            }
            return flag;
        }
        #endregion

        #region Create pdf String for DebitNote, Invoice, POD New Method with Bunch of Rnum
        /// <summary> 
        /// Create pdf String for DebitNote, Invoice, POD
        /// </summary>
        /// <param name="podVM"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns>htmlString</returns>
        public string GetPoDHtmlString(PODViewModel podVM, string HtmlTemplateNameOnly)
        {
            #region Variable Declaration
            string htmlString = "";
            string fileName = HtmlTemplateNameOnly + ".html";
            string fileNameWithPath = "";
            #endregion 
            try
            {
                #region Variable Initialization and Price Calculation
                string? baseUrl = _baseConfig.Value.BaseURL;
                var evcVM = podVM.evcDetailsVM;
                podVM.UtcSeel_INV = baseUrl + EnumHelper.DescriptionAttr(FileAddressEnum.UTCACSeel);
                evcVM.PlaceOfSupply = evcVM.State;
                evcVM.CurrentDate = Convert.ToDateTime(_currentDatetime).ToString("dd/MM/yyyy");
                podVM.CurrentDate = evcVM.CurrentDate;
                evcVM.FinalPriceInWordsDN = NumberToWordsConverterHelper.ConvertAmount(Convert.ToDecimal(evcVM.FinalPriceDN??0));
                evcVM.FinalPriceInWordsInv = NumberToWordsConverterHelper.ConvertAmount(Convert.ToDecimal(evcVM.FinalPriceInv??0));
                #endregion

                #region Get Html String Dynamically
                var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"\PdfTemplates");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath); //Create directory if it doesn't exist
                }
                fileNameWithPath = string.Concat(filePath, "\\", fileName);

                htmlString = File.ReadAllText(fileNameWithPath);
                #endregion

                #region Code for Replace EVC Details
                if (HtmlTemplateNameOnly == "EVC_Debit_Note" || HtmlTemplateNameOnly == "EVC_Invoice")
                {
                    htmlString = htmlString.Replace("demoEVCName", evcVM?.Name + "-" + evcVM?.EvcStoreCode)
                            .Replace("demoAddress", evcVM?.Address).Replace("demoCity", evcVM?.City)
                            .Replace("demoState", evcVM?.State).Replace("demoPincode", evcVM?.Pincode)
                            .Replace("demoPlaceOfSupply", evcVM?.PlaceOfSupply).Replace("demoCurrentDate", evcVM?.CurrentDate)
                            .Replace("demoUtcSeel_INV", podVM.UtcSeel_INV);
                }
                #endregion

                #region Code for Create Bunch of Rnum for PoD, DebitNote and Invoice template
                string RnumBunch = "";
                if (podVM.podVMList != null && podVM.podVMList.Count > 0)
                {
                    if (HtmlTemplateNameOnly == "POD")
                    {
                        foreach (var podItem in podVM.podVMList)
                        {
                            RnumBunch += "<tr>" +
                                "<td> " + podItem.EVCBussinessName + " </td>" +
                        "<td> " + podItem.EVCRegdNo + " </td>" +
                        "<td> " + podItem.TicketNo + " </td>" +
                        "<td> " + podItem.RegdNo + " </td>" +
                     "<td> " + podItem.ProductCatName + " </td>" +
                     "<td> " + podItem.CustName + " </td>" +
                     "<td> " + podItem.CustPincode + " </td>" +
                     "<td> Delivered </td>" +
                     "<td> " + podVM.CurrentDate + " </td><td></td><tr>";
                        }
                    }
                    else if (HtmlTemplateNameOnly == "EVC_Debit_Note")
                    {
                        int srNum = 1;
                        foreach (var podItem in podVM.podVMList)
                        {
                            if (podItem != null && !Convert.ToBoolean(podItem.IsDebitNoteSkiped ?? false))
                            {
                                podItem.FullPoDUrl = baseUrl + podItem.Podurl;
                                RnumBunch += "<tr><td> " + srNum + " </td>" +
                                "<td> " + podItem.RegdNo + " <a href=" + podItem.FullPoDUrl + " target='_blanck'>" + podItem.FullPoDUrl + "</a> " + " </td>" +
                                "<td>1.00</td>" +
                                "<td> " + podItem.OrderAmtForEVCDN + " </td>" +
                                "<td> " + podItem.OrderAmtForEVCDN + " </td></tr>";
                                srNum++;
                            }
                        }
                        RnumBunch += "<tr><td> " + srNum + " </td>" +
                        "<td> " + "Bonus to Customer" + " </td>" +
                        "<td></td>" +
                        "<td> " + (evcVM?.TotalSweetenerAmt ?? 0) + " </td>" +
                        "<td> " + (evcVM?.TotalSweetenerAmt ?? 0) + " </td></tr>";
                    }
                    else if (HtmlTemplateNameOnly == "EVC_Invoice")
                    {
                        int srNum = 1;
                        foreach (var podItem in podVM.podVMList)
                        {
                            podItem.FullPoDUrl = baseUrl + podItem.Podurl;
                            //Create List with Multiple orders
                            RnumBunch += "<tr><td> " + srNum + " </td>" +
                               "<td><span>UTC Bridge Service Fees For Regd No : </span>" + podItem.RegdNo + " <a href=" + podItem.FullPoDUrl + " target='_blanck'>" + podItem.FullPoDUrl + "</a> " + " </td>" +
                                "<td>9961</td>" +
                                "<td>1.00</td>" +
                                "<td>" + podItem.BaseValue + "</td>" +
                                "<td>9%</td>" +
                                "<td>" + podItem.Cgstamt + "</td>" +
                                "<td>9%</td>" +
                                "<td>" + podItem.Sgstamt + "</td>" +
                                "<td>" + podItem.OrderAmtForEVCInv + " </td><tr>";
                            srNum++;
                        }
                    }
                    htmlString = htmlString.Replace("demoRnumList", RnumBunch);
                }
                #endregion

                #region Replace final price and BillNumber in Debit Note Or Invoice
                string GSTDisplay = ""; string ShippingCostDisplay = ""; string primeProdAmtDisplay = "";
                if (HtmlTemplateNameOnly == "EVC_Debit_Note")
                {
                    htmlString = htmlString.Replace("demoBillNumber", evcVM?.BillNumberDN)
                        .Replace("demoFinalPrice", evcVM?.FinalPriceDN?.ToString())
                        .Replace("demoFinalAmtInWords", evcVM?.FinalPriceInWordsDN?.ToString());
                }
                else if (HtmlTemplateNameOnly == "EVC_Invoice")
                {
                    GSTDisplay += "<tr><td>CGST9 (9%)</td><td>" + evcVM?.TotalCgstamt?.ToString() + "</td></tr>" +
                        "<tr><td>SGST9 (9%)</td><td>" + evcVM?.TotalSgstamt?.ToString() + "</td></tr>";
                    if (podVM.IsOrderAmtWithSweetener == true)
                    {
                        ShippingCostDisplay += "<tr><td>Shipping Cost </td><td>" + evcVM?.TotalLgccost?.ToString() + "</td></tr>";
                    }
                    if (evcVM?.TotalPrimeProductDiffAmt > 0)
                    {
                        primeProdAmtDisplay += "<tr><td>Additional Cost "
                            + "<span style=\"font-size: 11px; display: block; color: #aaa;\">(Prime Product Charges)</span>"
                            + "</td><td>" + evcVM?.TotalPrimeProductDiffAmt?.ToString() 
                            + "</td></tr>";
                    }
                    htmlString = htmlString.Replace("demoBillNumber", evcVM?.BillNumberInv)
                        .Replace("demoGSTDisplay", GSTDisplay).Replace("demoShippingCostDisplay", ShippingCostDisplay)
                        .Replace("demoAdditionalCost", primeProdAmtDisplay)
                        .Replace("demoPriceAfterAllGST", evcVM?.FinalPriceInv?.ToString())
                        .Replace("demoSubtotal", evcVM?.FinalPriceInv?.ToString())
                        .Replace("demoFinalPrice", evcVM?.FinalPriceInv?.ToString())
                        .Replace("demoGstType", podVM?.GstType?.ToString())
                        .Replace("demoFinalAmtInWords", evcVM?.FinalPriceInWordsInv?.ToString());
                }
                #endregion
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "GetPoDHtmlString", ex);
            }
            return htmlString;
        }
        #endregion

        #region create pdf string for Customer Declartion
        /// <summary>
        /// create pdf string for Customer Declartion
        /// </summary>
        /// <param name="orderLGCViewModel"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns></returns>
        public string GetCustDeclartionhtmlstring(LGCOrderViewModel lGCOrderViewModel, string HtmlTemplateNameOnly)
        {
            var DateTime = System.DateTime.Now;
            string FinalDate = DateTime.Date.ToShortDateString();
            string htmlString = "";
            string fileName = HtmlTemplateNameOnly + ".html";
            string fileNameWithPath = "";
            try
            {
                var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"\PdfTemplates");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                fileNameWithPath = string.Concat(filePath, "\\", fileName);
                htmlString = File.ReadAllText(fileNameWithPath);
                if (HtmlTemplateNameOnly == "customer_declaration")
                    htmlString = htmlString.Replace("[Regd_No]", lGCOrderViewModel.RegdNo)
                        .Replace("[Customer_Name]", lGCOrderViewModel.CustomerName)
                        .Replace("[Customer_Address_1]", lGCOrderViewModel.tblCustomerDetail.Address1)
                        .Replace("[Customer_City]", lGCOrderViewModel.tblCustomerDetail.City)
                        .Replace("[Customer_Pincode]", lGCOrderViewModel.tblCustomerDetail.ZipCode.ToString())
                        .Replace("[Short_Number]", lGCOrderViewModel.tblCustomerDetail.PhoneNumber)
                        .Replace("[Sponsor_Name]", lGCOrderViewModel.SponsorName)
                        .Replace("[New_Prod_Group]", lGCOrderViewModel.ProductCategory)
                        .Replace("[New_Product_Type]", lGCOrderViewModel.ProductType)
                        .Replace("[EVC_Bussiness_Name]", lGCOrderViewModel.EVCBusinessName)
                        .Replace("[Old_Brand_Name]", lGCOrderViewModel.BrandName)
                        .Replace("[date]", FinalDate);

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "GetCustDeclartionhtmlstring", ex);
            }
            return htmlString;
        }
        #endregion

        #region add statusid to tblorderlgc & tblexchange for rejected order at pickup
        /// <summary>
        /// add statusid to tblorderlgc & tblexchange for rejected order at pickup
        /// </summary>
        /// <returns>flag</returns>
        public bool AddRejectedOrderStatusToDB(string regdNo, string Commant, int loggedInUserId)
        {
            TblOrderLgc? tblOrderLgc = null;
            TblExchangeAbbstatusHistory? tblExchangeAbbstatusHistory = null;

            #region Common Implementations for (ABB or Exchange)
            TblExchangeOrder? tblExchangeOrder = null;
            TblAbbredemption? tblAbbredemption = null;
            TblAbbregistration? tblAbbregistration = null;
            int? customerDetailsId = null; string? sponsorOrderNumber = null; int orderType = 0;
            int? setStatusId = Convert.ToInt32(OrderStatusEnum.PickupDecline);
            #endregion

            bool flag = false;
            try
            {
                #region Common Implementations for (ABB or Exchange)
                TblOrderTran tblOrderTrans = _orderTransRepository.GetOrderTransByRegdno(regdNo);
                if (tblOrderTrans != null)
                {
                    orderType = (int)tblOrderTrans.OrderType;
                    if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                    {
                        tblExchangeOrder = tblOrderTrans.Exchange;
                        if (tblExchangeOrder != null)
                        {
                            regdNo = tblExchangeOrder.RegdNo;
                            customerDetailsId = tblExchangeOrder.CustomerDetailsId;
                            sponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                        }
                    }
                    else if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.ABB))
                    {
                        tblAbbredemption = tblOrderTrans.Abbredemption;
                        if (tblAbbredemption != null)
                        {
                            customerDetailsId = tblAbbredemption.CustomerDetailsId;
                            tblAbbregistration = tblAbbredemption.Abbregistration;
                            if (tblAbbregistration != null)
                            {
                                regdNo = tblAbbregistration.RegdNo;
                                sponsorOrderNumber = tblAbbregistration.SponsorOrderNo;
                            }
                        }
                    }
                }
                #endregion

                TblLogistic tblLogistic = _logisticsRepository.GetSingle(x => x.IsActive == true && x.RegdNo == regdNo);
                if (tblLogistic != null)
                {
                    TblServicePartner? tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == tblLogistic.ServicePartnerId).FirstOrDefault();
                    
                    if (tblOrderTrans != null && tblOrderTrans.OrderTransId > 0)
                    {
                        #region Update StatusId in Tbl WalletTransection 
                        TblWalletTransaction? tblWalletTransaction = _walletTransactionRepository.GetSingle(x => x.IsActive == true && x.RegdNo == regdNo);
                        if (tblWalletTransaction != null)
                        {
                            tblWalletTransaction.StatusId = setStatusId.ToString();
                            tblWalletTransaction.ModifiedBy = loggedInUserId;
                            tblWalletTransaction.ModifiedDate = _currentDatetime;
                            _walletTransactionRepository.Update(tblWalletTransaction);
                            _walletTransactionRepository.SaveChanges();
                        }
                        #endregion

                        tblOrderLgc = _orderLGCRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == tblOrderTrans.OrderTransId);
                        if (tblOrderLgc != null && tblOrderLgc.OrderLgcid > 0)
                        {
                            tblOrderLgc.StatusId = setStatusId;
                            tblOrderLgc.ModifiedBy = loggedInUserId;
                            tblOrderLgc.ModifiedDate = _currentDatetime;
                            tblOrderLgc.LogisticId = tblLogistic.LogisticId;
                            tblOrderLgc.Lgccomments = Commant;
                            tblOrderLgc.EvcregistrationId = tblWalletTransaction?.EvcregistrationId;
                            tblOrderLgc.EvcpartnerId = tblWalletTransaction?.EvcpartnerId;
                            _orderLGCRepository.Update(tblOrderLgc);
                        }
                        else
                        {
                            tblOrderLgc = new TblOrderLgc();
                            tblOrderLgc.OrderTransId = tblOrderTrans.OrderTransId;
                            tblOrderLgc.StatusId = setStatusId;
                            tblOrderLgc.IsActive = true;
                            tblOrderLgc.CreatedBy = loggedInUserId;
                            tblOrderLgc.CreatedDate = _currentDatetime;                          
                            tblOrderLgc.LogisticId = tblLogistic.LogisticId;
                            tblOrderLgc.Lgccomments = Commant;
                            tblOrderLgc.EvcregistrationId = tblWalletTransaction?.EvcregistrationId;
                            tblOrderLgc.EvcpartnerId = tblWalletTransaction?.EvcpartnerId;
                            tblOrderLgc.ModifiedBy = loggedInUserId;
                            tblOrderLgc.ModifiedDate = _currentDatetime;
                            _orderLGCRepository.Create(tblOrderLgc);
                        }
                        _orderLGCRepository.SaveChanges();

                        #region update statusid in tbllogistics
                        tblLogistic.StatusId = setStatusId;
                        //Priyanshi for use (LGC Reopen Task)
                        tblLogistic.IsActive = false;
                        tblLogistic.Modifiedby = loggedInUserId;
                        tblLogistic.ModifiedDate = _currentDatetime;
                        _logisticsRepository.Update(tblLogistic);
                        _logisticsRepository.SaveChanges();
                        #endregion

                        #region update statusId in Base tbl Exchange or ABB
                        if (tblExchangeOrder != null)
                        {
                            tblExchangeOrder.StatusId = setStatusId;
                            tblExchangeOrder.OrderStatus = "Pickup";
                            tblExchangeOrder.ModifiedBy = loggedInUserId;
                            tblExchangeOrder.ModifiedDate = _currentDatetime;
                            _exchangeOrderRepository.Update(tblExchangeOrder);
                            _exchangeOrderRepository.SaveChanges();
                        }
                        else if (tblAbbredemption != null && tblAbbregistration != null)
                        {
                            #region update status on tblAbbRedemption
                            tblAbbredemption.StatusId = setStatusId;
                            tblAbbredemption.AbbredemptionStatus = "Pickup";
                            tblAbbredemption.ModifiedBy = loggedInUserId;
                            tblAbbredemption.ModifiedDate = _currentDatetime;
                            _abbRedemptionRepository.Update(tblAbbredemption);
                            _abbRedemptionRepository.SaveChanges();
                            #endregion
                        }
                        #endregion

                        #region Insert into tblexchangeabbhistory
                        tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                        tblExchangeAbbstatusHistory.OrderType = (int)tblOrderTrans.OrderType;
                        tblExchangeAbbstatusHistory.SponsorOrderNumber = sponsorOrderNumber;
                        tblExchangeAbbstatusHistory.RegdNo = regdNo;
                        tblExchangeAbbstatusHistory.CustId = customerDetailsId;
                        tblExchangeAbbstatusHistory.StatusId = setStatusId;
                        tblExchangeAbbstatusHistory.IsActive = true;
                        tblExchangeAbbstatusHistory.CreatedBy = loggedInUserId;
                        tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                        tblExchangeAbbstatusHistory.OrderTransId = tblOrderTrans.OrderTransId;
                        tblExchangeAbbstatusHistory.Comment = Commant != null ? "TicketNo-" + tblLogistic.TicketNumber + "- Service Partner Name -" + tblServicePartner.ServicePartnerDescription + "Comment -" + Commant : string.Empty;
                        _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                        #endregion

                        #region update statusId in tblOrderTrans
                        tblOrderTrans.StatusId = setStatusId;
                        tblOrderTrans.ModifiedBy = loggedInUserId;
                        tblOrderTrans.ModifiedDate = _currentDatetime;
                        _orderTransRepository.Update(tblOrderTrans);
                        _orderTransRepository.SaveChanges();
                        #endregion

                        #region update TblVehicleJourneyTrackingDetails by Priyanshi date 6/09/2023
                        TblVehicleJourneyTrackingDetail? tblVehicleJourneyTrackingDetail = _vehicleJourneyTrackingDetails.GetSingle(x => x.IsActive == true && x.OrderTransId == tblOrderTrans.OrderTransId);
                        if (tblVehicleJourneyTrackingDetail != null)
                        {
                            tblVehicleJourneyTrackingDetail.IsActive = false;
                            tblVehicleJourneyTrackingDetail.EstimateEarning = 0;
                            tblVehicleJourneyTrackingDetail.StatusId = setStatusId;
                            tblVehicleJourneyTrackingDetail.ModifiedBy = loggedInUserId;
                            tblVehicleJourneyTrackingDetail.ModifiedDate = _currentDatetime;
                            _vehicleJourneyTrackingDetails.Update(tblVehicleJourneyTrackingDetail);
                            _vehicleJourneyTrackingDetails.SaveChanges();
                           
                        }
                        #endregion

                        return flag = true;
                    }
                    else
                    {
                        return flag;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCManager", "AddRejectedOrderStatusToDB", ex);
            }
            return flag;
        }
        #endregion

        #region get list of city & EVC with respect to UserId
        public List<TblOrderLgc> GetCityAndEvcList(int UserId)
        {
            List<TblOrderLgc>? tblOrderLgcList = null;
            List<SelectListItem> CityList = new List<SelectListItem>();
            int statusId = 0;
            try
            {
                statusId = Convert.ToInt32(OrderStatusEnum.LGCPickup);
                tblOrderLgcList = _orderLGCRepository.GetCityAndEvcList(UserId, statusId);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "GetCityAndEvcList", ex);
            }
            return tblOrderLgcList;

        }
        #endregion

        #region save details of driver along with list of loads
        /// <summary>
        /// save details of driver along with list of loads
        /// </summary>
        /// <param name="driverDetailsViewModel"></param>
        /// <returns>flag</returns>
        public bool saveListOfLoads(DriverDetailsViewModel driverDetailsViewModel, int LoggedInUserId)
        {
            int count = 0;
            TblDriverDetail tblDriverDetail = null;
            TblOrderLgc tblOrderLgc = null;
            try
            {
                if (driverDetailsViewModel != null)
                {
                    tblDriverDetail = new TblDriverDetail();
                    tblDriverDetail.DriverName = driverDetailsViewModel.DriverName;
                    tblDriverDetail.DriverPhoneNumber = driverDetailsViewModel.DriverPhoneNumber;
                    tblDriverDetail.VehicleNumber = driverDetailsViewModel.VehicleNumber;
                    if (driverDetailsViewModel.City != null)
                    {
                        driverDetailsViewModel.City = _cityRepository.GetSingle(x => x.IsActive == true && x.CityId.ToString() == driverDetailsViewModel.City.ToString()).Name;
                    }
                    /*tblDriverDetail.City = driverDetailsViewModel.City;
                    */
                    tblDriverDetail.IsActive = true;
                    tblDriverDetail.CreatedBy = LoggedInUserId;
                    tblDriverDetail.CreatedDate = _currentDatetime;
                    _driverDetailsRepository.Create(tblDriverDetail);
                    _driverDetailsRepository.SaveChanges();
                    if (tblDriverDetail != null)
                    {
                        if (driverDetailsViewModel.OrderTransId != null && driverDetailsViewModel.OrderTransId[0] != null)
                        {
                            string Values = string.Join(",", Array.ConvertAll(driverDetailsViewModel.OrderTransId, x => x.ToString()));
                            string[] OrderTranIDList = Values.Split(",");
                            for (int i = 0; i < OrderTranIDList.Count(); i++)
                            {
                                tblOrderLgc = _orderLGCRepository.GetOrderLGCByOrderTransId(Convert.ToInt32(OrderTranIDList[i]), Convert.ToInt32(OrderStatusEnum.LGCPickup));
                                tblOrderLgc.DriverDetailsId = tblDriverDetail.DriverDetailsId;
                                tblOrderLgc.ModifiedBy = LoggedInUserId;
                                tblOrderLgc.ModifiedDate = _currentDatetime;
                                _orderLGCRepository.Update(tblOrderLgc);
                                _orderLGCRepository.SaveChanges();
                                count += 1;
                            }
                            if (count == OrderTranIDList.Count())
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "saveListOfLoads", ex);
            }

            return false;
        }
        #endregion

        #region get evclist by cityId
        /// <summary>
        /// get evclist by cityId
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public JsonResult getEvcListByCityId(int cityId)
        {
            IEnumerable<SelectListItem> evcList = null;
            List<TblEvcregistration> tblEvcregistrationsList = null;

            try
            {
                tblEvcregistrationsList = _digi2L_DevContext.TblEvcregistrations.Where(x => x.IsActive == true && x.CityId == cityId).ToList();
                evcList = (tblEvcregistrationsList).AsEnumerable().Select(prodt => new SelectListItem() { Text = prodt.BussinessName.ToString(), Value = prodt.EvcregistrationId.ToString() });
                evcList = evcList.OrderBy(o => o.Text).ToList();
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "getEvcListByCityId", ex);
            }
            var result = new SelectList(evcList, "Value", "Text");
            return new JsonResult(result);
        }
        #endregion

        #region Get OrderLGC List By DriverId and EVCId
        public List<TblOrderLgc> GetOrderLGCListByDriverIdEVCId(int? DriverId, int? EVCRegistrationId)
        {
            List<TblOrderLgc> tblOrderLgcList = null;
            try
            {
                if (DriverId != null && EVCRegistrationId != null)
                {
                    tblOrderLgcList = _orderLGCRepository.GetList(x => x.IsActive == true && x.DriverDetailsId == DriverId && x.EvcregistrationId == EVCRegistrationId).ToList();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "GetOrderLGCListByDriverIdEVCId", ex);
            }
            return tblOrderLgcList;
        }
        #endregion

        #region Create By Priyanshi --LGC Admin 
        #region select Company
        public IList<TblServicePartner> SelectServicePartner()
        {           
            List<TblServicePartner> tblServicePartner = new List<TblServicePartner>();

            tblServicePartner = _servicePartnerRepository.GetList(x => x.IsActive == true&&x.ServicePartnerIsApprovrd==true&&x.IsServicePartnerLocal!=null).ToList();
            return tblServicePartner;
        }
        #endregion

        #region servicePartnerDashboard
        public ServicePartnerDashboardViewModel servicePartnerDashboard(int? ServicePartnerId)
        {
            ServicePartnerDashboardViewModel servicePartnerDashboardViewModel = new ServicePartnerDashboardViewModel();
            List<TblLogistic> tblLogistic = new List<TblLogistic>();
            List<TblOrderLgc> tblOrderLgc = new List<TblOrderLgc>();
            List<TblOrderTran> tblOrderTrans = new List<TblOrderTran>();

            #region tblLogistic and tblOrderLgc
            tblLogistic = _context.TblLogistics.Include(x => x.OrderTrans)
                    .Include(x => x.ServicePartner)
                    .Where(x => x.IsActive == true && x.OrderTrans.StatusId != null && x.OrderTrans != null
                    && ((ServicePartnerId == null || ServicePartnerId == 0) || (ServicePartnerId > 0 && x.ServicePartnerId == ServicePartnerId))
                    ).ToList();

            tblOrderLgc = _context.TblOrderLgcs.Include(x => x.Logistic).Include(x => x.OrderTrans).Include(x => x.Evcregistration)
                .Where(x => x.IsActive == true && x.Logistic != null && x.OrderTrans != null && x.OrderTrans.IsActive == true && x.OrderTrans.StatusId != null
                && ((ServicePartnerId == null || ServicePartnerId == 0) || (ServicePartnerId > 0 && x.Logistic.ServicePartnerId == ServicePartnerId))
                ).ToList();
            #endregion

            if (tblLogistic != null)
            {
                servicePartnerDashboardViewModel.ReadyForPickup = tblLogistic.Count(x => x.StatusId == 18 &&x.OrderTrans.StatusId==18);
                servicePartnerDashboardViewModel.PickupDone = tblOrderLgc.Count(x => x.OrderTrans.StatusId == 23&&x.StatusId==23&&x.Logistic.StatusId==23 && x.DriverDetailsId == null && x.Logistic.IsActive == true);
                servicePartnerDashboardViewModel.LoadDone = tblOrderLgc.Count(x => x.OrderTrans.StatusId == 23 && x.StatusId == 23 && x.Logistic.StatusId == 23 && x.DriverDetailsId != null && x.Logistic.IsActive == true);
                servicePartnerDashboardViewModel.PickupDecline = tblOrderLgc.Count(x => x.OrderTrans.StatusId == 26 && x.StatusId==26&&x.Logistic.StatusId==26 && x.Logistic.IsActive == false);
                servicePartnerDashboardViewModel.Drop = tblOrderLgc.Count(x => x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.Posted) && x.DriverDetailsId != null && x.StatusId == Convert.ToInt32(OrderStatusEnum.Posted) && x.Logistic.StatusId == Convert.ToInt32(OrderStatusEnum.Posted) && x.Logistic.IsActive == true&&x.Evcregistration!=null);
                servicePartnerDashboardViewModel.ServicePartnerId = ServicePartnerId > 0 ? ServicePartnerId : 0;
            }
            return servicePartnerDashboardViewModel;
        }
        #endregion
        #endregion

        #region ADDED BY Ashwin _LGC/Registation  
        /// <summary>
        /// Used in api of LGC Registeration in mobile development
        /// </summary>
        /// <param name="LGCRegisterationModel"></param>
        /// <param name=""></param>
        /// <returns>tblServicePartner</returns>
        public ResponseResult LGCRegister(RegisterServicePartnerDataModel LGCRegisterationModel)
        {
            TblEvcregistration TblEvcregistration = new TblEvcregistration();

            TblServicePartner tblServicePartner = new TblServicePartner();         
            int userId = 3;           
            ResponseResult responseMessage = new ResponseResult();
            try
            {
                if (LGCRegisterationModel != null)
                {
                    tblServicePartner = _mapper.Map<RegisterServicePartnerDataModel, TblServicePartner>(LGCRegisterationModel);

                    if (tblServicePartner != null)
                    {
                        //Code to Insert the object 
                        tblServicePartner.IsActive = true;
                        tblServicePartner.CreatedDate = _currentDatetime;
                        tblServicePartner.CreatedBy = userId;

                        _servicePartnerRepository.Create(tblServicePartner);
                        int result = _servicePartnerRepository.SaveChanges();
                        if (result == 1)
                        {
                            //response = true;
                            responseMessage.message = "Regisiteration Success";
                            responseMessage.Status = true;
                        }
                        else
                        {
                            responseMessage.message = "Regisiteration failed";
                            responseMessage.Status = false;
                        }

                    }
                    else
                    {
                        responseMessage.message = "Data Not Map properply";
                        responseMessage.Status = false;
                    }

                }
            }
            catch (Exception ex)
            {
                responseMessage.message = ex.Message;
                responseMessage.Status = false;
                _logging.WriteErrorToDB("LogisticManager", "LGCRegister", ex);
                return responseMessage;
            }
            return responseMessage;
        }
        #endregion

        #region Added By Priyanshi --LGC_Admin ReOpen Task (Optimized and Modified by VK for ABB Redemption Date : 28-June-2023)
        public bool ReOpenLGCOrder(int OrderTransId, int loggedInUserId, string Comment)
        {
            #region Variable Declaration
            TblOrderLgc tblOrderLgc = null;
            TblExchangeOrder tblExchangeOrder = null;
            TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = null;
            TblWalletTransaction tblWalletTransaction = null;
            TblOrderTran tblOrderTrans = null;
            //TblLogistic tblLogistic = null;
            // TblOrderLgc tblOrderLGC = null;
            int statusId = Convert.ToInt32(OrderStatusEnum.ReopenforLogistics);
            bool flag = false;
            #endregion

            #region Variables for (ABB or Exchange)
            TblAbbredemption tblAbbredemption = null;
            TblAbbregistration tblAbbregistration = null; int orderType = 0;
            string regdNo = null; int? customerDetailsId = null; string sponsorOrderNumber = null;
            #endregion

            try
            {
                #region Common Implementations for (ABB or Exchange)
                tblOrderTrans = _orderTransRepository.GetOrderDetailsByOrderTransId(OrderTransId);
                if (tblOrderTrans != null)
                {
                    orderType = (int)tblOrderTrans.OrderType;
                    if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                    {
                        tblExchangeOrder = tblOrderTrans.Exchange;
                        if (tblExchangeOrder != null)
                        {
                            regdNo = tblExchangeOrder.RegdNo;
                            customerDetailsId = tblExchangeOrder.CustomerDetailsId;
                            sponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                        }
                    }
                    else if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.ABB))
                    {
                        tblAbbredemption = tblOrderTrans.Abbredemption;
                        if (tblAbbredemption != null)
                        {
                            customerDetailsId = tblAbbredemption.CustomerDetailsId;
                            tblAbbregistration = tblAbbredemption.Abbregistration;
                            if (tblAbbregistration != null)
                            {
                                regdNo = tblAbbregistration.RegdNo;
                                sponsorOrderNumber = tblAbbregistration.SponsorOrderNo;
                            }
                        }
                    }
                }
                #endregion

                tblOrderLgc = _orderLGCRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == OrderTransId);

                if (tblOrderLgc != null)
                {
                    #region Update tblOrderLgc
                    var test = tblOrderLgc;
                    tblOrderLgc = new TblOrderLgc();
                    tblOrderLgc = test;
                    tblOrderLgc.StatusId = statusId;
                    tblOrderLgc.Lgccomments = Comment;
                    tblOrderLgc.ModifiedBy = loggedInUserId;
                    tblOrderLgc.ModifiedDate = _currentDatetime;
                    _orderLGCRepository.Update(tblOrderLgc);
                    _orderLGCRepository.SaveChanges();
                    #endregion

                    #region update statusid in tbllogistics

                    // tblLogistic = _logisticsRepository.GetSingle(x => x.OrderTransId == OrderTransId);

                    TblLogistic tblLogistic = _context.TblLogistics.Where(x => x.IsActive == false && x.OrderTransId == OrderTransId).ToList().LastOrDefault();

                    if (tblLogistic != null)
                    {
                        tblLogistic.StatusId = statusId;
                        //Priyanshi for use (LGC Reopen Task)
                        tblLogistic.IsActive = false;
                        tblLogistic.Modifiedby = loggedInUserId;
                        tblLogistic.ModifiedDate = _currentDatetime;
                        _logisticsRepository.Update(tblLogistic);
                        _logisticsRepository.SaveChanges();
                    }
                    #endregion

                    #region update statusId in tblOrderTrans
                    tblOrderTrans.StatusId = statusId;
                    tblOrderTrans.ModifiedBy = loggedInUserId;
                    tblOrderTrans.ModifiedDate = _currentDatetime;
                    _orderTransRepository.Update(tblOrderTrans);
                    _orderTransRepository.SaveChanges();
                    #endregion

                    #region update statusId in Base tbl Exchange or ABB
                    if (tblExchangeOrder != null)
                    {
                        tblExchangeOrder.StatusId = statusId;
                        //tblExchangeOrder.OrderStatus = "Reopen for Logistics";
                        tblExchangeOrder.OrderStatus = "Customer requested to reopen ticket for pickup";
                        tblExchangeOrder.ModifiedBy = loggedInUserId;
                        tblExchangeOrder.ModifiedDate = _currentDatetime;
                        _exchangeOrderRepository.Update(tblExchangeOrder);
                        _exchangeOrderRepository.SaveChanges();
                    }
                    else if (tblAbbredemption != null && tblAbbregistration != null)
                    {
                        #region update status on tblAbbRedemption
                        tblAbbredemption.StatusId = statusId;
                        //tblAbbredemption.AbbredemptionStatus = "Reopen for Logistics";
                        tblAbbredemption.AbbredemptionStatus = "Customer requested to reopen ticket for pickup";
                        tblAbbredemption.ModifiedBy = loggedInUserId;
                        tblAbbredemption.ModifiedDate = _currentDatetime;
                        _abbRedemptionRepository.Update(tblAbbredemption);
                        _abbRedemptionRepository.SaveChanges();
                        #endregion
                    }
                    #endregion

                    #region Update StatusId in Tbl WalletTransection 
                    tblWalletTransaction = _walletTransactionRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == OrderTransId);
                    if (tblWalletTransaction != null)
                    {
                        tblWalletTransaction.StatusId = statusId.ToString();
                        tblWalletTransaction.ModifiedBy = loggedInUserId;
                        tblWalletTransaction.ModifiedDate = _currentDatetime;
                        _walletTransactionRepository.Update(tblWalletTransaction);
                        _walletTransactionRepository.SaveChanges();
                    }
                    #endregion

                    #region Insert into tblexchangeabbhistory
                    TblServicePartner tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == tblLogistic.ServicePartnerId).FirstOrDefault();
                    tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                    tblExchangeAbbstatusHistory.OrderType = orderType;
                    tblExchangeAbbstatusHistory.SponsorOrderNumber = sponsorOrderNumber;
                    tblExchangeAbbstatusHistory.RegdNo = regdNo;
                    tblExchangeAbbstatusHistory.CustId = customerDetailsId;
                    tblExchangeAbbstatusHistory.StatusId = statusId;
                    tblExchangeAbbstatusHistory.IsActive = true;
                    tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                    tblExchangeAbbstatusHistory.CreatedBy = loggedInUserId;
                    tblExchangeAbbstatusHistory.OrderTransId = OrderTransId;
                    tblExchangeAbbstatusHistory.Comment = Comment != null ? "TicketNo-" + tblLogistic.TicketNumber + "- Service Partner Name -" + tblServicePartner.ServicePartnerDescription + "Comment -" + Comment : string.Empty;
                    _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                    #endregion

                    return flag = true;
                }
                else
                {
                    return flag;
                }
                //}
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QCManager", "AddRejectedOrderStatusToDB", ex);
            }
            return flag;
        }
        #endregion

        #region Method to Update rescheduled date and Comment 
        /// <summary>
        ///Method to Update rescheduled date and Comment 
        /// </summary>
        /// <param name = "QCCommentVM" ></ param >
        /// < returns > QCCommentViewModel </ returns >
        public int RescheduledLGC(string RegdNo, String RescheduleComment, DateTime? RescheduleDate, int UserId)
        {
            TblOrderLgc tblOrderLgc = null;
            TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = null;
            int number = 0;
            HttpResponseMessage ResponseData = null;

            #region Common Implementations for (ABB or Exchange)
            TblExchangeOrder tblExchangeOrder = null;
            TblAbbredemption tblAbbredemption = null;
            TblAbbregistration tblAbbregistration = null;
            string regdNo = RegdNo; int? customerDetailsId = null; string sponsorOrderNumber = null;
            int? setStatusId = Convert.ToInt32(OrderStatusEnum.PickupReschedule);
            #endregion

           // bool flag = false;
            try
            {
                #region Common Implementations for (ABB or Exchange)
                TblOrderTran tblOrderTrans = _orderTransRepository.GetOrderTransByRegdno(regdNo);
                if (tblOrderTrans != null)
                {
                    if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                    {
                        tblExchangeOrder = tblOrderTrans.Exchange;
                        if (tblExchangeOrder != null)
                        {
                            customerDetailsId = tblExchangeOrder.CustomerDetailsId;
                            sponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                        }
                    }
                    else if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.ABB))
                    {
                        tblAbbredemption = tblOrderTrans.Abbredemption;
                        if (tblAbbredemption != null)
                        {
                            customerDetailsId = tblAbbredemption.CustomerDetailsId;
                            tblAbbregistration = tblAbbredemption.Abbregistration;
                            if (tblAbbregistration != null)
                            {
                                sponsorOrderNumber = tblAbbregistration.SponsorOrderNo;
                            }
                        }
                    }
                }
                #endregion
                TblLogistic tblLogistic = _logisticsRepository.GetSingle(x => x.IsActive == true && x.RegdNo == RegdNo);
                if (tblLogistic != null)
                {
                    tblOrderLgc = _orderLGCRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == tblLogistic.OrderTransId);
                    if (tblOrderTrans != null)
                    {
                        TblServicePartner tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == tblLogistic.ServicePartnerId).FirstOrDefault();

                        #region update statusId in Base tbl Exchange or ABB
                        if (tblExchangeOrder != null)
                        {
                            tblExchangeOrder.StatusId = setStatusId;
                            tblExchangeOrder.OrderStatus = "Pickup";
                            tblExchangeOrder.ModifiedBy = UserId;
                            tblExchangeOrder.ModifiedDate = _currentDatetime;
                            _exchangeOrderRepository.Update(tblExchangeOrder);
                            _exchangeOrderRepository.SaveChanges();
                        }
                        else if (tblAbbredemption != null && tblAbbregistration != null)
                        {
                            #region update status on tblAbbRedemption
                            tblAbbredemption.StatusId = setStatusId;
                            tblAbbredemption.AbbredemptionStatus = "Pickup";
                            tblAbbredemption.ModifiedBy = UserId;
                            tblAbbredemption.ModifiedDate = _currentDatetime;
                            TblAbbredemption OtblAbbredemption = new TblAbbredemption();
                            OtblAbbredemption = _abbRedemptionRepository.UpdateABBOrderStatus(tblAbbredemption.RegdNo, (int)setStatusId, UserId, tblAbbredemption.AbbredemptionStatus);

                           
                            #endregion
                        }
                        #endregion

                        #region update statusId in tblOrderTrans
                        tblOrderTrans.StatusId = setStatusId;
                        tblOrderTrans.ModifiedBy = UserId;
                        tblOrderTrans.ModifiedDate = _currentDatetime;
                        _orderTransRepository.Update(tblOrderTrans);
                        _orderTransRepository.SaveChanges();
                        #endregion

                        #region Insert into tblexchangeabbhistory
                        tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                        tblExchangeAbbstatusHistory.OrderType = (int)tblOrderTrans.OrderType;
                        tblExchangeAbbstatusHistory.SponsorOrderNumber = sponsorOrderNumber;
                        tblExchangeAbbstatusHistory.RegdNo = regdNo;
                        tblExchangeAbbstatusHistory.CustId = customerDetailsId;
                        tblExchangeAbbstatusHistory.StatusId = setStatusId;
                        tblExchangeAbbstatusHistory.IsActive = true;
                        tblExchangeAbbstatusHistory.CreatedBy = UserId;
                        tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                        tblExchangeAbbstatusHistory.OrderTransId = tblOrderTrans.OrderTransId;
                        tblExchangeAbbstatusHistory.Comment = RescheduleComment != null ? "TicketNo-" + tblLogistic.TicketNumber + "- Service Partner Name -" + tblServicePartner.ServicePartnerDescription + "Comment -" + RescheduleComment : string.Empty;
                        _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                        #endregion
                        #region Update StatusId in Tbl WalletTransection 
                        TblWalletTransaction tblWalletTransaction = _walletTransactionRepository.GetSingle(x => x.IsActive == true && x.RegdNo == RegdNo);
                        tblWalletTransaction.StatusId = setStatusId.ToString();
                        tblWalletTransaction.ModifiedBy = UserId;
                        tblWalletTransaction.ModifiedDate = _currentDatetime;
                        _walletTransactionRepository.Update(tblWalletTransaction);
                        _walletTransactionRepository.SaveChanges();
                        #endregion
                        #region update statusid in tbllogistics
                        tblLogistic.StatusId = setStatusId;
                        //Priyanshi for use (LGC Reopen Task)
                        tblLogistic.IsActive = false;
                        tblLogistic.Modifiedby = UserId;
                        tblLogistic.RescheduleComment = RescheduleComment;
                        tblLogistic.Rescheduledate = RescheduleDate;
                        tblLogistic.RescheduleCount = 1;
                        tblLogistic.ModifiedDate = _currentDatetime;
                        _logisticsRepository.Update(tblLogistic);
                        _logisticsRepository.SaveChanges();
                        #endregion
                        #region Update TBLOrderLGC
                        if (tblOrderLgc != null)
                        {
                            //used for LGC Drop
                            tblOrderLgc.StatusId = setStatusId;                                               
                            tblOrderLgc.ModifiedBy = UserId;
                            tblOrderLgc.ModifiedDate = _currentDatetime;
                            _orderLGCRepository.Update(tblOrderLgc);
                            _orderLGCRepository.SaveChanges();
                        }
                        #endregion

                        #region update TblVehicleJourneyTrackingDetails by Priyanshi date 6/09/2023
                        TblVehicleJourneyTrackingDetail? tblVehicleJourneyTrackingDetail = _vehicleJourneyTrackingDetails.GetSingle(x => x.IsActive == true && x.OrderTransId == tblOrderTrans.OrderTransId);
                        if (tblVehicleJourneyTrackingDetail != null)
                        {
                            tblVehicleJourneyTrackingDetail.IsActive = false;
                            tblVehicleJourneyTrackingDetail.EstimateEarning = 0;
                            tblVehicleJourneyTrackingDetail.StatusId = setStatusId;
                            tblVehicleJourneyTrackingDetail.ModifiedBy = UserId;
                            tblVehicleJourneyTrackingDetail.ModifiedDate = _currentDatetime;
                            _vehicleJourneyTrackingDetails.Update(tblVehicleJourneyTrackingDetail);
                            _vehicleJourneyTrackingDetails.SaveChanges();
                        }
                        #endregion

                        var priority = "High";
                        if (tblLogistic.ServicePartnerId == Convert.ToInt32(ServicePartnerEnum.Bizlog))
                        {
                            ResponseData= _ticketGenrateManager.CreateTicketWithBizlog(tblLogistic.RegdNo, priority, (int)tblLogistic.ServicePartnerId, UserId);

                            ResponseData responseData1 = new ResponseData
                            {
                                Regno = tblLogistic.RegdNo,
                                ServicePartner = "Bizlog",
                                StatusCode = (int)ResponseData.StatusCode,
                                Content = ResponseData.Content.ReadAsStringAsync().Result
                            };

                            ContentData? contentData1 = JsonConvert.DeserializeObject<ContentData>(responseData1.Content);
                            if (contentData1 != null && responseData1.StatusCode == 200)
                            {
                                TblLogistic tblLogistic1 = _logisticsRepository.GetSingle(x => x.IsActive == true && x.RegdNo == RegdNo);
                                tblLogistic1.PickupScheduleDate = tblLogistic.Rescheduledate;
                                tblLogistic1.Modifiedby = UserId;
                                _logisticsRepository.Update(tblLogistic);
                                _logisticsRepository.SaveChanges();
                                number = 1;
                            }                            
                        }
                        else if (tblLogistic.ServicePartnerId == Convert.ToInt32(ServicePartnerEnum.Mahindra))
                        {
                            ResponseData= _ticketGenrateManager.RequestMahindraLGC(tblLogistic.RegdNo, (int)tblLogistic.ServicePartnerId, UserId);
                            // Inside the foreach loop
                            ResponseData responseData2 = new ResponseData
                            {
                                ServicePartner = "Mahindra",
                                Regno = tblLogistic.RegdNo,
                                StatusCode = (int)ResponseData.StatusCode,
                                Content = ResponseData.Content.ReadAsStringAsync().Result
                            };

                            ContentData contentData2 = JsonConvert.DeserializeObject<ContentData>(responseData2.Content);

                            if (contentData2 != null && responseData2.StatusCode == 200)
                            {
                                TblLogistic tblLogistic1 = _logisticsRepository.GetSingle(x => x.IsActive == true && x.RegdNo == RegdNo);
                                tblLogistic1.PickupScheduleDate = tblLogistic.Rescheduledate;
                                tblLogistic1.Modifiedby = UserId;
                                _logisticsRepository.Update(tblLogistic);
                                _logisticsRepository.SaveChanges();
                                number = 1;
                            }
                        }
                        else 
                        {
                            ResponseData= _ticketGenrateManager.GenerateTicketForLocalLgcPartner(tblLogistic.RegdNo, (int)tblLogistic.ServicePartnerId, UserId);

                            // Inside the foreach loop

                            ResponseData responseData3 = new ResponseData
                            {
                                Regno = regdNo,
                                StatusCode = (int)ResponseData.StatusCode,
                                Content = ResponseData.Content.ReadAsStringAsync().Result
                            };
                            
                            dynamic contentData3 = JsonConvert.DeserializeObject(responseData3.Content);

                            // Fill the properties in the ResponseData model
                            if (contentData3 != null && responseData3.StatusCode == 200)
                            {
                                TblLogistic tblLogistic1 = _logisticsRepository.GetSingle(x => x.IsActive == true && x.RegdNo == RegdNo);
                                tblLogistic1.PickupScheduleDate = tblLogistic.Rescheduledate;
                                tblLogistic1.Modifiedby = UserId;
                                _logisticsRepository.Update(tblLogistic);
                                _logisticsRepository.SaveChanges();
                                number = 1;
                            }
                        }
                    }
                    else
                    {
                        number = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "RescheduledLGC", ex);
            }
            return number;
        }
        #endregion

        IList<TblExchangeOrderStatus> ILogisticManager.GetExchangeOrderStatusByLGCDepartment()
        {
            List<TblExchangeOrderStatus>? tblExchangeOrderStatuses = null;
            tblExchangeOrderStatuses = _context.TblExchangeOrderStatuses.Where(x => x.StatusName.ToLower().Equals("Pickup") || x.StatusName.ToLower().Equals("EVC Drop")).ToList();

            return tblExchangeOrderStatuses;
        }
        
        #region Create by Priyanshi for PayNow, Modified by VK for Abb Redumption 
        //Create by Priyanshi for PayNow 
        public bool LGCPayNow(string RegdNo)
        {
            bool flag = false;
            try
            {
                if (RegdNo != null)
                {
                    TblOrderTran tblOrderTran = _context.TblOrderTrans.Where(x => x.IsActive == true && x.RegdNo == RegdNo).FirstOrDefault();
                    if (tblOrderTran != null)
                    {
                        TblExchangeOrder tblExchangeOrder = _context.TblExchangeOrders.Where(x => x.IsActive == true && x.Id == tblOrderTran.ExchangeId && x.RegdNo == RegdNo && x.IsDefferedSettlement == true).FirstOrDefault();
                        TblAbbredemption tblAbbredemption = _context.TblAbbredemptions.Where(x => x.IsActive == true && x.RedemptionId == tblOrderTran.AbbredemptionId && x.RegdNo == RegdNo).FirstOrDefault();
                        
                        if (tblExchangeOrder != null || tblAbbredemption != null)
                        {
                            TblOrderQc tblOrderQc = _context.TblOrderQcs.Where(x => x.IsActive == true && x.OrderTransId == tblOrderTran.OrderTransId && x.Upiid != null).FirstOrDefault();
                            if (tblOrderQc != null)
                            {
                                flag = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "LGCPayNow", ex);
            }
            return flag;
        }
        #endregion

        #region Added By Priyanshi --LGC_Admin Cancel ticket by utc Task (Modified by VK for ABB Redemption Date : 21-June-2023)
        public bool CancelTicketByUTC(int OrderTransId, int loggedInUserId, string Comment)
        {
            #region Variable Declaration
            TblExchangeOrder tblExchangeOrder = null;
            TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = null;
            TblWalletTransaction tblWalletTransaction = null;
            TblOrderTran tblOrderTrans = null;
            //TblLogistic tblLogistic = null;
            // TblOrderLgc tblOrderLGC = null;
            int statusId = Convert.ToInt32(OrderStatusEnum.PickupTicketcancellationbyUTC);
            bool flag = false;
            #endregion

            #region Variables for (ABB or Exchange)
            TblAbbredemption tblAbbredemption = null;
            TblAbbregistration tblAbbregistration = null; int orderType = 0;
            string regdNo = null; int? customerDetailsId = null; string sponsorOrderNumber = null;
            #endregion

            try
            {
                #region Common Implementations for (ABB or Exchange)
                tblOrderTrans = _orderTransRepository.GetOrderDetailsByOrderTransId(OrderTransId);
                if (tblOrderTrans != null)
                {
                    orderType = (int)tblOrderTrans.OrderType;
                    if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                    {
                        tblExchangeOrder = tblOrderTrans.Exchange;
                        if (tblExchangeOrder != null)
                        {
                            regdNo = tblExchangeOrder.RegdNo;
                            customerDetailsId = tblExchangeOrder.CustomerDetailsId;
                            sponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                        }
                    }
                    else if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.ABB))
                    {
                        tblAbbredemption = tblOrderTrans.Abbredemption;
                        if (tblAbbredemption != null)
                        {
                            customerDetailsId = tblAbbredemption.CustomerDetailsId;
                            tblAbbregistration = tblAbbredemption.Abbregistration;
                            if (tblAbbregistration != null)
                            {
                                regdNo = tblAbbregistration.RegdNo;
                                sponsorOrderNumber = tblAbbregistration.SponsorOrderNo;
                            }
                        }
                    }
                }
                #endregion
                
                #region update statusid in tbllogistics
                TblLogistic tblLogistic = _logisticsRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == OrderTransId);
                    if (tblLogistic != null)
                    {
                        tblLogistic.StatusId = statusId;
                        //Priyanshi for use (LGC Reopen Task)
                        tblLogistic.IsActive = false;
                        tblLogistic.RescheduleComment = Comment;
                        tblLogistic.Modifiedby = loggedInUserId;
                        tblLogistic.ModifiedDate = _currentDatetime;
                        _logisticsRepository.Update(tblLogistic);
                        _logisticsRepository.SaveChanges();
                    #endregion

                    TblServicePartner tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == tblLogistic.ServicePartnerId).FirstOrDefault();
                    
                    #region update statusId in tblOrderTrans
                    tblOrderTrans.StatusId = statusId;
                    tblOrderTrans.ModifiedBy = loggedInUserId;
                    tblOrderTrans.ModifiedDate = _currentDatetime;
                    _orderTransRepository.Update(tblOrderTrans);
                    _orderTransRepository.SaveChanges();
                    #endregion

                    #region update statusId in Base tbl Exchange or ABB
                    if (tblExchangeOrder != null)
                    {
                        tblExchangeOrder.StatusId = statusId;
                        tblExchangeOrder.OrderStatus = "Pickup Ticket cancellation by UTC";
                        tblExchangeOrder.ModifiedBy = loggedInUserId;
                        tblExchangeOrder.ModifiedDate = _currentDatetime;
                        _exchangeOrderRepository.Update(tblExchangeOrder);
                        _exchangeOrderRepository.SaveChanges();
                    }
                    else if (tblAbbredemption != null && tblAbbregistration != null)
                    {
                        #region update status on tblAbbRedemption
                        tblAbbredemption.StatusId = statusId;
                        tblAbbredemption.AbbredemptionStatus = "Pickup Ticket cancellation by UTC";
                        tblAbbredemption.ModifiedBy = loggedInUserId;
                        tblAbbredemption.ModifiedDate = _currentDatetime;
                        _abbRedemptionRepository.Update(tblAbbredemption);
                        _abbRedemptionRepository.SaveChanges();
                        #endregion
                    }
                    #endregion

                    #region Insert into tblexchangeabbhistory
                    tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                    tblExchangeAbbstatusHistory.OrderType = orderType;
                    tblExchangeAbbstatusHistory.SponsorOrderNumber = sponsorOrderNumber;
                    tblExchangeAbbstatusHistory.RegdNo = regdNo;
                    tblExchangeAbbstatusHistory.CustId = customerDetailsId;
                    tblExchangeAbbstatusHistory.StatusId = statusId;
                    tblExchangeAbbstatusHistory.IsActive = true;
                    tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                    tblExchangeAbbstatusHistory.CreatedBy = loggedInUserId;
                    tblExchangeAbbstatusHistory.OrderTransId = OrderTransId;
                    tblExchangeAbbstatusHistory.Comment = Comment != null ? "TicketNo-" + tblLogistic.TicketNumber + "- Service Partner Name -" + tblServicePartner.ServicePartnerDescription + "Comment -" + Comment : string.Empty;
                    _commonManager.InsertExchangeAbbstatusHistory(tblExchangeAbbstatusHistory);
                    #endregion

                    #region Update StatusId in Tbl WalletTransection 
                    tblWalletTransaction = _walletTransactionRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == OrderTransId);
                    if (tblWalletTransaction != null)
                    {
                        tblWalletTransaction.StatusId = statusId.ToString();
                        tblWalletTransaction.ModifiedBy = loggedInUserId;
                        tblWalletTransaction.ModifiedDate = _currentDatetime;
                        _walletTransactionRepository.Update(tblWalletTransaction);
                        _walletTransactionRepository.SaveChanges();
                    }
                    #endregion

                    return flag = true;
                }
                else
                {
                    return flag;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "CancelTicketByUTC", ex);
            }
            return flag;
        }
        #endregion

        #region Get LGC Pickup Order Details by RegdNum
        /// <summary>
        /// Get LGC Pickup Order Details by RegdNum
        /// </summary>
        /// <param name="regdNo"></param>
        /// <returns></returns>
        public LGCOrderViewModel GetLGCPickupOrderDetailsByRegdNo(string regdNo)
        {
            LGCOrderViewModel lGCOrderViewModel = new LGCOrderViewModel();
            TblLogistic? tbllogistic = null;
            TblWalletTransaction? tblWalletTransaction = null;
            TblOrderTran? tblOrderTrans = null;
            TblAreaLocality? tblAreaLocality = null;
            TblBusinessUnit? tblBusinessUnit = null;
            BrandViewModel? brandVM = null;
            EVC_PartnerViewModel? evcPartnerDetailsVM = null;
            try
            {
                tblOrderTrans = _orderTransRepository.GetRegdno(regdNo);
                if (tblOrderTrans != null)
                {
                    tblWalletTransaction = _logisticsRepository.GetEvcDetailsByRegdno(regdNo);
                    #region Implementation for Exchange or ABB
                    if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.Exchange) && tblWalletTransaction != null)
                    {
                        tbllogistic = _logisticsRepository.GetExchangeDetailsByRegdno(regdNo);
                        if (tbllogistic != null)
                        {
                            #region Order Details
                            lGCOrderViewModel.RegdNo = tbllogistic.OrderTrans.Exchange.RegdNo;
                            lGCOrderViewModel.ProductCategory = tbllogistic.OrderTrans.Exchange.ProductType.ProductCat.Description;
                            lGCOrderViewModel.ProductType = tbllogistic.OrderTrans.Exchange.ProductType.DescriptionForAbb;
                            lGCOrderViewModel.TicketNumber = tbllogistic.TicketNumber;
                            lGCOrderViewModel.SponsorName = tbllogistic.OrderTrans.Exchange.CompanyName;
                            lGCOrderViewModel.BrandName = tbllogistic.OrderTrans.Exchange.Brand.Name;
                            lGCOrderViewModel.PickupScheduleDate = tbllogistic.PickupScheduleDate != null
                ? Convert.ToDateTime(tbllogistic.PickupScheduleDate).ToString("MM/dd/yyyy")
                : Convert.ToDateTime(tbllogistic.CreatedDate).ToString("MM/dd/yyyy");
                            lGCOrderViewModel.AmountPayableThroughLGC = Convert.ToDecimal(tbllogistic.AmtPaybleThroughLgc);
                            #endregion

                            #region Customer Details
                            int bpId =Convert.ToInt32(tbllogistic.OrderTrans.Exchange.BusinessPartnerId);
                            lGCOrderViewModel.IsDefaultPickupAddress = false;
                            TblBusinessPartner tblBusinessPartner = _businessPartnerRepository.GetSingle(x=>x.IsActive==true && x.BusinessPartnerId==bpId);
                            if(tblBusinessPartner !=null && tblBusinessPartner.IsDefaultPickupAddress == true)
                            {
                                lGCOrderViewModel.tblBusinessPartner = tblBusinessPartner;
                                lGCOrderViewModel.IsDefaultPickupAddress = true;
                            }
                            if (lGCOrderViewModel.IsDefaultPickupAddress == false)
                            {
                                lGCOrderViewModel.TblCustomerDetail = tbllogistic.OrderTrans.Exchange.CustomerDetails;
                                lGCOrderViewModel.CustomerName = tbllogistic.OrderTrans.Exchange.CustomerDetails.FirstName + " " + tbllogistic.OrderTrans.Exchange.CustomerDetails.LastName;
                                if (tbllogistic.OrderTrans.Exchange.CustomerDetails.AreaLocalityId != null)
                                {
                                    tblAreaLocality = _AreaLocalityRepository.GetArealocalityById(tbllogistic.OrderTrans.Exchange.CustomerDetails.AreaLocalityId);
                                    if (tblAreaLocality != null)
                                    {
                                        tbllogistic.OrderTrans.Exchange.CustomerDetails.AreaLocality = tblAreaLocality.AreaLocality;
                                    }
                                }
                            }
                            
                            #endregion
                        }
                    }
                    else if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.ABB) && tblWalletTransaction != null)
                    {
                        tbllogistic = _logisticsRepository.GetAbbRedumptionDetailsByRegdno(regdNo);
                        if (tbllogistic != null)
                        {
                            #region Order Details
                            lGCOrderViewModel.RegdNo = tbllogistic.OrderTrans.RegdNo;
                            lGCOrderViewModel.ProductCategory = tbllogistic.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description;
                            lGCOrderViewModel.ProductType = tbllogistic.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.DescriptionForAbb;
                            lGCOrderViewModel.TicketNumber = tbllogistic.TicketNumber;
                            lGCOrderViewModel.SponsorName = tbllogistic.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name;
                            //lGCOrderViewModel.BrandName = tbllogistic.OrderTrans.Exchange.Brand.Name;
                            lGCOrderViewModel.PickupScheduleDate = tbllogistic.PickupScheduleDate != null
                ? Convert.ToDateTime(tbllogistic.PickupScheduleDate).ToString("MM/dd/yyyy")
                : Convert.ToDateTime(tbllogistic.CreatedDate).ToString("MM/dd/yyyy");
                            lGCOrderViewModel.AmountPayableThroughLGC = Convert.ToDecimal(tbllogistic.AmtPaybleThroughLgc);
                            #endregion

                            #region Check BusinessUnit configuration
                            tblBusinessUnit = _businessUnitRepository.GetBUDetailsByTransId(tblOrderTrans.OrderTransId);
                            if (tblBusinessUnit != null)
                            {
                                int? newBrandId = tbllogistic.OrderTrans?.Abbredemption?.Abbregistration?.NewBrandId;
                                brandVM = _commonManager.GetAbbBrandDetailsById(tblBusinessUnit.IsBumultiBrand, newBrandId);
                                lGCOrderViewModel.BrandName = brandVM?.Name;
                            }
                            #endregion

                            #region Customer Details
                            if (tbllogistic.OrderTrans.Abbredemption.CustomerDetails == null)
                            {
                                tbllogistic.OrderTrans.Abbredemption.CustomerDetails = new TblCustomerDetail();
                            }
                            else
                            {
                                lGCOrderViewModel.TblCustomerDetail = tbllogistic.OrderTrans.Abbredemption.CustomerDetails;
                                lGCOrderViewModel.CustomerName = tbllogistic.OrderTrans.Abbredemption.CustomerDetails.FirstName + " " + tbllogistic.OrderTrans.Abbredemption.CustomerDetails.LastName;
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region EVC Details
                    lGCOrderViewModel.Tblevcregistration = tblWalletTransaction.Evcregistration;
                    if (tblWalletTransaction.Evcpartner != null)
                    {
                        evcPartnerDetailsVM = _mapper.Map<TblEvcPartner,EVC_PartnerViewModel>(tblWalletTransaction.Evcpartner);
                        lGCOrderViewModel.evcPartnerDetailsVM = evcPartnerDetailsVM;
                        lGCOrderViewModel.evcPartnerDetailsVM.CityName = tblWalletTransaction.Evcpartner?.City?.Name;
                        lGCOrderViewModel.evcPartnerDetailsVM.StateName = tblWalletTransaction.Evcpartner?.State?.Name;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "GetLGCPickupOrderDetailsByRegdNo", ex);
            }
            return lGCOrderViewModel;
        }
        #endregion

        #region Create by VK Get BU based PayNow redirection Link (Currently configure for TechGuard)
        /// <summary>
        /// Get BU based PayNow redirection Link
        /// </summary>
        /// <param name="RegdNo"></param>
        /// <returns></returns>
        public string GetLGCPayNowLinkBasedOnBU(string RegdNo)
        {
            string LgcPayNowLink = null;
            
            #region Common Implementations for (ABB or Exchange)
            TblExchangeOrder tblExchangeOrder = null;
            TblAbbredemption tblAbbredemption = null;
            TblAbbregistration tblAbbregistration = null;
            TblBusinessUnit tblBusinessUnit = null;
            TblBusinessPartner tblBusinessPartner = null;
            #endregion
            try
            {
                int? techGuardBuid = _baseConfig.Value.TechGuardBUId;
                if (!(techGuardBuid != null && techGuardBuid > 0))
                {
                    techGuardBuid = Convert.ToInt32(BussinessUnitEnum.Tech_Guard);
                }
                if (RegdNo != null)
                {
                    #region Common Implementations for (ABB or Exchange)
                    TblOrderTran tblOrderTrans = _orderTransRepository.GetOrderTransByRegdno(RegdNo);
                    if (tblOrderTrans != null)
                    {
                        if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                        {
                            tblExchangeOrder = tblOrderTrans.Exchange;
                            if (tblExchangeOrder != null && tblExchangeOrder.IsDefferedSettlement == true)
                            {
                                tblBusinessPartner = _businessPartnerRepository.GetBPId(tblExchangeOrder.BusinessPartnerId);
                                if (tblBusinessPartner != null)
                                {
                                    tblBusinessUnit = _businessUnitRepository.Getbyid(tblBusinessPartner.BusinessUnitId);
                                }
                            }
                        }
                        else if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.ABB))
                        {
                            tblAbbredemption = tblOrderTrans.Abbredemption;
                            if (tblAbbredemption != null && tblAbbredemption.IsDefferedSettelment == true)
                            {
                                tblAbbregistration = tblAbbredemption.Abbregistration;
                                if (tblAbbregistration != null)
                                {
                                    tblBusinessUnit = _businessUnitRepository.Getbyid(tblAbbregistration.BusinessUnitId);
                                }
                            }
                        }
                    }
                    #endregion
                    
                    if (tblBusinessUnit != null)
                    {   // Currently TechGuard Buid is 22 on Dev and 21 is on QA
                        if (tblBusinessUnit.BusinessUnitId == techGuardBuid)
                        {
                            if (tblBusinessUnit.IsPaymentThirdParty == true)
                            {
                                LgcPayNowLink = "/PayOut/PayMentConfirmationTechGuard";
                            }
                        }
                        else
                        {
                            TblOrderQc tblOrderQc = _context.TblOrderQcs.Where(x => x.IsActive == true && x.OrderTransId == tblOrderTrans.OrderTransId && x.Upiid != null).FirstOrDefault();
                            if (tblOrderQc != null)
                            {
                                LgcPayNowLink = "/PayOut/PayOutConfirmation";
                            }
                        }    
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "LGCPayNowBasedOnBU", ex);
            }
            return LgcPayNowLink;
        }
        #endregion

        #region Get Driver Details by DriverDetailsId
        public DriverDetailsViewModel GetDriverDetailsById(int driverId)
        {
            DriverDetailsViewModel? driverDetailVM = null;
            TblDriverDetail? tblDriverDetail = null;
            try
            {
                tblDriverDetail = _driverDetailsRepository.GetDriverDetailsById(driverId);
                driverDetailVM = _mapper.Map<DriverDetailsViewModel>(tblDriverDetail);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "GetDriverDetailsById", ex);
            }
            return driverDetailVM;
        }
        #endregion

        #region Get details of service partner by userid
        /// <summary>
        /// Get details of service partner by userid
        /// </summary>
        /// <returns>tblServicePartner<LGCOrderViewModel></returns>
        public ServicePartnerViewModel GetServicePartnerByUserId(int userId)
        {
            TblServicePartner? tblServicePartner = null;
            ServicePartnerViewModel? servicePartnerVM = null;
            try
            {
                tblServicePartner = _servicePartnerRepository.GetServicePartnerByUserId(userId);
                if (tblServicePartner != null)
                {
                    servicePartnerVM = _mapper.Map<ServicePartnerViewModel>(tblServicePartner);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "GetServicePartnerDetails", ex);
            }
            return servicePartnerVM;
        }
        #endregion

        #region Get Driver Details by TrackingId
        public DriverDetailsViewModel GetDriverDetailsByTrackingId(int trackingId)
        {
            DriverDetailsViewModel? driverDetailVM = null;
            TblDriverDetail? tblDriverDetail = null;
            TblVehicleJourneyTracking? tblVehicleJourneyTracking = null;
            try
            {
                tblVehicleJourneyTracking = _vehicleJourneyTrackingRepository.GetVehicleJourneyTrackingById(trackingId);
                if (tblVehicleJourneyTracking != null)
                {
                    tblDriverDetail = _driverDetailsRepository.GetDriverDetailsById(tblVehicleJourneyTracking.DriverId??0);
                    if (tblDriverDetail != null)
                    {
                        driverDetailVM = _mapper.Map<DriverDetailsViewModel>(tblDriverDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "GetDriverDetailsById", ex);
            }
            return driverDetailVM;
        }
        #endregion

        #region Generate and Save DebitNote PDF
        public bool GenerateAndSaveDebitNote(List<TblOrderLgc> orderList, PODViewModel podVM, int loggedUserId)
        {
            #region Variable Declaration
            bool flag = false;
            string? debitNotePdfName = null;
            bool debitNoteSaved = false;
            string? debitNotefilePath = null;
            string? debitNoteHtmlString = null;
            decimal? finalAmountDN = 0;
            //Generate Invoice
            int MaxInvSrNum = 0;
            #endregion
            try
            {
                string DNBillNumber = "DN-" + podVM.FinancialYear + "-" + podVM.BillCounterNum;
                podVM.evcDetailsVM.BillNumberDN = DNBillNumber;
                debitNotePdfName = DNBillNumber.Replace("/", "-") + ".pdf";
                debitNotefilePath = EnumHelper.DescriptionAttr(FilePathEnum.EVCDebitNote);
                debitNoteHtmlString = GetPoDHtmlString(podVM, "EVC_Debit_Note");
                debitNoteSaved = _htmlToPdfConverterHelper.GeneratePDF(debitNoteHtmlString, debitNotefilePath, debitNotePdfName);
                #region Update Drop Status In DB  
                if (debitNoteSaved)
                {
                    podVM.DebitNotePdfName = debitNotePdfName;
                    podVM.DnsrNum = podVM.InvSrNum;
                    podVM.DebitNoteAmount = podVM?.evcDetailsVM?.FinalPriceDN;
                    flag = UpdateLGCDropStatus(orderList, podVM, loggedUserId);
                }
                #endregion
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "GetnerateAndSaveDebitNote", ex);
            }
            return flag;
        }
        #endregion

        #region Generate and Save Invoice PDF
        public bool GenerateAndSaveInvoice(List<TblOrderLgc> orderList, PODViewModel podVM, int loggedUserId)
        {
            #region Variable Declaration
            bool flag = false;
            string? invoicePdfName = null;
            bool invoiceSaved = false;
            string? invoicefilePath = null;
            string? invoiceHtmlString = null;
            #endregion
            
            try
            {
                string InvBillNumber = "INV-" + podVM.FinancialYear + "/" + podVM.BillCounterNum;
                podVM.evcDetailsVM.BillNumberInv = InvBillNumber;
                invoicePdfName = InvBillNumber.Replace("/", "-") + ".pdf";
                invoicefilePath = EnumHelper.DescriptionAttr(FilePathEnum.EVCInvoice);
                invoiceHtmlString = GetPoDHtmlString(podVM, "EVC_Invoice");
                invoiceSaved = _htmlToPdfConverterHelper.GeneratePDF(invoiceHtmlString, invoicefilePath, invoicePdfName);
                #region Update Drop Status In DB  
                if (invoiceSaved)
                {
                    podVM.InvoicePdfName = invoicePdfName;
                    podVM.InvoiceAmount = podVM.evcDetailsVM.FinalPriceInv;
                    flag = UpdateInvoicePostedStatus(orderList, podVM, loggedUserId);
                }
                #endregion
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "GetnerateAndSaveInvoice", ex);
            }
            return flag;
        }
        #endregion

        #region Get Service Partner Details by TrackingId
        public ServicePartnerViewModel GetSPDetailsByTrackingId(int trackingId)
        {
            ServicePartnerViewModel? servicePartnerVM = null;
            TblServicePartner? tblServicePartner = null;
            TblVehicleJourneyTracking? tblVehicleJourneyTracking = null;
            try
            {
                tblVehicleJourneyTracking = _vehicleJourneyTrackingRepository.GetVehicleJourneyTrackingById(trackingId);
                if (tblVehicleJourneyTracking != null)
                {
                    tblServicePartner = _servicePartnerRepository.GetServicePartnerById(tblVehicleJourneyTracking.ServicePartnerId ?? 0);
                    if (tblServicePartner != null)
                    {
                        servicePartnerVM = _mapper.Map<ServicePartnerViewModel>(tblServicePartner);
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "GetDriverDetailsById", ex);
            }
            return servicePartnerVM;
        }
        #endregion

        #region Get Service Partner Details by Id
        public ServicePartnerViewModel GetSPDetailsById(int servicePartnerId)
        {
            ServicePartnerViewModel? servicePartnerVM = null;
            TblServicePartner? tblServicePartner = null;
            try
            {
                if (servicePartnerId > 0)
                {
                    tblServicePartner = _servicePartnerRepository.GetServicePartnerById(servicePartnerId);
                    if (tblServicePartner != null)
                    {
                        servicePartnerVM = _mapper.Map<ServicePartnerViewModel>(tblServicePartner);
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "GetSPDetailsById", ex);
            }
            return servicePartnerVM;
        }
        #endregion

        #region Get LGC Drop Order Details by EVC Partner Id
        public LGCOrderViewModel GetLGCDropDetails(int? evcPartnerId)
        {
            LGCOrderViewModel? lGCOrderViewModel = null;
            TblEvcPartner? tblEvcPartner = null;
            TblEvcregistration? tblEvcregistration = null;
            try
            {
                if (evcPartnerId > 0)
                {
                    lGCOrderViewModel = new LGCOrderViewModel();
                    tblEvcPartner = _evcpartnerRepository.GetEVCPartnerDetails(evcPartnerId??0);
                    if (tblEvcPartner != null)
                    {
                        lGCOrderViewModel.evcPartnerDetailsVM = _mapper.Map<TblEvcPartner, EVC_PartnerViewModel>(tblEvcPartner);
                        lGCOrderViewModel.evcPartnerDetailsVM.CityName = tblEvcPartner?.City?.Name;
                        lGCOrderViewModel.evcPartnerDetailsVM.StateName = tblEvcPartner?.State?.Name;
                        lGCOrderViewModel.EvcpartnerId = tblEvcPartner?.EvcPartnerId;
                        lGCOrderViewModel.EVCRegistrationId = tblEvcPartner?.EvcregistrationId ?? 0;
                        tblEvcregistration = tblEvcPartner?.Evcregistration;
                        if (tblEvcregistration != null)
                        {
                            lGCOrderViewModel.EVCBusinessName = tblEvcregistration.BussinessName;
                            lGCOrderViewModel.EVCMobileNumber = tblEvcregistration.EvcmobileNumber;
                            lGCOrderViewModel.EVCContactPerson = tblEvcregistration.ContactPerson;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("LogisticManager", "GetLGCDropDetails", ex);
            }
            return lGCOrderViewModel;
        }
        #endregion

        #region Back up for LGC Drop Main Method Date : 13-Oct-2023
        //public bool SaveLGCDropStatus(PODViewModel podVM, int loggedUserId)
        //{
        //    #region Variable Declaration
        //    List<TblOrderLgc>? tblOrderLgcListAll = null;
        //    podVM.podVMList = new List<PODViewModel>();
        //    PODViewModel? podVMtemp = null;
        //    bool flag = false;
        //    string? podfilePath = null;
        //    string? podHtmlString = null;
        //    bool podPDFSave = false;
        //    string? podPdfName = null;
        //    decimal? finalAmountDN = 0;
        //    //Generate Invoice
        //    int MaxInvSrNum = 0;
        //    int InvSrNumFromConfig = 0;
        //    string FinancialYear = "";
        //    decimal? finalAmountInv = 0;
        //    List<TblConfiguration>? tblConfigurationList = null;
        //    //Generate Invoice

        //    TblEvcpoddetail? tblEvcpoddetailObj = null;

        //    #endregion

        //    OrderLGCViewModel orderLGC = new OrderLGCViewModel();
        //    try
        //    {
        //        if (podVM != null && podVM.DriverId > 0 && podVM.EVCRegistrationId > 0 && podVM.EvcpartnerId > 0)
        //        {
        //            #region Code for Get Data from TblConfiguration
        //            tblConfigurationList = _digi2L_DevContext.TblConfigurations.Where(x => x.IsActive == true).ToList();
        //            if (tblConfigurationList != null && tblConfigurationList.Count > 0)
        //            {
        //                // MaxInvSrNum = tblEvcpoddetail.Max(x => x.InvSrNum).Value;
        //                var startInvoiceSrNum = tblConfigurationList.FirstOrDefault(x => x.Name == ConfigurationEnum.StartInvoiceSrNum.ToString());
        //                if (startInvoiceSrNum != null && startInvoiceSrNum.Value != null)
        //                {
        //                    InvSrNumFromConfig = Convert.ToInt32(startInvoiceSrNum.Value.Trim());
        //                }
        //                var financialYear = tblConfigurationList.FirstOrDefault(x => x.Name == ConfigurationEnum.FinancialYear.ToString());
        //                if (financialYear != null && financialYear.Value != null)
        //                {
        //                    FinancialYear = financialYear.Value.Trim();
        //                }
        //            }
        //            #endregion

        //            #region Code for get Max InvSrNum from TblEvcpoddetails
        //            // tblEvcpoddetail = _evcPoDDetailsRepository.GetList(x => x.IsActive == true && x.InvSrNum != null && x.InvSrNum > 0).ToList();
        //            tblEvcpoddetailObj = _evcPoDDetailsRepository.GetList(x => x.IsActive == true && x.InvSrNum != null && x.InvSrNum != null && x.InvSrNum > 0 && x.FinancialYear == FinancialYear).OrderByDescending(x => x.InvSrNum).FirstOrDefault();

        //            if (tblEvcpoddetailObj != null && tblEvcpoddetailObj.InvSrNum <= InvSrNumFromConfig)
        //            {
        //                MaxInvSrNum = InvSrNumFromConfig;
        //            }
        //            else if (tblEvcpoddetailObj != null && tblEvcpoddetailObj.InvSrNum >= InvSrNumFromConfig)
        //            {
        //                MaxInvSrNum = Convert.ToInt32(tblEvcpoddetailObj.InvSrNum);
        //            }
        //            else if (tblEvcpoddetailObj == null && InvSrNumFromConfig > 0)
        //            {
        //                MaxInvSrNum = InvSrNumFromConfig;
        //            }
        //            #endregion

        //            tblOrderLgcListAll = _orderLGCRepository.GetOrderLGCListByDriverIdEVCId(podVM.DriverId, podVM.EVCRegistrationId)
        //                .Where(x => x.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup)).ToList();

        //            if (tblOrderLgcListAll != null && tblOrderLgcListAll.Count > 0)
        //            {
        //                var totalOrderRecords = 0;
        //                int pageSize = 8;
        //                int skip = 0;
        //                int totalPages = 1;
        //                int reminder = 0;

        //                #region if Orders List is greater than [pageSize] records per invoice
        //                totalOrderRecords = tblOrderLgcListAll.Count;
        //                if (totalOrderRecords > pageSize)
        //                {
        //                    totalPages = Convert.ToInt32(totalOrderRecords / pageSize);
        //                    reminder = Convert.ToInt32(totalOrderRecords % pageSize);
        //                    if (reminder > 0) { totalPages++; }
        //                }
        //                for (int i = 1; i <= totalPages; i++)
        //                {
        //                    podVM.DNOrderCount = 0; podVM.InvOrderCount = 0;
        //                    finalAmountDN = 0; finalAmountInv = 0; bool DropCompleted = false; bool PostedCompleted = false;
        //                    podVM.podVMList = new List<PODViewModel>();
        //                    MaxInvSrNum++;
        //                    skip = (i - 1) * pageSize;
        //                    List<TblOrderLgc> orderList = tblOrderLgcListAll.Skip(skip).Take(pageSize).ToList();

        //                    #region Set Counter Sr. Number 
        //                    podVM.BillCounterNum = String.Format("{0:D6}", MaxInvSrNum);
        //                    podPdfName = "POD-" + FinancialYear.Replace("/", "-") + "-" + podVM.BillCounterNum + ".pdf";
        //                    podVM.PoDPdfName = podPdfName;
        //                    #endregion

        //                    #region Initialize POD and Debit Note Data
        //                    foreach (var item1 in orderList)
        //                    {
        //                        if (item1 != null && item1.Evcregistration != null && item1.Logistic != null && item1.OrderTrans != null
        //                            && (item1.OrderTrans.Exchange != null || item1.OrderTrans.Abbredemption != null) && item1.OrderTrans.TblWalletTransactions != null && item1.OrderTrans.TblWalletTransactions.Count > 0)
        //                        {
        //                            var tblWalletTransObj = item1.OrderTrans.TblWalletTransactions.FirstOrDefault(x => x.OrderTransId == item1.OrderTransId);
        //                            if (tblWalletTransObj != null)
        //                            {
        //                                #region Common Variable Declaration for (ABB or Exchange)
        //                                TblExchangeOrder? tblExchangeOrder = null; TblCustomerDetail? tblCustomerDetail = null;
        //                                TblAbbredemption? tblAbbredemption = null;
        //                                TblAbbregistration? tblAbbregistration = null;
        //                                string? productTypeDesc = null; string? productCatDesc = null; decimal FinalExchPriceWithoutSweetner = 0;
        //                                #endregion

        //                                #region Common Implementations for (ABB or Exchange)
        //                                if (item1.OrderTrans.OrderType == Convert.ToInt32(LoVEnum.Exchange))
        //                                {
        //                                    tblExchangeOrder = item1.OrderTrans.Exchange;
        //                                    tblCustomerDetail = tblExchangeOrder?.CustomerDetails;
        //                                    productTypeDesc = tblExchangeOrder?.ProductType?.Name;
        //                                    productCatDesc = tblExchangeOrder?.ProductType?.ProductCat?.Name;
        //                                    #region Condition based set Final Exchange Price
        //                                    bool IsFEPWithoutSweetener = Convert.ToBoolean(tblExchangeOrder?.IsFinalExchangePriceWithoutSweetner ?? false);
        //                                    if (IsFEPWithoutSweetener)
        //                                    {
        //                                        FinalExchPriceWithoutSweetner = Convert.ToDecimal(tblExchangeOrder?.FinalExchangePrice);
        //                                    }
        //                                    else
        //                                    {
        //                                        FinalExchPriceWithoutSweetner = Convert.ToDecimal(tblExchangeOrder?.FinalExchangePrice) - Convert.ToDecimal(tblExchangeOrder?.Sweetener);
        //                                    }
        //                                    #endregion
        //                                }
        //                                else if (item1.OrderTrans.OrderType == Convert.ToInt32(LoVEnum.ABB))
        //                                {
        //                                    tblAbbredemption = item1.OrderTrans.Abbredemption;
        //                                    tblAbbregistration = tblAbbredemption?.Abbregistration;
        //                                    tblCustomerDetail = item1.OrderTrans.Abbredemption?.CustomerDetails;
        //                                    productTypeDesc = tblAbbregistration?.NewProductCategoryTypeNavigation?.Name;
        //                                    productCatDesc = tblAbbregistration?.NewProductCategory?.Name;
        //                                    FinalExchPriceWithoutSweetner = Convert.ToDecimal(item1.OrderTrans?.FinalPriceAfterQc);
        //                                }
        //                                #endregion

        //                                podVMtemp = new PODViewModel();
        //                                //podVMtemp.EVCBussinessName = item1.Evcregistration.BussinessName;
        //                                podVMtemp.EVCBussinessName = item1.Evcregistration.BussinessName;
        //                                podVMtemp.EVCRegdNo = item1.Evcregistration.EvcregdNo;
        //                                podVMtemp.ServicePartnerName = item1.Logistic.ServicePartner.ServicePartnerName;
        //                                podVMtemp.TicketNo = podVMtemp.ServicePartnerName + "-" + item1.Logistic.TicketNumber;
        //                                podVMtemp.RegdNo = item1.Logistic.RegdNo;
        //                                podVMtemp.ProductCatName = productCatDesc;
        //                                if (tblCustomerDetail != null)
        //                                {
        //                                    podVMtemp.CustName = tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName;
        //                                    podVMtemp.CustPincode = tblCustomerDetail.ZipCode;
        //                                }
        //                                podVMtemp.Podurl = EnumHelper.DescriptionAttr(FileAddressEnum.EVCPoD) + podPdfName;

        //                                #region Check BusinessUnit configuration
        //                                podVMtemp.IsDebitNoteSkiped = _commonManager.CheckBUCongigByKey(item1.OrderTransId, BUConfigKeyEnum.IsDebitNoteSkiped.ToString());
        //                                #endregion

        //                                #region Initialize Distinct data for invoice
        //                                decimal? orderAmt = 0;
        //                                orderAmt = tblWalletTransObj.OrderAmount;
        //                                if (Convert.ToBoolean(podVMtemp.IsDebitNoteSkiped ?? false))
        //                                {
        //                                    podVM.InvOrderCount = ++podVM.InvOrderCount;
        //                                    podVMtemp.OrderAmtForEVCInv = Convert.ToDecimal(orderAmt);
        //                                }
        //                                else
        //                                {
        //                                    podVM.DNOrderCount = ++podVM.DNOrderCount;
        //                                    podVM.InvOrderCount = ++podVM.InvOrderCount;
        //                                    podVMtemp.OrderAmtForEVCDN = FinalExchPriceWithoutSweetner; //item1.OrderTrans.TblWalletTransactions.FirstOrDefault(x=>x.OrderTransId == item1.OrderTransId).OrderAmount;
        //                                    finalAmountDN += Convert.ToDecimal(podVMtemp.OrderAmtForEVCDN);
        //                                    podVMtemp.OrderAmtForEVCInv = Convert.ToDecimal(orderAmt) - Convert.ToDecimal(FinalExchPriceWithoutSweetner);
        //                                }
        //                                finalAmountInv += Convert.ToDecimal(podVMtemp.OrderAmtForEVCInv);
        //                                podVM.podVMList.Add(podVMtemp);
        //                                #endregion
        //                            }
        //                        }
        //                    }
        //                    #endregion

        //                    if (podVM != null && podVM.podVMList != null && podVM.podVMList.Count > 0)
        //                    {
        //                        #region Generate PoD
        //                        podVM.FinancialYear = FinancialYear;
        //                        podVM.InvSrNum = MaxInvSrNum;
        //                        podfilePath = EnumHelper.DescriptionAttr(FilePathEnum.EVCPoD);
        //                        podHtmlString = GetPoDHtmlString(podVM, "POD");
        //                        podPDFSave = _htmlToPdfConverterHelper.GeneratePDF(podHtmlString, podfilePath, podPdfName);
        //                        #endregion

        //                        #region Generate and Save Debit Note
        //                        if (podVM.DNOrderCount > 0)
        //                        {
        //                            podVM.evcDetailsVM.FinalPriceDN = finalAmountDN;
        //                            DropCompleted = GenerateAndSaveDebitNote(orderList, podVM, loggedUserId);
        //                        }
        //                        #endregion

        //                        #region Generate and Save Invoice Pdf
        //                        if (podVM.InvOrderCount > 0)
        //                        {
        //                            podVM.evcDetailsVM.FinalPriceInv = finalAmountInv;
        //                            flag = GenerateAndSaveInvoice(orderList, podVM, loggedUserId);
        //                        }
        //                        #endregion
        //                    }
        //                }
        //                #endregion
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.WriteErrorToDB("LogisticManager", "SaveLGCDropStatus", ex);
        //    }
        //    return flag;
        //}
        #endregion

    }
}
