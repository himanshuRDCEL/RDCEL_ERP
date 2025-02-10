using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Users;
using System;
using RDCELERP.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RDCELERP.Common.Enums;
using RDCELERP.Model.Company;
using RDCELERP.Common.Constant;
using RDCELERP.DAL.Helper;
using RDCELERP.Model.Role;
using RDCELERP.Model.LGC;
using Mailjet.Client.Resources;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Utilities.Collections;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.EVC_Allocated;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using RDCELERP.Model.CommonModel;
using RDCELERP.Model.OrderTrans;
using RDCELERP.Model.ABBRedemption;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.BusinessUnit;
using RDCELERP.Model.Master;
using ExcelDataReader.Log.Logger;
using RDCELERP.Model.EVC;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;
using DocumentFormat.OpenXml.Drawing;
using NPOI.SS.Formula.Functions;
using RDCELERP.Model.APICalls;

namespace RDCELERP.BAL.MasterManager
{
    public class CommonManager : ICommonManager
    {
        #region  Variable Declaration
        IApiCallsRepository _apiCallsRepository;
        IAccessListRepository _accessListRepository;
        IRoleAccessRepository _roleAccessRepository;
        IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IOrderTransRepository _orderTransRepository;
        IOrderImageUploadRepository _orderImageUploadRepository;
        ILovRepository _lovRepository;
        IExchangeABBStatusHistoryRepository _exchangeABBStatusHistoryRepository;
        ITemplateConfigurationRepository _configurationRepository;
        private DAL.Entities.Digi2l_DevContext _context;
        IOrderQCRepository _orderQCRepository;
        ISelfQCRepository _selfQCRepository;
        IWebHostEnvironment _webHostEnvironment;
        IVehicleJourneyTrackingDetailsRepository _vehicleJourneyTrackingDetailsRepository;
        IVehicleIncentiveRepository _vehicleIncentiveRepository;
        IPriceMasterMappingRepository _priceMasterMappingRepository;
        IPriceMasterNameRepository _priceMasterNameRepository;
        IBusinessUnitRepository _businessUnitRepository;
        IBrandRepository _brandRepository;
        IBrandSmartBuyRepository _smartBuyRepository;
        IEVCRepository _EVCRepository;
        IPinCodeRepository _pinCodeRepository;
        IAreaLocalityRepository _areaLocalityRepository;
        IEVCPartnerRepository _eVCPartnerRepository;
        IEVCPriceMasterRepository _eVCPriceMasterRepository;
        IEVCPriceRangeMasterRepository _eVCPriceRangeMasterRepository;
        #endregion

        #region Constructor
        public CommonManager(IAccessListRepository accessListRepository, IRoleRepository roleRepository, IRoleAccessRepository roleAccessRepository, IMapper mapper, ILogging logging, IOrderTransRepository orderTransRepository, IOrderImageUploadRepository orderImageUploadRepository, ILovRepository lovRepository, IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository, ITemplateConfigurationRepository configurationRepository, DAL.Entities.Digi2l_DevContext context, IOrderQCRepository orderQCRepository, ISelfQCRepository selfQCRepository, IWebHostEnvironment webHostEnvironment, IVehicleJourneyTrackingDetailsRepository vehicleJourneyTrackingDetailsRepository, IVehicleIncentiveRepository vehicleIncentiveRepository, IPriceMasterMappingRepository priceMasterMappingRepository, IPriceMasterNameRepository priceMasterNameRepository, IBusinessUnitRepository businessUnitRepository, IBrandRepository brandRepository, IBrandSmartBuyRepository smartBuyRepository, IEVCRepository eVCRepository, IPinCodeRepository pinCodeRepository, IAreaLocalityRepository areaLocalityRepository, IEVCPriceMasterRepository eVCPriceMasterRepository, IEVCPriceRangeMasterRepository eVCPriceRangeMasterRepository, IEVCPartnerRepository eVCPartnerRepository, IApiCallsRepository apiCallsRepository)
        {
            _accessListRepository = accessListRepository;
            _roleAccessRepository = roleAccessRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logging = logging;
            _orderTransRepository = orderTransRepository;
            _orderImageUploadRepository = orderImageUploadRepository;
            _lovRepository = lovRepository;
            _exchangeABBStatusHistoryRepository = exchangeABBStatusHistoryRepository;
            _configurationRepository = configurationRepository;
            _context = context;
            _orderQCRepository = orderQCRepository;
            _selfQCRepository = selfQCRepository;
            _webHostEnvironment = webHostEnvironment;
            _vehicleJourneyTrackingDetailsRepository = vehicleJourneyTrackingDetailsRepository;
            _vehicleIncentiveRepository = vehicleIncentiveRepository;
            _priceMasterMappingRepository = priceMasterMappingRepository;
            _priceMasterNameRepository = priceMasterNameRepository;
            _businessUnitRepository = businessUnitRepository;
            _brandRepository = brandRepository;
            _smartBuyRepository = smartBuyRepository;
            _EVCRepository = eVCRepository;
            _pinCodeRepository = pinCodeRepository;
            _areaLocalityRepository = areaLocalityRepository;
            _eVCPriceMasterRepository = eVCPriceMasterRepository;
            _eVCPriceRangeMasterRepository = eVCPriceRangeMasterRepository;
            _eVCPartnerRepository = eVCPartnerRepository;
            _apiCallsRepository = apiCallsRepository;
        }
        #endregion
        /// <summary>
        /// Method to get the All Access List
        /// </summary>     
        /// <returns>AccessListViewModel</returns>
        public IList<AccessListViewModel> GetAllAccessList(int roleid)
        {
            IList<AccessListViewModel> AccessListVMs = null;

            List<TblAccessList> TblAccessList = null;
            List<TblRoleAccess> roleAccesses = null;
            TblRole TblRole = null;
            try
            {
                List<TblAccessList> all_accessList = _accessListRepository.GetList(x => x.IsActive == true && x.IsMenu == true).ToList();
                TblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == roleid);
                if (TblRole != null)
                {
                    if (TblRole.RoleName == EnumHelper.DescriptionAttr(RoleEnum.SuperAdmin))
                    {
                        //roleAccesses = _roleAccessRepository.GetList(x => x.IsActive == true && x.RoleId == roleid).ToList();
                        TblAccessList = all_accessList;
                    }
                    else
                    {
                        roleAccesses = _roleAccessRepository.GetList(x => x.IsActive == true && x.RoleId == roleid && x.CanView == true).ToList();
                        TblAccessList = new List<TblAccessList>();
                        if (roleAccesses != null && all_accessList != null && all_accessList.Count > 0)
                        {
                            foreach (TblAccessList item in all_accessList)
                            {
                                /*TblRoleAccess temproleAccess = roleAccesses.FirstOrDefault(x => x.AccessListId == item.AccessListId && x.CanView == true);
                                */
                                TblRoleAccess temproleAccess = roleAccesses.FirstOrDefault(x => x.AccessListId == item.AccessListId);
                                if (temproleAccess != null)
                                {
                                    TblAccessList.Add(item);
                                }
                            }
                        }
                    }
                }

                if (TblAccessList != null && TblAccessList.Count > 0)
                {
                    AccessListVMs = _mapper.Map<IList<TblAccessList>, IList<AccessListViewModel>>(TblAccessList);
                }
                if (AccessListVMs == null)
                {
                    AccessListVMs = new List<AccessListViewModel>();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CommonManager", "GetAllAccessList", ex);
            }
            return AccessListVMs;
        }

        #region Method To Get List of Images on the basis of order trans id and Lov id
        /// <summary>
        /// Method To Get List of Images on the basis of order trans id and Lov id
        /// </summary>
        /// <param name="orderTransId"></param>
        /// <param name="LoVId"></param>
        /// <returns></returns>
        public List<OrderImageUploadViewModel> GetOrderImagesByTransIdAndLoVId(int? orderTransId, int? LoVId)
        {
            List<OrderImageUploadViewModel> orderImageUploadVMList = null;
            List<TblOrderImageUpload> tblOrderImageUpload = new List<TblOrderImageUpload>();
            try
            {
                if (orderTransId > 0 && LoVId > 0)
                {
                    tblOrderImageUpload = _orderImageUploadRepository.GetList(x => x.IsActive == true && x.ImageUploadby == LoVId && x.OrderTransId == orderTransId).ToList();
                    if (tblOrderImageUpload != null)
                    {
                        orderImageUploadVMList = _mapper.Map<List<TblOrderImageUpload>, List<OrderImageUploadViewModel>>(tblOrderImageUpload);
                    }
                    else
                    {
                        tblOrderImageUpload = new List<TblOrderImageUpload>();
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CommonManager", "GetOrderImagesByTransIdAndLoVId", ex);
            }
            return orderImageUploadVMList;
        }
        #endregion

        #region Method To Get List of Images on the basis of order trans id
        /// <summary>
        /// Method To Get List of Images on the basis of order trans id
        /// </summary>
        /// <param name="orderTransId"></param>
        /// <returns></returns>
        public List<OrderImageUploadViewModel> GetOrderImagesByTransId(int? orderTransId)
        {
            List<OrderImageUploadViewModel> orderImageUploadVMList = null;
            List<TblOrderImageUpload> tblOrderImageUpload = new List<TblOrderImageUpload>();
            try
            {
                if (orderTransId > 0)
                {
                    tblOrderImageUpload = _orderImageUploadRepository.GetList(x => x.IsActive == true && x.OrderTransId == orderTransId).ToList();
                    if (tblOrderImageUpload != null)
                    {
                        orderImageUploadVMList = _mapper.Map<List<TblOrderImageUpload>, List<OrderImageUploadViewModel>>(tblOrderImageUpload);
                    }
                    else
                    {
                        tblOrderImageUpload = new List<TblOrderImageUpload>();
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CommonManager", "GetOrderImagesByTransId", ex);
            }
            return orderImageUploadVMList;
        }
        #endregion

        #region Created by Priyanshi --Common Method for Insert into tblexchangeabbhistory
        public void InsertExchangeAbbstatusHistory(TblExchangeAbbstatusHistory tblExchangeAbbstatusHistorys)
        {
            try
            {
                TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();

                // tblExchangeAbbstatusHistory.StatusHistoryId = tblExchangeAbbstatusHistorys.StatusHistoryId;
                tblExchangeAbbstatusHistory.OrderType = tblExchangeAbbstatusHistorys.OrderType > 0 ? tblExchangeAbbstatusHistorys.OrderType : 0;
                tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeAbbstatusHistorys.SponsorOrderNumber != null ? tblExchangeAbbstatusHistorys.SponsorOrderNumber : string.Empty;
                tblExchangeAbbstatusHistory.RegdNo = tblExchangeAbbstatusHistorys.RegdNo != null ? tblExchangeAbbstatusHistorys.RegdNo : string.Empty;
                tblExchangeAbbstatusHistory.ZohoSponsorId = tblExchangeAbbstatusHistorys.ZohoSponsorId != null ? tblExchangeAbbstatusHistorys.ZohoSponsorId : string.Empty;
                tblExchangeAbbstatusHistory.CustId = tblExchangeAbbstatusHistorys.CustId > 0 ? tblExchangeAbbstatusHistorys.CustId : 0;
                tblExchangeAbbstatusHistory.StatusId = tblExchangeAbbstatusHistorys.StatusId > 0 ? tblExchangeAbbstatusHistorys.StatusId : 0;
                tblExchangeAbbstatusHistory.IsActive = tblExchangeAbbstatusHistorys.IsActive != null ? tblExchangeAbbstatusHistorys.IsActive : false;
                tblExchangeAbbstatusHistory.CreatedBy = tblExchangeAbbstatusHistorys.CreatedBy != null ? tblExchangeAbbstatusHistorys.CreatedBy : 0;
                tblExchangeAbbstatusHistory.CreatedDate = tblExchangeAbbstatusHistorys.CreatedDate != null ? tblExchangeAbbstatusHistorys.CreatedDate : _currentDatetime;
                tblExchangeAbbstatusHistory.OrderTransId = tblExchangeAbbstatusHistorys.OrderTransId != null ? tblExchangeAbbstatusHistorys.OrderTransId : 0;
                tblExchangeAbbstatusHistory.Comment = tblExchangeAbbstatusHistorys.Comment != null ? tblExchangeAbbstatusHistorys.Comment : string.Empty;
                if (tblExchangeAbbstatusHistorys.ServicepartnerId > 0)
                {
                    tblExchangeAbbstatusHistory.ServicepartnerId = tblExchangeAbbstatusHistorys.ServicepartnerId != null ? tblExchangeAbbstatusHistorys.ServicepartnerId : 0;
                }
                if (tblExchangeAbbstatusHistorys.DriverDetailId > 0)
                {
                    tblExchangeAbbstatusHistory.DriverDetailId = tblExchangeAbbstatusHistorys.DriverDetailId != null ? tblExchangeAbbstatusHistorys.DriverDetailId : 0;
                }
                tblExchangeAbbstatusHistory.Evcid = tblExchangeAbbstatusHistorys.Evcid != null ? tblExchangeAbbstatusHistorys.Evcid : 0;

                tblExchangeAbbstatusHistory.JsonObjectString = JsonConvert.SerializeObject(tblExchangeAbbstatusHistory);
                _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                _exchangeABBStatusHistoryRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CommonManager", "InsertExchangeAbbstatusHistory", ex);
            }
        }

        #endregion

        #region Update into TblExchangeOrders
        //public void updateExchangeandOrderTrans(TblExchangeAbbstatusHistory tblExchangeAbbstatusHistorys)
        //{

        //   TblExchangeOrder TblExchangeOrders = _exchangeOrderRepository.GetSingle(x => x.IsActive == true && x.RegdNo == tblExchangeAbbstatusHistorys.RegdNo);
        //    if (TblExchangeOrders != null)
        //    {
        //        TblExchangeOrders.StatusId = tblExchangeAbbstatusHistorys.StatusId;
        //        TblExchangeOrders.OrderStatus = "EVC_Assign";
        //        TblExchangeOrders.ModifiedBy = tblExchangeAbbstatusHistorys.CreatedBy;
        //        TblExchangeOrders.ModifiedDate = _currentDatetime;
        //        _exchangeOrderRepository.Update(TblExchangeOrders);
        //        _exchangeOrderRepository.SaveChanges();
        //    }
        //    #region Update into TblOrderTrans
        //   TblOrderTran tblOrderTran = _orderTransactionRepository.GetSingle(x => x.OrderTransId == tblExchangeAbbstatusHistorys.OrderTransId);
        //    if (tblOrderTran != null)
        //    {
        //        tblOrderTran.Evcprice = tblWalletTransaction.OrderAmount;
        //        tblOrderTran.StatusId = tblExchangeAbbstatusHistorys.StatusId;
        //        tblOrderTran.ModifiedBy = tblExchangeAbbstatusHistorys.CreatedBy;
        //        tblOrderTran.ModifiedDate = _currentDatetime;
        //        _orderTransactionRepository.Update(tblOrderTran);
        //        _orderTransactionRepository.SaveChanges();
        //    }
        //    #endregion
        //  }
        #endregion

        #region Common method for All Templates
        public string GetTemplate(string Templatename)
        {
            TblConfiguration tblConfiguration = null;
            string Value = null;
            if (Templatename != null)
            {
                tblConfiguration = _configurationRepository.GetSingleTemplate(Templatename);
                if (tblConfiguration != null && tblConfiguration.Value != null)
                {
                    Value = tblConfiguration.Value;
                }
            }
            return Value;
        }
        #endregion

        #region CalculateEVCPrice
        public int CalculateEVCPrice(int OrderTransId)
        {
            // Initialize variables
            TblOrderTran? tblOrderTran = null;
            int OrderPrice = 0;
            int LGCCost = 0;
            decimal UTCPer = 0;
            int EVCAmount = 0;
            int AmountWithLGCCost = 0;
            TblEvcPriceMaster? tblEvcPriceMaster = null;
            TblEvcpriceRangeMaster? tblEvcpriceRangeMaster = null;

            try
            {

                // Retrieve TblOrderTran based on OrderTransId
                tblOrderTran = _orderTransRepository.GetSingleOrderWithExchangereference(OrderTransId);

                if (tblOrderTran != null&&tblOrderTran.FinalPriceAfterQc>0)
                {
                    // Retrieve TblConfiguration for "UseEVCPriceMater" setting
                    var tblConfiguration = _context.TblConfigurations
                        .FirstOrDefault(x => x.Name == "UseEVCPriceMater" && x.IsActive == true); 

                    if (tblConfiguration != null && tblConfiguration.Value == "1")
                    {
                        // Calculate OrderPrice without Sweetener

                        //OrderPrice = (int)(tblOrderTran.Exchange.FinalExchangePrice - (tblOrderTran.Exchange.Sweetener > 0 ? tblOrderTran.Exchange.Sweetener : 0));
                        if (tblOrderTran.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                        {
                            if (tblOrderTran.Exchange.IsFinalExchangePriceWithoutSweetner == true)
                            {
                                OrderPrice = (int)(tblOrderTran.FinalPriceAfterQc);
                            }
                            else
                            {
                                OrderPrice = (int)(tblOrderTran.FinalPriceAfterQc - (tblOrderTran.Sweetner > 0 ? tblOrderTran.Sweetner : 0));
                            }
                        }
                        else
                        {
                            OrderPrice = (int)(tblOrderTran.FinalPriceAfterQc - (tblOrderTran.Sweetner > 0 ? tblOrderTran.Sweetner : 0));
                        }
                        if (OrderPrice != 0)
                        {
                            // Retrieve TblEvcPriceMaster for the specified ProductType, ProductCategory, and BusinessUnit
                            if (tblOrderTran.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                            {
                                tblEvcPriceMaster = _context.TblEvcPriceMasters
                                   .FirstOrDefault(x => x.ProductTypeId == tblOrderTran.Exchange.ProductType.Id &&
                                                        x.ProductCategoryId == tblOrderTran.Exchange.ProductType.ProductCat.Id &&
                                                        x.BusinessUnitId == tblOrderTran.Exchange.BusinessPartner.BusinessUnit.BusinessUnitId);
                            }
                            else
                            {
                                tblEvcPriceMaster = _context.TblEvcPriceMasters
                                      .FirstOrDefault(x => x.ProductTypeId == tblOrderTran.Abbredemption.Abbregistration.NewProductCategoryTypeId &&
                                                           x.ProductCategoryId == tblOrderTran.Abbredemption.Abbregistration.NewProductCategoryId &&
                                                           x.BusinessUnitId == tblOrderTran.Abbredemption.Abbregistration.BusinessUnitId);

                            }

                            if (tblEvcPriceMaster != null)
                            {
                                LGCCost = tblEvcPriceMaster.Lgccost != null ? (int)tblEvcPriceMaster.Lgccost : 0;

                            }
                            AmountWithLGCCost = OrderPrice + LGCCost;
                            // Retrieve TblEvcpriceRangeMaster based on AmountWithLGCCost and BusinessUnitId
                            if (tblOrderTran.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                            {
                                tblEvcpriceRangeMaster = _context.TblEvcpriceRangeMasters
                                .FirstOrDefault(x => x.IsActive == true && x.PriceStartRange <= AmountWithLGCCost &&
                                                     x.PriceEndRange >= AmountWithLGCCost &&
                                                     x.BusinessUnitId == tblOrderTran.Exchange.BusinessPartner.BusinessUnit.BusinessUnitId);
                            }
                            else
                            {
                                tblEvcpriceRangeMaster = _context.TblEvcpriceRangeMasters
                                   .FirstOrDefault(x => x.IsActive == true && x.PriceStartRange <= AmountWithLGCCost &&
                                                        x.PriceEndRange >= AmountWithLGCCost &&
                                                        x.BusinessUnitId == tblOrderTran.Abbredemption.Abbregistration.BusinessUnit.BusinessUnitId);
                            }
                            if (tblEvcpriceRangeMaster != null)
                            {
                                UTCPer = (decimal)tblEvcpriceRangeMaster.EvcApplicablePercentage;

                                if (OrderPrice > 0)
                                {
                                    // Calculate EVC Price based on UTCPer
                                    decimal EVCPercentage = (AmountWithLGCCost * UTCPer) / 100;
                                    EVCAmount = Convert.ToInt32(AmountWithLGCCost + EVCPercentage);
                                    return EVCAmount;
                                }
                                else
                                {
                                    // LGC Cost not available
                                    EVCAmount = -1;
                                    return EVCAmount;
                                }
                            }
                            else if (tblEvcpriceRangeMaster == null && AmountWithLGCCost > 0)
                            {
                                // No range found, return Amount
                                EVCAmount = Convert.ToInt32(AmountWithLGCCost);
                                return EVCAmount;
                            }
                            else
                            {
                                // UTC Per not available
                                EVCAmount = -2;
                                return EVCAmount;
                            }
                        }

                        else
                        {
                            // UseEVCPriceMater is not enabled
                            EVCAmount = 0;
                            return EVCAmount;
                        }

                    }
                    else
                    {
                        // tblOrderTran is null
                        return EVCAmount;
                    }
                }
                else
                {
                    return EVCAmount;
                }

            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately or log them
                Console.WriteLine("An error occurred: " + ex.Message);
                return EVCAmount;
            }
        }

        #endregion

        #region Method To Delete selfQc video
        /// <param name="regdno"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public TblSelfQc GetDeleteSelfQCVideo(string regdno)
        {
            TblSelfQc tblSelfQc = null;
            try
            {
                if (regdno != null)
                {
                    tblSelfQc = _selfQCRepository.GetSelfqcorder(regdno);
                    if (tblSelfQc != null)
                    {
                        tblSelfQc.IsActive = false;
                        tblSelfQc.ModifiedDate = _currentDatetime;
                        _selfQCRepository.Update(tblSelfQc);
                        _selfQCRepository.SaveChanges();
                        string path = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"\DBFiles\QC\SelfQC\" + tblSelfQc.ImageName);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)//check file exsit or not  
                        {
                            file.Delete();
                        }
                        else
                        {
                            Console.WriteLine("File not Exits!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CommonManager", "GetDeleteSelfQCVideo", ex);
            }
            return tblSelfQc;
        }
        #endregion

        #region Common Trans Data Mapping - Added by Pooja
        /// <summary>
        /// Common Trans Data Mapping 
        /// </summary>
        /// <param name="tblOrderTran"></param>
        /// <returns></returns>
        public MapOrderTransModel MapOrderTransData(TblOrderTran tblOrderTran)
        {
            MapOrderTransModel mapOrderTransModel = new MapOrderTransModel();
            BusinessUnitViewModel businessUnitData = new BusinessUnitViewModel();
            try
            {
                if (tblOrderTran != null)
                {
                    mapOrderTransModel = _mapper.Map<TblOrderTran, MapOrderTransModel>(tblOrderTran);
                }
                else
                {
                    mapOrderTransModel = null;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ConfirmPaymentDetails", "MapOrderTransData", ex);
            }

            return mapOrderTransModel;

        }
        #endregion

        #region CalculateDriverIncentive
        public bool CalculateDriverIncentive(int OrderTransId)
        {
            try
            {
                #region tblOrderTran
                TblOrderTran tblOrderTran = _orderTransRepository.GetSingleOrderWithExchangereference(OrderTransId);
                if (tblOrderTran == null)
                {
                    // Order not available
                    return false;
                }
                #endregion

                #region tblVehicleIncentive
                TblVehicleIncentive tblVehicleIncentive = _vehicleIncentiveRepository.GetSingle(x =>
                    x.IsActive == true 
                    && x.ProductTypeId == (tblOrderTran?.Exchange!=null?tblOrderTran?.Exchange?.ProductTypeId: tblOrderTran?.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation?.Id) 
                    && x.ProductCategoryId == (tblOrderTran?.Exchange != null ? tblOrderTran?.Exchange?.ProductType?.ProductCatId : tblOrderTran?.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation?.ProductCatId)
                    && x.BusinessUnitId == (tblOrderTran?.Exchange != null?tblOrderTran?.Exchange?.BusinessPartner?.BusinessUnitId : tblOrderTran?.Abbredemption?.Abbregistration?.BusinessUnitId));
                if (tblVehicleIncentive == null)
                {
                    // tblVehicleIncentive entry not found 
                    return false;
                }
                #endregion

                #region tblVehicleJourneyTrackingDetail
                //TblVehicleJourneyTrackingDetail tblVehicleJourneyTrackingDetail = _vehicleJourneyTrackingDetailsRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == OrderTransId);

                TblVehicleJourneyTrackingDetail tblVehicleJourneyTrackingDetail = _vehicleJourneyTrackingDetailsRepository.GetVehicleJourneyTrackingDetail(OrderTransId);
                if (tblVehicleJourneyTrackingDetail == null)
                {
                    // tblVehicleJourneyTrackingDetail valid data not found
                    return false;
                }
                #endregion
               
                if (tblVehicleJourneyTrackingDetail.PickupStartDatetime != null && tblVehicleJourneyTrackingDetail.PickupEndDatetime != null)
                {
                    tblVehicleJourneyTrackingDetail.BasePrice = tblVehicleIncentive.BasePrice;

                    #region Calculate PickupTat
                    if (tblVehicleJourneyTrackingDetail.PickupTat == null)
                    {
                        DateTime startDatetime = (DateTime)tblVehicleJourneyTrackingDetail.PickupStartDatetime;
                        DateTime endDatetime = (DateTime)tblVehicleJourneyTrackingDetail.PickupEndDatetime;
                        TimeSpan pickupTat = endDatetime - startDatetime;
                        string pickupTatString = string.Format("{0}:{1}:{2}", pickupTat.Hours, pickupTat.Minutes, pickupTat.Seconds);
                        if (TimeSpan.TryParse(pickupTatString, out TimeSpan pickupTimeSpan))
                        {
                            tblVehicleJourneyTrackingDetail.PickupTat = pickupTimeSpan;
                        }

                        if (tblVehicleJourneyTrackingDetail.PickupTat <= tblVehicleIncentive.PickupTatinHr)
                        {
                            tblVehicleJourneyTrackingDetail.PickupInc = tblVehicleIncentive.PickupIncAmount;
                            if (tblVehicleJourneyTrackingDetail.IsPackedImg == true)
                            {
                                tblVehicleJourneyTrackingDetail.PackingInc = tblVehicleIncentive.PackagingIncentive;
                            }
                            else
                            {
                                tblVehicleJourneyTrackingDetail.PackingInc = 0;
                            }
                        }
                        else
                        {
                            tblVehicleJourneyTrackingDetail.PickupInc = 0;
                            tblVehicleJourneyTrackingDetail.PackingInc = 0;
                        }
                    }
                    #endregion
                }
                if (tblVehicleJourneyTrackingDetail.OrderDropTime != null && tblVehicleJourneyTrackingDetail.DropTat == null)
                {
                    #region Calculate DropTat
                    DateTime orderdroptime = (DateTime)tblVehicleJourneyTrackingDetail.OrderDropTime;
                    DateTime pickupstarttime = (DateTime)tblVehicleJourneyTrackingDetail.PickupStartDatetime;
                 
                    TimeSpan dropTat = (orderdroptime - pickupstarttime);
                    string dropTatString = string.Format("{0}:{1}:{2}", dropTat.Hours, dropTat.Minutes, dropTat.Seconds);



                    if (TimeSpan.TryParse(dropTatString, out TimeSpan dropTimeSpan))
                    {
                        tblVehicleJourneyTrackingDetail.DropTat = dropTimeSpan;
                    }
                    if (tblVehicleJourneyTrackingDetail.DropTat <= tblVehicleIncentive.DropTatinHr)
                    {
                        tblVehicleJourneyTrackingDetail.DropInc = tblVehicleIncentive.DropIncAmount;
                        tblVehicleJourneyTrackingDetail.DropImageInc = tblVehicleIncentive.DropImageIncentive ?? 0;
                    }
                    else
                    {
                        tblVehicleJourneyTrackingDetail.DropInc = 0;
                        tblVehicleJourneyTrackingDetail.DropImageInc = 0;
                    }
                    #endregion
                }
                #region Calculate Total Price
                int TBasePrice = (int)(tblVehicleJourneyTrackingDetail.BasePrice ?? 0);
                int TpickupInsentive = (int)(tblVehicleJourneyTrackingDetail.PickupInc ?? 0);
                int TpackingInsentive = (int)(tblVehicleJourneyTrackingDetail.PackingInc ?? 0);
                int TdropInsentive = (int)(tblVehicleJourneyTrackingDetail.DropInc ?? 0);
                int TdropImagInsentive = (int)(tblVehicleJourneyTrackingDetail.DropImageInc ?? 0);
                #endregion

                #region Calculate ExpectedErning
                int EBasePrice = (int)(tblVehicleIncentive.BasePrice ?? 0);
                int EpickupInsentive = (int)(tblVehicleIncentive.PickupIncAmount ?? 0);
                int EpackingInsentive = (int)(tblVehicleIncentive.PackagingIncentive ?? 0);
                int EdropInsentive = (int)(tblVehicleIncentive.DropIncAmount ?? 0);
                int EdropImagInsentive = (int)(tblVehicleIncentive.DropImageIncentive ?? 0);
                #endregion
                int TotalErning = TBasePrice + TpickupInsentive + TpackingInsentive + TdropInsentive + TdropImagInsentive;
                int ExpectedErning = EBasePrice + EpickupInsentive + EpackingInsentive + EdropInsentive + EdropImagInsentive;

                tblVehicleJourneyTrackingDetail.Total = TotalErning;
                tblVehicleJourneyTrackingDetail.EstimateEarning = ExpectedErning;
                _vehicleJourneyTrackingDetailsRepository.Update(tblVehicleJourneyTrackingDetail);
                int result = _vehicleJourneyTrackingDetailsRepository.SaveChanges();
                if (result == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately or log them
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }

        #endregion

        #region Method to check is UPI is required or not - Added by Pooja
        /// <summary>
        /// Method to check is UPI is required or not
        /// </summary>
        /// <param name="Tblordertrans"></param>
        /// <returns></returns>
        public bool CheckUpiisRequired(int ordertransid)
        {
            bool isupiflag = true;
            TblOrderTran tblOrderTran = null;
            tblOrderTran = _orderTransRepository.GetExchangeABBbyordertrans(ordertransid);
            if (tblOrderTran != null && tblOrderTran.OrderType == (int?)LoVEnum.Exchange)
            {
                if (tblOrderTran.Exchange.BusinessPartner.BusinessUnit.IsUpiIdRequired == false)
                {
                    isupiflag = false;
                }
            }
            else
            {
                if (tblOrderTran.Abbredemption.Abbregistration.BusinessUnit.IsUpiIdRequired == false)
                {
                    isupiflag = false;
                }
            }
            return isupiflag;
        }
        #endregion

        #region GetAllPriceMasterNameList 
        /// <summary>
        /// Added by Ashwin for getting all GetAllPriceMasterNameList
        /// </summary>
        /// <returns></returns>
        public List<PriceMasterNameViewModel> GetAllPriceMasterNameList()
        {
            List<PriceMasterNameViewModel> priceMasterNameViewModel = null;
            try
            {
                List<TblPriceMasterName> PriceMasterName = _priceMasterNameRepository.GetAll().ToList();
                if (PriceMasterName != null && PriceMasterName.Count > 0)
                {
                    priceMasterNameViewModel = _mapper.Map<List<TblPriceMasterName>, List<PriceMasterNameViewModel>>(PriceMasterName);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CommonManager", "PriceMasterMappingViewModel", ex);
            }
            return priceMasterNameViewModel;
        }
        #endregion

        #region Check Business Unit Configuration for All
        public bool CheckBUCongigByKey(int orderTransId, string configKey)
        {
            #region Variable Declaration
            bool IsBillingPartial = false;
            bool IsDebitNoteSkiped = false;
            bool flag = false;
            TblBuconfigurationMapping? tblBuconfigMappingObj = null;
            List<TblBuconfigurationMapping>? tblBuconfigMappingList = null;
            #endregion
            try
            {
                if (configKey == BUConfigKeyEnum.IsDebitNoteSkiped.ToString())
                {
                    tblBuconfigMappingList = _businessUnitRepository.GetBUConfigListByTransId(orderTransId);
                    if (tblBuconfigMappingList != null && tblBuconfigMappingList.Count > 0)
                    {
                        foreach (TblBuconfigurationMapping tblBuconfigMapping in tblBuconfigMappingList)
                        {
                            if (tblBuconfigMapping.Config?.Key == configKey || (tblBuconfigMapping.Config?.Key == BUConfigKeyEnum.IsBillingPartial.ToString()))
                            {
                                if ((tblBuconfigMapping.Config?.Key == BUConfigKeyEnum.IsBillingPartial.ToString()) && Convert.ToBoolean(tblBuconfigMapping.Value))
                                {
                                    IsBillingPartial = true;
                                }
                                else if ((tblBuconfigMapping.Config?.Key == BUConfigKeyEnum.IsDebitNoteSkiped.ToString()) && Convert.ToBoolean(tblBuconfigMapping.Value))
                                {
                                    IsDebitNoteSkiped = true;
                                }
                            }
                            if (IsBillingPartial && IsDebitNoteSkiped)
                            {
                                flag = true;
                                break;
                            }
                            //else if (tblBuconfigMapping.Config?.Key == configKey)
                            //{
                            //    if (Convert.ToBoolean(tblBuconfigMapping.Value))
                            //    {
                            //        flag = true;
                            //        break;
                            //    }
                            //}
                        }
                    }
                }
                else
                {
                    tblBuconfigMappingObj = _businessUnitRepository.GetBUConfigDetailsByTransId(orderTransId, configKey);
                    if (tblBuconfigMappingObj != null)
                    {
                        if (Convert.ToBoolean(tblBuconfigMappingObj.Value))
                        {
                            flag = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CommonManager", "CheckBUCongigByKey", ex);
            }
            return flag;
        }
        #endregion

        #region Get Brand Details for AbbRedemption by Id from Different Tables
        public BrandViewModel GetAbbBrandDetailsById(bool? IsBuMultiBrand, int? NewBrandId)
        {
            TblBrand tblBrand = null;
            TblBrandSmartBuy tblBrandSmartBuy = null;
            BrandViewModel? brandVM = null;
            try
            {
                if (NewBrandId > 0)
                {
                    if (IsBuMultiBrand == true)
                    {
                        tblBrandSmartBuy = _smartBuyRepository.GetBrand(NewBrandId);
                        if (tblBrandSmartBuy != null)
                        {
                            brandVM = _mapper.Map<TblBrandSmartBuy, BrandViewModel>(tblBrandSmartBuy);
                        }
                    }
                    else
                    {
                        tblBrand = _brandRepository.GetBrand(NewBrandId);
                        if (tblBrand != null)
                        {
                            brandVM = _mapper.Map<TblBrand, BrandViewModel>(tblBrand);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CommonManager", "GetAbbBrandDetailsById", ex);
            }
            return brandVM;
        }
        #endregion

        #region Calculate EVC clear Balance
        public EVCClearBalanceViewModel? CalculateEVCClearBalance(int EVCRegistrationId)
        {
            EVCClearBalanceViewModel? eVCClearBalanceViewModel = null;
            List<TblWalletTransaction>? TblWalletTransactions = null;
            decimal? IsProgress = 0;
            decimal? IsDeleverd = 0;
            decimal? RunningBlns = 0;

            if (EVCRegistrationId > 0)
            {
                TblEvcregistration EVC_Reg = _EVCRepository.GetSingle(x => x.IsActive == true && x.Isevcapprovrd == true && x.EvcregistrationId== EVCRegistrationId);
                if (EVC_Reg != null )
                {
                    TblWalletTransactions = _context.TblWalletTransactions.Where(x => x.EvcregistrationId == EVC_Reg.EvcregistrationId && x.IsActive == true && x.StatusId != 26.ToString() && x.StatusId != 21.ToString() && x.StatusId != 44.ToString()).ToList();

                    if (TblWalletTransactions != null)
                    {
                        TblWalletTransaction tblWalletTransaction = new TblWalletTransaction();
                        eVCClearBalanceViewModel = new EVCClearBalanceViewModel();
                        foreach (var items in TblWalletTransactions)
                        {
                            if (items.OrderofAssignDate !=null&& items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate == null && items.OrderOfCompleteDate == null && items.StatusId != 26.ToString() && items.StatusId != 21.ToString() && items.StatusId != 44.ToString())
                            {
                                IsProgress += items.OrderAmount;
                            }
                            if (items.OrderofAssignDate != null && items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate != null && items.OrderOfCompleteDate == null && items.StatusId != 26.ToString() && items.StatusId != 21.ToString() && items.StatusId != 44.ToString())
                            {
                                IsDeleverd += items.OrderAmount;
                            }
                        }                 
                    }
                    eVCClearBalanceViewModel.InProgresAmount = IsProgress == null ? 0 : IsProgress;
                    eVCClearBalanceViewModel.DeliverdAmount = IsDeleverd == null ? 0 : IsDeleverd;
                    eVCClearBalanceViewModel.walletAmount = EVC_Reg.EvcwalletAmount == null ? 0 : EVC_Reg.EvcwalletAmount;
                    eVCClearBalanceViewModel.clearBalance = eVCClearBalanceViewModel.walletAmount -(eVCClearBalanceViewModel.InProgresAmount + eVCClearBalanceViewModel.DeliverdAmount);
                }
            }
            return eVCClearBalanceViewModel;
        }
        #endregion

        #region Check Entered City is Valid Added By PJ
        public IList<string> CheckCityisValid(string cityname, int StateId)
        {
            
            IList<string> list = null;
            if (cityname == null)
            {
                return list;
            }
            else
            {
                cityname = cityname.TrimStart();
                list = _context.TblCities
                           .Where(e => e.StateId == StateId && e.Name.Contains(cityname))
                           .Select(e => e.Name)
                           .ToArray();
                if (list.Count == 0)
                {
                    list = null;
                }
            }
            return list;
        }
        #endregion

        #region Check State is Valid Added By PJ
        public IList<string> CheckStateisValid(string Statename)
        {
            IList<string> list = null;
            if (Statename == null)
            {
                return list;
            }
            else
            {
                Statename = Statename.TrimStart();
                list = _context.TblStates
                           .Where(e => e.Name.Contains(Statename))
                           .Select(e => e.Name)
                           .ToArray();
                if (list.Count == 0)
                {
                    list = null;
                }
            }
            return list;
        }
        #endregion

        #region Check Prodcat is Valid Added By PJ
        public IList<string> CheckProdcatValid(string Prodcat)
        {
            IList<string> list = null;
            if (Prodcat == null)
            {
                return list;
            }
            else
            {
                Prodcat = Prodcat.TrimStart();
                list = _context.TblProductCategories
                           .Where(e => e.Description.Contains(Prodcat))
                           .Select(e => e.Description)
                           .ToArray();
                if (list.Count == 0)
                {
                    list = null;
                }
            }
            return list;
        }
        #endregion

        #region Check Product Type is Valid Added By PJ
        public IList<string> CheckProdTypeValid(string ProdType)
        {
            IList<string> list = null;
            if (ProdType == null)
            {
                return list;
            }
            else
            {
                ProdType = ProdType.TrimStart();
                list = _context.TblProductTypes
                           .Where(e => e.Description.Contains(ProdType))
                           .Select(e => e.Description)
                           .ToArray();
                if (list.Count == 0)
                {
                    list = null;
                }
            }
            return list;
        }
        #endregion

        #region Check Brand is Valid Added By PJ
        public IList<string> CheckBrandValid(string Brand)
        {
            IList<string> list = null;
            if (Brand == null)
            {
                return list;
            }
            else
            {
                Brand = Brand.TrimStart();
                list = _context.TblBrands
                           .Where(e => e.Name.Contains(Brand))
                           .Select(e => e.Name)
                           .ToArray();
                if (list.Count == 0)
                {
                    list = null;
                }
            }
            return list;
        }
        #endregion

        #region Check Pincode is Valid Added By PJ
        public TblPinCode CheckPincodeValid(int Pincode, string Editcityname, string Editstatename)
        {
            TblPinCode? tblPinCode = null;
            if (Pincode == 0)
            {
                return tblPinCode;
            }
            else
            {
                if (Pincode > 0 && !string.IsNullOrEmpty(Editcityname) && !string.IsNullOrEmpty(Editstatename))
                {
                    tblPinCode = _pinCodeRepository.GetPincodebycity(Pincode, Editcityname, Editstatename);
                }
            }
            return tblPinCode;
        }
        #endregion

        #region Check Area Locaity is Valid Added By PJ
        public TblAreaLocality CheckArealocaityValid(string Arealocality)
        {
            TblAreaLocality? areaLocality = null;
            if (Arealocality != null)
            {
                return areaLocality;
            }
            else
            {
                areaLocality = _areaLocalityRepository.GetArealocalityByname(Arealocality);
            }
            return areaLocality;
        }
        #endregion

        #region Methods for EVC Price Calculations Algo New Added by VK
        #region Calculate the EVC Price
        /// <summary>
        /// Methods for EVC Price Calculations Algo New Added by VK
        /// </summary>
        /// <param name="OrderTransId"></param>
        /// <param name="IsSweetenerAmtInclude"></param>
        /// <param name="GstTypeId"></param>
        /// <returns></returns>
        public EVCPriceViewModel CalculateEVCPriceNew1(int OrderTransId, bool? IsSweetenerAmtInclude, int? GstTypeId = null)
        {
            #region Variable Declaration
            EVCPriceViewModel evcPriceVM = new EVCPriceViewModel();
            TblOrderTran? tblOrderTran = null;
            int OrderPrice = 0;
            int SweetenerAmt = 0;
            decimal GstAmt = 0;
            int LGCCost = 0;
            decimal UTCPer = 0;
            decimal EVCPercentage = 0;
            int EVCAmount = 0;
            int AmountWithLGCCost = 0;
            TblEvcPriceMaster? tblEvcPriceMaster = null;
            TblEvcpriceRangeMaster? tblEvcpriceRangeMaster = null;
            int? prodTypeId = 0;
            int? prodCatId = 0;
            int? buid = 0;
            #endregion
            try
            {
                // Retrieve TblOrderTran based on OrderTransId
                tblOrderTran = _orderTransRepository.GetSingleOrderWithExchangereference(OrderTransId);

                if (tblOrderTran != null && tblOrderTran.FinalPriceAfterQc > 0)
                {
                    //Retrieve TblConfiguration for "UseEVCPriceMater" setting
                    var tblConfiguration = _context.TblConfigurations
                        .FirstOrDefault(x => x.Name == "UseEVCPriceMater" && x.IsActive == true);

                    if (tblConfiguration != null && tblConfiguration.Value == "1")
                    {
                        #region Get Order Details
                        OrderPrice = Convert.ToInt32(tblOrderTran.FinalPriceAfterQc ?? 0);
                        SweetenerAmt = Convert.ToInt32(tblOrderTran.Sweetner ?? 0);
                        if (tblOrderTran.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                        {
                            if (tblOrderTran.Exchange?.IsFinalExchangePriceWithoutSweetner != true)
                            {
                                OrderPrice = OrderPrice - SweetenerAmt;
                            }
                            prodTypeId = tblOrderTran?.Exchange?.ProductType?.Id;
                            prodCatId = tblOrderTran?.Exchange?.ProductType?.ProductCat?.Id;
                            buid = tblOrderTran?.Exchange?.BusinessPartner?.BusinessUnit?.BusinessUnitId;
                        }
                        else
                        {
                            OrderPrice = OrderPrice - SweetenerAmt;
                            prodTypeId = tblOrderTran?.Abbredemption?.Abbregistration?.NewProductCategoryTypeId;
                            prodCatId = tblOrderTran?.Abbredemption?.Abbregistration?.NewProductCategoryId;
                            buid = tblOrderTran?.Abbredemption?.Abbregistration?.BusinessUnitId;
                        }

                        LGCCost = GetLGCCost(prodTypeId, prodCatId, buid);
                        AmountWithLGCCost = OrderPrice + LGCCost;
                        #endregion

                        #region Sweetener Implementation as per EVC
                        if (IsSweetenerAmtInclude == true)
                        {
                            if (OrderPrice > 0)
                            {
                                EVCPercentage = GetUTCCost(OrderPrice, buid);
                            }
                            if (GstTypeId > 0)
                            {
                                GstAmt = GetGstAmount(EVCPercentage, GstTypeId);
                            }
                            else
                            {
                                GstAmt = GetGstAmount(EVCPercentage, Convert.ToInt32(LoVEnum.GSTExclusive));
                            }
                            //Add Sweetener Amount
                            AmountWithLGCCost = AmountWithLGCCost + SweetenerAmt;
                        }
                        else
                        {
                            if (AmountWithLGCCost > 0)
                            {
                                EVCPercentage = GetUTCCost(AmountWithLGCCost, buid);
                            }
                        }
                        #endregion

                        #region Calculate EVC Price
                        EVCAmount = Convert.ToInt32(AmountWithLGCCost + EVCPercentage + GstAmt);
                        #endregion

                        #region Map Data into the View Model
                        evcPriceVM.OrderTransId = OrderTransId;
                        evcPriceVM.OrderPrice = OrderPrice;
                        evcPriceVM.SweetenerAmt = SweetenerAmt;
                        evcPriceVM.LGCCost = LGCCost;
                        evcPriceVM.AmountWithLGCCost = AmountWithLGCCost;
                        evcPriceVM.UTCPer = UTCPer;
                        evcPriceVM.UTCAmount = EVCPercentage;
                        evcPriceVM.GstAmount = GstAmt;
                        evcPriceVM.EVCAmount = EVCAmount;
                        evcPriceVM.IsSweetenerAmtInclude = IsSweetenerAmtInclude;
                        evcPriceVM.GstTypeId = GstTypeId;
                        evcPriceVM.ProdCatId = prodCatId;
                        evcPriceVM.ProdTypeId = prodTypeId;
                        evcPriceVM.Buid = buid;
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately or log them
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return evcPriceVM;
        }
        #endregion

        #region Calculate EVC Price with Detailed add by VK
        /// <summary>
        /// Calculate EVC Price with Detailed add by VK
        /// </summary>
        /// <param name="tblOrderTran"></param>
        /// <param name="IsSweetenerAmtInclude"></param>
        /// <param name="GstTypeId"></param>
        /// <returns></returns>
        public EVCPriceViewModel CalculateEVCPriceDetailed(TblOrderTran? tblOrderTran, bool? IsSweetenerAmtInclude, int? GstTypeId = null)
        {
            // Initialize variables
            EVCPriceViewModel evcPriceVM = new EVCPriceViewModel();
            try
            {
                if (tblOrderTran != null && tblOrderTran.FinalPriceAfterQc > 0)
                {
                    evcPriceVM.OrderTransId = tblOrderTran.OrderTransId;
                    evcPriceVM.GstTypeId = GstTypeId;
                    //Retrieve TblConfiguration for "UseEVCPriceMater" setting
                    var tblConfiguration = _context.TblConfigurations
                        .FirstOrDefault(x => x.Name == "UseEVCPriceMater" && x.IsActive == true);

                    if (tblConfiguration != null && tblConfiguration.Value == "1")
                    {
                        #region Get Order Details
                        evcPriceVM.OrderPrice = Convert.ToInt32(tblOrderTran.FinalPriceAfterQc ?? 0);
                        if (tblOrderTran.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                        {
                            if (tblOrderTran.Exchange?.IsFinalExchangePriceWithoutSweetner != true)
                            {
                                evcPriceVM.SweetenerAmt = Convert.ToInt32(tblOrderTran.Sweetner ?? 0);
                                evcPriceVM.OrderPrice = evcPriceVM.OrderPrice - evcPriceVM.SweetenerAmt;
                            }
                            evcPriceVM.ProdTypeId = tblOrderTran?.Exchange?.ProductType?.Id;
                            evcPriceVM.ProdCatId = tblOrderTran?.Exchange?.ProductType?.ProductCat?.Id;
                            evcPriceVM.Buid = tblOrderTran?.Exchange?.BusinessPartner?.BusinessUnit?.BusinessUnitId;
                        }
                        else
                        {
                            evcPriceVM.OrderPrice = evcPriceVM.OrderPrice - evcPriceVM.SweetenerAmt;
                            evcPriceVM.ProdTypeId = tblOrderTran?.Abbredemption?.Abbregistration?.NewProductCategoryTypeId;
                            evcPriceVM.ProdCatId = tblOrderTran?.Abbredemption?.Abbregistration?.NewProductCategoryId;
                            evcPriceVM.Buid = tblOrderTran?.Abbredemption?.Abbregistration?.BusinessUnitId;
                        }

                        evcPriceVM.LGCCost = GetLGCCost(evcPriceVM.ProdTypeId, evcPriceVM.ProdCatId, evcPriceVM.Buid);
                        evcPriceVM.AmountWithLGCCost = evcPriceVM.OrderPrice + evcPriceVM.LGCCost;
                        #endregion

                        #region Sweetener Implementation as per EVC
                        if (IsSweetenerAmtInclude == true)
                        {
                            if (evcPriceVM.OrderPrice > 0)
                            {
                                evcPriceVM.UTCAmount = GetUTCCost(evcPriceVM.OrderPrice, evcPriceVM.Buid);
                            }
                            if (GstTypeId == Convert.ToInt32(LoVEnum.GSTExclusive))
                            {
                                evcPriceVM.GstAmount = GetGstAmount(evcPriceVM.UTCAmount, GstTypeId);
                                evcPriceVM.AmountWithLGCCost = evcPriceVM.AmountWithLGCCost + evcPriceVM.GstAmount;
                            }
                            else if(GstTypeId == Convert.ToInt32(LoVEnum.GSTInclusive))
                            {
                                evcPriceVM.GstAmount = GetGstAmount(evcPriceVM.UTCAmount, GstTypeId);
                                evcPriceVM.UTCAmount = evcPriceVM.UTCAmount - evcPriceVM.GstAmount;
                            }
                            else
                            {
                                evcPriceVM.GstAmount = GetGstAmount(evcPriceVM.UTCAmount, GstTypeId);
                            }
                            //Add Sweetener Amount
                            evcPriceVM.AmountWithLGCCost = evcPriceVM.AmountWithLGCCost + evcPriceVM.SweetenerAmt;
                            evcPriceVM.IsSweetenerAmtInclude = true;
                            evcPriceVM.BaseValue = evcPriceVM.UTCAmount;
                        }
                        else
                        {
                            if (evcPriceVM.AmountWithLGCCost > 0)
                            {
                                evcPriceVM.UTCAmount = GetUTCCost((int)evcPriceVM.AmountWithLGCCost, evcPriceVM.Buid);
                                evcPriceVM.BaseValue = evcPriceVM.UTCAmount + evcPriceVM.LGCCost;
                                evcPriceVM.GstAmount = GetGstAmount(evcPriceVM.BaseValue, Convert.ToInt32(LoVEnum.GSTInclusive));
                            }
                            evcPriceVM.BaseValue = evcPriceVM.BaseValue -  evcPriceVM.GstAmount;
                        }
                        #endregion

                        #region Calculate EVC Price
                        evcPriceVM.EVCAmount = Convert.ToInt32(evcPriceVM.AmountWithLGCCost + evcPriceVM.UTCAmount);
                        #endregion

                        #region GST Amount
                        if (evcPriceVM.GstAmount > 0)
                        {
                            evcPriceVM.IGSTAmount = evcPriceVM.GstAmount;
                            var gst = evcPriceVM.GstAmount / 2;
                            evcPriceVM.CGSTAmount = gst;
                            evcPriceVM.SGSTAmount = gst;
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately or log them
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return evcPriceVM;
        }
        #endregion

        #region Calculate the EVC Price with integer return type
        /// <summary>
        /// Methods for EVC Price Calculations Algo New Added by VK
        /// </summary>
        /// <param name="OrderTransId"></param>
        /// <param name="IsSweetenerAmtInclude"></param>
        /// <param name="GstTypeId"></param>
        /// <returns></returns>
        public int CalculateEVCPriceNew(int OrderTransId, bool? IsSweetenerAmtInclude, int? GstTypeId = null)
        {
            EVCPriceViewModel? eVCPriceViewModel = null;
            TblOrderTran? tblOrderTran = null;
            int EVCAmount = 0;
            try
            {
                // Retrieve TblOrderTran based on OrderTransId
                tblOrderTran = _orderTransRepository.GetSingleOrderWithExchangereference(OrderTransId);
                if (tblOrderTran != null && tblOrderTran.FinalPriceAfterQc > 0)
                {
                    eVCPriceViewModel = CalculateEVCPriceDetailed(tblOrderTran, IsSweetenerAmtInclude, GstTypeId);
                    if (eVCPriceViewModel != null)
                    {
                        EVCAmount = eVCPriceViewModel.EVCAmount;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CommonManager", "CalculateEVCPriceNew", ex.Message);
            }
            return EVCAmount;
        }
        #endregion

        #region Get Gst Amount by Base Price and Gst Type Id
        /// <summary>
        /// Get Gst Amount by Base Price and Gst Type Id
        /// </summary>
        /// <param name="baseAmt"></param>
        /// <param name="gstTypeId"></param>
        /// <returns></returns>
        public decimal GetGstAmount(decimal baseAmt, int? gstTypeId)
        {
            decimal gstAmt = 0;
            TblLoV? tblLoV = null;
            decimal finalBasePrice = 0;
            bool isGSTInclusive = false; // Get this value from BU table. For now its is kept as true;
            bool isGSTExclusive = false;
            try
            {
                #region Get Gst Type by Id
                tblLoV = _lovRepository.GetById(gstTypeId);
                if (tblLoV != null)
                {
                    if (tblLoV.LoVname == EnumHelper.DescriptionAttr(LoVEnum.GSTInclusive))
                    {
                        finalBasePrice = baseAmt / Convert.ToDecimal(GeneralConstant.GSTPercentage);
                        isGSTInclusive = true;
                    }
                    else if (tblLoV.LoVname == EnumHelper.DescriptionAttr(LoVEnum.GSTExclusive))
                    {
                        finalBasePrice = baseAmt;
                        isGSTExclusive = true;
                    }
                }
                #endregion

                #region GST Calculation
                if ((isGSTInclusive || isGSTExclusive) && finalBasePrice > 0)
                {
                    finalBasePrice = Math.Round((finalBasePrice), 2);
                    gstAmt = finalBasePrice * Convert.ToDecimal(GeneralConstant.TotalGST);
                    gstAmt = Math.Round((gstAmt), 2);
                }
                #endregion
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CommonManager", "GetGstAmount", ex);
            }
            return gstAmt;
        }
        #endregion

        #region Get List of EVC Having Clear Balance (EVC Price Change Algo) Added by VK
        /// <summary>
        /// Get List of EVC Having Clear Balance (EVC Price Change Algo) Added by VK
        /// </summary>
        /// <param name="orderTransId"></param>
        /// <param name="evcPartnerList"></param>
        /// <returns></returns>
        public List<TblEvcPartner> GetEVCPartnerListHavingClearBalance(int orderTransId, List<TblEvcPartner> evcPartnerList)
        {
            List<TblEvcPartner>? tblEvcPartnerList = new List<TblEvcPartner>();
            List<TblEvcPartner>? tblEvcPartnerList1 = new List<TblEvcPartner>();
            List<TblEvcPartner>? tblEvcPartnerList2 = new List<TblEvcPartner>();
            EVCPriceViewModel? eVCPrice = null;
            bool EnableRevisedAlgoForEVC = true;
            try
            {
                if (orderTransId > 0)
                {
                    #region Get Expected EVC Price
                    int EVCPriceWithoutSweetener = CalculateEVCPriceNew(orderTransId, false, Convert.ToInt32(LoVEnum.GSTInclusive));
                    #endregion

                    #region
                    if (EnableRevisedAlgoForEVC == true)
                    {
                        #region Get Expected EVC Price
                        int EVCPriceWithSweetener = CalculateEVCPriceNew(orderTransId, true, Convert.ToInt32(LoVEnum.GSTExclusive));
                        #endregion

                        #region Filter EVC Partner List according to Flag
                        if (evcPartnerList != null && evcPartnerList.Count > 0)
                        {
                            if (EVCPriceWithSweetener > 0)
                            {
                                tblEvcPartnerList1 = evcPartnerList.Where(x => x.Evcregistration != null && x.Evcregistration.IsSweetenerAmtInclude == true).ToList();
                                tblEvcPartnerList1 = _eVCPartnerRepository.GetEvcPartnerListHavingClearBalance(tblEvcPartnerList1, EVCPriceWithSweetener);
                                if (tblEvcPartnerList1 != null && tblEvcPartnerList1.Count > 0)
                                {
                                    tblEvcPartnerList.AddRange(tblEvcPartnerList1);
                                }
                            }
                            if (EVCPriceWithoutSweetener > 0)
                            {
                                tblEvcPartnerList2 = evcPartnerList.Where(x => x.Evcregistration != null && x.Evcregistration.IsSweetenerAmtInclude != true).ToList();
                                tblEvcPartnerList2 = _eVCPartnerRepository.GetEvcPartnerListHavingClearBalance(tblEvcPartnerList2, EVCPriceWithoutSweetener);
                                if (tblEvcPartnerList2 != null && tblEvcPartnerList2.Count > 0)
                                {
                                    tblEvcPartnerList.AddRange(tblEvcPartnerList2);
                                }
                            }
                            tblEvcPartnerList = tblEvcPartnerList.DistinctBy(x => x.EvcPartnerId).ToList();
                        }
                        #endregion
                    }
                    else
                    {
                        #region According to Old Synerio
                        if (evcPartnerList != null && evcPartnerList.Count > 0)
                        {
                            if (EVCPriceWithoutSweetener > 0)
                            {
                                tblEvcPartnerList1 = evcPartnerList.Where(x => x.Evcregistration != null).ToList();
                                tblEvcPartnerList1 = _eVCPartnerRepository.GetEvcPartnerListHavingClearBalance(tblEvcPartnerList1, EVCPriceWithoutSweetener);
                                if (tblEvcPartnerList1 != null && tblEvcPartnerList1.Count > 0)
                                {
                                    tblEvcPartnerList = tblEvcPartnerList1;
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CommonManager", "GetEVCPartnerListHavingClearBalance", ex);
            }
            return tblEvcPartnerList;
        }
        #endregion

        #region Combine the EVC Partners List for EVC Amount Algo Change Added by VK
        //public List<TblEvcPartner> GetEVCPartnerListHavingClearBalance(List<TblEvcPartner> evcPartnerList, int expectedPriceWithoutSweetener = 0, int expectedPriceWithSweetener = 0)
        //{
        //    List<TblEvcPartner>? tblEvcPartnerList = new List<TblEvcPartner>();
        //    List<TblEvcPartner>? tblEvcPartnerList1 = new List<TblEvcPartner>();
        //    List<TblEvcPartner>? tblEvcPartnerList2 = new List<TblEvcPartner>();
        //    int orderTransId = 0;
        //    try
        //    {
        //        #region Get Expected EVC Price
        //        int EVCPriceWithSweetener = CalculateEVCPriceNew(orderTransId, true, Convert.ToInt32(LoVEnum.GSTExclusive));
        //        int EVCPriceWithoutSweetener = CalculateEVCPriceNew(orderTransId, false, Convert.ToInt32(LoVEnum.GSTInclusive));
        //        #endregion
        //        #region Filter EVC Partner List according to Flag
        //        if (evcPartnerList != null && evcPartnerList.Count > 0)
        //        {
        //            if (expectedPriceWithoutSweetener > 0)
        //            {
        //                tblEvcPartnerList1 = evcPartnerList.Where(x => x.Evcregistration != null && x.Evcregistration.IsSweetenerAmtInclude != true).ToList();
        //                tblEvcPartnerList1 = _eVCPartnerRepository.GetEvcPartnerListHavingClearBalance(tblEvcPartnerList1, expectedPriceWithoutSweetener);
        //                if (tblEvcPartnerList1 != null && tblEvcPartnerList1.Count > 0)
        //                {
        //                    tblEvcPartnerList.AddRange(tblEvcPartnerList1);
        //                }
        //            }
        //            if (expectedPriceWithSweetener > 0)
        //            {
        //                tblEvcPartnerList2 = evcPartnerList.Where(x => x.Evcregistration != null && x.Evcregistration.IsSweetenerAmtInclude == true).ToList();
        //                tblEvcPartnerList2 = _eVCPartnerRepository.GetEvcPartnerListHavingClearBalance(tblEvcPartnerList2, expectedPriceWithSweetener);
        //                if (tblEvcPartnerList2 != null && tblEvcPartnerList2.Count > 0)
        //                {
        //                    tblEvcPartnerList.AddRange(tblEvcPartnerList2);
        //                }
        //            }
        //            tblEvcPartnerList = tblEvcPartnerList.DistinctBy(x => x.EvcPartnerId).ToList();
        //        }
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.WriteErrorToDB("CommonManager", "GetEVCPartnerListHavingClearBalance", ex);
        //    }
        //    return tblEvcPartnerList;
        //}
        #endregion

        #region Get LGC Cost by Product Type, Product Category Id and BUId
        /// <summary>
        /// Get LGC Cost by Product Type, Product Category Id and BUId
        /// </summary>
        /// <param name="prodTypeId"></param>
        /// <param name="prodCatId"></param>
        /// <param name="buid"></param>
        /// <returns></returns>
        public int GetLGCCost(int? prodTypeId, int? prodCatId, int? buid)
        {
            int lgcCost = 0;
            TblEvcPriceMaster? tblEvcPriceMaster = null;
            try
            {
                if (prodTypeId > 0 && prodCatId > 0 && buid > 0)
                {
                    tblEvcPriceMaster = _eVCPriceMasterRepository.GetEvcPriceMaster(prodTypeId, prodCatId, buid);
                    if (tblEvcPriceMaster != null)
                    {
                        lgcCost = Convert.ToInt32(tblEvcPriceMaster.Lgccost ?? 0);
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CommonManager", "GetLGCCost", ex);
            }
            return lgcCost;
        }
        #endregion

        #region Get UTC Cost by PriceRange and Buid
        /// <summary>
        /// Get UTC Cost by PriceRange and Buid
        /// </summary>
        /// <param name="priceRange"></param>
        /// <param name="buid"></param>
        /// <returns></returns>
        public decimal GetUTCCost(int priceRange, int? buid)
        {
            decimal utcAmount = 0;
            int utcPercentage = 0;
            TblEvcpriceRangeMaster? tblEvcpriceRangeMaster = null;
            try
            {
                if (priceRange > 0 && buid > 0)
                {
                    tblEvcpriceRangeMaster = _eVCPriceRangeMasterRepository.GetEvcPriceRangeMaster(priceRange, buid);
                    if (tblEvcpriceRangeMaster != null)
                    {
                        utcPercentage = Convert.ToInt32(tblEvcpriceRangeMaster.EvcApplicablePercentage ?? 0);
                    }
                    if (utcPercentage > 0)
                    {
                        utcAmount = (priceRange * utcPercentage) / 100;
                    }
                    utcAmount = Convert.ToDecimal(utcAmount);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CommonManager", "GetUTCCost", ex);
            }
            return utcAmount;
        }
        #endregion
        #endregion

        #region Save into ApicallTableViewModel
        public void SaveApiCallInfo(ApicallViewModel apicallTableViewModel)
        {
            TblApicall apicallTable = null;
            try
            {
                if (apicallTableViewModel != null)
                {
                    //apicallTable= _mapper.Map<ApicallViewModel, TblApicall>(apicallTableViewModel);
                    apicallTable=_mapper.Map<ApicallViewModel, TblApicall>(apicallTableViewModel);
                    _apiCallsRepository.SaveApicall(apicallTable);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ApicallTableViewModelDetails", "ApicallTable", ex);
            }
        }
        #endregion
    }
}
