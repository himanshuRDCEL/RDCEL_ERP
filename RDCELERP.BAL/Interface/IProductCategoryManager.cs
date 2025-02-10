using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.Master;
using RDCELERP.Model.MobileApplicationModel;

namespace RDCELERP.BAL.Interface
{
    public interface IProductCategoryManager
    {
        /// <summary>
        /// Method to manage (Add/Edit) ProductCategory 
        /// </summary>
        /// <param name="ProductCategoryVM">ProductCategoryVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageProductCategory(ProductCategoryViewModel ProductCategoryVM, int userId);

        /// <summary>
        /// Method to get the ProductCategory by id 
        /// </summary>
        /// <param name="id">ProductCategoryId</param>
        /// <returns>ProductCategoryViewModel</returns>
        public Model.Master.ProductCategoryViewModel GetProductCategoryById(int id);

        /// <summary>
        /// Method to delete ProductCategory by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletProductCategoryById(int id);

        /// <summary>
        /// Method to get the All ProductCategory
        /// </summary>     
        /// <returns>ProductCategoryViewModel</returns>
        public IList<ProductCategoryViewModel> GetAllProductCategory();

        public ExecutionResult GetProductCategory();

        /// <summary>
        /// Method to get the Brand by id 
        /// </summary>
        /// <param name="id">BrandId</param>
        /// <returns>BrandViewModel</returns>
        public ExecutionResult ProductCategoryById(int id);
        public ResponseResult GetCategoryListByBUId(string username);
        public IList<ProductCategoryViewModel> GetProductCategoryDescById(int id);
        public ProductCategoryViewModel ManageProductCategoryBulk(ProductCategoryViewModel ProductCategoryVM, int userId);

        public List<BuProductCatDataModel> GetAllProductCategoryByAbbPlanMaster(int Buid);
        
        #region Get Product Category for Diagnose V2
        public ResponseResult GetProdCatListForDiagnose();
        #endregion
    }
}
