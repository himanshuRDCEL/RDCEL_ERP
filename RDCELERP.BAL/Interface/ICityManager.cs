using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.City;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.State;

namespace RDCELERP.BAL.Interface
{
    public interface ICityManager
    {
        /// <summary>
        /// Method to manage (Add/Edit) City
        /// </summary>
        /// <param name="CityVM">CityVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        int ManageCity(CityViewModel CityVM, int Id);

        /// <summary>
        /// Method to get the City by id 
        /// </summary>
        /// <param name="id">CityId</param>
        /// <returns>CityViewModel</returns>
        CityViewModel GetCityById(int id);

        /// <summary>
        /// Method to delete City by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        bool DeleteCityById(int id);

        /// <summary>
        /// Method to get the All City
        /// </summary>     
        /// <returns>List  CityViewModel</returns>

        IList<CityViewModel> GetAllCity();
        IList<CityViewModel> GetCityBYStateID(int StateId);

        public ResponseResult GetCity();

        /// <summary>
        /// Method to get the Brand by id 
        /// </summary>
        /// <param name="id">BrandId</param>
        /// <returns>BrandViewModel</returns>
        public ExecutionResult CityById(int id);
        /// <summary>
        /// Get Cities Details according to state state id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseResult CityByStateId(int id);
        public IList<CityViewModel> GetCityBYState(string State);

        public ResponseResult CityByStateLists(StateList id);

        public ResponseResult GetCitiesbyLgcId(int ServicePartnerid);
        public CityViewModel ManageCityBulk(CityViewModel CityVM, int userId);
        public IList<CityViewModel> GetCityByStatesBulk(string states);

    }
}
