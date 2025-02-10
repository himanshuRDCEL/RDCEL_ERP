using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Model.ABBRedemption;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using AutoMapper;
using RDCELERP.Common.Helper;
using RDCELERP.Model.AbbRegistration;
using System.Text.Json;
using static RDCELERP.Model.ABBRedemption.LoVViewModel;
using RDCELERP.Model.Master;
using RDCELERP.Model.ABBPlanMaster;
using RDCELERP.Common.Enums;
using Newtonsoft.Json;
using RestSharp;
using static Org.BouncyCastle.Math.EC.ECCurve;
using static RDCELERP.Model.Whatsapp.WhatsappSelfqcViewModel;
using RDCELERP.Common.Constant;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.ExchangeBulkLiquidation;
using UserDetails = RDCELERP.Model.Whatsapp.WhatsappSelfqcViewModel.UserDetails;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.BAL.Enum;
using Mailjet.Client.Resources;
using static RDCELERP.Common.Helper.MessageHelper;
using RDCELERP.Model.ResponseModel;
using RDCELERP.BAL.Helper;
using RDCELERP.Model.LGC;
using RDCELERP.Model.Company;
using static RDCELERP.Model.ExchangeOrder.ExchangeOrderViewModel;
using RDCELERP.Model.ExchangeOrder;

namespace RDCELERP.BAL.MasterManager
{
    public class ABBRedemptionManager : IABBRedemptionManager
    {
        #region  Variable Declaration
        IAbbRegistrationRepository _abbRegistrationRepository;
        IABBRedemptionRepository _abbRedemptionRepository;
        IUserRepository _userRepository;
        IUserRoleRepository _userRoleRepository;
        IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        private Digi2l_DevContext _context;
        IListofValueRepository _listofValueRepository;
        IOrderTransRepository _orderTransRepository;
        IOrderTransactionManager _orderTransactionManager;
        IExchangeABBStatusHistoryManager _exchangeABBStatusHistoryManager;
        IAbbRegistrationManager _AbbRegistrationManager;
        IABBPlanMasterRepository _aBBPlanMasterRepository;
        IExchangeOrderManager _exchangeOrderManager;
        ICustomerDetailsRepository _customerDetailsRepository;
        IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _WhatsAppMessageRepository;
        IOptions<ApplicationSettings> _config;
        IVoucherStatusRepository _voucherStatusRepository;
        ICommonManager _commonManager;
        #endregion

        public ABBRedemptionManager(IListofValueRepository listofValueRepository, Digi2l_DevContext context, IExchangeABBStatusHistoryManager exchangeABBStatusHistoryManager, IOrderTransactionManager orderTransactionManager, IAbbRegistrationRepository abbRegistrationRepository, IABBRedemptionRepository abbRedemptionRepository, IUserRoleRepository userRoleRepository, IRoleRepository roleRepository, IMapper mapper, ILogging logging, IOrderTransRepository orderTransRepository, IAbbRegistrationManager abbRegistrationManager, IABBPlanMasterRepository aBBPlanMasterRepository, IExchangeOrderManager exchangeOrderManager, ICustomerDetailsRepository customerDetailsRepository, IWhatsAppMessageRepository whatsAppMessageRepository, IWhatsappNotificationManager whatsappNotificationManager, IOptions<ApplicationSettings> config, IVoucherStatusRepository voucherStatusRepository, ICommonManager commonManager)
        {
            _abbRegistrationRepository = abbRegistrationRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logging = logging;
            _exchangeABBStatusHistoryManager = exchangeABBStatusHistoryManager;
            _orderTransactionManager = orderTransactionManager;
            _abbRedemptionRepository = abbRedemptionRepository;
            _context = context;
            _listofValueRepository = listofValueRepository;
            _orderTransRepository = orderTransRepository;
            _AbbRegistrationManager = abbRegistrationManager;
            _aBBPlanMasterRepository = aBBPlanMasterRepository;
            _exchangeOrderManager = exchangeOrderManager;
            _customerDetailsRepository = customerDetailsRepository;
            _WhatsAppMessageRepository = whatsAppMessageRepository;
            _whatsappNotificationManager = whatsappNotificationManager;
            _config = config;
            _voucherStatusRepository = voucherStatusRepository;
            _commonManager = commonManager;
        }

        #region Get ABBRedemption by Id
        /// <summary>
        /// Method is Used for get abb reistration data on basis of abb registeration Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ABBRedemptionViewModel GetABBRedemptionById(int id)
        {
            ABBRedemptionViewModel AbbRedemptionVM = null;
            TblAbbredemption tblAbbredemption = null;
            AbbRegistrationModel abbRegistrationModel = new AbbRegistrationModel();
            try
            {
                tblAbbredemption = _abbRedemptionRepository.GetSingle(where: x => x.IsActive == true || x.AbbregistrationId == id || x.RedemptionId == id);
                if (tblAbbredemption != null)
                {
                    AbbRedemptionVM = _mapper.Map<TblAbbredemption, ABBRedemptionViewModel>(tblAbbredemption);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRedemptionManager", "GetABBRedemptionById", ex);
            }
            return AbbRedemptionVM;
        }
        #endregion

        #region GetAllABBRedemptionDetails
        public ABBRedemptionViewModel GetAllABBRedemptionDetails()
        {
            ABBRedemptionViewModel AbbRedemptionModelList = null;
            List<TblAbbredemption> tblAbbRedemptionlist = new List<TblAbbredemption>();
            // TblUseRole TblUseRole = null;
            try
            {

                tblAbbRedemptionlist = _abbRedemptionRepository.GetList(x => x.IsActive == true).ToList();

                if (tblAbbRedemptionlist != null && tblAbbRedemptionlist.Count > 0)
                {
                    AbbRedemptionModelList = _mapper.Map<List<TblAbbredemption>, ABBRedemptionViewModel>(tblAbbRedemptionlist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRedemptionManager", "GetAllABBRedemptionDetails", ex);
            }
            return AbbRedemptionModelList;
        }
        #endregion

        #region SaveABBRedemptionDetails
        public int SaveABBRedemptionDetails(ABBRedemptionViewModel abbRedemptionViewModel)
        {
            TblAbbredemption Tblabbredemption = new TblAbbredemption();
            TblOrderTran TblOrderTran = new TblOrderTran();
            WhatasappResponse whatasappResponse = new WhatasappResponse();
            TblWhatsAppMessage? tblwhatsappmessage = null;
            string voucherName = null;
            string baseurl = _config.Value.BaseURL + "QCPortal/SelfQC?regdno=" + abbRedemptionViewModel.RegdNo;

            int tranid = 0;
            //int? loginId = 3;
            try
            {
                if (abbRedemptionViewModel != null)
                {
                    Tblabbredemption = _mapper.Map<ABBRedemptionViewModel, TblAbbredemption>(abbRedemptionViewModel);
                    if (Tblabbredemption.RedemptionId > 0)
                    {
                        //Code to update the object                      
                        Tblabbredemption.ModifiedDate = _currentDatetime;
                        _abbRedemptionRepository.Update(Tblabbredemption);
                    }
                    else
                    {
                        //Code to Insert the object 
                        Tblabbredemption.IsActive = true;
                        Tblabbredemption.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                        Tblabbredemption.AbbredemptionStatus = EnumHelper.DescriptionAttr(OrderStatusEnum.OrderCreatedbySponsor);
                        if (Tblabbredemption.RedemptionDate == null)
                        {
                            Tblabbredemption.RedemptionDate = _currentDatetime;
                        }
                        Tblabbredemption.CreatedDate = _currentDatetime;
                        Tblabbredemption.ModifiedDate = _currentDatetime;
                        _abbRedemptionRepository.Create(Tblabbredemption);
                    }

                    _abbRedemptionRepository.SaveChanges();


                    #region Code to add order in transaction and history
                    TblAbbregistration abbregistration = _abbRegistrationRepository.GetSingleOrder((int)Tblabbredemption.AbbregistrationId);
                    if (abbregistration != null && abbregistration != null)
                    {

                        #region update AbbRegistration
                        if (abbRedemptionViewModel.IsCustomerDetailsEdittable == true && abbRedemptionViewModel.CustomerDetailsId > 0)
                        {
                            TblCustomerDetail tblCustomerDetail = _customerDetailsRepository.GetSingle(x => x.IsActive == true && x.Id == Convert.ToInt32(abbRedemptionViewModel.CustomerDetailsId));
                            tblCustomerDetail.FirstName = abbRedemptionViewModel.CustFirstName != string.Empty ? abbRedemptionViewModel.CustFirstName : string.Empty;
                            tblCustomerDetail.LastName = abbRedemptionViewModel.CustLastName != string.Empty ? abbRedemptionViewModel.CustLastName : string.Empty;
                            tblCustomerDetail.PhoneNumber = abbRedemptionViewModel.CustMobile != string.Empty ? abbRedemptionViewModel.CustMobile : string.Empty;
                            tblCustomerDetail.Address1 = abbRedemptionViewModel.CustAddress1 != string.Empty ? abbRedemptionViewModel.CustAddress1 : string.Empty;
                            tblCustomerDetail.Address2 = abbRedemptionViewModel.CustAddress2 != string.Empty ? abbRedemptionViewModel.CustAddress2 : string.Empty;
                            tblCustomerDetail.Email = abbRedemptionViewModel.CustEmail != string.Empty ? abbRedemptionViewModel.CustEmail : string.Empty;
                            tblCustomerDetail.City = abbRedemptionViewModel.CustCity != string.Empty ? abbRedemptionViewModel.CustCity : string.Empty;

                            abbregistration.CustFirstName = abbRedemptionViewModel.CustFirstName != string.Empty ? abbRedemptionViewModel.CustFirstName : string.Empty;
                            abbregistration.CustLastName = abbRedemptionViewModel.CustLastName != string.Empty ? abbRedemptionViewModel.CustLastName : string.Empty;
                            abbregistration.CustMobile = abbRedemptionViewModel.CustMobile != string.Empty ? abbRedemptionViewModel.CustMobile : string.Empty;
                            abbregistration.CustAddress1 = abbRedemptionViewModel.CustAddress1 != string.Empty ? abbRedemptionViewModel.CustAddress1 : string.Empty;
                            abbregistration.CustAddress2 = abbRedemptionViewModel.CustAddress2 != string.Empty ? abbRedemptionViewModel.CustAddress2 : string.Empty;
                            abbregistration.CustEmail = abbRedemptionViewModel.CustEmail != string.Empty ? abbRedemptionViewModel.CustEmail : string.Empty;
                            abbregistration.CustCity = abbRedemptionViewModel.CustCity != string.Empty ? abbRedemptionViewModel.CustCity : string.Empty;
                            abbregistration.Location = abbRedemptionViewModel.Location != string.Empty ? abbRedemptionViewModel.Location : string.Empty;

                            _customerDetailsRepository.Update(tblCustomerDetail);
                            _customerDetailsRepository.SaveChanges();
                        }
                        abbregistration.ModifiedBy = Tblabbredemption.CreatedBy;
                        abbregistration.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                        abbregistration.ModifiedDate = _currentDatetime;
                        _abbRegistrationRepository.Update(abbregistration);
                        _abbRegistrationRepository.SaveChanges();
                        #endregion

                        //Code for Order tran
                        OrderTransactionViewModel orderTransactionVM = new OrderTransactionViewModel();
                        orderTransactionVM.OrderType = (int)ExchangeABBEnum.ABB;
                        orderTransactionVM.AbbredemptionId = Tblabbredemption.RedemptionId;
                        orderTransactionVM.RegdNo = Tblabbredemption.RegdNo;
                        orderTransactionVM.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                        orderTransactionVM.ExchangePrice = Tblabbredemption.RedemptionValue;
                        orderTransactionVM.AmountPaidToCustomer = false;
                        orderTransactionVM.CreatedBy = Tblabbredemption.CreatedBy;

                        TblOrderTran = _mapper.Map<OrderTransactionViewModel, TblOrderTran>(orderTransactionVM);
                        if (TblOrderTran != null)
                        {
                            TblOrderTran.IsActive = true;
                            TblOrderTran.CreatedDate = _currentDatetime;
                            TblOrderTran.ModifiedDate = _currentDatetime;
                            TblOrderTran.ModifiedBy = Tblabbredemption.CreatedBy;
                            TblOrderTran.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                            _orderTransRepository.Create(TblOrderTran);
                            int orderTransResult = _orderTransRepository.SaveChanges();
                            if (orderTransResult > 0)
                            {
                                tranid = Convert.ToInt32(TblOrderTran.OrderTransId);
                            }
                            //Code for Order history
                            if (tranid > 0)
                            {
                                ExchangeABBStatusHistoryViewModel exchangeABBStatusHistoryVM = new ExchangeABBStatusHistoryViewModel();
                                exchangeABBStatusHistoryVM.OrderType = (int)ExchangeABBEnum.ABB;
                                exchangeABBStatusHistoryVM.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                                exchangeABBStatusHistoryVM.OrderTransId = tranid;
                                exchangeABBStatusHistoryVM.RegdNo = Tblabbredemption.RegdNo;
                                exchangeABBStatusHistoryVM.CustId = Convert.ToInt32(Tblabbredemption.CustomerDetailsId);
                                int resultHistory = _exchangeABBStatusHistoryManager.ManageExchangeABBStatusHistory(exchangeABBStatusHistoryVM);

                                #region code to send selfqc link on whatsappNotification
                                if (abbregistration != null && !string.IsNullOrEmpty(abbregistration.RegdNo) && abbregistration.CustMobile != null)
                                {
                                    #region code to send selfqc link on whatsappNotification
                                    WhatsappTemplate whatsappObj = new WhatsappTemplate();
                                    whatsappObj.userDetails = new UserDetails();
                                    whatsappObj.notification = new SelfQC();
                                    whatsappObj.notification.@params = new URL();
                                    whatsappObj.userDetails.number = abbregistration.CustMobile;
                                    whatsappObj.notification.sender = _config.Value.YelloaiSenderNumber;
                                    whatsappObj.notification.type = _config.Value.YellowaiMesssaheType;
                                    whatsappObj.notification.templateId = NotificationConstants.SelfQC_Link;
                                    whatsappObj.notification.@params.Link = baseurl;
                                    whatsappObj.notification.@params.CustomerName = abbregistration.CustFirstName + " " + abbregistration.CustLastName;
                                    string url = _config.Value.YellowAiUrl;
                                    RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                                    if (response.Content != null)
                                    {
                                        whatasappResponse = JsonConvert.DeserializeObject<WhatasappResponse>(response.Content);
                                        tblwhatsappmessage = new TblWhatsAppMessage();
                                        tblwhatsappmessage.TemplateName = NotificationConstants.SelfQC_Link;
                                        tblwhatsappmessage.IsActive = true;
                                        tblwhatsappmessage.PhoneNumber = abbregistration.CustMobile;
                                        tblwhatsappmessage.SendDate = DateTime.Now;
                                        tblwhatsappmessage.MsgId = whatasappResponse.msgId;
                                        _WhatsAppMessageRepository.Create(tblwhatsappmessage);
                                        _WhatsAppMessageRepository.SaveChanges();
                                    }
                                    #endregion
                                    //bool selfQCLink = _exchangeOrderManager.sendSelfQCUrl(abbregistration.RegdNo, abbregistration.CustMobile, loginId);
                                }
                                #endregion
                            }
                            #region For Insatant voucher
                            if (abbregistration.BusinessPartner.IsRedemptionSettelemtInstant == true && abbregistration.BusinessPartner.IsVoucher == true && abbregistration.BusinessPartner.VoucherType == Convert.ToInt32(VoucherTypeEnum.Discount))
                            {
                                voucherName = "Generated";
                                TblVoucherStatus voucherStatu = _voucherStatusRepository.GetSingle(x => x.VoucherStatusName == voucherName);
                                Tblabbredemption.VoucherCodeExpDate = DateTime.Now.AddHours(Convert.ToDouble(abbregistration.BusinessUnit.VoucherExpiryTime));
                                Tblabbredemption.VoucherCode = GenerateVoucher();
                                Tblabbredemption.IsVoucherUsed = false;
                                Tblabbredemption.VoucherStatusId = voucherStatu.VoucherStatusId;
                                Tblabbredemption.BusinessPartnerId = abbregistration.BusinessPartnerId;
                                Tblabbredemption.IsDefferedSettelment = false;
                                Tblabbredemption.ModifiedDate = _currentDatetime;
                            }
                            #endregion
                            #region For Cash voucher
                            else if (abbregistration.BusinessPartner.IsRedemptionSettelemtInstant == false && abbregistration.BusinessPartner.IsVoucher == true && abbregistration.BusinessPartner.VoucherType == Convert.ToInt32(VoucherTypeEnum.Cash))
                            {
                                voucherName = "Generated";
                                TblVoucherStatus voucherStatu = _voucherStatusRepository.GetSingle(x => x.VoucherStatusName == voucherName);
                                Tblabbredemption.VoucherCodeExpDate = DateTime.Now.AddHours(Convert.ToDouble(abbregistration.BusinessUnit.VoucherExpiryTime));
                                Tblabbredemption.VoucherCode = GenerateVoucher();
                                Tblabbredemption.IsVoucherUsed = false;
                                Tblabbredemption.VoucherStatusId = voucherStatu.VoucherStatusId;
                                Tblabbredemption.BusinessPartnerId = abbregistration.BusinessPartnerId;
                                Tblabbredemption.IsDefferedSettelment = true;
                                Tblabbredemption.ModifiedDate = _currentDatetime;
                            }
                            if (abbregistration.BusinessPartner.IsRedemptionSettelemtInstant == false || abbregistration.BusinessPartner.IsRedemptionSettelemtInstant == null)
                            {
                                Tblabbredemption.IsDefferedSettelment = true;
                            }
                            else
                            {
                                Tblabbredemption.IsDefferedSettelment = false;
                            }
                            #endregion
                            _abbRedemptionRepository.Update(Tblabbredemption);
                            _abbRedemptionRepository.SaveChanges();

                            #region Send Voucher Message to customer
                            if (abbregistration.BusinessPartner.IsRedemptionSettelemtInstant == true && abbregistration.BusinessPartner.IsVoucher == true && abbregistration.BusinessPartner.VoucherType == Convert.ToInt32(VoucherTypeEnum.Discount))
                            {
                                var result = _abbRedemptionRepository.GetSingle(x => x.AbbregistrationId == abbregistration.AbbregistrationId && x.IsActive == true);
                                // string Instantbaseurl = _config.Value.BaseURL + "ABBRedemption/InstantVouchar?id=" + result.RedemptionId;
                                string Instantbaseurl = _config.Value.MVCBaseURL + "/Home/RV/" + result.RedemptionId;

                                WhatsappTemplateInstant whatsappObj = new WhatsappTemplateInstant();
                                whatsappObj.userDetails = new UserDetails();
                                whatsappObj.notification = new NotificationInstant();
                                whatsappObj.notification.@params = new SendVoucherOnWhatssapp();
                                whatsappObj.userDetails.number = abbregistration.CustMobile;
                                whatsappObj.notification.sender = _config.Value.YelloaiSenderNumber;
                                whatsappObj.notification.type = _config.Value.YellowaiMesssaheType;
                                whatsappObj.notification.templateId = NotificationConstants.Send_Voucher_Code_Template;
                                whatsappObj.notification.@params.voucherAmount = Tblabbredemption.RedemptionValue.ToString();
                                whatsappObj.notification.@params.VoucherExpiry = Convert.ToDateTime(Tblabbredemption.VoucherCodeExpDate).ToString("dd/MM/yyyy");
                                whatsappObj.notification.@params.voucherCode = Tblabbredemption.VoucherCode.ToString();
                                whatsappObj.notification.@params.BrandName = abbregistration.BusinessUnit.Name.ToString();
                                whatsappObj.notification.@params.BrandName2 = abbregistration.BusinessUnit.Name.ToString();
                                whatsappObj.notification.@params.VoucherLink = Instantbaseurl;
                                string url = _config.Value.YellowAiUrl;

                                RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                                if (response.Content != null)
                                {
                                    whatasappResponse = JsonConvert.DeserializeObject<WhatasappResponse>(response.Content);
                                    tblwhatsappmessage = new TblWhatsAppMessage();
                                    tblwhatsappmessage.TemplateName = NotificationConstants.Send_Voucher_Code_Template;
                                    tblwhatsappmessage.IsActive = true;
                                    tblwhatsappmessage.PhoneNumber = abbregistration.CustMobile;
                                    tblwhatsappmessage.SendDate = DateTime.Now;
                                    tblwhatsappmessage.MsgId = whatasappResponse.msgId;
                                    _WhatsAppMessageRepository.Create(tblwhatsappmessage);
                                    _WhatsAppMessageRepository.SaveChanges();
                                }
                            }
                            #endregion

                        }
                       
                        #endregion
                    }
                    else
                    {
                        Tblabbredemption.IsActive = false;
                        _abbRedemptionRepository.Update(Tblabbredemption);
                        _abbRedemptionRepository.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRedemptionManager", "ManageUser", ex);
            }
            return Tblabbredemption.RedemptionId;
        }
        #endregion

        #region GetABBDetailsByRegdNo
        public ABBRedemptionViewModel GetABBDetailsByRegdNo(string regdno)
        {
            ABBRegistrationViewModel aBBRegistrationViewModel = null;
            //AbbRegistrationModel abbRegistrationModel = null;
            ABBRedemptionViewModel aBBRedemptionViewModel = new ABBRedemptionViewModel();
            //TblAbbredemption tblAbbredemption = null;
            TblAbbregistration tblAbbregistration = null;

            try
            {
                if (!string.IsNullOrEmpty(regdno))
                {
                    tblAbbregistration = _abbRegistrationRepository.GetSingle(where: x => x.IsActive == true && x.RegdNo == regdno);

                    if (tblAbbregistration != null)
                    {
                        aBBRegistrationViewModel = _mapper.Map<TblAbbregistration, ABBRegistrationViewModel>(tblAbbregistration);

                        TblProductCategory tblProductCategory = _context.TblProductCategories.FirstOrDefault(x => x.Id == aBBRegistrationViewModel.NewProductCategoryId);
                        aBBRegistrationViewModel.NewProductCategoryName = tblProductCategory != null ? tblProductCategory.Description : string.Empty;

                        TblProductType tblProductType = _context.TblProductTypes.FirstOrDefault(x => x.Id == Convert.ToInt32(aBBRegistrationViewModel.NewProductCategoryTypeId));
                        aBBRegistrationViewModel.NewProductCategoryType = tblProductType != null ? tblProductType.Description : string.Empty;

                        TblBrand tblBrand = _context.TblBrands.FirstOrDefault(x => x.Id == aBBRegistrationViewModel.NewBrandId);
                        aBBRegistrationViewModel.SponsorName = tblBrand != null ? tblBrand.Name : string.Empty;

                        aBBRedemptionViewModel.ABBRegistrationId = aBBRegistrationViewModel.ABBRegistrationId != null ? aBBRegistrationViewModel.ABBRegistrationId : 0;
                        aBBRedemptionViewModel.CustFirstName = aBBRegistrationViewModel.CustFirstName.ToString() != null ? aBBRegistrationViewModel.CustFirstName : string.Empty;
                        aBBRedemptionViewModel.CustLastName = aBBRegistrationViewModel.CustLastName.ToString() != null ? aBBRegistrationViewModel.CustLastName : string.Empty;
                        aBBRedemptionViewModel.CustMobile = aBBRegistrationViewModel.CustMobile.ToString() != null ? aBBRegistrationViewModel.CustMobile : string.Empty;
                        aBBRedemptionViewModel.CustEmail = aBBRegistrationViewModel.CustEmail != null ? aBBRegistrationViewModel.CustEmail : string.Empty;
                        aBBRedemptionViewModel.CustAddress1 = aBBRegistrationViewModel.CustAddress1 != null ? aBBRegistrationViewModel.CustAddress1 : string.Empty;
                        //aBBRedemptionViewModel.CustAddress2 = aBBRegistrationViewModel.CustAddress2.ToString() != null ? aBBRegistrationViewModel.CustAddress2 : string.Empty;                       
                        aBBRedemptionViewModel.CustPinCode = aBBRegistrationViewModel.CustPinCode.ToString() != null ? aBBRegistrationViewModel.CustPinCode : string.Empty;
                        aBBRedemptionViewModel.CustCity = aBBRegistrationViewModel.CustCity != null ? aBBRegistrationViewModel.CustCity.ToString() : string.Empty;
                        aBBRedemptionViewModel.NewProductCategoryId = aBBRegistrationViewModel.NewProductCategoryId != 0 ? aBBRegistrationViewModel.NewProductCategoryId : 0;
                        aBBRedemptionViewModel.NewProductCategoryTypeId = aBBRegistrationViewModel.NewProductCategoryTypeId != 0 ? aBBRegistrationViewModel.NewProductCategoryTypeId : 0;
                        aBBRedemptionViewModel.NewProductCategoryName = aBBRegistrationViewModel.NewProductCategoryName.ToString() != null ? aBBRegistrationViewModel.NewProductCategoryName : string.Empty;
                        aBBRedemptionViewModel.NewProductCategoryType = aBBRegistrationViewModel.NewProductCategoryType.ToString() != null ? aBBRegistrationViewModel.NewProductCategoryType : string.Empty;
                        aBBRedemptionViewModel.SponsorName = aBBRegistrationViewModel.SponsorName.ToString() != null ? aBBRegistrationViewModel.SponsorName.ToString() : string.Empty;
                        aBBRedemptionViewModel.NewBrandId = aBBRegistrationViewModel.NewBrandId != 0 ? aBBRegistrationViewModel.NewBrandId : 0;
                        aBBRedemptionViewModel.NewSize = aBBRegistrationViewModel.NewSize != null ? aBBRegistrationViewModel.NewSize : string.Empty;
                        aBBRedemptionViewModel.ProductSrNo = aBBRegistrationViewModel.ProductSrNo != null ? aBBRegistrationViewModel.ProductSrNo.ToString() : string.Empty;
                        aBBRedemptionViewModel.ModelNumberId = aBBRegistrationViewModel.ModelNumberId != null ? aBBRegistrationViewModel.ModelNumberId : Convert.ToInt32(null);
                        aBBRedemptionViewModel.AbbplanName = aBBRegistrationViewModel.AbbplanName != null ? aBBRegistrationViewModel.AbbplanName.ToString() : string.Empty;
                        aBBRedemptionViewModel.ProductNetPrice = aBBRegistrationViewModel.ProductNetPrice != null ? aBBRegistrationViewModel.ProductNetPrice : Convert.ToInt32(null);
                        aBBRedemptionViewModel.AbbplanPeriod = aBBRegistrationViewModel.AbbplanPeriod != null ? aBBRegistrationViewModel.AbbplanPeriod : string.Empty;
                        aBBRedemptionViewModel.InvoiceNo = aBBRegistrationViewModel.InvoiceNo != null ? aBBRegistrationViewModel.InvoiceNo.ToString() : string.Empty;
                        aBBRedemptionViewModel.InvoiceDate = aBBRegistrationViewModel.InvoiceDate != null ? aBBRegistrationViewModel.InvoiceDate : null;
                        aBBRedemptionViewModel.InvoiceImage = aBBRegistrationViewModel.InvoiceImage != null ? aBBRegistrationViewModel.InvoiceImage.ToString() : string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRedemptionManager", "GetABBRedempDetailsByRegdNo", ex);
            }
            return aBBRedemptionViewModel;
        }
        #endregion

        #region GetAutoCompleteRegdNo
        public List<TblAbbregistration> GetAutoCompleteRegdNo(string regdNum)
        {
            List<TblAbbregistration> tblAbbregistrations = null;
            try
            {
                if (regdNum != null)
                {
                    tblAbbregistrations = _abbRegistrationRepository.
                   GetList(x => x.IsActive == true && (x.RegdNo != null && x.RegdNo.StartsWith(regdNum.ToUpper()))).ToList();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRedemptionManager", "GetAutoCompleteRegdNo", ex);
            }
            return tblAbbregistrations;
        }
        #endregion

        #region GetRedemptionPeriod
        public List<TblLoV> GetRedemptionPeriod()
        {
            List<TblLoV> tblLoVListvm = null;
            try
            {
                tblLoVListvm = _listofValueRepository.GetList(where: x => x.IsActive == true && x.ParentId == Convert.ToInt32(LoVViewModeEnum.Redempperiod)).ToList();
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRedemptionManager", "GetRedemptionPeriod", ex);
            }
            return tblLoVListvm;
        }
        #endregion

        #region GetRedemptionPercentage
        public List<TblLoV> GetRedemptionPercentage()
        {
            List<TblLoV> tblLoVListvm = null;

            try
            {
                tblLoVListvm = _listofValueRepository.GetList(where: x => x.IsActive == true && x.ParentId == Convert.ToInt32(LoVViewModeEnum.Redemppercent)).ToList();
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRedemptionManager", "GetRedemptionPeriod", ex);
            }
            return tblLoVListvm;
        }
        #endregion

        #region Get ABBRedemption by regdNo for Edit
        public ABBRedemptionViewModel GetABBRedemptionByRegdNo(string rno)
        {
            ABBRedemptionViewModel AbbRedemptionVM = null;
            TblAbbredemption tblAbbredemption = null;

            try
            {
                tblAbbredemption = _abbRedemptionRepository.GetSingle(where: x => x.IsActive == true && x.RegdNo == rno);
                if (tblAbbredemption != null)
                {
                    AbbRedemptionVM = _mapper.Map<TblAbbredemption, ABBRedemptionViewModel>(tblAbbredemption);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRedemptionManager", "GetABBRedemptionById", ex);
            }
            return AbbRedemptionVM;
        }
        #endregion

        #region get exchnage order status
        /// <summary>
        /// get exchnage order status
        /// </summary>
        /// <returns></returns>
        public List<TblExchangeOrderStatus> GetExchangeOrderStatus()
        {
            List<TblExchangeOrderStatus> tblExchangeOrderStatuses = null;
            tblExchangeOrderStatuses = _context.TblExchangeOrderStatuses.ToList();

            return tblExchangeOrderStatuses;
        }
        #endregion

        #region Method to get the status list  by name
        /// <summary>
        /// Method to get the status list  by name
        /// </summary>
        /// <param name="statusName">statusName</param>
        /// <returns>List of TblExchangeOrderStatus</returns>
        public List<TblExchangeOrderStatus> GetExchangeOrderStatusByDepartment(string statusName)
        {
            List<TblExchangeOrderStatus> tblExchangeOrderStatuses = null;
            tblExchangeOrderStatuses = _context.TblExchangeOrderStatuses.Where(x => x.StatusName.ToLower().Equals(statusName)).ToList();

            return tblExchangeOrderStatuses;
        }
        #endregion

        #region  GetRedemptionPeriod ( BU/ProductCatId/ProductTypeId Based )
        public ABBPlanMasterViewModel GetABBPlanMasterDetails(int? BuId, int? productCatId, int? productTypeId, int? monthsdiff)
        {
            ABBPlanMasterViewModel aBBPlanMasterViewModel = null;
            try
            {
                if (BuId > 0 && productCatId > 0 && productTypeId > 0)
                {
                    TblAbbplanMaster tblAbbplanMaster1 = _aBBPlanMasterRepository.GetSingle(x => x.IsActive == true
                            && x.ProductCatId == productCatId && x.ProductTypeId == productTypeId && x.BusinessUnitId == BuId);
                    if (tblAbbplanMaster1 != null && monthsdiff > Convert.ToInt32(tblAbbplanMaster1.NoClaimPeriod))
                    {
                        TblAbbplanMaster tblAbbplanMaster = _aBBPlanMasterRepository.GetSingle(x => x.IsActive == true
                            && x.ProductCatId == productCatId && x.ProductTypeId == productTypeId && x.BusinessUnitId == BuId && x.FromMonth <= monthsdiff && x.ToMonth >= monthsdiff);
                        if (tblAbbplanMaster != null)
                        {
                            aBBPlanMasterViewModel = _mapper.Map<TblAbbplanMaster, ABBPlanMasterViewModel>(tblAbbplanMaster);
                        }
                    }
                    else
                    {
                        aBBPlanMasterViewModel = _mapper.Map<TblAbbplanMaster, ABBPlanMasterViewModel>(tblAbbplanMaster1);
                    }

                    return aBBPlanMasterViewModel;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRedemptionManager", "GetAbbOrderDetails", ex);
            }
            return aBBPlanMasterViewModel;
        }
        #endregion

        #region get ABB Redemption order details by regdno -- Added by Shashi
        /// <summary>
        /// get ABB Redemption order details by regdno
        /// </summary>
        /// <param name="regdNo"></param>
        /// <returns></returns>
        public ABBRedemptionViewModel GetAbbOrderDetails(string regdNo)
        {
            ABBRedemptionViewModel aBBRedemptionViewModel = new ABBRedemptionViewModel();
            TblAbbredemption? tblAbbredemption = null;
            TblOrderTran? tblOrderTran = null;
            BrandViewModel? brandVM = null;
            string brandName = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(regdNo))
                {
                    tblOrderTran = _orderTransRepository.GetOrderTransByRegdno(regdNo);
                    if (tblOrderTran != null && tblOrderTran.Abbredemption != null)
                    {
                        aBBRedemptionViewModel.OrderTransId = tblOrderTran.OrderTransId;
                        aBBRedemptionViewModel.RedemptionId = (int)tblOrderTran.AbbredemptionId;
                        tblAbbredemption = tblOrderTran.Abbredemption;

                        if (tblAbbredemption != null)
                        {
                            aBBRedemptionViewModel.RegdNo = tblAbbredemption.RegdNo != null ? tblAbbredemption.RegdNo : string.Empty;
                            aBBRedemptionViewModel.AbbCreatedDate = Convert.ToDateTime(tblAbbredemption.CreatedDate).ToString("dd/MM/yyyy");
                            aBBRedemptionViewModel.AbbModifiedDate = Convert.ToDateTime(tblAbbredemption.ModifiedDate).ToString("dd/MM/yyyy");
                            aBBRedemptionViewModel.RedemptionValue = (decimal)(tblAbbredemption.RedemptionValue != null ? tblAbbredemption.RedemptionValue : 0);
                            #region set IsDefferedSettlement
                            if (tblAbbredemption.IsDefferedSettelment == true)
                            {
                                aBBRedemptionViewModel.IsDefferedSettlement = YesNoEnum.Yes.ToString();
                            }
                            else
                            {
                                aBBRedemptionViewModel.IsDefferedSettlement = YesNoEnum.No.ToString();
                            }
                            #endregion
                            #region Business unit details
                            if (tblAbbredemption.Abbregistration?.BusinessUnit != null)
                            {
                                int? newBrandId = tblAbbredemption.Abbregistration.NewBrandId;
                                bool? Isbumultibrand = tblAbbredemption.Abbregistration.BusinessUnit.IsBumultiBrand;
                                brandVM = _commonManager.GetAbbBrandDetailsById(Isbumultibrand, newBrandId);
                                aBBRedemptionViewModel.BrandName = brandVM?.Name;
                                aBBRedemptionViewModel.CompanyName = tblAbbredemption.Abbregistration.BusinessUnit.Name != null ? tblAbbredemption.Abbregistration.BusinessUnit.Name : string.Empty;
                            }
                            #endregion
                            #region Business Partner details
                            if (tblAbbredemption.Abbregistration.BusinessPartner != null)
                            {
                                aBBRedemptionViewModel.StoreCode = tblAbbredemption.Abbregistration.BusinessPartner.StoreCode != null ? tblAbbredemption.Abbregistration.BusinessPartner.StoreCode : string.Empty;
                                aBBRedemptionViewModel.StorePhoneNumber = tblAbbredemption.Abbregistration.BusinessPartner.PhoneNumber != null ? tblAbbredemption.Abbregistration.BusinessPartner.PhoneNumber : string.Empty;
                                aBBRedemptionViewModel.StoreName = tblAbbredemption.Abbregistration.BusinessPartner.Name != null ? tblAbbredemption.Abbregistration.BusinessPartner.Name : string.Empty;
                                aBBRedemptionViewModel.StoreAssociateCode = tblAbbredemption.Abbregistration.BusinessPartner.AssociateCode != null ? tblAbbredemption.Abbregistration.BusinessPartner.AssociateCode : string.Empty;
                                aBBRedemptionViewModel.StoreEmail = tblAbbredemption.Abbregistration.BusinessPartner.Email != null ? tblAbbredemption.Abbregistration.BusinessPartner.Email : string.Empty;
                            }
                            #endregion
                            #region Customer details
                            if (tblAbbredemption.CustomerDetails != null)
                            {
                                aBBRedemptionViewModel.CustFirstName = tblAbbredemption.CustomerDetails.FirstName != null ? tblAbbredemption.CustomerDetails.FirstName : string.Empty;
                                aBBRedemptionViewModel.CustLastName = tblAbbredemption.CustomerDetails.LastName != null ? tblAbbredemption.CustomerDetails.LastName : string.Empty;
                                aBBRedemptionViewModel.CustEmail = tblAbbredemption.CustomerDetails.Email != null ? tblAbbredemption.CustomerDetails.Email : string.Empty;
                                aBBRedemptionViewModel.CustCity = tblAbbredemption.CustomerDetails.City != null ? tblAbbredemption.CustomerDetails.City : string.Empty;
                                aBBRedemptionViewModel.CustPinCode = tblAbbredemption.CustomerDetails.ZipCode != null ? tblAbbredemption.CustomerDetails.ZipCode : string.Empty;
                                aBBRedemptionViewModel.CustAddress1 = tblAbbredemption.CustomerDetails.Address1 != null ? tblAbbredemption.CustomerDetails.Address1 : string.Empty;
                                aBBRedemptionViewModel.CustAddress2 = tblAbbredemption.CustomerDetails.Address2 != null ? tblAbbredemption.CustomerDetails.Address2 : string.Empty;
                                aBBRedemptionViewModel.CustMobile = tblAbbredemption.CustomerDetails.PhoneNumber != null ? tblAbbredemption.CustomerDetails.PhoneNumber : string.Empty;
                                aBBRedemptionViewModel.CustState = tblAbbredemption.CustomerDetails.State != null ? tblAbbredemption.CustomerDetails.State : string.Empty;
                            }
                            #endregion
                            #region Status details
                            if (tblAbbredemption.Status != null)
                            {
                                aBBRedemptionViewModel.StatusId = tblAbbredemption.Status.Id;
                                aBBRedemptionViewModel.StatusCode = tblAbbredemption.Status.StatusCode != null ? tblAbbredemption.Status.StatusCode : string.Empty;
                                aBBRedemptionViewModel.StatusDescription = tblAbbredemption.Status.StatusDescription != null ? tblAbbredemption.Status.StatusDescription : string.Empty;
                            }
                            #endregion
                            #region ProductCategory details
                            if (tblAbbredemption.Abbregistration.NewProductCategory != null)
                            {
                                aBBRedemptionViewModel.NewProductCategoryName = tblAbbredemption.Abbregistration.NewProductCategory.Description != null ? tblAbbredemption.Abbregistration.NewProductCategory.Description : string.Empty;
                            }
                            #endregion
                            #region NewProductCategoryType details
                            if (tblAbbredemption.Abbregistration.NewProductCategoryTypeNavigation != null)
                            {
                                aBBRedemptionViewModel.NewProductCategoryType = tblAbbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description != null ? tblAbbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description : string.Empty;
                            }
                            #endregion
                            #region Abbregistration details
                            if (tblAbbredemption.Abbregistration != null)
                            {
                                aBBRedemptionViewModel.ProductNetPrice = tblAbbredemption.Abbregistration.ProductNetPrice != null ? tblAbbredemption.Abbregistration.ProductNetPrice : 0;
                                aBBRedemptionViewModel.ProductSrNo = tblAbbredemption.Abbregistration.ProductSrNo != null ? tblAbbredemption.Abbregistration.ProductSrNo : string.Empty;
                                aBBRedemptionViewModel.AbbplanName = tblAbbredemption.Abbregistration.AbbplanName != null ? tblAbbredemption.Abbregistration.AbbplanName : string.Empty;
                                aBBRedemptionViewModel.InvoiceNo = tblAbbredemption.Abbregistration.InvoiceNo != null ? tblAbbredemption.Abbregistration.InvoiceNo : string.Empty;
                                aBBRedemptionViewModel.InvoiceDateOnly = Convert.ToDateTime(tblAbbredemption.Abbregistration.InvoiceDate).ToString("dd/MM/yyyy");
                                aBBRedemptionViewModel.InvoiceImageName = tblAbbredemption.Abbregistration.InvoiceImage != null ? tblAbbredemption.Abbregistration.InvoiceImage : string.Empty;
                                aBBRedemptionViewModel.InvoiceImage = tblAbbredemption.Abbregistration.UploadImage != null ? tblAbbredemption.Abbregistration.UploadImage : string.Empty;
                            }
                            #endregion
                            #region Model details
                            if (tblAbbredemption.Abbregistration.ModelNumber != null)
                            {
                                aBBRedemptionViewModel.ModelNumber = tblAbbredemption.Abbregistration.ModelNumber.ModelName;
                            }
                            #endregion

                        }
                        else
                        {
                            return aBBRedemptionViewModel;
                        }
                    }
                    else
                    {
                        return aBBRedemptionViewModel;
                    }
                }

                else
                {
                    return aBBRedemptionViewModel;
                }
                //if (!string.IsNullOrEmpty(regdNo))
                //{
                //    tblOrderTran = _orderTransRepository.GetOrderTransByRegdno(  regdNo);

                //   // tblOrderTran = _orderTransRepository.GetSingle(x => x.IsActive == true && x.RegdNo.ToLower() == regdNo.ToLower());
                //    if (tblOrderTran != null)
                //    {
                //        aBBRedemptionViewModel.OrderTransId = tblOrderTran.OrderTransId;
                //        aBBRedemptionViewModel.RedemptionId = (int)tblOrderTran.AbbredemptionId;

                //    }
                //    tblAbbredemption = tblOrderTran.Abbredemption;

                //   // tblAbbredemption = _abbRedemptionRepository.GetAbbOrderDetails(regdNo);
                //    if (tblAbbredemption != null)
                //    {
                //        aBBRedemptionViewModel.RegdNo = tblAbbredemption.RegdNo != null ? tblAbbredemption.RegdNo : string.Empty;
                //        if (tblAbbredemption.Abbregistration.BusinessUnit != null)
                //        {
                //            int? newBrandId = tblAbbredemption.Abbregistration.NewBrandId;
                //            bool? Isbumultibrand = tblAbbredemption.Abbregistration.BusinessUnit.IsBumultiBrand;
                //            brandVM = _commonManager.GetAbbBrandDetailsById(Isbumultibrand, newBrandId);
                //            aBBRedemptionViewModel.BrandName = brandVM?.Name;


                //            //if (tblAbbredemption.Abbregistration.BusinessUnit.IsBumultiBrand == true)
                //            //{
                //            //    tblBrandSmartBuy = _context.TblBrandSmartBuys.Where(x => x.IsActive == true && x.Id == tblAbbredemption.Abbregistration.NewBrandId).FirstOrDefault();
                //            //    if (tblBrandSmartBuy != null)
                //            //    {
                //            //        aBBRedemptionViewModel.BrandName = tblBrandSmartBuy.Name;
                //            //    }
                //            //    else
                //            //    {
                //            //        aBBRedemptionViewModel.BrandName = "";
                //            //    }
                //            //}
                //            //else
                //            //{
                //            //    tblBrand = _context.TblBrands.Where(x => x.IsActive == true && x.Id == tblAbbredemption.Abbregistration.NewBrandId).FirstOrDefault();
                //            //    if (tblBrand != null)
                //            //    {
                //            //        aBBRedemptionViewModel.BrandName = tblBrand.Name;
                //            //    }
                //            //    else
                //            //    {
                //            //        aBBRedemptionViewModel.BrandName = "";
                //            //    }
                //            //}
                //            aBBRedemptionViewModel.CompanyName = tblAbbredemption.Abbregistration.BusinessUnit.Name != null ? tblAbbredemption.Abbregistration.BusinessUnit.Name : string.Empty;
                //        }
                //        else
                //        {
                //            aBBRedemptionViewModel.CompanyName = "";
                //        }
                //        if (tblAbbredemption.Abbregistration.BusinessPartner != null)
                //        {
                //            aBBRedemptionViewModel.StoreCode = tblAbbredemption.Abbregistration.BusinessPartner.StoreCode != null ? tblAbbredemption.Abbregistration.BusinessPartner.StoreCode : string.Empty;
                //            aBBRedemptionViewModel.StorePhoneNumber = tblAbbredemption.Abbregistration.BusinessPartner.PhoneNumber != null ? tblAbbredemption.Abbregistration.BusinessPartner.PhoneNumber : string.Empty;
                //            aBBRedemptionViewModel.StoreName = tblAbbredemption.Abbregistration.BusinessPartner.Name != null ? tblAbbredemption.Abbregistration.BusinessPartner.Name : string.Empty;
                //            aBBRedemptionViewModel.StoreAssociateCode = tblAbbredemption.Abbregistration.BusinessPartner.AssociateCode != null ? tblAbbredemption.Abbregistration.BusinessPartner.AssociateCode : string.Empty;
                //            aBBRedemptionViewModel.StoreEmail = tblAbbredemption.Abbregistration.BusinessPartner.Email != null ? tblAbbredemption.Abbregistration.BusinessPartner.Email : string.Empty;
                //        }
                //        else
                //        {
                //            aBBRedemptionViewModel.StoreCode = "";
                //            aBBRedemptionViewModel.StorePhoneNumber = "";
                //            aBBRedemptionViewModel.StoreName = "";
                //            aBBRedemptionViewModel.StoreAssociateCode = "";
                //            aBBRedemptionViewModel.StoreEmail = "";
                //        }
                //        if (tblAbbredemption.CustomerDetails != null)
                //        {
                //            aBBRedemptionViewModel.CustFirstName = tblAbbredemption.CustomerDetails.FirstName != null ? tblAbbredemption.CustomerDetails.FirstName : string.Empty;
                //            aBBRedemptionViewModel.CustLastName = tblAbbredemption.CustomerDetails.LastName != null ? tblAbbredemption.CustomerDetails.LastName : string.Empty;
                //            aBBRedemptionViewModel.CustEmail = tblAbbredemption.CustomerDetails.Email != null ? tblAbbredemption.CustomerDetails.Email : string.Empty;
                //            aBBRedemptionViewModel.CustCity = tblAbbredemption.CustomerDetails.City != null ? tblAbbredemption.CustomerDetails.City : string.Empty;
                //            aBBRedemptionViewModel.CustPinCode = tblAbbredemption.CustomerDetails.ZipCode != null ? tblAbbredemption.CustomerDetails.ZipCode : string.Empty;
                //            aBBRedemptionViewModel.CustAddress1 = tblAbbredemption.CustomerDetails.Address1 != null ? tblAbbredemption.CustomerDetails.Address1 : string.Empty;
                //            aBBRedemptionViewModel.CustAddress2 = tblAbbredemption.CustomerDetails.Address2 != null ? tblAbbredemption.CustomerDetails.Address2 : string.Empty;
                //            aBBRedemptionViewModel.CustMobile = tblAbbredemption.CustomerDetails.PhoneNumber != null ? tblAbbredemption.CustomerDetails.PhoneNumber : string.Empty;
                //            aBBRedemptionViewModel.CustState = tblAbbredemption.CustomerDetails.State != null ? tblAbbredemption.CustomerDetails.State : string.Empty;
                //        }
                //        else
                //        {
                //            aBBRedemptionViewModel.CustFirstName = "";
                //            aBBRedemptionViewModel.CustLastName = "";
                //            aBBRedemptionViewModel.CustEmail = "";
                //            aBBRedemptionViewModel.CustCity = "";
                //            aBBRedemptionViewModel.CustPinCode = "";
                //            aBBRedemptionViewModel.CustAddress1 = "";
                //            aBBRedemptionViewModel.CustAddress2 = "";
                //            aBBRedemptionViewModel.CustMobile = "";
                //            aBBRedemptionViewModel.CustState = "";
                //        }
                //        if (tblAbbredemption.Status != null)
                //        {
                //            aBBRedemptionViewModel.StatusId = tblAbbredemption.Status.Id;
                //            aBBRedemptionViewModel.StatusCode = tblAbbredemption.Status.StatusCode != null ? tblAbbredemption.Status.StatusCode : string.Empty;
                //            aBBRedemptionViewModel.StatusDescription = tblAbbredemption.Status.StatusDescription != null ? tblAbbredemption.Status.StatusDescription : string.Empty;
                //        }
                //        else
                //        {
                //            aBBRedemptionViewModel.StatusId = 0;
                //            aBBRedemptionViewModel.StatusCode = "";
                //            aBBRedemptionViewModel.StatusDescription = "";
                //        }
                //        aBBRedemptionViewModel.AbbCreatedDate = Convert.ToDateTime(tblAbbredemption.CreatedDate).ToString("dd/MM/yyyy");
                //        aBBRedemptionViewModel.AbbModifiedDate = Convert.ToDateTime(tblAbbredemption.ModifiedDate).ToString("dd/MM/yyyy");
                //        if (tblAbbredemption.Abbregistration.NewProductCategory != null)
                //        {
                //            aBBRedemptionViewModel.NewProductCategoryName = tblAbbredemption.Abbregistration.NewProductCategory.Description != null ? tblAbbredemption.Abbregistration.NewProductCategory.Description : string.Empty;
                //        }
                //        else
                //        {
                //            aBBRedemptionViewModel.NewProductCategoryName = "";
                //        }
                //        if (tblAbbredemption.Abbregistration.NewProductCategoryTypeNavigation != null)
                //        {
                //            aBBRedemptionViewModel.NewProductCategoryType = tblAbbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description != null ? tblAbbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description : string.Empty;
                //        }
                //        else
                //        {
                //            aBBRedemptionViewModel.NewProductCategoryType = "";
                //        }
                //        if (tblAbbredemption.Abbregistration != null)
                //        {
                //            aBBRedemptionViewModel.ProductNetPrice = tblAbbredemption.Abbregistration.ProductNetPrice != null ? tblAbbredemption.Abbregistration.ProductNetPrice : 0;
                //            aBBRedemptionViewModel.ProductSrNo = tblAbbredemption.Abbregistration.ProductSrNo != null ? tblAbbredemption.Abbregistration.ProductSrNo : string.Empty;
                //            aBBRedemptionViewModel.AbbplanName = tblAbbredemption.Abbregistration.AbbplanName != null ? tblAbbredemption.Abbregistration.AbbplanName : string.Empty;
                //            aBBRedemptionViewModel.InvoiceNo = tblAbbredemption.Abbregistration.InvoiceNo != null ? tblAbbredemption.Abbregistration.InvoiceNo : string.Empty;
                //            aBBRedemptionViewModel.InvoiceDateOnly = Convert.ToDateTime(tblAbbredemption.Abbregistration.InvoiceDate).ToString("dd/MM/yyyy");
                //            aBBRedemptionViewModel.InvoiceImageName = tblAbbredemption.Abbregistration.InvoiceImage != null ? tblAbbredemption.Abbregistration.InvoiceImage : string.Empty;
                //            aBBRedemptionViewModel.InvoiceImage = tblAbbredemption.Abbregistration.UploadImage != null ? tblAbbredemption.Abbregistration.UploadImage : string.Empty;
                //        }
                //        else
                //        {
                //            aBBRedemptionViewModel.ProductNetPrice = 0;
                //            aBBRedemptionViewModel.ProductSrNo = "";
                //            aBBRedemptionViewModel.AbbplanName = "";
                //            aBBRedemptionViewModel.InvoiceNo = "";
                //            aBBRedemptionViewModel.InvoiceDateOnly = "";
                //            aBBRedemptionViewModel.InvoiceImageName = "";
                //            aBBRedemptionViewModel.InvoiceImage = "";
                //        }
                //        if (tblAbbredemption.Abbregistration.ModelNumber != null)
                //        {
                //            aBBRedemptionViewModel.ModelNumber = tblAbbredemption.Abbregistration.ModelNumber.ModelName;
                //        }
                //        else
                //        {
                //            aBBRedemptionViewModel.ModelNumber = "";
                //        }
                //        aBBRedemptionViewModel.RedemptionValue = (decimal)(tblAbbredemption.RedemptionValue != null ? tblAbbredemption.RedemptionValue : 0);
                //    }
                //}
                //else
                //{
                //    return aBBRedemptionViewModel;
                //}

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRedemptionManager", "GetRedemptionPeriod", ex);
            }
            return aBBRedemptionViewModel;

        }
        #endregion

        #region Genereate Voucher
        /// <summary>
        /// Method to generate the voucher code
        /// </summary>
        /// <param name="buCode">business unit code</param>
        /// <returns>string</returns>
        public string GenerateVoucher()
        {
            string code = null;

            try
            {
                code = "V" + UniqueString.RandomNumberByLength(8);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRedemptionManager", "GenerateVoucher", ex);
            }

            return code;
        }
        #endregion

        #region GetVoucherDataForRedemption
        public RedemptionDataContract GetOrderData(int Id)
        {

            RedemptionDataContract RedemptionDataDC = new RedemptionDataContract();
            TblAbbredemption redemptionObj = null;
            TblAbbregistration registrationObj = null;
            TblBusinessUnit businessUnitObj = null;
            try
            {
                if (Id > 0)
                {
                    redemptionObj = _abbRedemptionRepository.GetRedemptionData(Id);
                    if (redemptionObj != null)
                    {
                        RedemptionDataDC.VoucherCode = redemptionObj.VoucherCode;
                        RedemptionDataDC.VoucherCodeExpDate = redemptionObj.VoucherCodeExpDate;
                        RedemptionDataDC.RedemptionValue = redemptionObj.RedemptionValue;
                        RedemptionDataDC.BusinessUnitId = redemptionObj.Abbregistration?.BusinessUnitId;
                        RedemptionDataDC.BULogoName = redemptionObj?.Abbregistration?.BusinessUnit?.LogoName;

                    }
                    else
                    {
                        RedemptionDataDC.ErrorMessage = "ABB redemption data not found";
                    }
                }
                else
                {
                    RedemptionDataDC.ErrorMessage = "order id is not provided";
                }
            }
            catch (Exception ex)
            {
                RedemptionDataDC.ErrorMessage = ex.Message;
                _logging.WriteErrorToDB("AbbRedemptionManager", "GetOrderData", ex);
            }
            return RedemptionDataDC;
        }

    }
    #endregion
}

