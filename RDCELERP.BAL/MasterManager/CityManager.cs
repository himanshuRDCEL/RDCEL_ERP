using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.City;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.State;

namespace RDCELERP.BAL.MasterManager
{
    public class CityManager : ICityManager
    {

        #region  Variable Declaration
        ICityRepository _CityRepository;
        IStateRepository _stateRepository;
        IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;
        private Digi2l_DevContext _context;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        ImapServicePartnerCityStateRepository _imapServicePartnerCityStateRepository;
        #endregion
        public CityManager(ICityRepository CityRepository, IStateRepository stateRepository, Digi2l_DevContext context, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging, ImapServicePartnerCityStateRepository imapServicePartnerCityStateRepository)
        {
            _CityRepository = CityRepository;
            _userRoleRepository = userRoleRepository;
            _stateRepository = stateRepository;
            _mapper = mapper;
            _context = context;
            _logging = logging;
            _imapServicePartnerCityStateRepository = imapServicePartnerCityStateRepository;
        }
        /// <summary>
        /// Method to manage (Add/Edit) City
        /// </summary>
        /// <param name="CityVM">CityVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageCity(CityViewModel CityVM, int Id)
        {
            TblCity TblCity = new TblCity();       
            try
            {
                if (CityVM != null)
                {
                    TblCity = _mapper.Map<CityViewModel, TblCity>(CityVM);
                    TblCity.Name = CityVM.Name!.Trim();
                    if (TblCity.CityId > 0)
                    {
                        //Code to update the object                       
                        TblCity.ModifiedBy = Id;
                        TblCity.ModifiedDate = _currentDatetime;
                        _CityRepository.Update(TblCity);
                    }
                    else
                    {
                        var Check = _CityRepository.GetSingle(x => x.Name == CityVM.Name);
                        if (Check == null)
                        {
                            TblCity.IsActive = true;
                            TblCity.CreatedDate = _currentDatetime;
                            TblCity.CreatedBy = Id;
                            _CityRepository.Create(TblCity);
                        }
                    }
                    _CityRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CityManager", "ManageCity", ex);
            }
            return TblCity.CityId;
        }

        public CityViewModel ManageCityBulk(CityViewModel CityVM, int userId)
        {
            List<TblCity> tblCity = new List<TblCity>();

            if (CityVM != null && CityVM.CityVMList != null && CityVM.CityVMList.Count > 0)
            {
                var ValidatedCityList = CityVM.CityVMList.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
                CityVM.CityVMErrorList = CityVM.CityVMList.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();

                //tblCity = _mapper.Map<List<CityVMExcel>, List<TblCity>>(ValidatedCityList);
                if (ValidatedCityList != null && ValidatedCityList.Count > 0)
                {
                    foreach (var item in ValidatedCityList)
                    {
                        try
                        {
                            if (item.CityId > 0)
                            {
                                var state = _stateRepository.GetSingle(x => x.Name == item.Name);
                                TblCity TblCities = new TblCity();
                                //Code to update the object 
                                TblCities.Name = item.Name;
                                TblCities.IsMetro = item.IsMetro;
                                TblCities.IsActive = item.IsActive;
                                TblCities.CityId = item.CityId;
                                TblCities.CityCode = item.CityCode;
                                TblCities.StateId = state.StateId;
                                TblCities.ModifiedDate = _currentDatetime;
                                TblCities.ModifiedBy = userId;
                                _CityRepository.Update(TblCities);
                                _CityRepository.SaveChanges();

                            }
                            else
                            {

                                var state = _stateRepository.GetSingle(x => x.Name == item.StateName);
                                TblCity TblCities = new TblCity();
                                //Code to update the object 
                                TblCities.Name = item.Name;
                                TblCities.CityCode = item.CityCode;
                                TblCities.StateId = state.StateId;
                                TblCities.IsMetro = item.IsMetro;
                                TblCities.IsActive = item.IsActive;
                                TblCities.CreatedDate = _currentDatetime;
                                TblCities.CreatedBy = userId;
                                _CityRepository.Create(TblCities);
                                _CityRepository.SaveChanges();

                            }
                        }
                        catch (Exception ex)
                        {
                            item.Remarks += ex.Message + ", ";
                            CityVM.CityVMErrorList.Add(item);
                        }
                    }
                }
            }
            return CityVM;
        }

        /// <summary>
        /// Method to get the City by id 
        /// </summary>
        /// <param name="id">CityId</param>
        /// <returns>CityViewModel</returns>
        public CityViewModel GetCityById(int id)
        {
            CityViewModel CityVM = null;
            TblCity TblCity = null;

            try
            {
                TblCity = _CityRepository.GetSingle(where: x => x.CityId == id);
                if (TblCity != null)
                {
                    CityVM = _mapper.Map<TblCity, CityViewModel>(TblCity);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CityManager", "GetCityById", ex);
            }
            return CityVM;
        }

        /// <summary>
        /// Method to delete City by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeleteCityById(int id)
        {
            bool flag = false;
            try
            {
                TblCity TblCity = _CityRepository.GetSingle(x => x.IsActive == true && x.CityId == id);
                if (TblCity != null)
                {
                    TblCity.IsActive = false;
                    _CityRepository.Update(TblCity);
                    _CityRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CityManager", "DeleteCityById", ex);
            }
            return flag;
        }

        /// <summary>
        /// Method to get the All City
        /// </summary>     
        /// <returns>List  CityViewModel</returns>

        public IList<CityViewModel> GetAllCity()
        {
            IList<CityViewModel> CityVMList = null;
            List<TblCity> TblCitylist = new List<TblCity>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblCitylist = _CityRepository.GetList(x => x.IsActive == true).ToList();

                if (TblCitylist != null && TblCitylist.Count > 0)
                {
                    CityVMList = _mapper.Map<IList<TblCity>, IList<CityViewModel>>(TblCitylist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PincodeManager", "GetAllPinCode", ex);
            }
            return CityVMList;
        }

        public IList<CityViewModel> GetCityBYStateID(int StateId)
        {
            IList<CityViewModel> CityVMList = null;
            List<TblCity> TblCitylist = new List<TblCity>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblCitylist = _CityRepository.GetList(x => x.IsActive == true && x.StateId == StateId).ToList();

                if (TblCitylist != null && TblCitylist.Count > 0)
                {
                    CityVMList = _mapper.Map<IList<TblCity>, IList<CityViewModel>>(TblCitylist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CityManager", "GetCityBYStateID", ex);
            }
            return CityVMList;
        }
        public IList<CityViewModel> GetCityByStatesBulk(string states)
        {
            IList<CityViewModel> cityVMList = new List<CityViewModel>();

            try
            {
                string[] stateList = states.Split(",");

                foreach (string state in stateList)
                {
                    if (!string.IsNullOrEmpty(state))
                    {
                        TblState tblState = _stateRepository.GetSingle(x => x.IsActive == true && x.Name == state);

                        if (tblState != null)
                        {
                            List<TblCity> tblCityList = _CityRepository.GetList(x => x.IsActive == true && x.StateId == tblState.StateId).ToList();

                            if (tblCityList.Count > 0)
                            {
                                IList<CityViewModel> stateCityVMList = _mapper.Map<IList<TblCity>, IList<CityViewModel>>(tblCityList);
                                cityVMList = cityVMList.Concat(stateCityVMList).ToList();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CityManager", "GetCityByStates", ex);
            }

            return cityVMList;
        }
        public IList<CityViewModel> GetCityBYState(string State)
        {
            IList<CityViewModel> CityVMList = null;
            List<TblCity> TblCitylist = new List<TblCity>();
            TblState TblState = null;
            // TblUseRole TblUseRole = null;
            try
            {
                //TblState = _context.TblStates.Where(x => x.IsActive == true && x.Name == State);
                TblState = _stateRepository.GetSingle(x => x.IsActive == true && x.Name == State);
                if (TblState != null &&  TblState.StateId != null)
                {
                    TblCitylist = _CityRepository.GetList(x => x.IsActive == true && x.StateId == TblState.StateId).ToList();
                }


                if (TblCitylist != null && TblCitylist.Count > 0)
                {
                    CityVMList = _mapper.Map<IList<TblCity>, IList<CityViewModel>>(TblCitylist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CityManager", "GetCityBYStateID", ex);
            }
            return CityVMList;
        }

        /// <summary>
        /// Get City - list of cities
        /// </summary>
        /// <returns>responseResult</returns>
        public ResponseResult GetCity()
        {
            ResponseResult responseResult = null;
            List<TblCity> TblCitylist = new List<TblCity>();
            IList<CityList> cityLists = null;

            // TblUseRole TblUseRole = null;
            try
            {

                TblCitylist = _CityRepository.GetList(x => x.IsActive == true).ToList();

                if (TblCitylist != null && TblCitylist.Count > 0)
                {
                    cityLists = _mapper.Map<IList<TblCity>, IList<CityList>>(TblCitylist);
                    if (cityLists != null && cityLists.Count > 0)
                    {
                        responseResult = new ResponseResult();
                        responseResult.Status = true;
                        responseResult.message = "Success";
                        responseResult.Status_Code = HttpStatusCode.OK;
                        responseResult.Data = cityLists;
                    }
                    else
                    {
                        responseResult = new ResponseResult();
                        responseResult.Status = false;
                        responseResult.message = "Not Success,mapping data failed";
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                    return responseResult;
                }
                else
                {
                    responseResult = new ResponseResult();
                    responseResult.Status = false;
                    responseResult.message = "Not Success,data not found";
                    responseResult.Status_Code = HttpStatusCode.NotFound;
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult = new ResponseResult();
                responseResult.Status = false;
                responseResult.message = ex.ToString();
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("BrandManager", "GetBrand", ex);
            }
            return responseResult;
        }

        /// <summary>
        /// Method to get the Brand by id 
        /// </summary>
        /// <param name="id">BrandId</param>
        /// <returns>BrandViewModel</returns>
        public ExecutionResult CityById(int id)
        {
            CityViewModel CityVM = null;
            TblCity TblCity = null;

            try
            {
                TblCity = _CityRepository.GetSingle(where: x => x.IsActive == true && x.CityId == id);
                if (TblCity != null)
                {
                    CityVM = _mapper.Map<TblCity, CityViewModel>(TblCity);
                    return new ExecutionResult(new InfoMessage(true, "Success", CityVM));

                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CityManager", "CityById", ex);
            }
            return new ExecutionResult(new InfoMessage(true, "No data found"));
        }

        /// <summary> 
        /// Method to city by stateID used for api
        /// added by ashwin 
        /// </summary>
        /// <param name="id">stateId</param>
        /// <returns>responseResult</returns>
        public ResponseResult CityByStateId(int id)
        {
            ResponseResult responseResult = new ResponseResult();
           
            List<TblCity> TblCity = null;
            IList<CityList> cityLists = null;

            try
            {
                TblCity = _CityRepository.GetList(where: x => x.IsActive == true && x.StateId == id).ToList();
                if (TblCity != null)
                {
                    cityLists = _mapper.Map<IList<TblCity>, IList<CityList>>(TblCity);
                    if (cityLists != null && cityLists.Count > 0)
                    {
                        responseResult.message = "Success";
                        responseResult.Status = true;
                        responseResult.Status_Code = HttpStatusCode.OK;
                        responseResult.Data = cityLists;
                    }
                    else
                    {
                        responseResult.message = "Not Success,error occurs while mapping dataa";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    responseResult.message = "Not Success,data not found";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.NotFound;
                }
                return responseResult;

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CityManager", "CityByStateId", ex);
                responseResult.message = ex.ToString();
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }

        /// <summary> 
        /// Method to cities by stateList used for api
        /// added by ashwin 
        /// </summary>
        /// <param name="id">statelist</param>
        /// <returns>responseResult</returns>
        public ResponseResult CityByStateLists(StateList id)
        {
            ResponseResult responseResult = new ResponseResult();
            List<TblCity> TblCity = null;
            List<CityList> cityLists = new List<CityList>();
            List<CityList> finalcityLists = null;
            try
            {
                if(id !=null && id.stateIdLists.Count>0)
                {
                    finalcityLists =  new List<CityList>();
                    foreach (var stateid in id.stateIdLists)
                    {
                     
                        if (stateid.id > 0 )
                        {
                            TblCity = _CityRepository.GetList(where: x => x.IsActive == true && x.StateId == stateid.id).ToList();
                            if (TblCity != null && TblCity.Count > 0)
                            {
                                cityLists = _mapper.Map<List<TblCity>, List<CityList>>(TblCity);
                                if (cityLists.Count > 0)
                                {
                                    finalcityLists.AddRange(cityLists);
                                }
                                else
                                {
                                    responseResult.message = "failed,an error occurs while mapping the data";
                                    responseResult.Status = false;
                                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                                    return responseResult;
                                }
                            }
                            else
                            {
                                if ( TblCity.Count == 0 && TblCity ==null)
                                {
                                    responseResult.message = "failed,Parameters must be greater than zero";
                                    responseResult.Status = false;
                                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                                    return responseResult;
                                }
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        
                    }
                    if (finalcityLists.Count > 0)
                    {
                        responseResult.Status = true;
                        responseResult.Status_Code = HttpStatusCode.OK;
                        responseResult.message = "Success";
                        responseResult.Data = finalcityLists;
                    }
                    else
                    {
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        responseResult.message = "Not Success";
                        //responseResult.Data = finalcityLists;
                    }
                    
                }
                else
                {
                    responseResult.message = "failed,Request parameter should not be null";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    return responseResult;
                }
                
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CityManager", "CityByStateLists", ex);
                responseResult.message = ex.ToString();
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }
        /// <summary>
        /// get list of cities by service partner id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseResult GetCitiesbyLgcId(int ServicePartnerid)
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.message = string.Empty;
            //List<TblCity> TblCity = null;
            List<CityList> cityLists = new List<CityList>();
            List<CityList> finalcityLists = new List<CityList>();
            List<MapServicePartnerCityState> mapList = new List<MapServicePartnerCityState>();
             try
            {
                if (ServicePartnerid > 0)
                {

                    mapList = _imapServicePartnerCityStateRepository.GetList(x => x.IsActive == true && x.ServicePartnerId == ServicePartnerid).ToList();
                    if(mapList!=null && mapList.Count > 0)
                    {
                        foreach(var item in mapList)
                        {
                            CityList cityList = new CityList();

                            TblCity tblCiti = _CityRepository.GetSingle(x => x.IsActive == true && x.CityId == item.CityId);
                            
                            if(tblCiti!=null && tblCiti.CityId > 0 && !finalcityLists.Exists(x=>x.CityId== tblCiti.CityId))
                            {
                                cityList.CityId = tblCiti.CityId;
                                cityList.Name = tblCiti.Name.ToString();
                                cityList.StateId = Convert.ToInt32(tblCiti.StateId);
                                finalcityLists.Add(cityList);
                            }
                                                           
                        }
                        
                        if (finalcityLists.Count > 0)
                        {
                            responseResult.message = "Success";
                            responseResult.Data = finalcityLists;
                            responseResult.Status = true;
                            responseResult.Status_Code = HttpStatusCode.OK;
                            return responseResult;
                        }
                        else
                        {
                            responseResult.message = "no city found for service partner";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            return responseResult;
                        }
                    }
                    else
                    {
                        responseResult.message = "no active city found for service partner";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        return responseResult;
                    }
                }
                else
                {
                    responseResult.message = "failed,Request parameter should not be null";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    return responseResult;
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CityManager", "GetCitiesbyLgcId", ex);
                responseResult.message = ex.ToString();
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }

    }
}
