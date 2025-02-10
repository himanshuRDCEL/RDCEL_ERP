using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.ProductQuality;

namespace RDCELERP.BAL.Interface
{
    public interface IProductQualityIndexManager
    {
        /// <summary>
        /// Method to manage (Add/Edit) ProductQualityIndex
        /// </summary>
        /// <param name="ProductQualityIndexVM">ProductQualityIndexVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageProductQualityIndex(ProductQualityIndexViewModel ProductQualityIndexVM, int userId);

        /// <summary>
        /// Method to get the ProductQualityIndex by id 
        /// </summary>
        /// <param name="id">ProductQualityIndexId</param>
        /// <returns>ProductQualityIndexViewModel</returns>
        public Model.ProductQuality.ProductQualityIndexViewModel GetProductQualityIndexById(int id);

        /// <summary>
        /// Method to delete ProductQualityIndex by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletProductQualityIndexById(int id);

        /// <summary>
        /// Method to get the All ProductQualityIndex
        /// </summary>     
        /// <returns>ProductQualityIndexViewModel</returns>
        public IList<ProductQualityIndexViewModel> GetAllProductQualityIndex();

        public ExecutionResult GetProductQualityIndex();


        public ExecutionResult ProductQualityIndexById(int id);

    }
}
