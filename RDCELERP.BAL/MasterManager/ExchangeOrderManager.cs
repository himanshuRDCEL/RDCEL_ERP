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
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Common.Constant;
using RDCELERP.DAL.Helper;
using RDCELERP.Model.QCComment;
using RDCELERP.Model.Product;
using RDCELERP.Model;
using RDCELERP.Model.BusinessPartner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using static RDCELERP.Model.ExchangeOrder.ExchangeOrderViewModel;
using static RDCELERP.Model.Whatsapp.WhatsappSelfqcViewModel;
using RestSharp;
using Newtonsoft.Json;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.Questioners;
using System.Net;
using RDCELERP.BAL.Helper;
using RDCELERP.Model.Master;
using static RDCELERP.Model.Whatsapp.WhatsappOrderConfirmationViewModel;
using RDCELERP.Model.AbbRegistration;
using RDCELERP.Model.TicketGenrateModel;
using RDCELERP.Model.BusinessUnit;
using static RDCELERP.Common.Helper.MessageHelper;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.ResponseModel;
using RDCELERP.Model.ExchangeBulkLiquidation;
using WhatasappResponse = RDCELERP.Model.Whatsapp.WhatsappSelfqcViewModel.WhatasappResponse;
using UserDetails = RDCELERP.Model.Whatsapp.WhatsappSelfqcViewModel.UserDetails;
using System.Drawing;
using RDCELERP.Model.DealerDashBoard;
using RDCELERP.Model.UniversalPriceMaster;
using RDCELERP.Model.Whatsapp;
using GoogleMaps.LocationServices;
using Mailjet.Client.TransactionalEmails.Response;
using Login = RDCELERP.DAL.Entities.Login;
using RDCELERP.Model.MobileApplicationModel.LGC;
using Microsoft.Owin.Security.Notifications;

namespace RDCELERP.BAL.MasterManager
{
    public class ExchangeOrderManager : IExchangeOrderManager
    {
        #region  Variable Declaration
        Digi2l_DevContext _context;
        IExchangeOrderRepository _ExchangeOrderRepository;
        IUserRoleRepository _userRoleRepository;
        IExchangeOrderStatusRepository _ExchangeOrderStatusRepository;
        IBusinessPartnerRepository _businessPartnerRepository;
        IBusinessUnitRepository _businessUnitRepository;
        ILoginRepository _loginRepository;
        IPriceMasterRepository _priceMasterRepository;
        IBrandRepository _brandRepository;
        IOrderQCRepository _orderQCRepository;
        IOrderTransRepository _orderTransRepository;
        IEVCPriceMasterRepository _eVCPriceMAster;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IOptions<ApplicationSettings> _config;
        INotificationManager _notificationManager;
        IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _WhatsAppMessageRepository;
        IProductTypeRepository _ProductTypeRepository;
        IProductCategoryRepository _ProductCategoryRepository;
        ICustomerDetailsRepository _customerDetailsRepository;
        IQCCommentManager _qCCommentManager;
        ICommonManager _commonManager;
        IExchangeABBStatusHistoryRepository _exchangeABBStatusHistoryRepository;
        IOrderTransactionManager _orderTransactionManager;
        IModelNumberRepository _modelNumberRepository;
        IUserRepository _userRepository;
        IAreaLocalityRepository _areaLocalityRepository;
        IMailManager _mailManager;
        IProductConditionLabelRepository _productConditionLabelRepository;
        ILogisticsRepository _logisticsRepository;
        IWalletTransactionRepository _walletTransactionRepository;
        IPriceMasterMappingRepository _priceMasterMappingRepository;
        IUniversalPriceMasterRepository _universalPriceMasterRepository;
        #endregion

        #region Constructor
        public ExchangeOrderManager(IOrderTransactionManager orderTransactionManager, IUniversalPriceMasterRepository universalPriceMasterRepository, IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository, ICommonManager commonManager, IQCCommentManager qCCommentManager, ICustomerDetailsRepository customerDetailsRepository, Digi2l_DevContext context, INotificationManager notificationManager, IOptions<ApplicationSettings> config, IEVCPriceMasterRepository eVCPriceMAster, IOrderTransRepository orderTransRepository, IOrderQCRepository orderQCRepository, IBusinessPartnerRepository businessPartnerRepository, IBrandRepository brandRepository, IPriceMasterRepository priceMasterRepository, ILoginRepository loginRepository, IBusinessUnitRepository businessUnitRepository, IExchangeOrderStatusRepository ExchangeOrderStatusRepository, IExchangeOrderRepository ExchangeOrderRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging, IWhatsappNotificationManager whatsappNotificationManager, IWhatsAppMessageRepository whatsAppMessageRepository, IProductTypeRepository productTypeRepository, IProductCategoryRepository productCategoryRepository, IModelNumberRepository modelNumberRepository, IOrderTransRepository orderTransactionRepository, IAreaLocalityRepository areaLocalityRepository, IUserRepository userRepository, IMailManager mailManager, ILogisticsRepository logisticsRepository, IProductConditionLabelRepository productConditionLabelRepository, IWalletTransactionRepository walletTransactionRepository, IPriceMasterMappingRepository priceMasterMappingRepository)
        {
            _orderTransactionManager = orderTransactionManager;
            _qCCommentManager = qCCommentManager;
            _customerDetailsRepository = customerDetailsRepository;
            _context = context;
            _eVCPriceMAster = eVCPriceMAster;
            _brandRepository = brandRepository;
            _priceMasterRepository = priceMasterRepository;
            _loginRepository = loginRepository;
            _businessUnitRepository = businessUnitRepository;
            _businessPartnerRepository = businessPartnerRepository;
            _ExchangeOrderRepository = ExchangeOrderRepository;
            _userRoleRepository = userRoleRepository;
            _orderQCRepository = orderQCRepository;
            _mapper = mapper;
            _logging = logging;
            _ExchangeOrderStatusRepository = ExchangeOrderStatusRepository;
            _config = config;
            _notificationManager = notificationManager;
            _whatsappNotificationManager = whatsappNotificationManager;
            _WhatsAppMessageRepository = whatsAppMessageRepository;
            _ProductTypeRepository = productTypeRepository;
            _ProductCategoryRepository = productCategoryRepository;
            _commonManager = commonManager;
            _exchangeABBStatusHistoryRepository = exchangeABBStatusHistoryRepository;
            _modelNumberRepository = modelNumberRepository;
            _orderTransRepository = orderTransRepository;
            _userRepository = userRepository;
            _areaLocalityRepository = areaLocalityRepository;
            _mailManager = mailManager;
            _productConditionLabelRepository = productConditionLabelRepository;
            _logisticsRepository = logisticsRepository;
            _walletTransactionRepository = walletTransactionRepository;
            _priceMasterMappingRepository = priceMasterMappingRepository;
            _universalPriceMasterRepository = universalPriceMasterRepository;
        }

        public static string GetEnumDescription(object orderStatus)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Method to manage (Add/Edit) ExchangeOrder 
        /// <summary>
        /// Method to manage (Add/Edit) ExchangeOrder 
        /// </summary>
        /// <param name="ExchangeOrderVM">ExchangeOrderVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageExchangeOrder(ExchangeOrderViewModel ExchangeOrderVM, int userId)
        {
            TblExchangeOrder TblExchangeOrder = new TblExchangeOrder();
            TblOrderTran tblOrderTran = new TblOrderTran();
            TblCustomerDetail tblCustomerDetail = new TblCustomerDetail();
            TblLogistic? tblLogistic = null;
            TblOrderQc? tblOrderQc = null;
            TblWalletTransaction? tblWalletTransaction = null;
            TblBrand? tblBrand = null;
            try
            {
                if (ExchangeOrderVM != null)
                {
                    TblExchangeOrder = _ExchangeOrderRepository.GetExchOrderByRegdNo(ExchangeOrderVM.RegdNo);
                    if (TblExchangeOrder.Id > 0)
                    {
                        tblCustomerDetail= _customerDetailsRepository.GetCustDetails(TblExchangeOrder.CustomerDetailsId);
                        if(tblCustomerDetail != null)
                        {
                            if(ExchangeOrderVM.CustomerDetailViewModel.CustomerName != null)
                            {
                                int countSpace = CountSpaces(ExchangeOrderVM.CustomerDetailViewModel.CustomerName);
                                if(countSpace == 1)
                                {
                                    string[] nameParts = ExchangeOrderVM.CustomerDetailViewModel.CustomerName.Split(' ');
                                    if (nameParts.Length >= 2)
                                    {
                                        string firstName = nameParts[0];
                                        string lastName = nameParts[nameParts.Length - 1];

                                        tblCustomerDetail.FirstName = firstName;
                                        tblCustomerDetail.LastName = lastName;
                                    }
                                }
                                else if (countSpace == 0)
                                {
                                    tblCustomerDetail.FirstName = ExchangeOrderVM.CustomerDetailViewModel.CustomerName;
                                    tblCustomerDetail.LastName = string.Empty;
                                }
                            }
                            else
                            {
                                tblCustomerDetail.FirstName = "";
                                tblCustomerDetail.LastName = "";
                            }
                            tblCustomerDetail.PhoneNumber = ExchangeOrderVM.CustomerDetailViewModel.PhoneNumber;
                            tblCustomerDetail.Address1 = ExchangeOrderVM.CustomerDetailViewModel.Address1;
                            tblCustomerDetail.Address2 = ExchangeOrderVM.CustomerDetailViewModel.Address2;
                            tblCustomerDetail.State = ExchangeOrderVM.CustomerDetailViewModel.State;
                            tblCustomerDetail.Email = ExchangeOrderVM.CustomerDetailViewModel.Email;
                            tblCustomerDetail.City = ExchangeOrderVM.CustomerDetailViewModel.City;
                            tblCustomerDetail.ZipCode = ExchangeOrderVM.CustomerDetailViewModel.ZipCode;
                            tblCustomerDetail.AreaLocalityId = ExchangeOrderVM.CustomerDetailViewModel.AreaLocalityId;
                            _customerDetailsRepository.Update(tblCustomerDetail);
                            _customerDetailsRepository.SaveChanges();
                        }
                        //Code to update the object
                        if(ExchangeOrderVM.ProductConditionId > 0)
                        {
                            int qualityid = Convert.ToInt32(ExchangeOrderVM.ProductConditionId);
                            TblProductConditionLabel tblProductConditionLabel = _productConditionLabelRepository.GetOrderSequenceNo(qualityid);
                            if (tblProductConditionLabel != null)
                            {
                                switch (tblProductConditionLabel.OrderSequence)
                                {
                                    case 1:
                                        ExchangeOrderVM.ProductCondition = "Excellent";
                                        break;
                                    case 2:
                                        ExchangeOrderVM.ProductCondition = "Good";
                                        break;
                                    case 3:
                                        ExchangeOrderVM.ProductCondition = "Average";
                                        break;
                                    case 4:
                                        ExchangeOrderVM.ProductCondition = "Not Working";
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        if(ExchangeOrderVM.BrandName != null)
                        {
                            tblBrand = _context.TblBrands.FirstOrDefault(x => x.IsActive == true && x.Name == ExchangeOrderVM.BrandName);
                            if (tblBrand != null)
                            {
                                TblExchangeOrder.BrandId = tblBrand.Id;
                            }
                        }
                        TblExchangeOrder.ProductTypeId = ExchangeOrderVM.ProductTypeId;
                        TblExchangeOrder.ProductCondition = ExchangeOrderVM.ProductCondition;
                        TblExchangeOrder.ExchangePrice = ExchangeOrderVM.ExchangePrice;
                        if(new[] { Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval), Convert.ToInt32(OrderStatusEnum.QCByPass), Convert.ToInt32(OrderStatusEnum.AmountApprovedbyCustomerAfterQC), Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted), Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated) }.Contains(ExchangeOrderVM.StatusId.Value))                        
                        {
                            TblExchangeOrder.FinalExchangePrice = ExchangeOrderVM.ExchangePrice;
                        }
                        TblExchangeOrder.ModifiedBy = userId;
                        TblExchangeOrder.ModifiedDate = _currentDatetime;
                        _ExchangeOrderRepository.Update(TblExchangeOrder);
                        _ExchangeOrderRepository.SaveChanges();
                    }

                    tblOrderTran=_orderTransRepository.GetOrderTransByRegdno(ExchangeOrderVM.RegdNo);
                    if(tblOrderTran != null)
                    {
                        tblOrderTran.ExchangePrice = ExchangeOrderVM.ExchangePrice;
                        if (new[] { Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval), Convert.ToInt32(OrderStatusEnum.QCByPass), Convert.ToInt32(OrderStatusEnum.AmountApprovedbyCustomerAfterQC), Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted), Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated) }.Contains(ExchangeOrderVM.StatusId.Value))
                        {
                            tblOrderTran.FinalPriceAfterQc = ExchangeOrderVM.ExchangePrice;
                        }
                        tblOrderTran.ModifiedBy = userId;
                        tblOrderTran.ModifiedDate= _currentDatetime;
                        _orderTransRepository.Update(tblOrderTran);
                        _orderTransRepository.SaveChanges();
                        
                        if (new[] { Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval), Convert.ToInt32(OrderStatusEnum.QCByPass), Convert.ToInt32(OrderStatusEnum.AmountApprovedbyCustomerAfterQC), Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted), Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated) }.Contains(ExchangeOrderVM.StatusId.Value))
                        {
                            tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran.OrderTransId);
                            if (tblOrderQc != null)
                            {
                                tblOrderQc.PriceAfterQc = ExchangeOrderVM.ExchangePrice;
                                tblOrderQc.QualityAfterQc = ExchangeOrderVM.ProductCondition;
                                tblOrderQc.ModifiedBy = userId;
                                tblOrderQc.ModifiedDate = _currentDatetime;
                                _orderQCRepository.Update(tblOrderQc);
                                _orderQCRepository.SaveChanges();
                            }
                        }
                        if (new[] { Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted), Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated) }.Contains(ExchangeOrderVM.StatusId.Value))
                        {
                            int EVCAmount = _commonManager.CalculateEVCPrice(tblOrderTran.OrderTransId);
                            if( EVCAmount > 0)
                            {
                                tblWalletTransaction = _walletTransactionRepository.GetSingle(x => x.OrderTransId == tblOrderTran.OrderTransId);
                                if (tblWalletTransaction != null)
                                {
                                    tblWalletTransaction.OrderAmount = EVCAmount;
                                    tblWalletTransaction.ModifiedBy = userId;
                                    tblWalletTransaction.ModifiedDate = _currentDatetime;
                                    _walletTransactionRepository.Update(tblWalletTransaction);
                                    _walletTransactionRepository.SaveChanges();
                                }
                            }
                        }
                        if (new[] { Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated) }.Contains(ExchangeOrderVM.StatusId.Value))
                        {
                            tblLogistic = _logisticsRepository.GetExchangeDetailsByOrdertransId(tblOrderTran.OrderTransId);
                            if (tblLogistic != null)
                            {
                                tblLogistic.AmtPaybleThroughLgc = ExchangeOrderVM.ExchangePrice;
                                tblLogistic.ModifiedDate = _currentDatetime;
                                _logisticsRepository.Update(tblLogistic);
                                _logisticsRepository.SaveChanges();
                            }
                        }
                    }

                }
            }
            
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ExchangeOrderManager", "ManageExchangeOrder", ex);
            }

            return TblExchangeOrder.Id;
        }
        #endregion

        #region Method to Bulk Upload LOV (Add/Edit) ExchangeOrder 
        /// <summary>
        /// Method to Bulk Upload (Add/Edit) ExchangeOrder 
        /// </summary>
        /// <param name="ExchangeOrderVM">ExchangeOrderVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public ExchangeOrderViewModel ManageExchangeOrderBulk(ExchangeOrderViewModel ExchangeVM, int userId)
        {

            if (ExchangeVM != null && ExchangeVM.ExchangeVMList != null && ExchangeVM.ExchangeVMList.Count > 0)
            {
                var ValidatedExchangeList = ExchangeVM.ExchangeVMList.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
                ExchangeVM.ExchangeVMErrorList = ExchangeVM.ExchangeVMList.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();
                if (ValidatedExchangeList != null && ValidatedExchangeList.Count > 0)
                {
                    foreach (var item in ValidatedExchangeList)
                    {
                        try
                        {
                            if (item.Id > 0)
                            {

                                TblCustomerDetail TblCustomerDetails = new TblCustomerDetail();
                                //Code to update the object 
                                TblCustomerDetails.FirstName = item.CustomerFirstName;
                                TblCustomerDetails.LastName = item.CustomerLastName;
                                TblCustomerDetails.PhoneNumber = item.CustomerPhoneNumber;
                                TblCustomerDetails.City = item.CustomerCity;
                                TblCustomerDetails.State = item.CustomerState;
                                TblCustomerDetails.ZipCode = item.CustomerPinCode;
                                TblCustomerDetails.Address1 = item.CustomerAddress1;
                                TblCustomerDetails.Address2 = item.CustomerAddress2;
                                TblCustomerDetails.ModifiedDate = _currentDatetime;
                                TblCustomerDetails.ModifiedBy = userId;
                                _customerDetailsRepository.Update(TblCustomerDetails);
                                _customerDetailsRepository.SaveChanges();

                                TblExchangeOrder TblExchangeOrder = new TblExchangeOrder();
                                //Code to update the object 
                                TblExchangeOrder.CustomerDetailsId = TblCustomerDetails.Id;

                                var ProductType = _ProductTypeRepository.GetSingle(x => x.Description + x.Size == item.ProductType);
                                TblExchangeOrder.ProductTypeId = ProductType.Id;

                                var Brand = _brandRepository.GetSingle(x => x.Name == item.Brand);
                                TblExchangeOrder.BrandId = Brand.Id;
                                TblExchangeOrder.Bonus = "0";
                                TblExchangeOrder.LoginId = 1;
                                TblExchangeOrder.PurchasedProductCategory = item.PurchasedProductCategory;
                                if (item.SponsorOrderNumber != null)
                                {
                                    TblExchangeOrder.SponsorOrderNumber = item.SponsorOrderNumber.Trim() + TblExchangeOrder.RegdNo;
                                }
                                else
                                {
                                    TblExchangeOrder.SponsorOrderNumber = "ECH" + UniqueString.RandomNumberByLength(9);
                                }
                                TblExchangeOrder.IsDtoC = item.IsDtoC;
                                TblExchangeOrder.IsDefferedSettlement = item.IsDefferedSettlement;
                                TblExchangeOrder.ExchangePrice = item.ExchangePrice;
                                TblExchangeOrder.StoreCode = item.StoreCode;

                                var BusinessPartner = _businessPartnerRepository.GetSingle(x => x.StoreCode == item.StoreCode);
                                if (BusinessPartner != null)
                                {
                                    TblExchangeOrder.BusinessPartnerId = BusinessPartner.BusinessPartnerId;
                                }


                                TblExchangeOrder.ProductNumber = item.ProductNumber;
                                TblExchangeOrder.InvoiceNumber = item.InvoiceNumber;

                                var ModelNumber = _modelNumberRepository.GetSingle(x => x.ModelName == item.ModelNumber);
                                if (ModelNumber != null)
                                {
                                    TblExchangeOrder.ModelNumberId = ModelNumber.ModelNumberId;
                                }
                                TblExchangeOrder.Sweetener = item.Sweetener;
                                TblExchangeOrder.SerialNumber = item.SerialNumber;
                                TblExchangeOrder.FinalExchangePrice = item.FinalExchangePrice;
                                TblExchangeOrder.ProductCondition = item.ProductCondition;

                                var ProductCategory = _ProductCategoryRepository.GetSingle(x => x.Description == item.NewProductcategory);
                                TblExchangeOrder.NewProductCategoryId = ProductCategory.Id;

                                var ProductType1 = _ProductTypeRepository.GetSingle(x => x.Description + x.Size == item.NewProductType);
                                TblExchangeOrder.NewProductTypeId = ProductType1.Id;

                                var Brand1 = _brandRepository.GetSingle(x => x.Name == item.NewBrand);
                                TblExchangeOrder.NewBrandId = Brand1.Id;

                                TblExchangeOrder.VoucherCode = item.VoucherCode;
                                TblExchangeOrder.IsVoucherused = item.IsVoucherused;

                                var BusinessUnit = _businessUnitRepository.GetSingle(x => x.Name == item.CompanyName);
                                TblExchangeOrder.EstimatedDeliveryDate = DateTime.Now.AddHours(Convert.ToInt32(BusinessUnit.ExpectedDeliveryHours)).ToString("dd-MM-yyyy");


                                TblExchangeOrder.ModifiedDate = _currentDatetime;
                                TblExchangeOrder.ModifiedBy = userId;
                                _ExchangeOrderRepository.Update(TblExchangeOrder);
                                _ExchangeOrderRepository.SaveChanges();

                                //Code for Order tran
                                TblOrderTran TblOrderTran = new TblOrderTran();
                                TblOrderTran.OrderType = Convert.ToInt32(OrderTypeEnum.Exchange); ;
                                TblOrderTran.ExchangeId = TblExchangeOrder.Id;
                                TblOrderTran.SponsorOrderNumber = TblExchangeOrder.SponsorOrderNumber;
                                TblOrderTran.RegdNo = TblExchangeOrder.RegdNo;
                                TblOrderTran.ExchangePrice = TblExchangeOrder.ExchangePrice;
                                TblOrderTran.Sweetner = TblExchangeOrder.Sweetener;

                                TblOrderTran.IsActive = true;
                                TblOrderTran.ModifiedDate = _currentDatetime;
                                TblOrderTran.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                                TblOrderTran.ModifiedBy = userId;
                                TblOrderTran.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                                _orderTransRepository.Update(TblOrderTran);
                                _orderTransRepository.SaveChanges();


                                //Code for Order history

                                TblExchangeAbbstatusHistory TblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                TblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(OrderTypeEnum.Exchange);
                                TblExchangeAbbstatusHistory.OrderTransId = TblOrderTran.OrderTransId;

                                TblExchangeAbbstatusHistory.SponsorOrderNumber = TblExchangeOrder.SponsorOrderNumber;
                                TblExchangeAbbstatusHistory.RegdNo = TblExchangeOrder.RegdNo;
                                TblExchangeAbbstatusHistory.CustId = TblCustomerDetails.Id;
                                TblExchangeAbbstatusHistory.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                                TblExchangeAbbstatusHistory.IsActive = true;
                                TblExchangeAbbstatusHistory.ModifiedDate = _currentDatetime;
                                TblExchangeAbbstatusHistory.ModifiedBy = userId;
                                _exchangeABBStatusHistoryRepository.Update(TblExchangeAbbstatusHistory);
                                _exchangeABBStatusHistoryRepository.SaveChanges();

                            }
                            else
                            {

                                TblCustomerDetail TblCustomerDetails = new TblCustomerDetail();
                                //Code to insert the object 
                                TblCustomerDetails.FirstName = item.CustomerFirstName;
                                TblCustomerDetails.LastName = item.CustomerLastName;
                                TblCustomerDetails.PhoneNumber = item.CustomerPhoneNumber;
                                TblCustomerDetails.City = item.CustomerCity;
                                TblCustomerDetails.State = item.CustomerState;
                                TblCustomerDetails.ZipCode = item.CustomerPinCode;
                                TblCustomerDetails.Address1 = item.CustomerAddress1;
                                TblCustomerDetails.Address2 = item.CustomerAddress2;
                                TblCustomerDetails.IsActive = true;
                                TblCustomerDetails.CreatedDate = _currentDatetime;
                                TblCustomerDetails.CreatedBy = userId;
                                _customerDetailsRepository.Create(TblCustomerDetails);
                                _customerDetailsRepository.SaveChanges();

                                TblExchangeOrder TblExchangeOrder = new TblExchangeOrder();
                                //Code to insert the object 
                                TblExchangeOrder.CompanyName = item.CompanyName;
                                TblExchangeOrder.CustomerDetailsId = TblCustomerDetails.Id;
                                TblExchangeOrder.RegdNo = "E" + UniqueString.RandomNumberByLength(7);

                                var ProductType = _ProductTypeRepository.GetSingle(x => x.Description + x.Size == item.ProductType);
                                TblExchangeOrder.ProductTypeId = ProductType.Id;

                                var Brand = _brandRepository.GetSingle(x => x.Name == item.Brand);
                                TblExchangeOrder.BrandId = Brand.Id;
                                TblExchangeOrder.Bonus = "0";
                                TblExchangeOrder.LoginId = 1;
                                TblExchangeOrder.PurchasedProductCategory = item.PurchasedProductCategory;
                                if (item.SponsorOrderNumber != null)
                                {
                                    TblExchangeOrder.SponsorOrderNumber = item.SponsorOrderNumber.Trim() + TblExchangeOrder.RegdNo;
                                }
                                else
                                {
                                    TblExchangeOrder.SponsorOrderNumber = "ECH" + UniqueString.RandomNumberByLength(9);
                                }


                                TblExchangeOrder.IsDtoC = item.IsDtoC;
                                TblExchangeOrder.IsDefferedSettlement = item.IsDefferedSettlement;
                                TblExchangeOrder.ExchangePrice = item.ExchangePrice;
                                TblExchangeOrder.StoreCode = item.StoreCode;

                                var BusinessPartner = _businessPartnerRepository.GetSingle(x => x.StoreCode == item.StoreCode);
                                if (BusinessPartner != null)
                                {
                                    TblExchangeOrder.BusinessPartnerId = BusinessPartner.BusinessPartnerId;
                                }


                                TblExchangeOrder.ProductNumber = item.ProductNumber;
                                TblExchangeOrder.InvoiceNumber = item.InvoiceNumber;

                                var ModelNumber = _modelNumberRepository.GetSingle(x => x.ModelName == item.ModelNumber);
                                if (ModelNumber != null)
                                {
                                    TblExchangeOrder.ModelNumberId = ModelNumber.ModelNumberId;
                                }
                                TblExchangeOrder.Sweetener = item.Sweetener;
                                TblExchangeOrder.SerialNumber = item.SerialNumber;
                                TblExchangeOrder.FinalExchangePrice = item.FinalExchangePrice;
                                TblExchangeOrder.ProductCondition = item.ProductCondition;

                                var ProductCategory = _ProductCategoryRepository.GetSingle(x => x.Description == item.NewProductcategory);
                                if (ProductCategory != null)
                                {
                                    TblExchangeOrder.NewProductCategoryId = ProductCategory.Id;
                                }


                                var ProductType1 = _ProductTypeRepository.GetSingle(x => x.Description + x.Size == item.NewProductType);
                                if (ProductType1 != null)
                                {
                                    TblExchangeOrder.NewProductTypeId = ProductType1.Id;
                                }


                                var Brand1 = _brandRepository.GetSingle(x => x.Name == item.NewBrand);
                                if (Brand1 != null)
                                {
                                    TblExchangeOrder.NewBrandId = Brand1.Id;
                                }


                                TblExchangeOrder.VoucherCode = item.VoucherCode;
                                TblExchangeOrder.IsVoucherused = item.IsVoucherused;
                                TblExchangeOrder.OrderStatus = "Order Created";
                                TblExchangeOrder.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);

                                var BusinessUnit = _businessUnitRepository.GetSingle(x => x.Name == item.CompanyName);
                                if (BusinessUnit != null)
                                {
                                    TblExchangeOrder.EstimatedDeliveryDate = DateTime.Now.AddHours(Convert.ToInt32(BusinessUnit.ExpectedDeliveryHours)).ToString("dd-MM-yyyy");
                                }

                                TblExchangeOrder.CreatedDate = _currentDatetime;
                                TblExchangeOrder.CreatedBy = userId;
                                TblExchangeOrder.ModifiedDate = _currentDatetime;
                                TblExchangeOrder.ModifiedBy = userId;
                                TblExchangeOrder.IsActive = true;
                                _ExchangeOrderRepository.Create(TblExchangeOrder);
                                _ExchangeOrderRepository.SaveChanges();


                                //Code for Order tran
                                TblOrderTran TblOrderTran = new TblOrderTran();
                                TblOrderTran.OrderType = Convert.ToInt32(OrderTypeEnum.Exchange);
                                TblOrderTran.ExchangeId = TblExchangeOrder.Id;
                                TblOrderTran.SponsorOrderNumber = TblExchangeOrder.SponsorOrderNumber;
                                TblOrderTran.RegdNo = TblExchangeOrder.RegdNo;
                                TblOrderTran.ExchangePrice = TblExchangeOrder.ExchangePrice;
                                TblOrderTran.Sweetner = TblExchangeOrder.Sweetener;

                                TblOrderTran.IsActive = true;
                                TblOrderTran.CreatedDate = _currentDatetime;
                                TblOrderTran.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                                TblOrderTran.CreatedBy = userId;
                                TblOrderTran.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                                _orderTransRepository.Create(TblOrderTran);
                                _orderTransRepository.SaveChanges();


                                //Code for Order history

                                TblExchangeAbbstatusHistory TblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                TblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(OrderTypeEnum.Exchange);
                                TblExchangeAbbstatusHistory.OrderTransId = TblOrderTran.OrderTransId;

                                TblExchangeAbbstatusHistory.SponsorOrderNumber = TblExchangeOrder.SponsorOrderNumber;
                                TblExchangeAbbstatusHistory.RegdNo = TblExchangeOrder.RegdNo;
                                TblExchangeAbbstatusHistory.CustId = TblCustomerDetails.Id;
                                TblExchangeAbbstatusHistory.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                                TblExchangeAbbstatusHistory.IsActive = true;
                                TblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                TblExchangeAbbstatusHistory.CreatedBy = userId;
                                _exchangeABBStatusHistoryRepository.Create(TblExchangeAbbstatusHistory);
                                _exchangeABBStatusHistoryRepository.SaveChanges();


                            }
                        }

                        catch (Exception ex)
                        {
                            item.Remarks += ex.Message + ", ";
                            ExchangeVM.ExchangeVMList.Add(item);
                        }
                    }
                }
            }

            return ExchangeVM;
        }
        #endregion

        #region Method to Bulk Upload for Bulk Liquidation (Add/Edit) ExchangeOrder 
        /// <summary>
        /// Method to Bulk Upload (Add/Edit) ExchangeOrder 
        /// </summary>
        /// <param name="ExchangeOrderVM">ExchangeOrderVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public ExchangeBulkLiquidatioModel ManageExchangeBulkUpload(ExchangeBulkLiquidatioModel ExchangeVM1, int userId)
        {
            int condition = 0;

            if (ExchangeVM1 != null && ExchangeVM1.ExchangeVMList1 != null && ExchangeVM1.ExchangeVMList1.Count > 0)
            {
                var ValidatedExchangeList1 = ExchangeVM1.ExchangeVMList1.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
                ExchangeVM1.ExchangeVMErrorList1 = ExchangeVM1.ExchangeVMList1.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();
                if (ValidatedExchangeList1 != null && ValidatedExchangeList1.Count > 0)
                {
                    TblCustomerDetail TblCustomerDetails = new TblCustomerDetail();
                    //Code to insert the object 
                    DAL.Entities.TblUser tblUser = _context.TblUsers.Where(x => x.UserId == userId && x.IsActive == true).FirstOrDefault();
                    if (tblUser != null)
                    {
                        tblUser.Email = SecurityHelper.DecryptString(tblUser.Email, _config.Value.SecurityKey);
                    }

                    if (tblUser.Email != null)
                    {
                        TblBusinessPartner BusinessPartner = _context.TblBusinessPartners.Where(x => x.Email == tblUser.Email && x.IsActive == true).FirstOrDefault();
                        if (BusinessPartner != null)
                        {
                            TblCustomerDetails.FirstName = tblUser.FirstName;
                            TblCustomerDetails.LastName = tblUser.LastName;
                            TblCustomerDetails.PhoneNumber = BusinessPartner.PhoneNumber;
                            TblCustomerDetails.Email = BusinessPartner.Email;
                            TblCustomerDetails.City = BusinessPartner.City;
                            TblCustomerDetails.State = BusinessPartner.State;
                            TblCustomerDetails.ZipCode = BusinessPartner.Pincode;
                            TblCustomerDetails.Address1 = BusinessPartner.AddressLine1;
                            TblCustomerDetails.Address2 = BusinessPartner.AddressLine2;
                            TblCustomerDetails.IsActive = true;
                            TblCustomerDetails.CreatedDate = _currentDatetime;
                            TblCustomerDetails.CreatedBy = userId;
                            _customerDetailsRepository.Create(TblCustomerDetails);
                            _customerDetailsRepository.SaveChanges();
                        }
                    }
                    foreach (var item in ValidatedExchangeList1)
                    {
                        try
                        {

                            TblBusinessPartner businessPartner = _context.TblBusinessPartners.Where(x => x.Email == tblUser.Email && x.IsActive == true).FirstOrDefault();
                            TblExchangeOrder TblExchangeOrder = new TblExchangeOrder();
                            //Code to insert the object 

                            TblExchangeOrder.CustomerDetailsId = TblCustomerDetails.Id;
                            TblExchangeOrder.RegdNo = "E" + UniqueString.RandomNumberByLength(7);

                            TblProductType ProductType = _context.TblProductTypes.Where(x => x.Description + x.Size == item.ProductType && x.IsActive == true).FirstOrDefault();
                            TblExchangeOrder.ProductTypeId = ProductType.Id;
                            TblProductCategory ProductCategory = _context.TblProductCategories.Where(x => x.Description == item.ProductCategory && x.IsActive == true).FirstOrDefault();
                            var Brand = _brandRepository.GetSingle(x => x.Name == item.Brand);
                            TblExchangeOrder.BrandId = Brand.Id;

                            //TblExchangeOrder.PurchasedProductCategory = item.PurchasedProductCategory;
                            if (item.SponsorOrderNumber != null)
                            {
                                TblExchangeOrder.SponsorOrderNumber = item.SponsorOrderNumber.Trim();
                            }
                            

                            TblExchangeOrder.IsDtoC = businessPartner.IsD2c;
                            TblExchangeOrder.IsDefferedSettlement = businessPartner.IsDefferedSettlement;
                            TblExchangeOrder.Bonus = Convert.ToString(0);
                            TblExchangeOrder.LoginId = 3;

                            TblExchangeOrder.StoreCode = businessPartner.StoreCode;
                            TblExchangeOrder.BusinessPartnerId = businessPartner.BusinessPartnerId;

                            TblExchangeOrder.ProductCondition = item.ProductCondition;


                            TblExchangeOrder.OrderStatus = "Order Created";
                            TblExchangeOrder.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);

                            var BusinessUnit = _context.TblBusinessUnits.Where(x => x.BusinessUnitId == businessPartner.BusinessUnitId && x.IsActive == true).FirstOrDefault();
                            if (BusinessUnit != null)
                            {
                                TblExchangeOrder.CompanyName = BusinessUnit.Name;
                                TblExchangeOrder.BusinessUnitId = BusinessUnit.BusinessUnitId;
                                TblExchangeOrder.Sweetener = Convert.ToDecimal(businessPartner.SweetenerBu ?? 0) + Convert.ToDecimal(businessPartner.SweetenerBp ?? 0) + Convert.ToDecimal(businessPartner.SweetenerDigi2l ?? 0);
                                TblExchangeOrder.SweetenerBu = Convert.ToDecimal(businessPartner.SweetenerBu ?? 0);
                                TblExchangeOrder.SweetenerBp = Convert.ToDecimal(businessPartner.SweetenerBp ?? 0);
                                TblExchangeOrder.SweetenerDigi2l = Convert.ToDecimal(businessPartner.SweetenerDigi2l ?? 0);

                                //DAL.Entities.Login login = _context.Logins.Where(x => x.SponsorId == BusinessUnit.BusinessUnitId).FirstOrDefault();

                                if (item.ProductCondition == "Working")
                                {
                                    condition = 2;
                                }
                                //if (item.ProductCondition == "Good")
                                //{
                                //    condition = 2;
                                //}
                                if (item.ProductCondition == "Heavily Used")
                                {
                                    condition = 3;
                                }
                                if (item.ProductCondition == "Not_Working" || item.ProductCondition == "Not Working")
                                {
                                    condition = 4;
                                }

                                TblProductConditionLabel tblProductConditionLabel = _productConditionLabelRepository.GetSingle(x => x.BusinessUnitId == BusinessUnit.BusinessUnitId && x.BusinessPartnerId == businessPartner.BusinessPartnerId && x.OrderSequence == condition && x.IsActive == true);

                                TblPriceMasterMapping tblPriceMasterMapping = new TblPriceMasterMapping();

                                tblPriceMasterMapping = _priceMasterMappingRepository.GetSingle(x => x.BusinessUnitId == BusinessUnit.BusinessUnitId && x.BusinessPartnerId == businessPartner.BusinessPartnerId && x.IsActive == true);

                                TblUniversalPriceMaster tblUniversalPriceMaster = new TblUniversalPriceMaster();
                                if (tblPriceMasterMapping != null)
                                {
                                    tblUniversalPriceMaster = _context.TblUniversalPriceMasters.Where(x => x.PriceMasterNameId == tblPriceMasterMapping.PriceMasterNameId && x.ProductTypeId == ProductType.Id && x.IsActive == true).FirstOrDefault();
                                }


                                if (tblUniversalPriceMaster != null)
                                {
                                    TblUniversalPriceMaster? UniversalPriceMasterPrime = _context.TblUniversalPriceMasters
                                     .Where(x => x.PriceMasterNameId == tblUniversalPriceMaster.PriceMasterNameId && (x.BrandName1 == item.Brand || x.BrandName2 == item.Brand || x.BrandName3 == item.Brand || x.BrandName4 == item.Brand)
                                      && x.IsActive == true)
                                            .FirstOrDefault();



                                    if (UniversalPriceMasterPrime != null)
                                    {
                                        TblUniversalPriceMaster UniversalPriceMasterPrime1 = new TblUniversalPriceMaster();

                                        UniversalPriceMasterPrime1 = _universalPriceMasterRepository.GetSingle(x =>
                                        x.PriceMasterNameId == tblUniversalPriceMaster?.PriceMasterNameId

                                        && x.ProductCategoryId == ProductCategory?.Id
                                        && x.ProductTypeId == ProductType?.Id &&

                                       (x.BrandName1 == item.Brand ||
                                        x.BrandName2 == item.Brand ||
                                        x.BrandName3 == item.Brand ||
                                        x.BrandName4 == item.Brand) &&
                                        x.IsActive == true);
                                        if (UniversalPriceMasterPrime1 != null)
                                        {
                                            switch (tblProductConditionLabel.PclabelName)
                                            {
                                                //case "Well Maintained":
                                                //    TblExchangeOrder.ExchangePrice = Convert.ToDecimal(UniversalPriceMasterPrime1.QuotePHigh ?? "0") + Convert.ToDecimal(TblExchangeOrder.Sweetener ?? 0);
                                                //    break;
                                                case "Working":
                                                    TblExchangeOrder.ExchangePrice = Convert.ToDecimal(UniversalPriceMasterPrime1.QuoteQHigh ?? "0") + Convert.ToDecimal(TblExchangeOrder.Sweetener ?? 0);
                                                    break;
                                                case "Heavily Used":
                                                    TblExchangeOrder.ExchangePrice = Convert.ToDecimal(UniversalPriceMasterPrime1.QuoteRHigh ?? "0") + Convert.ToDecimal(TblExchangeOrder.Sweetener ?? 0);
                                                    break;
                                                case "Non Working":
                                                    TblExchangeOrder.ExchangePrice = Convert.ToDecimal(UniversalPriceMasterPrime1.QuoteSHigh ?? "0") + Convert.ToDecimal(TblExchangeOrder.Sweetener ?? 0);
                                                    break;


                                            }

                                        }
                                    }
                                    else
                                    {
                                        TblUniversalPriceMaster? UniversalPriceMasterOthers = _context.TblUniversalPriceMasters.Where(x => x.PriceMasterNameId == tblUniversalPriceMaster.PriceMasterNameId
                                        && x.ProductCategoryId == ProductCategory.Id
                                        && x.ProductTypeId == ProductType.Id
                                        && x.OtherBrand == item.Brand
                                        && x.IsActive == true).FirstOrDefault();
                                        if (UniversalPriceMasterOthers != null)
                                        {
                                            switch (tblProductConditionLabel.PclabelName)
                                            {
                                                //case "Well Maintained":
                                                //    TblExchangeOrder.ExchangePrice = Convert.ToDecimal(UniversalPriceMasterOthers.QuoteP ?? "0") + Convert.ToDecimal(TblExchangeOrder.Sweetener ?? 0);
                                                //    break;
                                                case "Working":
                                                    TblExchangeOrder.ExchangePrice = Convert.ToDecimal(UniversalPriceMasterOthers.QuoteQ ?? "0") + Convert.ToDecimal(TblExchangeOrder.Sweetener ?? 0);
                                                    break;
                                                case "Heavily Used":
                                                    TblExchangeOrder.ExchangePrice = Convert.ToDecimal(UniversalPriceMasterOthers.QuoteR ?? "0") + Convert.ToDecimal(TblExchangeOrder.Sweetener ?? 0);
                                                    break;
                                                case "Non Working":
                                                    TblExchangeOrder.ExchangePrice = Convert.ToDecimal(UniversalPriceMasterOthers.QuoteS ?? "0") + Convert.ToDecimal(TblExchangeOrder.Sweetener ?? 0);
                                                    break;
                                            }
                                        }

                                    }

                                }


                                TblExchangeOrder.BaseExchangePrice = Convert.ToDecimal(TblExchangeOrder.ExchangePrice ?? 0) - Convert.ToDecimal(TblExchangeOrder.Sweetener ?? 0);
                                TblExchangeOrder.EstimatedDeliveryDate = DateTime.Now.AddHours(Convert.ToInt32(BusinessUnit.ExpectedDeliveryHours)).ToString("dd-MM-yyyy");
                            }

                            TblExchangeOrder.CreatedDate = _currentDatetime;
                            TblExchangeOrder.CreatedBy = userId;
                            TblExchangeOrder.ModifiedDate = _currentDatetime;
                            TblExchangeOrder.ModifiedBy = userId;
                            TblExchangeOrder.IsActive = true;
                            _ExchangeOrderRepository.Create(TblExchangeOrder);
                            _ExchangeOrderRepository.SaveChanges();


                            //Code for Order tran
                            TblOrderTran TblOrderTran = new TblOrderTran();
                            TblOrderTran.OrderType = Convert.ToInt32(OrderTypeEnum.Exchange);
                            TblOrderTran.ExchangeId = TblExchangeOrder.Id;
                            TblOrderTran.SponsorOrderNumber = TblExchangeOrder.SponsorOrderNumber;
                            TblOrderTran.RegdNo = TblExchangeOrder.RegdNo;
                            TblOrderTran.ExchangePrice = TblExchangeOrder.ExchangePrice;
                            TblOrderTran.Sweetner = TblExchangeOrder.Sweetener;


                            TblOrderTran.IsActive = true;
                            TblOrderTran.CreatedDate = _currentDatetime;
                            TblOrderTran.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                            TblOrderTran.CreatedBy = userId;

                            _orderTransRepository.Create(TblOrderTran);
                            _orderTransRepository.SaveChanges();


                            //Code for Order history

                            TblExchangeAbbstatusHistory TblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                            TblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(OrderTypeEnum.Exchange);
                            TblExchangeAbbstatusHistory.OrderTransId = TblOrderTran.OrderTransId;

                            TblExchangeAbbstatusHistory.SponsorOrderNumber = TblExchangeOrder.SponsorOrderNumber;
                            TblExchangeAbbstatusHistory.RegdNo = TblExchangeOrder.RegdNo;
                            TblExchangeAbbstatusHistory.CustId = TblCustomerDetails.Id;
                            TblExchangeAbbstatusHistory.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                            TblExchangeAbbstatusHistory.IsActive = true;
                            TblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                            TblExchangeAbbstatusHistory.CreatedBy = userId;
                            _exchangeABBStatusHistoryRepository.Create(TblExchangeAbbstatusHistory);
                            _exchangeABBStatusHistoryRepository.SaveChanges();



                        }

                        catch (Exception ex)
                        {
                            item.Remarks += ex.Message + ", ";
                            ExchangeVM1.ExchangeVMList1.Add(item);
                        }
                    }
                }
            }

            return ExchangeVM1;
        }
        #endregion

        #region Method to get the ExchangeOrder by id 
        /// <summary>
        /// Method to get the ExchangeOrder by id 
        /// </summary>
        /// <param name="id">ExchangeOrderId</param>
        /// <returns>ExchangeOrderViewModel</returns>
        public ExchangeOrderViewModel GetExchangeOrderById(int id)
        {
            ExchangeOrderViewModel ExchangeOrderVM = null;
            TblExchangeOrder TblExchangeOrder = null;

            try
            {
                TblExchangeOrder = _ExchangeOrderRepository.GetSingle(x => x.IsActive == true && x.Id == id);
                if (TblExchangeOrder != null)
                {
                    ExchangeOrderVM = _mapper.Map<TblExchangeOrder, ExchangeOrderViewModel>(TblExchangeOrder);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ExchangeOrderManager", "GetExchangeOrderById", ex);
            }
            return ExchangeOrderVM;
        }
        #endregion

        #region Method to delete ExchangeOrder by id
        /// <summary>
        /// Method to delete ExchangeOrder by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletExchangeOrderById(int id)
        {
            bool flag = false;
            try
            {
                TblExchangeOrder TblExchangeOrder = _ExchangeOrderRepository.GetSingle(x => x.IsActive == true && x.Id == id);
                if (TblExchangeOrder != null)
                {
                    TblExchangeOrder.IsActive = false;
                    _ExchangeOrderRepository.Update(TblExchangeOrder);
                    _ExchangeOrderRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ExchangeOrderManager", "DeletExchangeOrderById", ex);
            }
            return flag;
        }
        #endregion

        #region Method to get the QCOrder by Exchange id 
        /// <summary>
        /// Method to get the QCOrder by Exchange id 
        /// </summary>
        /// <param name="id">ExchangeOrderId</param>
        /// <returns>ExchangeOrderViewModel</returns>
        public ExchangeOrderViewModel GetQCOrderByExchangeId(int Id)
        {
            ExchangeOrderViewModel exchangeOrderView = null;
            ExchangeOrderViewModel exchangeOrderViewModel = new ExchangeOrderViewModel();
            TblBusinessPartner businessPartner = null;
            TblModelNumber modelNumber = null;
            TblAreaLocality tblAreaLocality = null;
            TblProductType productType = null;
            TblBusinessUnit tblBusinessUnit = null;
            TblBrand? tblBrand = null;
            try
            {
                TblOrderTran orderTran = _orderTransRepository.GetQcDetailsByExchangeId(Id);
                if (orderTran != null)
                {
                    TblExchangeOrder exchangeOrder = _ExchangeOrderRepository.GetSingleOrder(Id);
                    if (exchangeOrder != null)
                    {
                        exchangeOrderViewModel = _mapper.Map<TblExchangeOrder, ExchangeOrderViewModel>(exchangeOrder);
                        exchangeOrderViewModel.CustomerDetailViewModel = _mapper.Map<TblCustomerDetail, CustomerDetailViewModel>(exchangeOrder.CustomerDetails);
                        exchangeOrderViewModel.ProductTypeViewModel = _mapper.Map<TblProductType, ProductTypeViewModel>(exchangeOrder.ProductType);

                        if (exchangeOrderViewModel.VoucherCodeExpDate != null)
                        {
                            DateTime dt = Convert.ToDateTime(exchangeOrderViewModel.VoucherCodeExpDate);
                            exchangeOrderViewModel.VoucherCodeExpDate = dt.ToString("dd/MM/yyyy");
                        }

                        if (exchangeOrderViewModel.CustomerDetailViewModel != null)
                        {
                            exchangeOrderViewModel.CustomerDetailViewModel.CustomerName = exchangeOrderViewModel.CustomerDetailViewModel.FirstName + " " + exchangeOrderViewModel.CustomerDetailViewModel.LastName;
                            if (exchangeOrderViewModel.CustomerDetailViewModel.AreaLocalityId != null)
                            {
                                tblAreaLocality = _areaLocalityRepository.GetArealocalityById(exchangeOrderViewModel.CustomerDetailViewModel.AreaLocalityId);
                                if (tblAreaLocality != null)
                                {
                                    exchangeOrderViewModel.CustomerDetailViewModel.AreaLocality = tblAreaLocality.AreaLocality;
                                }
                            }
                        }
                        //Set IsDtoC
                        if (exchangeOrderViewModel.IsDtoC == "True")
                        {
                            exchangeOrderViewModel.IsDtoC = YesNoEnum.Yes.ToString();
                        }
                        else
                        {
                            exchangeOrderViewModel.IsDtoC = YesNoEnum.No.ToString();
                        }
                        //set IsDefferedSettlement
                        if (exchangeOrderViewModel.IsDefferedSettlement == "True")
                        {
                            exchangeOrderViewModel.IsDefferedSettlement = YesNoEnum.Yes.ToString();
                        }
                        else
                        {
                            exchangeOrderViewModel.IsDefferedSettlement = YesNoEnum.No.ToString();
                        }
                        if (exchangeOrderViewModel.ProductTypeId > 0)
                        {
                            productType = _ProductTypeRepository.GetTypebyId(exchangeOrderViewModel.ProductTypeId);
                            exchangeOrderViewModel.ProductTypeSize = productType != null ? productType.Size : null;
                            exchangeOrderViewModel.ProductTypeName = productType != null ? productType.Description : null;
                        }
                        if (exchangeOrderViewModel.NewProductTypeId > 0)
                        {
                            productType = _ProductTypeRepository.GetTypebyId(exchangeOrderViewModel.NewProductTypeId);
                            exchangeOrderViewModel.NewProductTypeSize = productType != null ? productType.Size : null;
                        }
                        if(exchangeOrderViewModel.BrandId > 0)
                        {
                            tblBrand=_brandRepository.GetBrand(exchangeOrderViewModel.BrandId);
                            if(tblBrand != null)
                            {
                                exchangeOrderViewModel.BrandName=tblBrand.Name;
                            }
                        }
                        exchangeOrderViewModel.ProductType = exchangeOrder.ProductType.ProductCat.Description;
                        exchangeOrderViewModel.ProductCategoryId = exchangeOrder.ProductType.ProductCat.Id;
                        exchangeOrderViewModel.ProductCondition = exchangeOrder.ProductCondition;
                        exchangeOrderViewModel.Sweetener = exchangeOrder.Sweetener;
                        exchangeOrderViewModel.OrderTransId = orderTran.OrderTransId;
                        exchangeOrderViewModel.EstimatedDeliveryDate = exchangeOrder.EstimatedDeliveryDate;
                        TblExchangeOrderStatus tblExchangeOrderStatus = _ExchangeOrderStatusRepository.GetByStatusId(exchangeOrderViewModel.StatusId);
                        exchangeOrderViewModel.StatusDiscription = tblExchangeOrderStatus.Id > 0 ? tblExchangeOrderStatus.StatusDescription : string.Empty;
                        exchangeOrderViewModel.StatusCode = tblExchangeOrderStatus.Id > 0 ? tblExchangeOrderStatus.StatusCode : string.Empty;

                        if (exchangeOrderViewModel.Qcdate != null && exchangeOrderViewModel.StartTime != null && exchangeOrderViewModel.EndTime != null)
                        {
                            exchangeOrderViewModel.Qcdate = exchangeOrderViewModel.Qcdate + " " + exchangeOrderViewModel.StartTime + "-" + exchangeOrderViewModel.EndTime;
                        }

                        businessPartner = _businessPartnerRepository.GetBPId(exchangeOrder.BusinessPartnerId);

                        if (businessPartner != null)
                        {
                            exchangeOrderViewModel.SaleAssociateCode = businessPartner.AssociateCode != null ? businessPartner.AssociateCode.ToString() : string.Empty;
                            exchangeOrderViewModel.SaleAssociateName = businessPartner.Name != null ? businessPartner.Name.ToString() : string.Empty;
                            exchangeOrderViewModel.SalesAssociateEmail = businessPartner.Email != null ? businessPartner.Email.ToString() : string.Empty;
                            exchangeOrderViewModel.SalesAssociatePhone = businessPartner.PhoneNumber != null ? businessPartner.PhoneNumber.ToString() : string.Empty;
                            
                            tblBusinessUnit = _context.TblBusinessUnits.Where(x => x.IsActive == true && x.BusinessUnitId == businessPartner.BusinessUnitId).FirstOrDefault();
                            if (tblBusinessUnit != null)
                            {
                                exchangeOrderViewModel.bUViewModel = _mapper.Map<TblBusinessUnit, BUViewModel>(tblBusinessUnit);
                            }

                            modelNumber = _context.TblModelNumbers.FirstOrDefault(x => x.ModelNumberId == exchangeOrder.ModelNumberId);
                            if (businessPartner.IsOrc == true)
                            {
                                exchangeOrderViewModel.NewProductcategory = exchangeOrder.ProductType.ProductCat.Description;
                                exchangeOrderViewModel.NewProductType = exchangeOrder.ProductType.Description;
                                //exchangeOrderViewModel.BrandName = exchangeOrder.Brand.Name;
                                if (modelNumber != null)
                                {
                                    exchangeOrderViewModel.ModelNumber = modelNumber.ModelName != null ? modelNumber.ModelName : "";
                                }
                            }
                            //exchangeOrderViewModel.BusinessPartnerViewModel = _mapper.Map<TblBusinessPartner, BusinessPartnerViewModel>(exchangeOrder.BusinessPartner);
                        }

                        if (exchangeOrderViewModel.NewProductTypeId != null)
                        {
                            TblProductType tblProductType = _ProductTypeRepository.GetBytypeid(exchangeOrderViewModel.NewProductTypeId);
                            if (tblProductType != null)
                            {
                                exchangeOrderViewModel.NewProductType = tblProductType.Description != null ? tblProductType.Description : string.Empty;
                            }
                        }
                        if (exchangeOrderViewModel.NewProductCategoryId != null)
                        {
                            TblProductCategory tblProductCategory = _ProductCategoryRepository.GeByid(exchangeOrderViewModel.NewProductCategoryId);
                            if (tblProductCategory != null)
                            {
                                exchangeOrderViewModel.NewProductcategory = tblProductCategory.Description != null ? tblProductCategory.Description : string.Empty;
                            }
                        }

                        //exchangeOrderViewModel.BrandName = exchangeOrder.NewBrandId != null ? exchangeOrder.Brand.Name : string.Empty;
                        if (modelNumber != null)
                        {
                            exchangeOrderViewModel.ModelNumber = modelNumber.ModelName != null ? modelNumber.ModelName : "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ExchangeOrderManager", "GetQCOrderByExchangeId", ex);
            }
            return exchangeOrderViewModel;

        }
        #endregion

        #region Method to Get Exchange Order Status Flag
        /// <summary>
        /// Method to get the multiple flags by department
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ExchangeOrderStatusViewModel</returns>

        public List<ExchangeOrderStatusViewModel> GetAllFlag()
        {
            List<ExchangeOrderStatusViewModel> exchangeOrderStatusViews = null;
            List<TblExchangeOrderStatus> tblExchangeOrderStatuses = new List<TblExchangeOrderStatus>();
            try
            {
                tblExchangeOrderStatuses = _ExchangeOrderStatusRepository.GetList(x => x.IsActive == true).ToList();

                if (tblExchangeOrderStatuses != null && tblExchangeOrderStatuses.Count > 0)
                {
                    exchangeOrderStatusViews = _mapper.Map<List<TblExchangeOrderStatus>, List<ExchangeOrderStatusViewModel>>(tblExchangeOrderStatuses);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ExchangeOrderManager", "GetAllFlag", ex);
            }
            return exchangeOrderStatusViews;

        }
        #endregion

        #region Method to get PQRS price 
        /// <summary>
        /// Method to get PQRS price 
        /// </summary>
        /// <param name="exchangeOrderViewModel"></param>
        /// <returns></returns>
        public ExchangeOrderViewModel GetPQRSPrice(ExchangeOrderViewModel exchangeOrderViewModel)
        {
            TblPriceMaster priceMasters = null;
            try
            {
                TblExchangeOrder exchangeOrder = _ExchangeOrderRepository.GetSingleOrder(exchangeOrderViewModel.Id);
                if (exchangeOrder != null)
                {
                    priceMasters = _ExchangeOrderRepository.GetOrderPrices(exchangeOrder.ProductTypeId, exchangeOrder.BusinessPartner != null ? exchangeOrder.BusinessPartner.BusinessUnit.Login.PriceCode : null, exchangeOrderViewModel.CompanyName);
                }
                if (priceMasters != null)
                {
                    exchangeOrderViewModel.Excellent = Convert.ToDecimal(priceMasters.QuotePHigh);
                    exchangeOrderViewModel.Good = Convert.ToDecimal(priceMasters.QuoteQHigh);
                    exchangeOrderViewModel.Average = Convert.ToDecimal(priceMasters.QuoteRHigh);
                    exchangeOrderViewModel.Notworking = Convert.ToDecimal(priceMasters.QuoteSHigh);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ExchangeOrderManager", "GetPQRSPrice", ex);
            }
            return exchangeOrderViewModel;
        }
        #endregion

        #region method to send selfqc Url to customer
        /// <summary>
        /// method to send selfqc Url to customer
        /// </summary>
        /// <param name="regdNo"></param>
        /// <returns></returns>
        public bool sendSelfQCUrl(string regdNo, string mobnumber, int? loginid)
        {
            WhatasappResponse whatasappResponse = new WhatasappResponse();
            TblWhatsAppMessage tblwhatsappmessage = null;
            TblExchangeOrder tblExchangeOrder = null;
            TblCustomerDetail tblCustomerDetail = null;
            TblOrderTran tblOrderTran = null;
            bool flag = true;
            string message = string.Empty;
            string baseurl = _config.Value.BaseURL + "QCPortal/SelfQC?regdno=" + regdNo;
            TblAbbredemption tblAbbredemption = null;
            try
            {
                tblOrderTran = _orderTransRepository.GetSingle(x => x.IsActive == true && x.RegdNo == regdNo);
                if (tblOrderTran != null)
                {
                    if (tblOrderTran.OrderType == Convert.ToInt32(OrderTypeEnum.ABB))
                    {
                        tblAbbredemption = _context.TblAbbredemptions.Where(x => x.IsActive == true && x.RegdNo == tblOrderTran.RegdNo).FirstOrDefault();
                        if (tblAbbredemption != null && tblAbbredemption.CustomerDetailsId > 0)
                        {
                            tblCustomerDetail = _customerDetailsRepository.GetCustDetails(tblAbbredemption.CustomerDetailsId);
                        }
                    }
                    else if (tblOrderTran.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                    {
                        tblExchangeOrder = _context.TblExchangeOrders.Where(x => x.IsActive == true && x.RegdNo == tblOrderTran.RegdNo).FirstOrDefault();
                        if (tblExchangeOrder != null && tblExchangeOrder.CustomerDetailsId > 0)
                        {
                            tblCustomerDetail = _customerDetailsRepository.GetCustDetails(tblExchangeOrder.CustomerDetailsId);
                        }
                    }

                    tblOrderTran.IsActive = true;
                    tblOrderTran.SelfQclinkResendby = loginid;
                    tblOrderTran.ModifiedDate = _currentDatetime; 
                    _orderTransRepository.Update(tblOrderTran);
                    _orderTransRepository.SaveChanges();
                }
                #region code to send selfqc link on whatsappNotification
                WhatsappTemplate whatsappObj = new WhatsappTemplate();
                whatsappObj.userDetails = new UserDetails();
                whatsappObj.notification = new SelfQC();
                whatsappObj.notification.@params = new URL();
                whatsappObj.userDetails.number = mobnumber;
                whatsappObj.notification.sender = _config.Value.YelloaiSenderNumber;
                whatsappObj.notification.type = _config.Value.YellowaiMesssaheType;
                whatsappObj.notification.templateId = NotificationConstants.SelfQC_Link;
                whatsappObj.notification.@params.Link = baseurl;
                whatsappObj.notification.@params.CustomerName = tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName;
                string url = _config.Value.YellowAiUrl;
                RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                if (response.Content != null)
                {
                    whatasappResponse = JsonConvert.DeserializeObject<WhatasappResponse>(response.Content);
                    tblwhatsappmessage = new TblWhatsAppMessage();
                    tblwhatsappmessage.TemplateName = NotificationConstants.Logi_Drop;
                    tblwhatsappmessage.IsActive = true;
                    tblwhatsappmessage.PhoneNumber = mobnumber;
                    tblwhatsappmessage.SendDate = DateTime.Now;
                    tblwhatsappmessage.MsgId = whatasappResponse.msgId;
                    _WhatsAppMessageRepository.Create(tblwhatsappmessage);
                    _WhatsAppMessageRepository.SaveChanges();
                }
                #endregion
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ExchangeOrderManager", "SendSelfQCUrlToCustomer", ex);
            }
            return flag;
        }
        #endregion

        #region Add Multiple exchange orders for D2C through api
        /// <summary>
        /// Add Multiple orders through api for D2C
        /// Added by ashwin
        /// </summary>
        /// <param name="multipleExchangeOrdersDataModel"></param>
        /// <returns>responseResult</returns>
        public ResponseResult AddMultipleOrders(MultipleExchangeOrdersDataModel multipleExchangeOrdersDataModel, string username)
        {
            #region Variables Declarations
            ResponseResult responseResult = new ResponseResult();
            OrderBasicDetailsDataViewModel orderBasicDetailsDataViewModel = new OrderBasicDetailsDataViewModel();
            responseResult.message = string.Empty;
            int customerDetailsId = 0;
            //in case of estimated hours not found in database
            //int DefaultEstimatedhours = 48 ;
            int BusinessUnitEnum = 4;
            DAL.Entities.Login login = null;
            TblBusinessUnit businessUnit = null;
            TblBusinessPartner tblBusinessPartner = null;
            List<OrderResult> orderResult = new List<OrderResult>();
            TblCustomerDetail tblCustomerDetail = new TblCustomerDetail();
            #endregion
            //added by shashi for inserting userid into questioners 
            int userId = 3;
            try
            {
                if (multipleExchangeOrdersDataModel != null && multipleExchangeOrdersDataModel.CustomerDetailViewModel != null && multipleExchangeOrdersDataModel.productDetailsDataViewModels.Count > 0)
                {
                    #region bind login User details 
                    login = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.Username) && x.Username.ToLower().Equals(username.ToLower()));
                    if (login != null && login.BusinessPartnerId != null)
                    {
                        orderBasicDetailsDataViewModel.BusinessPartnerId = (int)login.BusinessPartnerId;
                        orderBasicDetailsDataViewModel.BUId = login.SponsorId != null ? Convert.ToInt32(login.SponsorId) : 0;
                        if (login != null && login.SponsorId != null)
                        {
                            businessUnit = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == login.SponsorId);
                            if (businessUnit != null)
                            {
                                orderBasicDetailsDataViewModel.LoginID = businessUnit.BusinessUnitId;
                                orderBasicDetailsDataViewModel.CompanyName = businessUnit.Name;
                                orderBasicDetailsDataViewModel.EstimatedDeliveryDate = DateTime.Now.AddHours(Convert.ToInt32(businessUnit.ExpectedDeliveryHours)).ToString("dd-MMM-yyyy");

                                tblBusinessPartner = _businessPartnerRepository.GetSingle(x => x.BusinessUnitId == businessUnit.BusinessUnitId
                                                    && x.IsActive.Equals(true)
                                                    && x.BusinessPartnerId.Equals(login.BusinessPartnerId));
                                if (tblBusinessPartner != null)
                                {
                                    orderBasicDetailsDataViewModel.StoreCode = tblBusinessPartner.StoreCode;
                                    orderBasicDetailsDataViewModel.IsDefferedSettlement = tblBusinessPartner.IsDefferedSettlement == null ? false : Convert.ToBoolean(tblBusinessPartner.IsDefferedSettlement);
                                }
                                else
                                {
                                    responseResult.message = "No associated store found for this order";
                                    responseResult.Status = false;
                                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                                    return responseResult;
                                }
                            }
                            else
                            {
                                responseResult.message = "No Bussiness Unit found for the requested user";
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.BadRequest;
                                return responseResult;
                            }
                        }
                        else
                        {
                            responseResult.message = "Sponsor Id not found for login user";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            return responseResult;
                        }
                    }
                    else
                    {
                        responseResult.message = "Details not found for login user";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        return responseResult;
                    }
                    #endregion

                    #region Add CustomerDetails 
                    if (multipleExchangeOrdersDataModel.CustomerDetailViewModel != null && multipleExchangeOrdersDataModel.CustomerDetailViewModel.Id == 0)
                    {
                        CustomerDetailViewModel customerDetailViewModel = multipleExchangeOrdersDataModel.CustomerDetailViewModel;

                        tblCustomerDetail = _mapper.Map<CustomerDetailViewModel, TblCustomerDetail>(customerDetailViewModel);
                        if (tblCustomerDetail != null)
                        {
                            tblCustomerDetail.CreatedDate = _currentDatetime;
                            tblCustomerDetail.IsActive = true;
                            _customerDetailsRepository.Create(tblCustomerDetail);
                            int custId = _customerDetailsRepository.SaveChanges();
                            customerDetailsId = tblCustomerDetail.Id;
                            orderBasicDetailsDataViewModel.CustomerMobileNumber = tblCustomerDetail.PhoneNumber;
                        }
                        else
                        {
                            responseResult.message = "Error occurs while mapping customer details";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            return responseResult;
                        }
                    }
                    else if (multipleExchangeOrdersDataModel.CustomerDetailViewModel != null && multipleExchangeOrdersDataModel.CustomerDetailViewModel.Id > 0)
                    {
                        customerDetailsId = multipleExchangeOrdersDataModel.CustomerDetailViewModel.Id;
                        tblCustomerDetail = _customerDetailsRepository.GetSingle(x => x.IsActive == true && x.Id == multipleExchangeOrdersDataModel.CustomerDetailViewModel.Id);
                        orderBasicDetailsDataViewModel.CustomerMobileNumber = multipleExchangeOrdersDataModel.CustomerDetailViewModel.PhoneNumber;
                    }
                    else
                    {
                        responseResult.message = "Customer details is needed";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        return responseResult;
                    }
                    #endregion

                    #region Add Products details
                    if (multipleExchangeOrdersDataModel.productDetailsDataViewModels.Count > 0)
                    {
                        #region loop for orders insert
                        foreach (var item in multipleExchangeOrdersDataModel.productDetailsDataViewModels)
                        {
                            OrderResult orderResult1 = new OrderResult();
                            orderResult1.customerDetailsId = customerDetailsId;
                            ProductDetailsDataViewModel productDetailsDataViewModels = new ProductDetailsDataViewModel();
                            productDetailsDataViewModels = item;
                            string SponsorOrderNumber = string.Empty;
                            productDetailsDataViewModels.SponsorOrderNumber = null;

                            #region add sponsor order number

                            if (string.IsNullOrEmpty(productDetailsDataViewModels.SponsorOrderNumber))
                                productDetailsDataViewModels.SponsorOrderNumber = "API" + UniqueString.RandomNumberByLength(9);

                            else if (businessUnit.BusinessUnitId == BusinessUnitEnum)
                            {
                                if (string.IsNullOrEmpty(productDetailsDataViewModels.SponsorOrderNumber))
                                    productDetailsDataViewModels.SponsorOrderNumber = "D2C" + UniqueString.RandomNumberByLength(9);
                            }
                            //check sponsor number in database
                            if (productDetailsDataViewModels.SponsorOrderNumber != null)
                            {
                                bool check = false;
                                bool check1 = false;
                                //TblExchangeOrder tblExchangeOrders = _ExchangeOrderRepository.GetSingle(x => x.IsActive == true && x.SponsorOrderNumber == productDetailsDataViewModels.SponsorOrderNumber);
                                var checkemail = _context.TblExchangeOrders.ToList();
                                string checksponsorordernumber = productDetailsDataViewModels.SponsorOrderNumber;
                                check = checkemail.Exists(p => p.SponsorOrderNumber == checksponsorordernumber);
                                if (check == true)
                                {
                                    while (check == true)
                                    {
                                        if (string.IsNullOrEmpty(productDetailsDataViewModels.SponsorOrderNumber))
                                            productDetailsDataViewModels.SponsorOrderNumber = "API" + UniqueString.RandomNumberByLength(9);

                                        else if (businessUnit.BusinessUnitId == BusinessUnitEnum)
                                        {
                                            if (string.IsNullOrEmpty(productDetailsDataViewModels.SponsorOrderNumber))
                                                productDetailsDataViewModels.SponsorOrderNumber = "D2C" + UniqueString.RandomNumberByLength(9);
                                        }
                                        var checkemail1 = _context.TblExchangeOrders.ToList();
                                        //TblExchangeOrder tblExchangeOrders1 = _ExchangeOrderRepository.GetSingle(x => x.IsActive == true && x.SponsorOrderNumber == productDetailsDataViewModels.SponsorOrderNumber);
                                        string checksponsorordernumber1 = productDetailsDataViewModels.SponsorOrderNumber;
                                        check1 = checkemail.Exists(p => p.SponsorOrderNumber == checksponsorordernumber);

                                        check = check1;
                                    }
                                }
                            }
                            #endregion

                            #region add regdno
                            if (orderBasicDetailsDataViewModel.StoreCode != null)
                            {
                                productDetailsDataViewModels.RegdNo = "E" + UniqueString.RandomNumberByLength(7);
                                productDetailsDataViewModels.UploadDateTime = _currentDatetime.ToString();
                                orderResult1.orderRegdNo = productDetailsDataViewModels.RegdNo;
                            }
                            #endregion

                            #region insert tblexchangeorder,tblordertrans,tblaxchangeabbhistory
                            if (orderBasicDetailsDataViewModel != null && orderBasicDetailsDataViewModel.LoginID > 0)
                            {
                                int resultexchangeorder = 0;
                                int isquestionerinsert = 0;
                                bool flag = true;
                                TblExchangeOrder tblExchangeOrder; // = new TblExchangeOrder();
                                tblExchangeOrder = _mapper.Map<ProductDetailsDataViewModel, TblExchangeOrder>(productDetailsDataViewModels);
                                if (tblExchangeOrder != null)
                                {
                                    tblExchangeOrder.LoginId = orderBasicDetailsDataViewModel.LoginID;
                                    tblExchangeOrder.StoreCode = orderBasicDetailsDataViewModel.StoreCode;
                                    tblExchangeOrder.IsDefferedSettlement = orderBasicDetailsDataViewModel.IsDefferedSettlement;
                                    tblExchangeOrder.EstimatedDeliveryDate = orderBasicDetailsDataViewModel.EstimatedDeliveryDate;
                                    tblExchangeOrder.BusinessPartnerId = orderBasicDetailsDataViewModel.BusinessPartnerId;
                                    tblExchangeOrder.CompanyName = orderBasicDetailsDataViewModel.CompanyName;
                                    tblExchangeOrder.CustomerDetailsId = customerDetailsId;
                                    tblExchangeOrder.IsDtoC = true;
                                    tblExchangeOrder.IsActive = true;
                                    tblExchangeOrder.CreatedDate = _currentDatetime;
                                    tblExchangeOrder.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                                    tblExchangeOrder.OrderStatus = EnumHelper.DescriptionAttr(OrderStatusEnum.OrderCreatedbySponsor);
                                    if (item.questionerViewModel.FinalPrice > 0)
                                    {
                                        tblExchangeOrder.ExchangePrice = Convert.ToDecimal(item.questionerViewModel.FinalPrice);
                                    }
                                    else if (item.questionerViewModel.NonWorkingPrice > 0)
                                    {
                                        tblExchangeOrder.ExchangePrice = Convert.ToDecimal(item.questionerViewModel.NonWorkingPrice);
                                    }
                                    _ExchangeOrderRepository.Create(tblExchangeOrder);
                                    resultexchangeorder = _ExchangeOrderRepository.SaveChanges();
                                    orderResult1.OrderId = tblExchangeOrder.Id;
                                    orderResult1.productId = tblExchangeOrder.ProductTypeId;
                                }
                                if (resultexchangeorder > 0 && orderResult1.OrderId > 0)
                                {
                                    #region add data into tblordertrans
                                    TblOrderTran tblOrderTran = new TblOrderTran();
                                    OrderTransactionViewModel orderTransactionViewModel = new OrderTransactionViewModel();

                                    orderTransactionViewModel.OrderType = Convert.ToInt32(LoVEnum.Exchange);
                                    orderTransactionViewModel.ExchangeId = orderResult1.OrderId;
                                    orderTransactionViewModel.SponsorOrderNumber = productDetailsDataViewModels.SponsorOrderNumber;
                                    orderTransactionViewModel.RegdNo = productDetailsDataViewModels.RegdNo;
                                    orderTransactionViewModel.ExchangePrice = tblExchangeOrder.ExchangePrice;
                                    orderTransactionViewModel.Sweetner = tblExchangeOrder.Sweetener;
                                    orderTransactionViewModel.IsActive = true;
                                    orderTransactionViewModel.CreatedDate = _currentDatetime;

                                    int ordertransresult = _orderTransactionManager.ManageOrderTransaction(orderTransactionViewModel);

                                    if (ordertransresult > 0)
                                    {
                                        orderResult1.ordertransId = ordertransresult;
                                    }
                                    #endregion

                                    #region add history for order created into tblExchangeAbbstatusHistory
                                    TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                    tblExchangeAbbstatusHistory.CustId = customerDetailsId;
                                    tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                    tblExchangeAbbstatusHistory.Comment = "D2C order by WebApi";
                                    tblExchangeAbbstatusHistory.IsActive = true;
                                    tblExchangeAbbstatusHistory.CreatedBy = 3;
                                    tblExchangeAbbstatusHistory.RegdNo = tblExchangeOrder.RegdNo;
                                    tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                                    tblExchangeAbbstatusHistory.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                                    tblExchangeAbbstatusHistory.OrderTransId = orderResult1.ordertransId;
                                    _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                                    int exchangabbhistory = _exchangeABBStatusHistoryRepository.SaveChanges();
                                    if (exchangabbhistory > 0)
                                    {
                                        orderResult1.isHistoryCreated = true;
                                    }
                                    #endregion

                                    #region Whatsapp notification and selfQC link send to customer whatsapp
                                    //old
                                    //bool isWhatsappsent = sendSelfQCUrl(productDetailsDataViewModels.RegdNo, orderBasicDetailsDataViewModel.CustomerMobileNumber);
                                    //orderResult1.isWhatsAppNotificationSend = isWhatsappsent;
                                    //new
                                    #region code to send selfqc link on whatsappNotification
                                    WhatasappResponseOrderConfirmation whatasappResponse = new WhatasappResponseOrderConfirmation();
                                    string baseurl = _config.Value.BaseURL + "QCPortal/SelfQC?regdno=" + productDetailsDataViewModels.RegdNo;
                                    TblWhatsAppMessage tblWhatsAppMessage = null;
                                    WhatsappTemplateOrderConfirmation whatsappObj = new WhatsappTemplateOrderConfirmation();

                                    whatsappObj.userDetails = new UserDetailsOrderConfirmation();
                                    whatsappObj.notification = new SelfQCOrderConfirmation();
                                    whatsappObj.notification.@params = new URLOrderConfirmation();
                                    whatsappObj.userDetails.number = orderBasicDetailsDataViewModel.CustomerMobileNumber;
                                    whatsappObj.notification.sender = _config.Value.YelloaiSenderNumber;
                                    whatsappObj.notification.type = _config.Value.YellowaiMesssaheType;
                                    whatsappObj.notification.templateId = NotificationConstants.orderConfirmationForExchangeUpdated;
                                    if (tblCustomerDetail != null)
                                    {
                                        whatsappObj.notification.@params.CustName = tblCustomerDetail.FirstName;
                                        whatsappObj.notification.@params.Address = tblCustomerDetail.Address1 + " " + tblCustomerDetail.Address2;
                                        whatsappObj.notification.@params.Email = tblCustomerDetail.Email != null ? tblCustomerDetail.Email : string.Empty;
                                        whatsappObj.notification.@params.PhoneNumber = tblCustomerDetail.PhoneNumber != null ? tblCustomerDetail.PhoneNumber : string.Empty;
                                        whatsappObj.notification.@params.CustomerName = tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName;
                                    }
                                    whatsappObj.notification.@params.Link = baseurl;

                                    TblProductType tblProductType = _ProductTypeRepository.GetSingle(x => x.IsActive == true && x.Id == tblExchangeOrder.ProductTypeId);
                                    if (tblProductType != null && tblProductType.ProductCatId > 0)
                                    {
                                        TblProductCategory tblProductCategory = _ProductCategoryRepository.GetSingle(x => x.IsActive == true && x.Id == tblProductType.ProductCatId);
                                        whatsappObj.notification.@params.ProdCategory = tblProductCategory != null ? tblProductCategory.Description : string.Empty;
                                        productDetailsDataViewModels.productDescription = tblProductCategory != null ? tblProductCategory.Description : string.Empty;
                                        whatsappObj.notification.@params.ProdType = tblProductType.Description != string.Empty ? tblProductType.Description : string.Empty;
                                    }
                                    whatsappObj.notification.@params.RegdNO = productDetailsDataViewModels.RegdNo;
                                    string url = _config.Value.YellowAiUrl;
                                    RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                                    if (response.Content != null)
                                    {
                                        whatasappResponse = JsonConvert.DeserializeObject<WhatasappResponseOrderConfirmation>(response.Content);
                                        tblWhatsAppMessage = new TblWhatsAppMessage();
                                        tblWhatsAppMessage.TemplateName = NotificationConstants.orderConfirmationForExchangeUpdated;
                                        tblWhatsAppMessage.IsActive = true;
                                        tblWhatsAppMessage.PhoneNumber = orderBasicDetailsDataViewModel.CustomerMobileNumber;
                                        tblWhatsAppMessage.SendDate = DateTime.Now;
                                        tblWhatsAppMessage.MsgId = whatasappResponse.msgId;
                                        _WhatsAppMessageRepository.Create(tblWhatsAppMessage);
                                        _WhatsAppMessageRepository.SaveChanges();
                                        orderResult1.isWhatsAppNotificationSend = true;

                                    }
                                    #endregion
                                    #region Code to Send Mail to Customer
                                    if (tblCustomerDetail != null)
                                    {
                                        if (!string.IsNullOrEmpty(tblCustomerDetail.Email) && !string.IsNullOrEmpty(baseurl)
                                            && !string.IsNullOrEmpty(productDetailsDataViewModels.productDescription)
                                            && !string.IsNullOrEmpty(tblCustomerDetail.FirstName))
                                        {
                                            tblCustomerDetail.Email.Trim();
                                            SendABBEmail(tblCustomerDetail.Email, tblCustomerDetail.FirstName, productDetailsDataViewModels.productDescription, baseurl);
                                        }
                                    }
                                    #endregion

                                    #endregion

                                    #region Questioner's insert
                                    productDetailsDataViewModels.questionerViewModel.OrderTrandId = Convert.ToInt32(orderResult1.ordertransId);
                                    QuestionerViewModel questionerViewModel = new QuestionerViewModel();
                                    List<QCRatingViewModel> qCRatingViewModels = new List<QCRatingViewModel>();
                                    questionerViewModel = productDetailsDataViewModels.questionerViewModel;
                                    qCRatingViewModels = productDetailsDataViewModels.qCRatingViewModels;

                                    if (questionerViewModel != null && qCRatingViewModels.Count > 0)
                                    {
                                        isquestionerinsert = _qCCommentManager.saveAndSubmit(questionerViewModel, qCRatingViewModels, userId, flag);
                                        if (isquestionerinsert > 0)
                                        {
                                            orderResult1.isQuestionersdone = true;
                                        }
                                        else
                                        {
                                            orderResult1.isQuestionersdone = false;

                                        }
                                    }
                                    #endregion
                                }
                                orderResult.Add(orderResult1);
                            }
                            #endregion
                        }
                        #endregion
                        if (orderResult.Count > 0)
                        {
                            responseResult.Data = orderResult;
                            responseResult.Status = true;
                            responseResult.Status_Code = HttpStatusCode.OK;
                            responseResult.message = "Success";
                            return responseResult;
                        }
                        else
                        {
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            responseResult.message = "Registration failed";
                        }
                    }
                    else
                    {
                        responseResult.message = "Products details should not be null";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        return responseResult;
                    }
                    #endregion
                }
                else
                {
                    responseResult.message = "Request object is null";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("QCCommentManager", "GetQuestionswithLov", ex);
            }
            return responseResult;
        }
        #endregion

        #region Send ABB Email
        private void SendABBEmail(string custEmail, string customerName, string productDescription, string selfQcUrl)
        {
            try
            {
                //string subject = "Customer Details";
                string subject = "Exchange Detail";
                string body = EmailBody();
                string too = custEmail;
                body = body.Replace("[customerName]", customerName);
                body = body.Replace("[ProductCategory]", productDescription);
                body = body.Replace("[SelfQCUrl]", selfQcUrl);
                body = body.Replace("[productCatDescription]", productDescription);

                _mailManager.JetMailSend(too, body, subject);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ExchangeOrderManager", "SendABBEmail", ex);
            }
        }
        #endregion

        #region Email Body
        public string EmailBody()
        {

            string htmlstring = @"<!DOCTYPE html>
<html>
<meta http-equiv='Content-Type' content='text/html charset=UTF-8'>

<head>
    <style type='text/css'>
        table {
            border-collapse: separate;
        }

        a,
        a:link,
        a:visited {
            text-decoration: none;
            color: #00788a;
        }

            a:hover {
                text-decoration: underline;
            }

        h2,
        h2 a,
        h2 a:visited,
        h3,
        h3 a,
        h3 a:visited,
        h4,
        h5,
        h6,
        .t_cht {
            color: #000 !important;
        }

        .ExternalClass p,
        .ExternalClass span,
        .ExternalClass font,
        .ExternalClass td {
            line-height: 100%;
        }

        .ExternalClass {
            width: 100%;
        }
    </style>
</head>

<body style='background-color:azure; font-family: Arial, Helvetica, sans-serif; font-size: 14px; line-height: 22px; mso-line-height-rule:exactly; padding: 40px 0;'>




    <span style='padding: 40px 0'>
        <span style='width: 660px; margin: 0 auto;background-color: #fff;box-sizing: border-box;border-top: 7px solid #f26d42;display: block;'>
            <span>
                <span style='height:0; max-height:0;'>

                    <img src='http://digi2l.in/wp-content/uploads/2023/02/banner-01.png'
                         style='width: 100%; object-fit: contain;'>

                </span>
                <span style='display:block; padding: 0 40px 10px;'>
                    <p><b>Greetings from Digi2l!!</b></p>
                    <p>Hello [customerName],</p>
                    <p>
                        We have received your sales order on our platform. Our team will get in touch with you. 
                    </p>
                    <p>
                        Thank you for choosing our Smart Sell service for your [ProductCategory]. Please provide the prerequisite
                        for
                        the video quality check by clicking the link below.
                    </p>
                    <span style='font-size: 12px;display: block;'>
                        <span class='app-category' style='text-align: center;margin: 20px 0;width: 100%;display: block;'>
                            <a href='[SelfQCUrl]' target='_blank'
                               style='border-radius: 3px;background-color: #0D004C;color: #fff;display: inline-block;padding: 12px 20px;margin: 5px 10px;text-decoration: none;border-bottom: 4px solid #4f0495;font-weight: normal;'>[productCatDescription]</a>
                        </span>
                    </span>
                    <p>
                        For clarifications/query call us on <a href='tel:9619697745'
                                                               style='color: #45079c;text-decoration: none;font-weight: bold;'>+91 9619697745</a> or reach us at <a href='mailto:customercare@digi2l.in'
                                                                                                                                                                    style='color: #45079c;text-decoration: none;font-weight: bold;'>customercare@digi2l.in</a>.
                    </p>

                    <p>TEAM DIGI2L</p>
                </span>

            </span>

        </span>
    </span>
    <span style='background-color: #0D004C;color: #fff;padding: 18px 12px;text-align: center;width: 660px;margin: 0 auto;box-sizing: border-box;border-bottom: 6px solid #7135d8;display: block;'>
        India's 1<sup style='font-size: 11px;'>st</sup> Platform To Sell Used Appliance
        <p style='font-size: 12px; padding: 8px 0 0 0; border-top: 1px solid #676767; font-weight:lighter;letter-spacing: 1px; opacity: 0.6;'>
            For more information visit our website: <a style='color: #fff;text-decoration: none;font-weight: bold;'
                                                       href='https://digi2l.in/' target='_blank'>https://digi2l.in/</a>
        </p>
        <span class='socialmediabtns' style='margin: 0;display: block;'>
            <a href='https://www.facebook.com/digi2l1/' target='_blank'
               style='width: 24px;display: inline-block;margin: 0px 10px 0 10px;padding: 4px;color: #45079c;text-decoration: none;font-weight: normal;border: 1px solid #858585;max-height: 24px;'>
                <img src='https://digi2l.in/wp-content/uploads/2023/01/social-01.png' alt='' style='width: 100%;'>
            </a>
            <a href='https://www.instagram.com/digi2l_/' target='_blank'
               style='width: 24px;display: inline-block;margin: 0px 10px 0 10px;padding: 4px;color: #45079c;text-decoration: none;font-weight: normal;border: 1px solid #858585;max-height: 24px;'>
                <img src='https://digi2l.in/wp-content/uploads/2023/01/social-02.png' alt='' style='width: 100%;'>
            </a>
            <a href='https://twitter.com/digi2l_' target='_blank'
               style='width: 24px;display: inline-block;margin: 0px 10px 0 10px;padding: 4px;color: #45079c;text-decoration: none;font-weight: normal;border: 1px solid #858585;max-height: 24px;'>
                <img src='https://digi2l.in/wp-content/uploads/2023/01/social-03.png' alt='' style='width: 100%;'>
            </a>
            <a href='https://www.linkedin.com/company/digi2l1/' target='_blank'
               style='width: 24px;display: inline-block;margin: 0px 10px 0 10px;padding: 4px;color: #45079c;text-decoration: none;font-weight: normal;border: 1px solid #858585;max-height: 24px;'>
                <img src='https://digi2l.in/wp-content/uploads/2023/01/social-04.png' alt='' style='width: 100%;'>
            </a>
        </span>
    </span>

</body>

</html>";

            return htmlstring;
        }
        #endregion


        #region Add  exchange orders for Samsung
        /// <summary>
        /// Added by ashwin
        /// </summary>
        /// <returns>responseResult</returns>
        public string AddExchangeOrders(ExchangeOrderDataContract ExchangeOrdersDataModel)
        {
            #region Variables Declarations
            ResponseResult responseResult = new ResponseResult();
            OrderBasicDetailsDataViewModel orderBasicDetailsDataViewModel = new OrderBasicDetailsDataViewModel();
            responseResult.message = string.Empty;
            //int customerDetailsId = 0;
            //int BusinessUnitEnum = 4;
            //DAL.Entities.Login login = null;
            //TblBusinessUnit businessUnit = null;
            //TblBusinessPartner tblBusinessPartner = null;
            List<OrderResult> orderResult = new List<OrderResult>();
            string resultedRegdNo = string.Empty;
            TblCustomerDetail TblCustomerDetails = new TblCustomerDetail();
            TblExchangeOrder TblExchangeOrder = new TblExchangeOrder();

            #endregion
            int userId = 3;
            try
            {
                if (ExchangeOrdersDataModel != null)
                {
                    #region Add Customer details

                    //Code to insert the object 
                    TblCustomerDetails.FirstName = ExchangeOrdersDataModel.FirstName;
                    TblCustomerDetails.LastName = ExchangeOrdersDataModel.LastName;
                    TblCustomerDetails.PhoneNumber = ExchangeOrdersDataModel.PhoneNumber;
                    TblCustomerDetails.Email = ExchangeOrdersDataModel.Email;
                    TblCustomerDetails.City = ExchangeOrdersDataModel.CityName;

                    TblCustomerDetails.State = ExchangeOrdersDataModel.StateName;
                    TblCustomerDetails.ZipCode = ExchangeOrdersDataModel.PinCode;
                    TblCustomerDetails.Address1 = ExchangeOrdersDataModel.Address1;
                    TblCustomerDetails.Address2 = ExchangeOrdersDataModel.Address2;
                    TblCustomerDetails.IsActive = true;
                    TblCustomerDetails.CreatedDate = _currentDatetime;
                    TblCustomerDetails.CreatedBy = ExchangeOrdersDataModel.CreatedBy;
                    _customerDetailsRepository.Create(TblCustomerDetails);
                    _customerDetailsRepository.SaveChanges();


                    #endregion

                    #region add exchange data into tblexchangeorder

                    if(TblCustomerDetails != null && TblCustomerDetails.Id>0)
                    {
                        //Code to insert the object 
                        #region BU Details & BP Details
                        if (ExchangeOrdersDataModel.BusinessUnitId > 0)
                        {
                            TblBusinessUnit tblBusinessUnit = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == ExchangeOrdersDataModel.BusinessUnitId);
                            if (tblBusinessUnit != null)
                            {
                                TblExchangeOrder.CompanyName = tblBusinessUnit.Name != string.Empty ? tblBusinessUnit.Name : string.Empty;
                                TblExchangeOrder.BusinessUnitId = Convert.ToInt32(tblBusinessUnit.BusinessUnitId);
                                TblExchangeOrder.EstimatedDeliveryDate = DateTime.Now.AddHours(Convert.ToInt32(tblBusinessUnit.ExpectedDeliveryHours)).ToString("dd-MM-yyyy");

                            }
                        }
                        if (ExchangeOrdersDataModel.BusinessPartnerId > 0)
                        {
                            TblBusinessPartner tblBusinessPartner = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == ExchangeOrdersDataModel.BusinessPartnerId);
                            if (tblBusinessPartner != null)
                            {
                                TblExchangeOrder.StoreCode = tblBusinessPartner.StoreCode != string.Empty ? tblBusinessPartner.StoreCode : string.Empty;
                                TblExchangeOrder.BusinessPartnerId = Convert.ToInt32(tblBusinessPartner.BusinessPartnerId);
                                TblExchangeOrder.IsDefferedSettlement = tblBusinessPartner.IsDefferedSettlement == true ? tblBusinessPartner.IsDefferedSettlement : false;
                                TblExchangeOrder.IsDtoC = tblBusinessPartner.IsD2c == true ? tblBusinessPartner.IsD2c : false;
                            }
                        }

                        #endregion

                        TblExchangeOrder.CustomerDetailsId = TblCustomerDetails.Id;
                        TblExchangeOrder.RegdNo = "E" + UniqueString.RandomNumberByLength(7);
                        resultedRegdNo = TblExchangeOrder.RegdNo;

                        #region product quality
                        //TblProductConditionLabel conditionLabel = _productConditionLabelRepository.GetSingle(x => x.BusinessUnitId == ExchangeOrdersDataModel.BusinessUnitId && x.BusinessPartnerId == ExchangeOrdersDataModel.BusinessPartnerId && x.OrderSequence == ExchangeOrdersDataModel.QualityCheckValue && x.IsActive == true);

                        //if (conditionLabel != null)
                        //{
                        //    TblExchangeOrder.ProductCondition = conditionLabel.PclabelName != string.Empty ? conditionLabel.PclabelName : string.Empty;
                        //}
                        //else
                        //{
                        //    conditionLabel = _productConditionLabelRepository.GetSingle(x => x.BusinessUnitId == ExchangeOrdersDataModel.BusinessUnitId && x.BusinessPartnerId == null && x.OrderSequence == ExchangeOrdersDataModel.QualityCheckValue && x.IsActive == true);
                        //    TblExchangeOrder.ProductCondition = conditionLabel.PclabelName!=string.Empty? conditionLabel.PclabelName:string.Empty;
                        //}

                        switch (ExchangeOrdersDataModel.QualityCheckValue)
                        {
                            case 1:
                                TblExchangeOrder.ProductCondition = "Excellent";
                                break;
                            case 2:
                                TblExchangeOrder.ProductCondition = "Good";
                                break;
                            case 3:
                                TblExchangeOrder.ProductCondition = "Average";
                                break;
                            case 4:
                                TblExchangeOrder.ProductCondition = "Not Working";
                                break;
                            default:
                                break;
                        }
                        #endregion


                        //TblExchangeOrder.CustomerDetailsId = ExchangeOrdersDataModel.ProductTypeId;
                        TblExchangeOrder.ProductTypeId = ExchangeOrdersDataModel.ProductTypeId;

                        TblExchangeOrder.BrandId = ExchangeOrdersDataModel.BrandId;
                        TblExchangeOrder.Bonus = "0";
                        TblExchangeOrder.LoginId = ExchangeOrdersDataModel.CreatedBy;
                        TblExchangeOrder.PurchasedProductCategory = ExchangeOrdersDataModel.PurchasedProductCategory;
                        if (ExchangeOrdersDataModel.SponsorOrderNumber != null)
                        {
                            TblExchangeOrder.SponsorOrderNumber = ExchangeOrdersDataModel.SponsorOrderNumber.Trim() + TblExchangeOrder.RegdNo;
                        }
                        else
                        {
                            TblExchangeOrder.SponsorOrderNumber = "ECH" + UniqueString.RandomNumberByLength(9);
                        }

                        TblExchangeOrder.ExchangePrice = ExchangeOrdersDataModel.ExchangePrice;
                        TblExchangeOrder.BaseExchangePrice = ExchangeOrdersDataModel.ExchangePrice;

                        TblExchangeOrder.Sweetener = ExchangeOrdersDataModel.Sweetener;
                        TblExchangeOrder.PriceMasterNameId = Convert.ToInt32(ExchangeOrdersDataModel.PriceMasterNameId);

                        TblExchangeOrder.OrderStatus = "Order Created";
                        TblExchangeOrder.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);

                        //var BusinessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == ExchangeOrdersDataModel.BusinessUnitId);
                        //if (BusinessUnit != null)
                        //{
                        //    TblExchangeOrder.EstimatedDeliveryDate = DateTime.Now.AddHours(Convert.ToInt32(BusinessUnit.ExpectedDeliveryHours)).ToString("dd-MM-yyyy");
                        //}

                        TblExchangeOrder.CreatedDate = _currentDatetime;
                        TblExchangeOrder.CreatedBy = ExchangeOrdersDataModel.CreatedBy;
                        TblExchangeOrder.ModifiedDate = _currentDatetime;
                        TblExchangeOrder.ModifiedBy = ExchangeOrdersDataModel.CreatedBy;
                        TblExchangeOrder.IsActive = true;
                        _ExchangeOrderRepository.Create(TblExchangeOrder);
                        _ExchangeOrderRepository.SaveChanges();
                    }
                    else
                    {
                        resultedRegdNo = "Customer data not found";
                    }
                    
                    #endregion

                    #region add Order trans details
                    //Code for Order tran
                    TblOrderTran TblOrderTran = new TblOrderTran();
                    TblOrderTran.OrderType = Convert.ToInt32(OrderTypeEnum.Exchange);
                    TblOrderTran.ExchangeId = TblExchangeOrder.Id;
                    TblOrderTran.SponsorOrderNumber = TblExchangeOrder.SponsorOrderNumber;
                    TblOrderTran.RegdNo = TblExchangeOrder.RegdNo;
                    TblOrderTran.ExchangePrice = TblExchangeOrder.ExchangePrice;
                    TblOrderTran.Sweetner = TblExchangeOrder.Sweetener;
                    TblOrderTran.IsActive = true;
                    TblOrderTran.CreatedDate = _currentDatetime;
                    TblOrderTran.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                    TblOrderTran.CreatedBy = userId;
                    TblOrderTran.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                    _orderTransRepository.Create(TblOrderTran);
                    _orderTransRepository.SaveChanges();
                    #endregion

                    #region add history
                    //Code for Order history
                    TblExchangeAbbstatusHistory TblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                    TblExchangeAbbstatusHistory.OrderType = Convert.ToInt32(OrderTypeEnum.Exchange);
                    TblExchangeAbbstatusHistory.OrderTransId = TblOrderTran.OrderTransId;

                    TblExchangeAbbstatusHistory.SponsorOrderNumber = TblExchangeOrder.SponsorOrderNumber;
                    TblExchangeAbbstatusHistory.RegdNo = TblExchangeOrder.RegdNo;
                    TblExchangeAbbstatusHistory.CustId = TblCustomerDetails.Id;
                    TblExchangeAbbstatusHistory.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                    TblExchangeAbbstatusHistory.IsActive = true;
                    TblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                    TblExchangeAbbstatusHistory.CreatedBy = userId;
                    _exchangeABBStatusHistoryRepository.Create(TblExchangeAbbstatusHistory);
                    _exchangeABBStatusHistoryRepository.SaveChanges();
                    #endregion

                    #region send notification to whatsapp
                    WhatasappResponseOrderConfirmation whatsAppResponse = new WhatasappResponseOrderConfirmation();

                    WhatsappTemplateExchangeOrderPlaceConfirmation whatsappObjforOrderConfirmation = new WhatsappTemplateExchangeOrderPlaceConfirmation();
                    whatsappObjforOrderConfirmation.userDetails = new UserDetailsOrderConfirmation();
                    whatsappObjforOrderConfirmation.notification = new OrderPlaceWithSelfQCConfirmation();
                    whatsappObjforOrderConfirmation.notification.@params = new OrderPlaceConfirmation();
                    whatsappObjforOrderConfirmation.userDetails.number = TblCustomerDetails.PhoneNumber;
                    whatsappObjforOrderConfirmation.notification.sender = _config.Value.YelloaiSenderNumber;
                    whatsappObjforOrderConfirmation.notification.type = _config.Value.YellowaiMesssaheType;
                    whatsappObjforOrderConfirmation.notification.templateId = NotificationConstants.orderConfirmationForExchange;
                    whatsappObjforOrderConfirmation.notification.@params.CustName = TblCustomerDetails.FirstName + " " + TblCustomerDetails.LastName;
                    whatsappObjforOrderConfirmation.notification.@params.CustomerName = TblCustomerDetails.FirstName + " " + TblCustomerDetails.LastName;
                    whatsappObjforOrderConfirmation.notification.@params.PhoneNumber = TblCustomerDetails.PhoneNumber;
                    whatsappObjforOrderConfirmation.notification.@params.Link = _config.Value.BaseURL + "QCPortal/SelfQC?regdno=" + TblExchangeOrder.RegdNo;

                    TblBrand tblBrand = _brandRepository.GetSingle(x => x.IsActive == true && x.Id == TblExchangeOrder.BrandId);
                    if (tblBrand != null)
                    {
                        whatsappObjforOrderConfirmation.notification.@params.ProductBrand = tblBrand.Name != string.Empty ? tblBrand.Name : string.Empty;
                        ExchangeOrdersDataModel.ProductBrand = tblBrand.Name != string.Empty ? tblBrand.Name : string.Empty;
                    }

                    TblProductCategory tblProductCategory = _ProductCategoryRepository.GetSingle(x => x.IsActive == true && x.Id == ExchangeOrdersDataModel.ProductCategoryId);
                    if (tblProductCategory != null)
                    {
                        whatsappObjforOrderConfirmation.notification.@params.ProdCategory = tblProductCategory.Description != string.Empty ? tblProductCategory.Description : string.Empty;
                        ExchangeOrdersDataModel.ProductCategory = tblProductCategory.Description != string.Empty ? tblProductCategory.Description : string.Empty;
                    }

                    TblProductType tblProductType = _ProductTypeRepository.GetSingle(x => x.IsActive == true && x.Id == TblExchangeOrder.ProductTypeId);
                    if (tblProductType != null)
                    {
                        whatsappObjforOrderConfirmation.notification.@params.ProdType = tblProductType.Description;
                        ExchangeOrdersDataModel.ProductType = tblProductType.Description != string.Empty ? tblProductType.Description : string.Empty;
                    }
                    whatsappObjforOrderConfirmation.notification.@params.RegdNO = TblExchangeOrder.RegdNo.ToString();
                    string urlforwhatsapp = _config.Value.YellowAiUrl;
                    RestResponse responseConfirmation = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(urlforwhatsapp, Method.Post, whatsappObjforOrderConfirmation);
                    //ResponseCode = responseConfirmation.StatusCode.ToString();
                    //WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                    if (responseConfirmation.Content != null && responseConfirmation.StatusCode == HttpStatusCode.OK || responseConfirmation.StatusCode == HttpStatusCode.Accepted)
                    {

                        whatsAppResponse = JsonConvert.DeserializeObject<WhatasappResponseOrderConfirmation>(responseConfirmation.Content);
                        TblWhatsAppMessage whatsapObj = new TblWhatsAppMessage();
                        whatsapObj.TemplateName = NotificationConstants.orderConfirmationForExchange;
                        whatsapObj.IsActive = true;
                        whatsapObj.PhoneNumber = TblCustomerDetails.PhoneNumber;
                        whatsapObj.SendDate = DateTime.Now;
                        whatsapObj.MsgId = whatsAppResponse.msgId;
                        _WhatsAppMessageRepository.Create(whatsapObj);
                        _WhatsAppMessageRepository.SaveChanges();

                    }
                    else
                    {
                        //  string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(productOrderDataContract);
                        //logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", productOrderDataContract.SponsorOrderNumber, JsonObjectForExchangeOrder);
                    }

                    #endregion

                    #region Send mail for order confirmation
                    var filename = "ExchangeMailDeffered.html";
                    var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MailTemplates", filename);
                    string Emailbody = File.ReadAllText(file);
                    Emailbody = Emailbody.Replace("[CustomerName]", TblCustomerDetails.FirstName + " " + TblCustomerDetails.LastName)
                                                        .Replace("[BusinessUnitName]", TblExchangeOrder.CompanyName)
                                                        .Replace("[SponserOrderNumber]", TblExchangeOrder.SponsorOrderNumber)
                                                        .Replace("[CreatedDate]", DateTime.Now.ToString("dd/MM/yyyy"))
                                                        .Replace("[CustName]", TblCustomerDetails.FirstName)
                                                        .Replace("[CustMobile]", TblCustomerDetails.PhoneNumber)
                                                        .Replace("[CustAdd1]", TblCustomerDetails.Address1)
                                                        .Replace("[CustAdd2]", TblCustomerDetails.Address2)
                                                        //.Replace("[State]", productOrderDataContract.State)
                                                        .Replace("[PinCode]", TblCustomerDetails.ZipCode)
                                                        .Replace("[CustCity]", TblCustomerDetails.City)
                                                        .Replace("[ProductCategory]", ExchangeOrdersDataModel.ProductCategory)
                                                        .Replace("[OldProdType]", ExchangeOrdersDataModel.ProductType)
                                                        .Replace("[OldBrand]", ExchangeOrdersDataModel.BrandName)
                                                        //.Replace("[Size]", productType.Size)
                                                        .Replace("[ExchangePrice]", TblExchangeOrder.ExchangePrice.ToString())
                                                        .Replace("[EstimatedDeliveryDate]", TblExchangeOrder.EstimatedDeliveryDate)
                                                        .Replace("[SelfQCLink]", whatsappObjforOrderConfirmation.notification.@params.Link);

                    string subject = TblExchangeOrder.CompanyName + ": Exchange Detail";
                    string too = TblCustomerDetails.Email != string.Empty ? TblCustomerDetails.Email : string.Empty;

                    _mailManager.JetMailSend(too, Emailbody, subject);
                    #endregion
                }
                else
                {
                    resultedRegdNo = "data not found.";
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ExchangeOrdermanager", "AddExchangeOrders", ex);
            }
            return resultedRegdNo;
        }
        #endregion


        #region count space
        static int CountSpaces(string str)
        {
            int spaceCount = 0;

            foreach (char c in str)
            {
                if (c == ' ')
                {
                    spaceCount++;
                }
            }

            return spaceCount;
        }
        #endregion

        #region Diagnose 2.0 Added by VK
        #region Add Multiple exchange orders for D2C through api Added by VK
        /// <summary>
        /// Add Multiple orders through api for D2C
        /// Added by ashwin
        /// </summary>
        /// <param name="multipleExchangeOrdersDataModel"></param>
        /// <returns>responseResult</returns>
        public ResponseResult AddMultipleOrdersV2(MultipleExchangeOrdersDataModel multipleExchangeOrdersDataModel, string username)
        {
            #region Variables Declarations
            ResponseResult responseResult = new ResponseResult();
            OrderBasicDetailsDataViewModel orderBasicDetailsDataViewModel = new OrderBasicDetailsDataViewModel();
            responseResult.message = string.Empty;
            int customerDetailsId = 0;
            //in case of estimated hours not found in database
            //int DefaultEstimatedhours = 48 ;
            Login? login = null;
            TblBusinessUnit? businessUnit = null;
            TblBusinessPartner? tblBusinessPartner = null;
            List<OrderResult> orderResult = new List<OrderResult>();
            TblCustomerDetail tblCustomerDetail = new TblCustomerDetail();
            #endregion
            //added by shashi for inserting userid into questioners 
            int userId = 3;
            try
            {
                if (multipleExchangeOrdersDataModel != null && multipleExchangeOrdersDataModel.CustomerDetailViewModel != null && multipleExchangeOrdersDataModel.productDetailsDataViewModels.Count > 0)
                {
                    #region bind login User details 
                    login = _loginRepository.GetBULoginByUsername(username);
                    if (login != null && login.BusinessPartnerId != null)
                    {
                        orderBasicDetailsDataViewModel.BusinessPartnerId = (int)login.BusinessPartnerId;
                        orderBasicDetailsDataViewModel.BUId = login.SponsorId != null ? Convert.ToInt32(login.SponsorId) : 0;
                        if (login != null && login.SponsorId != null)
                        {
                            businessUnit = _businessUnitRepository.Getbyid(login.SponsorId);
                            if (businessUnit != null)
                            {
                                orderBasicDetailsDataViewModel.LoginID = businessUnit.BusinessUnitId;
                                orderBasicDetailsDataViewModel.CompanyName = businessUnit.Name;
                                orderBasicDetailsDataViewModel.EstimatedDeliveryDate = DateTime.Now.AddHours(Convert.ToInt32(businessUnit.ExpectedDeliveryHours)).ToString("dd-MMM-yyyy");
                                orderBasicDetailsDataViewModel.BUId = businessUnit.BusinessUnitId;

                                tblBusinessPartner = _businessPartnerRepository.GetById(login.BusinessPartnerId);
                                if (tblBusinessPartner != null)
                                {
                                    orderBasicDetailsDataViewModel.StoreCode = tblBusinessPartner.StoreCode;
                                    orderBasicDetailsDataViewModel.IsDefferedSettlement = tblBusinessPartner.IsDefferedSettlement == null ? false : Convert.ToBoolean(tblBusinessPartner.IsDefferedSettlement);
                                }
                                else
                                {
                                    responseResult.message = "No associated store found for this order";
                                    responseResult.Status = false;
                                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                                    return responseResult;
                                }
                            }
                            else
                            {
                                responseResult.message = "No Bussiness Unit found for the requested user";
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.BadRequest;
                                return responseResult;
                            }
                        }
                        else
                        {
                            responseResult.message = "Sponsor Id not found for login user";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            return responseResult;
                        }
                    }
                    else
                    {
                        responseResult.message = "Details not found for login user";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        return responseResult;
                    }
                    #endregion

                    #region Add Customer Details 
                    if (multipleExchangeOrdersDataModel.CustomerDetailViewModel != null && multipleExchangeOrdersDataModel.CustomerDetailViewModel.Id == 0)
                    {
                        CustomerDetailViewModel customerDetailViewModel = multipleExchangeOrdersDataModel.CustomerDetailViewModel;
                        tblCustomerDetail = ManageCustomerDetails(customerDetailViewModel);
                        if (tblCustomerDetail != null && tblCustomerDetail.Id > 0)
                        {
                            customerDetailsId = tblCustomerDetail.Id;
                            orderBasicDetailsDataViewModel.CustomerMobileNumber = customerDetailViewModel?.PhoneNumber;
                            orderBasicDetailsDataViewModel.CustomerDetailsId = tblCustomerDetail.Id;
                        }
                        else
                        {
                            responseResult.message = "Error occurs while mapping customer details";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            return responseResult;
                        }
                    }
                    else if (multipleExchangeOrdersDataModel.CustomerDetailViewModel != null && multipleExchangeOrdersDataModel.CustomerDetailViewModel.Id > 0)
                    {
                        customerDetailsId = multipleExchangeOrdersDataModel.CustomerDetailViewModel.Id;
                        //tblCustomerDetail = _customerDetailsRepository.GetSingle(x => x.IsActive == true && x.Id == multipleExchangeOrdersDataModel.CustomerDetailViewModel.Id);
                        tblCustomerDetail = _customerDetailsRepository.GetCustDetails(multipleExchangeOrdersDataModel?.CustomerDetailViewModel?.Id);
                        orderBasicDetailsDataViewModel.CustomerMobileNumber = multipleExchangeOrdersDataModel?.CustomerDetailViewModel?.PhoneNumber;
                    }
                    else
                    {
                        responseResult.message = "Customer details is needed";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        return responseResult;
                    }
                    #endregion

                    #region Add Products Details
                    if (multipleExchangeOrdersDataModel.productDetailsDataViewModels.Count > 0)
                    {
                        #region Manage Product Details
                        orderResult = ManageProductDetails(multipleExchangeOrdersDataModel.productDetailsDataViewModels, orderBasicDetailsDataViewModel, tblCustomerDetail);
                        #endregion

                        if (orderResult.Count > 0)
                        {
                            responseResult.Data = orderResult;
                            responseResult.Status = true;
                            responseResult.Status_Code = HttpStatusCode.OK;
                            responseResult.message = "Success";
                            return responseResult;
                        }
                        else
                        {
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            responseResult.message = "Registration failed";
                        }
                    }
                    else
                    {
                        responseResult.message = "Products details should not be null";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        return responseResult;
                    }
                    #endregion
                }
                else
                {
                    responseResult.message = "Request object is null";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("ExchangeOrderManager", "AddMultipleOrders", ex);
            }
            return responseResult;
        }
        #endregion

        #region Manage Customer Details
        public TblCustomerDetail ManageCustomerDetails(CustomerDetailViewModel customerDetailViewModel)
        {
            int customerId = 0;
            TblCustomerDetail? tblCustomerDetail = null;
            try
            {
                if (customerDetailViewModel != null)
                {
                    tblCustomerDetail = _mapper.Map<CustomerDetailViewModel, TblCustomerDetail>(customerDetailViewModel);
                    if (tblCustomerDetail != null)
                    {
                        tblCustomerDetail.CreatedDate = _currentDatetime;
                        tblCustomerDetail.IsActive = true;
                        _customerDetailsRepository.Create(tblCustomerDetail);
                        int custId = _customerDetailsRepository.SaveChanges();
                        if (custId > 0)
                        {
                            customerId = tblCustomerDetail.Id;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ExchangeOrderManager", "ManageCustomerDetails", ex);
            }
            return tblCustomerDetail;
        }
        #endregion

        #region Manage Product Details
        public List<OrderResult> ManageProductDetails(List<ProductDetailsDataViewModel> prodDetailsDataVMList, OrderBasicDetailsDataViewModel orderBasicDetailsDataViewModel,TblCustomerDetail tblCustomerDetail)
        {
            int result = 0;
            int customerDetailsId = 0;
            List<OrderResult> orderResult = new List<OrderResult>();
            //added by shashi for inserting userid into questioners 
            int userId = 3;
            try
            {
                int D2CBuid = Convert.ToInt32(BussinessUnitEnum.D2C);
                customerDetailsId = orderBasicDetailsDataViewModel?.CustomerDetailsId ?? 0;
                if (prodDetailsDataVMList != null && prodDetailsDataVMList.Count > 0)
                {
                    #region loop for orders insert
                    foreach (var item in prodDetailsDataVMList)
                    {
                        OrderResult orderResult1 = new OrderResult();
                        orderResult1.customerDetailsId = customerDetailsId;
                        ProductDetailsDataViewModel productDetailsDataViewModels = new ProductDetailsDataViewModel();
                        productDetailsDataViewModels = item;
                        string SponsorOrderNumber = string.Empty;
                        productDetailsDataViewModels.SponsorOrderNumber = null;

                        #region add sponsor order number
                        if (string.IsNullOrEmpty(productDetailsDataViewModels.SponsorOrderNumber))
                            productDetailsDataViewModels.SponsorOrderNumber = "API" + UniqueString.RandomNumberByLength(9);

                        else if (orderBasicDetailsDataViewModel?.BUId == D2CBuid)
                        {
                            if (string.IsNullOrEmpty(productDetailsDataViewModels.SponsorOrderNumber))
                                productDetailsDataViewModels.SponsorOrderNumber = "D2C" + UniqueString.RandomNumberByLength(9);
                        }
                        //check sponsor number in database
                        if (productDetailsDataViewModels.SponsorOrderNumber != null)
                        {
                            bool check = false;
                            bool check1 = false;
                            check = _ExchangeOrderRepository.CheckSponsorOrderNumber(productDetailsDataViewModels.SponsorOrderNumber);
                            if (check == true)
                            {
                                while (check == true)
                                {
                                    if (string.IsNullOrEmpty(productDetailsDataViewModels.SponsorOrderNumber))
                                        productDetailsDataViewModels.SponsorOrderNumber = "API" + UniqueString.RandomNumberByLength(9);

                                    else if (orderBasicDetailsDataViewModel?.BUId == D2CBuid)
                                    {
                                        if (string.IsNullOrEmpty(productDetailsDataViewModels.SponsorOrderNumber))
                                            productDetailsDataViewModels.SponsorOrderNumber = "D2C" + UniqueString.RandomNumberByLength(9);
                                    }
                                    check1 = _ExchangeOrderRepository.CheckSponsorOrderNumber(productDetailsDataViewModels.SponsorOrderNumber);

                                    check = check1;
                                }
                            }
                        }
                       

                        #region add regdno
                        if (orderBasicDetailsDataViewModel.StoreCode != null)
                        {
                            productDetailsDataViewModels.RegdNo = "E" + UniqueString.RandomNumberByLength(7);
                            productDetailsDataViewModels.UploadDateTime = _currentDatetime.ToString();
                            orderResult1.orderRegdNo = productDetailsDataViewModels.RegdNo;
                        }
                        #endregion

                        #region insert tblexchangeorder,tblordertrans,tblaxchangeabbhistory
                        if (orderBasicDetailsDataViewModel != null && orderBasicDetailsDataViewModel.LoginID > 0)
                        {
                            int resultexchangeorder = 0;
                            int isquestionerinsert = 0;
                            bool flag = true;

                            #region Save Data in tblExchangeOrder
                            TblExchangeOrder tblExchangeOrder; // = new TblExchangeOrder();
                            tblExchangeOrder = ManageExchangeOrderDetails(productDetailsDataViewModels, orderBasicDetailsDataViewModel);
                            if (tblExchangeOrder != null && tblExchangeOrder.Id > 0)
                            {
                                orderResult1.OrderId = tblExchangeOrder.Id;
                                orderResult1.productId = tblExchangeOrder.ProductTypeId;
                                productDetailsDataViewModels.ProductTypeId = tblExchangeOrder?.ProductTypeId??0;
                                resultexchangeorder = 1;
                            }
                            #endregion

                            #region Save Data into the tblOrderTrans

                           

                            if (resultexchangeorder > 0 && orderResult1.OrderId > 0)
                            {
                                #region add data into tblordertrans
                                TblOrderTran tblOrderTran = new TblOrderTran();
                                OrderTransactionViewModel orderTransactionViewModel = new OrderTransactionViewModel();

                                orderTransactionViewModel.OrderType = Convert.ToInt32(LoVEnum.Exchange);
                                orderTransactionViewModel.ExchangeId = orderResult1.OrderId;
                                orderTransactionViewModel.SponsorOrderNumber = productDetailsDataViewModels.SponsorOrderNumber;
                                orderTransactionViewModel.RegdNo = productDetailsDataViewModels.RegdNo;
                                orderTransactionViewModel.ExchangePrice = tblExchangeOrder.ExchangePrice;
                                orderTransactionViewModel.Sweetner = tblExchangeOrder.Sweetener;
                                orderTransactionViewModel.IsActive = true;
                                orderTransactionViewModel.CreatedDate = _currentDatetime;

                                int ordertransresult = _orderTransactionManager.ManageOrderTransaction(orderTransactionViewModel);

                                if (ordertransresult > 0)
                                {
                                    orderResult1.ordertransId = ordertransresult;
                                }
                                #endregion

                                #region add history for order created into tblExchangeAbbstatusHistory
                                TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();
                                tblExchangeAbbstatusHistory.CustId = customerDetailsId;
                                tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                tblExchangeAbbstatusHistory.Comment = "D2C order by WebApi";
                                tblExchangeAbbstatusHistory.IsActive = true;
                                tblExchangeAbbstatusHistory.CreatedBy = 3;
                                tblExchangeAbbstatusHistory.RegdNo = tblExchangeOrder.RegdNo;
                                tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeOrder.SponsorOrderNumber;
                                tblExchangeAbbstatusHistory.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                                tblExchangeAbbstatusHistory.OrderTransId = orderResult1.ordertransId;
                                _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                                int exchangabbhistory = _exchangeABBStatusHistoryRepository.SaveChanges();
                                if (exchangabbhistory > 0)
                                {
                                    orderResult1.isHistoryCreated = true;
                                }
                                #endregion

                                #region Whatsapp notification and selfQC link send to customer whatsapp
                                #region code to send selfqc link on whatsappNotification
                                int result1 = SendSelfQCLinkByWA(productDetailsDataViewModels, orderBasicDetailsDataViewModel, tblCustomerDetail);
                                if (result1 > 0)
                                {
                                    orderResult1.isWhatsAppNotificationSend = true;
                                }
                                #endregion

                                #region Code to Send Mail to Customer
                                string baseurl = _config.Value.BaseURL + "QCPortal/SelfQC?regdno=" + productDetailsDataViewModels.RegdNo;
                                if (tblCustomerDetail != null)
                                {
                                    if (!string.IsNullOrEmpty(tblCustomerDetail.Email) && !string.IsNullOrEmpty(baseurl)
                                        && !string.IsNullOrEmpty(productDetailsDataViewModels.productDescription)
                                        && !string.IsNullOrEmpty(tblCustomerDetail.FirstName))
                                    {
                                        tblCustomerDetail.Email.Trim();
                                        SendABBEmail(tblCustomerDetail.Email, tblCustomerDetail.FirstName, productDetailsDataViewModels.productDescription, baseurl);
                                    }
                                }
                                #endregion
                                #endregion

                                #region Questioner's insert
                                productDetailsDataViewModels.questionerViewModel.OrderTrandId = Convert.ToInt32(orderResult1.ordertransId);
                                QuestionerViewModel questionerViewModel = new QuestionerViewModel();
                                List<QCRatingViewModel> qCRatingViewModels = new List<QCRatingViewModel>();
                                questionerViewModel = productDetailsDataViewModels.questionerViewModel;
                                qCRatingViewModels = productDetailsDataViewModels.qCRatingViewModels;

                                if (questionerViewModel != null && qCRatingViewModels.Count > 0)
                                {
                                    isquestionerinsert = _qCCommentManager.saveAndSubmit(questionerViewModel, qCRatingViewModels, userId, flag);
                                    if (isquestionerinsert > 0)
                                    {
                                        orderResult1.isQuestionersdone = true;
                                    }
                                    else
                                    {
                                        orderResult1.isQuestionersdone = false;
                                    }
                                }
                                #endregion
                            }
                            orderResult.Add(orderResult1);
                            #endregion
                        }
                        #endregion

                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ExchangeOrderManager", "ManageProductDetails", ex);
            }
            return orderResult;
        }

        #endregion

        #region Manage Exchange Order Details
        public TblExchangeOrder ManageExchangeOrderDetails(ProductDetailsDataViewModel productDetailsDataViewModel, OrderBasicDetailsDataViewModel orderBasicDetailsDataViewModel)
        {
            TblExchangeOrder? tblExchangeOrder = null;
            try
            {
                if (productDetailsDataViewModel != null && orderBasicDetailsDataViewModel != null)
                {
                    tblExchangeOrder = _mapper.Map<ProductDetailsDataViewModel, TblExchangeOrder>(productDetailsDataViewModel);
                    if (tblExchangeOrder != null)
                    {
                        tblExchangeOrder.LoginId = orderBasicDetailsDataViewModel.LoginID;
                        tblExchangeOrder.StoreCode = orderBasicDetailsDataViewModel.StoreCode;
                        tblExchangeOrder.IsDefferedSettlement = orderBasicDetailsDataViewModel.IsDefferedSettlement;
                        tblExchangeOrder.EstimatedDeliveryDate = orderBasicDetailsDataViewModel.EstimatedDeliveryDate;
                        tblExchangeOrder.BusinessPartnerId = orderBasicDetailsDataViewModel.BusinessPartnerId;
                        tblExchangeOrder.CompanyName = orderBasicDetailsDataViewModel.CompanyName;
                        tblExchangeOrder.CustomerDetailsId = orderBasicDetailsDataViewModel.CustomerDetailsId;
                        tblExchangeOrder.IsDtoC = true;
                        tblExchangeOrder.IsActive = true;
                        tblExchangeOrder.CreatedDate = _currentDatetime;
                        tblExchangeOrder.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                        tblExchangeOrder.OrderStatus = EnumHelper.DescriptionAttr(OrderStatusEnum.OrderCreatedbySponsor);
                        tblExchangeOrder.ProductTechnologyId = productDetailsDataViewModel?.questionerViewModel?.ProductTechnologyId;
                        tblExchangeOrder.IsDiagnoseV2 = true;

                        if (productDetailsDataViewModel?.questionerViewModel?.FinalPrice > 0)
                        {
                            tblExchangeOrder.ExchangePrice = Convert.ToDecimal(productDetailsDataViewModel.questionerViewModel.FinalPrice);
                        }
                        else if (productDetailsDataViewModel?.questionerViewModel?.NonWorkingPrice > 0)
                        {
                            tblExchangeOrder.ExchangePrice = Convert.ToDecimal(productDetailsDataViewModel.questionerViewModel.NonWorkingPrice);
                        }
                        _ExchangeOrderRepository.Create(tblExchangeOrder);
                        _ExchangeOrderRepository.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ExchangeOrderManager", "ManageExchangeOrderDetails", ex);
            }
            return tblExchangeOrder;
        }
        #endregion

        #region Code to send selfqc link on whatsappNotification
        public int SendSelfQCLinkByWA(ProductDetailsDataViewModel productDetailsDataViewModels, OrderBasicDetailsDataViewModel orderBasicDetailsDataViewModel, TblCustomerDetail tblCustomerDetail)
        {
            int result = 0;
            try
            {
                #region code to send selfqc link on whatsappNotification
                WhatasappResponseOrderConfirmation whatasappResponse = new WhatasappResponseOrderConfirmation();
                string baseurl = _config.Value.BaseURL + "QCPortal/SelfQC?regdno=" + productDetailsDataViewModels.RegdNo;
                TblWhatsAppMessage? tblWhatsAppMessage = null;
                WhatsappTemplateOrderConfirmation whatsappObj = new WhatsappTemplateOrderConfirmation();

                whatsappObj.userDetails = new UserDetailsOrderConfirmation();
                whatsappObj.notification = new SelfQCOrderConfirmation();
                whatsappObj.notification.@params = new URLOrderConfirmation();
                whatsappObj.userDetails.number = orderBasicDetailsDataViewModel.CustomerMobileNumber;
                whatsappObj.notification.sender = _config.Value.YelloaiSenderNumber;
                whatsappObj.notification.type = _config.Value.YellowaiMesssaheType;
                whatsappObj.notification.templateId = NotificationConstants.orderConfirmationForExchangeUpdated;
                if (tblCustomerDetail != null)
                {
                    whatsappObj.notification.@params.CustName = tblCustomerDetail.FirstName;
                    whatsappObj.notification.@params.Address = tblCustomerDetail.Address1 + " " + tblCustomerDetail.Address2;
                    whatsappObj.notification.@params.Email = tblCustomerDetail.Email != null ? tblCustomerDetail.Email : string.Empty;
                    whatsappObj.notification.@params.PhoneNumber = tblCustomerDetail.PhoneNumber != null ? tblCustomerDetail.PhoneNumber : string.Empty;
                    whatsappObj.notification.@params.CustomerName = tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName;
                }
                whatsappObj.notification.@params.Link = baseurl;

                TblProductType? tblProductType = _ProductTypeRepository.GetCatTypebytypeid(productDetailsDataViewModels.ProductTypeId);
                if (tblProductType != null && tblProductType.ProductCatId > 0)
                {
                    TblProductCategory? tblProductCategory = tblProductType?.ProductCat;
                    whatsappObj.notification.@params.ProdCategory = tblProductCategory != null ? tblProductCategory.Description : string.Empty;
                    productDetailsDataViewModels.productDescription = tblProductCategory != null ? tblProductCategory.Description : string.Empty;
                    whatsappObj.notification.@params.ProdType = tblProductType?.Description != string.Empty ? tblProductType?.Description : string.Empty;
                }
                whatsappObj.notification.@params.RegdNO = productDetailsDataViewModels.RegdNo;
                string? url = _config.Value.YellowAiUrl;
                RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                if (response.Content != null)
                {
                    whatasappResponse = JsonConvert.DeserializeObject<WhatasappResponseOrderConfirmation>(response.Content);
                    tblWhatsAppMessage = new TblWhatsAppMessage();
                    tblWhatsAppMessage.TemplateName = NotificationConstants.orderConfirmationForExchangeUpdated;
                    tblWhatsAppMessage.IsActive = true;
                    tblWhatsAppMessage.PhoneNumber = orderBasicDetailsDataViewModel.CustomerMobileNumber;
                    tblWhatsAppMessage.SendDate = DateTime.Now;
                    tblWhatsAppMessage.MsgId = whatasappResponse?.msgId;
                    _WhatsAppMessageRepository.Create(tblWhatsAppMessage);
                    result = _WhatsAppMessageRepository.SaveChanges();
                }
                #endregion
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ExchangeOrderManager", "SendSelfQCLink", ex);
            }
            return result;
        }
        #endregion
    }
}

#endregion