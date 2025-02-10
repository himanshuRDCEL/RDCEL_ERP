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
using RDCELERP.Model.PriceMaster;

namespace RDCELERP.BAL.MasterManager
{
    public class PriceMasterNameManager : IPriceMasterNameManager
    {
        #region  Variable Declaration
        IPriceMasterNameRepository _PriceMasterNameRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IUserRoleRepository _userRoleRepository;
        IProductTypeRepository _productTypeRepository;
        IProductCategoryRepository _productCategoryRepository;
        IPriceMasterMappingRepository _priceMasterMappingRepository;
       
        #endregion

        public PriceMasterNameManager(IPriceMasterNameRepository PriceMasterNameRepository, IProductTypeRepository productTypeRepository, IProductCategoryRepository productCategoryRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging, IPriceMasterMappingRepository priceMasterMappingRepository)
        {
            _PriceMasterNameRepository = PriceMasterNameRepository;
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
        /// <param name="PriceMasterVM">PriceMasterVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManagePriceMasterName(PriceMasterNameViewModel PriceMasterVM, int userId)
        {
            TblPriceMasterName TblPriceMasterName = new TblPriceMasterName();

            try
            {
                if (PriceMasterVM != null)
                {
                    TblPriceMasterName = _mapper.Map<PriceMasterNameViewModel, TblPriceMasterName>(PriceMasterVM);
                   

                    if (TblPriceMasterName.PriceMasterNameId > 0)
                    {
                        //Code to update the object

                        TblPriceMasterName.ModifiedBy = userId;
                        TblPriceMasterName.ModifiedDate = _currentDatetime;
                        TblPriceMasterName = TrimHelper.TrimAllValuesInModel(TblPriceMasterName);
                        _PriceMasterNameRepository.Update(TblPriceMasterName);
                    }
                    else
                    {

                        //Code to Insert the object 

                        TblPriceMasterName.IsActive = true;
                        TblPriceMasterName.CreatedDate = _currentDatetime;
                        TblPriceMasterName.CreatedBy = userId;
                        TblPriceMasterName = TrimHelper.TrimAllValuesInModel(TblPriceMasterName);
                        _PriceMasterNameRepository.Create(TblPriceMasterName);


                    }
                    _PriceMasterNameRepository.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PriceMasterManager", "PriceMasterImageLabel", ex);
            }

            return TblPriceMasterName.PriceMasterNameId;
        }


        /// <summary>
        /// Method to get the Price Master Name by id 
        /// </summary>
        /// <param name="id">PriceMasterId</param>
        /// <returns>PriceMasterNameViewModel</returns>
        public PriceMasterNameViewModel GetPriceMasterNameById(int id)
        {
            PriceMasterNameViewModel PriceMasterVM = null;
            TblPriceMasterName TblPriceMasterName = null;

            try
            {
                TblPriceMasterName = _PriceMasterNameRepository.GetSingle(where: x => x.IsActive == true && x.PriceMasterNameId == id);
                if (TblPriceMasterName != null)
                {
                    PriceMasterVM = _mapper.Map<TblPriceMasterName, PriceMasterNameViewModel>(TblPriceMasterName);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PriceMasterNameManager", "GetPriceMasterNameById", ex);
            }
            return PriceMasterVM;
        }

        /// <summary>
        /// Method to delete Price Master Name by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletPriceMasterNameById(int id)
        {
            bool flag = false;
            try
            {
                TblPriceMasterName TblPriceMasterName = _PriceMasterNameRepository.GetSingle(x => x.IsActive == true && x.PriceMasterNameId == id);
                if (TblPriceMasterName != null)
                {
                    TblPriceMasterName.IsActive = false;
                    _PriceMasterNameRepository.Update(TblPriceMasterName);
                    _PriceMasterNameRepository.SaveChanges();
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
        public IList<PriceMasterNameViewModel> GetAllPriceMasterName()
        {
            IList<PriceMasterNameViewModel> PriceMasterNameVMList = null;
            List<TblPriceMasterName> TblPriceMasterNamelist = new List<TblPriceMasterName>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblPriceMasterNamelist = _PriceMasterNameRepository.GetList(x => x.IsActive == true).ToList();

                if (TblPriceMasterNamelist != null && TblPriceMasterNamelist.Count > 0)
                {
                    PriceMasterNameVMList = _mapper.Map<IList<TblPriceMasterName>, IList<PriceMasterNameViewModel>>(TblPriceMasterNamelist);
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
