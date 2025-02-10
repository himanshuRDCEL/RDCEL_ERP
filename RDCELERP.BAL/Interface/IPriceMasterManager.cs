using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Master;
using RDCELERP.Model.PriceMaster;

namespace RDCELERP.BAL.Interface
{
    public interface IPriceMasterManager
    {
        /// <summary>
        /// Method to manage (Add/Edit) Price Master
        /// </summary>
        /// <param name="PriceMasterVM">PriceMasterVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManagePriceMaster(PriceMasterViewModel PriceMasterVM, int userId);

        /// <summary>
        /// Method to get the Price Master by id 
        /// </summary>
        /// <param name="id">PriceMasterId</param>
        /// <returns>PriceMasterViewModel</returns>
        public PriceMasterViewModel GetPriceMasterById(int id);

        /// <summary>
        /// Method to delete PriceMaster by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletPriceMasterById(int id);

        /// <summary>
        /// Method to get the All Price Master
        /// </summary>     
        /// <returns>PriceMasterViewModel</returns>
        public IList<PriceMasterViewModel> GetAllPriceMaster();
        public PriceMasterViewModel ManagePriceMasterBulk(PriceMasterViewModel PriceMasterVM, int userId);

        public int AddPriceMasterMapping(PriceMasterMappingViewModel priceMasterMappingViewModel, int userId);

        //public int AddPriceMasterName(PriceMasterNameViewModel priceMasterNameViewModel, int userId);

    }
}
