using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.StoreCode;

namespace RDCELERP.BAL.MasterManager
{
    public class StoreCodeManager : IStoreCodeManager
    {

        #region  Variable Declaration
        IStoreCodeRepository _storeCodeRepository;
        IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion
        public StoreCodeManager(IStoreCodeRepository storeCodeRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging)
        {
            IStoreCodeRepository IStoreCodeRepository = storeCodeRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
            _logging = logging;
        }

        /// <summary>
        /// Method to manage (Add/Edit) StoreCode 
        /// </summary>
        /// <param name="StoreCodeVM">StoreCodeVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageStoreCode(StoreCodeViewModel StoreCodeVM, int userId)
        {
            TblStoreCode TblStoreCode = new TblStoreCode();

            try
            {
                if (StoreCodeVM != null)
                {
                    TblStoreCode = _mapper.Map<StoreCodeViewModel, TblStoreCode>(StoreCodeVM);
                    if (TblStoreCode.StoreCodeId > 0)
                    {
                        //Code to update the object                      
                        TblStoreCode.ModifiedBy = userId;
                        TblStoreCode.ModifiedDate = _currentDatetime;
                        _storeCodeRepository.Update(TblStoreCode);
                    }
                    else
                    {
                        //Code to Insert the object 
                        TblStoreCode.IsActive = true;
                        TblStoreCode.CreatedDate = _currentDatetime;
                        TblStoreCode.CreatedBy = userId;
                        _storeCodeRepository.Create(TblStoreCode);
                    }
                    _storeCodeRepository.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("StoreCodeManager", "ManageStoreCode", ex);
            }
            return TblStoreCode.StoreCodeId;
        }

        /// <summary>
        /// Method to get the StoreCode by id 
        /// </summary>
        /// <param name="id">StoreCodeId</param>
        /// <returns>StoreCodeViewModel</returns>
        public StoreCodeViewModel GetStoreCodeById(int id)
        {
            StoreCodeViewModel StoreCodeVM = null;
            TblStoreCode TblStoreCode = null;

            try
            {
                TblStoreCode = _storeCodeRepository.GetSingle(where: x => x.IsActive == true && x.StoreCodeId == id);
                if (TblStoreCode != null)
                {
                    StoreCodeVM = _mapper.Map<TblStoreCode, StoreCodeViewModel>(TblStoreCode);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("StoreCodeManager", "GetStoreCodeById", ex);
            }
            return StoreCodeVM;

        }

        /// <summary>
        /// Method to delete StoreCode by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletStoreCodeById(int id)
        {
            bool flag = false;
            try
            {
                TblStoreCode TblStoreCode = _storeCodeRepository.GetSingle(x => x.IsActive == true && x.StoreCodeId == id);
                if (TblStoreCode != null)
                {
                    TblStoreCode.IsActive = false;
                    _storeCodeRepository.Update(TblStoreCode);
                    _storeCodeRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("StoreCodeManager", "DeletStoreCodeById", ex);
            }
            return flag;

        }

        /// <summary>
        /// Method to get the All StoreCode
        /// </summary>     
        /// <returns>List  StoreCodeViewModel</returns>

        public IList<StoreCodeViewModel> GetAllStoreCode(object i, int roleId, int userId)
        {
            IList<StoreCodeViewModel> StoreCodeVMList = null;
            List<TblStoreCode> TblStoreCodelist = new List<TblStoreCode>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblStoreCodelist = _storeCodeRepository.GetList(x => x.IsActive == true).ToList();

                if (TblStoreCodelist != null && TblStoreCodelist.Count > 0)
                {
                    StoreCodeVMList = _mapper.Map<IList<TblStoreCode>, IList<StoreCodeViewModel>>(TblStoreCodelist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PincodeManager", "GetAllPinCode", ex);
            }
            return (IList<StoreCodeViewModel>)TblStoreCodelist;
        }
    }
}