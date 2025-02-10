using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.Questioners;
using RDCELERP.Model.QCComment;

namespace RDCELERP.CoreWebApi.Controllers.Questioner
{
    [Authorize]
    [Route("api/diagnostic/[controller]")]
    [ApiController]
    public class QuestionersController : ControllerBase
    {
        #region Variables Declaration
        ILogging _logging;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        private readonly IBrandManager _brandManager;
        private readonly IProductTechnologyManager _productTechnologyManager;
        private readonly IQCCommentManager _qCCommentManager;
        private readonly IExchangeOrderManager _exchangeOrderManager;
        public readonly IOptions<ApplicationSettings> _baseConfig;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IQuestionerLOVRepository _questionerLOVRepository;
        private readonly IQuestionerLovmappingRepository _questionerLovmappingRepository;
        private readonly IQcratingMasterMappingRepository _qcratingMasterMappingRepository;
        private readonly IQCRatingMasterRepository _qcratingMasterRepository;

        #endregion

        #region Constructors
        public QuestionersController(IExchangeOrderManager exchangeOrderManager, IQCCommentManager qCCommentManager, IProductTechnologyManager productTechnologyManager, IBrandManager brandManager, IProductTypeManager productTypeManager, IProductCategoryManager productCategoryManager, ILogging logging, IOptions<ApplicationSettings> baseConfig, IProductCategoryRepository productCategoryRepository, IQuestionerLOVRepository questionerLOVRepository, IQuestionerLovmappingRepository questionerLovmappingRepository, IQcratingMasterMappingRepository qcratingMasterMappingRepository, IQCRatingMasterRepository qcratingMasterRepository)
        {
            _qCCommentManager = qCCommentManager;
            _productTechnologyManager = productTechnologyManager;
            _productCategoryManager = productCategoryManager;
            _logging = logging;
            _productTypeManager = productTypeManager;
            _brandManager = brandManager;
            _exchangeOrderManager = exchangeOrderManager;
            _baseConfig = baseConfig;
            _productCategoryRepository = productCategoryRepository;
            _questionerLOVRepository = questionerLOVRepository;
            _questionerLovmappingRepository = questionerLovmappingRepository;
            _qcratingMasterMappingRepository = qcratingMasterMappingRepository;
            _qcratingMasterRepository = qcratingMasterRepository;
        }
        #endregion

        #region Test QuestionersController Api
        [HttpGet]
        [Route("TestApi")]
        public ResponseResult GetTest()
        {
            ResponseResult responseResult = new ResponseResult();
            try
            {
                responseResult.message = "API Working..";
                responseResult.Status = true;
                responseResult.Status_Code = HttpStatusCode.Accepted;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QuestionersController", "GetTest", ex);
            }
            return responseResult;
        }
        #endregion 

        #region Get Product Categories by Bussiness Unit for Questioners 
        [HttpGet]
        [Route("GetProductCategory")]
        public ResponseResult GetProductCategory()
        {
            ResponseResult responseResult = new ResponseResult();
            string username = string.Empty;
            try
            {
                if (HttpContext != null && HttpContext.User != null
                            && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        //call manager GetCategoryListByBUId
                        responseResult = _productCategoryManager.GetCategoryListByBUId(username);
                    }
                    else
                    {
                        responseResult.message = "Invalid User";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.Unauthorized;
                    responseResult.message = "Token Not Verified";
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QuestionersController", "GetProductCategory", ex);
            }
            return responseResult;
        }
        #endregion

        #region Get Product Type by productcatId  for Questioners 
        [HttpGet]
        [Route("GetProductType")]
        public ResponseResult GetProductType(int catid)
        {
            ResponseResult responseResult = new ResponseResult();
            string username = string.Empty;
            try
            {
                if (HttpContext != null && HttpContext.User != null
                            && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        //call manager GetCategoryListByBUId
                        responseResult = _productTypeManager.GetProductTypeListByBUId(catid, username);
                    }
                    else
                    {
                        responseResult.message = "Invalid User";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.Unauthorized;
                    responseResult.message = "Token Not Verified";
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("QuestionersController", "GetProductType", ex);
            }
            return responseResult;
        }
        #endregion 

        #region  GetBrands by productcatId  for Questioners 
        [HttpGet]
        [Route("GetProductBrands")]
        public ResponseResult GetProductBrands(int catid, int producttypeid)
        {
            ResponseResult responseResult = new ResponseResult();
            string username = string.Empty;
            try
            {
                if (HttpContext != null && HttpContext.User != null
                            && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        responseResult = _brandManager.GetBrandsByBUId(username, catid, producttypeid);
                    }
                    else
                    {
                        responseResult.message = "invalid user";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.Unauthorized;
                    responseResult.message = "Token Not Verified";
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("QuestionersController", "GetProductBrands", ex);
            }
            return responseResult;
        }
        #endregion 

        #region GetProductTechnology productcatId  for Questioners 
        [HttpGet]
        [Route("GetProductTechnology")]
        public ResponseResult GetProductTechnology(int catid)
        {
            ResponseResult responseResult = new ResponseResult();
            string username = string.Empty;
            try
            {
                if (HttpContext != null && HttpContext.User != null
                            && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        responseResult = _productTechnologyManager.ProductTechnologybycatid(catid);
                    }
                    else
                    {
                        responseResult.message = "invalid user";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.Unauthorized;
                    responseResult.message = "Token Not Verified";
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("QuestionersController", "GetProductTechnology", ex);
            }
            return responseResult;
        }
        #endregion 

        #region GetProductASP for Questioners 
        [HttpGet]
        [Route("GetProductASP")]
        public ResponseResult GetProductASP(int producttypeid, int techid, int brandid)
        {
            ResponseResult responseResult = new ResponseResult();
            string username = string.Empty;
            try
            {
                if (HttpContext != null && HttpContext.User != null
                            && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        if (producttypeid > 0 && techid > 0 && brandid > 0)
                        {
                            decimal data = _qCCommentManager.GetASP(producttypeid, techid, brandid);
                            if (data > 0)
                            {
                                AverageSellingPriceDataViewModel aspprice = new AverageSellingPriceDataViewModel();
                                aspprice.AverageSellingPrice = data;

                                #region excellent price
                                double asp = Convert.ToDouble(data);
                                var aspPercentile = _baseConfig.Value.ASPPercentage;
                                var excellentPrice = (asp * Convert.ToDouble(aspPercentile)) / 100;
                                excellentPrice = Math.Round(excellentPrice / 10.0) * 10;
                                aspprice.excellentPrice = excellentPrice;
                                #endregion

                                responseResult.Data = aspprice;
                                responseResult.message = "Success";
                                responseResult.Status = true;
                                responseResult.Status_Code = HttpStatusCode.OK;
                            }
                            else
                            {
                                responseResult.message = "ASP not found for request";
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.BadRequest;
                            }
                        }
                        else
                        {
                            responseResult.message = "Request parameter must be greater than zero";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseResult.message = "invalid user";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.Unauthorized;
                    responseResult.message = "Token Not Verified";
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("QuestionersController", "GetProductTechnology", ex);
            }
            return responseResult;
        }
        #endregion 

        #region GetProductNonWorkingPrice for Questioners 
        [HttpGet]
        [Route("GetProductNonWorkingPrice")]
        public ResponseResult GetProductNonWorkingPrice(int producttypeid, int techid)
        {
            ResponseResult responseResult = new ResponseResult();
            string username = string.Empty;
            try
            {
                if (HttpContext != null && HttpContext.User != null
                            && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        if (producttypeid > 0 && techid > 0)
                        {
                            decimal data = _qCCommentManager.GetNonWorkingPrice(producttypeid, techid);
                            if (data > 0)
                            {
                                NonWorkingPriceDataViewModel nonWorkingPriceDataViewModel = new NonWorkingPriceDataViewModel();
                                nonWorkingPriceDataViewModel.NonWorkingPrice = data;
                                responseResult.Data = nonWorkingPriceDataViewModel;
                                responseResult.message = "Success";
                                responseResult.Status = true;
                                responseResult.Status_Code = HttpStatusCode.OK;
                            }
                            else
                            {
                                responseResult.message = "price not found for request";
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.BadRequest;
                            }
                        }
                        else
                        {
                            responseResult.message = "Request parameter must be greater than zero";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseResult.message = "invalid user";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.Unauthorized;
                    responseResult.message = "Token Not Verified";
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("QuestionersController", "GetProductNonWorkingPrice", ex);
            }
            return responseResult;
        }
        #endregion 

        #region GetListofQuestions productcatId  for Questioners 
        [HttpGet]
        [Route("GetListofQuestions")]
        public ResponseResult GetListofQuestions(int catid)
        {
            ResponseResult responseResult = new ResponseResult();
            string username = string.Empty;
            try
            {
                if (HttpContext != null && HttpContext.User != null
                            && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        List<QCRatingViewModel> qCRatingViewModels = new List<QCRatingViewModel>();
                        responseResult = _qCCommentManager.GetQuestionswithLov(catid);
                        if (responseResult != null && responseResult.message != string.Empty)
                        {
                            return responseResult;
                        }
                        else
                        {
                            responseResult.message = "No data dound for request";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseResult.message = "invalid user";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.Unauthorized;
                    responseResult.message = "Token Not Verified";
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("QuestionersController", "GetListofQuestions", ex);
            }
            return responseResult;
        }
        #endregion

        #region GetQuatedPrice productcatId  for Questioners 
        [HttpGet]
        [Route("GetQuatedPrice")]
        public ResponseResult GetQuatedPrice(List<QCRatingViewModel> qCRatingViewModels)
        {
            ResponseResult responseResult = new ResponseResult();
            string username = string.Empty;
            try
            {
                if (HttpContext != null && HttpContext.User != null
                            && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        // = new List<QCRatingViewModel>();
                        List<double> QuatedPrice = null;
                        QuatedPrice = _qCCommentManager.GetQuotedPrice(qCRatingViewModels);
                        if (QuatedPrice.Count > 0)
                        {
                            ProductPriceListViewModel productPriceListViewModel = new ProductPriceListViewModel();
                            productPriceListViewModel.ExcellentPrice = QuatedPrice[0];
                            productPriceListViewModel.QuotedPrice = QuatedPrice[1];
                            productPriceListViewModel.SweetnerPrice = QuatedPrice[2];
                            productPriceListViewModel.QuotedWithSweetnerPrice = QuatedPrice[3];
                            productPriceListViewModel.FinalPrice = QuatedPrice[4];

                            responseResult.Data = productPriceListViewModel;
                            responseResult.message = "Success";
                            responseResult.Status = true;
                            responseResult.Status_Code = HttpStatusCode.OK;
                            return responseResult;
                        }
                        else
                        {
                            responseResult.message = "No data dound for request";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseResult.message = "invalid user";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.Unauthorized;
                    responseResult.message = "Token Not Verified";
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("QuestionersController", "GetListofQuestions", ex);
            }
            return responseResult;
        }
        #endregion

        #region AddProducts Order for Exchange - Questioners 
        [HttpPost]
        [Route("AddProducts")]
        public ResponseResult AddExchangeProducts(MultipleExchangeOrdersDataModel multipleExchangeOrdersDataModel)
        {
            ResponseResult responseResult = new ResponseResult();
            string username = string.Empty;
            try
            {
                if (HttpContext != null && HttpContext.User != null
                            && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        responseResult = _exchangeOrderManager.AddMultipleOrders(multipleExchangeOrdersDataModel, username);
                    }
                    else
                    {
                        responseResult.message = "invalid user";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.Unauthorized;
                    responseResult.message = "Token Not Verified";
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("QuestionersController", "AddExchangeProducts", ex);
            }
            return responseResult;
        }
        #endregion

      
      
        #region Diagnosis Version 2.0 Api's  -- YASH RATHORE 

        #region Get Product Categories by Bussiness Unit for Questioners // COMPLETED
        [HttpGet]
        [Route("GetProductCategoryV2")]
        public ResponseResult GetProductCategoryV2()
        {
            ResponseResult responseResult = new ResponseResult();
            string username = string.Empty;
            try
            {
                if (HttpContext != null && HttpContext.User != null
                            && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        
                        responseResult = _productCategoryManager.GetProdCatListForDiagnose();
                    }
                    else
                    {
                        responseResult.message = "Invalid User";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.Unauthorized;
                    responseResult.message = "Token Not Verified";
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("QuestionersController", "GetProductCategory", ex);
            }
            return responseResult;
        }
        #endregion  

        #region Get Product Type by productcatId  for Questioners // COMPLETED
        [HttpGet]
        [Route("GetProductTypeV2")]
        public ResponseResult GetProductTypeV2(int catid)
        {
            ResponseResult responseResult = new ResponseResult();
            string username = string.Empty;
            try
            {
                if (HttpContext != null && HttpContext.User != null
                            && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        //call manager GetCategoryListByBUId
                        responseResult = _productTypeManager.GetProdTypeListByCatIdv2(catid, username);
                    }
                    else
                    {
                        responseResult.message = "Invalid User";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.Unauthorized;
                    responseResult.message = "Token Not Verified";
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("QuestionersController", "GetProductTypev2", ex);
            }
            return responseResult;
        }
        #endregion 

        #region GetBrands by productcatId  for Questioners // COMPLETED
        [HttpGet]
        [Route("GetProductBrandsV2")]
        public ResponseResult GetProductBrandsV2(int catid)
        {
            ResponseResult responseResult = new ResponseResult();
            string username = string.Empty;
            try
            {
                if (HttpContext != null && HttpContext.User != null
                            && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        responseResult = _brandManager.GetBrandsByCatIdV2(username, catid);
                    }
                    else
                    {
                        responseResult.message = "invalid user";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.Unauthorized;
                    responseResult.message = "Token Not Verified";
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("QuestionersController", "GetProductBrands", ex);
            }
            return responseResult;
        }
        #endregion

        #region GetAgeQuestionLov productcatId  for Questioners // COMPLETED
        [HttpGet]
        [Route("GetAgeQuestion")]
        public ResponseResult GetAgeQuestion(int catid)
        {
            ResponseResult responseResult = new ResponseResult();
            string username = string.Empty;
            try
            {
                if (HttpContext != null && HttpContext.User != null
                       && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        responseResult = _qCCommentManager.GetAgeQuestionwithLov(catid);
                        if (responseResult != null && responseResult.message != string.Empty)
                        {
                            return responseResult;
                        }
                        else
                        {
                            responseResult.message = "No data dound for request";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseResult.message = "invalid user";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.Unauthorized;
                    responseResult.message = "Token Not Verified";
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("QuestionersController", "GetListofQuestions", ex);
            }
            return responseResult;
        }
        #endregion

        #region Get Product upto ASP price for Questioners // COMPLETED
        [HttpGet]
        [Route("GetProdUptoPriceV2")]
        public ResponseResult GetProdUptoPriceV2(int producttypeid, int techid, int brandid, int QuestionerLovmappingId)
        {
            ResponseResult responseResult = new ResponseResult();
            string username = string.Empty;
            ProdCatBrandMapViewModel? prodCatBrandMapViewModel = null;
            TblQuestionerLovmapping? tblQuestionerLovmapping = null;
            TblQcratingMaster? tblQcratingMaster = null;
            try
            {
                if (HttpContext != null && HttpContext.User != null
                            && HttpContext?.User?.Identity?.Name != null)
                {
                    username = HttpContext.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        if (producttypeid > 0 && techid > 0 && brandid > 0 && QuestionerLovmappingId > 0)
                        {
                            prodCatBrandMapViewModel = _qCCommentManager.GetASPV2(producttypeid, techid, brandid);
                            if (prodCatBrandMapViewModel != null && prodCatBrandMapViewModel.FinalASP > 0)
                            {
                                AverageSellingPriceDataViewModel aspprice = new AverageSellingPriceDataViewModel();
                                aspprice.AverageSellingPrice = prodCatBrandMapViewModel.FinalASP;

                                #region excellent price and ASP 

                                aspprice.excellentPrice = Convert.ToDouble(prodCatBrandMapViewModel.ExcellentPrice);
                                aspprice.AverageSellingPrice = prodCatBrandMapViewModel.FinalASP;
                                
                                #endregion

                                #region Get QuestionerLOVMaping Rating Weightage --YR 
                                tblQcratingMaster = _qcratingMasterRepository.GetAgeQueweightage(prodCatBrandMapViewModel.ProductCatId);
                                double price = 0;
                                if (tblQcratingMaster != null)
                                {
                                    price =  (aspprice.excellentPrice * Convert.ToDouble(tblQcratingMaster?.RatingWeightage ?? 0) / 100);
                                }
                                tblQuestionerLovmapping = _questionerLovmappingRepository.GetRatingWeightageV2(QuestionerLovmappingId);

                                if (tblQuestionerLovmapping != null)
                                {
                                    Double excellentPrice = aspprice.excellentPrice - price;
                                    excellentPrice =  excellentPrice + (price * Convert.ToDouble(tblQuestionerLovmapping?.RatingWeightageLov ?? 0) / 10);
                                    aspprice.excellentPrice = Math.Round(Convert.ToInt32(excellentPrice) / 10.0) * 10;
                                }
                             
                                #endregion

                                responseResult.Data = aspprice;
                                responseResult.message = "Success";
                                responseResult.Status = true;
                                responseResult.Status_Code = HttpStatusCode.OK;
                            }
                            else
                            {
                                responseResult.message = "ASP not found for request";
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.BadRequest;
                            }
                        }
                        else
                        {
                            responseResult.message = "Request parameter must be greater than zero";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseResult.message = "invalid user";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.Unauthorized;
                    responseResult.message = "Token Not Verified";
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("QuestionersController", "GetProdUptoPriceV2", ex);
            }
            return responseResult;
        }
        #endregion

        #region GetListofNewQuestions productcatId  for Questioners// COMPLETED
        [HttpGet]
        [Route("GetListofNewQuestions")]
        public ResponseResult GetListofNewQuestions(int catid, int typeid, int techid) 
        {
            ResponseResult responseResult = new ResponseResult();
            string username = string.Empty;
            try
            {
                if (HttpContext != null && HttpContext.User != null
                            && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                      
                        responseResult = _qCCommentManager.GetNewQuestionswithLov(catid, typeid, techid);
                        if (responseResult != null && responseResult.message != string.Empty)
                        {
                            return responseResult;
                        }
                        else
                        {
                            responseResult.message = "No data dound for request";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseResult.message = "invalid user";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.Unauthorized;
                    responseResult.message = "Token Not Verified";
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("QuestionersController", "GetListofQuestions", ex);
            }
            return responseResult;
        }
        #endregion

        #region GetNewQuatedPrice productcatId  for Questioners// COMPLETED 
        [HttpPost]
        [Route("GetNewQuatedPricev2")]
        public ResponseResult GetNewQuatedPricev2(List<QCRatingViewModel> qCRatingViewModels)
        {
            ResponseResult responseResult = new ResponseResult();
            string username = string.Empty;
            try
            {
                if (HttpContext != null && HttpContext.User != null
                            && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        
                        List<double> QuatedPrice = null;
                        QuatedPrice = _qCCommentManager.GetNewQuotedPrice(qCRatingViewModels);
                        if (QuatedPrice.Count > 0)
                        {
                            ProductPriceListViewModel productPriceListViewModel = new ProductPriceListViewModel();
                            productPriceListViewModel.ExcellentPrice = QuatedPrice[0];
                            productPriceListViewModel.QuotedPrice = QuatedPrice[1];
                            productPriceListViewModel.SweetnerPrice = QuatedPrice[2];
                            productPriceListViewModel.QuotedWithSweetnerPrice = QuatedPrice[3];
                            productPriceListViewModel.FinalPrice = QuatedPrice[4];

                            responseResult.Data = productPriceListViewModel;
                            responseResult.message = "Success";
                            responseResult.Status = true;
                            responseResult.Status_Code = HttpStatusCode.OK;
                            return responseResult;
                        }
                        else
                        {
                            responseResult.message = "No data dound for request";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseResult.message = "invalid user";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.Unauthorized;
                    responseResult.message = "Token Not Verified";
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("QuestionersController", "GetListofQuestions", ex);
            }
            return responseResult;
        }
        #endregion

        #region AddProducts Order for Exchange - Questioners// COMPLETED
        [HttpPost]
        [Route("AddNewProductsv2")]
        public ResponseResult AddNewExchangeProductsv2(MultipleExchangeOrdersDataModel multipleExchangeOrdersDataModel)
        {
            ResponseResult responseResult = new ResponseResult();
            string username = string.Empty;
            try
            {
                if (HttpContext != null && HttpContext.User != null
                            && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        responseResult = _exchangeOrderManager.AddMultipleOrdersV2(multipleExchangeOrdersDataModel, username);
                    }
                    else
                    {
                        responseResult.message = "invalid user";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.Unauthorized;
                    responseResult.message = "Token Not Verified";
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("QuestionersController", "AddNewProductsv2", ex);
            }
            return responseResult;
        }
        #endregion
        #region GetProductTechnology productcatId  for Questioners v2
        [HttpGet]
        [Route("GetProductTechnologyv2")]
        public ResponseResult GetProductTechnologyv2(int catid)
        {
            ResponseResult responseResult = new ResponseResult();
            string username = string.Empty;
            try
            {
                if (HttpContext != null && HttpContext.User != null
                            && HttpContext.User.Identity.Name != null)
                {
                    username = HttpContext.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        responseResult = _productTechnologyManager.ProductTechnologybycatidv2(catid);
                    }
                    else
                    {
                        responseResult.message = "invalid user";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.Unauthorized;
                    responseResult.message = "Token Not Verified";
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
                _logging.WriteErrorToDB("QuestionersController", "GetProductTechnology", ex);
            }
            return responseResult;
        }
        #endregion
        #endregion

    }
}
