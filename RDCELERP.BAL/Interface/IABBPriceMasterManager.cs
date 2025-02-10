using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.ABBPriceMaster;

namespace RDCELERP.BAL.Interface
{
    public interface IABBPriceMasterManager
    {
        /// <summary>
        /// Method to manage(Add/Edit) ABBPriceMaster 
        /// </summary>
        /// <param name = "ABBPlanMasterVM" > ABBPriceMasterVM </ param >
        /// < param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageABBPriceMaster(ABBPriceMasterViewModel ABBPriceMasterVM, int userId);

        /// <summary>
        /// Method to get the ABBPlanMaster by id 
        /// </summary>
        /// <param name="id">ABBPlanMasterId</param>
        /// <returns>ABBPlanMasterViewModel</returns>
        public ABBPriceMasterViewModel GetABBPriceMasterById(int id);

        /// <summary>
        /// Method to delete ABBPlanMaster by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletABBPriceMasterById(int id);

        /// <summary>
        /// Method to get the All ABBPlanMaster
        /// </summary>     
        /// <returns>ABBPlanMasterViewModel</returns>
        public IList<ABBPriceMasterViewModel> GetAllABBPriceMaster();
        public ABBPriceMasterViewModel ManageABBPriceMasterBulk(ABBPriceMasterViewModel ABBPriceMasterVM, int userId);
    }
}
