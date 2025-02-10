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
using RDCELERP.Model.Master;

namespace RDCELERP.BAL.MasterManager
{
    public class PriceMasterMappingManager : IPriceMasterMappingManager
    {
        #region  Variable Declaration
        IPriceMasterNameRepository   _priceMasterNameRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IUserRoleRepository _userRoleRepository;
        IProductTypeRepository _productTypeRepository;
        IProductCategoryRepository _productCategoryRepository;
        IPriceMasterMappingRepository _priceMasterMappingRepository;

        #endregion

        public PriceMasterMappingManager(IPriceMasterNameRepository PriceMasterNameRepository, IProductTypeRepository productTypeRepository, IProductCategoryRepository productCategoryRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging, IPriceMasterMappingRepository priceMasterMappingRepository)
        {
             _priceMasterNameRepository = PriceMasterNameRepository;
            _userRoleRepository = userRoleRepository;
            _productCategoryRepository = productCategoryRepository;
            _productTypeRepository = productTypeRepository;
            _mapper = mapper;
            _logging = logging;
            _priceMasterMappingRepository = priceMasterMappingRepository;

        }

        /// <summary>
        /// Method to manage (Add/Edit) Price Master Name
        /// </summary>
        /// <param name="PriceMasterMappingVM">PriceMasterMappingVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManagePriceMasterMapping(PriceMasterMappingViewModel PriceMasterMappingVM, int userId)
        {
            TblPriceMasterMapping TblPriceMasterMapping = new TblPriceMasterMapping();

            try
            {
                if (PriceMasterMappingVM != null)
                {
                    //TblPriceMasterMapping = _mapper.Map<PriceMasterMappingViewModel, TblPriceMasterMapping>(PriceMasterMappingVM);


                    if (PriceMasterMappingVM.PriceMasterMappingId > 0)
                    {
                        //Code to update the object
                        TblPriceMasterMapping.BusinessUnitId = PriceMasterMappingVM.BusinessUnitId;
                        TblPriceMasterMapping.PriceMasterMappingId = PriceMasterMappingVM.PriceMasterMappingId;
                        TblPriceMasterMapping.BrandId = PriceMasterMappingVM.BrandId;
                        TblPriceMasterMapping.BusinessPartnerId = PriceMasterMappingVM.BusinessPartnerId;
                        TblPriceMasterMapping.PriceMasterNameId = PriceMasterMappingVM.PriceMasterNameId;
                        DateTime startDate = Convert.ToDateTime(PriceMasterMappingVM.StartDate);
                        DateTime endDate = Convert.ToDateTime(PriceMasterMappingVM.EndDate);
                        TblPriceMasterMapping.Startdate = startDate;
                        TblPriceMasterMapping.Enddate = endDate;
                        TblPriceMasterMapping.ModifiedBy = userId;
                        TblPriceMasterMapping.ModifiedDate = _currentDatetime;
                        TblPriceMasterMapping.CreatedBy = PriceMasterMappingVM.CreatedBy;
                        TblPriceMasterMapping.CreatedDate = PriceMasterMappingVM.CreatedDate;
                        TblPriceMasterMapping.ModifiedDate = _currentDatetime;
                        TblPriceMasterMapping.IsActive = PriceMasterMappingVM.IsActive;
                        TblPriceMasterMapping = TrimHelper.TrimAllValuesInModel(TblPriceMasterMapping);
                        _priceMasterMappingRepository.Update(TblPriceMasterMapping);
                    }
                    else
                    {

                        //Code to Insert the object 
                        TblPriceMasterMapping.BusinessUnitId = PriceMasterMappingVM.BusinessUnitId;
                        TblPriceMasterMapping.BrandId = PriceMasterMappingVM.BrandId;
                        TblPriceMasterMapping.BusinessPartnerId = PriceMasterMappingVM.BusinessPartnerId;
                        TblPriceMasterMapping.PriceMasterNameId = PriceMasterMappingVM.PriceMasterNameId;
                        DateTime startDate = Convert.ToDateTime(PriceMasterMappingVM.StartDate);
                        DateTime endDate = Convert.ToDateTime(PriceMasterMappingVM.EndDate);
                        TblPriceMasterMapping.Startdate = startDate;
                        TblPriceMasterMapping.Enddate = endDate;
                        TblPriceMasterMapping.IsActive = true;
                        TblPriceMasterMapping.CreatedDate = _currentDatetime;
                        TblPriceMasterMapping.CreatedBy = userId;
                        TblPriceMasterMapping = TrimHelper.TrimAllValuesInModel(TblPriceMasterMapping);
                        _priceMasterMappingRepository.Create(TblPriceMasterMapping);


                    }
                    _priceMasterMappingRepository.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PriceMasterManager", "PriceMasterImageLabel", ex);
            }

            return TblPriceMasterMapping.PriceMasterMappingId;
        }


        /// <summary>
        /// Method to get the Price Master Name by id 
        /// </summary>
        /// <param name="id">PriceMasterId</param>
        /// <returns>PriceMasterNameViewModel</returns>
        public PriceMasterMappingViewModel GetPriceMasterMappingById(int id)
        {
            PriceMasterMappingViewModel PriceMasterMappingVM = null;
            TblPriceMasterMapping TblPriceMasterMapping = null;

            try
            {
                TblPriceMasterMapping =   _priceMasterMappingRepository.GetSingle(where: x => x.IsActive == true && x.PriceMasterMappingId == id);
                if (TblPriceMasterMapping != null)
                {
                    PriceMasterMappingVM = _mapper.Map<TblPriceMasterMapping, PriceMasterMappingViewModel>(TblPriceMasterMapping);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PriceMasterNameManager", "GetPriceMasterNameById", ex);
            }
            return PriceMasterMappingVM;
        }

        /// <summary>
        /// Method to delete Price Master Name by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletPriceMasterMappingById(int id)
        {
            bool flag = false;
            try
            {
                TblPriceMasterMapping TblPriceMasterMapping =   _priceMasterMappingRepository.GetSingle(x => x.IsActive == true && x.PriceMasterMappingId == id);
                if (TblPriceMasterMapping != null)
                {
                    TblPriceMasterMapping.IsActive = false;
                      _priceMasterMappingRepository.Update(TblPriceMasterMapping);
                      _priceMasterMappingRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PriceMasterManager", "DeletePriceMasterById", ex);
            }
            return flag;
        }

        /// <summary>
        /// Method to get the All Price Master Name
        /// </summary>     
        /// <returns>PriceMasterNameViewModel</returns>
        public IList<PriceMasterMappingViewModel> GetAllPriceMasterName()
        {
            IList<PriceMasterMappingViewModel> PriceMasterNameVMList = null;
            List<TblPriceMasterMapping> TblPriceMasterMappinglist = new List<TblPriceMasterMapping>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblPriceMasterMappinglist =   _priceMasterMappingRepository.GetList(x => x.IsActive == true).ToList();

                if (TblPriceMasterMappinglist != null && TblPriceMasterMappinglist.Count > 0)
                {
                    PriceMasterNameVMList = _mapper.Map<IList<TblPriceMasterMapping>, IList<PriceMasterMappingViewModel>>(TblPriceMasterMappinglist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PriceMasterNameManager", "GetAllPriceMasterName", ex);
            }
            return PriceMasterNameVMList;
        }
    }
}
