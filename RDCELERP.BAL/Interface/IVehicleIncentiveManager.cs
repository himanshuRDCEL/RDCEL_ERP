using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.VehicleIncentive;

namespace RDCELERP.BAL.Interface
{
    public interface IVehicleIncentiveManager
    {
        /// <summary>
        /// Method to manage (Add/Edit) VehicleIncentive 
        /// </summary>
        /// <param name="VehicleIncentiveVM">VehicleIncentiveVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageVehicleIncentive(VehicleIncentiveViewModel VehicleIncentiveVM, int userId);



        /// <summary>
        /// Method to get the VehicleIncentive by id 
        /// </summary>
        /// <param name="id">VehicleIncentiveId</param>
        /// <returns>VehicleIncentiveViewModel</returns>
        public VehicleIncentiveViewModel GetVehicleIncentiveById(int id);

        /// <summary>
        /// Method to delete VehicleIncentive by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletVehicleIncentiveById(int id);

        /// <summary>
        /// Method to get the All Vehicle Incentive
        /// </summary>     
        /// <returns>VehiclIncentiveViewModel</returns>
        public IList<VehicleIncentiveViewModel> GetAllVehiclIncentive();

        public VehicleIncentiveViewModel ManageVehicleIncentiveBulk(VehicleIncentiveViewModel VehicleIncentiveVM, int userId);
    }
}
