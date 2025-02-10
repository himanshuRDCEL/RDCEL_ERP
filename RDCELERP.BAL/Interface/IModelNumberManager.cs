using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.ModelNumber;

namespace RDCELERP.BAL.Interface
{
    public interface IModelNumberManager
    {
        /// <summary>
        /// Method to manage (Add/Edit) Model Number 
        /// </summary>
        /// <param name="ModelNumberVM">ModelNumberVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageModelNumber(ModelNumberViewModel ModelNumberVM, int userId);

        /// <summary>
        /// Method to get the ModelNumber by id 
        /// </summary>
        /// <param name="id">ModelNumberId</param>
        /// <returns>ModelNumberViewModel</returns>
        public ModelNumberViewModel GetModelNumberById(int id);

        /// <summary>
        /// Method to delete ModelNumber by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletModelNumberVMById(int id);

        /// <summary>
        /// Method to get the All ModelNumber
        /// </summary>     
        /// <returns>ModelNumberViewModel</returns>
        public IList<ModelNumberViewModel> GetAllModelNumber();
        public ModelNumberViewModel ManageModelNumberBulk(ModelNumberViewModel ModelNumberVM, int userId);
        public int ManageSweetner(ModelNumberViewModel ModelNumberVM, int userId);


        /// <summary>
        /// Method to manage (Add/Edit) Model Mapping
        /// </summary>
        /// <param name="ModelNumberVM">ModelMappingVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageModelMapping(ModelMappingViewModel ModelMappingVM, int userId);


        /// <summary>
        /// Method to get the ModelNumber by id 
        /// </summary>
        /// <param name="id">ModelNumberId</param>
        /// <returns>ModelNumberViewModel</returns>
        public ModelMappingViewModel GetModelMappingById(int id);


    }
}
