using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.Product;

namespace RDCELERP.BAL.Interface
{
    public interface IProductTypeManager
    {
        /// <summary>
        /// Method to manage (Add/Edit) Product Type
        /// </summary>
        /// <param name="ProductTypeVM">ProductTypeVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageProductType(ProductTypeViewModel ProductTypeVM, int userId);
        /// <summary>
        /// Method to get the ProductType by id 
        /// </summary>
        /// <param name="id">ProductTypeId</param>
        /// <returns>ProductTypeViewModel</returns>
        public Model.Product.ProductTypeViewModel GetProductTypeById(int id);

        /// <summary>
        /// Method to get ProductType by userId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public ProductTypeViewModel GetProductTypeListByUserId(int? userId);

        /// <summary>
        /// Method to delete ProductType by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletProductTypeById(int id);

        /// <summary>
        /// Method to get the All ProductType
        /// </summary>     
        /// <returns>ProductTypeViewModel</returns>
        public IList<ProductTypeViewModel> GetAllProductType();

        public ExecutionResult GetProductType();

        /// <summary>
        /// Method to get the Brand by id 
        /// </summary>
        /// <param name="id">BrandId</param>
        /// <returns>BrandViewModel</returns>
        public ExecutionResult ProductTypeById(int id);
        public IList<ProductTypeViewModel> GetProductTypeByCategoryId(int productCategoryId);
        public ResponseResult GetProductTypeListByBUId(int catid, string UserName);
        public IList<ProductTypeViewModel> GetProductTypeBYCategory(int? Id);
        public IList<ProductTypeViewModel> GetProductTypeBYCategoryDesc(string Description);
        public IList<ProductTypeViewModel> GetProductTypeDescById(int id);
        public IList<ProductTypeViewModel> GetProductTypeBYCategoryDescription(string desc);
        public ProductTypeViewModel ManageProductTypeBulk(ProductTypeViewModel ProductTypeVM, int userId);

        public IList<ProductTypeViewModel> GetAllProductTypeByAbbPlanMaster(int NewProductCategoryId, int BuId);

        #region Get Product Type List for Diagnose V2
        /// <summary>
        /// Get Product Type List for Diagnose V2
        /// </summary>
        /// <param name="catid"></param>
        /// <returns></returns>
        public ResponseResult GetProdTypeListByCatIdv2(int catid, string? UserName);
        #endregion
    }
}