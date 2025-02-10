using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Master;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.MobileApplicationModel;
using System.Net;
using RDCELERP.Model.Base;
using Microsoft.Extensions.Options;

namespace RDCELERP.BAL.MasterManager
{
    public class ProductCategoryManager : IProductCategoryManager
    {
        #region  Variable Declaration
        IProductCategoryRepository _ProductCategoryRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IUserRoleRepository _userRoleRepository;
        IBUProductCategoryMapping _bUProductCategoryMapping;
        IBusinessUnitRepository _businessUnitRepository;
        IProductCategoryRepository _productCategoryRepository;
        IABBPlanMasterRepository _aBBPlanMasterRepository;
        public readonly IOptions<ApplicationSettings> _baseConfig;
        IPriceMasterQuestionersRepository _priceMasterQuestionersRepository;
        #endregion

        public ProductCategoryManager(IProductCategoryRepository productCategoryRepository, IBusinessUnitRepository businessUnitRepository, IBUProductCategoryMapping bUProductCategoryMapping, IProductCategoryRepository ProductCategoryRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging, IABBPlanMasterRepository aBBPlanMasterRepository, IOptions<ApplicationSettings> baseConfig, IPriceMasterQuestionersRepository priceMasterQuestionersRepository)
        {
            _ProductCategoryRepository = ProductCategoryRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
            _logging = logging;
            _bUProductCategoryMapping = bUProductCategoryMapping;
            _businessUnitRepository = businessUnitRepository;
            _productCategoryRepository = productCategoryRepository;
            _aBBPlanMasterRepository = aBBPlanMasterRepository;
            _baseConfig = baseConfig;
            _priceMasterQuestionersRepository = priceMasterQuestionersRepository;
        }

        /// <summary>
        /// Method to manage (Add/Edit) ProductCategory 
        /// </summary>
        /// <param name="ProductCategoryVM">ProductCategoryVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageProductCategory(ProductCategoryViewModel ProductCategoryVM, int userId)
        {
            TblProductCategory TblProductCategory = new TblProductCategory();

            try
            {
                if (ProductCategoryVM != null)
                {
                    TblProductCategory = _mapper.Map<ProductCategoryViewModel, TblProductCategory>(ProductCategoryVM);

                    TblProductCategory = TrimHelper.TrimAllValuesInModel(TblProductCategory);


                    if (TblProductCategory.Id > 0)
                    {
                        //Code to update the object                      
                        TblProductCategory.ModifiedBy = userId;
                        TblProductCategory.ModifiedDate = _currentDatetime;
                        _ProductCategoryRepository.Update(TblProductCategory);
                    }
                    else
                    {
                        var Check = _ProductCategoryRepository.GetSingle(x => x.Name == ProductCategoryVM.Name);
                        if (Check == null)
                        {
                            //Code to Insert the object 
                            TblProductCategory.IsActive = true;
                            TblProductCategory.CreatedDate = _currentDatetime;
                            TblProductCategory.CreatedBy = userId;
                            _ProductCategoryRepository.Create(TblProductCategory);
                        }
                           
                    }
                    _ProductCategoryRepository.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductCategoryManager", "ManageProductCategory", ex);
            }

            return TblProductCategory.Id;
        }

        public ProductCategoryViewModel ManageProductCategoryBulk(ProductCategoryViewModel ProductCategoryVM, int userId)
        {
            List<TblProductCategory> tblProductCategory = new List<TblProductCategory>();

            if (ProductCategoryVM != null && ProductCategoryVM.ProductCategoryVMList != null && ProductCategoryVM.ProductCategoryVMList.Count > 0)
            {
                var ValidatedProductCategoryList = ProductCategoryVM.ProductCategoryVMList.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
                ProductCategoryVM.ProductCategoryVMErrorList = ProductCategoryVM.ProductCategoryVMList.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();

                tblProductCategory = _mapper.Map<List<ProductCategoryVMExcel>, List<TblProductCategory>>(ValidatedProductCategoryList);

                if (tblProductCategory != null && tblProductCategory.Count > 0)
                {
                    foreach (var item in ValidatedProductCategoryList)
                    {
                        try
                        {
                            if (item.Id > 0)
                            {

                                TblProductCategory TblProductCategory = new TblProductCategory();
                                //Code to update the object 
                                TblProductCategory.Name = item.Name;
                                TblProductCategory.Description = item.Description;
                                TblProductCategory.Code = item.Code;
                                TblProductCategory.DescriptionForAbb = item.DescriptionForAbb;
                                TblProductCategory.IsActive = item.IsActive;
                                TblProductCategory.IsAllowedForOld = item.IsAllowedForOld;
                                TblProductCategory.IsAllowedForNew = item.IsAllowedForNew;
                                TblProductCategory.ModifiedDate = _currentDatetime;
                                TblProductCategory.ModifiedBy = userId;

                                TblProductCategory = TrimHelper.TrimAllValuesInModel(TblProductCategory);

                                _ProductCategoryRepository.Update(TblProductCategory);
                                _ProductCategoryRepository.SaveChanges();

                            }
                            else
                            {


                                TblProductCategory TblProductCategory = new TblProductCategory();
                                //Code to update the object 
                                TblProductCategory.Name = item.Name;
                                TblProductCategory.Description = item.Description;
                                TblProductCategory.Code = item.Code;
                                TblProductCategory.DescriptionForAbb = item.DescriptionForAbb;
                                TblProductCategory.IsActive = item.IsActive;
                                TblProductCategory.IsAllowedForOld = item.IsAllowedForOld;
                                TblProductCategory.IsAllowedForNew = item.IsAllowedForNew;
                                TblProductCategory.CreatedDate = _currentDatetime;
                                TblProductCategory.CreatedBy = userId;

                                TblProductCategory = TrimHelper.TrimAllValuesInModel(TblProductCategory);

                                _ProductCategoryRepository.Create(TblProductCategory);
                                _ProductCategoryRepository.SaveChanges();
                            }
                        }

                        catch (Exception ex)
                        {
                            item.Remarks += ex.Message + ", ";
                            ProductCategoryVM.ProductCategoryVMList.Add(item);
                        }
                    }
                }
            }

            return ProductCategoryVM;
        }
         

            
       

        /// <summary>
        /// Method to get the ProductCategory by id 
        /// </summary>
        /// <param name="id">ProductCategoryId</param>
        /// <returns>ProductCategoryViewModel</returns>
        public Model.Master.ProductCategoryViewModel GetProductCategoryById(int id)
        {
            Model.Master.ProductCategoryViewModel ProductCategoryVM = null;
            TblProductCategory TblProductCategory = null;

            try
            {
                TblProductCategory = _ProductCategoryRepository.GetSingle(where: x =>  x.Id == id);
                if (TblProductCategory != null)
                {
                    ProductCategoryVM = _mapper.Map<TblProductCategory, Model.Master.ProductCategoryViewModel>(TblProductCategory);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductCategoryManager", "GetProductCategoryById", ex);
            }
            return ProductCategoryVM;
        }

        /// <summary>
        /// Method to delete ProductCategory by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletProductCategoryById(int id)
        {
            bool flag = false;
            try
            {
                TblProductCategory TblProductCategory = _ProductCategoryRepository.GetSingle(x => x.IsActive == true && x.Id == id);
                if (TblProductCategory != null)
                {
                    TblProductCategory.IsActive = false;
                    _ProductCategoryRepository.Update(TblProductCategory);
                    _ProductCategoryRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductCategoryManager", "DeletProductCategoryById", ex);
            }
            return flag;
        }

        /// <summary>
        /// Method to get the All ProductCategory
        /// </summary>     
        /// <returns>ProductCategoryViewModel</returns>
        public IList<ProductCategoryViewModel> GetAllProductCategory()
        {
            IList<ProductCategoryViewModel> ProductCategoryVMList = null;
            List<TblProductCategory> TblProductCategorylist = new List<TblProductCategory>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblProductCategorylist = _ProductCategoryRepository.GetList(x => x.IsActive == true).ToList();

                if (TblProductCategorylist != null && TblProductCategorylist.Count > 0)
                {
                    ProductCategoryVMList = _mapper.Map<IList<TblProductCategory>, IList<ProductCategoryViewModel>>(TblProductCategorylist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductCategoryManager", "GetAllProductCategory", ex);
            }
            return ProductCategoryVMList;
        }

        public ExecutionResult GetProductCategory()
        {
            IList<ProductCategoryViewModel> ProductCategoryVMList = null;
            List<TblProductCategory> TblProductCategorylist = new List<TblProductCategory>();


            // TblUseRole TblUseRole = null;
            try
            {

                TblProductCategorylist = _ProductCategoryRepository.GetList(x => x.IsActive == true).ToList();

                if (TblProductCategorylist != null && TblProductCategorylist.Count > 0)
                {
                    ProductCategoryVMList = _mapper.Map<IList<TblProductCategory>, IList<ProductCategoryViewModel>>(TblProductCategorylist);
                    return new ExecutionResult(new InfoMessage(true, "Success", ProductCategoryVMList));

                }

                else
                {
                    return new ExecutionResult(new InfoMessage(true, "No data found"));


                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PinCodeManager", "GetPinCode", ex);
            }
            return new ExecutionResult(new InfoMessage(true, "No data found"));
        }

        /// <summary>
        /// Method to get the Product Category by id 
        /// </summary>
        /// <param name="id">ProductCategoryId</param>
        /// <returns>ProductcategoryViewModel</returns>
        public ExecutionResult ProductCategoryById(int id)
        {
            ProductCategoryViewModel ProductCategoryVM = null;
            TblProductCategory TblProductCategory = null;

            try
            {
                TblProductCategory = _ProductCategoryRepository.GetSingle(where: x => x.IsActive == true && x.Id == id);
                if (TblProductCategory != null)
                {
                    ProductCategoryVM = _mapper.Map<TblProductCategory, ProductCategoryViewModel>(TblProductCategory);
                    return new ExecutionResult(new InfoMessage(true, "Success", ProductCategoryVM));

                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductcategoryManager", "ProductcategoryById", ex);
            }
            return new ExecutionResult(new InfoMessage(true, "No data found"));
        }

        /// <summary>
        /// Method to get the list of Category by BUID
        /// </summary>
        /// <param name="buid">buid</param>
        /// <returns>List of ProductCategoryDataContract</returns>
        #region add for questioners api
        public ResponseResult GetCategoryListByBUId(string username)
        {
            ResponseResult responseResult = new ResponseResult();
            List<BuProductCatDataModel> buProductCatDataModelList = new List<BuProductCatDataModel>();
            List<TblBuproductCategoryMapping> productCategoryForNew = new List<TblBuproductCategoryMapping>();
            try
            {
                TblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => !string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Equals(username.ToLower()));
                if (businessUnit != null)
                {
                    productCategoryForNew  = _bUProductCategoryMapping.GetList(x=>x.IsActive== true && x.BusinessUnitId == businessUnit.BusinessUnitId && x.IsExchange==true)
                    .GroupBy(x => x.ProductCatId).Select(g => g.FirstOrDefault()).ToList();
                }
                if (productCategoryForNew.Count > 0)
                {
                    foreach (var productCategory in productCategoryForNew)
                    {
                        TblProductCategory productObj = _productCategoryRepository.GetSingle(x => x.Id == productCategory.ProductCatId && x.IsActive == true);
                        if (productObj != null)
                        {
                            BuProductCatDataModel buProductCatDataModel = new BuProductCatDataModel();

                            buProductCatDataModel = _mapper.Map<TblProductCategory, BuProductCatDataModel>(productObj);

                            #region Add images path for Product Category
                            string imagepath = string.Empty;

                            if (!string.IsNullOrEmpty(buProductCatDataModel.ProductCategoryImage))
                            {
                               
                                imagepath = _baseConfig.Value.BaseURL + "DBFiles\\Masters\\ProductCategoryImages\\" + buProductCatDataModel.ProductCategoryImage;

                                if (!string.IsNullOrEmpty(imagepath))
                                {
                                    buProductCatDataModel.ProductCategoryImage = "";
                                    buProductCatDataModel.ProductCategoryImage = imagepath;
                                }
                                imagepath = string.Empty;
                            }
                            #endregion

                            buProductCatDataModelList.Add(buProductCatDataModel);
                        }
                        
                    }
                    if (buProductCatDataModelList!=null && buProductCatDataModelList.Count > 0)
                    {
                        responseResult.Data = buProductCatDataModelList;
                        responseResult.message = "Success";
                        responseResult.Status = true;
                        responseResult.Status_Code = HttpStatusCode.OK;
                    }
                    else
                    {
                        responseResult.Data = buProductCatDataModelList;
                        responseResult.message = "No data found for the bussiness unit";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                    
                }
                else
                {
                    buProductCatDataModelList = null;
                    responseResult.Data = buProductCatDataModelList;
                    responseResult.message = "Not Success";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }

            }
            catch (Exception ex)
            {
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("ProductCategoryManager", "GetCategoryListByBUId", ex);
            }
            return responseResult;


        }

        public IList<ProductCategoryViewModel> GetProductCategoryDescById(int id)
        {
            IList<ProductCategoryViewModel> ProductCategoryVMList = null;
            List<TblProductCategory> TblProductCategorylist = new List<TblProductCategory>();
            TblProductCategory TblProductCategory = null;
            // TblUseRole TblUseRole = null;
            try
            {
               
                TblProductCategorylist = _ProductCategoryRepository.GetList(x => x.Id == id).ToList();

                if (TblProductCategorylist != null && TblProductCategorylist.Count > 0)
                {
                    ProductCategoryVMList = _mapper.Map<IList<TblProductCategory>, IList<ProductCategoryViewModel>>(TblProductCategorylist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductCategoryManager", "GetProductCategoryDescById", ex);
            }
            return ProductCategoryVMList;
        }
        #endregion

        #region Get Product Category on the basis of AbbPlanMaster 
        /// Get Product category by  AbbPlanMaster 
        /// </summary>
        /// <returns></returns>
        public List<BuProductCatDataModel> GetAllProductCategoryByAbbPlanMaster(int Buid)
        {
            List<TblAbbplanMaster> tblAbbplanMasters = new List<TblAbbplanMaster>();
            List<BuProductCatDataModel> buProductCatDataModelList = new List<BuProductCatDataModel>();
            try
            {
                if (Buid > 0)
                {
                    tblAbbplanMasters = _aBBPlanMasterRepository.GetList(x => x.IsActive == true && x.BusinessUnitId==Buid)
                                      .GroupBy(x => x.ProductCatId).Select(g => g.FirstOrDefault()).ToList();
                    if (tblAbbplanMasters!=null && tblAbbplanMasters.Count > 0)
                    {
                        foreach (var productCat in tblAbbplanMasters)
                        {
                            TblProductCategory prodCatData = _productCategoryRepository.GetSingle(x => x.IsActive == true && x.Id == productCat.ProductCatId);
                            if (prodCatData != null)
                            {
                                BuProductCatDataModel buProductCatDataModel = new BuProductCatDataModel();
                                buProductCatDataModel = _mapper.Map<TblProductCategory, BuProductCatDataModel>(prodCatData);
                                buProductCatDataModelList.Add(buProductCatDataModel);
                            }

                        }
                    }
                    else
                    {
                        buProductCatDataModelList = new List<BuProductCatDataModel>();
                    }
                }
                else
                {
                    buProductCatDataModelList = new List<BuProductCatDataModel>();
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("AbbRegistrationManager", "GetAllProductCategoryByAbbPlanMaster", ex);
            }
            return buProductCatDataModelList;
        }
        #endregion

        #region Get Product Category for Diagnose V2
        public ResponseResult GetProdCatListForDiagnose()
        {
            ResponseResult responseResult = new ResponseResult();
            List<BuProductCatDataModel> buProductCatDataModelList = new List<BuProductCatDataModel>();
            List<TblPriceMasterQuestioner>? priceMasterQuestList = new List<TblPriceMasterQuestioner>();
            try
            {
                priceMasterQuestList = _priceMasterQuestionersRepository.GetProdCatList();
                if (priceMasterQuestList != null && priceMasterQuestList.Count > 0)
                {
                    foreach (TblPriceMasterQuestioner productCategory in priceMasterQuestList)
                    {
                        TblProductCategory productObj = productCategory?.ProductCat;
                        if (productObj != null)
                        {
                            BuProductCatDataModel buProductCatDataModel = new BuProductCatDataModel();
                            buProductCatDataModel = _mapper.Map<TblProductCategory, BuProductCatDataModel>(productObj);

                            #region Add images path for Product Category
                            string imagepath = string.Empty;

                            if (!string.IsNullOrEmpty(buProductCatDataModel.ProductCategoryImage))
                            {
                                imagepath = _baseConfig.Value.BaseURL + "DBFiles/Masters/ProductCategoryImages/" + buProductCatDataModel.ProductCategoryImage;

                                if (!string.IsNullOrEmpty(imagepath))
                                {
                                    buProductCatDataModel.ProductCategoryImage = "";
                                    buProductCatDataModel.ProductCategoryImage = imagepath;
                                }
                                imagepath = string.Empty;
                            }
                            #endregion

                            buProductCatDataModelList.Add(buProductCatDataModel);
                        }
                    }
                    if (buProductCatDataModelList != null && buProductCatDataModelList.Count > 0)
                    {
                        responseResult.Data = buProductCatDataModelList;
                        responseResult.message = "Success";
                        responseResult.Status = true;
                        responseResult.Status_Code = HttpStatusCode.OK;
                    }
                    else
                    {
                        responseResult.Data = buProductCatDataModelList;
                        responseResult.message = "No data found for the bussiness unit";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    buProductCatDataModelList = null;
                    responseResult.Data = buProductCatDataModelList;
                    responseResult.message = "Not Success";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }

            }
            catch (Exception ex)
            {
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("ProductCategoryManager", "GetCategoryListByBUId", ex);
            }
            return responseResult;
        }
        #endregion
    }
}

