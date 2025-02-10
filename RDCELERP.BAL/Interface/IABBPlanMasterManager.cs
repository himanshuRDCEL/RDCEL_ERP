using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.ABBPlanMaster;
using RDCELERP.Model.ABBPriceMaster;

namespace RDCELERP.BAL.Interface
{
    public interface IABBPlanMasterManager
    {
        /// <summary>
        /// Method to manage (Add/Edit) ABBPlanMaster 
        /// </summary>
        /// <param name="ABBPlanMasterVM">ABBPlanMasterVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageABBPlanMaster(ABBPlanMasterViewModel ABBPlanMasterVM, List<PlanDetails> planDetails, List<PlanDetails> remainingplans, int userId);

        /// <summary>
        /// Method to get the ABBPlanMaster by id 
        /// </summary>
        /// <param name="id">ABBPlanMasterId</param>
        /// <returns>ABBPlanMasterViewModel</returns>
        public ABBPlanMasterViewModel GetABBPlanMasterById(int id);

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
        public IList<ABBPlanMasterViewModel> GetAllABBPlanMaster();

        /// <summary>
        /// AbbPlanPriceDetails
        /// </summary>
        /// <param name="productCatId"></param>
        /// <param name="producttypeId"></param>
        /// <param name="productValue"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public Abbplandetail GetabbPlanPrice(int productCatId, int producttypeId, string productValue, string username);
        public ABBPlanMasterViewModel ManageABBPlanMasterBulk(ABBPlanMasterViewModel ABBPlanMasterVM, int userId);



    }
}
