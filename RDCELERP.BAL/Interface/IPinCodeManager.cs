using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.PinCode;

namespace RDCELERP.BAL.Interface
{
    public interface IPinCodeManager
    {
        /// <summary>
        /// Method to manage (Add/Edit) Pincode 
        /// </summary>
        /// <param name="PinCodeVM">PinCodeVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        int ManagePinCode(PinCodeViewModel PinCodeVM, int userId);

        /// <summary>
        /// Method to get the Pincode by id 
        /// </summary>
        /// <param name="id">PincodeId</param>
        /// <returns>PinCodeViewModel</returns>
        PinCodeViewModel GetPinCodeById(int id);

        /// <summary>
        /// Method to delete Pincode by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        bool DeletPinCodeById(int id);

        /// <summary>
        /// Method to get the All Pincode
        /// </summary>     
        /// <returns>List  PinCodeViewModel</returns>

        IList<PinCodeViewModel> GetAllPinCode();

        public ExecutionResult GetPinCode();

        /// <summary>
        /// Method to get the Brand by id 
        /// </summary>
        /// <param name="id">BrandId</param>
        /// <returns>BrandViewModel</returns>
        public ExecutionResult PinCodeById(int id);

        public ResponseResult PinCodesByCities(CitiesID id);
        public IList<PinCodeViewModel> GetPinCodeBYCityId(int? Id);
        public PinCodeViewModel ManagePinCodeBulk(PinCodeViewModel PinCodeVM, int userId);
        public ResponseResult PinCodesByServicePartner(int cityId, int servicepartnerId);
    }
}
