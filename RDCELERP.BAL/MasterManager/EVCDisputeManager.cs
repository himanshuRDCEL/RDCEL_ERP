using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.EVC;
using RDCELERP.Model.EVCdispute;
using RDCELERP.Model.InfoMessage;

namespace RDCELERP.BAL.MasterManager
{
    public class EVCDisputeManager : IEVCDisputeManager
    {
        #region  Variable Declaration
        IUserRepository _userRepository;
        IUserRoleRepository _userRoleRepository;
        IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        private DAL.Entities.Digi2l_DevContext _context;
        IEVCRepository _evcRepository;
        IEntityRepository _entityRepository;
        IWalletTransactionRepository _walletTransactionRepository;
        IExchangeOrderRepository _exchangeOrderRepository;
        IEVCWalletAdditionRepository _EVCWalletAdditionRepository;
        IEVCDisputeRepository _eVCDisputeRepository;
        IProductTypeRepository _productTypeRepository;
        IOrderTransRepository _OrderTransRepository;
        IWebHostEnvironment _webHostEnvironment;
        ILovRepository _ilovrepository;
        IOrderImageUploadRepository _orderImageUploadRepository;
        IImageHelper _imageHelper;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion

        #region Constructor
        public EVCDisputeManager(IEVCRepository evcRepository, DAL.Entities.Digi2l_DevContext context,
            IWalletTransactionRepository walletTransactionRepository,
            IUserRoleRepository userRoleRepository, IRoleRepository roleRepository,
            IMapper mapper, ILogging logging,
            IEntityRepository entityRepository,
            IEVCWalletAdditionRepository eVCWalletAdditionRepository,
            IExchangeOrderRepository exchangeOrderRepository,
            IEVCDisputeRepository eVCDisputeRepository,
            IProductTypeRepository productTypeRepository,
            IOrderTransRepository orderTransRepository,
            IWebHostEnvironment webHostEnvironment,
            ILovRepository ilovrepository,
            IOrderImageUploadRepository orderImageUploadRepository, IImageHelper imageHelper)
        {
            _orderImageUploadRepository = orderImageUploadRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logging = logging;
            _evcRepository = evcRepository;
            _entityRepository = entityRepository;
            _walletTransactionRepository = walletTransactionRepository;
            _context = context;
            _EVCWalletAdditionRepository = eVCWalletAdditionRepository;
            _exchangeOrderRepository = exchangeOrderRepository;
            _eVCDisputeRepository = eVCDisputeRepository;
            _productTypeRepository = productTypeRepository;
            _OrderTransRepository = orderTransRepository;
            _webHostEnvironment = webHostEnvironment;
            _ilovrepository = ilovrepository;
            _imageHelper = imageHelper;
        }
        #endregion

        /// <summary>
        /// Update by Ashwin : for adding ordertransid,image save with file name on tblEvcdispute
        /// </summary>
        /// <param name="EVCDisputeViewModels"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int SaveEVCDisputeDetails(EVCDisputeViewModel EVCDisputeViewModels, int userId)
        {
            #region Variable Declaration
            TblEvcdispute TblEvcdispute = new TblEvcdispute();
            TblOrderImageUpload tblOrderImageUpload = new TblOrderImageUpload();
            TblOrderImageUpload tblOrderImageUpload1 = new TblOrderImageUpload();
            TblOrderImageUpload tblOrderImageUpload2 = new TblOrderImageUpload();
            string imageUploadedBy = "EVC_Dispute";
            TblLoV tblLoV = null;
            var filePath = string.Empty;
            #region Common Implementations for (ABB or Exchange)
            TblExchangeOrder tblExchangeOrder = null;
            TblAbbredemption tblAbbredemption = null;
            TblAbbregistration tblAbbregistration = null;
            string regdNo = null; int? productTypeId = null; int? productCatId = null;
            TblOrderTran tblOrderTrans = null;
            #endregion
            #endregion
            try
            {
                if (EVCDisputeViewModels != null)
                {
                    tblLoV = _ilovrepository.GetSingle(x => x.IsActive == true && x.LoVname == imageUploadedBy);
                    //TblEvcdispute = _mapper.Map<EVCDisputeViewModel, TblEvcdispute>(EVCDisputeViewModels);

                    if (TblEvcdispute.EvcdisputeId > 0)
                    {
                        //Code to update the object

                        TblEvcdispute.ModifiedBy = userId;
                        TblEvcdispute.ModifiedDate = _currentDatetime;
                        _eVCDisputeRepository.Update(TblEvcdispute);
                    }
                    else
                    {
                        var Check = _eVCDisputeRepository.GetSingle(x => x.OrderTransId == EVCDisputeViewModels.orderTransId);
                        if (Check == null)
                        {
                            //Code to Insert the object                             
                            TblEvcdispute.IsActive = true;
                            TblEvcdispute.CreatedDate = _currentDatetime;
                            TblEvcdispute.CreatedBy = userId;

                            var GetEvcDetails = _evcRepository.GetSingle(x => x.EvcregistrationId == EVCDisputeViewModels.EvcregistrationId);
                            var GetWalletTransactionDetails = _walletTransactionRepository.GetSingle(x => x.EvcregistrationId == EVCDisputeViewModels.EvcregistrationId && x.OrderTransId == EVCDisputeViewModels.orderTransId);

                            if (EVCDisputeViewModels.orderTransId > 0)
                            {
                                #region Common Implementations for (ABB or Exchange)
                                tblOrderTrans = _OrderTransRepository.GetOrderDetailsByOrderTransId(EVCDisputeViewModels.orderTransId);
                                if (tblOrderTrans != null)
                                {
                                    regdNo = tblOrderTrans.RegdNo;
                                    if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                                    {
                                        tblExchangeOrder = tblOrderTrans.Exchange;
                                        if (tblExchangeOrder != null)
                                        {
                                            productTypeId = tblExchangeOrder.ProductTypeId;
                                            productCatId = tblExchangeOrder.ProductType.ProductCatId;
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
                                                productCatId = tblAbbregistration.NewProductCategoryId;
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }

                            TblEvcdispute.EvcregistrationId = EVCDisputeViewModels.EvcregistrationId;
                            TblEvcdispute.Evcdisputedescription = EVCDisputeViewModels.Evcdisputedescription;
                            TblEvcdispute.Status = EVCDisputeViewModels.Status;
                            TblEvcdispute.OrderTransId = EVCDisputeViewModels.orderTransId;

                            TblEvcdispute.ProductTypeId = productTypeId;
                            TblEvcdispute.ProductCatId = productCatId;
                            TblEvcdispute.Amount = (decimal)GetWalletTransactionDetails.OrderAmount;

                            #region SaveDispute Images
                            filePath = @"\DBFiles\EVC\EVCUser_Dispute";
                            if (EVCDisputeViewModels.Image1 != null)
                            {
                                var image1Name = regdNo + "_" + "DisputeImage1" + ".jpg";
                                bool image1Saved = _imageHelper.SaveFileFromBase64(EVCDisputeViewModels.Image1, filePath, image1Name);
                                if (image1Saved)
                                {
                                    TblEvcdispute.Image1 = image1Name;
                                    tblOrderImageUpload.ImageName = TblEvcdispute.Image1;
                                    tblOrderImageUpload.ImageUploadby = tblLoV.LoVid;
                                    tblOrderImageUpload.OrderTransId = EVCDisputeViewModels.orderTransId;
                                    tblOrderImageUpload.IsActive = true;
                                    tblOrderImageUpload.LgcpickDrop = "EVCDispute";
                                    tblOrderImageUpload.CreatedDate = DateTime.Now;
                                    _orderImageUploadRepository.Create(tblOrderImageUpload);
                                }
                            }
                            if (EVCDisputeViewModels.Image2 != null)
                            {
                                var image2Name = regdNo + "_" + "DisputeImage2" + ".jpg";
                                bool image2Saved = _imageHelper.SaveFileFromBase64(EVCDisputeViewModels.Image2, filePath, image2Name);
                                if (image2Saved)
                                {
                                    TblEvcdispute.Image2 = image2Name;
                                    tblOrderImageUpload1.ImageName = TblEvcdispute.Image2;
                                    tblOrderImageUpload1.ImageUploadby = tblLoV.LoVid;
                                    tblOrderImageUpload1.OrderTransId = EVCDisputeViewModels.orderTransId;
                                    tblOrderImageUpload1.IsActive = true;
                                    tblOrderImageUpload1.CreatedDate = DateTime.Now;
                                    tblOrderImageUpload1.LgcpickDrop = "EVCDispute";
                                    _orderImageUploadRepository.Create(tblOrderImageUpload1);
                                }
                            }
                            if (EVCDisputeViewModels.Image3 != null)
                            {
                                var image3Name = regdNo + "_" + "DisputeImage3" + ".jpg";
                                bool image3Saved = _imageHelper.SaveFileFromBase64(EVCDisputeViewModels.Image3, filePath, image3Name);
                                if (image3Saved)
                                {
                                    TblEvcdispute.Image3 = image3Name;
                                    tblOrderImageUpload2.ImageName = TblEvcdispute.Image3;
                                    tblOrderImageUpload2.ImageUploadby = tblLoV.LoVid;
                                    tblOrderImageUpload2.OrderTransId = EVCDisputeViewModels.orderTransId;
                                    tblOrderImageUpload2.IsActive = true;
                                    tblOrderImageUpload2.CreatedDate = DateTime.Now;
                                    tblOrderImageUpload2.LgcpickDrop = "EVCDispute";
                                    _orderImageUploadRepository.Create(tblOrderImageUpload2);
                                }
                            }
                            _orderImageUploadRepository.SaveChanges();
                            #endregion

                            TblEvcdispute.DisputeRegno = GetEvcDetails.EvcregdNo + "-" + UniqueString.RandomNumber() + "-" + regdNo;
                            _eVCDisputeRepository.Create(TblEvcdispute);
                        }
                        _eVCDisputeRepository.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "SaveEVCDisputeDetails", ex);
            }
            return TblEvcdispute.EvcdisputeId;
        }
        public EVCDisputeViewModel GetDisputeByEVCDisputeId(int id)
        {
            EVCDisputeViewModel eVCDisputeViewModel = null;
            TblEvcdispute TblEvcdispute = null;

            try
            {
                TblEvcdispute = _eVCDisputeRepository.GetSingle(where: x => x.IsActive == true && x.EvcdisputeId == id);
                if (TblEvcdispute != null)
                {
                    eVCDisputeViewModel = _mapper.Map<TblEvcdispute, EVCDisputeViewModel>(TblEvcdispute);

                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCDisputeManager", "GetDisputeByEVCDisputeId", ex);
            }
            return eVCDisputeViewModel;
        }

        #region SaveEVCDisputeDetailsForAdmin Modified and Optimized by VK for ABB Redemption Date(12-July-2023)
        public int SaveEVCDisputeDetailsForAdmin(EVCDisputeViewModel EVCDisputeViewModels, int userId)
        {
            TblEvcdispute TblEvcdispute = new TblEvcdispute();
            #region Common Implementations for (ABB or Exchange)
            TblExchangeOrder tblExchangeOrder = null;
            TblAbbredemption tblAbbredemption = null;
            TblAbbregistration tblAbbregistration = null;
            string regdNo = null; int? productTypeId = null; int? productCatId = null;
            TblOrderTran tblOrderTrans = null;
            #endregion
            try
            {
                if (EVCDisputeViewModels != null)
                {
                    TblEvcdispute = _mapper.Map<EVCDisputeViewModel, TblEvcdispute>(EVCDisputeViewModels);

                    if (TblEvcdispute.EvcdisputeId > 0)
                    {
                        //Code to update the object
                        TblEvcdispute.ModifiedBy = userId;
                        TblEvcdispute.ModifiedDate = _currentDatetime;
                        _eVCDisputeRepository.Update(TblEvcdispute);
                    }
                    else
                    {
                        var Check = _eVCDisputeRepository.GetSingle(x => x.OrderTransId == EVCDisputeViewModels.orderTransId);
                        if (Check == null)
                        {
                            //Code to Insert the object                             
                            TblEvcdispute.IsActive = true;
                            TblEvcdispute.CreatedDate = _currentDatetime;
                            TblEvcdispute.CreatedBy = userId;
                            var GetEvcDetails = _evcRepository.GetSingle(x => x.EvcregistrationId == EVCDisputeViewModels.EvcregistrationId);
                            var GetWalletTransactionDetails = _walletTransactionRepository.GetSingle(x => x.EvcregistrationId == EVCDisputeViewModels.EvcregistrationId && x.OrderTransId == EVCDisputeViewModels.orderTransId);

                            if (EVCDisputeViewModels.orderTransId > 0)
                            {
                                #region Common Implementations for (ABB or Exchange)
                                tblOrderTrans = _OrderTransRepository.GetOrderDetailsByOrderTransId(EVCDisputeViewModels.orderTransId);
                                if (tblOrderTrans != null)
                                {
                                    regdNo = tblOrderTrans.RegdNo;
                                    if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                                    {
                                        tblExchangeOrder = tblOrderTrans.Exchange;
                                        if (tblExchangeOrder != null)
                                        {
                                            productTypeId = tblExchangeOrder.ProductTypeId;
                                            productCatId = tblExchangeOrder.ProductType.ProductCatId;
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
                                                productCatId = tblAbbregistration.NewProductCategoryId;
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }

                            TblEvcdispute.EvcregistrationId = EVCDisputeViewModels.EvcregistrationId;
                            TblEvcdispute.Evcdisputedescription = EVCDisputeViewModels.Evcdisputedescription;
                            TblEvcdispute.Status = EVCDisputeViewModels.Status;
                            TblEvcdispute.OrderTransId = EVCDisputeViewModels.orderTransId;
                            TblEvcdispute.ProductTypeId = productTypeId;
                            TblEvcdispute.ProductCatId = productCatId;
                            TblEvcdispute.Amount = (decimal)GetWalletTransactionDetails.OrderAmount;
                            TblEvcdispute.DisputeRegno = GetEvcDetails.EvcregdNo + "-" + UniqueString.RandomNumber() + "-" + regdNo;
                            _eVCDisputeRepository.Create(TblEvcdispute);
                        }
                    }
                    _eVCDisputeRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EVCManager", "SaveEVCDisputeDetailsForAdmin", ex);
            }
            return TblEvcdispute.EvcdisputeId;
        }
        #endregion


        
    }
}

