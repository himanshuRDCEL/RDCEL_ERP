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
using RDCELERP.Model.ABBPriceMaster;

namespace RDCELERP.BAL.MasterManager
{
    public class ABBPriceMasterManager : IABBPriceMasterManager
    {
        #region  Variable Declaration
        IABBPriceMasterRepository _AbbPriceMasterRepository;
        IBusinessUnitRepository _businessUnitRepository;
        IProductCategoryRepository _productCategoryRepository;
        IProductTypeRepository _productTypeRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IUserRoleRepository _userRoleRepository;

        #endregion

        public ABBPriceMasterManager(IABBPriceMasterRepository AbbPriceMasterRepository, IBusinessUnitRepository businessUnitRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging, IProductCategoryRepository productCategoryRepository, IProductTypeRepository productTypeRepository)
        {
            _AbbPriceMasterRepository = AbbPriceMasterRepository;
            _userRoleRepository = userRoleRepository;
            _productCategoryRepository = productCategoryRepository;
            _productTypeRepository = productTypeRepository;
            _businessUnitRepository = businessUnitRepository;
            _mapper = mapper;
            _logging = logging;
        }

        /// <summary>
        /// Method to manage(Add/Edit) ABBPriceMaster 
        /// </summary>
        /// <param name = "ABBPlanMasterVM" > ABBPriceMasterVM </ param >
        /// < param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageABBPriceMaster(ABBPriceMasterViewModel ABBPriceMasterVM, int userId)
        {
            TblAbbpriceMaster TblAbbpriceMaster = new TblAbbpriceMaster();
            TblBusinessUnit TblBusinessUnit = new TblBusinessUnit();
            TblProductType TblProductType = new TblProductType();
            TblProductCategory TblProductCategory = new TblProductCategory();

            try
            {
                if (ABBPriceMasterVM != null)
                {
                    TblAbbpriceMaster = _mapper.Map<ABBPriceMasterViewModel, TblAbbpriceMaster>(ABBPriceMasterVM);
                    TblBusinessUnit = _businessUnitRepository.GetSingle(where: x => x.BusinessUnitId == TblAbbpriceMaster.BusinessUnitId);
                    TblProductCategory = _productCategoryRepository.GetSingle(where: x => x.Description == TblAbbpriceMaster.ProductCategory);
                    TblProductType = _productTypeRepository.GetSingle(where: x => x.Description + x.Size == TblAbbpriceMaster.ProductSabcategory);

                    if (TblAbbpriceMaster.PriceMasterId > 0)
                    {
                        //Code to update the object
                        TblAbbpriceMaster.Sponsor = TblBusinessUnit.Name;
                        TblAbbpriceMaster.ProductCatId = TblProductCategory.Id;
                        TblAbbpriceMaster.ProductTypeId = TblProductType.Id;
                        TblAbbpriceMaster.ModifiedBy = userId;
                        TblAbbpriceMaster.ModifiedDate = _currentDatetime;
                        TblAbbpriceMaster = TrimHelper.TrimAllValuesInModel(TblAbbpriceMaster);
                        _AbbPriceMasterRepository.Update(TblAbbpriceMaster);
                    }
                    else
                    {

                        //Code to Insert the object
                        TblAbbpriceMaster.Sponsor = TblBusinessUnit.Name;
                        TblAbbpriceMaster.ProductCatId = TblProductCategory.Id;
                        TblAbbpriceMaster.ProductTypeId = TblProductType.Id;
                        TblAbbpriceMaster.IsActive = true;
                        TblAbbpriceMaster.CreatedDate = _currentDatetime;
                        TblAbbpriceMaster.CreatedBy = userId;
                        TblAbbpriceMaster.Sponsor = TblBusinessUnit.Name;
                        TblAbbpriceMaster = TrimHelper.TrimAllValuesInModel(TblAbbpriceMaster);
                        _AbbPriceMasterRepository.Create(TblAbbpriceMaster);
                        //}

                    }
                    _AbbPriceMasterRepository.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ABBPlanMasterManager", "ManageABBPlanMaster", ex);
            }

            return TblAbbpriceMaster.PriceMasterId;
        }

        public ABBPriceMasterViewModel ManageABBPriceMasterBulk(ABBPriceMasterViewModel ABBPriceMasterVM, int userId)
        {
            
            if (ABBPriceMasterVM != null && ABBPriceMasterVM.ABBPriceMasterVMList != null && ABBPriceMasterVM.ABBPriceMasterVMList.Count > 0)
            {

                var ValidatedABBPriceMasterList = ABBPriceMasterVM.ABBPriceMasterVMList.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
                ABBPriceMasterVM.ABBPriceMasterVMErrorList = ABBPriceMasterVM.ABBPriceMasterVMList.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();

                //tblBusinessPartner = _mapper.Map<List<BusinessPartnerVMExcelModel>, List<TblBusinessPartner>>(ValidatedBusinessPartnerList);
                if (ValidatedABBPriceMasterList != null && ValidatedABBPriceMasterList.Count > 0)
                {
                    foreach (var item in ValidatedABBPriceMasterList)
                    {
                        try
                        {
                            if (item.PriceMasterId > 0)
                            {

                                TblAbbpriceMaster TblAbbpriceMaster = new TblAbbpriceMaster();

                                //Code to update the object 
                                var BusinessUnit = _businessUnitRepository.GetSingle(where: x => x.Name == item.CompanyName);
                                TblAbbpriceMaster.BusinessUnitId = BusinessUnit.BusinessUnitId;
                                TblAbbpriceMaster.Sponsor = item.CompanyName;
                                TblAbbpriceMaster.FeesApplicableAmt = item.FeesApplicableAmt;
                                TblAbbpriceMaster.FeesApplicablePercentage = item.FeesApplicablePercentage;
                                TblAbbpriceMaster.PlanPeriodInMonths = item.PlanPeriodInMonths;
                                TblAbbpriceMaster.FeeType = item.FeeType;
                                TblAbbpriceMaster.FeeTypeId = item.FeeTypeId;
                                TblAbbpriceMaster.PriceStartRange = item.PriceStartRange;
                                TblAbbpriceMaster.PriceEndRange = item.PriceEndRange;
                                var ProductCategory = _productCategoryRepository.GetSingle(where: x => x.Description == item.ProductCategory);
                                TblAbbpriceMaster.ProductCatId = ProductCategory.Id;
                                TblAbbpriceMaster.ProductCategory = item.ProductCategory;
                                var ProductType = _productTypeRepository.GetSingle(where: x => x.Description + x.Size == item.ProductSabcategory);
                                TblAbbpriceMaster.ProductSabcategory = ProductType.Description;
                                TblAbbpriceMaster.ProductTypeId = ProductType.Id;

                                TblAbbpriceMaster.BusinessUnitMarginAmount = item.BusinessUnitMarginAmount;
                                TblAbbpriceMaster.BusinessUnitMarginPerc = item.BusinessUnitMarginPerc;
                                TblAbbpriceMaster.BusinessPartnerMarginAmount = item.BusinessPartnerMarginAmount;
                                TblAbbpriceMaster.BusinessPartnerMarginPerc = item.BusinessPartnerMarginPerc;
                                TblAbbpriceMaster.Gstexclusive = item.Gstexclusive;
                                TblAbbpriceMaster.Gstinclusive = item.Gstinclusive;
                                TblAbbpriceMaster.GstValueForNewProduct = item.GstValueForNewProduct;

                                TblAbbpriceMaster.ModifiedDate = _currentDatetime;
                                TblAbbpriceMaster.ModifiedBy = userId;
                                _AbbPriceMasterRepository.Update(TblAbbpriceMaster);
                                _AbbPriceMasterRepository.SaveChanges();

                            }
                            else
                            {
                                TblAbbpriceMaster TblAbbpriceMaster = new TblAbbpriceMaster();
                                //Code to insert the object 
                                var BusinessUnit = _businessUnitRepository.GetSingle(where: x => x.Name == item.CompanyName);
                                TblAbbpriceMaster.BusinessUnitId = BusinessUnit.BusinessUnitId;
                                TblAbbpriceMaster.Sponsor = item.CompanyName;
                                TblAbbpriceMaster.FeesApplicableAmt = item.FeesApplicableAmt;
                                TblAbbpriceMaster.FeesApplicablePercentage = item.FeesApplicablePercentage;
                                TblAbbpriceMaster.PlanPeriodInMonths = item.PlanPeriodInMonths;
                                TblAbbpriceMaster.FeeType = item.FeeType;
                                TblAbbpriceMaster.FeeTypeId = item.FeeTypeId;
                                TblAbbpriceMaster.PriceStartRange = item.PriceStartRange;
                                TblAbbpriceMaster.PriceEndRange = item.PriceEndRange;
                                var ProductCategory = _productCategoryRepository.GetSingle(where: x => x.Description == item.ProductCategory);
                                TblAbbpriceMaster.ProductCatId = ProductCategory.Id;
                                TblAbbpriceMaster.ProductCategory = item.ProductCategory;
                                var ProductType = _productTypeRepository.GetSingle(where: x => x.Description + x.Size == item.ProductSabcategory);
                                TblAbbpriceMaster.ProductSabcategory = ProductType.Description;
                                TblAbbpriceMaster.ProductTypeId = ProductType.Id;

                                TblAbbpriceMaster.BusinessUnitMarginAmount = item.BusinessUnitMarginAmount;
                                TblAbbpriceMaster.BusinessUnitMarginPerc = item.BusinessUnitMarginPerc;
                                TblAbbpriceMaster.BusinessPartnerMarginAmount = item.BusinessPartnerMarginAmount;
                                TblAbbpriceMaster.BusinessPartnerMarginPerc = item.BusinessPartnerMarginPerc;
                                TblAbbpriceMaster.Gstexclusive = item.Gstexclusive;
                                TblAbbpriceMaster.Gstinclusive = item.Gstinclusive;
                                TblAbbpriceMaster.GstValueForNewProduct = item.GstValueForNewProduct;
                               
                                TblAbbpriceMaster.IsActive = true;
                                TblAbbpriceMaster.CreatedDate = _currentDatetime;
                               
                                _AbbPriceMasterRepository.Create(TblAbbpriceMaster);
                                _AbbPriceMasterRepository.SaveChanges();

                            }
                        }

                        catch (Exception ex)
                        {
                            item.Remarks += ex.Message + ", ";
                            ABBPriceMasterVM.ABBPriceMasterVMList.Add(item);
                        }
                    }
                }
            }

            return ABBPriceMasterVM;
        }

        /// <summary>
        /// Method to get the ABBPlanMaster by id 
        /// </summary>
        /// <param name="id">ABBPlanMasterId</param>
        /// <returns>ABBPlanMasterViewModel</returns>
        public ABBPriceMasterViewModel GetABBPriceMasterById(int id)
        {
            ABBPriceMasterViewModel ABBPriceMasterVM = null;
            TblAbbpriceMaster TblAbbpriceMaster = null;

            try
            {
                TblAbbpriceMaster = _AbbPriceMasterRepository.GetSingle(where: x => x.PriceMasterId == id);
                if (TblAbbpriceMaster != null)
                {
                    ABBPriceMasterVM = _mapper.Map<TblAbbpriceMaster, ABBPriceMasterViewModel>(TblAbbpriceMaster);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ABBPlanMasterManager", "GetABBPlanMasterById", ex);
            }
            return ABBPriceMasterVM;
        }

        /// <summary>
        /// Method to delete ABBPlanMaster by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletABBPriceMasterById(int id)
        {
            bool flag = false;
            try
            {
                TblAbbpriceMaster TblAbbpriceMaster = _AbbPriceMasterRepository.GetSingle(x => x.PriceMasterId == id);
                if (TblAbbpriceMaster != null)
                {
                    //TblProductCategory.IsActive = false;
                    _AbbPriceMasterRepository.Update(TblAbbpriceMaster);
                    _AbbPriceMasterRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ABBPlanMasterManager", "DeletABBPlanMasterById", ex);
            }
            return flag;
        }

        /// <summary>
        /// Method to get the All ABBPlanMaster
        /// </summary>     
        /// <returns>ABBPlanMasterViewModel</returns>
        public IList<ABBPriceMasterViewModel> GetAllABBPriceMaster()
        {
            IList<ABBPriceMasterViewModel> ABBPriceMasterVMList = null;
            List<TblAbbpriceMaster> TblAbbpriceMasterlist = new List<TblAbbpriceMaster>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblAbbpriceMasterlist = _AbbPriceMasterRepository.GetList(x => x.IsActive == true).ToList();
                if (TblAbbpriceMasterlist != null && TblAbbpriceMasterlist.Count > 0)
                {
                    ABBPriceMasterVMList = _mapper.Map<IList<TblAbbpriceMaster>, IList<ABBPriceMasterViewModel>>(TblAbbpriceMasterlist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductCategoryManager", "GetAllProductCategory", ex);
            }
            return ABBPriceMasterVMList;
        }


    }
}
