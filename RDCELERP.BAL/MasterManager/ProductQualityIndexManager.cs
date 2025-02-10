using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.ProductQuality;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.InfoMessage;

namespace RDCELERP.BAL.MasterManager
{
    public class ProductQualityIndexManager : IProductQualityIndexManager
    {
        #region  Variable Declaration
        IProductQualityIndexRepository _ProductQualityIndexRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IUserRoleRepository _userRoleRepository;
        IProductCategoryRepository _productCategoryRepository;

        #endregion

        public ProductQualityIndexManager(IProductQualityIndexRepository ProductQualityIndexRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging, IProductCategoryRepository productCategoryRepository)
        {
            _ProductQualityIndexRepository = ProductQualityIndexRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
            _logging = logging;
            _productCategoryRepository = productCategoryRepository;
        }

        /// <summary>
        /// Method to manage (Add/Edit) ProductQualityIndex
        /// </summary>
        /// <param name="ProductQualityIndexVM">ProductQualityIndexVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageProductQualityIndex(ProductQualityIndexViewModel ProductQualityIndexVM, int userId)
        {
            TblProductQualityIndex TblProductQualityIndex = new TblProductQualityIndex();

            try
            {
                if (ProductQualityIndexVM != null)
                {
                    TblProductQualityIndex = _mapper.Map<ProductQualityIndexViewModel, TblProductQualityIndex>(ProductQualityIndexVM);

                    TblProductQualityIndex = TrimHelper.TrimAllValuesInModel(TblProductQualityIndex);

                    var GetCategoryFromProductCategory = _productCategoryRepository.GetCategoryById(TblProductQualityIndex.ProductCategoryId);

                    var GetCategoryQualityIndex = _ProductQualityIndexRepository.GetProductQualityIndexByProductCategoryId(GetCategoryFromProductCategory.Id);

                    if (GetCategoryFromProductCategory != null  && GetCategoryFromProductCategory.Id > 0 
                        && GetCategoryQualityIndex != null && GetCategoryQualityIndex.ProductQualityIndexId > 0 || 
                        TblProductQualityIndex.ProductQualityIndexId > 0)
                    {
                        //Code to update the object
                        if (TblProductQualityIndex.ProductQualityIndexId <= 0)
                        {
                            TblProductQualityIndex.ProductQualityIndexId = GetCategoryQualityIndex.ProductQualityIndexId;
                        }
                        TblProductQualityIndex.ModifiedBy = userId;
                        TblProductQualityIndex.ModifiedDate = _currentDatetime;
                        _ProductQualityIndexRepository.UpdateRecord(TblProductQualityIndex, GetCategoryFromProductCategory);
                    }
                    else
                    {
                        var Check = _ProductQualityIndexRepository.GetSingle(x => x.Name == ProductQualityIndexVM.Name);
                        if (Check == null)
                        {
                            //Code to Insert the object 
                            TblProductQualityIndex.IsActive = true;
                            TblProductQualityIndex.CategoryName = ProductQualityIndexVM?.ProductCategoryName;
                            TblProductQualityIndex.CreatedDate = _currentDatetime;
                            TblProductQualityIndex.CreatedBy = userId;
                            TblProductQualityIndex = TrimHelper.TrimAllValuesInModel(TblProductQualityIndex);

                            _ProductQualityIndexRepository.Create(TblProductQualityIndex);
                            _ProductQualityIndexRepository.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductQualityIndexManager", "ManageProductQualityIndex", ex);
            }

            return TblProductQualityIndex.ProductQualityIndexId;
        }

        /// <summary>
        /// Method to get the ProductQualityIndex by id 
        /// </summary>
        /// <param name="id">ProductQualityIndexId</param>
        /// <returns>ProductQualityIndexViewModel</returns>
        public Model.ProductQuality.ProductQualityIndexViewModel GetProductQualityIndexById(int id)
        {
            Model.ProductQuality.ProductQualityIndexViewModel ProductQualityIndexVM = null;
            TblProductQualityIndex TblProductQualityIndex = null;

            try
            {
                TblProductQualityIndex = _ProductQualityIndexRepository.GetSingle(x=> x.ProductQualityIndexId == id);
                if (TblProductQualityIndex != null)
                {
                    ProductQualityIndexVM = _mapper.Map<TblProductQualityIndex, Model.ProductQuality.ProductQualityIndexViewModel>(TblProductQualityIndex);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductQualityIndexManager", "GetProductQualityIndexById", ex);
            }
            return ProductQualityIndexVM;
        }

        /// <summary>
        /// Method to delete ProductQualityIndex by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletProductQualityIndexById(int id)
        {
            bool flag = false;
            try
            {
                TblProductQualityIndex TblProductQualityIndex = _ProductQualityIndexRepository.GetSingle(x => x.IsActive == true && x.ProductQualityIndexId == id);
                if (TblProductQualityIndex != null)
                {
                    TblProductQualityIndex.IsActive = false;
                    _ProductQualityIndexRepository.Update(TblProductQualityIndex);
                    _ProductQualityIndexRepository.SaveChanges();
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
        /// Method to get the All ProductQualityIndex
        /// </summary>     
        /// <returns>ProductQualityIndexViewModel</returns>
        public IList<ProductQualityIndexViewModel> GetAllProductQualityIndex()
        {
            IList<ProductQualityIndexViewModel> ProductQualityIndexVMList = null;
            List<TblProductQualityIndex> TblProductQualityIndexlist = new List<TblProductQualityIndex>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblProductQualityIndexlist = _ProductQualityIndexRepository.GetList(x => x.IsActive == true).ToList();

                if (TblProductQualityIndexlist != null && TblProductQualityIndexlist.Count > 0)
                {
                    ProductQualityIndexVMList = _mapper.Map<IList<TblProductQualityIndex>, IList<ProductQualityIndexViewModel>>(TblProductQualityIndexlist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductQualityIndexManager", "GetAllProductQualityIndex", ex);
            }
            return ProductQualityIndexVMList;
        }

        public ExecutionResult GetProductQualityIndex()
        {
            IList<ProductQualityIndexViewModel> ProductQualityIndexVMList = null;
            List<TblProductQualityIndex> TblProductQualityIndexlist = new List<TblProductQualityIndex>();


            // TblUseRole TblUseRole = null;
            try
            {

                TblProductQualityIndexlist = _ProductQualityIndexRepository.GetList(x => x.IsActive == true).ToList();

                if (TblProductQualityIndexlist != null && TblProductQualityIndexlist.Count > 0)
                {
                    ProductQualityIndexVMList = _mapper.Map<IList<TblProductQualityIndex>, IList<ProductQualityIndexViewModel>>(TblProductQualityIndexlist);
                    return new ExecutionResult(new InfoMessage(true, "Success", ProductQualityIndexVMList));
                }

                else
                {
                    return new ExecutionResult(new InfoMessage(true, "No data found"));
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductQualityIndexManager", "GetProductQualityIndex", ex);
            }
            return new ExecutionResult(new InfoMessage(true, "No data found"));
        }

        
        public ExecutionResult ProductQualityIndexById(int id)
        {
            ProductQualityIndexViewModel ProductQualityIndexVM = null;
            TblProductQualityIndex TblProductQualityIndex = null;

            try
            {
                TblProductQualityIndex = _ProductQualityIndexRepository.GetSingle(where: x => x.IsActive == true && x.ProductQualityIndexId == id);
                if (_ProductQualityIndexRepository != null)
                {
                    ProductQualityIndexVM = _mapper.Map<TblProductQualityIndex, ProductQualityIndexViewModel>(TblProductQualityIndex);
                    return new ExecutionResult(new InfoMessage(true, "Success", ProductQualityIndexVM));
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductQualityIndexManager", "ProductQualityIndexById", ex);
            }
            return new ExecutionResult(new InfoMessage(true, "No data found"));
        }
    }
}
