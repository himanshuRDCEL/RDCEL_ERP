
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
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Master;
using RDCELERP.Model.PriceMaster;
using RDCELERP.Model.UniversalPriceMaster;

namespace RDCELERP.BAL.MasterManager
{
    public class UniversalPriceMasterManager : IUniversalPriceMasterManager
    {
        #region  Variable Declaration
        IUniversalPriceMasterRepository _UniversalPriceMasterRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        IPriceMasterMappingRepository _PriceMasterMappingRepository;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IUserRoleRepository _userRoleRepository;
        IProductTypeRepository _productTypeRepository;
        IProductCategoryRepository _productCategoryRepository;
     
        IPriceMasterNameRepository _PriceMasterNameRepository;
        IBusinessPartnerRepository _BusinessPartnerRepository;
        IBusinessUnitRepository _BusinessUnitRepository;
        #endregion

        public UniversalPriceMasterManager( IUniversalPriceMasterRepository UniversalPriceMasterRepository, IProductTypeRepository productTypeRepository, IProductCategoryRepository productCategoryRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging, IPriceMasterMappingRepository PriceMasterMappingRepository, IPriceMasterNameRepository PriceMasterNameRepository, IBusinessPartnerRepository BusinessPartnerRepository, IBusinessUnitRepository BusinessUnitRepository)
        {
            _UniversalPriceMasterRepository = UniversalPriceMasterRepository;
            _userRoleRepository = userRoleRepository;
            _productCategoryRepository = productCategoryRepository;
            _productTypeRepository = productTypeRepository;
            _mapper = mapper;
            _logging = logging;
            _PriceMasterMappingRepository = PriceMasterMappingRepository;
            _PriceMasterNameRepository = PriceMasterNameRepository;
            _BusinessPartnerRepository = BusinessPartnerRepository;
            _BusinessUnitRepository = BusinessUnitRepository;
        }

        /// <summary>
        /// Method to manage (Add/Edit) Price Master
        /// </summary>
        /// <param name="UniversalPriceMasterVM">UniversalPriceMasterVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageUniversalPriceMaster(UniversalPriceMasterViewModel UniversalPriceMasterVM, int userId)
        {
            TblUniversalPriceMaster TblUniversalPriceMaster = new TblUniversalPriceMaster();

            try
            {
                if (UniversalPriceMasterVM != null)
                {
                    var ProductCategory = _productCategoryRepository.GetSingle(where: x => x.Description == UniversalPriceMasterVM.ProductCategoryDiscription);
                    var ProductType = _productTypeRepository.GetSingle(where: x => x.Description == UniversalPriceMasterVM.ProductTypeName);
                    var PriceMasterName = _PriceMasterNameRepository.GetSingle(where: x => x.Name == UniversalPriceMasterVM.PriceMasterName);
                    if (ProductCategory != null)
                    {
                        UniversalPriceMasterVM.ProductCategoryName = ProductCategory.Name;
                        UniversalPriceMasterVM.ProductCategoryId = ProductCategory.Id;
                    }


                    if (ProductType != null)
                    {
                        UniversalPriceMasterVM.ProductTypeId = ProductType.Id;
                        UniversalPriceMasterVM.ProductTypeCode = ProductType.Code;
                    }
                    if (PriceMasterName != null)
                    {
                        UniversalPriceMasterVM.PriceMasterNameId = PriceMasterName.PriceMasterNameId;
                    }

                    TblUniversalPriceMaster = _mapper.Map<UniversalPriceMasterViewModel, TblUniversalPriceMaster>(UniversalPriceMasterVM);
                    
                    if (TblUniversalPriceMaster.PriceMasterUniversalId > 0)
                    {
                        //Code to update the object
                        
                        TblUniversalPriceMaster.ModifiedBy = userId;
                        TblUniversalPriceMaster.ModifiedDate = _currentDatetime;
                        TblUniversalPriceMaster = TrimHelper.TrimAllValuesInModel(TblUniversalPriceMaster);
                        _UniversalPriceMasterRepository.Update(TblUniversalPriceMaster);
                    }
                    else
                    {

                        //Code to Insert the object 
                       
                        TblUniversalPriceMaster.IsActive = true;
                        TblUniversalPriceMaster.CreatedDate = _currentDatetime;
                        TblUniversalPriceMaster.CreatedBy = userId;
                        TblUniversalPriceMaster = TrimHelper.TrimAllValuesInModel(TblUniversalPriceMaster);
                        _UniversalPriceMasterRepository.Create(TblUniversalPriceMaster);


                    }
                    _UniversalPriceMasterRepository.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UniversalPriceMasterManager", "UniversalPriceMasterImageLabel", ex);
            }

            return TblUniversalPriceMaster.PriceMasterUniversalId;
        }


        public UniversalPriceMasterViewModel ManageUniversalPriceMasterBulk(UniversalPriceMasterViewModel UniversalPriceMasterVM, int userId)
        {

            if (UniversalPriceMasterVM != null && UniversalPriceMasterVM.UniversalPriceMasterVMList != null && UniversalPriceMasterVM.UniversalPriceMasterVMList.Count > 0)
            {

                var ValidatedUniversalPriceMasterList = UniversalPriceMasterVM.UniversalPriceMasterVMList.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
                UniversalPriceMasterVM.UniversalPriceMasterVMErrorList = UniversalPriceMasterVM.UniversalPriceMasterVMList.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();

                //tblBusinessPartner = _mapper.Map<List<BusinessPartnerVMExcelModel>, List<TblBusinessPartner>>(ValidatedBusinessPartnerList);
                if (ValidatedUniversalPriceMasterList != null && ValidatedUniversalPriceMasterList.Count > 0)
                {
                    foreach (var item in ValidatedUniversalPriceMasterList)
                    {
                        try
                        {
                            //TblUniversalPriceMaster tblUniversalPriceMaster = _UniversalPriceMasterRepository.GetSingle(x => x.PriceMasterName == item.PriceMasterName && x.IsActive == true);
                            TblPriceMasterName tblPriceMasterName = _PriceMasterNameRepository.GetSingle(x => x.Name == item.PriceMasterName && x.IsActive == true);
                            if (tblPriceMasterName != null)
                            {

                                //Code to update the object of Price Master Name
                                //TblPriceMasterName tblPriceMasterName = _PriceMasterNameRepository.GetSingle(x => x.Name == item.PriceMasterName && x.IsActive == true);
                                   TblPriceMasterName TblPriceMasterNames = new TblPriceMasterName();
                                
                                    tblPriceMasterName.PriceMasterNameId = tblPriceMasterName.PriceMasterNameId;
                                    tblPriceMasterName.Name = tblPriceMasterName.Name;
                                    //tblPriceMasterName.Description == " ";
                                    tblPriceMasterName.IsActive = false;
                                    tblPriceMasterName.ModifiedBy = userId;
                                    tblPriceMasterName.ModifiedDate = _currentDatetime;
                                    tblPriceMasterName.CreatedBy = tblPriceMasterName.CreatedBy;
                                    tblPriceMasterName.CreatedDate = tblPriceMasterName.CreatedDate;
                                    _PriceMasterNameRepository.Update(tblPriceMasterName);
                                    _PriceMasterNameRepository.SaveChanges();


                                    TblPriceMasterNames.Name = item.PriceMasterName;
                                    TblPriceMasterNames.IsActive = true;
                                    TblPriceMasterNames.CreatedBy = userId;
                                    TblPriceMasterNames.CreatedDate = _currentDatetime;
                                    _PriceMasterNameRepository.Create(TblPriceMasterNames);
                                    _PriceMasterNameRepository.SaveChanges();

                               
                                //Code to update object of Price Master Mapping
                                TblPriceMasterMapping PriceMasterMapping = _PriceMasterMappingRepository.GetSingle(x => x.PriceMasterNameId == tblPriceMasterName.PriceMasterNameId && x.IsActive == true);
                                TblBusinessUnit tblBusinessUnit = _BusinessUnitRepository.GetSingle(x => x.Name == item.Company && x.IsActive == true);
                                TblBusinessPartner tblBusinessPartner = _BusinessPartnerRepository.GetSingle(x => x.StoreCode == item.StoreCode && x.IsActive == true);
                                if (PriceMasterMapping != null)
                                {
                                    TblPriceMasterMapping tblPriceMasterMapping = new TblPriceMasterMapping();
                                    if(tblBusinessUnit != null)
                                    {
                                        tblPriceMasterMapping.BusinessUnitId = tblBusinessUnit.BusinessUnitId;
                                    }
                                    if (tblBusinessPartner != null)
                                    {
                                        tblPriceMasterMapping.BusinessPartnerId = tblBusinessPartner.BusinessPartnerId;
                                    }
                                    tblPriceMasterMapping.PriceMasterNameId = tblPriceMasterName.PriceMasterNameId;
                                    tblPriceMasterMapping.PriceMasterMappingId = PriceMasterMapping.PriceMasterMappingId;
                                    tblPriceMasterMapping.IsActive = false;
                                    tblPriceMasterMapping.CreatedBy = PriceMasterMapping.CreatedBy;
                                    tblPriceMasterMapping.CreatedDate = PriceMasterMapping.CreatedDate;
                                    tblPriceMasterMapping.ModifiedBy = userId;
                                    tblPriceMasterMapping.ModifiedDate = _currentDatetime;
                                    _PriceMasterMappingRepository.Update(tblPriceMasterMapping);
                                    _PriceMasterMappingRepository.SaveChanges();

                                    TblPriceMasterMapping TblPriceMasterMapping = new TblPriceMasterMapping();
                                    if (tblBusinessUnit != null)
                                    {
                                        tblPriceMasterMapping.BusinessUnitId = tblBusinessUnit.BusinessUnitId;
                                    }
                                    if (tblBusinessPartner != null)
                                    {
                                        tblPriceMasterMapping.BusinessPartnerId = tblBusinessPartner.BusinessPartnerId;
                                    }
                                    TblPriceMasterMapping.PriceMasterNameId = TblPriceMasterNames.PriceMasterNameId;
                                    TblPriceMasterMapping.IsActive = true;
                                    TblPriceMasterMapping.CreatedBy = userId;
                                    TblPriceMasterMapping.CreatedDate = _currentDatetime;
                                    _PriceMasterMappingRepository.Create(TblPriceMasterMapping);
                                    _PriceMasterMappingRepository.SaveChanges();


                                }
                               

                                //Code to update object of Universal Price Master 
                                   List<TblUniversalPriceMaster> TblUniversalPriceMaster = _UniversalPriceMasterRepository.GetList(x => x.PriceMasterName == tblPriceMasterName.Name && x.IsActive == true).ToList();
                                   if(TblUniversalPriceMaster != null && TblUniversalPriceMaster.Count > 0)
                                { 
                                    foreach(var item1 in TblUniversalPriceMaster)
                                    {
                                        TblUniversalPriceMaster TblUniversalPriceMasters = new TblUniversalPriceMaster();
                                        TblUniversalPriceMasters.PriceMasterUniversalId = item1.PriceMasterUniversalId;
                                        TblUniversalPriceMasters.PriceMasterNameId = item1.PriceMasterNameId;
                                        TblUniversalPriceMasters.PriceMasterName = item1.PriceMasterName;
                                        var ProductCategory1 = _productCategoryRepository.GetSingle(where: x => x.Description == item.ProductCategory);
                                        if (ProductCategory1 != null)
                                        {
                                            TblUniversalPriceMasters.ProductCategoryName = ProductCategory1.Name;
                                            TblUniversalPriceMasters.ProductCategoryId = ProductCategory1.Id;
                                        }

                                        var ProductType1 = _productTypeRepository.GetSingle(where: x => x.Description + x.Size == item.ProductType);
                                        if (ProductType1 != null)
                                        {
                                            TblUniversalPriceMasters.ProductTypeId = ProductType1.Id;
                                            TblUniversalPriceMasters.ProductTypeCode = ProductType1.Code;
                                        }
                                        TblUniversalPriceMasters.ProductTypeName = item1.ProductTypeName;
                                        TblUniversalPriceMasters.BrandName1 = item1.BrandName1;
                                        TblUniversalPriceMasters.BrandName2 = item1.BrandName2;
                                        TblUniversalPriceMasters.BrandName3 = item1.BrandName3;
                                        TblUniversalPriceMasters.BrandName4 = item1.BrandName4;
                                        TblUniversalPriceMasters.OtherBrand = item1.OtherBrand;
                                        TblUniversalPriceMasters.QuoteQ = item1.QuoteQ;
                                        TblUniversalPriceMasters.QuoteS = item1.QuoteS;
                                        TblUniversalPriceMasters.QuoteR = item1.QuoteR;
                                        TblUniversalPriceMasters.QuoteP = item1.QuoteP;
                                        TblUniversalPriceMasters.QuotePHigh = item1.QuotePHigh;
                                        TblUniversalPriceMasters.QuoteSHigh = item1.QuoteSHigh;
                                        TblUniversalPriceMasters.QuoteRHigh = item1.QuoteRHigh;
                                        TblUniversalPriceMasters.QuoteQHigh = item1.QuoteQHigh;
                                        TblUniversalPriceMasters.PriceEndDate = item1.PriceEndDate;
                                        TblUniversalPriceMasters.PriceStartDate = item1.PriceStartDate;
                                        TblUniversalPriceMasters.IsActive = false;
                                        TblUniversalPriceMasters.CreatedDate = item1.CreatedDate;
                                        TblUniversalPriceMasters.CreatedBy = item1.CreatedBy;
                                        TblUniversalPriceMasters.ModifiedDate = _currentDatetime;
                                        TblUniversalPriceMasters.ModifiedBy = userId;
                                        _UniversalPriceMasterRepository.Update(TblUniversalPriceMasters);
                                        _UniversalPriceMasterRepository.SaveChanges();
                                    }
                                   
                                }
                                 
                                    TblUniversalPriceMaster tblUniversalPriceMasters = new TblUniversalPriceMaster();
                                    //Code to insert the object 
                                    tblUniversalPriceMasters.PriceMasterName = TblPriceMasterNames.Name; ;
                                    tblUniversalPriceMasters.PriceMasterNameId = TblPriceMasterNames.PriceMasterNameId;
                                    var ProductCategory = _productCategoryRepository.GetSingle(where: x => x.Description == item.ProductCategory);
                                    if (ProductCategory != null)
                                    {
                                        tblUniversalPriceMasters.ProductCategoryName = ProductCategory.Name;
                                        tblUniversalPriceMasters.ProductCategoryId = ProductCategory.Id;
                                    }

                                    var ProductType = _productTypeRepository.GetSingle(where: x => x.Description + x.Size == item.ProductType);
                                    if (ProductType != null)
                                    {
                                    tblUniversalPriceMasters.ProductTypeId = ProductType.Id;
                                    tblUniversalPriceMasters.ProductTypeCode = ProductType.Code;
                                    }
                                    tblUniversalPriceMasters.ProductTypeName = item.ProductType;
                                    tblUniversalPriceMasters.BrandName1 = item.BrandName1;
                                    tblUniversalPriceMasters.BrandName2 = item.BrandName2;
                                    tblUniversalPriceMasters.BrandName3 = item.BrandName3;
                                    tblUniversalPriceMasters.BrandName4 = item.BrandName4;
                                    tblUniversalPriceMasters.OtherBrand = item.OtherBrand;
                                    tblUniversalPriceMasters.QuoteQ = item.QuoteQ;
                                    tblUniversalPriceMasters.QuoteS = item.QuoteS;
                                    tblUniversalPriceMasters.QuoteR = item.QuoteR;
                                    tblUniversalPriceMasters.QuoteP = item.QuoteP;
                                    tblUniversalPriceMasters.QuotePHigh = item.QuotePHigh;
                                    tblUniversalPriceMasters.QuoteSHigh = item.QuoteSHigh;
                                    tblUniversalPriceMasters.QuoteRHigh = item.QuoteRHigh;
                                    tblUniversalPriceMasters.QuoteQHigh = item.QuoteQHigh;
                                    tblUniversalPriceMasters.PriceEndDate = item.PriceEndDate;
                                    tblUniversalPriceMasters.PriceStartDate = item.PriceStartDate;
                                    tblUniversalPriceMasters.IsActive = true;
                                    tblUniversalPriceMasters.CreatedDate = _currentDatetime;
                                    tblUniversalPriceMasters.CreatedBy = userId;
                                    _UniversalPriceMasterRepository.Create(tblUniversalPriceMasters);
                                    _UniversalPriceMasterRepository.SaveChanges();
                                
                            }


                            else
                            {

                                //Code to insert the object of Price Master Name
                                TblPriceMasterName tblPriceMasterNames = new TblPriceMasterName();
                                tblPriceMasterNames.Name = item.PriceMasterName;
                                //tblPriceMasterName.Description == " ";
                                tblPriceMasterNames.IsActive = true;
                                tblPriceMasterNames.CreatedBy = userId;
                                tblPriceMasterNames.CreatedDate = _currentDatetime;
                                _PriceMasterNameRepository.Create(tblPriceMasterNames);
                                _PriceMasterNameRepository.SaveChanges();

                                //Code to insert object of Price Master Mapping
                                TblBusinessUnit tblBusinessUnit = _BusinessUnitRepository.GetSingle(x => x.Name == item.Company && x.IsActive == true);
                                TblBusinessPartner tblBusinessPartner = _BusinessPartnerRepository.GetSingle(x => x.StoreCode == item.StoreCode && x.IsActive == true);
                                TblPriceMasterMapping tblPriceMasterMapping = new TblPriceMasterMapping();
                                tblPriceMasterMapping.PriceMasterNameId = tblPriceMasterName.PriceMasterNameId;
                                if(tblBusinessUnit != null)
                                {
                                    tblPriceMasterMapping.BusinessUnitId = tblBusinessUnit.BusinessUnitId;
                                }
                                if (tblBusinessPartner != null)
                                {
                                    tblPriceMasterMapping.BusinessPartnerId = tblBusinessPartner.BusinessPartnerId;
                                }

                                tblPriceMasterMapping.IsActive = true;
                                tblPriceMasterMapping.CreatedBy = userId;
                                tblPriceMasterMapping.CreatedDate = _currentDatetime;
                                _PriceMasterMappingRepository.Create(tblPriceMasterMapping);
                                _PriceMasterMappingRepository.SaveChanges();

                                TblUniversalPriceMaster TblUniversalPriceMaster = new TblUniversalPriceMaster();
                                //Code to insert the object 
                                TblUniversalPriceMaster.PriceMasterName = tblPriceMasterName.Name; ;
                                TblUniversalPriceMaster.PriceMasterNameId = tblPriceMasterName.PriceMasterNameId;
                                var ProductCategory = _productCategoryRepository.GetSingle(where: x => x.Description == item.ProductCategory);
                                if (ProductCategory != null)
                                {
                                    TblUniversalPriceMaster.ProductCategoryName = ProductCategory.Name;
                                    TblUniversalPriceMaster.ProductCategoryId = ProductCategory.Id;
                                }

                                var ProductType = _productTypeRepository.GetSingle(where: x => x.Description + x.Size == item.ProductType);
                                if (ProductType != null)
                                {
                                    TblUniversalPriceMaster.ProductTypeId = ProductType.Id;
                                    TblUniversalPriceMaster.ProductTypeCode = ProductType.Code;
                                }
                                TblUniversalPriceMaster.ProductTypeName = item.ProductType;
                                TblUniversalPriceMaster.BrandName1 = item.BrandName1;
                                TblUniversalPriceMaster.BrandName2 = item.BrandName2;
                                TblUniversalPriceMaster.BrandName3 = item.BrandName3;
                                TblUniversalPriceMaster.BrandName4 = item.BrandName4;
                                TblUniversalPriceMaster.OtherBrand = item.OtherBrand;
                                TblUniversalPriceMaster.QuoteQ = item.QuoteQ;
                                TblUniversalPriceMaster.QuoteS = item.QuoteS;
                                TblUniversalPriceMaster.QuoteR = item.QuoteR;
                                TblUniversalPriceMaster.QuoteP = item.QuoteP;
                                TblUniversalPriceMaster.QuotePHigh = item.QuotePHigh;
                                TblUniversalPriceMaster.QuoteSHigh = item.QuoteSHigh;
                                TblUniversalPriceMaster.QuoteRHigh = item.QuoteRHigh;
                                TblUniversalPriceMaster.QuoteQHigh = item.QuoteQHigh;
                                TblUniversalPriceMaster.PriceEndDate = item.PriceEndDate;
                                TblUniversalPriceMaster.PriceStartDate = item.PriceStartDate;
                                TblUniversalPriceMaster.IsActive = true;
                                TblUniversalPriceMaster.CreatedDate = _currentDatetime;
                                TblUniversalPriceMaster.CreatedBy = userId;
                                _UniversalPriceMasterRepository.Create(TblUniversalPriceMaster);
                                _UniversalPriceMasterRepository.SaveChanges();

                            }
                        }

                        catch (Exception ex)
                        {
                            item.Remarks += ex.Message + ", ";
                            UniversalPriceMasterVM.UniversalPriceMasterVMList.Add(item);
                        }
                    }
                }
            }

            return UniversalPriceMasterVM;
        }

        /// <summary>
        /// Method to get the Price Master by id 
        /// </summary>
        /// <param name="id">UniversalPriceMasterId</param>
        /// <returns>UniversalPriceMasterViewModel</returns>
        public UniversalPriceMasterViewModel GetUniversalPriceMasterById(int id)
        {
            UniversalPriceMasterViewModel UniversalPriceMasterVM = null;
            TblUniversalPriceMaster TblUniversalPriceMaster = null;

            try
            {

                TblUniversalPriceMaster = _UniversalPriceMasterRepository.GetSingle(where: x => x.PriceMasterUniversalId == id);

                if (TblUniversalPriceMaster != null)
                {

                    UniversalPriceMasterVM = _mapper.Map<TblUniversalPriceMaster, UniversalPriceMasterViewModel>(TblUniversalPriceMaster);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UniversalPriceMasterManager", "GetUniversalPriceMasterById", ex);
            }
            return UniversalPriceMasterVM;
        }

        /// <summary>
        /// Method to delete UniversalPriceMaster by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeleteUniversalPriceMasterById(int id)
        {
            bool flag = false;
            try
            {
                TblUniversalPriceMaster TblUniversalPriceMaster = null;
                if (TblUniversalPriceMaster != null)
                {
                    TblUniversalPriceMaster.IsActive = false;
                   
                    _UniversalPriceMasterRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UniversalPriceMasterManager", "DeleteUniversalPriceMasterById", ex);
            }
            return flag;
        }

        /// <summary>
        /// Method to get the All Price Master
        /// </summary>     
        /// <returns>UniversalPriceMasterViewModel</returns>
        public IList<UniversalPriceMasterViewModel> GetAllUniversalPriceMaster()
        {
            IList<UniversalPriceMasterViewModel> UniversalPriceMasterVMList = null;
            List<TblUniversalPriceMaster> TblUniversalPriceMasterlist = new List<TblUniversalPriceMaster>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblUniversalPriceMasterlist = null;

                if (TblUniversalPriceMasterlist != null && TblUniversalPriceMasterlist.Count > 0)
                {
                    UniversalPriceMasterVMList = _mapper.Map<IList<TblUniversalPriceMaster>, IList<UniversalPriceMasterViewModel>>(TblUniversalPriceMasterlist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UniversalPriceMasterManager", "GetAllUniversalPriceMaster", ex);
            }
            return UniversalPriceMasterVMList;
        }

        #region add AddPriceMasterMapping
        /// <summary>
        /// Used for adding new PriceMasterName into db
        /// </summary>
        /// <param name="UniversalPriceMasterVM"></param>
        /// <returns></returns>
        public int AddPriceMasterName(PriceMasterNameViewModel PriceMasterNameViewModel, int userId)
        {

            TblPriceMasterName PriceMasterName = new TblPriceMasterName();

            int result = 0;
            try
            {
                if (PriceMasterNameViewModel != null)
                {
                    PriceMasterName = _mapper.Map<PriceMasterNameViewModel, TblPriceMasterName>(PriceMasterNameViewModel);

                    if (PriceMasterName.PriceMasterNameId > 0)
                    {
                        //Code to update the objec
                        PriceMasterName.ModifiedBy = userId;
                        PriceMasterName.ModifiedDate = _currentDatetime;
                        _PriceMasterNameRepository.Update(PriceMasterName);
                    }
                    else
                    {
                        PriceMasterName.IsActive = true;
                        PriceMasterName.CreatedDate = _currentDatetime;
                        PriceMasterName.CreatedBy = userId;
                        _PriceMasterNameRepository.Create(PriceMasterName);
                    }
                    result = _PriceMasterNameRepository.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UniversalPriceMasterManager", "UniversalPriceMasterImageLabel", ex);
            }

            return result;
        }
        #endregion

        #region add Price master mapping
        /// <summary>
        /// Used for adding new PriceMasterName into db
        /// </summary>
        /// <param name="UniversalPriceMasterVM"></param>
        /// <returns></returns>
        public int AddPriceMasterMapping(PriceMasterMappingViewModel PriceMasterMappingViewModel, int userId)
        {
            //TblUniversalPriceMaster TblUniversalPriceMaster = new TblUniversalPriceMaster();
            TblPriceMasterMapping tblPriceMasterMapping = new TblPriceMasterMapping();
            int result = 0;
            try
            {
                if (PriceMasterMappingViewModel != null)
                {
                    tblPriceMasterMapping = _mapper.Map<PriceMasterMappingViewModel, TblPriceMasterMapping>(PriceMasterMappingViewModel);
                    //var ProductCategory = _productCategoryRepository.GetSingle(where: x => x.Description == TblUniversalPriceMaster.ProductCat);
                    //var ProductType = _productTypeRepository.GetSingle(where: x => x.Description == TblUniversalPriceMaster.ProductType);

                    if (tblPriceMasterMapping.PriceMasterMappingId > 0)
                    {
                        //Code to update the objec
                        tblPriceMasterMapping.ModifiedBy = userId;
                        tblPriceMasterMapping.ModifiedDate = _currentDatetime;
                        _PriceMasterMappingRepository.Update(tblPriceMasterMapping);
                    }
                    else
                    {

                        tblPriceMasterMapping.IsActive = true;
                        tblPriceMasterMapping.CreatedDate = _currentDatetime;
                        tblPriceMasterMapping.CreatedBy = userId;
                        _PriceMasterMappingRepository.Create(tblPriceMasterMapping);


                    }
                    result = _PriceMasterMappingRepository.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UniversalPriceMasterManager", "UniversalPriceMasterImageLabel", ex);
            }

            return result;
        }

        #endregion


        /// <summary>
        /// Method to get the Universal Price Master List by BuId 
        /// </summary>
        /// <param name="id">BuId</param>
        /// <returns>List<UniversalPriceMasterViewModel></returns>
        public List<PriceMasterNameViewModel> GetListofpriceMasterByBUId(int BUId)
        {
            List<PriceMasterNameViewModel> PriceMasterNameVMList = null;
            List<TblPriceMasterMapping> tblPriceMasterMapping = null;
            List<TblPriceMasterName> TblPriceMasterName = null;

            try
            {
                tblPriceMasterMapping = _PriceMasterMappingRepository.GetList(x => x.IsActive == true && x.BusinessUnitId == BUId).ToList();
                if (tblPriceMasterMapping != null && tblPriceMasterMapping.Count > 0)
                {
                    TblPriceMasterName = _PriceMasterNameRepository.GetList(x => x.IsActive == true && x.PriceMasterNameId == tblPriceMasterMapping[0].PriceMasterNameId).ToList();
                    PriceMasterNameVMList = _mapper.Map<List<TblPriceMasterName>, List<PriceMasterNameViewModel>>(TblPriceMasterName);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessPartnerManager", "GetListofBusinessPartnerByBUId", ex);
            }
            return PriceMasterNameVMList;
        }
    }
}
