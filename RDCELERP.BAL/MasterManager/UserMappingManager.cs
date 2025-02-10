//using AutoMapper;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using RDCELERP.BAL.Interface;
//using RDCELERP.Common.Helper;
//using RDCELERP.DAL.Entities;
//using RDCELERP.DAL.IRepository;
//using RDCELERP.DAL.Repository;
//using RDCELERP.Model.Base;
//using RDCELERP.Model.InfoMessage;
//using RDCELERP.Model.Master;
//using RDCELERP.Model.MobileApplicationModel;
//using RDCELERP.Model.UserMapping;

//namespace RDCELERP.BAL.MasterManager
//{
//    public class UserMappingManager : IUserMappingManager
//    {
//        #region  Variable Declaration
//        IUserMappingRepository _userMappingRepository;
//        private readonly IMapper _mapper;
//        ILogging _logging;
//        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
//        IUserRoleRepository _userRoleRepository;
//        IBusinessPartnerManager _businessPartnerManager;
//        IBusinessUnitRepository _businessUnitRepository;


//        public readonly IOptions<ApplicationSettings> _baseConfig;

//        #endregion

//        public UserMappingManager(IBusinessPartnerManager businessPartnerManager, IUserMappingRepository userMappingRepository, IBusinessUnitRepository businessUnitRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging, IOptions<ApplicationSettings> baseConfig)
//        {

//            _userRoleRepository = userRoleRepository;
//            _mapper = mapper;
//            _logging = logging;
//            _businessPartnerManager = businessPartnerManager;
//            _businessUnitRepository = businessUnitRepository;
//            _userMappingRepository = userMappingRepository;
//            _baseConfig = baseConfig;
//        }

//        /// <summary>
//        /// Method to manage (Add/Edit) User Mapping
//        /// </summary>
//        /// <param name="UserMappingVM">UserMappingVM</param>
//        /// <param name="userId">userId</param>
//        /// <returns>int</returns>
//        public int ManageUserMapping(UserMappingViewModel UserMappingVM, int userId)
//        {
//            TblUserMapping TblUserMapping = new TblUserMapping();

//            try
//            {
//                if (UserMappingVM != null)
//                {
//                    TblUserMapping = _mapper.Map<UserMappingViewModel, TblUserMapping>(UserMappingVM);


//                    if (TblUserMapping.UserMappingId > 0)
//                    {
//                        //Code to update the object                      
//                        TblUserMapping.ModifiedBy = userId;
//                        TblUserMapping.ModifiedDate = _currentDatetime;
//                        _userMappingRepository.Update(TblUserMapping);
//                    }
//                    else
//                    {
//                        //Code to Insert the object 
//                        TblUserMapping.IsActive = true;
//                        TblUserMapping.CreatedDate = _currentDatetime;
//                        TblUserMapping.CreatedBy = userId;
//                        _userMappingRepository.Create(TblProductCategory);

//                    }
//                    _ProductCategoryRepository.SaveChanges();


//                }
//            }
//            catch (Exception ex)
//            {
//                _logging.WriteErrorToDB("ProductCategoryManager", "ManageProductCategory", ex);
//            }

//            return TblProductCategory.Id;
//        }

//        public ProductCategoryViewModel ManageProductCategoryBulk(ProductCategoryViewModel ProductCategoryVM, int userId)
//        {
//            List<TblProductCategory> tblProductCategory = new List<TblProductCategory>();

//            if (ProductCategoryVM != null && ProductCategoryVM.ProductCategoryVMList != null && ProductCategoryVM.ProductCategoryVMList.Count > 0)
//            {
//                var ValidatedProductCategoryList = ProductCategoryVM.ProductCategoryVMList.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
//                ProductCategoryVM.ProductCategoryVMErrorList = ProductCategoryVM.ProductCategoryVMList.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();

//                tblProductCategory = _mapper.Map<List<ProductCategoryVMExcel>, List<TblProductCategory>>(ValidatedProductCategoryList);
//                if (tblProductCategory != null && tblProductCategory.Count > 0)
//                {
//                    foreach (var item in ValidatedProductCategoryList)
//                    {
//                        try
//                        {
//                            if (item.Id > 0)
//                            {

//                                TblProductCategory TblProductCategory = new TblProductCategory();
//                                //Code to update the object 
//                                TblProductCategory.Name = item.Name;
//                                TblProductCategory.Description = item.Description;
//                                TblProductCategory.Code = item.Code;
//                                TblProductCategory.DescriptionForAbb = item.DescriptionForAbb;
//                                TblProductCategory.IsActive = item.IsActive;
//                                TblProductCategory.IsAllowedForOld = item.IsAllowedForOld;
//                                TblProductCategory.IsAllowedForNew = item.IsAllowedForNew;
//                                TblProductCategory.ModifiedDate = _currentDatetime;
//                                TblProductCategory.ModifiedBy = userId;
//                                _ProductCategoryRepository.Update(TblProductCategory);
//                                _ProductCategoryRepository.SaveChanges();

//                            }
//                            else
//                            {


//                                TblProductCategory TblProductCategory = new TblProductCategory();
//                                //Code to update the object 
//                                TblProductCategory.Name = item.Name;
//                                TblProductCategory.Description = item.Description;
//                                TblProductCategory.Code = item.Code;
//                                TblProductCategory.DescriptionForAbb = item.DescriptionForAbb;
//                                TblProductCategory.IsActive = item.IsActive;
//                                TblProductCategory.IsAllowedForOld = item.IsAllowedForOld;
//                                TblProductCategory.IsAllowedForNew = item.IsAllowedForNew;
//                                TblProductCategory.CreatedDate = _currentDatetime;
//                                TblProductCategory.CreatedBy = userId;
//                                _ProductCategoryRepository.Create(TblProductCategory);
//                                _ProductCategoryRepository.SaveChanges();

//                            }
//                        }

//                        catch (Exception ex)
//                        {
//                            item.Remarks += ex.Message + ", ";
//                            ProductCategoryVM.ProductCategoryVMList.Add(item);
//                        }
//                    }
//                }
//            }

//            return ProductCategoryVM;
//        }


//        /// <summary>
//        /// Method to get the ProductCategory by id 
//        /// </summary>
//        /// <param name="id">ProductCategoryId</param>
//        /// <returns>ProductCategoryViewModel</returns>
//        public Model.Master.ProductCategoryViewModel GetProductCategoryById(int id)
//        {
//            Model.Master.ProductCategoryViewModel ProductCategoryVM = null;
//            TblProductCategory TblProductCategory = null;

//            try
//            {
//                TblProductCategory = _ProductCategoryRepository.GetSingle(where: x => x.Id == id);
//                if (TblProductCategory != null)
//                {
//                    ProductCategoryVM = _mapper.Map<TblProductCategory, Model.Master.ProductCategoryViewModel>(TblProductCategory);
//                }

//            }
//            catch (Exception ex)
//            {
//                _logging.WriteErrorToDB("ProductCategoryManager", "GetProductCategoryById", ex);
//            }
//            return ProductCategoryVM;
//        }

//        /// <summary>
//        /// Method to delete ProductCategory by id
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns>bool</returns>
//        public bool DeletProductCategoryById(int id)
//        {
//            bool flag = false;
//            try
//            {
//                TblProductCategory TblProductCategory = _ProductCategoryRepository.GetSingle(x => x.IsActive == true && x.Id == id);
//                if (TblProductCategory != null)
//                {
//                    TblProductCategory.IsActive = false;
//                    _ProductCategoryRepository.Update(TblProductCategory);
//                    _ProductCategoryRepository.SaveChanges();
//                    flag = true;
//                }
//            }
//            catch (Exception ex)
//            {
//                _logging.WriteErrorToDB("ProductCategoryManager", "DeletProductCategoryById", ex);
//            }
//            return flag;
//        }

//        /// <summary>
//        /// Method to get the All ProductCategory
//        /// </summary>     
//        /// <returns>ProductCategoryViewModel</returns>
//        public IList<ProductCategoryViewModel> GetAllProductCategory()
//        {
//            IList<ProductCategoryViewModel> ProductCategoryVMList = null;
//            List<TblProductCategory> TblProductCategorylist = new List<TblProductCategory>();
//            // TblUseRole TblUseRole = null;
//            try
//            {

//                TblProductCategorylist = _ProductCategoryRepository.GetList(x => x.IsActive == true).ToList();

//                if (TblProductCategorylist != null && TblProductCategorylist.Count > 0)
//                {
//                    ProductCategoryVMList = _mapper.Map<IList<TblProductCategory>, IList<ProductCategoryViewModel>>(TblProductCategorylist);
//                }
//            }
//            catch (Exception ex)
//            {
//                _logging.WriteErrorToDB("ProductCategoryManager", "GetAllProductCategory", ex);
//            }
//            return ProductCategoryVMList;
//        }

//        public ExecutionResult GetProductCategory()
//        {
//            IList<ProductCategoryViewModel> ProductCategoryVMList = null;
//            List<TblProductCategory> TblProductCategorylist = new List<TblProductCategory>();


//            // TblUseRole TblUseRole = null;
//            try
//            {

//                TblProductCategorylist = _ProductCategoryRepository.GetList(x => x.IsActive == true).ToList();

//                if (TblProductCategorylist != null && TblProductCategorylist.Count > 0)
//                {
//                    ProductCategoryVMList = _mapper.Map<IList<TblProductCategory>, IList<ProductCategoryViewModel>>(TblProductCategorylist);
//                    return new ExecutionResult(new InfoMessage(true, "Success", ProductCategoryVMList));

//                }

//                else
//                {
//                    return new ExecutionResult(new InfoMessage(true, "No data found"));


//                }
//            }
//            catch (Exception ex)
//            {
//                _logging.WriteErrorToDB("PinCodeManager", "GetPinCode", ex);
//            }
//            return new ExecutionResult(new InfoMessage(true, "No data found"));
//        }

//        /// <summary>
//        /// Method to get the Product Category by id 
//        /// </summary>
//        /// <param name="id">ProductCategoryId</param>
//        /// <returns>ProductcategoryViewModel</returns>
//        public ExecutionResult ProductCategoryById(int id)
//        {
//            ProductCategoryViewModel ProductCategoryVM = null;
//            TblProductCategory TblProductCategory = null;

//            try
//            {
//                TblProductCategory = _ProductCategoryRepository.GetSingle(where: x => x.IsActive == true && x.Id == id);
//                if (TblProductCategory != null)
//                {
//                    ProductCategoryVM = _mapper.Map<TblProductCategory, ProductCategoryViewModel>(TblProductCategory);
//                    return new ExecutionResult(new InfoMessage(true, "Success", ProductCategoryVM));

//                }

//            }
//            catch (Exception ex)
//            {
//                _logging.WriteErrorToDB("ProductcategoryManager", "ProductcategoryById", ex);
//            }
//            return new ExecutionResult(new InfoMessage(true, "No data found"));
//        }

//    }








