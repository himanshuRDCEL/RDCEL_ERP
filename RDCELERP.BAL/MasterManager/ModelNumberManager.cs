using AutoMapper;
using Microsoft.Extensions.Logging;
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
using RDCELERP.Model.ModelNumber;

namespace RDCELERP.BAL.MasterManager
{
    public class ModelNumberManager : IModelNumberManager
    {
        #region  Variable Declaration
        IModelNumberRepository _ModelNumberRepository;
        IModelMappingRepository _ModelMappingRepository;
        IBusinessUnitRepository _businessUnitRepository;
        IBusinessPartnerRepository _businessPartnerRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IUserRoleRepository _userRoleRepository;
        IProductCategoryRepository _productCategoryRepository;
        IProductTypeRepository _productTypeRepository;
        IBrandRepository _brandRepository;
        IBrandSmartBuyRepository _brandSmartBuyRepository;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        #endregion

        public ModelNumberManager(IModelNumberRepository ModelNumberRepository, IBusinessPartnerRepository businessPartnerRepository, IBrandRepository brandRepository, IBrandSmartBuyRepository brandSmartBuyRepository, IProductTypeRepository productTypeRepository, IProductCategoryRepository productCategoryRepository, IBusinessUnitRepository businessUnitRepository, IUserRoleRepository userRoleRepository, IModelMappingRepository ModelMappingRepository, IMapper mapper, ILogging logging, RDCELERP.DAL.Entities.Digi2l_DevContext context)
        {
            _ModelNumberRepository = ModelNumberRepository;
            _userRoleRepository = userRoleRepository;
            _businessUnitRepository = businessUnitRepository;
            _productCategoryRepository = productCategoryRepository;
            _productTypeRepository = productTypeRepository;
            _brandRepository = brandRepository;
            _brandSmartBuyRepository = brandSmartBuyRepository;
            _ModelMappingRepository = ModelMappingRepository;
            _businessPartnerRepository = businessPartnerRepository;
            _mapper = mapper;
            _logging = logging;
            _context = context;
        }


        /// <summary>
        /// Method to manage (Add/Edit) Sweetner
        /// </summary>
        /// <param name="ModelNumberVM">ModelNumberVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageSweetner(ModelNumberViewModel ModelNumberVM, int userId)
        {
            TblModelNumber TblModelNumber = null;
            TblBusinessUnit TblBusinessUnit = new TblBusinessUnit();

            if (ModelNumberVM != null)
            {
                TblModelNumber = _mapper.Map<ModelNumberViewModel, TblModelNumber>(ModelNumberVM);
                TblBusinessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == ModelNumberVM.BusinessUnitId && x.IsActive == true);
                if (TblBusinessUnit.IsSweetnerModelBased == true)
                {
                    if (ModelNumberVM.ModelList != null)
                    {
                        //Code to update the object

                        List<TblModelNumber> TblModelNumberlist = new List<TblModelNumber>();

                        foreach (var item in ModelNumberVM.ModelList)
                        {
                            var ModelNumber = _ModelNumberRepository.GetSingle(x => x.Code == item.Code && x.IsActive == true);
                            TblModelNumber modelNumber = new TblModelNumber(); // Create a new instance of TblModelNumber
                            modelNumber.ModelNumberId = ModelNumber.ModelNumberId;
                            modelNumber.SweetnerForDtc = ModelNumberVM.SweetnerForDtc;
                            modelNumber.SweetnerForDtd = ModelNumberVM.SweetnerForDtd;
                            modelNumber.BusinessUnitId = ModelNumberVM.BusinessUnitId;
                            modelNumber.ModifiedBy = userId;
                            modelNumber.ModifiedDate = _currentDatetime;
                            #region update Sweetner for DtoC & DtoD in tblModelNumber added by Kranti Silawat  
                            _ModelNumberRepository.UpdateModelNumber(modelNumber.ModelNumberId, modelNumber.SweetnerForDtc, modelNumber.SweetnerForDtd, modelNumber.BusinessUnitId, userId);

                            #endregion
                        }

                    }

                }
                else
                {
                    if (TblModelNumber.ModelNumberId == 0 && TblModelNumber.ProductCategoryId == null)
                    {
                        TblBusinessUnit TblbusinessUnit = new TblBusinessUnit(); // Create a new instance of TblModelNumber
                        TblbusinessUnit.BusinessUnitId = (int)ModelNumberVM.BusinessUnitId;
                        TblbusinessUnit.SweetnerForDtc = ModelNumberVM.SweetnerForDtc;
                        TblbusinessUnit.SweetnerForDtd = ModelNumberVM.SweetnerForDtd;
                        TblbusinessUnit.ModifiedBy = userId;
                        TblbusinessUnit.ModifiedDate = _currentDatetime;
                        #region update Sweetner for DtoC & DtoD in tblBusinessUnit added by Kranti Silawat  
                        _businessUnitRepository.UpdateBusinessUnit(TblbusinessUnit.BusinessUnitId, TblbusinessUnit.SweetnerForDtc, TblbusinessUnit.SweetnerForDtd, userId);
                        #endregion

                    }
                }


                _ModelNumberRepository.SaveChanges();

            }

            return TblModelNumber.ModelNumberId;
        }


        /// <summary>
        /// Method to manage (Add/Edit) Model Number/ Model Mapping by kranti 
        /// </summary>
        /// <param name="ModelNumberVM">ModelNumberVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageModelNumber(ModelNumberViewModel ModelNumberVM, int userId)
        {
            TblModelNumber TblModelNumber = null;
            TblModelMapping TblModelMapping = new TblModelMapping();
            TblBusinessUnit TblBusinessUnit = new TblBusinessUnit();
            TblBrand tblBrand = new TblBrand();
            TblBrandSmartBuy tblBrandSmartBuy = new TblBrandSmartBuy();


            if (ModelNumberVM != null)
            {
                TblModelNumber = _mapper.Map<ModelNumberViewModel, TblModelNumber>(ModelNumberVM);

            

                tblBrand = _brandRepository.GetSingle(x => x.Id == TblModelNumber.BrandId && x.Name == ModelNumberVM.BrandName && x.IsActive == true);
                #region Chek Brand is existing If not then create
                if (tblBrand != null)
                {
                    tblBrandSmartBuy = _brandSmartBuyRepository.GetSingle(x => x.BrandId == tblBrand.Id && x.IsActive == true && x.BusinessUnitId == TblModelNumber.BusinessUnitId);
                }
                else
                {
                    TblBrand tblBrand1 = new TblBrand();
                    tblBrand1.IsActive = true;
                    tblBrand1.Name = ModelNumberVM.BrandName;
                    tblBrand1.BusinessUnitId = TblModelNumber?.BusinessUnitId;

                    tblBrand1.CreatedDate = _currentDatetime;
                    tblBrand1.CreatedBy = userId;
                    tblBrand1 = TrimHelper.TrimAllValuesInModel(tblBrand1);
                    _brandRepository.Create(tblBrand1);
                    _brandRepository.SaveChanges();
                    TblModelNumber.BrandId = tblBrand1.Id;

                }
                #endregion


                if (TblModelNumber.ModelNumberId > 0)
                {
                    //Code to update the object

                    TblModelNumber.ModifiedBy = userId;
                    TblModelNumber.ModifiedDate = _currentDatetime;

                    #region Check brand is existing in tblBrandSmartBuy If not then create
                    if (tblBrandSmartBuy != null && tblBrandSmartBuy.BrandId > 0)
                    {
                        TblModelNumber.BrandId = ModelNumberVM.BrandId;
                    }
                    else if (tblBrandSmartBuy != null && tblBrandSmartBuy.BrandId > 0 && TblModelNumber.IsAbb == false)
                    {
                        TblBrandSmartBuy TblBrandSmartBuy = new TblBrandSmartBuy();

                        TblBrandSmartBuy.IsActive = false;
                        TblBrandSmartBuy.BrandId = tblBrandSmartBuy.BrandId;
                        TblBrandSmartBuy.BusinessUnitId = tblBrandSmartBuy.BusinessUnitId;
                        TblBrandSmartBuy.ProductCategoryId = tblBrandSmartBuy.ProductCategoryId;
                        TblBrandSmartBuy.ModifiedDate = _currentDatetime;
                        TblBrandSmartBuy.ModifiedBy = userId;
                        TblBrandSmartBuy = TrimHelper.TrimAllValuesInModel(TblBrandSmartBuy);
                        _brandSmartBuyRepository.Update(TblBrandSmartBuy);
                        _brandSmartBuyRepository.SaveChanges();
                    }
                    else
                    {
                        TblBrandSmartBuy TblBrandSmartBuy = new TblBrandSmartBuy();

                        TblBrandSmartBuy.IsActive = true;
                        TblBrandSmartBuy.BrandId = TblModelNumber.BrandId;
                        TblBrandSmartBuy.BusinessUnitId = TblModelNumber.BusinessUnitId;
                        TblBrandSmartBuy.ProductCategoryId = TblModelNumber.ProductCategoryId;
                        TblBrandSmartBuy.CreatedDate = _currentDatetime;
                        TblBrandSmartBuy.CreatedBy = userId;
                        TblBrandSmartBuy = TrimHelper.TrimAllValuesInModel(TblBrandSmartBuy);
                        _brandSmartBuyRepository.Create(TblBrandSmartBuy);
                        _brandSmartBuyRepository.SaveChanges();
                    }
                    #endregion
                    TblModelNumber = TrimHelper.TrimAllValuesInModel(TblModelNumber);
                    _ModelNumberRepository.Update(TblModelNumber);

                  

                }
                else
                {

                    //Code to Insert the object 

                    TblModelNumber.IsActive = true;
                    TblModelNumber.CreatedDate = _currentDatetime;
                    TblModelNumber.CreatedBy = userId;

                    #region Check brand is existing in tblBrandSmartBuy If not then create
                    if (tblBrandSmartBuy != null && tblBrandSmartBuy.BrandId > 0)
                    {
                        TblModelNumber.BrandId = ModelNumberVM.BrandId;
                    }
                    else if (tblBrandSmartBuy != null && tblBrandSmartBuy.BrandId > 0 && TblModelNumber.IsAbb == false)
                    {
                        TblBrandSmartBuy TblBrandSmartBuy = new TblBrandSmartBuy();
                        TblBrandSmartBuy.IsActive = false;
                        TblBrandSmartBuy.BrandId = tblBrandSmartBuy.BrandId;
                        TblBrandSmartBuy.BusinessUnitId = tblBrandSmartBuy.BusinessUnitId;
                        TblBrandSmartBuy.ProductCategoryId = tblBrandSmartBuy.ProductCategoryId;
                        TblBrandSmartBuy.ModifiedDate = _currentDatetime;
                        TblBrandSmartBuy.ModifiedBy = userId;
                        TblBrandSmartBuy = TrimHelper.TrimAllValuesInModel(TblBrandSmartBuy);
                        _brandSmartBuyRepository.Update(TblBrandSmartBuy);
                        _brandSmartBuyRepository.SaveChanges();
                    }
                    else
                    {
                        TblBrandSmartBuy TblBrandSmartBuy = new TblBrandSmartBuy();
                        TblBrandSmartBuy.IsActive = true;
                        TblBrandSmartBuy.BrandId = TblModelNumber.BrandId;
                        TblBrandSmartBuy.BusinessUnitId = TblModelNumber.BusinessUnitId;
                        TblBrandSmartBuy.ProductCategoryId = TblModelNumber.ProductCategoryId;
                        TblBrandSmartBuy.CreatedDate = _currentDatetime;
                        TblBrandSmartBuy.CreatedBy = userId;
                        TblBrandSmartBuy = TrimHelper.TrimAllValuesInModel(TblBrandSmartBuy);
                        _brandSmartBuyRepository.Create(TblBrandSmartBuy);
                        _brandSmartBuyRepository.SaveChanges();
                    }
                    #endregion
                    TblModelNumber = TrimHelper.TrimAllValuesInModel(TblModelNumber);
                    _ModelNumberRepository.Create(TblModelNumber);
                }

                _ModelNumberRepository.SaveChanges();

               


            }

            return TblModelNumber.ModelNumberId;
        }

        #region Method for Bulk Excel Upload for ModelNumber/ModelMapping by Kranti
        public ModelNumberViewModel ManageModelNumberBulk(ModelNumberViewModel ModelNumberVM, int userId)
        {

            if (ModelNumberVM != null && ModelNumberVM.ModelNumberVMList != null && ModelNumberVM.ModelNumberVMList.Count > 0)
            {
                var ValidatedModelNumberList = ModelNumberVM.ModelNumberVMList.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
                ModelNumberVM.ModelNumberVMErrorList = ModelNumberVM.ModelNumberVMList.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();
                if (ValidatedModelNumberList != null && ValidatedModelNumberList.Count > 0)
                {
                    foreach (var item in ValidatedModelNumberList)
                    {
                        try
                        {
                            TblBrandSmartBuy tblBrandSmartBuy = new TblBrandSmartBuy();
                            TblBusinessUnit BusinessUnit = new TblBusinessUnit();
                            TblProductCategory ProductCategory = new TblProductCategory();
                            TblProductType ProductType = new TblProductType();

                            BusinessUnit = _businessUnitRepository.GetSingle(where: x => x.IsActive == true && x.Name == item.CompanyName);
                            ProductCategory = _productCategoryRepository.GetSingle(where: x => x.IsActive == true && x.Description == item.ProductCategory);
                            ProductType = _productTypeRepository.GetSingle(where: x => x.IsActive == true && x.Description + x.Size == item.ProductType);
                            TblBrand tblBrand = _brandRepository.GetSingle(where: x => x.IsActive == true && x.Name == item.BrandName);


                            #region Check Brand is existing If not then create
                            if (tblBrand != null)
                            {
                                tblBrandSmartBuy = _brandSmartBuyRepository.GetSingle(x => x.BrandId == tblBrand.Id && x.IsActive == true && x.BusinessUnitId == BusinessUnit.BusinessUnitId);
                            }

                            else
                            {
                                TblBrand tblBrand1 = new TblBrand();
                                tblBrand1.IsActive = true;
                                tblBrand1.Name = item?.BrandName;

                                if (BusinessUnit != null)
                                {
                                    tblBrand1.BusinessUnitId = BusinessUnit.BusinessUnitId;
                                }
                                tblBrand1.CreatedDate = _currentDatetime;
                                tblBrand1.CreatedBy = userId;
                                _brandRepository.Create(tblBrand1);
                                _brandRepository.SaveChanges();

                            }
                            #endregion

                            #region Update in Model Number
                            if (item?.ModelNumberId > 0)
                            {

                                TblModelNumber TblModelNumber = new TblModelNumber();
                                //Code to update the object 
                                TblModelNumber.ModelName = item.ModelName;
                                TblModelNumber.Description = item.Description;
                                TblModelNumber.Code = item.Code;
                                TblModelNumber.IsAbb = item.IsAbb;
                                TblModelNumber.IsExchange = item.IsExchange;
                                TblModelNumber.SweetenerBu = item.SweetenerBu;
                                TblModelNumber.SweetenerBp = item.SweetenerBp;
                                TblModelNumber.SweetenerDigi2l = item.SweetenerDigi2l;
                                TblModelNumber.BusinessUnitId = BusinessUnit?.BusinessUnitId;
                                TblModelNumber.ProductCategoryId = ProductCategory.Id;
                                TblModelNumber.ProductTypeId = ProductType?.Id;
                               
                                TblModelNumber.ModifiedDate = _currentDatetime;
                                TblModelNumber.ModifiedBy = userId;
                                TblModelNumber.IsDefaultProduct = item.IsDefaultProduct;

                                #region Check Brand is exist in TblBrandSmartBuy if not then Create
                                if (tblBrandSmartBuy != null && tblBrandSmartBuy.BrandId > 0)
                                {
                                    TblModelNumber.BrandId = tblBrand?.Id;
                                }
                                else if (tblBrandSmartBuy != null && tblBrandSmartBuy.BrandId > 0 /*&& item.IsAbb == false*/)
                                {
                                    TblBrandSmartBuy TblBrandSmartBuy = new TblBrandSmartBuy();

                                    TblBrandSmartBuy.IsActive = false;
                                    TblBrandSmartBuy.BrandId = tblBrandSmartBuy.BrandId;
                                    TblBrandSmartBuy.BusinessUnitId = tblBrandSmartBuy.BusinessUnitId;
                                    TblBrandSmartBuy.ProductCategoryId = tblBrandSmartBuy.ProductCategoryId;
                                    TblBrandSmartBuy.ModifiedDate = _currentDatetime;
                                    TblBrandSmartBuy.ModifiedBy = userId;
                                    _brandSmartBuyRepository.Update(TblBrandSmartBuy);
                                    _brandSmartBuyRepository.SaveChanges();
                                }
                                else
                                {
                                    TblBrandSmartBuy TblBrandSmartBuy = new TblBrandSmartBuy();

                                    TblBrandSmartBuy.IsActive = true;
                                    TblBrandSmartBuy.BrandId = tblBrand?.Id;
                                    TblBrandSmartBuy.BusinessUnitId = BusinessUnit?.BusinessUnitId;
                                    TblBrandSmartBuy.ProductCategoryId = ProductCategory?.Id;
                                    TblBrandSmartBuy.CreatedDate = _currentDatetime;
                                    TblBrandSmartBuy.CreatedBy = userId;
                                    _brandSmartBuyRepository.Create(TblBrandSmartBuy);
                                    _brandSmartBuyRepository.SaveChanges();
                                }
                                #endregion

                                _ModelNumberRepository.Update(TblModelNumber);
                                _ModelNumberRepository.SaveChanges();

                               


                            }
                            #endregion

                            #region Insert in Model Number
                            else
                            {
                                TblModelNumber TblModelNumber = new TblModelNumber();
                                //Code to insert the object 
                                TblModelNumber.ModelName = item.ModelName;
                                TblModelNumber.Description = item.Description;
                                TblModelNumber.Code = item.Code;
                                TblModelNumber.IsAbb = item.IsAbb;
                                TblModelNumber.IsExchange = item.IsExchange;
                                TblModelNumber.SweetenerBu = item.SweetenerBu;
                                TblModelNumber.SweetenerBp = item.SweetenerBp;
                                TblModelNumber.SweetenerDigi2l = item.SweetenerDigi2l;
                                TblModelNumber.BusinessUnitId = BusinessUnit?.BusinessUnitId;
                                TblModelNumber.BrandId = tblBrand?.Id;
                                TblModelNumber.ProductCategoryId = ProductCategory.Id;
                                TblModelNumber.ProductTypeId = ProductType?.Id;
                                
                                TblModelNumber.IsDefaultProduct = item.IsDefaultProduct;
                                TblModelNumber.IsActive = true;
                                TblModelNumber.CreatedDate = _currentDatetime;
                                TblModelNumber.CreatedBy = userId;

                                #region Check Brand is exist in TblBrandSmartBuy if not then Create
                                if (tblBrandSmartBuy != null && tblBrandSmartBuy.BrandId > 0)
                                {
                                    TblModelNumber.BrandId = tblBrand?.Id;
                                }
                                else if (tblBrandSmartBuy != null && tblBrandSmartBuy.BrandId > 0 && TblModelNumber.IsAbb == false)
                                {
                                    TblBrandSmartBuy TblBrandSmartBuy = new TblBrandSmartBuy();
                                    TblBrandSmartBuy.IsActive = false;
                                    TblBrandSmartBuy.BrandId = tblBrandSmartBuy.BrandId;
                                    TblBrandSmartBuy.BusinessUnitId = tblBrandSmartBuy.BusinessUnitId;
                                    TblBrandSmartBuy.ProductCategoryId = tblBrandSmartBuy.ProductCategoryId;
                                    TblBrandSmartBuy.ModifiedDate = _currentDatetime;
                                    TblBrandSmartBuy.ModifiedBy = userId;
                                    _brandSmartBuyRepository.Update(TblBrandSmartBuy);
                                    _brandSmartBuyRepository.SaveChanges();
                                }
                                else
                                {
                                    TblBrandSmartBuy TblBrandSmartBuy = new TblBrandSmartBuy();
                                    TblBrandSmartBuy.IsActive = true;
                                    TblBrandSmartBuy.BrandId = tblBrand?.Id;
                                    TblBrandSmartBuy.BusinessUnitId = BusinessUnit?.BusinessUnitId;
                                    TblBrandSmartBuy.ProductCategoryId = ProductCategory?.Id;
                                    TblBrandSmartBuy.CreatedDate = _currentDatetime;
                                    TblBrandSmartBuy.CreatedBy = userId;
                                    _brandSmartBuyRepository.Create(TblBrandSmartBuy);
                                    _brandSmartBuyRepository.SaveChanges();
                                }
                                #endregion
                                _ModelNumberRepository.Create(TblModelNumber);
                                _ModelNumberRepository.SaveChanges();

                                

                            }

                            #endregion

                        }

                        catch (Exception ex)
                        {
                            item.Remarks += ex.Message + ", ";
                            ModelNumberVM.ModelNumberVMList.Add(item);
                        }
                    }
                }
            }

            return ModelNumberVM;
        }
        #endregion



        /// <summary>
        /// Method to get the ModelNumber by id 
        /// </summary>
        /// <param name="id">ModelNumberId</param>
        /// <returns>ModelNumberViewModel</returns>
        public ModelNumberViewModel GetModelNumberById(int id)
        {
            ModelNumberViewModel ModelNumberVM = null;
            TblModelNumber TblModelNumber = null;

            try
            {
                TblModelNumber = _ModelNumberRepository.GetSingle(where: x => x.ModelNumberId == id);
                if (TblModelNumber != null)
                {
                    ModelNumberVM = _mapper.Map<TblModelNumber, ModelNumberViewModel>(TblModelNumber);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductCategoryManager", "GetProductCategoryById", ex);
            }
            return ModelNumberVM;
        }

        /// <summary>
        /// Method to delete ModelNumber by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletModelNumberVMById(int id)
        {
            bool flag = false;
            try
            {
                TblModelNumber TblModelNumber = _ModelNumberRepository.GetSingle(x => x.IsActive == true && x.ModelNumberId == id);
                if (TblModelNumber != null)
                {
                    TblModelNumber.IsActive = false;
                    _ModelNumberRepository.Update(TblModelNumber);
                    _ModelNumberRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ModelNumberManager", "DeletModelNumberById", ex);
            }
            return flag;
        }

        /// <summary>
        /// Method to get the All ModelNumber
        /// </summary>     
        /// <returns>ModelNumberViewModel</returns>
        public IList<ModelNumberViewModel> GetAllModelNumber()
        {
            IList<ModelNumberViewModel> ModelNumberVMList = null;
            List<TblModelNumber> TblModelNumberlist = new List<TblModelNumber>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblModelNumberlist = _ModelNumberRepository.GetList(x => x.IsActive == true).ToList();

                if (TblModelNumberlist != null && TblModelNumberlist.Count > 0)
                {
                    ModelNumberVMList = _mapper.Map<IList<TblModelNumber>, IList<ModelNumberViewModel>>(TblModelNumberlist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ModelNumberManager", "GetAllModelNumber", ex);
            }
            return ModelNumberVMList;
        }

        /// <summary>
        /// Method to manage (Add/Edit) Model Mapping
        /// </summary>
        /// <param name="ModelNumberVM">ModelMappingVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageModelMapping(ModelMappingViewModel ModelMappingVM, int userId)
        {
            TblModelNumber TblModelNumber = null;
            TblModelMapping TblModelMapping = new TblModelMapping();
            TblBusinessUnit TblBusinessUnit = new TblBusinessUnit();
            TblBrand tblBrand = new TblBrand();
            TblBrandSmartBuy tblBrandSmartBuy = new TblBrandSmartBuy();


            if (ModelMappingVM != null)
            {

                TblModelMapping = _mapper.Map<ModelMappingViewModel, TblModelMapping>(ModelMappingVM);


                if (TblModelMapping.Id > 0)
                {
                    //Code to update the object
                    _ModelMappingRepository.Update(TblModelMapping);
                    _ModelMappingRepository.SaveChanges();

                }
                else
                {

                    //Code to Insert the object 

                    TblModelMapping.IsActive = true;
                    TblModelMapping.CreatedBy = userId;
                    TblModelMapping.CreatedDate = _currentDatetime;
                    _ModelMappingRepository.Create(TblModelMapping);
                    _ModelMappingRepository.SaveChanges();
                }


            }

            return TblModelNumber.ModelNumberId;
        }


        /// <summary>
        /// Method to get the ModelNumber by id 
        /// </summary>
        /// <param name="id">ModelNumberId</param>
        /// <returns>ModelNumberViewModel</returns>
        public ModelMappingViewModel GetModelMappingById(int id)
        {
            ModelMappingViewModel ModelMappingVM = null;
            TblModelMapping TblModelMapping = null;

            try
            {
                TblModelMapping = _ModelMappingRepository.GetSingle(where: x => x.IsActive == true && x.Id == id);
                if (TblModelMapping != null)
                {
                    ModelMappingVM = _mapper.Map<TblModelMapping, ModelMappingViewModel>(TblModelMapping);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ModelMappingManager", "GetModelMappingVMById", ex);
            }
            return ModelMappingVM;
        }
    }
}


