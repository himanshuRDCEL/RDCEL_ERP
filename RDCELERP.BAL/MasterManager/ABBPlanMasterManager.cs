using AutoMapper;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.ABBPlanMaster;

namespace RDCELERP.BAL.MasterManager
{
    public class ABBPlanMasterManager : IABBPlanMasterManager
    {
        #region  Variable Declaration
        IABBPlanMasterRepository _AbbPlanMasterRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IUserRoleRepository _userRoleRepository;
        IBusinessUnitRepository _businessUnitRepository;
        IABBPriceMasterRepository _aBBPriceMasterRepository;
        IProductCategoryRepository _productCategoryRepository;
        IProductTypeRepository _productTypeRepository;

        #endregion

        public ABBPlanMasterManager(IABBPlanMasterRepository AbbPlanMasterRepository, IProductTypeRepository productTypeRepository, IProductCategoryRepository productCategoryRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging, IBusinessUnitRepository businessUnitRepository, IABBPriceMasterRepository aBBPriceMasterRepository)
        {
            _AbbPlanMasterRepository = AbbPlanMasterRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
            _logging = logging;
            _businessUnitRepository = businessUnitRepository;
            _aBBPriceMasterRepository = aBBPriceMasterRepository;
            _productCategoryRepository = productCategoryRepository;
            _productTypeRepository = productTypeRepository;
        }


        /// <summary>
        /// Method to manage (Add/Edit) ABBPlanMaster 
        /// </summary>
        /// <param name="ABBPlanMasterVM">ABBPlanMasterVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageABBPlanMaster(ABBPlanMasterViewModel ABBPlanMasterVM,  List<PlanDetails> planDetails, List<PlanDetails> remainingplans, int userId)
        {
            TblAbbplanMaster TblAbbplanMaster = new TblAbbplanMaster();

            try
            {
                if (ABBPlanMasterVM != null)
                {
                    TblAbbplanMaster = _mapper.Map<ABBPlanMasterViewModel, TblAbbplanMaster>(ABBPlanMasterVM);
                    var BusinessUnit = _businessUnitRepository.GetSingle(where: x => x.BusinessUnitId == TblAbbplanMaster.BusinessUnitId);

                    // generate fields based on plan period

                    if (TblAbbplanMaster.PlanMasterId > 0)
                    {
                        //Code to update the object
                        if (BusinessUnit != null)
                        {
                            TblAbbplanMaster.Sponsor = BusinessUnit.Name;
                        }
                        TblAbbplanMaster.ModifiedBy = userId;
                        TblAbbplanMaster.ModifiedDate = _currentDatetime;
                        TblAbbplanMaster = TrimHelper.TrimAllValuesInModel(TblAbbplanMaster);
                        _AbbPlanMasterRepository.Update(TblAbbplanMaster);
                        _AbbPlanMasterRepository.SaveChanges();
                        if (remainingplans != null && remainingplans.Count > 0)
                        {
                            foreach (var item in remainingplans)
                            {
                                TblAbbplanMaster TblAbbplanMasters = new TblAbbplanMaster();
                                if (BusinessUnit != null)
                                {
                                    TblAbbplanMasters.Sponsor = BusinessUnit.Name;
                                }
                                TblAbbplanMasters.AbbplanName = TblAbbplanMaster.AbbplanName;
                                TblAbbplanMasters.PlanMasterId = Convert.ToInt32(item.PlanMasterId);
                                TblAbbplanMasters.BusinessUnitId = TblAbbplanMaster.BusinessUnitId;
                                TblAbbplanMasters.ProductCatId = TblAbbplanMaster.ProductCatId;
                                TblAbbplanMasters.ProductTypeId = TblAbbplanMaster.ProductTypeId;
                                TblAbbplanMasters.PlanPeriodInMonth = TblAbbplanMaster.PlanPeriodInMonth;
                                TblAbbplanMasters.NoClaimPeriod = TblAbbplanMaster.NoClaimPeriod;
                                TblAbbplanMasters.FromMonth = Convert.ToInt32(item.fromMonth);
                                TblAbbplanMasters.ToMonth = Convert.ToInt32(item.toMonth);
                                TblAbbplanMasters.AssuredBuyBackPercentage = Convert.ToInt32(item.percentage);
                                TblAbbplanMasters.IsActive = true;
                                TblAbbplanMasters.ModifiedBy = userId;
                                TblAbbplanMasters.ModifiedDate = _currentDatetime;
                                TblAbbplanMaster = TrimHelper.TrimAllValuesInModel(TblAbbplanMaster);
                                _AbbPlanMasterRepository.Update(TblAbbplanMasters);
                                _AbbPlanMasterRepository.SaveChanges();
                            }

                            if (planDetails != null && planDetails.Count > 0)
                            {
                                foreach (var item in planDetails)
                                {
                                    TblAbbplanMaster TblAbbplanMasters = new TblAbbplanMaster();
                                    if (BusinessUnit != null)
                                    {
                                        TblAbbplanMasters.Sponsor = BusinessUnit.Name;
                                    }
                                    TblAbbplanMasters.AbbplanName = TblAbbplanMaster.AbbplanName;
                                    TblAbbplanMasters.BusinessUnitId = TblAbbplanMaster.BusinessUnitId;
                                    TblAbbplanMasters.ProductCatId = TblAbbplanMaster.ProductCatId;
                                    TblAbbplanMasters.ProductTypeId = TblAbbplanMaster.ProductTypeId;
                                    TblAbbplanMasters.PlanPeriodInMonth = TblAbbplanMaster.PlanPeriodInMonth;
                                    TblAbbplanMasters.NoClaimPeriod = TblAbbplanMaster.NoClaimPeriod;
                                    TblAbbplanMasters.FromMonth = Convert.ToInt32(item.fromMonth);
                                    TblAbbplanMasters.ToMonth = Convert.ToInt32(item.toMonth);
                                    TblAbbplanMasters.AssuredBuyBackPercentage = Convert.ToInt32(item.percentage);
                                    TblAbbplanMasters.IsActive = true;
                                    TblAbbplanMasters.CreatedBy = userId;
                                    TblAbbplanMasters.CreatedDate = _currentDatetime;
                                    TblAbbplanMaster = TrimHelper.TrimAllValuesInModel(TblAbbplanMaster);
                                    _AbbPlanMasterRepository.Create(TblAbbplanMasters);
                                    _AbbPlanMasterRepository.SaveChanges();
                                }

                            }


                        }
                    }
                    else
                    {
                        if (BusinessUnit != null)
                        {
                            TblAbbplanMaster.Sponsor = BusinessUnit.Name;
                        }
                        TblAbbplanMaster.IsActive = true;
                        TblAbbplanMaster.CreatedBy = userId;
                        TblAbbplanMaster.CreatedDate = _currentDatetime;
                        TblAbbplanMaster = TrimHelper.TrimAllValuesInModel(TblAbbplanMaster);
                        _AbbPlanMasterRepository.Create(TblAbbplanMaster);
                        _AbbPlanMasterRepository.SaveChanges();
                        if (planDetails != null && planDetails.Count > 0)
                        {
                            foreach (var item in planDetails)
                            {
                                TblAbbplanMaster TblAbbplanMasters = new TblAbbplanMaster();
                                if (BusinessUnit != null)
                                {
                                    TblAbbplanMasters.Sponsor = BusinessUnit.Name;
                                }
                                TblAbbplanMasters.AbbplanName = TblAbbplanMaster.AbbplanName;
                                TblAbbplanMasters.BusinessUnitId = TblAbbplanMaster.BusinessUnitId;
                                TblAbbplanMasters.ProductCatId = TblAbbplanMaster.ProductCatId;
                                TblAbbplanMasters.ProductTypeId = TblAbbplanMaster.ProductTypeId;
                                TblAbbplanMasters.PlanPeriodInMonth = TblAbbplanMaster.PlanPeriodInMonth;
                                TblAbbplanMasters.NoClaimPeriod = TblAbbplanMaster.NoClaimPeriod;
                                TblAbbplanMasters.FromMonth = Convert.ToInt32(item.fromMonth);
                                TblAbbplanMasters.ToMonth = Convert.ToInt32(item.toMonth);
                                TblAbbplanMasters.AssuredBuyBackPercentage = Convert.ToInt32(item.percentage);
                                TblAbbplanMasters.IsActive = true;
                                TblAbbplanMasters.CreatedBy = userId;
                                TblAbbplanMasters.CreatedDate = _currentDatetime;
                                TblAbbplanMaster = TrimHelper.TrimAllValuesInModel(TblAbbplanMaster);
                                _AbbPlanMasterRepository.Create(TblAbbplanMasters);
                                _AbbPlanMasterRepository.SaveChanges();
                            }
                            
                        }
                    }


                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ABBPlanMasterManager", "ManageABBPlanMaster", ex);
            }

            return TblAbbplanMaster.PlanMasterId;
        }

        public ABBPlanMasterViewModel ManageABBPlanMasterBulk(ABBPlanMasterViewModel ABBPlanMasterVM, int userId)
        {
            
            if (ABBPlanMasterVM != null && ABBPlanMasterVM.ABBPlanMasterVMList != null && ABBPlanMasterVM.ABBPlanMasterVMList.Count > 0)
            {

                var ValidatedABBPlanMasterList = ABBPlanMasterVM.ABBPlanMasterVMList.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
                ABBPlanMasterVM.ABBPlanMasterVMErrorList = ABBPlanMasterVM.ABBPlanMasterVMList.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();

                //tblBusinessPartner = _mapper.Map<List<BusinessPartnerVMExcelModel>, List<TblBusinessPartner>>(ValidatedBusinessPartnerList);
                if (ValidatedABBPlanMasterList != null && ValidatedABBPlanMasterList.Count > 0)
                {
                    foreach (var item in ValidatedABBPlanMasterList)
                    {
                        try
                        {
                            

                            if (item.PlanMasterId > 0)
                            {

                                TblAbbplanMaster TblAbbplanMaster = new TblAbbplanMaster();

                                //Code to update the object 
                                var BusinessUnit = _businessUnitRepository.GetSingle(where: x => x.Name == item.CompanyName);
                                TblAbbplanMaster.BusinessUnitId = BusinessUnit.BusinessUnitId;
                                TblAbbplanMaster.Sponsor = item.CompanyName;
                                TblAbbplanMaster.FromMonth = item.FromMonth;
                                TblAbbplanMaster.ToMonth = item.ToMonth;
                                TblAbbplanMaster.PlanPeriodInMonth = item.PlanPeriodInMonth;
                                TblAbbplanMaster.AbbplanName = item.AbbplanName;
                                TblAbbplanMaster.AssuredBuyBackPercentage = item.AssuredBuyBackPercentage;
                                TblAbbplanMaster.NoClaimPeriod = item.NoClaimPeriod;
                                var ProductCategory = _productCategoryRepository.GetSingle(where: x => x.Description == item.ProductCategoryName);
                                TblAbbplanMaster.ProductCatId = ProductCategory.Id;
                                var ProductType = _productTypeRepository.GetSingle(where: x => x.Description + x.Size == item.ProductTypeName);
                                
                                TblAbbplanMaster.ProductTypeId = ProductType.Id;
                                

                                TblAbbplanMaster.ModifiedDate = _currentDatetime;
                                TblAbbplanMaster.ModifiedBy = userId;

                                _AbbPlanMasterRepository.Update(TblAbbplanMaster);
                                _AbbPlanMasterRepository.SaveChanges();

                            }
                            else
                            {


                                TblAbbplanMaster TblAbbplanMaster = new TblAbbplanMaster();
                                //Code to insert the object 
                                var BusinessUnit = _businessUnitRepository.GetSingle(where: x => x.Name == item.CompanyName);
                                TblAbbplanMaster.BusinessUnitId = BusinessUnit.BusinessUnitId;
                                TblAbbplanMaster.Sponsor = item.CompanyName;
                                TblAbbplanMaster.FromMonth = item.FromMonth;
                                TblAbbplanMaster.ToMonth = item.ToMonth;
                                TblAbbplanMaster.PlanPeriodInMonth = item.PlanPeriodInMonth;
                                TblAbbplanMaster.AbbplanName = item.AbbplanName;
                                TblAbbplanMaster.AssuredBuyBackPercentage = item.AssuredBuyBackPercentage;
                                TblAbbplanMaster.NoClaimPeriod = item.NoClaimPeriod;
                                var ProductCategory = _productCategoryRepository.GetSingle(where: x => x.Description == item.ProductCategoryName);
                                TblAbbplanMaster.ProductCatId = ProductCategory.Id;

                                var ProductType = _productTypeRepository.GetSingle(where: x => x.Description + x.Size == item.ProductTypeName);

                                TblAbbplanMaster.ProductTypeId = ProductType.Id;

                                TblAbbplanMaster.IsActive = true;
                                TblAbbplanMaster.CreatedDate = _currentDatetime;
                                TblAbbplanMaster.CreatedBy = userId;

                                _AbbPlanMasterRepository.Create(TblAbbplanMaster);
                                _AbbPlanMasterRepository.SaveChanges();

                            }
                        }

                        catch (Exception ex)
                        {
                            item.Remarks += ex.Message + ", ";
                            ABBPlanMasterVM.ABBPlanMasterVMList.Add(item);
                        }
                    }
                }
            }

            return ABBPlanMasterVM;
        }
        /// <summary>
        /// Method to get the ABBPlanMaster by id 
        /// </summary>
        /// <param name="id">ABBPlanMasterId</param>
        /// <returns>ABBPlanMasterViewModel</returns>
        public ABBPlanMasterViewModel GetABBPlanMasterById(int id)
        {
            ABBPlanMasterViewModel ABBPlanMasterVM = null;
            TblAbbplanMaster TblAbbplanMaster = null;

            try
            {
                TblAbbplanMaster = _AbbPlanMasterRepository.GetSingle(where: x => x.PlanMasterId == id);
                if (TblAbbplanMaster != null)
                {
                    ABBPlanMasterVM = _mapper.Map<TblAbbplanMaster, ABBPlanMasterViewModel>(TblAbbplanMaster);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ABBPlanMasterManager", "GetABBPlanMasterById", ex);
            }
            return ABBPlanMasterVM;
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
                TblAbbplanMaster TblAbbplanMaster = _AbbPlanMasterRepository.GetSingle(x=> x.PlanMasterId == id);
                if (TblAbbplanMaster != null)
                {
                    //TblProductCategory.IsActive = false;
                    _AbbPlanMasterRepository.Update(TblAbbplanMaster);
                    _AbbPlanMasterRepository.SaveChanges();
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
        public IList<ABBPlanMasterViewModel> GetAllABBPlanMaster()
        {
            IList<ABBPlanMasterViewModel> ABBPlanMasterVMList = null;
            List<TblAbbplanMaster> TblAbbplanMasterlist = new List<TblAbbplanMaster>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblAbbplanMasterlist = _AbbPlanMasterRepository.GetList(x => x.IsActive == true).ToList();

                if (TblAbbplanMasterlist != null && TblAbbplanMasterlist.Count > 0)
                {
                    ABBPlanMasterVMList = _mapper.Map<IList<TblAbbplanMaster>, IList<ABBPlanMasterViewModel>>(TblAbbplanMasterlist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductCategoryManager", "GetAllProductCategory", ex);
            }
            return ABBPlanMasterVMList;
        }

        public Abbplandetail GetabbPlanPrice(int productCatId, int producttypeId, string productValue, string username)
        {
            List<TblAbbpriceMaster> abbplanpriceObj = new List<TblAbbpriceMaster>();
            string planPrice = string.Empty;
            Abbplandetail plan = new Abbplandetail();
            try
            {
                TblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => !string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Equals(username.ToLower()));
                if (productCatId > 0 && producttypeId > 0 && businessUnit.BusinessUnitId > 0 && productValue != string.Empty || productValue != null)
                {
                    abbplanpriceObj = _aBBPriceMasterRepository.GetList(x => x.ProductCatId == productCatId && x.ProductTypeId == producttypeId
                    && x.BusinessUnitId == businessUnit.BusinessUnitId && x.PriceStartRange <= Convert.ToInt32(productValue) && x.PriceEndRange >= Convert.ToInt32(productValue)).ToList();
                    if (abbplanpriceObj != null && abbplanpriceObj.Count > 0)
                    {
                        TblAbbplanMaster planmasterObj = _AbbPlanMasterRepository.GetSingle(x => x.ProductCatId == productCatId && x.ProductTypeId == producttypeId && x.BusinessUnitId == businessUnit.BusinessUnitId);
                        if (planmasterObj != null)
                        {
                            int productPrice = Convert.ToInt32(productValue);
                            foreach (var item in abbplanpriceObj)
                            {
                                if (item.PriceStartRange > 0 && item.PriceEndRange > 0)
                                {
                                    if (productPrice >= item.PriceStartRange && productPrice <= item.PriceEndRange)
                                    {
                                        plan.planprice = item.FeesApplicableAmt.ToString();
                                        plan.planduration = item.PlanPeriodInMonths.ToString();
                                        plan.NoClaimPeriod = planmasterObj.NoClaimPeriod;
                                        plan.planName = planmasterObj.AbbplanName;
                                    }
                                }
                                else if (item.FeesApplicablePercentage > 0)
                                {
                                    double productprice = Convert.ToDouble(productValue);
                                    double Fees_percentage = Convert.ToDouble(item.FeesApplicablePercentage);
                                    var planprice = (productprice * Fees_percentage) / 100;
                                    plan.planprice = planprice.ToString();
                                    plan.planduration = item.PlanPeriodInMonths.ToString();
                                    plan.NoClaimPeriod = planmasterObj.NoClaimPeriod;
                                    plan.planName = planmasterObj.AbbplanName;
                                }

                            }
                        }

                    }
                    else
                    {
                        plan = null;
                    }

                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ABBPlanMasterManager", "GetabbPlanPrice", ex);
            }
            return plan;
        }



    }
}
