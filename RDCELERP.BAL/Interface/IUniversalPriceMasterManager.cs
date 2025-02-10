
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Master;
using RDCELERP.Model.UniversalPriceMaster;

namespace RDCELERP.BAL.Interface
{
    public interface IUniversalPriceMasterManager
    {
        /// <summary>
        /// Method to manage (Add/Edit) Price Master
        /// </summary>
        /// <param name="UniversalPriceMasterVM">UniversalPriceMasterVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageUniversalPriceMaster(UniversalPriceMasterViewModel UniversalPriceMasterVM, int userId);

        /// <summary>
        /// Method to get the Price Master by id 
        /// </summary>
        /// <param name="id">UniversalPriceMasterId</param>
        /// <returns>UniversalPriceMasterViewModel</returns>
        public UniversalPriceMasterViewModel GetUniversalPriceMasterById(int id);

        /// <summary>
        /// Method to delete UniversalPriceMaster by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeleteUniversalPriceMasterById(int id);

        /// <summary>
        /// Method to get the All Price Master
        /// </summary>     
        /// <returns>UniversalPriceMasterViewModel</returns>
        public IList<UniversalPriceMasterViewModel> GetAllUniversalPriceMaster();
        public UniversalPriceMasterViewModel ManageUniversalPriceMasterBulk(UniversalPriceMasterViewModel UniversalPriceMasterVM, int userId);

        public int AddPriceMasterMapping(PriceMasterMappingViewModel PriceMasterMappingViewModel, int userId);

       public int AddPriceMasterName(PriceMasterNameViewModel PriceMasterNameViewModel, int userId);
       public List<PriceMasterNameViewModel> GetListofpriceMasterByBUId(int BUId);

    }
}
