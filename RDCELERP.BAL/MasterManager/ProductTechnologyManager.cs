using AutoMapper;
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
using RDCELERP.DAL.Repository;
using RDCELERP.Model.City;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.Questioners;
using RDCELERP.Model.State;

namespace RDCELERP.BAL.MasterManager
{
    public class ProductTechnologyManager : IProductTechnologyManager
        
    {

        #region  Variable Declaration
        
        private readonly IMapper _mapper;
        private readonly IProductTechnologyRepository _productTechnologyRepository;
        IPriceMasterQuestionersRepository _priceMasterQuestionersRepository;
        private Digi2l_DevContext _context;
        ILogging _logging;
        
        #endregion
        public ProductTechnologyManager(IProductTechnologyRepository productTechnologyRepository, Digi2l_DevContext context, IMapper mapper, ILogging logging, IPriceMasterQuestionersRepository priceMasterQuestionersRepository)
        {
            _productTechnologyRepository = productTechnologyRepository;
            _priceMasterQuestionersRepository = priceMasterQuestionersRepository;
            _mapper = mapper;
            _context = context;
            _logging = logging;
        }

        #region add for questioners api
        public ResponseResult ProductTechnologybycatid(int catid)
        {
            ResponseResult responseResult = new ResponseResult();
            List<TblProductTechnology> tblProductTechnologies = null;
            ProductTechnologyDataViewModel productTechnologyDataViewModel = new ProductTechnologyDataViewModel();
            List<ProductTechnologyDataViewModel> productTechnologyDataViewModels = new List<ProductTechnologyDataViewModel>();
            try
            {
                if (catid > 0)
                {
                    tblProductTechnologies = _productTechnologyRepository.GetList(x => x.IsActive == true && x.ProductCatId == catid && x.Isusedold==true).ToList();
                    if (tblProductTechnologies!=null && tblProductTechnologies.Count>0)
                    {
                        productTechnologyDataViewModels = _mapper.Map<List<TblProductTechnology>, List<ProductTechnologyDataViewModel>>(tblProductTechnologies).ToList();
                        if(productTechnologyDataViewModels != null && productTechnologyDataViewModels.Count > 0)
                        {
                            responseResult.Data = productTechnologyDataViewModels;
                            responseResult.message = "Success";
                            responseResult.Status = true;
                            responseResult.Status_Code = HttpStatusCode.OK;
                        }
                        else
                        {
                            responseResult.message = "error occurs while mapping the data";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseResult.message = "Not data found";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
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
                _logging.WriteErrorToDB("ProductTechnologyManager", "ProductTechnologybycatid", ex);
            }
            return responseResult;


        }
        #endregion
     
        #region add for questioners api v2
        public ResponseResult ProductTechnologybycatidv2(int catid)

        {
            ResponseResult responseResult = new ResponseResult();
            List<TblProductTechnology> tblProductTechnologies = null;
            ProductTechnologyDataViewModel productTechnologyDataViewModel = new ProductTechnologyDataViewModel();
            List<ProductTechnologyDataViewModel> productTechnologyDataViewModels = new List<ProductTechnologyDataViewModel>();
            List<TblPriceMasterQuestioner> tblPriceMasterQuestioners = null;
            try
            {
                if (catid > 0)
                {
                    tblPriceMasterQuestioners = _priceMasterQuestionersRepository.GetProdTechnologyListByCatId(catid);
                    if (tblPriceMasterQuestioners != null && tblPriceMasterQuestioners.Count > 0)
                    {
                        foreach (var item in tblPriceMasterQuestioners)
                        {

                            tblProductTechnologies = _productTechnologyRepository.GetList(x => x.IsActive == true && x.ProductCatId == item.ProductCatId && x.IsusedNew == true).ToList();

                        }
                        if (tblProductTechnologies != null && tblProductTechnologies.Count > 0)
                        {
                            productTechnologyDataViewModels = _mapper.Map<List<TblProductTechnology>, List<ProductTechnologyDataViewModel>>(tblProductTechnologies).ToList();
                        }
                        else { responseResult.message = "error occurs while mapping the data"; }

                        if (productTechnologyDataViewModels != null && productTechnologyDataViewModels.Count > 0)
                        {
                            responseResult.Data = productTechnologyDataViewModels;
                            responseResult.message = "Success";
                            responseResult.Status = true;
                            responseResult.Status_Code = HttpStatusCode.OK;
                        }
                        else
                        {
                            responseResult.message = "error occurs while mapping the data";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        responseResult.message = "Not data found";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
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
                _logging.WriteErrorToDB("ProductTechnologyManager", "ProductTechnologybycatid", ex);
            }
            return responseResult;

        }
        #endregion
    }
}
