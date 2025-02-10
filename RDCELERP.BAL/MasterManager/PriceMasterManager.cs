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
    public class PriceMasterManager : IPriceMasterManager
    {
        #region  Variable Declaration
        IPriceMasterRepository _PriceMasterRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IUserRoleRepository _userRoleRepository;
        IProductTypeRepository _productTypeRepository;
        IProductCategoryRepository _productCategoryRepository;
        IPriceMasterMappingRepository _priceMasterMappingRepository;
        IPriceMasterNameRepository _priceMasterNameRepository;
        #endregion

        public PriceMasterManager(IPriceMasterRepository PriceMasterRepository, IProductTypeRepository productTypeRepository, IProductCategoryRepository productCategoryRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging, IPriceMasterMappingRepository priceMasterMappingRepository, IPriceMasterNameRepository priceMasterNameRepository)
        {
            _PriceMasterRepository = PriceMasterRepository;
            _userRoleRepository = userRoleRepository;
            _productCategoryRepository = productCategoryRepository;
            _productTypeRepository = productTypeRepository;
            _mapper = mapper;
            _logging = logging;
            _priceMasterMappingRepository = priceMasterMappingRepository;
            _priceMasterNameRepository = priceMasterNameRepository;
        }

        /// <summary>
        /// Method to manage (Add/Edit) Price Master
        /// </summary>
        /// <param name="PriceMasterVM">PriceMasterVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManagePriceMaster(PriceMasterViewModel PriceMasterVM, int userId)
        {
            TblPriceMaster TblPriceMaster = new TblPriceMaster();

            try
            {
                if (PriceMasterVM != null)
                {
                    TblPriceMaster = _mapper.Map<PriceMasterViewModel, TblPriceMaster>(PriceMasterVM);
                    var ProductCategory = _productCategoryRepository.GetSingle(where: x => x.Description == TblPriceMaster.ProductCat);
                    var ProductType = _productTypeRepository.GetSingle(where: x => x.Code == TblPriceMaster.ProductTypeCode);

                    if (TblPriceMaster.Id > 0)
                    {
                        //Code to update the object
                        if (ProductCategory != null)
                        {
                            TblPriceMaster.ProductCat = ProductCategory.Name;
                            TblPriceMaster.ProductCategoryId = ProductCategory.Id;
                 
                        }
                        if (ProductType != null)
                        {
                            TblPriceMaster.ProductTypeCode = ProductType.Code;
                            TblPriceMaster.ProductTypeId = ProductType.Id;
                        }
                        TblPriceMaster.ModifiedBy = userId;
                        TblPriceMaster.ModifiedDate = _currentDatetime;
                        TblPriceMaster = TrimHelper.TrimAllValuesInModel(TblPriceMaster);
                        _PriceMasterRepository.Update(TblPriceMaster);
                    }
                    else
                    {

                        //Code to Insert the object 
                        if (ProductCategory != null)
                        {
                            TblPriceMaster.ProductCat = ProductCategory.Name;
                            TblPriceMaster.ProductCategoryId = ProductCategory.Id;
                        }
                        if (ProductType != null)
                        {
                            TblPriceMaster.ProductTypeCode = ProductType.Code;
                            TblPriceMaster.ProductTypeId = ProductType.Id;
                        }
                        TblPriceMaster.IsActive = true;
                        TblPriceMaster.CreatedDate = _currentDatetime;
                        TblPriceMaster.CreatedBy = userId;
                        TblPriceMaster = TrimHelper.TrimAllValuesInModel(TblPriceMaster);
                        _PriceMasterRepository.Create(TblPriceMaster);


                    }
                    _PriceMasterRepository.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PriceMasterManager", "PriceMasterImageLabel", ex);
            }

            return TblPriceMaster.Id;
        }


        public PriceMasterViewModel ManagePriceMasterBulk(PriceMasterViewModel PriceMasterVM, int userId)
        {

            if (PriceMasterVM != null && PriceMasterVM.PriceMasterVMList != null && PriceMasterVM.PriceMasterVMList.Count > 0)
            {

                var ValidatedPriceMasterList = PriceMasterVM.PriceMasterVMList.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
                PriceMasterVM.PriceMasterVMErrorList = PriceMasterVM.PriceMasterVMList.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();

                //tblBusinessPartner = _mapper.Map<List<BusinessPartnerVMExcelModel>, List<TblBusinessPartner>>(ValidatedBusinessPartnerList);
                if (ValidatedPriceMasterList != null && ValidatedPriceMasterList.Count > 0)
                {
                    foreach (var item in ValidatedPriceMasterList)
                    {
                        try
                        {

                            var check = _PriceMasterRepository.GetSingle(x => x.ExchPriceCode == item.ExchPriceCode && x.IsActive == true);
                            if (check != null)
                            {

                                TblPriceMaster TblPriceMaster = new TblPriceMaster();
                                TblPriceMaster.Id = check.Id;
                                TblPriceMaster.IsActive = false;
                                _PriceMasterRepository.Update(TblPriceMaster);
                                _PriceMasterRepository.SaveChanges();
                            }

                            if (item.Id > 0)
                            {

                                TblPriceMaster TblPriceMaster = new TblPriceMaster();

                                //Code to update the object 

                                TblPriceMaster.ExchPriceCode = item.ExchPriceCode; ;

                                var ProductCategory = _productCategoryRepository.GetSingle(where: x => x.Description == item.ProductCategory);
                                TblPriceMaster.ProductCat = ProductCategory.Name;
                                TblPriceMaster.ProductCategoryId = ProductCategory.Id;
                                var ProductType = _productTypeRepository.GetSingle(where: x => x.Description + x.Size == item.ProductType);
                                TblPriceMaster.ProductType = ProductType.Description;
                                TblPriceMaster.ProductTypeId = ProductType.Id;
                                TblPriceMaster.ProductTypeCode = ProductType.Code;
                                TblPriceMaster.BrandName1 = item.BrandName1;
                                TblPriceMaster.BrandName2 = item.BrandName2;
                                TblPriceMaster.BrandName3 = item.BrandName3;
                                TblPriceMaster.BrandName4 = item.BrandName4;
                                TblPriceMaster.OtherBrand = item.OtherBrand;
                                TblPriceMaster.QuoteQ = item.QuoteQ;
                                TblPriceMaster.QuoteS = item.QuoteS;
                                TblPriceMaster.QuoteR = item.QuoteR;
                                TblPriceMaster.QuoteP = item.QuoteP;
                                TblPriceMaster.QuotePHigh = item.QuotePHigh;
                                TblPriceMaster.QuoteSHigh = item.QuoteSHigh;
                                TblPriceMaster.QuoteRHigh = item.QuoteRHigh;
                                TblPriceMaster.QuoteQHigh = item.QuoteQHigh;
                                TblPriceMaster.PriceEndDate = item.PriceEndDate;
                                TblPriceMaster.PriceStartDate = item.PriceStartDate;
                                TblPriceMaster.ModifiedDate = _currentDatetime;
                                TblPriceMaster.ModifiedBy = userId;
                                _PriceMasterRepository.Update(TblPriceMaster);
                                _PriceMasterRepository.SaveChanges();

                            }
                            else
                            {


                                TblPriceMaster TblPriceMaster = new TblPriceMaster();
                                //Code to insert the object 
                                TblPriceMaster.ExchPriceCode = item.ExchPriceCode; ;

                                var ProductCategory = _productCategoryRepository.GetSingle(where: x => x.Description == item.ProductCategory);
                                TblPriceMaster.ProductCat = ProductCategory.Name;
                                TblPriceMaster.ProductCategoryId = ProductCategory.Id;
                                var ProductType = _productTypeRepository.GetSingle(where: x => x.Description + x.Size == item.ProductType);
                                TblPriceMaster.ProductType = ProductType.Description;
                                TblPriceMaster.ProductTypeId = ProductType.Id;
                                TblPriceMaster.ProductTypeCode = ProductType.Code;
                                TblPriceMaster.BrandName1 = item.BrandName1;
                                TblPriceMaster.BrandName2 = item.BrandName2;
                                TblPriceMaster.BrandName3 = item.BrandName3;
                                TblPriceMaster.BrandName4 = item.BrandName4;
                                TblPriceMaster.OtherBrand = item.OtherBrand;
                                TblPriceMaster.QuoteQ = item.QuoteQ;
                                TblPriceMaster.QuoteS = item.QuoteS;
                                TblPriceMaster.QuoteR = item.QuoteR;
                                TblPriceMaster.QuoteP = item.QuoteP;
                                TblPriceMaster.QuotePHigh = item.QuotePHigh;
                                TblPriceMaster.QuoteSHigh = item.QuoteSHigh;
                                TblPriceMaster.QuoteRHigh = item.QuoteRHigh;
                                TblPriceMaster.QuoteQHigh = item.QuoteQHigh;
                                TblPriceMaster.PriceEndDate = item.PriceEndDate;
                                TblPriceMaster.PriceStartDate = item.PriceStartDate;
                                TblPriceMaster.IsActive = true;
                                TblPriceMaster.CreatedDate = _currentDatetime;
                                TblPriceMaster.CreatedBy = userId;

                                _PriceMasterRepository.Create(TblPriceMaster);
                                _PriceMasterRepository.SaveChanges();

                            }
                        }

                        catch (Exception ex)
                        {
                            item.Remarks += ex.Message + ", ";
                            PriceMasterVM.PriceMasterVMList.Add(item);
                        }
                    }
                }
            }

            return PriceMasterVM;
        }

        /// <summary>
        /// Method to get the Price Master by id 
        /// </summary>
        /// <param name="id">PriceMasterId</param>
        /// <returns>PriceMasterViewModel</returns>
        public PriceMasterViewModel GetPriceMasterById(int id)
        {
            PriceMasterViewModel PriceMasterVM = null;
            TblPriceMaster TblPriceMaster = null;

            try
            {
                TblPriceMaster = _PriceMasterRepository.GetSingle(where: x => x.IsActive == true && x.Id == id);
                if (TblPriceMaster != null)
                {
                    PriceMasterVM = _mapper.Map<TblPriceMaster, PriceMasterViewModel>(TblPriceMaster);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PriceMasterManager", "GetPriceMasterById", ex);
            }
            return PriceMasterVM;
        }

        /// <summary>
        /// Method to delete PriceMaster by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletPriceMasterById(int id)
        {
            bool flag = false;
            try
            {
                TblPriceMaster TblPriceMaster = _PriceMasterRepository.GetSingle(x => x.IsActive == true && x.Id == id);
                if (TblPriceMaster != null)
                {
                    TblPriceMaster.IsActive = false;
                    _PriceMasterRepository.Update(TblPriceMaster);
                    _PriceMasterRepository.SaveChanges();
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
        /// Method to get the All Price Master
        /// </summary>     
        /// <returns>PriceMasterViewModel</returns>
        public IList<PriceMasterViewModel> GetAllPriceMaster()
        {
            IList<PriceMasterViewModel> PriceMasterVMList = null;
            List<TblPriceMaster> TblPriceMasterlist = new List<TblPriceMaster>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblPriceMasterlist = _PriceMasterRepository.GetList(x => x.IsActive == true).ToList();

                if (TblPriceMasterlist != null && TblPriceMasterlist.Count > 0)
                {
                    PriceMasterVMList = _mapper.Map<IList<TblPriceMaster>, IList<PriceMasterViewModel>>(TblPriceMasterlist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PriceMasterManager", "GetAllPriceMaster", ex);
            }
            return PriceMasterVMList;
        }

        //#region add AddPriceMasterMapping
        ///// <summary>
        ///// Used for adding new pricemastername into db
        ///// </summary>
        ///// <param name="PriceMasterVM"></param>
        ///// <returns></returns>
        //public int AddPriceMasterName(PriceMasterNameViewModel priceMasterNameViewModel, int userId)
        //{

        //    PriceMasterName priceMasterName = new PriceMasterName();

        //    int result = 0;
        //    try
        //    {
        //        if (priceMasterNameViewModel != null)
        //        {
        //            priceMasterName = _mapper.Map<PriceMasterNameViewModel, PriceMasterName>(priceMasterNameViewModel);

        //            if (priceMasterName.PriceMasterNameId > 0)
        //            {
        //                //Code to update the objec
        //                priceMasterName.ModifiedBy = userId;
        //                priceMasterName.ModifiedDate = _currentDatetime;
        //                _priceMasterNameRepository.Update(priceMasterName);
        //            }
        //            else
        //            {
        //                priceMasterName.IsActive = true;
        //                priceMasterName.CreatedDate = _currentDatetime;
        //                priceMasterName.CreatedBy = userId;
        //                _priceMasterNameRepository.Create(priceMasterName);
        //            }
        //            result = _priceMasterNameRepository.SaveChanges();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.WriteErrorToDB("PriceMasterManager", "PriceMasterImageLabel", ex);
        //    }

        //    return result;
        //}
        //#endregion

        #region add Price master mapping
        /// <summary>
        /// Used for adding new pricemastername into db
        /// </summary>
        /// <param name="PriceMasterVM"></param>
        /// <returns></returns>
        public int AddPriceMasterMapping(PriceMasterMappingViewModel priceMasterMappingViewModel, int userId)
        {
            //TblPriceMaster TblPriceMaster = new TblPriceMaster();
            TblPriceMasterMapping tblPriceMasterMapping = new TblPriceMasterMapping();
            int result = 0;
            try
            {
                if (priceMasterMappingViewModel != null)
                {
                    tblPriceMasterMapping = _mapper.Map<PriceMasterMappingViewModel, TblPriceMasterMapping>(priceMasterMappingViewModel);
                    //var ProductCategory = _productCategoryRepository.GetSingle(where: x => x.Description == TblPriceMaster.ProductCat);
                    //var ProductType = _productTypeRepository.GetSingle(where: x => x.Description == TblPriceMaster.ProductType);

                    if (tblPriceMasterMapping.PriceMasterMappingId > 0)
                    {
                        //Code to update the objec
                        tblPriceMasterMapping.ModifiedBy = userId;
                        tblPriceMasterMapping.ModifiedDate = _currentDatetime;
                        _priceMasterMappingRepository.Update(tblPriceMasterMapping);
                    }
                    else
                    {

                        tblPriceMasterMapping.IsActive = true;
                        tblPriceMasterMapping.CreatedDate = _currentDatetime;
                        tblPriceMasterMapping.CreatedBy = userId;
                        _priceMasterMappingRepository.Create(tblPriceMasterMapping);


                    }
                    result = _priceMasterMappingRepository.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PriceMasterManager", "PriceMasterImageLabel", ex);
            }

            return result;
        }
        #endregion
    }
}
