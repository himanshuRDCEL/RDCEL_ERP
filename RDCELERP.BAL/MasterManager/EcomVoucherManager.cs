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
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        private CustomDataProtection _protector;
        IErrorLogManager _errorLogManager;
        IMailManager _mailManager;
        IMapper _mapper;
        ILogging _logging;
        IOptions<ApplicationSettings> _config;
        public EcomVoucherManager(IErrorLogManager errorLogManager,
        IMailManager mailManager,IMapper mapper,ILogging logging,
        IOptions<ApplicationSettings> config, IEcomVoucherRepository ecomVoucherRepository, CustomDataProtection protector) {
            _ecomVoucherRepository = ecomVoucherRepository;
            _protector = protector;
            _errorLogManager = errorLogManager;
            _mailManager = mailManager;
            _mapper = mapper;
            _logging = logging;
            _config = config;
        }

        /// <summary>
        /// Method to manage (Add/Edit) voucher 
        /// </summary>
        /// <param name="EcomVM">EcomVM</param>
        /// <param name="EcomVMId">EcomVMId</param>
        /// <returns>int</returns>
      public  int ManageEcomVoucher(EcomVoucherViewModel EcomVM, int userId, int? companyId)
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
                        if (EcomVM.PhoneNumbers != null && EcomVM.PhoneNumbers.Any() && EcomVM.EcomVoucherType==Convert.ToInt32(EcomVoucherTypeEnum.PhoneSpecificVoucher))
                        {
                            ManagePhoneSpecificVoucher(EcomVM,userId,companyId);         
                    }

                        if (EcomVM.VoucherCount != null && EcomVM.VoucherCount > 0 && EcomVM.EcomVoucherType == Convert.ToInt32(EcomVoucherTypeEnum.GenericVoucher))
                        {
                            flag = ManageGenericVoucher(EcomVM,userId,companyId);
                        }
                        else  if (EcomVM.Phoneno != null && EcomVM.EcomVoucherType == Convert.ToInt32(EcomVoucherTypeEnum.BrandSpecificVoucher))
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
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EcomVoucherManager", "ManageEcomVoucher", ex);
            }

            return TblEcomVoucher.EcomVoucherId;
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
                        StartDate = EcomVM.StartDate,
                        EndDate = EcomVM.EndDate,
                        CompanyId = EcomVM.CompanyId,
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
            try
            {
                if (EcomVM != null && EcomVM.PhoneNumbers != null)
                {
                    foreach (var item in EcomVM.PhoneNumbers)
                    {
                        TblEcomVoucher voucher = new TblEcomVoucher
                        {
                            Phoneno = SecurityHelper.EncryptString(item.PhoneNumber, _config.Value.SecurityKey),
                            IsActive = true,
                            Voucherstatus = EcomVoucherStatusEnum.Generated.ToString(),
                            VoucherCode = GenerateVoucher(),
                            StartDate =EcomVM.StartDate,
                            EndDate =EcomVM.EndDate,
                            ValueType =EcomVM.ValueType,
                            FixedValue =EcomVM.FixedValue,
                            Percentage =EcomVM.Percentage,
                            PercLimit =EcomVM.PercLimit,
                            CreatedDate = _currentDatetime,
                            CreatedBy = userId,
                            CompanyId = companyId,
                            EcomVoucherType = Convert.ToInt32(EcomVoucherTypeEnum.PhoneSpecificVoucher)
                        };
                        _ecomVoucherRepository.Create(voucher);
                    }
                    _ecomVoucherRepository.SaveChanges();
flag=true;
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
