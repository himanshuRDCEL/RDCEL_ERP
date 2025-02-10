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
using Microsoft.AspNetCore.Mvc;
using RDCELERP.BAL.Enum;
using RDCELERP.Model.VoucherRedemption;
using Org.BouncyCastle.Crypto;
using static QRCoder.PayloadGenerator;
using System.Data;
using Microsoft.Data.SqlClient;
using DocumentFormat.OpenXml.Wordprocessing;

namespace RDCELERP.BAL.MasterManager
{
    public class VoucherRedemptionManager : IVoucherRedemptionManager
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
        IMapper _mapper;
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
        ADOHelper _adoHelper;
        IOptions<ConnectionStrings> _dbConnectionString;
        IVoucherRepository _voucherVerificationRepository;
        IVoucherStatusRepository _voucherStatusRepository;


        #endregion

        #region Constructor
        public VoucherRedemptionManager(IOrderTransactionManager orderTransactionManager, IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository, ICommonManager commonManager, IQCCommentManager qCCommentManager, ICustomerDetailsRepository customerDetailsRepository, Digi2l_DevContext context, INotificationManager notificationManager, IOptions<ApplicationSettings> config, IEVCPriceMasterRepository eVCPriceMAster, IOrderTransRepository orderTransRepository, IOrderQCRepository orderQCRepository, IBusinessPartnerRepository businessPartnerRepository, IBrandRepository brandRepository, IPriceMasterRepository priceMasterRepository, ILoginRepository loginRepository, IBusinessUnitRepository businessUnitRepository, IExchangeOrderStatusRepository ExchangeOrderStatusRepository, IExchangeOrderRepository ExchangeOrderRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging, IWhatsappNotificationManager whatsappNotificationManager, IWhatsAppMessageRepository whatsAppMessageRepository, IProductTypeRepository productTypeRepository, IProductCategoryRepository productCategoryRepository, IModelNumberRepository modelNumberRepository, IOrderTransRepository orderTransactionRepository, IAreaLocalityRepository areaLocalityRepository, IUserRepository userRepository, IMailManager mailManager, ILogisticsRepository logisticsRepository, IProductConditionLabelRepository productConditionLabelRepository, IWalletTransactionRepository walletTransactionRepository, ADOHelper adoHelper, IOptions<ConnectionStrings> dbConnectionString, IVoucherRepository voucherVerificationRepository, IVoucherStatusRepository voucherStatusRepository)
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
            _adoHelper = adoHelper;
            _dbConnectionString = dbConnectionString;
            _voucherVerificationRepository = voucherVerificationRepository;
            _voucherStatusRepository = voucherStatusRepository;
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

        #region verify voucher 

        public VoucherDataContract VerifyVoucherCode(VoucherDataContract voucherDataContract)
        {
            string empt = string.Empty;
            //VerifyVoucherResult verifyVoucherResult = new VerifyVoucherResult();
            VoucherDataContract voucherDataContractresult = new VoucherDataContract();
            voucherDataContractresult.verifyVoucherResult = new VerifyVoucherResult();
            voucherDataContractresult.ExchangeOrderDataContract = new ExchangeOrderDataContract();
            voucherDataContractresult.verifyVoucherResult.isVerified= true;
            //verifyVoucherResult.isVerified = false;
            voucherDataContract.verifyVoucherResult = new VerifyVoucherResult();
            //ExchangeOrderDataContract exchangeOrderData = new ExchangeOrderDataContract();
            DateTime _dateTime = DateTime.Now;
            try
            {
                if (voucherDataContract != null)
                {
                    TblExchangeOrder exchangeOrder = _ExchangeOrderRepository.GetSingle(x => x.VoucherCode == voucherDataContract.VoucherCode && x.IsActive == true);
                    if (exchangeOrder != null)
                    {
                        #region bind exchange data
                        //voucherDataContractresult.ExchangeOrderDataContract = new ExchangeOrderDataContract();
                        //voucherDataContractresult.ExchangeOrderDataContract.ProductTypeId = Convert.ToInt32(exchangeOrder.ProductTypeId);
                        //voucherDataContractresult.ExchangeOrderDataContract.Id = Convert.ToInt32(exchangeOrder.Id);
                        //voucherDataContractresult.ExchangeOrderDataContract.ProductCondition = exchangeOrder.ProductCondition;
                        //voucherDataContractresult.ExchangeOrderDataContract.ExchangePrice =Convert.ToDecimal(exchangeOrder.ExchangePrice);
                        
                        //if (Convert.ToInt32(exchangeOrder.NewProductCategoryId) > 0)
                        //{
                        //    voucherDataContractresult.ExchangeOrderDataContract.NewProductCategoryId = Convert.ToInt32(exchangeOrder.NewProductCategoryId);
                        //}
                        //if (Convert.ToInt32(exchangeOrder.NewProductTypeId) > 0)
                        //{
                        //    voucherDataContractresult.ExchangeOrderDataContract.NewProductCategoryTypeId = Convert.ToInt32(exchangeOrder.NewProductTypeId);
                        //}
                        //if (Convert.ToInt32(exchangeOrder.ModelNumberId) >0)
                        //{
                        //    voucherDataContractresult.ExchangeOrderDataContract.ModelNumberId = Convert.ToInt32(exchangeOrder.ModelNumberId);
                        //}
                        //voucherDataContract.ExchangeOrderDataContract.NewProductCategoryId =Convert.ToInt32(exchangeOrder.NewProductCategoryId);
                        #endregion

                        if (voucherDataContract.loginUserDetailsforVoucher!=null && exchangeOrder.BusinessUnitId == voucherDataContract.loginUserDetailsforVoucher.businessUnitId)
                        {
                            TblBusinessPartner businessPartnerObj = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId == exchangeOrder.BusinessPartnerId);
                            if (businessPartnerObj != null)
                            {
                                if (businessPartnerObj.VoucherType != null)
                                {
                                    if (businessPartnerObj.VoucherType == Convert.ToInt32(VoucherStatusEnum.Discount))
                                    {
                                        if (exchangeOrder.IsVoucherused == false)
                                        {
                                            if (exchangeOrder.VoucherCodeExpDate >= _dateTime)
                                            {
                                                TblCustomerDetail customerObj = _customerDetailsRepository.GetSingle(x =>  x.Id == exchangeOrder.CustomerDetailsId);
                                                if (customerObj != null)
                                                {
                                                    if (customerObj.PhoneNumber == voucherDataContract.PhoneNumber)
                                                    {
                                                        voucherDataContract.verifyVoucherResult.isVerified = true;
                                                        voucherDataContract.verifyVoucherResult.responseMesage = "success";
                                                        #region bind customer details

                                                        //voucherDataContractresult.CustomerId = customerObj.Id;
                                                        //voucherDataContractresult.ExchangeOrderDataContract.CustomerDetailsId = customerObj.Id!=null? customerObj.Id:0;
                                                        //voucherDataContractresult.ExchangeOrderDataContract.FirstName = customerObj.FirstName!=string.Empty? customerObj.FirstName:string.Empty;
                                                        //voucherDataContractresult.ExchangeOrderDataContract.LastName = customerObj.LastName!=string.Empty? customerObj.LastName:string.Empty;
                                                        //voucherDataContractresult.ExchangeOrderDataContract.ZipCode = customerObj.ZipCode != string.Empty ? customerObj.ZipCode : string.Empty ;
                                                        //voucherDataContractresult.ExchangeOrderDataContract.Email = customerObj.Email != string.Empty ? customerObj.Email : string.Empty;
                                                        //voucherDataContractresult.ExchangeOrderDataContract.Address1 = customerObj.Address1 != string.Empty ? customerObj.Address1 : string.Empty ;
                                                        //voucherDataContractresult.ExchangeOrderDataContract.Address2 = customerObj.Address2 != string.Empty ? customerObj.Address2 : string.Empty;
                                                        //voucherDataContractresult.ExchangeOrderDataContract.CityName = customerObj.City != string.Empty ? customerObj.City : string.Empty;

                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        voucherDataContract.verifyVoucherResult.isVerified = false;
                                                        voucherDataContract.verifyVoucherResult.responseMesage = "Phone number is not correct";
                                                    }
                                                }
                                                else
                                                {
                                                    voucherDataContract.verifyVoucherResult.isVerified = false;
                                                    voucherDataContract.verifyVoucherResult.responseMesage = "Customer details not found";
                                                }
                                            }
                                            else
                                            {
                                                voucherDataContract.verifyVoucherResult.isVerified = false;
                                                voucherDataContract.verifyVoucherResult.responseMesage = "Voucher already expired ";
                                            }
                                        }
                                        else
                                        {
                                            voucherDataContract.verifyVoucherResult.isVerified = false;

                                            voucherDataContract.verifyVoucherResult.responseMesage = "Voucher is already used";
                                        }
                                    }
                                    else
                                    {
                                        voucherDataContract.verifyVoucherResult.isVerified = false;

                                        voucherDataContract.verifyVoucherResult.responseMesage = "Voucher is not discount type cannot redeem voucher";
                                    }

                                }
                                else
                                {
                                    voucherDataContract.verifyVoucherResult.isVerified = false;

                                    voucherDataContract.verifyVoucherResult.responseMesage = "Voucher type not defined in exchange order";
                                }
                            }
                            else
                            {
                                voucherDataContract.verifyVoucherResult.isVerified = false;
                                voucherDataContract.verifyVoucherResult.responseMesage = "Store Details are not found voucher";
                            }
                        }
                        else
                        {
                            voucherDataContract.verifyVoucherResult.isVerified = false;
                            voucherDataContract.verifyVoucherResult.responseMesage = "you are not eligible to validate this voucher";
                        }

                    }
                    else
                    {
                        voucherDataContract.verifyVoucherResult.isVerified = false;

                        voucherDataContract.verifyVoucherResult.responseMesage = "Voucher code is not found";
                    }
                }
                else
                {
                    voucherDataContract.verifyVoucherResult.isVerified = false;

                    voucherDataContract.verifyVoucherResult.responseMesage = "Voucher data not found";
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VoucherRedemptionManager", "VerifyVoucherCode", ex);
            }

            //return Json(exchangeOrderData, JsonRequestBehavior.AllowGet);
            return voucherDataContract;
        }
        #endregion

        #region Method to get the exchange order id
        /// <summary>
        /// Method to get the exchange order id
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ExchangeOrderDataContract GetExchangeOrderDCByVoucherCode(string vcode, string custphone)
        {
            ExchangeOrderDataContract? exchangeOrderDC = null;
            
            DataTable dt = new DataTable();
            TblExchangeOrder exchangeObj = null;
            try
            {
                if (vcode !=string.Empty && custphone != string.Empty)
                {
                    #region call sp for voucher details

                    string dbConnectionString = _dbConnectionString.Value.Digi2l_DevContext.ToString();

                    SqlParameter[] sqlParam =  {
                        new SqlParameter("@VoucherCode", vcode),
                        new SqlParameter("@CustPhoneNumber", custphone)

                        };
                    dt = _adoHelper.ExecuteDataTable("sp_GetExchangeOrderDCByVoucherCode", dbConnectionString, sqlParam);

                    List<TblExchangeOrder>? list = null;
                    //List<TblExchangeOrder>? exchangeObj = null;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string JSONresult;
                        JSONresult = JsonConvert.SerializeObject(dt);
                        list = JsonConvert.DeserializeObject<List<TblExchangeOrder>>(JSONresult);

                        if (list.Count > 0)
                        {
                            exchangeObj = list != null && list.Count > 0 ? list[0] : null;
                        }
                    }
                    #endregion

                    #region data mapping 
                    if (exchangeObj != null)
                    {
                        #region old product details mapping
                        exchangeOrderDC = _mapper.Map<TblExchangeOrder, ExchangeOrderDataContract>(exchangeObj);
                        TblCustomerDetail custObj = _customerDetailsRepository.GetSingle(x => x.Id == exchangeObj.CustomerDetailsId);
                        if (exchangeOrderDC != null && custObj != null)
                        {
                            exchangeOrderDC.CustomerDetailsId = custObj.Id;
                            exchangeOrderDC.FirstName = custObj.FirstName;
                            exchangeOrderDC.LastName = custObj.LastName;
                            exchangeOrderDC.ZipCode = custObj.ZipCode;
                            exchangeOrderDC.Address1 = custObj.Address1;
                            exchangeOrderDC.Address2 = custObj.Address2;
                            exchangeOrderDC.City = custObj.City;
                            exchangeOrderDC.Email = custObj.Email;
                            exchangeOrderDC.PhoneNumber = custObj.PhoneNumber;


                            // new Order details
                            if (exchangeObj.NewProductCategoryId != null)
                            {
                                exchangeOrderDC.NewProductCategoryTypeId = (int)exchangeObj.NewProductTypeId;
                                if (exchangeObj.ModelNumberId != null)
                                {
                                    exchangeOrderDC.ModelNumberId = (int)exchangeObj.ModelNumberId;
                                }
                            }


                        }
                        exchangeOrderDC.ExchangePrice =Convert.ToDecimal( exchangeObj.ExchangePrice);

                        if (exchangeObj.ProductCondition != null)
                        {
                            if (exchangeObj.ProductCondition == "Excellent")
                            {
                                exchangeOrderDC.QualityCheckValue = 1;
                                exchangeOrderDC.ProductCondition = "Excellent";
                            }
                            else if (exchangeObj.ProductCondition == "Good")
                            {
                                exchangeOrderDC.QualityCheckValue = 2;
                                exchangeOrderDC.ProductCondition = "Good";
                            }
                            else if (exchangeObj.ProductCondition == "Average")
                            {
                                exchangeOrderDC.QualityCheckValue = 3;
                                exchangeOrderDC.ProductCondition = "Average";
                            }
                            else if (exchangeObj.ProductCondition == "Not Working")
                            {
                                exchangeOrderDC.QualityCheckValue = 4;
                                exchangeOrderDC.ProductCondition = "Not Working";
                            }

                        }
                        //Code to fill prod cat
                        TblProductType prodTypeObj = _ProductTypeRepository.GetSingle(x => x.Id == exchangeObj.ProductTypeId);

                        TblProductCategory prodCatObj = prodTypeObj != null && prodTypeObj.ProductCatId > 0 ? _ProductCategoryRepository.GetSingle(x => x.Id == prodTypeObj.ProductCatId) : null;
                        exchangeOrderDC.ProductCategoryId = prodCatObj!=null || prodCatObj.Id>0? prodCatObj.Id:0;
                        exchangeOrderDC.ProductCategory = prodCatObj != null || prodCatObj.Description !=string.Empty ? prodCatObj.Description : string.Empty;
                        exchangeOrderDC.ProductType = prodTypeObj != null || prodTypeObj.Description != string.Empty ? prodTypeObj.Description : string.Empty;
                        if (exchangeObj!=null && exchangeObj.BrandId!=null && exchangeObj.BrandId>0)
                        {
                            TblBrand tblBrand = _brandRepository.GetSingle(x => x.Id == exchangeObj.BrandId);
                            exchangeOrderDC.BrandName = tblBrand != null ? tblBrand.Name : string.Empty;
                        }
                        exchangeOrderDC.Response = "Success";
                        #endregion

                        #region new productdetails

                        #endregion
                    }
                    #endregion



                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VoucherRedemptionManager", "GetExchangeOrderDCByVoucherCode", ex);
            }
            return exchangeOrderDC;
        }
        #endregion

        #region Save Data for voucher redemption 
        public int AddVouchertoDB(ExchangeOrderDataContract voucherData)
        {
            TblExchangeOrder exchangeObj = new TblExchangeOrder();
            string WhatssAppStatusEnum = string.Empty;
            string ResponseCode = string.Empty;
            TblBusinessPartner businessPartnerObj = null;

            WhatasappResponse whatssappresponseDC = null;
            int result = 0;
            bool isVoucherProccessed = false;
            string responseforWhatasapp = string.Empty;
            decimal sweetner = 0;
            string voucherStatusName = null;
            string unique = "_C";
            try
            {
                TblVoucherVerfication voucherVerfication = _voucherVerificationRepository.GetSingle(x => x.ExchangeOrderId == voucherData.Id && x.NewProductCategoryId == voucherData.NewProductCategoryId);
                TblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == voucherData.BusinessUnitId);
                TblExchangeOrder exchangeOrder = _ExchangeOrderRepository.GetSingle(x => x.Id == voucherData.Id);
                TblCustomerDetail customerDetail = _customerDetailsRepository.GetSingle(x => x.Id == exchangeOrder.CustomerDetailsId);

                if (voucherVerfication == null)
                {
                    //voucherVerfication = GenericMapper<VoucherDataContract, tblVoucherVerfication>.MapObject(voucherData);
                    voucherVerfication = new TblVoucherVerfication();
                    voucherVerfication.VoucherCode = voucherData.VoucherCode;
                    voucherVerfication.ExchangePrice = voucherData.ExchangePrice;
                    voucherVerfication.BusinessPartnerId = voucherData.BusinessPartnerId;
                    voucherVerfication.ExchangeOrderId = exchangeOrder.Id;
                    voucherVerfication.CustomerId = exchangeOrder.CustomerDetailsId;
                    if (voucherData.ModelNumberId !=null && voucherData.ModelNumberId > 0)
                    {
                        voucherVerfication.ModelNumberId = voucherData.ModelNumberId;

                    }
                    voucherVerfication.InvoiceNumber = voucherData.InvoiceNumber;

                    voucherStatusName = "Captured";
                    TblVoucherStatus tblVoucherStatu = _voucherStatusRepository.GetSingle(x => x.VoucherStatusName == voucherStatusName);

                    voucherVerfication.VoucherStatusId = tblVoucherStatu.VoucherStatusId;

                    voucherVerfication.IsVoucherused = true;
                    voucherVerfication.IsActive = true;
                    voucherVerfication.CreatedDate = DateTime.Now.TrimMilliseconds();
                    voucherVerfication.NewProductTypeId = voucherData.NewProductCategoryTypeId;
                    voucherVerfication.InvoiceNumber = voucherData.InvoiceNumber;


                    _voucherVerificationRepository.Create(voucherVerfication);
                    _voucherVerificationRepository.SaveChanges();
                    result = voucherVerfication.VoucherVerficationId;

                    #region Code to update the Bill Cloud API  (Voucher Reedeemed)

                    isVoucherProccessed = true;
                    if (result>0 && voucherVerfication.IsVoucherused==true)
                    {
                        //voucherStatusName = "Captured";
                        //TblVoucherStatus tblVoucherStatu = _voucherStatusRepository.GetSingle(x => x.VoucherStatusName == voucherStatusName);
                        ////voucherVerfication = new tblVoucherVerfication();
                        ////_voucherVerificationRepository = new VoucherVerificationRepository();
                        //voucherVerfication = new TblVoucherVerfication();
                        //voucherVerfication = _voucherVerificationRepository.GetSingle(x => x.VoucherVerficationId == result);
                        //voucherVerfication.ExchangePrice = Convert.ToDecimal(exchangeOrder.ExchangePrice);
                        //if (exchangeOrder.IsDtoC == true)
                        //{
                        //    voucherVerfication.Sweetneer = Convert.ToDecimal(exchangeOrder.Sweetener);
                        //}
                        //else
                        //{
                        //    voucherVerfication.Sweetneer = Convert.ToDecimal(exchangeOrder.Sweetener);
                        //}
                        //voucherVerfication.IsVoucherused = true;
                        //voucherVerfication.VoucherStatusId = tblVoucherStatu.VoucherStatusId;
                        //_voucherVerificationRepository.Update(voucherVerfication);
                        //_voucherVerificationRepository.SaveChanges();
                        //result = voucherVerfication.VoucherVerficationId;

                        if (exchangeOrder != null)
                        {
                            exchangeOrder.IsVoucherused = true;
                            exchangeOrder.VoucherStatusId = tblVoucherStatu.VoucherStatusId;
                            exchangeOrder.BusinessPartnerId = voucherVerfication.BusinessPartnerId;
                            //exchangeObj.SaleAssociateName = voucherData.ExchangeOrderDataContract.SaleAssociateName;
                            //exchangeObj.SalesAssociateEmail = voucherData.ExchangeOrderDataContract.AssociateEmail;
                            //exchangeObj.SalesAssociatePhone = voucherData.ExchangeOrderDataContract.StorePhoneNumber;
                            if (!(string.IsNullOrEmpty(voucherData.SerialNumber)))
                            {
                                exchangeOrder.SerialNumber = voucherData.SerialNumber;
                            }
                            _ExchangeOrderRepository.Update(exchangeOrder);
                            _ExchangeOrderRepository.SaveChanges();
                        }
                    }
                    #endregion

                    #region Update Voucher Detail in Zoho
                    TblVoucherStatus voucherStatus = _voucherStatusRepository.GetSingle(x => x.VoucherStatusId == voucherVerfication.VoucherStatusId);


                    #endregion

                    #region Code To send Notification for working Product
                    TblProductType producttype = _ProductTypeRepository.GetSingle(x => x.Id == exchangeOrder.ProductTypeId);
                    if (producttype != null)
                    {
                        TblProductCategory productCategoryobj = _ProductCategoryRepository.GetSingle(x => x.Id == producttype.ProductCatId);
                        if (productCategoryobj != null)
                        {
                            string productCondition = "Excellent";
                            if (exchangeOrder.ProductCondition == productCondition)
                            {
                                #region code to send whatsappNotification For Voucher verification
                                voucherCapture whatsappObj = new voucherCapture();
                                whatsappObj.userDetails = new voucherUserDetails();
                                whatsappObj.notification = new vaucherCaptureNotification();
                                whatsappObj.notification.@params = new vouchercaptureProperties();
                                whatsappObj.userDetails.number = customerDetail.PhoneNumber;
                                whatsappObj.notification.sender = _config.Value.YelloaiSenderNumber.ToString();
                                whatsappObj.notification.type = _config.Value.YellowaiMesssaheType.ToString();
                                whatsappObj.notification.templateId = NotificationConstants.voucher_capture_working;
                                whatsappObj.notification.@params.companyName = exchangeOrder.CompanyName.ToString();
                                whatsappObj.notification.@params.customerName = customerDetail.FirstName;
                                whatsappObj.notification.@params.vouchercode = exchangeOrder.VoucherCode;
                                whatsappObj.notification.@params.oldProductcategory = productCategoryobj.Description;
                                whatsappObj.notification.@params.olsProductCategory = productCategoryobj.Description;
                                whatsappObj.notification.@params.workingQualities = productCategoryobj.CommentForWorking;
                                string url = _config.Value.YellowAiUrl.ToString();
                                RestResponse responsewhatsapp = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                                ResponseCode = responsewhatsapp.StatusCode.ToString();
                                WhatssAppStatusEnum = WhatsappEnum.SuccessCode.ToString();
                                if (ResponseCode == WhatssAppStatusEnum)
                                {
                                    responseforWhatasapp = responsewhatsapp.Content;
                                    if (responseforWhatasapp != null)
                                    {
                                        whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                        TblWhatsAppMessage whatsapObj = new TblWhatsAppMessage();
                                        whatsapObj.TemplateName = NotificationConstants.Test_Template;
                                        whatsapObj.IsActive = true;
                                        whatsapObj.PhoneNumber = customerDetail.PhoneNumber;
                                        whatsapObj.SendDate = DateTime.Now;
                                        whatsapObj.MsgId = whatssappresponseDC.msgId;
                                        _WhatsAppMessageRepository.Create(whatsapObj);
                                        _WhatsAppMessageRepository.SaveChanges();
                                    }
                                    else
                                    {
                                        string ExchOrderObj = JsonConvert.SerializeObject(voucherData);
                                        _logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", voucherData.SponsorOrderNumber, ExchOrderObj);
                                    }
                                }
                                else
                                {
                                    string ExchOrderObj = JsonConvert.SerializeObject(voucherData);
                                    _logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", voucherData.SponsorOrderNumber, ExchOrderObj);
                                }
                            }
                        }

                        #endregion
                    }
                    #endregion
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VoucherRedemption", "AddVouchertoDB", ex);
            }

            return result;
        }


        #endregion

    }

}

