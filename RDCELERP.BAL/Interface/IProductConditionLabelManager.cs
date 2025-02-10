using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.ProductConditionLabel;

namespace RDCELERP.BAL.Interface
{
    public interface IProductConditionLabelManager
    {
        #region Method for Add/Edit ProductContionLabel (Added by Kranti)
        /// <summary>
        /// Method to manage (Add/Edit) ProductConditionLabel 
        /// </summary>
        /// <param name="ProductConditionLabelVM">ProductConditionLabelVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageProductConditionLabel(ProductConditionLabelViewModel ProductConditionLabelVM, int userId);

        #endregion

        #region Method for Get ProductConditionLabel by Id (Added by Kranti)
        /// <summary>
        /// Method to get the ProductConditionLabel by id 
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>ProductConditionLabelViewModel</returns>
        public ProductConditionLabelViewModel GetProductConditionLabelById(int id);

        #endregion

        #region Method for Delete ProductCondionLabel by id (Added by Kranti)
        /// <summary>
        /// Method to delete ProductConditionLabel by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletProductConditionLabelById(int id);

        #endregion

        #region Method for Get all ProductConditionLabel list (Added by Kranti)
        /// <summary>
        /// Method to get the All ProductConditionLabel
        /// </summary>     
        /// <returns>ProductConditionLabelViewModel</returns>
        public IList<ProductConditionLabelViewModel> GetAllProductConditionLabel();
        #endregion
    }
}
