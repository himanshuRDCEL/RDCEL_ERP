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
using RDCELERP.Model.BusinessUnit;
using RDCELERP.BAL.Helper;
using RDCELERP.Model.Refurbisher;

namespace RDCELERP.BAL.MasterManager
{
    public class RefurbisherManager : IRefurbisherManager
    {
        #region  Variable Declaration
        
        private readonly IMapper _mapper;
        ILogging _logging;       
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IRefurbisherRepository _refurbisherRepository;
        #endregion

        public RefurbisherManager(IMapper mapper, ILogging logging, IRefurbisherRepository refurbisherRepository)
        {
            _mapper = mapper;
            _logging = logging;
            _refurbisherRepository = refurbisherRepository;
        }

        /// <summary>
        /// Method to manage (Add/Edit) Company 
        /// </summary>
        /// <param name="RefurbisherViewModel">RefurbisherViewModel</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageRefurbisher(RefurbisherRegViewModel RefurbisherViewModel, int userId)
        {
            TblRefurbisherRegistration TblRefurbisherRegistration = new TblRefurbisherRegistration();
            try
            {
                if (RefurbisherViewModel != null)
                {
                    TblRefurbisherRegistration = _mapper.Map<RefurbisherRegViewModel, TblRefurbisherRegistration>(RefurbisherViewModel);
                    
                    if (TblRefurbisherRegistration.RefurbisherId > 0)
                    {
                        //Code to update the object
                        TblRefurbisherRegistration.IsActive = true;
                        TblRefurbisherRegistration.ModifiedBy = userId;
                        TblRefurbisherRegistration.ModifiedDate = _currentDatetime;
                        _refurbisherRepository.Update(TblRefurbisherRegistration);
                    }
                    else
                    {
                        //Code to Insert the object 
                        TblRefurbisherRegistration.IsActive = true;
                        TblRefurbisherRegistration.CreatedDate = _currentDatetime;
                        TblRefurbisherRegistration.CreatedBy = userId;
                        _refurbisherRepository.Create(TblRefurbisherRegistration);
                    }
                    _refurbisherRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RefurbisherManager", "ManageRefurbisher", ex);
            }

            return TblRefurbisherRegistration.RefurbisherId;
        }

        /// <summary>
        /// Method to get the Company by id 
        /// </summary>
        /// <param name="id">CompanyId</param>
        /// <returns>RefurbisherRegViewModel</returns>
        public RefurbisherRegViewModel GetRefurbisherById(int RefurbisherId)
        {
            RefurbisherRegViewModel? RefurbisherViewModel = null;
            TblRefurbisherRegistration? TblRefurbisherRegistration = null;

            try
            {
                TblRefurbisherRegistration = _refurbisherRepository.GetSingle(x => x.IsActive == true && x.RefurbisherId == RefurbisherId);
                if (TblRefurbisherRegistration != null)
                {
                    RefurbisherViewModel = _mapper.Map<TblRefurbisherRegistration, RefurbisherRegViewModel>(TblRefurbisherRegistration);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RefurbisherManager", "GetRefurbisherById", ex);
            }
            return RefurbisherViewModel;
        }

        /// <summary>
        /// Method to delete Company by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletRefurbisherById(int RefurbisherId)
        {
            bool flag = false;
            try
            {
                TblRefurbisherRegistration TblRefurbisherRegistration = _refurbisherRepository.GetSingle(x => x.IsActive == true && x.RefurbisherId == RefurbisherId);
                if (TblRefurbisherRegistration != null)
                {
                    TblRefurbisherRegistration.IsActive = false;
                    _refurbisherRepository.Update(TblRefurbisherRegistration);
                    _refurbisherRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RefurbisherManager", "DeletRefurbisherById", ex);
            }
            return flag;
        }

    }
}
