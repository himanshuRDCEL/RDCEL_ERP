using AutoMapper;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.EcomVoucher;
using RDCELERP.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace RDCELERP.BAL.MasterManager
{
    public class EcomVoucherManager : IEcomVoucherManager
    {
        IEcomVoucherRepository _ecomVoucherRepository;
        IEcomPhoneSpecificsRepository   _ecomPhoneSpecificsRepository;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        private CustomDataProtection _protector;
        IErrorLogManager _errorLogManager;
        IMailManager _mailManager;
        IMapper _mapper;
        ILogging _logging;
        IOptions<ApplicationSettings> _config;
        public EcomVoucherManager(IErrorLogManager errorLogManager,
        IMailManager mailManager,IMapper mapper,ILogging logging,
        IOptions<ApplicationSettings> config, IEcomVoucherRepository ecomVoucherRepository, CustomDataProtection protector,IEcomPhoneSpecificsRepository  ecomPhoneSpecificsRepository) {
            _ecomVoucherRepository = ecomVoucherRepository;
            _protector = protector;
            _errorLogManager = errorLogManager;
            _mailManager = mailManager;
            _mapper = mapper;
            _logging = logging;
            _config = config;
            _ecomPhoneSpecificsRepository = ecomPhoneSpecificsRepository;
        }

        /// <summary>
        /// Method to manage (Add/Edit) voucher 
        /// </summary>
        /// <param name="EcomVM">EcomVM</param>
        /// <param name="EcomVMId">EcomVMId</param>
        /// <returns>int</returns>
      public  bool ManageEcomVoucher(EcomVoucherViewModel EcomVM, int userId, int? companyId)
        {
            TblEcomVoucher TblEcomVoucher = new TblEcomVoucher();
            bool flag = false;
            try
            {
                if (EcomVM != null)
                {

                    TblEcomVoucher = _mapper.Map<EcomVoucherViewModel, TblEcomVoucher>(EcomVM);
                    TblEcomVoucher.Phoneno = TblEcomVoucher.Phoneno?.Trim();
                    if (TblEcomVoucher.Phoneno != null)
                    {
                        TblEcomVoucher.Phoneno = SecurityHelper.EncryptString(TblEcomVoucher.Phoneno, _config.Value.SecurityKey);
                        TblEcomVoucher.Phoneno = TblEcomVoucher.Phoneno?.Trim();

                    }
                    if (TblEcomVoucher.EcomVoucherId > 0 && EcomVM.EcomVoucherType == Convert.ToInt32(EcomVoucherTypeEnum.BrandSpecificVoucher))
                    {
                        TblEcomVoucher.ModifiedBy = userId;
                        TblEcomVoucher.ModifiedDate = _currentDatetime;
                        _ecomVoucherRepository.Update(TblEcomVoucher);
                    }
                    else
                    {
                        if (EcomVM.EcomPhoneSpecificsListVM != null && EcomVM.EcomPhoneSpecificsListVM.Any() && EcomVM.EcomVoucherType==Convert.ToInt32(EcomVoucherTypeEnum.PhoneSpecificVoucher))
                        {
                            flag= ManagePhoneSpecificVoucher(EcomVM,userId,companyId);         
                        }

                      else  if (EcomVM.VoucherCount != null && EcomVM.VoucherCount > 0 && EcomVM.EcomVoucherType == Convert.ToInt32(EcomVoucherTypeEnum.GenericVoucher))
                        {
                            flag = ManageGenericVoucher(EcomVM,userId,companyId);
                        }
                        else  if (EcomVM.EcomVoucherType == Convert.ToInt32(EcomVoucherTypeEnum.BrandSpecificVoucher))
                            {

                            //Code to Insert the object
                           
                            TblEcomVoucher.IsActive = true;
                            TblEcomVoucher.Voucherstatus = EcomVoucherStatusEnum.Generated.ToString(); ;
                            TblEcomVoucher.VoucherCode = GenerateVoucher();
                            TblEcomVoucher.CreatedDate = _currentDatetime;
                            TblEcomVoucher.CreatedBy = userId;
                            TblEcomVoucher.CompanyId = companyId;
                            _ecomVoucherRepository.Create(TblEcomVoucher);
                        }
                    }
                    _ecomVoucherRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EcomVoucherManager", "ManageEcomVoucher", ex);
            }

            return flag;
        }

        /// <summary>
        /// Method to get the User by id 
        /// </summary>
        /// <param name="id">UserId</param>
        /// <returns>EcomVoucherViewModel</returns>
       public EcomVoucherViewModel GetEcomVoucherById(int id)
        {
            EcomVoucherViewModel EcomVM = null;
            TblEcomVoucher TblEcomVoucher = null;

            try
            {

                TblEcomVoucher = _ecomVoucherRepository.GetSingle(x => x.IsActive == true && x.EcomVoucherId == id);

                if (TblEcomVoucher != null)
                {
                    
                    EcomVM = _mapper.Map<TblEcomVoucher, EcomVoucherViewModel>(TblEcomVoucher);

                    if (EcomVM != null)
                    {
                        if (EcomVM.VoucherCode != null)
                        {
                            EcomVM.VoucherCode = StringHelper.MaskVoucherCode(EcomVM.VoucherCode);

                        }

                        List<TblEcomPhoneSpecific> tblEcomPhone=_ecomPhoneSpecificsRepository.GetList(x=>x.IsActive == true && x.EcomVoucherId==EcomVM.EcomVoucherId).ToList();

                        if (tblEcomPhone != null && tblEcomPhone.Count>0)
                        {
                            EcomVM.EcomPhoneSpecificsListVM = _mapper.Map<List<TblEcomPhoneSpecific>,List<EcomPhoneSpecificsViewModel>>(tblEcomPhone);
                            foreach(var item in EcomVM.EcomPhoneSpecificsListVM)
                            {
                                if (item.Phoneno != null)
                                {
                                    item.Phoneno=SecurityHelper.DecryptString(item.Phoneno, _config.Value.SecurityKey);
                                  //  item.VoucherCode = StringHelper.MaskVoucherCode(item.VoucherCode);

                                }
                            }
                        }
                        }

                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EcomVoucherManager", "GetEcomVoucherById", ex);
            }
            return EcomVM;
        }

        /// <summary>
        /// methd to manage Genenric type voucher
        /// </summary>
        /// <param name="EcomVM"></param>
        /// <returns></returns>
        public bool ManageGenericVoucher(EcomVoucherViewModel EcomVM, int? userId, int? companyId)
        {
            bool flag = false;
            try
            {
                if(EcomVM != null && EcomVM.VoucherCount!=null) {
                for (int i = 0; i < EcomVM.VoucherCount; i++)
                {

                    TblEcomVoucher voucher = new TblEcomVoucher
                    {

                        IsActive = true,
                        Voucherstatus = EcomVoucherStatusEnum.Generated.ToString(),
                        VoucherCode = GenerateVoucher(),
                        CreatedDate = _currentDatetime,
                        CreatedBy= userId,
                        StartDate = EcomVM.StartDate,
                        EndDate = EcomVM.EndDate,
                        CompanyId = companyId,
                        ValueType = EcomVM.ValueType,
                        FixedValue = EcomVM.FixedValue,
                        Percentage = EcomVM.Percentage,
                        PercLimit = EcomVM.PercLimit,
                        EcomVoucherType = Convert.ToInt32(EcomVoucherTypeEnum.GenericVoucher)
                    };

                    _ecomVoucherRepository.Create(voucher);
                    _ecomVoucherRepository.SaveChanges();
                        flag = true;

                }

            }
            }
            catch (Exception ex)
            {

            }
            return flag;
        }

        public bool ManagePhoneSpecificVoucher(EcomVoucherViewModel EcomVM,int? userId, int? companyId)
        {
            bool flag = false;
            int ecomVoucherId = 0;
            try
            {
                if (EcomVM != null && EcomVM.EcomPhoneSpecificsListVM != null)
                {
                    TblEcomVoucher voucher = new TblEcomVoucher
                    {

                        IsActive = true,
                        Voucherstatus = EcomVoucherStatusEnum.Generated.ToString(),
                        BrandId = EcomVM.BrandId,
                        CategoryIds = EcomVM.CategoryIds,
                        StartDate = EcomVM.StartDate,
                        EndDate = EcomVM.EndDate,
                        ValueType = EcomVM.ValueType,
                        FixedValue = EcomVM.FixedValue,
                        Percentage = EcomVM.Percentage,
                        PercLimit = EcomVM.PercLimit,
                        CreatedDate = _currentDatetime,
                        CreatedBy = userId,
                        CompanyId = companyId,
                        EcomVoucherType = Convert.ToInt32(EcomVoucherTypeEnum.PhoneSpecificVoucher)
                    };
                    _ecomVoucherRepository.Create(voucher);
                    _ecomVoucherRepository.SaveChanges();
                    ecomVoucherId = voucher.EcomVoucherId;

                }


                if (ecomVoucherId > 0)
                {

                    foreach (var item in EcomVM.EcomPhoneSpecificsListVM)
                    {
                        TblEcomPhoneSpecific pHvoucher = new TblEcomPhoneSpecific
                        {
                            Phoneno = SecurityHelper.EncryptString(item.Phoneno, _config.Value.SecurityKey),
                            Voucherstatus = EcomVoucherStatusEnum.Generated.ToString(),
                            VoucherCode = GenerateVoucher(),
                            EcomVoucherId=ecomVoucherId,
                            IsActive = true,
                            CreatedDate = _currentDatetime,
                            CreatedBy = userId,
                        };
                        _ecomPhoneSpecificsRepository.Create(pHvoucher);
                    }
                    _ecomPhoneSpecificsRepository.SaveChanges();
                    flag = true;
                }
            
            }
            catch (Exception ex)
            {

            }
            return flag;
        }
        public string GenerateVoucher()
        {
            string code = null;

            try
            {
                code = "V" + UniqueString.RandomNumberByLength(8);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EcomVoucherManager", "GenerateVoucher", ex);
            }

            return code;
        }

    }
}
