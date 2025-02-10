using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Company;
using AutoMapper;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Common.Helper;
using System.Net.Http;
using RDCELERP.Model.Base;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.PriceMaster;
using RDCELERP.Model.Product;

namespace RDCELERP.BAL.MasterManager
{
    public class BrandManager : IBrandManager
    {
        #region  Variable Declaration
        IBrandRepository _BrandRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IUserRoleRepository _userRoleRepository;
        ILoginRepository _loginRepository;
        IPriceMasterRepository _priceMasterRepository;
        IBusinessUnitRepository _businessUnitRepository;
        IBrandSmartBuyRepository _brandSmartBuyRepository;
        IProdCatBrandMappingRepository _prodCatBrandMappingRepository;
        #endregion

        public BrandManager(IPriceMasterRepository priceMasterRepository, ILoginRepository loginRepository, IBrandRepository BrandRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging, IBusinessUnitRepository businessUnitRepository, IBrandSmartBuyRepository brandSmartBuyRepository, IProdCatBrandMappingRepository prodCatBrandMappingRepository)
        {
            _BrandRepository = BrandRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
            _logging = logging;
            _loginRepository = loginRepository;
            _priceMasterRepository = priceMasterRepository;
            _businessUnitRepository = businessUnitRepository;
            _brandSmartBuyRepository = brandSmartBuyRepository;
            _prodCatBrandMappingRepository = prodCatBrandMappingRepository;
        }

        /// <summary>
        /// Method to manage (Add/Edit) Brand 
        /// </summary>
        /// <param name="BrandVM">BrandVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageBrand(BrandViewModel BrandVM, int userId)
        {
            TblBrand? TblBrand = new TblBrand();
            try
            {
                if (BrandVM != null)
                {
                    TblBrand = _mapper.Map<BrandViewModel, TblBrand>(BrandVM);

                    if (TblBrand != null && TblBrand.Id > 0)
                    {
                        TblBrand.BrandLogoUrl = BrandVM.BrandLogoUrl;
                        TblBrand.ModifiedBy = userId;
                        TblBrand.ModifiedDate = _currentDatetime;
                        _BrandRepository.Update(TblBrand);
                        _BrandRepository.SaveChanges();
                    }
                    else
                    {
                        if (TblBrand != null && !string.IsNullOrEmpty(TblBrand.Name) && TblBrand.BusinessUnitId > 0)
                        {
                            var BusinessUnit = _businessUnitRepository.GetBusinessunitDetails(BrandVM?.BusinessUnitId ?? 0);
                            var existingBrandForBusinessUnit = _BrandRepository.GetBrandByBusinessUnit(BrandVM?.BusinessUnitId);
                            bool IsMultibrand = BusinessUnit?.IsBumultiBrand ?? false;

                            if (!IsMultibrand && existingBrandForBusinessUnit == null || IsMultibrand)
                            {
                                TblBrand.Name = TblBrand.Name?.Trim();
                                TblBrand.BusinessUnitId = BrandVM?.BusinessUnitId;
                                TblBrand.BrandLogoUrl = BrandVM?.BrandLogoUrl?.Trim();
                                TblBrand.CreatedDate = _currentDatetime;
                                TblBrand.CreatedBy = userId;
                                TblBrand.IsActive = true;
                                _BrandRepository.Create(TblBrand);                                
                            }                          
                            _BrandRepository.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BrandManager", "ManageBrand", ex);
            }

            return TblBrand.Id;
        }

        public BrandViewModel ManageBrandBulk(BrandViewModel BrandVM, int userId)
        {

            if (BrandVM != null && BrandVM.BrandVMList != null && BrandVM.BrandVMList.Count > 0)
            {
                var ValidatedBrandList = BrandVM.BrandVMList.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
                BrandVM.BrandVMErrorList = BrandVM.BrandVMList.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();

                if (ValidatedBrandList != null && ValidatedBrandList.Count > 0)
                {
                    TblBrand? TblBrand = new TblBrand();

                    foreach (var item in ValidatedBrandList)
                    {
                        try
                        {
                            var BusinessUnit = _businessUnitRepository.GetBUByName(item!.Company!.Trim());

                            if (item != null && !string.IsNullOrEmpty(item.Name) && BusinessUnit != null && BusinessUnit.BusinessUnitId > 0)
                            {
                                //var BusinessUnit = _businessUnitRepository.GetBusinessunitDetails(BrandVM?.BusinessUnitId ?? 0);
                                var existingBrandForBusinessUnit = _BrandRepository.GetBrandByBusinessUnit(BusinessUnit.BusinessUnitId);
                                bool IsMultibrand = BusinessUnit.IsBumultiBrand ?? false;

                                if (!IsMultibrand && existingBrandForBusinessUnit == null || IsMultibrand)
                                {
                                    TblBrand.Name = item.Name?.Trim();
                                    TblBrand.BusinessUnitId = BusinessUnit.BusinessUnitId;
                                    TblBrand.CreatedDate = _currentDatetime;
                                    TblBrand.CreatedBy = userId;
                                    _BrandRepository.Create(TblBrand);
                                    _BrandRepository.SaveChanges();
                                }
                            }                           
                        }

                        catch (Exception ex)
                        {
                            item.Remarks += ex.Message + ", ";
                            BrandVM.BrandVMList.Add(item);
                        }
                    }
                }
            }
            return BrandVM;
        }

        /// <summary>
        /// Method to get the Brand by id 
        /// </summary>
        /// <param name="id">BrandId</param>
        /// <returns>BrandViewModel</returns>
        public BrandViewModel GetBrandById(int id)
        {
            BrandViewModel BrandVM = null;
            TblBrand TblBrand = null;

            try
            {
                TblBrand = _BrandRepository.GetSingle(where: x => x.Id == id);
                if (TblBrand != null)
                {
                    BrandVM = _mapper.Map<TblBrand, Model.Company.BrandViewModel>(TblBrand);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BrandManager", "GetBrandById", ex);
            }
            return BrandVM;
        }

        /// <summary>
        /// Method to delete Brand by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletBrandById(int id)
        {
            bool flag = false;
            try
            {
                TblBrand TblBrand = _BrandRepository.GetSingle(x => x.IsActive == true && x.Id == id);
                if (TblBrand != null)
                {
                    TblBrand.IsActive = false;
                    _BrandRepository.Update(TblBrand);
                    _BrandRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BrandManager", "DeletBrandById", ex);
            }
            return flag;
        }

        /// <summary>
        /// Method to get the All Brand
        /// </summary>     
        /// <returns>BrandViewModel</returns>
        public IList<BrandViewModel> GetAllBrand()
        {
            IList<BrandViewModel> BrandVMList = null;
            List<TblBrand> TblBrandlist = new List<TblBrand>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblBrandlist = _BrandRepository.GetList(x => x.IsActive == true).ToList();

                if (TblBrandlist != null && TblBrandlist.Count > 0)
                {
                    BrandVMList = _mapper.Map<IList<TblBrand>, IList<BrandViewModel>>(TblBrandlist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BrandManager", "GetAllBrand", ex);
            }
            return BrandVMList;
        }


        public ExecutionResult GetBrand()
        {
            IList<BrandViewModel> BrandVMList = null;
            List<TblBrand> TblBrandlist = new List<TblBrand>();


            // TblUseRole TblUseRole = null;
            try
            {

                TblBrandlist = _BrandRepository.GetList(x => x.IsActive == true).ToList();

                if (TblBrandlist != null && TblBrandlist.Count > 0)
                {
                    BrandVMList = _mapper.Map<IList<TblBrand>, IList<BrandViewModel>>(TblBrandlist);
                    return new ExecutionResult(new InfoMessage(true, "Success", BrandVMList));

                }

                else
                {
                    return new ExecutionResult(new InfoMessage(true, "No data found"));


                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BrandManager", "GetBrand", ex);
            }
            return new ExecutionResult(new InfoMessage(true, "No data found"));
        }

        /// <summary>
        /// Method to get the Brand by id 
        /// </summary>
        /// <param name="id">BrandId</param>
        /// <returns>BrandViewModel</returns>
        public BrandViewModel BrandById(int id)
        {
            Model.Company.BrandViewModel BrandVM = null;
            TblBrand TblBrand = null;

            try
            {
                TblBrand = _BrandRepository.GetSingle(where: x => x.IsActive == true && x.Id == id);
                if (TblBrand != null)
                {
                    BrandVM = _mapper.Map<TblBrand, Model.Company.BrandViewModel>(TblBrand);
                    return BrandVM;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BrandManager", "BrandById", ex);
            }
            return BrandVM;
        }

        #region GetBrands by procatid,protypeid - add for questioners api
        /// <summary>
        /// Method to get the list of Category by BUID
        /// </summary>
        /// <param name="product_catid,product_typeid">buid</param>
        /// <returns>List of ProductCategoryDataContract</returns>
        public ResponseResult GetBrandsByBUId(string username, int catid, int typeid)
        {
            ResponseResult responseResult = new ResponseResult();
            TblPriceMaster tblPriceMaster = null;
            List<BrandViewModels> brandViewModels = new List<BrandViewModels>();

            Login tbllogin = null;
            try
            {
                if (catid > 0 && typeid > 0)
                {
                    tbllogin = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.Username) && x.Username.ToLower().Equals(username.ToLower()));

                    if (tbllogin != null)
                    {
                        tblPriceMaster = _priceMasterRepository.GetSingle(x => x.IsActive == true && x.ExchPriceCode == tbllogin.PriceCode && x.ProductCategoryId == catid && x.ProductTypeId == typeid);

                        if (tblPriceMaster != null)
                        {

                            if (tblPriceMaster.BrandName1 != null)
                            {
                                TblBrand tblBrands1 = _BrandRepository.GetSingle(x => x.IsActive == true && x.Name == tblPriceMaster.BrandName1);
                                BrandViewModels brandViewModels1 = new BrandViewModels();
                                brandViewModels1 = _mapper.Map<TblBrand, BrandViewModels>(tblBrands1);
                                brandViewModels.Add(brandViewModels1);
                            }
                            if (tblPriceMaster.BrandName2 != null)
                            {
                                TblBrand tblBrands2 = _BrandRepository.GetSingle(x => x.IsActive == true && x.Name == tblPriceMaster.BrandName2);
                                BrandViewModels brandViewModels2 = new BrandViewModels();
                                brandViewModels2 = _mapper.Map<TblBrand, BrandViewModels>(tblBrands2);
                                brandViewModels.Add(brandViewModels2);
                            }
                            if (tblPriceMaster.BrandName3 != null)
                            {
                                TblBrand tblBrands3 = _BrandRepository.GetSingle(x => x.IsActive == true && x.Name == tblPriceMaster.BrandName3);
                                BrandViewModels brandViewModels3 = new BrandViewModels();
                                brandViewModels3 = _mapper.Map<TblBrand, BrandViewModels>(tblBrands3);
                                brandViewModels.Add(brandViewModels3);
                            }
                            if (tblPriceMaster.BrandName4 != null)
                            {
                                TblBrand tblBrands4 = _BrandRepository.GetSingle(x => x.IsActive == true && x.Name == tblPriceMaster.BrandName4);
                                BrandViewModels brandViewModels4 = new BrandViewModels();
                                brandViewModels4 = _mapper.Map<TblBrand, BrandViewModels>(tblBrands4);
                                brandViewModels.Add(brandViewModels4);
                            }

                            if (brandViewModels != null && brandViewModels.Count > 0)
                            {
                                responseResult.message = "Success";
                                responseResult.Data = brandViewModels;
                                responseResult.Status = true;
                                responseResult.Status_Code = HttpStatusCode.OK;
                            }
                            else
                            {
                                responseResult.message = "No data found";
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.BadRequest;
                            }
                        }
                        else
                        {
                            responseResult.message = "No data found";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseResult.message = "invlaid user";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.message = "invlaid request parameters";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }

            }
            catch (Exception ex)
            {
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("BrandManager", "GetBrandsByBUId", ex);
            }
            return responseResult;
        }
        #endregion

        #region Get Brand  for ABB  BU based and New category Id based
        /// <summary>
        /// Get Product Type by  AbbPlanMaster on BU ID
        /// </summary>
        /// <param name="BuiD"></param>
        /// <returns></returns>
        public List<BrandViewModel> GetAllBrandForAbb(int NewProductCategoryId, int BuiD)
        {
            List<BrandViewModel> BrandVMList = null;
            List<TblBrand> TblBrandlist = new List<TblBrand>();
            try
            {
                if (BuiD > 0 && NewProductCategoryId > 0)
                {
                    TblBusinessUnit tblBuID = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == Convert.ToInt32(BuiD));
                    if (tblBuID != null && tblBuID.IsBumultiBrand == true)
                    {
                        List<TblBrandSmartBuy> tblBrandSmart = _brandSmartBuyRepository.GetList(x => x.IsActive == true && x.BusinessUnitId == tblBuID.BusinessUnitId && x.ProductCategoryId == NewProductCategoryId).ToList();
                        if (tblBrandSmart != null)
                        {
                            BrandVMList = _mapper.Map<List<TblBrandSmartBuy>, List<BrandViewModel>>(tblBrandSmart);
                        }

                    }
                    else
                    {
                        List<TblBrand> tblBrand = _BrandRepository.GetList(x => x.IsActive == true).ToList();
                        if (tblBrand != null)
                        {
                            BrandVMList = _mapper.Map<List<TblBrand>, List<BrandViewModel>>(tblBrand);
                        }
                    }
                }
                else
                {
                    BrandVMList = new List<BrandViewModel>();
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BrandManager", "GetAllBrandForAbb", ex);
            }
            return BrandVMList;
        }
        #endregion

        #region GetBrands by procatid added by VK
        /// <summary>
        /// GetBrands by procatid added by VK
        /// </summary>
        /// <param name="username"></param>
        /// <param name="catid"></param>
        /// <returns></returns>
        public ResponseResult GetBrandsByCatIdV2(string username, int catid)
        {
            ResponseResult responseResult = new ResponseResult();
            List<TblProdCatBrandMapping>? tblProdCatBrandMappList = null;
            List<BrandViewModels> brandViewModels = new List<BrandViewModels>();

            Login tbllogin = null;
            try
            {
                if (catid > 0)
                {
                    tbllogin = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.Username) && x.Username.ToLower().Equals(username.ToLower()));

                    if (tbllogin != null)
                    {
                        tblProdCatBrandMappList = _prodCatBrandMappingRepository.GetProdBrandListByCatId(catid);

                        if (tblProdCatBrandMappList != null && tblProdCatBrandMappList.Count > 0)
                        {
                            foreach (TblProdCatBrandMapping item in tblProdCatBrandMappList)
                            {
                                TblBrand tblBrands1 = item?.Brand;
                                BrandViewModels brandViewModels1 = new BrandViewModels();
                                brandViewModels1 = _mapper.Map<TblBrand, BrandViewModels>(tblBrands1);
                                brandViewModels.Add(brandViewModels1);
                            }
                            if (brandViewModels != null && brandViewModels.Count > 0)
                            {
                                responseResult.message = "Success";
                                responseResult.Data = brandViewModels;
                                responseResult.Status = true;
                                responseResult.Status_Code = HttpStatusCode.OK;
                            }
                            else
                            {
                                responseResult.message = "No data found";
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.BadRequest;
                            }
                        }
                        else
                        {
                            responseResult.message = "No data found";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseResult.message = "invlaid user";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.message = "invlaid request parameters";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }

            }
            catch (Exception ex)
            {
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("BrandManager", "GetBrandsByProdcatId", ex);
            }
            return responseResult;
        }
        #endregion
    }
}


