using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Common.Enums;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.DealerDashBoard;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.BAL.Enum;
using Microsoft.EntityFrameworkCore;

namespace RDCELERP.BAL.MasterManager
{
    public class DealerDashBoardManager : IDealerManager
    {
        #region  Variable Declaration
        IBusinessPartnerRepository _bussinesPartnerRepository;
        IExchangeOrderRepository _exchangeOrderRepository;
        IBusinessUnitRepository _businessUnitRepository;
        IVoucherRepository _voucherRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        Digi2l_DevContext _context;
        IListofValueRepository _listofValueRepository;
        IOrderTransactionManager _orderTransactionManager;
        IExchangeABBStatusHistoryManager _exchangeABBStatusHistoryManager;
        #endregion
        public DealerDashBoardManager(IBusinessPartnerRepository bussinesPartnerRepository, IExchangeOrderRepository exchangeOrderRepository, ILogging logging, IBusinessUnitRepository businessUnitRepository, IVoucherRepository voucherRepository, Digi2l_DevContext context)
        {
            _bussinesPartnerRepository = bussinesPartnerRepository;
            _exchangeOrderRepository = exchangeOrderRepository;
            _logging = logging;
            _businessUnitRepository = businessUnitRepository;
            _voucherRepository = voucherRepository;
            _context = context;
        }

        public OrderCountViewModel GetOrderCount(string AssociateCode, string SelectedCompanyName, string RoleName, string UserComapny)
        {
            OrderCountViewModel OrderCount = new OrderCountViewModel();
            List<TblExchangeOrder> OrderList = null;
            List<TblExchangeOrder> VoucherRedeemedList = null;
            List<TblExchangeOrder> VoucherIssuedList = null;
            OrderCount.StateList = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            try
            {
                if (AssociateCode != null)
                {
                    string RoleDescription = EnumHelper.DescriptionAttr(RoleEnum.SuperAdmin);
                    string comapnyDescription = EnumHelper.DescriptionAttr(CompanyNameenum.UTC);
                    OrderCount.InternalCompanyName = comapnyDescription;
                    OrderCount.RoleForSuperAdmin = RoleDescription;
                    if (AssociateCode != null && RoleDescription != RoleName && SelectedCompanyName != null && UserComapny != comapnyDescription)
                    {
                        OrderList = _context.TblExchangeOrders.Include(x => x.BusinessPartner).Where(x => x.BusinessPartner.AssociateCode == AssociateCode && x.CompanyName == SelectedCompanyName && x.IsActive == true).ToList();
                    }
                    if (SelectedCompanyName != null && RoleName == RoleDescription)
                    {
                        OrderList = _context.TblExchangeOrders.Where(x => x.CompanyName == SelectedCompanyName && x.IsActive == true).ToList();
                    }
                    if (SelectedCompanyName == null && RoleName == RoleDescription)
                    {
                        OrderList = _context.TblExchangeOrders.Where(x => x.IsActive == true).ToList();
                    }
                    if (UserComapny == comapnyDescription && SelectedCompanyName == null)
                    {
                        OrderList = _context.TblExchangeOrders.Where(x => x.IsActive == true).ToList();
                    }
                    if (UserComapny == comapnyDescription && SelectedCompanyName != null)
                    {
                        OrderList = _context.TblExchangeOrders.Where(x => x.IsActive == true && x.CompanyName == SelectedCompanyName).ToList();
                    }
                    //OrderList = _exchangeOrderRepository.GetOrderCount(AssociateCode, CompanyName);
                    if (OrderList.Count > 0)
                    {
                        OrderCount.OderCount = OrderList.Count;
                    }
                    else
                    {
                        OrderCount.OderCount = 0;
                    }
                    if (AssociateCode != null && RoleDescription != RoleName && SelectedCompanyName != null && UserComapny != comapnyDescription)
                    {
                        VoucherRedeemedList = _context.TblExchangeOrders.Include(x => x.BusinessPartner).Where(x => x.BusinessPartner.AssociateCode == AssociateCode && x.IsVoucherused == true && x.CompanyName == SelectedCompanyName && x.IsActive == true).ToList();
                    }
                    if (SelectedCompanyName != null && RoleName == RoleDescription)
                    {
                        VoucherRedeemedList = _context.TblExchangeOrders.Where(x => x.IsActive == true && x.IsVoucherused == true && x.CompanyName == SelectedCompanyName).ToList();
                    }

                    if (SelectedCompanyName == null && RoleName == RoleDescription)
                    {
                        VoucherRedeemedList = _context.TblExchangeOrders.Where(x => x.IsActive == true && x.IsVoucherused == true).ToList();
                    }
                    if (SelectedCompanyName == null && UserComapny == comapnyDescription)
                    {
                        VoucherRedeemedList = _context.TblExchangeOrders.Where(x => x.IsActive == true && x.IsVoucherused == true).ToList();
                    }
                    if (SelectedCompanyName != null && UserComapny == comapnyDescription)
                    {
                        VoucherRedeemedList = _context.TblExchangeOrders.Where(x => x.IsActive == true && x.IsVoucherused == true).ToList();
                    }
                    if (VoucherRedeemedList.Count > 0)
                    {
                        OrderCount.VoucherRedeemed = VoucherRedeemedList.Count;
                    }
                    else
                    {
                        OrderCount.VoucherRedeemed = 0;
                    }
                    if (AssociateCode != null && RoleDescription != RoleName && SelectedCompanyName != null && UserComapny != comapnyDescription)
                    {
                        VoucherIssuedList = _context.TblExchangeOrders.Include(x => x.BusinessPartner).Where(x => x.BusinessPartner.AssociateCode == AssociateCode && x.VoucherCode != null && x.CompanyName == SelectedCompanyName && x.IsActive == true).ToList();
                    }
                    if (SelectedCompanyName != null && RoleName == RoleDescription)
                    {
                        VoucherIssuedList = _context.TblExchangeOrders.Where(x => x.VoucherCode != null && x.CompanyName == SelectedCompanyName && x.IsActive == true).ToList();
                    }
                    if (SelectedCompanyName == null && RoleName == RoleDescription)
                    {
                        VoucherIssuedList = _context.TblExchangeOrders.Where(x => x.VoucherCode != null && x.IsActive == true).ToList();
                    }
                    if (SelectedCompanyName == null && UserComapny == comapnyDescription)
                    {
                        VoucherIssuedList = _context.TblExchangeOrders.Where(x => x.VoucherCode != null && x.IsActive == true).ToList();
                    }
                    if (SelectedCompanyName != null && UserComapny == comapnyDescription)
                    {
                        VoucherIssuedList = _context.TblExchangeOrders.Where(x => x.VoucherCode != null && x.IsActive == true && x.CompanyName == SelectedCompanyName).ToList();
                    }

                    if (VoucherIssuedList.Count > 0)
                    {
                        OrderCount.VoucherIssued = VoucherIssuedList.Count;
                    }
                    else
                    {
                        OrderCount.VoucherIssued = 0;
                    }

                    OrderCount.CompanyName = SelectedCompanyName;
                    OrderCount.UserCompanyName = UserComapny;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashBoardManager", "GetOrderCount", ex);
            }


            return OrderCount;
        }
        public CityListModel GetCityList(string state, int buid, string AssociateCode, string userCompany, string userRole)
        {
            CityListModel cityList = new CityListModel();
            List<TblBusinessPartner> businessspartnersList = null;
            cityList.CityList = new List<SelectListItem>();
            try
            {
                string RoleAssigned = EnumHelper.DescriptionAttr(RoleEnum.SuperAdmin);
                string UserAssignedCompany = EnumHelper.DescriptionAttr(CompanyNameenum.UTC);
                if (state != null && buid >= 0 && (userCompany == UserAssignedCompany || userRole == RoleAssigned))
                {
                    businessspartnersList = _bussinesPartnerRepository.GetCityByState(state, buid);
                    if (businessspartnersList.Count > 0)
                    {
                        List<string> city = businessspartnersList.Where(x => x.City != null && x.IsExchangeBp != null && x.IsExchangeBp == true).OrderBy(o => o.City).Select(x => x.City).Distinct().ToList();
                        cityList.CityList = city.Select(x => new SelectListItem
                        {
                            Text = x,
                            Value = x
                        }).ToList();
                    }
                    else
                    {
                        cityList.CityList = new List<SelectListItem> { new SelectListItem { Text = "No city available", Value = "0" } };

                    }
                }
                if (state != null && buid > 0 && (userCompany != UserAssignedCompany && userRole != RoleAssigned))
                {
                    businessspartnersList = _bussinesPartnerRepository.GetCityByStateandAssociate(state, buid, AssociateCode);
                    if (businessspartnersList.Count > 0)
                    {
                        List<string> city = businessspartnersList.Where(x => x.City != null && x.IsExchangeBp != null && x.IsExchangeBp == true).OrderBy(o => o.City).Select(x => x.City).Distinct().ToList();
                        cityList.CityList = city.Select(x => new SelectListItem
                        {
                            Text = x,
                            Value = x
                        }).ToList();
                    }
                    else
                    {
                        cityList.CityList = new List<SelectListItem> { new SelectListItem { Text = "No city available", Value = "0" } };

                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashBoardManager", "GetCityList", ex);
            }
            return cityList;
        }

        public StoreListModel GetStoreList(int buid, string AssociateCode, string City, string UserComapny, string userRole)
        {
            StoreListModel storeMOdel = new StoreListModel();
            List<TblBusinessPartner> BusineessPartnerList = null;
            storeMOdel.StoreList = new List<SelectListItem>();
            try
            {
                string comapnyDescription = EnumHelper.DescriptionAttr(CompanyNameenum.UTC);
                string RoleAssignedd = EnumHelper.DescriptionAttr(RoleEnum.SuperAdmin);

                if (buid >0 && AssociateCode != null && City != null && UserComapny != comapnyDescription && userRole != RoleAssignedd)
                {
                    BusineessPartnerList = _bussinesPartnerRepository.GetStoreList(City, AssociateCode, buid);
                    if (BusineessPartnerList.Count > 0)
                    {
                        storeMOdel.StoreList = BusineessPartnerList.Select(x => new SelectListItem
                        {
                            Text = x.Name + ", " + x.AddressLine1,
                            Value = x.BusinessPartnerId.ToString()
                        }).ToList();
                    }
                    else
                    {
                        storeMOdel.StoreList = new List<SelectListItem> { new SelectListItem { Text = "No store available", Value = "0" } };
                    }
                }

                else if (UserComapny == null || UserComapny == comapnyDescription || userRole == RoleAssignedd)
                {
                    BusineessPartnerList = _bussinesPartnerRepository.GetStoreListForInternalUser(City, buid);
                    if (BusineessPartnerList.Count > 0)
                    {
                        storeMOdel.StoreList = BusineessPartnerList.Select(x => new SelectListItem
                        {
                            Text = x.Name + ", " + x.AddressLine1,
                            Value = x.BusinessPartnerId.ToString()
                        }).ToList();
                    }
                    else
                    {
                        storeMOdel.StoreList = new List<SelectListItem> { new SelectListItem { Text = "No store available", Value = "0" } };
                    }
                }


            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashBoardManager", "GetStoreList", ex);
            }
            return storeMOdel;
        }

        public DealerDashboardViewModel GetOrderList(int BusinesspartnerId, string startdate, string enddate)
        {
            throw new NotImplementedException();
        }

        public List<DealerDashboardViewModel> GetDashboardList(ExchangeOrderDataContract exchanegDC, int skip, int pageSize)
        {
            List<DealerDashboardViewModel> dataList = new List<DealerDashboardViewModel>();
            List<TblExchangeOrder> ExchangeList = null;
            TblVoucherVerfication voucherVerification = null;
            int bpid = 0;
            try
            {

                string RoleDescription = EnumHelper.DescriptionAttr(RoleEnum.SuperAdmin);
                string comapnyDescription = EnumHelper.DescriptionAttr(CompanyNameenum.UTC);
                if (exchanegDC.BusinessPartnerId > 0 && exchanegDC.startdate != null && exchanegDC.enddate != null && exchanegDC.CompanyName != null)
                {

                    bpid = Convert.ToInt32(exchanegDC.BusinessPartnerId);
                    ExchangeList = _exchangeOrderRepository.GetOrderDetailsForDashBoard(exchanegDC.StartRangedate, exchanegDC.EndRangeDate, bpid, exchanegDC.CompanyName);


                }

                else if (exchanegDC.enddate == null && exchanegDC.startdate == null && exchanegDC.AssociateCode != null && exchanegDC.userCompany != comapnyDescription && exchanegDC.userRole != RoleDescription)
                {
                    ExchangeList = _exchangeOrderRepository.GetOrderDataforsingeldealer(exchanegDC.AssociateCode, exchanegDC.CompanyName);


                }

                else if (exchanegDC.BusinessPartnerId > 0 && exchanegDC.userCompany == comapnyDescription && exchanegDC.startdate != null && exchanegDC.enddate != null)
                {
                    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                        .ThenInclude(x => x.ProductCat)
                        .Include(x => x.BusinessPartner)
                        .Include(x => x.CustomerDetails)
                        .Include(x => x.TblVoucherVerfications).Where(x => x.BusinessPartnerId == exchanegDC.BusinessPartnerId && x.CreatedDate >= exchanegDC.StartRangedate && x.CreatedDate <= exchanegDC.EndRangeDate && x.IsActive == true).OrderByDescending(x => x.Id).ToList();

                }


                else if (exchanegDC.BusinessPartnerId == null && exchanegDC.userCompany == comapnyDescription && exchanegDC.CompanyName != null)
                {
                    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                       .ThenInclude(x => x.ProductCat)
                       .Include(x => x.BusinessPartner)
                       .Include(x => x.CustomerDetails)
                       .Include(x => x.TblVoucherVerfications).Where(x => x.CompanyName != null && x.CompanyName == exchanegDC.CompanyName && x.IsActive == true).OrderByDescending(x => x.Id).ToList();

                }

                else if (exchanegDC.BusinessPartnerId == null && exchanegDC.userCompany == comapnyDescription && exchanegDC.CompanyName == null)
                {
                    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                       .ThenInclude(x => x.ProductCat)
                       .Include(x => x.BusinessPartner)
                       .Include(x => x.CustomerDetails)
                       .Include(x => x.TblVoucherVerfications).Where(x => x.IsActive == true).OrderByDescending(x => x.Id).ToList();

                }

                else if (exchanegDC.BusinessPartnerId > 0 && exchanegDC.userRole == RoleDescription && exchanegDC.startdate != null && exchanegDC.enddate != null)
                {
                    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                       .ThenInclude(x => x.ProductCat)
                       .Include(x => x.BusinessPartner)
                       .Include(x => x.CustomerDetails)
                       .Include(x => x.TblVoucherVerfications).Where(x => x.BusinessPartnerId == exchanegDC.BusinessPartnerId && x.CreatedDate >= exchanegDC.StartRangedate && x.CreatedDate <= exchanegDC.EndRangeDate && x.IsActive == true).OrderByDescending(x => x.Id).ToList();

                }

                else if (exchanegDC.BusinessPartnerId == null && exchanegDC.userRole == RoleDescription && exchanegDC.BusinessUnitId > 0)
                {
                    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                      .ThenInclude(x => x.ProductCat)
                      .Include(x => x.BusinessPartner)
                      .Include(x => x.CustomerDetails)
                      .Include(x => x.TblVoucherVerfications).Where(x => x.IsActive == true && x.CompanyName != null && x.BusinessPartner.BusinessUnitId == exchanegDC.BusinessUnitId).OrderByDescending(x => x.Id).ToList();

                }

                else if (exchanegDC.BusinessPartnerId == null && exchanegDC.userRole == RoleDescription && exchanegDC.CompanyName == null)
                {
                    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                      .ThenInclude(x => x.ProductCat)
                      .Include(x => x.BusinessPartner)
                      .Include(x => x.CustomerDetails)
                      .Include(x => x.TblVoucherVerfications).Where(x => x.IsActive == true).OrderByDescending(x => x.Id).ToList();

                }
                else if (exchanegDC.BusinessUnitId > 0 && (exchanegDC.userRole == RoleDescription || exchanegDC.userCompany == comapnyDescription))
                {
                    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                      .ThenInclude(x => x.ProductCat)
                      .Include(x => x.BusinessPartner)
                      .Include(x => x.CustomerDetails)
                      .Include(x => x.TblVoucherVerfications).Where(x => x.IsActive == true && x.BusinessPartner.BusinessUnitId == exchanegDC.BusinessUnitId).OrderByDescending(x => x.Id).ToList();

                }
                if (ExchangeList != null)
                {
                    if (ExchangeList.Count > 0)
                    {
                        ExchangeList = ExchangeList.Skip(skip).Take(pageSize).ToList();
                        DealerDashboardViewModel dealerdata;
                        foreach (var item in ExchangeList)
                        {
                            dealerdata = new DealerDashboardViewModel();
                            dealerdata.Id = item.Id;
                            dealerdata.CompanyName = item.CompanyName;
                            dealerdata.RegdNo = item.RegdNo;
                            dealerdata.VoucherCode = item.VoucherCode;
                            dealerdata.ExchangePrice = item.ExchangePrice.ToString();
                            dealerdata.CustomerName = item.CustomerDetails.FirstName + " " + item.CustomerDetails.LastName;
                            if (item.ProductType != null)
                            {
                                if (item.ProductType.ProductCat != null)
                                {
                                    dealerdata.OldProductCategory = item.ProductType.ProductCat.Description;
                                }

                                dealerdata.OldProductType = item.ProductType.Description;
                            }

                            dealerdata.Sweetner = item.Sweetener.ToString();
                            DateTime OrderDate = Convert.ToDateTime(item.CreatedDate);
                            dealerdata.OrderDate = OrderDate.ToString("dd/MMM/yyyy");
                            if (item.IsDefferedSettlement == true)
                            {
                                dealerdata.TypeOfSettelment = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.deffred);
                            }
                            else
                            {
                                dealerdata.TypeOfSettelment = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.instant);
                            }
                            voucherVerification = _voucherRepository.GetVoucherDataByExchangeId(item.Id);
                            if (voucherVerification != null)
                            {
                                DateTime voucherdate = Convert.ToDateTime(voucherVerification.CreatedDate);
                                dealerdata.VoucherRedeemDate = voucherdate.ToString("dd/MMM/yyyy");

                                string status = EnumHelper.DescriptionAttr(VoucherStatusEnum.Redeemed);
                                if (voucherVerification.VoucherStatusId == Convert.ToInt32(VoucherStatusEnum.Redeemed))
                                {
                                    dealerdata.VoucherStatus = status;
                                    dealerdata.Paymentstatus = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.Paid);
                                }
                                if (voucherVerification.VoucherStatusId == Convert.ToInt32(VoucherStatusEnum.Captured))
                                {
                                    string capturedStatus = EnumHelper.DescriptionAttr(VoucherStatusEnum.Captured);
                                    dealerdata.VoucherStatus = capturedStatus;
                                    dealerdata.Paymentstatus = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.NotPaid);
                                }
                            }
                            dataList.Add(dealerdata);


                        }
                    }

                }
                if (dataList.Count > 0)
                {
                    dataList.OrderByDescending(x => x.Id);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashBoardManager", "GetDashboardList", ex);
            }
            return dataList;
        }

        public List<DealerDashboardViewModel> ExportDealerdata(string AssociateCode)
        {
            List<DealerDashboardViewModel> dataList = new List<DealerDashboardViewModel>();
            List<TblExchangeOrder> ExchangeList = null;
            try
            {
                if (AssociateCode != null)
                {
                    ExchangeList = _exchangeOrderRepository.GetOrderDataforsingeldealerToExport(AssociateCode);
                    if (ExchangeList.Count > 0)
                    {
                        DealerDashboardViewModel dealerdata;
                        foreach (var item in ExchangeList)
                        {
                            dealerdata = new DealerDashboardViewModel();
                            dealerdata.Id = item.Id;
                            dealerdata.CompanyName = item.CompanyName;
                            dealerdata.RegdNo = item.RegdNo;
                            dealerdata.VoucherCode = item.VoucherCode;
                            dealerdata.ExchangePrice = item.ExchangePrice.ToString();
                            dealerdata.Sweetner = item.Sweetener.ToString();
                            //customer date
                            dealerdata.CustomerName = item.CustomerDetails.FirstName + " " + item.CustomerDetails.LastName;
                            //Order date
                            if (item.CreatedDate != null)
                            {
                                DateTime orderDate = Convert.ToDateTime(item.CreatedDate);
                                dealerdata.OrderDate = orderDate.ToString("dd/MMM/yyyy");
                            }
                            if (item.ProductType != null)
                            {
                                dealerdata.OldProductType = item.ProductType.Description;

                                if (item.ProductType.ProductCat != null)
                                {
                                    dealerdata.OldProductCategory = item.ProductType.ProductCat.Description;
                                }
                            }
                            if (item.IsDefferedSettlement == true)
                            {
                                dealerdata.TypeOfSettelment = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.deffred);
                            }
                            else
                            {
                                dealerdata.TypeOfSettelment = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.instant);
                            }

                            //voucher redeem date
                            dealerdata.VoucherRedeemDate = item.CreatedDate.ToString();
                            string status;
                            if (item.VoucherStatusId == Convert.ToInt32(VoucherStatusEnum.Redeemed))
                            {
                                status = EnumHelper.DescriptionAttr(VoucherStatusEnum.Redeemed);
                            }
                            else
                            {
                                status = EnumHelper.DescriptionAttr(VoucherStatusEnum.Captured);
                            }
                            string lateststatus = EnumHelper.DescriptionAttr(VoucherStatusEnum.Redeemed);
                            if (status == lateststatus)
                            {
                                dealerdata.Paymentstatus = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.Paid);
                            }
                            else
                            {
                                dealerdata.Paymentstatus = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.NotPaid);
                            }

                            dataList.Add(dealerdata);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashBoardManager", "GetDashboardList", ex);
            }
            return dataList;
        }

        public StateListModel GetStateList(int buid, string AssociateCode, string UserCompany)
        {
            StateListModel stateList = new StateListModel();
            List<TblBusinessPartner> businessspartnersList = null;
            stateList.StateList = new List<SelectListItem>();
            string UserCompanyDescription = EnumHelper.DescriptionAttr(CompanyNameenum.UTC);
            try
            {
                if (buid >= 0)
                {
                    if (buid >= 0 && (string.IsNullOrEmpty(UserCompany) || UserCompany == UserCompanyDescription))
                    {
                        businessspartnersList = _bussinesPartnerRepository.GetStateList(buid);
                        if (businessspartnersList.Count > 0)
                        {
                            List<string> state = businessspartnersList.Where(x => x.State != null && x.IsExchangeBp != null && x.IsExchangeBp == true).OrderBy(o => o.State).Select(x => x.State).Distinct().ToList();
                            stateList.StateList = state.Select(x => new SelectListItem
                            {
                                Text = x,
                                Value = x
                            }).ToList();
                        }
                        else
                        {
                            stateList.StateList = new List<SelectListItem> { new SelectListItem { Text = "No state available", Value = "0" } };

                        }
                    }
                    else if (!string.IsNullOrEmpty(AssociateCode) && !string.IsNullOrEmpty(UserCompany) && buid > 0)
                    {
                        businessspartnersList = _bussinesPartnerRepository.GetStateListDelaer(buid, UserCompany, AssociateCode);
                        if (businessspartnersList.Count > 0)
                        {
                            List<string> state = businessspartnersList.Where(x => x.State != null && x.IsExchangeBp != null && x.IsExchangeBp == true).OrderBy(o => o.State).Select(x => x.State).Distinct().ToList();
                            stateList.StateList = state.Select(x => new SelectListItem
                            {
                                Text = x,
                                Value = x
                            }).ToList();
                        }
                        else
                        {
                            stateList.StateList = new List<SelectListItem> { new SelectListItem { Text = "No state available", Value = "0" } };

                        }
                    }
                    else
                    {
                        stateList.StateList = new List<SelectListItem> { new SelectListItem { Text = "No state available", Value = "0" } };

                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashBoardManager", "GetCityList", ex);
            }
            return stateList;
        }

        public CompanyList GetCompanyList()
        {
            CompanyList companyModel = new CompanyList();
            List<TblBusinessUnit> sponsorlist = null;
            companyModel.BusinessUnitList = new List<SelectListItem>();
            try
            {

                sponsorlist = _businessUnitRepository.GetSponsorList();
                if (sponsorlist.Count > 0)
                {
                    companyModel.BusinessUnitList = sponsorlist.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.BusinessUnitId.ToString()
                    }).ToList();
                }
                else
                {
                    companyModel.BusinessUnitList = new List<SelectListItem> { new SelectListItem { Text = "No Sponsor available", Value = "0" } };
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashBoardManager", "GetStoreList", ex);
            }
            return companyModel;
        }

        #region Get Counts For Company
        public OrderCountViewModel GetOrderCountsForBU(int BusinessUnitId)
        {
            OrderCountViewModel ordercount = new OrderCountViewModel();
            List<TblExchangeOrder> ExchangeList = null;
            List<TblExchangeOrder> VoucherIssuedCount = null;
            List<TblExchangeOrder> VoucherCapturedCount = null;
            List<TblExchangeOrder> CompletedOrders = null;
            List<TblExchangeOrder> CanceledOrders = null;

            try
            {
                ExchangeList = _exchangeOrderRepository.GetOrderCountBU(BusinessUnitId);

                if (ExchangeList.Count > 0)
                {
                   
                    ordercount.OderCount = ExchangeList.Count;
                }
                else
                {
                    ordercount.OderCount = 0;
                }

                VoucherIssuedCount = _exchangeOrderRepository.GetVoucherIssuedCountBU(BusinessUnitId);
                if (VoucherIssuedCount.Count > 0)
                {
                    ordercount.VoucherIssued = VoucherIssuedCount.Count;
                }
                else
                {
                    ordercount.VoucherIssued = 0;
                }
                VoucherCapturedCount = _exchangeOrderRepository.GetVoucherredeemedCountBU(BusinessUnitId);
                if (VoucherCapturedCount.Count > 0)
                {
                    ordercount.VoucherRedeemed = VoucherCapturedCount.Count;
                }
                else
                {
                    ordercount.VoucherRedeemed = 0;
                }

                CompletedOrders = _exchangeOrderRepository.GetCompletedOrderBU(BusinessUnitId);
                if (CompletedOrders.Count > 0)
                {
                    ordercount.CompletedOrders = CompletedOrders.Count;
                }
                else
                {
                    ordercount.CompletedOrders = 0;
                }
                CanceledOrders = _exchangeOrderRepository.GetCancelledOrderBu(BusinessUnitId);
                if (CanceledOrders.Count > 0)
                {
                    ordercount.CancelledOrders = CanceledOrders.Count;
                }
                else
                {
                    ordercount.CancelledOrders = 0;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashBoardManager", "GetOrderCountsForBU", ex);
            }
            return ordercount;
        }
        #endregion
    }
}
