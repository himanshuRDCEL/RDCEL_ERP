using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.Master;

namespace RDCELERP.BAL.Interface
{
    public interface IImageLabelMasterManager
    {

        /// <summary>
        /// Method to manage (Add/Edit) ImageLabel
        /// </summary>
        /// <param name="ImageLabelVM">ImageLabelVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageImageLabel(ImageLabelNewViewModel ImageLabelVM, int userId);

        /// <summary>
        /// Method to get the ImageLabel by id 
        /// </summary>
        /// <param name="id">ImageLabelId</param>
        /// <returns>ImageLabelViewModel</returns>
        public ImageLabelNewViewModel GetImageLabelById(int id);

        /// <summary>
        /// Method to delete ImageLabel by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletImageLabelById(int id);

        /// <summary>
        /// Method to get the All ImageLabel
        /// </summary>     
        /// <returns>ImageLabelViewModel</returns>
        public IList<ImageLabelViewModel> GetAllImageLabel();
        public ImageLabelNewViewModel ManageImageLabelBulk(ImageLabelNewViewModel ImageLabelVM, int userId);
    }
}
