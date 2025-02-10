using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Master;

namespace RDCELERP.BAL.Interface
{
    public interface IPriceMasterMappingManager
    {

        /// <summary>
        /// Method to manage (Add/Edit) Price Master Name
        /// </summary>
        /// <param name="PriceMasterMappingVM">PriceMasterMappingVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManagePriceMasterMapping(PriceMasterMappingViewModel PriceMasterMappingVM, int userId);


        /// <summary>
        /// Method to get the Price Master Name by id 
        /// </summary>
        /// <param name="id">PriceMasterId</param>
        /// <returns>PriceMasterNameViewModel</returns>
        public PriceMasterMappingViewModel GetPriceMasterMappingById(int id);

        /// <summary>
        /// Method to delete Price Master Name by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletPriceMasterMappingById(int id);

        /// <summary>
        /// Method to get the All Price Master Name
        /// </summary>     
        /// <returns>PriceMasterNameViewModel</returns>
        public IList<PriceMasterMappingViewModel> GetAllPriceMasterName();
    }
}
