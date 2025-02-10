using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.Product;

namespace RDCELERP.BAL.MasterManager
{
    public class ProductTypeManager : IProductTypeManager
    {
        #region  Variable Declaration
        IProductTypeRepository _ProductTypeRepository;
        IProductCategoryRepository _ProductCategoryRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IUserRoleRepository _userRoleRepository;

        ILoginRepository _LoginRepository;
        IPriceMasterRepository _priceMasterRepository;

        IABBPlanMasterRepository _aBBPlanMasterRepository;
        IProductTypeRepository _productTypeRepository;
        public readonly IOptions<ApplicationSettings> _baseConfig;
        IPriceMasterQuestionersRepository _priceMasterQuestionersRepository;

        #endregion

        public ProductTypeManager(IProductTypeRepository ProductTypeRepository, IProductCategoryRepository ProductCategoryRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging, ILoginRepository loginRepository, IPriceMasterRepository priceMasterRepository, IABBPlanMasterRepository aBBPlanMasterRepository, IProductTypeRepository productTypeRepository, IOptions<ApplicationSettings> baseConfig, IPriceMasterQuestionersRepository priceMasterQuestionersRepository)
        {
            _ProductTypeRepository = ProductTypeRepository;
            _userRoleRepository = userRoleRepository;
            _ProductCategoryRepository = ProductCategoryRepository;
            _mapper = mapper;
            _logging = logging;
            _LoginRepository = loginRepository;
            _priceMasterRepository = priceMasterRepository;
            _aBBPlanMasterRepository = aBBPlanMasterRepository;
            _productTypeRepository = productTypeRepository;
            _baseConfig = baseConfig;
            _priceMasterQuestionersRepository = priceMasterQuestionersRepository;
        }

        /// <summary>
        /// Method to manage (Add/Edit) ProductCategory 
        /// </summary>
        /// <param name="ProductCategoryVM">ProductCategoryVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageProductType(ProductTypeViewModel ProductTypeVM, int userId)
        {
            TblProductType TblProductType = new TblProductType();

            try
            {
                if (ProductTypeVM != null)
                {
                    TblProductType = _mapper.Map<ProductTypeViewModel, TblProductType>(ProductTypeVM);

                    TblProductType = TrimHelper.TrimAllValuesInModel(TblProductType);

                    if (TblProductType.Id > 0)
                    {
                        //Code to update the object                      
                        TblProductType.ModifiedBy = userId;
                        TblProductType.ModifiedDate = _currentDatetime;
                        _ProductTypeRepository.Update(TblProductType);
                    }
                    else
                    {
                        var Check = _ProductTypeRepository.GetSingle(x => x.Name == ProductTypeVM.Name);
                        if (Check == null)
                        {
                            //Code to Insert the object 
                            TblProductType.IsActive = true;
                            TblProductType.CreatedDate = _currentDatetime;
                            TblProductType.CreatedBy = userId;
                            _ProductTypeRepository.Create(TblProductType);
                        }

                    }
                    _ProductTypeRepository.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductCategoryManager", "ManageProductCategory", ex);
            }

            return TblProductType.Id;
        }


        public ProductTypeViewModel ManageProductTypeBulk(ProductTypeViewModel ProductTypeVM, int userId)
        {
            List<TblProductType> tblProductType = new List<TblProductType>();

            if (ProductTypeVM != null && ProductTypeVM.ProductTypeVMList != null && ProductTypeVM.ProductTypeVMList.Count > 0)
            {
                var ValidatedProductTypeList = ProductTypeVM.ProductTypeVMList.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
                ProductTypeVM.ProductTypeVMErrorList = ProductTypeVM.ProductTypeVMList.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();

                tblProductType = _mapper.Map<List<ProductTypeVMExcel>, List<TblProductType>>(ValidatedProductTypeList);
                if (tblProductType != null && tblProductType.Count > 0)
                {
                    foreach (var item in ValidatedProductTypeList)
                    {
                        try
                        {
                            if (item.Id > 0)
                            {

                                TblProductType TblProductType = new TblProductType();
                                //Code to update the object 
                                TblProductType.Name = item.Name;
                                TblProductType.Description = item.Description;
                                TblProductType.Code = item.Code;
                                var ProductCategory = _ProductCategoryRepository.GetSingle(where: x => x.IsActive == true && x.Description == item.ProductCategoryName);
                                TblProductType.ProductCatId = ProductCategory.Id;
                                TblProductType.DescriptionForAbb = item.DescriptionForAbb;
                                TblProductType.IsActive = item.IsActive;
                                TblProductType.IsAllowedForOld = item.IsAllowedForOld;
                                TblProductType.IsAllowedForNew = item.IsAllowedForNew;
                                TblProductType.ModifiedDate = _currentDatetime;
                                TblProductType.ModifiedBy = userId;

                                TblProductType = TrimHelper.TrimAllValuesInModel(TblProductType);

                                _ProductTypeRepository.Update(TblProductType);
                                _ProductTypeRepository.SaveChanges();

                            }
                            else
                            {


                                TblProductType TblProductType = new TblProductType();
                                //Code to update the object 
                                TblProductType.Name = item.Name;
                                TblProductType.Description = item.Description;
                                TblProductType.Code = item.Code;
                                var ProductCategory = _ProductCategoryRepository.GetSingle(where: x => x.IsActive == true && x.Description == item.ProductCategoryName);
                                TblProductType.ProductCatId = ProductCategory.Id;
                                TblProductType.DescriptionForAbb = item.DescriptionForAbb;
                                TblProductType.IsActive = item.IsActive;
                                TblProductType.IsAllowedForOld = item.IsAllowedForOld;
                                TblProductType.IsAllowedForNew = item.IsAllowedForNew;
                                TblProductType.CreatedDate = _currentDatetime;
                                TblProductType.CreatedBy = userId;

                                TblProductType = TrimHelper.TrimAllValuesInModel(TblProductType);

                                _ProductTypeRepository.Create(TblProductType);
                                _ProductTypeRepository.SaveChanges();

                            }
                        }

                        catch (Exception ex)
                        {
                            item.Remarks += ex.Message + ", ";
                            ProductTypeVM.ProductTypeVMList.Add(item);
                        }
                    }
                }
            }

            return ProductTypeVM;
        }


        /// <summary>
        /// Method to get the ProductType by id 
        /// </summary>
        /// <param name="id">ProductTypeId</param>
        /// <returns>ProductTypeViewModel</returns>
        public Model.Product.ProductTypeViewModel GetProductTypeById(int id)
        {
            Model.Product.ProductTypeViewModel ProductTypeVM = null;
            TblProductType TblProductType = null;

            try
            {

                TblProductType = _ProductTypeRepository.GetSingle(where: x => x.Id == id);
                if (TblProductType != null)
                {
                    ProductTypeVM = _mapper.Map<TblProductType, ProductTypeViewModel>(TblProductType);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductTypeManager", "GetProductTypeById", ex);
            }
            return ProductTypeVM;
        }

        /// <summary>
        /// Method to get ProductType by userId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public ProductTypeViewModel GetProductTypeListByUserId(int? userId)
        {
            ProductTypeViewModel ProductTypeVM = null;
            TblProductType TblProductType = null;
            TblUserRole TblUserRole = null;
            try
            {
                if (userId > 0)
                {
                    TblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == userId);
                    if (TblUserRole != null)
                    {
                        TblProductType = _ProductTypeRepository.GetSingle(x => x.IsActive == true && x.Id == TblProductType.Id);
                    }
                }
                if (TblProductType != null)
                {
                    ProductTypeVM = _mapper.Map<TblProductType, ProductTypeViewModel>(TblProductType);
                }
                else
                {
                    ProductTypeVM = new ProductTypeViewModel();
                }
            }

            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductTypeManager", "GetProductTypeByUserId", ex);
            }
            return ProductTypeVM;
        }


        public IList<ProductTypeViewModel> GetProductTypeBYCategoryDesc(string Description)
        {
            IList<ProductTypeViewModel> ProductTypeVMList = null;
            List<TblProductType> TblProductTypelist = new List<TblProductType>();
            TblProductCategory TblProductCategory = null;
            // TblUseRole TblUseRole = null;
            try
            {
                //TblState = _context.TblStates.Where(x => x.IsActive == true && x.Name == State);
                TblProductCategory = _ProductCategoryRepository.GetSingle(x => x.IsActive == true && x.Description == Description);
                TblProductTypelist = _ProductTypeRepository.GetList(x => x.ProductCatId == TblProductCategory.Id).ToList();

                if (TblProductTypelist != null && TblProductTypelist.Count > 0)
                {
                    ProductTypeVMList = _mapper.Map<IList<TblProductType>, IList<ProductTypeViewModel>>(TblProductTypelist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductTypeManager", "GetProductTypeBYCategory", ex);
            }
            return ProductTypeVMList;
        }


        public IList<ProductTypeViewModel> GetProductTypeDescById(int id)
        {
            IList<ProductTypeViewModel> ProductTypeVMList = null;
            List<TblProductType> TblProductTypelist = new List<TblProductType>();
            TblProductType TblProductType = null;
            // TblUseRole TblUseRole = null;
            try
            {

                TblProductTypelist = _ProductTypeRepository.GetList(x => x.Id == id).ToList();

                if (TblProductTypelist != null && TblProductTypelist.Count > 0)
                {
                    ProductTypeVMList = _mapper.Map<IList<TblProductType>, IList<ProductTypeViewModel>>(TblProductTypelist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductTypeManager", "GetProductTypeDescById", ex);
            }
            return ProductTypeVMList;

        }

        /// <summary>
        /// Method to delete ProductType by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletProductTypeById(int id)
        {
            bool flag = false;
            try
            {
                TblProductType TblProductType = _ProductTypeRepository.GetSingle(x => x.IsActive == true && x.Id == id);
                if (TblProductType != null)
                {
                    TblProductType.IsActive = false;
                    _ProductTypeRepository.Update(TblProductType);
                    _ProductTypeRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductTypeManager", "DeletProductTypeById", ex);
            }
            return flag;
        }


        public IList<ProductTypeViewModel> GetAllProductType()
        {
            IList<ProductTypeViewModel> ProductTypeVMList = null;
            List<TblProductType> TblProductTypelist = new List<TblProductType>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblProductTypelist = _ProductTypeRepository.GetList(x => x.IsActive == true).ToList();

                if (TblProductTypelist != null && TblProductTypelist.Count > 0)
                {
                    ProductTypeVMList = _mapper.Map<IList<TblProductType>, IList<ProductTypeViewModel>>(TblProductTypelist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductTypeManager", "GetAllProductType", ex);
            }
            return ProductTypeVMList;
        }

        public ExecutionResult GetProductType()
        {
            IList<ProductTypeViewModel> ProductTypeVMList = null;
            List<TblProductType> TblProductTypelist = new List<TblProductType>();


            // TblUseRole TblUseRole = null;
            try
            {

                TblProductTypelist = _ProductTypeRepository.GetList(x => x.IsActive == true).ToList();

                if (TblProductTypelist != null && TblProductTypelist.Count > 0)
                {
                    ProductTypeVMList = _mapper.Map<IList<TblProductType>, IList<ProductTypeViewModel>>(TblProductTypelist);
                    return new ExecutionResult(new InfoMessage(true, "Success", ProductTypeVMList));

                }

                else
                {
                    return new ExecutionResult(new InfoMessage(true, "No data found"));


                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductTypeManager", "GetProductType", ex);
            }
            return new ExecutionResult(new InfoMessage(true, "No data found"));
        }

        /// <summary>
        /// Method to get the Brand by id 
        /// </summary>
        /// <param name="id">BrandId</param>
        /// <returns>BrandViewModel</returns>
        public ExecutionResult ProductTypeById(int id)
        {
            ProductTypeViewModel ProductTypeVM = null;
            TblProductType TblProductType = null;

            try
            {
                TblProductType = _ProductTypeRepository.GetSingle(where: x => x.IsActive == true && x.Id == id);
                if (TblProductType != null)
                {
                    ProductTypeVM = _mapper.Map<TblProductType, ProductTypeViewModel>(TblProductType);
                    return new ExecutionResult(new InfoMessage(true, "Success", ProductTypeVM));

                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductTypeManager", "ProductTypeById", ex);
            }
            return new ExecutionResult(new InfoMessage(true, "No data found"));
        }

        public IList<ProductTypeViewModel> GetProductTypeBYCategory(int? Id)
        {
            IList<ProductTypeViewModel> ProductTypeVMList = null;
            List<TblProductType> TblProductTypelist = new List<TblProductType>();
            TblProductCategory TblProductCategory = null;
            // TblUseRole TblUseRole = null;
            try
            {
                //TblState = _context.TblStates.Where(x => x.IsActive == true && x.Name == State);
                TblProductCategory = _ProductCategoryRepository.GetSingle(x => x.IsActive == true && x.Id == Id);
                TblProductTypelist = _ProductTypeRepository.GetList(x => x.ProductCatId == TblProductCategory.Id).ToList();

                if (TblProductTypelist != null && TblProductTypelist.Count > 0)
                {
                    ProductTypeVMList = _mapper.Map<IList<TblProductType>, IList<ProductTypeViewModel>>(TblProductTypelist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductTypeManager", "GetProductTypeBYCategory", ex);
            }
            return ProductTypeVMList;
        }

        public IList<ProductTypeViewModel> GetProductTypeBYCategoryDescription(string desc)
        {
            IList<ProductTypeViewModel> ProductTypeVMList = null;
            List<TblProductType> TblProductTypelist = new List<TblProductType>();
            TblProductCategory TblProductCategory = null;
            // TblUseRole TblUseRole = null;
            try
            {
                //TblState = _context.TblStates.Where(x => x.IsActive == true && x.Name == State);
                TblProductCategory = _ProductCategoryRepository.GetSingle(x => x.IsActive == true && x.Description == desc);
                TblProductTypelist = _ProductTypeRepository.GetList(x => x.ProductCatId == TblProductCategory.Id).ToList();

                if (TblProductTypelist != null && TblProductTypelist.Count > 0)
                {
                    ProductTypeVMList = _mapper.Map<IList<TblProductType>, IList<ProductTypeViewModel>>(TblProductTypelist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductTypeManager", "GetProductTypeBYCategory", ex);
            }
            return ProductTypeVMList;
        }


        #region Get Product type on the basis of product category
        /// <summary>
        /// Get ProductType By category
        /// </summary>
        /// <param name="ProductCategoryId"></param>
        /// <returns></returns>
        public IList<ProductTypeViewModel> GetProductTypeByCategoryId(int productCategoryId)
        {
            IList<ProductTypeViewModel> productTypeList = null;
            List<TblProductType> tblProductTypesList = new List<TblProductType>();

            try
            {
                tblProductTypesList = _ProductTypeRepository.GetList(x => x.IsActive == true && x.ProductCatId == productCategoryId).ToList();
                if (tblProductTypesList != null && tblProductTypesList.Count > 0)
                {
                    productTypeList = _mapper.Map<IList<TblProductType>, IList<ProductTypeViewModel>>(tblProductTypesList);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductTypeManager", "GetProductTypeByCategoryId", ex);
            }
            return productTypeList;
        }
        #endregion


        #region product type for bussiness unit
        /// <summary>
        ///   product type for bussiness unit
        ///   Used in api
        /// </summary>
        /// <param name="catid"></param>
        /// <returns></returns>
        public ResponseResult GetProductTypeListByBUId(int catid, string UserName)
        {
            ResponseResult responseResult = new ResponseResult();
            List<ProductTypeDataResponseModel> productTypeList = new List<ProductTypeDataResponseModel>();
            List<TblBuproductCategoryMapping> productCategoryForNew = new List<TblBuproductCategoryMapping>();
            List<TblPriceMaster> tblPriceMaster = new List<TblPriceMaster>();
            try
            {
                if (!string.IsNullOrEmpty(UserName) && catid > 0)
                {
                    Login tbllogin = _LoginRepository.GetSingle(x => !string.IsNullOrEmpty(x.Username) && x.Username.ToLower().Equals(UserName.ToLower()));
                    if (tbllogin != null)
                    {
                        tblPriceMaster = _priceMasterRepository.GetList(x => x.IsActive == true && x.ExchPriceCode == tbllogin.PriceCode && x.ProductCategoryId == catid)
                                         .GroupBy(x => x.ProductTypeId).Select(g => g.FirstOrDefault()).ToList();

                        if (tblPriceMaster.Count > 0)
                        {
                            foreach (var producttype in tblPriceMaster)
                            {
                                TblProductType prodTypeData = _ProductTypeRepository.GetSingle(x => x.IsActive == true && x.Id == producttype.ProductTypeId);
                                if (prodTypeData != null)
                                {
                                    ProductTypeDataResponseModel productType = new ProductTypeDataResponseModel();
                                    productType = _mapper.Map<TblProductType, ProductTypeDataResponseModel>(prodTypeData);

                                    #region Add images path for Questions
                                    string imagepath = string.Empty;

                                    if (!string.IsNullOrEmpty(productType.ProductTypeImage))
                                    {
                                        imagepath = _baseConfig.Value.BaseURL + "DBFiles\\Masters\\ProductTypeImages\\" + productType.ProductTypeImage;

                                        if (!string.IsNullOrEmpty(imagepath))
                                        {
                                            productType.ProductTypeImage = "";
                                            productType.ProductTypeImage = imagepath;
                                        }
                                        imagepath = string.Empty;
                                    }
                                    #endregion

                                    productTypeList.Add(productType);
                                }

                            }
                            productTypeList.RemoveAll(x => string.IsNullOrEmpty(x.Size));
                        }
                        else
                        {
                            responseResult.message = "error occurs while mapping the data from price master";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            return responseResult;
                        }
                    }
                    else
                    {
                        responseResult.message = "User Not found";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        return responseResult;
                    }

                    if (productTypeList != null && productTypeList.Count > 0)
                    {

                        responseResult.Data = productTypeList;
                        responseResult.message = "success";
                        responseResult.Status = true;
                        responseResult.Status_Code = HttpStatusCode.OK;
                        return responseResult;
                    }
                    else
                    {
                        responseResult.message = "No data found";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        return responseResult;
                    }
                }
                else
                {
                    responseResult.message = "Request Parameter should be interger type or greater than zero";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    return responseResult;
                }

            }
            catch (Exception ex)
            {
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("ProductTypeManager", "GetProductTypeListByBUId", ex);
            }
            return responseResult;
        }
        #endregion

        #region Get Product Type on the basis of AbbPlanMaster 
        /// <summary>
        /// Get Product Type by  AbbPlanMaster 
        /// </summary>
        /// <param name="NewProductCategoryId"></param>
        /// <returns></returns>
        public IList<ProductTypeViewModel> GetAllProductTypeByAbbPlanMaster(int NewProductCategoryId,int BuId)
        {
            IList<ProductTypeViewModel> productTypeViewModels = new List<ProductTypeViewModel>();

            List<TblAbbplanMaster> tblAbbplanMasters = new List<TblAbbplanMaster>();

            try
            {
                if ( NewProductCategoryId > 0 && BuId>0)
                {
                    tblAbbplanMasters = _aBBPlanMasterRepository.GetList(x => x.IsActive == true  && x.ProductCatId == NewProductCategoryId && x.BusinessUnitId== BuId)
                                     .GroupBy(x => x.ProductTypeId).Select(g => g.FirstOrDefault()).ToList();

                    if (tblAbbplanMasters.Count > 0)
                    {
                        foreach (var producttype in tblAbbplanMasters)
                        {
                            TblProductType prodTypeData = _productTypeRepository.GetSingle(x => x.IsActive == true && x.Id == producttype.ProductTypeId && x.IsAllowedForNew == true);
                            if (prodTypeData != null)
                            {
                                ProductTypeViewModel productTypeView = new ProductTypeViewModel();
                                productTypeView = _mapper.Map<TblProductType, ProductTypeViewModel>(prodTypeData);
                                if (string.IsNullOrEmpty(productTypeView.Size))
                                {
                                    //productTypeView.Description = productTypeView.Description + "(" + productTypeView.Size + ")";
                                    productTypeViewModels.Add(productTypeView);
                                }
                            }
                        }

                    }
                    else
                    {
                        productTypeViewModels = new List<ProductTypeViewModel>();
                    }
                }
                else
                {
                    productTypeViewModels = new List<ProductTypeViewModel>();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "GetAllProductCategoryByAbbPlanMaster", ex);
            }
            return productTypeViewModels;
        }
        #endregion

        

        #region Get Product Type List for Diagnose V2
        /// <summary>
        /// Get Product Type List for Diagnose V2
        /// </summary>
        /// <param name="catid"></param>
        /// <returns></returns>
        public ResponseResult GetProdTypeListByCatIdv2(int catid, string? UserName)
        {
            ResponseResult responseResult = new ResponseResult();
            List<ProductTypeDataResponseModel> productTypeList = new List<ProductTypeDataResponseModel>();
            List<TblBuproductCategoryMapping> productCategoryForNew = new List<TblBuproductCategoryMapping>();
            List<TblPriceMasterQuestioner>? priceMasterQuestList = new List<TblPriceMasterQuestioner>();
            try
            {
                if (!string.IsNullOrEmpty(UserName) && catid > 0)
                {
                    Login tbllogin = _LoginRepository.GetSingle(x => !string.IsNullOrEmpty(x.Username) && x.Username.ToLower().Equals(UserName.ToLower()));
                    if (tbllogin != null)
                    {
                        priceMasterQuestList = _priceMasterQuestionersRepository.GetProdTypeListByCatId(catid);
                        if (priceMasterQuestList != null && priceMasterQuestList.Count > 0)
                        {
                            foreach (TblPriceMasterQuestioner tblPriceMasterQuest in priceMasterQuestList)
                            {
                                TblProductType prodTypeData = tblPriceMasterQuest?.ProductType;
                                if (prodTypeData != null)
                                {
                                    ProductTypeDataResponseModel productType = new ProductTypeDataResponseModel();
                                    productType = _mapper.Map<TblProductType, ProductTypeDataResponseModel>(prodTypeData);

                                    #region Add images path for Questions
                                    string imagepath = string.Empty;

                                    if (!string.IsNullOrEmpty(productType.ProductTypeImage))
                                    {
                                        imagepath = _baseConfig.Value.BaseURL + @"DBFiles\\Masters\\ProductTypeImages\\" + productType.ProductTypeImage;

                                        if (!string.IsNullOrEmpty(imagepath))
                                        {
                                            productType.ProductTypeImage = "";
                                            productType.ProductTypeImage = imagepath;
                                        }
                                        imagepath = string.Empty;
                                    }
                                    #endregion

                                    productTypeList.Add(productType);
                                }

                            }
                            productTypeList.RemoveAll(x => string.IsNullOrEmpty(x.Size));
                        }
                        else
                        {
                            responseResult.message = "error occurs while mapping the data from price master questioners";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            return responseResult;
                        }
                    }
                    else
                    {
                        responseResult.message = "User Not found";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        return responseResult;
                    }

                    if (productTypeList != null && productTypeList.Count > 0)
                    {

                        responseResult.Data = productTypeList;
                        responseResult.message = "success";
                        responseResult.Status = true;
                        responseResult.Status_Code = HttpStatusCode.OK;
                        return responseResult;
                    }
                    else
                    {
                        responseResult.message = "No data found";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        return responseResult;
                    }
                }
                else
                {
                    responseResult.message = "Request Parameter should be interger type or greater than zero";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    return responseResult;
                }

            }
            catch (Exception ex)
            {
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("ProductTypeManager", "GetProductTypeListByBUId", ex);
            }
            return responseResult;
        }
        #endregion   
    }
}